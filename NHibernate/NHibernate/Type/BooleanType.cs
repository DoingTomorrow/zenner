// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.BooleanType
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
  public class BooleanType : 
    PrimitiveType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    public BooleanType()
      : base(SqlTypeFactory.Boolean)
    {
    }

    public BooleanType(AnsiStringFixedLengthSqlType sqlType)
      : base((SqlType) sqlType)
    {
    }

    public override object Get(IDataReader rs, int index) => (object) Convert.ToBoolean(rs[index]);

    public override object Get(IDataReader rs, string name) => (object) Convert.ToBoolean(rs[name]);

    public override System.Type PrimitiveClass => typeof (bool);

    public override System.Type ReturnedClass => typeof (bool);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) (bool) value;
    }

    public override string Name => "Boolean";

    public override object DefaultValue => (object) false;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return dialect.ToBooleanValueString((bool) value);
    }

    public virtual object StringToObject(string xml) => this.FromStringValue(xml);

    public override object FromStringValue(string xml) => (object) bool.Parse(xml);
  }
}
