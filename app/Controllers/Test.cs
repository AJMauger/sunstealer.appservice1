using AppService1.Models;
using AppService1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppService1.Controllers;


public class Test : Controller
{
    private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;
    private readonly ILogger<Test> logger;

    public Test(IDbContextFactory<ApplicationDbContext> dbContextFactory, ILogger<Test> logger) { 
        this.dbContextFactory = dbContextFactory;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("~/Test1")]
    public IActionResult Test1()
    {
        List<Table1> data = new List<Table1>();
        using (var db = this.dbContextFactory.CreateDbContext())
        {
            var now = DateTime.Now;
            data = db.table1.Where(f => f.UUID > 0).ToList();
            data.ForEach(f => f.Number1++);
            db.SaveChanges();
            this.logger.LogInformation($"Duration: {DateTime.Now - now}");
        }
        return Ok(data);
    }
}
