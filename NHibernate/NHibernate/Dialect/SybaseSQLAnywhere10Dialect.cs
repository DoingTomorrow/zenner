// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.SybaseSQLAnywhere10Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class SybaseSQLAnywhere10Dialect : NHibernate.Dialect.Dialect
  {
    public SybaseSQLAnywhere10Dialect()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SybaseSQLAnywhereDriver";
      this.DefaultProperties["prepare_sql"] = "false";
      this.RegisterCharacterTypeMappings();
      this.RegisterNumericTypeMappings();
      this.RegisterDateTimeTypeMappings();
      this.RegisterReverseNHibernateTypeMappings();
      this.RegisterFunctions();
      this.RegisterKeywords();
    }

    protected virtual void RegisterCharacterTypeMappings()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(1)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, (int) short.MaxValue, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(1)");
      this.RegisterColumnType(DbType.AnsiString, (int) short.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "LONG VARCHAR");
      this.RegisterColumnType(DbType.StringFixedLength, "NCHAR(1)");
      this.RegisterColumnType(DbType.StringFixedLength, (int) short.MaxValue, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, "NVARCHAR(1)");
      this.RegisterColumnType(DbType.String, (int) short.MaxValue, "NVARCHAR($l)");
      this.RegisterColumnType(DbType.String, int.MaxValue, "LONG NVARCHAR");
      this.RegisterColumnType(DbType.Binary, "BINARY(1)");
      this.RegisterColumnType(DbType.Binary, (int) short.MaxValue, "VARBINARY($l)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "LONG BINARY");
      this.RegisterColumnType(DbType.Guid, "UNIQUEIDENTIFIER");
    }

    protected virtual void RegisterNumericTypeMappings()
    {
      this.RegisterColumnType(DbType.Boolean, "BIT");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.UInt64, "UNSIGNED BIGINT");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.UInt16, "UNSIGNED SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INTEGER");
      this.RegisterColumnType(DbType.UInt32, "UNSIGNED INTEGER");
      this.RegisterColumnType(DbType.Single, "REAL");
      this.RegisterColumnType(DbType.Double, "DOUBLE");
      this.RegisterColumnType(DbType.Decimal, "NUMERIC(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "NUMERIC($p, $s)");
    }

    protected virtual void RegisterDateTimeTypeMappings()
    {
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.Time, "TIME");
      this.RegisterColumnType(DbType.DateTime, "TIMESTAMP");
    }

    protected virtual void RegisterReverseNHibernateTypeMappings()
    {
    }

    protected virtual void RegisterFunctions()
    {
      this.RegisterMathFunctions();
      this.RegisterXmlFunctions();
      this.RegisterAggregationFunctions();
      this.RegisterBitFunctions();
      this.RegisterDateFunctions();
      this.RegisterStringFunctions();
      this.RegisterSoapFunctions();
      this.RegisterMiscellaneousFunctions();
    }

    protected virtual void RegisterMathFunctions()
    {
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs"));
      this.RegisterFunction("acos", (ISQLFunction) new StandardSQLFunction("acos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("asin", (ISQLFunction) new StandardSQLFunction("asin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan", (ISQLFunction) new StandardSQLFunction("atan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan2", (ISQLFunction) new StandardSQLFunction("atan2", (IType) NHibernateUtil.Double));
      this.RegisterFunction("ceiling", (ISQLFunction) new StandardSQLFunction("ceiling", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cos", (ISQLFunction) new StandardSQLFunction("cos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cot", (ISQLFunction) new StandardSQLFunction("cot", (IType) NHibernateUtil.Double));
      this.RegisterFunction("degrees", (ISQLFunction) new StandardSQLFunction("degrees", (IType) NHibernateUtil.Double));
      this.RegisterFunction("exp", (ISQLFunction) new StandardSQLFunction("exp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("floor", (ISQLFunction) new StandardSQLFunction("floor", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log", (ISQLFunction) new StandardSQLFunction("log", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log10", (ISQLFunction) new StandardSQLFunction("log10", (IType) NHibernateUtil.Double));
      this.RegisterFunction("mod", (ISQLFunction) new StandardSQLFunction("mod"));
      this.RegisterFunction("pi", (ISQLFunction) new NoArgSQLFunction("pi", (IType) NHibernateUtil.Double, true));
      this.RegisterFunction("power", (ISQLFunction) new StandardSQLFunction("power", (IType) NHibernateUtil.Double));
      this.RegisterFunction("radians", (ISQLFunction) new StandardSQLFunction("radians", (IType) NHibernateUtil.Double));
      this.RegisterFunction("rand", (ISQLFunction) new StandardSQLFunction("rand", (IType) NHibernateUtil.Double));
      this.RegisterFunction("remainder", (ISQLFunction) new StandardSQLFunction("remainder"));
      this.RegisterFunction("round", (ISQLFunction) new StandardSQLFunction("round"));
      this.RegisterFunction("sign", (ISQLFunction) new StandardSQLFunction("sign", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("sin", (ISQLFunction) new StandardSQLFunction("sin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sqrt", (ISQLFunction) new StandardSQLFunction("sqrt", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tan", (ISQLFunction) new StandardSQLFunction("tan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("truncate", (ISQLFunction) new StandardSQLFunction("truncate"));
    }

    protected virtual void RegisterXmlFunctions()
    {
      this.RegisterFunction("xmlconcat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "xmlconcat(", ",", ")"));
      this.RegisterFunction("xmlelement", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "xmlelement(", ",", ")"));
      this.RegisterFunction("xmlgen", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "xmlgen(", ",", ")"));
    }

    protected virtual void RegisterAggregationFunctions()
    {
      this.RegisterFunction("bit_or", (ISQLFunction) new StandardSQLFunction("bit_or"));
      this.RegisterFunction("bit_and", (ISQLFunction) new StandardSQLFunction("bit_and"));
      this.RegisterFunction("bit_xor", (ISQLFunction) new StandardSQLFunction("bit_xor"));
      this.RegisterFunction("covar_pop", (ISQLFunction) new StandardSQLFunction("covar_pop", (IType) NHibernateUtil.Double));
      this.RegisterFunction("covar_samp", (ISQLFunction) new StandardSQLFunction("covar_samp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("corr", (ISQLFunction) new StandardSQLFunction("corr", (IType) NHibernateUtil.Double));
      this.RegisterFunction("first_value", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.Double, "first_value(", ",", ")"));
      this.RegisterFunction("grouping", (ISQLFunction) new StandardSQLFunction("grouping", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("last_value", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.Double, "last_value(", ",", ")"));
      this.RegisterFunction("list", (ISQLFunction) new VarArgsSQLFunction("list(", ",", ")"));
      this.RegisterFunction("regr_avgx", (ISQLFunction) new StandardSQLFunction("regr_avgx", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_avgy", (ISQLFunction) new StandardSQLFunction("regr_avgy", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_count", (ISQLFunction) new StandardSQLFunction("regr_count", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_intercept", (ISQLFunction) new StandardSQLFunction("regr_intercept", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_r2", (ISQLFunction) new StandardSQLFunction("regr_r2", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_slope", (ISQLFunction) new StandardSQLFunction("regr_slope", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_sxx", (ISQLFunction) new StandardSQLFunction("regr_sxx", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_sxy", (ISQLFunction) new StandardSQLFunction("regr_sxy", (IType) NHibernateUtil.Double));
      this.RegisterFunction("regr_syy", (ISQLFunction) new StandardSQLFunction("regr_syy", (IType) NHibernateUtil.Double));
      this.RegisterFunction("set_bits", (ISQLFunction) new StandardSQLFunction("set_bits"));
      this.RegisterFunction("stddev", (ISQLFunction) new StandardSQLFunction("stddev", (IType) NHibernateUtil.Double));
      this.RegisterFunction("stddev_pop", (ISQLFunction) new StandardSQLFunction("stddev_pop", (IType) NHibernateUtil.Double));
      this.RegisterFunction("stddev_samp", (ISQLFunction) new StandardSQLFunction("stddev_samp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("variance", (ISQLFunction) new StandardSQLFunction("variance", (IType) NHibernateUtil.Double));
      this.RegisterFunction("var_pop", (ISQLFunction) new StandardSQLFunction("var_pop", (IType) NHibernateUtil.Double));
      this.RegisterFunction("var_samp", (ISQLFunction) new StandardSQLFunction("var_samp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("xmlagg", (ISQLFunction) new StandardSQLFunction("xmlagg"));
    }

    protected virtual void RegisterBitFunctions()
    {
      this.RegisterFunction("bit_length", (ISQLFunction) new StandardSQLFunction("bit_length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("bit_substr", (ISQLFunction) new StandardSQLFunction("bit_substr"));
      this.RegisterFunction("get_bit", (ISQLFunction) new StandardSQLFunction("get_bit", (IType) NHibernateUtil.Boolean));
      this.RegisterFunction("set_bit", (ISQLFunction) new VarArgsSQLFunction("set_bit(", ",", ")"));
    }

    protected virtual void RegisterDateFunctions()
    {
      this.RegisterFunction("date", (ISQLFunction) new StandardSQLFunction("date", (IType) NHibernateUtil.Date));
      this.RegisterFunction("dateadd", (ISQLFunction) new StandardSQLFunction("dateadd", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("datediff", (ISQLFunction) new StandardSQLFunction("datediff", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("dateformat", (ISQLFunction) new StandardSQLFunction("dateformat", (IType) NHibernateUtil.String));
      this.RegisterFunction("datename", (ISQLFunction) new StandardSQLFunction("datename", (IType) NHibernateUtil.String));
      this.RegisterFunction("datepart", (ISQLFunction) new StandardSQLFunction("datepart", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("datetime", (ISQLFunction) new StandardSQLFunction("datetime", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("day", (ISQLFunction) new StandardSQLFunction("day", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("dayname", (ISQLFunction) new StandardSQLFunction("dayname", (IType) NHibernateUtil.String));
      this.RegisterFunction("days", (ISQLFunction) new StandardSQLFunction("days"));
      this.RegisterFunction("dow", (ISQLFunction) new StandardSQLFunction("dow", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("getdate", (ISQLFunction) new StandardSQLFunction("getdate", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("hour", (ISQLFunction) new StandardSQLFunction("hour", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("hours", (ISQLFunction) new StandardSQLFunction("hours"));
      this.RegisterFunction("minute", (ISQLFunction) new StandardSQLFunction("minute", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("minutes", (ISQLFunction) new StandardSQLFunction("minutes"));
      this.RegisterFunction("month", (ISQLFunction) new StandardSQLFunction("month", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("monthname", (ISQLFunction) new StandardSQLFunction("monthname", (IType) NHibernateUtil.String));
      this.RegisterFunction("months", (ISQLFunction) new StandardSQLFunction("months"));
      this.RegisterFunction("now", (ISQLFunction) new NoArgSQLFunction("now", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("quarter", (ISQLFunction) new StandardSQLFunction("quarter", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("second", (ISQLFunction) new StandardSQLFunction("second", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("seconds", (ISQLFunction) new StandardSQLFunction("seconds"));
      this.RegisterFunction("today", (ISQLFunction) new NoArgSQLFunction("now", (IType) NHibernateUtil.Date));
      this.RegisterFunction("weeks", (ISQLFunction) new StandardSQLFunction("weeks"));
      this.RegisterFunction("year", (ISQLFunction) new StandardSQLFunction("year", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("years", (ISQLFunction) new StandardSQLFunction("years"));
      this.RegisterFunction("ymd", (ISQLFunction) new StandardSQLFunction("ymd", (IType) NHibernateUtil.Date));
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Timestamp, true));
      this.RegisterFunction("current_time", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Time, true));
      this.RegisterFunction("current_date", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Date, true));
    }

    protected virtual void RegisterStringFunctions()
    {
      this.RegisterFunction("ascii", (ISQLFunction) new StandardSQLFunction("ascii", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("byte64_decode", (ISQLFunction) new StandardSQLFunction("byte64_decode", (IType) NHibernateUtil.StringClob));
      this.RegisterFunction("byte64_encode", (ISQLFunction) new StandardSQLFunction("byte64_encode", (IType) NHibernateUtil.StringClob));
      this.RegisterFunction("byte_length", (ISQLFunction) new StandardSQLFunction("byte_length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("byte_substr", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "byte_substr(", ",", ")"));
      this.RegisterFunction("char", (ISQLFunction) new StandardSQLFunction("char", (IType) NHibernateUtil.String));
      this.RegisterFunction("charindex", (ISQLFunction) new StandardSQLFunction("charindex", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("char_length", (ISQLFunction) new StandardSQLFunction("char_length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("compare", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.Int32, "compare(", ",", ")"));
      this.RegisterFunction("compress", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.BinaryBlob, "compress(", ",", ")"));
      this.RegisterFunction("concat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "(", "+", ")"));
      this.RegisterFunction("csconvert", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.StringClob, "csconvert(", ",", ")"));
      this.RegisterFunction("decompress", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.BinaryBlob, "decompress(", ",", ")"));
      this.RegisterFunction("decrypt", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.BinaryBlob, "decrypt(", ",", ")"));
      this.RegisterFunction("difference", (ISQLFunction) new StandardSQLFunction("difference", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("encrypt", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.BinaryBlob, "encrypt(", ",", ")"));
      this.RegisterFunction("hash", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "hash(", ",", ")"));
      this.RegisterFunction("insertstr", (ISQLFunction) new StandardSQLFunction("insertstr", (IType) NHibernateUtil.String));
      this.RegisterFunction("lcase", (ISQLFunction) new StandardSQLFunction("lcase", (IType) NHibernateUtil.String));
      this.RegisterFunction("left", (ISQLFunction) new StandardSQLFunction("left", (IType) NHibernateUtil.String));
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("locate", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.Int32, "locate(", ",", ")"));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSQLFunction("lower", (IType) NHibernateUtil.String));
      this.RegisterFunction("ltrim", (ISQLFunction) new StandardSQLFunction("ltrim", (IType) NHibernateUtil.String));
      this.RegisterFunction("patindex", (ISQLFunction) new StandardSQLFunction("patindex", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("repeat", (ISQLFunction) new StandardSQLFunction("repeat", (IType) NHibernateUtil.String));
      this.RegisterFunction("replace", (ISQLFunction) new StandardSQLFunction("replace", (IType) NHibernateUtil.String));
      this.RegisterFunction("replicate", (ISQLFunction) new StandardSQLFunction("replicate", (IType) NHibernateUtil.String));
      this.RegisterFunction("reverse", (ISQLFunction) new StandardSQLFunction("reverse", (IType) NHibernateUtil.String));
      this.RegisterFunction("right", (ISQLFunction) new StandardSQLFunction("right", (IType) NHibernateUtil.String));
      this.RegisterFunction("rtrim", (ISQLFunction) new StandardSQLFunction("rtrim", (IType) NHibernateUtil.String));
      this.RegisterFunction("similar", (ISQLFunction) new StandardSQLFunction("rtrim", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("sortkey", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.Binary, "sortkey(", ",", ")"));
      this.RegisterFunction("soundex", (ISQLFunction) new StandardSQLFunction("soundex", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("space", (ISQLFunction) new StandardSQLFunction("space", (IType) NHibernateUtil.String));
      this.RegisterFunction("str", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "str(", ",", ")"));
      this.RegisterFunction("string", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "string(", ",", ")"));
      this.RegisterFunction("strtouuid", (ISQLFunction) new StandardSQLFunction("strtouuid"));
      this.RegisterFunction("stuff", (ISQLFunction) new StandardSQLFunction("stuff", (IType) NHibernateUtil.String));
      this.RegisterFunction("substr", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "substr(", ",", ")"));
      this.RegisterFunction("substring", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "substr(", ",", ")"));
      this.RegisterFunction("to_char", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "to_char(", ",", ")"));
      this.RegisterFunction("to_nchar", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "to_nchar(", ",", ")"));
      this.RegisterFunction("trim", (ISQLFunction) new StandardSQLFunction("trim", (IType) NHibernateUtil.String));
      this.RegisterFunction("ucase", (ISQLFunction) new StandardSQLFunction("ucase", (IType) NHibernateUtil.String));
      this.RegisterFunction("unicode", (ISQLFunction) new StandardSQLFunction("unicode", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("unistr", (ISQLFunction) new StandardSQLFunction("unistr", (IType) NHibernateUtil.String));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper", (IType) NHibernateUtil.String));
      this.RegisterFunction("uuidtostr", (ISQLFunction) new StandardSQLFunction("uuidtostr", (IType) NHibernateUtil.String));
    }

    protected virtual void RegisterSoapFunctions()
    {
      this.RegisterFunction("html_decode", (ISQLFunction) new StandardSQLFunction("html_decode", (IType) NHibernateUtil.String));
      this.RegisterFunction("html_encode", (ISQLFunction) new StandardSQLFunction("html_encode", (IType) NHibernateUtil.String));
      this.RegisterFunction("http_decode", (ISQLFunction) new StandardSQLFunction("http_decode", (IType) NHibernateUtil.String));
      this.RegisterFunction("http_encode", (ISQLFunction) new StandardSQLFunction("http_encode", (IType) NHibernateUtil.String));
      this.RegisterFunction("http_header", (ISQLFunction) new StandardSQLFunction("http_header", (IType) NHibernateUtil.String));
      this.RegisterFunction("http_variable", (ISQLFunction) new VarArgsSQLFunction("http_variable(", ",", ")"));
      this.RegisterFunction("next_http_header", (ISQLFunction) new StandardSQLFunction("next_http_header", (IType) NHibernateUtil.String));
      this.RegisterFunction("next_http_variable", (ISQLFunction) new StandardSQLFunction("next_http_variable", (IType) NHibernateUtil.String));
      this.RegisterFunction("next_soap_header", (ISQLFunction) new VarArgsSQLFunction("next_soap_header(", ",", ")"));
    }

    protected virtual void RegisterMiscellaneousFunctions()
    {
      this.RegisterFunction("argn", (ISQLFunction) new VarArgsSQLFunction("argn(", ",", ")"));
      this.RegisterFunction("coalesce", (ISQLFunction) new VarArgsSQLFunction("coalesce(", ",", ")"));
      this.RegisterFunction("conflict", (ISQLFunction) new StandardSQLFunction("conflict", (IType) NHibernateUtil.Boolean));
      this.RegisterFunction("connection_property", (ISQLFunction) new VarArgsSQLFunction("connection_property(", ",", ")"));
      this.RegisterFunction("connection_extended_property", (ISQLFunction) new VarArgsSQLFunction("connection_extended_property(", ",", ")"));
      this.RegisterFunction("db_extended_property", (ISQLFunction) new VarArgsSQLFunction("db_extended_property(", ",", ")"));
      this.RegisterFunction("db_property", (ISQLFunction) new VarArgsSQLFunction("db_property(", ",", ")"));
      this.RegisterFunction("errormsg", (ISQLFunction) new NoArgSQLFunction("errormsg", (IType) NHibernateUtil.String, true));
      this.RegisterFunction("estimate", (ISQLFunction) new VarArgsSQLFunction("estimate(", ",", ")"));
      this.RegisterFunction("estimate_source", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "estimate_source(", ",", ")"));
      this.RegisterFunction("experience_estimate", (ISQLFunction) new VarArgsSQLFunction("experience_estimate(", ",", ")"));
      this.RegisterFunction("explanation", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "explanation(", ",", ")"));
      this.RegisterFunction("exprtype", (ISQLFunction) new StandardSQLFunction("exprtype", (IType) NHibernateUtil.String));
      this.RegisterFunction("get_identity", (ISQLFunction) new VarArgsSQLFunction("get_identity(", ",", ")"));
      this.RegisterFunction("graphical_plan", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "graphical_plan(", ",", ")"));
      this.RegisterFunction("greater", (ISQLFunction) new StandardSQLFunction("greater"));
      this.RegisterFunction("identity", (ISQLFunction) new StandardSQLFunction("identity"));
      this.RegisterFunction("ifnull", (ISQLFunction) new VarArgsSQLFunction("ifnull(", ",", ")"));
      this.RegisterFunction("index_estimate", (ISQLFunction) new VarArgsSQLFunction("index_estimate(", ",", ")"));
      this.RegisterFunction("isnull", (ISQLFunction) new VarArgsSQLFunction("isnull(", ",", ")"));
      this.RegisterFunction("lesser", (ISQLFunction) new StandardSQLFunction("lesser"));
      this.RegisterFunction("newid", (ISQLFunction) new NoArgSQLFunction("newid", (IType) NHibernateUtil.String, true));
      this.RegisterFunction("nullif", (ISQLFunction) new StandardSQLFunction("nullif"));
      this.RegisterFunction("number", (ISQLFunction) new NoArgSQLFunction("number", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("plan", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "plan(", ",", ")"));
      this.RegisterFunction("property", (ISQLFunction) new StandardSQLFunction("property", (IType) NHibernateUtil.String));
      this.RegisterFunction("property_description", (ISQLFunction) new StandardSQLFunction("property_description", (IType) NHibernateUtil.String));
      this.RegisterFunction("property_name", (ISQLFunction) new StandardSQLFunction("property_name", (IType) NHibernateUtil.String));
      this.RegisterFunction("property_number", (ISQLFunction) new StandardSQLFunction("property_number", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("rewrite", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "rewrite(", ",", ")"));
      this.RegisterFunction("row_number", (ISQLFunction) new NoArgSQLFunction("row_number", (IType) NHibernateUtil.Int32, true));
      this.RegisterFunction("sqldialect", (ISQLFunction) new StandardSQLFunction("sqldialect", (IType) NHibernateUtil.String));
      this.RegisterFunction("sqlflagger", (ISQLFunction) new StandardSQLFunction("sqlflagger", (IType) NHibernateUtil.String));
      this.RegisterFunction("traceback", (ISQLFunction) new NoArgSQLFunction("traceback", (IType) NHibernateUtil.String));
      this.RegisterFunction("transactsql", (ISQLFunction) new StandardSQLFunction("transactsql", (IType) NHibernateUtil.String));
      this.RegisterFunction("varexists", (ISQLFunction) new StandardSQLFunction("varexists", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("watcomsql", (ISQLFunction) new StandardSQLFunction("watcomsql", (IType) NHibernateUtil.String));
    }

    protected virtual void RegisterKeywords()
    {
      this.RegisterKeyword("TOP");
      this.RegisterKeyword("FIRST");
      this.RegisterKeyword("FETCH");
      this.RegisterKeyword("START");
      this.RegisterKeyword("AT");
      this.RegisterKeyword("WITH");
      this.RegisterKeyword("CONTAINS");
      this.RegisterKeyword("REGEXP");
      this.RegisterKeyword("SIMILAR");
      this.RegisterKeyword("SEQUENCE");
    }

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString => "select @@identity";

    public override string IdentityColumnString => "not null default autoincrement";

    public override SqlString AppendIdentitySelectToInsert(SqlString insertSql)
    {
      return insertSql.Append("; " + this.IdentitySelectString);
    }

    public override bool SupportsInsertSelectIdentity => true;

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => true;

    public override bool SupportsVariableLimit => true;

    public override bool OffsetStartsAtOne => true;

    private static int GetAfterSelectInsertPoint(SqlString sql)
    {
      if (sql.StartsWithCaseInsensitive("select distinct"))
        return 15;
      return sql.StartsWithCaseInsensitive("select") ? 6 : 0;
    }

    public override SqlString GetLimitString(SqlString sql, SqlString offset, SqlString limit)
    {
      int selectInsertPoint = SybaseSQLAnywhere10Dialect.GetAfterSelectInsertPoint(sql);
      if (selectInsertPoint <= 0)
        return sql;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("select");
      if (selectInsertPoint > 6)
        sqlStringBuilder.Add(" distinct ");
      sqlStringBuilder.Add(" top ");
      sqlStringBuilder.Add(limit);
      if (offset != null)
      {
        sqlStringBuilder.Add(" start at ");
        sqlStringBuilder.Add(offset);
      }
      sqlStringBuilder.Add(sql.Substring(selectInsertPoint));
      return sqlStringBuilder.ToSqlString();
    }

    public override string GetForUpdateString(LockMode lockMode)
    {
      if (lockMode == LockMode.Read)
        return this.ForReadOnlyString;
      if (lockMode == LockMode.Upgrade)
        return this.ForUpdateByLockString;
      if (lockMode == LockMode.UpgradeNoWait)
        return this.ForUpdateNowaitString;
      return lockMode == LockMode.Force ? this.ForUpdateNowaitString : string.Empty;
    }

    public override bool ForUpdateOfColumns => false;

    public override bool SupportsOuterJoinForUpdate => true;

    public override string ForUpdateString => this.ForUpdateByLockString;

    public string ForReadOnlyString => " for read only";

    public string ForUpdateByLockString => " for update by lock";

    public override string ForUpdateNowaitString => this.ForUpdateByLockString;

    public override bool DoesReadCommittedCauseWritersToBlockReaders => true;

    public override bool DoesRepeatableReadCauseReadersToBlockWriters => true;

    public override bool SupportsCurrentTimestampSelection => true;

    public override string CurrentTimestampSQLFunctionName => "getdate";

    public override bool IsCurrentTimestampSelectStringCallable => false;

    public override string CurrentTimestampSelectString => "select getdate()";

    public override char CloseQuote => '"';

    public override char OpenQuote => '"';

    public override bool SupportsEmptyInList => false;

    public override bool SupportsResultSetPositionQueryMethodsOnForwardOnlyCursor => false;

    public override bool SupportsExistsInSelect => false;

    public override bool AreStringComparisonsCaseInsensitive => true;

    public override bool SupportsCommentOn => false;

    public override int MaxAliasLength => (int) sbyte.MaxValue;

    public override string AddColumnString => "add ";

    public override string NullColumnString => " null";

    public override bool QualifyIndexName => false;

    public override string NoColumnsInsertString => " values (default) ";

    public override bool DropConstraints => false;

    public override string DropForeignKeyString => " drop foreign key ";

    public override bool SupportsTemporaryTables => true;

    public override string CreateTemporaryTableString => "create local temporary table ";

    public override string CreateTemporaryTablePostfix => " on commit preserve rows ";

    public override bool? PerformTemporaryTableDDLInIsolation() => new bool?();

    public override int RegisterResultSetOutParameter(DbCommand statement, int position)
    {
      return position;
    }

    public override DbDataReader GetResultSet(DbCommand statement) => statement.ExecuteReader();

    public override string SelectGUIDString => "select newid()";

    public override bool SupportsUnionAll => true;

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new SybaseAnywhereDataBaseMetaData(connection);
    }
  }
}
