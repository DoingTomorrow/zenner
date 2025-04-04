// Decompiled with JetBrains decompiler
// Type: SQLitePCL.SQLiteStatement
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SQLitePCL
{
  public class SQLiteStatement : ISQLiteStatement, IDisposable
  {
    private IPlatformMarshal platformMarshal;
    private ISQLite3Provider sqlite3Provider;
    private SQLiteConnection connection;
    private IntPtr stm;
    private Dictionary<string, int> columnNameIndexDic;
    private Dictionary<int, string> columnIndexNameDic;
    private bool disposed;

    internal SQLiteStatement(SQLiteConnection connection, IntPtr stm)
    {
      this.platformMarshal = Platform.Instance.PlatformMarshal;
      this.sqlite3Provider = Platform.Instance.SQLite3Provider;
      this.connection = connection;
      this.stm = stm;
      this.columnNameIndexDic = new Dictionary<string, int>();
      this.columnIndexNameDic = new Dictionary<int, string>();
      for (int index = 0; index < this.ColumnCount; ++index)
      {
        string managed = this.platformMarshal.MarshalStringNativeUTF8ToManaged(this.sqlite3Provider.Sqlite3ColumnName(this.stm, index));
        if (!string.IsNullOrEmpty(managed) && !this.columnNameIndexDic.ContainsKey(managed))
          this.columnNameIndexDic.Add(managed, index);
        this.columnIndexNameDic.Add(index, managed);
      }
    }

    ~SQLiteStatement() => this.Dispose(false);

    public ISQLiteConnection Connection => (ISQLiteConnection) this.connection;

    public int ColumnCount => this.sqlite3Provider.Sqlite3ColumnCount(this.stm);

    public int DataCount => this.sqlite3Provider.Sqlite3DataCount(this.stm);

    public object this[int index]
    {
      get
      {
        object destination = (object) null;
        switch (this.sqlite3Provider.Sqlite3ColumnType(this.stm, index))
        {
          case 1:
            destination = (object) this.sqlite3Provider.Sqlite3ColumnInt64(this.stm, index);
            break;
          case 2:
            destination = (object) this.sqlite3Provider.Sqlite3ColumnDouble(this.stm, index);
            break;
          case 3:
            destination = (object) this.platformMarshal.MarshalStringNativeUTF8ToManaged(this.sqlite3Provider.Sqlite3ColumnText(this.stm, index));
            break;
          case 4:
            IntPtr source = this.sqlite3Provider.Sqlite3ColumnBlob(this.stm, index);
            if (source != IntPtr.Zero)
            {
              int length = this.sqlite3Provider.Sqlite3ColumnBytes(this.stm, index);
              destination = (object) new byte[length];
              this.platformMarshal.Copy(source, (byte[]) destination, 0, length);
              break;
            }
            destination = (object) new byte[0];
            break;
        }
        return destination;
      }
    }

    public object this[string name] => this[this.ColumnIndex(name)];

    public SQLiteType DataType(int index)
    {
      return (SQLiteType) this.sqlite3Provider.Sqlite3ColumnType(this.stm, index);
    }

    public SQLiteType DataType(string name) => this.DataType(this.ColumnIndex(name));

    public string ColumnName(int index)
    {
      string str;
      if (this.columnIndexNameDic.TryGetValue(index, out str))
        return str;
      throw new SQLiteException("Unable to find column with the specified index: " + (object) index);
    }

    public int ColumnIndex(string name)
    {
      int num;
      if (this.columnNameIndexDic.TryGetValue(name, out num))
        return num;
      throw new SQLiteException("Unable to find column with the specified name: " + name);
    }

    public SQLiteResult Step() => (SQLiteResult) this.sqlite3Provider.Sqlite3Step(this.stm);

    public long GetInteger(int index)
    {
      SQLiteType sqLiteType = this.DataType(index);
      if (sqLiteType != SQLiteType.INTEGER)
        throw new SQLiteException("Unable to cast existing data type to Integer type: " + sqLiteType.ToString());
      return (long) this[index];
    }

    public long GetInteger(string name) => this.GetInteger(this.ColumnIndex(name));

    public double GetFloat(int index)
    {
      SQLiteType sqLiteType = this.DataType(index);
      if (sqLiteType != SQLiteType.FLOAT)
        throw new SQLiteException("Unable to cast existing data type to Float type: " + sqLiteType.ToString());
      return (double) this[index];
    }

    public double GetFloat(string name) => this.GetFloat(this.ColumnIndex(name));

    public string GetText(int index)
    {
      SQLiteType sqLiteType = this.DataType(index);
      if (sqLiteType != SQLiteType.TEXT)
        throw new SQLiteException("Unable to cast existing data type to Text type: " + sqLiteType.ToString());
      return (string) this[index];
    }

    public string GetText(string name) => this.GetText(this.ColumnIndex(name));

    public byte[] GetBlob(int index)
    {
      SQLiteType sqLiteType = this.DataType(index);
      if (sqLiteType != SQLiteType.BLOB)
        throw new SQLiteException("Unable to cast existing data type to Blob type: " + sqLiteType.ToString());
      return (byte[]) this[index];
    }

    public byte[] GetBlob(string name) => this.GetBlob(this.ColumnIndex(name));

    public void Reset()
    {
      if (this.sqlite3Provider.Sqlite3Reset(this.stm) != 0)
        throw new SQLiteException(this.connection.ErrorMessage());
    }

    public void Bind(int index, object value)
    {
      int num = 0;
      if (value == null)
        num = this.sqlite3Provider.Sqlite3BindNull(this.stm, index);
      else if (SQLiteStatement.IsSupportedInteger(value))
        num = this.sqlite3Provider.Sqlite3BindInt64(this.stm, index, SQLiteStatement.GetInteger(value));
      else if (SQLiteStatement.IsSupportedFloat(value))
        num = this.sqlite3Provider.Sqlite3BindDouble(this.stm, index, SQLiteStatement.GetFloat(value));
      else if (SQLiteStatement.IsSupportedText(value))
      {
        int size;
        IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(value.ToString(), out size);
        try
        {
          num = this.sqlite3Provider.Sqlite3BindText(this.stm, index, nativeUtF8, size - 1, (IntPtr) -1);
        }
        finally
        {
          if (nativeUtF8 != IntPtr.Zero)
            this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
        }
      }
      else
      {
        if (!(value is byte[]))
          throw new SQLiteException("Unable to bind parameter with unsupported type: " + value.GetType().FullName);
        num = this.sqlite3Provider.Sqlite3BindBlob(this.stm, index, (byte[]) value, ((byte[]) value).Length, (IntPtr) -1);
      }
      if (num != 0)
        throw new SQLiteException(this.connection.ErrorMessage());
    }

    public void Bind(string paramName, object value)
    {
      IntPtr nativeUtF8 = this.platformMarshal.MarshalStringManagedToNativeUTF8(paramName);
      try
      {
        this.Bind(this.sqlite3Provider.Sqlite3BindParameterIndex(this.stm, nativeUtF8), value);
      }
      finally
      {
        if (nativeUtF8 != IntPtr.Zero)
          this.platformMarshal.CleanUpStringNativeUTF8(nativeUtF8);
      }
    }

    public void ClearBindings()
    {
      if (this.sqlite3Provider.Sqlite3ClearBindings(this.stm) != 0)
        throw new SQLiteException(this.connection.ErrorMessage());
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.sqlite3Provider.Sqlite3Finalize(this.stm);
      this.stm = IntPtr.Zero;
      this.disposed = true;
    }

    private static bool IsSupportedInteger(object value)
    {
      switch (value)
      {
        case byte _:
        case sbyte _:
        case short _:
        case ushort _:
        case int _:
        case uint _:
        case long _:
          return true;
        default:
          return value is ulong;
      }
    }

    private static bool IsSupportedFloat(object value)
    {
      switch (value)
      {
        case Decimal _:
        case float _:
          return true;
        default:
          return value is double;
      }
    }

    private static bool IsSupportedText(object value) => value is char || value is string;

    private static long GetInteger(object value)
    {
      switch (value)
      {
        case byte integer1:
          return (long) integer1;
        case sbyte integer2:
          return (long) integer2;
        case short integer3:
          return (long) integer3;
        case ushort integer4:
          return (long) integer4;
        case int integer5:
          return (long) integer5;
        case uint integer6:
          return (long) integer6;
        case long integer7:
          return integer7;
        case ulong num:
          if (num > (ulong) long.MaxValue)
            throw new SQLiteException("Unable to cast provided ulong value. Overflow ocurred: " + value.ToString());
          return (long) (ulong) value;
        default:
          throw new SQLiteException("Unable to cast provided value with unsupported Integer type: " + value.GetType().FullName);
      }
    }

    private static double GetFloat(object value)
    {
      switch (value)
      {
        case Decimal num1:
          return (double) num1;
        case float num2:
          return (double) num2;
        case double num3:
          return num3;
        default:
          throw new SQLiteException("Unable to cast provided value with unsupported Real type: " + value.GetType().FullName);
      }
    }
  }
}
