// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ICharStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

#nullable disable
namespace Antlr.Runtime
{
  public interface ICharStream : IIntStream
  {
    string Substring(int start, int length);

    int LT(int i);

    int Line { get; set; }

    int CharPositionInLine { get; set; }
  }
}
