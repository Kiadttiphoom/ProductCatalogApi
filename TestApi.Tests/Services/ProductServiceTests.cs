using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Models.Entities;
using ProductCatalogApi.Services;
using Xunit;

namespace TestApi.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly IMemoryCache _cache;
    private readonly Mock<ILogger<ProductService>> _mockLogger;
    private readonly ProductService _service;

    /// <summary>
    /// เตรียมความพร้อมสำหรับแต่ละ Test Case
    /// </summary>
    public ProductServiceTests()
    {
        // สร้าง Mock สำหรับ Repository และ Logger เพื่อจำลองการทำงาน
        _mockRepository = new Mock<IProductRepository>();
        _cache = new MemoryCache(new MemoryCacheOptions()); // ใช้ MemoryCache จริงในการทดสอบ
        _mockLogger = new Mock<ILogger<ProductService>>();
        
        // สร้าง instance ของ ProductService โดยส่ง Mock และ Cache เข้าไป
        _service = new ProductService(_mockRepository.Object, _cache, _mockLogger.Object);
    }

    /// <summary>
    /// กรณีทดสอบ: เมื่อไม่มีข้อมูลใน Cache ระบบต้องไปดึงข้อมูลจาก Repository (Database)
    /// </summary>
    [Fact]
    public async Task GetProductsAsync_WithNoCachedData_ShouldReturnProductsFromRepository()
    {
        // 1. Arrange: เตรียมข้อมูลจำลอง
        var products = new List<Product>
        {
            new Product { ModID = "1", ModName = "Product 1", PriceWS = 100 },
            new Product { ModID = "2", ModName = "Product 2", PriceWS = 200 }
        };

        // ตั้งค่าให้ Repository จำลองส่งข้อมูลกลับมาเป็นรายการด้านบน
        _mockRepository.Setup(r => r.GetAllProductsAsync())
            .ReturnsAsync(products);

        // 2. Act: เรียกใช้งาน Method ที่ต้องการทดสอบ
        var result = await _service.GetProductsAsync();

        // 3. Assert: ตรวจสอบผลลัพธ์
        result.Success.Should().BeTrue(); // ต้องสำเร็จ
        result.Data.Should().HaveCount(2); // ต้องมีข้อมูล 2 รายการ
        result.Data![0].ModName.Should().Be("Product 1"); // ข้อมูลตัวแรกต้องตรง
        
        // ตรวจสอบว่ามีการเรียกใช้ Repository จริงๆ 1 ครั้ง
        _mockRepository.Verify(r => r.GetAllProductsAsync(), Times.Once);
    }

    /// <summary>
    /// กรณีทดสอบ: เมื่อมีข้อมูลใน Cache อยู่แล้ว ระบบต้องดึงจาก Cache ทันทีโดยไม่ผ่าน Repository
    /// </summary>
    [Fact]
    public async Task GetProductsAsync_WithCachedData_ShouldReturnCachedProducts()
    {
        // 1. Arrange: เตรียมข้อมูลและใส่ไว้ใน Cache ล่วงหน้า
        var cachedProducts = new List<Product>
        {
            new Product { ModID = "1", ModName = "Cached Product", PriceWS = 150 }
        };

        _cache.Set("product_list", cachedProducts);

        // 2. Act: เรียกใช้งาน Method
        var result = await _service.GetProductsAsync();

        // 3. Assert: ตรวจสอบผลลัพธ์
        result.Success.Should().BeTrue();
        result.Data![0].ModName.Should().Be("Cached Product");
        
        // สำคัญ: ต้องไม่มีการเรียกใช้ Repository เลย (เพราะมีใน Cache แล้ว)
        _mockRepository.Verify(r => r.GetAllProductsAsync(), Times.Never);
    }

    /// <summary>
    /// กรณีทดสอบ: เมื่อ Repository เกิด Exception (Error) ระบบต้องส่ง ErrorResponse กลับมาอย่างถูกต้อง
    /// </summary>
    [Fact]
    public async Task GetProductsAsync_WithRepositoryException_ShouldReturnErrorResponse()
    {
        // 1. Arrange: ล้าง Cache และตั้งค่าให้ Repository พ่น Error ออกมา
        _cache.Remove("product_list");

        _mockRepository.Setup(r => r.GetAllProductsAsync())
            .ThrowsAsync(new Exception("Database error"));

        // 2. Act: เรียกใช้งาน Method
        var result = await _service.GetProductsAsync();

        // 3. Assert: ตรวจสอบผลลัพธ์ Error
        result.Success.Should().BeFalse(); // ต้องไม่สำเร็จ
        result.Message.Should().Contain("Error"); // ต้องมีคำว่า Error ในข้อความแจ้งเตือน
    }
}
