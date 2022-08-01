echo Building PLAYLIST ...

cd ../Novit.Academia.ChallengePlaylist/Novit.Academia.ChallengePlaylist
cmd.exe /c dotnet restore
cmd.exe /c dotnet publish --configuration Release -o ../PLAYLIST_Build

cd ../
cd ./SGC_API_Build

echo Deploying API SGC...

"C:/Program Files/IIS/Microsoft Web Deploy V3/msdeploy.exe" ^
	-source:dirPath='%~dp0PLAYLIST_Build' ^
	-dest:dirPath="C:\inetpub\wwwroot\TST_PLAYLIST",computerName="http://54.83.24.28/MSDEPLOYAGENTSERVICE",userName="WebDeployIIS",password="Tivon123456",authtype=NTLM,includeAcls=False ^
	-verb:sync ^
	-skip:skipaction='Delete',objectname='filePath',absolutepath='\\appsettings.json' ^
	-skip:skipaction='Delete',objectname='dirPath',absolutepath='\\App_Data' ^
	-disableLink:AppPoolExtension ^
	-disableLink:ContentExtension ^
	-disableLink:CertificateExtension

set /p DUMMY=Hit ENTER to continue...