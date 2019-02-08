# Pattern Matching

The [Aho-Corasick](https://en.wikipedia.org/wiki/Aho–Corasick_algorithm)  algorithm allows fast matching of pattern on binary files, useful for malware analysis. The **Aho-Corasick** implementation in **PeNet** is based on the work of [Pēteris Ņikiforovs](https://github.com/pdonald/aho-corasick).

```csharp
var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");

var trie = new PeNet.PatternMatching.Trie();

// Add string patterns which should match on ASCII encoded strings in the PE file.
trie.Add("MicrosoftCalculator", Encoding.ASCII, "pattern1");
trie.Add("<assemblyIdentity", Encoding.ASCII, "pattern2");
trie.Add("not in the binary", Encoding.ASCII, "pattern3");

// Add a byte pattern to match for.
trie.Add(new byte[] {0x54, 0x40, 0x00, 0x00, 0xe0, 0x41}, "pattern4");

// Match all pattern in the PE file.
var matches = trie.Find(pe.Buff);

foreach (var match in matches)
{
    Console.WriteLine($"Pattern {match.Item1} at offset {match.Item2}");
}
```

The output for this code would be:

```none
Pattern pattern1 at offset 2538
Pattern pattern4 at offset 9993
Pattern pattern2 at offset 12450
Pattern pattern2 at offset 12675
```
