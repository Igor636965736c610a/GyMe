using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Options;
using GymAppInfrastructure.Repo;
using GymAppInfrastructure.Requirements;
using GymAppInfrastructure.Services;
using GymAppInfrastructure.Services.InternalManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GymAppInfrastructure.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ISimpleExerciseService, SimpleExerciseService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IChartService, ChartService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddSingleton<ErrorService>();
        services.AddSingleton<PaymentMessagesService>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IExerciseRepo, ExerciseRepo>();
        services.AddScoped<ISimpleExerciseRepo, SimpleExerciseRepo>();
        services.AddScoped<IUserRepo, UserRepo>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        }).AddFacebook(options => 
        {
           options.AppId = configuration["FacebookOptions:AppId"];
           options.AppSecret = configuration["FacebookOptions:AppSecret"];
       });

        return services;
    }

    public static IServiceCollection AddAuthorizationSet(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationRequirement, SourceRequirement>();
        services.AddSingleton<IAuthorizationHandler, SourceRequirementHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("SSO", policyBuilder =>
            {
                policyBuilder.Requirements.Add(new SourceRequirement());
            });
        });

        return services;
    }

    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "GyMe", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddCookies(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToAccessDenied =
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.FromResult<object>(null!);
                };
        });

        return services;
    }

    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<GyMePostgresContext>(options => {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<GyMePostgresContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection ConfigureRefit(this IServiceCollection services)
    {
        services.AddRefitClient<IJokeApiService>()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri("https://v2.jokeapi.dev/"));
        services.AddRefitClient<IFacebookApiService>()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri("https://graph.facebook.com"));

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        return services;
    }

    public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        services.AddSingleton<MongoDbSettings>();
        services.Configure<StripeOptions>(configuration.GetSection(nameof(StripeOptions)));
        services.AddSingleton<StripeOptions>();
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.AddSingleton<EmailOptions>();

        return services;
    }
}  