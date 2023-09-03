using ServerApi.Contexts;
using ServerApi.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ServerApi.Services
{
public class DataBaseContextConfigureService
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LocalConnect")));

        services.AddDefaultIdentity<User>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole<int>>()
        .AddEntityFrameworkStores<RestaurantDbContext>()
        .AddUserManager<UserManager<User>>()
        .AddSignInManager<SignInManager<User>>();
    }
}
}
