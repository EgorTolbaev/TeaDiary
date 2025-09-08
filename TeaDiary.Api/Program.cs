using TeaDiary.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using TeaDiary.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// -----------------------------
// Создание билдера приложения
// -----------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Конфигурация базы данных
// -----------------------------
// Используем InMemoryDatabase для тестов, PostgreSQL для боевого режима
if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<TeaDiaryContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<TeaDiaryContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// -----------------------------
// Регистрация контроллеров и Swagger
// -----------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Информация о Swagger-документации
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tea Diary API",
        Description = "API для управления дневником чаёв и впечатлениями",
        TermsOfService = new Uri("https://yourdomain.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Egor Tolbaev",
            Email = "egor05.09.97@gmail.com",
            Url = new Uri("https://yourdomain.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Use under XYZ License",
            Url = new Uri("https://yourdomain.com/license")
        }
    });

    // -----------------------------
    // JWT аутентификация в Swagger
    // -----------------------------
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Введите 'Bearer' [пробел] и ваш токен.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    // -----------------------------
    // XML-комментарии для документации Swagger
    // -----------------------------
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// -----------------------------
// Авторизация и JWT аутентификация
// -----------------------------
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Получаем параметры для JWT из конфигурации
    IConfigurationSection jwtSection = builder.Configuration.GetSection("Jwt");
    string? keyString = jwtSection["Key"];

    // Проверка наличия и длины ключа
    if (string.IsNullOrWhiteSpace(keyString))
        throw new InvalidOperationException("JWT key is missing in configuration!");
    if (keyString.Length < 16)
        throw new InvalidOperationException("JWT key must be at least 16 characters!");

    // Настройка параметров токенов
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString))
    };
});
builder.Services.AddSingleton<JwtTokenService>();

// -----------------------------
// Сборка и запуск приложения
// -----------------------------
WebApplication app = builder.Build();

// Глобальная обработка ошибок (middleware)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Документация Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Принудительное перенаправление на HTTPS
app.UseHttpsRedirection();

// Включение аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Маршрутизация контроллеров
app.MapControllers();

// Запуск приложения
app.Run();
