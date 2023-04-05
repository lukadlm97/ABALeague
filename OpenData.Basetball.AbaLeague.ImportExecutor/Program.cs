using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenData.Basetball.AbaLeague.Application.Model;
using OpenData.Basetball.AbaLeague.ImportExecutor.Services;
using OpenData.Basetball.AbaLeague.Persistence;

var hostBuilder = CreateHostBuilder(null);

await hostBuilder.Build().RunAsync();

static IHostBuilder CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

          
           services.AddOptions<PersistenceSettings>()
               .BindConfiguration(nameof(PersistenceSettings))
               .ValidateOnStart();

            services.ConfigurePersistenceServices(configuration);
            services.AddHostedService<SimpleImporter>();
        });