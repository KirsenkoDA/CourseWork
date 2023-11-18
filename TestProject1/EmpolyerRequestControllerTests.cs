using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork2.Controllers;
using CourseWork2.Data;
using CourseWork2.Models;
using Microsoft.AspNetCore.Identity;
using EmploymentAgency.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TestProject1
{
    public class EmployerRequestControllerTests
    {
        [Test]
        public async Task Create_ValidModelState_ReturnsRedirect()
        {
            // Arrange
            var mockIdentityUser = new Mock<IdentityUser>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "aspnet-CourseWork2-5004d21e-6d95-4c23-8121-9556ec6ed09f")
                .Options;
            var context = new ApplicationDbContext(options);

            var controller = new ResumesController(context, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "TestUser")
                    }))
                }
            };

            var resume = new Resume
            {
                Id = 1,
                UserId = "1",
                DateCreated = DateTime.Now,
                Post = "Software Developer",
                Info = "Looking for a skilled developer",
                EducationId = 1,
                Salary = 50000,
                StatusId = 1
            };

            // Act
            var result = await controller.Create(resume) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            var mockIdentityUser = new Mock<IdentityUser>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;
            var context = new ApplicationDbContext(options);

            var controller = new ResumesController(context, mockUserManager.Object);
            controller.ModelState.AddModelError("error", "ModelState is invalid");

            var resume = new Resume
            {
                Id = 1,
                UserId = "1",
                DateCreated = DateTime.Now,
                Post = "Software Developer",
                Info = "Looking for a skilled developer",
                EducationId = 1,
                Salary = 50000,
                StatusId = 1
            };

            // Act
            var result = await controller.Create(resume) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
        }
    }
}
