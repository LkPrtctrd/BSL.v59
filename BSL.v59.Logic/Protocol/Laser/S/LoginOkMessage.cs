namespace BSL.v59.Logic.Protocol.Laser.S;

public class LoginOkMessage : PiranhaMessage
{
    public override void Encode()
    {
        base.Encode();

        { // logiclong
            ByteStream.WriteInt(0);
            ByteStream.WriteInt(1);
        }

        { // logiclong
            ByteStream.WriteInt(0);
            ByteStream.WriteInt(1);
        }
        
        ByteStream.WriteString("abcdefghijklmnopqrstuvwxyz");
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteInt(59);
        ByteStream.WriteInt(197);
        ByteStream.WriteInt(1);
        ByteStream.WriteString("dev");
        ByteStream.WriteInt(0);
        ByteStream.WriteInt(0);
        ByteStream.WriteInt(0);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteInt(0);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString("RU");
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteInt(0);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteInt(2);
        ByteStream.WriteString("https://game-assets.brawlstarsgame.com");
        ByteStream.WriteString("http://a678dbc1c015a893c9fd-4e8cc3b1ad3a3c940c504815caefa967.r87.cf2.rackcdn.com");
        ByteStream.WriteInt(2);
        ByteStream.WriteString("https://event-assets.brawlstars.com");
        ByteStream.WriteString("https://24b999e6da07674e22b0-8209975788a0f2469e68e84405ae4fcf.ssl.cf2.rackcdn.com/event-assets");
        ByteStream.WriteVInt(0);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteBoolean(true);
        ByteStream.WriteBoolean(false);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteString(String.Empty);
        ByteStream.WriteBoolean(false);
        ByteStream.WriteBoolean(false);
        ByteStream.WriteBoolean(false);
        ByteStream.WriteBoolean(false);
        ByteStream.WriteBoolean(false);
    }

    public override int GetMessageType()
    {
        return 20104;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}