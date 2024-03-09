using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AppService1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogInformation("HomeController.HomeController()");
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (AppService1.Services.Application.Auth)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                ViewBag.AccessToken = accessToken;

                var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accessToken);

                // ajm: JArray not accessible ... ?! {{
                object obj;
                if (jwt.Payload.TryGetValue("scope", out obj))
                {
                    var array = string.Join(",", obj).Replace("\r\n", "").Replace("\u0022", "").Replace("  ", "");
                    jwt.Payload.Remove("scope");
                    jwt.Payload.TryAdd("scope", array);
                }

                if (jwt.Payload.TryGetValue("amr", out obj))
                {
                    var array = string.Join(",", obj).Replace("\r\n", "").Replace("\u0022", "").Replace("  ", "");
                    jwt.Payload.Remove("amr");
                    jwt.Payload.TryAdd("amr", array);
                }
                // ajm: }}

                var token = System.Text.Json.JsonSerializer.Serialize(jwt, new System.Text.Json.JsonSerializerOptions { MaxDepth = 10, WriteIndented = true }); ;

                ViewBag.Token = token;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new AppService1.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
