using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.Models;
using ProductManagement.Business.Interface;
using ProductManagement.Common.Enums;

namespace ProductManagement.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IProductManager productManager;

        public BaseController(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        protected IActionResult GetErrorResult(ErrorEnum error)
        {
            return error switch
            {
                ErrorEnum.NotFound => NotFound(new { message = "Resource not found." }),
                ErrorEnum.InsufficientStock => Conflict(new { message = "Insufficient stock." }),
                _ => BadRequest(new { message = "The request could not be processed." })
            };
        }

        protected IActionResult SuccessResult<T>(T data)
        {
            return Ok(new ApiResponse<T>
            {
                Succeeded = true,
                Data = data
            });
        }
    }
}
