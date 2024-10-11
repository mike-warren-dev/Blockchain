using System; 
using System.Net.Sockets;
using System.Text;

public class TcpClientHandler
{
    public void ConnectToServer(string serverIP, int port, string message)
    {
        try
        {
            TcpClient client = new TcpClient(serverIP, port);
            NetworkStream stream = client.GetStream(); 

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            //Buffer to store the response bytes
            data = new Byte[256];

            //string to store the response ASCII representation. 
            String responseData = String.Empty;

            //read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine($"Response: {responseData}");
        }
        catch 
        {
            Console.WriteLine("error");
        }
    }
}