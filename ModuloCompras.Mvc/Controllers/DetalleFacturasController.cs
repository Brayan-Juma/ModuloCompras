using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;

namespace ModuloCompras.Mvc.Controllers
{

    public class DetalleFacturasController : Controller
    {
        private string urlApi;

        public DetalleFacturasController(IConfiguration configuration)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/DetalleFacturas";
        }

        // GET: DetalleFacturasController
        public ActionResult Index()
        {
            var data = Crud<DetalleFactura>.Read(urlApi);
            return View(data);
        }

        // GET: DetalleFacturasController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<DetalleFactura>.Read_ById(urlApi, id);
            return View(data);
        }

        // GET: DetalleFacturasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetalleFacturasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetalleFactura data)
        {
            try
            {
                var newData = Crud<DetalleFactura>.Create(urlApi, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: DetalleFacturasController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<DetalleFactura>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: DetalleFacturasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DetalleFactura data)
        {
            try
            {
                Crud<DetalleFactura>.Update(urlApi, id, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: DetalleFacturasController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<DetalleFactura>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: DetalleFacturasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DetalleFactura data)
        {
            try
            {
                Crud<DetalleFactura>.Delete(urlApi, id);
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
