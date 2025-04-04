// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.SubselectCollectionLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Loader.Collection
{
  public class SubselectCollectionLoader : BasicCollectionLoader
  {
    private const int BatchSizeForSubselectFetching = 1;
    private readonly object[] keys;
    private readonly IDictionary<string, TypedValue> namedParameters;
    private readonly IType[] types;
    private readonly object[] values;
    private readonly System.Collections.Generic.List<IParameterSpecification> parametersSpecifications;

    public SubselectCollectionLoader(
      IQueryableCollection persister,
      SqlString subquery,
      ICollection<EntityKey> entityKeys,
      QueryParameters queryParameters,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(persister, 1, factory, enabledFilters)
    {
      this.keys = new object[entityKeys.Count];
      int num = 0;
      foreach (EntityKey entityKey in (IEnumerable<EntityKey>) entityKeys)
        this.keys[num++] = entityKey.Identifier;
      this.namedParameters = (IDictionary<string, TypedValue>) new Dictionary<string, TypedValue>(queryParameters.NamedParameters);
      this.parametersSpecifications = queryParameters.ProcessedSqlParameters.ToList<IParameterSpecification>();
      RowSelection processedRowSelection = queryParameters.ProcessedRowSelection;
      SqlString subquery1 = subquery;
      if (queryParameters.ProcessedRowSelection != null && !SubselectClauseExtractor.HasOrderBy(queryParameters.ProcessedSql))
        subquery1 = this.GetSubSelectWithLimits(subquery, (ICollection<IParameterSpecification>) this.parametersSpecifications, processedRowSelection, this.namedParameters);
      this.InitializeFromWalker(persister, subquery1, 1, enabledFilters, factory);
      this.types = queryParameters.PositionalParameterTypes;
      this.values = queryParameters.PositionalParameterValues;
    }

    public override void Initialize(object id, ISessionImplementor session)
    {
      this.LoadCollectionSubselect(session, this.keys, this.values, this.types, this.namedParameters, this.KeyType);
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return (IEnumerable<IParameterSpecification>) this.parametersSpecifications;
    }
  }
}
