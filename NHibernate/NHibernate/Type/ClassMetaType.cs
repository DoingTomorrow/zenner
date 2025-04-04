// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ClassMetaType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class ClassMetaType : AbstractType
  {
    public override SqlType[] SqlTypes(IMapping mapping)
    {
      return new SqlType[1]{ NHibernateUtil.String.SqlType };
    }

    public override int GetColumnSpan(IMapping mapping) => 1;

    public override System.Type ReturnedClass => typeof (string);

    public override object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, names[0], session, owner);
    }

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      int ordinal = rs.GetOrdinal(name);
      if (rs.IsDBNull(ordinal))
        return (object) null;
      string str = (string) NHibernateUtil.String.Get(rs, ordinal);
      return !string.IsNullOrEmpty(str) ? (object) str : (object) null;
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      if (!settable[0])
        return;
      this.NullSafeSet(st, value, index, session);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      ISessionImplementor session)
    {
      if (value == null)
        ((IDataParameter) st.Parameters[index]).Value = (object) DBNull.Value;
      else
        NHibernateUtil.String.Set(st, value, index);
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      return this.ToXMLString(value, factory);
    }

    public override string Name => nameof (ClassMetaType);

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return value;
    }

    public override bool IsMutable => false;

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return checkable[0] && this.IsDirty(old, current, session);
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory)
    {
      return this.FromXMLString(xml.Value, factory);
    }

    public object FromXMLString(string xml, IMapping factory) => (object) xml;

    public override object Replace(
      object original,
      object current,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      return original;
    }

    public override void SetToXMLNode(
      XmlNode node,
      object value,
      ISessionFactoryImplementor factory)
    {
      node.Value = this.ToXMLString(value, factory);
    }

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      throw new NotSupportedException();
    }

    public string ToXMLString(object value, ISessionFactoryImplementor factory) => (string) value;
  }
}
