namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для передачи данных типа чая
    /// </summary>
    public class TeaTypeReadDto
    {
        /// <summary>
        /// Id типа чая
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Тип чая
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
