
using System.Net.Sockets;

public abstract class AbstractServer
{
    protected abstract User ReceiveCurrentUser(TcpClient client,NetworkStream stream);

    protected abstract void ProcessClientAsync(TcpClient client, NetworkStream stream, User currentUser);

    protected abstract void ListenForMessages(TcpClient client, NetworkStream stream, User currentUser);

    protected abstract void SendUserLeftNotification(TcpClient client, User currentUser);
}