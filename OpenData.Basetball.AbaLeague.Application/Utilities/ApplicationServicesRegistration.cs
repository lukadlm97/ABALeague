using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public  static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWebPageProcessor, EuroPageProcessor>();

            return services;
        }
    }
}
