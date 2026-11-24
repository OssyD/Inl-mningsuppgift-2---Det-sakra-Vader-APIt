# FailSafe WeatherMonitor

Console application that imports weather measurements from a CSV file and the Open-Meteo API into SQL Server using EF Core.

## Solution structure

| Project | Responsibility |
|---|---|
| `WeatherApp.Domain` | Entities, result types and abstractions. |
| `WeatherApp.Infrastructure` | EF Core `DbContext`, migrations and repositories. |
| `WeatherApp.Services` | File import, API fetch, validation, logging and statistics. |
| `WeatherApp.App` | Spectre.Console menus and views. |

## Getting started

1. Create the database (see below).
2. Run `WeatherApp.App`.

The local connection string (`(localdb)\MSSQLLocalDB`) is in `WeatherApp.App/appsettings.json`.

## Database migrations

```powershell
dotnet ef database update --project WeatherApp.Infrastructure --startup-project WeatherApp.App
```

## Cloud database (Azure SQL)

The Azure connection string is in `WeatherApp.App/appsettings.Production.json`. It is used when `DOTNET_ENVIRONMENT` is set to `Production`:

```powershell
$env:DOTNET_ENVIRONMENT = "Production"
dotnet ef database update --project WeatherApp.Infrastructure --startup-project WeatherApp.App
dotnet run --project WeatherApp.App
```

Tables migrated to Azure SQL Database:

![Tables in Azure SQL Database](docs/azure-tables.png)
