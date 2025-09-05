using System.ComponentModel.DataAnnotations;
using TeaDiary.Api.Validation;

namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления чая.
    /// </summary>
    public class TeaCreateDto
    {
        /// <summary>
        /// Названия чая
        /// </summary>
        [Required(ErrorMessage = "Имя чая обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина имени от 2 до 50 символов")]
        public required string Name { get; set; }
        /// <summary>
        /// Внешний ключ на TeaType
        /// </summary>
        [Required(ErrorMessage = "Id типа чая обязателен")]
        public Guid? TeaTypeId { get; set; }
        /// <summary>
        /// Год сбора
        /// </summary>
        [FlexibleDateFormat]
        public string? YearCollection { get; set; }
        /// <summary>
        /// Количество чая (в граммах)
        /// </summary>
        [Range(1, 10000, ErrorMessage = "Количество должно быть от 1 до 10000")]
        public int Quantity { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        [Range(0, 1000000, ErrorMessage = "Цена должна быть от 0 до 1 000 000")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Ссылка на покупку
        /// </summary>
        public string? LinkPurchase { get; set; }
        /// <summary>
        /// Флаг «Купил бы снова» 
        /// </summary>
        public bool? WouldBuyAgain { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [StringLength(500, ErrorMessage = "Описание может содержать не более 500 символов")]
        public string? Description { get; set; }
        /// <summary>
        /// Внешний ключ на пользователя
        /// </summary>
        [Required(ErrorMessage = "Id пользователя обязателен")]
        public required Guid UserId { get; set; }
        /// <summary>
        /// Ссылка на фото
        /// </summary>
        public string? LinkToPhoto { get; set; }
    }
}
