using System.Collections;
using System.Net.Sockets;

public class UsersList : IUsersList
{
    private Dictionary<TcpClient, NetworkStream> _users;

    public UsersList()
    {
        _users = new Dictionary<TcpClient,NetworkStream>();
    }
    public void Add(TcpClient client, NetworkStream stream)
    {
        _users.Add(client, stream);
    }    
    public void Remove(TcpClient client)
    {
        _users.Remove(client);
    }

    public ICollection<NetworkStream> GetAllUsersStreams()
    {
        return _users.Values;
    }
}