using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat
{
   
    public partial class ChatPage : Page
    {
        private Frame _frame;
        private StackPanel _panel;
        private User _user;
        private TcpClient _client;
        private NetworkStream _stream;
        public ChatPage(Frame frame, string userName)
        {
            _frame = frame;
            _panel = new StackPanel();
            _user = new User(userName);

            Loaded += ChatPage_Loaded;
            InitializeComponent();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            DisconnectUser(_client, _stream);
            _frame.NavigationService.GoBack();
           
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            { 
                string message =txtMessage.Text;
                byte[] bytesToSend = Encoding.UTF8.GetBytes(message);
                _stream.Write(bytesToSend, 0, bytesToSend.Length);
                txtMessage.Clear();
            }
        }

        private async void ChatPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ConnectionSetup();
        }

        public async Task ConnectionSetup()
        {
            _client = new TcpClient();
            await _client.ConnectAsync("127.0.0.1", 8888);
            _stream = _client.GetStream();

            try
            {
                SendUserData(_user, _stream);
                var receiver = Task.Run(() => {
                ReceiveMessage(_user, _stream, _client);
                });
            }
            catch
            {
                _client.Close();
                _frame.Navigate(new RegisterPage(_frame));
            }
        }

        
        private void SendUserData(User user, NetworkStream stream)
        {
            string jsonData = JsonSerializer.Serialize(user);
            byte[] userData = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(userData, 0, userData.Length);
        }

        private async void ReceiveMessage(User user, NetworkStream stream, TcpClient client)
        {
            try
            {
                while (client.Connected)
                {
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    if (bytesToRead.Length > 0)
                    {
                        int bytesRead = _stream.Read(bytesToRead, 0, _client.ReceiveBufferSize);
                        string receivedText = Encoding.UTF8.GetString(bytesToRead, 0, bytesRead);

                        await Dispatcher.InvokeAsync(() =>
                        {
                            var textBlock = new TextBlock();
                            textBlock.Text = receivedText;
                            textBlock.TextWrapping = TextWrapping.Wrap;
                            textBlock.Margin = new Thickness(0, 0, 0, 10);
                            ((StackPanel)txtDialog.Content).Children.Add(textBlock);
                        });

                    }
                }
            }
            catch
            {
                _client.Close();  
                _frame.Navigate(new RegisterPage(_frame));
            }
        }

        public void DisconnectUser(TcpClient client, NetworkStream stream)
        {
            byte[] emptyData = new byte[0];
            stream.Write(emptyData, 0, 0);
            client.Close();
            client.Dispose();
        }

    }

    public class User
    { 
        public string Name { get; set; }

        public User(string name) 
        {
            Name = name;
        }
    }
}
