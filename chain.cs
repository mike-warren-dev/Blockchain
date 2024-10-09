using System.Text;

public class Chain 
{
    private static Chain _instance;
    public List<Block> Blocks { get; set; }
    private readonly int _blockDifficulty = 3;

    private Chain() { 
        Blocks = new List<Block> { new Block(0, 42, "call me Satoshi", new byte[0]) };
    }

    // public Chain () {        
        
    // }

    public static Chain Instance {
         get
        {
            if (_instance == null)
                _instance = new Chain();

            return _instance;
        }
    }

    public bool AddBlock(Block block){
        string pointer = Encoding.ASCII.GetString(block.Content.PreviousBlock);
        string lastHash = Encoding.ASCII.GetString(GetLastBlock().Hash);

        if (pointer != lastHash)
            return false;

        if (GetLastBlock().Content.Index + 1 != block.Content.Index)
            return false;

        if (block.Hash.SequenceEqual(Block.HashBlock(block.Content.ToString())) == false)
            return false;

        var segment = new Span<byte>(block.Hash, 0, _blockDifficulty);

        if (segment.SequenceEqual(new byte[_blockDifficulty]) == false) 
            return false;

        Blocks.Add(block);

        return true;
    }

    public void GenerateNextBlock(string content) {
        int i = 0;
        var lastHash = GetLastBlock().Hash;
        var index = Blocks.Count;

        while (i < int.MaxValue){
            Block block = new Block(index, i, content, lastHash); 

            if (AddBlock(block)) {
                Console.WriteLine($"Tried {i+1} blocks.");
                Console.WriteLine($"New block created with hash {StringifyHash(block.Hash)}");
                
                //TODO:announce new block

                return;
            }

            i++;
        }
    }

    public Block GetLastBlock() {
        if (Blocks.Count == 0)
            throw new ArgumentException("Chain is empty, no block to return");
        else
            return Blocks.Last();
    }

    public bool ValidateChain() {

        if (Blocks.Count == 0)
            return false;
        if (Blocks.Count == 1)
            return true;

        bool result = true;

        for (var i = 1; i < Blocks.Count; i++){
            
            if (Blocks[i-1].Hash == Blocks[i].Content.PreviousBlock) 
                continue;
            else
                result = false;
            
        }

        return result;
    }

    private string StringifyHash (byte[] hash){
        StringBuilder hexString = new StringBuilder();

        foreach (byte b in hash)
        {
            hexString.Append(b.ToString("x2")); // "x2" formats to 2-digit hexadecimal
        }
            
        
        return hexString.ToString();
    }
}