# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Solution/SolutionX dosyasýný kopyala
COPY AssetManager.slnx ./

# 2. Tüm .csproj dosyalarýný (TESTLER DAHÝL) kopyala
# Uygulama Projeleri
COPY src/AssetManager.API/*.csproj src/AssetManager.API/
COPY src/AssetManager.Web/*.csproj src/AssetManager.Web/
COPY src/AssetManager.Application/*.csproj src/AssetManager.Application/
COPY src/AssetManager.Infrastructure/*.csproj src/AssetManager.Infrastructure/
COPY src/AssetManager.Domain/*.csproj src/AssetManager.Domain/

# Test Projeleri (Solution dosyasý bunlarý istediði için kopyalamalýyýz)
COPY tests/AssetManager.IntegrationTests/*.csproj tests/AssetManager.IntegrationTests/
COPY tests/AssetManager.UnitTests/*.csproj tests/AssetManager.UnitTests/

# 3. Restore iþlemini yap
RUN dotnet restore AssetManager.slnx

# 4. Tüm kaynak kodu kopyala
COPY . .

# 5. Publish API
RUN dotnet publish src/AssetManager.API/AssetManager.API.csproj -c Release -o /app/api --no-restore

# 6. Publish WEB
RUN dotnet publish src/AssetManager.Web/AssetManager.Web.csproj -c Release -o /app/web --no-restore

# ---------- RUNTIME API ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime-api
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
# API'nin çalýþmasý için gerekli ortam deðiþkenleri
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build /app/api .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AssetManager.API.dll"]

# ---------- RUNTIME WEB ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime-web
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
# Web'in çalýþmasý için gerekli ortam deðiþkenleri
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build /app/web .
EXPOSE 5000
ENTRYPOINT ["dotnet", "AssetManager.Web.dll"]