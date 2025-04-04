// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AbstractDateTimeSpecificKindType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class AbstractDateTimeSpecificKindType : DateTimeType
  {
    protected abstract DateTimeKind DateTimeKind { get; }

    protected virtual DateTime CreateDateTime(DateTime dateValue)
    {
      return new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, dateValue.Hour, dateValue.Minute, dateValue.Second, this.DateTimeKind);
    }

    public override object FromStringValue(string xml)
    {
      return (object) DateTime.SpecifyKind(DateTime.Parse(xml), this.DateTimeKind);
    }

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      return 31 * base.GetHashCode(x, entityMode) + ((DateTime) x).Kind.GetHashCode();
    }

    public override bool IsEqual(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && base.IsEqual(x, y) && ((DateTime) x).Kind == ((DateTime) y).Kind;
    }

    public override void Set(IDbCommand st, object value, int index)
    {
      DateTime dateValue = (DateTime) value;
      ((IDataParameter) st.Parameters[index]).Value = (object) this.CreateDateTime(dateValue);
    }

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        return (object) this.CreateDateTime(Convert.ToDateTime(rs[index]));
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[index]), ex);
      }
    }
  }
}
