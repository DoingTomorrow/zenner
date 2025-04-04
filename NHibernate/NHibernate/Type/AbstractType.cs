// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AbstractType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class AbstractType : IType, ICacheAssembler
  {
    public virtual bool IsAssociationType => false;

    public virtual bool IsXMLElement => false;

    public virtual bool IsCollectionType => false;

    public virtual bool IsComponentType => false;

    public virtual bool IsEntityType => false;

    public virtual object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return value == null ? (object) null : this.DeepCopy(value, session.EntityMode, session.Factory);
    }

    public virtual object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return cached == null ? (object) null : this.DeepCopy(cached, session.EntityMode, session.Factory);
    }

    public virtual void BeforeAssemble(object cached, ISessionImplementor session)
    {
    }

    public virtual bool IsDirty(object old, object current, ISessionImplementor session)
    {
      return !this.IsSame(old, current, session.EntityMode);
    }

    public virtual object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, names, session, owner);
    }

    public virtual object ResolveIdentifier(
      object value,
      ISessionImplementor session,
      object owner)
    {
      return value;
    }

    public virtual object SemiResolve(object value, ISessionImplementor session, object owner)
    {
      return value;
    }

    public virtual bool IsAnyType => false;

    public virtual bool IsModified(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return this.IsDirty(old, current, session);
    }

    public override bool Equals(object obj)
    {
      if (obj == this)
        return true;
      return obj != null && obj.GetType() == this.GetType();
    }

    public override int GetHashCode() => this.GetType().GetHashCode();

    public abstract object DeepCopy(
      object val,
      EntityMode entityMode,
      ISessionFactoryImplementor factory);

    public abstract SqlType[] SqlTypes(IMapping mapping);

    public abstract int GetColumnSpan(IMapping mapping);

    public virtual object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection)
    {
      return !(!this.IsAssociationType ? ForeignKeyDirection.ForeignKeyFromParent.Equals((object) foreignKeyDirection) : ((IAssociationType) this).ForeignKeyDirection == foreignKeyDirection) ? target : this.Replace(original, target, session, owner, copyCache);
    }

    public virtual bool IsSame(object x, object y, EntityMode entityMode)
    {
      return this.IsEqual(x, y, entityMode);
    }

    public virtual bool IsEqual(object x, object y, EntityMode entityMode)
    {
      return EqualsHelper.Equals(x, y);
    }

    public virtual bool IsEqual(
      object x,
      object y,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return this.IsEqual(x, y, entityMode);
    }

    public virtual int GetHashCode(object x, EntityMode entityMode) => x.GetHashCode();

    public virtual int GetHashCode(
      object x,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return this.GetHashCode(x, entityMode);
    }

    public virtual int Compare(object x, object y, EntityMode? entityMode)
    {
      IComparable comparable1 = x as IComparable;
      IComparable comparable2 = y as IComparable;
      if (comparable1 != null)
        return comparable1.CompareTo(y);
      return comparable2 != null ? comparable2.CompareTo(x) : throw new HibernateException(string.Format("Can't compare {0} with {1}; you must implement System.IComparable", (object) x.GetType(), (object) y.GetType()));
    }

    public virtual IType GetSemiResolvedType(ISessionFactoryImplementor factory) => (IType) this;

    protected internal static void ReplaceNode(XmlNode container, XmlNode value)
    {
      if (container == value)
        return;
      container.ParentNode.ReplaceChild(value, container);
    }

    public abstract object Replace(
      object original,
      object current,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready);

    public abstract void SetToXMLNode(
      XmlNode node,
      object value,
      ISessionFactoryImplementor factory);

    public abstract object FromXMLNode(XmlNode xml, IMapping factory);

    public abstract bool[] ToColumnNullness(object value, IMapping mapping);

    public abstract bool IsMutable { get; }

    public abstract string Name { get; }

    public abstract object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner);

    public abstract object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner);

    public abstract void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session);

    public abstract void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      ISessionImplementor session);

    public abstract System.Type ReturnedClass { get; }

    public abstract string ToLoggableString(object value, ISessionFactoryImplementor factory);

    public abstract bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session);
  }
}
