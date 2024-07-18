using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using Newtonsoft.Json;
using System.Net.Http;

namespace ModuloCompras.Mvc.Controllers
{
    public class FacturasController : Controller
    {
        private string urlApi;
        private readonly HttpClient _httpClient;

        public FacturasController(IConfiguration configuration)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/Facturas";
        }
        

        // GET: FacturasController
        public ActionResult Index()
        {
            var data = Crud<Factura>.Read(urlApi);
            return View(data);
        }

        // GET: FacturasController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApi, id);
            return View(data);
        }

        // GET: FacturasController/Create
       public ActionResult Create()
        {
            return View();
       }


        // POST: FacturasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Factura data)
        {
            try
            {
                var newData = Crud<Factura>.Create(urlApi, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: FacturasController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: FacturasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Factura data)
        {
            try
            {
                Crud<Factura>.Update(urlApi, id, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: FacturasController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: FacturasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Factura data)
        {
            try
            {
                Crud<Factura>.Delete(urlApi, id);
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

