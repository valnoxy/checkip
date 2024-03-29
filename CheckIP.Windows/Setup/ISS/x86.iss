; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName          "CheckIP"
#define MyAppVersion       GetFileVersion('..\..\CheckIP\bin\Release\publish\win-x86\CheckIP.exe')
#define MyAppPlatform      "32bit"
#define MyAppPublisher     "Exploitox"
#define MyAppURL           "https://github.com/valnoxy/checkip"
#define MyAppExeName       "CheckIP.exe"
#define MyAppStartingYear  "2018"
#define MyAppEndingYear    GetDateTimeString('yyyy','','')
              
[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{47128A10-4D16-4AE3-AE08-F311BDB04571}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion} ({#MyAppPlatform})

VersionInfoDescription={#MyAppName} Installer
VersionInfoVersion={#MyAppVersion}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}
AppCopyright=Copyright © {#MyAppStartingYear} - {#MyAppEndingYear} {#MyAppPublisher}. All rights reserved.

AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

UninstallDisplayIcon={app}\CheckIP.exe
UninstallDisplayName={#MyAppName}
AppPublisher={#MyAppPublisher}

WizardStyle=modern
ShowLanguageDialog=yes
UsePreviousLanguage=no

DefaultDirName={commonpf32}\{#MyAppPublisher}\{#MyAppName}
UsePreviousAppDir=yes
DisableProgramGroupPage=yes
LicenseFile=..\..\..\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=..\Output
OutputBaseFilename=CheckIP_{#MyAppVersion}_x86
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german";  MessagesFile: "compiler:Languages\German.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\..\CheckIP\bin\Release\publish\win-x86\*"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

