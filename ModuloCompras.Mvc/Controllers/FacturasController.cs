using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModuloCompras.Mvc.Models;

namespace ModuloCompras.Mvc.Controllers
{
    public class FacturasController : Controller
    {
        private readonly string urlApiFacturas;
        private readonly string urlApiProveedores;
        private readonly string urlApiProductos;
        private readonly HttpClient _httpClient;

        public FacturasController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            urlApiFacturas = configuration.GetValue<string>("ApiUrlBase") + "/Facturas";
            urlApiProveedores = configuration.GetValue<string>("ApiUrlBase") + "/Proveedores";
            urlApiProductos = configuration.GetValue<string>("ApiUrlProductos") + "/Producto";
        }

        // GET: FacturasController
        public ActionResult Index()
        {
            var data = Crud<Factura>.Read(urlApiFacturas);
            return View(data);
        }

        // GET: FacturasController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApiFacturas, id);
            return View(data);
        }

        // GET: FacturasController/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var proveedoresActivos = await ObtenerProveedores();
            var productos = await ObtenerProductos();
            ViewBag.ProveedoresActivos = proveedoresActivos;
            ViewBag.Productos = productos;

            var viewModel = new FacturaDetalleViewModel
            {
                Factura = new Factura(),
                DetalleFactura = new DetalleFactura()
            };

            return View(viewModel);
        }

        // POST: FacturasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacturaDetalleViewModel viewModel)
        {
            try
            {
                var newData = Crud<Factura>.Create(urlApiFacturas, viewModel.Factura);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var proveedoresActivos = await ObtenerProveedores();
                var productos = await ObtenerProductos();
                ViewBag.ProveedoresActivos = proveedoresActivos;
                ViewBag.Productos = productos;
                return View(viewModel);
            }
        }

        // GET: FacturasController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApiFacturas, id);
            return View(data);
        }

        // POST: FacturasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Factura data)
        {
            try
            {
                Crud<Factura>.Update(urlApiFacturas, id, data);
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
            var data = Crud<Factura>.Read_ById(urlApiFacturas, id);
            return View(data);
        }

        // POST: FacturasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Factura data)
        {
            try
            {
                Crud<Factura>.Delete(urlApiFacturas, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        private async Task<List<Proveedor>> ObtenerProveedores()
        {
            var response = await _httpClient.GetAsync(urlApiProveedores);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Proveedor>>(jsonResponse).Where(p => p.Estado).ToList();
            }
            return new List<Proveedor>();
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
