using Business.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ServerApi.Services
{
    public class AuthenticationConfigureService
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = new JwtOptions
                {
                    Audience = configuration.GetSection("Jwt:Audience").Value,
                    Issuer = configuration.GetSection("Jwt:Issuer").Value,
                    SecretKey = configuration.GetSection("Jwt:SecretKey").Value,
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = jwtOptions.SymmetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                };
            });
        }
    }
}
