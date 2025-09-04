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
        public required string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
