# Export Data

This article shows options to export data from PeNet.

## As String

All structures in PeNet support a `ToString()` method, which returns the members of the structure with their values as a string. Some arrays (especially with primitive types) are omitted. Some of the omitted arrays are present in the `ToJSON` method.

Print whole PE file as a string:

```csharp
var pefile = new PeNet.PeFile(@"c:\windows\system32\kernel32.dll");
Console.WriteLine(pefile);
```

Print one specific structure as a string:

```csharp
var pefile = new PeNet.PeFile(@"c:\windows\system32\kernel32.dll");
Console.WriteLine(pefile.ImageResourceDirectory);
```

Output: 

```none
IMAGE_RESOURCE_DIRECTORY
Characteristics: 0
TimeDateStamp: 0
MajorVersion: 0
MinorVersion: 0
NumberOfNameEntries: 0
NumberOfIdEntries: 1
```

## As JSON

All structures in PeNet support a `ToJson()` method, which returns the members of the structure with their values as a JSON string. Some byte arrays are omitted.

Print the whole PE file as a JSON string:

```csharp
var pefile = new PeNet.PeFile(@"c:\windows\system32\kernel32.dll");
var json = pefile.ToJson();
Console.WriteLine(json);
```

Print the whole PE file as a **formatted** JSON string:

```csharp
var pefile = new PeNet.PeFile(@"c:\windows\system32\kernel32.dll");
var json = pefile.ToJson(true);
Console.WriteLine(json);
```

Print one specific structure as a **formatted** JSON string:

```csharp
var pefile = new PeNet.PeFile(@"c:\windows\system32\kernel32.dll");
var json = pefile.ImageResourceDirectory.ToJson(true);
Console.WriteLine(json);
```

Output:

```json
{
  "DirectoryEntries": [
    {
      "ResourceDirectory": {
        "DirectoryEntries": [
          {
            "ResourceDirectory": {
              "DirectoryEntries": [
                {
                  "ResourceDirectory": null,
                  "ResourceDataEntry": {
                    "OffsetToData": 726104,
                    "Size1": 200,
                    "CodePage": 0,
                    "Reserved": 0
                  },
                  "Name": 1033,
                  "ResolvedName": "unknown",
                  "ID": 1033,
                  "OffsetToData": 128,
                  "OffsetToDirectory": 128,
                  "DataIsDirectory": false,
                  "IsNamedEntry": false,
                  "IsIdEntry": true
                }
              ],
              "Characteristics": 0,
              "TimeDateStamp": 0,
              "MajorVersion": 0,
              "MinorVersion": 0,
              "NumberOfNameEntries": 0,
              "NumberOfIdEntries": 1
            },
            "ResourceDataEntry": null,
            "Name": 1,
            "ResolvedName": "Cursor",
            "ID": 1,
            "OffsetToData": 2147483728,
            "OffsetToDirectory": 80,
            "DataIsDirectory": true,
            "IsNamedEntry": false,
            "IsIdEntry": true
          }
        ],
        "Characteristics": 0,
        "TimeDateStamp": 0,
        "MajorVersion": 0,
        "MinorVersion": 0,
        "NumberOfNameEntries": 0,
        "NumberOfIdEntries": 1
      },
      "ResourceDataEntry": null,
      "Name": 2147483808,
      "ResolvedName": "MUI",
      "ID": 160,
      "OffsetToData": 2147483680,
      "OffsetToDirectory": 32,
      "DataIsDirectory": true,
      "IsNamedEntry": true,
      "IsIdEntry": false
    },
    {
      "ResourceDirectory": {
        "DirectoryEntries": [
          {
            "ResourceDirectory": {
              "DirectoryEntries": [
                {
                  "ResourceDirectory": null,
                  "ResourceDataEntry": {
                    "OffsetToData": 725168,
                    "Size1": 932,
                    "CodePage": 0,
                    "Reserved": 0
                  },
                  "Name": 1033,
                  "ResolvedName": "unknown",
                  "ID": 1033,
                  "OffsetToData": 144,
                  "OffsetToDirectory": 144,
                  "DataIsDirectory": false,
                  "IsNamedEntry": false,
                  "IsIdEntry": true
                }
              ],
              "Characteristics": 0,
              "TimeDateStamp": 0,
              "MajorVersion": 0,
              "MinorVersion": 0,
              "NumberOfNameEntries": 0,
              "NumberOfIdEntries": 1
            },
            "ResourceDataEntry": null,
            "Name": 1,
            "ResolvedName": "Cursor",
            "ID": 1,
            "OffsetToData": 2147483752,
            "OffsetToDirectory": 104,
            "DataIsDirectory": true,
            "IsNamedEntry": false,
            "IsIdEntry": true
          }
        ],
        "Characteristics": 0,
        "TimeDateStamp": 0,
        "MajorVersion": 0,
        "MinorVersion": 0,
        "NumberOfNameEntries": 0,
        "NumberOfIdEntries": 1
      },
      "ResourceDataEntry": null,
      "Name": 16,
      "ResolvedName": "Version",
      "ID": 16,
      "OffsetToData": 2147483704,
      "OffsetToDirectory": 56,
      "DataIsDirectory": true,
      "IsNamedEntry": false,
      "IsIdEntry": true
    }
  ],
  "Characteristics": 0,
  "TimeDateStamp": 0,
  "MajorVersion": 0,
  "MinorVersion": 0,
  "NumberOfNameEntries": 1,
  "NumberOfIdEntries": 1
}
```