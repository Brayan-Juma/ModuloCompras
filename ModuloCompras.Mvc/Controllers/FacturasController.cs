using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using ModuloCompras.Mvc.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ModuloCompras.Mvc.Models;

namespace ModuloCompras.Mvc.Controllers
{
    public class FacturasController : Controller
    {
        private readonly string urlApiFacturas;
        private readonly string urlApiDetalles;
        private readonly string urlApiProveedores;
        private readonly string urlApiProductos;
        private readonly HttpClient _httpClient;

        public FacturasController(IConfiguration configuration)
        {
            urlApiFacturas = configuration.GetValue<string>("ApiUrlBase") + "/Facturas";
            urlApiDetalles = configuration.GetValue<string>("ApiUrlBase") + "/DetalleFacturas";
            urlApiProveedores = configuration.GetValue<string>("ApiUrlBase") + "/Proveedores";
            urlApiProductos = configuration.GetValue<string>("ApiUrlProductos");
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> Create()
        {
            var proveedores = await ObtenerProveedores();
            var productos = await ObtenerProductos();

            ViewBag.Proveedores = proveedores;
            ViewBag.Productos = productos;

            return View(new FacturaDetalleViewModel());
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
            var response = await _httpClient.GetAsync(urlApiProductos + "/Producto");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Producto>>(jsonResponse);
            }
            return new List<Producto>();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FacturaDetalleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Crear la factura
                    var facturaResponse = await _httpClient.PostAsJsonAsync(urlApiFacturas, model.Factura);
                    if (facturaResponse.IsSuccessStatusCode)
                    {
                        var factura = await facturaResponse.Content.ReadAsAsync<Factura>();

                        // Crear los detalles de la factura
                        foreach (var detalle in model.DetalleFacturas)
                        {
                            detalle.FacturaId = factura.Id;
                            await _httpClient.PostAsJsonAsync(urlApiDetalles, detalle);
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var proveedores = await ObtenerProveedores();
            var productos = await ObtenerProductos();
            ViewBag.Proveedores = proveedores;
            ViewBag.Productos = productos;

            return View(model);
        }

        public ActionResult Index()
        {
            var data = Crud<Factura>.Read(urlApiFacturas);
            return View(data);
        }

        public ActionResult Details(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApiFacturas, id);
            return View(data);
        }

        public ActionResult Edit(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApiFacturas, id);
            return View(data);
        }

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

        public ActionResult Delete(int id)
        {
            var data = Crud<Factura>.Read_ById(urlApiFacturas, id);
            return View(data);
        }

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
    }
}
