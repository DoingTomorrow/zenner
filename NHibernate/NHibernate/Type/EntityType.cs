// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.EntityType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class EntityType : AbstractType, IAssociationType, IType, ICacheAssembler
  {
    protected readonly string uniqueKeyPropertyName;
    private readonly bool eager;
    private bool isEmbeddedInXML;
    private readonly string associatedEntityName;
    private readonly bool unwrapProxy;
    private System.Type returnedClass;

    protected internal EntityType(
      string entityName,
      string uniqueKeyPropertyName,
      bool eager,
      bool isEmbeddedInXML,
      bool unwrapProxy)
    {
      this.associatedEntityName = entityName;
      this.uniqueKeyPropertyName = uniqueKeyPropertyName;
      this.isEmbeddedInXML = isEmbeddedInXML;
      this.eager = eager;
      this.unwrapProxy = unwrapProxy;
    }

    public override sealed bool IsEntityType => true;

    public override bool IsEqual(
      object x,
      object y,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      IEntityPersister entityPersister = factory.GetEntityPersister(this.associatedEntityName);
      if (!entityPersister.CanExtractIdOutOfEntity)
        return this.IsEqual(x, y, entityMode);
      object x1 = !x.IsProxy() ? entityPersister.GetIdentifier(x, entityMode) : (x as INHibernateProxy).HibernateLazyInitializer.Identifier;
      object y1 = !y.IsProxy() ? entityPersister.GetIdentifier(y, entityMode) : (y as INHibernateProxy).HibernateLazyInitializer.Identifier;
      return entityPersister.IdentifierType.IsEqual(x1, y1, entityMode, factory);
    }

    public virtual bool IsNull(object owner, ISessionImplementor session) => false;

    public override bool IsSame(object x, object y, EntityMode entityMode)
    {
      return object.ReferenceEquals(x, y);
    }

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, new string[1]{ name }, session, owner);
    }

    public override System.Type ReturnedClass
    {
      get
      {
        if (this.returnedClass == null)
          this.returnedClass = this.DetermineAssociatedEntityClass();
        return this.returnedClass;
      }
    }

    private static object GetIdentifier(
      object obj,
      IEntityPersister persister,
      EntityMode entityMode)
    {
      return obj.IsProxy() ? (obj as INHibernateProxy).HibernateLazyInitializer.Identifier : persister.GetIdentifier(obj, entityMode);
    }

    protected internal object GetIdentifier(object value, ISessionImplementor session)
    {
      if (this.IsNotEmbedded(session))
        return value;
      if (this.IsReferenceToPrimaryKey)
        return ForeignKeys.GetEntityIdentifierIfNotUnsaved(this.GetAssociatedEntityName(), value, session);
      if (value == null)
        return (object) null;
      IEntityPersister entityPersister = session.Factory.GetEntityPersister(this.GetAssociatedEntityName());
      object identifier = entityPersister.GetPropertyValue(value, this.uniqueKeyPropertyName, session.EntityMode);
      IType propertyType = entityPersister.GetPropertyType(this.uniqueKeyPropertyName);
      if (propertyType.IsEntityType)
        identifier = ((EntityType) propertyType).GetIdentifier(identifier, session);
      return identifier;
    }

    protected internal virtual bool IsNotEmbedded(ISessionImplementor session) => false;

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      if (value == null)
        return "null";
      IEntityPersister entityPersister = factory.GetEntityPersister(this.associatedEntityName);
      StringBuilder stringBuilder = new StringBuilder().Append(this.associatedEntityName);
      if (entityPersister.HasIdentifierProperty)
      {
        EntityMode? nullable = entityPersister.GuessEntityMode(value);
        object obj;
        if (!nullable.HasValue)
        {
          if (this.isEmbeddedInXML)
            throw new InvalidCastException(value.GetType().FullName);
          obj = value;
        }
        else
          obj = EntityType.GetIdentifier(value, entityPersister, nullable.Value);
        stringBuilder.Append('#').Append(entityPersister.IdentifierType.ToLoggableString(obj, factory));
      }
      return stringBuilder.ToString();
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory)
    {
      return !this.isEmbeddedInXML ? this.GetIdentifierType(factory).FromXMLNode(xml, factory) : (object) xml;
    }

    public override string Name => this.associatedEntityName;

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return value;
    }

    public override bool IsMutable => false;

    public abstract bool IsOneToOne { get; }

    public override object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache)
    {
      if (original == null)
        return (object) null;
      object obj1 = copyCache[original];
      if (obj1 != null)
        return obj1;
      if (original == target)
        return target;
      if (session.GetContextEntityIdentifier(original) == null && ForeignKeys.IsTransient(this.associatedEntityName, original, new bool?(false), session))
      {
        object obj2 = session.Factory.GetEntityPersister(this.associatedEntityName).Instantiate((object) null, session.EntityMode);
        copyCache.Add(original, obj2);
        return obj2;
      }
      return this.ResolveIdentifier(this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory).Replace(this.GetIdentifier(original, session) ?? throw new AssertionFailure("non-transient entity has a null id"), (object) null, session, owner, copyCache), session, owner);
    }

    public override bool IsAssociationType => true;

    public override sealed object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.ResolveIdentifier(this.Hydrate(rs, names, session, owner), session, owner);
    }

    public abstract override object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner);

    public bool IsUniqueKeyReference => this.uniqueKeyPropertyName != null;

    public abstract bool IsNullable { get; }

    public IJoinable GetAssociatedJoinable(ISessionFactoryImplementor factory)
    {
      return (IJoinable) factory.GetEntityPersister(this.associatedEntityName);
    }

    public IType GetIdentifierOrUniqueKeyType(IMapping factory)
    {
      if (this.IsReferenceToPrimaryKey)
        return this.GetIdentifierType(factory);
      IType identifierOrUniqueKeyType = factory.GetReferencedPropertyType(this.GetAssociatedEntityName(), this.uniqueKeyPropertyName);
      if (identifierOrUniqueKeyType.IsEntityType)
        identifierOrUniqueKeyType = ((EntityType) identifierOrUniqueKeyType).GetIdentifierOrUniqueKeyType(factory);
      return identifierOrUniqueKeyType;
    }

    public string GetIdentifierOrUniqueKeyPropertyName(IMapping factory)
    {
      return this.IsReferenceToPrimaryKey ? factory.GetIdentifierPropertyName(this.GetAssociatedEntityName()) : this.uniqueKeyPropertyName;
    }

    internal virtual IType GetIdentifierType(IMapping factory)
    {
      return factory.GetIdentifierType(this.GetAssociatedEntityName());
    }

    internal virtual IType GetIdentifierType(ISessionImplementor session)
    {
      return this.GetIdentifierType((IMapping) session.Factory);
    }

    protected object ResolveIdentifier(object id, ISessionImplementor session)
    {
      string associatedEntityName = this.GetAssociatedEntityName();
      bool flag = this.unwrapProxy && session.Factory.GetEntityPersister(associatedEntityName).IsInstrumented(session.EntityMode);
      object entity = session.InternalLoad(associatedEntityName, id, this.eager, this.IsNullable && !flag);
      if (entity.IsProxy())
        ((INHibernateProxy) entity).HibernateLazyInitializer.Unwrap = flag;
      return entity;
    }

    public override object ResolveIdentifier(
      object value,
      ISessionImplementor session,
      object owner)
    {
      if (this.IsNotEmbedded(session))
        return value;
      if (value == null)
        return (object) null;
      if (this.IsNull(owner, session))
        return (object) null;
      return this.IsReferenceToPrimaryKey ? this.ResolveIdentifier(value, session) : this.LoadByUniqueKey(this.GetAssociatedEntityName(), this.uniqueKeyPropertyName, value, session);
    }

    public virtual string GetAssociatedEntityName(ISessionFactoryImplementor factory)
    {
      return this.GetAssociatedEntityName();
    }

    public string GetAssociatedEntityName() => this.associatedEntityName;

    public abstract ForeignKeyDirection ForeignKeyDirection { get; }

    public abstract bool UseLHSPrimaryKey { get; }

    public string LHSPropertyName => (string) null;

    public string RHSUniqueKeyPropertyName => this.uniqueKeyPropertyName;

    public virtual string PropertyName => (string) null;

    public override int GetHashCode(
      object x,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      IEntityPersister entityPersister = factory.GetEntityPersister(this.associatedEntityName);
      if (!entityPersister.CanExtractIdOutOfEntity)
        return this.GetHashCode(x, entityMode);
      object x1 = !x.IsProxy() ? entityPersister.GetIdentifier(x, entityMode) : (x as INHibernateProxy).HibernateLazyInitializer.Identifier;
      return entityPersister.IdentifierType.GetHashCode(x1, entityMode, factory);
    }

    public abstract bool IsAlwaysDirtyChecked { get; }

    public bool IsReferenceToPrimaryKey => string.IsNullOrEmpty(this.uniqueKeyPropertyName);

    public bool IsEmbeddedInXML => this.isEmbeddedInXML;

    public string GetOnCondition(
      string alias,
      ISessionFactoryImplementor factory,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      return this.IsReferenceToPrimaryKey ? string.Empty : this.GetAssociatedJoinable(factory).FilterFragment(alias, enabledFilters);
    }

    public override IType GetSemiResolvedType(ISessionFactoryImplementor factory)
    {
      return factory.GetEntityPersister(this.associatedEntityName).IdentifierType;
    }

    public object LoadByUniqueKey(
      string entityName,
      string uniqueKeyPropertyName,
      object key,
      ISessionImplementor session)
    {
      ISessionFactoryImplementor factory = session.Factory;
      IUniqueKeyLoadable entityPersister = (IUniqueKeyLoadable) factory.GetEntityPersister(entityName);
      EntityUniqueKey euk = new EntityUniqueKey(entityName, uniqueKeyPropertyName, key, this.GetIdentifierOrUniqueKeyType((IMapping) factory), session.EntityMode, session.Factory);
      IPersistenceContext persistenceContext = session.PersistenceContext;
      try
      {
        object impl = persistenceContext.GetEntity(euk) ?? entityPersister.LoadByUniqueKey(uniqueKeyPropertyName, key, session);
        return impl == null ? (object) null : persistenceContext.ProxyFor(impl);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(factory.SQLExceptionConverter, ex, "Error performing LoadByUniqueKey");
      }
    }

    public override int Compare(object x, object y, EntityMode? entityMode)
    {
      IComparable comparable1 = x as IComparable;
      IComparable comparable2 = y as IComparable;
      if (comparable1 != null)
        return comparable1.CompareTo(y);
      return comparable2 != null ? -comparable2.CompareTo(x) : 0;
    }

    private System.Type DetermineAssociatedEntityClass()
    {
      try
      {
        return ReflectHelper.ClassForFullName(this.GetAssociatedEntityName());
      }
      catch (Exception ex)
      {
        return typeof (IDictionary);
      }
    }

    public override void SetToXMLNode(
      XmlNode node,
      object value,
      ISessionFactoryImplementor factory)
    {
      if (!this.isEmbeddedInXML)
      {
        this.GetIdentifierType((IMapping) factory).SetToXMLNode(node, value, factory);
      }
      else
      {
        XmlNode xmlNode = (XmlNode) value;
        AbstractType.ReplaceNode(node, xmlNode);
      }
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.GetAssociatedEntityName() + (object) ')';
    }

    public override bool IsXMLElement => this.isEmbeddedInXML;
  }
}
