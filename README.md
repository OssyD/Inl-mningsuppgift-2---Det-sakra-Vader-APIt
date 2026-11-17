# FailSafe WeatherMonitor

Console application that imports weather measurements from a data file and the Open-Meteo API into SQL Server using EF Core.

## Solution structure

| Project | Responsibility |
|---|---|
| `WeatherApp.Domain` | Entities, result types and abstractions. No external dependencies. |
| `WeatherApp.Infrastructure` | EF Core `DbContext`, migrations and repositories. |
| `WeatherApp.Services` | Application logic used by the UI: file import, API fetch, validation, bad data logging and statistics. The only layer that talks to Infrastructure. |
| `WeatherApp.App` | Spectre.Console menus and views. References Services and Domain only, never the database layer. |

## Getting started

_TODO_

## Database migrations

_TODO_

## Cloud database (Azure SQL)

_TODO_
