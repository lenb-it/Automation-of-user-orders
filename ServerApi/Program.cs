using ServerApi.Services;

namespace ServerApi
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            _configuration = builder.Configuration;
            ConfigureServices(builder.Services);

            var app = builder.Build();
            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            AuthenticationConfigureService.Configure(services, _configuration);
            DataBaseContextConfigureService.Configure(services, _configuration);

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            SwaggerConfigureService.Configure(services);

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("MyPolicy");

            app.MapControllers();
        }
    }
}