// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.CollectionLoader
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
  public class CollectionLoader : OuterJoinLoader, ICollectionInitializer
  {
    private readonly IQueryableCollection collectionPersister;
    private IParameterSpecification[] parametersSpecifications;

    public CollectionLoader(
      IQueryableCollection persister,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.collectionPersister = persister;
    }

    public override bool IsSubselectLoadingEnabled => this.HasSubselectLoadableCollections();

    protected IType KeyType => this.collectionPersister.KeyType;

    public virtual void Initialize(object id, ISessionImplementor session)
    {
      this.LoadCollection(session, id, this.KeyType);
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.collectionPersister.Role + (object) ')';
    }

    protected virtual IEnumerable<IParameterSpecification> CreateParameterSpecificationsAndAssignBackTrack(
      IEnumerable<Parameter> sqlPatameters)
    {
      System.Collections.Generic.List<IParameterSpecification> andAssignBackTrack = new System.Collections.Generic.List<IParameterSpecification>();
      int num1 = 0;
      Parameter[] array = sqlPatameters.ToArray<Parameter>();
      int num2 = 0;
      while (num2 < array.Length)
      {
        PositionalParameterSpecification parameterSpecification1 = new PositionalParameterSpecification(1, 0, num1++);
        parameterSpecification1.ExpectedType = this.KeyType;
        PositionalParameterSpecification parameterSpecification2 = parameterSpecification1;
        foreach (string str in parameterSpecification2.GetIdsForBackTrack((IMapping) this.Factory))
          array[num2++].BackTrack = (object) str;
        andAssignBackTrack.Add((IParameterSpecification) parameterSpecification2);
      }
      return (IEnumerable<IParameterSpecification>) andAssignBackTrack;
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return (IEnumerable<IParameterSpecification>) this.parametersSpecifications ?? (IEnumerable<IParameterSpecification>) (this.parametersSpecifications = this.CreateParameterSpecificationsAndAssignBackTrack(this.SqlString.GetParameters()).ToArray<IParameterSpecification>());
    }

    protected SqlString GetSubSelectWithLimits(
      SqlString subquery,
      ICollection<IParameterSpecification> parameterSpecs,
      RowSelection processedRowSelection,
      IDictionary<string, TypedValue> parameters)
    {
      ISessionFactoryImplementor factory = this.Factory;
      NHibernate.Dialect.Dialect dialect = factory.Dialect;
      RowSelection selection = processedRowSelection;
      if (!this.UseLimit(selection, dialect))
        return subquery;
      bool flag = NHibernate.Loader.Loader.GetFirstRow(selection) > 0 && dialect.SupportsLimitOffset;
      int maxOrLimit = NHibernate.Loader.Loader.GetMaxOrLimit(dialect, selection);
      int? offset = flag ? new int?(dialect.GetOffsetValue(NHibernate.Loader.Loader.GetFirstRow(selection))) : new int?();
      int? limit = maxOrLimit != int.MaxValue ? new int?(maxOrLimit) : new int?();
      Parameter offsetParameter = (Parameter) null;
      Parameter limitParameter = (Parameter) null;
      if (offset.HasValue)
      {
        string str = "nhsubselectskip";
        NamedParameterSpecification parameterSpecification1 = new NamedParameterSpecification(1, 0, str);
        parameterSpecification1.ExpectedType = (IType) NHibernateUtil.Int32;
        NamedParameterSpecification parameterSpecification2 = parameterSpecification1;
        offsetParameter = Parameter.Placeholder;
        offsetParameter.BackTrack = (object) parameterSpecification2.GetIdsForBackTrack((IMapping) factory).First<string>();
        parameters.Add(str, new TypedValue(parameterSpecification2.ExpectedType, (object) offset.Value, EntityMode.Poco));
        parameterSpecs.Add((IParameterSpecification) parameterSpecification2);
      }
      if (limit.HasValue)
      {
        string str = "nhsubselecttake";
        NamedParameterSpecification parameterSpecification3 = new NamedParameterSpecification(1, 0, str);
        parameterSpecification3.ExpectedType = (IType) NHibernateUtil.Int32;
        NamedParameterSpecification parameterSpecification4 = parameterSpecification3;
        limitParameter = Parameter.Placeholder;
        limitParameter.BackTrack = (object) parameterSpecification4.GetIdsForBackTrack((IMapping) factory).First<string>();
        parameters.Add(str, new TypedValue(parameterSpecification4.ExpectedType, (object) limit.Value, EntityMode.Poco));
        parameterSpecs.Add((IParameterSpecification) parameterSpecification4);
      }
      return dialect.GetLimitString(subquery, offset, limit, offsetParameter, limitParameter);
    }
  }
}
