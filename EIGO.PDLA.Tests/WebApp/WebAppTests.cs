using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace EIGO.PDLA.Tests.WebApp;

[TestFixture]
public class WebAPPTests
{
    private readonly HttpContext httpContext = new DefaultHttpContext
    {
        TraceIdentifier = "Test"
    };

        private readonly Mock<IHttpContextAccessor> httpContextAccessor = new();

        [SetUp]
        public void Setup()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void HomeController()
        {
            var options = new DbContextOptionsBuilder<DeclaracionesContext>()
                .UseInMemoryDatabase(databaseName: "DeclaracionesDb")
                .Options;
            using var context = new DeclaracionesContext(httpContextAccessor.Object, options);
            HomeController homeController = new(httpContextAccessor.Object, new NullLogger<HomeController>(), context)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

        Assert.IsNotNull(homeController);

        IActionResult index = homeController.Index();
        Assert.IsNotNull(index);

        IActionResult privacy = homeController.Privacy();
        Assert.IsNotNull(privacy);

        IActionResult error = homeController.Error();
        Assert.IsNotNull(error);
    }
}