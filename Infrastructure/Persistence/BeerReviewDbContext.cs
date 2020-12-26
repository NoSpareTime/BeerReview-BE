using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence
{
    public class BeerReviewDbContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public BeerReviewDbContext(DbContextOptions<BeerReviewDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
