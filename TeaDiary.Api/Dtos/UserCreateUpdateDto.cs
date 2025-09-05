using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления пользователя (с паролем в открытом виде)
    /// </summary>
    public class UserCreateUpdateDto
    {
        /// <summary>
        /// Имя пользователя. Обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина имени от 2 до 50 символов")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Фамилия пользователя (необязательное поле)
        /// </summary>
        [StringLength(50, ErrorMessage = "Длина не более 50 символов")]
        public string? LastName { get; set; }

        /// <summary>
        /// Отчество пользователя (необязательное поле)
        /// </summary>
        [StringLength(50, ErrorMessage = "Длина не более 50 символов")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Email пользователя. Обязательное, формат email проверяется.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя в открытом виде. Обязательное поле.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Подтверждение пароля пользователя. Должно совпадать с полем <see cref="Password"/>.
        /// Используется для проверки правильности ввода пароля при регистрации или обновлении.
        /// </summary>
        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;

        /// <summary>
        /// Идентификатор аватара пользователя (необязательное поле)
        /// </summary>
        public string? AvatarId { get; set; }
    }
}