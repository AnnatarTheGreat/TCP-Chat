public interface IUserReceiver
{
    User ReceiveCurrentUser(Socket client, NetworkStream stream);
}