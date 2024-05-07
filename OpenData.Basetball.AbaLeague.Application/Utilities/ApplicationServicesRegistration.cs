using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using System.Reflection;
using MediatR;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;
using OpenData.Basketball.AbaLeague.Application.Configurations.Players;
using OpenData.Basketball.AbaLeague.Application.Behaviourses;
using OpenData.Basketball.AbaLeague.Application.Configurations;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public  static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, 
                                                                        IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IStandingsService, StandingsService>();
            services.AddScoped<IStatisticsCalculator, StatisticsCalculator>();

            return services;
        }

        public static IServiceCollection ConfigureAdminApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDocumentFetcher, DocumentFetcher>();
            services.ConfigureApplicationServices(configuration);

            // business logic configurations
            services.Configure<PlayerSettings>(configuration.GetSection(nameof(PlayerSettings)));
            services.Configure<CacheSettings>(configuration.GetSection(nameof(CacheSettings)));

            services.ConfigureFusionCache(configuration);

            return services;
        }
        public static IServiceCollection ConfigureFusionCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddFusionCache();

            return services;
        }
    }
}
