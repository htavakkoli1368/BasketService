using BasketService.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasketService.Infrastructure.Context
{
    public class BasketDatabaseContext:DbContext
    {

        public BasketDatabaseContext(DbContextOptions<BasketDatabaseContext> options):base(options)
        {
            
        }

        public DbSet<Basket> Basket { get; set; }

        public DbSet<BasketItems> BasketItems { get; set; }

    }
}
