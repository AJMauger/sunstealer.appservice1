using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppService1.Services
{
    // ajm: interface
    public interface IApplication
    {
        Models.Configuration Configuration { get; }

        bool Reconfigure(Models.Configuration configuration);
    }

    // ajm: service
    public class Application : IApplication, IHostedService
    {
        public const bool Auth = false;

        private readonly Microsoft.EntityFrameworkCore.IDbContextFactory<ApplicationDbContext> dbContextFactory;
        private readonly ILogger<Application> logger;

        public Models.Configuration Configuration { get; private set; }

        public Application(ILogger<Application> logger, IDbContextFactory<ApplicationDbContext> dbContext) {
            this.Configuration = new Models.Configuration();
            this.dbContextFactory = dbContext;
            this.logger = logger;
        }

        public bool Reconfigure(Models.Configuration configuration)
        {
            try {
                if (configuration.Validate(configuration)) {
                    return true;
                }
            } catch(System.Exception e) {
                this.logger.LogCritical(e.ToString());
            }

            return false;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("ApplicationService.StopAsync()");

            

            try {
                this.logger.LogInformation($"ApplicationService.StartAsync()");
            }
            catch(Exception e)
            {
                this.logger.LogError(e, "ApplicationService.StartAsync()");
            }

            var dbContext = dbContextFactory.CreateDbContext();
            var table1 = dbContext.table1.FromSql($"select * from [dbo].[Table1]").ToList();
            logger.LogInformation($"table1.rows: {table1.Count}");
            dbContext.Dispose();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("ApplicationService.StopAsync()");
            return Task.CompletedTask;
        }
    }
}