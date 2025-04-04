// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Range`1
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Diagnostics;

#nullable disable
namespace ZR_ClassLibrary
{
  [DebuggerDisplay("{Start} - {End}")]
  public class Range<T> where T : IComparable<T>
  {
    private T end;
    private T start;

    public Range(T start, T end)
    {
      if (start.CompareTo(end) <= 0)
      {
        this.start = start;
        this.end = end;
      }
      else
      {
        this.start = end;
        this.end = start;
      }
    }

    public T Start => this.start;

    public T End => this.end;

    public bool Contains(T valueToFind)
    {
      ref T local1 = ref valueToFind;
      T obj;
      if ((object) default (T) == null)
      {
        obj = local1;
        local1 = ref obj;
      }
      T start = this.Start;
      int num;
      if (local1.CompareTo(start) >= 0)
      {
        ref T local2 = ref valueToFind;
        obj = default (T);
        if ((object) obj == null)
        {
          obj = local2;
          local2 = ref obj;
        }
        T end = this.End;
        num = local2.CompareTo(end) <= 0 ? 1 : 0;
      }
      else
        num = 0;
      return num != 0;
    }
  }
}
