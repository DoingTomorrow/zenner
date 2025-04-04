// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GuidType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class GuidType : 
    PrimitiveType,
    IDiscriminatorType,
    IIdentifierType,
    IType,
    ICacheAssembler,
    ILiteralType
  {
    public GuidType()
      : base(SqlTypeFactory.Guid)
    {
    }

    public override object Get(IDataReader rs, int index)
    {
      if (rs.GetFieldType(index) == typeof (Guid))
        return (object) rs.GetGuid(index);
      return rs.GetFieldType(index) == typeof (byte[]) ? (object) new Guid((byte[]) rs[index]) : (object) new Guid(Convert.ToString(rs[index]));
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override System.Type ReturnedClass => typeof (Guid);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      IDataParameter parameter = (IDataParameter) cmd.Parameters[index];
      parameter.Value = parameter.DbType == DbType.Binary ? (object) ((Guid) value).ToByteArray() : value;
    }

    public override string Name => "Guid";

    public override object FromStringValue(string xml) => (object) new Guid(xml);

    public object StringToObject(string xml)
    {
      return !string.IsNullOrEmpty(xml) ? this.FromStringValue(xml) : (object) null;
    }

    public override System.Type PrimitiveClass => typeof (Guid);

    public override object DefaultValue => (object) Guid.Empty;

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return "'" + value + "'";
    }
  }
}
