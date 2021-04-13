using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SSpartanoInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration c;
        private RepositorioUsuario ru;

        public UsuariosController(IConfiguration c)
        {
            ru = new RepositorioUsuario(c);
            this.c = c;
        }

        public ActionResult Index()
        {
            var lista = ru.ObtenerTodos();
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            return View(ru.ObtenerPorId(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario u)
        {
            try
            {
                u.TipoCuenta = 0;
                ru.Alta(u);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                //ViewData["Error"] = e.Message;
                ViewData["Error"] = "Error desconocido";
                if (e is SqlException && ((SqlException)e).Number == 2627)
                    ViewData["Error"] =  "Error de registro, email usado";
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var u = ru.ObtenerPorId(id);
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Usuario u = null;
            try
            {
                u = ru.ObtenerPorId(id);
                u.Nombre = collection["Nombre"];
                u.Apellido = collection["Apellido"];
                u.Email = collection["Email"];
                u.TipoCuenta = Convert.ToInt32(collection["TipoCuenta"]);
                ru.Modificacion(u);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(u);
            }
        }

        public ActionResult Delete(int id)
        {
            var u = ru.ObtenerPorId(id);
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario u)
        {
            try
            {
                ru.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(u);
            }
        }

        public async Task<IActionResult> Login()
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "id"),
                        new Claim(ClaimTypes.Role, "tipo"),
                    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            return View();
        }
    }

}
