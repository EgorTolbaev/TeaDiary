using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления впечатлений.
    /// </summary>
    public class ImpressionUpdateDto
    {
        /// <summary>
        /// Текст впечатления
        /// </summary>
        [Required(ErrorMessage = "Текст впечатления обязательный")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Длина текста должна быть от 10 до 500 символов")]
        public string Text { get; set; } = null!;
    }
}
