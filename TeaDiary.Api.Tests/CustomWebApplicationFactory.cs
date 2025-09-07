using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TeaDiary.Api.Data;
using TeaDiary.Api.Models;
using System.Linq;

namespace TeaDiary.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Guid TeaTypeId { get; } = Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TeaDiaryContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<TeaDiaryContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<TeaDiaryContext>();
                    db.Database.EnsureCreated();

                    if (!db.TeaTypes.Any())
                    {
                        db.TeaTypes.Add(new TeaType
                        {
                            Id = TeaTypeId,
                            Name = "Black Tea"
                        });
                        db.SaveChanges();
                    }
                }
            });
        }
    }
}
