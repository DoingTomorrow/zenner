// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.TypeType
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
  public class TypeType : ImmutableType
  {
    internal TypeType()
      : base((SqlType) new StringSqlType())
    {
    }

    internal TypeType(StringSqlType sqlType)
      : base((SqlType) sqlType)
    {
    }

    public override SqlType SqlType => NHibernateUtil.String.SqlType;

    public override object Get(IDataReader rs, int index)
    {
      string classFullName = (string) NHibernateUtil.String.Get(rs, index);
      if (string.IsNullOrEmpty(classFullName))
        return (object) null;
      try
      {
        return (object) ReflectHelper.ClassForFullName(classFullName);
      }
      catch (Exception ex)
      {
        throw new HibernateException("Class not found: " + classFullName, ex);
      }
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override void Set(IDbCommand cmd, object value, int index)
    {
      NHibernateUtil.String.Set(cmd, (object) ((System.Type) value).AssemblyQualifiedName, index);
    }

    public override string ToString(object value) => ((System.Type) value).AssemblyQualifiedName;

    public override System.Type ReturnedClass => typeof (System.Type);

    public override string Name => "Type";

    public override object FromStringValue(string xml)
    {
      try
      {
        return (object) ReflectHelper.ClassForFullName(xml);
      }
      catch (Exception ex)
      {
        throw new HibernateException("could not parse xml:" + xml, ex);
      }
    }
  }
}
