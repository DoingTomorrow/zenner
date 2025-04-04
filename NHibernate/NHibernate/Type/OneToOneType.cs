// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.OneToOneType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class OneToOneType : EntityType, IAssociationType, IType, ICacheAssembler
  {
    private static readonly SqlType[] NoSqlTypes = new SqlType[0];
    private readonly ForeignKeyDirection foreignKeyDirection;
    private readonly string propertyName;
    private readonly string entityName;

    public override int GetColumnSpan(IMapping session) => 0;

    public override SqlType[] SqlTypes(IMapping session) => OneToOneType.NoSqlTypes;

    public OneToOneType(
      string referencedEntityName,
      ForeignKeyDirection foreignKeyType,
      string uniqueKeyPropertyName,
      bool lazy,
      bool unwrapProxy,
      bool isEmbeddedInXML,
      string entityName,
      string propertyName)
      : base(referencedEntityName, uniqueKeyPropertyName, !lazy, isEmbeddedInXML, unwrapProxy)
    {
      this.foreignKeyDirection = foreignKeyType;
      this.propertyName = propertyName;
      this.entityName = entityName;
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
    }

    public override void NullSafeSet(
      IDbCommand cmd,
      object value,
      int index,
      ISessionImplementor session)
    {
    }

    public override bool IsOneToOne => true;

    public override bool IsDirty(object old, object current, ISessionImplementor session) => false;

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return false;
    }

    public override bool IsModified(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return false;
    }

    public override bool IsNull(object owner, ISessionImplementor session)
    {
      if (this.propertyName == null)
        return false;
      IEntityPersister entityPersister = session.Factory.GetEntityPersister(this.entityName);
      EntityKey ownerKey = new EntityKey(session.GetContextEntityIdentifier(owner), entityPersister, session.EntityMode);
      return session.PersistenceContext.IsPropertyNull(ownerKey, this.PropertyName);
    }

    public override ForeignKeyDirection ForeignKeyDirection => this.foreignKeyDirection;

    public override object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      IType identifierOrUniqueKeyType = this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory);
      object entityIdentifier = session.GetContextEntityIdentifier(owner);
      if (!(identifierOrUniqueKeyType is EmbeddedComponentType embeddedComponentType) || !(session.GetEntityPersister((string) null, owner).IdentifierType is EmbeddedComponentType identifierType))
        return entityIdentifier;
      object[] propertyValues = identifierType.GetPropertyValues(entityIdentifier, session);
      EntityKey key = new EntityKey(embeddedComponentType.ResolveIdentifier((object) propertyValues, session, (object) null), session.Factory.GetEntityPersister(identifierOrUniqueKeyType.ReturnedClass.FullName), session.EntityMode);
      return session.PersistenceContext.GetEntity(key);
    }

    public override bool IsNullable
    {
      get => this.foreignKeyDirection.Equals((object) ForeignKeyDirection.ForeignKeyToParent);
    }

    public override bool UseLHSPrimaryKey => true;

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return (object) null;
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return this.ResolveIdentifier(session.GetContextEntityIdentifier(owner), session, owner);
    }

    public override bool IsAlwaysDirtyChecked => false;

    public override string PropertyName => this.propertyName;

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      return ArrayHelper.EmptyBoolArray;
    }
  }
}
