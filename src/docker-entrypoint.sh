#!/bin/sh

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

source /etc/profile 

HELP_URL="https://github.com/Division42LLC/division42-dotnet-webca"

echo $IMAGE_NAME | figlet -w 132
echo ""
echo -e "        ${Yellow}Image Name............: ${IMAGE_NAME}${NC}"
echo -e "        ${Yellow}Image Version.........: ${IMAGE_VERSION}${NC}"
echo -e "        ${Yellow}Image Built...........: ${IMAGE_BUILT}${NC}"
echo -e "        ${Yellow}Help URL..............: ${HELP_URL}${NC}"
echo ""
echo ""
echo "------ Environment ------ \/\/\/\/\/\/\/ "
env | sort
echo "------ Environment ------ /\/\/\/\/\/\/\ "
echo ""
exec "$@"