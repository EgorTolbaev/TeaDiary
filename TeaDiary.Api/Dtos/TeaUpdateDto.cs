namespace TeaDiary.Api.Dtos
{
    /// <summary>
    /// DTO для создания и обновления чая.
    /// </summary>
    public class TeaUpdateDto
    {
        /// <summary>
        /// Названия чая
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Внешний ключ на TeaType
        /// </summary>
        public Guid? TeaTypeId { get; set; }
        /// <summary>
        /// Год сбора
        /// </summary>
        public string? YearCollection { get; set; }
        /// <summary>
        /// Количество чая (в граммах)
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
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
        public string? Description { get; set; }
        /// <summary>
        /// Ссылка на фото
        /// </summary>
        public string? LinkToPhoto { get; set; }
    }
}
