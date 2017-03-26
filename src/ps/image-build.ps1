$HELP_URL=git config remote.origin.url
$APP_NAME=type ../app-name.txt
$APP_VERSION=type ../app-version.txt

Write-Host "Docker Image Builder v1.0 (bash)" -Foreground Yellow
Write-Host ""
Write-Host "  App Name............: $APP_NAME " -Foreground Cyan
Write-Host "  App Version.........: $APP_VERSION" -Foreground Cyan
Write-Host "  Git Repo/Help.......: $HELP_URL " -Foreground Cyan
Write-Host ""

Write-Host ""
Write-Host "[*] Starting .NET Core build of '${APP_NAME}:${APP_VERSION}'..." -Foreground Cyan

cd ..\

Write-Host ""
Write-Host "[*] Dotnet RESTORE starting.." -Foreground Cyan
dotnet restore
If ( $? )
{
    Write-Host "[+] Dotnet RESTORE completed successfully." -Foreground Green
	Write-Host ""
	Write-Host "[*] Dotnet BUILD starting.." -Foreground Cyan
	dotnet build
	If ( $? )
	{
		Write-Host "[+] Dotnet BUILD completed successfully." -Foreground Green
		Write-Host ""
		Write-Host "[*] Dotnet PUBLISH starting.." -Foreground Cyan
		dotnet publish
		If ( $? )
		{
			Write-Host "[+] Dotnet PUBLISH completed successfully." -Foreground Green
		}
		Else
		{
			Write-Host "[-] Dotnet PUBLISH failed." -Foreground Red
			cd ..\ps
			exit 1
		}
	}
	Else
	{
		Write-Host "[-] Dotnet BUILD failed." -Foreground Red
		cd ..\ps
		exit 1
	}
}
Else
{
    Write-Host "[-] Dotnet RESTORE failed." -Foreground Red
    cd ..\ps
    exit 1
}

Write-Host ""
Write-Host "[*] Starting Docker build of '${APP_NAME}:${APP_VERSION}' and :latest..." -Foreground Cyan
Write-Host "    $ docker build . " -Foreground Cyan
Write-Host "        -t ${APP_NAME}:latest " -Foreground Cyan
Write-Host "        -t ${APP_NAME}:${APP_VERSION}" -Foreground Cyan


docker build . `
    -t ${APP_NAME}:latest `
    -t ${APP_NAME}:${APP_VERSION}

If ( $? )
{
    Write-Host "[*] Docker build of '${APP_NAME}:${APP_VERSION}' and :latest completed successfully." -Foreground Green
}
Else
{
    Write-Host "[-] Docker build of '${APP_NAME}:${APP_VERSION}' and :latest, failed." -Foreground Red
    exit 1
}
