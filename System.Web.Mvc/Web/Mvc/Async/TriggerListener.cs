// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.TriggerListener
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  internal sealed class TriggerListener
  {
    private readonly Trigger _activateTrigger;
    private readonly SingleEntryGate _continuationFiredGate = new SingleEntryGate();
    private readonly Trigger _setContinuationTrigger;
    private volatile Action _continuation;
    private int _outstandingTriggers;

    public TriggerListener()
    {
      this._activateTrigger = this.CreateTrigger();
      this._setContinuationTrigger = this.CreateTrigger();
    }

    public void Activate() => this._activateTrigger.Fire();

    public Trigger CreateTrigger()
    {
      Interlocked.Increment(ref this._outstandingTriggers);
      SingleEntryGate triggerFiredGate = new SingleEntryGate();
      return new Trigger((Action) (() =>
      {
        if (!triggerFiredGate.TryEnter())
          return;
        this.HandleTriggerFired();
      }));
    }

    private void HandleTriggerFired()
    {
      if (Interlocked.Decrement(ref this._outstandingTriggers) != 0 || !this._continuationFiredGate.TryEnter())
        return;
      this._continuation();
    }

    public void SetContinuation(Action continuation)
    {
      if (continuation == null)
        return;
      this._continuation = continuation;
      this._setContinuationTrigger.Fire();
    }
  }
}
