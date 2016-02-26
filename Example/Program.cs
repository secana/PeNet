using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");

            var trie = new PeNet.PatternMatching.Trie();
            trie.Add("MicrosoftCalculator", Encoding.ASCII, "pattern1");
            trie.Add("<assemblyIdentity", Encoding.ASCII, "pattern2");
            trie.Add("not in the binary", Encoding.ASCII, "pattern3");
            trie.Add(new byte[] {0x54, 0x40, 0x00, 0x00, 0xe0, 0x41}, "pattern4");
            trie.Build();

            var matches = trie.Find(pe.Buff);
            foreach (var match in matches)
            {
                Console.WriteLine($"Pattern {match.Item1} at offset {match.Item2}");
            }
            Console.ReadKey(true);
        }
    }
}
