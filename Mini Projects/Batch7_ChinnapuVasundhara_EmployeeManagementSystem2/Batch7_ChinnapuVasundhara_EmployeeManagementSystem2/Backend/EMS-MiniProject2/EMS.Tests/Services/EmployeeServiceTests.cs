using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Moq;
using NUnit.Framework;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _repoMock;
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repoMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsMappedDto()
        {
            // Arrange
            var fakeEmployee = new Employee
            {
                Id = 1,
                FirstName = "Priya",
                LastName = "Prabhu",
                Email = "p@h.com",
                Status = "Active",
                JoinDate = new DateTime(2023, 1, 1)
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(fakeEmployee);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FirstName, Is.EqualTo("Priya"));
            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once); // Confirms mock interaction
        }

        [Test]
        public async Task GetByIdAsync_NonExistentId_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(9999)).ReturnsAsync((Employee?)null);

            // Act
            var result = await _service.GetByIdAsync(9999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_CallsAddAsyncOnRepo()
        {
            // Arrange
            var newDto = new EmployeeRequestDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@user.com",
                Phone = "1234567890",
                Department = "IT",
                Designation = "Dev",
                Salary = 50000,
                JoinDate = DateTime.UtcNow,
                Status = "Active"
            };

            // Act
            var result = await _service.CreateAsync(newDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("test@user.com"));
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}