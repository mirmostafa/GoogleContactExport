# GoogleContactExport
"GoogleContactExport" lets you export your Google contacts into Outlook using the Google People API v1. You just need to authorize yourself using your Google account and then copy all your Google Contacts with one single click!

## Getting Started
These instructions will get you a copy of the project up and running on your own computer.

### Prerequisites
To use my Add-In you only need:
* A computer with at least Windows Vista
* [Microsoft .NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653)
* [Outlook 2016](https://products.office.com/outlook/email-and-calendar-software-microsoft-outlook) (duh)

Please note that while other Versions of Outlook might work aswell they haven't been tested. Please report back if the Add-In works with your version of Outlook!

### Installing
Installing is very easy, just run the installer. It will automatically extract the needed files to a temporary folder and start the VSTO installer which then adds the Add-In and all needed resources to Outlook!

## Built With

* [Microsoft Visual Studio 2017 Community](https://www.visualstudio.com/vs/) - The IDE and Compiler used
* [Inno Setup 5.5.9](http://www.jrsoftware.org/isinfo.php) - The installer software used
* [Semantic Versioning 2.0.0](http://semver.org/#semantic-versioning-200) - The way I version my software
* [Google People API v1](https://developers.google.com/people/) - The API used to read Googles contact data

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details!
