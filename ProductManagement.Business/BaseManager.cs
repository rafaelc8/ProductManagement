using ProductManagement.Business.Models;
using ProductManagement.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ProductManagement.Business
{
    public abstract class BaseManager : TransactionManager
    {
        protected BaseManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        protected OperationResult<T> Success<T>(T data)
        {
            return new OperationResult<T>(data);
        }

        protected OperationResult<T> Error<T>(ErrorEnum error)
        {
            return new OperationResult<T>(error);
        }
    }
}
