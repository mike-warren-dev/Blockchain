using System.Runtime.CompilerServices;
using System.Text;

 TcpServer server = new();
 server.StartServer(2339);

// PeerManager peerManager = new(); 

// Chain chain = Chain.Instance;

// Console.WriteLine(chain.Blocks.Count);

// while (chain.Blocks.Count < 10) {
//     Console.WriteLine($"Current count = {chain.Blocks.Count}");
//     chain.GenerateNextBlock(Guid.NewGuid().ToString());
// }

// Console.WriteLine("done baybee!");

// TcpServer server = new();

// server.StartServer(2339);

//TODOS
//FIRST, figure out how to make both my tcpServer and tcpClient methods async instead of blocking. 
//On startup, start server. 
//    > How to handle conflicting/occupied ports? 
//On startup, create client singleton(?)
//    > figure out how client can listen without blocking. 
//When find new block, broadcast. 
//    > generate string for transmission
//    > transmit
//    > handle response 
//listen for new block, try to add, 
//    > handle bad block, how to move to new block if good block. 