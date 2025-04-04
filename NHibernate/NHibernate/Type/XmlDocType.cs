// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.XmlDocType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class XmlDocType : MutableType
  {
    public XmlDocType()
      : base((SqlType) new XmlSqlType())
    {
    }

    public XmlDocType(SqlType sqlType)
      : base(sqlType)
    {
    }

    public override string Name => "XmlDoc";

    public override System.Type ReturnedClass => typeof (XmlDocument);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) ((XmlNode) value).OuterXml;
    }

    public override object Get(IDataReader rs, int index)
    {
      return this.FromStringValue(Convert.ToString(rs.GetValue(index)));
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override string ToString(object val) => ((XmlNode) val)?.OuterXml;

    public override object FromStringValue(string xml)
    {
      if (xml == null)
        return (object) null;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      return (object) xmlDocument;
    }

    public override object DeepCopyNotNull(object value)
    {
      XmlDocument xmlDocument1 = (XmlDocument) value;
      XmlDocument xmlDocument2 = new XmlDocument();
      xmlDocument2.LoadXml(xmlDocument1.OuterXml);
      return (object) xmlDocument2;
    }

    public override bool IsEqual(object x, object y)
    {
      if (x == null && y == null)
        return true;
      return x != null && y != null && ((XmlNode) x).OuterXml == ((XmlNode) y).OuterXml;
    }
  }
}
