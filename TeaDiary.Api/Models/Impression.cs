using System;
using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Models
{
    /// <summary>
    /// Модель впечатления пользователя о чае
    /// </summary>
    public class Impression
    {
        public Impression()
        {
            CreatedAt = DateTime.UtcNow; // Автоматическая установка времени создания в UTC
        }
        /// <summary>
        /// Id впечатления
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Текст впечатления
        /// </summary>
        [Required]
        public string Text { get; set; } = null!;
        /// <summary>
        /// Дата добавления
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Внешний ключ на чай
        /// </summary>
        [Required]
        public Guid TeaId { get; set; }
        /// <summary>
        /// Навигационное свойство на чай
        /// </summary>
        public Tea Tea { get; set; } = null!;
        /// <summary>
        /// Внешний ключ на пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Навигационное свойство на пользователя
        /// </summary>
        public User User { get; set; } = null!;
    }
}
