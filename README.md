
# Jobsity Chat App

## Description

A Chat Application written in .Net 6

## Getting Started

### Requirements

* You need to have Docker or the .Net CLI and RabbitMQ installed on your computer.
* The application uses SQLite so no database installation is required.

### Notes
Didn't have the time to configure a Volume for the docker container. Hence the data will be stored inside the container and will not be available after a container is stopped.
Some other things were not setup for docker. So I advise you have RabbitMQ installed and not run the app in a container.

### Installing

* Clone the project
* Open the /ChatApp folder on a terminal
* Run ``` dotnet ef database update ``` to create the database

### Executing program

* Open the /ChatApp folder on a terminal.
* If you have docker installed, run the command.
    ``` docker run -p 5274:80 -p 5672:5672 jobsitychatapp rabbitmq:3; ```
* else run the command ``` dotnet run```.
* Open the url https://localhost:7167 on your browser. You can also use http://localhost:5274.

## Authors

* [Sunrise Ezekikwu](https://linkedin.com/in/ezesunrise)
