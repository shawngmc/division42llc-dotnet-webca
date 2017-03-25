FROM microsoft/aspnetcore

LABEL Comment="Standard microsoft/aspnetcore:latest image with openssl, and a custom CA application hosted on port 80." \
      Author="Division42 LLC" \
      ImageRepo="https://github.com/Division42LLC/division42llc-dotnet-webca"

COPY docker-entrypoint.sh /usr/local/bin

ADD app/bin/Debug/netcoreapp1.1/publish/ /app
ADD app-name.txt /app
ADD app-version.txt /app

RUN echo "[*] Updating Aptitude..." && \
    apt-get update && \
    echo "[*] Installing packages: openssl nano figlet" && \
    apt-get install openssl nano figlet -y && \
    DISK_SPACE_BEFORE=`df -hm | grep "/$" | xargs | cut -d " " -f3` && \
    echo "[*] Clearing Aptitude cache..." && \
    apt-get clean -y && \
    apt-get purge -y && \
    rm -rf /var/lib/apt/lists/* && \
    echo "[*] Wiping system log files to reduce image size." && \
    cat /dev/null > /var/log/alternatives.log && \
    cat /dev/null > /var/log/apt/history.log && \
    cat /dev/null > /var/log/apt/term.log && \
    cat /dev/null > /var/log/bootstrap.log && \
    cat /dev/null > /var/log/btmp && \
    cat /dev/null > /var/log/dmesg && \
    cat /dev/null > /var/log/dpkg.log && \
    cat /dev/null > /var/log/faillog && \
    cat /dev/null > /var/log/lastlog && \
    cat /dev/null > /var/log/wtmp && \
    DISK_SPACE_AFTER=`df -hm | grep "/$" | xargs | cut -d " " -f3` && \
    echo "[*] Build run commands finished. [$(($DISK_SPACE_BEFORE - DISK_SPACE_AFTER))mb recovered]"

    

WORKDIR /app

ENTRYPOINT ["/bin/bash", "/usr/local/bin/docker-entrypoint.sh"]

CMD ["dotnet", "app.dll"]