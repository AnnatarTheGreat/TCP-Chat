public class MessageListener : IMessageListener
{
    private IUsersList _users;

    public MessageListener(IUsersList users)
    {
        _users = users;
    }

    public void ListenForMessages(TcpClient client, NetworkStream stream, User currentUser)
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
}