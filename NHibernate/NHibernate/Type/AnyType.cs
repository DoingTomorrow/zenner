// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AnyType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class AnyType : 
    AbstractType,
    IAbstractComponentType,
    IAssociationType,
    IType,
    ICacheAssembler
  {
    private readonly IType identifierType;
    private readonly IType metaType;
    private static readonly string[] PROPERTY_NAMES = new string[2]
    {
      "class",
      "id"
    };

    internal AnyType(IType metaType, IType identifierType)
    {
      this.identifierType = identifierType;
      this.metaType = metaType;
    }

    internal AnyType()
      : this((IType) NHibernateUtil.String, (IType) NHibernateUtil.Serializable)
    {
    }

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return value;
    }

    public override int GetColumnSpan(IMapping session) => 2;

    public override string Name => "Object";

    public override bool IsMutable => false;

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      throw new NotSupportedException("object is a multicolumn type");
    }

    public override object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.ResolveAny((string) this.metaType.NullSafeGet(rs, names[0], session, owner), this.identifierType.NullSafeGet(rs, names[1], session, owner), session);
    }

    public override object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return (object) new AnyType.ObjectTypeCacheEntry((string) this.metaType.NullSafeGet(rs, names[0], session, owner), this.identifierType.NullSafeGet(rs, names[1], session, owner));
    }

    public override object ResolveIdentifier(
      object value,
      ISessionImplementor session,
      object owner)
    {
      AnyType.ObjectTypeCacheEntry objectTypeCacheEntry = (AnyType.ObjectTypeCacheEntry) value;
      return this.ResolveAny(objectTypeCacheEntry.entityName, objectTypeCacheEntry.id, session);
    }

    public override object SemiResolve(object value, ISessionImplementor session, object owner)
    {
      throw new NotSupportedException("any mappings may not form part of a property-ref");
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      object obj;
      string entityName;
      if (value == null)
      {
        obj = (object) null;
        entityName = (string) null;
      }
      else
      {
        entityName = session.BestGuessEntityName(value);
        obj = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityName, value, session);
      }
      if (settable == null || settable[0])
        this.metaType.NullSafeSet(st, (object) entityName, index, session);
      if (settable == null)
      {
        this.identifierType.NullSafeSet(st, obj, index + 1, session);
      }
      else
      {
        bool[] flagArray = new bool[settable.Length - 1];
        Array.Copy((Array) settable, 1, (Array) flagArray, 0, flagArray.Length);
        this.identifierType.NullSafeSet(st, obj, index + 1, flagArray, session);
      }
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      ISessionImplementor session)
    {
      this.NullSafeSet(st, value, index, (bool[]) null, session);
    }

    public override System.Type ReturnedClass => typeof (object);

    public override SqlType[] SqlTypes(IMapping mapping)
    {
      return ArrayHelper.Join(this.metaType.SqlTypes(mapping), this.identifierType.SqlTypes(mapping));
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      return value != null ? NHibernateUtil.Entity(NHibernateProxyHelper.GetClassWithoutInitializingProxy(value)).ToLoggableString(value, factory) : "null";
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory)
    {
      throw new NotSupportedException();
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return cached is AnyType.ObjectTypeCacheEntry objectTypeCacheEntry ? session.InternalLoad(objectTypeCacheEntry.entityName, objectTypeCacheEntry.id, false, false) : (object) null;
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return value != null ? (object) new AnyType.ObjectTypeCacheEntry(session.BestGuessEntityName(value), ForeignKeys.GetEntityIdentifierIfNotUnsaved(session.BestGuessEntityName(value), value, session)) : (object) null;
    }

    public override object Replace(
      object original,
      object current,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      if (original == null)
        return (object) null;
      string entityName = session.BestGuessEntityName(original);
      object identifierIfNotUnsaved = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityName, original, session);
      return session.InternalLoad(entityName, identifierIfNotUnsaved, false, false);
    }

    public override bool IsAnyType => true;

    public CascadeStyle GetCascadeStyle(int i) => CascadeStyle.None;

    public FetchMode GetFetchMode(int i) => FetchMode.Select;

    public bool IsEmbedded => false;

    public virtual bool IsEmbeddedInXML => false;

    public string[] PropertyNames => AnyType.PROPERTY_NAMES;

    public object GetPropertyValue(object component, int i, ISessionImplementor session)
    {
      return i != 0 ? AnyType.Id(component, session) : (object) session.BestGuessEntityName(component);
    }

    public object[] GetPropertyValues(object component, EntityMode entityMode)
    {
      throw new NotSupportedException();
    }

    public object[] GetPropertyValues(object component, ISessionImplementor session)
    {
      return new object[2]
      {
        (object) session.BestGuessEntityName(component),
        AnyType.Id(component, session)
      };
    }

    private static object Id(object component, ISessionImplementor session)
    {
      try
      {
        return ForeignKeys.GetEntityIdentifierIfNotUnsaved(session.BestGuessEntityName(component), component, session);
      }
      catch (TransientObjectException ex)
      {
        return (object) null;
      }
    }

    public IType[] Subtypes
    {
      get
      {
        return new IType[2]
        {
          this.metaType,
          this.identifierType
        };
      }
    }

    public void SetPropertyValues(object component, object[] values, EntityMode entityMode)
    {
      throw new NotSupportedException();
    }

    public override bool IsComponentType => true;

    public ForeignKeyDirection ForeignKeyDirection => ForeignKeyDirection.ForeignKeyFromParent;

    public override bool IsAssociationType => true;

    public bool UseLHSPrimaryKey => false;

    public IJoinable GetAssociatedJoinable(ISessionFactoryImplementor factory)
    {
      throw new InvalidOperationException("any types do not have a unique referenced persister");
    }

    public string[] GetReferencedColumns(ISessionFactoryImplementor factory)
    {
      throw new InvalidOperationException("any types do not have unique referenced columns");
    }

    public string GetAssociatedEntityName(ISessionFactoryImplementor factory)
    {
      throw new InvalidOperationException("any types do not have a unique referenced persister");
    }

    public string LHSPropertyName => (string) null;

    public string RHSUniqueKeyPropertyName => (string) null;

    public override int GetHashCode() => 1;

    public bool IsAlwaysDirtyChecked => false;

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return this.IsDirty(old, current, session);
    }

    public override bool IsModified(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      if (current == null)
        return old != null;
      if (old == null)
        return current != null;
      AnyType.ObjectTypeCacheEntry objectTypeCacheEntry = (AnyType.ObjectTypeCacheEntry) old;
      bool[] flagArray = new bool[checkable.Length - 1];
      Array.Copy((Array) checkable, 1, (Array) flagArray, 0, flagArray.Length);
      return checkable[0] && !objectTypeCacheEntry.entityName.Equals(session.BestGuessEntityName(current)) || this.identifierType.IsModified(objectTypeCacheEntry.id, AnyType.Id(current, session), flagArray, session);
    }

    public bool[] PropertyNullability => (bool[]) null;

    public string GetOnCondition(
      string alias,
      ISessionFactoryImplementor factory,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      throw new NotSupportedException();
    }

    public override int Compare(object x, object y, EntityMode? entityMode) => 0;

    public virtual bool IsMethodOf(MethodBase method) => false;

    public override bool IsSame(object x, object y, EntityMode entityMode) => x == y;

    public bool ReferenceToPrimaryKey => true;

    private object ResolveAny(string entityName, object id, ISessionImplementor session)
    {
      return entityName != null && id != null ? session.InternalLoad(entityName, id, false, false) : (object) null;
    }

    public override void SetToXMLNode(
      XmlNode xml,
      object value,
      ISessionFactoryImplementor factory)
    {
      throw new NotSupportedException("any types cannot be stringified");
    }

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      bool[] array = new bool[this.GetColumnSpan(mapping)];
      if (value != null)
        ArrayHelper.Fill<bool>(array, true);
      return array;
    }

    [Serializable]
    public sealed class ObjectTypeCacheEntry
    {
      internal string entityName;
      internal object id;

      internal ObjectTypeCacheEntry(string entityName, object id)
      {
        this.entityName = entityName;
        this.id = id;
      }
    }
  }
}
