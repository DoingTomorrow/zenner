// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DateTimeType
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
  public class DateTimeType : 
    PrimitiveType,
    IIdentifierType,
    ILiteralType,
    IVersionType,
    IType,
    ICacheAssembler
  {
    private static readonly DateTime BaseDateValue = DateTime.MinValue;

    public DateTimeType()
      : base(SqlTypeFactory.DateTime)
    {
    }

    public DateTimeType(SqlType sqlTypeDateTime)
      : base(sqlTypeDateTime)
    {
    }

    public override string Name => "DateTime";

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        DateTime dateTime = Convert.ToDateTime(rs[index]);
        return (object) new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[index]), ex);
      }
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override System.Type ReturnedClass => typeof (DateTime);

    public override void Set(IDbCommand st, object value, int index)
    {
      DateTime dateTime = (DateTime) value;
      ((IDataParameter) st.Parameters[index]).Value = (object) new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }

    public virtual object Next(object current, ISessionImplementor session) => this.Seed(session);

    public virtual object Seed(ISessionImplementor session)
    {
      return (object) TimestampType.Round(DateTime.Now, 10000000L);
    }

    public override bool IsEqual(object x, object y)
    {
      if (x == y)
        return true;
      if (x == null || y == null)
        return false;
      DateTime dateTime1 = (DateTime) x;
      DateTime dateTime2 = (DateTime) y;
      if (dateTime1.Equals(dateTime2))
        return true;
      return dateTime1.Year == dateTime2.Year && dateTime1.Month == dateTime2.Month && dateTime1.Day == dateTime2.Day && dateTime1.Hour == dateTime2.Hour && dateTime1.Minute == dateTime2.Minute && dateTime1.Second == dateTime2.Second;
    }

    public virtual IComparer Comparator => (IComparer) Comparer<DateTime>.Default;

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      DateTime dateTime = (DateTime) x;
      return 31 * (31 * (31 * (31 * (31 * (31 * 1 + dateTime.Second) + dateTime.Minute) + dateTime.Hour) + dateTime.Day) + dateTime.Month) + dateTime.Year;
    }

    public override string ToString(object val) => ((DateTime) val).ToString();

    public object StringToObject(string xml)
    {
      return !string.IsNullOrEmpty(xml) ? this.FromStringValue(xml) : (object) null;
    }

    public override object FromStringValue(string xml) => (object) DateTime.Parse(xml);

    public override System.Type PrimitiveClass => typeof (DateTime);

    public override object DefaultValue => (object) DateTimeType.BaseDateValue;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + (object) (DateTime) value + "'";
    }
  }
}
