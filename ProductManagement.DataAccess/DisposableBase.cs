using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess
{
    public abstract class DisposableBase : IDisposable
    {
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">Call custom dispose?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CustomDispose();
            }
        }

        protected abstract void CustomDispose();
    }
}
