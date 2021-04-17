using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var lista = ru.ObtenerTodos();
            return View(lista);
        }

        [Authorize(Policy = "Administrador")]
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
                u.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(c["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 700,
                        numBytesRequested: 256 / 8));
                u.TipoCuenta = 0;
                ru.Alta(u);
                TempData["Info"] = "Cuenta creada correctamente, ingrese sus datos para continuar.";
                //return RedirectToAction(nameof(Login));
                return View("Login");
            }
            catch (Exception e)
            {
                TempData["ErrorM"] = "Error desconocido";
                if (e is SqlException && ((SqlException)e).Number == 2627)
                    TempData["ErrorM"] =  "Error de registro, email usado por otro usuario";
                return View();
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            var u = ru.ObtenerPorId(id);
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
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
                TempData["Alerta"] = $"Datos del usuario #'{u.Id}' modificados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(u);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var u = ru.ObtenerPorId(id);
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario u)
        {
            try
            {
                ru.Baja(id);
                TempData["Alerta"] = $"Usuario #'{id}' eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(u);
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Restore(int id)
        {
            var u = ru.ObtenerPorId(id);
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Restore(int id, Usuario u)
        {
            try
            {
                ru.Restaurar(id);
                TempData["Alerta"] = $"Usuario #'{id}' restaurado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(u);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: collection["Clave"],
                        salt: System.Text.Encoding.ASCII.GetBytes(c["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 700,
                        numBytesRequested: 256 / 8));

                    Usuario u = ru.ObtenerPorEmail(collection["Email"].ToString());
                    if (u == null || u.Clave != hashed)
                    {
                        TempData["ErrorM"] = "Error al iniciar sesión, datos incorrectos.";
                        return View();
                    }else if(u.Estado != 1)
                    {
                        TempData["ErrorM"] = "Error al iniciar sesión, la cuenta fue eliminada/bloqueada por un administrador.";
                        return View();
                    }
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, u.Email),
                        new Claim(ClaimTypes.Role, u.TipoCuentaNombre)
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    
                    TempData["Info"] = "Sesión iniciada correctamente.";
                    return RedirectToAction("Index", "Home");
                }
                throw new Exception("Error");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        [Authorize(Policy = "Empleado")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }

}
