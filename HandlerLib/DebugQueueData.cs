// Decompiled with JetBrains decompiler
// Type: HandlerLib.DebugQueueData
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace HandlerLib
{
  public class DebugQueueData
  {
    public SortedList<int, string> QueueEvents;
    public List<DebugQueueEntry> QueueEntries;

    public DebugQueueData()
    {
      this.QueueEntries = new List<DebugQueueEntry>();
      this.QueueEntries.Add(new DebugQueueEntry("Test1", new DateTime(2023, 1, 1), (ushort) 0));
      this.QueueEntries.Add(new DebugQueueEntry("Test1", new DateTime(2023, 1, 1), (ushort) 5));
      this.QueueEntries.Add(new DebugQueueEntry("Test2", new DateTime(2023, 1, 1), (ushort) 7));
      this.QueueEntries.Add(new DebugQueueEntry("Test1", new DateTime(2023, 1, 1), (ushort) 9));
      this.QueueEntries.Sort();
    }

    public DebugQueueData(
      SortedList<int, string> queueEvents,
      byte[] buffer,
      int offset,
      int byteSize)
    {
      this.QueueEvents = queueEvents;
      this.QueueEntries = new List<DebugQueueEntry>();
      int num = byteSize / 12;
      for (int index = 0; index < num; ++index)
      {
        this.QueueEntries.Add(new DebugQueueEntry(this.QueueEvents, buffer, offset));
        offset += 12;
      }
      this.QueueEntries.Sort();
    }
  }
}
