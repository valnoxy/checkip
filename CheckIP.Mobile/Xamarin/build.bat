@echo off

:: Read content of file and write it to string 
set file=keystore.pw
set content=
for /f "delims=*" %%a in ('type %file%') do set content=%%a

msbuild -restore CheckIPApp\CheckIPApp.Android\CheckIPApp.Android.csproj ^
  -t:SignAndroidPackage ^
  -p:AndroidPackageFormat=aab ^
  -p:Configuration=Release ^
  -p:AndroidKeyStore=True ^
  -p:AndroidSigningKeyStore=C:\Users\jonas\Documents\GitHub\checkip\CheckIP.Mobile\Xamarin\key.keystore ^
  -p:AndroidSigningStorePass=%content% ^
  -p:AndroidSigningKeyAlias=CheckIP ^
  -p:AndroidSigningKeyPass=%content%

::dotnet publish -f:net6.0-android -c:Release /p:AndroidSigningKeyPass=%content% /p:AndroidSigningStorePass=%content%
pause

