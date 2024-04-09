using System.Net;

public class Server
{
    private TcpListener _server;

    private IUsersList _users;
    private IUserHandler _userHandler;
    private IUserReceiver _userReceiver;
    public Server(IUsersList users, IUserReceiver userReceiver, IUserHandler userHandler)
    {
        _server = new TcpListener(IPAddress.Any, 8888);

        _users = users;

        _userReceiver = userReceiver;

        _userHandler = userHandler;
    }
    public async Task RunAsync()
    {
        try
        {
            _server.Start();
            Console.WriteLine("Server is running!");
            while (true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                NetworkStream stream = client.GetStream();

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
            _server.Stop();
        }
    }


}