FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY IdentityService/IdentityService.csproj IdentityService/
RUN dotnet restore IdentityService/IdentityService.csproj

COPY IdentityService/ IdentityService/

RUN dotnet publish IdentityService/IdentityService.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "IdentityService.dll"]
