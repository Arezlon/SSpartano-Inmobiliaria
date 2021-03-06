using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSpartanoInmobiliaria.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace SSpartanoInmobiliaria.Controllers
{
    [Authorize(Policy = "Empleado")]
    public class PropietariosController : Controller
    {
        private readonly IConfiguration c;
        private RepositorioPropietario rp;

        public PropietariosController(IConfiguration c)
        {
            rp = new RepositorioPropietario(c);
            this.c = c;
        }
        public ActionResult Index()
        {
            var lista = rp.ObtenerTodos();
            return View(lista);
        }

        // GET: PropietariosController/Details/5
        public ActionResult Details(int id)
        {
            return View(rp.ObtenerPorId(id));
        }

        // GET: PropietariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                rp.Alta(p);
                TempData["Info"] = "Propietario creado correctamente.";
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
            var p = rp.ObtenerPorId(id);
            return View(p);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                p = rp.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Email = collection["Email"];
                p.Telefono = collection["Telefono"];
                rp.Modificacion(p);
                TempData["Alerta"] = $"Datos del propietario #'{id}' modificados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                TempData["ErrorM"] = "Error desconocido.";
                return View(p);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var p = rp.ObtenerPorId(id);
            return View(p);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                rp.Baja(id);
                TempData["Alerta"] = $"Propietario #'{id}' eliminado correctamente (junto con todos sus inmuebles)";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                TempData["ErrorM"] = "Error desconocido.";
                return View(entidad);
            }
        }
    }
}
