using Microsoft.EntityFrameworkCore;
using MyCoreApp.Models;

namespace MyCoreApp.Data
{
    public class MyAppCoreContext : DbContext
    {
        public MyAppCoreContext(DbContextOptions<MyAppCoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemClient>().HasKey(ic => new
            {
                ic.ItemId,
                ic.ClientId
            });

            modelBuilder.Entity<ItemClient>().HasOne(i=>i.Item).WithMany(ic=>ic.ItemClients).HasForeignKey(ic=>ic.ItemId);

            modelBuilder.Entity<ItemClient>().HasOne(i => i.Client).WithMany(ic => ic.ItemClients).HasForeignKey(ic => ic.ClientId);
            // Seed principal entities first
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Audio" },
                new Category { Id = 2, Name = "Video" }
            );

            // Then seed items that reference those categories
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 4, Name = "Microphone", Price = 49, SerialNumberId = 10, CategoryId = 1 },
                  new Item { Id = 6, Name = "Volt", Price = 44, SerialNumberId = 11, CategoryId = 2 }
            );

            // Then seed serial numbers
            modelBuilder.Entity<SerialNumber>().HasData(
                new SerialNumber { Id = 10, Name = "MIC120", ItemId = 4 },
                new SerialNumber { Id = 11, Name = "VLT220", ItemId = 6 }
            );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; } = default!;

        public DbSet<SerialNumber> SerialNumbers { get; set; } = default!;

        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<Client> Clients { get; set; } = default!;
        public DbSet<ItemClient> ItemClients { get; set; } = default!;

    }
}
