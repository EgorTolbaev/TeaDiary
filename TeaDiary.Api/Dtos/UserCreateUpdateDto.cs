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
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Фамилия пользователя (необязательное поле)
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Отчество пользователя (необязательное поле)
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Email пользователя. Обязательное, формат email проверяется.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя в открытом виде. Обязательное поле.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
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