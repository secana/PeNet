# PeNet
<p align="center">
    <img src="https://github.com/secana/PeNet/blob/devel/PEditor/Icons/PEditor.png" />
</p>
PeNet is a parser for Windows Portable Executable headers. It completely written in C# and does not rely on any native Windows APIs. The only exception is the signature check for PE files. Since .Net has no function for it, the native API is use transparently here.
Furthermore it supports the creation of ImpHashs, which is a feature often used in malware analysis. You can extract Certificate Revocation List, compute different Hash sums and other useful stuff for working with PE files.

For help see the Wiki.

The API reference can be found hrere: http://secana.github.io/PeNet

# License Apache 2
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
