// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnection
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Transactions;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteConnection : DbConnection, ICloneable, IDisposable
  {
    internal const DbType BadDbType = ~DbType.AnsiString;
    internal const string DefaultBaseSchemaName = "sqlite_default_schema";
    private const string MemoryFileName = ":memory:";
    internal const System.Data.IsolationLevel DeferredIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
    internal const System.Data.IsolationLevel ImmediateIsolationLevel = System.Data.IsolationLevel.Serializable;
    private const SQLiteConnectionFlags FallbackDefaultFlags = SQLiteConnectionFlags.Default;
    private const SQLiteSynchronousEnum DefaultSynchronous = SQLiteSynchronousEnum.Default;
    private const SQLiteJournalModeEnum DefaultJournalMode = SQLiteJournalModeEnum.Default;
    private const System.Data.IsolationLevel DefaultIsolationLevel = System.Data.IsolationLevel.Serializable;
    internal const SQLiteDateFormats DefaultDateTimeFormat = SQLiteDateFormats.ISO8601;
    internal const DateTimeKind DefaultDateTimeKind = DateTimeKind.Unspecified;
    internal const string DefaultDateTimeFormatString = null;
    private const string DefaultDataSource = null;
    private const string DefaultUri = null;
    private const string DefaultFullUri = null;
    private const string DefaultHexPassword = null;
    private const string DefaultPassword = null;
    private const int DefaultVersion = 3;
    private const int DefaultPageSize = 4096;
    private const int DefaultMaxPageCount = 0;
    private const int DefaultCacheSize = -2000;
    private const int DefaultMaxPoolSize = 100;
    private const int DefaultConnectionTimeout = 30;
    private const int DefaultBusyTimeout = 0;
    private const bool DefaultNoDefaultFlags = false;
    private const bool DefaultNoSharedFlags = false;
    private const bool DefaultFailIfMissing = false;
    private const bool DefaultReadOnly = false;
    internal const bool DefaultBinaryGUID = true;
    private const bool DefaultUseUTF16Encoding = false;
    private const bool DefaultToFullPath = true;
    private const bool DefaultPooling = false;
    private const bool DefaultLegacyFormat = false;
    private const bool DefaultForeignKeys = false;
    private const bool DefaultRecursiveTriggers = false;
    private const bool DefaultEnlist = true;
    private const bool DefaultSetDefaults = true;
    internal const int DefaultPrepareRetries = 3;
    private const string DefaultVfsName = null;
    private const int DefaultProgressOps = 0;
    private const int SQLITE_FCNTL_CHUNK_SIZE = 6;
    private const int SQLITE_FCNTL_WIN32_AV_RETRY = 9;
    private const string _dataDirectory = "|DataDirectory|";
    private const string _masterdb = "sqlite_master";
    private const string _tempmasterdb = "sqlite_temp_master";
    private static readonly Assembly _assembly = typeof (SQLiteConnection).Assembly;
    private static readonly object _syncRoot = new object();
    private static SQLiteConnectionFlags _sharedFlags;
    [ThreadStatic]
    private static SQLiteConnection _lastConnectionInOpen;
    private ConnectionState _connectionState;
    private string _connectionString;
    internal int _transactionLevel;
    internal bool _noDispose;
    private bool _disposing;
    private System.Data.IsolationLevel _defaultIsolation;
    internal SQLiteEnlistment _enlistment;
    internal SQLiteDbTypeMap _typeNames;
    private SQLiteTypeCallbacksMap _typeCallbacks;
    internal SQLiteBase _sql;
    private string _dataSource;
    private byte[] _password;
    internal string _baseSchemaName;
    private SQLiteConnectionFlags _flags;
    private Dictionary<string, object> _cachedSettings;
    private DbType? _defaultDbType;
    private string _defaultTypeName;
    private string _vfsName;
    private int _defaultTimeout = 30;
    private int _busyTimeout;
    internal int _prepareRetries = 3;
    private int _progressOps;
    private bool _parseViaFramework;
    internal bool _binaryGuid;
    internal int _version;
    private SQLiteProgressCallback _progressCallback;
    private SQLiteAuthorizerCallback _authorizerCallback;
    private SQLiteUpdateCallback _updateCallback;
    private SQLiteCommitCallback _commitCallback;
    private SQLiteTraceCallback _traceCallback;
    private SQLiteRollbackCallback _rollbackCallback;
    private bool disposed;

    private static event SQLiteConnectionEventHandler _handlers;

    private event SQLiteProgressEventHandler _progressHandler;

    private event SQLiteAuthorizerEventHandler _authorizerHandler;

    private event SQLiteUpdateEventHandler _updateHandler;

    private event SQLiteCommitHandler _commitHandler;

    private event SQLiteTraceEventHandler _traceHandler;

    private event EventHandler _rollbackHandler;

    public override event StateChangeEventHandler StateChange;

    public SQLiteConnection()
      : this((string) null)
    {
    }

    public SQLiteConnection(string connectionString)
      : this(connectionString, false)
    {
    }

    internal SQLiteConnection(IntPtr db, string fileName, bool ownHandle)
      : this()
    {
      this._sql = (SQLiteBase) new SQLite3(SQLiteDateFormats.ISO8601, DateTimeKind.Unspecified, (string) null, db, fileName, ownHandle);
      this._flags = SQLiteConnectionFlags.None;
      this._connectionState = db != IntPtr.Zero ? ConnectionState.Open : ConnectionState.Closed;
      this._connectionString = (string) null;
    }

    public SQLiteConnection(string connectionString, bool parseViaFramework)
    {
      this._noDispose = false;
      UnsafeNativeMethods.Initialize();
      SQLiteLog.Initialize();
      this._cachedSettings = new Dictionary<string, object>((IEqualityComparer<string>) new TypeNameStringComparer());
      this._typeNames = new SQLiteDbTypeMap();
      this._typeCallbacks = new SQLiteTypeCallbacksMap();
      this._parseViaFramework = parseViaFramework;
      this._flags = SQLiteConnectionFlags.None;
      this._defaultDbType = new DbType?();
      this._defaultTypeName = (string) null;
      this._vfsName = (string) null;
      this._connectionState = ConnectionState.Closed;
      this._connectionString = (string) null;
      if (connectionString == null)
        return;
      this.ConnectionString = connectionString;
    }

    public SQLiteConnection(SQLiteConnection connection)
      : this(connection.ConnectionString, connection.ParseViaFramework)
    {
      if (connection.State != ConnectionState.Open)
        return;
      this.Open();
      using (DataTable schema = connection.GetSchema("Catalogs"))
      {
        foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
        {
          string strA = row[0].ToString();
          if (string.Compare(strA, "main", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(strA, "temp", StringComparison.OrdinalIgnoreCase) != 0)
          {
            using (SQLiteCommand command = this.CreateCommand())
            {
              command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "ATTACH DATABASE '{0}' AS [{1}]", row[1], row[0]);
              command.ExecuteNonQuery();
            }
          }
        }
      }
    }

    internal static void OnChanged(SQLiteConnection connection, ConnectionEventArgs e)
    {
      if (connection != null && !connection.CanRaiseEvents)
        return;
      SQLiteConnectionEventHandler connectionEventHandler;
      lock (SQLiteConnection._syncRoot)
        connectionEventHandler = SQLiteConnection._handlers == null ? (SQLiteConnectionEventHandler) null : SQLiteConnection._handlers.Clone() as SQLiteConnectionEventHandler;
      if (connectionEventHandler == null)
        return;
      connectionEventHandler((object) connection, e);
    }

    public static event SQLiteConnectionEventHandler Changed
    {
      add
      {
        lock (SQLiteConnection._syncRoot)
        {
          SQLiteConnection._handlers -= value;
          SQLiteConnection._handlers += value;
        }
      }
      remove
      {
        lock (SQLiteConnection._syncRoot)
          SQLiteConnection._handlers -= value;
      }
    }

    public static ISQLiteConnectionPool ConnectionPool
    {
      get => SQLiteConnectionPool.GetConnectionPool();
      set => SQLiteConnectionPool.SetConnectionPool(value);
    }

    public static object CreateHandle(IntPtr nativeHandle)
    {
      SQLiteConnectionHandle handle;
      try
      {
      }
      finally
      {
        handle = nativeHandle != IntPtr.Zero ? new SQLiteConnectionHandle(nativeHandle, true) : (SQLiteConnectionHandle) null;
      }
      if (handle != null)
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) handle, (string) null, (object) new object[2]
        {
          (object) typeof (SQLiteConnection),
          (object) nativeHandle
        }));
      return (object) handle;
    }

    public void BackupDatabase(
      SQLiteConnection destination,
      string destinationName,
      string sourceName,
      int pages,
      SQLiteBackupCallback callback,
      int retryMilliseconds)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Source database is not open.");
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      if (destination._connectionState != ConnectionState.Open)
        throw new ArgumentException("Destination database is not open.", nameof (destination));
      if (destinationName == null)
        throw new ArgumentNullException(nameof (destinationName));
      if (sourceName == null)
        throw new ArgumentNullException(nameof (sourceName));
      SQLiteBase sql = this._sql;
      if (sql == null)
        throw new InvalidOperationException("Connection object has an invalid handle.");
      SQLiteBackup backup = (SQLiteBackup) null;
      try
      {
        backup = sql.InitializeBackup(destination, destinationName, sourceName);
        bool retry = false;
        while (sql.StepBackup(backup, pages, ref retry) && (callback == null || callback(this, sourceName, destination, destinationName, pages, sql.RemainingBackup(backup), sql.PageCountBackup(backup), retry)))
        {
          if (retry && retryMilliseconds >= 0)
            Thread.Sleep(retryMilliseconds);
          if (pages == 0)
            break;
        }
      }
      catch (Exception ex)
      {
        if ((this._flags & SQLiteConnectionFlags.LogBackup) == SQLiteConnectionFlags.LogBackup)
          SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception while backing up database: {0}", (object) ex));
        throw;
      }
      finally
      {
        if (backup != null)
          sql.FinishBackup(backup);
      }
    }

    public int ClearCachedSettings()
    {
      this.CheckDisposed();
      int num = -1;
      if (this._cachedSettings != null)
      {
        num = this._cachedSettings.Count;
        this._cachedSettings.Clear();
      }
      return num;
    }

    internal bool TryGetCachedSetting(string name, object @default, out object value)
    {
      if (name != null && this._cachedSettings != null)
        return this._cachedSettings.TryGetValue(name, out value);
      value = @default;
      return false;
    }

    internal void SetCachedSetting(string name, object value)
    {
      if (name == null || this._cachedSettings == null)
        return;
      this._cachedSettings[name] = value;
    }

    public int ClearTypeMappings()
    {
      this.CheckDisposed();
      int num = -1;
      if (this._typeNames != null)
        num = this._typeNames.Clear();
      return num;
    }

    public Dictionary<string, object> GetTypeMappings()
    {
      this.CheckDisposed();
      Dictionary<string, object> typeMappings = (Dictionary<string, object>) null;
      if (this._typeNames != null)
      {
        typeMappings = new Dictionary<string, object>(this._typeNames.Count, this._typeNames.Comparer);
        foreach (KeyValuePair<string, SQLiteDbTypeMapping> typeName in (Dictionary<string, SQLiteDbTypeMapping>) this._typeNames)
        {
          SQLiteDbTypeMapping liteDbTypeMapping = typeName.Value;
          object obj1 = (object) null;
          object obj2 = (object) null;
          object obj3 = (object) null;
          if (liteDbTypeMapping != null)
          {
            obj1 = (object) liteDbTypeMapping.typeName;
            obj2 = (object) liteDbTypeMapping.dataType;
            obj3 = (object) liteDbTypeMapping.primary;
          }
          typeMappings.Add(typeName.Key, (object) new object[3]
          {
            obj1,
            obj2,
            obj3
          });
        }
      }
      return typeMappings;
    }

    public int AddTypeMapping(string typeName, DbType dataType, bool primary)
    {
      this.CheckDisposed();
      if (typeName == null)
        throw new ArgumentNullException(nameof (typeName));
      int num = -1;
      if (this._typeNames != null)
      {
        num = 0;
        if (primary && this._typeNames.ContainsKey(dataType))
          num += this._typeNames.Remove(dataType) ? 1 : 0;
        if (this._typeNames.ContainsKey(typeName))
          num += this._typeNames.Remove(typeName) ? 1 : 0;
        this._typeNames.Add(new SQLiteDbTypeMapping(typeName, dataType, primary));
      }
      return num;
    }

    public int ClearTypeCallbacks()
    {
      this.CheckDisposed();
      int num = -1;
      if (this._typeCallbacks != null)
      {
        num = this._typeCallbacks.Count;
        this._typeCallbacks.Clear();
      }
      return num;
    }

    public bool TryGetTypeCallbacks(string typeName, out SQLiteTypeCallbacks callbacks)
    {
      this.CheckDisposed();
      if (typeName == null)
        throw new ArgumentNullException(nameof (typeName));
      if (this._typeCallbacks != null)
        return this._typeCallbacks.TryGetValue(typeName, out callbacks);
      callbacks = (SQLiteTypeCallbacks) null;
      return false;
    }

    public bool SetTypeCallbacks(string typeName, SQLiteTypeCallbacks callbacks)
    {
      this.CheckDisposed();
      if (typeName == null)
        throw new ArgumentNullException(nameof (typeName));
      if (this._typeCallbacks == null)
        return false;
      if (callbacks == null)
        return this._typeCallbacks.Remove(typeName);
      callbacks.TypeName = typeName;
      this._typeCallbacks[typeName] = callbacks;
      return true;
    }

    public void BindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteFunction function)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for binding functions.");
      this._sql.BindFunction(functionAttribute, function, this._flags);
    }

    public void BindFunction(
      SQLiteFunctionAttribute functionAttribute,
      Delegate callback1,
      Delegate callback2)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for binding functions.");
      this._sql.BindFunction(functionAttribute, (SQLiteFunction) new SQLiteDelegateFunction(callback1, callback2), this._flags);
    }

    public bool UnbindFunction(SQLiteFunctionAttribute functionAttribute)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for unbinding functions.");
      return this._sql.UnbindFunction(functionAttribute, this._flags);
    }

    public bool UnbindAllFunctions(bool registered)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for unbinding functions.");
      return SQLiteFunction.UnbindAllFunctions(this._sql, this._flags, registered);
    }

    [Conditional("CHECK_STATE")]
    internal static void Check(SQLiteConnection connection)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      connection.CheckDisposed();
      if (connection._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("The connection is not open.");
      if (!(connection._sql is SQLite3 sql1))
        throw new InvalidOperationException("The connection handle wrapper is null.");
      SQLiteConnectionHandle sql2 = sql1._sql;
      if (sql2 == null)
        throw new InvalidOperationException("The connection handle is null.");
      if (sql2.IsInvalid)
        throw new InvalidOperationException("The connection handle is invalid.");
      if (sql2.IsClosed)
        throw new InvalidOperationException("The connection handle is closed.");
    }

    internal static SortedList<string, string> ParseConnectionString(
      string connectionString,
      bool parseViaFramework,
      bool allowNameOnly)
    {
      return SQLiteConnection.ParseConnectionString((SQLiteConnection) null, connectionString, parseViaFramework, allowNameOnly);
    }

    private static SortedList<string, string> ParseConnectionString(
      SQLiteConnection connection,
      string connectionString,
      bool parseViaFramework,
      bool allowNameOnly)
    {
      return !parseViaFramework ? SQLiteConnection.ParseConnectionString(connection, connectionString, allowNameOnly) : SQLiteConnection.ParseConnectionStringViaFramework(connection, connectionString, false);
    }

    private void SetupSQLiteBase(SortedList<string, string> opts)
    {
      SQLiteDateFormats fmt = SQLiteConnection.TryParseEnum(typeof (SQLiteDateFormats), SQLiteConnection.FindKey(opts, "DateTimeFormat", SQLiteDateFormats.ISO8601.ToString()), true) is SQLiteDateFormats sqLiteDateFormats ? sqLiteDateFormats : SQLiteDateFormats.ISO8601;
      DateTimeKind kind = SQLiteConnection.TryParseEnum(typeof (DateTimeKind), SQLiteConnection.FindKey(opts, "DateTimeKind", DateTimeKind.Unspecified.ToString()), true) is DateTimeKind dateTimeKind ? dateTimeKind : DateTimeKind.Unspecified;
      string key = SQLiteConnection.FindKey(opts, "DateTimeFormatString", (string) null);
      if (SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(opts, "UseUTF16Encoding", false.ToString())))
        this._sql = (SQLiteBase) new SQLite3_UTF16(fmt, kind, key, IntPtr.Zero, (string) null, false);
      else
        this._sql = (SQLiteBase) new SQLite3(fmt, kind, key, IntPtr.Zero, (string) null, false);
    }

    public new void Dispose()
    {
      if (this._noDispose)
        return;
      base.Dispose();
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteConnection).Name);
    }

    protected override void Dispose(bool disposing)
    {
      if ((this._flags & SQLiteConnectionFlags.TraceWarning) == SQLiteConnectionFlags.TraceWarning && this._noDispose)
        System.Diagnostics.Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Disposing of connection \"{0}\" with the no-dispose flag set.", (object) this._connectionString));
      this._disposing = true;
      try
      {
        if (this.disposed)
          return;
        this.Close();
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    public object Clone()
    {
      this.CheckDisposed();
      return (object) new SQLiteConnection(this);
    }

    public static void CreateFile(string databaseFileName) => File.Create(databaseFileName).Close();

    internal void OnStateChange(ConnectionState newState, ref StateChangeEventArgs eventArgs)
    {
      ConnectionState connectionState = this._connectionState;
      this._connectionState = newState;
      if (this.StateChange == null || newState == connectionState)
        return;
      StateChangeEventArgs e = new StateChangeEventArgs(connectionState, newState);
      this.StateChange((object) this, e);
      eventArgs = e;
    }

    private static System.Data.IsolationLevel GetFallbackDefaultIsolationLevel()
    {
      return System.Data.IsolationLevel.Serializable;
    }

    internal System.Data.IsolationLevel GetDefaultIsolationLevel() => this._defaultIsolation;

    [Obsolete("Use one of the standard BeginTransaction methods, this one will be removed soon")]
    public SQLiteTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel, bool deferredLock)
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(!deferredLock ? System.Data.IsolationLevel.Serializable : System.Data.IsolationLevel.ReadCommitted);
    }

    [Obsolete("Use one of the standard BeginTransaction methods, this one will be removed soon")]
    public SQLiteTransaction BeginTransaction(bool deferredLock)
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(!deferredLock ? System.Data.IsolationLevel.Serializable : System.Data.IsolationLevel.ReadCommitted);
    }

    public SQLiteTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(isolationLevel);
    }

    public SQLiteTransaction BeginTransaction()
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(this._defaultIsolation);
    }

    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
    {
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException();
      if (isolationLevel == System.Data.IsolationLevel.Unspecified)
        isolationLevel = this._defaultIsolation;
      isolationLevel = this.GetEffectiveIsolationLevel(isolationLevel);
      SQLiteTransaction transaction = isolationLevel == System.Data.IsolationLevel.Serializable || isolationLevel == System.Data.IsolationLevel.ReadCommitted ? new SQLiteTransaction(this, isolationLevel != System.Data.IsolationLevel.Serializable) : throw new ArgumentException(nameof (isolationLevel));
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.NewTransaction, (StateChangeEventArgs) null, (IDbTransaction) transaction, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
      return (DbTransaction) transaction;
    }

    public override void ChangeDatabase(string databaseName)
    {
      this.CheckDisposed();
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.ChangeDatabase, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, databaseName, (object) null));
      throw new NotImplementedException();
    }

    public override void Close()
    {
      this.CheckDisposed();
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Closing, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
      if (this._sql != null)
      {
        if (this._enlistment != null)
        {
          SQLiteConnection sqLiteConnection = new SQLiteConnection()
          {
            _sql = this._sql,
            _transactionLevel = this._transactionLevel,
            _enlistment = this._enlistment,
            _connectionState = this._connectionState,
            _version = this._version
          };
          sqLiteConnection._enlistment._transaction._cnn = sqLiteConnection;
          sqLiteConnection._enlistment._disposeConnection = true;
          this._sql = (SQLiteBase) null;
          this._enlistment = (SQLiteEnlistment) null;
        }
        if (this._sql != null)
        {
          this._sql.Close(!this._disposing);
          this._sql = (SQLiteBase) null;
        }
        this._transactionLevel = 0;
      }
      StateChangeEventArgs eventArgs = (StateChangeEventArgs) null;
      this.OnStateChange(ConnectionState.Closed, ref eventArgs);
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Closed, eventArgs, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
    }

    public int PoolCount => this._sql == null ? 0 : this._sql.CountPool();

    public static void ClearPool(SQLiteConnection connection)
    {
      if (connection._sql == null)
        return;
      connection._sql.ClearPool();
    }

    public static void ClearAllPools() => SQLiteConnectionPool.ClearAllPools();

    [Editor("SQLite.Designer.SQLiteConnectionStringEditor, SQLite.Designer, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [RefreshProperties(RefreshProperties.All)]
    [DefaultValue("")]
    public override string ConnectionString
    {
      get
      {
        this.CheckDisposed();
        return this._connectionString;
      }
      set
      {
        this.CheckDisposed();
        if (value == null)
          throw new ArgumentNullException();
        if (this._connectionState != ConnectionState.Closed)
          throw new InvalidOperationException();
        this._connectionString = value;
      }
    }

    public SQLiteCommand CreateCommand()
    {
      this.CheckDisposed();
      return new SQLiteCommand(this);
    }

    protected override DbCommand CreateDbCommand() => (DbCommand) this.CreateCommand();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string DataSource
    {
      get
      {
        this.CheckDisposed();
        return this._dataSource;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FileName
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.GetFileName("main") : throw new InvalidOperationException("Database connection not valid for getting file name.");
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Database
    {
      get
      {
        this.CheckDisposed();
        return "main";
      }
    }

    internal static string MapUriPath(string path)
    {
      if (path.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
        return path.Substring(7);
      if (path.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
        return path.Substring(5);
      return path.StartsWith("/", StringComparison.OrdinalIgnoreCase) ? path : throw new InvalidOperationException("Invalid connection string: invalid URI");
    }

    private static bool ShouldUseLegacyConnectionStringParser(SQLiteConnection connection)
    {
      string name = "No_SQLiteConnectionNewParser";
      object obj;
      return connection != null && connection.TryGetCachedSetting(name, (object) null, out obj) || connection == null && SQLiteConnection.TryGetLastCachedSetting(name, (object) null, out obj) || UnsafeNativeMethods.GetSettingValue(name, (string) null) != null;
    }

    private static SortedList<string, string> ParseConnectionString(
      string connectionString,
      bool allowNameOnly)
    {
      return SQLiteConnection.ParseConnectionString((SQLiteConnection) null, connectionString, allowNameOnly);
    }

    private static SortedList<string, string> ParseConnectionString(
      SQLiteConnection connection,
      string connectionString,
      bool allowNameOnly)
    {
      string source = connectionString;
      SortedList<string, string> connectionString1 = new SortedList<string, string>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
      string error = (string) null;
      string[] strArray = !SQLiteConnection.ShouldUseLegacyConnectionStringParser(connection) ? SQLiteConvert.NewSplit(source, ';', true, ref error) : SQLiteConvert.Split(source, ';');
      if (strArray == null)
        throw new ArgumentException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Invalid ConnectionString format, cannot parse: {0}", (object) (error ?? "could not split connection string into properties")));
      int length1 = strArray != null ? strArray.Length : 0;
      for (int index = 0; index < length1; ++index)
      {
        if (strArray[index] != null)
        {
          strArray[index] = strArray[index].Trim();
          if (strArray[index].Length != 0)
          {
            int length2 = strArray[index].IndexOf('=');
            if (length2 != -1)
              connectionString1.Add(SQLiteConnection.UnwrapString(strArray[index].Substring(0, length2).Trim()), SQLiteConnection.UnwrapString(strArray[index].Substring(length2 + 1).Trim()));
            else if (allowNameOnly)
              connectionString1.Add(SQLiteConnection.UnwrapString(strArray[index].Trim()), string.Empty);
            else
              throw new ArgumentException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Invalid ConnectionString format for part \"{0}\", no equal sign found", (object) strArray[index]));
          }
        }
      }
      return connectionString1;
    }

    private static SortedList<string, string> ParseConnectionStringViaFramework(
      SQLiteConnection connection,
      string connectionString,
      bool strict)
    {
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
      connectionStringBuilder.ConnectionString = connectionString;
      SortedList<string, string> stringViaFramework = new SortedList<string, string>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (string key in (IEnumerable) connectionStringBuilder.Keys)
      {
        object obj = connectionStringBuilder[key];
        string str = (string) null;
        if (obj is string)
        {
          str = (string) obj;
        }
        else
        {
          if (strict)
            throw new ArgumentException("connection property value is not a string", key);
          if (obj != null)
            str = obj.ToString();
        }
        stringViaFramework.Add(key, str);
      }
      return stringViaFramework;
    }

    public override void EnlistTransaction(Transaction transaction)
    {
      this.CheckDisposed();
      if (this._enlistment != null && transaction == this._enlistment._scope)
        return;
      if (this._enlistment != null)
        throw new ArgumentException("Already enlisted in a transaction");
      if (this._transactionLevel > 0 && transaction != (Transaction) null)
        throw new ArgumentException("Unable to enlist in transaction, a local transaction already exists");
      if (transaction == (Transaction) null)
        throw new ArgumentNullException("Unable to enlist in transaction, it is null");
      bool flag = (this._flags & SQLiteConnectionFlags.StrictEnlistment) == SQLiteConnectionFlags.StrictEnlistment;
      this._enlistment = new SQLiteEnlistment(this, transaction, SQLiteConnection.GetFallbackDefaultIsolationLevel(), flag, flag);
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.EnlistTransaction, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) new object[1]
      {
        (object) this._enlistment
      }));
    }

    internal static string FindKey(SortedList<string, string> items, string key, string defValue)
    {
      string str;
      return string.IsNullOrEmpty(key) || !items.TryGetValue(key, out str) && !items.TryGetValue(key.Replace(" ", string.Empty), out str) && !items.TryGetValue(key.Replace(" ", "_"), out str) ? defValue : str;
    }

    internal static object TryParseEnum(Type type, string value, bool ignoreCase)
    {
      if (!string.IsNullOrEmpty(value))
      {
        try
        {
          return Enum.Parse(type, value, ignoreCase);
        }
        catch
        {
        }
      }
      return (object) null;
    }

    private static bool TryParseByte(string value, NumberStyles style, out byte result)
    {
      return byte.TryParse(value, style, (IFormatProvider) null, out result);
    }

    public void SetConfigurationOption(SQLiteConfigDbOpsEnum option, bool enable)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Database connection not valid for {0} a configuration option.", enable ? (object) "enabling" : (object) "disabling"));
      if (option == SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION && (this._flags & SQLiteConnectionFlags.NoLoadExtension) == SQLiteConnectionFlags.NoLoadExtension)
        throw new SQLiteException("Loading extensions is disabled for this database connection.");
      int num = (int) this._sql.SetConfigurationOption(option, enable);
    }

    public void EnableExtensions(bool enable)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Database connection not valid for {0} extensions.", enable ? (object) "enabling" : (object) "disabling"));
      if ((this._flags & SQLiteConnectionFlags.NoLoadExtension) == SQLiteConnectionFlags.NoLoadExtension)
        throw new SQLiteException("Loading extensions is disabled for this database connection.");
      this._sql.SetLoadExtension(enable);
    }

    public void LoadExtension(string fileName)
    {
      this.CheckDisposed();
      this.LoadExtension(fileName, (string) null);
    }

    public void LoadExtension(string fileName, string procName)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for loading extensions.");
      if ((this._flags & SQLiteConnectionFlags.NoLoadExtension) == SQLiteConnectionFlags.NoLoadExtension)
        throw new SQLiteException("Loading extensions is disabled for this database connection.");
      this._sql.LoadExtension(fileName, procName);
    }

    public void CreateModule(SQLiteModule module)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for creating modules.");
      if ((this._flags & SQLiteConnectionFlags.NoCreateModule) == SQLiteConnectionFlags.NoCreateModule)
        throw new SQLiteException("Creating modules is disabled for this database connection.");
      this._sql.CreateModule(module, this._flags);
    }

    internal static byte[] FromHexString(string text)
    {
      string error = (string) null;
      return SQLiteConnection.FromHexString(text, ref error);
    }

    internal static string ToHexString(byte[] array)
    {
      if (array == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        stringBuilder.AppendFormat("{0:x2}", (object) array[index]);
      return stringBuilder.ToString();
    }

    private static byte[] FromHexString(string text, ref string error)
    {
      if (text == null)
      {
        error = "string is null";
        return (byte[]) null;
      }
      if (text.Length % 2 != 0)
      {
        error = "string contains an odd number of characters";
        return (byte[]) null;
      }
      byte[] numArray = new byte[text.Length / 2];
      for (int startIndex = 0; startIndex < text.Length; startIndex += 2)
      {
        string str = text.Substring(startIndex, 2);
        if (!SQLiteConnection.TryParseByte(str, NumberStyles.HexNumber, out numArray[startIndex / 2]))
        {
          error = HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "string contains \"{0}\", which cannot be converted to a byte value", (object) str);
          return (byte[]) null;
        }
      }
      return numArray;
    }

    private bool GetDefaultPooling()
    {
      bool defaultPooling = false;
      if (defaultPooling)
      {
        if ((this._flags & SQLiteConnectionFlags.NoConnectionPool) == SQLiteConnectionFlags.NoConnectionPool)
          defaultPooling = false;
        if ((this._flags & SQLiteConnectionFlags.UseConnectionPool) == SQLiteConnectionFlags.UseConnectionPool)
          defaultPooling = true;
      }
      else
      {
        if ((this._flags & SQLiteConnectionFlags.UseConnectionPool) == SQLiteConnectionFlags.UseConnectionPool)
          defaultPooling = true;
        if ((this._flags & SQLiteConnectionFlags.NoConnectionPool) == SQLiteConnectionFlags.NoConnectionPool)
          defaultPooling = false;
      }
      return defaultPooling;
    }

    private System.Data.IsolationLevel GetEffectiveIsolationLevel(System.Data.IsolationLevel isolationLevel)
    {
      if ((this._flags & SQLiteConnectionFlags.MapIsolationLevels) != SQLiteConnectionFlags.MapIsolationLevels)
        return isolationLevel;
      switch (isolationLevel)
      {
        case System.Data.IsolationLevel.Unspecified:
        case System.Data.IsolationLevel.Chaos:
        case System.Data.IsolationLevel.ReadUncommitted:
        case System.Data.IsolationLevel.ReadCommitted:
          return System.Data.IsolationLevel.ReadCommitted;
        case System.Data.IsolationLevel.RepeatableRead:
        case System.Data.IsolationLevel.Serializable:
        case System.Data.IsolationLevel.Snapshot:
          return System.Data.IsolationLevel.Serializable;
        default:
          return SQLiteConnection.GetFallbackDefaultIsolationLevel();
      }
    }

    public override void Open()
    {
      this.CheckDisposed();
      SQLiteConnection._lastConnectionInOpen = this;
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Opening, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
      if (this._connectionState != ConnectionState.Closed)
        throw new InvalidOperationException();
      this.Close();
      SortedList<string, string> connectionString = SQLiteConnection.ParseConnectionString(this, this._connectionString, this._parseViaFramework, false);
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.ConnectionString, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, this._connectionString, (object) new object[1]
      {
        (object) connectionString
      }));
      object obj = SQLiteConnection.TryParseEnum(typeof (SQLiteConnectionFlags), SQLiteConnection.FindKey(connectionString, "Flags", (string) null), true);
      bool boolean1 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "NoDefaultFlags", false.ToString()));
      if (obj is SQLiteConnectionFlags liteConnectionFlags)
        this._flags |= liteConnectionFlags;
      else if (!boolean1)
        this._flags |= SQLiteConnection.DefaultFlags;
      if (!SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "NoSharedFlags", false.ToString())))
      {
        lock (SQLiteConnection._syncRoot)
          this._flags |= SQLiteConnection._sharedFlags;
      }
      this._defaultDbType = SQLiteConnection.TryParseEnum(typeof (DbType), SQLiteConnection.FindKey(connectionString, "DefaultDbType", (string) null), true) is DbType dbType ? new DbType?(dbType) : new DbType?();
      if (this._defaultDbType.HasValue && this._defaultDbType.Value == ~DbType.AnsiString)
        this._defaultDbType = new DbType?();
      this._defaultTypeName = SQLiteConnection.FindKey(connectionString, "DefaultTypeName", (string) null);
      this._vfsName = SQLiteConnection.FindKey(connectionString, "VfsName", (string) null);
      bool flag1 = false;
      bool flag2 = false;
      string str = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Version", 3.ToString()), (IFormatProvider) CultureInfo.InvariantCulture) == 3 ? SQLiteConnection.FindKey(connectionString, "Data Source", (string) null) : throw new NotSupportedException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Only SQLite Version {0} is supported at this time", (object) 3));
      if (string.IsNullOrEmpty(str))
      {
        string key = SQLiteConnection.FindKey(connectionString, "Uri", (string) null);
        if (string.IsNullOrEmpty(key))
        {
          str = SQLiteConnection.FindKey(connectionString, "FullUri", (string) null);
          if (string.IsNullOrEmpty(str))
            throw new ArgumentException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Data Source cannot be empty.  Use {0} to open an in-memory database", (object) ":memory:"));
          flag2 = true;
        }
        else
        {
          str = SQLiteConnection.MapUriPath(key);
          flag1 = true;
        }
      }
      bool flag3 = string.Compare(str, ":memory:", StringComparison.OrdinalIgnoreCase) == 0;
      if ((this._flags & SQLiteConnectionFlags.TraceWarning) == SQLiteConnectionFlags.TraceWarning && !flag1 && !flag2 && !flag3 && !string.IsNullOrEmpty(str) && str.StartsWith("\\", StringComparison.OrdinalIgnoreCase) && !str.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
        System.Diagnostics.Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Detected a possibly malformed UNC database file name \"{0}\" that may have originally started with two backslashes; however, four leading backslashes may be required, e.g.: \"Data Source=\\\\\\{0};\"", (object) str));
      if (!flag2)
      {
        if (flag3)
        {
          str = ":memory:";
        }
        else
        {
          bool boolean2 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "ToFullPath", true.ToString()));
          str = SQLiteConnection.ExpandFileName(str, boolean2);
        }
      }
      try
      {
        bool boolean3 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Pooling", this.GetDefaultPooling().ToString()));
        int int32_1 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Max Pool Size", 100.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
        this._defaultTimeout = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Default Timeout", 30.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
        this._busyTimeout = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "BusyTimeout", 0.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
        this._prepareRetries = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "PrepareRetries", 3.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
        this._progressOps = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "ProgressOps", 0.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
        this._defaultIsolation = SQLiteConnection.TryParseEnum(typeof (System.Data.IsolationLevel), SQLiteConnection.FindKey(connectionString, "Default IsolationLevel", System.Data.IsolationLevel.Serializable.ToString()), true) is System.Data.IsolationLevel isolationLevel ? isolationLevel : System.Data.IsolationLevel.Serializable;
        this._defaultIsolation = this.GetEffectiveIsolationLevel(this._defaultIsolation);
        if (this._defaultIsolation != System.Data.IsolationLevel.Serializable && this._defaultIsolation != System.Data.IsolationLevel.ReadCommitted)
          throw new NotSupportedException("Invalid Default IsolationLevel specified");
        this._baseSchemaName = SQLiteConnection.FindKey(connectionString, "BaseSchemaName", "sqlite_default_schema");
        if (this._sql == null)
          this.SetupSQLiteBase(connectionString);
        SQLiteOpenFlagsEnum liteOpenFlagsEnum = SQLiteOpenFlagsEnum.None;
        if (!SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "FailIfMissing", false.ToString())))
          liteOpenFlagsEnum |= SQLiteOpenFlagsEnum.Create;
        SQLiteOpenFlagsEnum openFlags = !SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Read Only", false.ToString())) ? liteOpenFlagsEnum | SQLiteOpenFlagsEnum.ReadWrite : (liteOpenFlagsEnum | SQLiteOpenFlagsEnum.ReadOnly) & ~SQLiteOpenFlagsEnum.Create;
        if (flag2)
          openFlags |= SQLiteOpenFlagsEnum.Uri;
        this._sql.Open(str, this._vfsName, this._flags, openFlags, int32_1, boolean3);
        this._binaryGuid = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "BinaryGUID", true.ToString()));
        string key1 = SQLiteConnection.FindKey(connectionString, "HexPassword", (string) null);
        if (key1 != null)
        {
          string error = (string) null;
          this._sql.SetPassword(SQLiteConnection.FromHexString(key1, ref error) ?? throw new FormatException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Cannot parse 'HexPassword' property value into byte values: {0}", (object) error)));
        }
        else
        {
          string key2 = SQLiteConnection.FindKey(connectionString, "Password", (string) null);
          if (key2 != null)
            this._sql.SetPassword(Encoding.UTF8.GetBytes(key2));
          else if (this._password != null)
            this._sql.SetPassword(this._password);
        }
        this._password = (byte[]) null;
        this._dataSource = flag2 ? str : Path.GetFileNameWithoutExtension(str);
        ++this._version;
        ConnectionState connectionState = this._connectionState;
        this._connectionState = ConnectionState.Open;
        try
        {
          if (SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "SetDefaults", true.ToString())))
          {
            using (SQLiteCommand command = this.CreateCommand())
            {
              if (this._busyTimeout != 0)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA busy_timeout={0}", (object) this._busyTimeout);
                command.ExecuteNonQuery();
              }
              if (!flag2 && !flag3)
              {
                int int32_2 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Page Size", 4096.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
                if (int32_2 != 4096)
                {
                  command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA page_size={0}", (object) int32_2);
                  command.ExecuteNonQuery();
                }
              }
              int int32_3 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Max Page Count", 0.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
              if (int32_3 != 0)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA max_page_count={0}", (object) int32_3);
                command.ExecuteNonQuery();
              }
              bool boolean4 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Legacy Format", false.ToString()));
              if (boolean4)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA legacy_file_format={0}", boolean4 ? (object) "ON" : (object) "OFF");
                command.ExecuteNonQuery();
              }
              string key3 = SQLiteConnection.FindKey(connectionString, "Synchronous", SQLiteSynchronousEnum.Default.ToString());
              if (!(SQLiteConnection.TryParseEnum(typeof (SQLiteSynchronousEnum), key3, true) is SQLiteSynchronousEnum.Default))
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA synchronous={0}", (object) key3);
                command.ExecuteNonQuery();
              }
              int int32_4 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Cache Size", -2000.ToString()), (IFormatProvider) CultureInfo.InvariantCulture);
              if (int32_4 != -2000)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA cache_size={0}", (object) int32_4);
                command.ExecuteNonQuery();
              }
              string key4 = SQLiteConnection.FindKey(connectionString, "Journal Mode", SQLiteJournalModeEnum.Default.ToString());
              if (!(SQLiteConnection.TryParseEnum(typeof (SQLiteJournalModeEnum), key4, true) is SQLiteJournalModeEnum.Default))
              {
                string format = "PRAGMA journal_mode={0}";
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, (object) key4);
                command.ExecuteNonQuery();
              }
              bool boolean5 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Foreign Keys", false.ToString()));
              if (boolean5)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA foreign_keys={0}", boolean5 ? (object) "ON" : (object) "OFF");
                command.ExecuteNonQuery();
              }
              bool boolean6 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Recursive Triggers", false.ToString()));
              if (boolean6)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA recursive_triggers={0}", boolean6 ? (object) "ON" : (object) "OFF");
                command.ExecuteNonQuery();
              }
            }
          }
          if (this._progressHandler != null)
            this._sql.SetProgressHook(this._progressOps, this._progressCallback);
          if (this._authorizerHandler != null)
            this._sql.SetAuthorizerHook(this._authorizerCallback);
          if (this._commitHandler != null)
            this._sql.SetCommitHook(this._commitCallback);
          if (this._updateHandler != null)
            this._sql.SetUpdateHook(this._updateCallback);
          if (this._rollbackHandler != null)
            this._sql.SetRollbackHook(this._rollbackCallback);
          Transaction current = Transaction.Current;
          if (current != (Transaction) null && SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Enlist", true.ToString())))
            this.EnlistTransaction(current);
          this._connectionState = connectionState;
          StateChangeEventArgs eventArgs = (StateChangeEventArgs) null;
          this.OnStateChange(ConnectionState.Open, ref eventArgs);
          SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Opened, eventArgs, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
        }
        catch
        {
          this._connectionState = connectionState;
          throw;
        }
      }
      catch (SQLiteException ex)
      {
        this.Close();
        throw;
      }
    }

    public SQLiteConnection OpenAndReturn()
    {
      this.CheckDisposed();
      this.Open();
      return this;
    }

    public int DefaultTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._defaultTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._defaultTimeout = value;
      }
    }

    public int BusyTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._busyTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._busyTimeout = value;
      }
    }

    public int PrepareRetries
    {
      get
      {
        this.CheckDisposed();
        return this._prepareRetries;
      }
      set
      {
        this.CheckDisposed();
        this._prepareRetries = value;
      }
    }

    public int ProgressOps
    {
      get
      {
        this.CheckDisposed();
        return this._progressOps;
      }
      set
      {
        this.CheckDisposed();
        this._progressOps = value;
      }
    }

    public bool ParseViaFramework
    {
      get
      {
        this.CheckDisposed();
        return this._parseViaFramework;
      }
      set
      {
        this.CheckDisposed();
        this._parseViaFramework = value;
      }
    }

    public SQLiteConnectionFlags Flags
    {
      get
      {
        this.CheckDisposed();
        return this._flags;
      }
      set
      {
        this.CheckDisposed();
        this._flags = value;
      }
    }

    public DbType? DefaultDbType
    {
      get
      {
        this.CheckDisposed();
        return this._defaultDbType;
      }
      set
      {
        this.CheckDisposed();
        this._defaultDbType = value;
      }
    }

    public string DefaultTypeName
    {
      get
      {
        this.CheckDisposed();
        return this._defaultTypeName;
      }
      set
      {
        this.CheckDisposed();
        this._defaultTypeName = value;
      }
    }

    public string VfsName
    {
      get
      {
        this.CheckDisposed();
        return this._vfsName;
      }
      set
      {
        this.CheckDisposed();
        this._vfsName = value;
      }
    }

    public bool OwnHandle
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.OwnHandle : throw new InvalidOperationException("Database connection not valid for checking handle.");
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string ServerVersion
    {
      get
      {
        this.CheckDisposed();
        return SQLiteConnection.SQLiteVersion;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public long LastInsertRowId
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.LastInsertRowId : throw new InvalidOperationException("Database connection not valid for getting last insert rowid.");
      }
    }

    public void Cancel()
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for query cancellation.");
      this._sql.Cancel();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Changes
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.Changes : throw new InvalidOperationException("Database connection not valid for getting number of changes.");
      }
    }

    public bool IsReadOnly(string name)
    {
      this.CheckDisposed();
      return this._sql != null ? this._sql.IsReadOnly(name) : throw new InvalidOperationException("Database connection not valid for checking read-only status.");
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AutoCommit
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.AutoCommit : throw new InvalidOperationException("Database connection not valid for getting autocommit mode.");
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public long MemoryUsed
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.MemoryUsed : throw new InvalidOperationException("Database connection not valid for getting memory used.");
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public long MemoryHighwater
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.MemoryHighwater : throw new InvalidOperationException("Database connection not valid for getting maximum memory used.");
      }
    }

    public static void GetMemoryStatistics(ref IDictionary<string, long> statistics)
    {
      if (statistics == null)
        statistics = (IDictionary<string, long>) new Dictionary<string, long>();
      statistics["MemoryUsed"] = SQLite3.StaticMemoryUsed;
      statistics["MemoryHighwater"] = SQLite3.StaticMemoryHighwater;
    }

    public void ReleaseMemory()
    {
      this.CheckDisposed();
      SQLiteErrorCode errorCode = this._sql != null ? this._sql.ReleaseMemory() : throw new InvalidOperationException("Database connection not valid for releasing memory.");
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this._sql.GetLastError("Could not release connection memory."));
    }

    public static SQLiteErrorCode ReleaseMemory(
      int nBytes,
      bool reset,
      bool compact,
      ref int nFree,
      ref bool resetOk,
      ref uint nLargest)
    {
      return SQLite3.StaticReleaseMemory(nBytes, reset, compact, ref nFree, ref resetOk, ref nLargest);
    }

    public static SQLiteErrorCode SetMemoryStatus(bool value)
    {
      return SQLite3.StaticSetMemoryStatus(value);
    }

    public static string DefineConstants => SQLite3.DefineConstants;

    public static string SQLiteVersion => SQLite3.SQLiteVersion;

    public static string SQLiteSourceId => SQLite3.SQLiteSourceId;

    public static string SQLiteCompileOptions => SQLite3.SQLiteCompileOptions;

    public static string InteropVersion => SQLite3.InteropVersion;

    public static string InteropSourceId => SQLite3.InteropSourceId;

    public static string InteropCompileOptions => SQLite3.InteropCompileOptions;

    public static string ProviderVersion
    {
      get
      {
        return !(SQLiteConnection._assembly != (Assembly) null) ? (string) null : SQLiteConnection._assembly.GetName().Version.ToString();
      }
    }

    public static string ProviderSourceId
    {
      get
      {
        if (SQLiteConnection._assembly == (Assembly) null)
          return (string) null;
        string str1 = (string) null;
        if (SQLiteConnection._assembly.IsDefined(typeof (AssemblySourceIdAttribute), false))
          str1 = ((AssemblySourceIdAttribute) SQLiteConnection._assembly.GetCustomAttributes(typeof (AssemblySourceIdAttribute), false)[0]).SourceId;
        string str2 = (string) null;
        if (SQLiteConnection._assembly.IsDefined(typeof (AssemblySourceTimeStampAttribute), false))
          str2 = ((AssemblySourceTimeStampAttribute) SQLiteConnection._assembly.GetCustomAttributes(typeof (AssemblySourceTimeStampAttribute), false)[0]).SourceTimeStamp;
        if (str1 == null && str2 == null)
          return (string) null;
        if (str1 == null)
          str1 = "0000000000000000000000000000000000000000";
        if (str2 == null)
          str2 = "0000-00-00 00:00:00 UTC";
        return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", (object) str1, (object) str2);
      }
    }

    private static bool TryGetLastCachedSetting(string name, object @default, out object value)
    {
      if (SQLiteConnection._lastConnectionInOpen != null)
        return SQLiteConnection._lastConnectionInOpen.TryGetCachedSetting(name, @default, out value);
      value = @default;
      return false;
    }

    public static SQLiteConnectionFlags DefaultFlags
    {
      get
      {
        string name = "DefaultFlags_SQLiteConnection";
        object settingValue;
        if (!SQLiteConnection.TryGetLastCachedSetting(name, (object) null, out settingValue))
          settingValue = (object) UnsafeNativeMethods.GetSettingValue(name, (string) null);
        return settingValue == null || !(SQLiteConnection.TryParseEnum(typeof (SQLiteConnectionFlags), settingValue.ToString(), true) is SQLiteConnectionFlags liteConnectionFlags) ? SQLiteConnectionFlags.Default : liteConnectionFlags;
      }
    }

    public static SQLiteConnectionFlags SharedFlags
    {
      get
      {
        lock (SQLiteConnection._syncRoot)
          return SQLiteConnection._sharedFlags;
      }
      set
      {
        lock (SQLiteConnection._syncRoot)
          SQLiteConnection._sharedFlags = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override ConnectionState State
    {
      get
      {
        this.CheckDisposed();
        return this._connectionState;
      }
    }

    public SQLiteErrorCode Shutdown()
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for shutdown.");
      this._sql.Close(true);
      return this._sql.Shutdown();
    }

    public static void Shutdown(bool directories, bool noThrow)
    {
      SQLiteErrorCode errorCode = SQLite3.StaticShutdown(directories);
      if (errorCode != SQLiteErrorCode.Ok && !noThrow)
        throw new SQLiteException(errorCode, (string) null);
    }

    public void SetExtendedResultCodes(bool bOnOff)
    {
      this.CheckDisposed();
      if (this._sql == null)
        return;
      this._sql.SetExtendedResultCodes(bOnOff);
    }

    public SQLiteErrorCode ResultCode()
    {
      this.CheckDisposed();
      return this._sql != null ? this._sql.ResultCode() : throw new InvalidOperationException("Database connection not valid for getting result code.");
    }

    public SQLiteErrorCode ExtendedResultCode()
    {
      this.CheckDisposed();
      return this._sql != null ? this._sql.ExtendedResultCode() : throw new InvalidOperationException("Database connection not valid for getting extended result code.");
    }

    public void LogMessage(SQLiteErrorCode iErrCode, string zMessage)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for logging message.");
      this._sql.LogMessage(iErrCode, zMessage);
    }

    public void LogMessage(int iErrCode, string zMessage)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for logging message.");
      this._sql.LogMessage((SQLiteErrorCode) iErrCode, zMessage);
    }

    public void ChangePassword(string newPassword)
    {
      this.CheckDisposed();
      this.ChangePassword(string.IsNullOrEmpty(newPassword) ? (byte[]) null : Encoding.UTF8.GetBytes(newPassword));
    }

    public void ChangePassword(byte[] newPassword)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Database must be opened before changing the password.");
      this._sql.ChangePassword(newPassword);
    }

    public void SetPassword(string databasePassword)
    {
      this.CheckDisposed();
      this.SetPassword(string.IsNullOrEmpty(databasePassword) ? (byte[]) null : Encoding.UTF8.GetBytes(databasePassword));
    }

    public void SetPassword(byte[] databasePassword)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Closed)
        throw new InvalidOperationException("Password can only be set before the database is opened.");
      if (databasePassword != null && databasePassword.Length == 0)
        databasePassword = (byte[]) null;
      this._password = databasePassword;
    }

    public SQLiteErrorCode SetAvRetry(ref int count, ref int interval)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Database must be opened before changing the AV retry parameters.");
      IntPtr num = IntPtr.Zero;
      SQLiteErrorCode sqLiteErrorCode;
      try
      {
        num = Marshal.AllocHGlobal(8);
        Marshal.WriteInt32(num, 0, count);
        Marshal.WriteInt32(num, 4, interval);
        sqLiteErrorCode = this._sql.FileControl((string) null, 9, num);
        if (sqLiteErrorCode == SQLiteErrorCode.Ok)
        {
          count = Marshal.ReadInt32(num, 0);
          interval = Marshal.ReadInt32(num, 4);
        }
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
      return sqLiteErrorCode;
    }

    public SQLiteErrorCode SetChunkSize(int size)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Database must be opened before changing the chunk size.");
      IntPtr num = IntPtr.Zero;
      try
      {
        num = Marshal.AllocHGlobal(4);
        Marshal.WriteInt32(num, 0, size);
        return this._sql.FileControl((string) null, 6, num);
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    private static string UnwrapString(string value)
    {
      if (string.IsNullOrEmpty(value))
        return value;
      int length = value.Length;
      return value[0] == '\'' && value[length - 1] == '\'' || value[0] == '"' && value[length - 1] == '"' ? value.Substring(1, length - 2) : value;
    }

    private static string GetDataDirectory()
    {
      string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
      if (string.IsNullOrEmpty(dataDirectory))
        dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
      return dataDirectory;
    }

    private static string ExpandFileName(string sourceFile, bool toFullPath)
    {
      if (string.IsNullOrEmpty(sourceFile))
        return sourceFile;
      if (sourceFile.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
      {
        string dataDirectory = SQLiteConnection.GetDataDirectory();
        if (sourceFile.Length > "|DataDirectory|".Length && ((int) sourceFile["|DataDirectory|".Length] == (int) Path.DirectorySeparatorChar || (int) sourceFile["|DataDirectory|".Length] == (int) Path.AltDirectorySeparatorChar))
          sourceFile = sourceFile.Remove("|DataDirectory|".Length, 1);
        sourceFile = Path.Combine(dataDirectory, sourceFile.Substring("|DataDirectory|".Length));
      }
      if (toFullPath)
        sourceFile = Path.GetFullPath(sourceFile);
      return sourceFile;
    }

    public override DataTable GetSchema()
    {
      this.CheckDisposed();
      return this.GetSchema("MetaDataCollections", (string[]) null);
    }

    public override DataTable GetSchema(string collectionName)
    {
      this.CheckDisposed();
      return this.GetSchema(collectionName, new string[0]);
    }

    public override DataTable GetSchema(string collectionName, string[] restrictionValues)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException();
      string[] strArray = new string[5];
      if (restrictionValues == null)
        restrictionValues = new string[0];
      restrictionValues.CopyTo((Array) strArray, 0);
      switch (collectionName.ToUpper(CultureInfo.InvariantCulture))
      {
        case "METADATACOLLECTIONS":
          return SQLiteConnection.Schema_MetaDataCollections();
        case "DATASOURCEINFORMATION":
          return this.Schema_DataSourceInformation();
        case "DATATYPES":
          return this.Schema_DataTypes();
        case "COLUMNS":
        case "TABLECOLUMNS":
          return this.Schema_Columns(strArray[0], strArray[2], strArray[3]);
        case "INDEXES":
          return this.Schema_Indexes(strArray[0], strArray[2], strArray[3]);
        case "TRIGGERS":
          return this.Schema_Triggers(strArray[0], strArray[2], strArray[3]);
        case "INDEXCOLUMNS":
          return this.Schema_IndexColumns(strArray[0], strArray[2], strArray[3], strArray[4]);
        case "TABLES":
          return this.Schema_Tables(strArray[0], strArray[2], strArray[3]);
        case "VIEWS":
          return this.Schema_Views(strArray[0], strArray[2]);
        case "VIEWCOLUMNS":
          return this.Schema_ViewColumns(strArray[0], strArray[2], strArray[3]);
        case "FOREIGNKEYS":
          return this.Schema_ForeignKeys(strArray[0], strArray[2], strArray[3]);
        case "CATALOGS":
          return this.Schema_Catalogs(strArray[0]);
        case "RESERVEDWORDS":
          return SQLiteConnection.Schema_ReservedWords();
        default:
          throw new NotSupportedException();
      }
    }

    private static DataTable Schema_ReservedWords()
    {
      DataTable dataTable = new DataTable("ReservedWords");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("ReservedWord", typeof (string));
      dataTable.Columns.Add("MaximumVersion", typeof (string));
      dataTable.Columns.Add("MinimumVersion", typeof (string));
      dataTable.BeginLoadData();
      string keywords = SR.Keywords;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in keywords.Split(chArray))
      {
        DataRow row = dataTable.NewRow();
        row[0] = (object) str;
        dataTable.Rows.Add(row);
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private static DataTable Schema_MetaDataCollections()
    {
      DataTable dataTable = new DataTable("MetaDataCollections");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CollectionName", typeof (string));
      dataTable.Columns.Add("NumberOfRestrictions", typeof (int));
      dataTable.Columns.Add("NumberOfIdentifierParts", typeof (int));
      dataTable.BeginLoadData();
      StringReader reader = new StringReader(SR.MetaDataCollections);
      int num = (int) dataTable.ReadXml((TextReader) reader);
      reader.Close();
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_DataSourceInformation()
    {
      DataTable dataTable = new DataTable("DataSourceInformation");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductName, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductVersion, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductVersionNormalized, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.GroupByBehavior, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.IdentifierPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.IdentifierCase, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.OrderByColumnsInSelect, typeof (bool));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterMarkerFormat, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterMarkerPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterNameMaxLength, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterNamePattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.QuotedIdentifierPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.QuotedIdentifierCase, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.StatementSeparatorPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.StringLiteralPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.SupportedJoinOperators, typeof (int));
      dataTable.BeginLoadData();
      DataRow row = dataTable.NewRow();
      row.ItemArray = new object[17]
      {
        null,
        (object) "SQLite",
        (object) this._sql.Version,
        (object) this._sql.Version,
        (object) 3,
        (object) "(^\\[\\p{Lo}\\p{Lu}\\p{Ll}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Nd}@$#_]*$)|(^\\[[^\\]\\0]|\\]\\]+\\]$)|(^\\\"[^\\\"\\0]|\\\"\\\"+\\\"$)",
        (object) 1,
        (object) false,
        (object) "{0}",
        (object) "@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
        (object) (int) byte.MaxValue,
        (object) "^[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
        (object) "(([^\\[]|\\]\\])*)",
        (object) 1,
        (object) ";",
        (object) "'(([^']|'')*)'",
        (object) 15
      };
      dataTable.Rows.Add(row);
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Columns(string strCatalog, string strTable, string strColumn)
    {
      DataTable dataTable = new DataTable("Columns");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_GUID", typeof (Guid));
      dataTable.Columns.Add("COLUMN_PROPID", typeof (long));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("COLUMN_HASDEFAULT", typeof (bool));
      dataTable.Columns.Add("COLUMN_DEFAULT", typeof (string));
      dataTable.Columns.Add("COLUMN_FLAGS", typeof (long));
      dataTable.Columns.Add("IS_NULLABLE", typeof (bool));
      dataTable.Columns.Add("DATA_TYPE", typeof (string));
      dataTable.Columns.Add("TYPE_GUID", typeof (Guid));
      dataTable.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof (int));
      dataTable.Columns.Add("CHARACTER_OCTET_LENGTH", typeof (int));
      dataTable.Columns.Add("NUMERIC_PRECISION", typeof (int));
      dataTable.Columns.Add("NUMERIC_SCALE", typeof (int));
      dataTable.Columns.Add("DATETIME_PRECISION", typeof (long));
      dataTable.Columns.Add("CHARACTER_SET_CATALOG", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_SCHEMA", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_NAME", typeof (string));
      dataTable.Columns.Add("COLLATION_CATALOG", typeof (string));
      dataTable.Columns.Add("COLLATION_SCHEMA", typeof (string));
      dataTable.Columns.Add("COLLATION_NAME", typeof (string));
      dataTable.Columns.Add("DOMAIN_CATALOG", typeof (string));
      dataTable.Columns.Add("DOMAIN_NAME", typeof (string));
      dataTable.Columns.Add("DESCRIPTION", typeof (string));
      dataTable.Columns.Add("PRIMARY_KEY", typeof (bool));
      dataTable.Columns.Add("EDM_TYPE", typeof (string));
      dataTable.Columns.Add("AUTOINCREMENT", typeof (bool));
      dataTable.Columns.Add("UNIQUE", typeof (bool));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table' OR [type] LIKE 'view'", (object) strCatalog, (object) str), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(strTable, sqLiteDataReader1.GetString(2), StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}]", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                  using (DataTable schemaTable = sqLiteDataReader2.GetSchemaTable(true, true))
                  {
                    foreach (DataRow row1 in (InternalDataCollectionBase) schemaTable.Rows)
                    {
                      if (string.Compare(row1[SchemaTableColumn.ColumnName].ToString(), strColumn, StringComparison.OrdinalIgnoreCase) == 0 || strColumn == null)
                      {
                        DataRow row2 = dataTable.NewRow();
                        row2["NUMERIC_PRECISION"] = row1[SchemaTableColumn.NumericPrecision];
                        row2["NUMERIC_SCALE"] = row1[SchemaTableColumn.NumericScale];
                        row2["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
                        row2["COLUMN_NAME"] = row1[SchemaTableColumn.ColumnName];
                        row2["TABLE_CATALOG"] = (object) strCatalog;
                        row2["ORDINAL_POSITION"] = row1[SchemaTableColumn.ColumnOrdinal];
                        row2["COLUMN_HASDEFAULT"] = (object) (row1[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value);
                        row2["COLUMN_DEFAULT"] = row1[SchemaTableOptionalColumn.DefaultValue];
                        row2["IS_NULLABLE"] = row1[SchemaTableColumn.AllowDBNull];
                        row2["DATA_TYPE"] = (object) row1["DataTypeName"].ToString().ToLower(CultureInfo.InvariantCulture);
                        row2["EDM_TYPE"] = (object) SQLiteConvert.DbTypeToTypeName(this, (DbType) row1[SchemaTableColumn.ProviderType], this._flags).ToString().ToLower(CultureInfo.InvariantCulture);
                        row2["CHARACTER_MAXIMUM_LENGTH"] = row1[SchemaTableColumn.ColumnSize];
                        row2["TABLE_SCHEMA"] = row1[SchemaTableColumn.BaseSchemaName];
                        row2["PRIMARY_KEY"] = row1[SchemaTableColumn.IsKey];
                        row2["AUTOINCREMENT"] = row1[SchemaTableOptionalColumn.IsAutoIncrement];
                        row2["COLLATION_NAME"] = row1["CollationType"];
                        row2["UNIQUE"] = row1[SchemaTableColumn.IsUnique];
                        dataTable.Rows.Add(row2);
                      }
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Indexes(string strCatalog, string strTable, string strIndex)
    {
      DataTable dataTable = new DataTable("Indexes");
      List<int> intList = new List<int>();
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("INDEX_CATALOG", typeof (string));
      dataTable.Columns.Add("INDEX_SCHEMA", typeof (string));
      dataTable.Columns.Add("INDEX_NAME", typeof (string));
      dataTable.Columns.Add("PRIMARY_KEY", typeof (bool));
      dataTable.Columns.Add("UNIQUE", typeof (bool));
      dataTable.Columns.Add("CLUSTERED", typeof (bool));
      dataTable.Columns.Add("TYPE", typeof (int));
      dataTable.Columns.Add("FILL_FACTOR", typeof (int));
      dataTable.Columns.Add("INITIAL_SIZE", typeof (int));
      dataTable.Columns.Add("NULLS", typeof (int));
      dataTable.Columns.Add("SORT_BOOKMARKS", typeof (bool));
      dataTable.Columns.Add("AUTO_UPDATE", typeof (bool));
      dataTable.Columns.Add("NULL_COLLATION", typeof (int));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_GUID", typeof (Guid));
      dataTable.Columns.Add("COLUMN_PROPID", typeof (long));
      dataTable.Columns.Add("COLLATION", typeof (short));
      dataTable.Columns.Add("CARDINALITY", typeof (Decimal));
      dataTable.Columns.Add("PAGES", typeof (int));
      dataTable.Columns.Add("FILTER_CONDITION", typeof (string));
      dataTable.Columns.Add("INTEGRATED", typeof (bool));
      dataTable.Columns.Add("INDEX_DEFINITION", typeof (string));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) str), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            bool flag = false;
            intList.Clear();
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(sqLiteDataReader1.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].table_info([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                {
                  while (sqLiteDataReader2.Read())
                  {
                    if (sqLiteDataReader2.GetInt32(5) != 0)
                    {
                      intList.Add(sqLiteDataReader2.GetInt32(0));
                      if (string.Compare(sqLiteDataReader2.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0)
                        flag = true;
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
            if (intList.Count == 1)
            {
              if (flag)
              {
                DataRow row = dataTable.NewRow();
                row["TABLE_CATALOG"] = (object) strCatalog;
                row["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
                row["INDEX_CATALOG"] = (object) strCatalog;
                row["PRIMARY_KEY"] = (object) true;
                row["INDEX_NAME"] = (object) HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{1}_PK_{0}", (object) sqLiteDataReader1.GetString(2), (object) str);
                row["UNIQUE"] = (object) true;
                if (string.Compare((string) row["INDEX_NAME"], strIndex, StringComparison.OrdinalIgnoreCase) == 0 || strIndex == null)
                  dataTable.Rows.Add(row);
                intList.Clear();
              }
            }
            try
            {
              using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].index_list([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader())
                {
                  while (sqLiteDataReader3.Read())
                  {
                    if (string.Compare(sqLiteDataReader3.GetString(1), strIndex, StringComparison.OrdinalIgnoreCase) == 0 || strIndex == null)
                    {
                      DataRow row = dataTable.NewRow();
                      row["TABLE_CATALOG"] = (object) strCatalog;
                      row["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
                      row["INDEX_CATALOG"] = (object) strCatalog;
                      row["INDEX_NAME"] = (object) sqLiteDataReader3.GetString(1);
                      row["UNIQUE"] = (object) SQLiteConvert.ToBoolean(sqLiteDataReader3.GetValue(2), (IFormatProvider) CultureInfo.InvariantCulture, false);
                      row["PRIMARY_KEY"] = (object) false;
                      using (SQLiteCommand sqLiteCommand4 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{2}] WHERE [type] LIKE 'index' AND [name] LIKE '{1}'", (object) strCatalog, (object) sqLiteDataReader3.GetString(1).Replace("'", "''"), (object) str), this))
                      {
                        using (SQLiteDataReader sqLiteDataReader4 = sqLiteCommand4.ExecuteReader())
                        {
                          if (sqLiteDataReader4.Read())
                          {
                            if (!sqLiteDataReader4.IsDBNull(4))
                              row["INDEX_DEFINITION"] = (object) sqLiteDataReader4.GetString(4);
                          }
                        }
                      }
                      if (intList.Count > 0 && sqLiteDataReader3.GetString(1).StartsWith("sqlite_autoindex_" + sqLiteDataReader1.GetString(2), StringComparison.InvariantCultureIgnoreCase))
                      {
                        using (SQLiteCommand sqLiteCommand5 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].index_info([{1}])", (object) strCatalog, (object) sqLiteDataReader3.GetString(1)), this))
                        {
                          using (SQLiteDataReader sqLiteDataReader5 = sqLiteCommand5.ExecuteReader())
                          {
                            int num = 0;
                            while (sqLiteDataReader5.Read())
                            {
                              if (!intList.Contains(sqLiteDataReader5.GetInt32(1)))
                              {
                                num = 0;
                                break;
                              }
                              ++num;
                            }
                            if (num == intList.Count)
                            {
                              row["PRIMARY_KEY"] = (object) true;
                              intList.Clear();
                            }
                          }
                        }
                      }
                      dataTable.Rows.Add(row);
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Triggers(string catalog, string table, string triggerName)
    {
      DataTable dataTable = new DataTable("Triggers");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("TRIGGER_NAME", typeof (string));
      dataTable.Columns.Add("TRIGGER_DEFINITION", typeof (string));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(table))
        table = (string) null;
      if (string.IsNullOrEmpty(catalog))
        catalog = "main";
      string str = string.Compare(catalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{0}].[{1}] WHERE [type] LIKE 'trigger'", (object) catalog, (object) str), this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            if ((string.Compare(sqLiteDataReader.GetString(1), triggerName, StringComparison.OrdinalIgnoreCase) == 0 || triggerName == null) && (table == null || string.Compare(table, sqLiteDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) == 0))
            {
              DataRow row = dataTable.NewRow();
              row["TABLE_CATALOG"] = (object) catalog;
              row["TABLE_NAME"] = (object) sqLiteDataReader.GetString(2);
              row["TRIGGER_NAME"] = (object) sqLiteDataReader.GetString(1);
              row["TRIGGER_DEFINITION"] = (object) sqLiteDataReader.GetString(4);
              dataTable.Rows.Add(row);
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Tables(string strCatalog, string strTable, string strType)
    {
      DataTable dataTable = new DataTable("Tables");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_TYPE", typeof (string));
      dataTable.Columns.Add("TABLE_ID", typeof (long));
      dataTable.Columns.Add("TABLE_ROOTPAGE", typeof (int));
      dataTable.Columns.Add("TABLE_DEFINITION", typeof (string));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) str), this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            string strB = sqLiteDataReader.GetString(0);
            if (string.Compare(sqLiteDataReader.GetString(2), 0, "SQLITE_", 0, 7, StringComparison.OrdinalIgnoreCase) == 0)
              strB = "SYSTEM_TABLE";
            if ((string.Compare(strType, strB, StringComparison.OrdinalIgnoreCase) == 0 || strType == null) && (string.Compare(sqLiteDataReader.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) == 0 || strTable == null))
            {
              DataRow row = dataTable.NewRow();
              row["TABLE_CATALOG"] = (object) strCatalog;
              row["TABLE_NAME"] = (object) sqLiteDataReader.GetString(2);
              row["TABLE_TYPE"] = (object) strB;
              row["TABLE_ID"] = (object) sqLiteDataReader.GetInt64(5);
              row["TABLE_ROOTPAGE"] = (object) sqLiteDataReader.GetInt32(3);
              row["TABLE_DEFINITION"] = (object) sqLiteDataReader.GetString(4);
              dataTable.Rows.Add(row);
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Views(string strCatalog, string strView)
    {
      DataTable dataTable = new DataTable("Views");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("VIEW_DEFINITION", typeof (string));
      dataTable.Columns.Add("CHECK_OPTION", typeof (bool));
      dataTable.Columns.Add("IS_UPDATABLE", typeof (bool));
      dataTable.Columns.Add("DESCRIPTION", typeof (string));
      dataTable.Columns.Add("DATE_CREATED", typeof (DateTime));
      dataTable.Columns.Add("DATE_MODIFIED", typeof (DateTime));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str1 = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'view'", (object) strCatalog, (object) str1), this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            if (string.Compare(sqLiteDataReader.GetString(1), strView, StringComparison.OrdinalIgnoreCase) == 0 || string.IsNullOrEmpty(strView))
            {
              string source = sqLiteDataReader.GetString(4).Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
              int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, " AS ", CompareOptions.IgnoreCase);
              if (num > -1)
              {
                string str2 = source.Substring(num + 4).Trim();
                DataRow row = dataTable.NewRow();
                row["TABLE_CATALOG"] = (object) strCatalog;
                row["TABLE_NAME"] = (object) sqLiteDataReader.GetString(2);
                row["IS_UPDATABLE"] = (object) false;
                row["VIEW_DEFINITION"] = (object) str2;
                dataTable.Rows.Add(row);
              }
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Catalogs(string strCatalog)
    {
      DataTable dataTable = new DataTable("Catalogs");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CATALOG_NAME", typeof (string));
      dataTable.Columns.Add("DESCRIPTION", typeof (string));
      dataTable.Columns.Add("ID", typeof (long));
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand("PRAGMA database_list", this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            if (string.Compare(sqLiteDataReader.GetString(1), strCatalog, StringComparison.OrdinalIgnoreCase) == 0 || strCatalog == null)
            {
              DataRow row = dataTable.NewRow();
              row["CATALOG_NAME"] = (object) sqLiteDataReader.GetString(1);
              row["DESCRIPTION"] = (object) sqLiteDataReader.GetString(2);
              row["ID"] = (object) sqLiteDataReader.GetInt64(0);
              dataTable.Rows.Add(row);
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_DataTypes()
    {
      DataTable dataTable = new DataTable("DataTypes");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TypeName", typeof (string));
      dataTable.Columns.Add("ProviderDbType", typeof (int));
      dataTable.Columns.Add("ColumnSize", typeof (long));
      dataTable.Columns.Add("CreateFormat", typeof (string));
      dataTable.Columns.Add("CreateParameters", typeof (string));
      dataTable.Columns.Add("DataType", typeof (string));
      dataTable.Columns.Add("IsAutoIncrementable", typeof (bool));
      dataTable.Columns.Add("IsBestMatch", typeof (bool));
      dataTable.Columns.Add("IsCaseSensitive", typeof (bool));
      dataTable.Columns.Add("IsFixedLength", typeof (bool));
      dataTable.Columns.Add("IsFixedPrecisionScale", typeof (bool));
      dataTable.Columns.Add("IsLong", typeof (bool));
      dataTable.Columns.Add("IsNullable", typeof (bool));
      dataTable.Columns.Add("IsSearchable", typeof (bool));
      dataTable.Columns.Add("IsSearchableWithLike", typeof (bool));
      dataTable.Columns.Add("IsLiteralSupported", typeof (bool));
      dataTable.Columns.Add("LiteralPrefix", typeof (string));
      dataTable.Columns.Add("LiteralSuffix", typeof (string));
      dataTable.Columns.Add("IsUnsigned", typeof (bool));
      dataTable.Columns.Add("MaximumScale", typeof (short));
      dataTable.Columns.Add("MinimumScale", typeof (short));
      dataTable.Columns.Add("IsConcurrencyType", typeof (bool));
      dataTable.BeginLoadData();
      StringReader reader = new StringReader(SR.DataTypes);
      int num = (int) dataTable.ReadXml((TextReader) reader);
      reader.Close();
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_IndexColumns(
      string strCatalog,
      string strTable,
      string strIndex,
      string strColumn)
    {
      DataTable dataTable = new DataTable("IndexColumns");
      List<KeyValuePair<int, string>> keyValuePairList = new List<KeyValuePair<int, string>>();
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CONSTRAINT_CATALOG", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_SCHEMA", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("INDEX_NAME", typeof (string));
      dataTable.Columns.Add("COLLATION_NAME", typeof (string));
      dataTable.Columns.Add("SORT_MODE", typeof (string));
      dataTable.Columns.Add("CONFLICT_OPTION", typeof (int));
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str1 = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) str1), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            bool flag = false;
            keyValuePairList.Clear();
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(sqLiteDataReader1.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].table_info([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                {
                  while (sqLiteDataReader2.Read())
                  {
                    if (sqLiteDataReader2.GetInt32(5) == 1)
                    {
                      keyValuePairList.Add(new KeyValuePair<int, string>(sqLiteDataReader2.GetInt32(0), sqLiteDataReader2.GetString(1)));
                      if (string.Compare(sqLiteDataReader2.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0)
                        flag = true;
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
            if (keyValuePairList.Count == 1 && flag)
            {
              DataRow row = dataTable.NewRow();
              row["CONSTRAINT_CATALOG"] = (object) strCatalog;
              row["CONSTRAINT_NAME"] = (object) HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{1}_PK_{0}", (object) sqLiteDataReader1.GetString(2), (object) str1);
              row["TABLE_CATALOG"] = (object) strCatalog;
              row["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
              row["COLUMN_NAME"] = (object) keyValuePairList[0].Value;
              row["INDEX_NAME"] = row["CONSTRAINT_NAME"];
              row["ORDINAL_POSITION"] = (object) 0;
              row["COLLATION_NAME"] = (object) "BINARY";
              row["SORT_MODE"] = (object) "ASC";
              row["CONFLICT_OPTION"] = (object) 2;
              if (string.IsNullOrEmpty(strIndex) || string.Compare(strIndex, (string) row["INDEX_NAME"], StringComparison.OrdinalIgnoreCase) == 0)
                dataTable.Rows.Add(row);
            }
            using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{2}] WHERE [type] LIKE 'index' AND [tbl_name] LIKE '{1}'", (object) strCatalog, (object) sqLiteDataReader1.GetString(2).Replace("'", "''"), (object) str1), this))
            {
              using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader())
              {
                while (sqLiteDataReader3.Read())
                {
                  int num = 0;
                  if (!string.IsNullOrEmpty(strIndex))
                  {
                    if (string.Compare(strIndex, sqLiteDataReader3.GetString(1), StringComparison.OrdinalIgnoreCase) != 0)
                      continue;
                  }
                  try
                  {
                    using (SQLiteCommand sqLiteCommand4 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].index_info([{1}])", (object) strCatalog, (object) sqLiteDataReader3.GetString(1)), this))
                    {
                      using (SQLiteDataReader sqLiteDataReader4 = sqLiteCommand4.ExecuteReader())
                      {
                        while (sqLiteDataReader4.Read())
                        {
                          string str2 = sqLiteDataReader4.IsDBNull(2) ? (string) null : sqLiteDataReader4.GetString(2);
                          DataRow row = dataTable.NewRow();
                          row["CONSTRAINT_CATALOG"] = (object) strCatalog;
                          row["CONSTRAINT_NAME"] = (object) sqLiteDataReader3.GetString(1);
                          row["TABLE_CATALOG"] = (object) strCatalog;
                          row["TABLE_NAME"] = (object) sqLiteDataReader3.GetString(2);
                          row["COLUMN_NAME"] = (object) str2;
                          row["INDEX_NAME"] = (object) sqLiteDataReader3.GetString(1);
                          row["ORDINAL_POSITION"] = (object) num;
                          string collationSequence = (string) null;
                          int sortMode = 0;
                          int onError = 0;
                          if (str2 != null)
                            this._sql.GetIndexColumnExtendedInfo(strCatalog, sqLiteDataReader3.GetString(1), str2, ref sortMode, ref onError, ref collationSequence);
                          if (!string.IsNullOrEmpty(collationSequence))
                            row["COLLATION_NAME"] = (object) collationSequence;
                          row["SORT_MODE"] = sortMode == 0 ? (object) "ASC" : (object) "DESC";
                          row["CONFLICT_OPTION"] = (object) onError;
                          ++num;
                          if (strColumn == null || string.Compare(strColumn, str2, StringComparison.OrdinalIgnoreCase) == 0)
                            dataTable.Rows.Add(row);
                        }
                      }
                    }
                  }
                  catch (SQLiteException ex)
                  {
                  }
                }
              }
            }
          }
        }
      }
      dataTable.EndLoadData();
      dataTable.AcceptChanges();
      return dataTable;
    }

    private DataTable Schema_ViewColumns(string strCatalog, string strView, string strColumn)
    {
      DataTable dataTable = new DataTable("ViewColumns");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("VIEW_CATALOG", typeof (string));
      dataTable.Columns.Add("VIEW_SCHEMA", typeof (string));
      dataTable.Columns.Add("VIEW_NAME", typeof (string));
      dataTable.Columns.Add("VIEW_COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("COLUMN_HASDEFAULT", typeof (bool));
      dataTable.Columns.Add("COLUMN_DEFAULT", typeof (string));
      dataTable.Columns.Add("COLUMN_FLAGS", typeof (long));
      dataTable.Columns.Add("IS_NULLABLE", typeof (bool));
      dataTable.Columns.Add("DATA_TYPE", typeof (string));
      dataTable.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof (int));
      dataTable.Columns.Add("NUMERIC_PRECISION", typeof (int));
      dataTable.Columns.Add("NUMERIC_SCALE", typeof (int));
      dataTable.Columns.Add("DATETIME_PRECISION", typeof (long));
      dataTable.Columns.Add("CHARACTER_SET_CATALOG", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_SCHEMA", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_NAME", typeof (string));
      dataTable.Columns.Add("COLLATION_CATALOG", typeof (string));
      dataTable.Columns.Add("COLLATION_SCHEMA", typeof (string));
      dataTable.Columns.Add("COLLATION_NAME", typeof (string));
      dataTable.Columns.Add("PRIMARY_KEY", typeof (bool));
      dataTable.Columns.Add("EDM_TYPE", typeof (string));
      dataTable.Columns.Add("AUTOINCREMENT", typeof (bool));
      dataTable.Columns.Add("UNIQUE", typeof (bool));
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'view'", (object) strCatalog, (object) str), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            if (string.IsNullOrEmpty(strView) || string.Compare(strView, sqLiteDataReader1.GetString(2), StringComparison.OrdinalIgnoreCase) == 0)
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}]", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                string source = sqLiteDataReader1.GetString(4).Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
                int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, " AS ", CompareOptions.IgnoreCase);
                if (num >= 0)
                {
                  using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(source.Substring(num + 4), this))
                  {
                    using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                      using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader(CommandBehavior.SchemaOnly))
                      {
                        using (DataTable schemaTable1 = sqLiteDataReader2.GetSchemaTable(false, false))
                        {
                          using (DataTable schemaTable2 = sqLiteDataReader3.GetSchemaTable(false, false))
                          {
                            for (int index = 0; index < schemaTable2.Rows.Count; ++index)
                            {
                              DataRow row1 = schemaTable1.Rows[index];
                              DataRow row2 = schemaTable2.Rows[index];
                              if (string.Compare(row1[SchemaTableColumn.ColumnName].ToString(), strColumn, StringComparison.OrdinalIgnoreCase) == 0 || strColumn == null)
                              {
                                DataRow row3 = dataTable.NewRow();
                                row3["VIEW_CATALOG"] = (object) strCatalog;
                                row3["VIEW_NAME"] = (object) sqLiteDataReader1.GetString(2);
                                row3["TABLE_CATALOG"] = (object) strCatalog;
                                row3["TABLE_SCHEMA"] = row2[SchemaTableColumn.BaseSchemaName];
                                row3["TABLE_NAME"] = row2[SchemaTableColumn.BaseTableName];
                                row3["COLUMN_NAME"] = row2[SchemaTableColumn.BaseColumnName];
                                row3["VIEW_COLUMN_NAME"] = row1[SchemaTableColumn.ColumnName];
                                row3["COLUMN_HASDEFAULT"] = (object) (row1[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value);
                                row3["COLUMN_DEFAULT"] = row1[SchemaTableOptionalColumn.DefaultValue];
                                row3["ORDINAL_POSITION"] = row1[SchemaTableColumn.ColumnOrdinal];
                                row3["IS_NULLABLE"] = row1[SchemaTableColumn.AllowDBNull];
                                row3["DATA_TYPE"] = row1["DataTypeName"];
                                row3["EDM_TYPE"] = (object) SQLiteConvert.DbTypeToTypeName(this, (DbType) row1[SchemaTableColumn.ProviderType], this._flags).ToString().ToLower(CultureInfo.InvariantCulture);
                                row3["CHARACTER_MAXIMUM_LENGTH"] = row1[SchemaTableColumn.ColumnSize];
                                row3["TABLE_SCHEMA"] = row1[SchemaTableColumn.BaseSchemaName];
                                row3["PRIMARY_KEY"] = row1[SchemaTableColumn.IsKey];
                                row3["AUTOINCREMENT"] = row1[SchemaTableOptionalColumn.IsAutoIncrement];
                                row3["COLLATION_NAME"] = row1["CollationType"];
                                row3["UNIQUE"] = row1[SchemaTableColumn.IsUnique];
                                dataTable.Rows.Add(row3);
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      dataTable.EndLoadData();
      dataTable.AcceptChanges();
      return dataTable;
    }

    private DataTable Schema_ForeignKeys(string strCatalog, string strTable, string strKeyName)
    {
      DataTable dataTable = new DataTable("ForeignKeys");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CONSTRAINT_CATALOG", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_SCHEMA", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_TYPE", typeof (string));
      dataTable.Columns.Add("IS_DEFERRABLE", typeof (bool));
      dataTable.Columns.Add("INITIALLY_DEFERRED", typeof (bool));
      dataTable.Columns.Add("FKEY_ID", typeof (int));
      dataTable.Columns.Add("FKEY_FROM_COLUMN", typeof (string));
      dataTable.Columns.Add("FKEY_FROM_ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("FKEY_TO_CATALOG", typeof (string));
      dataTable.Columns.Add("FKEY_TO_SCHEMA", typeof (string));
      dataTable.Columns.Add("FKEY_TO_TABLE", typeof (string));
      dataTable.Columns.Add("FKEY_TO_COLUMN", typeof (string));
      dataTable.Columns.Add("FKEY_ON_UPDATE", typeof (string));
      dataTable.Columns.Add("FKEY_ON_DELETE", typeof (string));
      dataTable.Columns.Add("FKEY_MATCH", typeof (string));
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = "main";
      string str = string.Compare(strCatalog, "temp", StringComparison.OrdinalIgnoreCase) == 0 ? "sqlite_temp_master" : "sqlite_master";
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) str), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(strTable, sqLiteDataReader1.GetString(2), StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommandBuilder liteCommandBuilder = new SQLiteCommandBuilder())
              {
                using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].foreign_key_list([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
                {
                  using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                  {
                    while (sqLiteDataReader2.Read())
                    {
                      DataRow row = dataTable.NewRow();
                      row["CONSTRAINT_CATALOG"] = (object) strCatalog;
                      row["CONSTRAINT_NAME"] = (object) HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "FK_{0}_{1}_{2}", sqLiteDataReader1[2], (object) sqLiteDataReader2.GetInt32(0), (object) sqLiteDataReader2.GetInt32(1));
                      row["TABLE_CATALOG"] = (object) strCatalog;
                      row["TABLE_NAME"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader1.GetString(2));
                      row["CONSTRAINT_TYPE"] = (object) "FOREIGN KEY";
                      row["IS_DEFERRABLE"] = (object) false;
                      row["INITIALLY_DEFERRED"] = (object) false;
                      row["FKEY_ID"] = sqLiteDataReader2[0];
                      row["FKEY_FROM_COLUMN"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader2[3].ToString());
                      row["FKEY_TO_CATALOG"] = (object) strCatalog;
                      row["FKEY_TO_TABLE"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader2[2].ToString());
                      row["FKEY_TO_COLUMN"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader2[4].ToString());
                      row["FKEY_FROM_ORDINAL_POSITION"] = sqLiteDataReader2[1];
                      row["FKEY_ON_UPDATE"] = sqLiteDataReader2.FieldCount > 5 ? sqLiteDataReader2[5] : (object) string.Empty;
                      row["FKEY_ON_DELETE"] = sqLiteDataReader2.FieldCount > 6 ? sqLiteDataReader2[6] : (object) string.Empty;
                      row["FKEY_MATCH"] = sqLiteDataReader2.FieldCount > 7 ? sqLiteDataReader2[7] : (object) string.Empty;
                      if (string.IsNullOrEmpty(strKeyName) || string.Compare(strKeyName, row["CONSTRAINT_NAME"].ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                        dataTable.Rows.Add(row);
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
          }
        }
      }
      dataTable.EndLoadData();
      dataTable.AcceptChanges();
      return dataTable;
    }

    public event SQLiteProgressEventHandler Progress
    {
      add
      {
        this.CheckDisposed();
        if (this._progressHandler == null)
        {
          this._progressCallback = new SQLiteProgressCallback(this.ProgressCallback);
          if (this._sql != null)
            this._sql.SetProgressHook(this._progressOps, this._progressCallback);
        }
        this._progressHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._progressHandler -= value;
        if (this._progressHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetProgressHook(0, (SQLiteProgressCallback) null);
        this._progressCallback = (SQLiteProgressCallback) null;
      }
    }

    public event SQLiteAuthorizerEventHandler Authorize
    {
      add
      {
        this.CheckDisposed();
        if (this._authorizerHandler == null)
        {
          this._authorizerCallback = new SQLiteAuthorizerCallback(this.AuthorizerCallback);
          if (this._sql != null)
            this._sql.SetAuthorizerHook(this._authorizerCallback);
        }
        this._authorizerHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._authorizerHandler -= value;
        if (this._authorizerHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetAuthorizerHook((SQLiteAuthorizerCallback) null);
        this._authorizerCallback = (SQLiteAuthorizerCallback) null;
      }
    }

    public event SQLiteUpdateEventHandler Update
    {
      add
      {
        this.CheckDisposed();
        if (this._updateHandler == null)
        {
          this._updateCallback = new SQLiteUpdateCallback(this.UpdateCallback);
          if (this._sql != null)
            this._sql.SetUpdateHook(this._updateCallback);
        }
        this._updateHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._updateHandler -= value;
        if (this._updateHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetUpdateHook((SQLiteUpdateCallback) null);
        this._updateCallback = (SQLiteUpdateCallback) null;
      }
    }

    private SQLiteProgressReturnCode ProgressCallback(IntPtr pUserData)
    {
      try
      {
        ProgressEventArgs e = new ProgressEventArgs(pUserData, SQLiteProgressReturnCode.Continue);
        if (this._progressHandler != null)
          this._progressHandler((object) this, e);
        return e.ReturnCode;
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) == SQLiteConnectionFlags.LogCallbackException)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Progress\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
      return (this._flags & SQLiteConnectionFlags.InterruptOnException) == SQLiteConnectionFlags.InterruptOnException ? SQLiteProgressReturnCode.Interrupt : SQLiteProgressReturnCode.Continue;
    }

    private SQLiteAuthorizerReturnCode AuthorizerCallback(
      IntPtr pUserData,
      SQLiteAuthorizerActionCode actionCode,
      IntPtr pArgument1,
      IntPtr pArgument2,
      IntPtr pDatabase,
      IntPtr pAuthContext)
    {
      try
      {
        AuthorizerEventArgs e = new AuthorizerEventArgs(pUserData, actionCode, SQLiteConvert.UTF8ToString(pArgument1, -1), SQLiteConvert.UTF8ToString(pArgument2, -1), SQLiteConvert.UTF8ToString(pDatabase, -1), SQLiteConvert.UTF8ToString(pAuthContext, -1), SQLiteAuthorizerReturnCode.Ok);
        if (this._authorizerHandler != null)
          this._authorizerHandler((object) this, e);
        return e.ReturnCode;
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) == SQLiteConnectionFlags.LogCallbackException)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Authorize\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
      return (this._flags & SQLiteConnectionFlags.DenyOnException) == SQLiteConnectionFlags.DenyOnException ? SQLiteAuthorizerReturnCode.Deny : SQLiteAuthorizerReturnCode.Ok;
    }

    private void UpdateCallback(
      IntPtr puser,
      int type,
      IntPtr database,
      IntPtr table,
      long rowid)
    {
      try
      {
        this._updateHandler((object) this, new UpdateEventArgs(SQLiteConvert.UTF8ToString(database, -1), SQLiteConvert.UTF8ToString(table, -1), (UpdateEventType) type, rowid));
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) != SQLiteConnectionFlags.LogCallbackException)
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Update\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
    }

    public event SQLiteCommitHandler Commit
    {
      add
      {
        this.CheckDisposed();
        if (this._commitHandler == null)
        {
          this._commitCallback = new SQLiteCommitCallback(this.CommitCallback);
          if (this._sql != null)
            this._sql.SetCommitHook(this._commitCallback);
        }
        this._commitHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._commitHandler -= value;
        if (this._commitHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetCommitHook((SQLiteCommitCallback) null);
        this._commitCallback = (SQLiteCommitCallback) null;
      }
    }

    public event SQLiteTraceEventHandler Trace
    {
      add
      {
        this.CheckDisposed();
        if (this._traceHandler == null)
        {
          this._traceCallback = new SQLiteTraceCallback(this.TraceCallback);
          if (this._sql != null)
            this._sql.SetTraceCallback(this._traceCallback);
        }
        this._traceHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._traceHandler -= value;
        if (this._traceHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetTraceCallback((SQLiteTraceCallback) null);
        this._traceCallback = (SQLiteTraceCallback) null;
      }
    }

    private void TraceCallback(IntPtr puser, IntPtr statement)
    {
      try
      {
        if (this._traceHandler == null)
          return;
        this._traceHandler((object) this, new TraceEventArgs(SQLiteConvert.UTF8ToString(statement, -1)));
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) != SQLiteConnectionFlags.LogCallbackException)
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Trace\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
    }

    public event EventHandler RollBack
    {
      add
      {
        this.CheckDisposed();
        if (this._rollbackHandler == null)
        {
          this._rollbackCallback = new SQLiteRollbackCallback(this.RollbackCallback);
          if (this._sql != null)
            this._sql.SetRollbackHook(this._rollbackCallback);
        }
        this._rollbackHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._rollbackHandler -= value;
        if (this._rollbackHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetRollbackHook((SQLiteRollbackCallback) null);
        this._rollbackCallback = (SQLiteRollbackCallback) null;
      }
    }

    private int CommitCallback(IntPtr parg)
    {
      try
      {
        CommitEventArgs e = new CommitEventArgs();
        if (this._commitHandler != null)
          this._commitHandler((object) this, e);
        return e.AbortTransaction ? 1 : 0;
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) == SQLiteConnectionFlags.LogCallbackException)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Commit\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
      return (this._flags & SQLiteConnectionFlags.RollbackOnException) == SQLiteConnectionFlags.RollbackOnException ? 1 : 0;
    }

    private void RollbackCallback(IntPtr parg)
    {
      try
      {
        if (this._rollbackHandler == null)
          return;
        this._rollbackHandler((object) this, EventArgs.Empty);
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) != SQLiteConnectionFlags.LogCallbackException)
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Rollback\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
    }

    protected override DbProviderFactory DbProviderFactory
    {
      get => (DbProviderFactory) SQLiteFactory.Instance;
    }
  }
}
