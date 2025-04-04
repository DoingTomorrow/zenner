// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ManyToOneType
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
  public class ManyToOneType : EntityType
  {
    private readonly bool ignoreNotFound;

    public ManyToOneType(string className)
      : this(className, false)
    {
    }

    public ManyToOneType(string className, bool lazy)
      : base(className, (string) null, !lazy, true, false)
    {
      this.ignoreNotFound = false;
    }

    public ManyToOneType(
      string entityName,
      string uniqueKeyPropertyName,
      bool lazy,
      bool unwrapProxy,
      bool isEmbeddedInXML,
      bool ignoreNotFound)
      : base(entityName, uniqueKeyPropertyName, !lazy, isEmbeddedInXML, unwrapProxy)
    {
      this.ignoreNotFound = ignoreNotFound;
    }

    public override int GetColumnSpan(IMapping mapping)
    {
      return this.GetIdentifierOrUniqueKeyType(mapping).GetColumnSpan(mapping);
    }

    public override SqlType[] SqlTypes(IMapping mapping)
    {
      return this.GetIdentifierOrUniqueKeyType(mapping).SqlTypes(mapping);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory).NullSafeSet(st, this.GetIdentifier(value, session), index, settable, session);
    }

    public override void NullSafeSet(
      IDbCommand cmd,
      object value,
      int index,
      ISessionImplementor session)
    {
      this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory).NullSafeSet(cmd, this.GetIdentifier(value, session), index, session);
    }

    public override bool IsOneToOne => false;

    public override ForeignKeyDirection ForeignKeyDirection
    {
      get => ForeignKeyDirection.ForeignKeyFromParent;
    }

    public override object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      object id = this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory).NullSafeGet(rs, names, session, owner);
      this.ScheduleBatchLoadIfNeeded(id, session);
      return id;
    }

    private void ScheduleBatchLoadIfNeeded(object id, ISessionImplementor session)
    {
      if (this.uniqueKeyPropertyName != null || id == null)
        return;
      IEntityPersister entityPersister = session.Factory.GetEntityPersister(this.GetAssociatedEntityName());
      EntityKey key = new EntityKey(id, entityPersister, session.EntityMode);
      if (session.PersistenceContext.ContainsEntity(key))
        return;
      session.PersistenceContext.BatchFetchQueue.AddBatchLoadableEntityKey(key);
    }

    public override bool UseLHSPrimaryKey => false;

    public override bool IsModified(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      if (current == null)
        return old != null;
      return old == null || this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory).IsDirty(old, this.GetIdentifier(current, session), session);
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      if (this.IsNotEmbedded(session))
        return this.GetIdentifierType(session).Disassemble(value, session, owner);
      if (value == null)
        return (object) null;
      return this.GetIdentifierType(session).Disassemble(ForeignKeys.GetEntityIdentifierIfNotUnsaved(this.GetAssociatedEntityName(), value, session) ?? throw new AssertionFailure("cannot cache a reference to an object with a null id: " + this.GetAssociatedEntityName()), session, owner);
    }

    public override object Assemble(object oid, ISessionImplementor session, object owner)
    {
      object id = this.AssembleId(oid, session);
      if (this.IsNotEmbedded(session))
        return id;
      return id == null ? (object) null : this.ResolveIdentifier(id, session);
    }

    public override void BeforeAssemble(object oid, ISessionImplementor session)
    {
      this.ScheduleBatchLoadIfNeeded(this.AssembleId(oid, session), session);
    }

    private object AssembleId(object oid, ISessionImplementor session)
    {
      return this.GetIdentifierType(session).Assemble(oid, session, (object) null);
    }

    public override bool IsAlwaysDirtyChecked => this.ignoreNotFound;

    public override bool IsDirty(object old, object current, ISessionImplementor session)
    {
      if (this.IsSame(old, current, session.EntityMode))
        return false;
      object identifier1 = this.GetIdentifier(old, session);
      object identifier2 = this.GetIdentifier(current, session);
      return this.GetIdentifierType(session).IsDirty(identifier1, identifier2, session);
    }

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      if (this.IsAlwaysDirtyChecked)
        return this.IsDirty(old, current, session);
      if (this.IsSame(old, current, session.EntityMode))
        return false;
      object identifier1 = this.GetIdentifier(old, session);
      object identifier2 = this.GetIdentifier(current, session);
      return this.GetIdentifierType(session).IsDirty(identifier1, identifier2, checkable, session);
    }

    public override bool IsNullable => this.ignoreNotFound;

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      bool[] array = new bool[this.GetColumnSpan(mapping)];
      if (value != null)
        ArrayHelper.Fill<bool>(array, true);
      return array;
    }
  }
}
