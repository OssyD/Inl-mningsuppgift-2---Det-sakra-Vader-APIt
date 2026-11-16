# FailSafe WeatherMonitor

Console application that imports weather measurements from a data file and the Open-Meteo API into SQL Server using EF Core.

## Solution structure

| Project | Responsibility |
|---|---|
| `WeatherApp.Domain` | Entities, result types and abstractions. No external dependencies. |
| `WeatherApp.Infrastructure` | EF Core `DbContext`, migrations and repositories. |
| `WeatherApp.Integration` | Data sources (file and API), parsing, validation and bad data logging. |
| `WeatherApp.App` | Spectre.Console UI, application services and composition root. |

## Getting started

_TODO_

## Database migrations

_TODO_

## Cloud database (Azure SQL)

_TODO_
