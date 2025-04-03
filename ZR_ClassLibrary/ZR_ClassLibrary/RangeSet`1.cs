// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.RangeSet`1
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace ZR_ClassLibrary
{
  [DebuggerDisplay("{Start} - {End}")]
  [Serializable]
  public class RangeSet<T> : List<Range<T>> where T : IComparable<T>, new()
  {
    private T end;
    private T start;

    public T Start => this.start;

    public T End => this.end;

    public bool Contains(T valueToFind)
    {
      foreach (Range<T> range in (List<Range<T>>) this)
      {
        if (range.Contains(valueToFind))
          return true;
      }
      return false;
    }

    public new void Clear()
    {
      this.start = new T();
      this.end = new T();
      base.Clear();
    }

    public void Add(T startRange, T endRange) => this.Add(new Range<T>(startRange, endRange));

    public new void Add(Range<T> range)
    {
      this.UpdateStartAndEnd(range);
      base.Add(range);
    }

    public new void Insert(int index, Range<T> range)
    {
      this.UpdateStartAndEnd(range);
      base.Insert(index, range);
    }

    private void UpdateStartAndEnd(Range<T> range)
    {
      if (this.Count == 0)
      {
        this.start = range.Start;
        this.end = range.End;
      }
      if (range.Start.CompareTo(this.start) < 0)
        this.start = range.Start;
      if (range.End.CompareTo(this.end) <= 0)
        return;
      this.end = range.End;
    }
  }
}
