using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using FluentValidation;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.InternalManagement;
using GyMeInfrastructure.IServices;
using GyMeInfrastructure.Models.Validations;
using GyMeInfrastructure.MyMapper;
using GyMeInfrastructure.Options;
using GyMeInfrastructure.Repo;
using GyMeInfrastructure.Requirements;
using GyMeInfrastructure.Services;
using GyMeInfrastructure.Services.InternalManagement;
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

namespace GyMeInfrastructure.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IExerciseService, ExerciseService>()
            .AddScoped<IIdentityService, IdentityService>()
            .AddScoped<ISimpleExerciseService, SimpleExerciseService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IChartService, ChartService>()
            .AddScoped<IUserContextService, UserContextService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IReactionService, ReactionService>()
            .AddScoped<ICommentService, CommentService>()
            .AddScoped<IGyMeResourceService, GyMeResourceService>()
            .AddScoped<ICommentReactionService, CommentReactionService>()
            .AddScoped<IMainPageService, MainPageService>()
            .AddScoped<IGyMeMapper, GyMeMapper>()
            .AddSingleton<ErrorService>()
            .AddSingleton<PaymentMessagesService>()
            .AddSingleton<OpinionService>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IExerciseRepo, ExerciseRepo>()
            .AddScoped<ISimpleExerciseRepo, SimpleExerciseRepo>()
            .AddScoped<IUserRepo, UserRepo>()
            .AddScoped<IReactionRepo, ReactionRepo>()
            .AddScoped<ICommentRepo, CommentRepo>()
            .AddScoped<ICommentReactionRepo, CommentReactionRepo>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(serviceProvider.GetRequiredService<JwtSettings>().Secret)),
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
            options.AddPolicy("AppSys", policyBuilder =>
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

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
        });

        return services;
    }

    public static IServiceCollection ConfigureRefit(this IServiceCollection services)
    {
        services.AddRefitClient<IJokeApiService>()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri("https://v2.jokeapi.dev/"));

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

    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssemblyContaining<ActivateAccountModelValidator>()
            .AddValidatorsFromAssemblyContaining<BaseSeriesDtoValidator>()
            .AddValidatorsFromAssemblyContaining<OpinionRequestBodyValidator>()
            .AddValidatorsFromAssemblyContaining<PaymentRequestModelValidator>()
            .AddValidatorsFromAssemblyContaining<PostSimpleExerciseDtoValidator>()
            .AddValidatorsFromAssemblyContaining<PutSimpleExerciseDtoValidator>()
            .AddValidatorsFromAssemblyContaining<PutUserDtoValidator>()
            .AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>()
            .AddValidatorsFromAssemblyContaining<PostCommentDtoValidator>()
            .AddValidatorsFromAssemblyContaining<PutCommentDtoValidator>()
            .AddValidatorsFromAssemblyContaining<PostReactionDtoValidator>()
            .AddValidatorsFromAssemblyContaining<PostCommentReactionValidator>();

        return services;
    }

    public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)))
            .Configure<StripeOptions>(configuration.GetSection(nameof(StripeOptions)))
            .Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)))
            .AddSingleton<MongoDbSettings>()
            .AddSingleton<StripeOptions>()
            .AddSingleton<EmailOptions>();
        
        var swaggerSettings = new SwaggerSettings();
        var jwtSettings = new JwtSettings();
        configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);
        configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
        services
            .AddSingleton(swaggerSettings)
            .AddSingleton(jwtSettings);

        return services;
    }
}