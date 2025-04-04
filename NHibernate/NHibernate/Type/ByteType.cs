// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ByteType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class ByteType : 
    PrimitiveType,
    IDiscriminatorType,
    IIdentifierType,
    ILiteralType,
    IVersionType,
    IType,
    ICacheAssembler
  {
    private static readonly byte ZERO;

    public ByteType()
      : base(SqlTypeFactory.Byte)
    {
    }

    public override object Get(IDataReader rs, int index) => (object) Convert.ToByte(rs[index]);

    public override object Get(IDataReader rs, string name) => (object) Convert.ToByte(rs[name]);

    public override System.Type ReturnedClass => typeof (byte);

    public override System.Type PrimitiveClass => typeof (byte);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = (object) Convert.ToByte(value);
    }

    public override string Name => "Byte";

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return value.ToString();
    }

    public virtual object StringToObject(string xml) => (object) byte.Parse(xml);

    public override object FromStringValue(string xml) => (object) byte.Parse(xml);

    public virtual object Next(object current, ISessionImplementor session)
    {
      return (object) (byte) ((uint) (byte) current + 1U);
    }

    public virtual object Seed(ISessionImplementor session) => (object) ByteType.ZERO;

    public IComparer Comparator => (IComparer) Comparer.DefaultInvariant;

    public override object DefaultValue => (object) ByteType.ZERO;
  }
}
