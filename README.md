# Jobs
<img alt="GitHub Workflow Status (branch)" src="https://img.shields.io/github/workflow/status/MirolimMajidov/Jobs/Build%20Jobs%20solution/master?label=Build%20Jobs%20solution">  <img alt="GitHub Workflow Status" src="https://img.shields.io/github/workflow/status/MirolimMajidov/Jobs/Identity%20service/master?label=Identity%20service%20tests">  <img alt="GitHub Workflow Status" src="https://img.shields.io/github/workflow/status/MirolimMajidov/Jobs/Job%20service/master?label=Job%20service%20tests">  <img alt="GitHub Workflow Status" src="https://img.shields.io/github/workflow/status/MirolimMajidov/Jobs/Payment%20service/master?label=Payment%20service%20tests">

Jobs is an open-source job marketplace application, powered by micro-services ☸️ architecture and cross-platform developed 📱🖥🐳 based on .NET 8.

## Architecture Overview

The architecture proposes a microservice-oriented architecture implementation with multiple autonomous microservices (each one owning its own data/db) and has one shared project to work with the Repository and Controller for CRUD operations, but implementing different approaches within each microservice using REST/HTTP as the communication protocol between the client apps, and supports asynchronous communication for data updates propagation across multiple services based on gRPC/HTTP2. All microservices are based on SOLID design principles and use popular modern technologies. Also, configured the CI/CD pipelines using GitHub Actions for building and testing the code and publishing the docker image files to the Docker Hub.

<center><img src="img/JobsArchitecture.png"/></center>

## List of micro-services and infrastructure components

<table>
   <thead>
    <th>№</th>
    <th>Service</th>
    <th>Description</th>
    <th>Build status</th>
    <th>Endpoints</th>
  </thead>
  <tbody>
    <tr>
        <td align="center">1.</td>
        <td>API Gateway (Ocelot)</td>
        <td>This service is responsible for all other micro-services</td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td align="center">2.</td>
        <td>Identity API (JWT Token, NLog logging, FW Core, gRPC, CRUD by MySQL, RabbitMQ, FluentValidation, DTO, AutoMapper, Functionality and Unit testing with MSTest)</td>
        <td>Identity management service</td>
        <td>
            <a href="https://github.com/MirolimMajidov/Jobs/actions?query=workflow%3AIdentity%20service">
                <img src="https://github.com/MirolimMajidov/Jobs/workflows/Identity%20service/badge.svg?branch=master">
            </a>
        </td>
        <td align="center"> 
            <a href="https://petstore.swagger.io/?url=https://raw.githubusercontent.com/MirolimMajidov/Jobs/master/src/Services/Identity/Identity.API/Swagger/v1/docs.json">
               APIs
            </a>
       </td>
    </tr>
    <tr>
        <td align="center">3.</td>
        <td>Job API (NLog logging, FW Core, CRUD by SQL Server, RabbitMQ, FluentValidation, DTO, AutoMapper, Unit Testing with NUnit)</td>
        <td>This service is responsible for the main part of the current application. All CRUD operations related to Jobs will be here.</td>
        <td>
            <a href="https://github.com/MirolimMajidov/Jobs/actions?query=workflow%3AJob%20service">
                <img src="https://github.com/MirolimMajidov/Jobs/workflows/Job%20service/badge.svg?branch=master">
            </a>
        </td>
        <td align="center"> 
            <a href="https://petstore.swagger.io/?url=https://raw.githubusercontent.com/MirolimMajidov/Jobs/master/src/Services/Job/Job.API/Swagger/v1/docs.json">
               APIs
            </a>
       </td>
    </tr>
    <tr>
        <td align="center">4.</td>
        <td>Payment API (Serilog logging, Repocitory, CRUD by MongoDB and Redis, RabbitMQ, FluentValidation, DTO, AutoMapper, Unit Testing with xTest)</td>
        <td>Responsible for financial and payments</td>
        <td>
            <a href="https://github.com/MirolimMajidov/Jobs/actions?query=workflow%3APayment%20service">
                <img src="https://github.com/MirolimMajidov/Jobs/workflows/Payment%20service/badge.svg?branch=master">
            </a>
        </td>
        <td align="center"> 
            <a href="https://petstore.swagger.io/?url=https://raw.githubusercontent.com/MirolimMajidov/Jobs/master/src/Services/Payment/Payment.API/Swagger/v1/docs.json">
               APIs
            </a>
       </td>
    </tr>
  </tbody>  
</table>

Each microservice has its own docker image file with the latest code of the master branch on my [Docker Hub](https://hub.docker.com/u/mirolimmajidov/) with the `latest` tag.

## Using the Jobs services
Ensure you have installed and configured [Docker Desktop for Windows](https://docs.docker.com/docker-for-windows/install/) in your machine. 

### Running the services on Docker
You need to just run the commands below from the main Jobs repository directory and get started with the `Jobs` services immediately.

```powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

You should be able to browse each service of the application by using the below URLs:
<ul>
   <li><a href="http://localhost:7000/status" rel="nofollow">API Gateway's status</a></li>
   <li><a href="http://localhost:7001/" rel="nofollow">Identity service</a> | <a href="http://localhost:7101/" rel="nofollow">Identity service for gRPC</a></li>
   <li><a href="http://localhost:7002/" rel="nofollow">Jobs service</a></li>
   <li><a href="http://localhost:7003/" rel="nofollow">Payment service</a></li>
   <li><a href="http://localhost:7004/" rel="nofollow">RabbitMQ Management</a></li>
</ul>

### Running the services on Kubernetes (K8s)
Before running the Jobs services, you need to make sure you have enabled the Kubernetes from the Docker Desktop. Then you can run one of the scripts of commands below from the main Jobs repository's `K8s\Commands` directory and get started with the `Jobs` services immediately:
<ul>
   <li>StartServices.sh - For starting all Jobs services.</li>
   <li>StopServices.sh - For stopping all Jobs services.</li>
   <li>RestartServices.sh - For stopping and starting all Jobs services.</li>
</ul>

You should be able to browse each service of the application by using the below URLs:
<ul>
   <li><a href="http://localhost:8000/status" rel="nofollow">API Gateway's status</a></li>
   <li><a href="http://localhost:8001/" rel="nofollow">Identity service</a> | <a href="http://localhost:8101/" rel="nofollow">Identity service for gRPC</a></li>
   <li><a href="http://localhost:8002/" rel="nofollow">Jobs service</a></li>
   <li><a href="http://localhost:8003/" rel="nofollow">Payment service</a></li>
   <li><a href="http://localhost:8004/" rel="nofollow">RabbitMQ Management</a></li>
</ul>
