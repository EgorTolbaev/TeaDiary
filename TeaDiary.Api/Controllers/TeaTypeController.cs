using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaDiary.Api.Data;
using TeaDiary.Api.Dtos;
using TeaDiary.Api.Exceptions;
using TeaDiary.Api.Models;

namespace TeaDiary.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с типами чая.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TeaTypeController : ControllerBase
    {
        private readonly TeaDiaryContext _context;

        public TeaTypeController(TeaDiaryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список всех типов чая.
        /// </summary>
        /// <returns>Список типов чая.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeaTypeReadDto>>> GetTeaTypes()
        {
            List<TeaType> teaTypes = await _context.TeaTypes.ToListAsync();
            List<TeaTypeReadDto> teaTypeDtos = teaTypes.Select(i => new TeaTypeReadDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
            }).ToList();

            return teaTypeDtos;
        }

        /// <summary>
        /// Получить тип чая по Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор типа чая.</param>
        /// <returns>Информация о типе чае или 404, если не найдено.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeaTypeReadDto>> GetTeaType(Guid id)
        {
            TeaType? teaType = await _context.TeaTypes.FirstOrDefaultAsync(t => t.Id == id) ?? throw new NotFoundException("Тип чая не найден");
            TeaTypeReadDto teaTypeReadDto = new()
            {
                Id = teaType.Id,
                Name = teaType.Name,
                Description = teaType.Description
            };

            return teaTypeReadDto;
        }

        /// <summary>
        /// Создать новый тип чай.
        /// </summary>
        /// <param name="teaTypeDto">Данные для создания типа чая.</param>
        /// <returns>Созданный тип чай с кодом 201.</returns>
        [HttpPost]
        public async Task<ActionResult<TeaTypeCreateUpdateDto>> PostTeaType([FromBody] TeaTypeCreateUpdateDto teaTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TeaType teaType = new()
            {
                Id = Guid.NewGuid(),
                Name = teaTypeDto.Name,
                Description = teaTypeDto.Description,
            };

            _context.TeaTypes.Add(teaType);
            await _context.SaveChangesAsync();

            TeaTypeReadDto teaTypeReadDto = new()
            {
                Id = teaType.Id,
                Name = teaType.Name,
                Description = teaType.Description
            };

            return CreatedAtAction(nameof(GetTeaType), new { id = teaType.Id }, teaTypeReadDto);
        }

        /// <summary>
        /// Обновить данные существующего типа чая.
        /// </summary>
        /// <param name="id">Id типа чай для обновления.</param>
        /// <param name="teaTypeCreateUpdateDto">Обновленные данные типа чая.</param>
        /// <returns>204 NoContent при успешном обновлении, ошибки при невалидных данных или 404 при отсутствии типа чая.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeaType(Guid id, [FromBody] TeaTypeCreateUpdateDto teaTypeCreateUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TeaType? teaType = await _context.TeaTypes.FindAsync(id) ?? throw new NotFoundException("Тип чая не найден");

            // Обновляем разрешённые поля
            teaType.Name = teaTypeCreateUpdateDto.Name;
            teaType.Description = teaTypeCreateUpdateDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeaTypeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Удалить тип чай.
        /// </summary>
        /// <param name="id">Id типа чая для удаления.</param>
        /// <returns>204 NoContent при успешном удалении или 404, если тип чая не найден.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTea(Guid id)
        {
            TeaType? teaType = await _context.TeaTypes.FindAsync(id) ?? throw new NotFoundException("Тип чая не найден");
            _context.TeaTypes.Remove(teaType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Проверяет, существует ли тип чая с заданным идентификатором.
        /// </summary>
        /// <param name="id">Уникальный идентификатор типа чая.</param>
        /// <returns>True, если тип чая существует; иначе False.</returns>
        private bool TeaTypeExists(Guid id)
        {
            return _context.TeaTypes.Any(e => e.Id == id);
        }
    }
}