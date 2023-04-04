

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.ImportExecutor.Services;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;

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

           /* services.AddDbContextPool<AbaLeagueDbContext>(builder =>
            {
                var connectionString = configuration.GetConnectionString("Database");

                builder.UseSqlServer(connectionString);
            });*/
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.ConfigurePersistenceServices(configuration);
            services.AddHostedService<SimpleImporter>();
        });