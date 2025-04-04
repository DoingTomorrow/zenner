// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DoubleType
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
  public class DoubleType : PrimitiveType
  {
    public DoubleType()
      : base(SqlTypeFactory.Double)
    {
    }

    public DoubleType(SqlType sqlType)
      : base(sqlType)
    {
    }

    public override object Get(IDataReader rs, int index) => (object) Convert.ToDouble(rs[index]);

    public override object Get(IDataReader rs, string name) => (object) Convert.ToDouble(rs[name]);

    public override System.Type ReturnedClass => typeof (double);

    public override void Set(IDbCommand st, object value, int index)
    {
      (st.Parameters[index] as IDataParameter).Value = value;
    }

    public override string Name => "Double";

    public override object FromStringValue(string xml) => (object) double.Parse(xml);

    public override System.Type PrimitiveClass => typeof (double);

    public override object DefaultValue => (object) 0.0;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return value.ToString();
    }
  }
}
