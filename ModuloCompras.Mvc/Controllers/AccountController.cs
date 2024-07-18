using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ModuloCompras.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var isValidUser = await ValidateUserWithApi(email, password);

                if (isValidUser)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Usuario no registrado en el sistema.");
                        return View();
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                }
            }

            return View();
        }

        private async Task<bool> ValidateUserWithApi(string email, string password)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = _configuration["ApiUrlSeguridad"]; // Obtener la URL de la API de seguridad desde la configuración
                var endpoint = "Users%20Login/post_api_login_module"; // Ruta completa del endpoint de login

                // Hacer la solicitud a la API de seguridad
                var response = await client.PostAsJsonAsync($"{apiUrl}/{endpoint}", new { Email = email, Password = password });

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);
                    return jsonObject["isValid"].Value<bool>(); // Validar si las credenciales son válidas según la respuesta de la API
                }

                return false; // Si la solicitud no fue exitosa, retornar falso
            }
        }
    }
}