#!/bin/bash
APP_NAME_ARM64="../CheckIP.Avalonia/bin/Release/net6.0/osx-arm64/publish/CheckIP.app"
APP_NAME_X64="../CheckIP.Avalonia/bin/Release/net6.0/osx-x64/publish/CheckIP.app"
OUTPUT="../Output"

echo "[INFO] Copying app file (arm64 - Silicon)"
cp -R $APP_NAME_ARM64 $OUTPUT/CheckIP-arm64.app

echo "[INFO] Copying app file (x64 - Intel)"
cp -R $APP_NAME_X64 $OUTPUT/CheckIP-x64.app