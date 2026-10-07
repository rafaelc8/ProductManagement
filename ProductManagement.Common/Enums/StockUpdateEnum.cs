using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Common.Enums
{
    public enum StockUpdateEnum
    {
        Success = 1,
        ProductNotFound,
        InsufficientStock
    }
}
