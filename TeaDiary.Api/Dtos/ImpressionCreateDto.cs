using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления впечатлений.
    /// </summary>
    public class ImpressionCreateDto
    {
        /// <summary>
        /// Текст впечатления
        /// </summary>
        [Required(ErrorMessage = "Текст впечатления обязательный")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Длина текста должна быть от 10 до 500 символов")]
        public string Text { get; set; } = null!;
        /// <summary>
        /// Внешний ключ на чай
        /// </summary>
        [Required(ErrorMessage = "Id чая обязателен")]
        public Guid TeaId { get; set; }
        /// <summary>
        /// Внешний ключ на пользователя
        /// </summary>
        [Required(ErrorMessage = "Id пользователя обязателен")]
        public Guid UserId { get; set; }
    }
}
