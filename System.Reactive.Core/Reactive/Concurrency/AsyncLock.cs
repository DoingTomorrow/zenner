// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.AsyncLock
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Concurrency
{
  public sealed class AsyncLock : IDisposable
  {
    private readonly Queue<Action> queue = new Queue<Action>();
    private bool isAcquired;
    private bool hasFaulted;

    public void Wait(Action action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      bool flag = false;
      lock (this.queue)
      {
        if (!this.hasFaulted)
        {
          this.queue.Enqueue(action);
          flag = !this.isAcquired;
          this.isAcquired = true;
        }
      }
      if (!flag)
        return;
      while (true)
      {
        Action action1 = (Action) null;
        lock (this.queue)
        {
          if (this.queue.Count > 0)
          {
            action1 = this.queue.Dequeue();
          }
          else
          {
            this.isAcquired = false;
            break;
          }
        }
        try
        {
          action1();
        }
        catch
        {
          lock (this.queue)
          {
            this.queue.Clear();
            this.hasFaulted = true;
          }
          throw;
        }
      }
    }

    public void Dispose()
    {
      lock (this.queue)
      {
        this.queue.Clear();
        this.hasFaulted = true;
      }
    }
  }
}
