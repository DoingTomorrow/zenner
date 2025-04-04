// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.ADOExceptionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Exceptions
{
  public static class ADOExceptionHelper
  {
    public const string SQLNotAvailable = "SQL not available";

    public static Exception Convert(
      ISQLExceptionConverter converter,
      AdoExceptionContextInfo exceptionContextInfo)
    {
      if (exceptionContextInfo == null)
        throw new AssertionFailure("The argument exceptionContextInfo is null.");
      string actualSqlQuery = ADOExceptionHelper.TryGetActualSqlQuery(exceptionContextInfo.SqlException, exceptionContextInfo.Sql);
      ADOExceptionReporter.LogExceptions(exceptionContextInfo.SqlException, ADOExceptionHelper.ExtendMessage(exceptionContextInfo.Message, actualSqlQuery, (object[]) null, (IDictionary<string, TypedValue>) null));
      return converter.Convert(exceptionContextInfo);
    }

    public static Exception Convert(
      ISQLExceptionConverter converter,
      Exception sqlException,
      string message,
      SqlString sql)
    {
      return ADOExceptionHelper.Convert(converter, new AdoExceptionContextInfo()
      {
        SqlException = sqlException,
        Message = message,
        Sql = sql?.ToString()
      });
    }

    public static Exception Convert(
      ISQLExceptionConverter converter,
      Exception sqlException,
      string message)
    {
      string actualSqlQuery = ADOExceptionHelper.TryGetActualSqlQuery(sqlException, "SQL not available");
      return ADOExceptionHelper.Convert(converter, new AdoExceptionContextInfo()
      {
        SqlException = sqlException,
        Message = message,
        Sql = actualSqlQuery
      });
    }

    public static Exception Convert(
      ISQLExceptionConverter converter,
      Exception sqle,
      string message,
      SqlString sql,
      object[] parameterValues,
      IDictionary<string, TypedValue> namedParameters)
    {
      sql = ADOExceptionHelper.TryGetActualSqlQuery(sqle, sql);
      string message1 = ADOExceptionHelper.ExtendMessage(message, sql?.ToString(), parameterValues, namedParameters);
      ADOExceptionReporter.LogExceptions(sqle, message1);
      return ADOExceptionHelper.Convert(converter, sqle, message1, sql);
    }

    public static DbException ExtractDbException(Exception sqlException)
    {
      Exception exception = sqlException;
      DbException dbException;
      for (dbException = sqlException as DbException; dbException == null && exception != null; dbException = exception as DbException)
        exception = exception.InnerException;
      return dbException;
    }

    public static string ExtendMessage(
      string message,
      string sql,
      object[] parameterValues,
      IDictionary<string, TypedValue> namedParameters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(message).Append(Environment.NewLine).Append("[ ").Append(sql ?? "SQL not available").Append(" ]");
      if (parameterValues != null && parameterValues.Length > 0)
      {
        stringBuilder.Append(Environment.NewLine).Append("Positional parameters: ");
        for (int index = 0; index < parameterValues.Length; ++index)
        {
          object obj = parameterValues[index] ?? (object) "null";
          stringBuilder.Append(" #").Append(index).Append(">").Append(obj);
        }
      }
      if (namedParameters != null && namedParameters.Count > 0)
      {
        stringBuilder.Append(Environment.NewLine);
        foreach (KeyValuePair<string, TypedValue> namedParameter in (IEnumerable<KeyValuePair<string, TypedValue>>) namedParameters)
        {
          object obj = namedParameter.Value.Value ?? (object) "null";
          stringBuilder.Append("  ").Append("Name:").Append(namedParameter.Key).Append(" - Value:").Append(obj);
        }
      }
      stringBuilder.Append(Environment.NewLine);
      return stringBuilder.ToString();
    }

    public static SqlString TryGetActualSqlQuery(Exception sqle, SqlString sql)
    {
      string sql1 = (string) sqle.Data[(object) "actual-sql-query"];
      if (sql1 != null)
        sql = new SqlString(sql1);
      return sql;
    }

    public static string TryGetActualSqlQuery(Exception sqle, string sql)
    {
      return (string) sqle.Data[(object) "actual-sql-query"] ?? sql;
    }
  }
}
