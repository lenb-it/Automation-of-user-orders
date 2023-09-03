using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Business.Constants;
using ServerApi.DbModels;
using Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ServerApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        /// <remarks>
        /// Пользователь, Админ, Работник:
        ///
        ///     POST /Todo
        ///     {
        ///        "email": "user@example.com",
        ///        "password": "string00";
        ///        "email": "admin@mail.ru",
        ///        "password": "secretpas123";
        ///        "email": "workerAnton@mail.ru",
        ///        "password": "123qweasd".
        ///     }
        ///     
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LogIn model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null) return Unauthorized();

            var resultSignIn = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!resultSignIn.Succeeded) return BadRequest(new[] { "error password" });

            var claims = (await _userManager.GetClaimsAsync(user)).ToList();
            var roles = await _userManager.GetRolesAsync(user);
            var resultModel = new LogInResult
            {
                Token = CreateToket(claims),
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? string.Empty,
                UserName = $"{user.FirstName} {user.LastName}",
            };

            return Ok(resultModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return BadRequest("Такой пользователь уже зарегестрирован");

            var registerUser = CreateRegisterUser(model);
            var createResult = await _userManager.CreateAsync(registerUser, model.Password);

            if (!createResult.Succeeded) return BadRequest(createResult.Errors);

            var user = await _userManager.FindByEmailAsync(registerUser.Email);
            var role = RoleNames.User;
            await _userManager.AddToRoleAsync(user, role);

            var claims = CreateUserClaims(user, role);
            await _userManager.AddClaimsAsync(user, claims);
            var resultModel = new LogInResult
            {
                Token = CreateToket(claims),
                Email = user.Email,
                Role = role,
            };

            return Ok(resultModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CheckToken()
        {
            return Ok();
        }

        private string CreateToket(List<Claim> claims)
        {
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(_jwtOptions.Expire)),
                signingCredentials: new SigningCredentials(_jwtOptions.SymmetricSecurityKey, 
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private User CreateRegisterUser(Register model)
        {
            return new User
            {
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                CreatedDate = DateTime.UtcNow,
            };
        }

        private List<Claim> CreateUserClaims(User user, string role)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
            };
        }
    }
}
