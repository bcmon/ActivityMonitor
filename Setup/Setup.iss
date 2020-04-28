; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Activity Monitor"
#define MyAppVersion "0.9"
#define MyAppPublisher "Riccardo Bicelli"
#define MyAppURL "https://github.com/rbicelli/ActMon"
#define MyAppExeName "ActMon.exe"
#define MyReleasePath "..\ActMon\bin\Release"
#define MyAppTitle "Activity Monitor"
#define MyOutputfile "ActMon"


[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{37C2DF91-1B5A-4AB8-B14C-6BFA847D4483}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={commonpf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=..\LICENSE
OutputBaseFilename={#MyOutputFile}-{#MyAppVersion}-Setup
Compression=lzma
SolidCompression=yes
UninstallDisplayName={#MyAppTitle}
UninstallDisplayIcon={app}\{#MyAppExename}
MinVersion=0,6.1
AppCopyright={#MyAppPublisher}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\ActMon\bin\Release\ActMon.exe"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "{#MyReleasePath}\ActivityMonitor.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyReleasePath}\ActivityMonitor.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyReleasePath}\it-IT\ActMon.resources.dll"; DestDir: "{app}\it-IT"; Flags: ignoreversion
Source: "..\ActMon\ADMX\SequenceActMon.admx"; DestDir: "{app}\Tools\ADMX"; Flags: ignoreversion
Source: "..\ActMon\ADMX\SequenceSoftware.admx"; DestDir: "{app}\Tools\ADMX"; Flags: ignoreversion
Source: "..\ActMon\ADMX\en-US\Sequence.adml"; DestDir: "{app}\Tools\ADMX\en-US"; Flags: ignoreversion
Source: "..\ActMon\ADMX\en-US\SequenceActMon.adml"; DestDir: "{app}\Tools\ADMX\en-US"; Flags: ignoreversion
Source: "..\Database Schema\Schema-Current.sql"; DestDir: "{app}\Tools\Database Schemas"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"; IconIndex: 0
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"; IconIndex: 0; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

