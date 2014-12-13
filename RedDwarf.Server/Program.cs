using System.Net;

namespace RedDwarf.Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var server = new RedDwarfServer())
            {
                server.Start(new IPEndPoint(IPAddress.Any, 0xBEEF));
            }
        }
    }
}
