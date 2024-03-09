
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace AppService1.Controllers
{
    [Route("~/Identity")]
    public class Identity : Controller
    {
        [HttpGet]
        // ajm: [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "sunstealer.read")]
        public async Task<IActionResult> GetAsync()
        {
            // ajm: sandpit: for swagger => use user login token
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(jwtEncodedString: accessToken);
            return new JsonResult(token);
        }
    }
}
