# About **PeNet**

PeNet is a library to parse and analyse Windows [Portable Executables (PE)](https://docs.microsoft.com/en-us/windows/desktop/Debug/pe-format) files. It is completely written in C# and compiles to a cross-platform conform .Net Standard library.

Besides access to all typical PE structures (native and .Net header), some utility function like the [Import Hash](https://www.fireeye.com/blog/threat-research/2014/01/tracking-malware-import-hashing.html) used in malware-analysis are provided.

## Quick Start

This paragraph gives a short introduction on how to use PeNet with a few examples. For a full API documentation, see the link in the header. For more example check the Article link in the header.

### Install the library

You can install PeNet into your project directly from [Nuget](https://www.nuget.org/packages/PeNet/).

### Open a PE file

You can open a file on disk or parse a byte array of a PE file.

```csharp
var peHeader1 = new PeNet.PeFile(@"C:\Windows\System32\kernel32.dll");
```

```csharp
var bin = File.ReadAllBytes(@"C:\Windows\System32\kernel32.dll");
var peHeader2 = new PeNet.PeFile(bin);
```

### Work with the PE header

The parsed PE header is split into multiple modules and sub-modules. To see how the parser structures the PE header and which information can be found where see the API documentation page (link in header). Here are a few examples on how to access different parts of the PE header. For more examples, check the Article link in the header.

Get the file alignment of the PE file:

```csharp
var fileAlignment = peHeader.WindowsSpecificFields.FileAlignment;
```

Get the import descriptors of the PE file:

```csharp
var if = peHeader.DataDirectories.ImageImportDescriptors;
```

Get the imported and exported functions of the PE file in a parsed form:

```csharp
var if = peHeader.ImportedFunctions;
var ef = peHeader.ExportedFunctions;
```
