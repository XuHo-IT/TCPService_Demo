using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ChatSever
{
    public partial class MainWindow : Window
    {
        private TcpListener _server;

        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }
        private async void StartServer()
        {
            _server = new TcpListener(IPAddress.Any, 13000);
            _server.Start();
            ServerLogs.Items.Add("Server started...");

            while (true)
            {
                var client = await _server.AcceptTcpClientAsync();
                ServerLogs.Items.Add("Client connected.");
                _ = Task.Run(() => HandleClient(client));
            }
        }
        private async Task HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[256];
            int bytes;

            while ((bytes = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytes);
                Dispatcher.Invoke(() =>
                {
                    ServerLogs.Items.Add($"Client: {message}");
                });
                string response = message.ToUpper();
                byte[] data = Encoding.ASCII.GetBytes(response);
                await stream.WriteAsync(data, 0, data.Length);
            }
        }
    }

}