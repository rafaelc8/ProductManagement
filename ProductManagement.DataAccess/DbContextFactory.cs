using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductManagement.Data.Context;
using ProductManagement.DataAccess.Interfaces;

namespace ProductManagement.DataAccess
{
    public class DbContextFactory : IDbContextFactory, IDisposable
    {
        private readonly IOptions<DbContextSettings> settings;
        public ApplicationDbContext DbContext { get; private set; }

        public DbContextFactory(IOptions<DbContextSettings> settings)
        {
            this.settings = settings;
            Create();
        }
        private void Create()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().
            UseSqlServer(settings.Value.DefaultConnection).Options;
            DbContext = new ApplicationDbContext(options);
            DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            DbContext.ChangeTracker.LazyLoadingEnabled = false;
        }

        ~DbContextFactory()
        {
            Dispose();
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
        public void Reset()
        {
            Dispose();
            Create();
        }
    }
}
