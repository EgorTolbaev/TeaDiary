namespace TeaDiary.Api.Models
{
    /// <summary>
    /// Модель ответа об ошибке.
    /// </summary>
    public class ApiErrorResponse
    {
        public string Code { get; set; } = "error";
        public string Message { get; set; } = "Произошла ошибка";
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}