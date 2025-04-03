// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStatement
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLiteStatement : IDisposable
  {
    internal SQLiteBase _sql;
    internal string _sqlStatement;
    internal SQLiteStatementHandle _sqlite_stmt;
    internal int _unnamedParameters;
    internal string[] _paramNames;
    internal SQLiteParameter[] _paramValues;
    internal SQLiteCommand _command;
    private SQLiteConnectionFlags _flags;
    private string[] _types;
    private bool disposed;

    internal SQLiteStatement(
      SQLiteBase sqlbase,
      SQLiteConnectionFlags flags,
      SQLiteStatementHandle stmt,
      string strCommand,
      SQLiteStatement previous)
    {
      this._sql = sqlbase;
      this._sqlite_stmt = stmt;
      this._sqlStatement = strCommand;
      this._flags = flags;
      int num = 0;
      int length = this._sql.Bind_ParamCount(this, this._flags);
      if (length <= 0)
        return;
      if (previous != null)
        num = previous._unnamedParameters;
      this._paramNames = new string[length];
      this._paramValues = new SQLiteParameter[length];
      for (int index = 0; index < length; ++index)
      {
        string str = this._sql.Bind_ParamName(this, this._flags, index + 1);
        if (string.IsNullOrEmpty(str))
        {
          str = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, ";{0}", (object) num);
          ++num;
          ++this._unnamedParameters;
        }
        this._paramNames[index] = str;
        this._paramValues[index] = (SQLiteParameter) null;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteStatement).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._sqlite_stmt != null)
        {
          this._sqlite_stmt.Dispose();
          this._sqlite_stmt = (SQLiteStatementHandle) null;
        }
        this._paramNames = (string[]) null;
        this._paramValues = (SQLiteParameter[]) null;
        this._sql = (SQLiteBase) null;
        this._sqlStatement = (string) null;
      }
      this.disposed = true;
    }

    ~SQLiteStatement() => this.Dispose(false);

    internal bool TryGetChanges(ref int changes, ref bool readOnly)
    {
      if (this._sql == null || !this._sql.IsOpen())
        return false;
      changes = this._sql.Changes;
      readOnly = this._sql.IsReadOnly(this);
      return true;
    }

    internal bool MapParameter(string s, SQLiteParameter p)
    {
      if (this._paramNames == null)
        return false;
      int indexA = 0;
      if (s.Length > 0 && ":$@;".IndexOf(s[0]) == -1)
        indexA = 1;
      int length = this._paramNames.Length;
      for (int index = 0; index < length; ++index)
      {
        if (string.Compare(this._paramNames[index], indexA, s, 0, Math.Max(this._paramNames[index].Length - indexA, s.Length), StringComparison.OrdinalIgnoreCase) == 0)
        {
          this._paramValues[index] = p;
          return true;
        }
      }
      return false;
    }

    internal void BindParameters()
    {
      if (this._paramNames == null)
        return;
      int length = this._paramNames.Length;
      for (int index = 0; index < length; ++index)
        this.BindParameter(index + 1, this._paramValues[index]);
    }

    private static SQLiteConnection GetConnection(SQLiteStatement statement)
    {
      try
      {
        if (statement != null)
        {
          SQLiteCommand command = statement._command;
          if (command != null)
          {
            SQLiteConnection connection = command.Connection;
            if (connection != null)
              return connection;
          }
        }
      }
      catch (ObjectDisposedException ex)
      {
      }
      return (SQLiteConnection) null;
    }

    private void InvokeBindValueCallback(int index, SQLiteParameter parameter, out bool complete)
    {
      complete = false;
      SQLiteConnectionFlags flags = this._flags;
      this._flags &= ~SQLiteConnectionFlags.UseConnectionBindValueCallbacks;
      try
      {
        if (parameter == null)
          return;
        SQLiteConnection connection = SQLiteStatement.GetConnection(this);
        if (connection == null)
          return;
        string typeName = parameter.TypeName;
        if (typeName == null && (this._flags & SQLiteConnectionFlags.UseParameterNameForTypeName) == SQLiteConnectionFlags.UseParameterNameForTypeName)
          typeName = parameter.ParameterName;
        if (typeName == null && (this._flags & SQLiteConnectionFlags.UseParameterDbTypeForTypeName) == SQLiteConnectionFlags.UseParameterDbTypeForTypeName)
          typeName = SQLiteConvert.DbTypeToTypeName(connection, parameter.DbType, this._flags);
        SQLiteTypeCallbacks callbacks;
        if (typeName == null || !connection.TryGetTypeCallbacks(typeName, out callbacks) || callbacks == null)
          return;
        SQLiteBindValueCallback bindValueCallback = callbacks.BindValueCallback;
        if (bindValueCallback == null)
          return;
        object bindValueUserData = callbacks.BindValueUserData;
        bindValueCallback((SQLiteConvert) this._sql, this._command, flags, parameter, typeName, index, bindValueUserData, out complete);
      }
      finally
      {
        this._flags |= SQLiteConnectionFlags.UseConnectionBindValueCallbacks;
      }
    }

    private void BindParameter(int index, SQLiteParameter param)
    {
      if (param == null)
        throw new SQLiteException("Insufficient parameters supplied to the command");
      if ((this._flags & SQLiteConnectionFlags.UseConnectionBindValueCallbacks) == SQLiteConnectionFlags.UseConnectionBindValueCallbacks)
      {
        bool complete;
        this.InvokeBindValueCallback(index, param, out complete);
        if (complete)
          return;
      }
      object obj = param.Value;
      DbType dbType = param.DbType;
      if (obj != null && dbType == DbType.Object)
        dbType = SQLiteConvert.TypeToDbType(obj.GetType());
      if ((this._flags & SQLiteConnectionFlags.LogPreBind) == SQLiteConnectionFlags.LogPreBind)
        SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Binding statement {0} paramter #{1} with database type {2} and raw value {{{3}}}...", (object) (IntPtr) this._sqlite_stmt, (object) index, (object) dbType, obj));
      if (obj == null || Convert.IsDBNull(obj))
      {
        this._sql.Bind_Null(this, this._flags, index);
      }
      else
      {
        CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        bool flag = (this._flags & SQLiteConnectionFlags.BindInvariantText) == SQLiteConnectionFlags.BindInvariantText;
        if ((this._flags & SQLiteConnectionFlags.BindAllAsText) == SQLiteConnectionFlags.BindAllAsText)
        {
          if (obj is DateTime dt)
            this._sql.Bind_DateTime(this, this._flags, index, dt);
          else
            this._sql.Bind_Text(this, this._flags, index, flag ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : obj.ToString());
        }
        else
        {
          CultureInfo provider = CultureInfo.CurrentCulture;
          if ((this._flags & SQLiteConnectionFlags.ConvertInvariantText) == SQLiteConnectionFlags.ConvertInvariantText)
            provider = invariantCulture;
          switch (dbType)
          {
            case DbType.Binary:
              this._sql.Bind_Blob(this, this._flags, index, (byte[]) obj);
              break;
            case DbType.Byte:
              this._sql.Bind_UInt32(this, this._flags, index, (uint) Convert.ToByte(obj, (IFormatProvider) provider));
              break;
            case DbType.Boolean:
              this._sql.Bind_Boolean(this, this._flags, index, SQLiteConvert.ToBoolean(obj, (IFormatProvider) provider, true));
              break;
            case DbType.Currency:
            case DbType.Double:
            case DbType.Single:
              this._sql.Bind_Double(this, this._flags, index, Convert.ToDouble(obj, (IFormatProvider) provider));
              break;
            case DbType.Date:
            case DbType.DateTime:
            case DbType.Time:
              this._sql.Bind_DateTime(this, this._flags, index, obj is string ? this._sql.ToDateTime((string) obj) : Convert.ToDateTime(obj, (IFormatProvider) provider));
              break;
            case DbType.Decimal:
              this._sql.Bind_Text(this, this._flags, index, Convert.ToDecimal(obj, (IFormatProvider) provider).ToString((IFormatProvider) invariantCulture));
              break;
            case DbType.Guid:
              if (this._command.Connection._binaryGuid)
              {
                this._sql.Bind_Blob(this, this._flags, index, ((Guid) obj).ToByteArray());
                break;
              }
              this._sql.Bind_Text(this, this._flags, index, flag ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : obj.ToString());
              break;
            case DbType.Int16:
              this._sql.Bind_Int32(this, this._flags, index, (int) Convert.ToInt16(obj, (IFormatProvider) provider));
              break;
            case DbType.Int32:
              this._sql.Bind_Int32(this, this._flags, index, Convert.ToInt32(obj, (IFormatProvider) provider));
              break;
            case DbType.Int64:
              this._sql.Bind_Int64(this, this._flags, index, Convert.ToInt64(obj, (IFormatProvider) provider));
              break;
            case DbType.SByte:
              this._sql.Bind_Int32(this, this._flags, index, (int) Convert.ToSByte(obj, (IFormatProvider) provider));
              break;
            case DbType.UInt16:
              this._sql.Bind_UInt32(this, this._flags, index, (uint) Convert.ToUInt16(obj, (IFormatProvider) provider));
              break;
            case DbType.UInt32:
              this._sql.Bind_UInt32(this, this._flags, index, Convert.ToUInt32(obj, (IFormatProvider) provider));
              break;
            case DbType.UInt64:
              this._sql.Bind_UInt64(this, this._flags, index, Convert.ToUInt64(obj, (IFormatProvider) provider));
              break;
            default:
              this._sql.Bind_Text(this, this._flags, index, flag ? SQLiteConvert.ToStringWithProvider(obj, (IFormatProvider) invariantCulture) : obj.ToString());
              break;
          }
        }
      }
    }

    internal string[] TypeDefinitions => this._types;

    internal void SetTypes(string typedefs)
    {
      int num = typedefs.IndexOf("TYPES", 0, StringComparison.OrdinalIgnoreCase);
      if (num == -1)
        throw new ArgumentOutOfRangeException();
      string[] strArray = typedefs.Substring(num + 6).Replace(" ", string.Empty).Replace(";", string.Empty).Replace("\"", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("`", string.Empty).Split(',', '\r', '\n', '\t');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (string.IsNullOrEmpty(strArray[index]))
          strArray[index] = (string) null;
      }
      this._types = strArray;
    }
  }
}
