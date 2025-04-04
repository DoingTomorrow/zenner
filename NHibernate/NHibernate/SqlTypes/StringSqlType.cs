// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlTypes.StringSqlType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.SqlTypes
{
  [Serializable]
  public class StringSqlType : SqlType
  {
    public StringSqlType()
      : base(DbType.String)
    {
    }

    public StringSqlType(int length)
      : base(DbType.String, length)
    {
    }
  }
}
