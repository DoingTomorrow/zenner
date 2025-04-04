// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AnsiStringType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class AnsiStringType : AbstractStringType
  {
    internal AnsiStringType()
      : base((SqlType) new AnsiStringSqlType())
    {
    }

    internal AnsiStringType(AnsiStringSqlType sqlType)
      : base((SqlType) sqlType)
    {
    }

    public override string Name => "AnsiString";
  }
}
