// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.Sql.SQLCustomQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query.Sql;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Loader.Custom.Sql
{
  public class SQLCustomQuery : ICustomQuery
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SQLCustomQuery));
    private readonly List<IReturn> customQueryReturns = new List<IReturn>();
    private readonly ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
    private readonly SqlString sql;
    private List<IParameterSpecification> parametersSpecifications;

    public SQLCustomQuery(
      INativeSQLQueryReturn[] queryReturns,
      string sqlQuery,
      ICollection<string> additionalQuerySpaces,
      ISessionFactoryImplementor factory)
    {
      SQLCustomQuery.log.Debug((object) ("starting processing of sql query [" + sqlQuery + "]"));
      SQLQueryReturnProcessor queryReturnProcessor = new SQLQueryReturnProcessor(queryReturns, factory);
      SQLQueryReturnProcessor.ResultAliasContext aliasContext = queryReturnProcessor.Process();
      SQLQueryParser sqlQueryParser = new SQLQueryParser(factory, sqlQuery, (SQLQueryParser.IParserContext) new SQLCustomQuery.ParserContext(aliasContext));
      this.sql = sqlQueryParser.Process();
      ArrayHelper.AddAll((IList) this.customQueryReturns, queryReturnProcessor.GenerateCustomReturns(sqlQueryParser.QueryHasAliases));
      this.parametersSpecifications = sqlQueryParser.CollectedParametersSpecifications.ToList<IParameterSpecification>();
      if (additionalQuerySpaces == null)
        return;
      this.querySpaces.AddAll(additionalQuerySpaces);
    }

    public IEnumerable<IParameterSpecification> CollectedParametersSpecifications
    {
      get => (IEnumerable<IParameterSpecification>) this.parametersSpecifications;
    }

    public SqlString SQL => this.sql;

    public ISet<string> QuerySpaces => this.querySpaces;

    public IList<IReturn> CustomQueryReturns => (IList<IReturn>) this.customQueryReturns;

    private class ParserContext : SQLQueryParser.IParserContext
    {
      private readonly SQLQueryReturnProcessor.ResultAliasContext aliasContext;

      public ParserContext(
        SQLQueryReturnProcessor.ResultAliasContext aliasContext)
      {
        this.aliasContext = aliasContext;
      }

      public bool IsEntityAlias(string alias) => this.GetEntityPersisterByAlias(alias) != null;

      public ISqlLoadable GetEntityPersisterByAlias(string alias)
      {
        return this.aliasContext.GetEntityPersister(alias);
      }

      public string GetEntitySuffixByAlias(string alias)
      {
        return this.aliasContext.GetEntitySuffix(alias);
      }

      public bool IsCollectionAlias(string alias)
      {
        return this.GetCollectionPersisterByAlias(alias) != null;
      }

      public ISqlLoadableCollection GetCollectionPersisterByAlias(string alias)
      {
        return this.aliasContext.GetCollectionPersister(alias);
      }

      public string GetCollectionSuffixByAlias(string alias)
      {
        return this.aliasContext.GetCollectionSuffix(alias);
      }

      public IDictionary<string, string[]> GetPropertyResultsMapByAlias(string alias)
      {
        return this.aliasContext.GetPropertyResultsMap(alias);
      }
    }
  }
}
