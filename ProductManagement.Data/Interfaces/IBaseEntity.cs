using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.Data.Interfaces
{
    public interface IBaseEntity<T>
    {
        T Id { get; set; }
    }
}
