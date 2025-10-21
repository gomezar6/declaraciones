using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EIGO.PDLA.Tests.Common
{
    [TestFixture]
    public class DataTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor = new();
        private readonly DbContextOptions<DeclaracionesContext> options = new DbContextOptionsBuilder<DeclaracionesContext>()
                .UseInMemoryDatabase(databaseName: "DeclaracioneDataTestDb")
                .Options;
        private readonly DateTime FechaCreado = new(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private readonly DateTime FechaModificado = new(2019, 2, 2, 0, 0, 0, DateTimeKind.Utc);
        private readonly string TextoCorto = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit";
        private readonly string TextoLargo = @"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna. Nunc viverra imperdiet enim. Fusce est.
Vivamus a tellus. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci. Aenean nec lorem.
In porttitor. Donec laoreet nonummy augue. Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.
Fusce aliquet pede non pede. Suspendisse dapibus lorem pellentesque magna. Integer nulla. Donec blandit feugiat ligula. Donec hendrerit, felis et imperdiet euismod, purus ipsum pretium metus, in lacinia nulla nisl eget sapien.
Donec ut est in lectus consequat consequat. Etiam eget dui. Aliquam erat volutpat. Sed at lorem in nunc porta tristique. Proin nec augue.
Quisque aliquam tempor magna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nunc ac magna. Maecenas odio dolor, vulputate vel, auctor ac, accumsan id, felis. Pellentesque cursus sagittis felis.
Pellentesque porttitor, velit lacinia egestas auctor, diam eros tempus arcu, nec vulputate augue magna vel risus. Cras non magna vel ante adipiscing rhoncus. Vivamus a mi. Morbi neque. Aliquam erat volutpat.
Integer ultrices lobortis eros. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin semper, ante vitae sollicitudin posuere, metus quam iaculis nibh, vitae scelerisque nunc massa eget pede. Sed velit urna, interdum vel, ultricies vel, faucibus at, quam. Donec elit est, consectetuer eget, consequat quis, tempus quis, wisi.
In in nunc. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos hymenaeos. Donec ullamcorper fringilla eros. Fusce in sapien eu purus dapibus commodo. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
Cras faucibus condimentum odio. Sed ac ligula. Aliquam at eros. Etiam at ligula et tellus ullamcorper ultrices. In fermentum, lorem non cursus porttitor, diam urna accumsan lacus, sed interdum wisi nibh nec nisl.
";
        private readonly DeclaracionesContext context;
        private readonly AlertaRepository alertaRepository;

        public DataTests()
        {
            context = new DeclaracionesContext(httpContextAccessor.Object, options);
            alertaRepository = new AlertaRepository(context);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Dispose();
        }

        [Test]
        public async Task AlertaAsync()
        {
            Proceso proceso = new() { NombreProceso = "Prueba" };
            context.Add(proceso);
            context.SaveChanges();

            Alerta alerta = new()
            {
                Asunto = "Asunto",
                AvisoConfidencialidad = TextoLargo,
                IdAlerta = 0,
                IdProceso = 1,
                Creado = FechaCreado,
                CreadoPor = "Creado",
                Cuerpo = TextoLargo,
                Diligenciado = false,
                Eliminado = false,
                Estatus = true,
                Fecha = FechaModificado,
                Modificado = FechaModificado,
                ModificadoPor = "Modificado",
                SubTitulo = TextoCorto,
                Titulo = TextoCorto,
            };

            Assert.IsNotNull(alerta);

            _ = await alertaRepository.AddAsync(alerta);

            var a0 = await alertaRepository.GetByIdAsync(1);
            Assert.AreEqual(a0, alerta);
            Assert.AreEqual(false, a0?.Diligenciado);
            Assert.AreEqual(true, a0?.Estatus);
            Assert.AreEqual(TextoLargo, a0?.Cuerpo);
            Assert.AreEqual(FechaModificado, a0?.Fecha);
            Assert.AreEqual(1, a0?.IdProceso);
            Assert.AreEqual(proceso, a0?.IdProcesoNavigation);
            Assert.AreEqual(TextoCorto, a0?.Titulo);
            Assert.AreEqual(TextoCorto, a0?.SubTitulo);

            alerta.Asunto = "Asunto 2";
            _ = await alertaRepository.UpdateAsync(alerta);

            a0 = await alertaRepository.GetByIdAsync(1);
            Assert.AreEqual(a0, alerta);
        }

        [Test]
        public void Data()
        {
            Proceso proceso = new();
            Assert.IsNotNull(proceso);

            Alerta alerta = new();
            Assert.IsNotNull(alerta);

            alerta = new Alerta
            {
                Asunto = "Asunto",
                AvisoConfidencialidad = "Texto",
                IdAlerta = 99,
                Creado = new DateTime(2000, 1, 1, 0, 0, 0),
                CreadoPor = "Creado",
                Cuerpo = "Cuerpo",
                Diligenciado = false,
                Eliminado = false,
                Estatus = true,
                Fecha = new DateTime(2001, 1, 1, 0, 0, 0),
                IdProceso = proceso.IdProceso,
                IdProcesoNavigation = proceso,
                Modificado = new DateTime(2002, 1, 1, 0, 0, 0),
                ModificadoPor = "Modificado",
                SubTitulo = "SubTitulo",
                Titulo = "Titulo"
            };

            Assert.AreEqual("Asunto", alerta.Asunto);
            Assert.AreEqual("Texto", alerta.AvisoConfidencialidad);
            Assert.AreEqual(99, alerta.IdAlerta);
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 0, 0), alerta.Creado);
        }

        [Test]
        public void AuditableTest()
        {
            Auditable auditable = new()
            {
                Creado = new DateTime(2022, 1, 1, 0, 0, 0),
                CreadoPor = "Creado Por",
                Eliminado = false,
                Modificado = new DateTime(2022, 1, 2, 0, 0, 0),
                ModificadoPor = "Modificado Por"
            };

            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), auditable.Creado);
            Assert.AreEqual("Creado Por", auditable.CreadoPor);
            Assert.IsFalse(auditable.Eliminado);
            Assert.AreEqual(new DateTime(2022, 1, 2, 0, 0, 0), auditable.Modificado);
            Assert.AreEqual("Modificado Por", auditable.ModificadoPor);
        }

        [Test]
        public void CiudadTest()
        {
            Ciudad ciudad = new()
            {
                Creado = new DateTime(2022, 1, 1, 0, 0, 0),
                CreadoPor = "Creado Por",
                Eliminado = false,
                IdCiudad = 1,
                IdPais = 2,
                Declaraciones = new List<Declaracion>(),
                IdPaisNavigation = new Pais() { },
                Modificado = new DateTime(2022, 2, 2, 0, 0, 0),
                ModificadoPor = "Modificado Por",
                NombreCiudad = "Nombre ciudad"
            };

            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), ciudad.Creado);
            Assert.AreEqual("Creado Por", ciudad.CreadoPor);
            Assert.IsFalse(ciudad.Eliminado);
            Assert.AreEqual(1, ciudad.IdCiudad);
            Assert.AreEqual(2, ciudad.IdPais);
            Assert.AreEqual(typeof(List<Declaracion>), ciudad.Declaraciones.GetType());
            Assert.AreEqual(typeof(Pais), ciudad.IdPaisNavigation.GetType());
            Assert.AreEqual(new DateTime(2022, 2, 2, 0, 0, 0), ciudad.Modificado);
            Assert.AreEqual("Modificado Por", ciudad.ModificadoPor);
            Assert.AreEqual("Nombre ciudad", ciudad.NombreCiudad);
        }

        [Test]
        public void DeclaracionTest()
        {
            Declaracion declaracion = new()
            {
                Cargo = "Cargo",
                ConfirmacionResponsabilidad = true,
                Creado = new DateTime(2022, 1, 1, 0, 0, 0),
                CreadoPor = "Creado Por",
                Eliminado = false,
                Modificado = new DateTime(2022, 2, 2, 0, 0, 0),
                ModificadoPor = "Modificado Por",
                FechaDeclaracion = new DateTime(2022, 1, 3, 0, 0, 0),
                IdCiudad = 0,
                IdCiudadNavigation = new Ciudad(),
                IdDeclaracion = 1,
                IdEstadoDeclaracion = 2,
                IdEstadoDeclaracionNavigation = new EstadoDeclaracion(),
                IdFormulario = 3,
                IdFormularioNavigation = new Formulario(),
                IdFuncionario = 4,
                IdFuncionarioNavigation = new Funcionario(),
                UnidadOrganizacional = "Unidad",
                RecibidaEnFisico = true
            };
            Assert.AreEqual("Cargo", declaracion.Cargo);
            Assert.IsTrue(declaracion.ConfirmacionResponsabilidad);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), declaracion.Creado);
            Assert.AreEqual("Creado Por", declaracion.CreadoPor);
            Assert.IsFalse(declaracion.Eliminado);
            Assert.AreEqual(new DateTime(2022, 2, 2, 0, 0, 0), declaracion.Modificado);
            Assert.AreEqual("Modificado Por", declaracion.ModificadoPor);
            Assert.AreEqual(0, declaracion.IdCiudad);
            Assert.AreEqual(typeof(Ciudad), declaracion.IdCiudadNavigation.GetType());
            Assert.AreEqual(1, declaracion.IdDeclaracion);
            Assert.AreEqual(2, declaracion.IdEstadoDeclaracion);
            Assert.AreEqual(typeof(EstadoDeclaracion), declaracion.IdEstadoDeclaracionNavigation.GetType());
            Assert.AreEqual(3, declaracion.IdFormulario);
            Assert.AreEqual(typeof(Formulario), declaracion.IdFormularioNavigation.GetType());
            Assert.AreEqual(4, declaracion.IdFuncionario);
            Assert.AreEqual(typeof(Funcionario), declaracion.IdFuncionarioNavigation.GetType());
            Assert.AreEqual("Unidad", declaracion.UnidadOrganizacional);
            Assert.IsTrue(declaracion.RecibidaEnFisico);
        }
    }
}
