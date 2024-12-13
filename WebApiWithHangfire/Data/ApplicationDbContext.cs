using Microsoft.EntityFrameworkCore;
using WebApiWithHangfire.Models.Entities;

namespace WebApiWithHangfire.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; } = default!;
    }

}
