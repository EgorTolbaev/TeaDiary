namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления впечатлений.
    /// </summary>
    public class ImpressionCreateUpdateDto
    {
        /// <summary>
        /// Текст впечатления
        /// </summary>
        public string Text { get; set; } = null!;
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
