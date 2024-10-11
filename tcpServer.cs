using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TcpServer
{
    private TcpListener listener;
    private PeerManager peerManager;
    public TcpServer () {
        listener = new TcpListener(IPAddress.Any, 1);
        peerManager = new PeerManager();
    }

    public void StartServer(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Server started. Listening on port {port}...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");
            NetworkStream stream = client.GetStream();

            // Receive data
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // ParseCall(receivedData);
            IPEndPoint remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            string clientAddress = remoteEndPoint.Address.ToString();
            int clientPort = remoteEndPoint.Port;

            Console.WriteLine($"Received: {receivedData} from {clientAddress} on port {clientPort}");

            //route the call

            string[] parts = receivedData.Split('|');

            switch (parts[0])
            {
                case "intro":
                    IntroParser(parts, clientAddress, clientPort);
                    break;
                case "nbloc":
                    NewBlockParser(parts);
                    break;
                case "lbloc":
                    LastBlockParser(parts);
                    break;
                case "peers":
                    GetPeersParser(parts);
                    break;
                case "blocs":
                    GetBlocksParser(parts);
                    break;
                default:
                    ReturnError(parts);
                    break;
            }

            // Send a response
            byte[] response = Encoding.ASCII.GetBytes("Message Received (robot voice)");
            stream.Write(response, 0, response.Length);

            client.Close();
        }
    }

    public void StopServer()
    {
        listener.Stop();
        Console.WriteLine("Server stopped.");
    }

    public void IntroParser(string[] parts, string clientAddress, int clientPort)
    {
        string introId = parts[0];
        int port = Int32.TryParse(parts[1], out int temp) ? temp : -1;

        Peer? peer = peerManager.Peers.FirstOrDefault(o => o.id == introId && o.ip == clientAddress);
        
        //intro|{guid}|{port}
        //data = {guid}|{port}

        if (peer == null)        
            peerManager.Peers.Add(new Peer {id=introId, ip=clientAddress, port = clientPort, status = "Active", lastContact = DateTime.UtcNow});
        else
        {
            peer.lastContact = DateTime.UtcNow;
            peer.port = clientPort;
        }
    }

    public void NewBlockParser(string[] parts)
    {
        //nblock|b635d80a154e89d4eea6350d16214604114959e84cdc66eb25676f9359422ef4|1|1|some text|b635d80a154e89d4eea6350d16214604114959e84cdc66eb25676f9359422ef4
        //{call}|{64chars}|
        int index = Int32.Parse(parts[1]);
        
        if (index < Chain.Instance.Blocks.Count) 
        {
            //reply to inform sender they're behind
            return;
        }
        else if (index > Chain.Instance.Blocks.Count)
        {
            //investigate, as receiver might be behind!
            return;
        }
        //else index is correct
        
        byte[] hash = HexStringToByteArray(parts[0]);//convert from hex to byte array
        int nonce = Int32.Parse(parts[2]);
        string stuff = parts[3];
        byte[] previousBlock = HexStringToByteArray(parts[4]);

        //lo8k9o8po9-[08o;/o8iefdr]
        // DONE:  Need to figure out how a stringified block will look. 
        // DONE:  Need to do quick check on blockindex. 
        // Need to be able to convert back into bytes to verify. 

        // public byte[] Hash { get; set; }
        // public Contents Content { get; set; }
            // public int Index { get; set; }
            // public int Nonce { get; set; }
            // public string Stuff { get; set; }
            // public byte[] PreviousBlock { get; set; }

    }

    public void LastBlockParser(string[] parts)
    {
        
    }

    public void GetPeersParser(string[] parts)
    {
        
    }

    public void GetBlocksParser(string[] parts)
    {
        
    }

    public void ReturnError(string[] parts)
    {
        
    }

    private byte[] HexStringToByteArray(string hex)
    {
        // Ensure the hex string has an even length
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Invalid hex string");

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            // Convert each 2-character hex string into a byte
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

}
