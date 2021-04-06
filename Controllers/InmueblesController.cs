using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        // GET: InmueblesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: InmueblesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InmueblesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InmueblesController/Create
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

        // GET: InmueblesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InmueblesController/Edit/5
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

        // GET: InmueblesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InmueblesController/Delete/5
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
