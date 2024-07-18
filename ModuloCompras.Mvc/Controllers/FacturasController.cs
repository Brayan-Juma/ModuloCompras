using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using Newtonsoft.Json;

public class FacturasController : Controller
{
    private string urlApi;
    private readonly HttpClient _httpClient;
    private readonly string urlApiProveedores;

    public FacturasController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/Facturas";
        urlApiProveedores = configuration.GetValue<string>("ApiUrlBase") + "/Proveedores";
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
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var proveedoresActivos = await ObtenerProveedores();
        ViewBag.ProveedoresActivos = proveedoresActivos;
        return View();
    }

    // POST: FacturasController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Factura data)
    {
        try
        {
            var newData = Crud<Factura>.Create(urlApi, data);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var proveedoresActivos = await ObtenerProveedores();
            ViewBag.ProveedoresActivos = proveedoresActivos;
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
}