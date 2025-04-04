// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.OperationCounter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  public sealed class OperationCounter
  {
    private int _count;

    public event EventHandler Completed;

    public int Count => Thread.VolatileRead(ref this._count);

    private int AddAndExecuteCallbackIfCompleted(int value)
    {
      int num = Interlocked.Add(ref this._count, value);
      if (num == 0)
        this.OnCompleted();
      return num;
    }

    public int Decrement() => this.AddAndExecuteCallbackIfCompleted(-1);

    public int Decrement(int value) => this.AddAndExecuteCallbackIfCompleted(-value);

    public int Increment() => this.AddAndExecuteCallbackIfCompleted(1);

    public int Increment(int value) => this.AddAndExecuteCallbackIfCompleted(value);

    private void OnCompleted()
    {
      EventHandler completed = this.Completed;
      if (completed == null)
        return;
      completed((object) this, EventArgs.Empty);
    }
  }
}
