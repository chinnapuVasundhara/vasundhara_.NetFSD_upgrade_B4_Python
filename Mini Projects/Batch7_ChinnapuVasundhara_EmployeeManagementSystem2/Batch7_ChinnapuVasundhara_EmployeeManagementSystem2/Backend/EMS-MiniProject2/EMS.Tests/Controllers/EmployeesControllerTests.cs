using EMS.API.Controllers;
using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EMS.Tests.Controllers
{
    [TestFixture]
    public class EmployeesControllerTests
    {
        private Mock<IEmployeeRepository> _repoMock;
        private EmployeeService _service;
        private EmployeesController _controller;

        [SetUp]
        public void Setup()
        {
            // 1. Mock the Data Access Layer
            _repoMock = new Mock<IEmployeeRepository>();

            // 2. Inject mocked repo into the real Business Logic Layer
            _service = new EmployeeService(_repoMock.Object);

            // 3. Inject the service into the API Controller
            _controller = new EmployeesController(_service);
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsOkResultWithEmployee()
        {
            // Arrange
            var fakeEmployee = new Employee
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@domain.com",
                Department = "IT",
                Designation = "Developer",
                JoinDate = DateTime.UtcNow,
                Status = "Active"
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fakeEmployee);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.InstanceOf<EmployeeResponseDto>());

            var dto = okResult.Value as EmployeeResponseDto;
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.FirstName, Is.EqualTo("Test"));
        }

        [Test]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange - Repo returns null when an employee isn't found
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var fakeEmployee = new Employee { Id = 5, Email = "del@test.com" };

            // Setup GetById to return the employee so the delete logic proceeds
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(fakeEmployee);

            // Setup DeleteAsync to just return a completed task
            _repoMock.Setup(r => r.DeleteAsync(fakeEmployee)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(5);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Test]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            // Verify DeleteAsync was NEVER called because the ID wasn't found
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Employee>()), Times.Never);
        }
    }
}