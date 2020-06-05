# Imports

You can access the imported functions and also add imports to the PE file.

## Access Import Descriptors

The following code snipped shows how to access the `IMAGE_IMPORT_DESCRIPTOR`, `IMAGE_BOUND_IMPORT_DESCRIPTOR` and `IMAGE_DELAY_IMPORT_DESCRIPTOR` arrays.

```csharp
var peFile = new PeFile("myapp.exe");

var idescs = peFile.ImageImportDescriptors;
var bdescs = peFile.ImageBoundImportDescriptor;
var ddescs = peFile.ImageDelayImportDescriptor;
```

## Access imported functions

As a shortcut, it's possible to access the imported functions sorted by the module/DLL they are imported from.

```csharp
var peFile = new PeFile("myapp.exe");

// Print all imported modules with their corresponding functions.
foreach(var imp in peFile.ImportedFunctions)
{
    Console.WriteLine($"{imp.DLL} - {imp.Name} - {imp.Hint} - {imp.IATOffset}");
}
```

## Add Imports

It is also possible to add new imports to the PE file. If the PE file is signed, the signature will be invalid afterwards.

To add only **one import**, use the code below.

```csharp
var peFile = new PeFile("myapp.exe");
peFile.AddImport("gdi32.dll", "StartPage");
```

If you intend to add **multiple imports** from either the same module or different modules, use the function below.

```csharp
var peFile = new PeFile("myapp.exe");

var ai1 = new AdditionalImport("gdi32.dll", new List<string> { "StartPage" });
var ai2 = new AdditionalImport("ADVAPI32.dll", new List<string> { "RegCloseKey" });
var importList = new List<AdditionalImport> {ai1, ai2};

peFile.AddImports(importList);
```
