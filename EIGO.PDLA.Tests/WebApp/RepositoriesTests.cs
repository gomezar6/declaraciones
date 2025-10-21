#nullable disable
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EIGO.PDLA.Tests.WebApp
{
    [TestFixture]
    public class RepositoriesTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor = new();
        private readonly DbContextOptions<DeclaracionesContext> options = new DbContextOptionsBuilder<DeclaracionesContext>()
                .UseInMemoryDatabase(databaseName: "DeclaracionesRepositoriesTestsDb")
                .Options;
        private DeclaracionesContext context;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new DeclaracionesContext(httpContextAccessor.Object, options);
            context.EstadoProcesos.Add(new EstadoProceso
            {
                Creado = DateTime.UtcNow,
                CreadoPor = "Creado por",
                Eliminado = false,
                IdEstadoProceso = 1,
                Modificado = DateTime.UtcNow,
                ModificadoPor = "Modificador por",
                NombreEstadoProceso = "Estado proceso 1"
            });
            context.SaveChanges();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Dispose();
        }

        [Test]
        public async Task EstadoProceso()
        {
            EstadoProcesoRepository estadoProcesoRepository = new(context);
            Assert.IsNotNull(estadoProcesoRepository);

            var existe = await estadoProcesoRepository.Exist(2);
            Assert.IsFalse(existe);

            var elementos = await estadoProcesoRepository.GetAllAsync();
            Assert.IsNotNull(elementos);
            Assert.AreEqual(1, elementos.Count);

            EstadoProceso ep = new() { IdEstadoProceso = 1, NombreEstadoProceso = "Estado proceso 1" };

            Assert.CatchAsync<NotImplementedException>(async () => await estadoProcesoRepository.AddAsync(ep));
            Assert.CatchAsync<NotImplementedException>(async () => await estadoProcesoRepository.UpdateAsync(ep));
            Assert.CatchAsync<NotImplementedException>(async () => await estadoProcesoRepository.DeleteAsync(ep));

            EstadoProceso testep = await estadoProcesoRepository.GetByIdAsync(1);
            Assert.IsNotNull(testep);
            Assert.AreEqual(ep.NombreEstadoProceso, testep.NombreEstadoProceso);

            Assert.IsTrue(await estadoProcesoRepository.Exist(1));
        }


        [Test]
        public async Task ProcesoRepository()
        {
            ProcesoRepository procesoRepository = new(context);

            Proceso proceso = new()
            {
                Creado = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreadoPor = "CreadoPor",
                Eliminado = false,
                FechaFin = new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                FechaInicio = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                IdEstadoProceso = 1,
                IdProceso = 0,
                Modificado = new DateTime(2020, 1, 1, 0, 1, 0, DateTimeKind.Utc),
                ModificadoPor = "ModificadoPor",
                NombreProceso = "Proceso1",
                Observaciones = "Observaciones"
            };

            var p0 = await procesoRepository.AddAsync(proceso);
            Assert.AreEqual(proceso, p0);

            p0 = await procesoRepository.GetByIdAsync(1);
            Assert.AreEqual(proceso, p0);

            proceso.NombreProceso = "Proceso1Modificado";
            _ = await procesoRepository.UpdateAsync(proceso);

            p0 = await procesoRepository.GetByIdAsync(1);
            Assert.AreEqual(proceso, p0);

            p0.IdProceso = 0;
            await procesoRepository.AddAsync(p0);
            Assert.IsTrue(await procesoRepository.Exist(2));
            //TODO Validate more every parameters
        }

        public void AlertaRepository()
        {
            AlertaRepository alertaRepository = new(context);

            Assert.IsNotNull(alertaRepository);

        }
    }
}
