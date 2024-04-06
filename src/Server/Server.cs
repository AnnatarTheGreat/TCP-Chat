using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public class Server : AbstractServer, IServer
{
    private TcpListener _server;

    private UsersList _users;
    
    public Server()
    {
        _server = new TcpListener(IPAddress.Any, 8888);

        _users = new UsersList();
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

                User? user = ReceiveCurrentUser(client, stream);
                if (user == null)
                {
                    client.Close();
                }

                Task.Run(async () => ProcessClientAsync(client, stream, user));
            }
        }
        finally
        {
            _server.Stop();
        }
    }


    protected override User ReceiveCurrentUser(TcpClient client, NetworkStream stream)
    {
        try
        {
            var dataBuffer = new byte[1024];
            int receivedData = stream.Read(dataBuffer, 0, dataBuffer.Length);
            string jsonString = Encoding.UTF8.GetString(dataBuffer, 0, receivedData);
            return JsonSerializer.Deserialize<User>(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.Close();
            return null;
        }

    }

    protected override async void ProcessClientAsync(TcpClient client, NetworkStream stream, User currentUser)
    {
        try
        {
            string greetings = $"{currentUser.Name} has joined the chat";
            Console.WriteLine(greetings);
            byte[] greetingsBuffer = Encoding.UTF8.GetBytes(greetings);
            foreach (var user in _users.GetAllUsersStreams())
            {
                user.Write(greetingsBuffer, 0, greetingsBuffer.Length);
            }
            _users.Add(client, stream);
            ListenForMessages(client, stream, currentUser);
            SendUserLeftNotification(client, currentUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.Close();
        }
    }

    protected override void ListenForMessages(TcpClient client, NetworkStream stream, User currentUser)
    {
        while (client.Connected)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = 0;
            try
            {
                bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
            }
            catch
            {
                break;
            }
            if (bytesRead > 0)
            {
                string message = $"{currentUser.Name}: " + Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                foreach (var user in _users.GetAllUsersStreams())
                {
                    user.Write(messageBuffer, 0, messageBuffer.Length);
                }
            }
            else
            {
                break;
            }
        }
    }

    protected override void SendUserLeftNotification(TcpClient client, User currentUser)
    {
        try
        {
            string quitMessage = $"{currentUser.Name} has left the chat";
            Console.WriteLine(quitMessage);
            byte[] farewellBuffer = Encoding.UTF8.GetBytes(quitMessage);
            _users.Remove(client);
            foreach (var user in _users.GetAllUsersStreams())
            {
                user.Write(farewellBuffer, 0, farewellBuffer.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.Close();
        }
    }

}