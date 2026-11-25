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

### Proof of migration to Azure

The tables (`WeatherDataPoints` and `__EFMigrationsHistory`) have been migrated to the Azure SQL database `WeatherMonitorDb` on `failsafeserver` with EF Core. The screenshot below is from the query editor in the Azure portal and shows the table with data imported from the Open-Meteo API.

![WeatherDataPoints in Azure SQL Database](docs/images/azure-sql-weatherdatapoints.png)

