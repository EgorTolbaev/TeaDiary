using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TeaDiary.Api.Validation
{
    public class FlexibleDateFormatAttribute : ValidationAttribute
    {
        private static readonly Regex yearRegex = new(@"^\d{4}$");
        private static readonly Regex seasonYearRegex = new(@"^(лето|зима|весна|осень) \d{4}$", RegexOptions.IgnoreCase);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is string str)
            {
                str = str.Trim();

                // Попытка парсинга как дата (ISO формат)
                if (DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    return ValidationResult.Success;

                // Проверка формата "год"
                if (yearRegex.IsMatch(str))
                    return ValidationResult.Success;

                // Проверка формата "сезон год"
                if (seasonYearRegex.IsMatch(str))
                    return ValidationResult.Success;

                return new ValidationResult("Поле должно быть датой (гггг-MM-дд), годом или 'лето 2024', 'зима 2023' и т.п.");
            }

            return new ValidationResult("Некорректный тип данных");
        }
    }
}
