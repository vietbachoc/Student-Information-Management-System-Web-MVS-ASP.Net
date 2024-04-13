using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudentInformationManagementSystem.Controllers;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;
using Xunit;

namespace StudentInformationManagementSystem.Tests
{
    public class StudentsControllerTests
    {
        [Fact]
        public async Task Create_ValidStudentInformation_Success()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            using var dbContext = new SchoolManagementDbContext(dbContextOptions);
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();

            var controller = new StudentsController(dbContext, mockHostingEnvironment.Object);

            var studentViewModel = new StudentsViewsModel
            {
                firstName = "John",
                lastName = "Doe",
                gender = "Male",
                DoB = DateTime.Now.Date,
                email = "john.doe@example.com",
                address = "123 Street, City",
                photo = null ,
            };

            // Act
            var result = await controller.Create(studentViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming it redirects to the Index action
        }
      
    }

}
