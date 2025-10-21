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
    public class CatalogoAniosController : Controller
    {
        private readonly IEntityRepository<CatalogoAnios> _CatalogoAniosRepository;
        // GET: CatalogoAniosController
        public CatalogoAniosController(IEntityRepository<CatalogoAnios> CatalogoAniosRepository)
        {
            _CatalogoAniosRepository = CatalogoAniosRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: CiudadController/Details/5
        public ActionResult Details()
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
        public ActionResult Edit()
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
        public ActionResult Delete()
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
        public async Task<JsonResult> GetCatalogoAnios()
        {
       
            var catalogoAnios = await _CatalogoAniosRepository.GetAllActiveAsync();


            if (catalogoAnios != null)
            {

                return Json(new { catalogoAnios });

            }

            return Json(false);


        }
    }
}
