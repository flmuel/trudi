{ pkgs ? import <nixpkgs> {} }:

let
  optionalIfPresent = name:
    if builtins.hasAttr name pkgs then [ (builtins.getAttr name pkgs) ] else [ ];
  dotnetSdkPackage =
    if builtins.hasAttr "dotnet-sdk_8" pkgs then pkgs.dotnet-sdk_8
    else pkgs.dotnet-sdk;
  nodejsPackage =
    if builtins.hasAttr "nodejs_22" pkgs then pkgs.nodejs_22
    else pkgs.nodejs;
in
pkgs.mkShell {
  name = "trudi-dev-shell";

  packages = with pkgs; [
    dotnetSdkPackage
    nodejsPackage
  ]
}
