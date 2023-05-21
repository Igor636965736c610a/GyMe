using System.Text.Json.Serialization;
using GymAppApi.MiddleWare;
using GymAppInfrastructure.AutoMapper;
using GymAppInfrastructure.Extensions;
using GymAppInfrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        var converter = new JsonStringEnumConverter();
        x.JsonSerializerOptions.Converters.Add(converter);
    });
builder.Services.ConfigureRefit();
builder.Services.AddCorsPolicy();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDb(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddEmailSender(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddMvcModel();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());
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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();