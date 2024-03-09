namespace AppService1.Services {

    // ajm: interface
    public interface IBackground
    {
        void ConsoleWriteMessage(string message);
    }

    // ajm: service
    class Background : Microsoft.Extensions.Hosting.BackgroundService 
    {
        private readonly Microsoft.Extensions.Logging.ILogger<Application> logger;

        public Background(Microsoft.Extensions.Logging.ILogger<Application> logger) {
            this.logger = logger;
        }

        protected override async System.Threading.Tasks.Task<bool> ExecuteAsync(System.Threading.CancellationToken cancellationToken) {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    logger.LogInformation("BackgroundService.ExecuteAsync()");
                    await Task.Delay(5000, cancellationToken);
                }

                return true;
            } catch (System.Exception)
            {
                return false;
            }
        }

        public override Task StopAsync(System.Threading.CancellationToken cancellationToken)
        {
            logger.LogInformation("BackgroundService.StopAsync()");
            return Task.CompletedTask;
        }
    }
}