using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Application.Model;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.WebApi.Services.Contracts;
using OpenData.Basetball.AbaLeague.WebApi.Services.Implementations;
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

// Add services to the container.
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Logging.ClearProviders();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app = MigrateDatabase(app);
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