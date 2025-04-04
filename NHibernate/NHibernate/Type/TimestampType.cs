// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TimestampType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class TimestampType : PrimitiveType, IVersionType, IType, ICacheAssembler, ILiteralType
  {
    public TimestampType()
      : base(SqlTypeFactory.DateTime)
    {
    }

    public override object Get(IDataReader rs, int index) => (object) Convert.ToDateTime(rs[index]);

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override System.Type ReturnedClass => typeof (DateTime);

    public override void Set(IDbCommand st, object value, int index)
    {
      ((IDataParameter) st.Parameters[index]).Value = value is DateTime ? value : (object) DateTime.Now;
    }

    public override string Name => "Timestamp";

    public override string ToString(object val) => ((DateTime) val).ToShortTimeString();

    public override object FromStringValue(string xml) => (object) DateTime.Parse(xml);

    public object Next(object current, ISessionImplementor session) => this.Seed(session);

    public static DateTime Round(DateTime value, long resolution)
    {
      return value.AddTicks(-(value.Ticks % resolution));
    }

    public virtual object Seed(ISessionImplementor session)
    {
      return session == null ? (object) DateTime.Now : (object) TimestampType.Round(DateTime.Now, session.Factory.Dialect.TimestampResolutionInTicks);
    }

    public IComparer Comparator => (IComparer) Comparer.DefaultInvariant;

    public object StringToObject(string xml) => (object) DateTime.Parse(xml);

    public override System.Type PrimitiveClass => typeof (DateTime);

    public override object DefaultValue => (object) DateTime.MinValue;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return '\''.ToString() + value.ToString() + (object) '\'';
    }
  }
}
