using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManagement.Data.Context;
using ProductManagement.DataAccess;
using ProductManagement.DataAccess.Interfaces;
using ProductManagement.DataAccess.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not configured.");

            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddScoped<IDbContextFactory, DbContextFactory>();

            return services;
        }
    }
}
