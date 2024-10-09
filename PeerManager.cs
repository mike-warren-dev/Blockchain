using System;
using System.IO;
using System.Text.Json;

public class PeerManager {
    public List<Peer> Peers;
    public PeerManager()
    {
        string filePath = "peers.json";
        
        // Read the JSON file
        string jsonString = File.ReadAllText(filePath);
        
        // Deserialize the JSON string into an object
        Peers = JsonSerializer.Deserialize<List<Peer>>(jsonString);
        
        // Output the object properties

        if (Peers != null)
            foreach (Peer peer in Peers) {
                Console.WriteLine($"IP: {peer.ip}");
            Console.WriteLine($"ID: {peer.port}");
            Console.WriteLine($"ID: {peer.id}");
            Console.WriteLine($"Status: {peer.status}");
            Console.WriteLine($"lastContact: {peer.lastContact}");
            }
        else
        {
            Peers = new List<Peer>();
            Console.WriteLine("Peers is null bruh wtf");
        }
            
    }
}

public class Peer
{
    public required string ip { get; set; }
    public required string id { get; set; }
    public required int port { get; set; }
    public required string status { get; set; }
    public DateTime? lastContact { get; set; }
}