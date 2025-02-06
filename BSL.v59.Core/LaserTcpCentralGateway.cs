using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace BSL.v59.Core;

public class LaserTcpCentralGateway : TcpServer
{
    private readonly ushort _port;

    public LaserTcpCentralGateway(ushort port) : base("0.0.0.0", port)
    {
        if (port < 1000) 
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be at least 1000");
        
        _port = port;
    }

    public override bool Start()
    {
        Console.WriteLine($"LaserTcpCentralGateway starting ({_port}) ..."); 
        return base.Start();
    }

    public override bool Stop()
    {
        Console.WriteLine($"LaserTcpCentralGateway stopping ({_port}) ...");
        return base.Stop();
    }

    protected override TcpSession CreateSession()
    {
        return new LaserTcpCentralSession(this);
    }
}