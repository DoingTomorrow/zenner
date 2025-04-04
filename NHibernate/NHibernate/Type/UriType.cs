// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.UriType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class UriType : 
    ImmutableType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    public UriType()
      : base((SqlType) new StringSqlType())
    {
    }

    public UriType(SqlType sqlType)
      : base(sqlType)
    {
    }

    public override string Name => "Uri";

    public override System.Type ReturnedClass => typeof (Uri);

    public object StringToObject(string xml) => (object) new Uri(xml);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) this.ToString(value);
    }

    public override object Get(IDataReader rs, int index)
    {
      return this.StringToObject(Convert.ToString(rs[index]));
    }

    public override object Get(IDataReader rs, string name)
    {
      return this.StringToObject(Convert.ToString(rs[name]));
    }

    public override string ToString(object val) => ((Uri) val).OriginalString;

    public override object FromStringValue(string xml) => this.StringToObject(xml);

    public string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + ((Uri) value).OriginalString + "'";
    }
  }
}
