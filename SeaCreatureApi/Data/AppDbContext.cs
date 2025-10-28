// W pliku Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Models;

namespace SeaCreatureApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Змінюємо Games на SeaCreatures
        public DbSet<SeaCreature> SeaCreatures { get; set; }
    }
}