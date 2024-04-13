public interface IUsersList
{
    void Add(Socket client, NetworkStream stream);
    void Remove(Socket client);
    ICollection<NetworkStream> GetAllUsersStreams();
}