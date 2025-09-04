using TeaDiary.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TeaDiaryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
