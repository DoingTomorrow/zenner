// Decompiled with JetBrains decompiler
// Type: NHibernate.Transaction.AdoTransaction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Transaction
{
  public class AdoTransaction : ITransaction, IDisposable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AdoTransaction));
    private ISessionImplementor session;
    private IDbTransaction trans;
    private bool begun;
    private bool committed;
    private bool rolledBack;
    private bool commitFailed;
    private IList<ISynchronization> synchronizations;
    private bool _isAlreadyDisposed;
    private Guid sessionId;

    public AdoTransaction(ISessionImplementor session)
    {
      this.session = session;
      this.sessionId = this.session.SessionId;
    }

    public void Enlist(IDbCommand command)
    {
      if (this.trans == null)
      {
        if (AdoTransaction.log.IsWarnEnabled && command.Transaction != null)
          AdoTransaction.log.Warn((object) "set a nonnull IDbCommand.Transaction to null because the Session had no Transaction");
        command.Transaction = (IDbTransaction) null;
      }
      else
      {
        if (AdoTransaction.log.IsWarnEnabled && command.Transaction != null && command.Transaction != this.trans)
          AdoTransaction.log.Warn((object) "The IDbCommand had a different Transaction than the Session.  This can occur when Disconnecting and Reconnecting Sessions because the PreparedCommand Cache is Session specific.");
        AdoTransaction.log.Debug((object) "Enlist Command");
        command.Transaction = this.trans;
      }
    }

    public void RegisterSynchronization(ISynchronization sync)
    {
      if (sync == null)
        throw new ArgumentNullException(nameof (sync));
      if (this.synchronizations == null)
        this.synchronizations = (IList<ISynchronization>) new List<ISynchronization>();
      this.synchronizations.Add(sync);
    }

    public void Begin() => this.Begin(IsolationLevel.Unspecified);

    public void Begin(IsolationLevel isolationLevel)
    {
      using (new SessionIdLoggingContext(this.sessionId))
      {
        if (this.begun)
          return;
        if (this.commitFailed)
          throw new TransactionException("Cannot restart transaction after failed commit");
        if (isolationLevel == IsolationLevel.Unspecified)
          isolationLevel = this.session.Factory.Settings.IsolationLevel;
        AdoTransaction.log.Debug((object) string.Format("Begin ({0})", (object) isolationLevel));
        try
        {
          this.trans = isolationLevel != IsolationLevel.Unspecified ? this.session.Connection.BeginTransaction(isolationLevel) : this.session.Connection.BeginTransaction();
        }
        catch (HibernateException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          AdoTransaction.log.Error((object) "Begin transaction failed", ex);
          throw new TransactionException("Begin failed with SQL exception", ex);
        }
        this.begun = true;
        this.committed = false;
        this.rolledBack = false;
        this.session.AfterTransactionBegin((ITransaction) this);
      }
    }

    private void AfterTransactionCompletion(bool successful)
    {
      using (new SessionIdLoggingContext(this.sessionId))
      {
        this.session.AfterTransactionCompletion(successful, (ITransaction) this);
        this.NotifyLocalSynchsAfterTransactionCompletion(successful);
        this.session = (ISessionImplementor) null;
        this.begun = false;
      }
    }

    public void Commit()
    {
      using (new SessionIdLoggingContext(this.sessionId))
      {
        this.CheckNotDisposed();
        this.CheckBegun();
        this.CheckNotZombied();
        AdoTransaction.log.Debug((object) "Start Commit");
        if (this.session.FlushMode != FlushMode.Never)
          this.session.Flush();
        this.NotifyLocalSynchsBeforeTransactionCompletion();
        this.session.BeforeTransactionCompletion((ITransaction) this);
        try
        {
          this.trans.Commit();
          AdoTransaction.log.Debug((object) "IDbTransaction Committed");
          this.committed = true;
          this.AfterTransactionCompletion(true);
          this.Dispose();
        }
        catch (HibernateException ex)
        {
          AdoTransaction.log.Error((object) "Commit failed", (Exception) ex);
          this.AfterTransactionCompletion(false);
          this.commitFailed = true;
          throw;
        }
        catch (Exception ex)
        {
          AdoTransaction.log.Error((object) "Commit failed", ex);
          this.AfterTransactionCompletion(false);
          this.commitFailed = true;
          throw new TransactionException("Commit failed with SQL exception", ex);
        }
        finally
        {
          this.CloseIfRequired();
        }
      }
    }

    public void Rollback()
    {
      using (new SessionIdLoggingContext(this.sessionId))
      {
        this.CheckNotDisposed();
        this.CheckBegun();
        this.CheckNotZombied();
        AdoTransaction.log.Debug((object) nameof (Rollback));
        if (this.commitFailed)
          return;
        try
        {
          this.trans.Rollback();
          AdoTransaction.log.Debug((object) "IDbTransaction RolledBack");
          this.rolledBack = true;
          this.Dispose();
        }
        catch (HibernateException ex)
        {
          AdoTransaction.log.Error((object) "Rollback failed", (Exception) ex);
          throw;
        }
        catch (Exception ex)
        {
          AdoTransaction.log.Error((object) "Rollback failed", ex);
          throw new TransactionException("Rollback failed with SQL Exception", ex);
        }
        finally
        {
          this.AfterTransactionCompletion(false);
          this.CloseIfRequired();
        }
      }
    }

    public bool WasRolledBack => this.rolledBack;

    public bool WasCommitted => this.committed;

    public bool IsActive => this.begun && !this.rolledBack && !this.committed;

    public IsolationLevel IsolationLevel => this.trans.IsolationLevel;

    private void CloseIfRequired()
    {
    }

    ~AdoTransaction() => this.Dispose(false);

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool isDisposing)
    {
      using (new SessionIdLoggingContext(this.sessionId))
      {
        if (this._isAlreadyDisposed)
          return;
        if (isDisposing)
        {
          if (this.trans != null)
          {
            this.trans.Dispose();
            this.trans = (IDbTransaction) null;
            AdoTransaction.log.Debug((object) "IDbTransaction disposed.");
          }
          if (this.IsActive && this.session != null)
            this.AfterTransactionCompletion(false);
        }
        this._isAlreadyDisposed = true;
        GC.SuppressFinalize((object) this);
      }
    }

    private void CheckNotDisposed()
    {
      if (this._isAlreadyDisposed)
        throw new ObjectDisposedException(nameof (AdoTransaction));
    }

    private void CheckBegun()
    {
      if (!this.begun)
        throw new TransactionException("Transaction not successfully started");
    }

    private void CheckNotZombied()
    {
      if (this.trans != null && this.trans.Connection == null)
        throw new TransactionException("Transaction not connected, or was disconnected");
    }

    private void NotifyLocalSynchsBeforeTransactionCompletion()
    {
      if (this.synchronizations == null)
        return;
      for (int index = 0; index < this.synchronizations.Count; ++index)
      {
        ISynchronization synchronization = this.synchronizations[index];
        try
        {
          synchronization.BeforeCompletion();
        }
        catch (Exception ex)
        {
          AdoTransaction.log.Error((object) "exception calling user Synchronization", ex);
        }
      }
    }

    private void NotifyLocalSynchsAfterTransactionCompletion(bool success)
    {
      this.begun = false;
      if (this.synchronizations == null)
        return;
      for (int index = 0; index < this.synchronizations.Count; ++index)
      {
        ISynchronization synchronization = this.synchronizations[index];
        try
        {
          synchronization.AfterCompletion(success);
        }
        catch (Exception ex)
        {
          AdoTransaction.log.Error((object) "exception calling user Synchronization", ex);
        }
      }
    }
  }
}
