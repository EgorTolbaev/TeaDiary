using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления типов чая.
    /// </summary>
    public class TeaTypeCreateUpdateDto
    {
        /// <summary>
        /// Тип чая
        /// </summary>
        [Required(ErrorMessage = "Тип чая обязательный")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина имени от 2 до 50 символов")]
        public required string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [StringLength(500, ErrorMessage = "Описание может содержать не более 500 символов")]
        public string? Description { get; set; }
    }
}
