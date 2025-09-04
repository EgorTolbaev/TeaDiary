namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для передачи данных впечатлений на чтение.
    /// </summary>
    public class ImpressionReadDto
    {
        /// <summary>
        /// Id впечатления
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Текст впечатления
        /// </summary>
        public string Text { get; set; } = null!;
        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Внешний ключ на чай
        /// </summary>
        public Guid TeaId { get; set; }     
        /// <summary>
        /// Внешний ключ на пользователя
        /// </summary>
        public Guid UserId { get; set; }
    }
}
