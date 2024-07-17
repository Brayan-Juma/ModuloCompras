using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using Microsoft.Extensions.Configuration;
using System.Linq;
using ModuloCompras.Mvc.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModuloCompras.Mvc.Controllers
{
    public class FacturarController : Controller
    {
        private readonly string urlApiProveedores;
        private readonly string urlApiProductos;
        private readonly HttpClient _httpClient;

        public FacturarController(IConfiguration configuration)
        {
            urlApiProveedores = configuration.GetValue<string>("ApiUrlBase") + "/Proveedores";
            urlApiProductos = configuration.GetValue<string>("ApiUrlProductos");
            _httpClient = new HttpClient();
        }

        // GET: Facturar
        public async Task<ActionResult> Index()
        {
            var proveedores = Crud<Proveedor>.Read(urlApiProveedores).Where(p => p.Estado).ToList();
            ViewBag.Proveedores = proveedores;

            var productos = await ObtenerProductos();
            ViewBag.Productos = productos;

            return View();
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
    }
}
