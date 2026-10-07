using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Models;

namespace ProductManagement.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbContextOptions<ApplicationDbContext> Options { get; set; }

        /// <summary>
        /// Init Aplication database context
        /// </summary>
        /// <param name="options">ApplicationDbContext options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Options = options;
        }

        public ApplicationDbContext() : base()
        {
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types exposed in Microsoft.EntityFrameworkCore.DbSet`1 properties on your derived context. The resulting model may be cached and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically define extension methods on this object that allow you to configure aspects of the model that are specific to a given database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasSequence<int>("ProductIdSequence")
                .StartsAt(100000)
                .IncrementsBy(1)
                .HasMin(100000)
                .HasMax(999999);

            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEXT VALUE FOR ProductIdSequence");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}
