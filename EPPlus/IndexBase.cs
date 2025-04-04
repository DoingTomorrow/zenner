// Decompiled with JetBrains decompiler
// Type: IndexBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
internal class IndexBase : IComparable<IndexBase>
{
  internal short Index;

  public int CompareTo(IndexBase other)
  {
    if ((int) this.Index < (int) other.Index)
      return -1;
    return (int) this.Index <= (int) other.Index ? 0 : 1;
  }
}
