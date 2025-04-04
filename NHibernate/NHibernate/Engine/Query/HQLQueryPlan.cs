// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.HQLQueryPlan
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class HQLQueryPlan : IQueryPlan
  {
    protected static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (HQLQueryPlan));
    private readonly string _sourceQuery;

    protected HQLQueryPlan(string sourceQuery, IQueryTranslator[] translators)
    {
      this.Translators = translators;
      this._sourceQuery = sourceQuery;
      this.FinaliseQueryPlan();
    }

    internal HQLQueryPlan(HQLQueryPlan source)
    {
      this.Translators = source.Translators;
      this._sourceQuery = source._sourceQuery;
      this.QuerySpaces = source.QuerySpaces;
      this.ParameterMetadata = source.ParameterMetadata;
      this.ReturnMetadata = source.ReturnMetadata;
      this.SqlStrings = source.SqlStrings;
    }

    public ISet<string> QuerySpaces { get; private set; }

    public ParameterMetadata ParameterMetadata { get; private set; }

    public ReturnMetadata ReturnMetadata { get; private set; }

    public string[] SqlStrings { get; private set; }

    public IQueryTranslator[] Translators { get; private set; }

    public void PerformList(
      QueryParameters queryParameters,
      ISessionImplementor session,
      IList results)
    {
      if (HQLQueryPlan.Log.IsDebugEnabled)
      {
        HQLQueryPlan.Log.Debug((object) ("find: " + this._sourceQuery));
        queryParameters.LogParameters(session.Factory);
      }
      bool flag = queryParameters.RowSelection != null && queryParameters.RowSelection.DefinesLimits && this.Translators.Length > 1;
      QueryParameters queryParameters1;
      if (flag)
      {
        HQLQueryPlan.Log.Warn((object) "firstResult/maxResults specified on polymorphic query; applying in memory!");
        queryParameters1 = queryParameters.CreateCopyUsing(new RowSelection()
        {
          FetchSize = queryParameters.RowSelection.FetchSize,
          Timeout = queryParameters.RowSelection.Timeout
        });
      }
      else
        queryParameters1 = queryParameters;
      IList to = results ?? (IList) new List<object>();
      IdentitySet identitySet = new IdentitySet();
      int num1 = -1;
      for (int index1 = 0; index1 < this.Translators.Length; ++index1)
      {
        IList from = this.Translators[index1].List(session, queryParameters1);
        if (flag)
        {
          int firstRow = queryParameters.RowSelection.FirstRow == RowSelection.NoValue ? 0 : queryParameters.RowSelection.FirstRow;
          int num2 = queryParameters.RowSelection.MaxRows == RowSelection.NoValue ? RowSelection.NoValue : queryParameters.RowSelection.MaxRows;
          int count = from.Count;
          for (int index2 = 0; index2 < count; ++index2)
          {
            object o = from[index2];
            if (!identitySet.Add(o))
            {
              ++num1;
              if (num1 >= firstRow)
              {
                to.Add(o);
                if (num2 >= 0 && num1 > num2)
                  return;
              }
            }
          }
        }
        else
          ArrayHelper.AddAll(to, from);
      }
    }

    public IEnumerable PerformIterate(QueryParameters queryParameters, IEventSource session)
    {
      bool? isMany;
      IEnumerable[] results;
      IEnumerable result;
      this.DoIterate(queryParameters, session, out isMany, out results, out result);
      return !isMany.HasValue || !isMany.Value ? result : (IEnumerable) new JoinedEnumerable(results);
    }

    public IEnumerable<T> PerformIterate<T>(QueryParameters queryParameters, IEventSource session)
    {
      return (IEnumerable<T>) new SafetyEnumerable<T>(this.PerformIterate(queryParameters, session));
    }

    public int PerformExecuteUpdate(QueryParameters queryParameters, ISessionImplementor session)
    {
      if (HQLQueryPlan.Log.IsDebugEnabled)
      {
        HQLQueryPlan.Log.Debug((object) ("executeUpdate: " + this._sourceQuery));
        queryParameters.LogParameters(session.Factory);
      }
      if (this.Translators.Length != 1)
        HQLQueryPlan.Log.Warn((object) ("manipulation query [" + this._sourceQuery + "] resulted in [" + (object) this.Translators.Length + "] split queries"));
      int num = 0;
      for (int index = 0; index < this.Translators.Length; ++index)
        num += this.Translators[index].ExecuteUpdate(queryParameters, session);
      return num;
    }

    private void DoIterate(
      QueryParameters queryParameters,
      IEventSource session,
      out bool? isMany,
      out IEnumerable[] results,
      out IEnumerable result)
    {
      isMany = new bool?();
      results = (IEnumerable[]) null;
      if (HQLQueryPlan.Log.IsDebugEnabled)
      {
        HQLQueryPlan.Log.Debug((object) ("enumerable: " + this._sourceQuery));
        queryParameters.LogParameters(session.Factory);
      }
      if (this.Translators.Length == 0)
      {
        result = CollectionHelper.EmptyEnumerable;
      }
      else
      {
        results = (IEnumerable[]) null;
        bool flag = this.Translators.Length > 1;
        if (flag)
          results = new IEnumerable[this.Translators.Length];
        result = (IEnumerable) null;
        for (int index = 0; index < this.Translators.Length; ++index)
        {
          result = this.Translators[index].GetEnumerable(queryParameters, session);
          if (flag)
            results[index] = result;
        }
        isMany = new bool?(flag);
      }
    }

    private void FinaliseQueryPlan()
    {
      this.BuildSqlStringsAndQuerySpaces();
      this.BuildMetaData();
    }

    private void BuildMetaData()
    {
      if (this.Translators.Length == 0)
      {
        this.ParameterMetadata = new ParameterMetadata((IEnumerable<OrdinalParameterDescriptor>) null, (IDictionary<string, NamedParameterDescriptor>) null);
        this.ReturnMetadata = (ReturnMetadata) null;
      }
      else
      {
        this.ParameterMetadata = this.Translators[0].BuildParameterMetadata();
        if (this.Translators[0].IsManipulationStatement)
          this.ReturnMetadata = (ReturnMetadata) null;
        else if (this.Translators.Length > 1)
          this.ReturnMetadata = new ReturnMetadata(this.Translators[0].ReturnAliases, new IType[this.Translators[0].ReturnTypes.Length]);
        else
          this.ReturnMetadata = new ReturnMetadata(this.Translators[0].ReturnAliases, this.Translators[0].ReturnTypes);
      }
    }

    private void BuildSqlStringsAndQuerySpaces()
    {
      HashedSet<string> hashedSet = new HashedSet<string>();
      List<string> stringList = new List<string>();
      foreach (IQueryTranslator translator in this.Translators)
      {
        foreach (string querySpace in (IEnumerable<string>) translator.QuerySpaces)
          hashedSet.Add(querySpace);
        stringList.AddRange((IEnumerable<string>) translator.CollectSqlStrings);
      }
      this.SqlStrings = stringList.ToArray();
      this.QuerySpaces = (ISet<string>) hashedSet;
    }
  }
}
