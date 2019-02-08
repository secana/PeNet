# Import Hash

The **Import Hash** (ImpHash) is a hash over the imported functions by PE file. It is often used in malware analysis to identify malware binaries that belong to the same family.

You can access the **Import Hash** with **PeNet** like this:

```csharp
var ih = peHeader.ImpHash
```

The algorithm works like the following:

Resolve ordinals to function names when they appear and convert DLL name and function name to lower case. Now, remove the file extension from the imported module names. Store all strings in an ordered list and generate a MD5 over the list. The DLLs oleaut32, ws2_32 and wsock32 have a lot of ordinals without function names. That's why these DLL function names are resolved manually. This implementation is equal to the pefile 1.2.10-139 implementation of the ImpHash.