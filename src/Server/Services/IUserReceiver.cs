public interface IUserReceiver
{
    User ReceiveCurrentUser(TcpClient client, NetworkStream stream);
}