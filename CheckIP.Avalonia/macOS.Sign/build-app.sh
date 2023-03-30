#!/bin/bash

echo "[INFO] Building app file (arm64 - Silicon)"
dotnet msbuild ../CheckIP.sln -t:BundleApp -p:RuntimeIdentifier=osx-arm64 -p:UseAppHost=true -p:Configuration=Release

echo "[INFO] Building app file (x64 - Intel)"
dotnet msbuild ../CheckIP.sln -t:BundleApp -p:RuntimeIdentifier=osx-x64 -p:UseAppHost=true -p:Configuration=Release