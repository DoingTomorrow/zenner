// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCommand
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  [Designer("SQLite.Designer.SQLiteCommandDesigner, SQLite.Designer, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
  [ToolboxItem(true)]
  public sealed class SQLiteCommand : DbCommand, ICloneable
  {
    private static readonly string DefaultConnectionString = "Data Source=:memory:;";
    private string _commandText;
    private SQLiteConnection _cnn;
    private int _version;
    private WeakReference _activeReader;
    internal int _commandTimeout;
    private bool _designTimeVisible;
    private UpdateRowSource _updateRowSource;
    private SQLiteParameterCollection _parameterCollection;
    internal List<SQLiteStatement> _statementList;
    internal string _remainingText;
    private SQLiteTransaction _transaction;
    private bool disposed;

    public SQLiteCommand()
      : this((string) null, (SQLiteConnection) null)
    {
    }

    public SQLiteCommand(string commandText)
      : this(commandText, (SQLiteConnection) null, (SQLiteTransaction) null)
    {
    }

    public SQLiteCommand(string commandText, SQLiteConnection connection)
      : this(commandText, connection, (SQLiteTransaction) null)
    {
    }

    public SQLiteCommand(SQLiteConnection connection)
      : this((string) null, connection, (SQLiteTransaction) null)
    {
    }

    private SQLiteCommand(SQLiteCommand source)
      : this(source.CommandText, source.Connection, source.Transaction)
    {
      this.CommandTimeout = source.CommandTimeout;
      this.DesignTimeVisible = source.DesignTimeVisible;
      this.UpdatedRowSource = source.UpdatedRowSource;
      foreach (SQLiteParameter parameter in (DbParameterCollection) source._parameterCollection)
        this.Parameters.Add(parameter.Clone());
    }

    public SQLiteCommand(
      string commandText,
      SQLiteConnection connection,
      SQLiteTransaction transaction)
    {
      this._commandTimeout = 30;
      this._parameterCollection = new SQLiteParameterCollection(this);
      this._designTimeVisible = true;
      this._updateRowSource = UpdateRowSource.None;
      if (commandText != null)
        this.CommandText = commandText;
      if (connection != null)
      {
        this.DbConnection = (DbConnection) connection;
        this._commandTimeout = connection.DefaultTimeout;
      }
      if (transaction != null)
        this.Transaction = transaction;
      SQLiteConnection.OnChanged(connection, new ConnectionEventArgs(SQLiteConnectionEventType.NewCommand, (StateChangeEventArgs) null, (IDbTransaction) transaction, (IDbCommand) this, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
    }

    [Conditional("CHECK_STATE")]
    internal static void Check(SQLiteCommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      command.CheckDisposed();
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteCommand).Name);
    }

    protected override void Dispose(bool disposing)
    {
      SQLiteConnection.OnChanged(this._cnn, new ConnectionEventArgs(SQLiteConnectionEventType.DisposingCommand, (StateChangeEventArgs) null, (IDbTransaction) this._transaction, (IDbCommand) this, (IDataReader) null, (CriticalHandle) null, (string) null, (object) new object[2]
      {
        (object) disposing,
        (object) this.disposed
      }));
      bool flag = false;
      try
      {
        if (this.disposed || !disposing)
          return;
        SQLiteDataReader sqLiteDataReader = (SQLiteDataReader) null;
        if (this._activeReader != null)
        {
          try
          {
            sqLiteDataReader = this._activeReader.Target as SQLiteDataReader;
          }
          catch (InvalidOperationException ex)
          {
          }
        }
        if (sqLiteDataReader != null)
        {
          sqLiteDataReader._disposeCommand = true;
          this._activeReader = (WeakReference) null;
          flag = true;
        }
        else
        {
          this.Connection = (SQLiteConnection) null;
          this._parameterCollection.Clear();
          this._commandText = (string) null;
        }
      }
      finally
      {
        if (!flag)
        {
          base.Dispose(disposing);
          this.disposed = true;
        }
      }
    }

    internal static SQLiteConnectionFlags GetFlags(SQLiteCommand command)
    {
      try
      {
        if (command != null)
        {
          SQLiteConnection cnn = command._cnn;
          if (cnn != null)
            return cnn.Flags;
        }
      }
      catch (ObjectDisposedException ex)
      {
      }
      return SQLiteConnectionFlags.Default;
    }

    private void DisposeStatements()
    {
      if (this._statementList == null)
        return;
      int count = this._statementList.Count;
      for (int index = 0; index < count; ++index)
        this._statementList[index]?.Dispose();
      this._statementList = (List<SQLiteStatement>) null;
    }

    private void ClearDataReader()
    {
      if (this._activeReader == null)
        return;
      SQLiteDataReader sqLiteDataReader = (SQLiteDataReader) null;
      try
      {
        sqLiteDataReader = this._activeReader.Target as SQLiteDataReader;
      }
      catch (InvalidOperationException ex)
      {
      }
      sqLiteDataReader?.Close();
      this._activeReader = (WeakReference) null;
    }

    internal void ClearCommands()
    {
      this.ClearDataReader();
      this.DisposeStatements();
      this._parameterCollection.Unbind();
    }

    internal SQLiteStatement BuildNextCommand()
    {
      SQLiteStatement activeStatement = (SQLiteStatement) null;
      try
      {
        if (this._cnn != null && this._cnn._sql != null)
        {
          if (this._statementList == null)
            this._remainingText = this._commandText;
          activeStatement = this._cnn._sql.Prepare(this._cnn, this._remainingText, this._statementList == null ? (SQLiteStatement) null : this._statementList[this._statementList.Count - 1], (uint) (this._commandTimeout * 1000), ref this._remainingText);
          if (activeStatement != null)
          {
            activeStatement._command = this;
            if (this._statementList == null)
              this._statementList = new List<SQLiteStatement>();
            this._statementList.Add(activeStatement);
            this._parameterCollection.MapParameters(activeStatement);
            activeStatement.BindParameters();
          }
        }
        return activeStatement;
      }
      catch (Exception ex)
      {
        if (activeStatement != null)
        {
          if (this._statementList != null && this._statementList.Contains(activeStatement))
            this._statementList.Remove(activeStatement);
          activeStatement.Dispose();
        }
        this._remainingText = (string) null;
        throw;
      }
    }

    internal SQLiteStatement GetStatement(int index)
    {
      if (this._statementList == null)
        return this.BuildNextCommand();
      if (index == this._statementList.Count)
        return !string.IsNullOrEmpty(this._remainingText) ? this.BuildNextCommand() : (SQLiteStatement) null;
      SQLiteStatement statement = this._statementList[index];
      statement.BindParameters();
      return statement;
    }

    public override void Cancel()
    {
      this.CheckDisposed();
      if (this._activeReader == null || !(this._activeReader.Target is SQLiteDataReader target))
        return;
      target.Cancel();
    }

    [Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.All)]
    public override string CommandText
    {
      get
      {
        this.CheckDisposed();
        return this._commandText;
      }
      set
      {
        this.CheckDisposed();
        if (this._commandText == value)
          return;
        if (this._activeReader != null && this._activeReader.IsAlive)
          throw new InvalidOperationException("Cannot set CommandText while a DataReader is active");
        this.ClearCommands();
        this._commandText = value;
        SQLiteConnection cnn = this._cnn;
      }
    }

    [DefaultValue(30)]
    public override int CommandTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._commandTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._commandTimeout = value;
      }
    }

    [DefaultValue(CommandType.Text)]
    [RefreshProperties(RefreshProperties.All)]
    public override CommandType CommandType
    {
      get
      {
        this.CheckDisposed();
        return CommandType.Text;
      }
      set
      {
        this.CheckDisposed();
        if (value != CommandType.Text)
          throw new NotSupportedException();
      }
    }

    protected override DbParameter CreateDbParameter() => (DbParameter) this.CreateParameter();

    public SQLiteParameter CreateParameter()
    {
      this.CheckDisposed();
      return new SQLiteParameter((IDbCommand) this);
    }

    [DefaultValue(null)]
    [Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public SQLiteConnection Connection
    {
      get
      {
        this.CheckDisposed();
        return this._cnn;
      }
      set
      {
        this.CheckDisposed();
        if (this._activeReader != null && this._activeReader.IsAlive)
          throw new InvalidOperationException("Cannot set Connection while a DataReader is active");
        if (this._cnn != null)
          this.ClearCommands();
        this._cnn = value;
        if (this._cnn == null)
          return;
        this._version = this._cnn._version;
      }
    }

    protected override DbConnection DbConnection
    {
      get => (DbConnection) this.Connection;
      set => this.Connection = (SQLiteConnection) value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public SQLiteParameterCollection Parameters
    {
      get
      {
        this.CheckDisposed();
        return this._parameterCollection;
      }
    }

    protected override DbParameterCollection DbParameterCollection
    {
      get => (DbParameterCollection) this.Parameters;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SQLiteTransaction Transaction
    {
      get
      {
        this.CheckDisposed();
        return this._transaction;
      }
      set
      {
        this.CheckDisposed();
        if (this._cnn != null)
        {
          if (this._activeReader != null && this._activeReader.IsAlive)
            throw new InvalidOperationException("Cannot set Transaction while a DataReader is active");
          if (value != null && value._cnn != this._cnn)
            throw new ArgumentException("Transaction is not associated with the command's connection");
          this._transaction = value;
        }
        else
        {
          if (value != null)
            this.Connection = value.Connection;
          this._transaction = value;
        }
      }
    }

    protected override DbTransaction DbTransaction
    {
      get => (DbTransaction) this.Transaction;
      set => this.Transaction = (SQLiteTransaction) value;
    }

    public void VerifyOnly()
    {
      this.CheckDisposed();
      SQLiteConnection cnn = this._cnn;
      SQLiteBase sql = cnn._sql;
      if (cnn == null || sql == null)
        throw new SQLiteException("invalid or unusable connection");
      List<SQLiteStatement> sqLiteStatementList = (List<SQLiteStatement>) null;
      SQLiteStatement sqLiteStatement1 = (SQLiteStatement) null;
      try
      {
        string strRemain = this._commandText;
        uint timeoutMS = (uint) (this._commandTimeout * 1000);
        SQLiteStatement previous = (SQLiteStatement) null;
        while (strRemain != null && strRemain.Length > 0)
        {
          sqLiteStatement1 = sql.Prepare(cnn, strRemain, previous, timeoutMS, ref strRemain);
          previous = sqLiteStatement1;
          if (sqLiteStatement1 != null)
          {
            if (sqLiteStatementList == null)
              sqLiteStatementList = new List<SQLiteStatement>();
            sqLiteStatementList.Add(sqLiteStatement1);
            sqLiteStatement1 = (SQLiteStatement) null;
          }
          if (strRemain != null)
            strRemain = strRemain.Trim();
        }
      }
      finally
      {
        sqLiteStatement1?.Dispose();
        if (sqLiteStatementList != null)
        {
          foreach (SQLiteStatement sqLiteStatement2 in sqLiteStatementList)
            sqLiteStatement2?.Dispose();
          sqLiteStatementList.Clear();
        }
      }
    }

    private void InitializeForReader()
    {
      if (this._activeReader != null && this._activeReader.IsAlive)
        throw new InvalidOperationException("DataReader already active on this command");
      if (this._cnn == null)
        throw new InvalidOperationException("No connection associated with this command");
      if (this._cnn.State != ConnectionState.Open)
        throw new InvalidOperationException("Database is not open");
      if (this._cnn._version != this._version)
      {
        this._version = this._cnn._version;
        this.ClearCommands();
      }
      this._parameterCollection.MapParameters((SQLiteStatement) null);
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
      return (DbDataReader) this.ExecuteReader(behavior);
    }

    public static object Execute(
      string commandText,
      SQLiteExecuteType executeType,
      string connectionString,
      params object[] args)
    {
      return SQLiteCommand.Execute(commandText, executeType, CommandBehavior.Default, connectionString, args);
    }

    public static object Execute(
      string commandText,
      SQLiteExecuteType executeType,
      CommandBehavior commandBehavior,
      string connectionString,
      params object[] args)
    {
      SQLiteConnection sqLiteConnection = (SQLiteConnection) null;
      try
      {
        if (connectionString == null)
          connectionString = SQLiteCommand.DefaultConnectionString;
        using (sqLiteConnection = new SQLiteConnection(connectionString))
        {
          sqLiteConnection.Open();
          using (SQLiteCommand command = sqLiteConnection.CreateCommand())
          {
            command.CommandText = commandText;
            if (args != null)
            {
              foreach (object obj in args)
              {
                if (!(obj is SQLiteParameter parameter))
                {
                  parameter = command.CreateParameter();
                  parameter.DbType = DbType.Object;
                  parameter.Value = obj;
                }
                command.Parameters.Add(parameter);
              }
            }
            switch (executeType)
            {
              case SQLiteExecuteType.NonQuery:
                return (object) command.ExecuteNonQuery(commandBehavior);
              case SQLiteExecuteType.Scalar:
                return command.ExecuteScalar(commandBehavior);
              case SQLiteExecuteType.Reader:
                bool flag = true;
                try
                {
                  return (object) command.ExecuteReader(commandBehavior | CommandBehavior.CloseConnection);
                }
                catch
                {
                  flag = false;
                  throw;
                }
                finally
                {
                  if (flag)
                    sqLiteConnection._noDispose = true;
                }
            }
          }
        }
      }
      finally
      {
        if (sqLiteConnection != null)
          sqLiteConnection._noDispose = false;
      }
      return (object) null;
    }

    public SQLiteDataReader ExecuteReader(CommandBehavior behavior)
    {
      this.CheckDisposed();
      this.InitializeForReader();
      SQLiteDataReader target = new SQLiteDataReader(this, behavior);
      this._activeReader = new WeakReference((object) target, false);
      return target;
    }

    public SQLiteDataReader ExecuteReader()
    {
      this.CheckDisposed();
      return this.ExecuteReader(CommandBehavior.Default);
    }

    internal void ResetDataReader() => this._activeReader = (WeakReference) null;

    public override int ExecuteNonQuery()
    {
      this.CheckDisposed();
      return this.ExecuteNonQuery(CommandBehavior.Default);
    }

    public int ExecuteNonQuery(CommandBehavior behavior)
    {
      this.CheckDisposed();
      using (SQLiteDataReader sqLiteDataReader = this.ExecuteReader(behavior | CommandBehavior.SingleRow | CommandBehavior.SingleResult))
      {
        do
          ;
        while (sqLiteDataReader.NextResult());
        return sqLiteDataReader.RecordsAffected;
      }
    }

    public override object ExecuteScalar()
    {
      this.CheckDisposed();
      return this.ExecuteScalar(CommandBehavior.Default);
    }

    public object ExecuteScalar(CommandBehavior behavior)
    {
      this.CheckDisposed();
      using (SQLiteDataReader sqLiteDataReader = this.ExecuteReader(behavior | CommandBehavior.SingleRow | CommandBehavior.SingleResult))
      {
        if (sqLiteDataReader.Read())
        {
          if (sqLiteDataReader.FieldCount > 0)
            return sqLiteDataReader[0];
        }
      }
      return (object) null;
    }

    public void Reset()
    {
      this.CheckDisposed();
      this.Reset(true, false);
    }

    public void Reset(bool clearBindings, bool ignoreErrors)
    {
      this.CheckDisposed();
      if (clearBindings && this._parameterCollection != null)
        this._parameterCollection.Unbind();
      this.ClearDataReader();
      if (this._statementList == null)
        return;
      SQLiteBase sql = this._cnn._sql;
      foreach (SQLiteStatement statement in this._statementList)
      {
        if (statement != null)
        {
          SQLiteStatementHandle sqliteStmt = statement._sqlite_stmt;
          if (sqliteStmt != null)
          {
            SQLiteErrorCode errorCode = sql.Reset(statement);
            if (errorCode == SQLiteErrorCode.Ok && clearBindings && SQLite3.SQLiteVersionNumber >= 3003007)
              errorCode = UnsafeNativeMethods.sqlite3_clear_bindings((IntPtr) sqliteStmt);
            if (!ignoreErrors && errorCode != SQLiteErrorCode.Ok)
              throw new SQLiteException(errorCode, sql.GetLastError());
          }
        }
      }
    }

    public override void Prepare() => this.CheckDisposed();

    [DefaultValue(UpdateRowSource.None)]
    public override UpdateRowSource UpdatedRowSource
    {
      get
      {
        this.CheckDisposed();
        return this._updateRowSource;
      }
      set
      {
        this.CheckDisposed();
        this._updateRowSource = value;
      }
    }

    [DefaultValue(true)]
    [DesignOnly(true)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool DesignTimeVisible
    {
      get
      {
        this.CheckDisposed();
        return this._designTimeVisible;
      }
      set
      {
        this.CheckDisposed();
        this._designTimeVisible = value;
        TypeDescriptor.Refresh((object) this);
      }
    }

    public object Clone()
    {
      this.CheckDisposed();
      return (object) new SQLiteCommand(this);
    }
  }
}
