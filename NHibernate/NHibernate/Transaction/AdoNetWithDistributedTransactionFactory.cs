// Decompiled with JetBrains decompiler
// Type: NHibernate.Transaction.AdoNetWithDistributedTransactionFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Impl;
using System;
using System.Collections;
using System.Transactions;

#nullable disable
namespace NHibernate.Transaction
{
  public class AdoNetWithDistributedTransactionFactory : ITransactionFactory
  {
    private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof (ITransactionFactory));
    private readonly AdoNetTransactionFactory adoNetTransactionFactory = new AdoNetTransactionFactory();

    public void Configure(IDictionary props)
    {
    }

    public ITransaction CreateTransaction(ISessionImplementor session)
    {
      return (ITransaction) new AdoTransaction(session);
    }

    public void EnlistInDistributedTransactionIfNeeded(ISessionImplementor session)
    {
      if (session.TransactionContext != null || System.Transactions.Transaction.Current == (System.Transactions.Transaction) null)
        return;
      AdoNetWithDistributedTransactionFactory.DistributedTransactionContext transactionContext = new AdoNetWithDistributedTransactionFactory.DistributedTransactionContext(session, System.Transactions.Transaction.Current);
      session.TransactionContext = (ITransactionContext) transactionContext;
      AdoNetWithDistributedTransactionFactory.logger.DebugFormat("enlisted into DTC transaction: {0}", (object) transactionContext.AmbientTransation.IsolationLevel);
      session.AfterTransactionBegin((ITransaction) null);
      transactionContext.AmbientTransation.TransactionCompleted += (TransactionCompletedEventHandler) ((sender, e) =>
      {
        using (new SessionIdLoggingContext(session.SessionId))
        {
          ((AdoNetWithDistributedTransactionFactory.DistributedTransactionContext) session.TransactionContext).IsInActiveTransaction = false;
          bool successful = false;
          try
          {
            successful = e.Transaction.TransactionInformation.Status == TransactionStatus.Committed;
          }
          catch (ObjectDisposedException ex)
          {
            AdoNetWithDistributedTransactionFactory.logger.Warn((object) "Completed transaction was disposed, assuming transaction rollback", (Exception) ex);
          }
          session.AfterTransactionCompletion(successful, (ITransaction) null);
          if (transactionContext.ShouldCloseSessionOnDistributedTransactionCompleted)
            session.CloseSessionFromDistributedTransaction();
          session.TransactionContext = (ITransactionContext) null;
        }
      });
      transactionContext.AmbientTransation.EnlistVolatile((IEnlistmentNotification) transactionContext, EnlistmentOptions.EnlistDuringPrepareRequired);
    }

    public bool IsInDistributedActiveTransaction(ISessionImplementor session)
    {
      AdoNetWithDistributedTransactionFactory.DistributedTransactionContext transactionContext = (AdoNetWithDistributedTransactionFactory.DistributedTransactionContext) session.TransactionContext;
      return transactionContext != null && transactionContext.IsInActiveTransaction;
    }

    public void ExecuteWorkInIsolation(
      ISessionImplementor session,
      IIsolatedWork work,
      bool transacted)
    {
      using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
      {
        this.adoNetTransactionFactory.ExecuteWorkInIsolation(session, work, transacted);
        transactionScope.Complete();
      }
    }

    public class DistributedTransactionContext : 
      ITransactionContext,
      IDisposable,
      IEnlistmentNotification
    {
      private readonly ISessionImplementor sessionImplementor;
      public bool IsInActiveTransaction;

      public System.Transactions.Transaction AmbientTransation { get; set; }

      public bool ShouldCloseSessionOnDistributedTransactionCompleted { get; set; }

      public DistributedTransactionContext(
        ISessionImplementor sessionImplementor,
        System.Transactions.Transaction transaction)
      {
        this.sessionImplementor = sessionImplementor;
        this.AmbientTransation = transaction.Clone();
        this.IsInActiveTransaction = true;
      }

      void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
      {
        using (new SessionIdLoggingContext(this.sessionImplementor.SessionId))
        {
          try
          {
            using (TransactionScope transactionScope = new TransactionScope(this.AmbientTransation))
            {
              this.sessionImplementor.BeforeTransactionCompletion((ITransaction) null);
              if (this.sessionImplementor.FlushMode != FlushMode.Never && this.sessionImplementor.ConnectionManager.IsConnected)
              {
                using (this.sessionImplementor.ConnectionManager.FlushingFromDtcTransaction)
                {
                  AdoNetWithDistributedTransactionFactory.logger.Debug((object) string.Format("[session-id={0}] Flushing from Dtc Transaction", (object) this.sessionImplementor.SessionId));
                  this.sessionImplementor.Flush();
                }
              }
              AdoNetWithDistributedTransactionFactory.logger.Debug((object) "prepared for DTC transaction");
              transactionScope.Complete();
            }
            preparingEnlistment.Prepared();
          }
          catch (Exception ex)
          {
            AdoNetWithDistributedTransactionFactory.logger.Error((object) "DTC transaction prepre phase failed", ex);
            preparingEnlistment.ForceRollback(ex);
          }
        }
      }

      void IEnlistmentNotification.Commit(Enlistment enlistment)
      {
        using (new SessionIdLoggingContext(this.sessionImplementor.SessionId))
        {
          AdoNetWithDistributedTransactionFactory.logger.Debug((object) "committing DTC transaction");
          enlistment.Done();
          this.IsInActiveTransaction = false;
        }
      }

      void IEnlistmentNotification.Rollback(Enlistment enlistment)
      {
        using (new SessionIdLoggingContext(this.sessionImplementor.SessionId))
        {
          this.sessionImplementor.AfterTransactionCompletion(false, (ITransaction) null);
          AdoNetWithDistributedTransactionFactory.logger.Debug((object) "rolled back DTC transaction");
          enlistment.Done();
          this.IsInActiveTransaction = false;
        }
      }

      void IEnlistmentNotification.InDoubt(Enlistment enlistment)
      {
        using (new SessionIdLoggingContext(this.sessionImplementor.SessionId))
        {
          this.sessionImplementor.AfterTransactionCompletion(false, (ITransaction) null);
          AdoNetWithDistributedTransactionFactory.logger.Debug((object) "DTC transaction is in doubt");
          enlistment.Done();
          this.IsInActiveTransaction = false;
        }
      }

      public void Dispose()
      {
        if (!(this.AmbientTransation != (System.Transactions.Transaction) null))
          return;
        this.AmbientTransation.Dispose();
      }
    }
  }
}
