using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AppService1.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class Versioned1Controller : ControllerBase
    {
        private readonly ILogger<Versioned1Controller> _logger;

        public Versioned1Controller(ILogger<Versioned1Controller> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        [MapToApiVersion(1.0)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [Route("~/GetVersion")]
        public ActionResult<int> GetV1()
        {
            this._logger.LogInformation("Versioned1.Get()");
            return Ok(1);
        }
    }

    [ApiVersion(2.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class Versioned2Controller : ControllerBase
    {
        private readonly ILogger<Versioned2Controller> _logger;

        public Versioned2Controller(ILogger<Versioned2Controller> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        [MapToApiVersion(2.0)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [Route("~/GetVersion")]
        public ActionResult<int> GetV2()
        {
            this._logger.LogInformation("Versioned2.Get()");
            return Ok(2);
        }
    }
}
