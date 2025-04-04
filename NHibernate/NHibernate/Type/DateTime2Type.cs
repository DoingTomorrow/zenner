// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DateTime2Type
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class DateTime2Type : DateTimeType
  {
    internal DateTime2Type()
      : base(SqlTypeFactory.DateTime2)
    {
    }

    public override string Name => "DateTime2";

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        return (object) Convert.ToDateTime(rs[index]);
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[index]), ex);
      }
    }

    public override void Set(IDbCommand st, object value, int index)
    {
      ((IDataParameter) st.Parameters[index]).Value = (object) (DateTime) value;
    }

    public override bool IsEqual(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && x.Equals(y);
    }

    public override object Next(object current, ISessionImplementor session) => this.Seed(session);

    public override object Seed(ISessionImplementor session) => (object) DateTime.Now;
  }
}
