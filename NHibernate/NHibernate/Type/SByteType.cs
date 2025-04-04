// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.SByteType
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
  public class SByteType : 
    PrimitiveType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    private static readonly sbyte ZERO;

    public SByteType()
      : base(SqlTypeFactory.SByte)
    {
    }

    public override string Name => "SByte";

    public override object Get(IDataReader rs, int index)
    {
      try
      {
        return (object) Convert.ToSByte(rs[index]);
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[index]), ex);
      }
    }

    public override object Get(IDataReader rs, string name)
    {
      try
      {
        return (object) Convert.ToSByte(rs[name]);
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", rs[name]), ex);
      }
    }

    public override System.Type ReturnedClass => typeof (sbyte);

    public override void Set(IDbCommand rs, object value, int index)
    {
      ((IDataParameter) rs.Parameters[index]).Value = value;
    }

    public object StringToObject(string xml) => this.FromStringValue(xml);

    public override object FromStringValue(string xml) => (object) sbyte.Parse(xml);

    public virtual object Next(object current, ISessionImplementor session)
    {
      return (object) (sbyte) ((int) (sbyte) current + 1);
    }

    public virtual object Seed(ISessionImplementor session) => (object) (sbyte) 1;

    public IComparer Comparator => (IComparer) Comparer<sbyte>.Default;

    public override System.Type PrimitiveClass => typeof (sbyte);

    public override object DefaultValue => (object) SByteType.ZERO;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return value.ToString();
    }
  }
}
