using Microsoft.AspNetCore.Http.Json;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using OpenData.Basketball.AbaLeague.API.Contexts;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basetball.AbaLeague.Persistence;
using System.Runtime;
using OpenData.Basketball.AbaLeague.API.Configurations;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace OpenData.Basketball.AbaLeague.API.Exstentions
{

    [ExcludeFromCodeCoverage]
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureApplicationBuilder(this WebApplicationBuilder builder)
        {
            #region Logging

           
            #endregion Logging

            #region Serialisation

            _ = builder.Services.Configure<JsonOptions>(opt =>
            {
                opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.SerializerOptions.PropertyNameCaseInsensitive = true;
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

            #endregion Serialisation

            #region Swagger

            var ti = CultureInfo.CurrentCulture.TextInfo;

            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = $"Basketball analyse API - {ti.ToTitleCase(builder.Environment.EnvironmentName)}",
                        Description = "An example to share an implementation of Minimal API in .NET 8.",
                        Contact = new OpenApiContact
                        {
                            Name = "BasketballAnalyse API",
                            Email = "lukadlm97@gmail.com",
                            Url = new Uri("https://github.com/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "BasketballAnalyse API - License - MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        TermsOfService = new Uri("https://github.com/stphnwlsh/cleanminimalapi")
                    });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.DocInclusionPredicate((name, api) => true);
            });

            #endregion Swagger

            #region Validation

            _ = builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);

            #endregion Validation

            #region Configure EF UserContext
            builder.Services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(builder.Configuration.GetSection("PersistenceSettings:UserDatabase").Value)
            );

            builder.Services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<UserContext>();
            #endregion

            #region Authentification&Authorization
            builder.Services.AddAuthorization();
            #endregion

            #region Project Dependencies

            _ = builder.Services.ConfigureAdminApplicationServices(builder.Configuration);
            _ = builder.Services.ConfigurePersistenceServices(builder.Configuration);

            #endregion Project Dependencies

            #region OTLP tracing
            var logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger("Program");

            var url = builder.Configuration.GetSection(nameof(OTCSettings) + ":Url").Value;
            var protocol = builder.Configuration.GetSection(nameof(OTCSettings) + ":Protocol").Value;

            ArgumentNullException.ThrowIfNullOrWhiteSpace(url);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(protocol);

            logger.LogInformation(url);
            logger.LogInformation(protocol);

            builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource(DiagnosticsConfig.ActivitySource.Name)
                    .ConfigureResource(resource => resource
                        .AddService(DiagnosticsConfig.ServiceName))
                    .AddAspNetCoreInstrumentation()
                   // .AddConsoleExporter()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(url);
                        opt.Protocol = protocol.MapProtocolFromConfigs();
                    });
            });

            #endregion
            return builder;
        }
    }
}

