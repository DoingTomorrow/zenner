// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlTypes.StringClobSqlType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.SqlTypes
{
  [Serializable]
  public class StringClobSqlType : StringSqlType
  {
    public StringClobSqlType()
      : base(1073741823)
    {
    }

    public StringClobSqlType(int length)
      : base(length)
    {
    }
  }
}
