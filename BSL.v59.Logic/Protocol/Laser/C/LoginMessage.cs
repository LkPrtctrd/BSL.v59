using BSL.v59.TitanEngine.Msg.Messages;

namespace BSL.v59.Logic.Protocol.Laser.C;


public class LoginMessage : PiranhaMessage
{
    public long AccountId { get; set; }
    public int ClientBuild { get; set; }
    public int ClientMajorVersion { get; set; }
    public int ClientMinor { get; set; }
    public string PassToken { get; set; } = string.Empty;
    public string ResourceSha { get; set; } = string.Empty;

    public override void Decode()
    {
        base.Decode();

        _ = ByteStream.ReadInt();
        AccountId = ByteStream.ReadInt();
        
        PassToken = ByteStream.ReadString(1024);

        ClientMajorVersion = ByteStream.ReadInt();
        ClientMinor = ByteStream.ReadInt();
        ClientBuild = ByteStream.ReadInt();

        ResourceSha = ByteStream.ReadString(1024);
    }

    public override int GetMessageType()
    {
        return TitanLoginMessage.GetMessageType();
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}