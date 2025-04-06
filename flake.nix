{
  description = "Development shell for .NET 8.0";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };

  outputs = { self, nixpkgs }: {
    devShells.default = nixpkgs.lib.mkShell {
      buildInputs = [
        (nixpkgs.dotnetCorePackages.sdk_8_0)
      ];
    };
  };
}
      
