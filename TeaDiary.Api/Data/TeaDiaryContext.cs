using Microsoft.EntityFrameworkCore;
using TeaDiary.Api.Models;

namespace TeaDiary.Api.Data
{
    public class TeaDiaryContext : DbContext
    {
        public TeaDiaryContext(DbContextOptions<TeaDiaryContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TeaType> TeaTypes { get; set; }
        public DbSet<Tea> Teas { get; set; }
        public DbSet<Impression> Impressions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed — заполнение начальными типами чая
            modelBuilder.Entity<TeaType>().HasData(
                new TeaType { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Зеленый" },
                new TeaType { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Черный" },
                new TeaType { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Красный" },
                new TeaType { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Улун" },
                new TeaType { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Шу пуэр" },
                new TeaType { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Шен пуэр" },
                new TeaType { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Габа" },
                new TeaType { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "Белый" }
            );
        }
    }
}
