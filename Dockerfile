# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Tüm kaynak kodu kopyala
COPY . .

# 2. EKSTRA ÖNLEM: Yerel obj/bin kalýntýlarýný Docker içinde temizle
RUN find . -type d -name "obj" -exec rm -rf {} +
RUN find . -type d -name "bin" -exec rm -rf {} +

# 3. Restore iþlemini yap
RUN dotnet restore AssetManager.slnx

# 4. Publish API
RUN dotnet publish src/AssetManager.API/AssetManager.API.csproj -c Release -o /app/api --no-restore

# 5. Publish WEB
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