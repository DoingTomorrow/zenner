// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TimeType
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
  public class TimeType : PrimitiveType, IIdentifierType, IType, ICacheAssembler, ILiteralType
  {
    private static readonly DateTime BaseDateValue = new DateTime(1753, 1, 1);

    public TimeType()
      : base(SqlTypeFactory.Time)
    {
    }

    public override string Name => "Time";

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        if (rs[index] is TimeSpan)
        {
          TimeSpan r = (TimeSpan) rs[index];
          return (object) TimeType.BaseDateValue.AddTicks(r.Ticks);
        }
        DateTime dateTime = Convert.ToDateTime(rs[index]);
        return (object) new DateTime(1753, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
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
      ((IDataParameter) st.Parameters[index]).Value = (DateTime) value >= TimeType.BaseDateValue ? value : (object) DBNull.Value;
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
      return dateTime1.Hour == dateTime2.Hour && dateTime1.Minute == dateTime2.Minute && dateTime1.Second == dateTime2.Second;
    }

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      DateTime dateTime = (DateTime) x;
      return 31 * (31 * (31 * 1 + dateTime.Second) + dateTime.Minute) + dateTime.Hour;
    }

    public override string ToString(object val) => ((DateTime) val).ToShortTimeString();

    public object StringToObject(string xml)
    {
      return !string.IsNullOrEmpty(xml) ? this.FromStringValue(xml) : (object) null;
    }

    public override object FromStringValue(string xml) => (object) DateTime.Parse(xml);

    public override System.Type PrimitiveClass => typeof (DateTime);

    public override object DefaultValue => (object) TimeType.BaseDateValue;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + ((DateTime) value).ToShortTimeString() + "'";
    }
  }
}
