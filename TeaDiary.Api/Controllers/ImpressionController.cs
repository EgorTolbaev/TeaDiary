using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaDiary.Api.Data;
using TeaDiary.Api.Dtos;
using TeaDiary.Api.Exceptions;
using TeaDiary.Api.Models;

namespace TeaDiary.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с впечатлениями о чае.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ImpressionController : ControllerBase
    {
        private readonly TeaDiaryContext _context;

        public ImpressionController(TeaDiaryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список всех впечатлений о чае.
        /// </summary>
        /// <returns>Список впечатлений о чае.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImpressionReadDto>>> GetImpressions()
        {
            List<Impression> impression = await _context.Impressions.ToListAsync();

            List<ImpressionReadDto> impressionDtos = impression.Select(i => new ImpressionReadDto
            {
                Id = i.Id,
                Text = i.Text,
                CreatedAt = i.CreatedAt,
                TeaId = i.TeaId,
                UserId = i.UserId
            }).ToList();

            return impressionDtos;
        }

        /// <summary>
        /// Получить впечатление по Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор впечатления.</param>
        /// <returns>Информация о впечатлении или 404, если не найдено.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ImpressionReadDto>> GetImpression(Guid id)
        {
            Impression? impression = await _context.Impressions
                .Include(i => i.Tea)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id) ?? throw new NotFoundException("Впечатление не найдено");
            ImpressionReadDto impressionDtos = new()
            {
                Id = impression.Id,
                Text = impression.Text,
                CreatedAt = impression.CreatedAt,
                TeaId = impression.TeaId,
                UserId = impression.UserId
            };

            return impressionDtos;
        }

        /// <summary>
        /// Создать новое впечатление.
        /// </summary>
        /// <param name="impressionDto">Данные для создания впечатления.</param>
        /// <returns>Созданное впечатление с кодом 201.</returns>
        [HttpPost]
        public async Task<ActionResult<ImpressionCreateDto>> PostImpression([FromBody] ImpressionCreateDto impressionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверяем существование связанного чая
            if (!await _context.Teas.AnyAsync(t => t.Id == impressionDto.TeaId))
                return BadRequest($"Tea with Id {impressionDto.TeaId} does not exist.");

            // Проверяем существование связанного пользователя
            if (!await _context.Users.AnyAsync(u => u.Id == impressionDto.UserId))
                return BadRequest($"User with Id {impressionDto.UserId} does not exist.");

            Impression impression = new()
            {
                Id = Guid.NewGuid(),
                Text = impressionDto.Text,
                TeaId = impressionDto.TeaId,
                UserId = impressionDto.UserId
            };

            _context.Impressions.Add(impression);
            await _context.SaveChangesAsync();

            ImpressionReadDto impressionReadDto = new()
            {
                Id = impression.Id,
                Text = impression.Text,
                TeaId = impression.TeaId,
                UserId = impression.UserId
            };

            return CreatedAtAction(nameof(GetImpression), new { id = impression.Id }, impressionReadDto);
        }

        /// <summary>
        /// Обновить данные существующего впечатления.
        /// </summary>
        /// <param name="id">Id впечатления для обновления.</param>
        /// <param name="impressionUpdateDto">Обновленные данные впечатления в формате DTO.</param>
        /// <returns>204 NoContent при успешном обновлении, 400 при ошибках валидации или 404 при отсутствии впечатления.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImpression(Guid id, [FromBody] ImpressionUpdateDto impressionUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Impression? impression = await _context.Impressions.FindAsync(id) ?? throw new NotFoundException("Впечатление не найдено");
            impression.Text = impressionUpdateDto.Text;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                 if (!ImpressionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Удалить впечатление.
        /// </summary>
        /// <param name="id">Id впечатления для удаления.</param>
        /// <returns>204 NoContent при успешном удалении или 404, если впечатление не найдено.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImpression(Guid id)
        {
            Impression? impression = await _context.Impressions.FindAsync(id) ?? throw new NotFoundException("Впечатление не найдено");
            _context.Impressions.Remove(impression);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Проверяет, существует ли впечатление с заданным идентификатором.
        /// </summary>
        /// <param name="id">Уникальный идентификатор впечатления.</param>
        /// <returns>True, если впечатление существует; иначе False.</returns>
        private bool ImpressionExists(Guid id)
        {
            return _context.Impressions.Any(e => e.Id == id);
        }
    }
}
