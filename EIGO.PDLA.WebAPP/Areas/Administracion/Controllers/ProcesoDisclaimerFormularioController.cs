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
    public class ProcesoDisclaimerFormularioController : Controller
    {
        private readonly IEntityRepository<ProcesoDisclaimerFormulario> _procesoDisclaimerFormularioControllerRepository;
        public ProcesoDisclaimerFormularioController(IEntityRepository<ProcesoDisclaimerFormulario> procesoDisclaimerFormularioControllerRepository)
        {
            _procesoDisclaimerFormularioControllerRepository = procesoDisclaimerFormularioControllerRepository;
        }
        // GET: ProcesoDisclaimerFormularioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProcesoDisclaimerFormularioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProcesoDisclaimerFormularioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProcesoDisclaimerFormularioController/Create
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

        // GET: ProcesoDisclaimerFormularioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProcesoDisclaimerFormularioController/Edit/5
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

        // GET: ProcesoDisclaimerFormularioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProcesoDisclaimerFormularioController/Delete/5
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
