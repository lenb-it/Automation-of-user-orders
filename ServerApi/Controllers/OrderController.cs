using System.Reflection.Metadata.Ecma335;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ServerApi.Contexts;
using ServerApi.DbModels;

namespace ServerApi.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public OrderController(RestaurantDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderAdd model)
        {
            if (!ModelState.IsValid || model.DishAndDrinks?.Count == 0)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return BadRequest("Пользователь не найден");

            var order = CreateDefaultOrder(user, model.NumberPlace);
            var dishesIds = model.DishAndDrinks.Select(x => x.Id).ToList();
            var orderDishesAll = await _dbContext.DishesAndDrinks
                .Where(x => dishesIds.Contains(x.Id))
                .ToListAsync();

            order.Price = GetPrice(model.DishAndDrinks, dishesIds, orderDishesAll);

            _dbContext.Orders.Add(order);
            await _dbContext.OrderDishesAndDrinks
                .AddRangeAsync(GetAddDishes(model, order, orderDishesAll));
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetLastFiveHundred()
        {
            var orders = await _dbContext.Orders
                .OrderByDescending(x => x.Id)
                .Take(500)
                .Include(x => x.User)
                .Select(x => new Business.Models.Order
                {
                    Id = x.Id,
                    IsPaid = x.IsPaid,
                    NumberPlace = x.NumberPlace,
                    Price = x.Price,
                    Email = x.User.Email,
                    Name = $"{x.User.FirstName} {x.User.LastName}",
                    Date = x.CreateAt,
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDishAndDrinks([FromQuery] int id)
        {
            if (id <= 0) return BadRequest();

            var dishes = _dbContext.OrderDishesAndDrinks
                .Where(x => x.OrderId == id)
                .Include(x => x.DishAndDrink)
                .Select(x => new OrderDishAndDrinkViewModel
                {
                    ChangeAt = x.ChangeAt,
                    Id = x.Id,
                    Count = x.Count,
                    Name = x.DishAndDrink.Name,
                })
                .ToList();

            return Ok(dishes);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderDishAndDrinks([FromBody] OrderDishAndDrinkUpdate model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == model.OrderId);

            if (order is null) return BadRequest("Заказ не найден");

            await UpdateOrderDishes(model, order);

            var orderDishesAll = await _dbContext.OrderDishesAndDrinks
                .Where(x => x.OrderId == order.Id)
                .Include(x => x.DishAndDrink)
                .Select(x => x.DishAndDrink)
                .ToListAsync();

            var dishesIds = orderDishesAll.Select(x => x.Id).ToList();
            var dishesCount = await _dbContext.OrderDishesAndDrinks
                .Where(x => x.OrderId == order.Id)
                .Include(x => x.DishAndDrink)
                .Select(x => new DishAndDrinkAddOrder
                {
                    Count = x.Count,
                    Id = x.DishAndDrink.Id
                })
                .ToListAsync();

            order.ChangeAt = DateTime.UtcNow;
            order.Price = GetPrice(dishesCount, dishesIds, orderDishesAll);

            await _dbContext.SaveChangesAsync();

            return Ok(order.Price);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Business.Models.Order model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (order is null) return BadRequest("Заказ не найден");

            order.NumberPlace = model.NumberPlace;
            order.IsPaid = model.IsPaid;
            order.ChangeAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order is null) return BadRequest("Заказ не найден");

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetChequeInfo([FromQuery] int orderId)
        {
            if (orderId <= 0) return BadRequest();

            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderId);

            if (order is null) return BadRequest("Заказ не найден");

            var dishes = await _dbContext.OrderDishesAndDrinks
                .Where(x => x.OrderId == order.Id)
                .Include(x => x.DishAndDrink)
                .Select(x => new ChequeDish
                {
                    Count = x.Count,
                    LastChange = x.ChangeAt,
                    Name = x.DishAndDrink.Name,
                    Price = x.DishAndDrink.Price,
                    Id = x.DishAndDrinkId,
                })
                .ToListAsync();

            var discounts = await _dbContext.DiscountDishesAndDrinks.ToListAsync();

            foreach(var dish in dishes)
            {
                var dishDisounts = discounts
                    .Where(x => x.DishAndDrinkId == dish.Id
                        && x.DateStart <= dish.LastChange
                        && x.DateEnd >= dish.LastChange)
                    .ToList();

                if (dishDisounts.Count == 0) continue;

                foreach(var discount in dishDisounts)
                    dish.Price = (float)Math.Round(dish.Price - dish.Price * discount.DiscountValue / 100f, 2);
            }

            var totalPrice = dishes.Sum(x => x.TotalPrice);
            order.Price = totalPrice;
            order.IsPaid = true;
            await _dbContext.SaveChangesAsync();

            return Ok(new ChequeInfo
            {
                ChequeDishes = dishes,
                Date = order.ChangeAt,
                TotalPrice = order.Price,
            });
        }

        [HttpGet("{dateStart}/{dateEnd}")]
        public async Task<IActionResult> GetStatistics([FromRoute] DateTime dateStart, [FromRoute] DateTime dateEnd)
        {
            if (dateStart > dateEnd) return BadRequest("1");

            var orders = await _dbContext.Orders
                .Where(x => x.ChangeAt >= dateStart && x.ChangeAt <= dateEnd)
                .ToListAsync();

            var reservationCount = _dbContext.UserReservations
                .Where(x => x.Date >= dateStart && x.Date <= dateEnd)
                .Count();

            var dishCountById = await _dbContext.OrderDishesAndDrinks
                .Include(x => x.Order)
                .Where(x => x.Order.ChangeAt >= dateStart && x.Order.ChangeAt <= dateEnd)
                .GroupBy(x => x.DishAndDrinkId)
                .Select(x => new
                {
                    DishAndDrinkId = x.Key,
                    Count = x.Count(),
                })
                .ToListAsync();

            var dishes = await _dbContext.DishesAndDrinks.ToListAsync();
            var dishesCount = new List<StatisticDish>();

            foreach (var dish in dishes)
            {
                var count = dishCountById.FirstOrDefault(x => x.DishAndDrinkId == dish.Id)?.Count ?? default;
                dishesCount.Add(new StatisticDish
                {
                    Name = dish.Name,
                    Count = count,
                    Price = dish.Price,
                });
            }

            return Ok(new Statistics
            {
                AverageOrderPrice = (float)Math.Round(orders.Average(x => x.Price), 2),
                CountOrders = orders.Count,
                CountReservations = reservationCount,
                MaxOrderPrice = orders.Max(x => x.Price),
                MinOrderPrice = orders.Min(x => x.Price),
                Dishes = dishesCount,
            });
        }

        private async Task UpdateOrderDishes(OrderDishAndDrinkUpdate model, DbModels.Order order)
        {
            var dishes = _dbContext.DishesAndDrinks.ToList();
            var orderOldDishes = await _dbContext.OrderDishesAndDrinks
                            .Where(x => x.OrderId == model.OrderId)
                            .ToListAsync();

            var addDishes = model.AddDishAndDrinkToOrder.Where(x => x.IdOrderDishAndDrink <= 0).ToArray();
            var removeDishes = orderOldDishes
                .Where(x => !model.AddDishAndDrinkToOrder.Any(d => d.IdOrderDishAndDrink == x.Id))
                .ToList();

            foreach (var addDish in model.AddDishAndDrinkToOrder)
            {
                if (addDish.IdOrderDishAndDrink > 0)
                {
                    var updateDish = orderOldDishes.FirstOrDefault(x => x.Id == addDish.IdOrderDishAndDrink);

                    if (updateDish is null) continue;

                    updateDish.Count = addDish.Count;
                    updateDish.ChangeAt = DateTime.UtcNow;
                }
                else
                {
                    var dish = dishes.FirstOrDefault(x => x.Name == addDish.Name);

                    if (dish is null) continue;

                    _dbContext.Add(CreateDefaultOrderDishAndDrink(order, addDish.Count, dish));
                }
            }

            _dbContext.OrderDishesAndDrinks.RemoveRange(removeDishes);
            await _dbContext.SaveChangesAsync();
        }

        private DbModels.Order CreateDefaultOrder(User user, int numberPlace)
        {
            return new DbModels.Order
            {
                ChangeAt = DateTime.UtcNow,
                CreateAt = DateTime.UtcNow,
                IsPaid = false,
                NumberPlace = numberPlace,
                User = user,
                UserId = user.Id,
            };
        }

        private OrderDishAndDrink CreateDefaultOrderDishAndDrink(DbModels.Order order, float count, DishAndDrink currentDish)
        {
            return new OrderDishAndDrink
            {
                Order = order,
                OrderId = order.Id,
                Count = count,
                DishAndDrink = currentDish,
                DishAndDrinkId = currentDish.Id,
                ChangeAt = DateTime.UtcNow,
                CreateAt = DateTime.UtcNow,
            };
        }

        private List<OrderDishAndDrink> GetAddDishes(OrderAdd model, DbModels.Order order, List<DishAndDrink> orderDishesAll)
        {
            var addDishes = new List<OrderDishAndDrink>();

            foreach (var item in model.DishAndDrinks)
            {
                var currentDish = orderDishesAll.Find(x => x.Id == item.Id);

                if (currentDish is null) continue;

                addDishes.Add(CreateDefaultOrderDishAndDrink(order, item.Count, currentDish));
            }

            return addDishes;
        }

        private float GetPrice(List<DishAndDrinkAddOrder> dishAndDrinks, List<int> dishesIds, List<DishAndDrink> orderDishesAll)
        {
            var price = 0f;
            var now = DateTime.UtcNow;
            var discounts = _dbContext.DiscountDishesAndDrinks
                .Where(x => dishesIds.Contains(x.DishAndDrinkId) && x.DateStart <= now && x.DateEnd >= now)
                .ToList();

            var otherDishes = orderDishesAll
                .Where(x => discounts.All(y => y.DishAndDrinkId != x.Id))
                .ToList();

            foreach (var discount in discounts)
            {
                var dishAndDrink = orderDishesAll.FirstOrDefault(x => x.Id == discount.DishAndDrinkId);
                float count = dishAndDrinks.FirstOrDefault(x => x.Id == discount.DishAndDrinkId)?.Count ?? default;
                if (dishAndDrink is null || count == default) continue;

                price += (dishAndDrink.Price * (1 - discount.DiscountValue / 100f)) * count;
            }

            foreach (var dish in otherDishes)
            {
                var dishAndDrink = orderDishesAll.FirstOrDefault(x => x.Id == dish.Id);
                float count = dishAndDrinks.FirstOrDefault(x => x.Id == dish.Id)?.Count ?? default;

                if (dishAndDrink is null || count == default) continue;

                price += dishAndDrink.Price * count;
            }

            return (float)Math.Round(price, 2);
        }
    }
}
