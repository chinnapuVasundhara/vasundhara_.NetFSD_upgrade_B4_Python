using EMS.API.Data;
using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AppDbContext _db;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            // Set up InMemory DB for testing users
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _db = new AppDbContext(options);

            // Arrange - mock IConfiguration for JWT settings
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("TestSecretKey_32Chars_ForNUnit!!");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("EMS.API");
            mockConfig.Setup(c => c["Jwt:Audience"]).Returns("EMS.Client");
            mockConfig.Setup(c => c["Jwt:ExpiryHours"]).Returns("8");

            _authService = new AuthService(_db, mockConfig.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            _db.AppUsers.Add(new AppUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            });
            await _db.SaveChangesAsync();

            // Act
            var result = await _authService.LoginAsync(new AuthRequestDto { Username = "admin", Password = "admin123" });

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Token, Is.Not.Null.And.Not.Empty); // GenerateToken returns non-empty string
        }

        [Test]
        public async Task LoginAsync_WrongPassword_ReturnsFailure()
        {
            // Arrange
            _db.AppUsers.Add(new AppUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            });
            await _db.SaveChangesAsync();

            // Act
            var result = await _authService.LoginAsync(new AuthRequestDto { Username = "admin", Password = "wrongpassword" });

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid credentials."));
        }

        [Test]
        public async Task RegisterAsync_DuplicateUsername_ReturnsFailure()
        {
            // Arrange
            _db.AppUsers.Add(new AppUser { Username = "existinguser", PasswordHash = "hash", Role = "Viewer" });
            await _db.SaveChangesAsync();

            var request = new AuthRequestDto { Username = "existinguser", Password = "newpassword" };

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username already exists."));
        }
    }
}