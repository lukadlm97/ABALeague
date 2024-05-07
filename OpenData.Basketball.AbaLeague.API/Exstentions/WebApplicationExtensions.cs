﻿using Microsoft.AspNetCore.Identity;
using OpenData.Basketball.AbaLeague.API.Contexts;
using OpenData.Basketball.AbaLeague.API.Endpoints;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace OpenData.Basketball.AbaLeague.API.Exstentions
{

    [ExcludeFromCodeCoverage]
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            #region Logging


            #endregion Logging

            #region Security

            _ = app.UseHsts();

            #endregion Security

            #region API Configuration

            _ = app.UseHttpsRedirection();

            #endregion API Configuration

            #region Swagger

            var ti = CultureInfo.CurrentCulture.TextInfo;

            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    $"BasketballAnalyseAPI - {ti.ToTitleCase(app.Environment.EnvironmentName)} - V1"));

            #endregion Swagger

            #region auth
            _ = app.UseAuthorization();
            #endregion auth

            #region MinimalApi

            _ = app.MapGroup("/account").MapIdentityApi<ApplicationUser>();
            _ = app.MapSeasonEndpoints();

            #endregion MinimalApi

            return app;
        }
    }
}