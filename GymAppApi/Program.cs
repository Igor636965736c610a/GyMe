using System.Text.Json.Serialization;
using GymAppApi.Middleware;
using GymAppApi.Middleware.Extension;
using GymAppInfrastructure.AutoMapper;
using GymAppInfrastructure.Extensions;
using GymAppInfrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.BindOptions(builder.Configuration);
builder.Services.AddMiddlewares();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddMvcModel();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied =
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult<object>(null!);
        };
});
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

app.MapControllers();

app.Run();