// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TicksType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class TicksType : PrimitiveType, IVersionType, IType, ICacheAssembler, ILiteralType
  {
    public TicksType()
      : base(SqlTypeFactory.Int64)
    {
    }

    public override object Get(IDataReader rs, int index)
    {
      return (object) new DateTime(Convert.ToInt64(rs[index]));
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override System.Type ReturnedClass => typeof (DateTime);

    public override void Set(IDbCommand st, object value, int index)
    {
      ((IDataParameter) st.Parameters[index]).Value = (object) ((DateTime) value).Ticks;
    }

    public override string Name => "Ticks";

    public override string ToString(object val) => ((DateTime) val).Ticks.ToString();

    public override object FromStringValue(string xml) => (object) new DateTime(long.Parse(xml));

    public object Next(object current, ISessionImplementor session) => this.Seed(session);

    public virtual object Seed(ISessionImplementor session) => (object) DateTime.Now;

    public object StringToObject(string xml) => (object) long.Parse(xml);

    public IComparer Comparator => (IComparer) Comparer<DateTime>.Default;

    public override System.Type PrimitiveClass => typeof (DateTime);

    public override object DefaultValue => (object) DateTime.MinValue;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return '\''.ToString() + ((DateTime) value).Ticks.ToString() + (object) '\'';
    }
  }
}
