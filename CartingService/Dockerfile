FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY CartingService/src/Core/Core.csproj CartingService/src/Core/
COPY CartingService/src/Infrastructure/Infrastructure.csproj CartingService/src/Infrastructure/
COPY CartingService/src/UseCases/UseCases.csproj CartingService/src/UseCases/
COPY CartingService/src/Web/Web.csproj CartingService/src/Web/

COPY Common/Common.csproj Common/

RUN dotnet restore CartingService/src/Web/Web.csproj

COPY CartingService/ CartingService/
COPY Common/ Common/

RUN dotnet publish CartingService/src/Web/Web.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "Web.dll"]
