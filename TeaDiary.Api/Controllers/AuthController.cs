using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaDiary.Api.Data;
using TeaDiary.Api.Dtos;
using TeaDiary.Api.Models;

namespace TeaDiary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TeaDiaryContext _context;
        private readonly JwtTokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthController(TeaDiaryContext context, JwtTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserReadDto>> Register([FromBody] UserCreateUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверяем существование пользователя с таким email
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                return Conflict(new { message = "Пользователь с таким Email уже существует" });

            User user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                MiddleName = userDto.MiddleName,
                Email = userDto.Email,
                CreatedAt = DateTime.UtcNow,
                AvatarId = userDto.AvatarId ?? null
            };

            // Хэшируем пароль
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            UserReadDto createdUser = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                AvatarId = user.AvatarId
            };

            return CreatedAtAction("GetUser", "User", new { id = user.Id }, createdUser);
        }

        /// <summary>
        /// Логин пользователя и выдача JWT токена.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized(new { message = "Неверный Email или пароль" });

            var check = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (check == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Неверный Email или пароль" });

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Получить данные данные о себе
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserReadDto>> GetCurrentUser()
        {
            // Получаем userId из claims токена
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized();

            // Ищем пользователя в базе
            User? user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            // Формируем DTO для ответа
            UserReadDto userDto = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                AvatarId = user.AvatarId
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Обновить данные профиля
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserCreateUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.MiddleName = userDto.MiddleName;
            user.Email = userDto.Email;
            user.AvatarId = userDto.AvatarId;

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == userId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

    }
}
