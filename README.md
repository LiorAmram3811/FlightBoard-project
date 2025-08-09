````markdown
# FlightBoard

A full‑stack flight board: **ASP.NET Core 8** backend with **SQL Server + EF Core**, **SignalR** live updates, and a **React 19 + Vite + Tailwind** frontend. Comes with **Docker Compose** for a one‑command local stack.

> Docker Compose will start SQL Server, the API, and the web app (served by Nginx). Your browser talks to the web app on **http://localhost:5173**, and the API is on **http://localhost:8080** 
(also proxied under `/api` from the web container).

---

## Quick start (Docker Compose)

### Prerequisites
- Docker Desktop (or Docker Engine) + Docker Compose v2
- Ports free on your host: **1433**, **8080**, **5173**

### 1) Build & run
```bash
# from the repo root
docker compose up -d --build
````

Compose spins up three services:

* **db** — `mcr.microsoft.com/mssql/server:2022-latest`

  * Exposes **1433**
  * SA password (dev only): **set via environment variable** (see `.env.example`)
  * Persistent volume: `mssql_data`

* **api** — ASP.NET Core 8 (Kestrel) on **8080**

  * Connection string is injected via env, for example:
    `Server=db;Database=FlightBoardDB;User Id=sa;Password=<YOUR_DEV_PASSWORD>;TrustServerCertificate=True;Encrypt=False`
  * Waits for the DB healthcheck before starting
  * Runs **EF Core migrations** on startup and **seeds** sample flights if the DB is empty

* **web** — Built React app served by **Nginx** on **5173**

  * Proxies `/api/*` and `/hubs/*` to the **api** service (websockets enabled)

### 2) Open the apps

* Frontend: **[http://localhost:5173](http://localhost:5173)**
* API Swagger UI: **[http://localhost:8080/swagger](http://localhost:8080/swagger)**
* SignalR hub (for info): **[http://localhost:8080/hubs/flights](http://localhost:8080/hubs/flights)**

### 3) Useful commands

```bash
# View logs
docker compose logs -f api
docker compose logs -f web
docker compose logs -f db

# Rebuild everything
docker compose down -v
docker compose up -d --build
```

---

## Running locally without Docker

### Backend (ASP.NET Core 8)

1. Prerequisites: **.NET 8 SDK**, local SQL Server (or change `UseSqlServer(...)` to `UseSqlite(...)` for SQLite).
2. Configure the connection string:

   * `FlightBoard.API/appsettings.json` → `ConnectionStrings:FlightBoardDbConnectionString`, **or**
   * set an env var: `ConnectionStrings__FlightBoardDbConnectionString`.
3. Run:

   ```bash
   dotnet restore
   dotnet run --project FlightBoard.API
   ```
4. Browse Swagger at [http://localhost:{YOUR_LOCAL_PORT}/swagger](http://localhost:{YOUR_LOCAL_PORT}/swagger).

### Frontend (React + Vite)

1. Prerequisites: **Node 20+** and npm.
2. Configure env in `flightboard-frontend/.env`:

   ```env
   VITE_API_BASE=http://localhost:{YOUR_LOCAL_PORT}
   VITE_HUB_URL=http://localhost:{YOUR_LOCAL_PORT}/hubs/flights
   ```
3. Install & run:

   ```bash
   cd flightboard-frontend
   npm ci
   npm run dev
   ```
4. Open [http://localhost:5173](http://localhost:{YOUR_LOCAL_PORT})

---

## API overview

**Base URL (dev):** `http://localhost:8080/api`

| Method | Path              | Description                      |
| -----: | ----------------- | -------------------------------- |
|    GET | `/flights`        | List all flights                 |
|    GET | `/flights/search` | Filter by `status`/`destination` |
|   POST | `/flights`        | Create a flight                  |
| DELETE | `/flights/{id}`   | Delete a flight                  |

**SignalR hub:** `/hubs/flights`
Server emits: `FlightAdded`, `FlightDeleted`, `FlightStatusUpdated`.

---

## Frontend overview

* React 19 + Vite + TypeScript
* **React Query** manages server state
* **Redux Toolkit** stores UI state (filters)
* **SignalR** client listens for server events
* **Tailwind CSS** for styling; **Nginx** serves the built SPA in Docker

---

## Architecture & decisions (short)

* **Clean layering:** Domain → Application → Infrastructure → API.
* **EF Core** with SQL Server; migrations run at startup.
* **SignalR** for real‑time updates.
* **React Query** + **Axios** for API calls.
* **Redux Toolkit** for predictable state.
* **Nginx** reverse proxy to avoid CORS.

---

## Third‑party libraries

### Backend

* AutoMapper.Extensions.Microsoft.DependencyInjection
* Microsoft.AspNetCore.SignalR
* Microsoft.EntityFrameworkCore
* Microsoft.OpenApi & Swashbuckle.AspNetCore
* Newtonsoft.Json
* FluentValidation

### Frontend

* @microsoft/signalr
* @reduxjs/toolkit
* @tanstack/react-query
* axios
* react, react-dom, react-redux, react-hook-form, react-toastify
* tailwindcss
* yup

---

## Troubleshooting

* **DB healthcheck fails**: ensure ports are free and Docker has enough RAM.
* **CORS errors**: adjust `VITE_API_BASE` and CORS policy.
* **SignalR fails**: verify hub URL and ports.

---

© 2025 FlightBoard

