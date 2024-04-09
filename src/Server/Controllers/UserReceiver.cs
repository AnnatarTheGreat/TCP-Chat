using System.Text.Json;

public class UserReceiver : IUserReceiver
{
    public User ReceiveCurrentUser(TcpClient client, NetworkStream stream)
    {
        try
        {
            var dataBuffer = new byte[1024];
            int receivedData = stream.Read(dataBuffer, 0, dataBuffer.Length);
            string jsonString = Encoding.UTF8.GetString(dataBuffer, 0, receivedData);
            return JsonSerializer.Deserialize<User>(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.Close();
            return null;
        }

    }
}