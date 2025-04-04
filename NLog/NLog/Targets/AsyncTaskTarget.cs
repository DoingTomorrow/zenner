// Decompiled with JetBrains decompiler
// Type: NLog.Targets.AsyncTaskTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace NLog.Targets
{
  public abstract class AsyncTaskTarget : TargetWithContext
  {
    private readonly Timer _taskTimeoutTimer;
    private CancellationTokenSource _cancelTokenSource;
    private readonly Queue<AsyncLogEventInfo> _requestQueue;
    private readonly Action _taskStartNext;
    private readonly Action _taskCancelledToken;
    private readonly Action<Task, object> _taskCompletion;
    private Task _previousTask;

    [DefaultValue(150)]
    public int TaskTimeoutSeconds { get; set; }

    protected virtual TaskScheduler TaskScheduler => TaskScheduler.Default;

    protected AsyncTaskTarget()
    {
      this.TaskTimeoutSeconds = 150;
      this._taskStartNext = (Action) (() => this.TaskStartNext((Task) null));
      this._taskCompletion = new Action<Task, object>(this.TaskCompletion);
      this._taskCancelledToken = new Action(this.TaskCancelledToken);
      this._taskTimeoutTimer = new Timer(new TimerCallback(this.TaskTimeout), (object) null, -1, -1);
      this._cancelTokenSource = new CancellationTokenSource();
      this._cancelTokenSource.Token.Register(this._taskCancelledToken);
      this._requestQueue = new Queue<AsyncLogEventInfo>(10000);
    }

    protected abstract Task WriteAsyncTask(
      LogEventInfo logEvent,
      CancellationToken cancellationToken);

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      if (this._cancelTokenSource.IsCancellationRequested)
      {
        logEvent.Continuation((Exception) null);
      }
      else
      {
        this.MergeEventProperties(logEvent.LogEvent);
        this.PrecalculateVolatileLayouts(logEvent.LogEvent);
        this._requestQueue.Enqueue(logEvent);
        if (this._previousTask != null)
          return;
        this._previousTask = Task.Factory.StartNew(this._taskStartNext, this._cancelTokenSource.Token, TaskCreationOptions.None, this.TaskScheduler);
      }
    }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      if (this._previousTask == null)
      {
        InternalLogger.Debug<string>("{0} Flushing Nothing", this.Name);
        asyncContinuation((Exception) null);
      }
      else
      {
        InternalLogger.Debug<string, int>("{0} Flushing {1} items", this.Name, this._requestQueue.Count + 1);
        this._requestQueue.Enqueue(new AsyncLogEventInfo((LogEventInfo) null, asyncContinuation));
      }
    }

    protected override void CloseTarget()
    {
      this._taskTimeoutTimer.Change(-1, -1);
      this._cancelTokenSource.Cancel();
      this._requestQueue.Clear();
      this._previousTask = (Task) null;
      base.CloseTarget();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this._cancelTokenSource.Dispose();
      this._taskTimeoutTimer.WaitForDispose(TimeSpan.Zero);
    }

    private void TaskStartNext(Task previousTask)
    {
      AsyncLogEventInfo logEvent;
      do
      {
        lock (this.SyncRoot)
        {
          if (!this.IsInitialized || previousTask != null && previousTask != this._previousTask)
            break;
          if (this._requestQueue.Count == 0)
          {
            this._previousTask = (Task) null;
            break;
          }
          logEvent = this._requestQueue.Dequeue();
        }
      }
      while (!this.TaskCreation(logEvent));
    }

    private bool TaskCreation(AsyncLogEventInfo logEvent)
    {
      try
      {
        if (this._cancelTokenSource.IsCancellationRequested)
        {
          logEvent.Continuation((Exception) null);
          return false;
        }
        if (logEvent.LogEvent == null)
        {
          InternalLogger.Debug<string>("{0} Flush Completed", this.Name);
          logEvent.Continuation((Exception) null);
          return false;
        }
        Task task = this.WriteAsyncTask(logEvent.LogEvent, this._cancelTokenSource.Token);
        if (task == null)
        {
          InternalLogger.Debug<string>("{0} WriteAsyncTask returned null", this.Name);
        }
        else
        {
          lock (this.SyncRoot)
          {
            this._previousTask = task;
            if (this.TaskTimeoutSeconds > 0)
              this._taskTimeoutTimer.Change(this.TaskTimeoutSeconds * 1000, -1);
            this._previousTask.ContinueWith(this._taskCompletion, (object) logEvent.Continuation);
            if (this._previousTask.Status == TaskStatus.Created)
              this._previousTask.Start(this.TaskScheduler);
          }
          return true;
        }
      }
      catch (Exception ex)
      {
        try
        {
          InternalLogger.Error(ex, "{0} WriteAsyncTask failed on creation", (object) this.Name);
          logEvent.Continuation(ex);
        }
        catch
        {
        }
      }
      return false;
    }

    private void TaskCompletion(Task completedTask, object continuation)
    {
      try
      {
        if (completedTask == this._previousTask)
        {
          if (this.TaskTimeoutSeconds > 0)
            this._taskTimeoutTimer.Change(-1, -1);
        }
        else if (!this.IsInitialized)
          return;
        if (completedTask.IsCanceled)
        {
          if (completedTask.Exception != null)
            InternalLogger.Warn((Exception) completedTask.Exception, "{0} WriteAsyncTask was cancelled", (object) this.Name);
          else
            InternalLogger.Info<string>("{0} WriteAsyncTask was cancelled", this.Name);
        }
        else if (completedTask.Exception != null)
          InternalLogger.Warn((Exception) completedTask.Exception, "{0} WriteAsyncTask failed on completion", (object) this.Name);
        ((AsyncContinuation) continuation)((Exception) completedTask.Exception);
      }
      finally
      {
        this.TaskStartNext(completedTask);
      }
    }

    private void TaskTimeout(object state)
    {
      try
      {
        if (!this.IsInitialized)
          return;
        InternalLogger.Warn<string>("{0} WriteAsyncTask had timeout. Task will be cancelled.", this.Name);
        Task task = this._previousTask;
        try
        {
          lock (this.SyncRoot)
          {
            if (task != null && task == this._previousTask)
            {
              this._previousTask = (Task) null;
              this._cancelTokenSource.Cancel();
            }
            else
              task = (Task) null;
          }
          if (task != null)
          {
            if (task.Status != TaskStatus.Canceled)
            {
              if (task.Status != TaskStatus.Faulted)
              {
                if (task.Status != TaskStatus.RanToCompletion)
                {
                  if (!task.Wait(100))
                    InternalLogger.Debug<string, TaskStatus>("{0} WriteAsyncTask had timeout. Task did not cancel properly: {1}.", this.Name, task.Status);
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          object[] objArray = new object[1]
          {
            (object) this.Name
          };
          InternalLogger.Debug(ex, "{0} WriteAsyncTask had timeout. Task failed to cancel properly.", objArray);
        }
        if (task == null)
          return;
        this.TaskStartNext((Task) null);
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) this.Name
        };
        InternalLogger.Error(ex, "{0} WriteAsyncTask failed on timeout", objArray);
      }
    }

    private void TaskCancelledToken()
    {
      lock (this.SyncRoot)
      {
        if (!this.IsInitialized)
          return;
        this._cancelTokenSource = new CancellationTokenSource();
        this._cancelTokenSource.Token.Register(this._taskCancelledToken);
      }
    }
  }
}
