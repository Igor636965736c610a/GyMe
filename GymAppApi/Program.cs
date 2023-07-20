using System.Text.Json.Serialization;
using GymAppApi.Middleware;
using GymAppApi.Middleware.Extension;
using GymAppInfrastructure.AutoMapper;
using GymAppInfrastructure.Extensions;
using GymAppInfrastructure.Options;
using GymAppInfrastructure.Requirements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        var converter = new JsonStringEnumConverter();
        x.JsonSerializerOptions.Converters.Add(converter);
    });
builder.Services.BindOptions(builder.Configuration);
builder.Services.AddDb(builder.Configuration);
builder.Services.ConfigureRefit();
builder.Services.AddCorsPolicy();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddMiddlewares();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAutorizationSet();
builder.Services.AddMvcModel();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());
builder.Services.AddCookies();
var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(option =>
    {
        option.RouteTemplate = swaggerOptions.JsonRoute;
    });
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<DbTransactionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ValidAccountMiddleware>();

app.MapControllers();

app.Run();