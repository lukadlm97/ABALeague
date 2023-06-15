using System.Reflection;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Application.Model;
using OpenData.Basetball.AbaLeague.Crawler.Configurations;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.WebApi.Helpers;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

// Use Serilog
builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration
        .WriteTo.File("serilog-file.txt",outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.Console();
});

builder.Services.AddOptions<PersistenceSettings>()
    .BindConfiguration(nameof(PersistenceSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<CrawlerSettings>()
    .BindConfiguration(nameof(CrawlerSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

/*
builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetAssembly(typeof(AssemblyReference)))
    .AddClasses(classes=>classes.AssignableTo<IPlayerService>())
        .AsImplementedInterfaces()
        .WithScopedLifetime()
   
);
*/

builder.Services.Scan(scan =>
    scan.FromAssemblyOf<ILeagueFetcher>()
    .AddClasses(classes => classes.AssignableTo<ILeagueFetcher>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .FromAssemblyOf<IWebPageProcessor>()
    .AddClasses(classes => classes.AssignableTo<IWebPageProcessor>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);


builder.Services.Scan(scan => scan.FromAssemblyOf<ILeagueService>()
    .AddClasses(classes => classes.AssignableTo<ILeagueService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes =>classes.AssignableTo<ITeamService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes => classes.AssignableTo<ISeasonResourcesService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes => classes.AssignableTo<IRosterService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()   
    .AddClasses(classes => classes.AssignableTo<IPlayerService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);


// Add services to the container.
builder.Services.ConfigurePersistenceServices(builder.Configuration);
//builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Logging.ClearProviders();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            await context.HttpContext.Response.WriteAsync(
                $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s). " +
                $"Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken: token);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(
                "Too many requests. Please try again later. " +
                "Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken: token);
        }
    };
});
/*
options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
    PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress.ToString(), partition =>
            new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 600,
                Window = TimeSpan.FromMinutes(1)
            })),
    PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress.ToString(), partition =>
            new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 6000,
                Window = TimeSpan.FromHours(1)
            })));
});
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();
//app = MigrateDatabase(app);
app.Run();



 static WebApplication MigrateDatabase( WebApplication webApp)
{
    using (var scope = webApp.Services.CreateScope())
    {
        using (var appContext = scope.ServiceProvider.GetRequiredService<AbaLeagueDbContext>())
        {
            try
            {
                appContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                //Log errors or do anything you think it's needed
                throw;
            }
        }
    }
    return webApp;
}