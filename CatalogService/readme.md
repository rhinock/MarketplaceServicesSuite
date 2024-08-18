# EF Core

dotnet ef migrations add InitialCreate --project  .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\Web\

dotnet ef database update InitialCreate --project  .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\Web\
dotnet ef database update --project  .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\Web\
dotnet ef database update 0 --project  .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\Web\

dotnet ef migrations list --project  .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\Web\

dotnet ef migrations remove --project  .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\Web\