# TypeRefHash

The TypeRefHash (TRH) is a hash similar to the ImpHash, but for .NET samples where the ImpHash does not work.
It is calculated over the imported .NET namespaces and types and can be used to identify malware families which share code.

```csharp
var peFile = new PeFile(file);

// get the TRH as a hex-string.
var trh = peFile.TypeRefHash;

Console.WriteLine(trh);
// prints for example the TRH:
// > d633db771449e2c37e1689a8c291a4f4646ce156652a9dad5f67394c0d92a8c4
```

For more information on the TRH see: [Introducing the TypeRefHash (TRH)](https://www.gdatasoftware.com/blog/2020/06/36164-introducing-the-typerefhash-trh)