public interface INotifier
{
    void SendUserLeftNotification(Socket client, User currentUser);

    void SendUserJoinedNotification(User currentUser);
}