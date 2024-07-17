using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using ModuloCompras.Mvc.Models;

namespace ModuloCompras.Mvc.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrlProductos;

        public ProductosController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiUrlProductos = configuration["ApiUrlProductos"];
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"{_apiUrlProductos}/Producto");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var productos = JsonConvert.DeserializeObject<List<Producto>>(jsonResponse);
            return View(productos);
        }
    }
}
