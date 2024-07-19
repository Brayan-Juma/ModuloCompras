using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ModuloCompras.Mvc.Models;
using System.Collections.Generic;

namespace ModuloCompras.Mvc.Controllers
{
    public class DetalleFacturasController : Controller
    {
        private readonly string urlApi;
        private readonly string urlApiProductos;
        private readonly HttpClient _httpClient;

        public DetalleFacturasController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            urlApi = configuration.GetValue<string>("ApiUrlBase") + "/DetalleFacturas";
            urlApiProductos = configuration.GetValue<string>("ApiUrlProductos") + "/Producto";
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
        public async Task<ActionResult> Create()
        {
            var productos = await ObtenerProductos();
            ViewBag.Productos = productos;
            return View();
        }

        // POST: DetalleFacturasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DetalleFactura data)
        {
            try
            {
                var newData = Crud<DetalleFactura>.Create(urlApi, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var productos = await ObtenerProductos();
                ViewBag.Productos = productos;
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
        public async Task<ActionResult> Edit(int id, DetalleFactura data)
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
        public async Task<ActionResult> Delete(int id, DetalleFactura data)
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

        private async Task<List<Producto>> ObtenerProductos()
        {
            var response = await _httpClient.GetAsync(urlApiProductos);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Producto>>(jsonResponse);
            }
            return new List<Producto>();
        }
    }
}
