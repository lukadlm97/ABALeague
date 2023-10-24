﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using System.Reflection;
using MediatR;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public  static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
           // services.AddScoped<IEuroleagueProcessor, EuroPageProcessor>();
           // services.AddScoped<IWebPageProcessor, WebPageProcessor>();

            return services;
        }

        public static IServiceCollection ConfigureAdminApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDocumentFetcher, DocumentFetcher>();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
