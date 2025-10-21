using EIGO.PDLA.Common.Exporter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers
{
    [Authorize(Policy = "GroupAdmin")]
    public class ReporteController : Controller
    {
        private readonly IEntityRepository<Participacion> _participacionesRepository;
        private readonly IEntityRepository<Proceso> _ProcesoRepository;
        // GET: ReporteController


        public ReporteController(
       IEntityRepository<Participacion> participacionesRepository, IEntityRepository<Proceso> ProcesoRepository)
        {

            _participacionesRepository = participacionesRepository;
             _ProcesoRepository = _ProcesoRepository;

        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: ReporteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public async Task<IActionResult> ExportDetails(int idProceso, int? Estado,int TipoReporte, byte tipoCargo)
        {

            if (idProceso==0)
            {
                return NotFound();

            }
            List<Participacion> participaciones = await ((ParticipacionRepository)_participacionesRepository).GetAllActiveDeclaracionAsync(idProceso, Estado);

            if (TipoReporte==0)/// buscamos  cargo
            {
                if (tipoCargo == 1) // tabla 1
                {
                    var participacionDTO = (List<ParticipacionReporteCargoDTO>)participaciones.ToParticipacionDtoCargoCollection();
                    for (var i = 0; i < participacionDTO.Count; i++)
                    {
                        participacionDTO[i].Pais = participacionDTO[i].Pais.Replace(" ", "");

                    }


                    Stream stream = Exporter.ExportToCSV(participacionDTO.Where(e => e.Cargo != null && e.Cargo != "" && e.tipoCargo == 1).ToList(), false);
                    stream.Position = 0;

                    return File(stream, "text/csv", $"InversionCargos{DateTime.Now.Year}.csv");
                }
                else // tabla 2
                {
                    var participacionDTO = (List<ParticipacionReporteCargoDTO>)participaciones.ToParticipacionDtoCargoCollection();
                    for (var i = 0; i < participacionDTO.Count; i++)
                    {
                        participacionDTO[i].Pais = participacionDTO[i].Pais.Replace(" ", "");

                    }


                    Stream stream = Exporter.ExportToCSV(participacionDTO.Where(e => e.Cargo != null && e.Cargo != "" && e.tipoCargo !=1).ToList(), false);
                    stream.Position = 0;

                    return File(stream, "text/csv", $"InversionCargos{DateTime.Now.Year}.csv");
                }
                
            }
            else
            { // tabla 3
                /// buscamos  porcentaje accionario
                var participacionDTO = (List<ParticipacionReportePorAccionarioDTO>)participaciones.ToParticipacionDtoPorAccionarioCollection();
                for (var i = 0; i < participacionDTO.Count; i++)
                {
                    participacionDTO[i].Pais = participacionDTO[i].Pais.Replace(" ", "");

                }
                Stream stream = Exporter.ExportToCSV(participacionDTO.Where(e => e.PctAccionario != null && e.PctAccionario != 0).ToList(),false);
                stream.Position = 0;
                return File(stream, "text/csv", $"InversionParticipacion{DateTime.Now.Year}.csv");
            }



           
          
        }

        // GET: ReporteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReporteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReporteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReporteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReporteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReporteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
