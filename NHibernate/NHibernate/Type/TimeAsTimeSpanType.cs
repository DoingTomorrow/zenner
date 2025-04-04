// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TimeAsTimeSpanType
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
  public class TimeAsTimeSpanType : PrimitiveType, IVersionType, IType, ICacheAssembler
  {
    private static readonly DateTime BaseDateValue = new DateTime(1753, 1, 1);

    public TimeAsTimeSpanType()
      : base(SqlTypeFactory.Time)
    {
    }

    public override string Name => "TimeAsTimeSpan";

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        object r = rs[index];
        return r is TimeSpan timeSpan ? (object) timeSpan : (object) ((DateTime) r).TimeOfDay;
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[index]), ex);
      }
    }

    public override object Get(IDataReader rs, string name)
    {
      try
      {
        object r = rs[name];
        return r is TimeSpan timeSpan ? (object) timeSpan : (object) ((DateTime) r).TimeOfDay;
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[name]), ex);
      }
    }

    public override void Set(IDbCommand st, object value, int index)
    {
      DateTime dateTime = TimeAsTimeSpanType.BaseDateValue.AddTicks(((TimeSpan) value).Ticks);
      ((IDataParameter) st.Parameters[index]).Value = (object) dateTime;
    }

    public override System.Type ReturnedClass => typeof (TimeSpan);

    public override string ToString(object val) => ((TimeSpan) val).Ticks.ToString();

    public object Next(object current, ISessionImplementor session) => this.Seed(session);

    public virtual object Seed(ISessionImplementor session)
    {
      return (object) new TimeSpan(DateTime.Now.Ticks);
    }

    public object StringToObject(string xml) => (object) TimeSpan.Parse(xml);

    public IComparer Comparator => (IComparer) Comparer<TimeSpan>.Default;

    public override object FromStringValue(string xml) => (object) TimeSpan.Parse(xml);

    public override System.Type PrimitiveClass => typeof (TimeSpan);

    public override object DefaultValue => (object) TimeSpan.Zero;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return '\''.ToString() + ((TimeSpan) value).Ticks.ToString() + (object) '\'';
    }
  }
}
