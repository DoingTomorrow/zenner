// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSql2000Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Dialect
{
  public class MsSql2000Dialect : NHibernate.Dialect.Dialect
  {
    public MsSql2000Dialect()
    {
      this.RegisterCharacterTypeMappings();
      this.RegisterNumericTypeMappings();
      this.RegisterDateTimeTypeMappings();
      this.RegisterLargeObjectTypeMappings();
      this.RegisterGuidTypeMapping();
      this.RegisterFunctions();
      this.RegisterKeywords();
      this.RegisterDefaultProperties();
    }

    protected virtual void RegisterDefaultProperties()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver";
      this.DefaultProperties["adonet.batch_size"] = "20";
      this.DefaultProperties["query.substitutions"] = "true 1, false 0, yes 'Y', no 'N'";
    }

    protected virtual void RegisterKeywords()
    {
      this.RegisterKeyword("top");
      this.RegisterKeyword("int");
      this.RegisterKeyword("integer");
      this.RegisterKeyword("tinyint");
      this.RegisterKeyword("smallint");
      this.RegisterKeyword("bigint");
      this.RegisterKeyword("numeric");
      this.RegisterKeyword("decimal");
      this.RegisterKeyword("bit");
      this.RegisterKeyword("money");
      this.RegisterKeyword("smallmoney");
      this.RegisterKeyword("float");
      this.RegisterKeyword("real");
      this.RegisterKeyword("datetime");
      this.RegisterKeyword("smalldatetime");
      this.RegisterKeyword("char");
      this.RegisterKeyword("varchar");
      this.RegisterKeyword("text");
      this.RegisterKeyword("nchar");
      this.RegisterKeyword("nvarchar");
      this.RegisterKeyword("ntext");
      this.RegisterKeyword("binary");
      this.RegisterKeyword("varbinary");
      this.RegisterKeyword("image");
      this.RegisterKeyword("cursor");
      this.RegisterKeyword("timestamp");
      this.RegisterKeyword("uniqueidentifier");
      this.RegisterKeyword("sql_variant");
      this.RegisterKeyword("table");
    }

    protected virtual void RegisterFunctions()
    {
      this.RegisterFunction("count", (ISQLFunction) new MsSql2000Dialect.CountBigQueryFunction());
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs"));
      this.RegisterFunction("absval", (ISQLFunction) new StandardSQLFunction("absval"));
      this.RegisterFunction("sign", (ISQLFunction) new StandardSQLFunction("sign", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("ceiling", (ISQLFunction) new StandardSQLFunction("ceiling"));
      this.RegisterFunction("ceil", (ISQLFunction) new StandardSQLFunction("ceil"));
      this.RegisterFunction("floor", (ISQLFunction) new StandardSQLFunction("floor"));
      this.RegisterFunction("round", (ISQLFunction) new StandardSQLFunction("round"));
      this.RegisterFunction("acos", (ISQLFunction) new StandardSQLFunction("acos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("asin", (ISQLFunction) new StandardSQLFunction("asin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan", (ISQLFunction) new StandardSQLFunction("atan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cos", (ISQLFunction) new StandardSQLFunction("cos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cot", (ISQLFunction) new StandardSQLFunction("cot", (IType) NHibernateUtil.Double));
      this.RegisterFunction("degrees", (ISQLFunction) new StandardSQLFunction("degrees", (IType) NHibernateUtil.Double));
      this.RegisterFunction("exp", (ISQLFunction) new StandardSQLFunction("exp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("float", (ISQLFunction) new StandardSQLFunction("float", (IType) NHibernateUtil.Double));
      this.RegisterFunction("hex", (ISQLFunction) new StandardSQLFunction("hex", (IType) NHibernateUtil.String));
      this.RegisterFunction("ln", (ISQLFunction) new StandardSQLFunction("ln", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log", (ISQLFunction) new StandardSQLFunction("log", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log10", (ISQLFunction) new StandardSQLFunction("log10", (IType) NHibernateUtil.Double));
      this.RegisterFunction("mod", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "((?1) % (?2))"));
      this.RegisterFunction("radians", (ISQLFunction) new StandardSQLFunction("radians", (IType) NHibernateUtil.Double));
      this.RegisterFunction("rand", (ISQLFunction) new NoArgSQLFunction("rand", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sin", (ISQLFunction) new StandardSQLFunction("sin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("soundex", (ISQLFunction) new StandardSQLFunction("soundex", (IType) NHibernateUtil.String));
      this.RegisterFunction("sqrt", (ISQLFunction) new StandardSQLFunction("sqrt", (IType) NHibernateUtil.Double));
      this.RegisterFunction("stddev", (ISQLFunction) new StandardSQLFunction("stddev", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tan", (ISQLFunction) new StandardSQLFunction("tan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("variance", (ISQLFunction) new StandardSQLFunction("variance", (IType) NHibernateUtil.Double));
      this.RegisterFunction("left", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "left(?1, ?2)"));
      this.RegisterFunction("right", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "right(?1, ?2)"));
      this.RegisterFunction("locate", (ISQLFunction) new StandardSQLFunction("charindex", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.DateTime, true));
      this.RegisterFunction("second", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(second, ?1)"));
      this.RegisterFunction("minute", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(minute, ?1)"));
      this.RegisterFunction("hour", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(hour, ?1)"));
      this.RegisterFunction("day", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(day, ?1)"));
      this.RegisterFunction("month", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(month, ?1)"));
      this.RegisterFunction("year", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(year, ?1)"));
      this.RegisterFunction("date", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Date, "dateadd(dd, 0, datediff(dd, 0, ?1))"));
      this.RegisterFunction("concat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "(", "+", ")"));
      this.RegisterFunction("digits", (ISQLFunction) new StandardSQLFunction("digits", (IType) NHibernateUtil.String));
      this.RegisterFunction("chr", (ISQLFunction) new StandardSQLFunction("chr", (IType) NHibernateUtil.Character));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper"));
      this.RegisterFunction("ucase", (ISQLFunction) new StandardSQLFunction("ucase"));
      this.RegisterFunction("lcase", (ISQLFunction) new StandardSQLFunction("lcase"));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSQLFunction("lower"));
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("len", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("ltrim", (ISQLFunction) new StandardSQLFunction("ltrim"));
      this.RegisterFunction("trim", (ISQLFunction) new AnsiTrimEmulationFunction());
      this.RegisterFunction("iif", (ISQLFunction) new SQLFunctionTemplate((IType) null, "case when ?1 then ?2 else ?3 end"));
      this.RegisterFunction("replace", (ISQLFunction) new StandardSafeSQLFunction("replace", (IType) NHibernateUtil.String, 3));
      this.RegisterFunction("str", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "cast(?1 as nvarchar(50))"));
      this.RegisterFunction("substring", (ISQLFunction) new EmulatedLengthSubstringFunction());
      this.RegisterFunction("bit_length", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datalength(?1) * 8"));
      this.RegisterFunction("extract", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(?1, ?3)"));
    }

    protected virtual void RegisterGuidTypeMapping()
    {
      this.RegisterColumnType(DbType.Guid, "UNIQUEIDENTIFIER");
    }

    protected virtual void RegisterLargeObjectTypeMappings()
    {
      this.RegisterColumnType(DbType.Binary, "VARBINARY(8000)");
      this.RegisterColumnType(DbType.Binary, 8000, "VARBINARY($l)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "IMAGE");
    }

    protected virtual void RegisterDateTimeTypeMappings()
    {
      this.RegisterColumnType(DbType.Time, "DATETIME");
      this.RegisterColumnType(DbType.Date, "DATETIME");
      this.RegisterColumnType(DbType.DateTime, "DATETIME");
    }

    protected virtual void RegisterNumericTypeMappings()
    {
      this.RegisterColumnType(DbType.Boolean, "BIT");
      this.RegisterColumnType(DbType.Byte, "TINYINT");
      this.RegisterColumnType(DbType.Currency, "MONEY");
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "DECIMAL($p, $s)");
      this.RegisterColumnType(DbType.Double, "DOUBLE PRECISION");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INT");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "REAL");
    }

    protected virtual void RegisterCharacterTypeMappings()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(255)");
      this.RegisterColumnType(DbType.AnsiString, 8000, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.StringFixedLength, "NCHAR(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 4000, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, "NVARCHAR(255)");
      this.RegisterColumnType(DbType.String, 4000, "NVARCHAR($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "NTEXT");
    }

    public override string AddColumnString => "add";

    public override string NullColumnString => " null";

    public override string CurrentTimestampSQLFunctionName => "CURRENT_TIMESTAMP";

    public override string CurrentTimestampSelectString => "SELECT CURRENT_TIMESTAMP";

    public override bool IsCurrentTimestampSelectStringCallable => false;

    public override bool SupportsCurrentTimestampSelection => true;

    public override bool QualifyIndexName => false;

    public override string SelectGUIDString => "select newid()";

    public override string GetDropTableString(string tableName)
    {
      return string.Format("if exists (select * from dbo.sysobjects where id = object_id(N'{0}') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table {0}", (object) tableName);
    }

    public override string ForUpdateString => string.Empty;

    public override SqlString AppendIdentitySelectToInsert(SqlString insertSql)
    {
      return insertSql.Append("; " + this.IdentitySelectString);
    }

    public override bool SupportsInsertSelectIdentity => true;

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString => "select SCOPE_IDENTITY()";

    public override string IdentityColumnString => "IDENTITY NOT NULL";

    public override string NoColumnsInsertString => "DEFAULT VALUES";

    public override char CloseQuote => ']';

    public override char OpenQuote => '[';

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => false;

    public override bool SupportsVariableLimit => false;

    public override SqlString GetLimitString(
      SqlString querySqlString,
      SqlString offset,
      SqlString limit)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(" top ");
      sqlStringBuilder.Add(limit);
      return querySqlString.Insert(MsSql2000Dialect.GetAfterSelectInsertPoint(querySqlString), sqlStringBuilder.ToSqlString());
    }

    public override bool UseMaxForLimit => true;

    public override bool SupportsTemporaryTables => true;

    public override string GenerateTemporaryTableName(string baseTableName) => "#" + baseTableName;

    public override bool DropTemporaryTableAfterUse() => true;

    protected override string Quote(string name)
    {
      return this.OpenQuote.ToString() + name.Replace(this.CloseQuote.ToString(), new string(this.CloseQuote, 2)) + (object) this.CloseQuote;
    }

    public override string UnQuote(string quoted)
    {
      if (this.IsQuoted(quoted))
        quoted = quoted.Substring(1, quoted.Length - 2);
      return quoted.Replace(new string(this.CloseQuote, 2), this.CloseQuote.ToString());
    }

    private static int GetAfterSelectInsertPoint(SqlString sql)
    {
      if (sql.StartsWithCaseInsensitive("select distinct"))
        return 15;
      if (sql.StartsWithCaseInsensitive("select"))
        return 6;
      throw new NotSupportedException("The query should start with 'SELECT' or 'SELECT DISTINCT'");
    }

    protected bool NeedsLockHint(LockMode lockMode) => lockMode.GreaterThan(LockMode.Read);

    public override string AppendLockHint(LockMode lockMode, string tableName)
    {
      return this.NeedsLockHint(lockMode) ? tableName + " with (updlock, rowlock)" : tableName;
    }

    public override SqlString ApplyLocksToSql(
      SqlString sql,
      IDictionary<string, LockMode> aliasedLockModes,
      IDictionary<string, string[]> keyColumnNames)
    {
      bool flag = false;
      foreach (KeyValuePair<string, LockMode> aliasedLockMode in (IEnumerable<KeyValuePair<string, LockMode>>) aliasedLockModes)
      {
        if (this.NeedsLockHint(aliasedLockMode.Value))
        {
          flag = true;
          break;
        }
      }
      return !flag ? sql : new MsSql2000Dialect.LockHintAppender(this, aliasedLockModes).AppendLockHint(sql);
    }

    public override long TimestampResolutionInTicks => 100000;

    public override string GetIfExistsDropConstraint(Table table, string name)
    {
      return string.Format("if exists ({0})", (object) this.GetSelectExistingObject(name, table));
    }

    protected virtual string GetSelectExistingObject(string name, Table table)
    {
      return string.Format("select 1 from sysobjects where id = OBJECT_ID(N'{0}') AND parent_obj = OBJECT_ID('{1}')", (object) (table.GetQuotedSchemaName((NHibernate.Dialect.Dialect) this) + this.Quote(name)), (object) table.GetQuotedName((NHibernate.Dialect.Dialect) this));
    }

    public override string GetIfNotExistsCreateConstraint(Table table, string name)
    {
      return string.Format("if not exists ({0})", (object) this.GetSelectExistingObject(name, table));
    }

    public override bool SupportsCircularCascadeDeleteConstraints => false;

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new MsSqlDataBaseSchema(connection);
    }

    public override bool SupportsUnionAll => true;

    public override bool SupportsSqlBatches => true;

    public override bool IsKnownToken(string currentToken, string nextToken)
    {
      return currentToken == "n" && nextToken == "'";
    }

    [Serializable]
    protected class CountBigQueryFunction : ClassicAggregateFunction
    {
      public CountBigQueryFunction()
        : base("count_big", true)
      {
      }

      public override IType ReturnType(IType columnType, IMapping mapping)
      {
        return (IType) NHibernateUtil.Int64;
      }
    }

    public struct LockHintAppender
    {
      private const string UnescapedNameRegex = "\\w+";
      private const string EscapedNameRegex = "\\[([^\\]]|\\]\\])+\\]";
      private const string NameRegex = "(\\w+|\\[([^\\]]|\\]\\])+\\])";
      private const string NameSeparatorRegex = "\\s*\\.\\s*";
      private const string FromTableNameRegex = "from\\s+((\\w+|\\[([^\\]]|\\]\\])+\\])?\\s*\\.\\s*){0,2}(\\w+|\\[([^\\]]|\\]\\])+\\])";
      private static readonly Regex FromClauseTableNameRegex = new Regex("from\\s+((\\w+|\\[([^\\]]|\\]\\])+\\])?\\s*\\.\\s*){0,2}(\\w+|\\[([^\\]]|\\]\\])+\\])", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      private readonly MsSql2000Dialect _dialect;
      private readonly IDictionary<string, LockMode> _aliasedLockModes;
      private readonly Regex _matchRegex;
      private readonly Regex _unionSubclassRegex;

      public LockHintAppender(
        MsSql2000Dialect dialect,
        IDictionary<string, LockMode> aliasedLockModes)
      {
        this._dialect = dialect;
        this._aliasedLockModes = aliasedLockModes;
        string str = StringHelper.Join("|", (IEnumerable) aliasedLockModes.Keys);
        this._matchRegex = new Regex(" (" + str + ")([, ]|$)");
        this._unionSubclassRegex = new Regex("from\\s+\\(((?:.|\\r|\\n)*)\\)(?:\\s+as)?\\s+(?<alias>" + str + ")", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      }

      public SqlString AppendLockHint(SqlString sql)
      {
        SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
        foreach (object obj in sql)
        {
          if (obj == Parameter.Placeholder)
            sqlStringBuilder.Add((Parameter) obj);
          else
            sqlStringBuilder.Add(this.ProcessUnionSubclassCase((string) obj) ?? this._matchRegex.Replace((string) obj, new MatchEvaluator(this.ReplaceMatch)));
        }
        return sqlStringBuilder.ToSqlString();
      }

      private string ProcessUnionSubclassCase(string part)
      {
        Match match = this._unionSubclassRegex.Match(part);
        if (!match.Success)
          return (string) null;
        LockMode lockMode = this._aliasedLockModes[match.Groups["alias"].Value];
        MsSql2000Dialect.LockHintAppender @this = this;
        string replacement = MsSql2000Dialect.LockHintAppender.FromClauseTableNameRegex.Replace(match.Value, (MatchEvaluator) (m => @this._dialect.AppendLockHint(lockMode, m.Value)));
        return this._unionSubclassRegex.Replace(part, replacement);
      }

      private string ReplaceMatch(Match match)
      {
        string str = match.Groups[1].Value;
        return " " + this._dialect.AppendLockHint(this._aliasedLockModes[str], str) + match.Groups[2].Value;
      }
    }
  }
}
