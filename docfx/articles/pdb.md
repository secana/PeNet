# PDB & Debug Information

*PeNet* is able to extract *Code View PDB v7* information if one is present in a debug entry in the debug directory.

The information is valuable for malware analysis, as the `PdbFileName` if often unique to a malware family.

For memory forensics with [Rekall](https://github.com/google/rekall) or any debugger that uses PDB files, the `Signature` GUID can be used to download the correct PDB file from the [Microsoft public symbol server](https://docs.microsoft.com/en-us/windows-hardware/drivers/debugger/microsoft-public-symbols).

```csharp
using System;
using System.Linq;
using PeNet;

namespace Pdb
{
    class Program
    {
        static void Main(string[] args)
        {
            var peFile = new PeFile("peWithDbgInfo.exe");

            // Select the first debug directory with
            // PDB information available.
            var pdbInfo = peFile
                .ImageDebugDirectory
                .First(idb => idb.CvInfoPdb70 != null)
                .CvInfoPdb70;

            // Print content of the Code View PDB v7 structure
            Console.WriteLine(pdbInfo);
        }
    }
}
```

Output:

```shell
CvInfoPdb70
CvSignature: 1396986706
Signature: 0de6dc23-8e19-4bb7-8608-d54b1e6fa379
Age: 1
PdbFileName: ntkrnlmp.pdb
```
