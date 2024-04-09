public interface IUserHandler
{

void ProcessClientAsync(TcpClient client, NetworkStream stream, User currentUser);

}