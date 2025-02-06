namespace BSL.v59.Logic.Protocol.Laser.S;

public class ServerHelloMessage : PiranhaMessage
{
    private byte[] _serverHelloToken = null!;

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteBytes(_serverHelloToken, _serverHelloToken.Length);
    }

    public void SetServerHelloToken(byte[] a1)
    {
        _serverHelloToken = a1;
    }

    public byte[] RemoveServerHelloToken()
    {
        var cloned = _serverHelloToken;
        _serverHelloToken = null!;
        return cloned;
    }

    public override int GetMessageType()
    {
        return 20100;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}