using System.Net.Sockets;

class Program
{
    static void ConnectServer(string server, int port)
    {
        string message, responseData;
        int bytes;
        try
        {
            TcpClient client = new TcpClient(server, port);
            Console.Title = "Client Application";
            NetworkStream stream = null;
            while (true)
            {
                Console.Write("Input message <press Enter to exit>:");
                message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    break;
                }
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);
                data = new byte[256];
                bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
            }
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }
    }

    static void Main(string[] args)
    {
        string server = "127.0.0.1";
        int port = 13000;
        ConnectServer(server, port);
    }
}