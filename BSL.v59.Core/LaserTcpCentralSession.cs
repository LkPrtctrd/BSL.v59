using BSL.v59.Logic;
using NetCoreServer;

namespace BSL.v59.Core;

public class LaserTcpCentralSession(TcpServer server) : TcpSession(server)
{
    private Messaging? _messaging;
    
    protected override void OnConnecting()
    {
        base.OnConnecting();
        
        Console.WriteLine($"New user connected! Id: {Id}; RAddress: {Socket.RemoteEndPoint}");

        _messaging = new Messaging(this);
    }

    protected override void OnDisconnected()
    {
        base.OnDisconnected();
        
        Console.WriteLine($"New user disconnected! Id: {Id}");
        
        _messaging = null;
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        if (_messaging is null) return;
        
        _messaging.NextMessage(buffer, offset, size, out _);
    }
}