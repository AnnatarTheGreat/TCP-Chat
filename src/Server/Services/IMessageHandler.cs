public interface IMessageHandler
{
    void ListenForMessages(TcpClient client, NetworkStream stream, User currentUser);

    void SendMessage(User currentUser, byte[] buffer, int bytesRead);
}