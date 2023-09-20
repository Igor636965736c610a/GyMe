using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using GymAppApi.Middleware;
using GymAppApi.Middleware.Extension;
using GymAppInfrastructure.AutoMapper;
using GymAppInfrastructure.Extensions;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        var converter = new JsonStringEnumConverter();
        x.JsonSerializerOptions.Converters.Add(converter);
    });
builder.Services.BindOptions(builder.Configuration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddDb(builder.Configuration);
builder.Services.AddValidations();
builder.Services.ConfigureRefit();
builder.Services.AddCorsPolicy();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddMiddlewares();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorizationSet();
builder.Services.AddSwaggerConfig();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());
builder.Services.AddCookies();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<GyMePostgresContext>();
var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(option =>
    {
        option.RouteTemplate = app.Services.GetRequiredService<SwaggerSettings>().JsonRoute;
    });
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint(app.Services.GetRequiredService<SwaggerSettings>().UiEndpoint, app.Services.GetRequiredService<SwaggerSettings>().Description);
    });
}

app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<DbTransactionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ValidAccountMiddleware>();

app.MapControllers();

app.Run();