// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.CharBooleanType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class CharBooleanType(AnsiStringFixedLengthSqlType sqlType) : BooleanType(sqlType)
  {
    protected abstract string TrueString { get; }

    protected abstract string FalseString { get; }

    public override object Get(IDataReader rs, int index)
    {
      string a = Convert.ToString(rs[index]);
      return a == null ? (object) null : (object) StringHelper.EqualsCaseInsensitive(a, this.TrueString);
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) this.ToCharacter(value);
    }

    private string ToCharacter(object value) => !(bool) value ? this.FalseString : this.TrueString;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + this.ToCharacter(value) + "'";
    }

    public override object StringToObject(string xml)
    {
      if (StringHelper.EqualsCaseInsensitive(this.TrueString, xml))
        return (object) true;
      if (StringHelper.EqualsCaseInsensitive(this.FalseString, xml))
        return (object) false;
      throw new HibernateException("Could not interpret: " + xml);
    }
  }
}
