using ProductManagement.Common.Enums;
using ProductManagement.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Business.Models
{
    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public Exception Exception { get; set; }
        public string ExceptionMessage { get; set; }
        public string ErrorDescription { get; set; }
        public ErrorEnum Error { get; set; }

        public OperationResult() { }

        public OperationResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public OperationResult(Exception ex)
        {
            Succeeded = false;
            Exception = ex;

            SetErrorCode(ex);
        }

        public OperationResult(bool succedded, Exception ex)
        {
            Succeeded = succedded;
            Exception = ex;

            SetErrorCode(ex);
        }

        private void SetErrorCode(Exception ex)
        {
            Error = ErrorEnum.System;
            ErrorDescription = ex.Message;
        }
    }

    public class OperationResult<Tout> : OperationResult
    {
        public Tout Data { get; set; }

        public OperationResult()
        {

        }

        public OperationResult(Tout data)
            : base(true)
        {
            Data = data;
        }

        public OperationResult(Tout data, bool succeeded)
            : base(succeeded)
        {
            Data = data;
        }

        public OperationResult(Tout data, Exception ex, ErrorEnum error)
           : base(false)
        {
            Error = error;
            ErrorDescription = ex.Message;
            Data = data;
        }

        public OperationResult(Exception ex, ErrorEnum error)
          : base(false)
        {
            Error = error;
            ErrorDescription = ex.Message;
        }

        public OperationResult(ErrorEnum error)
         : base(false)
        {
            Error = error;

            var message = ErrorResource.ResourceManager.GetString(error.ToString());
            ErrorDescription = !string.IsNullOrWhiteSpace(message) ? message : ErrorResource.System;
        }

        public OperationResult(Exception ex)
            : base(ex)
        {
            Data = default(Tout);
        }
    }
}
