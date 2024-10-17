using System; 
using System.Net.Sockets;
using System.Text;

public class TcpClientHandler
{
    private static async Task ConnectToServer(string serverIP, int port, string message)
    {
        try
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(serverIP, port);

            NetworkStream stream = client.GetStream(); 

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);

            //Buffer to store the response bytes
            data = new Byte[256];

            //string to store the response ASCII representation. 
            String responseData = String.Empty;

            //read the first batch of the TcpServer response bytes.
            Int32 bytes = await stream.ReadAsync(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine($"Response: {responseData}");

            stream.Close();
        }
        catch 
        {
            Console.WriteLine("error");
        }
    }

    public static string GetInput()
    {
        return Console.ReadLine() ?? "";
    }

    public static async Task SendMessage(string serverIP, int port, string message)
    {
        await ConnectToServer(serverIP, port, message);
    }
}