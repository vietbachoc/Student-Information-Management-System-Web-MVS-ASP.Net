using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudentInformationManagementSystem.Controllers;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using StudentInformationManagementSystem.Views;
using Xunit;

namespace StudentInformationManagementSystem.Tests
{
    public class ClassesControllerTests
    {
        [Fact]
        public async Task Create_ValidClass_ReturnsRedirectToActionResult()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            using var dbContext = new SchoolManagementDbContext(dbContextOptions);
            var notyfServiceMock = new Mock<INotyfService>();

            var controller = new ClassesController(dbContext, notyfServiceMock.Object);

            var classModel = new Class
            {
                LecturerId = 1, // Replace with existing lecturer ID from the test database
                CourseId = 1, // Replace with existing course ID from the test database
                Time = TimeSpan.FromHours(10) // Example time, replace with desired time
            };

            // Act
            var result = await controller.Create(classModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming it redirects to the Index action
        }
    }

}
