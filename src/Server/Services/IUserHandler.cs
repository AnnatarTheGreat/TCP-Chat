public interface IUserHandler
{
    void ProcessClientAsync(Socket client, NetworkStream stream, User currentUser);
}