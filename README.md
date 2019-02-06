# PeNet
<p align="center">
    <img src="https://github.com/secana/PeNet/blob/master/src/PEditor/Icons/PEditor.png" />
</p>

[![license](https://img.shields.io/github/license/secana/penet.svg)](https://raw.githubusercontent.com/secana/PeNet/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PeNet.svg)](https://www.nuget.org/packages/PeNet/)
[![NuGet](https://img.shields.io/nuget/dt/PeNet.svg)](https://www.nuget.org/packages/PeNet/)

PeNet is a parser for Windows Portable Executable headers. It completely written in C# and does not rely on any native Windows APIs.
Furthermore it supports the creation of Import Hashes (ImpHash), which is a feature often used in malware analysis. You can extract Certificate Revocation List, compute different hash sums and other useful stuff for working with PE files.

## Getting started

For help see the [Wiki](https://github.com/secana/PeNet/wiki).

## API Reference

The API reference can be found here: [PeNet API Reference](http://secana.github.io/PeNet).

## Continuous Integration

The project is automatically build, tested and released with an [Azure Pipeline](https://dev.azure.com/secana/PeNet).
