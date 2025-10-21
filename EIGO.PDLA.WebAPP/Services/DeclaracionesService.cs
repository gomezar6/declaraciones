using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.WebAPP.Services
{
    public interface IDeclaracionesService
    {
        Task GenerarAsync(DeclaracionDTO declaracion, IFormCollection form, List<CatalogoAnios> catalogoAnios);
        Task ActualizarNacionalidadAsync(DeclaracionNacionalidadAjaxDTO DeclaracionNacionalidad);
    }
    public class DeclaracionesService : IDeclaracionesService
    {
        private readonly DeclaracionesContext _context;
        private readonly IPdlaLogger _logger;

        public DeclaracionesService(DeclaracionesContext context, IPdlaLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task GenerarAsync(DeclaracionDTO declaracion,
            IFormCollection form, List<CatalogoAnios> catalogoAnios)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var declaracionInsertada = _context.Declaraciones.Update(declaracion.ToDeclaracion());
                var formulario = await _context.Formularios.FirstOrDefaultAsync(f => f.IdFormulario == declaracion.IdDeclaracion);
                declaracion.Formulario = (await _context.Formularios.FirstOrDefaultAsync(d => d.IdFormulario == declaracion.IdFormulario))?.ToFormularioDTO();
                declaracion.EstadoDeclaracion = (await _context.EstadoDeclaraciones.FirstOrDefaultAsync(ed => ed.IdEstado == 2))?.ToEstadoDeclaracionDTO();
              //  await _logger.LogAsync(declaracion.Formulario.IdProceso, "Diligenciar Declaracion " + declaracion.Formulario.Titulo, declaracion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);

             //   await _logger.LogAsync(declaracion.Formulario?.IdProceso, " Diligenciar Declaracion " + declaracion.Formulario.Titulo, declaracion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);

                for (int i = 0; i <= form.Count; i++)
                {
                    var IdFamiliar = form["NombreCompleto[" + i + "]"];
                    var IdParticipacion = form["IdParticipacion[" + i + "]"];
                    var IdDeclaracion = declaracion.IdDeclaracion;
                    var IdPais = form["Pais[" + i + "]"];
                    var PorcentajeParticipacion = form["PorcentajeParticipacion[" + i + "]"];
                    var NombreEmpresa = form["NombreEmpresa[" + i + "]"];
                    var CargoParticipacion = form["CargoParticipacion[" + i + "]"];
                    var Parentesco = form["Parentesco[" + i + "]"];
                    var NombreFamiliar = form["NombreCompleto[" + i + "]"];
                    var bOtro = form["bOtro[" + i + "]"];
                    var mes = form["Mes[" + i + "]"];
                    var anio = form["Anio[" + i + "]"];
                    var ntipoCargo = form["ntipoCargo[" + i + "]"];


                    if (NombreFamiliar == "-1")
                    {
                        NombreFamiliar = form["NuevaParticipacionNombre[" + i + "]"];
                    }
                    if (PorcentajeParticipacion == "" || PorcentajeParticipacion.Count == 0)
                    {
                        PorcentajeParticipacion = "0";
                    }
                    if (!string.IsNullOrEmpty(IdPais))
                    {
                        if (IdParticipacion == "" || IdParticipacion.Count == 0)
                        {
                            int.TryParse(IdParticipacion, out int IdParticipacionint);
                            bool existParticipacion = await _context.Participaciones.AnyAsync(p => p.IdParticipacion == IdParticipacionint);
                            if (!existParticipacion)
                            {
                                int.TryParse(IdFamiliar, out int intIdFamiliar);
                                int.TryParse(IdPais, out int intIdPais);
                                int.TryParse(Parentesco, out int intIdParentesco);
                                // manejo tipoCargo
                                byte intntipoCargo = 0;
                                if (!string.IsNullOrEmpty(ntipoCargo)) 
                                {
                                    byte.TryParse(ntipoCargo, out intntipoCargo);
                                }
                                // manejo dMesAnio
                                dynamic participacionInsertada;
                                if (!string.IsNullOrEmpty(mes) && mes != "" && mes != "0" && !string.IsNullOrEmpty(anio) && anio != "" && anio != "0") // La primer tabla no lleva valor en el mes y anio
                                {
                                    var dtMesAnio = new DateTime(catalogoAnios[int.Parse(anio) - 1].Anio, int.Parse(mes), 01);

                                    var insertarOtro = false;

                                    if(string.IsNullOrEmpty(bOtro)==  true){
                                        insertarOtro = false;
                                    }
                                    else if(bOtro == true)
                                    {
                                       
                                        insertarOtro = true;

                                    }


                                    participacionInsertada = await _context.Participaciones.AddAsync(new Participacion
                                    {
                                        IdPais = intIdPais,
                                        IdDeclaracion = IdDeclaracion,
                                        NombreEmpresa = NombreEmpresa,
                                        PctAccionario = decimal.Parse(PorcentajeParticipacion),
                                        NombreFamiliar = NombreFamiliar,
                                        Cargo = CargoParticipacion,
                                        IdParentesco = intIdParentesco,
                                        dMesAnioInicio = dtMesAnio,
                                        bOtro = insertarOtro,
                                        ntipoCargo = intntipoCargo
                                    });
                                }
                                else 
                                {
                                    participacionInsertada = await _context.Participaciones.AddAsync(new Participacion
                                    {
                                        IdPais = intIdPais,
                                        IdDeclaracion = IdDeclaracion,
                                        NombreEmpresa = NombreEmpresa,
                                        PctAccionario = decimal.Parse(PorcentajeParticipacion),
                                        NombreFamiliar = NombreFamiliar,
                                        Cargo = CargoParticipacion,
                                        IdParentesco = intIdParentesco,
                                        dMesAnioInicio = null,
                                        bOtro = null,
                                        ntipoCargo = intntipoCargo
                                    });
                                }
                                //
                                //await _logger.LogAsync(declaracion.Formulario.IdProceso, "Registrar Participación Declaración "+declaracion.Formulario.Titulo, participacionInsertada.Entity.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);
                            }

                        }
                        else
                        {
                            int.TryParse(IdFamiliar, out int intIdFamiliar);
                            int.TryParse(IdParticipacion, out int intIdParticipacion);
                            var verificarParticipacion = await _context.Participaciones.AnyAsync(p => p.IdParticipacion == intIdParticipacion && p.IdDeclaracion == IdDeclaracion);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                //await _logger.LogAsync(declaracion.Formulario?.IdProceso, "Declaracion:Creación", declaracion.ToAuditoria(), $"fallido - {ex.Message}", TipoAuditoria.Negocio);
                //await transaction.RollbackAsync();
                //throw;
            }
        }

        public async Task ActualizarNacionalidadAsync(DeclaracionNacionalidadAjaxDTO DeclaracionNacionalidad)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Declaraciones.Update(DeclaracionNacionalidad.declaracion.ToDeclaracion());
                for (int i = 0; i < DeclaracionNacionalidad.NacionalidadesDelete.Count; i++)
                {


                    var validarNacionalidad = await _context.FuncionarioNacionalidad.FirstOrDefaultAsync(ep => ep.IdDeclaracion == DeclaracionNacionalidad.declaracion.IdDeclaracion
                    && ep.Nacionalidad == DeclaracionNacionalidad.NacionalidadesDelete[i] && ep.Eliminado == false);
                    if (validarNacionalidad != null)
                    {

                        FuncionarioNacionalidad funcionarioNacionalidad = new FuncionarioNacionalidad
                        {
                            IdFuncionario = DeclaracionNacionalidad.declaracion.IdFuncionario.GetValueOrDefault(),
                            Nacionalidad = DeclaracionNacionalidad.NacionalidadesDelete[i],
                            IdDeclaracion = DeclaracionNacionalidad.declaracion.IdDeclaracion
                        };
                        funcionarioNacionalidad = await _context.FuncionarioNacionalidad.FirstOrDefaultAsync(ep => ep.IdFuncionario == funcionarioNacionalidad.IdFuncionario && 
                        ep.IdDeclaracion == funcionarioNacionalidad.IdDeclaracion && ep.Nacionalidad == funcionarioNacionalidad.Nacionalidad && ep.Eliminado == false);

                        var Pais = await _context.Paises.FirstOrDefaultAsync(ep => ep.IdPais == funcionarioNacionalidad.Nacionalidad);

                        await _logger.LogAsync(DeclaracionNacionalidad.declaracion.Formulario.IdProceso, "Eliminar Nacionalidad Declaración " + DeclaracionNacionalidad.declaracion.Formulario.Titulo, Pais.NombrePais.ToString(), "exitoso", TipoAuditoria.Negocio);
                        _context.FuncionarioNacionalidad.Remove(funcionarioNacionalidad);
                    }
                }

                for (int i = 0; i < DeclaracionNacionalidad.Nacionalidades.Count; i++)
                {
                    FuncionarioNacionalidad funcionariNacio = new();

                    var validarNacionalidad = await _context.FuncionarioNacionalidad.FirstOrDefaultAsync(ep => ep.IdDeclaracion == DeclaracionNacionalidad.declaracion.IdDeclaracion
                   && ep.Nacionalidad == DeclaracionNacionalidad.Nacionalidades[i] && ep.Eliminado == false);

                    var funcionarioNacionalidadUpdate = await _context.FuncionarioNacionalidad.AddAsync(new FuncionarioNacionalidad
                    {
                        IdFuncionario = DeclaracionNacionalidad.declaracion.IdFuncionario.GetValueOrDefault(),
                        Nacionalidad = DeclaracionNacionalidad.Nacionalidades[i],
                        IdDeclaracion = DeclaracionNacionalidad.declaracion.IdDeclaracion
                    });
                    var Pais = await _context.Paises.FirstOrDefaultAsync(ep => ep.IdPais == funcionarioNacionalidadUpdate.Entity.Nacionalidad);
                    await _logger.LogAsync(DeclaracionNacionalidad.declaracion.Formulario.IdProceso, "Registrar Nacionalidad Declaración " + DeclaracionNacionalidad.declaracion.Formulario.Titulo, Pais.NombrePais.ToString(), "exitoso", TipoAuditoria.Negocio);
                }

              
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(DeclaracionNacionalidad.declaracion.Formulario?.IdProceso, "Declaracion:Creación", DeclaracionNacionalidad.declaracion.ToAuditoria(), $"fallido - {ex.Message}", TipoAuditoria.Negocio);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
