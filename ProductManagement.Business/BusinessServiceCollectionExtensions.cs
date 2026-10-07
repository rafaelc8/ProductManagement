using ProductManagement.Business.Interface;
using ProductManagement.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BusinessServiceCollectionExtensions
    {
        /// <summary>
        /// Configure managers classes dependency injection.
        /// </summary>
        /// <param name = "services">Specifies the contract for a collection of service descriptors.</param>
        /// <returns>Specifies the contract for a collection of service descriptors.</returns>
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddTransient<IProductManager, ProductManager>();
            return services;
        }
    }
}
