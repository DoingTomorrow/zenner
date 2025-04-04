// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DateType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class DateType : 
    PrimitiveType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType,
    IParameterizedType
  {
    public const string BaseValueParameterName = "BaseValue";
    public static readonly DateTime BaseDateValue = new DateTime(1753, 1, 1);
    private DateTime customBaseDate = DateType.BaseDateValue;

    public DateType()
      : base(SqlTypeFactory.Date)
    {
    }

    public override string Name => "Date";

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        return (object) Convert.ToDateTime(rs[index]).Date;
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
      IDataParameter parameter = st.Parameters[index] as IDataParameter;
      DateTime dateTime = (DateTime) value;
      if (dateTime < this.customBaseDate)
      {
        parameter.Value = (object) DBNull.Value;
      }
      else
      {
        parameter.DbType = DbType.Date;
        parameter.Value = (object) dateTime.Date;
      }
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
      return dateTime1.Day == dateTime2.Day && dateTime1.Month == dateTime2.Month && dateTime1.Year == dateTime2.Year;
    }

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      DateTime dateTime = (DateTime) x;
      return 31 * (31 * (31 * 1 + dateTime.Day) + dateTime.Month) + dateTime.Year;
    }

    public override string ToString(object val) => ((DateTime) val).ToShortDateString();

    public override object FromStringValue(string xml) => (object) DateTime.Parse(xml);

    public object StringToObject(string xml)
    {
      return !string.IsNullOrEmpty(xml) ? this.FromStringValue(xml) : (object) null;
    }

    public override System.Type PrimitiveClass => typeof (DateTime);

    public override object DefaultValue => (object) this.customBaseDate;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return '\''.ToString() + ((DateTime) value).ToShortDateString() + (object) '\'';
    }

    public void SetParameterValues(IDictionary<string, string> parameters)
    {
      string s;
      if (parameters == null || !parameters.TryGetValue("BaseValue", out s))
        return;
      this.customBaseDate = DateTime.Parse(s);
    }
  }
}
