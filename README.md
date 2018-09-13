# MftReader
MftReader is a Command-Line interface (CLI) program which reads the Master File Table (MFT) from NTFS volume.
(C# Implementation with PInvoke)

## Downloads

- https://github.com/Scicrop/MftReader/releases/latest

## How to install

- Open the directory where the file mftreader_setup.exe was downloaded and double-click it
  - Windows 10: You may need permission to run unsigned executable files or may need to enable Developer Mode

## How to run

![alt text](https://github.com/Scicrop/MftReader/blob/master/dist/mftreader-capture.png?raw=true "MftReader ScreenShot")

- Open the directory where the MftReader was installed, example: `C:\Program Files (x86)\MftReader\`
- Fill the configuration file `MftReader.exe.config` with the following parameters:
  1. search_volume: Example: `C`. Must be JUST a NTFS volume/drive letter WITHOUT ':', like C, D, F, G, etc. The current user must have administration rights over this volume.
  2. report_folder: Folder where the .json result will be stored, example `c:/windows/temp`. Must be a valid folder, and the current user must have write access in it. The valid slash must be / and NOT \\.
  3. search_extensions: File extensions to be scanned, example `.txt .pdf .doc`. Is the representation of a file extension, like .txt, .pdf, .doc, etc, WITH dot (.) WITHOUT asterisk (*).
  4. calc_md5: Boolean value to decide if the  sofware will make a MD5 hash of each file (True|False)
- Open a Command Prompt as Administrator and type `MftReader.exe` then press `enter`

## Configuration file example

```xml
        <MftReader.Properties.Settings>
            <setting name="search_volume" serializeAs="String">
                <value>C</value>
            </setting>
            <setting name="report_folder" serializeAs="String">
                <value>c:/windows/temp</value>
            </setting>
            <setting name="search_extensions" serializeAs="String">
                <value>.txt .pdf .doc</value>
            </setting>
	    <setting name="calc_md5" serializeAs="String">
                <value>True</value>
            </setting>
        </MftReader.Properties.Settings>
```

## Result example

```json
{
	"initTime": 1535868785,
	"search_volume": "C",
	"report_folder": "c:/windows/temp",
	"search_extensions": ".txt .pdf .doc",
	"totalDriveSize": 135770664960,
	"objectLst": [{
			"fileName": "C%3A%5CWindows%5CWinSxS%5Cwow64_microsoft-windows-mccs-syncutil_31bf3856ad364e35_10.0.14393.0_none_fe5045edee3ebb3e%5CLiveDomainList.txt",
			"fileSize": 12,
			"fileCreationDate": "16/07/2016 10:19:02",
			"fileUpdateDate": "23/03/2018 11:53:59",
			"fileAuthor": "NT%20AUTHORITY%5CSYSTEM",
			"fqdn": "SCICROP-W16.",
			"md5": "5c4bf9eade67d24a7f96ed8f6a640b4d"
		},
		{
			"fileName": "C%3A%5CUsers%5CAdministrator%5CAppData%5CLocal%5CMicrosoft%5CVisualStudio%5CSettingsLogs%5Cheader.txt",
			"fileSize": 38,
			"fileCreationDate": "12/02/2018 14:54:12",
			"fileUpdateDate": "28/08/2018 15:57:47",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"md5": "c96f85e67b3865cedbeca1587d07a13b"
		},
		{
			"fileName": "C%3A%5CWindows%5CTemp%5CSQLServer2016-KB4019088-x64_decompression_log.txt",
			"fileSize": 1461,
			"fileCreationDate": "21/09/2017 14:46:57",
			"fileUpdateDate": "21/09/2017 14:51:17",
			"fileAuthor": "NT%20AUTHORITY%5CSYSTEM",
			"fqdn": "SCICROP-W16.",
			"md5": "c1eab5fb04bbff83e122cf4b4b9ca8b0"
		},
		{
			"fileName": "C%3A%5CUsers%5CAdministrator%5CAppData%5CLocal%5CPackages%5CMicrosoft.Windows.Cortana_cw5n1h2txyewy%5CLocalState%5CDeviceSearchCache%5CSettingsCache.txt",
			"fileSize": 279380,
			"fileCreationDate": "21/09/2017 14:53:23",
			"fileUpdateDate": "21/09/2017 14:53:23",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"md5": "04ac236a828f25ac5e3582f24ae63e69"
		},
		{
			"fileName": "C%3A%5CUsers%5CAdministrator%5CAppData%5CLocal%5CPackages%5CMicrosoft.Windows.Cortana_cw5n1h2txyewy%5CLocalState%5CConstraintIndex%5CSettings_%7B6b79e225-d4c1-437f-97cf-5b752a7a57fc%7D%5C0.0.filtertrie.intermediate.txt",
			"fileSize": 86020,
			"fileCreationDate": "21/09/2017 14:53:23",
			"fileUpdateDate": "21/09/2017 14:53:23",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"md5": "4f5f2344a27a5b8e016157a868f38c7c"
		},
		{
			"fileName": "C%3A%5CUsers%5CAdministrator%5CAppData%5CLocal%5CPackages%5CMicrosoft.Windows.Cortana_cw5n1h2txyewy%5CLocalState%5CConstraintIndex%5CSettings_%7B6b79e225-d4c1-437f-97cf-5b752a7a57fc%7D%5C0.1.filtertrie.intermediate.txt",
			"fileSize": 5,
			"fileCreationDate": "21/09/2017 14:53:23",
			"fileUpdateDate": "21/09/2017 14:53:23",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"md5": "34bd1dfb9f72cf4f86e6df6da0a9e49a"
		},
		{
			"fileName": "C%3A%5CUsers%5CAdministrator%5CAppData%5CLocal%5CPackages%5CMicrosoft.Windows.Cortana_cw5n1h2txyewy%5CLocalState%5CConstraintIndex%5CSettings_%7B6b79e225-d4c1-437f-97cf-5b752a7a57fc%7D%5C0.2.filtertrie.intermediate.txt",
			"fileSize": 5,
			"fileCreationDate": "21/09/2017 14:53:23",
			"fileUpdateDate": "21/09/2017 14:53:23",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"md5": "c204e9faaf8565ad333828beff2d786e"
		},
		{
			"fileName": "C%3A%5CWindows%5CWinSxS%5Camd64_windows-defender-service_31bf3856ad364e35_10.0.14393.0_none_f31a8e496d7f3859%5CThirdPartyNotices.txt",
			"fileSize": 1091,
			"fileCreationDate": "16/07/2016 10:19:22",
			"fileUpdateDate": "16/07/2016 10:19:22",
			"fileAuthor": "NT%20SERVICE%5CTrustedInstaller",
			"fqdn": "SCICROP-W16.",
			"md5": "314ce81bed1547b8fa40f405f4c8b9fc"
		}
	],
	"endTime": 1535868826
}
```

## References

- https://docs.microsoft.com/en-us/windows/desktop/fileio/master-file-table
