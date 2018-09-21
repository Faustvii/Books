using Books.EF;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Transactions;

namespace Books.API.IntegrationTests.Core
{
    public abstract class TestBase : IDisposable
    {
        protected readonly TestEnvironment<Startup> _testEnvironment;
        private readonly TransactionScope _transactionScope;
        private readonly IDbContextTransaction _transaction;

        public BooksContext Context => _testEnvironment.ServiceProvider.GetService<BooksContext>();

        protected TestBase()
        {
            _testEnvironment = new TestEnvironment<Startup>("src");
            TestDatabaseManager.Initialize(Context);
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _transaction = Context.Database.BeginTransaction();
        }

        #region IDisposable Support
        private bool isDisposed; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _transactionScope.Dispose();
                    _transaction.Rollback();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
