using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EIGO.PDLA.Tests.Common;

public class LoggerTests
{
    private readonly HttpContext httpContextEmpty = new DefaultHttpContext
    {
    };

    private readonly HttpContext httpContextFull = new DefaultHttpContext
    {
        TraceIdentifier = "Test",
        Connection =
                {
                    RemoteIpAddress = IPAddress.Parse("128.0.0.1")
                },
        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{
            new Claim(ClaimTypes.Name, "Username"),
            new Claim("name","Username Name")
        }))
    };

    private readonly Mock<IHttpContextAccessor> httpContextAccessorEmpty = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorFull = new();
    private readonly DbContextOptions<DeclaracionesContext> optionsEmpty = new DbContextOptionsBuilder<DeclaracionesContext>()
            .UseInMemoryDatabase(databaseName: "DeclaracionesLoggerTestDbEmpty")
            .Options;
    private readonly DbContextOptions<DeclaracionesContext> optionsFull = new DbContextOptionsBuilder<DeclaracionesContext>()
            .UseInMemoryDatabase(databaseName: "DeclaracionesLoggerTestDbFull")
            .Options;
    private readonly DeclaracionesContext contextEmpty;
    private readonly DeclaracionesContext contextFull;

    public LoggerTests()
    {
        httpContextAccessorEmpty.Setup(ctx => ctx.HttpContext).Returns(httpContextEmpty);
        httpContextAccessorFull.Setup(ctx => ctx.HttpContext).Returns(httpContextFull);
        contextEmpty = new DeclaracionesContext(httpContextAccessorEmpty.Object, optionsEmpty);
        contextFull = new DeclaracionesContext(httpContextAccessorFull.Object, optionsFull);
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        contextEmpty.Dispose();
        contextFull.Dispose();
    }

    [Test]
    public async Task TestLoggerEmpty()
    {
        IPdlaLogger logger = new PdlaLogger(contextEmpty, httpContextAccessorEmpty.Object);
        Assert.IsNotNull(logger);

        Assert.AreEqual(0, await contextEmpty.Auditoria.CountAsync());

        await logger.LogAsync(1, "Evento", "Descripcion", "Resultado", TipoAuditoria.Negocio);

        Assert.AreEqual(1, await contextEmpty.Auditoria.CountAsync());
        var entry = await contextEmpty.Auditoria.FirstOrDefaultAsync();
        Assert.IsNotNull(entry);
        if (entry != null)
        {
            Assert.AreEqual(1, entry.IdProceso);
            Assert.AreEqual("Evento", entry.Evento);
            Assert.AreEqual("Descripcion", entry.Descripcion);
            Assert.AreEqual("Resultado", entry.Resultado);
            Assert.AreEqual("No determinado", entry.Ip);
            Assert.AreEqual("No determinado", entry.IdUsuario);
            Assert.AreEqual("No determinado", entry.Usuario);
        }
        else
        {
            throw new Exception("Esto no debería ser nulo");
        }
    }

    [Test]
    public async Task TestLoggerFull()
    {
        IPdlaLogger logger = new PdlaLogger(contextFull, httpContextAccessorFull.Object);
        Assert.IsNotNull(logger);

        Assert.AreEqual(0, await contextFull.Auditoria.CountAsync());

        await logger.LogAsync(1, "Evento", "Descripcion", "Resultado", TipoAuditoria.Negocio);

        Assert.AreEqual(1, await contextFull.Auditoria.CountAsync());
        var entry = await contextFull.Auditoria.FirstOrDefaultAsync();
        Assert.IsNotNull(entry);
        if (entry != null)
        {
            Assert.AreEqual(1, entry.IdProceso);
            Assert.AreEqual("Evento", entry.Evento);
            Assert.AreEqual("Descripcion", entry.Descripcion);
            Assert.AreEqual("Resultado", entry.Resultado);
            Assert.AreEqual("128.0.0.1", entry.Ip);
            Assert.AreEqual("Username", entry.IdUsuario);
            Assert.AreEqual("Username Name", entry.Usuario);
        }
        else
        {
            throw new Exception("Esto no debería ser nulo");
        }
    }
}
