// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCommandBuilder
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteCommandBuilder : DbCommandBuilder
  {
    private bool disposed;

    public SQLiteCommandBuilder()
      : this((SQLiteDataAdapter) null)
    {
    }

    public SQLiteCommandBuilder(SQLiteDataAdapter adp)
    {
      this.QuotePrefix = "[";
      this.QuoteSuffix = "]";
      this.DataAdapter = adp;
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteCommandBuilder).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = this.disposed ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    protected override void ApplyParameterInfo(
      DbParameter parameter,
      DataRow row,
      StatementType statementType,
      bool whereClause)
    {
      parameter.DbType = (DbType) row[SchemaTableColumn.ProviderType];
    }

    protected override string GetParameterName(string parameterName)
    {
      return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "@{0}", (object) parameterName);
    }

    protected override string GetParameterName(int parameterOrdinal)
    {
      return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "@param{0}", (object) parameterOrdinal);
    }

    protected override string GetParameterPlaceholder(int parameterOrdinal)
    {
      return this.GetParameterName(parameterOrdinal);
    }

    protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
    {
      if (adapter == base.DataAdapter)
        ((SQLiteDataAdapter) adapter).RowUpdating -= new EventHandler<RowUpdatingEventArgs>(this.RowUpdatingEventHandler);
      else
        ((SQLiteDataAdapter) adapter).RowUpdating += new EventHandler<RowUpdatingEventArgs>(this.RowUpdatingEventHandler);
    }

    private void RowUpdatingEventHandler(object sender, RowUpdatingEventArgs e)
    {
      this.RowUpdatingHandler(e);
    }

    public SQLiteDataAdapter DataAdapter
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteDataAdapter) base.DataAdapter;
      }
      set
      {
        this.CheckDisposed();
        this.DataAdapter = (DbDataAdapter) value;
      }
    }

    public SQLiteCommand GetDeleteCommand()
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetDeleteCommand();
    }

    public SQLiteCommand GetDeleteCommand(bool useColumnsForParameterNames)
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetDeleteCommand(useColumnsForParameterNames);
    }

    public SQLiteCommand GetUpdateCommand()
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetUpdateCommand();
    }

    public SQLiteCommand GetUpdateCommand(bool useColumnsForParameterNames)
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetUpdateCommand(useColumnsForParameterNames);
    }

    public SQLiteCommand GetInsertCommand()
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetInsertCommand();
    }

    public SQLiteCommand GetInsertCommand(bool useColumnsForParameterNames)
    {
      this.CheckDisposed();
      return (SQLiteCommand) base.GetInsertCommand(useColumnsForParameterNames);
    }

    [Browsable(false)]
    public override CatalogLocation CatalogLocation
    {
      get
      {
        this.CheckDisposed();
        return base.CatalogLocation;
      }
      set
      {
        this.CheckDisposed();
        base.CatalogLocation = value;
      }
    }

    [Browsable(false)]
    public override string CatalogSeparator
    {
      get
      {
        this.CheckDisposed();
        return base.CatalogSeparator;
      }
      set
      {
        this.CheckDisposed();
        base.CatalogSeparator = value;
      }
    }

    [DefaultValue("[")]
    [Browsable(false)]
    public override string QuotePrefix
    {
      get
      {
        this.CheckDisposed();
        return base.QuotePrefix;
      }
      set
      {
        this.CheckDisposed();
        base.QuotePrefix = value;
      }
    }

    [Browsable(false)]
    public override string QuoteSuffix
    {
      get
      {
        this.CheckDisposed();
        return base.QuoteSuffix;
      }
      set
      {
        this.CheckDisposed();
        base.QuoteSuffix = value;
      }
    }

    public override string QuoteIdentifier(string unquotedIdentifier)
    {
      this.CheckDisposed();
      return string.IsNullOrEmpty(this.QuotePrefix) || string.IsNullOrEmpty(this.QuoteSuffix) || string.IsNullOrEmpty(unquotedIdentifier) ? unquotedIdentifier : this.QuotePrefix + unquotedIdentifier.Replace(this.QuoteSuffix, this.QuoteSuffix + this.QuoteSuffix) + this.QuoteSuffix;
    }

    public override string UnquoteIdentifier(string quotedIdentifier)
    {
      this.CheckDisposed();
      return string.IsNullOrEmpty(this.QuotePrefix) || string.IsNullOrEmpty(this.QuoteSuffix) || string.IsNullOrEmpty(quotedIdentifier) || !quotedIdentifier.StartsWith(this.QuotePrefix, StringComparison.OrdinalIgnoreCase) || !quotedIdentifier.EndsWith(this.QuoteSuffix, StringComparison.OrdinalIgnoreCase) ? quotedIdentifier : quotedIdentifier.Substring(this.QuotePrefix.Length, quotedIdentifier.Length - (this.QuotePrefix.Length + this.QuoteSuffix.Length)).Replace(this.QuoteSuffix + this.QuoteSuffix, this.QuoteSuffix);
    }

    [Browsable(false)]
    public override string SchemaSeparator
    {
      get
      {
        this.CheckDisposed();
        return base.SchemaSeparator;
      }
      set
      {
        this.CheckDisposed();
        base.SchemaSeparator = value;
      }
    }

    protected override DataTable GetSchemaTable(DbCommand sourceCommand)
    {
      using (IDataReader dataReader = (IDataReader) sourceCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
      {
        DataTable schemaTable = dataReader.GetSchemaTable();
        if (this.HasSchemaPrimaryKey(schemaTable))
          this.ResetIsUniqueSchemaColumn(schemaTable);
        return schemaTable;
      }
    }

    private bool HasSchemaPrimaryKey(DataTable schema)
    {
      DataColumn column = schema.Columns[SchemaTableColumn.IsKey];
      foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
      {
        if ((bool) row[column])
          return true;
      }
      return false;
    }

    private void ResetIsUniqueSchemaColumn(DataTable schema)
    {
      DataColumn column1 = schema.Columns[SchemaTableColumn.IsUnique];
      DataColumn column2 = schema.Columns[SchemaTableColumn.IsKey];
      foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
      {
        if (!(bool) row[column2])
          row[column1] = (object) false;
      }
      schema.AcceptChanges();
    }
  }
}
