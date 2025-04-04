// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.IToken
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal interface IToken
  {
    int Type { get; set; }

    int Line { get; set; }

    int CharPositionInLine { get; set; }

    int Channel { get; set; }

    int TokenIndex { get; set; }

    string Text { get; set; }
  }
}
