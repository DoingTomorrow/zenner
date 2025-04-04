// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.XDocType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Xml.Linq;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class XDocType : MutableType
  {
    public XDocType()
      : base((SqlType) new XmlSqlType())
    {
    }

    public XDocType(SqlType sqlType)
      : base(sqlType)
    {
    }

    public override string Name => "XDoc";

    public override System.Type ReturnedClass => typeof (XDocument);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) ((XNode) value).ToString(SaveOptions.DisableFormatting);
    }

    public override object Get(IDataReader rs, int index)
    {
      return this.FromStringValue(Convert.ToString(rs.GetValue(index)));
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override string ToString(object val) => val?.ToString();

    public override object FromStringValue(string xml)
    {
      return xml != null ? (object) XDocument.Parse(xml) : (object) null;
    }

    public override object DeepCopyNotNull(object value)
    {
      return (object) new XDocument((XDocument) value);
    }

    public override bool IsEqual(object x, object y)
    {
      if (x == null && y == null)
        return true;
      return x != null && y != null && XNode.DeepEquals((XNode) x, (XNode) y);
    }
  }
}
