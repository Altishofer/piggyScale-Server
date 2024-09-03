![db-sqlite3](https://img.shields.io/badge/db--sqlite3-0.0.1-yellow)
![dotnet](https://img.shields.io/badge/.NET-7.0-blue)
![csharp](https://img.shields.io/badge/C%23-10.0-purple)
![swagger](https://img.shields.io/badge/Swagger-6.2.3-green)

# Piggy Scale Server: 
🎉 Real-Time Scale and Weight Analytics 🎉

This repository contains all three components of the Piggy Scale system. The Scale component reads data from the load cells and publishes the measurements to the MQTT server. The MQTT server stores these real-time measurements and makes them available to all subscribed web clients. The web client subscribes to these real-time signals and loads previous weights from the database, allowing users to view, visualize, and analyze both current and historical measurements. The server handles requests from the client to read, write, and manipulate stored records.

## Table of Contents

- [Features](#features)
- [Built With](#built-with)
- [Main Components](#main-components)
- [Hardware](#hardware)
- [Deployment](#deployment)

## Features
- Real-time visualization of scale data
- Automatic weight estimation using real-time measurements
- Manual storage, retrieval, and manipulation of weight data
- View and analyze the history and trends of different pig batches
- Export measurement data to Excel

## Built With

- **[SQLite3](https://sqlite.org/index.html):** A library for manipulating lightweight open-source database files.
- **[ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet):** A cross-platform framework for building modern cloud-based web applications.
- **[Swagger](https://swagger.io/):** A toolset for designing, building, documenting, and consuming RESTful web services.
- **[Mosquitto](https://mosquitto.org/):** An open-source message broker that implements the MQTT protocol.
- **[C#](https://docs.microsoft.com/en-us/dotnet/csharp/):** The primary programming language used in the .NET ecosystem.


## Main Components

- **[Weight Controller](https://github.com/Altishofer/piggyScale-Server/tree/main/Project/Controllers/WeightController):** Manages a RESTful API for interacting with the weights database, which stores permanent records.
- **[User Controller](https://github.com/Altishofer/piggyScale-Server/tree/main/Project/Controllers/UserController):** Manages a RESTful API for interacting with the user database, for authentication.

## Hardware

### Prerequisites
- 4 **[Weight Cells](https://de.aliexpress.com/item/32993892413.html?srcSns=sns_WhatsApp&spreadType=socialShare&bizType=ProductDetail&social_params=60729542385&aff_fcid=967cfb0bb3c44d5994dca7a8b6821253-1721983637820-00146-_EIhxwWR&tt=MG&aff_fsk=_EIhxwWR&aff_platform=default&sk=_EIhxwWR&aff_trace_key=967cfb0bb3c44d5994dca7a8b6821253-1721983637820-00146-_EIhxwWR&shareId=60729542385&businessType=ProductDetail&platform=AE&terminal_id=15f1fe9867124f1a9b3cca645aa764d7&afSmartRedirect=y)**
- 1x **[IPS Touch Screen](https://de.aliexpress.com/item/1005006420023450.html?spm=a2g0o.order_list.order_list_main.11.3b3f5c5fLSSfJb&gatewayAdapt=glo2deu)**
- 4x **[Weight Cell Sensor (HX711)](https://de.aliexpress.com/item/1005006293368575.html?spm=a2g0o.order_list.order_list_main.23.3b3f5c5fLSSfJb&gatewayAdapt=glo2deu)**
- 2x **[Raspberry Pi 4B](https://www.raspberrypi.com/products/raspberry-pi-4-model-b/)**

### Instructions
- Solder each weight cell to a HX711
- Connect each HX711 to an individual GPIO

## Deployment

### Deployment - REST-Server & Scale

#### Software Prerequisites
- [ASP .NET](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code)

#### Setup

1. Clone this repository to your local machine:
  ```shell
  git clone https://github.com/Altishofer/piggyScale-Server.git
  ```
2. Enter newly cloned directory
  ```shell
  cd piggyScale-Server/Project
  ```
3. Execute Predefined Launch Profile
 ```console
 dotnet run --launch-profile Production
 ```
4. Open UI in your favorite browser
[http://localhost:59331/api/Weight/status](http://localhost:59331/api/Weight/status)

5. Open Swagger UI in your favorite browser
[http://localhost:59331/api/Weight/status](http://localhost:59331/swagger/index.html)
 

### Deployment - Client

### MQTT - Server Setup (Linux)
1. Update and ugrade all packages:
  ```shell
  sudo apt update && sudo apt upgrade
  ```
2. Install mosquitto broker:
  ```shell
  sudo apt install -y mosquitto mosquitto-clients
  ```
3. Open mosquitto configuration:
  ```shell
  sudo nano /etc/mosquitto/mosquitto.conf
  ```
4. Replace mosquitto configuration:
  ```shell
  # replace, save and close file
  pid_file /run/mosquitto/mosquitto.pid
  
  persistence true
  persistence_location /var/lib/mosquitto/
  
  log_dest file /var/log/mosquitto/mosquitto.log
  include_dir /etc/mosquitto/conf.d
  
  listener 1883 0.0.0.0
  allow_anonymous true
  protocol mqtt
  
  listener 9001 0.0.0.0
  allow_anonymous true
  protocol websockets
  
  connection_messages true
  log_timestamp true
  ```
5. Run MQTT-Broker in the background:
  ```shell
  mosquitto -v -d
  ```
6. Open NEW shell and test subscription:
  ```shell
  mosquitto _sub -h localhost -t /test/topic
  ```
7. Open NEW shell and test publishing:
  ```shell
  # Switch to subscription shell and check if output is visible
  mosquitto _pub -h localhost -t /test/topic -m "Hello World!"
  ```

### Autostart for Raspbberry Pi
Add all script to autostart (optional)

1. Open Crontab with root rights:
 ```console
 sudo crontab -e
 ```
2. Add automatic reset on reboot
 ```console
@reboot sleep 60 && bash /home/pi/Desktop/startAll.sh
 ```
3. Create new bash script on Desktop
 ```console
cd /home/pi/Desktop && nano startAll.sh
 ```
3. Insert Deployment code into bash file
```console
#!/bin/bash

# Activate logger
#exec > /home/pi/Desktop/startAll.log 2>&1

source /home/pi/.nvm/nvm.sh

# Start Angular project
cd /home/pi/Desktop/piggyScale
/usr/bin/git pull
/usr/local/bin/ng serve --configuration production &

# Start .NET project
cd /home/pi/Desktop/piggyScale-Server/Project
/usr/bin/git pull
/home/pi/.dotnet/dotnet run --launch-profile Production &

# Start Python script
/home/pi/Desktop/piggyScale/.venv/bin/python /home/pi/Desktop/piggyScale/scale/measure.py &
```
5. Make file executable
```console
sudo chmod +x startAll.sh
```
6. Reboot
```console
sudo reboot
```
7. If automatic deployment fails, uncomment logger in bash script and open log file after reboot.

