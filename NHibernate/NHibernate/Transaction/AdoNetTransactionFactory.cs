// Decompiled with JetBrains decompiler
// Type: NHibernate.Transaction.AdoNetTransactionFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Exceptions;
using NHibernate.Impl;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Transaction
{
  public class AdoNetTransactionFactory : ITransactionFactory
  {
    private readonly IInternalLogger isolaterLog = LoggerProvider.LoggerFor(typeof (ITransactionFactory));

    public ITransaction CreateTransaction(ISessionImplementor session)
    {
      return (ITransaction) new AdoTransaction(session);
    }

    public void EnlistInDistributedTransactionIfNeeded(ISessionImplementor session)
    {
    }

    public bool IsInDistributedActiveTransaction(ISessionImplementor session) => false;

    public void ExecuteWorkInIsolation(
      ISessionImplementor session,
      IIsolatedWork work,
      bool transacted)
    {
      IDbConnection dbConnection = (IDbConnection) null;
      IDbTransaction transaction = (IDbTransaction) null;
      try
      {
        dbConnection = !(session.Factory.Dialect is SQLiteDialect) ? session.Factory.ConnectionProvider.GetConnection() : session.Connection;
        if (transacted)
          transaction = dbConnection.BeginTransaction();
        work.DoWork(dbConnection, transaction);
        if (!transacted)
          return;
        transaction.Commit();
      }
      catch (Exception ex1)
      {
        using (new SessionIdLoggingContext(session.SessionId))
        {
          try
          {
            if (transaction != null)
            {
              if (dbConnection.State != ConnectionState.Closed)
                transaction.Rollback();
            }
          }
          catch (Exception ex2)
          {
            this.isolaterLog.Debug((object) ("unable to release connection on exception [" + (object) ex2 + "]"));
          }
          switch (ex1)
          {
            case HibernateException _:
              throw;
            case DbException _:
              throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, ex1, "error performing isolated work");
            default:
              throw new HibernateException("error performing isolated work", ex1);
          }
        }
      }
      finally
      {
        if (!(session.Factory.Dialect is SQLiteDialect))
          session.Factory.ConnectionProvider.CloseConnection(dbConnection);
      }
    }

    public void Configure(IDictionary props)
    {
    }
  }
}
