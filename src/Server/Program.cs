using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ServerSpace;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        IServer server = new Server();
        await server.RunAsync();
    }
}