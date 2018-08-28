# MftReader
MftReader is a Command-Line interface (CLI) program which reads the Master File Table (MFT) from NTFS volume.
(C# Implementation with PInvoke)

## Downloads

- https://github.com/Scicrop/MftReader/releases/latest

## How to run

![alt text](https://github.com/Scicrop/MftReader/blob/master/dist/mftreader-capture.png?raw=true "MftReader ScreenShot")

- Open the directory where the MftReader was installed, example: `C:\Program Files (x86)\MftReader\`
- Fill the configuration file `MftReader.exe.config` with the following parameters:
  1. search_volume: Example: `C`. Must be JUST a NTFS volume/drive letter WITHOUT ':', like C, D, F, G, etc. The current user must have administration rights over this volume.
  2. report_folder: Folder where the .json result will be stored, example `c:/windows/temp`. Must be a valid folder, and the current user must have write access in it. The valid slash must be / and NOT \\.
  3. search_extensions: File extensions to be scanned, example `.txt .pdf .doc`. Is the representation of a file extension, like .txt, .pdf, .doc, etc, WITH dot (.) WITHOUT asterisk (*).
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
        </MftReader.Properties.Settings>
```

## Result example

```json
{
	"initTime": 1535481071,
	"search_volume": "C",
	"report_folder": "c:/windows/temp",
	"search_extensions": ".txt .pdf .doc",
	"objectLst": [{
			"fileName": "C%3A%5CWindows%5CWinSxS%5Cwow64_microsoft-windows-mccs-syncutil_31bf3856ad364e35_10.0.14393.0_none_fe5045edee3ebb3e%5CLiveDomainList.txt",
			"fileSize": 12,
			"fileCreationDate": "16/07/2016 10:19:02",
			"fileUpdateDate": "23/03/2018 11:53:59",
			"fileAuthor": "NT%20AUTHORITY%5CSYSTEM",
			"fqdn": "SCICROP-W16.",
			"machineName": "SCICROP-W16"
		},
		{
			"fileName": "C%3A%5CUsers%5CAdministrator%5CAppData%5CLocal%5CMicrosoft%5CVisualStudio%5CSettingsLogs%5Cheader.txt",
			"fileSize": 38,
			"fileCreationDate": "12/02/2018 14:54:12",
			"fileUpdateDate": "26/07/2018 19:16:58",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"machineName": "SCICROP-W16"
		},
		{
			"fileName": "C%3A%5CProgram%20Files%5CS3%20Browser%5Clicense.txt",
			"fileSize": 5731,
			"fileCreationDate": "22/08/2018 09:15:55",
			"fileUpdateDate": "22/05/2017 13:34:24",
			"fileAuthor": "BUILTIN%5CAdministrators",
			"fqdn": "SCICROP-W16.",
			"machineName": "SCICROP-W16"
		}
	],
	"endTime": 1535481074
}
```

## References

- https://docs.microsoft.com/en-us/windows/desktop/fileio/master-file-table
