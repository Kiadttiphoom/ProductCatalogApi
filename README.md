<div align="center">

# 🛒 ProductCatalogApi

### A Modern REST API for Product Management

**Built with .NET 8.0 | ASP.NET Core | Dapper | SQL Server**

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue.svg)](https://docs.microsoft.com/aspnet/core)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

</div>

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [API Endpoints](#api-endpoints)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [Performance](#performance)
- [Testing](#testing)
- [Roadmap](#roadmap)

---

## Overview

A comprehensive REST API for managing products, provinces, and districts with enterprise-grade features including caching, structured logging, input validation, and comprehensive testing.

### ✨ Key Highlights

- **Auto-generated API Documentation** via Swagger/OpenAPI
- **Structured Logging** with Serilog
- **Input Validation** with query parameters & pagination
- **Memory Caching** for optimal performance
- **Clean Architecture** with separation of concerns
- **Comprehensive Testing** with xUnit & Moq
- **API Versioning** ready for future scalability

---

## Features

### 📚 Swagger/OpenAPI Documentation
- Interactive API documentation at `http://localhost:5000/`
- Full endpoint descriptions with request/response schemas
- Try-it-out functionality for instant testing
- **Package**: `Swashbuckle.AspNetCore v6.4.10`

### 📝 Structured Logging
- Professional logging with Serilog integration
- Dual output: Console + File (daily rolling)
- Correlation ID support for distributed tracing
- **Package**: `Serilog.AspNetCore v8.0.1`

### ✓ Input Validation & Query Parameters
- Advanced product filtering:
  - `SearchTerm` - Search by product name
  - `BrandID` - Filter by brand
- Automatic validation with descriptive error responses

### 🔒 CORS Configuration
- Development: Allow all origins for easy testing
- Production: Configurable for security
- Seamless cross-origin API access

### 📌 API Versioning
- v1 API version implemented
- Backward compatibility ready
- Easy migration path to v2
- **Package**: `Asp.Versioning.Mvc.ApiExplorer v8.1.0`

### 🏗️ Clean Architecture

```
ProductCatalogApi/
├── Controllers/              # API Controllers
├── DTOs/                     # Data Transfer Objects
│   ├── ApiResponse.cs
│   ├── PaginationQuery.cs
│   └── ProductQuery.cs
├── Repositories/             # Data Access Layer
│   ├── IRepository.cs
│   └── Repository.cs
├── Services/                 # Business Logic Layer
│   └── DataService.cs
├── Models/                   # Entity Models
└── ProductCatalogApi.Tests/  # Unit & Integration Tests
```

### 🧪 Comprehensive Testing
- Unit tests for DataService
- Pagination query validation tests
- API response DTOs tests
- Mock repositories for isolation
- **Packages**: `xUnit v2.7.1`, `Moq v4.20.70`

---

## API Endpoints

### 📦 Products

```http
GET /api/products?searchTerm=laptop&brandId=1
```

**Response:**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [...]
  },
  "elapsedMilliseconds": 45
}
```

### 🌍 Provinces

```http
GET /api/province
```

- **Cache Duration**: 30 minutes
- **Response**: List of all provinces

### 🏘️ Districts

```http
GET /api/district?provinceId=1
```

- **Cache Duration**: 30 minutes
- **Response**: List of districts by province

---

## Error Handling

All errors return consistent JSON responses:

```json
{
  "statusCode": 500,
  "message": "Internal Server Error",
  "detailed": "Actual error message (dev only)",
  "timestamp": "2026-05-07T10:30:00Z"
}
```

---

## Getting Started

### 🚀 Development

```bash
# Clone the repository
git clone <repository-url>
cd TestApi

# Restore dependencies
dotnet restore

# Run the application
dotnet run

# Access Swagger UI
open http://localhost:5000
```

### 🏭 Production

```bash
# Build for production
dotnet publish -c Release -o ./publish

# Run the published application
dotnet ./publish/ProductCatalogApi.dll
```

### 🧪 Running Tests

```bash
# Run all tests
dotnet test ProductCatalogApi.Tests/ProductCatalogApi.Tests.csproj

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## Configuration

### 📄 appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sadata;User Id=sa;Password=1234;TrustServerCertificate=True"
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      "Console",
      {
        "Name": "File",
        "Args": {
          "path": "logs/.log",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName"]
  }
}
```

### 📊 Logging Configuration

| Output | Location | Format |
|--------|----------|--------|
| **Console** | Terminal | Real-time output |
| **File** | `logs/YYYY-MM-DD.log` | Daily rolling files |

---

## Project Structure

| Directory | Purpose |
|-----------|---------|
| `Controllers/` | API endpoint handlers |
| `DTOs/` | Request/Response models with validation attributes |
| `Models/` | Entity models (Product, Province, District) |
| `Repositories/` | Data access layer using Dapper + SQL |
| `Services/` | Business logic, caching, and logging |
| `Views/` | Razor views (if needed) |
| `wwwroot/` | Static assets |
| `ProductCatalogApi.Tests/` | Unit and integration tests |

---

## Technology Stack

| Category | Technology | Version |
|----------|------------|---------|
| **Framework** | .NET | 8.0 |
| **Web Framework** | ASP.NET Core | 8.0 |
| **ORM** | Dapper | Latest |
| **Database** | SQL Server | Latest |
| **Caching** | In-Memory Cache | Built-in |
| **Logging** | Serilog | 8.0.1 |
| **API Docs** | Swagger/OpenAPI | 6.4.10 |
| **API Versioning** | Asp.Versioning | 8.1.0 |
| **Testing** | xUnit, Moq | 2.7.1, 4.20.70 |
| **CORS** | Built-in | - |

---

## Performance

### 💾 Memory Caching Strategy

| Data Type | Cache Duration | Strategy |
|-----------|----------------|----------|
| Provinces | 30 minutes | Full cache |
| Districts | 30 minutes | Full cache |
| Products | Query-based | Not cached (dynamic filters) |

### ⚡ Database Query Optimizations

- **Dapper ORM** for high-performance data access
- **SQL OFFSET/FETCH** for efficient pagination
- **Connection pooling** for optimal resource usage
- **Parameterized queries** to prevent SQL injection

### 📈 Response Metrics

- Elapsed time tracking included in all API responses
- Performance logging for monitoring
- Correlation IDs for request tracing

---

## Database Health Check

The application automatically performs a database connection check on startup:

```
✅ Database Connection: Success!
```

If the connection fails:
- Logs an error but continues running
- Uncomment `// throw;` in `Program.cs` to fail fast

---

## Roadmap

### 🎯 Future Enhancements

- [ ] 🔐 Authentication/Authorization (JWT)
- [ ] 🗄️ Database query optimization
- [ ] 🔄 Distributed caching (Redis)
- [ ] 🚦 Rate limiting
- [ ] 📊 API usage analytics
- [ ] 📦 Batch operations
- [ ] 📤 Export to CSV/Excel
- [ ] 🔍 Advanced filtering and sorting
- [ ] 🌐 Internationalization (i18n)
- [ ] 📱 GraphQL support

### ⚠️ Known Issues

- Database connection must be available on startup
- In-memory cache not shared across multiple instances (consider Redis for distributed scenarios)

---

## Support

For issues, questions, or contributions:

- 📝 Check the logs in `logs/` folder
- 📖 Review the API documentation at Swagger UI
- 🐛 Open an issue for bugs
- 💡 Submit a pull request for improvements

---

<div align="center">

**Built with ❤️ using .NET 8.0**

</div>
