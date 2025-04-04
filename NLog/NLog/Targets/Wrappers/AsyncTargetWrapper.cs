// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AsyncTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("AsyncWrapper", IsWrapper = true)]
  public class AsyncTargetWrapper : WrapperTargetBase
  {
    private readonly object _writeLockObject = new object();
    private readonly object _timerLockObject = new object();
    private Timer _lazyWriterTimer;
    private readonly ReusableAsyncLogEventList _reusableAsyncLogEventList = new ReusableAsyncLogEventList(200);
    private Action<object> _flushEventsInQueueDelegate;

    public AsyncTargetWrapper()
      : this((Target) null)
    {
    }

    public AsyncTargetWrapper(string name, Target wrappedTarget)
      : this(wrappedTarget)
    {
      this.Name = name;
    }

    public AsyncTargetWrapper(Target wrappedTarget)
      : this(wrappedTarget, 10000, AsyncTargetWrapperOverflowAction.Discard)
    {
    }

    public AsyncTargetWrapper(
      Target wrappedTarget,
      int queueLimit,
      AsyncTargetWrapperOverflowAction overflowAction)
    {
      this.RequestQueue = new AsyncRequestQueue(10000, AsyncTargetWrapperOverflowAction.Discard);
      this.TimeToSleepBetweenBatches = 50;
      this.BatchSize = 200;
      this.FullBatchSizeWriteLimit = 5;
      this.WrappedTarget = wrappedTarget;
      this.QueueLimit = queueLimit;
      this.OverflowAction = overflowAction;
    }

    [DefaultValue(200)]
    public int BatchSize { get; set; }

    [DefaultValue(50)]
    public int TimeToSleepBetweenBatches { get; set; }

    [DefaultValue("Discard")]
    public AsyncTargetWrapperOverflowAction OverflowAction
    {
      get => this.RequestQueue.OnOverflow;
      set => this.RequestQueue.OnOverflow = value;
    }

    [DefaultValue(10000)]
    public int QueueLimit
    {
      get => this.RequestQueue.RequestLimit;
      set => this.RequestQueue.RequestLimit = value;
    }

    [DefaultValue(5)]
    public int FullBatchSizeWriteLimit { get; set; }

    internal AsyncRequestQueue RequestQueue { get; private set; }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      if (this._flushEventsInQueueDelegate == null)
        this._flushEventsInQueueDelegate = new Action<object>(this.FlushEventsInQueue);
      AsyncHelpers.StartAsyncTask(this._flushEventsInQueueDelegate, (object) asyncContinuation);
    }

    protected override void InitializeTarget()
    {
      base.InitializeTarget();
      if (!this.OptimizeBufferReuse && this.WrappedTarget != null && this.WrappedTarget.OptimizeBufferReuse)
        this.OptimizeBufferReuse = this.GetType() == typeof (AsyncTargetWrapper);
      this.RequestQueue.Clear();
      InternalLogger.Trace<string>("AsyncWrapper(Name={0}): Start Timer", this.Name);
      this._lazyWriterTimer = new Timer(new TimerCallback(this.ProcessPendingEvents), (object) null, -1, -1);
      this.StartLazyWriterTimer();
    }

    protected override void CloseTarget()
    {
      this.StopLazyWriterThread();
      if (Monitor.TryEnter(this._writeLockObject, 500))
      {
        try
        {
          this.WriteEventsInQueue(int.MaxValue, "Closing Target");
        }
        finally
        {
          Monitor.Exit(this._writeLockObject);
        }
      }
      base.CloseTarget();
    }

    protected virtual void StartLazyWriterTimer()
    {
      lock (this._timerLockObject)
      {
        if (this._lazyWriterTimer == null)
          return;
        if (this.TimeToSleepBetweenBatches <= 0)
        {
          InternalLogger.Trace<string>("AsyncWrapper(Name={0}): Throttled timer scheduled", this.Name);
          this._lazyWriterTimer.Change(1, -1);
        }
        else
          this._lazyWriterTimer.Change(this.TimeToSleepBetweenBatches, -1);
      }
    }

    protected virtual bool StartInstantWriterTimer()
    {
      bool flag = false;
      try
      {
        flag = Monitor.TryEnter(this._writeLockObject);
        if (flag)
        {
          lock (this._timerLockObject)
          {
            if (this._lazyWriterTimer != null)
            {
              this._lazyWriterTimer.Change(0, -1);
              return true;
            }
          }
        }
        return false;
      }
      finally
      {
        if (flag)
          Monitor.Exit(this._writeLockObject);
      }
    }

    protected virtual void StopLazyWriterThread()
    {
      lock (this._timerLockObject)
      {
        Timer lazyWriterTimer = this._lazyWriterTimer;
        if (lazyWriterTimer == null)
          return;
        this._lazyWriterTimer = (Timer) null;
        lazyWriterTimer.WaitForDispose(TimeSpan.FromSeconds(1.0));
      }
    }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      this.MergeEventProperties(logEvent.LogEvent);
      this.PrecalculateVolatileLayouts(logEvent.LogEvent);
      if (!this.RequestQueue.Enqueue(logEvent) || this.TimeToSleepBetweenBatches > 0)
        return;
      this.StartInstantWriterTimer();
    }

    protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
    {
      try
      {
        this.Write(logEvent);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        else
          logEvent.Continuation(ex);
      }
    }

    private void ProcessPendingEvents(object state)
    {
      if (this._lazyWriterTimer == null)
        return;
      bool flag = false;
      try
      {
        lock (this._writeLockObject)
        {
          if (this.WriteEventsInQueue(this.BatchSize, "Timer") == this.BatchSize)
            flag = true;
          if (!flag || this.TimeToSleepBetweenBatches > 0)
            return;
          this.StartInstantWriterTimer();
        }
      }
      catch (Exception ex)
      {
        flag = false;
        InternalLogger.Error(ex, "AsyncWrapper(Name={0}): Error in lazy writer timer procedure.", (object) this.Name);
        if (!ex.MustBeRethrownImmediately())
          return;
        throw;
      }
      finally
      {
        if (this.TimeToSleepBetweenBatches <= 0)
        {
          if (!flag && this.RequestQueue.RequestCount > 0)
            this.StartLazyWriterTimer();
        }
        else
          this.StartLazyWriterTimer();
      }
    }

    private void FlushEventsInQueue(object state)
    {
      try
      {
        AsyncContinuation asyncContinuation = state as AsyncContinuation;
        lock (this._writeLockObject)
        {
          this.WriteEventsInQueue(int.MaxValue, "Flush Async");
          if (asyncContinuation != null)
            base.FlushAsync(asyncContinuation);
        }
        if (this.TimeToSleepBetweenBatches > 0 || this.RequestQueue.RequestCount <= 0)
          return;
        this.StartLazyWriterTimer();
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "AsyncWrapper(Name={0}): Error in flush procedure.", (object) this.Name);
        if (!ex.MustBeRethrownImmediately())
          return;
        throw;
      }
    }

    private int WriteEventsInQueue(int batchSize, string reason)
    {
      if (this.WrappedTarget == null)
      {
        InternalLogger.Error<string>("AsyncWrapper(Name={0}): WrappedTarget is NULL", this.Name);
        return 0;
      }
      int num = 0;
      for (int index = 0; index < this.FullBatchSizeWriteLimit; ++index)
      {
        if (!this.OptimizeBufferReuse || batchSize == int.MaxValue)
        {
          AsyncLogEventInfo[] asyncLogEventInfoArray = this.RequestQueue.DequeueBatch(batchSize);
          if (asyncLogEventInfoArray.Length != 0)
          {
            if (reason != null)
              InternalLogger.Trace<string, int, string>("AsyncWrapper(Name={0}): Writing {1} events ({2})", this.Name, asyncLogEventInfoArray.Length, reason);
            this.WrappedTarget.WriteAsyncLogEvents(asyncLogEventInfoArray);
          }
          num = asyncLogEventInfoArray.Length;
        }
        else
        {
          using (ReusableObjectCreator<IList<AsyncLogEventInfo>>.LockOject lockOject = this._reusableAsyncLogEventList.Allocate())
          {
            IList<AsyncLogEventInfo> result = lockOject.Result;
            this.RequestQueue.DequeueBatch(batchSize, result);
            if (result.Count > 0)
            {
              if (reason != null)
                InternalLogger.Trace<string, int, string>("AsyncWrapper(Name={0}): Writing {1} events ({2})", this.Name, result.Count, reason);
              this.WrappedTarget.WriteAsyncLogEvents(result);
            }
            num = result.Count;
          }
        }
        if (num < batchSize)
          break;
      }
      return num;
    }
  }
}
