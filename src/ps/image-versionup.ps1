$HELP_URL=git config remote.origin.url
$APP_NAME=type ..\app-name.txt
$APP_VERSION_FULL=type ..\app-version.txt

# Increment
$APP_VERSION_FULL=type ..\app-version.txt
$APP_VERSION_PARTS=$APP_VERSION_FULL.Split(".")
$MAJ=$APP_VERSION_PARTS[0]
$MIN=$APP_VERSION_PARTS[1]
$REV=[Int32]$APP_VERSION_PARTS[2] + 1
$APP_VERSION="${MAJ}.${MIN}.${REV}"
echo $APP_VERSION > ..\app-version.txt


Write-Host "Docker Image Versioner v1.0 (Powershell)" -Foreground Yellow
Write-Host ""
Write-Host "  App Name............: $APP_NAME " -Foreground Cyan
Write-Host "  Git Repo/Help.......: $HELP_URL " -Foreground Cyan
Write-Host ""
Write-Host "  OLD App Version.....: $APP_VERSION_FULL" -Foreground Cyan
Write-Host "  NEW App Version.....: $APP_VERSION" -Foreground Magenta
Write-Host ""

Write-Host "[*] Adding Git tag for this version." -Foreground Cyan
git tag -a $APP_VERSION -m "Version ${APP_VERSION}."
Write-Host "[*] Current tags for this Git branch:" -Foreground Cyan
git tag