// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.MetaData
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Data.SqlTypes;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class MetaData : IDisposable
  {
    internal uint ordinal;
    internal uint size;
    internal object value;
    internal bool isReadOnly;
    internal bool isRowVersion;
    internal bool isExpression;
    internal bool isIdentity;
    internal bool isUnique;
    internal bool isKey;
    internal bool isNullable;
    internal bool hasDefault;
    internal string baseTableName;
    internal string baseColumnName;
    internal SqlCeType typeMap;
    private SqlMetaData sqlMetaData;

    public SqlMetaData SqlMetaData => this.sqlMetaData;

    public string ColumnName => this.sqlMetaData.Name;

    private MetaData()
    {
    }

    public MetaData(
      string name,
      SqlCeType typeMap,
      byte precision,
      byte scale,
      long maxLength,
      string databaseName,
      string schemaName)
    {
      this.typeMap = typeMap;
      SqlDbType sqlDbType = typeMap.sqlDbType;
      if (precision == (byte) 0)
        precision = typeMap.maxpre;
      if (0L == maxLength)
        maxLength = (long) typeMap.fixlen;
      if (scale == (byte) 0)
        scale = typeMap.scale;
      if (sqlDbType == SqlDbType.NText || sqlDbType == SqlDbType.Image)
        maxLength = -1L;
      if (sqlDbType == SqlDbType.NVarChar && maxLength > 4000L)
        maxLength = 4000L;
      else if (sqlDbType == SqlDbType.VarBinary && maxLength > 8000L)
        maxLength = 8000L;
      this.sqlMetaData = new SqlMetaData(name, sqlDbType, maxLength, precision, scale, 0L, SqlCompareOptions.None, (Type) null);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~MetaData() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      if (this.value != null && this.value is IntPtr && this.typeMap.isBLOB)
      {
        IntPtr ppUnknown = (IntPtr) this.value;
        if (IntPtr.Zero != ppUnknown)
        {
          NativeMethods.SafeRelease(ref ppUnknown);
          this.value = (object) null;
        }
      }
      if (!disposing)
        return;
      this.value = (object) null;
      this.sqlMetaData = (SqlMetaData) null;
      this.baseTableName = (string) null;
      this.baseColumnName = (string) null;
      this.typeMap = (SqlCeType) null;
    }
  }
}
