// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeCommandBuilder
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Data.Common;
using System.Globalization;
using System.Text;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeCommandBuilder : DbCommandBuilder
  {
    private SqlCeRowUpdatingEventHandler _rowUpdatingHandler;

    public SqlCeCommandBuilder()
    {
      GC.SuppressFinalize((object) this);
      base.QuotePrefix = "[";
      base.QuoteSuffix = "]";
    }

    public SqlCeCommandBuilder(SqlCeDataAdapter adapter)
      : this()
    {
      this.DataAdapter = adapter;
    }

    public override ConflictOption ConflictOption
    {
      get => base.ConflictOption;
      set
      {
        base.ConflictOption = value != ConflictOption.CompareAllSearchableValues ? value : throw new NotSupportedException();
      }
    }

    public override CatalogLocation CatalogLocation
    {
      get => CatalogLocation.Start;
      set
      {
        if (CatalogLocation.Start != value)
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (CatalogLocation)));
      }
    }

    public override string CatalogSeparator
    {
      get => ".";
      set
      {
        if ("." != value)
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (CatalogSeparator)));
      }
    }

    public SqlCeDataAdapter DataAdapter
    {
      get => (SqlCeDataAdapter) base.DataAdapter;
      set
      {
        SqlCeDataAdapter dataAdapter = (SqlCeDataAdapter) base.DataAdapter;
        if (dataAdapter == value)
          return;
        if (dataAdapter != null)
        {
          dataAdapter.RowUpdating -= this._rowUpdatingHandler;
          this._rowUpdatingHandler = (SqlCeRowUpdatingEventHandler) null;
        }
        this.DataAdapter = (DbDataAdapter) value;
      }
    }

    public override string QuotePrefix
    {
      get => base.QuotePrefix;
      set
      {
        base.QuotePrefix = !("[" != value) || !("\"" != value) || value.Length == 0 ? value : throw new ArgumentException(Res.GetString("ADP_InvalidQuotePrefix"), string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (QuotePrefix)));
      }
    }

    public override string QuoteSuffix
    {
      get => base.QuoteSuffix;
      set
      {
        base.QuoteSuffix = !("]" != value) || !("\"" != value) || value.Length == 0 ? value : throw new ArgumentException(Res.GetString("ADP_InvalidQuoteSuffix"), string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (QuoteSuffix)));
      }
    }

    public override string SchemaSeparator
    {
      get => ".";
      set
      {
        if ("." != value)
          throw new ArgumentException(Res.GetString("ADP_InvalidSchemaSeparator"), string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (SchemaSeparator)));
      }
    }

    private void SqlCeRowUpdatingHandler(object sender, SqlCeRowUpdatingEventArgs ruevent)
    {
      this.RowUpdatingHandler((RowUpdatingEventArgs) ruevent);
    }

    public SqlCeCommand GetInsertCommand() => (SqlCeCommand) base.GetInsertCommand();

    public SqlCeCommand GetUpdateCommand() => (SqlCeCommand) base.GetUpdateCommand();

    public SqlCeCommand GetDeleteCommand() => (SqlCeCommand) base.GetDeleteCommand();

    protected override void ApplyParameterInfo(
      DbParameter param,
      DataRow row,
      StatementType statementType,
      bool whereClause)
    {
      SqlCeParameter sqlCeParameter = (SqlCeParameter) param;
      sqlCeParameter.SqlDbType = ((SqlCeType) row["ProviderType"]).SqlDbType;
      sqlCeParameter.IsNullable = (bool) row["AllowDBNull"];
      if (DBNull.Value != row["NumericPrecision"])
        sqlCeParameter.Precision = (byte) (short) row["NumericPrecision"];
      if (DBNull.Value != row["NumericScale"])
        sqlCeParameter.Scale = (byte) (short) row["NumericScale"];
      sqlCeParameter.Size = 0;
    }

    protected override string GetParameterName(int parameterOrdinal)
    {
      return "@p" + parameterOrdinal.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    protected override string GetParameterName(string parameterName)
    {
      throw new NotSupportedException();
    }

    protected override string GetParameterPlaceholder(int parameterOrdinal)
    {
      return "@p" + parameterOrdinal.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
    {
      SqlCeDataAdapter sqlCeDataAdapter = (SqlCeDataAdapter) adapter;
      if (sqlCeDataAdapter == null)
        return;
      this._rowUpdatingHandler = new SqlCeRowUpdatingEventHandler(this.SqlCeRowUpdatingHandler);
      sqlCeDataAdapter.RowUpdating += this._rowUpdatingHandler;
    }

    private void ConsistentQuoteDelimiters(string quotePrefix, string quoteSuffix)
    {
      if ("\"" == quotePrefix && "\"" != quoteSuffix || "[" == quotePrefix && "]" != quoteSuffix)
        throw new ArgumentException(Res.GetString("ADP_InvalidPrefixSuffix", (object) quotePrefix, (object) quoteSuffix));
    }

    private static string BuildQuotedString(
      string quotePrefix,
      string quoteSuffix,
      string unQuotedString)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!ADP.IsEmpty(quotePrefix))
        stringBuilder.Append(quotePrefix);
      if (!ADP.IsEmpty(quoteSuffix))
      {
        stringBuilder.Append(unQuotedString.Replace(quoteSuffix, quoteSuffix + quoteSuffix));
        stringBuilder.Append(quoteSuffix);
      }
      else
        stringBuilder.Append(unQuotedString);
      return stringBuilder.ToString();
    }

    private static bool RemoveStringQuotes(
      string quotePrefix,
      string quoteSuffix,
      string quotedString,
      out string unquotedString)
    {
      int length1 = quotePrefix != null ? quotePrefix.Length : 0;
      int length2 = quoteSuffix != null ? quoteSuffix.Length : 0;
      if (length2 + length1 == 0)
      {
        unquotedString = quotedString;
        return true;
      }
      if (quotedString == null)
      {
        unquotedString = quotedString;
        return false;
      }
      int length3 = quotedString.Length;
      if (length3 < length1 + length2)
      {
        unquotedString = quotedString;
        return false;
      }
      if (length1 > 0 && !quotedString.StartsWith(quotePrefix, StringComparison.Ordinal))
      {
        unquotedString = quotedString;
        return false;
      }
      if (length2 > 0)
      {
        if (!quotedString.EndsWith(quoteSuffix, StringComparison.Ordinal))
        {
          unquotedString = quotedString;
          return false;
        }
        unquotedString = quotedString.Substring(length1, length3 - (length1 + length2)).Replace(quoteSuffix + quoteSuffix, quoteSuffix);
      }
      else
        unquotedString = quotedString.Substring(length1, length3 - length1);
      return true;
    }

    public override string QuoteIdentifier(string unquotedIdentifier)
    {
      if (unquotedIdentifier == null)
        throw new ArgumentNullException(nameof (unquotedIdentifier));
      string quoteSuffix = this.QuoteSuffix;
      string quotePrefix = this.QuotePrefix;
      this.ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
      return SqlCeCommandBuilder.BuildQuotedString(quotePrefix, quoteSuffix, unquotedIdentifier);
    }

    public override string UnquoteIdentifier(string quotedIdentifier)
    {
      if (quotedIdentifier == null)
        throw new ArgumentNullException(nameof (quotedIdentifier));
      string quoteSuffix = this.QuoteSuffix;
      string quotePrefix = this.QuotePrefix;
      this.ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
      string unquotedString;
      SqlCeCommandBuilder.RemoveStringQuotes(quotePrefix, quoteSuffix, quotedIdentifier, out unquotedString);
      return unquotedString;
    }
  }
}
