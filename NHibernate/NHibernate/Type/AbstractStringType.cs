// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AbstractStringType
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
  public abstract class AbstractStringType : 
    ImmutableType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    public AbstractStringType(SqlType sqlType)
      : base(sqlType)
    {
    }

    public override void Set(IDbCommand cmd, object value, int index)
    {
      IDbDataParameter parameter = (IDbDataParameter) cmd.Parameters[index];
      parameter.Value = value;
      if (parameter.Size > 0 && ((string) value).Length > parameter.Size)
        throw new HibernateException("The length of the string value exceeds the length configured in the mapping/parameter.");
    }

    public override object Get(IDataReader rs, int index) => (object) Convert.ToString(rs[index]);

    public override object Get(IDataReader rs, string name) => (object) Convert.ToString(rs[name]);

    public override string ToString(object val) => (string) val;

    public override object FromStringValue(string xml) => (object) xml;

    public override System.Type ReturnedClass => typeof (string);

    public object StringToObject(string xml) => (object) xml;

    public string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + (string) value + "'";
    }
  }
}
