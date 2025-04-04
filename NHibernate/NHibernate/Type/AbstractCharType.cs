// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AbstractCharType
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
  public abstract class AbstractCharType : 
    PrimitiveType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    public AbstractCharType(SqlType sqlType)
      : base(sqlType)
    {
    }

    public override object DefaultValue => throw new NotSupportedException("not a valid id type");

    public override object Get(IDataReader rs, int index)
    {
      string str = Convert.ToString(rs[index]);
      return str.Length > 0 ? (object) str[0] : (object) char.MinValue;
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override System.Type PrimitiveClass => typeof (char);

    public override System.Type ReturnedClass => typeof (char);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) Convert.ToChar(value);
    }

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return '\''.ToString() + value.ToString() + (object) '\'';
    }

    public virtual object StringToObject(string xml)
    {
      return xml.Length == 1 ? (object) xml[0] : throw new MappingException("multiple or zero characters found parsing string");
    }

    public override object FromStringValue(string xml) => (object) xml[0];
  }
}
