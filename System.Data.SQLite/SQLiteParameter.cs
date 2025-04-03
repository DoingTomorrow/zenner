// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteParameter
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.ComponentModel;
using System.Data.Common;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteParameter : DbParameter, ICloneable
  {
    private const DbType UnknownDbType = ~DbType.AnsiString;
    private IDbCommand _command;
    internal DbType _dbType;
    private DataRowVersion _rowVersion;
    private object _objValue;
    private string _sourceColumn;
    private string _parameterName;
    private int _dataSize;
    private bool _nullable;
    private bool _nullMapping;
    private string _typeName;

    internal SQLiteParameter(IDbCommand command)
      : this()
    {
      this._command = command;
    }

    public SQLiteParameter()
      : this((string) null, ~DbType.AnsiString, 0, (string) null, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(string parameterName)
      : this(parameterName, ~DbType.AnsiString, 0, (string) null, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(string parameterName, object value)
      : this(parameterName, ~DbType.AnsiString, 0, (string) null, DataRowVersion.Current)
    {
      this.Value = value;
    }

    public SQLiteParameter(string parameterName, DbType dbType)
      : this(parameterName, dbType, 0, (string) null, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(string parameterName, DbType dbType, string sourceColumn)
      : this(parameterName, dbType, 0, sourceColumn, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(
      string parameterName,
      DbType dbType,
      string sourceColumn,
      DataRowVersion rowVersion)
      : this(parameterName, dbType, 0, sourceColumn, rowVersion)
    {
    }

    public SQLiteParameter(DbType dbType)
      : this((string) null, dbType, 0, (string) null, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(DbType dbType, object value)
      : this((string) null, dbType, 0, (string) null, DataRowVersion.Current)
    {
      this.Value = value;
    }

    public SQLiteParameter(DbType dbType, string sourceColumn)
      : this((string) null, dbType, 0, sourceColumn, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(DbType dbType, string sourceColumn, DataRowVersion rowVersion)
      : this((string) null, dbType, 0, sourceColumn, rowVersion)
    {
    }

    public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize)
      : this(parameterName, parameterType, parameterSize, (string) null, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      string sourceColumn)
      : this(parameterName, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      string sourceColumn,
      DataRowVersion rowVersion)
    {
      this._parameterName = parameterName;
      this._dbType = parameterType;
      this._sourceColumn = sourceColumn;
      this._rowVersion = rowVersion;
      this._dataSize = parameterSize;
      this._nullable = true;
    }

    private SQLiteParameter(SQLiteParameter source)
      : this(source.ParameterName, source._dbType, 0, source.Direction, source.IsNullable, (byte) 0, (byte) 0, source.SourceColumn, source.SourceVersion, source.Value)
    {
      this._nullMapping = source._nullMapping;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      ParameterDirection direction,
      bool isNullable,
      byte precision,
      byte scale,
      string sourceColumn,
      DataRowVersion rowVersion,
      object value)
      : this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
    {
      this.Direction = direction;
      this.IsNullable = isNullable;
      this.Value = value;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      ParameterDirection direction,
      byte precision,
      byte scale,
      string sourceColumn,
      DataRowVersion rowVersion,
      bool sourceColumnNullMapping,
      object value)
      : this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
    {
      this.Direction = direction;
      this.SourceColumnNullMapping = sourceColumnNullMapping;
      this.Value = value;
    }

    public SQLiteParameter(DbType parameterType, int parameterSize)
      : this((string) null, parameterType, parameterSize, (string) null, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(DbType parameterType, int parameterSize, string sourceColumn)
      : this((string) null, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
    {
    }

    public SQLiteParameter(
      DbType parameterType,
      int parameterSize,
      string sourceColumn,
      DataRowVersion rowVersion)
      : this((string) null, parameterType, parameterSize, sourceColumn, rowVersion)
    {
    }

    public IDbCommand Command
    {
      get => this._command;
      set => this._command = value;
    }

    public override bool IsNullable
    {
      get => this._nullable;
      set => this._nullable = value;
    }

    [DbProviderSpecificTypeProperty(true)]
    [RefreshProperties(RefreshProperties.All)]
    public override DbType DbType
    {
      get
      {
        if (this._dbType != ~DbType.AnsiString)
          return this._dbType;
        return this._objValue != null && this._objValue != DBNull.Value ? SQLiteConvert.TypeToDbType(this._objValue.GetType()) : DbType.String;
      }
      set => this._dbType = value;
    }

    public override ParameterDirection Direction
    {
      get => ParameterDirection.Input;
      set
      {
        if (value != ParameterDirection.Input)
          throw new NotSupportedException();
      }
    }

    public override string ParameterName
    {
      get => this._parameterName;
      set => this._parameterName = value;
    }

    public override void ResetDbType() => this._dbType = ~DbType.AnsiString;

    [DefaultValue(0)]
    public override int Size
    {
      get => this._dataSize;
      set => this._dataSize = value;
    }

    public override string SourceColumn
    {
      get => this._sourceColumn;
      set => this._sourceColumn = value;
    }

    public override bool SourceColumnNullMapping
    {
      get => this._nullMapping;
      set => this._nullMapping = value;
    }

    public override DataRowVersion SourceVersion
    {
      get => this._rowVersion;
      set => this._rowVersion = value;
    }

    [RefreshProperties(RefreshProperties.All)]
    [TypeConverter(typeof (StringConverter))]
    public override object Value
    {
      get => this._objValue;
      set
      {
        this._objValue = value;
        if (this._dbType != ~DbType.AnsiString || this._objValue == null || this._objValue == DBNull.Value)
          return;
        this._dbType = SQLiteConvert.TypeToDbType(this._objValue.GetType());
      }
    }

    public string TypeName
    {
      get => this._typeName;
      set => this._typeName = value;
    }

    public object Clone() => (object) new SQLiteParameter(this);
  }
}
