using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;

namespace ModuloCompras.Mvc.Controllers
{
    public class ProveedoresController : Controller
    {
        private string urlApi;

        public ProveedoresController(IConfiguration configuration)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/Proveedores";
        }

        // GET: ProveedoresController
        public ActionResult Index()
        {
            var data = Crud<Proveedor>.Read(urlApi);
            return View(data);
        }

        // GET: ProveedoresController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<Proveedor>.Read_ById(urlApi, id);
            return View(data);
        }

        // GET: ProveedoresController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProveedoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proveedor data)
        {
            try
            {
                var newData = Crud<Proveedor>.Create(urlApi, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: ProveedoresController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Proveedor>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: ProveedoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Proveedor data)
        {
            try
            {
                Crud<Proveedor>.Update(urlApi, id, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: ProveedoresController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Proveedor>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: ProveedoresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Proveedor data)
        {
            try
            {
                Crud<Proveedor>.Delete(urlApi, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }
    }
}

