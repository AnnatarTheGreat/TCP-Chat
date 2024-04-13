public class Notifier : INotifier
{
    private IUsersList _users;
    public Notifier(IUsersList users)
    {
        _users = users;
    }

    public void SendUserLeftNotification(Socket client, User currentUser)
    {
        try
        {
            string quitMessage = $"{currentUser.Name} has left the chat";
            Console.WriteLine(quitMessage);
            byte[] farewellBuffer = Encoding.UTF8.GetBytes(quitMessage);
            _users.Remove(client);
            foreach (var user in _users.GetAllUsersStreams())
            {
                user.Write(farewellBuffer, 0, farewellBuffer.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.Close();
        }
    }


    public void SendUserJoinedNotification(User currentUser)
    {
        string greetings = $"{currentUser.Name} has joined the chat";
        Console.WriteLine(greetings);
        byte[] greetingsBuffer = Encoding.UTF8.GetBytes(greetings);
        foreach (var user in _users.GetAllUsersStreams())
        {
            user.Write(greetingsBuffer, 0, greetingsBuffer.Length);
        }
    }

}