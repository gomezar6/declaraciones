#nullable disable
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Policy = "AnyGroup")]
    public class CiudadController : Controller
    {
        private readonly IEntityRepository<Ciudad> _CiudadRepository;
        // GET: CiudadController

        public CiudadController(IEntityRepository<Ciudad> CiudadRepository)
        {
            _CiudadRepository = CiudadRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: CiudadController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CiudadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CiudadController/Create
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

        // GET: CiudadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CiudadController/Edit/5
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

        // GET: CiudadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CiudadController/Delete/5
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

        [HttpGet]
        public async Task<JsonResult> GetCiudadByPais(int IdPais)
        {
            var Ciudades = await ((CiudadRepository)_CiudadRepository).GetCiudadByPais(IdPais);

        
            if (Ciudades != null)
            {

                return Json(new { Ciudades });

            }

            return Json(false);


        }
    }
}
