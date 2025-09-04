namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для передачи данных пользователя на чтение (без пароля)
    /// </summary>
    public class UserReadDto
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
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
        /// Email пользователя
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// Дата и время создания пользователя (UTC)
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Идентификатор аватара пользователя (может быть null)
        /// </summary>
        public string? AvatarId { get; set; }
    }
}
