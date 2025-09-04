using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Models
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class User
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow; // Автоматическая установка времени создания в UTC
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Имя обязательно")]
        public required string FirstName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// Email пользователя
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public required string Email { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Аватар пользователя
        /// </summary>
        public string? AvatarId { get; set; }
        /// <summary>
        /// Коллекция чаёв, относящихся к этому пользователю
        /// </summary>
        public List<Tea> Teas { get; set; } = [];
    }
}