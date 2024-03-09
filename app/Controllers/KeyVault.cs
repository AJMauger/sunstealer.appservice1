using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AppService1.Controllers;

[Route("~/KeyVault")]
public class KeyVault : Controller
{
    private readonly ILogger<KeyVault> logger;
    private Services.IApplication applicationService;
    private SecretClient keyVaultClient;

    public KeyVault(Services.IApplication applicationService, ILogger<KeyVault> logger)
    {

        this.applicationService = applicationService;
        this.logger = logger;
        this.logger.LogInformation("KeyVault.KeyVault()");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    [Route("~/KeyVault")]

    // ajm: [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "sunstealer.read")]
    public ActionResult Get(string uri, string secret)
    {
        HttpStatusCode code = HttpStatusCode.OK;
        string payload = string.Empty;
        try
        {
            var connection = Request.Query["uri"];
            var secret1 = Request.Query["secret"];
            if (string.IsNullOrWhiteSpace(connection))
            {
                code = HttpStatusCode.BadRequest;
                payload = $"connection: \"{connection}\"";
            }
            else if (string.IsNullOrWhiteSpace(secret))
            {
                code = HttpStatusCode.BadRequest;
                payload = $"connection: \"{connection}\"";
            }
            else
            {
                var uri1 = new Uri(connection);
                this.keyVaultClient = new SecretClient(uri1, new DefaultAzureCredential());
                var value = this.keyVaultClient.GetSecret(secret).Value.Value;
                this.logger.LogInformation($"secret: \"{value}\"");
                return Json(value);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "KeyVault.Get()");
            code = HttpStatusCode.InternalServerError;
            payload = e.ToString();
        }
        var response = new HttpResponseMessage(code);
        response.Content = new StringContent(payload);
        throw new HttpResponseException(response);
    }
}
