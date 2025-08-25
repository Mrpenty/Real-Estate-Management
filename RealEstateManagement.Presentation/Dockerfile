# ===================== STAGE 1: BUILD =====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj để tối ưu cache restore
COPY RealEstateManagement.Presentation/RealEstateManagement.Presentation.csproj RealEstateManagement.Presentation/
COPY RealEstateManagement.Business/RealEstateManagement.Business.csproj RealEstateManagement.Business/
COPY RealEstateManagement.Data/RealEstateManagement.Data.csproj RealEstateManagement.Data/

RUN dotnet restore RealEstateManagement.Presentation/RealEstateManagement.Presentation.csproj

# Copy toàn bộ source và publish
COPY . .
RUN dotnet publish RealEstateManagement.Presentation/RealEstateManagement.Presentation.csproj \
    -c Release -o /out /p:UseAppHost=false

# ===================== STAGE 2: RUNTIME =====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# App listen trong container
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RealEstateManagement.Presentation.dll"]
