using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaDiary.Api.Data;
using TeaDiary.Api.Dtos;
using TeaDiary.Api.Exceptions;
using TeaDiary.Api.Models;

namespace TeaDiary.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с чаем.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TeaController : ControllerBase
    {
        private readonly TeaDiaryContext _context;

        public TeaController(TeaDiaryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список всех чаев.
        /// </summary>
        /// <returns>Список чаев.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeaReadDto>>> GetTeas()
        {
            List<Tea> tea = await _context.Teas.Include(t => t.TeaType).ToListAsync();
            List<TeaReadDto> teaDtos = tea.Select(t => new TeaReadDto
            {
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                Name = t.Name,
                TeaTypeId = t.TeaTypeId,
                YearCollection = t.YearCollection,
                Quantity = t.Quantity,
                Price = t.Price,
                LinkPurchase = t.LinkPurchase,
                WouldBuyAgain = t.WouldBuyAgain,
                Description = t.Description,
                UserId = t.UserId,
                LinkToPhoto = t.LinkToPhoto

            }).ToList();

            return teaDtos;
        }

        /// <summary>
        /// Получить чай по Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор чая.</param>
        /// <returns>Информация о чае или 404, если не найдено.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeaReadDto>> GetTea(Guid id)
        {
            Tea? tea = await _context.Teas.Include(t => t.TeaType).FirstOrDefaultAsync(t => t.Id == id) ?? throw new NotFoundException("Чай не найден");
            TeaReadDto teaReadDto = new()
            {
                Id = tea.Id,
                CreatedAt = tea.CreatedAt,
                Name = tea.Name,
                TeaTypeId = tea.TeaTypeId,
                YearCollection = tea.YearCollection,
                Quantity = tea.Quantity,
                Price = tea.Price,
                LinkPurchase = tea.LinkPurchase,
                WouldBuyAgain = tea.WouldBuyAgain,
                Description = tea.Description,
                UserId = tea.UserId,
                LinkToPhoto = tea.LinkToPhoto
            };

            return teaReadDto;
        }

        /// <summary>
        /// Создать новый чай.
        /// </summary>
        /// <param name="teaCreateDto">Данные для создания чая.</param>
        /// <returns>Созданный чай с кодом 201.</returns>
        [HttpPost]
        public async Task<ActionResult<TeaCreateDto>> PostTea([FromBody] TeaCreateDto teaCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Tea tea = new()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = teaCreateDto.Name,
                TeaTypeId = teaCreateDto.TeaTypeId,
                YearCollection = teaCreateDto.YearCollection,
                Quantity = teaCreateDto.Quantity,
                Price = teaCreateDto.Price,
                LinkPurchase = teaCreateDto.LinkPurchase,
                WouldBuyAgain = teaCreateDto.WouldBuyAgain,
                Description = teaCreateDto.Description,
                UserId = teaCreateDto.UserId,
                LinkToPhoto = teaCreateDto.LinkToPhoto
            };

            _context.Teas.Add(tea);
            await _context.SaveChangesAsync();

            TeaReadDto teaReadDto = new()
            {
                Id = tea.Id,
                CreatedAt = tea.CreatedAt,
                Name = tea.Name,
                TeaTypeId = tea.TeaTypeId,
                YearCollection = tea.YearCollection,
                Quantity = tea.Quantity,
                Price = tea.Price,
                LinkPurchase = tea.LinkPurchase,
                WouldBuyAgain = tea.WouldBuyAgain,
                Description = tea.Description,
                UserId = tea.UserId,
                LinkToPhoto = tea.LinkToPhoto
            };

            return CreatedAtAction(nameof(GetTea), new { id = tea.Id }, teaReadDto);
        }

        /// <summary>
        /// Обновить данные существующего чая.
        /// </summary>
        /// <param name="id">Id чай для обновления.</param>
        /// <param name="teaUpdateDto">Обновленные данные чая.</param>
        /// <returns>204 NoContent при успешном обновлении, ошибки при невалидных данных или 404 при отсутствии впечатления.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTea(Guid id, [FromBody] TeaUpdateDto teaUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Tea? tea = await _context.Teas.FindAsync(id);
            if (tea == null)
                return NotFound();

            // Обновляем разрешённые поля
            tea.Name = teaUpdateDto.Name;
            tea.TeaTypeId = teaUpdateDto.TeaTypeId;
            tea.YearCollection = teaUpdateDto.YearCollection;
            tea.Quantity = teaUpdateDto.Quantity;
            tea.Price = teaUpdateDto.Price;
            tea.LinkPurchase = teaUpdateDto.LinkPurchase;
            tea.WouldBuyAgain = teaUpdateDto.WouldBuyAgain;
            tea.Description = teaUpdateDto.Description;
            tea.LinkToPhoto = teaUpdateDto.LinkToPhoto;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Удалить чай.
        /// </summary>
        /// <param name="id">Id чая для удаления.</param>
        /// <returns>204 NoContent при успешном удалении или 404, если чай не найден.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTea(Guid id)
        {
            Tea? tea = await _context.Teas.FindAsync(id) ?? throw new NotFoundException("Чай не найден");
            _context.Teas.Remove(tea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Получить список всех чаев пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TeaReadDto>>> GetTeasByUser(Guid userId)
        {
            List<Tea> teas = await _context.Teas
                        .Where(t => t.UserId == userId)
                        .Include(t => t.TeaType)
                        .ToListAsync();

            List<TeaReadDto> teaDtos = teas.Select(t => new TeaReadDto
            {
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                Name = t.Name,
                TeaTypeId = t.TeaTypeId,
                YearCollection = t.YearCollection,
                Quantity = t.Quantity,
                Price = t.Price,
                LinkPurchase = t.LinkPurchase,
                WouldBuyAgain = t.WouldBuyAgain,
                Description = t.Description,
                UserId = t.UserId,
                LinkToPhoto = t.LinkToPhoto
            }).ToList();

            return teaDtos;
        }

        /// <summary>
        /// Проверяет, существует ли чай с заданным идентификатором.
        /// </summary>
        /// <param name="id">Уникальный идентификатор чая.</param>
        /// <returns>True, если чай существует; иначе False.</returns>
        private bool TeaExists(Guid id)
        {
            return _context.Teas.Any(e => e.Id == id);
        }
    }
}
