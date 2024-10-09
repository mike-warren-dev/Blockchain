using System.Runtime.CompilerServices;
using System.Text;

// TcpServer server = new();
// server.StartServer(2339);

PeerManager peerManager = new(); 

Chain chain = Chain.Instance;

Console.WriteLine(chain.Blocks.Count);

while (chain.Blocks.Count < 10) {
    Console.WriteLine($"Current count = {chain.Blocks.Count}");
    chain.GenerateNextBlock(Guid.NewGuid().ToString());
}

// Console.WriteLine("done baybee!");

// TcpServer server = new();

// server.StartServer(2339);

//TODOS
//need to be able to hash a block.
//need to be able to add blocks to a chain. 
//need to be able to validate the chain. 
//need to be able to search for solutions. 