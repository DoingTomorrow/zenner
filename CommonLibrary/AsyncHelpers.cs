// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.AsyncHelpers
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class AsyncHelpers
  {
    [Obsolete]
    public static void RunSync(Func<Task> task)
    {
      SynchronizationContext current = SynchronizationContext.Current;
      AsyncHelpers.ExclusiveSynchronizationContext synch = new AsyncHelpers.ExclusiveSynchronizationContext();
      SynchronizationContext.SetSynchronizationContext((SynchronizationContext) synch);
      synch.Post((SendOrPostCallback) (async _ =>
      {
        try
        {
          await task();
        }
        catch (Exception ex)
        {
          synch.InnerException = ex;
          throw;
        }
        finally
        {
          synch.EndMessageLoop();
        }
      }), (object) null);
      synch.BeginMessageLoop();
      SynchronizationContext.SetSynchronizationContext(current);
    }

    [Obsolete]
    public static T RunSync<T>(Func<Task<T>> task)
    {
      SynchronizationContext current = SynchronizationContext.Current;
      AsyncHelpers.ExclusiveSynchronizationContext synch = new AsyncHelpers.ExclusiveSynchronizationContext();
      SynchronizationContext.SetSynchronizationContext((SynchronizationContext) synch);
      T ret = default (T);
      synch.Post((SendOrPostCallback) (async _ =>
      {
        try
        {
          T obj = await task();
          ret = obj;
          obj = default (T);
        }
        catch (Exception ex)
        {
          synch.InnerException = ex;
          throw;
        }
        finally
        {
          synch.EndMessageLoop();
        }
      }), (object) null);
      synch.BeginMessageLoop();
      SynchronizationContext.SetSynchronizationContext(current);
      return ret;
    }

    private class ExclusiveSynchronizationContext : SynchronizationContext
    {
      private bool done;
      private readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
      private readonly Queue<Tuple<SendOrPostCallback, object>> items = new Queue<Tuple<SendOrPostCallback, object>>();

      public Exception InnerException { get; set; }

      public override void Send(SendOrPostCallback d, object state)
      {
        throw new NotSupportedException("We cannot send to our same thread");
      }

      public override void Post(SendOrPostCallback d, object state)
      {
        lock (this.items)
          this.items.Enqueue(Tuple.Create<SendOrPostCallback, object>(d, state));
        this.workItemsWaiting.Set();
      }

      public void EndMessageLoop()
      {
        this.Post((SendOrPostCallback) (_ => this.done = true), (object) null);
      }

      public void BeginMessageLoop()
      {
        while (!this.done)
        {
          Tuple<SendOrPostCallback, object> tuple = (Tuple<SendOrPostCallback, object>) null;
          lock (this.items)
          {
            if (this.items.Count > 0)
              tuple = this.items.Dequeue();
          }
          if (tuple != null)
          {
            tuple.Item1(tuple.Item2);
            if (this.InnerException != null)
              throw new AggregateException("AsyncHelpers.Run method threw an exception.", this.InnerException);
          }
          else
            this.workItemsWaiting.WaitOne();
        }
      }

      public override SynchronizationContext CreateCopy() => (SynchronizationContext) this;
    }
  }
}
