// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeConnection
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Transactions;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeConnection : DbConnection
  {
    private static Hashtable connStrCache;
    private Hashtable connTokens;
    private SqlCeDelegatedTransaction _delegatedTransaction;
    private bool isOpened;
    private bool isHostControlled;
    private bool removePwd;
    private IntPtr pStoreService;
    private IntPtr pStoreServer;
    private IntPtr pSeStore;
    private IntPtr pQpServices;
    private IntPtr pQpDatabase;
    private IntPtr pQpSession;
    private IntPtr pTx;
    private IntPtr pStoreEvents;
    private IntPtr pError;
    private string connStr;
    private string dataSource;
    private string modifiedConnStr;
    private ConnectionState state;
    private bool isDisposed;
    private SqlCeConnection.ObjectLifeTimeTracker weakReferenceCache;
    private FlushFailureEventHandler flushFailureEventHandler;

    public string DatabaseIdentifier
    {
      get
      {
        this.CheckStateOpen("GetDatabaseInfo");
        string empty = string.Empty;
        IntPtr pwszGuidString = (IntPtr) 0;
        int databaseInstanceId = NativeMethods.GetDatabaseInstanceID(this.pSeStore, out pwszGuidString, this.pError);
        if (databaseInstanceId != 0)
          this.ProcessResults(databaseInstanceId);
        string stringBstr = Marshal.PtrToStringBSTR(pwszGuidString);
        NativeMethods.uwutil_SysFreeString(pwszGuidString);
        return stringBstr;
      }
    }

    internal void OnFlushFailure(int hr, IntPtr pError)
    {
      SqlCeFlushFailureEventHandler failureEventHandler = (SqlCeFlushFailureEventHandler) this.Events[ADP.EventFlushFailure];
      if (failureEventHandler == null)
        return;
      try
      {
        failureEventHandler((object) this, new SqlCeFlushFailureEventArgs(hr, pError, (object) this));
      }
      catch (Exception ex)
      {
        if (!ADP.IsCatchableExceptionType(ex))
          throw ex;
      }
    }

    public override string ConnectionString
    {
      get
      {
        if (this.connStr == null || this.connStr.Trim().Length == 0)
          return this.connStr = string.Empty;
        if (this.removePwd)
        {
          if (this.connTokens == null)
            return string.Empty;
          string str = "ssce:database password";
          string key = "persist security info";
          if (this.connTokens.Contains((object) str) && (!this.connTokens.Contains((object) key) || !(bool) this.connTokens[(object) key]))
            this.connStr = ConStringUtil.RemoveKeyValuesFromString(this.connStr, str);
          this.removePwd = false;
        }
        return this.connStr;
      }
      set
      {
        if (this.state != ConnectionState.Closed)
          throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionPropertySet", (object) nameof (ConnectionString), (object) this.state));
        Hashtable connStrCache = SqlCeConnection.connStrCache;
        if (connStrCache != null && value != null && connStrCache.Contains((object) value))
        {
          object[] objArray = (object[]) connStrCache[(object) value];
          this.modifiedConnStr = (string) objArray[0];
          if (this.state != ConnectionState.Closed)
            throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionPropertySet", (object) nameof (ConnectionString), (object) this.state));
          this.connTokens = (Hashtable) objArray[1];
        }
        else if (value != null && value.Length > 0)
        {
          this.connTokens = ConStringUtil.ParseConnectionString(ref value);
          this.modifiedConnStr = value;
          if (this.connTokens != null)
            SqlCeConnection.CachedConnectionStringAdd(value, this.modifiedConnStr, this.connTokens);
          else
            this.modifiedConnStr = (string) null;
        }
        else
        {
          this.modifiedConnStr = (string) null;
          this.connTokens = (Hashtable) null;
        }
        this.connStr = value;
        this.removePwd = false;
        if (this.connTokens == null)
          return;
        this.dataSource = (string) this.connTokens[(object) "data source"];
      }
    }

    public override int ConnectionTimeout => 0;

    public override string Database => this.dataSource;

    public override string DataSource => this.dataSource;

    internal SqlCeDelegatedTransaction DelegatedTransaction
    {
      get => this._delegatedTransaction;
      set => this._delegatedTransaction = value;
    }

    internal bool HasDelegatedTransaction => null != this._delegatedTransaction;

    public override ConnectionState State => this.state;

    public override string ServerVersion => "3.5.8080.0";

    protected override DbProviderFactory DbProviderFactory
    {
      get => (DbProviderFactory) SqlCeProviderFactory.Instance;
    }

    public event SqlCeInfoMessageEventHandler InfoMessage
    {
      add => this.Events.AddHandler(ADP.EventInfoMessage, (Delegate) value);
      remove => this.Events.RemoveHandler(ADP.EventInfoMessage, (Delegate) value);
    }

    public event SqlCeFlushFailureEventHandler FlushFailure
    {
      add => this.Events.AddHandler(ADP.EventFlushFailure, (Delegate) value);
      remove => this.Events.RemoveHandler(ADP.EventFlushFailure, (Delegate) value);
    }

    [Obsolete("This property is obsolete and will be removed in a future version.")]
    public override event StateChangeEventHandler StateChange
    {
      add => this.Events.AddHandler(ADP.EventStateChange, (Delegate) value);
      remove => this.Events.RemoveHandler(ADP.EventStateChange, (Delegate) value);
    }

    internal IntPtr ITransact => this.pTx;

    internal IntPtr IQPSession => this.pQpSession;

    internal IntPtr IQPServices => this.pQpServices;

    internal bool IsEnlisted => (bool) this.connTokens[(object) "ssce:enlist"];

    public override void EnlistTransaction(System.Transactions.Transaction SysTrans)
    {
      if ((System.Transactions.Transaction) null == SysTrans)
        throw new NullReferenceException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (SysTrans)));
      if (!this.IsEnlisted)
        return;
      if (this.Transaction != null)
        throw new InvalidOperationException(Res.GetString("ADP_LocalTransactionPresent"));
      if (this.DelegatedTransaction == null)
      {
        if (ConnectionState.Open == this.State)
        {
          try
          {
            this.Enlist(SysTrans);
            SqlCeDelegatedTransaction delegatedTransaction = this.DelegatedTransaction;
            for (int indx = 0; indx < this.weakReferenceCache.Count; ++indx)
            {
              object obj = this.weakReferenceCache.GetObject(indx);
              if (obj is SqlCeCommand)
                ((SqlCeCommand) obj).Transaction = delegatedTransaction.SqlCeTransaction;
            }
            return;
          }
          catch
          {
            throw;
          }
        }
      }
      if (!this.HasDelegatedTransaction || !(SysTrans == this.DelegatedTransaction.Transaction))
        throw new InvalidOperationException(Res.GetString("ADP_ConnectionNotEnlisted"));
    }

    internal void Enlist(System.Transactions.Transaction tx)
    {
      SqlCeDelegatedTransaction promotableSinglePhaseNotification = !((System.Transactions.Transaction) null == tx) ? new SqlCeDelegatedTransaction(this, tx) : throw new NullReferenceException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "transaction"));
      this._delegatedTransaction = tx.EnlistPromotableSinglePhase((IPromotableSinglePhaseNotification) promotableSinglePhaseNotification) ? promotableSinglePhaseNotification : throw new InvalidOperationException(Res.GetString("ADP_ConnectionNotEnlisted"));
    }

    internal SqlCeTransaction Transaction
    {
      get
      {
        for (int indx = 0; indx < this.weakReferenceCache.Count; ++indx)
        {
          object obj = this.weakReferenceCache.GetObject(indx);
          if (obj is SqlCeTransaction)
          {
            SqlCeTransaction transaction = (SqlCeTransaction) obj;
            if (!this.HasDelegatedTransaction || transaction != this.DelegatedTransaction.SqlCeTransaction)
              return transaction;
          }
        }
        return (SqlCeTransaction) null;
      }
    }

    public SqlCeConnection()
    {
      SqlCeRestriction.CheckExplicitWebHosting();
      NativeMethods.LoadNativeBinaries();
      this.dataSource = string.Empty;
      this.isHostControlled = false;
      this.weakReferenceCache = new SqlCeConnection.ObjectLifeTimeTracker(true);
      NativeMethods.DllAddRef();
    }

    public SqlCeConnection(string connectionString)
      : this()
    {
      this.ConnectionString = connectionString;
    }

    ~SqlCeConnection() => this.Dispose(false);

    private void ReleaseNativeInterfaces()
    {
      if (IntPtr.Zero != this.pQpSession)
        NativeMethods.SafeRelease(ref this.pQpSession);
      if (IntPtr.Zero != this.pQpDatabase)
        NativeMethods.SafeRelease(ref this.pQpDatabase);
      if (IntPtr.Zero != this.pTx)
        NativeMethods.SafeRelease(ref this.pTx);
      if (IntPtr.Zero != this.pStoreService)
        NativeMethods.SafeRelease(ref this.pStoreService);
      if (IntPtr.Zero != this.pQpServices)
        NativeMethods.SafeRelease(ref this.pQpServices);
      if (IntPtr.Zero != this.pStoreServer)
        NativeMethods.SafeRelease(ref this.pStoreServer);
      if (IntPtr.Zero != this.pStoreEvents)
        NativeMethods.SafeRelease(ref this.pStoreEvents);
      if (IntPtr.Zero != this.pError)
        NativeMethods.SafeDelete(ref this.pError);
      if (!(IntPtr.Zero != this.pSeStore))
        return;
      NativeMethods.CloseStore(this.pSeStore);
      NativeMethods.SafeRelease(ref this.pSeStore);
    }

    internal void DisposeSqlCeDataRdr(SqlCeTransaction tx)
    {
      if (this.weakReferenceCache == null)
        return;
      this.weakReferenceCache.CloseDataRdr(tx);
    }

    public new void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (this.isDisposed)
        return;
      if (this.HasDelegatedTransaction)
      {
        if (!disposing)
          return;
        this.isDisposed = true;
        this.state = ConnectionState.Closed;
        if (this.weakReferenceCache == null)
          return;
        this.weakReferenceCache.CloseDataRdr((SqlCeTransaction) null);
      }
      else
      {
        if (disposing)
        {
          if (this.isOpened)
          {
            try
            {
              this.OnStateChange(ConnectionState.Open, ConnectionState.Closed);
            }
            catch (Exception ex)
            {
              if (!ADP.IsCatchableExceptionType(ex))
                throw ex;
            }
          }
          if (this.weakReferenceCache != null)
          {
            this.weakReferenceCache.Close(true);
            this.weakReferenceCache = (SqlCeConnection.ObjectLifeTimeTracker) null;
          }
          this.connStr = (string) null;
          this.dataSource = (string) null;
          this.modifiedConnStr = (string) null;
          NativeMethods.DllRelease();
          this.isOpened = false;
          this.isDisposed = true;
          this.state = ConnectionState.Closed;
        }
        if (this.isHostControlled)
          return;
        this.ReleaseNativeInterfaces();
      }
    }

    public override void Close()
    {
      this.Close(false);
      GC.KeepAlive((object) this);
    }

    internal void Zombie(SqlCeTransaction tx)
    {
      if (this.weakReferenceCache == null)
        return;
      this.weakReferenceCache.Zombie(tx);
    }

    private void Close(bool silent)
    {
      if (!this.isOpened)
        return;
      if (this.HasDelegatedTransaction)
      {
        this.state = ConnectionState.Closed;
        if (this.weakReferenceCache == null)
          return;
        this.weakReferenceCache.CloseDataRdr((SqlCeTransaction) null);
      }
      else
      {
        if (!silent)
          this.OnStateChange(ConnectionState.Open, ConnectionState.Closed);
        if (this.weakReferenceCache != null)
          this.weakReferenceCache.Close(false);
        this.ReleaseNativeInterfaces();
        this.isOpened = false;
        this.state = ConnectionState.Closed;
      }
    }

    public List<KeyValuePair<string, string>> GetDatabaseInfo()
    {
      List<KeyValuePair<string, string>> databaseInfo = new List<KeyValuePair<string, string>>();
      int locale1 = 0;
      this.CheckStateOpen(nameof (GetDatabaseInfo));
      int locale2 = NativeMethods.GetLocale(this.pSeStore, ref locale1, this.pError);
      if (locale2 != 0)
        this.ProcessResults(locale2);
      databaseInfo.Add(new KeyValuePair<string, string>("locale identifier", locale1.ToString()));
      int encryptionMode1 = 0;
      int encryptionMode2 = NativeMethods.GetEncryptionMode(this.pSeStore, ref encryptionMode1, this.pError);
      if (encryptionMode2 != 0)
        this.ProcessResults(encryptionMode2);
      string str1 = (string) null;
      switch (encryptionMode1)
      {
        case 0:
          str1 = string.Empty;
          break;
        case 2:
          str1 = "engine default";
          break;
        case 3:
          str1 = "ppc2003 compatibility";
          break;
      }
      databaseInfo.Add(new KeyValuePair<string, string>("encryption mode", str1));
      int sortFlags = 0;
      int localeFlags = NativeMethods.GetLocaleFlags(this.pSeStore, ref sortFlags, this.pError);
      if (localeFlags != 0)
        this.ProcessResults(localeFlags);
      string str2 = 1 != (sortFlags & 1) ? bool.TrueString : bool.FalseString;
      databaseInfo.Add(new KeyValuePair<string, string>("case sensitive", str2));
      return databaseInfo;
    }

    public SqlCeTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
    {
      if (this.HasDelegatedTransaction)
        throw new InvalidOperationException(Res.GetString("ADP_ParallelTransactionsNotSupported", (object) this.GetType().Name));
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeConnection));
      this.CheckStateOpen(nameof (BeginTransaction));
      SEISOLATION isoLevel;
      switch (isolationLevel)
      {
        case System.Data.IsolationLevel.Unspecified:
        case System.Data.IsolationLevel.ReadCommitted:
          isoLevel = SEISOLATION.ISO_READ_COMMITTED;
          break;
        case System.Data.IsolationLevel.RepeatableRead:
          isoLevel = SEISOLATION.ISO_REPEATABLE_READ;
          break;
        case System.Data.IsolationLevel.Serializable:
          isoLevel = SEISOLATION.ISO_SERIALIZABLE;
          break;
        default:
          throw new ArgumentException(Res.GetString("ADP_InvalidIsolationLevel", (object) isolationLevel.ToString()));
      }
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      SqlCeTransaction sqlCeTransaction;
      try
      {
        int hr = NativeMethods.OpenTransaction(this.pSeStore, this.pQpDatabase, isoLevel, ref zero1, ref zero2, this.pError);
        if (hr != 0)
          this.ProcessResults(hr);
        sqlCeTransaction = new SqlCeTransaction(this, isolationLevel, zero1, zero2);
        this.AddWeakReference((object) sqlCeTransaction);
      }
      catch (Exception ex)
      {
        if (IntPtr.Zero != zero2)
          NativeMethods.SafeRelease(ref zero2);
        if (IntPtr.Zero != zero1)
          NativeMethods.SafeRelease(ref zero1);
        throw ex;
      }
      return sqlCeTransaction;
    }

    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
    {
      return (DbTransaction) this.BeginTransaction(isolationLevel);
    }

    public SqlCeTransaction BeginTransaction()
    {
      return this.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
    }

    public override void ChangeDatabase(string value)
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeConnection));
      this.CheckStateOpen(nameof (ChangeDatabase));
      if (value == null || value.Trim().Length == 0)
        throw new ArgumentException(Res.GetString("ADP_EmptyDatabaseName"));
      string dataSource = this.dataSource;
      try
      {
        this.dataSource = value;
        this.Close(true);
        this.Open(true);
      }
      catch (Exception ex)
      {
        this.dataSource = dataSource;
        throw ex;
      }
    }

    internal void CheckStateOpen(string method)
    {
      if (ConnectionState.Open != this.State)
      {
        string name = (string) null;
        switch (method)
        {
          case "BeginTransaction":
            name = "ADP_OpenConnectionRequired_BeginTransaction";
            break;
          case "ChangeDatabase":
            name = "ADP_OpenConnectionRequired_ChangeDatabase";
            break;
          case "CommitTransaction":
            name = "ADP_OpenConnectionRequired_CommitTransaction";
            break;
          case "RollbackTransaction":
            name = "ADP_OpenConnectionRequired_RollbackTransaction";
            break;
          case "set_Connection":
            name = "ADP_OpenConnectionRequired_SetConnection";
            break;
          case "GetDatabaseInfo":
            name = "ADP_OpenConnectionRequired_GetDatabaseInfo";
            break;
        }
        throw new InvalidOperationException(Res.GetString(name, (object) method, (object) this.State));
      }
    }

    internal void AddWeakReference(object value)
    {
      if (this.isDisposed || this.weakReferenceCache == null)
        throw new ObjectDisposedException(nameof (SqlCeConnection));
      this.weakReferenceCache.Add(value);
    }

    internal void RemoveWeakReference(object value)
    {
      if (this.weakReferenceCache == null)
        throw new ObjectDisposedException(nameof (SqlCeConnection));
      this.weakReferenceCache.Remove(value);
    }

    internal bool HasOpenedCursors(SqlCeTransaction tx)
    {
      return this.weakReferenceCache != null && this.weakReferenceCache.HasOpenedCursors(tx);
    }

    protected override DbCommand CreateDbCommand() => (DbCommand) this.CreateCommand();

    public SqlCeCommand CreateCommand()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeConnection));
      return new SqlCeCommand("", this);
    }

    private void OnStateChange(ConnectionState original, ConnectionState state)
    {
      StateChangeEventHandler changeEventHandler = (StateChangeEventHandler) this.Events[ADP.EventStateChange];
      if (changeEventHandler == null)
        return;
      try
      {
        changeEventHandler((object) this, new StateChangeEventArgs(original, state));
      }
      catch (Exception ex)
      {
        if (!ADP.IsCatchableExceptionType(ex))
          throw ex;
      }
    }

    public override void Open()
    {
      if (this.HasDelegatedTransaction)
        throw new InvalidOperationException(Res.GetString("ADP_ConnectionNotEnlisted"));
      this.Open(false);
      if (!this.IsEnlisted)
        return;
      if (!((System.Transactions.Transaction) null != System.Transactions.Transaction.Current))
        return;
      try
      {
        this.Enlist(System.Transactions.Transaction.Current);
        SqlCeDelegatedTransaction delegatedTransaction = this.DelegatedTransaction;
        for (int indx = 0; indx < this.weakReferenceCache.Count; ++indx)
        {
          object obj = this.weakReferenceCache.GetObject(indx);
          if (obj is SqlCeCommand)
            ((SqlCeCommand) obj).Transaction = delegatedTransaction.SqlCeTransaction;
        }
      }
      catch
      {
        this.Close();
        throw;
      }
    }

    internal void Open(bool silent)
    {
      int num1 = -1;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      int num5 = -1;
      int num6 = -1;
      int num7 = -1;
      int num8 = -1;
      string source1 = (string) null;
      string source2 = (string) null;
      string str1 = (string) null;
      SEOPENFLAGS seopenflags = SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE;
      bool flag = false;
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeConnection));
      if (this.ConnectionString == null || this.ConnectionString.Length == 0)
        throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
      if (this.dataSource == null || this.dataSource.Trim().Length == 0)
        throw new ArgumentException(Res.GetString("ADP_EmptyDatabaseName"));
      if (this.isOpened)
        throw new InvalidOperationException(Res.GetString("ADP_ConnectionAlreadyOpen", (object) ConnectionState.Open.ToString()));
      MEOPENINFO structure = new MEOPENINFO();
      IntPtr num9 = NativeMethods.CoTaskMemAlloc(sizeof (MEOPENINFO));
      if (IntPtr.Zero == num9)
        throw new OutOfMemoryException();
      try
      {
        if (ADP.IsEmpty(this.modifiedConnStr))
          throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
        object connToken1 = this.connTokens[(object) "locale identifier"];
        if (connToken1 != null)
          num1 = (int) connToken1;
        object connToken2 = this.connTokens[(object) "ssce:max buffer size"];
        if (connToken2 != null)
          num2 = (int) connToken2 * 1024;
        object connToken3 = this.connTokens[(object) "ssce:autoshrink threshold"];
        if (connToken3 != null)
          num3 = (int) connToken3;
        object connToken4 = this.connTokens[(object) "ssce:max database size"];
        if (connToken4 != null)
          num4 = (int) connToken4 * 256;
        object connToken5 = this.connTokens[(object) "ssce:temp file max size"];
        if (connToken5 != null)
          num5 = (int) connToken5 * 256;
        object connToken6 = this.connTokens[(object) "ssce:flush interval"];
        if (connToken6 != null)
          num8 = (int) connToken6;
        object connToken7 = this.connTokens[(object) "ssce:default lock escalation"];
        if (connToken7 != null)
          num6 = (int) connToken7;
        object connToken8 = this.connTokens[(object) "ssce:default lock timeout"];
        if (connToken8 != null)
          num7 = (int) connToken8;
        object connToken9 = this.connTokens[(object) "ssce:temp file directory"];
        if (connToken9 != null)
          source2 = (string) connToken9;
        object connToken10 = this.connTokens[(object) "ssce:encryption mode"];
        if (connToken10 != null)
          str1 = (string) connToken10;
        object connToken11 = this.connTokens[(object) "ssce:database password"];
        if (connToken11 != null)
        {
          string str2 = (string) connToken11;
          if (str2.Length > 0)
            source1 = str2;
        }
        object connToken12 = this.connTokens[(object) "ssce:case sensitive"];
        if (connToken12 != null)
          flag = (bool) connToken12;
        string str3 = (string) null;
        object connToken13 = this.connTokens[(object) "ssce:mode"];
        if (connToken13 != null)
          str3 = (string) connToken13;
        if (str3 != null)
        {
          switch (str3.ToLower(CultureInfo.CurrentCulture))
          {
            case "read only":
              seopenflags = SEOPENFLAGS.MODE_READ;
              break;
            case "read write":
              seopenflags = SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE;
              break;
            case "exclusive":
              seopenflags = SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE | SEOPENFLAGS.MODE_SHARE_DENY_READ | SEOPENFLAGS.MODE_SHARE_DENY_WRITE;
              break;
            case "shared read":
              seopenflags = SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE | SEOPENFLAGS.MODE_SHARE_DENY_WRITE;
              break;
          }
        }
        structure.pwszFileName = NativeMethods.MarshalStringToLPWSTR(this.dataSource);
        structure.pwszPassword = NativeMethods.MarshalStringToLPWSTR(source1);
        structure.pwszTempPath = NativeMethods.MarshalStringToLPWSTR(source2);
        structure.lcidLocale = num1;
        structure.cbBufferPool = num2;
        structure.dwAutoShrinkPercent = num3;
        structure.dwFlushInterval = num8;
        structure.cMaxPages = num4;
        structure.cMaxTmpPages = num5;
        structure.dwDefaultTimeout = num7;
        structure.dwDefaultEscalation = num6;
        structure.dwFlags = seopenflags;
        structure.dwEncryptionMode = ConStringUtil.MapEncryptionMode(str1);
        structure.dwLocaleFlags = 0;
        if (flag)
          structure.dwLocaleFlags &= 1;
        this.flushFailureEventHandler = new FlushFailureEventHandler(this.OnFlushFailure);
        Marshal.StructureToPtr((object) structure, num9, false);
        int hr = NativeMethods.OpenStore(num9, Marshal.GetFunctionPointerForDelegate((Delegate) this.flushFailureEventHandler), ref this.pStoreService, ref this.pStoreServer, ref this.pQpServices, ref this.pSeStore, ref this.pTx, ref this.pQpDatabase, ref this.pQpSession, ref this.pStoreEvents, ref this.pError);
        if (hr != 0)
          this.ProcessResults(hr);
        this.removePwd = true;
        this.state = ConnectionState.Open;
        this.isOpened = true;
      }
      finally
      {
        NativeMethods.CoTaskMemFree(structure.pwszFileName);
        NativeMethods.CoTaskMemFree(structure.pwszPassword);
        NativeMethods.CoTaskMemFree(structure.pwszTempPath);
        NativeMethods.CoTaskMemFree(num9);
        if (ConnectionState.Open != this.state)
        {
          this.Close();
          this.removePwd = false;
          this.state = ConnectionState.Closed;
        }
      }
      if (silent)
        return;
      this.OnStateChange(ConnectionState.Closed, ConnectionState.Open);
    }

    private static void CachedConnectionStringAdd(
      string connStr,
      string modifiedConnStr,
      Hashtable connTokens)
    {
      Hashtable hashtable = SqlCeConnection.connStrCache;
      lock (typeof (SqlCeConnection))
      {
        if (hashtable == null)
        {
          hashtable = new Hashtable();
          hashtable[(object) connStr] = (object) new object[2]
          {
            (object) modifiedConnStr,
            (object) connTokens
          };
          SqlCeConnection.connStrCache = hashtable;
          return;
        }
      }
      lock (hashtable.SyncRoot)
      {
        if (hashtable.Contains((object) connStr))
          return;
        if (hashtable.Count < 250)
          hashtable[(object) connStr] = (object) new object[2]
          {
            (object) modifiedConnStr,
            (object) connTokens
          };
        else
          SqlCeConnection.connStrCache = (Hashtable) null;
      }
    }

    private void ProcessResults(int hr)
    {
      Exception exception = (Exception) this.ProcessResults(hr, this.pError, (object) this);
      if (exception != null)
        throw exception;
    }

    internal SqlCeException ProcessResults(int hr, IntPtr pError, object src)
    {
      if (hr == 0)
        return (SqlCeException) null;
      if (NativeMethods.Failed(hr))
        return SqlCeException.FillErrorInformation(hr, pError);
      if ((object) this.Events[ADP.EventInfoMessage] != null)
      {
        SqlCeInfoMessageEventHandler messageEventHandler = (SqlCeInfoMessageEventHandler) this.Events[ADP.EventInfoMessage];
        if (messageEventHandler != null)
        {
          try
          {
            messageEventHandler((object) this, new SqlCeInfoMessageEventArgs(hr, pError, src));
          }
          catch (Exception ex)
          {
            if (!ADP.IsCatchableExceptionType(ex))
              throw ex;
          }
        }
      }
      else
        NativeMethods.ClearErrorInfo(pError);
      return (SqlCeException) null;
    }

    private class ObjectLifeTimeTracker : WeakReferenceCache
    {
      internal ObjectLifeTimeTracker(bool trackResurrection)
        : base(trackResurrection)
      {
      }

      internal bool HasOpenedCursors(SqlCeTransaction tx)
      {
        lock (this)
        {
          int length = this.items.Length;
          for (int index = 0; index < length; ++index)
          {
            WeakReference weakReference = this.items[index];
            if (ADP.IsAlive(weakReference))
            {
              object target;
              try
              {
                target = weakReference.Target;
                if (target == null)
                  continue;
              }
              catch (InvalidOperationException ex)
              {
                continue;
              }
              if (target is SqlCeDataReader)
              {
                SqlCeDataReader sqlCeDataReader = (SqlCeDataReader) target;
                if (tx == sqlCeDataReader.transaction && !sqlCeDataReader.IsClosed)
                  return true;
              }
            }
          }
          return false;
        }
      }

      internal void CloseDataRdr(SqlCeTransaction tx)
      {
        ArrayList arrayList = new ArrayList();
        int length = this.items.Length;
        for (int index = 0; index < length; ++index)
        {
          WeakReference weakReference = this.items[index];
          if (ADP.IsAlive(weakReference))
          {
            object target;
            try
            {
              target = weakReference.Target;
              if (target == null)
                continue;
            }
            catch (InvalidOperationException ex)
            {
              continue;
            }
            if (target is SqlCeDataReader)
            {
              SqlCeDataReader sqlCeDataReader = (SqlCeDataReader) target;
              if ((tx == null || tx == sqlCeDataReader.transaction) && !sqlCeDataReader.IsClosed)
              {
                arrayList.Add(target);
                this.items[index] = (WeakReference) null;
              }
            }
          }
        }
        foreach (SqlCeDataReader sqlCeDataReader in arrayList)
          sqlCeDataReader.Dispose();
      }

      internal void Close(bool isDisposing)
      {
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        int length = this.items.Length;
        for (int index = 0; index < length; ++index)
        {
          WeakReference weakReference = this.items[index];
          if (ADP.IsAlive(weakReference))
          {
            object target;
            try
            {
              target = weakReference.Target;
              if (target == null)
                continue;
            }
            catch (InvalidOperationException ex)
            {
              continue;
            }
            if (target is SqlCeDataReader)
            {
              arrayList3.Add(target);
              this.items[index] = (WeakReference) null;
            }
            else if (target is SqlCeCommand)
              arrayList2.Add(target);
            else if (target is SqlCeTransaction)
            {
              arrayList1.Add(target);
              this.items[index] = (WeakReference) null;
            }
            else if (target is SqlCeChangeTracking)
            {
              arrayList4.Add(target);
              this.items[index] = (WeakReference) null;
            }
          }
        }
        foreach (SqlCeDataReader sqlCeDataReader in arrayList3)
          sqlCeDataReader.Dispose();
        foreach (SqlCeChangeTracking ceChangeTracking in arrayList4)
          ceChangeTracking.Dispose();
        foreach (SqlCeCommand sqlCeCommand in arrayList2)
        {
          sqlCeCommand.CloseFromConnection();
          if (isDisposing)
            sqlCeCommand.Connection = (SqlCeConnection) null;
        }
        foreach (SqlCeTransaction sqlCeTransaction in arrayList1)
          sqlCeTransaction.Dispose();
      }

      internal void Zombie(SqlCeTransaction tx)
      {
        lock (this)
        {
          int length = this.items.Length;
          for (int index = 0; index < length; ++index)
          {
            WeakReference weakReference = this.items[index];
            if (ADP.IsAlive(weakReference))
            {
              object target;
              try
              {
                target = weakReference.Target;
                if (target == null)
                  continue;
              }
              catch (InvalidOperationException ex)
              {
                continue;
              }
              if (target is SqlCeCommand && tx == ((SqlCeCommand) target).Transaction)
                ((SqlCeCommand) target).Transaction = (SqlCeTransaction) null;
            }
          }
        }
      }
    }
  }
}
