// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.NativeSQLQueryPlan
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Action;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class NativeSQLQueryPlan
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NativeSQLQueryPlan));
    private readonly string sourceQuery;
    private readonly SQLCustomQuery customQuery;

    public NativeSQLQueryPlan(
      NativeSQLQuerySpecification specification,
      ISessionFactoryImplementor factory)
    {
      this.sourceQuery = specification.QueryString;
      this.customQuery = new SQLCustomQuery(specification.SqlQueryReturns, specification.QueryString, (ICollection<string>) specification.QuerySpaces, factory);
    }

    public string SourceQuery => this.sourceQuery;

    public SQLCustomQuery CustomQuery => this.customQuery;

    private void CoordinateSharedCacheCleanup(ISessionImplementor session)
    {
      BulkOperationCleanupAction cleanupAction = new BulkOperationCleanupAction(session, this.CustomQuery.QuerySpaces);
      cleanupAction.Init();
      if (!session.IsEventSource)
        return;
      ((IEventSource) session).ActionQueue.AddAction(cleanupAction);
    }

    public int PerformExecuteUpdate(QueryParameters queryParameters, ISessionImplementor session)
    {
      this.CoordinateSharedCacheCleanup(session);
      RowSelection rowSelection = !queryParameters.Callable ? queryParameters.RowSelection : throw new ArgumentException("callable not yet supported for native queries");
      try
      {
        List<IParameterSpecification> list1 = this.customQuery.CollectedParametersSpecifications.ToList<IParameterSpecification>();
        SqlString sql = this.ExpandDynamicFilterParameters(this.customQuery.SQL, (ICollection<IParameterSpecification>) list1, session);
        list1.ResetEffectiveExpectedType(queryParameters);
        List<Parameter> list2 = sql.GetParameters().ToList<Parameter>();
        SqlType[] queryParameterTypes = list1.GetQueryParameterTypes(list2, session.Factory);
        IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, sql, queryParameterTypes);
        try
        {
          if (rowSelection != null && rowSelection.Timeout != RowSelection.NoValue)
            dbCommand.CommandTimeout = rowSelection.Timeout;
          foreach (IParameterSpecification parameterSpecification in list1)
            parameterSpecification.Bind(dbCommand, (IList<Parameter>) list2, queryParameters, session);
          return session.Batcher.ExecuteNonQuery(dbCommand);
        }
        finally
        {
          if (dbCommand != null)
            session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, ex, "could not execute native bulk manipulation query:" + this.sourceQuery);
      }
    }

    private SqlString ExpandDynamicFilterParameters(
      SqlString sqlString,
      ICollection<IParameterSpecification> parameterSpecs,
      ISessionImplementor session)
    {
      IDictionary<string, NHibernate.IFilter> enabledFilters = session.EnabledFilters;
      if (enabledFilters.Count == 0 || sqlString.ToString().IndexOf(":") < 0)
        return sqlString;
      NHibernate.Dialect.Dialect dialect = session.Factory.Dialect;
      string delim = " \n\r\f\t,()=<>&|+-=/*'^![]#~\\;" + (object) dialect.OpenQuote + (object) dialect.CloseQuote;
      SqlString sqlString1 = sqlString.Compact();
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      foreach (object part in (IEnumerable) sqlString1.Parts)
      {
        Parameter parameter1 = part as Parameter;
        if (parameter1 != (Parameter) null)
        {
          sqlStringBuilder.Add(parameter1);
        }
        else
        {
          foreach (string sql1 in new StringTokenizer(part.ToString(), delim, true))
          {
            if (sql1.StartsWith(":"))
            {
              string[] filterParameterName = StringHelper.ParseFilterParameterName(sql1.Substring(1));
              string str1 = filterParameterName[0];
              string str2 = filterParameterName[1];
              FilterImpl filterImpl = (FilterImpl) enabledFilters[str1];
              object parameter2 = filterImpl.GetParameter(str2);
              IType parameterType = filterImpl.FilterDefinition.GetParameterType(str2);
              int columnSpan = parameterType.GetColumnSpan((IMapping) session.Factory);
              ICollection collection = parameter2 as ICollection;
              int? collectionSpan = new int?();
              string element = string.Join(", ", Enumerable.Repeat<string>("?", columnSpan).ToArray<string>());
              string sql2;
              if (collection != null && !parameterType.ReturnedClass.IsArray)
              {
                collectionSpan = new int?(collection.Count);
                sql2 = string.Join(", ", Enumerable.Repeat<string>(element, collection.Count).ToArray<string>());
              }
              else
                sql2 = element;
              SqlString sqlString2 = SqlString.Parse(sql2);
              DynamicFilterParameterSpecification parameterSpecification = new DynamicFilterParameterSpecification(str1, str2, parameterType, collectionSpan);
              Parameter[] array = sqlString2.GetParameters().ToArray<Parameter>();
              int num = 0;
              foreach (string str3 in parameterSpecification.GetIdsForBackTrack((IMapping) session.Factory))
                array[num++].BackTrack = (object) str3;
              parameterSpecs.Add((IParameterSpecification) parameterSpecification);
              sqlStringBuilder.Add(sqlString2);
            }
            else
              sqlStringBuilder.Add(sql1);
          }
        }
      }
      return sqlStringBuilder.ToSqlString().Compact();
    }
  }
}
