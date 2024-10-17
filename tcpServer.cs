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
            string responseString = parts[0] switch {
                "intro" => IntroParser(parts, clientAddress, clientPort),
                "nbloc" => NewBlockParser(parts),
                "lbloc" => LastBlockParser(parts),
                "peers" => GetPeersParser(parts),
                "blocs" => GetBlocksParser(parts),
                _ => ReturnError(parts)
            }; 

            // switch (parts[0])
            // {
            //     case "intro":
            //         IntroParser(parts, clientAddress, clientPort);
            //         break;
            //     case "nbloc":
            //         NewBlockParser(parts);
            //         break;
            //     case "lbloc":
            //         LastBlockParser(parts);
            //         break;
            //     case "peers":
            //         GetPeersParser(parts);
            //         break;
            //     case "blocs":
            //         GetBlocksParser(parts);
            //         break;
            //     default:
            //         ReturnError(parts);
            //         break;
            // }

            // Send a response
            byte[] response = Encoding.ASCII.GetBytes(responseString);
            stream.Write(response, 0, response.Length);

            client.Close();
        }
    }

    public void StopServer()
    {
        listener.Stop();
        Console.WriteLine("Server stopped.");
    }

    public string IntroParser(string[] parts, string clientAddress, int clientPort)
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

        //TODO: RETURN REAL STRING!
        return "";
    }

    public string NewBlockParser(string[] parts)
    {
        //nblock|b635d80a154e89d4eea6350d16214604114959e84cdc66eb25676f9359422ef4|1|1|some text|b635d80a154e89d4eea6350d16214604114959e84cdc66eb25676f9359422ef4
        //{call}|{64chars}|
        int index = Int32.Parse(parts[1]);
        
        if (index < Chain.Instance.Blocks.Count) 
        {
            //TODO: reply to inform sender they're behind
            return "";
        }
        else if (index > Chain.Instance.Blocks.Count)
        {
            //TODO: investigate, as receiver might be behind!
            return "";
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


        //TODO: handle all possibilities! ALL!!!!!1!
        return "";
    }

    public string LastBlockParser(string[] parts)
    {
        return "";
    }

    public string GetPeersParser(string[] parts)
    {
        return "";
    }

    public string GetBlocksParser(string[] parts)
    {
        return "";
    }

    public string ReturnError(string[] parts)
    {
        return "error";
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
