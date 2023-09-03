using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ServerApi.Services
{
    public class SwaggerConfigureService
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        BearerFormat = "JWT",
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Description = "JWT Authorization. \r\n\r\nExample: \"Bearer {token}\"",
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            },
                        },
                        new List<string>()
                    }
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "RestaurantAPI.xml"));
            });
        }
    }
}
