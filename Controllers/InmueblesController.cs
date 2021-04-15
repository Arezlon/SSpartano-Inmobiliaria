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
    public class InmueblesController : Controller
    {
        private readonly IConfiguration c;
        private RepositorioInmueble ri;
        private RepositorioPropietario rp;

        public InmueblesController(IConfiguration c)
        {
            ri = new RepositorioInmueble(c);
            rp = new RepositorioPropietario(c);
            this.c = c;
        }
        public ActionResult Index()
        {
            var lista = ri.ObtenerTodos();
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            return View(ri.ObtenerPorId(id));
        }

        public ActionResult Create()
        {
            ViewData["ListaPropietarios"] = rp.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                ri.Alta(i);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var i = ri.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewData["ListaPropietarios"] = rp.ObtenerTodos();
            return View(i);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inmueble i = null;
            try
            {
                i = ri.ObtenerPorId(id);
                i.PropietarioId = Convert.ToInt32(collection["PropietarioId"]);
                i.Direccion = collection["Direccion"];
                i.Uso = collection["Uso"];
                i.Tipo = collection["Tipo"];
                i.Ambientes = Convert.ToInt32(collection["Ambientes"]);
                i.Precio = Convert.ToInt32(collection["Precio"]);
                ri.Modificacion(i);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                ViewData["ListaPropietarios"] = rp.ObtenerTodos();
                return View(i);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = ri.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble e)
        {
            try
            {
                ri.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(e);
            }
        }

        public ActionResult Hide(int id, Inmueble e)
        {
            try
            {
                ri.CambiarVisibilidad(id, 2);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        public ActionResult Show(int id, Inmueble e)
        {
            try
            {
                ri.CambiarVisibilidad(id, 1);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(e);
            }
        }
    }
}
