<div align="center">

# 🛒 ProductCatalogApi

### A Modern REST API for Product Management

**Built with .NET 8.0 | ASP.NET Core | Dapper | SQL Server**

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue.svg)](https://docs.microsoft.com/aspnet/core)

</div>

---

## 📋 Overview
ระบบ API สำหรับจัดการข้อมูลสินค้า จังหวัด และอำเภอ โดยออกแบบด้วยสถาปัตยกรรมที่แยกส่วนชัดเจน (Clean Architecture) เน้นประสิทธิภาพด้วยระบบ Caching และความปลอดภัยด้วย API Key

## ✨ Key Features
- 🚀 **High Performance**: ใช้ Dapper ORM และ Memory Caching เพื่อการตอบสนองที่รวดเร็ว
- 🔐 **Security**: ระบบตรวจสอบสิทธิ์ด้วย API Key (`X-API-KEY`)
- 📝 **Structured Logging**: บันทึก Log อย่างเป็นระบบด้วย Serilog (Console & File)
- ✅ **Validation**: ตรวจสอบความถูกต้องของข้อมูลด้วย FluentValidation
- 🩺 **Health Checks**: ระบบตรวจสอบสถานะความพร้อมของ API และ Database
- 🧪 **Unit Testing**: ครอบคลุมการทดสอบด้วย xUnit, Moq และ FluentAssertions

## 🏗️ Project Structure
```
├── Controllers/       # API Handlers
├── Services/          # Business Logic & Caching
├── Repositories/      # Data Access Layer (Dapper)
├── Middleware/        # ApiKey & Error Handling
├── Models/Entities/   # Database Models
├── DTOs/              # Request/Response Models
├── Validators/        # Validation Rules
├── Extensions/        # Dependency Injection Config
└── Routes/            # Minimal API Route Definitions
```

## 🛣️ API Endpoints

### Products
- `GET /api/products` - รายการสินค้า (รองรับ Search & Filter)
- `GET /api/province` - รายการจังหวัด (Cached 30m)
- `GET /api/district` - รายการอำเภอตามจังหวัด (Cached 30m)

## ⚙️ Configuration

### appsettings.json (Template)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
  },
  "Authentication": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

> [!IMPORTANT]
> กรุณาใช้ไฟล์ `appsettings.Development.json` สำหรับการตั้งค่าในเครื่องพัฒนา และห้าม Push ค่าจริงขึ้น Git

## 🚀 Getting Started

1. **Restore & Build**:
   ```bash
   dotnet restore
   dotnet build
   ```
2. **Run Application**:
   ```bash
   dotnet run
   ```
3. **Run Tests**:
   ```bash
   dotnet test
   ```

---

<div align="center">
Developed with ❤️ using .NET 8.0
</div>
