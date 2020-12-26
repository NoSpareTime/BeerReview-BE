using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class BeerReviewDbContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }

        public BeerReviewDbContext(DbContextOptions<BeerReviewDbContext> options) : base(options)
        {
            
        }
    }
}
