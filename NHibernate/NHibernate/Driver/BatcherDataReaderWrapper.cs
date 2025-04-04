// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.BatcherDataReaderWrapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class BatcherDataReaderWrapper : IDataReader, IDisposable, IDataRecord
  {
    private readonly IBatcher batcher;
    private readonly IDbCommand command;
    private readonly IDataReader reader;

    public BatcherDataReaderWrapper(IBatcher batcher, IDbCommand command)
    {
      if (batcher == null)
        throw new ArgumentNullException(nameof (batcher));
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      this.batcher = batcher;
      this.command = command;
      this.reader = batcher.ExecuteReader(command);
    }

    public void Dispose() => this.batcher.CloseCommand(this.command, this.reader);

    public string GetName(int i) => this.reader.GetName(i);

    public string GetDataTypeName(int i) => this.reader.GetDataTypeName(i);

    public Type GetFieldType(int i) => this.reader.GetFieldType(i);

    public object GetValue(int i) => this.reader.GetValue(i);

    public int GetValues(object[] values) => this.reader.GetValues(values);

    public int GetOrdinal(string name) => this.reader.GetOrdinal(name);

    public bool GetBoolean(int i) => this.reader.GetBoolean(i);

    public byte GetByte(int i) => this.reader.GetByte(i);

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      return this.reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
    }

    public char GetChar(int i) => this.reader.GetChar(i);

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      return this.reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
    }

    public Guid GetGuid(int i) => this.reader.GetGuid(i);

    public short GetInt16(int i) => this.reader.GetInt16(i);

    public int GetInt32(int i) => this.reader.GetInt32(i);

    public long GetInt64(int i) => this.reader.GetInt64(i);

    public float GetFloat(int i) => this.reader.GetFloat(i);

    public double GetDouble(int i) => this.reader.GetDouble(i);

    public string GetString(int i) => this.reader.GetString(i);

    public Decimal GetDecimal(int i) => this.reader.GetDecimal(i);

    public DateTime GetDateTime(int i) => this.reader.GetDateTime(i);

    public IDataReader GetData(int i) => this.reader.GetData(i);

    public bool IsDBNull(int i) => this.reader.IsDBNull(i);

    public int FieldCount => this.reader.FieldCount;

    public object this[int i] => this.reader[i];

    public object this[string name] => this.reader[name];

    public override bool Equals(object obj) => this.reader.Equals(obj);

    public override int GetHashCode() => this.reader.GetHashCode();

    public void Close() => this.batcher.CloseCommand(this.command, this.reader);

    public DataTable GetSchemaTable() => this.reader.GetSchemaTable();

    public bool NextResult() => this.reader.NextResult();

    public bool Read() => this.reader.Read();

    public int Depth => this.reader.Depth;

    public bool IsClosed => this.reader.IsClosed;

    public int RecordsAffected => this.reader.RecordsAffected;
  }
}
