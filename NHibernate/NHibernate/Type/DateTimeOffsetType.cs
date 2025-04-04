// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DateTimeOffsetType
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
  public class DateTimeOffsetType : 
    PrimitiveType,
    IIdentifierType,
    ILiteralType,
    IVersionType,
    IType,
    ICacheAssembler
  {
    public DateTimeOffsetType()
      : base(SqlTypeFactory.DateTimeOffSet)
    {
    }

    public override string Name => "DateTimeOffset";

    public override System.Type ReturnedClass => typeof (DateTimeOffset);

    public override System.Type PrimitiveClass => typeof (DateTimeOffset);

    public override object DefaultValue => throw new NotImplementedException();

    public IComparer Comparator => (IComparer) Comparer<DateTimeOffset>.Default;

    public override void Set(IDbCommand st, object value, int index)
    {
      DateTimeOffset dateTimeOffset = (DateTimeOffset) value;
      ((IDataParameter) st.Parameters[index]).Value = (object) new DateTimeOffset(dateTimeOffset.Ticks, dateTimeOffset.Offset);
    }

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        DateTimeOffset r = (DateTimeOffset) rs[index];
        return (object) new DateTimeOffset(r.Ticks, r.Offset);
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[index]), ex);
      }
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public object Next(object current, ISessionImplementor session) => this.Seed(session);

    public object Seed(ISessionImplementor session) => (object) DateTimeOffset.Now;

    public override bool IsEqual(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && ((DateTimeOffset) x).Equals((DateTimeOffset) y);
    }

    public object StringToObject(string xml)
    {
      return !string.IsNullOrEmpty(xml) ? this.FromStringValue(xml) : (object) null;
    }

    public override string ToString(object val) => ((DateTimeOffset) val).ToString();

    public override object FromStringValue(string xml) => (object) DateTimeOffset.Parse(xml);

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + (object) (DateTimeOffset) value + "'";
    }
  }
}
