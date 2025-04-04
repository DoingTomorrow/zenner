// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ICharStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal interface ICharStream : IIntStream
  {
    int Line { get; set; }

    int CharPositionInLine { get; set; }

    int LT(int i);

    string Substring(int start, int stop);
  }
}
