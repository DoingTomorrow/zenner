// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AsyncRequestQueue
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers
{
  internal class AsyncRequestQueue
  {
    private readonly Queue<AsyncLogEventInfo> _logEventInfoQueue = new Queue<AsyncLogEventInfo>();

    public AsyncRequestQueue(int requestLimit, AsyncTargetWrapperOverflowAction overflowAction)
    {
      this.RequestLimit = requestLimit;
      this.OnOverflow = overflowAction;
    }

    public int RequestLimit { get; set; }

    public AsyncTargetWrapperOverflowAction OnOverflow { get; set; }

    public int RequestCount
    {
      get
      {
        lock (this)
          return this._logEventInfoQueue.Count;
      }
    }

    public bool Enqueue(AsyncLogEventInfo logEventInfo)
    {
      lock (this)
      {
        if (this._logEventInfoQueue.Count >= this.RequestLimit)
        {
          InternalLogger.Debug("Async queue is full");
          switch (this.OnOverflow)
          {
            case AsyncTargetWrapperOverflowAction.Grow:
              InternalLogger.Debug("The overflow action is Grow, adding element anyway");
              break;
            case AsyncTargetWrapperOverflowAction.Discard:
              InternalLogger.Debug("Discarding one element from queue");
              this._logEventInfoQueue.Dequeue();
              break;
            case AsyncTargetWrapperOverflowAction.Block:
              while (this._logEventInfoQueue.Count >= this.RequestLimit)
              {
                InternalLogger.Debug("Blocking because the overflow action is Block...");
                Monitor.Wait((object) this);
                InternalLogger.Trace("Entered critical section.");
              }
              InternalLogger.Trace("Limit ok.");
              break;
          }
        }
        this._logEventInfoQueue.Enqueue(logEventInfo);
        return this._logEventInfoQueue.Count == 1;
      }
    }

    public AsyncLogEventInfo[] DequeueBatch(int count)
    {
      AsyncLogEventInfo[] asyncLogEventInfoArray;
      lock (this)
      {
        if (this._logEventInfoQueue.Count < count)
          count = this._logEventInfoQueue.Count;
        if (count == 0)
          return ArrayHelper.Empty<AsyncLogEventInfo>();
        asyncLogEventInfoArray = new AsyncLogEventInfo[count];
        for (int index = 0; index < count; ++index)
          asyncLogEventInfoArray[index] = this._logEventInfoQueue.Dequeue();
        if (this.OnOverflow == AsyncTargetWrapperOverflowAction.Block)
          Monitor.PulseAll((object) this);
      }
      return asyncLogEventInfoArray;
    }

    public void DequeueBatch(int count, IList<AsyncLogEventInfo> result)
    {
      lock (this)
      {
        if (this._logEventInfoQueue.Count < count)
          count = this._logEventInfoQueue.Count;
        for (int index = 0; index < count; ++index)
          result.Add(this._logEventInfoQueue.Dequeue());
        if (this.OnOverflow != AsyncTargetWrapperOverflowAction.Block)
          return;
        Monitor.PulseAll((object) this);
      }
    }

    public void Clear()
    {
      lock (this)
        this._logEventInfoQueue.Clear();
    }
  }
}
