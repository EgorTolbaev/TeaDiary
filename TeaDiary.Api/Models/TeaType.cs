using System.ComponentModel.DataAnnotations;

namespace TeaDiary.Api.Models
{
    /// <summary>
    /// Модель типа чая
    /// </summary>
    public class TeaType
    {
        /// <summary>
        /// Id типа чая
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Тип чая
        /// </summary>
        [Required]
        public required string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Коллекция чаёв, относящихся к этому типу
        /// </summary>
        public List<Tea> Teas { get; set; } = [];
    }
}