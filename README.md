# 🏢 Enterprise Asset Manager (DevOps 101 Final Project)

This repository contains a high-performance **Enterprise Asset Management** system built with **.NET 10**, following **Clean Architecture** principles. The project is fully containerized using **Docker** and features a complete **CI/CD pipeline** via GitHub Actions for automated deployment to an Ubuntu VPS.

---

## 🚀 Technologies & Frameworks

* **Runtime:** .NET 10 (C#)
* **Architecture:** Clean Architecture with **CQRS** Pattern using **MediatR**
* **Validation:** **FluentValidation** (Automatic pipeline validation)
* **Mapping:** **AutoMapper** for DTO/Entity transformations
* **Database:** Microsoft SQL Server 2022 managed via **Entity Framework Core**
* **API Documentation:** Swagger / OpenApi
* **Security:** JWT (JSON Web Token) & Role-Based Access Control (RBAC)
* **UI:** ASP.NET Core MVC (Razor Views) with **Refit** or **HttpClient** for API consumption
* **DevOps:** Docker, Docker Compose, Nginx (Reverse Proxy), and GitHub Actions
* **Testing:** xUnit, Moq, and FluentAssertions for **Unit & Integration Tests**

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

---

## 🏗️ Architecture Overview

The project is structured into four main layers to ensure maintainability and scalability:

* **Domain:** Core  `Entities`, `Enums` and domain logic.
* **Application:** Orchestrates the flow of data. Includes `MediatR` Commands/Queries, `DTOs`, `Service and Repository Interfaces`, `AutoMapper Profiles`, and `Validation Logic` (Behaviors).
* **Infrastructure:** Deals with external concerns. Implements `Repositories`, `EF Core DbContext`, `Identity Services` (Token/Password Hashing), `Migrations`, and `Data Seeders`.
* **API & Web:** The entry points of the application. The **API** serves as the RESTful backend, while the **Web** project provides a modern MVC interface for end-users.

**Execution Pipeline:**
`Client` ➔ `Nginx` ➔ `MVC Controller` ➔ `Internal Service` ➔ `REST API` ➔ `MediatR Pipeline (Logging/Validation)` ➔ `Handler` ➔ `Infrastructure` ➔ `SQL Server`

---

## 🔐 Authorization Roles

| Role | Permissions | Responsibilities |
| :--- | :--- | :--- |
| **Admin** | Full Access (CRUD + System) | Manages settings, views logs, and oversees entire inventory. |
| **Editor** | Read & Update | Updates asset and department details; no access to logs. |
| **Guest** | Read-Only | Views inventory list and public endpoints. |

---

## 📡 Required Project Endpoints

As per the project requirements, the following endpoints are available:

* **Health Check:** `GET /health` -> Returns `200 OK` if the API and database are healthy. The response body will display "Healthy".
* **Information:** `GET /api/info` -> Returns a JSON object containing student details and environment info.

**Sample `/api/info` output:**
```json
{
 "student": "Aliye Bulut",
 "environment": "Production",
 "serverTimeUtc": "2026-03-17T18:00:00Z"
}
```

---

## 🛠️ Local Setup & Installation

This guide explains how to run the **Enterprise Asset Manager** project on your local machine for development purposes.

### 1. Clone the Repository
```bash
git clone https://github.com/AlyBlt/Enterprise-Asset-Manager.git
cd Enterprise-Asset-Manager
```

### 2. User Secrets
Sensitive information written in the .env.example such as database passwords or JWT key should be stored using User Secrets. You can set them via the command line:
```
cd src/AssetManager.API
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "YourSuperSecretKeyHere"
dotnet user-secrets set "Jwt:Issuer" "GivenValueHere"
dotnet user-secrets set "Jwt:Audience" "GivenValueHere"
dotnet user-secrets set "STUDENT_NAME" "GivenValueHere"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=GivenDBName;Trusted_Connection=True;TrustServerCertificate=True;"
```
User Secrets is for local development only.

### 3 .env File
So if you are running locally via dotnet run or dotnet ef, .env is not required. It is primarily used in Docker Compose.

### 4. Database Migrations
Migrations will use the Development environment settings. When you run the API, it will automatically apply any pending migrations and seed the database with initial data for departments, users, and assets.

### 5. Running the Application
```
# Terminal 1
cd src/AssetManager.API
dotnet run

# Terminal 2
cd src/AssetManager.Web
dotnet run
```

### 6. Summary
- Local development: LaunchSettings.json + User Secrets
- Sensitive data: Store in User Secrets
- .env file: Optional locally; used mainly for Docker/Compose
- Database: Automatically migrated and seeded in development environment

The required endpoints will be accessible at:
* **Health Check:** `GET /health`: `https://localhost:7111/health` (your configured `applicationUrl`). 
* **Information:** `GET /api/info`: `https://localhost:7111/api/info` (your configured `applicationUrl`).

---

## Running with Docker Compose (Recommended)

You should be in the root directory of the project to run the following commands.

### 1. Configure Environment Variables
Copy `.env.example` to a new file named `.env` and fill in your local configuration values.

Variables in the .env file include:
- `APP_PORT`: The port on which the application will run.
- `ASPNETCORE_ENVIRONMENT`: The environment for the application (e.g., Development, Production).
- `MSSQL_SA_PASSWORD`: The password for the SQL Server `sa` user.
- `STUDENT_NAME`: Student information.
- `ConnectionStrings__DefaultConnection`: The connection string for the SQL Server database.
- `Jwt__Key`: The secret key used for signing JWT tokens.
- `Jwt__Issuer`: The issuer of the JWT tokens.
- `Jwt__Audience`: The audience for the JWT tokens. 
- `Jwt__DurationInMinutes`: The duration (in minutes) for which the JWT tokens are valid.

### 2. Run with Docker Compose
To spin up all services (API, Web UI, SQL Server, and Nginx) simultaneously:
```bash
docker compose --env-file .env up --build
```
The application will be accessible at:
* **Health Check:** `GET /health`: `http://localhost:11111/health` (or your configured `APP_PORT`). 
* **Information:** `GET /api/info`: `http://localhost:11111/api/info` (or your configured `APP_PORT`).
* **UI** (Web MVC): `http://localhost:11111/` (or your configured `APP_PORT`).

* Note: If you want to run the tests; 
```bash
dotnet test
```

---

## ⚙️ CI/CD Pipeline & Deployment

The deployment is fully automated using **GitHub Actions**. Every push to the `main` branch triggers the following workflow:

1.  **Checkout:** Pulls the latest code from the repository.
2.  **Environment Setup:** Configures .NET 10 SDK.
3.  **Build & Test:** Restores dependencies and runs all **Unit & Integration tests**.
4.  **Deployment:** * Connects to the Ubuntu VPS via **SSH**.
    * Dynamically creates the `.env` file from **GitHub Secrets**.
    * Executes `docker-compose down` and `docker-compose up -d --build`.

### Required GitHub Secrets
To successfully run the deployment, the following secrets must be configured in **GitHub Repository Secrets**:

| Secret       | Description |
|--------------|-------------|
| VPS_HOST     | Target Ubuntu server IP address |
| VPS_USER     | SSH username |
| VPS_SSH_KEY  | SSH Key |
| DEPLOY_PATH  | Target directory on the server |
| APP_PORT     | External port the application will run on |
| STUDENT_NAME | Student full name (used in the Info endpoint) |
| MSSQL_SA_PASSWORD | SQL Server container password |
| ConnectionStrings__DefaultConnection | Connection string for SQL Server (used in the .env file) |
| Jwt__Key | Secret key for JWT token signing (used in the .env file) |
| Jwt__Audience | Audience for JWT tokens (used in the .env file) |
| Jwt__Issuer | Issuer for JWT tokens (used in the .env file) |
| Jwt__DurationInMinutes | Duration for JWT tokens in minutes (used in the .env file) |

> [!TIP]
> **Authentication Note:** If you prefer using a password instead of an SSH Key, save your password as `VPS_PASSWORD` in GitHub Secrets and update the `deploy.yml` file by replacing the `key: ${{ secrets.VPS_SSH_KEY }}` line with `password: ${{ secrets.VPS_PASSWORD }}`.

### Preparation on the Ubuntu VPS:

1. Install Docker and Docker Compose.
2. Ensure the server is accessible via SSH.
3. Create the target deployment directory (e.g., `/home/username/asset-manager`).
```
mkdir -p /home/ubuntu/assetmanager
cd /home/ubuntu/assetmanager
git clone https://github.com/AlyBlt/Enterprise-Asset-Manager.git .
```
The GitHub Actions workflow will handle the rest of the deployment process, including building and running the Docker containers. (Make sure to push your code to the `main` branch to trigger the deployment.)

### Post-Deployment
After deployment, the application will be accessible at `http://devops101.gturkmen.net:11111`. 
- You can verify the deployment by accessing the health check and info endpoints:
* **Health Check:** `GET /health`: `http://devops101.gturkmen.net:11111/health`
* **Information:** `GET /api/info`: `http://devops101.gturkmen.net:11111/api/info`

---

## 🧪 Running Tests (Optional for Local Development)

```bash
dotnet test AssetManager.slnx
```

---

## 👤 Contact & Support

**Aliye Bulut**
* **GitHub:** [AlyBlt](https://github.com/AlyBlt)
* **LinkedIn:** [Aliye Bulut](https://www.linkedin.com/in/aliye-bulut-phd-867453357/)
* **Project Link:** [https://github.com/AlyBlt/Enterprise-Asset-Manager](https://github.com/AlyBlt/Enterprise-Asset-Manager)

> This project was developed as a Final Project for the **DevOps 101** course.

