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
APP_VERSION_FULL=`cat ../app-version.txt`

# Increment
MAJ=`cat ../app-version.txt | cut -d "." -f1`
MIN=`cat ../app-version.txt | cut -d "." -f2`
REV=`cat ../app-version.txt | cut -d "." -f3`
APP_VERSION="${MAJ}.${MIN}.$(($REV+1))"
echo $APP_VERSION > ../app-version.txt


echo -e "${Yellow}Docker Image Versioner v1.0 (bash)${NC}"
echo ""
echo -e "  ${LightBlue}App Name............: $APP_NAME ${NC}"
echo -e "  ${LightBlue}Git Repo/Help.......: $HELP_URL ${NC}"
echo ""
echo -e "  ${LightCyan}OLD App Version.....: $APP_VERSION_FULL${NC}"
echo -e "  ${LightCyan}NEW App Version.....: $APP_VERSION${NC}"
echo ""

echo -e "[${LightCyan}*${NC}] ${LightCyan}Adding Git tag for this version.${NC}"
git tag -a $APP_VERSION -m "Version ${APP_VERSION}."
echo -e "[${LightCyan}*${NC}] ${LightCyan}Current tags for this Git branch:${NC}"
git tag