// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.CollectionElementLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Loader.Entity
{
  public class CollectionElementLoader : OuterJoinLoader
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (CollectionElementLoader));
    private readonly IOuterJoinLoadable persister;
    private readonly IType keyType;
    private readonly IType indexType;
    private readonly string entityName;
    private IParameterSpecification[] parametersSpecifications;

    public CollectionElementLoader(
      IQueryableCollection collectionPersister,
      ISessionFactoryImplementor factory,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.keyType = collectionPersister.KeyType;
      this.indexType = collectionPersister.IndexType;
      this.persister = (IOuterJoinLoadable) collectionPersister.ElementPersister;
      this.entityName = this.persister.EntityName;
      this.InitFromWalker((JoinWalker) new EntityJoinWalker(this.persister, ArrayHelper.Join(collectionPersister.KeyColumnNames, collectionPersister.IndexColumnNames), 1, LockMode.None, factory, enabledFilters));
      this.PostInstantiate();
      CollectionElementLoader.log.Debug((object) ("Static select for entity " + this.entityName + ": " + (object) this.SqlString));
    }

    private IEnumerable<IParameterSpecification> CreateParameterSpecificationsAndAssignBackTrack(
      IEnumerable<Parameter> sqlPatameters)
    {
      IParameterSpecification[] parameterSpecificationArray1 = new IParameterSpecification[2];
      IParameterSpecification[] parameterSpecificationArray2 = parameterSpecificationArray1;
      PositionalParameterSpecification parameterSpecification1 = new PositionalParameterSpecification(1, 0, 0);
      parameterSpecification1.ExpectedType = this.keyType;
      PositionalParameterSpecification parameterSpecification2 = parameterSpecification1;
      parameterSpecificationArray2[0] = (IParameterSpecification) parameterSpecification2;
      IParameterSpecification[] parameterSpecificationArray3 = parameterSpecificationArray1;
      PositionalParameterSpecification parameterSpecification3 = new PositionalParameterSpecification(1, 0, 1);
      parameterSpecification3.ExpectedType = this.indexType;
      PositionalParameterSpecification parameterSpecification4 = parameterSpecification3;
      parameterSpecificationArray3[1] = (IParameterSpecification) parameterSpecification4;
      IParameterSpecification[] source = parameterSpecificationArray1;
      Parameter[] array = sqlPatameters.ToArray<Parameter>();
      int num = 0;
      foreach (string str in ((IEnumerable<IParameterSpecification>) source).SelectMany<IParameterSpecification, string>((System.Func<IParameterSpecification, IEnumerable<string>>) (specification => specification.GetIdsForBackTrack((IMapping) this.Factory))))
        array[num++].BackTrack = (object) str;
      return (IEnumerable<IParameterSpecification>) source;
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return (IEnumerable<IParameterSpecification>) this.parametersSpecifications ?? (IEnumerable<IParameterSpecification>) (this.parametersSpecifications = this.CreateParameterSpecificationsAndAssignBackTrack(this.SqlString.GetParameters()).ToArray<IParameterSpecification>());
    }

    protected override bool IsSingleRowLoader => true;

    public virtual object LoadElement(ISessionImplementor session, object key, object index)
    {
      IList list = this.LoadEntity(session, key, index, this.keyType, this.indexType, (IEntityPersister) this.persister);
      if (list.Count == 1)
        return list[0];
      if (list.Count == 0)
        return (object) null;
      if (this.CollectionOwners != null)
        return list[0];
      throw new HibernateException("More than one row was found");
    }

    protected override object GetResultColumnOrRow(
      object[] row,
      IResultTransformer transformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      return row[row.Length - 1];
    }
  }
}
