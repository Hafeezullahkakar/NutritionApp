using Microsoft.EntityFrameworkCore;

using NutritionAppBackend.Models;

namespace NutritionAppBackend.Data
{
    public class NutritionDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FoodEntry> FoodEntries { get; set; }
        public DbSet<FoodItem> FoodItem { get; set; }
        public DbSet<Nutrient> Nutrient { get; set; }

        public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<FoodEntry>().HasKey(d => d.Id);
            modelBuilder.Entity<FoodItem>().HasKey(d => d.Id);
            modelBuilder.Entity<Nutrient>().HasKey(d => d.Id);

            modelBuilder.Entity<FoodEntry>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<FoodItem>()
        .HasOne(f => f.Nutrient)
        .WithOne(n => n.FoodItem)
        .HasForeignKey<Nutrient>(n => n.FoodItemId);

        }

    }
}
