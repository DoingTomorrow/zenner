// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.YesNoType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class YesNoType : CharBooleanType
  {
    public YesNoType()
      : base(new AnsiStringFixedLengthSqlType(1))
    {
    }

    protected override sealed string TrueString => "Y";

    protected override sealed string FalseString => "N";

    public override string Name => "YesNo";
  }
}
