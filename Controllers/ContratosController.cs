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
            ViewData["ListaInmuebles"] = ri.ObtenerTodos();
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            int totalPagos = rc.TotalPagos(id);
            Contrato contrato = rc.ObtenerPorId(id);

            DateTime ProximoPago = contrato.FechaInicio.AddMonths(totalPagos);
            contrato.ProximoPago = ProximoPago;

            ViewData["TotalPagos"] = totalPagos;
            if (ProximoPago > DateTime.Now)
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
                if (c.FechaInicio >= c.FechaFin)
                    throw new Exception("No se puede crear el contrato, la fecha de inicio del mismo no puede ser mayor/igual a la de cierre.");
                //int diferenciaMeses = ((c.FechaFin.Year - c.FechaInicio.Year) * 12) + c.FechaFin.Month - c.FechaInicio.Month;

                //El default del fechafin es el dia 1 del mes y año seleccionados
                c.FechaFin = DateTime.Parse(c.FechaFin.ToString()).AddDays(c.FechaInicio.Day - 1); //el AddDays() puede cambiar el mes

                if (c.FechaInicio.Day > 28)
                {
                    switch (c.FechaFin.Month) //casos para cada mes siguiente a uno que no sea de 31 días
                    {
                        case 3: //2 -> 28/29
                            c.FechaFin = new DateTime(c.FechaFin.Year, 2, DateTime.IsLeapYear(c.FechaFin.Year) ? 29 : 28);
                            break;
                        case 5:
                        case 7:
                        case 10:
                        case 12:
                            if (c.FechaInicio.Day > 30)
                                c.FechaFin = new DateTime(c.FechaFin.Year, c.FechaFin.Month - 1, 30);
                            break;
                    }
                }
                if (rc.ComprobarPorInmuebleYFechas(c.InmuebleId, c.FechaInicio, c.FechaFin) == true)
                {
                    c.PrecioInmueble = ri.ObtenerPrecioInmueble(c.InmuebleId);
                    rc.Alta(c);
                    TempData["Info"] = "Contrato creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new Exception("No se puede crear el contrato, el inmueble seleccionado está ocupado en esas fechas.");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorM"] = e.Message;
                ViewData["ListaInmuebles"] = ri.ObtenerDisponibles();
                ViewData["ListaInquilinos"] = riq.ObtenerTodos();
                return View();
            }


        }



        public ActionResult Renovar(Contrato c, int IdViejo) //revisar
        {
            try
            {
                if (c.FechaInicio >= c.FechaFin)
                    throw new Exception("No se puede renovar el contrato, la fecha de inicio del mismo no puede ser mayor/igual a la de cierre.");

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
                                c.FechaFin = new DateTime(c.FechaFin.Year, c.FechaFin.Month - 1, 30);
                            break;
                    }
                }
                if (!rc.ComprobarPorInmuebleYFechas(c.InmuebleId, c.FechaInicio, c.FechaFin))
                {
                    throw new Exception("No se puede renovar el contrato, el inmueble seleccionado está ocupado por otro contrato en las fechas seleccionadas.");
                }
                else
                {
                    c.PrecioInmueble = ri.ObtenerPrecioInmueble(c.InmuebleId); //volver a copiar el precio del inmueble (por si existen cambios)
                    rc.Alta(c);
                    rc.Renovar(IdViejo); //ESTADO 2 (CUMPLIDO) EN EL CONTRATO VIEJO
                    TempData["Info"] = "Contrato renovado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                TempData["ErrorM"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(int id)
        {
            ViewData["TotalPagos"] = rc.TotalPagos(id);
            var c = rc.ObtenerPorId(id);
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
                                c.FechaFin = new DateTime(c.FechaFin.Year, c.FechaFin.Month - 1, 30);
                            break;
                    }
                }

                if (!rc.ComprobarPorInmuebleYFechas(c.InmuebleId, c.FechaInicio, c.FechaFin, id))
                {
                    throw new Exception("No se puede editar el contrato, el inmueble seleccionado está ocupado por otro contrato en las fechas seleccionadas.");
                }
                else if (c.FechaFin <= c.FechaInicio || (c.FechaInicio.Year == c.FechaFin.Year && c.FechaInicio.Month == c.FechaFin.Month))
                {
                    throw new Exception("No se puede editar el contrato, las fechas seleccionadas no son válidas");
                }
                else
                {
                    rc.Modificacion(c);
                    TempData["Alerta"] = $"Datos del contrato #'{id}' modificados correctamente.";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorM"] = ex.Message;
                ViewData["TotalPagos"] = rc.TotalPagos(id);
                ViewData["ListaInmuebles"] = ri.ObtenerDisponibles();
                ViewData["ListaInquilinos"] = riq.ObtenerTodos();
                return View(c);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = rc.ObtenerPorId(id);
            return View(i);
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Cancelar(int id, int idInm)
        {
            rc.Cancelar(id);
            TempData["Alerta"] = $"Contrato #'{id}' cancelado correctamente.";
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
                TempData["Alerta"] = $"Contrato #'{id}' eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                TempData["ErrorM"] = "Error desconocido.";
                return View(e);
            }
        }

        public ActionResult Buscar(IFormCollection collection)
        {
            string sqlWhere = "";

            if (!String.IsNullOrEmpty(collection["buscar_inicio"]) && !String.IsNullOrEmpty(collection["buscar_fin"]))
            {
                sqlWhere = "WHERE Contratos.Estado = 1";
                DateTime FiltroFechaInicio = DateTime.Parse(collection["buscar_inicio"]);
                DateTime FiltroFechaFin = DateTime.Parse(collection["buscar_fin"]);
                sqlWhere += $" AND ((FechaFin >= '{FiltroFechaInicio.ToString("MM-dd-yyyy")}' AND FechaFin <= '{FiltroFechaFin.ToString("MM-dd-yyyy")}') OR (FechaInicio <= '{FiltroFechaFin.ToString("MM-dd-yyyy")}' AND FechaInicio >= '{FiltroFechaInicio.ToString("MM-dd-yyyy")}') OR (FechaInicio <= '{FiltroFechaInicio.ToString("MM-dd-yyyy")}' AND FechaFin >= '{FiltroFechaFin.ToString("MM-dd-yyyy")}'))";
            }
            else if (collection["buscar_inmueble"] != "")
            {
                sqlWhere = "WHERE Contratos.Estado >= 0";
                sqlWhere += $" AND InmuebleId='{collection["buscar_inmueble"]}'";
            }
            var lista = rc.ObtenerPorFiltro(sqlWhere);
            ViewData["ListaInmuebles"] = ri.ObtenerTodos();
            return View("Index", lista);
        }
    }
}