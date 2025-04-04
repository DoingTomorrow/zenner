// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.AbstractEntityLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Loader.Entity
{
  public abstract class AbstractEntityLoader : OuterJoinLoader, IUniqueEntityLoader
  {
    protected static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractEntityLoader));
    protected readonly IOuterJoinLoadable persister;
    protected readonly string entityName;
    private IParameterSpecification[] parametersSpecifications;

    protected AbstractEntityLoader(
      IOuterJoinLoadable persister,
      IType uniqueKeyType,
      ISessionFactoryImplementor factory,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.UniqueKeyType = uniqueKeyType;
      this.entityName = persister.EntityName;
      this.persister = persister;
    }

    protected override bool IsSingleRowLoader => true;

    public object Load(object id, object optionalObject, ISessionImplementor session)
    {
      return this.Load(session, id, optionalObject, id);
    }

    protected virtual object Load(
      ISessionImplementor session,
      object id,
      object optionalObject,
      object optionalId)
    {
      IList list = this.LoadEntity(session, id, this.UniqueKeyType, optionalObject, this.entityName, optionalId, (IEntityPersister) this.persister);
      if (list.Count == 1)
        return list[0];
      if (list.Count == 0)
        return (object) null;
      if (this.CollectionOwners != null)
        return list[0];
      throw new HibernateException(string.Format("More than one row with the given identifier was found: {0}, for class: {1}", id, (object) this.persister.EntityName));
    }

    protected override object GetResultColumnOrRow(
      object[] row,
      IResultTransformer resultTransformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      return row[row.Length - 1];
    }

    protected IType UniqueKeyType { get; private set; }

    private IEnumerable<IParameterSpecification> CreateParameterSpecificationsAndAssignBackTrack(
      IEnumerable<Parameter> sqlPatameters)
    {
      System.Collections.Generic.List<IParameterSpecification> andAssignBackTrack = new System.Collections.Generic.List<IParameterSpecification>();
      int num1 = 0;
      Parameter[] array = sqlPatameters.ToArray<Parameter>();
      int num2 = 0;
      while (num2 < array.Length)
      {
        PositionalParameterSpecification parameterSpecification1 = new PositionalParameterSpecification(1, 0, num1++);
        parameterSpecification1.ExpectedType = this.UniqueKeyType;
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
  }
}
