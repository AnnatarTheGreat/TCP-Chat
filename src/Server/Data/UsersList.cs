
public class UsersList : IUsersList
{
    private Dictionary<Socket, NetworkStream> _users;

    public UsersList()
    {
        _users = new Dictionary<Socket, NetworkStream>();
    }
    public void Add(Socket client, NetworkStream stream)
    {
        _users.Add(client, stream);
    }
    public void Remove(Socket client)
    {
        _users.Remove(client);
    }

    public ICollection<NetworkStream> GetAllUsersStreams()
    {
        return _users.Values;
    }
}