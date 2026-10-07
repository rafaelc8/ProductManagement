using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Data.Context;

namespace ProductManagement.Tests
{
    internal static class TestServiceProviderFactory
    {
        public static ServiceProvider Create()
        {
            var services = new ServiceCollection();

            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase($"ProductManagementTests-{Guid.NewGuid()}"));

            return services.BuildServiceProvider();
        }
    }
}