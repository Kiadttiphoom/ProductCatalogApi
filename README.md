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

### � JWT Authentication
- Token-based authentication for API endpoints
- Configurable expiration time
- Secure token validation with HMAC SHA256
- Bearer token support
- **Package**: `System.IdentityModel.Tokens.Jwt v8.16.0`

### 📚 Swagger/OpenAPI Documentation
- Interactive API documentation at `http://localhost:5000/`
- Full endpoint descriptions with request/response schemas
- Try-it-out functionality for instant testing
- **Package**: `Swashbuckle.AspNetCore v6.5.0`

### ✅ FluentValidation
- Comprehensive input validation for all DTOs
- Custom validation rules for Product, Province, and District
- Automatic validation in request pipeline
- Detailed error messages
- **Package**: `FluentValidation v11.9.2`

### 📝 Structured Logging
- Professional logging with Serilog integration
- Dual output: Console + File (daily rolling)
- Correlation ID support for distributed tracing
- **Package**: `Serilog.AspNetCore v8.0.1`

### ❤️ Health Checks
- API health endpoint at `/health`
- Database connectivity checks
- JSON response format for monitoring
- Integration with monitoring systems
- **Package**: `Microsoft.Extensions.Diagnostics.HealthChecks v8.0.8`

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

### 🏗️ Clean Architecture with Dependency Injection

```
ProductCatalogApi/
├── Interfaces/               # Abstraction Layer
│   ├── Repositories/
│   │   ├── IProductRepository.cs
│   │   ├── IProvinceRepository.cs
│   │   └── IDistrictRepository.cs
│   └── Services/
│       ├── IProductService.cs
│       ├── IProvinceService.cs
│       └── IDistrictService.cs
├── Repositories/             # Data Access Layer
│   ├── ProductRepository.cs
│   ├── ProvinceRepository.cs
│   └── DistrictRepository.cs
├── Services/                 # Business Logic Layer
│   ├── ProductService.cs
│   ├── ProvinceService.cs
│   └── DistrictService.cs
├── Authentication/           # JWT Token Service
│   ├── JwtService.cs
│   ├── JwtSettings.cs
│   └── IJwtService.cs
├── HealthChecks/            # Health Check Implementation
│   ├── DatabaseHealthCheck.cs
│   └── ApiHealthCheck.cs
├── Validators/              # FluentValidation Rules
│   ├── ProductValidator.cs
│   ├── ProvinceValidator.cs
│   └── DistrictValidator.cs
├── Extensions/              # Dependency Injection Configuration
│   ├── ServiceCollectionExtensions.cs
│   ├── AuthenticationExtensions.cs
│   ├── ValidationExtensions.cs
│   └── HealthCheckExtensions.cs
├── Models/                  # Entity Models
├── DTOs/                    # Data Transfer Objects
├── Controllers/             # API Controllers
└── Routes/                  # Minimal API Routes
    ├── ProductRoutes.cs
    ├── ProvinceRoutes.cs
    └── DistrictRoutes.cs
```

### 🧪 Comprehensive Testing
- Unit tests with xUnit
- Mock objects with Moq
- Fluent Assertions for readable test code
- Service layer tests
- Authentication tests
- Validator tests
- **Packages**: `xunit v2.7.1`, `Moq v4.20.70`, `FluentAssertions v6.12.0`
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

## CI/CD Pipeline

### 🚀 GitHub Actions Workflows

The project includes automated CI/CD workflows:

#### 1. **Build and Test** (`.github/workflows/build-test.yml`)
- Triggers on: `push` to main/develop, `pull_request`
- Steps:
  - Restore dependencies
  - Build in Release mode
  - Run all unit tests
  - Upload test results
  - Publish code coverage

#### 2. **Code Quality** (`.github/workflows/code-quality.yml`)
- Triggers on: `push` to main/develop, `pull_request`
- Steps:
  - Static code analysis
  - SonarCloud integration (optional)
  - Code style checks

#### 3. **Deploy to Production** (`.github/workflows/deploy-production.yml`)
- Triggers on: `push` to main branch, version tags (v*)
- Steps:
  - Build and test
  - Publish release artifacts
  - Generate Docker image (optional)
  - Upload build artifacts

### 📋 Local CI/CD Simulation

```bash
# Build
dotnet build --configuration Release

# Run tests
dotnet test --configuration Release

# Publish
dotnet publish -c Release -o ./release-output
```

---

## Roadmap

### ✅ Completed Features

- [x] 🔐 Authentication/Authorization (JWT)
- [x] ✅ Input Validation (FluentValidation)
- [x] ❤️ Health Checks
- [x] 🏗️ Dependency Injection with Interface Abstraction
- [x] 🧪 Comprehensive Unit Tests
- [x] 🚀 CI/CD Pipeline

### 🎯 Future Enhancements

- [ ] 🗄️ Database query optimization
- [ ] 🔄 Distributed caching (Redis)
- [ ] 🚦 Rate limiting
- [ ] 📊 API usage analytics
- [ ] 📦 Batch operations
- [ ] 📤 Export to CSV/Excel
- [ ] 🔍 Advanced filtering and sorting
- [ ] 🌐 Internationalization (i18n)
- [ ] 📱 GraphQL support
- [ ] 🐳 Docker containerization
- [ ] ☸️ Kubernetes deployment manifests
- [ ] 📈 Prometheus metrics integration

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
