using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ProductManagement.DataAccess
{
    public class UnitOfWork : DisposableBase
    {
        private static IsolationLevel DefaultIsolationLevel = IsolationLevel.ReadCommitted;
        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(90);

        private TransactionScope transactionScope;

        /// <summary>
        /// Init new transaction scope
        /// </summary>
        public UnitOfWork()
            : this(UnitOfWork.DefaultIsolationLevel, UnitOfWork.DefaultTimeout, false)
        {
        }

        /// <summary>
        /// Init new transaction scope
        /// </summary>
        /// <param name="iso">Iso</param>
        public UnitOfWork(IsolationLevel iso)
            : this(iso, UnitOfWork.DefaultTimeout, false)
        {
        }

        /// <summary>
        /// Init new transaction scope
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public UnitOfWork(TimeSpan timeout)
            : this(UnitOfWork.DefaultIsolationLevel, timeout, false)
        {
        }

        /// <summary>
        /// Init new transaction scope
        /// </summary>
        /// <param name="requireNew">Is TransactionScopeOption.RequiresNew?</param>
        public UnitOfWork(bool requireNew)
            : this(UnitOfWork.DefaultIsolationLevel, UnitOfWork.DefaultTimeout, false)
        {
        }

        /// <summary>
        /// Init new transaction scope
        /// </summary>
        /// <param name="iso">IsolationLevel</param>
        /// <param name="timeout">Timeout</param>
        /// <param name="requireNew">Is TransactionScopeOption.RequiresNew?</param>
        public UnitOfWork(IsolationLevel iso, TimeSpan timeout, bool requireNew)
        {
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = iso;
            transactionOptions.Timeout = timeout;

            TransactionScopeOption scopeOption = requireNew ? TransactionScopeOption.RequiresNew : TransactionScopeOption.Required;

            transactionScope = new TransactionScope(scopeOption, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public void Commit()
        {
            transactionScope.Complete();
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public void RollBack()
        {
            this.Dispose();
        }

        /// <summary>
        /// Transaction scope dispose
        /// </summary>
        protected override void CustomDispose()
        {
            transactionScope.Dispose();
        }
    }
}
