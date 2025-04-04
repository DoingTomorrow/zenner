// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.CultureInfoType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Globalization;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class CultureInfoType : ImmutableType, ILiteralType
  {
    internal CultureInfoType()
      : base((SqlType) new StringSqlType(5))
    {
    }

    public override object Get(IDataReader rs, int index)
    {
      string name = (string) NHibernateUtil.String.Get(rs, index);
      return name == null ? (object) null : (object) new CultureInfo(name);
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override void Set(IDbCommand cmd, object value, int index)
    {
      NHibernateUtil.String.Set(cmd, (object) ((CultureInfo) value).Name, index);
    }

    public override string ToString(object value) => ((CultureInfo) value).Name;

    public override object FromStringValue(string xml)
    {
      return (object) CultureInfo.CreateSpecificCulture(xml);
    }

    public override System.Type ReturnedClass => typeof (CultureInfo);

    public override string Name => "CultureInfo";

    public string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return ((ILiteralType) NHibernateUtil.String).ObjectToSQLString((object) value.ToString(), dialect);
    }
  }
}
