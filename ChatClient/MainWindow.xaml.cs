using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            _client = new TcpClient("127.0.0.1", 13000);
            _stream = _client.GetStream();
            ChatMessages.Items.Add("Connected to server.");

            await Task.Run(() => ReceiveMessages());
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (!string.IsNullOrEmpty(message))
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                await _stream.WriteAsync(data, 0, data.Length);

                ChatMessages.Items.Add($"Me: {message}");
                MessageInput.Clear();
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[256];
            int bytes;
            while ((bytes = _stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string response = Encoding.ASCII.GetString(buffer, 0, bytes);
                Dispatcher.Invoke(() =>
                {
                    ChatMessages.Items.Add($"Server: {response}");
                });
            }
        }
    }
}
