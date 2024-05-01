
using AppService1.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AppService1.Swagger;

public class ConfigurationExample : IExamplesProvider<Models.Configuration>
{
    public Configuration GetExamples()
    {
        return new Configuration();
    }
}

