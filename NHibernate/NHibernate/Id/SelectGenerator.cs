// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.SelectGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Id
{
  public class SelectGenerator : AbstractPostInsertGenerator, IConfigurable
  {
    private string uniqueKeyPropertyName;

    public override IInsertGeneratedIdentifierDelegate GetInsertGeneratedIdentifierDelegate(
      IPostInsertIdentityPersister persister,
      ISessionFactoryImplementor factory,
      bool isGetGeneratedKeysEnabled)
    {
      return (IInsertGeneratedIdentifierDelegate) new SelectGenerator.SelectGeneratorDelegate(persister, factory, this.uniqueKeyPropertyName);
    }

    public void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      parms.TryGetValue("key", out this.uniqueKeyPropertyName);
    }

    private static string DetermineNameOfPropertyToUse(IEntityPersister persister, string supplied)
    {
      if (supplied != null)
        return supplied;
      int[] identifierProperties = persister.NaturalIdentifierProperties;
      if (identifierProperties == null)
        throw new IdentifierGenerationException("no natural-id property defined; need to specify [key] in generator parameters");
      if (identifierProperties.Length > 1)
        throw new IdentifierGenerationException("select generator does not currently support composite natural-id properties; need to specify [key] in generator parameters");
      if (persister.PropertyInsertGenerationInclusions[identifierProperties[0]] != ValueInclusion.None)
        throw new IdentifierGenerationException("natural-id also defined as insert-generated; need to specify [key] in generator parameters");
      return persister.PropertyNames[identifierProperties[0]];
    }

    public class SelectGeneratorDelegate : AbstractSelectingDelegate
    {
      private readonly ISessionFactoryImplementor factory;
      private readonly SqlString idSelectString;
      private readonly IType idType;
      private readonly IPostInsertIdentityPersister persister;
      private readonly string uniqueKeyPropertyName;
      private readonly IType uniqueKeyType;

      internal SelectGeneratorDelegate(
        IPostInsertIdentityPersister persister,
        ISessionFactoryImplementor factory,
        string suppliedUniqueKeyPropertyName)
        : base(persister)
      {
        this.persister = persister;
        this.factory = factory;
        this.uniqueKeyPropertyName = SelectGenerator.DetermineNameOfPropertyToUse((IEntityPersister) persister, suppliedUniqueKeyPropertyName);
        this.idSelectString = persister.GetSelectByUniqueKeyString(this.uniqueKeyPropertyName);
        this.uniqueKeyType = ((IEntityPersister) persister).GetPropertyType(this.uniqueKeyPropertyName);
        this.idType = persister.IdentifierType;
      }

      protected internal override SqlString SelectSQL => this.idSelectString;

      protected internal override SqlType[] ParametersTypes
      {
        get => this.uniqueKeyType.SqlTypes((IMapping) this.factory);
      }

      public override IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert()
      {
        return new IdentifierGeneratingInsert(this.factory);
      }

      protected internal override void BindParameters(
        ISessionImplementor session,
        IDbCommand ps,
        object entity)
      {
        object propertyValue = ((IEntityPersister) this.persister).GetPropertyValue(entity, this.uniqueKeyPropertyName, session.EntityMode);
        this.uniqueKeyType.NullSafeSet(ps, propertyValue, 0, session);
      }

      protected internal override object GetResult(
        ISessionImplementor session,
        IDataReader rs,
        object entity)
      {
        if (!rs.Read())
          throw new IdentifierGenerationException("the inserted row could not be located by the unique key: " + this.uniqueKeyPropertyName);
        return this.idType.NullSafeGet(rs, this.persister.RootTableKeyColumnNames, session, entity);
      }
    }
  }
}
