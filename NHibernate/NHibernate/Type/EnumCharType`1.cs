// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.EnumCharType`1
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
  public class EnumCharType<T> : AbstractEnumType
  {
    public EnumCharType()
      : base((SqlType) new StringFixedLengthSqlType(1), typeof (T))
    {
    }

    public virtual object GetInstance(object code)
    {
      switch (code)
      {
        case string _:
          return this.GetInstanceFromString((string) code);
        case char c:
          return this.GetInstanceFromChar(c);
        default:
          throw new HibernateException(string.Format("Can't Parse {0} as {1}", code, (object) this.ReturnedClass.Name));
      }
    }

    private object GetInstanceFromString(string s)
    {
      if (s.Length == 0)
        throw new HibernateException(string.Format("Can't Parse empty string as {0}", (object) this.ReturnedClass.Name));
      if (s.Length == 1)
        return this.GetInstanceFromChar(s[0]);
      try
      {
        return Enum.Parse(this.ReturnedClass, s, false);
      }
      catch (ArgumentException ex1)
      {
        try
        {
          return Enum.Parse(this.ReturnedClass, s, true);
        }
        catch (ArgumentException ex2)
        {
          throw new HibernateException(string.Format("Can't Parse {0} as {1}", (object) s, (object) this.ReturnedClass.Name), (Exception) ex2);
        }
      }
    }

    private object GetInstanceFromChar(char c)
    {
      object instanceFromChar = Enum.ToObject(this.ReturnedClass, (ushort) c);
      if (Enum.IsDefined(this.ReturnedClass, instanceFromChar))
        return instanceFromChar;
      object obj = Enum.ToObject(this.ReturnedClass, (ushort) this.Alternate(c));
      return Enum.IsDefined(this.ReturnedClass, obj) ? obj : throw new HibernateException(string.Format("Can't Parse {0} as {1}", (object) c, (object) this.ReturnedClass.Name));
    }

    private char Alternate(char c) => !char.IsUpper(c) ? char.ToUpper(c) : char.ToLower(c);

    public virtual object GetValue(object instance)
    {
      return instance == null ? (object) null : (object) (char) (int) instance;
    }

    public override void Set(IDbCommand cmd, object value, int index)
    {
      IDataParameter parameter = (IDataParameter) cmd.Parameters[index];
      if (value == null)
        parameter.Value = (object) DBNull.Value;
      else
        parameter.Value = (object) ((char) (int) value).ToString();
    }

    public override object Get(IDataReader rs, int index)
    {
      object r = rs[index];
      return r == DBNull.Value || r == null ? (object) null : this.GetInstance(r);
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override string Name => "enumchar - " + this.ReturnedClass.Name;

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

    public override object FromStringValue(string xml) => this.GetInstance((object) xml);

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return '\''.ToString() + this.GetValue(value).ToString() + (object) '\'';
    }
  }
}
