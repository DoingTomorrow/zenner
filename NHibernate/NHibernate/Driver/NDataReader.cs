// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.NDataReader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class NDataReader : IDataReader, IDisposable, IDataRecord
  {
    private NDataReader.NResult[] results;
    private bool isClosed;
    private int currentRowIndex = -1;
    private int currentResultIndex;
    private byte[] cachedByteArray;
    private char[] cachedCharArray;
    private int cachedColIndex = -1;

    public NDataReader(IDataReader reader, bool isMidstream)
    {
      ArrayList arrayList = new ArrayList(2);
      try
      {
        if (isMidstream)
          this.currentRowIndex = 0;
        arrayList.Add((object) new NDataReader.NResult(reader, isMidstream));
        while (reader.NextResult())
          arrayList.Add((object) new NDataReader.NResult(reader, false));
        this.results = (NDataReader.NResult[]) arrayList.ToArray(typeof (NDataReader.NResult));
      }
      catch (Exception ex)
      {
        throw new ADOException("There was a problem converting an IDataReader to NDataReader", ex);
      }
      finally
      {
        reader.Close();
      }
    }

    private void ClearCache()
    {
      this.cachedByteArray = (byte[]) null;
      this.cachedCharArray = (char[]) null;
      this.cachedColIndex = -1;
    }

    private NDataReader.NResult GetCurrentResult() => this.results[this.currentResultIndex];

    private object GetValue(string name)
    {
      return this.GetCurrentResult().GetValue(this.currentRowIndex, name);
    }

    public int RecordsAffected
    {
      get
      {
        throw new NotImplementedException("NDataReader should only be used for SELECT statements!");
      }
    }

    public bool IsClosed => this.isClosed;

    public bool NextResult()
    {
      ++this.currentResultIndex;
      this.currentRowIndex = -1;
      if (this.currentResultIndex >= this.results.Length)
      {
        --this.currentResultIndex;
        return false;
      }
      this.ClearCache();
      return true;
    }

    public void Close() => this.isClosed = true;

    public bool Read()
    {
      ++this.currentRowIndex;
      if (this.currentRowIndex >= this.results[this.currentResultIndex].RowCount)
      {
        --this.currentRowIndex;
        return false;
      }
      this.ClearCache();
      return true;
    }

    public int Depth => this.currentResultIndex;

    public DataTable GetSchemaTable() => this.GetCurrentResult().GetSchemaTable();

    public void Dispose()
    {
      this.isClosed = true;
      this.ClearCache();
      this.results = (NDataReader.NResult[]) null;
    }

    public int GetInt32(int i) => Convert.ToInt32(this.GetValue(i));

    public object this[string name] => this.GetValue(name);

    public object this[int i] => this.GetValue(i);

    public object GetValue(int i) => this.GetCurrentResult().GetValue(this.currentRowIndex, i);

    public bool IsDBNull(int i) => this.GetValue(i).Equals((object) DBNull.Value);

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
    {
      if (this.cachedByteArray == null || this.cachedColIndex != i)
      {
        this.cachedColIndex = i;
        this.cachedByteArray = (byte[]) this.GetValue(i);
      }
      long bytes = (long) this.cachedByteArray.Length - fieldOffset;
      if (buffer == null)
        return bytes;
      if (bytes < (long) length)
        length = (int) bytes;
      Array.Copy((Array) this.cachedByteArray, fieldOffset, (Array) buffer, (long) bufferOffset, (long) length);
      return (long) length;
    }

    public byte GetByte(int i) => Convert.ToByte(this.GetValue(i));

    public Type GetFieldType(int i) => this.GetCurrentResult().GetFieldType(i);

    public Decimal GetDecimal(int i) => Convert.ToDecimal(this.GetValue(i));

    public int GetValues(object[] values)
    {
      return this.GetCurrentResult().GetValues(this.currentRowIndex, values);
    }

    public string GetName(int i) => this.GetCurrentResult().GetName(i);

    public int FieldCount => this.GetCurrentResult().GetFieldCount();

    public long GetInt64(int i) => Convert.ToInt64(this.GetValue(i));

    public double GetDouble(int i) => Convert.ToDouble(this.GetValue(i));

    public bool GetBoolean(int i) => Convert.ToBoolean(this.GetValue(i));

    public Guid GetGuid(int i) => (Guid) this.GetValue(i);

    public DateTime GetDateTime(int i) => Convert.ToDateTime(this.GetValue(i));

    public int GetOrdinal(string name) => this.GetCurrentResult().GetOrdinal(name);

    public string GetDataTypeName(int i) => this.GetCurrentResult().GetDataTypeName(i);

    public float GetFloat(int i) => Convert.ToSingle(this.GetValue(i));

    public IDataReader GetData(int i)
    {
      throw new NotImplementedException("GetData(int) has not been implemented.");
    }

    public long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
    {
      if (this.cachedCharArray == null || this.cachedColIndex != i)
      {
        this.cachedColIndex = i;
        this.cachedCharArray = (char[]) this.GetValue(i);
      }
      long num = (long) this.cachedCharArray.Length - fieldOffset;
      if (num < (long) length)
        length = (int) num;
      Array.Copy((Array) this.cachedCharArray, fieldOffset, (Array) buffer, (long) bufferOffset, (long) length);
      return (long) length;
    }

    public string GetString(int i) => Convert.ToString(this.GetValue(i));

    public char GetChar(int i) => Convert.ToChar(this.GetValue(i));

    public short GetInt16(int i) => Convert.ToInt16(this.GetValue(i));

    private class NResult
    {
      private readonly object[][] records;
      private int colCount;
      private readonly DataTable schemaTable;
      private readonly IDictionary<string, int> fieldNameToIndex = (IDictionary<string, int>) new Dictionary<string, int>();
      private readonly IList<string> fieldIndexToName = (IList<string>) new List<string>();
      private readonly IList<Type> fieldTypes = (IList<Type>) new List<Type>();
      private readonly IList<string> fieldDataTypeNames = (IList<string>) new List<string>();

      internal NResult(IDataReader reader, bool isMidstream)
      {
        this.schemaTable = reader.GetSchemaTable();
        List<object[]> objArrayList = new List<object[]>();
        int num = 0;
        for (; isMidstream || reader.Read(); isMidstream = false)
        {
          if (num == 0)
          {
            for (int i = 0; i < reader.FieldCount; ++i)
            {
              string name = reader.GetName(i);
              this.fieldNameToIndex[name] = i;
              this.fieldIndexToName.Add(name);
              this.fieldTypes.Add(reader.GetFieldType(i));
              this.fieldDataTypeNames.Add(reader.GetDataTypeName(i));
            }
            this.colCount = reader.FieldCount;
          }
          ++num;
          object[] values = new object[reader.FieldCount];
          reader.GetValues(values);
          objArrayList.Add(values);
        }
        this.records = objArrayList.ToArray();
      }

      public string GetDataTypeName(int colIndex) => this.fieldDataTypeNames[colIndex];

      public int GetFieldCount() => this.fieldIndexToName.Count;

      public Type GetFieldType(int colIndex) => this.fieldTypes[colIndex];

      public string GetName(int colIndex) => this.fieldIndexToName[colIndex];

      public DataTable GetSchemaTable() => this.schemaTable;

      public int GetOrdinal(string colName)
      {
        if (this.fieldNameToIndex.ContainsKey(colName))
          return this.fieldNameToIndex[colName];
        foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) this.fieldNameToIndex)
        {
          if (StringHelper.EqualsCaseInsensitive(keyValuePair.Key, colName))
            return keyValuePair.Value;
        }
        throw new IndexOutOfRangeException(string.Format("No column with the specified name was found: {0}.", (object) colName));
      }

      public object GetValue(int rowIndex, int colIndex) => this.records[rowIndex][colIndex];

      public object GetValue(int rowIndex, string colName)
      {
        return this.GetValue(rowIndex, this.GetOrdinal(colName));
      }

      public int GetValues(int rowIndex, object[] values)
      {
        Array.Copy((Array) this.records[rowIndex], 0, (Array) values, 0, this.colCount);
        return this.colCount;
      }

      public int RowCount => this.records.Length;
    }
  }
}
