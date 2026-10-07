using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Business.Models;
using ProductManagement.Common.Enums;
using ProductManagement.Data.Context;

namespace ProductManagement.Business
{
    public abstract class TransactionManager(IServiceProvider serviceProvider)
    {

        protected readonly IServiceProvider ServiceProvider = serviceProvider;

        protected async Task<TOut> ExecuteOperationAsync<TOut>(Func<ApplicationDbContext, Task<TOut>> func)
            where TOut : OperationResult, new()
        {
            return await ExecuteOperationAsync(func, (exception) =>
            {
                TOut result = new TOut
                {
                    ExceptionMessage = exception.Message.Contains("Inner") && exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                    Exception = exception,
                    Error = ErrorEnum.System
                };
                return Task.FromResult(result);
            });
        }

        protected async Task<TOut> ExecuteOperationAsync<TOut>(Func<ApplicationDbContext, Task<TOut>> func, Func<Exception, Task<TOut>> fail) where TOut : OperationResult, new()
        {
            IDbContextFactory<ApplicationDbContext> dbContextFactory = ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

            await using ApplicationDbContext context = await dbContextFactory.CreateDbContextAsync();
            try
            {
                TOut result = await func.Invoke(context);
                return result;
            }
            catch (Exception ex)
            {
                return await fail(ex);
            }
        }
    }
}
