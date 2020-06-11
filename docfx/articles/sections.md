# Sections

You can not only access the sections in the PE file but also add and remove section.

## Access sections

Access the section table entries.

```csharp
var peFile = new PeFile("myapp.exe");

// Get array with all section table entries.
var sections = peFile.ImageSectionHeaders;
```

## Add section

Adds a new section at the end of the file.

```csharp
var peFile = new PeFile("myapp.exe");

// Add a new section with the name ".name", the size 100 and section characteristics.
// Section names have a max. length of 8 characters.
peFile.AddSection(".name", 100, (ScnCharacteristicsType)0x40000040);
```

## Remove section

```csharp
var peFile = new PeFile("myapp.exe");

// Remove the resource section from the section table and the content
// of the section from the file.
peFile.RemoveSection(".rsrc");

// Alternatively you can only remove the section from the section table
// and keep the content of the section in the file.
peFile.RemoveSection(".rsrc", false);
```
