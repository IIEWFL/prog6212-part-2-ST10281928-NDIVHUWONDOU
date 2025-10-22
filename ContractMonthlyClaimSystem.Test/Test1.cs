using ContractMonthlyClaimSystem.Controllers;
using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem.Test.Controllers
{
    [TestClass]
    public class LecturerControllerTests
    {
        private ApplicationDbContext _context;
        private LecturerController _controller;
        private Mock<IWebHostEnvironment> _envMock;

        [TestInitialize]
        public void Setup()
        {
            // 👇 Each test uses its own unique in-memory DB
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock the hosting environment (for file uploads)
            _envMock = new Mock<IWebHostEnvironment>();
            _envMock.Setup(e => e.WebRootPath).Returns(Directory.GetCurrentDirectory());

            _controller = new LecturerController(_context, _envMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // 🧪 INDEX TEST
        [TestMethod]
        public void Index_ReturnsViewWithClaims()
        {
            // Arrange
            _context.Claims.Add(new Claim { Id = 1, LecturerName = "Dr. Smith", Module = "Maths", HourlyRate = 300, HoursWorked = 10 });
            _context.SaveChanges();

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Claim>;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Dr. Smith", model.First().LecturerName);
        }

        // 🧪 DETAILS TESTS
        [TestMethod]
        public void Details_ReturnsNotFound_WhenClaimNotExists()
        {
            var result = _controller.Details(999);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Details_ReturnsView_WhenClaimExists()
        {
            _context.Claims.Add(new Claim { Id = 2, LecturerName = "Prof. John", Module = "Prog6212" });
            _context.SaveChanges();

            var result = _controller.Details(2) as ViewResult;

            Assert.IsNotNull(result);
            var model = result.Model as Claim;
            Assert.AreEqual("Prof. John", model.LecturerName);
        }

        // 🧪 CREATE TEST
        [TestMethod]
        public async Task Create_AddsClaimAndRedirects()
        {
            // Arrange
            var claim = new Claim { LecturerName = "Dr. Lee", Module = "AI", HoursWorked = 5, HourlyRate = 200 };
            IFormFile file = null; // No file upload

            // Act
            var result = await _controller.Create(claim, file) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, _context.Claims.Count());
            Assert.AreEqual("Dr. Lee", _context.Claims.First().LecturerName);
        }

        // 🧪 EDIT TEST
        [TestMethod]
        public async Task Edit_UpdatesClaimAndRedirects()
        {
            // Arrange
            _context.Claims.Add(new Claim { Id = 5, LecturerName = "Old Name", Module = "Old Module" });
            _context.SaveChanges();

            var updated = new Claim
            {
                Id = 5,
                LecturerName = "New Name",
                Module = "New Module",
                HoursWorked = 10,
                HourlyRate = 100,
                Status = "Approved"
            };

            // Act
            var result = await _controller.Edit(updated, null) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var edited = _context.Claims.First(c => c.Id == 5);
            Assert.AreEqual("New Name", edited.LecturerName);
            Assert.AreEqual("New Module", edited.Module);
            Assert.AreEqual("Approved", edited.Status);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesClaim_AndRedirects()
        {
            _context.Claims.Add(new Models.Claim { Id = 3, LecturerName = "Dr. ToDelete", Module = "DeleteTest" });
            _context.SaveChanges();

            var result = await _controller.DeleteConfirmed(3) as RedirectToActionResult;

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(0, _context.Claims.Count());
        }



    }
}
