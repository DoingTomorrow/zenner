// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.ResultSetWrapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.AdoNet
{
  public class ResultSetWrapper : IDataReader, IDisposable, IDataRecord
  {
    private readonly IDataReader rs;
    private readonly ColumnNameCache columnNameCache;
    private bool disposed;

    public ResultSetWrapper(IDataReader resultSet, ColumnNameCache columnNameCache)
    {
      this.rs = resultSet;
      this.columnNameCache = columnNameCache;
    }

    internal IDataReader Target => this.rs;

    public void Close() => this.rs.Close();

    public DataTable GetSchemaTable() => this.rs.GetSchemaTable();

    public bool NextResult() => this.rs.NextResult();

    public bool Read() => this.rs.Read();

    public int Depth => this.rs.Depth;

    public bool IsClosed => this.rs.IsClosed;

    public int RecordsAffected => this.rs.RecordsAffected;

    ~ResultSetWrapper() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing && this.rs != null)
      {
        if (!this.rs.IsClosed)
          this.rs.Close();
        this.rs.Dispose();
      }
      this.disposed = true;
    }

    public string GetName(int i) => this.rs.GetName(i);

    public string GetDataTypeName(int i) => this.rs.GetDataTypeName(i);

    public Type GetFieldType(int i) => this.rs.GetFieldType(i);

    public object GetValue(int i) => (object) this.rs.GetDecimal(i);

    public int GetValues(object[] values) => this.rs.GetValues(values);

    public int GetOrdinal(string name) => this.columnNameCache.GetIndexForColumnName(name, this);

    public bool GetBoolean(int i) => this.rs.GetBoolean(i);

    public byte GetByte(int i) => this.rs.GetByte(i);

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      return this.rs.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
    }

    public char GetChar(int i) => this.rs.GetChar(i);

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      return this.rs.GetChars(i, fieldoffset, buffer, bufferoffset, length);
    }

    public Guid GetGuid(int i) => this.rs.GetGuid(i);

    public short GetInt16(int i) => this.rs.GetInt16(i);

    public int GetInt32(int i) => this.rs.GetInt32(i);

    public long GetInt64(int i) => this.rs.GetInt64(i);

    public float GetFloat(int i) => this.rs.GetFloat(i);

    public double GetDouble(int i) => this.rs.GetDouble(i);

    public string GetString(int i) => this.rs.GetString(i);

    public Decimal GetDecimal(int i) => this.rs.GetDecimal(i);

    public DateTime GetDateTime(int i) => this.rs.GetDateTime(i);

    public IDataReader GetData(int i) => this.rs.GetData(i);

    public bool IsDBNull(int i) => this.rs.IsDBNull(i);

    public int FieldCount => this.rs.FieldCount;

    public object this[int i] => this.rs[i];

    public object this[string name]
    {
      get => this.rs[this.columnNameCache.GetIndexForColumnName(name, this)];
    }

    public override bool Equals(object obj) => this.rs.Equals(obj);

    public override int GetHashCode() => this.rs.GetHashCode();
  }
}
