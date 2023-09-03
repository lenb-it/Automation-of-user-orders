using ServerApi.Contexts;
using ServerApi.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Business.Models;
using Microsoft.AspNetCore.Identity;
using Business.Constants;

namespace ServerApi.Controllers
{
    public class ReservationController : BaseApiController
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ReservationController(RestaurantDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPut]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Update([FromBody] Reservation model)
        {
            if (!ModelState.IsValid) return BadRequest("хая ");

            var reservation = await _dbContext.UserReservations
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (reservation is null) return BadRequest("Резервирование не найдено");

            reservation.Date = model.Date;
            reservation.CountPeople = model.CountPeople;
            reservation.IsValid = model.IsConfirmed;
            reservation.ChangeAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var reservation = await _dbContext.UserReservations
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reservation is null) return BadRequest("Резервирование не найдено");

            _dbContext.UserReservations.Remove(reservation);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] Business.Models.UserReservation model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null) return BadRequest("Пользователь не найден");

            var reservation = new DbModels.UserReservation
            {
                UserId = user.Id,
                User = user,
                IsValid = false,
                CountPeople = model.CountPeople,
                Date = model.Date,
                CreateAt = DateTime.UtcNow,
                ChangeAt = DateTime.UtcNow,
            };

            _dbContext.UserReservations.Add(reservation);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> GetLastFiveHundred()
        {
            var reservations = await _dbContext.UserReservations
                .OrderByDescending(x => x.Id)
                .Take(500)
                .Include(x => x.User)
                .Select(x => new Reservation
                {
                    Date = x.Date,
                    Email = x.User.Email,
                    Name = $"{x.User.FirstName} {x.User.LastName}",
                    CountPeople = x.CountPeople,
                    IsConfirmed = x.IsValid,
                    PhoneNumber = x.User.PhoneNumber,
                    Id = x.Id,
                })
                .ToListAsync();

            return Ok(reservations);
        }
    }
}
