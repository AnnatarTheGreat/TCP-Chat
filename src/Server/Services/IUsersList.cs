public interface IUsersList
{
    void Add(TcpClient client, NetworkStream stream);
    void Remove(TcpClient client);
    ICollection<NetworkStream> GetAllUsersStreams();
}