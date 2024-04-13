using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StudentInformationManagementSystem.Controllers;
using StudentInformationManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using StudentInformationManagementSystem.Data;

namespace StudentInformationManagementSystem.Tests.Controllers
{
    public class StudentsControllerTests
    {
        [Fact]
        public async Task DeleteStudent_SuccessfulDeletion()
        {
            // Arrange
            var mockContext = new Mock<SchoolManagementDbContext>();
            var mockDbSet = new Mock<DbSet<Student>>();
            var existingStudent = new Student { Id = 1, FirstName = "John", LastName = "Doe" };

            mockContext.Setup(c => c.Students).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.Students.FindAsync(1)).ReturnsAsync(existingStudent);

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var controller = new StudentsController(mockContext.Object, mockWebHostEnvironment.Object);

            // Act
            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming you redirect to "Index" action after successful deletion

            mockDbSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Once);
        }
       // [Fact]
        //public async Task DeleteNonExistentStudent_HandleGracefully()
       // {
            // Arrange
           // var mockContext = new Mock<SchoolManagementDbContext>();
            //var mockDbSet = new Mock<DbSet<Student>>();

           // mockContext.Setup(c => c.Students).Returns(mockDbSet.Object);
           // mockContext.Setup(c => c.Students.FindAsync(It.IsAny<int>())).ReturnsAsync((Student)null); // Simulate non-existent student

           // var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

           // var controller = new StudentsController(mockContext.Object, mockWebHostEnvironment.Object);

            // Act
           // var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
           // Assert.NotNull(result);
            //Assert.Equal("Index", result.ActionName); // Assuming you redirect to "Index" action after attempting to delete non-existent record
        //}

        [Theory]
        [InlineData(1)] // Test case for existing student
        
        public async Task DeleteStudent_HandleGracefully(int studentId)
        {
            // Arrange
            var mockContext = new Mock<SchoolManagementDbContext>();
            var mockDbSet = new Mock<DbSet<Student>>();

            var existingStudent = studentId == 1 ? new Student { Id = 1, FirstName = "John", LastName = "Doe" } : null;
            mockContext.Setup(c => c.Students).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.Students.FindAsync(studentId)).ReturnsAsync(existingStudent);

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var controller = new StudentsController(mockContext.Object, mockWebHostEnvironment.Object);

            // Act
            var result = await controller.DeleteConfirmed(studentId) as IActionResult;

            // Assert
            if (existingStudent != null)
            {
                Assert.IsType<RedirectToActionResult>(result);
                var redirectResult = result as RedirectToActionResult;
                Assert.Equal("Index", redirectResult.ActionName); // Assuming you redirect to "Index" action after deleting an existing record
            }
            else
            {
                Assert.IsType<NotFoundResult>(result); // Non-existent student should return NotFoundResult
            }
        }
    }

}
