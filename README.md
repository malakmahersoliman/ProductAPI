# ProductAPI — Store Management Dashboard Backend

REST API for the **Store Management Dashboard**, a full-stack internal store operations application. This backend powers product catalog, customer records, cart checkout, order processing, user administration, and admin statistics for the [Angular frontend](https://github.com/malakmahersoliman/product-dashboard).

---

## Overview

| | |
|---|---|
| **Type** | REST API for store / back-office management |
| **Framework** | ASP.NET Core 10 |
| **Pattern** | CQRS with MediatR |
| **Database** | SQL Server + Entity Framework Core 10 |
| **Auth** | JWT Bearer (roles: `SuperAdmin`, `User`) |
| **Validation** | FluentValidation (MediatR pipeline) |
| **Docs** | Swagger UI (Development) |

### Domain responsibilities

- **Products & categories** — catalog with stock, availability, and category relationships
- **Customers** — records linked to orders at checkout
- **Orders** — transactional creation with stock validation and decrement
- **Payments** — simulated payment success/failure on order creation
- **Users** — staff accounts with role-based authorization
- **Statistics** — aggregated metrics for the admin dashboard

---

## Architecture

```
Controllers/          Thin HTTP layer → MediatR
Feature/              Commands & Queries (business logic handlers)
DTOs/                 Request/response contracts
Domain/               EF Core entities and enums
Data/                 AppDbContext + entity configurations
Services/             JWT, password hashing, current user
Common/               Validation pipeline, shared utilities
Migrations/           EF Core database migrations
```

Handlers inject `AppDbContext` directly (no repository layer). Business rules live in `Feature/**/**Handler.cs` files.

### Entity relationships

```
Category ──< Product ──< OrderItem >── Order >── Customer
                              │
                              └── Payment
Order ──> User (CreatedById)
```

---

## API endpoints

Base URL: `http://localhost:5023/api`

All endpoints except login require `Authorization: Bearer <token>`.

### Auth — `api/auth`

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/login` | None | Authenticate; returns JWT, email, role |

### Products — `api/products`

| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | `/` | Any | Paginated list (search, category, stock, sort) |
| GET | `/{id}` | Any | Product by ID |
| POST | `/` | SuperAdmin | Create product |
| PUT | `/{id}` | SuperAdmin | Update product |
| DELETE | `/{id}` | SuperAdmin | Delete (409 if linked to orders) |

### Categories — `api/categories`

| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | `/` | Any | List all categories |
| GET | `/{id}` | Any | Category by ID |
| POST | `/` | SuperAdmin | Create category |
| PUT | `/{id}` | SuperAdmin | Update category |
| DELETE | `/{id}` | SuperAdmin | Delete category |

### Customers — `api/customers`

| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | `/` | Any | List all customers |
| GET | `/{id}` | Any | Customer by ID |
| POST | `/` | Any | Create customer |
| PUT | `/{id}` | Any | Update customer |
| DELETE | `/{id}` | Any | Delete customer |

### Orders — `api/orders`

| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | `/` | Any | Paginated list (users see own orders only) |
| GET | `/{id}` | SuperAdmin | Order details |
| POST | `/` | Any | Create order (validates stock, uses transaction) |
| PUT | `/{id}/status` | SuperAdmin | Update order status |
| DELETE | `/{id}` | SuperAdmin | Delete order |

### Users — `api/users`

| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | `/` | SuperAdmin | List all users |
| GET | `/{id}` | SuperAdmin | User by ID |
| POST | `/` | SuperAdmin | Create user |

### Statistics — `api/statistics`

| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | `/` | SuperAdmin | Aggregated store metrics |

---

## Order creation (core business logic)

When `POST /api/orders` is called:

1. Validates customer and all products exist
2. Checks each product is available and has sufficient stock
3. Groups duplicate product lines
4. Simulates payment (`PaymentShouldSucceed` flag)
5. Runs inside a database transaction:
   - Decrements product stock
   - Creates `Order`, `OrderItem` records (with price snapshot), and `Payment`
   - Sets `CreatedById` from the JWT user

Regular users only see orders they created. SuperAdmin sees all orders.

---

## Roles

| Role | Capabilities |
|------|--------------|
| **SuperAdmin** | Full catalog management, all orders, statistics, user management |
| **User** | View products/categories, manage customers, create orders, view own orders |

JWT claims include `UserId`, `Email`, and `Role`. Token expiry defaults to 60 minutes (configurable in `appsettings.json`).

---

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full instance)
- [Angular frontend](https://github.com/malakmahersoliman/product-dashboard) (optional, for full app)

### Configuration

Update the connection string in `ProductAPI/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=Test;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
}
```

JWT settings are also in `appsettings.json` under the `Jwt` section. Change the secret key before any production deployment.

### Database

Apply migrations from the `ProductAPI` project directory:

```bash
cd ProductAPI
dotnet ef database update
```

### Run

```bash
cd ProductAPI
dotnet run --launch-profile http
```

| Profile | URL |
|---------|-----|
| HTTP | `http://localhost:5023` |
| HTTPS | `https://localhost:7128` |

### Swagger

In Development, open [http://localhost:5023/swagger](http://localhost:5023/swagger) to explore and test endpoints. Use the **Authorize** button with a Bearer token from `/api/auth/login`.

### CORS

The API allows requests from `http://localhost:4200` (Angular dev server).

---

## Project structure

```
ProductAPI/
├── Controllers/           Auth, Products, Orders, Customers, Categories, Users, Statistics
├── Feature/
│   ├── Auth/Login/
│   ├── Products/
│   ├── Orders/
│   ├── Customers/
│   ├── Categories/
│   ├── Users/
│   └── Statistics/
├── DTOs/                  Request/response types per domain
├── Domain/                Product, Category, Customer, Order, OrderItem, Payment, User
├── Data/
│   ├── AppDbContext.cs
│   └── Configrations/     EF entity configurations
├── Services/              JwtService, PasswordService, CurrentUserService
├── Common/Behaviors/      FluentValidation MediatR pipeline
├── Settings/              JwtSettings
├── Migrations/            EF Core migrations
├── Program.cs             DI, middleware, auth, CORS, exception handling
└── appsettings.json       Connection string, JWT, logging
```

---

## Tech stack

| Layer | Technology |
|-------|------------|
| Runtime | .NET 10 |
| Web | ASP.NET Core Web API |
| ORM | Entity Framework Core 10 + SQL Server |
| CQRS | MediatR 14 |
| Validation | FluentValidation 12 |
| Auth | JWT Bearer |
| API docs | Swashbuckle (Swagger) |
| Password | ASP.NET Identity PasswordHasher (login); BCrypt (user creation) |

---

## Error handling

Global exception middleware maps:

| Exception | HTTP status |
|-----------|-------------|
| `ValidationException` | 400 (field-level errors) |
| `UnauthorizedAccessException` | 401 |
| Other | 500 |

Controllers also return `404 Not Found`, `409 Conflict` (e.g. product delete with existing orders), and `401 Unauthorized` where appropriate.

---

## Related repository

**Frontend:** [product-dashboard](https://github.com/malakmahersoliman/product-dashboard) — Angular 21 SPA that consumes this API.

Together they form the **Store Management Dashboard** full-stack application.

### Frontend integration

- API base: `http://localhost:5023/api` (configured in frontend `environment.ts`)
- Auth: frontend stores JWT in `localStorage` and sends it via `Authorization` header
- CORS origin: `http://localhost:4200`

---

## Development notes

- Auto-migrate and seed code in `Program.cs` is commented out; run migrations manually or enable seeding as needed
- Ensure at least one user exists in the database before logging in (or uncomment seed logic)
- Swagger is enabled only in the Development environment
