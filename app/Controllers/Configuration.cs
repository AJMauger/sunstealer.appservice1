using AppService1.Services;
using AppService1.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace AppService1.Controllers;

[Route("~/Configuration")]
[Swashbuckle.AspNetCore.Filters.SwaggerResponseExample(200, typeof(ConfigurationExample))]
public class Configuration : Controller
{
    private readonly ILogger<Configuration> _logger;
    private Services.IApplication applicationService;
    private IConfiguration configuration;

    public Configuration(IApplication applicationService, IConfiguration configuration, ILogger<Configuration> logger) {

        this.applicationService = applicationService;
        this.configuration = configuration;
        this._logger = logger;
        this._logger.LogInformation("Configuration.Configuration()");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Configuration))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("~/Configuration")]

    // ajm: [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "sunstealer.read")]
    public ActionResult<Configuration> GetV1() {
        try
        {
            this._logger.LogInformation("Configuration.Get()");
            var value = this.applicationService.Configuration;
            return Json(value);
        } catch(System.Exception e) {
            _logger.LogError(e, "Configuration.Get()");
        }
        return new BadRequestResult(); 
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Configuration))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("~/Configuration")]

    // ajm: [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "sunstealer.write")]
    public ActionResult<Configuration> PostV1([FromBody] Models.Configuration configuration)
    {
        try {
            return Json(applicationService.Reconfigure(configuration));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Configuration.Post()");
        }
        return new BadRequestResult();
    }
}