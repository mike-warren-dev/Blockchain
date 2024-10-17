using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncTcpServer
{
    private TcpListener listener;
    private PeerManager peerManager;
    public AsyncTcpServer () {
        listener = new TcpListener(IPAddress.Any, 1);
        peerManager = new PeerManager();
    }
    public async Task StartServer(int port)
    {
        TcpListener listener = null;

        try
        {
            // Set up the TCP listener to listen on any IP address and the specified port
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server started. Listening on port " + port);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");

                // Accept a client connection asynchronously
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected!");

                // Handle the client connection asynchronously in a separate task
                _ = HandleClientAsync(client);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients
            listener?.Stop();
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        // Buffer for reading data
        byte[] buffer = new byte[1024];

        try
        {
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            // Read client data asynchronously
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received: " + clientMessage);

            // Send a response back to the client
            string response = "Hello from the async server!";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            Console.WriteLine("Sent: " + response);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
        }
        finally
        {
            // Close the connection
            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}
