using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static async Task Main(string[] args)
    {

        var services = new ServiceCollection();

        services.AddSingleton<IMessageHandler, MessageHandler>();
        services.AddSingleton<IUserHandler, UserHandler>();
        services.AddSingleton<IUserReceiver, UserReceiver>();
        services.AddSingleton<INotifier, Notifier>();
        services.AddSingleton<IUsersList, UsersList>();
        services.AddSingleton<Server>();

        var serviceProvider = services.BuildServiceProvider();

        var server = serviceProvider.GetService<Server>();
        Console.Clear();
        await server.RunAsync();
    }
}