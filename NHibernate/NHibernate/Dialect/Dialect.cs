// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Dialect.Function;
using NHibernate.Dialect.Lock;
using NHibernate.Dialect.Schema;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public abstract class Dialect
  {
    protected const string DefaultBatchSize = "15";
    protected const string NoBatch = "0";
    public const string PossibleQuoteChars = "`'\"[";
    public const string PossibleClosedQuoteChars = "`'\"]";
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (NHibernate.Dialect.Dialect));
    private readonly TypeNames _typeNames = new TypeNames();
    private readonly TypeNames _hibernateTypeNames = new TypeNames();
    private readonly IDictionary<string, string> _properties = (IDictionary<string, string>) new Dictionary<string, string>();
    private readonly IDictionary<string, ISQLFunction> _sqlFunctions;
    private readonly HashedSet<string> _sqlKeywords = new HashedSet<string>();
    private static readonly IDictionary<string, ISQLFunction> StandardAggregateFunctions = CollectionHelper.CreateCaseInsensitiveHashtable<ISQLFunction>();
    private static readonly IViolatedConstraintNameExtracter Extracter;

    static Dialect()
    {
      NHibernate.Dialect.Dialect.StandardAggregateFunctions["count"] = (ISQLFunction) new CountQueryFunctionInfo();
      NHibernate.Dialect.Dialect.StandardAggregateFunctions["avg"] = (ISQLFunction) new AvgQueryFunctionInfo();
      NHibernate.Dialect.Dialect.StandardAggregateFunctions["max"] = (ISQLFunction) new ClassicAggregateFunction("max", false);
      NHibernate.Dialect.Dialect.StandardAggregateFunctions["min"] = (ISQLFunction) new ClassicAggregateFunction("min", false);
      NHibernate.Dialect.Dialect.StandardAggregateFunctions["sum"] = (ISQLFunction) new SumQueryFunctionInfo();
      NHibernate.Dialect.Dialect.Extracter = (IViolatedConstraintNameExtracter) new NoOpViolatedConstraintNameExtracter();
    }

    protected Dialect()
    {
      NHibernate.Dialect.Dialect.Log.Info((object) ("Using dialect: " + (object) this));
      this._sqlFunctions = CollectionHelper.CreateCaseInsensitiveHashtable<ISQLFunction>(NHibernate.Dialect.Dialect.StandardAggregateFunctions);
      this.RegisterFunction("substring", (ISQLFunction) new AnsiSubstringFunction());
      this.RegisterFunction("locate", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "locate(?1, ?2, ?3)"));
      this.RegisterFunction("trim", (ISQLFunction) new AnsiTrimFunction());
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("bit_length", (ISQLFunction) new StandardSQLFunction("bit_length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("coalesce", (ISQLFunction) new StandardSQLFunction("coalesce"));
      this.RegisterFunction("nullif", (ISQLFunction) new StandardSQLFunction("nullif"));
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs"));
      this.RegisterFunction("mod", (ISQLFunction) new StandardSQLFunction("mod", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("sqrt", (ISQLFunction) new StandardSQLFunction("sqrt", (IType) NHibernateUtil.Double));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper"));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSQLFunction("lower"));
      this.RegisterFunction("cast", (ISQLFunction) new CastFunction());
      this.RegisterFunction("extract", (ISQLFunction) new AnsiExtractFunction());
      this.RegisterFunction("concat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "(", "||", ")"));
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("current_timestamp", (IType) NHibernateUtil.DateTime, true));
      this.RegisterFunction("sysdate", (ISQLFunction) new NoArgSQLFunction("sysdate", (IType) NHibernateUtil.DateTime, false));
      this.RegisterFunction("second", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(second from ?1)"));
      this.RegisterFunction("minute", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(minute from ?1)"));
      this.RegisterFunction("hour", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(hour from ?1)"));
      this.RegisterFunction("day", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(day from ?1)"));
      this.RegisterFunction("month", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(month from ?1)"));
      this.RegisterFunction("year", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(year from ?1)"));
      this.RegisterFunction("str", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "cast(?1 as char)"));
      this.RegisterHibernateType(DbType.Int64, NHibernateUtil.Int64.Name);
      this.RegisterHibernateType(DbType.Binary, NHibernateUtil.Binary.Name);
      this.RegisterHibernateType(DbType.Boolean, NHibernateUtil.Boolean.Name);
      this.RegisterHibernateType(DbType.AnsiString, NHibernateUtil.Character.Name);
      this.RegisterHibernateType(DbType.Date, NHibernateUtil.Date.Name);
      this.RegisterHibernateType(DbType.Double, NHibernateUtil.Double.Name);
      this.RegisterHibernateType(DbType.Single, NHibernateUtil.Single.Name);
      this.RegisterHibernateType(DbType.Int32, NHibernateUtil.Int32.Name);
      this.RegisterHibernateType(DbType.Int16, NHibernateUtil.Int16.Name);
      this.RegisterHibernateType(DbType.SByte, NHibernateUtil.SByte.Name);
      this.RegisterHibernateType(DbType.Time, NHibernateUtil.Time.Name);
      this.RegisterHibernateType(DbType.DateTime, NHibernateUtil.Timestamp.Name);
      this.RegisterHibernateType(DbType.String, NHibernateUtil.String.Name);
      this.RegisterHibernateType(DbType.VarNumeric, NHibernateUtil.Decimal.Name);
      this.RegisterHibernateType(DbType.Decimal, NHibernateUtil.Decimal.Name);
    }

    public static NHibernate.Dialect.Dialect GetDialect()
    {
      string property;
      try
      {
        property = NHibernate.Cfg.Environment.Properties["dialect"];
      }
      catch (Exception ex)
      {
        throw new HibernateException("The dialect was not set. Set the property 'dialect'.", ex);
      }
      return NHibernate.Dialect.Dialect.InstantiateDialect(property);
    }

    public static NHibernate.Dialect.Dialect GetDialect(IDictionary<string, string> props)
    {
      if (props == null)
        throw new ArgumentNullException(nameof (props));
      string dialectName;
      if (!props.TryGetValue("dialect", out dialectName))
        throw new InvalidOperationException("Could not find the dialect in the configuration");
      return dialectName == null ? NHibernate.Dialect.Dialect.GetDialect() : NHibernate.Dialect.Dialect.InstantiateDialect(dialectName);
    }

    private static NHibernate.Dialect.Dialect InstantiateDialect(string dialectName)
    {
      try
      {
        return (NHibernate.Dialect.Dialect) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(dialectName));
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not instantiate dialect class " + dialectName, ex);
      }
    }

    public virtual string GetTypeName(SqlType sqlType)
    {
      if (sqlType.LengthDefined || sqlType.PrecisionDefined)
      {
        string typeName = this._typeNames.Get(sqlType.DbType, sqlType.Length, (int) sqlType.Precision, (int) sqlType.Scale);
        if (typeName != null)
          return typeName;
      }
      return this._typeNames.Get(sqlType.DbType) ?? throw new HibernateException(string.Format("No default type mapping for SqlType {0}", (object) sqlType));
    }

    public virtual string GetTypeName(SqlType sqlType, int length, int precision, int scale)
    {
      return this._typeNames.Get(sqlType.DbType, length, precision, scale) ?? throw new HibernateException(string.Format("No type mapping for SqlType {0} of length {1}", (object) sqlType, (object) length));
    }

    public virtual string GetLongestTypeName(DbType dbType) => this._typeNames.GetLongest(dbType);

    public virtual string GetCastTypeName(SqlType sqlType)
    {
      return this.GetTypeName(sqlType, (int) byte.MaxValue, 19, 2);
    }

    protected void RegisterColumnType(DbType code, int capacity, string name)
    {
      this._typeNames.Put(code, capacity, name);
    }

    protected void RegisterColumnType(DbType code, string name) => this._typeNames.Put(code, name);

    public virtual bool DropConstraints => true;

    public virtual bool QualifyIndexName => true;

    public virtual bool SupportsUnique => true;

    public virtual bool SupportsUniqueConstraintInCreateAlterTable => true;

    public virtual bool SupportsForeignKeyConstraintInAlterTable => true;

    public virtual string GetAddForeignKeyConstraintString(
      string constraintName,
      string[] foreignKey,
      string referencedTable,
      string[] primaryKey,
      bool referencesPrimaryKey)
    {
      StringBuilder stringBuilder = new StringBuilder(200);
      if (this.SupportsForeignKeyConstraintInAlterTable)
        stringBuilder.Append(" add");
      stringBuilder.Append(" constraint ").Append(constraintName).Append(" foreign key (").Append(StringHelper.Join(", ", (IEnumerable) foreignKey)).Append(") references ").Append(referencedTable);
      if (!referencesPrimaryKey)
        stringBuilder.Append(" (").Append(StringHelper.Join(", ", (IEnumerable) primaryKey)).Append(')');
      return stringBuilder.ToString();
    }

    public virtual string GetAddPrimaryKeyConstraintString(string constraintName)
    {
      return " add constraint " + constraintName + " primary key ";
    }

    public virtual bool HasSelfReferentialForeignKeyBug => false;

    public virtual bool SupportsCommentOn => false;

    public virtual string GetTableComment(string comment) => string.Empty;

    public virtual string GetColumnComment(string comment) => string.Empty;

    public virtual bool SupportsIfExistsBeforeTableName => false;

    public virtual bool SupportsIfExistsAfterTableName => false;

    public virtual bool SupportsColumnCheck => true;

    public virtual bool SupportsTableCheck => true;

    public virtual bool SupportsCascadeDelete => true;

    public virtual bool SupportsNotNullUnique => true;

    public virtual IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      throw new NotSupportedException();
    }

    public virtual ILockingStrategy GetLockingStrategy(ILockable lockable, LockMode lockMode)
    {
      return (ILockingStrategy) new SelectLockingStrategy(lockable, lockMode);
    }

    public virtual string GetForUpdateString(LockMode lockMode)
    {
      if (lockMode == LockMode.Upgrade)
        return this.ForUpdateString;
      return lockMode == LockMode.UpgradeNoWait || lockMode == LockMode.Force ? this.ForUpdateNowaitString : string.Empty;
    }

    public virtual string ForUpdateString => " for update";

    public virtual bool ForUpdateOfColumns => false;

    public virtual bool SupportsOuterJoinForUpdate => true;

    public virtual string GetForUpdateString(string aliases) => this.ForUpdateString;

    public virtual string ForUpdateNowaitString => this.ForUpdateString;

    public virtual string GetForUpdateNowaitString(string aliases)
    {
      return this.GetForUpdateString(aliases);
    }

    public virtual SqlString ApplyLocksToSql(
      SqlString sql,
      IDictionary<string, LockMode> aliasedLockModes,
      IDictionary<string, string[]> keyColumnNames)
    {
      return sql.Append(new ForUpdateFragment(this, aliasedLockModes, keyColumnNames).ToSqlStringFragment());
    }

    public virtual string AppendLockHint(LockMode lockMode, string tableName) => tableName;

    public virtual string GetDropTableString(string tableName)
    {
      StringBuilder stringBuilder = new StringBuilder("drop table ");
      if (this.SupportsIfExistsBeforeTableName)
        stringBuilder.Append("if exists ");
      stringBuilder.Append(tableName).Append(this.CascadeConstraintsString);
      if (this.SupportsIfExistsAfterTableName)
        stringBuilder.Append(" if exists");
      return stringBuilder.ToString();
    }

    public virtual bool SupportsTemporaryTables => false;

    public virtual string GenerateTemporaryTableName(string baseTableName) => "HT_" + baseTableName;

    public virtual bool? PerformTemporaryTableDDLInIsolation() => new bool?();

    public virtual bool DropTemporaryTableAfterUse() => true;

    public virtual int RegisterResultSetOutParameter(DbCommand statement, int position)
    {
      throw new NotSupportedException(this.GetType().FullName + " does not support resultsets via stored procedures");
    }

    public virtual DbDataReader GetResultSet(DbCommand statement)
    {
      throw new NotSupportedException(this.GetType().FullName + " does not support resultsets via stored procedures");
    }

    public virtual bool SupportsCurrentTimestampSelection => false;

    public virtual long TimestampResolutionInTicks => 1;

    public virtual string GetDropForeignKeyConstraintString(string constraintName)
    {
      return " drop constraint " + constraintName;
    }

    public virtual string GetIfNotExistsCreateConstraint(Table table, string name) => "";

    public virtual string GetIfNotExistsCreateConstraintEnd(Table table, string name) => "";

    public virtual string GetIfExistsDropConstraint(Table table, string name) => "";

    public virtual string GetIfExistsDropConstraintEnd(Table table, string name) => "";

    public virtual string GetDropPrimaryKeyConstraintString(string constraintName)
    {
      return " drop constraint " + constraintName;
    }

    public virtual string GetDropIndexConstraintString(string constraintName)
    {
      return " drop constraint " + constraintName;
    }

    public virtual string CascadeConstraintsString => string.Empty;

    public virtual string DisableForeignKeyConstraintsString => (string) null;

    public virtual string EnableForeignKeyConstraintsString => (string) null;

    public virtual bool SupportsIdentityColumns => false;

    public virtual bool SupportsInsertSelectIdentity => false;

    public virtual bool HasDataTypeInIdentityColumn => true;

    public virtual SqlString AppendIdentitySelectToInsert(SqlString insertString) => insertString;

    public virtual string GetIdentitySelectString(
      string identityColumn,
      string tableName,
      DbType type)
    {
      return this.IdentitySelectString;
    }

    public virtual string IdentitySelectString
    {
      get => throw new MappingException("Dialect does not support identity key generation");
    }

    public virtual string GetIdentityColumnString(DbType type) => this.IdentityColumnString;

    public virtual string IdentityColumnString
    {
      get => throw new MappingException("Dialect does not support identity key generation");
    }

    public virtual bool GenerateTablePrimaryKeyConstraintForIdentityColumn => true;

    public virtual SqlString AddIdentifierOutParameterToInsert(
      SqlString insertString,
      string identifierColumnName,
      string parameterName)
    {
      return insertString;
    }

    public virtual InsertGeneratedIdentifierRetrievalMethod InsertGeneratedIdentifierRetrievalMethod
    {
      get => InsertGeneratedIdentifierRetrievalMethod.ReturnValueParameter;
    }

    public virtual string IdentityInsertString => (string) null;

    public virtual bool SupportsSequences => false;

    public virtual bool SupportsPooledSequences => false;

    public virtual string GetSequenceNextValString(string sequenceName)
    {
      throw new MappingException("Dialect does not support sequences");
    }

    public virtual string GetDropSequenceString(string sequenceName)
    {
      throw new MappingException("Dialect does not support sequences");
    }

    public virtual string[] GetDropSequenceStrings(string sequenceName)
    {
      return new string[1]
      {
        this.GetDropSequenceString(sequenceName)
      };
    }

    public virtual string GetSelectSequenceNextValString(string sequenceName)
    {
      throw new MappingException("Dialect does not support sequences");
    }

    public virtual string GetCreateSequenceString(string sequenceName)
    {
      throw new MappingException("Dialect does not support sequences");
    }

    public virtual string[] GetCreateSequenceStrings(
      string sequenceName,
      int initialValue,
      int incrementSize)
    {
      return new string[1]
      {
        this.GetCreateSequenceString(sequenceName, initialValue, incrementSize)
      };
    }

    protected virtual string GetCreateSequenceString(
      string sequenceName,
      int initialValue,
      int incrementSize)
    {
      if (!this.SupportsPooledSequences)
        throw new MappingException("Dialect does not support pooled sequences");
      return this.GetCreateSequenceString(sequenceName) + " start with " + (object) initialValue + " increment by " + (object) incrementSize;
    }

    public virtual string QuerySequencesString => (string) null;

    public virtual System.Type IdentityStyleIdentifierGeneratorClass
    {
      get
      {
        if (this.SupportsIdentityColumns)
          return typeof (IdentityGenerator);
        return this.SupportsSequences ? typeof (SequenceIdentityGenerator) : typeof (TriggerIdentityGenerator);
      }
    }

    public virtual System.Type NativeIdentifierGeneratorClass
    {
      get
      {
        if (this.SupportsIdentityColumns)
          return typeof (IdentityGenerator);
        return this.SupportsSequences ? typeof (SequenceGenerator) : typeof (TableHiLoGenerator);
      }
    }

    public virtual JoinFragment CreateOuterJoinFragment() => (JoinFragment) new ANSIJoinFragment();

    public virtual CaseFragment CreateCaseFragment() => (CaseFragment) new ANSICaseFragment(this);

    public virtual string ToBooleanValueString(bool value) => !value ? "0" : "1";

    internal static void ExtractColumnOrAliasNames(
      SqlString select,
      out List<SqlString> columnsOrAliases,
      out Dictionary<SqlString, SqlString> aliasToColumn,
      out Dictionary<SqlString, SqlString> columnToAlias)
    {
      columnsOrAliases = new List<SqlString>();
      aliasToColumn = new Dictionary<SqlString, SqlString>();
      columnToAlias = new Dictionary<SqlString, SqlString>();
      IList<SqlString> tokens = new NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer(select).GetTokens();
      int index1 = 0;
      while (index1 < tokens.Count)
      {
        SqlString key1 = tokens[index1];
        int index2 = ++index1;
        if (!key1.StartsWithCaseInsensitive(nameof (select)) && !key1.StartsWithCaseInsensitive("distinct") && !key1.StartsWithCaseInsensitive(","))
        {
          if (key1.StartsWithCaseInsensitive("from"))
            break;
          for (; index2 < tokens.Count && !tokens[index2].StartsWithCaseInsensitive("as") && !tokens[index2].StartsWithCaseInsensitive("from") && !tokens[index2].StartsWithCaseInsensitive(","); index2 = ++index1)
          {
            SqlString sql = tokens[index2];
            key1 = key1.Append(sql);
          }
          SqlString key2 = key1;
          if (key1.IndexOfCaseInsensitive("'") < 0 && key1.IndexOfCaseInsensitive("(") < 0)
          {
            int num = key1.IndexOfCaseInsensitive(".");
            if (num != -1)
              key2 = key1.Substring(num + 1);
          }
          if (index2 + 1 < tokens.Count && tokens[index2].IndexOfCaseInsensitive("as") >= 0)
          {
            key2 = tokens[index2 + 1];
            index1 += 2;
          }
          columnsOrAliases.Add(key2);
          aliasToColumn[key2] = key1;
          columnToAlias[key1] = key2;
        }
      }
    }

    public virtual bool SupportsLimit => false;

    public virtual bool SupportsLimitOffset => this.SupportsLimit;

    public virtual bool SupportsVariableLimit => this.SupportsLimit;

    public virtual bool UseMaxForLimit => false;

    public virtual bool OffsetStartsAtOne => false;

    public virtual SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      throw new NotSupportedException("Dialect does not have support for limit strings.");
    }

    public SqlString GetLimitString(
      SqlString queryString,
      int? offset,
      int? limit,
      Parameter offsetParameter,
      Parameter limitParameter)
    {
      if (!offset.HasValue && !limit.HasValue && offsetParameter == (Parameter) null && limitParameter == (Parameter) null)
        return queryString;
      if (!this.SupportsLimit)
        throw new NotSupportedException("Dialect does not support limits.");
      if (!this.SupportsVariableLimit && offsetParameter != (Parameter) null && !offset.HasValue)
        throw new NotSupportedException("Dialect does not support variable limits.");
      if (!this.SupportsVariableLimit && limitParameter != (Parameter) null && !limit.HasValue)
        throw new NotSupportedException("Dialect does not support variable limits.");
      if (!this.SupportsLimitOffset && (offset.HasValue || offsetParameter != (Parameter) null))
        throw new NotSupportedException("Dialect does not support limits with offsets.");
      SqlString offset1 = !this.SupportsVariableLimit || !(offsetParameter != (Parameter) null) ? (offset.HasValue ? new SqlString(offset.ToString()) : (SqlString) null) : new SqlString(offsetParameter);
      SqlString limit1 = !this.SupportsVariableLimit || !(limitParameter != (Parameter) null) ? (limit.HasValue ? new SqlString(limit.ToString()) : (SqlString) null) : new SqlString(limitParameter);
      return this.GetLimitString(queryString, offset1, limit1);
    }

    public int GetLimitValue(int offset, int limit)
    {
      if (limit == int.MaxValue)
        return int.MaxValue;
      return this.UseMaxForLimit ? this.GetOffsetValue(offset) + limit : limit;
    }

    public int GetOffsetValue(int offset) => this.OffsetStartsAtOne ? offset + 1 : offset;

    public virtual char OpenQuote => '"';

    public virtual char CloseQuote => '"';

    public virtual bool IsQuoted(string name)
    {
      return !string.IsNullOrEmpty(name) && (int) name[0] == (int) this.OpenQuote && (int) name[name.Length - 1] == (int) this.CloseQuote;
    }

    public virtual string Qualify(string catalog, string schema, string table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrEmpty(catalog))
        stringBuilder.Append(catalog).Append('.');
      if (!string.IsNullOrEmpty(schema))
        stringBuilder.Append(schema).Append('.');
      return stringBuilder.Append(table).ToString();
    }

    protected virtual string Quote(string name)
    {
      string str = name.Replace(this.OpenQuote.ToString(), new string(this.OpenQuote, 2));
      if ((int) this.OpenQuote != (int) this.CloseQuote)
        str = name.Replace(this.CloseQuote.ToString(), new string(this.CloseQuote, 2));
      return this.OpenQuote.ToString() + str + (object) this.CloseQuote;
    }

    public virtual string QuoteForAliasName(string aliasName)
    {
      return !this.IsQuoted(aliasName) ? this.Quote(aliasName) : aliasName;
    }

    public virtual string QuoteForColumnName(string columnName)
    {
      return !this.IsQuoted(columnName) ? this.Quote(columnName) : columnName;
    }

    public virtual string QuoteForTableName(string tableName)
    {
      return !this.IsQuoted(tableName) ? this.Quote(tableName) : tableName;
    }

    public virtual string QuoteForSchemaName(string schemaName)
    {
      return !this.IsQuoted(schemaName) ? this.Quote(schemaName) : schemaName;
    }

    public virtual string UnQuote(string quoted)
    {
      string str = (!this.IsQuoted(quoted) ? quoted : quoted.Substring(1, quoted.Length - 2)).Replace(new string(this.OpenQuote, 2), this.OpenQuote.ToString());
      if ((int) this.OpenQuote != (int) this.CloseQuote)
        str = str.Replace(new string(this.CloseQuote, 2), this.CloseQuote.ToString());
      return str;
    }

    public virtual string[] UnQuote(string[] quoted)
    {
      string[] strArray = new string[quoted.Length];
      for (int index = 0; index < quoted.Length; ++index)
        strArray[index] = this.UnQuote(quoted[index]);
      return strArray;
    }

    public virtual string GetSelectClauseNullString(SqlType sqlType) => "null";

    public virtual bool SupportsUnionAll => false;

    public virtual bool SupportsEmptyInList => true;

    public virtual bool AreStringComparisonsCaseInsensitive => false;

    public virtual bool SupportsRowValueConstructorSyntax => false;

    public virtual bool SupportsRowValueConstructorSyntaxInInList => false;

    public virtual bool UseInputStreamToInsertBlob => true;

    public virtual bool SupportsParametersInInsertSelect => true;

    public virtual bool SupportsResultSetPositionQueryMethodsOnForwardOnlyCursor => true;

    public virtual bool SupportsCircularCascadeDeleteConstraints => true;

    public virtual bool SupportsSubselectAsInPredicateLHS => true;

    public virtual bool SupportsExpectedLobUsagePattern => true;

    public virtual bool SupportsLobValueChangePropogation => true;

    public virtual bool SupportsUnboundedLobLocatorMaterialization => true;

    public virtual bool SupportsSubqueryOnMutatingTable => true;

    public virtual bool SupportsExistsInSelect => true;

    public virtual bool DoesReadCommittedCauseWritersToBlockReaders => false;

    public virtual bool DoesRepeatableReadCauseReadersToBlockWriters => false;

    public virtual bool SupportsBindAsCallableArgument => true;

    public virtual bool SupportsSubSelects => true;

    public IDictionary<string, string> DefaultProperties => this._properties;

    public virtual IDictionary<string, ISQLFunction> Functions => this._sqlFunctions;

    public HashedSet<string> Keywords => this._sqlKeywords;

    public virtual string SelectGUIDString
    {
      get
      {
        throw new NotSupportedException("dialect does not support server side GUIDs generation.");
      }
    }

    public virtual string CreateTableString => "create table";

    public virtual string CreateMultisetTableString => this.CreateTableString;

    public virtual string CreateTemporaryTableString => "create table";

    public virtual string CreateTemporaryTablePostfix => string.Empty;

    public virtual bool IsCurrentTimestampSelectStringCallable
    {
      get
      {
        throw new NotSupportedException("Database not known to define a current timestamp function");
      }
    }

    public virtual string CurrentTimestampSelectString
    {
      get
      {
        throw new NotSupportedException("Database not known to define a current timestamp function");
      }
    }

    public virtual string CurrentTimestampSQLFunctionName => "current_timestamp";

    public virtual IViolatedConstraintNameExtracter ViolatedConstraintNameExtracter
    {
      get => NHibernate.Dialect.Dialect.Extracter;
    }

    public virtual string NoColumnsInsertString => "values ( )";

    public virtual string LowercaseFunction => "lower";

    public virtual int MaxAliasLength => 10;

    public virtual string AddColumnString
    {
      get => throw new NotSupportedException("No add column syntax supported by Dialect");
    }

    public virtual string DropForeignKeyString => " drop constraint ";

    public virtual string TableTypeString => string.Empty;

    public virtual string NullColumnString => string.Empty;

    public virtual string PrimaryKeyString => "primary key";

    public virtual bool SupportsSqlBatches => false;

    public virtual bool IsKnownToken(string currentToken, string nextToken) => false;

    protected void RegisterKeyword(string word) => this.Keywords.Add(word);

    protected void RegisterFunction(string name, ISQLFunction function)
    {
      this._sqlFunctions[name] = function;
    }

    private void RegisterHibernateType(DbType code, string name)
    {
      this._hibernateTypeNames.Put(code, name);
    }

    public virtual ISQLExceptionConverter BuildSQLExceptionConverter()
    {
      return (ISQLExceptionConverter) new SQLStateConverter(this.ViolatedConstraintNameExtracter);
    }

    public class QuotedAndParenthesisStringTokenizer : IEnumerable<SqlString>, IEnumerable
    {
      private readonly SqlString _original;

      public QuotedAndParenthesisStringTokenizer(SqlString original) => this._original = original;

      IEnumerator<SqlString> IEnumerable<SqlString>.GetEnumerator()
      {
        NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.WhiteSpace;
        int parenthesisCount = 0;
        bool escapeQuote = false;
        char quoteType = '\'';
        int tokenStart = 0;
        int tokenLength = 0;
        string originalString = this._original.ToString();
        for (int i = 0; i < originalString.Length; ++i)
        {
          char ch = originalString[i];
          switch (state)
          {
            case NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.WhiteSpace:
              switch (ch)
              {
                case '"':
                  state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Quoted;
                  quoteType = '"';
                  ++tokenLength;
                  continue;
                case '\'':
                  state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Quoted;
                  quoteType = '\'';
                  ++tokenLength;
                  continue;
                case ',':
                  yield return new SqlString(",");
                  ++tokenStart;
                  continue;
                default:
                  if (ch == '(' || ch == '[')
                  {
                    state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.InParenthesis;
                    ++tokenLength;
                    parenthesisCount = 1;
                    continue;
                  }
                  if (!char.IsWhiteSpace(ch))
                  {
                    state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Token;
                    ++tokenLength;
                    continue;
                  }
                  ++tokenStart;
                  continue;
              }
            case NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Quoted:
              if (escapeQuote)
              {
                escapeQuote = false;
                ++tokenLength;
                break;
              }
              if (ch == '\\' || (int) ch == (int) quoteType && i + 1 < originalString.Length && (int) originalString[i + 1] == (int) quoteType)
              {
                escapeQuote = true;
                ++tokenLength;
                break;
              }
              if ((int) ch == (int) quoteType)
              {
                yield return this._original.Substring(tokenStart, tokenLength + 1);
                tokenStart += tokenLength + 1;
                tokenLength = 0;
                state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.WhiteSpace;
                break;
              }
              ++tokenLength;
              break;
            case NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.InParenthesis:
              if (ch == ')' || ch == ']')
              {
                ++tokenLength;
                --parenthesisCount;
                if (parenthesisCount == 0)
                {
                  yield return this._original.Substring(tokenStart, tokenLength);
                  tokenStart += tokenLength;
                  tokenLength = 0;
                  state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.WhiteSpace;
                  break;
                }
                break;
              }
              if (ch == '(' || ch == '[')
              {
                ++tokenLength;
                ++parenthesisCount;
                break;
              }
              ++tokenLength;
              break;
            case NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Token:
              if (char.IsWhiteSpace(ch))
              {
                yield return this._original.Substring(tokenStart, tokenLength);
                tokenStart += tokenLength + 1;
                tokenLength = 0;
                state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.WhiteSpace;
                break;
              }
              if (ch == ',')
              {
                yield return this._original.Substring(tokenStart, tokenLength);
                yield return new SqlString(",");
                tokenStart += tokenLength + 1;
                tokenLength = 0;
                state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.WhiteSpace;
                break;
              }
              if (ch == '(' || ch == '[')
              {
                state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.InParenthesis;
                parenthesisCount = 1;
                ++tokenLength;
                break;
              }
              switch (ch)
              {
                case '"':
                  state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Quoted;
                  quoteType = '"';
                  ++tokenLength;
                  continue;
                case '\'':
                  state = NHibernate.Dialect.Dialect.QuotedAndParenthesisStringTokenizer.TokenizerState.Quoted;
                  quoteType = '\'';
                  ++tokenLength;
                  continue;
                default:
                  ++tokenLength;
                  continue;
              }
            default:
              throw new InvalidExpressionException("Could not understand the string " + (object) this._original);
          }
        }
        if (tokenLength > 0)
          yield return this._original.Substring(tokenStart, tokenLength);
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) ((IEnumerable<SqlString>) this).GetEnumerator();
      }

      public IList<SqlString> GetTokens()
      {
        return (IList<SqlString>) new List<SqlString>((IEnumerable<SqlString>) this);
      }

      public enum TokenizerState
      {
        WhiteSpace,
        Quoted,
        InParenthesis,
        Token,
      }
    }
  }
}
