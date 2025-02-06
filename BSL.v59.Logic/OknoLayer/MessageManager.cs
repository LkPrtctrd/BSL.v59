using BSL.v59.Logic.Protocol;
using BSL.v59.Logic.Protocol.Laser.C;
using BSL.v59.Logic.Protocol.Laser.S;

namespace BSL.v59.Logic.OknoLayer;

public class MessageManager(Messaging messaging)
{
    public int ReceiveMessage(PiranhaMessage piranhaMessage)
    {
        return piranhaMessage.GetMessageType() switch
        {
            10101 => LogicMessageReceived((LoginMessage)piranhaMessage),
            _ => 1
        };
    }

    private int LogicMessageReceived(LoginMessage loginMessage)
    {
        if (loginMessage.ClientMajorVersion is not 59) return -1488; 
        messaging.Send(new LoginOkMessage());
        messaging.Send(new OwnHomeDataMessage());
        
        return 1;
    }
}