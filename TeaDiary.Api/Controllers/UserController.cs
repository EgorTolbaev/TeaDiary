using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TeaDiary.Api.Data;
using TeaDiary.Api.Models;
using TeaDiary.Api.Dtos;
using TeaDiary.Api.Exceptions;

namespace TeaDiary.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователем.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly TeaDiaryContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserController(TeaDiaryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список всех пользователей (без паролей).
        /// </summary>
        /// <returns>Список пользователей в безопасном формате.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();

            List<UserReadDto> userDtos = users.Select(u => new UserReadDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                AvatarId = u.AvatarId
            }).ToList();

            return userDtos;
        }

        /// <summary>
        /// Получить пользователя по Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя.</param>
        /// <returns>Информация о пользователе или 404, если не найден.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetUser(Guid id)
        {
            User? user = await _context.Users.FindAsync(id) ?? throw new NotFoundException("Пользователь не найден");
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

            return userDto;
        }

        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="userDto">Данные для создания пользователя.</param>
        /// <returns>Созданный пользователь (без пароля) с кодом 201.</returns>
        /// <response code="201">Созданный пользователь (без пароля)</response>
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> PostUser([FromBody] UserCreateUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                MiddleName = userDto.MiddleName,
                Email = userDto.Email,
                CreatedAt = DateTime.UtcNow,
                AvatarId = userDto.AvatarId
            };

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

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUser);
        }

        /// <summary>
        /// Обновить данные существующего пользователя.
        /// </summary>
        /// <param name="id">Id пользователя для обновления.</param>
        /// <param name="userDto">Обновленные данные пользователя.</param>
        /// <returns>204 NoContent при успешном обновлении, ошибки при невалидных данных или 404 при отсутствии пользователя.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, [FromBody] UserCreateUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User? user = await _context.Users.FindAsync(id) ?? throw new NotFoundException("Пользователь не найден");
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
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id">Id пользователя для удаления.</param>
        /// <returns>204 NoContent при успешном удалении или 404, если пользователь не найден.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            User? user = await _context.Users.FindAsync(id) ?? throw new NotFoundException("Пользователь не найден");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Проверяет, существует ли пользователь с данным id.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>True если пользователь существует, иначе false.</returns>
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
