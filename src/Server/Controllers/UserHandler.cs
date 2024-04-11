public class UserHandler : IUserHandler
{
    private IUsersList _users;
    private readonly IMessageHandler _messageListener;
    private readonly INotifier _notifier;


    public UserHandler(IMessageHandler messageListener, INotifier notifier, IUsersList users)
    {
        _messageListener = messageListener;
        _notifier = notifier;
        _users = users;
    }


    public async void ProcessClientAsync(TcpClient client, NetworkStream stream, User currentUser)
    {
        try
        {
            _notifier.SendUserJoinedNotification(currentUser);
            _users.Add(client, stream);
            _messageListener.ListenForMessages(client, stream, currentUser);
            _notifier.SendUserLeftNotification(client, currentUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.Close();
        }
    }

}