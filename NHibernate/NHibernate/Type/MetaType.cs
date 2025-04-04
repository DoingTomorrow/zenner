// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.MetaType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class MetaType : AbstractType
  {
    private readonly IDictionary<object, string> values;
    private readonly IDictionary<string, object> keys;
    private readonly IType baseType;

    public MetaType(IDictionary<object, string> values, IType baseType)
    {
      this.baseType = baseType;
      this.values = values;
      this.keys = (IDictionary<string, object>) new Dictionary<string, object>();
      foreach (KeyValuePair<object, string> keyValuePair in (IEnumerable<KeyValuePair<object, string>>) values)
        this.keys[keyValuePair.Value] = keyValuePair.Key;
    }

    public override SqlType[] SqlTypes(IMapping mapping) => this.baseType.SqlTypes(mapping);

    public override int GetColumnSpan(IMapping mapping) => this.baseType.GetColumnSpan(mapping);

    public override System.Type ReturnedClass => typeof (string);

    public override object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      object key = this.baseType.NullSafeGet(rs, names, session, owner);
      return key != null ? (object) this.values[key] : (object) null;
    }

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      object key = this.baseType.NullSafeGet(rs, name, session, owner);
      return key != null ? (object) this.values[key] : (object) null;
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
      this.baseType.NullSafeSet(st, value == null ? (object) null : this.keys[(string) value], index, session);
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      return this.ToXMLString(value, factory);
    }

    public override string Name => this.baseType.Name;

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

    internal object GetMetaValue(string className) => this.keys[className];
  }
}
