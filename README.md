# Hotspot Server VelosoNet
This is a Hotspot Server, utilized to manage the sellers and tickets.

## Instalation Guide
This is a guide to install and configure a Ubuntu 18.04 LST server from scracth.
Feel free to change anything along the way to best suit your environment.

### 1. Publish Configuration
The project should be published with the following configurations:
- Target: Folder
- Configuration: Release
- Target Framework: netcoreapp3.1
- Deployment Mode: Release
- Target Runtime: linux-x-64

### 2. SQL Server
Please refer to the official [SQL Server on Ubuntu](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-ubuntu?view=sql-server-linux-ver15&preserve-view=true) installation guide from Microsoft.
Make sure to install **SQL Server Express**.

### 3. .NET and .NET Core
Please refer to the official [.NET Microsoft Website](https://dotnet.microsoft.com/download) to install both **.NET** and **.NET Core**. 
Make sure to install version 3.1

### 4. Apache
Install apache using the following commands:
```
sudo apt update
sudo apt install apache2
```
Verify if it is running:
```
systemctl status apache2
```
Give permissions and activate some needed resources with the following commands:
```
sudo a2enmod rewrite
sudo a2enmod proxy
sudo a2enmod proxy_http
sudo a2enmod headers
sudo a2enmod ssl
```
Restart `apache2` service:
```
sudo service apache2 restart
```

### 6. Ubuntu Permissions
Do the steps below to create the project directory and give permissions.
- Create a folder called `hotspot` at `/var/www/`.
- Put *ALL* the files published from Visual Studio at `/var/www/hotspot/`
- Give permissions to the necessary users on the folder `/var/www/`, recursivelly.

### 6. Configure ASP.NET Core as Service
Create a VirtualHost file called `Hotspot.conf` at `/etc/apache2/sites-available/`:
```
sudo nano /etc/apache2/sites-available/Hotspot.conf
```
The file must contain the following content, which will configure the site to respond on port 80:
```
<VirtualHost *:80>
	ServerName hotspot.com.br
	ProxyPreserveHost On
	ProxyPass / https://localhost:5001/
	ProxyPassReverse / https://localhost:5001/
	RewriteEngine on
	RewriteCond %{HTTP:UPGRADE} ^WebSocket$ [NC]
	RewriteCond %{HTTP:CONNECTION} Upgrade$ [NC]
	RewriteRule /(.*) ws://127.0.0.1:5000/$1 [P]
	ErrorLog ${APACHE_LOG_DIR}/error-hotspot.com.log
    CustomLog ${APACHE_LOG_DIR}/access-hotspot.com.log combined
</VirtualHost>
```
Disable the default Site:
```
sudo a2dissite 000-default.conf
```
Activate the Hotspot Site and check the syntax:
```
sudo a2ensite Hotspot.conf
sudo apachectl configtest
```
Restart `apache2` service:
```
sudo service apache2 restart
```
Create a Systemd Service file called `/etc/systemd/system/` at `Hotspot.service`:
```
sudo nano /etc/systemd/system/Hotspot.service
```
The file must contain the following content:
```
[Unit]
Description=Hotspot Service

[Service]
WorkingDirectory=/var/www/hotspot
ExecStart=/usr/bin/dotnet /var/www/hotspot/Hotspot.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-example
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
[Install]
WantedBy=multi-user.target
```
Enable and Start the service created:
```
sudo systemctl enable Hotspot.service
sudo systemctl start Hotspot.service
```

## Conclusion
If all was done correctly, the server should be now running with the database already created.
Of course, Linux can be finicky sometimes, so you might encounter some issues along the way...
Good Luck!
# Hotspot
