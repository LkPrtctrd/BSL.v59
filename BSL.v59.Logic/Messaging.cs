using BSL.v59.Logic.OknoLayer;
using BSL.v59.Logic.Protocol;
using BSL.v59.Logic.Protocol.Laser.S;
using BSL.v59.TitanEngine.Blake;
using BSL.v59.TitanEngine.DataS;
using BSL.v59.TitanEngine.NaCl;
using BSL.v59.TitanEngine.Pepp;
using BSL.v59.TitanEngine.Pepp.Crypto;
using NetCoreServer;
using Buffer = System.Buffer;

namespace BSL.v59.Logic;

public class Messaging
{
    private readonly byte[] _secretKey;
    private readonly byte[] _sessionToken;
    
    private byte[] _clientPk = null!;

    private readonly byte[] _sNonce;

    private PepperEncrypter _decryptStream = null!;
    private PepperEncrypter _encryptStream = null!;

    private int _pepperState = 1;

    private byte[] _rNonce = null!;
    
    private MessageManager? _messageManager;

    public Messaging(TcpSession session) 
    {
        TcpSessionInst = session;

        _sessionToken = new byte[24];
        _secretKey = new byte[32];
        _sNonce = new byte[24];

        TweetNaCl.RandomBytes(_sessionToken);
        TweetNaCl.RandomBytes(_secretKey);
        TweetNaCl.RandomBytes(_sNonce);

        SetState(2);
    }

    public TcpSession TcpSessionInst { get; set; }

    public int NextMessage(byte[] buffer, long offset, long size, out (int, int, int) header)
    {
        if (offset + size > 2048) return (header = (-600, 0, 0)).Item1;
        if ((header = ReadHeader(buffer)).Item2 > buffer.Length || header.Item2 > size)
            return (header = (-601, 0, 0)).Item1;

        return ReadNewMessage(header.Item1, header.Item2, header.Item3, buffer.Skip(7).Take(header.Item2).ToArray());
    }

    public int ReadNewMessage(int type, int length, int version, byte[] payload)
    {
        switch (_pepperState)
        {
            case 2:
                if (type == 10100) SetState(3);
                else return -602;
                break;
            case 3:
                if (type != 10101) return -603;
                if ((payload = HandlePepperLogin(payload)) == null!) return -604;
                break;
            case 5:
                if (_decryptStream.Decrypt(payload, payload = new byte[length - _decryptStream.GetEncryptionOverhead()],
                        length) != 0)
                {
                    Console.WriteLine($"New crime message received: {type}.");
                    return -605;
                }

                break;
        }

        try
        {
            var piranhaMessage = LogicLaserMessageFactory.CreateMessageByType(type); 
            {
                if (piranhaMessage == null)
                {
                    Console.WriteLine($"New unknown message received: {type}.");
                    return 0;
                }
            }

            if (length > 0)
            { 
                piranhaMessage.ByteStream.SetByteArray(payload, payload.Length);

                piranhaMessage.Decode();
            }

            Console.WriteLine($"New message received: {type}.");

            if (type is 10100)
            {
                Send(new ServerHelloMessage());
                return 0;
            }

            if (_messageManager is null) return -666;
            return _messageManager.ReceiveMessage(piranhaMessage);
        }
        catch
        {
            return -606;
        }
    }

    public byte[] HandlePepperLogin(byte[] payload)
    {
        try
        {
            _clientPk = payload.Take(32).ToArray();
            
            var hasher = new Blake2BHasher();
            {
                hasher.Update(_clientPk);
                hasher.Update(PepperKey.ServerPublicKey);
            }

            var decrypted = TweetNaCl.CryptoBoxOpen(payload.Skip(32).ToArray(),
                hasher.Finish(), _clientPk, PepperKey.ServerSecretKey);

            if (!decrypted.Take(24).ToArray().SequenceEqual(_sessionToken)) return null!;

            _rNonce = decrypted.Skip(24).Take(24).ToArray();
            SetState(4);

            _messageManager = new MessageManager(this);
            return decrypted.Skip(48).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }

    public byte[] SendPepperLoginResponse(byte[] payload)
    {
        var packet = new byte[payload.Length + 32 + 24];

        Buffer.BlockCopy(_sNonce, 0, packet, 0, 24);
        Buffer.BlockCopy(_secretKey, 0, packet, 24, 32);
        Buffer.BlockCopy(payload, 0, packet, 24 + 32, payload.Length);

        var hasher = new Blake2BHasher();
        {
            hasher.Update(_rNonce);
            
            hasher.Update(_clientPk);
            hasher.Update(PepperKey.ServerPublicKey);
        }

        _decryptStream = new PepperEncrypter(_secretKey, _rNonce);
        _encryptStream = new PepperEncrypter(_secretKey, _sNonce);

        SetState(5);

        return TweetNaCl.CryptoBox(packet, hasher.Finish(),
            _clientPk, PepperKey.ServerSecretKey);
    }

    public void Send(PiranhaMessage piranhaMessage)
    {
        if (_pepperState == 3)
            if (piranhaMessage.GetMessageType() == new ServerHelloMessage().GetMessageType())
                ((ServerHelloMessage)piranhaMessage).SetServerHelloToken(_sessionToken);

        EncryptAndWrite(piranhaMessage);
    }

    public int EncryptAndWrite(PiranhaMessage piranhaMessage)
    {
        if (!piranhaMessage.IsServerToClientMessage()) return -1;
        if (piranhaMessage.GetEncodingLength() == 0) piranhaMessage.Encode();

        var payload = new byte[piranhaMessage.GetEncodingLength()];
        Buffer.BlockCopy(piranhaMessage.GetMessageBytes(), 0, payload, 0, payload.Length);
        
        switch (_pepperState)
        {
            case 4:
                payload = SendPepperLoginResponse(payload);
                break;
            case 5: 
                if (_encryptStream.Encrypt(payload,
                            payload = new byte[payload.Length + _encryptStream.GetEncryptionOverhead()], payload.Length) >=
                        0) break;
                return 0;
        }

        var message = WriteHeader(payload, piranhaMessage.GetMessageType(), piranhaMessage.GetMessageVersion());
        Buffer.BlockCopy(payload, 0, message, 7, payload.Length);

        TcpSessionInst.SendAsync(message);
        Console.WriteLine($"New message has been sent: {piranhaMessage.GetMessageType()}.");

        return 0;
    }
    
    public static (int, int, int) ReadHeader(byte[] headerBuffer)
    {
        var v1 = (headerBuffer[0] << 8) | headerBuffer[1]; // messageType (int16)
        var v2 = (headerBuffer[2] << 16) | (headerBuffer[3] << 8) | headerBuffer[4]; // messageLength (int24)
        var v3 = (headerBuffer[5] << 8) | headerBuffer[6]; // messageVersion (int16)

        return (v1, v2, v3);
    }
    
    public static byte[] WriteHeader(byte[] payload, int t, int v)
    {
        var final = new byte[payload.Length + 7];
        {
            // int16
            final[0] = (byte)(t >> 8);
            final[1] = (byte)t;
            // messageType

            // int24
            final[2] = (byte)(payload.Length >> 16);
            final[3] = (byte)(payload.Length >> 8);
            final[4] = (byte)payload.Length;
            // messageLength

            // int16
            final[5] = (byte)(v >> 8);
            final[6] = (byte)v;
            // messageVersion
        }

        return final;
    }

    public void SetState(int state)
    {
        _pepperState = state;
    }
}