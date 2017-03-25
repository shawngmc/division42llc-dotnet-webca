#!/bin/bash

Black='\033[0;30m'
DarkGray='\033[1;30m'
Red='\033[0;31m'
LightRed='\033[1;31m'
Green='\033[0;32m'
LightGreen='\033[1;32m'
Brown='\033[0;33m'
Yellow='\033[1;33m'
Blue='\033[0;34m'
LightBlue='\033[1;34m'
Purple='\033[0;35m'
LightPurple='\033[1;35m'
Cyan='\033[0;36m'
LightCyan='\033[1;36m'
LightGray='\033[0;37m'
White='\033[1;37m'
NC='\033[0m' # No Color

HELP_URL=`git config remote.origin.url`
APP_NAME=`cat ../app-name.txt`
APP_VERSION=`cat ../app-version.txt`

echo -e "${Yellow}Docker Image Builder v1.0 (bash)${NC}"
echo ""
echo -e "  ${LightBlue}App Name............: $APP_NAME ${NC}"
echo -e "  ${LightBlue}App Version.........: $APP_VERSION${NC}"
echo -e "  ${LightBlue}Git Repo/Help.......: $HELP_URL ${NC}"
echo ""

echo ""
echo -e "[${LightCyan}*${NC}] ${LightCyan}Starting .NET Core build of '${APP_NAME}:${APP_VERSION}'...${NC}"

cd ../app
dotnet restore && dotnet build && dotnet publish
if [ $? -eq 0 ]; then
    echo -e "[${LightGreen}*${NC}] ${LightGreen}Docker build of '${APP_NAME}:${APP_VERSION}' and :latest completed successfully.${NC}"
    cd ../sh
else
    echo -e "[${LightRed}-${NC}] ${LightRed}Docker build of '${APP_NAME}:${APP_VERSION}' and :latest, failed.${NC}"
    cd ../sh
    exit 1
fi

echo ""
echo -e "[${LightCyan}*${NC}] ${LightCyan}Starting Docker build of '${APP_NAME}:${APP_VERSION}' and :latest...${NC}"

docker build ../ \
    -t ${APP_NAME}:latest \
    -t ${APP_NAME}:${APP_VERSION}

if [ $? -eq 0 ]; then
    echo -e "[${LightGreen}*${NC}] ${LightGreen}Docker build of '${APP_NAME}:${APP_VERSION}' and :latest completed successfully.${NC}"
else
    echo -e "[${LightRed}-${NC}] ${LightRed}Docker build of '${APP_NAME}:${APP_VERSION}' and :latest, failed.${NC}"
    exit 1
fi
