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
            int totalPagos = rc.TotalPagos(id);
            Contrato contrato = rc.ObtenerPorId(id);

            DateTime ProximoPago = contrato.FechaInicio.AddMonths(totalPagos);
            contrato.ProximoPago = ProximoPago;

            ViewData["TotalPagos"] = totalPagos;
            if(ProximoPago > DateTime.Now)
                contrato.EstadoPagos = 1;
            else
                contrato.EstadoPagos = 2;

            return View(contrato);
        }

        public ActionResult Create()
        {
            ViewData["ListaInmuebles"] = ri.ObtenerDisponibles();
            ViewData["ListaInquilinos"] = riq.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            try
            {
                //int diferenciaMeses = ((c.FechaFin.Year - c.FechaInicio.Year) * 12) + c.FechaFin.Month - c.FechaInicio.Month;

                c.FechaFin = DateTime.Parse(c.FechaFin.ToString()).AddDays(c.FechaInicio.Day - 1);

                if (c.FechaInicio.Day > 28)
                {
                    switch (c.FechaFin.Month)
                    {
                        case 3:
                            c.FechaFin = new DateTime(c.FechaFin.Year, 2, DateTime.IsLeapYear(c.FechaFin.Year) ? 29 : 28);
                            break;
                        case 5:
                        case 7:
                        case 10:
                        case 12:
                            if (c.FechaInicio.Day > 30)
                                c.FechaInicio = new DateTime(c.FechaFin.Year, c.FechaFin.Month - 1, 30);
                            break;
                    }
                }
                if(rc.ComprobarPorInmuebleYFechas(c.InmuebleId, c.FechaInicio, c.FechaFin) == true)
                {
                    rc.Alta(c);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new Exception("El inmueble seleccionado está ocupado en esas fechas.");
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                ViewData["ListaInmuebles"] = ri.ObtenerDisponibles();
                ViewData["ListaInquilinos"] = riq.ObtenerTodos();
                return View();
            }
        }

        public ActionResult Renovar(Contrato c, int IdViejo)
        {
            try
            {
                //int diferenciaMeses = ((c.FechaFin.Year - c.FechaInicio.Year) * 12) + c.FechaFin.Month - c.FechaInicio.Month;

                c.FechaFin = DateTime.Parse(c.FechaFin.ToString()).AddDays(c.FechaInicio.Day - 1);

                if (c.FechaInicio.Day > 28)
                {
                    switch (c.FechaFin.Month)
                    {
                        case 3:
                            c.FechaFin = new DateTime(c.FechaFin.Year, 2, DateTime.IsLeapYear(c.FechaFin.Year) ? 29 : 28);
                            break;
                        case 5:
                        case 7:
                        case 10:
                        case 12:
                            if (c.FechaInicio.Day > 30)
                                c.FechaInicio = new DateTime(c.FechaFin.Year, c.FechaFin.Month - 1, 30);
                            break;
                    }
                }
                rc.Alta(c);
                rc.Renovar(IdViejo); //ESTADO 2 (CUMPLIDO) EN EL CONTRATO VIEJO
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
            IList<Inmueble> Inmuebles = ri.ObtenerDisponibles();
            if (c.Inmueble.Estado != 1)
                Inmuebles.Insert(0, c.Inmueble);
            ViewData["ListaInmuebles"] = Inmuebles;

            IList<Inquilino> Inquilinos = riq.ObtenerTodos();
            if (c.Inquilino.Estado != 1)
                Inquilinos.Insert(0, c.Inquilino);
            ViewData["ListaInquilinos"] = Inquilinos;

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
                ViewData["ListaInmuebles"] = ri.ObtenerDisponibles();
                ViewData["ListaInquilinos"] = riq.ObtenerTodos();
                return View(c);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = rc.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Cancelar(int id, int idInm)
        {
            //ri.CambiarDisponibilidad(idInm, 1);
            rc.Cancelar(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato e, int InmuebleId)
        {
            try
            {
                rc.Baja(id);
                //ri.CambiarDisponibilidad(InmuebleId, 1);
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