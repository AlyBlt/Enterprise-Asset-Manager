# Enterprise Asset Manager - DevOps 101 Final Project

This project is a **.NET 10** based Web API and Web UI application developed using **Clean Architecture principles** to manage corporate assets, users, and departments.

The system is fully **containerized using Docker**, served behind an **Nginx reverse proxy**, and features a **CI/CD pipeline with GitHub Actions** for automated deployment to an Ubuntu server.

---

## Technologies Used

### Backend
- .NET 10 Web API

### Frontend
- ASP.NET Core MVC (Web UI)

### Database
- MSSQL Server (Containerized)

### DevOps & Infrastructure
- Docker
- Docker Compose
- Nginx (Reverse Proxy)
- GitHub Actions (CI/CD)

### Testing
- xUnit
- Moq
- Shouldly  
*Unit & Integration Tests*

### Architecture
- Clean Architecture
- CQRS (MediatR)
- AutoMapper
- FluentValidation

---

## Project Structure

```text
repo/
|-- src/
|   |-- AssetManager.Domain/         # Entities, Enums & Core Domain Logic
|   |-- AssetManager.Application/    # CQRS (Features), DTOs, Interfaces, Mappings
|   |-- AssetManager.Infrastructure/ # Data (EF Core), Identity, Repositories, Migrations
|   |-- AssetManager.API/            # Web API Endpoints & Middlewares
|   `-- AssetManager.Web/            # Web UI (Controllers, Models, Razor Views)
|
|-- nginx/
|   `-- default.conf                 # Nginx Reverse Proxy Configuration
|
|-- tests/                           # Unit & Integration Test Projects
|
|-- .github/
|   `-- workflows/
|       `-- deploy.yml               # GitHub Actions CI/CD Workflow
|
|-- Dockerfile                       # Multi-stage Docker Build File
|-- compose.yml                      # Multi-container Orchestration (API, DB, Nginx)
|-- .env.example                     # Environment Variables Template
`-- README.md                        # Project Documentation
```

## Installation and Running

### Running the Enterprise Asset Manager Locally

This guide explains how to run the **Enterprise Asset Manager** project on your local machine for development purposes.

---

#### 1. Environment Setup

##### a. Launch Settings
The project uses **`launchSettings.json`** for local environment configuration. Ensure that the development environment is set:

```json
{
  "profiles": {
    "AssetManager.API": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "STUDENT_NAME": "Your Name"
      }
    }
  }
}

```
Note: The ASPNETCORE_ENVIRONMENT here controls EF migrations, seeding, and other environment-specific logic when running locally.

##### b. User Secrets

Sensitive information such as database passwords or JWT keys should be stored using User Secrets. You can set them via the command line:
```
cd src/AssetManager.API
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "YourSuperSecretKeyHere"
dotnet user-secrets set "Jwt:Issuer" "GivenValueHere"
dotnet user-secrets set "Jwt:Audience" "GivenValueHere"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=GivenDBName;Trusted_Connection=True;TrustServerCertificate=True;"
```
Do not commit secrets to source control. User Secrets is for local development only.

##### c. .env File

So if you are running locally via dotnet run or dotnet ef, .env is not required. It is primarily used in Docker Compose.

#### 2. Database Migrations
Before running the app locally, create the database and apply migrations:
```
cd src/AssetManager.Infrastructure

# Add migration (only needed once)
dotnet ef migrations add InitialCreate --startup-project ../AssetManager.API

# Apply migration
dotnet ef database update --startup-project ../AssetManager.API
```
Migrations will use the Development environment settings.

#### 3. Running the Application
```
cd src/AssetManager.API WEb de gelecek buraya !!!!!!!
dotnet run
```
The API will start using the Development environment.

Database seeding will automatically run for departments, users, and assets.

JWT and connection strings will be taken from User Secrets.

#### 4. Summary

Local development: LaunchSettings.json + User Secrets

Sensitive data: Store in User Secrets (never commit)

.env file: Optional locally; used mainly for Docker/Compose

Database: Automatically migrated and seeded in development environment

Following this setup ensures your local environment mimics production behavior safely without exposing sensitive information.

### 1. Local Development

Clone the repository:

```
git clone <repo-url>
```

Navigate to the project directory:

```
cd src
```

Restore dependencies:

```
dotnet restore
```

Run the API project:

```
dotnet run --project AssetManager.API
```

---

### 2. Running with Docker

To build and run the application as a single container:

```bash
docker build -t asset-manager-api .
docker run -d -p 8080:8080 asset-manager-api
```

---

### 3. Running with Docker Compose (Recommended)

To run the entire stack (**Nginx + API + SQL Server**):

Copy the environment template:

```bash
cp .env.example .env
```

Edit the `.env` file and fill in the required values.

Start the containers:

```bash
docker-compose up -d
```

---

## Assignment Requirements (Mandatory Endpoints)

The following mandatory endpoints are implemented in **AssetManager.API**.

### Health Check

```
GET /health
```

Verifies the system is running correctly.

Response:

```
200 OK
```

---

### Info Endpoint

```
GET /api/info
```

Returns student information, environment, and server time in JSON format.

Example response:

```json
{
  "student": "John Doe",
  "environment": "Production",
  "serverTimeUtc": "2026-03-07T18:00:00Z"
}
```

---

## Deployment Process (CI/CD)

The project uses **GitHub Actions** for automated deployment.

Every push to the **main branch** triggers the workflow.

### Workflow Steps

1. **Checkout**
   Pulls the source code from the repository.

2. **Setup .NET**
   Installs the .NET 10 SDK.

3. **Restore & Build**
   Restores dependencies and compiles the project.

4. **Test**
   Runs Unit and Integration tests.

5. **Deploy**
   Connects to the Ubuntu server via SSH, transfers files, and executes:

```bash
docker-compose up --build -d
```

---

## GitHub Secrets List

The following secrets must be configured in **GitHub Repository Secrets**.
| Secret       | Description |
|--------------|-------------|
| VPS_HOST     | Target Ubuntu server IP address |
| VPS_USER     | SSH username |
| VPS_PASSWORD | SSH password |
| DEPLOY_PATH  | Target directory on the server |
| APP_PORT     | External port the application will run on |
| STUDENT_NAME | Student full name (used in the Info endpoint) |
| DB_PASSWORD  | SQL Server container password |


---

## Features

- Asset management
- User and department management
- Clean Architecture implementation
- Containerized deployment with Docker
- Reverse proxy with Nginx
- Automated CI/CD pipeline with GitHub Actions
- Unit and Integration testing

---
## Note

This project was developed as the final assignment for the **DevOps 101 course**.
