// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DecimalType
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
  public class DecimalType(SqlType sqlType) : PrimitiveType(sqlType), IIdentifierType, IType, ICacheAssembler
  {
    public DecimalType()
      : this(SqlTypeFactory.Decimal)
    {
    }

    public override object Get(IDataReader rs, int index) => (object) Convert.ToDecimal(rs[index]);

    public override object Get(IDataReader rs, string name) => (object) Convert.ToDecimal(rs[name]);

    public override System.Type ReturnedClass => typeof (Decimal);

    public override void Set(IDbCommand st, object value, int index)
    {
      ((IDataParameter) st.Parameters[index]).Value = value;
    }

    public override string Name => "Decimal";

    public override System.Type PrimitiveClass => typeof (Decimal);

    public override object DefaultValue => (object) 0M;

    public override object FromStringValue(string xml) => (object) Decimal.Parse(xml);

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return value.ToString();
    }

    public object StringToObject(string xml) => this.FromStringValue(xml);
  }
}
