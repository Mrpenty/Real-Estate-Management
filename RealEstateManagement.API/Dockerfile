# ===================== STAGE 1: BUILD =====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj trước để cache restore (multi-project)
COPY RealEstateManagement.API/RealEstateManagement.API.csproj RealEstateManagement.API/
COPY RealEstateManagement.Business/RealEstateManagement.Business.csproj RealEstateManagement.Business/
COPY RealEstateManagement.Data/RealEstateManagement.Data.csproj RealEstateManagement.Data/

# Khôi phục package cho dự án API
RUN dotnet restore RealEstateManagement.API/RealEstateManagement.API.csproj

# Copy toàn bộ solution và publish
COPY . .
RUN dotnet publish RealEstateManagement.API/RealEstateManagement.API.csproj \
    -c Release -o /out /p:UseAppHost=false

# ===================== STAGE 2: RUNTIME =====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RealEstateManagement.API.dll"]
