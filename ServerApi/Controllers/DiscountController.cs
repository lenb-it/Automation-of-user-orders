using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Contexts;
using ServerApi.DbModels;

namespace ServerApi.Controllers
{
    [Authorize]
    public class DiscountController : BaseApiController
    {
        private readonly RestaurantDbContext _dbContext;

        public DiscountController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddRange([FromBody] DiscountAddRange model)
        {
            if (!ModelState.IsValid || model.DateStart >= model.DateEnd) 
                return BadRequest();


            var utcNow = DateTime.UtcNow;
            var discounts = model.DishAndDrinkIds
                .Select(id => new DiscountDishAndDrink
                {
                    ChangeAt = utcNow,
                    CreateAt = utcNow,
                    DateEnd = model.DateEnd,
                    DateStart = model.DateStart,
                    DiscountValue = model.Discount,
                    DishAndDrinkId = id
                })
                .ToArray();

            await _dbContext.AddRangeAsync(discounts);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetLastFiveHundred()
        {
            var discounts = _dbContext.DiscountDishesAndDrinks
                .OrderByDescending(x => x.Id)
                .Take(500)
                .Include(x => x.DishAndDrink)
                .Select(x => new DiscountViewModel
                {
                    DateEnd = x.DateEnd,
                    DateStart = x.DateStart,
                    DiscountValue = x.DiscountValue,
                    DishAndDrinkName = x.DishAndDrink.Name,
                    Id = x.Id,
                    DiscountPrice = x.DishAndDrink.Price * (100 - x.DiscountValue) / 100f, 
                })
                .ToList();

            return Ok(discounts);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id <= 0) BadRequest();

            var discount = await _dbContext.DiscountDishesAndDrinks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (discount is null) return BadRequest("Скидка не найдена");

            _dbContext.DiscountDishesAndDrinks.Remove(discount);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DiscountViewModel model)
        {
            if (!ModelState.IsValid || model.DateStart >= model.DateEnd) BadRequest();

            var discount = await _dbContext.DiscountDishesAndDrinks
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (discount is null) return BadRequest("Скидка не найдена");

            discount.DateStart = model.DateStart;
            discount.DateEnd = model.DateEnd;
            discount.DiscountValue = model.DiscountValue;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
