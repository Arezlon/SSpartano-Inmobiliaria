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
    public class InquilinosController : Controller
    {
        private readonly IConfiguration c;
        private RepositorioInquilino ri;

        public InquilinosController(IConfiguration c)
        {
            ri = new RepositorioInquilino(c);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                ri.Alta(i);
                TempData["Info"] = "Inquilino creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                TempData["ErrorM"] = "Error desconocido.";
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var p = ri.ObtenerPorId(id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = null;
            try
            {
                i = ri.ObtenerPorId(id);
                i.Nombre = collection["Nombre"];
                i.Apellido = collection["Apellido"];
                i.Dni = collection["Dni"];
                i.Email = collection["Email"];
                i.Telefono = collection["Telefono"];
                i.LugarTrabajo = collection["LugarTrabajo"];
                i.GaranteNombre = collection["GaranteNombre"];
                i.GaranteApellido = collection["GaranteApellido"];
                i.GaranteDni = collection["GaranteDni"];
                i.GaranteTelefono = collection["GaranteTelefono"];
                i.GaranteEmail = collection["GaranteEmail"];

                ri.Modificacion(i);
                TempData["Alerta"] = $"Datos del inquilino #'{id}' modificados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                TempData["ErrorM"] = "Error desconocido.";
                return View(i);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = ri.ObtenerPorId(id);
            return View(i);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino ei)
        {
            try
            {
                ri.Baja(id);
                TempData["Alerta"] = $"Inquilino #'{id}' eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                TempData["ErrorM"] = "Error desconocido.";
                return View(ei);
            }
        }
    }
}
