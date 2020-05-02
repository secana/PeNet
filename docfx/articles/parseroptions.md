# Parser Options

There are different options to parse a PE file. Depending on the size of the file, expected memory usage or runtime performance it makes sense to try out another option.

For a comparison of different methods and their impact on performance and memory usage have a look at these benchmarks:

- [.NET Random Access Performance on Files - Part 1](https://secanablog.wordpress.com/2020/03/04/net-random-access-performance-on-files/)
- [.NET Random Access Performance on Files - Part 2](https://secanablog.wordpress.com/2020/04/23/net-random-access-performance-on-files-part-2/)

## Open a PE file

You can open a file on disk or parse a `byte array` of a PE file. This methods is the best if you have **very small files**, or intend to **write/change the PE file**, because a whole copy of the file is hold in memory and only the copy is changed, not the original file on disk. You can than store the changes to a new file.

For larger files the memory usage reflects the size of the input file and the performance starts to decrease with files larger than 10 MB.

```csharp
var peHeader = new PeNet.PeFile(@"C:\Windows\System32\kernel32.dll");
```

```csharp
var bin = File.ReadAllBytes(@"C:\Windows\System32\kernel32.dll");
var peHeader = new PeNet.PeFile(bin);
```

Another option is to open a file as a `Stream`, for example a `FileStream`. This keeps the memory usage low and constant, regardless of the file size. The downside is a decreased performance and if you work on a file stream, all change are written directly to the original file, which can lead to unwanted side-effects.

```csharp
using var fileStream = File.OpenRead(@"C:\Windows\System32\kernel32.dll");
var peHeader = new PeNet.PeFile(fileStream);
```

The fastest method for **large files** with the **lowest memory consumption** of all methods is a memory mapped file. As with the streams, all writes are on the original input file!

```csharp
using var mmf = new PeNet.FileParser.MMFile(@"C:\Windows\System32\kernel32.dll");
var peHeader = new PeNet.PeFile(mmf);
```

## PE file test

To test if a file is a PE file, you do can use the following methods.

```csharp
using PeNet;
var file = @"C:\Windows\System32\kernel32.dll";

// Read directly from a path to file. Uses a stream to only read the needed bytes into memory.
var isPe1 = PeFile.IsPeFile(file);

// Use a buffer to check if the file is a PE file.
var buff = File.ReadAllBytes(file);
var isPe2 = PeFile.IsPeFile(buff);

// Uses a stream to check if "file" is a PE file.
using var fs = File.OpenRead(file);
var isPe3 = PeFile.IsPeFile(fs);

// Uses a memory mapped file to check if "file" is a PE file.
using var mmf = new MMFile(file);
var isPe4 = PeFile.IsPeFile(mmf);
```

## Try Parse

To combine the check, if a file is PE file, and the parsing of the file in the "is PE file" case, there are several `TryParse` methods available. If the file is a PE file, `true` is returned and the `out parameter` contains the parsed PE file. If the file is not a PE file, `false` is returned and the `out parameter` is `null`.

```csharp
using PeNet;
var file = @"C:\Windows\System32\kernel32.dll";

// From file path. Uses a FileStream internally.
var isPe1 = PeFile.TryParse(file, out var peFile);

// From a byte array.
var buff = File.ReadAllBytes(file);
var isPe2 = PeFile.TryParse(buff, out var peFile);

// From a stream.
using var fs = File.OpenRead(file);
var isPe3 = PeFile.TryParse(fs, out var peFile);

// From a memory mapped file.
using var mmf = new MMFile(file);
var isPe4 = PeFile.TryParse(mmf, out var peFile);
```
