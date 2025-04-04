// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.IIntStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  internal interface IIntStream
  {
    void Consume();

    int LA(int i);

    int Mark();

    int Index();

    void Rewind(int marker);

    void Rewind();

    void Release(int marker);

    void Seek(int index);

    [Obsolete("Please use property Count instead.")]
    int Size();

    int Count { get; }

    string SourceName { get; }
  }
}
