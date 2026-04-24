# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Run the service (binds to http://localhost:7000)
dotnet run

# Build
dotnet build

# EF Core migrations
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Start the database dependencies before running the service:
```bash
# From the repo root (taller-microservicios/)
docker compose up -d orders-db
```

## Architecture

This is the **OrdersService** microservice, one of four services in the `taller-microservicios` monorepo:

| Service | Stack | Port | Database |
|---|---|---|---|
| ApiGateway | ASP.NET Core 9 + Ocelot | 4000 | — |
| users-service | NestJS | 3000 | MongoDB (port 27017) |
| products-service | NestJS | 5000 | PostgreSQL (port 5432) |
| **OrdersService** | ASP.NET Core 9 | 7000 | SQL Server 2022 (port 1433) |

All external traffic enters through the API Gateway. Gateway routes `POST/GET /api/orders` → OrdersService `/orders` and `GET/DELETE /api/orders/{id}` → OrdersService `/orders/{id}`.

### Intended layering (partially scaffolded)

`Program.cs` wires up three layers that still need their source files created:

- **`OrdersService.Persistence`** — `OrderDbContext` (EF Core, SQL Server). Migrations already exist in `Migrations/`.
- **`OrdersService.Application.Services`** — `IOrderAppService` / `OrderAppService`. Business logic; calls out to users-service and products-service via injected `HttpClient`.
- **Controllers** — MVC controllers (not yet created) that map HTTP routes to `IOrderAppService`.

### Database schema

`Orders` (Id PK, UserId string, CreatedAt datetime2, TotalAmount decimal 18,2)  
`OrderItems` (Id PK, OrderId FK→Orders cascade, ProductId int, Quantity int)

### Connection string

Development uses SQL Server at `localhost,1433` (docker-compose maps the container's 1433 to host 1433). The connection string in `appsettings.Development.json` references port `14433` — **this is a known misconfiguration** that must be corrected before `dotnet ef database update` will work.