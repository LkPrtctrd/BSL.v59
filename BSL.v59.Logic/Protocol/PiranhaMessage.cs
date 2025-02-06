using BSL.v59.TitanEngine.DataS;
using BSL.v59.TitanEngine.DataS.ChecksumLogic;

namespace BSL.v59.Logic.Protocol;

public abstract class PiranhaMessage
{
    private int _messageVersion = -1;
    private int _proxySessionId;
    
    public ByteStream ByteStream { get; } = new(5);
    public ChecksumEncoder ChecksumEncoder { get; set; } = new();
    
    public abstract int GetMessageType();
    public abstract int GetServiceNodeType();

    public virtual string GetMessageTypeName()
    {
        return "ZoV";
    }

    public bool IsClientToServerMessage()
    {
        return GetMessageType() is >= 10000 and < 20000 or 30000;
    }

    public bool IsServerToClientMessage()
    {
        return GetMessageType() is >= 20000 and < 30000 or 40000;
    }

    public virtual int GetEncodingLength()
    {
        return ByteStream.GetLength();
    }

    public virtual byte[] GetMessageBytes()
    {
        return ByteStream.GetByteArray();
    }

    public virtual int GetMessageVersion()
    {
        return _messageVersion < 1 ? GetMessageType() == 20104 ? 1 : 0 : _messageVersion;
    }

    public virtual void Encode()
    {
    }

    public virtual void Decode()
    {
    }

    public virtual int GetProxySessionId()
    {
        return _proxySessionId;
    }

    public virtual void SetProxySessionId(int proxySessionId)
    {
        _proxySessionId = proxySessionId;
    }

    public void SetMessageVersion(int newArg)
    {
        _messageVersion = newArg;
    }

    public virtual void Clear()
    {
        ByteStream.Clear(GetEncodingLength());
    }

    public virtual void Destruct()
    {
        ByteStream.Destruct();
    }
}