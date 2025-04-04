// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.AsyncManager
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  public class AsyncManager
  {
    private readonly SynchronizationContext _syncContext;
    private int _timeout = 45000;

    public AsyncManager()
      : this((SynchronizationContext) null)
    {
    }

    public AsyncManager(SynchronizationContext syncContext)
    {
      this._syncContext = syncContext ?? SynchronizationContextUtil.GetSynchronizationContext();
      this.OutstandingOperations = new OperationCounter();
      this.OutstandingOperations.Completed += (EventHandler) ((param0, param1) => this.Finish());
      this.Parameters = (IDictionary<string, object>) new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    public event EventHandler Finished;

    public OperationCounter OutstandingOperations { get; private set; }

    public IDictionary<string, object> Parameters { get; private set; }

    public int Timeout
    {
      get => this._timeout;
      set
      {
        this._timeout = value >= -1 ? value : throw Error.AsyncCommon_InvalidTimeout(nameof (value));
      }
    }

    public virtual void Finish()
    {
      EventHandler finished = this.Finished;
      if (finished == null)
        return;
      finished((object) this, EventArgs.Empty);
    }

    public virtual void Sync(Action action) => this._syncContext.Sync(action);
  }
}
