// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.CircularQueue`1
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class CircularQueue<TType> where TType : class
  {
    private TType[] m_array;
    private int m_nextIndex;

    internal CircularQueue(int limit)
    {
      this.m_array = limit > 0 ? new TType[limit] : throw new ArgumentException("limit must be a positive integer", nameof (limit));
      this.m_nextIndex = 0;
    }

    internal void Add(TType item)
    {
      if ((object) item == null)
        return;
      this.m_array[this.m_nextIndex] = item;
      ++this.m_nextIndex;
      if (this.m_nextIndex < this.m_array.Length)
        return;
      this.m_nextIndex = 0;
    }

    internal void Reset()
    {
      this.m_array = new TType[this.m_array.Length];
      this.m_nextIndex = 0;
    }

    internal IEnumerable<TType> Enumerate()
    {
      int startIndex = this.m_nextIndex;
      --startIndex;
      if (startIndex < 0)
        startIndex = this.m_array.Length - 1;
      for (int count = this.m_array.Length; count > 0; --count)
      {
        TType item = this.m_array[startIndex];
        if ((object) item == null)
          break;
        yield return item;
        --startIndex;
        if (startIndex < 0)
          startIndex = this.m_array.Length - 1;
      }
    }
  }
}
