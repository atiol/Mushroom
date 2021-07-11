~~~~ Migrations Commands ~~~~

dotnet ef -s Core.Api -p Core.Data migrations add [version]

dotnet ef -s Core.Api database update
dotnet ef -s Core.Api database update [version]

dotnet ef -s Core.Api migrations remove

~~~~ Publish Command ~~~~

dotnet publish -c Release -f netcoreapp3.1 -r [win10-x64,linux-x64...] -o /home/usluo/Documents/Workspace/Publish

-r list
https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

~~~~ Install Commands ~~~~

sudo sh dotnet-install.sh --version 3.1.101 --channel LTS --install-dir /home/usluo/.dotnet

dotnet tool install --global dotnet-ef --version 3.1.1

dotnet tool update --global dotnet-ef --version 3.1.1

~~~~ Linux Configuration For Too Many Open Files ~~~~

Learn system maximum open files supported
cat /proc/sys/fs/file-max

Write to /etc/sysctl.conf

fs.file-max = 788406

Write to /etc/security/limits.conf

*    soft nofile 788406
*    hard nofile 788406
root soft nofile 788406
root hard nofile 788406
nginx soft nofile 788406
nginx hard nofile 788406

~~~~ Kestrel Prod Settings ~~~~

[Unit]
Description=Core.Net Kestrel Service

[Service]
WorkingDirectory=/var/www/Core.Net
ExecStart=/usr/bin/dotnet /var/www/Core.Net/Core.Api.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=core-net
User=root
LimitNOFILE=788406
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target

~~~~ NginX Configurations ~~~~

Write to /etc/nginx/conf.d/default.conf (in location /)

proxy_buffering off;
proxy_read_timeout 7200;

Write to /etc/nginx/nginx.conf

worker_rlimit_nofile 788406;

server {
    listen        80;
    server_name   example.com *.example.com;
    client_max_body_size 25MB;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}