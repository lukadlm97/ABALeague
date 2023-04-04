using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Application.Model;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;

namespace OpenData.Basetball.AbaLeague.Persistence
{
    public  static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AbaLeagueDbContext>(builder =>
                {
                    var connectionString = configuration.GetConnectionString("Database");
                    builder.UseInMemoryDatabase(connectionString);
                });
         

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPlayerRepository, PlayerRepository>();

            return services;
        }
    }
}
