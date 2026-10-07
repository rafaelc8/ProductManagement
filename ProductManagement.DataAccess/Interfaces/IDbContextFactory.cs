using ProductManagement.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.DataAccess.Interfaces
{
    public interface IDbContextFactory
    {
        ApplicationDbContext DbContext { get; }

        void Reset();
    }
}
