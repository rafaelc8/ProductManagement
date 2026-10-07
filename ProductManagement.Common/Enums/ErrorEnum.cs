using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Common.Enums
{
    public enum ErrorEnum
    {
        [Display(Name = "System")]
        System = 1,

        [Display(Name = "The product was not found")]
        NotFound,

        [Display(Name = "The product does not contain suficient stock")]
        InsufficientStock
    }
}
