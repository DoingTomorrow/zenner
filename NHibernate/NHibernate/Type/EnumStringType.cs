// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.EnumStringType
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
  public abstract class EnumStringType(System.Type enumClass, int length) : AbstractEnumType((SqlType) SqlTypeFactory.GetString(length), enumClass)
  {
    public const int MaxLengthForEnumString = 255;

    protected EnumStringType(System.Type enumClass)
      : this(enumClass, (int) byte.MaxValue)
    {
    }

    public override string Name => "enumstring - " + this.ReturnedClass.Name;

    public virtual object GetInstance(object code)
    {
      try
      {
        return this.StringToObject(code as string);
      }
      catch (ArgumentException ex)
      {
        throw new HibernateException(string.Format("Can't Parse {0} as {1}", code, (object) this.ReturnedClass.Name), (Exception) ex);
      }
    }

    public virtual object GetValue(object code)
    {
      return code != null ? (object) Enum.Format(this.ReturnedClass, code, "G") : (object) string.Empty;
    }

    public override void Set(IDbCommand cmd, object value, int index)
    {
      IDataParameter parameter = (IDataParameter) cmd.Parameters[index];
      if (value == null)
        parameter.Value = (object) DBNull.Value;
      else
        parameter.Value = this.GetValue(value);
    }

    public override object Get(IDataReader rs, int index)
    {
      object r = rs[index];
      return r == DBNull.Value || r == null ? (object) null : this.GetInstance(r);
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override string ToString(object value)
    {
      return value != null ? this.GetValue(value).ToString() : (string) null;
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return cached == null ? (object) null : this.GetInstance(cached);
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return value != null ? this.GetValue(value) : (object) null;
    }

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return this.GetValue(value).ToString();
    }
  }
}
