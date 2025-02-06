using BSL.v59.Core;
namespace BSL.v59;

public static class Program
{
    public static void Main(string[] args)
    {
        new LaserTcpCentralGateway(9339).Start();

        for (;;) ;
    }
}