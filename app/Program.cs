using AppService1;
using AppService1.Models;
using AppService1.Services;
using Asp.Versioning;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Swashbuckle.AspNetCore.Filters;

using Microsoft.Extensions.Hosting.WindowsServices;

// ajm: sandpit
var AppName = "AppService1";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = AppName;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("v"));
    options.ReportApiVersions = true;
}).AddMvc().AddApiExplorer(options =>
{
    options.GroupNameFormat = $"'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

// ajm: identity
if (AppService1.Services.Application.Auth)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:9001";
        options.ClientId = "sunstealer.code";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("offline_access");
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("sunstealer.read");
        options.Scope.Add("sunstealer.write");

        options.UsePkce = true;
        options.SignedOutRedirectUri = "https://localhost:5001/signout-callback-oidc";
    })

    /*
        {
            "keys": [
            {
                "kty": "RSA",
                "use": "sig",
                "kid": "07AE8B59E9FEDCBD43952B2F066DAD57",
                "e": "AQAB",
                "n": "u_aFyzgDAYBiOi99Ph57YgF4lkQYseatMx8YW2n4h6VmpDa3x71kPrB6JWqy4hgXsg4LZdhUD0UIBoYsuVHKUhV0D72fxLHQyVkAo10F_oP39EfDxpYpb054T7M4bN0NqotAo8P3k3pjdJVWkJ9jJQfSUJJryzABfutzsgikJlonbVXRD7aW2CNebdsvCEUc37t5dcdZk4cXiRkdfTAR789FnZVTi9vB5wUOPlBfsQU2FAaggB0wbXAlLtFRHOlohohXRticX0rXJPnJPo6lDZDwTKCziEqM1UKvFwC2Aa1cKELRetsbc7XCIrzQAVqsRNBTMQHKTzxEEnDO74LsHQ",
                "alg": "RS256"
            },
            {
                "kty": "RSA",
                "use": "sig",
                "kid": "9A8C898E12444F81C0F791BCA9224D38",
                "e": "AQAB",
                "n": "mnhS8f4hz-hqNdn08FOSEbl81mr94Y8wGfV1nzkAjI7oDh4LVnwv3Wl2kcElikJBdGMCm_pIpDGc7ZJqiIWBGqiUI3nd4vV7-N5uKflOAVEOmHlNLZO5yCY4zTCysDK1d2cUGMuXnSwMK3Mu4ce59rRofpZK98wAZ55RW-sftxwvzbpAzcN9XKBzkxuY04hiKkZ9h0BveWnFlrOaHDmgx9WZZAiZHvt086ajbu2bBJUm_-2T-mdvyfMz_CQddbCBMJ9cO1WODnJznp2nkgfJnrLH8en4Br72xUnYswMY_mmNsoMsDBjodIPE6dGC6tMTFHsR0gtUtiQqiBvd3gXtMQ",
                "alg": "RS256"
            },
            {
                "kty": "RSA",
                "use": "sig",
                "kid": "0391063D971A0A9066B0091924C0AB0D",
                "e": "AQAB",
                "n": "u9YcxixgmGxFxAx13yQ0ef_vkJzNaOkWVHqBviDWBzBbY4SACYF2HB8JscCcJXL0kAcZP5vgZOYSfMG-3l2IifWD-FeXRVskdX2cASUExnuNBoC2xL271mOajLoMVvJA1O_qXcujC5o1ysDm4GtRJErsZ_aXZCRH1Huz1UVZsCdkHtkVDLGRaOtm-npFd1nUSYIcPLEw44uzEGWqRouJeXHXLcjIABn-riQnb55GSlU9eFZ9xXYIfcVU4p8kOhic_wP2XhcNSa6KQHzkndpUd9ID3Hvp-3sF6eG-yelcdB1W_q6nvv_CEefUPoweMHRThorqDFeh3CCl46ipU2YItQ",
                "alg": "RS256"
            }]}*/

    .AddJwtBearer("Bearer", options =>
    {
        options.SaveToken = true;
        options.Authority = "https://localhost:9001";
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("u9YcxixgmGxFxAx13yQ0ef_vkJzNaOkWVHqBviDWBzBbY4SACYF2HB8JscCcJXL0kAcZP5vgZOYSfMG-3l2IifWD-FeXRVskdX2cASUExnuNBoC2xL271mOajLoMVvJA1O_qXcujC5o1ysDm4GtRJErsZ_aXZCRH1Huz1UVZsCdkHtkVDLGRaOtm-npFd1nUSYIcPLEw44uzEGWqRouJeXHXLcjIABn-riQnb55GSlU9eFZ9xXYIfcVU4p8kOhic_wP2XhcNSa6KQHzkndpUd9ID3Hvp-3sF6eG-yelcdB1W_q6nvv_CEefUPoweMHRThorqDFeh3CCl46ipU2YItQ")),
            ValidateIssuer = false,
            ValidIssuer = "https://localhost:9001",
            ValidateAudience = false,
            ValidAudience = "https://localhost:9001/resources"
        };

        /* ajm: options.Events ??= new JwtBearerEvents();

        options.Events = new JwtBearerEvents()
        {        
            OnAuthenticationFailed = e =>
            {
                System.Diagnostics.Debug.WriteLine($"OnAuthenticationFailed: {e.Request.Protocol} {e.Request.Method} {e.Request.Host}{e.Request.Path}?{e.Request.QueryString}");
                return Task.CompletedTask;
            },

            OnChallenge = e =>
            {
                System.Diagnostics.Debug.WriteLine($"OnChallenge: {e.Request.Protocol} {e.Request.Method} {e.Request.Host}{e.Request.Path}?{e.Request.QueryString}");
                return Task.CompletedTask;
            },

            OnForbidden = e =>
            {
                return Task.CompletedTask;
            },

            OnMessageReceived = e =>
            {
                System.Diagnostics.Debug.WriteLine($"OnMessageReceived: {e.Request.Protocol} {e.Request.Method} {e.Request.Host}{e.Request.Path}?{e.Request.QueryString}");
                return Task.CompletedTask;
            },


            OnTokenValidated = e =>
            {
                return Task.CompletedTask;
            }
        };*/
    });


    // ajm: identity
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("sunstealer.read", policy =>
        {
            policy.RequireClaim("scope", "sunstealer.read");
        });

        options.AddPolicy("sunstealer.write", policy =>
        {
            policy.RequireClaim("scope", "sunstealer.write");
        });
    });

    // builder.Services.AddRequiredScopeAuthorization();
}

builder.Services.AddSwaggerExamplesFromAssemblyOf<Configuration>(); 

// ajm: swagger, with token
builder.Services.AddSwaggerGen(options =>
{
    if (AppService1.Services.Application.Auth)
    {
        options.AddSecurityDefinition("bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Scheme = "bearer",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Id = "bearer",
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                    }
                },
                new string[] {}
            }
        });
    }

    options.ExampleFilters();
});

builder.Services.AddDbContextFactory<ApplicationDbContext>(options => {
    var connection = builder.Configuration.GetValue<string>("Database");
    options.UseSqlServer(connection);
});

builder.Services.AddSingleton<AppService1.Services.IApplication, AppService1.Services.Application>();
builder.Services.AddHostedService<AppService1.Services.Application>();

builder.Services.AddHostedService<AppService1.Services.Background>();

builder.Services.AddControllersWithViews();

if (!builder.Environment.IsDevelopment() && false)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        try
        {
            Console.WriteLine("ConfigureAppConfiguration()", "ConfigureAppConfiguration()");
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AppConfig", EnvironmentVariableTarget.Process);
            Console.WriteLine("ConfigureAppConfiguration().AddAzureAppConfiguration()", $"AppConfig ConnectionString: {connectionString}");
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                options.Connect(new Uri(connectionString), new ManagedIdentityCredential())
                .ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(new DefaultAzureCredential());
                })
                .Select("Sunstealer:*", "Sunstealer")
                .ConfigureRefresh(refreshOptions => refreshOptions.Register("Sunstealer:Sentinel", refreshAll: true));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"ConfigureAppConfiguration() {e}");
        }
    });
}

var app = builder.Build();

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // ajm: app.UseHsts();
}

app.UseCors();
// ajm: app.UseHttpsRedirection();
app.UseStaticFiles();

// ajm: identity
if (AppService1.Services.Application.Auth)
{
    app.UseAuthentication();
}

app.UseRouting();

if (AppService1.Services.Application.Auth)
{
    app.UseAuthorization();
}

// ajm: swagger
app.UseSwagger(options => { });
app.UseSwaggerUI(options => {
    if (AppService1.Services.Application.Auth)
    {
        options.EnablePersistAuthorization();
        options.OAuthAppName(AppName);
        options.OAuthClientId("sunstealer.code");
        options.OAuthUsePkce();
    }
    options.RoutePrefix = "Swagger";
});

if (AppService1.Services.Application.Auth)
{
    app.MapControllerRoute(
        name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"
    ).RequireAuthorization();
} 
else
{
    app.MapControllerRoute(
        name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"
    );
}

/* ajm: app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers().RequireAuthorization("sunstealer.read", "sunstealer.write");
});*/

app.Run();

