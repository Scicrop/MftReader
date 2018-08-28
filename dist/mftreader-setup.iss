#define MyAppName "MftReader"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "SciCrop"
#define MyAppURL "https://scicrop.com"
#define MyAppExeName "mftreader.exe"

[Setup]
AppId={{2b839e43-b842-42d8-9307-2fb159563790}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=C:\Users\Administrator\Documents\GitHub\MftReader\LICENSE
OutputDir=C:\Users\Administrator\Documents\release
OutputBaseFilename=mftreader_setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Administrator\Documents\GitHub\MftReader\c-sharp-land\MftReader\bin\Release\MftReader.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Administrator\Documents\GitHub\MftReader\c-sharp-land\MftReader\bin\Release\MftReader.exe.config"; DestDir: "{app}"; Flags: ignoreversion


[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
