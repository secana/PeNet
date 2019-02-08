# File Hashes

When comparing PE files for equality, file hashes come in handy. **PeNet** exports the following file hashes for convenience reasons.

The MD5 can be accessed with the property MD5

```csharp
var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");
Console.WriteLine($"MD5: {pe.MD5}");
```

The SHA-1 can be accessed with the property SHA1

```csharp
var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");
Console.WriteLine($"SHA-1: {pe.SHA1}");
```

The SHA-256 can be accessed with the property SHA256

```csharp
var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");
Console.WriteLine($"MD5: {pe.MD5}");
```