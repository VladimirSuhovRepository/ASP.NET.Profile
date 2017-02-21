using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests
{
    [TestClass]
    public abstract class TransactionalTest : DependencyInjectedTest, IDisposable
    {
        private TransactionScope _transaction;

        [TestInitialize]
        public virtual void OnStart()
        {
            _transaction = new TransactionScope();
        }

        [TestCleanup]
        public virtual void OnEnd()
        {
            Dispose();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
