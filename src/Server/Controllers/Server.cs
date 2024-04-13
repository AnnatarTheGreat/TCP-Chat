using System.Net;

public class Server
{
    private Socket _server;
    private IUserHandler _userHandler;
    private IUserReceiver _userReceiver;
    public Server(IUserReceiver userReceiver, IUserHandler userHandler)
    {
        _server = new Socket(SocketType.Stream, ProtocolType.Tcp);
        
        _userReceiver = userReceiver;

        _userHandler = userHandler;
    }
    public async Task RunAsync()
    {
        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            _server.Bind(iPEndPoint);
            _server.Listen();
            Console.WriteLine("Server is listening on");
            while (true)
            {
                Socket client = await _server.AcceptAsync();
                NetworkStream stream  = new NetworkStream(client, ownsSocket: true); 

                User? user = _userReceiver.ReceiveCurrentUser(client, stream);
                if (user == null)
                {
                    client.Close();
                }

                Task.Run(async () => _userHandler.ProcessClientAsync(client, stream, user));
            }
        }
        finally
        {
            _server.Dispose();
        }
    }

}