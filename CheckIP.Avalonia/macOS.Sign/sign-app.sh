#!/bin/bash
APP_NAME_ARM64="../CheckIP.Avalonia/bin/Release/net6.0/osx-arm64/publish/CheckIP.app"
APP_NAME_X64="../CheckIP.Avalonia/bin/Release/net6.0/osx-x64/publish/CheckIP.app"
ENTITLEMENTS="CheckIP.entitlements"
SIGNING_IDENTITY="Developer ID Application: Jonas Guenner (QHSR4A4M65)" # matches Keychain Access certificate name

find "$APP_NAME_ARM64/Contents/MacOS/"|while read fname; do
    if [[ -f $fname ]]; then
        echo "[INFO - ARM64] Signing $fname"
        codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" "$fname"
    fi
done

find "$APP_NAME_X64/Contents/MacOS/"|while read fname; do
    if [[ -f $fname ]]; then
        echo "[INFO - X64] Signing $fname"
        codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" "$fname"
    fi
done

echo "[INFO] Signing app file (arm64 - Silicon)"
codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" "$APP_NAME_ARM64"

echo "[INFO] Signing app file (x64 - Intel)"
codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" "$APP_NAME_X64"