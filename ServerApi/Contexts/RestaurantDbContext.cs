using ServerApi.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerApi.Contexts
{
    public class RestaurantDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserReservation> UserReservations { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDishAndDrink> OrderDishesAndDrinks { get; set; }

        public DbSet<DiscountDishAndDrink> DiscountDishesAndDrinks { get; set; }

        public DbSet<DishAndDrink> DishesAndDrinks { get; set; }

        public DbSet<DishAndDrinkCategory> DishAndDrinkCategories { get; set; }
    }
}
