// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.PrimitiveType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class PrimitiveType(SqlType sqlType) : ImmutableType(sqlType), ILiteralType
  {
    public abstract System.Type PrimitiveClass { get; }

    public abstract object DefaultValue { get; }

    public abstract string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect);

    public override string ToString(object val) => val.ToString();
  }
}
