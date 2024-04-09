public interface IMessageListener
{
    void ListenForMessages(TcpClient client, NetworkStream stream, User currentUser);

}