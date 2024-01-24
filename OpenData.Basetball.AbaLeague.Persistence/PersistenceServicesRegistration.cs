using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Application.Model;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Persistence.Repositories;

namespace OpenData.Basetball.AbaLeague.Persistence
{
    public  static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var inMemoryContext =
                bool.TryParse(configuration
                    .GetSection("PersistenceSettings:UseInMemory")
                    .Value, out bool result) 
                    ? result : true;

            if (inMemoryContext)
            {
                services.AddDbContextPool<AbaLeagueDbContext>(builder =>
                {
                    var connectionString = configuration.GetSection("PersistenceSettings:Database").Value;
                    if (connectionString == null)
                    {
                        throw new ArgumentNullException();
                    }
                    builder.UseInMemoryDatabase(connectionString);
                });

            }
            else
            {
                services.AddDbContextPool<AbaLeagueDbContext>(builder =>
                {
                    var connectionString = configuration.GetSection("PersistenceSettings:Database").Value;
                    if (connectionString == null)
                    {
                        throw new ArgumentNullException();
                    }
                    builder.UseSqlServer(connectionString);
                });
            }
          
         

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ISeasonResourcesRepository, SeasonResourcesRepository>();

            return services;
        }


        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
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
            return host;
        }
       
    }
}
