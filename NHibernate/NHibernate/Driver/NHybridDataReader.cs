// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.NHybridDataReader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class NHybridDataReader : IDataReader, IDisposable, IDataRecord
  {
    private IInternalLogger log = LoggerProvider.LoggerFor(typeof (NHybridDataReader));
    private IDataReader _reader;
    private bool _isMidstream;
    private bool _isAlreadyDisposed;

    public IDataReader Target => this._reader;

    public NHybridDataReader(IDataReader reader)
      : this(reader, false)
    {
    }

    public NHybridDataReader(IDataReader reader, bool inMemory)
    {
      if (inMemory)
        this._reader = (IDataReader) new NDataReader(reader, false);
      else
        this._reader = reader;
    }

    public void ReadIntoMemory()
    {
      if (this._reader.IsClosed || this._reader.GetType() == typeof (NDataReader))
        return;
      if (this.log.IsDebugEnabled)
        this.log.Debug((object) ("Moving IDataReader into an NDataReader.  It was converted in midstream " + this._isMidstream.ToString()));
      this._reader = (IDataReader) new NDataReader(this._reader, this._isMidstream);
    }

    public bool IsMidstream => this._isMidstream;

    public int RecordsAffected => this._reader.RecordsAffected;

    public bool IsClosed => this._reader.IsClosed;

    public bool NextResult()
    {
      this._isMidstream = false;
      return this._reader.NextResult();
    }

    public void Close() => this._reader.Close();

    public bool Read()
    {
      this._isMidstream = this._reader.Read();
      return this._isMidstream;
    }

    public int Depth => this._reader.Depth;

    public DataTable GetSchemaTable() => this._reader.GetSchemaTable();

    ~NHybridDataReader() => this.Dispose(false);

    public void Dispose()
    {
      this.log.Debug((object) "running NHybridDataReader.Dispose()");
      this.Dispose(true);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (this._isAlreadyDisposed)
        return;
      if (isDisposing)
        this._reader.Dispose();
      this._isAlreadyDisposed = true;
      GC.SuppressFinalize((object) this);
    }

    public int GetInt32(int i) => this._reader.GetInt32(i);

    public object this[string name] => this._reader[name];

    object IDataRecord.this[int i] => this._reader[i];

    public object GetValue(int i) => this._reader.GetValue(i);

    public bool IsDBNull(int i) => this._reader.IsDBNull(i);

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      return this._reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
    }

    public byte GetByte(int i) => this._reader.GetByte(i);

    public Type GetFieldType(int i) => this._reader.GetFieldType(i);

    public Decimal GetDecimal(int i) => this._reader.GetDecimal(i);

    public int GetValues(object[] values) => this._reader.GetValues(values);

    public string GetName(int i) => this._reader.GetName(i);

    public int FieldCount => this._reader.FieldCount;

    public long GetInt64(int i) => this._reader.GetInt64(i);

    public double GetDouble(int i) => this._reader.GetDouble(i);

    public bool GetBoolean(int i) => this._reader.GetBoolean(i);

    public Guid GetGuid(int i) => this._reader.GetGuid(i);

    public DateTime GetDateTime(int i) => this._reader.GetDateTime(i);

    public int GetOrdinal(string name) => this._reader.GetOrdinal(name);

    public string GetDataTypeName(int i) => this._reader.GetDataTypeName(i);

    public float GetFloat(int i) => this._reader.GetFloat(i);

    public IDataReader GetData(int i) => this._reader.GetData(i);

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      return this._reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
    }

    public string GetString(int i) => this._reader.GetString(i);

    public char GetChar(int i) => this._reader.GetChar(i);

    public short GetInt16(int i) => this._reader.GetInt16(i);
  }
}
