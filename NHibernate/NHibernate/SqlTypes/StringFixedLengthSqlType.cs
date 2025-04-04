// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlTypes.StringFixedLengthSqlType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.SqlTypes
{
  [Serializable]
  public class StringFixedLengthSqlType : SqlType
  {
    public StringFixedLengthSqlType()
      : base(DbType.StringFixedLength)
    {
    }

    public StringFixedLengthSqlType(int length)
      : base(DbType.StringFixedLength, length)
    {
    }
  }
}
