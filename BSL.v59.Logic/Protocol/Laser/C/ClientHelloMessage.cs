namespace BSL.v59.Logic.Protocol.Laser.C;

public class ClientHelloMessage : PiranhaMessage
{
    public int A1 { get; private set; }
    public int A2 { get; private set; }
    public int A3 { get; private set; }
    public int A4 { get; private set; }
    public int A5 { get; private set; }
    public int A6 { get; private set; }
    public string A7 { get; private set; } = string.Empty;
    public int A8 { get; private set; }
    public int A9 { get; private set; }

    public override void Decode()
    {
        base.Decode();

        A1 = ByteStream.ReadInt();
        A2 = ByteStream.ReadInt();
        A3 = ByteStream.ReadInt();
        A4 = ByteStream.ReadInt();
        A5 = ByteStream.ReadInt();
        A6 = ByteStream.ReadInt();
        A7 = ByteStream.ReadStringReference();
        A8 = ByteStream.ReadInt();
        A9 = ByteStream.ReadInt();
    }

    public override int GetMessageType()
    {
        return 10100;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}