using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenData.Basketball.AbaLeague.API.Contexts;
using OpenData.Basketball.AbaLeague.API.Exstentions;
using System;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApplicationBuilder();

// Add services to the container.
var app = builder
    .Build()
    .ConfigureApplication();

try
{
    app.Run();
    return 0;
}
catch (Exception ex)
{
    return 1;
}
finally
{
}