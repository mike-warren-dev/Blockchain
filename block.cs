using System.Text;
using System.Security.Cryptography;

public class Block{
    public Contents Content { get; set; }
    public byte[] Hash { get; set; }

    public Block(int index, int nonce, string content, byte[] previousBlock )
    {
        Content = new Contents(index, nonce, content, previousBlock);
        Hash = HashBlock(Content.ToString());
    }

    public static byte[] HashBlock(string content) {
        
        if (content.Length == 0)
            throw new ArgumentException("Can't hash an empty block");

        byte[] bytes;

        using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash as a byte array
                bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(content));
            }

        return bytes;
    }

        public override string ToString() {
            return $"{Content.ToString()}{StringifyHash(Hash)}";
        // return $"{Content.ToString()}\n{Encoding.ASCII.GetString(Hash)}";
    }

    public static string StringifyHash(byte[] hash) {
         
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hash)
        {
            sb.Append((char)b);
        }

        return sb.ToString();
    }
}

public class Contents
{
    public int Index { get; set; }
    public int Nonce { get; set; }
    public string Stuff { get; set; }
    public byte[] PreviousBlock { get; set; }

    public Contents(int index, int nonce, string stuff, byte[] previousBlock){
        Index = index;
        Nonce = nonce;
        Stuff = stuff;
        PreviousBlock = previousBlock;
    }

    public override string ToString() {
        // return $"{Nonce.ToString()}\n{Stuff}\n{Encoding.ASCII.GetString(PreviousBlock)}";
        return $"{Nonce.ToString()}\n{Stuff}\n{Block.StringifyHash(PreviousBlock)}";
    }
}