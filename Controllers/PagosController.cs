using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SSpartanoInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Controllers
{
    [Authorize(Policy = "Empleado")]
    public class PagosController : Controller
    {
        private readonly IConfiguration c;
        private RepositorioPago rp;

        public PagosController(IConfiguration c)
        {
            rp = new RepositorioPago(c);
            this.c = c;
        }
        public ActionResult Index(int ContratoId)
        {
            var lista = rp.ObtenerPorContrato(ContratoId);
            ViewData["ContratoId"] = ContratoId;
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            return View(rp.ObtenerPorId(id));
        }

        public ActionResult Create(int ContratoId)
        {
            RepositorioContrato rc = new RepositorioContrato(c);
            //ViewData["ContratoId"] = ContratoId;
            ViewData["Contrato"] = rc.ObtenerPorId(ContratoId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Pago p = new Pago();
                p.ContratoId = Convert.ToInt32(collection["ContratoId"]);
                rp.Alta(p);
                return RedirectToAction(nameof(Index),new { ContratoId = collection["ContratoId"]});
            }
            catch (Exception e)
            {
                RepositorioContrato rc = new RepositorioContrato(c);
                ViewData["Error"] = e.Message;
                ViewData["Contrato"] = rc.ObtenerPorId(Convert.ToInt32(collection["ContratoId"]));
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var p = rp.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Pago p = null;
            try
            {
                p = rp.ObtenerPorId(id);
                p.ContratoId = Convert.ToInt32(collection["ContratoId"]);
                p.Fecha = DateTime.Parse(collection["Fecha"]);

                rp.Modificacion(p);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(p);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var p = rp.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago p)
        {
            try
            {
                rp.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }
    }
}
