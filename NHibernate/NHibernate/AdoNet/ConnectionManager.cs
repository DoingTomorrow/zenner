// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.ConnectionManager
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate.AdoNet
{
  [Serializable]
  public class ConnectionManager : ISerializable, IDeserializationCallback
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ConnectionManager));
    [NonSerialized]
    private IDbConnection connection;
    private bool ownConnection;
    [NonSerialized]
    private ITransaction transaction;
    [NonSerialized]
    private IBatcher batcher;
    private readonly ISessionImplementor session;
    private readonly ConnectionReleaseMode connectionReleaseMode;
    private readonly IInterceptor interceptor;
    [NonSerialized]
    private bool isFlushing;
    private bool flushingFromDtcTransaction;

    public ConnectionManager(
      ISessionImplementor session,
      IDbConnection suppliedConnection,
      ConnectionReleaseMode connectionReleaseMode,
      IInterceptor interceptor)
    {
      this.session = session;
      this.connection = suppliedConnection;
      this.connectionReleaseMode = connectionReleaseMode;
      this.interceptor = interceptor;
      this.batcher = session.Factory.Settings.BatcherFactory.CreateBatcher(this, interceptor);
      this.ownConnection = suppliedConnection == null;
    }

    public bool IsInActiveTransaction
    {
      get
      {
        return this.transaction != null && this.transaction.IsActive || this.session.Factory.TransactionFactory.IsInDistributedActiveTransaction(this.session);
      }
    }

    public bool IsConnected => this.connection != null || this.ownConnection;

    public void Reconnect()
    {
      if (this.IsConnected)
        throw new HibernateException("session already connected");
      this.ownConnection = true;
    }

    public void Reconnect(IDbConnection suppliedConnection)
    {
      if (this.IsConnected)
        throw new HibernateException("session already connected");
      ConnectionManager.log.Debug((object) "reconnecting session");
      this.connection = suppliedConnection;
      this.ownConnection = false;
    }

    public IDbConnection Close()
    {
      if (this.batcher != null)
        this.batcher.Dispose();
      if (this.transaction != null)
        this.transaction.Dispose();
      if (this.connection != null)
        return this.Disconnect();
      this.ownConnection = false;
      return (IDbConnection) null;
    }

    private IDbConnection DisconnectSuppliedConnection()
    {
      IDbConnection dbConnection = this.connection != null ? this.connection : throw new HibernateException("Session already disconnected");
      this.connection = (IDbConnection) null;
      return dbConnection;
    }

    private void DisconnectOwnConnection()
    {
      if (this.connection == null)
        return;
      if (this.batcher != null)
        this.batcher.CloseCommands();
      this.CloseConnection();
    }

    public IDbConnection Disconnect()
    {
      if (this.IsInActiveTransaction)
        throw new InvalidOperationException("Disconnect cannot be called while a transaction is in progress.");
      try
      {
        if (!this.ownConnection)
          return this.DisconnectSuppliedConnection();
        this.DisconnectOwnConnection();
        this.ownConnection = false;
        return (IDbConnection) null;
      }
      finally
      {
        if (!this.IsInActiveTransaction)
          this.session.AfterTransactionCompletion(false, (ITransaction) null);
      }
    }

    private void CloseConnection()
    {
      this.session.Factory.ConnectionProvider.CloseConnection(this.connection);
      this.connection = (IDbConnection) null;
    }

    public IDbConnection GetConnection()
    {
      if (this.connection == null)
      {
        if (this.ownConnection)
        {
          this.connection = this.session.Factory.ConnectionProvider.GetConnection();
          if (this.session.Factory.Statistics.IsStatisticsEnabled)
            this.session.Factory.StatisticsImplementor.Connect();
        }
        else
        {
          if (this.session.IsOpen)
            throw new HibernateException("Session is currently disconnected");
          throw new HibernateException("Session is closed");
        }
      }
      return this.connection;
    }

    public void AfterTransaction()
    {
      if (this.IsAfterTransactionRelease)
        this.AggressiveRelease();
      else if (this.IsAggressiveRelease && this.batcher.HasOpenResources)
      {
        ConnectionManager.log.Info((object) "forcing batcher resource cleanup on transaction completion; forgot to close ScrollableResults/Enumerable?");
        this.batcher.CloseCommands();
        this.AggressiveRelease();
      }
      else if (this.IsOnCloseRelease)
        ConnectionManager.log.Debug((object) "transaction completed on session with on_close connection release mode; be sure to close the session to release ADO.Net resources!");
      this.transaction = (ITransaction) null;
    }

    public void AfterStatement()
    {
      if (!this.IsAggressiveRelease)
        return;
      if (this.isFlushing)
        ConnectionManager.log.Debug((object) "skipping aggressive-release due to flush cycle");
      else if (this.batcher.HasOpenResources)
        ConnectionManager.log.Debug((object) "skipping aggressive-release due to open resources on batcher");
      else
        this.AggressiveRelease();
    }

    private void AggressiveRelease()
    {
      if (!this.ownConnection || this.flushingFromDtcTransaction)
        return;
      ConnectionManager.log.Debug((object) "aggressively releasing database connection");
      if (this.connection == null)
        return;
      this.CloseConnection();
    }

    public void FlushBeginning()
    {
      ConnectionManager.log.Debug((object) "registering flush begin");
      this.isFlushing = true;
    }

    public void FlushEnding()
    {
      ConnectionManager.log.Debug((object) "registering flush end");
      this.isFlushing = false;
      this.AfterStatement();
    }

    private ConnectionManager(SerializationInfo info, StreamingContext context)
    {
      this.ownConnection = info.GetBoolean(nameof (ownConnection));
      this.session = (ISessionImplementor) info.GetValue(nameof (session), typeof (ISessionImplementor));
      this.connectionReleaseMode = (ConnectionReleaseMode) info.GetValue(nameof (connectionReleaseMode), typeof (ConnectionReleaseMode));
      this.interceptor = (IInterceptor) info.GetValue(nameof (interceptor), typeof (IInterceptor));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("ownConnection", this.ownConnection);
      info.AddValue("session", (object) this.session, typeof (ISessionImplementor));
      info.AddValue("connectionReleaseMode", (object) this.connectionReleaseMode, typeof (ConnectionReleaseMode));
      info.AddValue("interceptor", (object) this.interceptor, typeof (IInterceptor));
    }

    void IDeserializationCallback.OnDeserialization(object sender)
    {
      this.batcher = this.session.Factory.Settings.BatcherFactory.CreateBatcher(this, this.interceptor);
    }

    public ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      this.Transaction.Begin(isolationLevel);
      return this.transaction;
    }

    public ITransaction BeginTransaction()
    {
      this.Transaction.Begin();
      return this.transaction;
    }

    public ITransaction Transaction
    {
      get
      {
        if (this.transaction == null)
          this.transaction = this.session.Factory.TransactionFactory.CreateTransaction(this.session);
        return this.transaction;
      }
    }

    public void AfterNonTransactionalQuery(bool success)
    {
      ConnectionManager.log.Debug((object) "after autocommit");
      this.session.AfterTransactionCompletion(success, (ITransaction) null);
    }

    private bool IsAfterTransactionRelease
    {
      get => this.connectionReleaseMode == ConnectionReleaseMode.AfterTransaction;
    }

    private bool IsOnCloseRelease => this.connectionReleaseMode == ConnectionReleaseMode.OnClose;

    private bool IsAggressiveRelease
    {
      get
      {
        return this.connectionReleaseMode == ConnectionReleaseMode.AfterTransaction && !this.IsInActiveTransaction;
      }
    }

    public ISessionFactoryImplementor Factory => this.session.Factory;

    public bool IsReadyForSerialization
    {
      get
      {
        if (!this.ownConnection)
          return this.connection == null;
        return this.connection == null && !this.batcher.HasOpenResources;
      }
    }

    public IBatcher Batcher => this.batcher;

    public IDisposable FlushingFromDtcTransaction
    {
      get
      {
        this.flushingFromDtcTransaction = true;
        return (IDisposable) new ConnectionManager.StopFlushingFromDtcTransaction(this);
      }
    }

    public IDbCommand CreateCommand()
    {
      IDbCommand command = this.GetConnection().CreateCommand();
      this.Transaction.Enlist(command);
      return command;
    }

    public interface Callback
    {
      void ConnectionOpened();

      void ConnectionCleanedUp();

      bool IsTransactionInProgress { get; }
    }

    private class StopFlushingFromDtcTransaction : IDisposable
    {
      private readonly ConnectionManager manager;

      public StopFlushingFromDtcTransaction(ConnectionManager manager) => this.manager = manager;

      public void Dispose() => this.manager.flushingFromDtcTransaction = false;
    }
  }
}
