using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GymAppInfrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class JwtAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly IConfiguration _configuration;
    
    public JwtAuthorizeAttribute(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasJwtToken = context.HttpContext.Request.Headers.ContainsKey("Authorization");
        if (!hasJwtToken)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var jwtToken = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!IsValidJwtToken(jwtToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }

    private bool IsValidJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            SecurityToken validatedToken;
            tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);

            return true; 
        }
        catch (SecurityTokenException)
        {
            return false; 
        }
    }

}
