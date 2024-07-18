using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ModuloCompras.Mvc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IHttpClientFactory clientFactory, ILogger<LoginModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                try
                {
                    var loginRequest = new
                    {
                        usr_user = Input.Username,
                        usr_password = Input.Password,
                        mod_name = "Compras"
                    };

                    var response = await client.PostAsJsonAsync("https://api-modulo-seguridad.onrender.com/api/login_module", loginRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<AuthResponseModel>();

                        // Autenticar al usuario sin usar el token para otras interacciones
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Input.Username)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimsIdentity));

                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Error logging in: {responseContent}");
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error al iniciar sesión.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while logging in: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while logging in.");
                }
            }

            return Page();
        }
    }

    public class AuthResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        // Añade otros campos según la respuesta de tu API
    }
}
