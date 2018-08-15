# MftReader
MftReader is a Command-Line interface (CLI) program which reads the Master File Table (MFT) from NTFS volume.
(C# Implementation with PInvoke)

## Downloads

- https://github.com/Scicrop/MftReader/releases/latest

## How to run

![alt text](https://github.com/Scicrop/MftReader/blob/master/dist/mftreader-capture.png?raw=true "MftReader ScreenShot")

- Open a Command Prompt as Administrator:
- `MftReader.exe C c:/temp .shp`
  1. Executable `MftReader.exe`
  2. Volume to be scanned `C`
  3. Folder where the .json result will be stored `c:/temp` *Do not use \\*
  4. File extension to be scanned `.shp`

## Result example

```json
{
	"objectLst": [{
			"fileName": "C%3A%5CWindows%5CWinSxS%5Cx86_netfx4-aspnet_webadmin_code_b03f5f7f11d50a3a_4.0.14305.0_none_b5951a9f218f715e%5CApplicationConfigurationPage.cs",
			"fileSize": 700,
			"fileCreationDate": "16/07/2016 10:20:07",
			"fileUpdateDate": "16/07/2016 10:20:07",
			"fileAuthor": "NT%20SERVICE%5CTrustedInstaller"
		},
		{
			"fileName": "C%3A%5CWindows%5CWinSxS%5Camd64_netfx4-aspnet_webadmin_code_b03f5f7f11d50a3a_4.0.14305.0_none_6de7e3c80d134858%5CApplicationConfigurationPage.cs",
			"fileSize": 700,
			"fileCreationDate": "16/07/2016 10:19:51",
			"fileUpdateDate": "16/07/2016 10:19:51",
			"fileAuthor": "NT%20SERVICE%5CTrustedInstaller"
		},
		{
			"fileName": "C%3A%5CWindows%5CMicrosoft.NET%5CFramework64%5Cv4.0.30319%5CASP.NETWebAdminFiles%5CApp_Code%5CApplicationConfigurationPage.cs",
			"fileSize": 700,
			"fileCreationDate": "16/07/2016 10:23:26",
			"fileUpdateDate": "16/07/2016 10:21:30",
			"fileAuthor": "NT%20AUTHORITY%5CSYSTEM"
		}
	]
}
```

## References

- https://docs.microsoft.com/en-us/windows/desktop/fileio/master-file-table
