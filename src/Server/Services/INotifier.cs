public interface INotifier
{
    void SendUserLeftNotification(TcpClient client, User currentUser);

    void SendUserJoinedNotification(User currentUser);
}