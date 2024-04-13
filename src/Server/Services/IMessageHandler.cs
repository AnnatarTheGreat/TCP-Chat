public interface IMessageHandler
{
    void ListenForMessages(Socket client, NetworkStream stream, User currentUser);

    void SendMessage(User currentUser, byte[] buffer, int bytesRead);
}