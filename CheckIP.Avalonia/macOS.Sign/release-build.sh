#!/bin/sh
APP_PATH_ARM64="../CheckIP.Avalonia/bin/Release/net6.0/osx-arm64/publish"
APP_PATH_X64="../CheckIP.Avalonia/bin/Release/net6.0/osx-x64/publish"

# cleanup folders
echo "[INFO] Cleanup old builds ..."
rm -rf "App/CheckIP-arm64.app/Contents/MacOS/" 
rm -rf "App/CheckIP-arm64.app/Contents/CodeResources" 
rm -rf "App/CheckIP-arm64.app/Contents/_CodeSignature" 
rm -rf "App/CheckIP-arm64.app/Contents/embedded.provisionprofile" 
mkdir -p "App/CheckIP-arm64.app/Contents/Frameworks/"
mkdir -p "App/CheckIP-arm64.app/Contents/MacOS/"
mkdir -p "App/CheckIP-arm64.app/Contents/Resources/"

rm -rf "App/CheckIP-x64.app/Contents/MacOS/" 
rm -rf "App/CheckIP-x64.app/Contents/CodeResources" 
rm -rf "App/CheckIP-x64.app/Contents/_CodeSignature" 
rm -rf "App/CheckIP-x64.app/Contents/embedded.provisionprofile" 
mkdir -p "App/CheckIP-x64.app/Contents/Frameworks/"
mkdir -p "App/CheckIP-x64.app/Contents/MacOS/"
mkdir -p "App/CheckIP-x64.app/Contents/Resources/"

# Build app
echo "[INFO] Building app file (arm64 - Silicon)"
dotnet publish ../CheckIP.sln -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true

echo "[INFO] Building app file (x64 - Intel)"
dotnet publish ../CheckIP.sln -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true

# Move app
echo "[INFO] Moving app files (arm64 - Silicon)"
cp -R -f $APP_PATH_ARM64/* "App/CheckIP-arm64.app/Contents/MacOS/"

rm "App/CheckIP-arm64.app/Contents/MacOS/CheckIP.pdb"
ln -s "App/CheckIP-arm64.app/Contents/MacOS/libAvaloniaNative.dylib" "App/CheckIP-arm64.app/Contents/Frameworks/libAvaloniaNative.dylib"
ln -s "App/CheckIP-arm64.app/Contents/MacOS/libHarfBuzzSharp.dylib" "App/CheckIP-arm64.app/Contents/Frameworks/libHarfBuzzSharp.dylib"
ln -s "App/CheckIP-arm64.app/Contents/MacOS/libSkiaSharp.dylib" "App/CheckIP-arm64.app/Contents/Frameworks/libSkiaSharp.dylib"
mv "App/CheckIP-arm64.app/Contents/MacOS/CheckIP.icns" "App/CheckIP-arm64.app/Contents/Resources/CheckIP.icns"
cp Info.plist "App/CheckIP-arm64.app/Contents/"

echo "[INFO] Moving app files (x64 - Intel)"
cp -R -f $APP_PATH_X64/* "App/CheckIP-x64.app/Contents/MacOS/"

rm "App/CheckIP-x64.app/Contents/MacOS/CheckIP.pdb"
ln -s "App/CheckIP-x64.app/Contents/MacOS/libAvaloniaNative.dylib" "App/CheckIP-x64.app/Contents/Frameworks/libAvaloniaNative.dylib"
ln -s "App/CheckIP-x64.app/Contents/MacOS/libHarfBuzzSharp.dylib" "App/CheckIP-x64.app/Contents/Frameworks/libHarfBuzzSharp.dylib"
ln -s "App/CheckIP-x64.app/Contents/MacOS/libSkiaSharp.dylib" "App/CheckIP-x64.app/Contents/Frameworks/libSkiaSharp.dylib"
mv "App/CheckIP-x64.app/Contents/MacOS/CheckIP.icns" "App/CheckIP-x64.app/Contents/Resources/CheckIP.icns"
cp Info.plist "App/CheckIP-x64.app/Contents/"

# Sign app
APP_ENTITLEMENTS="AppExecutable.entitlements"
HELPER_ENTITLEMENTS="Helper.entitlements"
APP_SIGNING_IDENTITY="Apple Distribution: Jonas Guenner (QHSR4A4M65)"
INSTALLER_SIGNING_IDENTITY="3rd Party Mac Developer Installer: Jonas Guenner (QHSR4A4M65)"
APP_NAME_ARM64="App/CheckIP-arm64.app"
APP_NAME_X64="App/CheckIP-x64.app"

echo "[INFO] Switch provisionprofile to AppStore"
\cp -R -f CheckIP.provisionprofile "App/CheckIP-arm64.app/Contents/embedded.provisionprofile"
\cp -R -f CheckIP.provisionprofile "App/CheckIP-x64.app/Contents/embedded.provisionprofile"

#echo "[INFO] Fix libuv.dylib architectures"
#lipo -remove i386 "App/CheckIP-arm64.app/Contents/Frameworks/libuv.dylib" "App/CheckIP-arm64.app/Contents/Frameworks/libuv.dylib"
#lipo -remove i386 "App/CheckIP-x64.app/Contents/Frameworks/libuv.dylib" "App/CheckIP-x64.app/Contents/Frameworks/libuv.dylib"

find "$APP_NAME_ARM64/Contents/Frameworks/"|while read fname; do
    if [[ -f $fname ]]; then
        echo "[INFO - ARM64] Signing $fname"
        codesign --force --sign "$APP_SIGNING_IDENTITY" "$fname"
    fi
done

find "$APP_NAME_X64/Contents/Frameworks/"|while read fname; do
    if [[ -f $fname ]]; then
        echo "[INFO - X64] Signing $fname"
        codesign --force --sign "$APP_SIGNING_IDENTITY" "$fname"
    fi
done

echo "[INFO] Signing app executables"
codesign --force --entitlements "$HELPER_ENTITLEMENTS" --sign "$APP_SIGNING_IDENTITY" "App/CheckIP-arm64.app/Contents/MacOS/CheckIP"
codesign --force --entitlements "$HELPER_ENTITLEMENTS" --sign "$APP_SIGNING_IDENTITY" "App/CheckIP-x64.app/Contents/MacOS/CheckIP"

echo "[INFO] Signing app bundle"
codesign --force --entitlements "$APP_ENTITLEMENTS" --sign "$APP_SIGNING_IDENTITY" "$APP_NAME_ARM64"
codesign --force --entitlements "$APP_ENTITLEMENTS" --sign "$APP_SIGNING_IDENTITY" "$APP_NAME_X64"

echo "[INFO] Creating CheckIP-arm64.pkg"
productbuild --component App/CheckIP-arm64.app /Applications --sign "$INSTALLER_SIGNING_IDENTITY" CheckIP-arm64.pkg

echo "[INFO] Creating CheckIP-x64.pkg"
productbuild --component App/CheckIP-x64.app /Applications --sign "$INSTALLER_SIGNING_IDENTITY" CheckIP-x64.pkg