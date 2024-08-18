FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY CatalogService/src/Core/Core.csproj CatalogService/src/Core/
COPY CatalogService/src/Infrastructure/Infrastructure.csproj CatalogService/src/Infrastructure/
COPY CatalogService/src/UseCases/UseCases.csproj CatalogService/src/UseCases/
COPY CatalogService/src/Web/Web.csproj CatalogService/src/Web/

COPY Common/Common.csproj Common/

RUN dotnet restore CatalogService/src/Web/Web.csproj

COPY CatalogService/ CatalogService/
COPY Common/ Common/

RUN dotnet publish CatalogService/src/Web/Web.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "Web.dll"]
