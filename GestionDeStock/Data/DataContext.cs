using Microsoft.EntityFrameworkCore;
using GestionDeStock.Entities;

namespace GestionDeStock.Data
{
    internal class DataContext : DbContext
    {
        public DbSet<Products>? Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite("Data source=GestionDeStock.db");
        }
    }
}
