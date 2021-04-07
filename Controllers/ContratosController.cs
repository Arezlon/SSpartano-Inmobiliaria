﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SSpartanoInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Controllers
{
    public class ContratosController : Controller
    {
        private readonly IConfiguration c;
        private RepositorioInmueble ri;
        private RepositorioInquilino riq;
        private RepositorioContrato rc;

        public ContratosController(IConfiguration c)
        {
            ri = new RepositorioInmueble(c);
            riq = new RepositorioInquilino(c);
            rc = new RepositorioContrato(c);
            this.c = c;
        }
        public ActionResult Index()
        {
            var lista = rc.ObtenerTodos();
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            return View(rc.ObtenerPorId(id));
        }

        public ActionResult Create()
        {
            ViewData["ListaInmuebles"] = ri.ObtenerTodos();
            ViewData["ListaInquilinos"] = riq.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            try
            {
                rc.Alta(c);
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
            var c = rc.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewData["ListaInmuebles"] = ri.ObtenerTodos();
            ViewData["ListaInquilinos"] = riq.ObtenerTodos();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Contrato c = null;
            try
            {
                c = rc.ObtenerPorId(id);
                c.InmuebleId = Convert.ToInt32(collection["InmuebleId"]);
                c.InquilinoId = Convert.ToInt32(collection["InquilinoId"]);
                c.FechaInicio = DateTime.Parse(collection["FechaInicio"]);
                c.FechaFin = DateTime.Parse(collection["FechaFin"]);
                rc.Modificacion(c);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(c);
            }
        }

        public ActionResult Delete(int id)
        {
            var i = rc.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato e)
        {
            try
            {
                rc.Baja(id);
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