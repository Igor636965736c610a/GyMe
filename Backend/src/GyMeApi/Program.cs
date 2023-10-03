using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using GyMeApi.Middleware;
using GyMeApi.Middleware.Extension;
using GyMeInfrastructure.AutoMapper;
using GyMeInfrastructure.Extensions;
using GyMeInfrastructure.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        var converter = new JsonStringEnumConverter();
        x.JsonSerializerOptions.Converters.Add(converter);
    });

builder.Services
    .BindOptions(builder.Configuration)
    .AddFluentValidationAutoValidation()
    .AddDb(builder.Configuration)
    .AddValidations()
    .ConfigureRefit()
    .AddCorsPolicy()
    .AddEndpointsApiExplorer()
    .AddServices()
    .AddRepositories()
    .AddMiddlewares()
    .AddAuthentication(builder.Configuration)
    .AddAuthorizationSet()
    .AddSwaggerConfig()
    .AddSingleton(AutoMapperConfig.Initialize())
    .AddCookies()
    .AddHttpContextAccessor();

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