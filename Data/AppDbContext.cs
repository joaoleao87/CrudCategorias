using Microsoft.EntityFrameworkCore;
using CrudCategorias.Models;

namespace CrudCategorias.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
    }
}
