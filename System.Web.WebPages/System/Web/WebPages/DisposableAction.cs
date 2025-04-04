// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DisposableAction
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  internal class DisposableAction : IDisposable
  {
    private Action _action;
    private bool _hasDisposed;

    public DisposableAction(Action action)
    {
      this._action = action != null ? action : throw new ArgumentNullException(nameof (action));
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      lock (this)
      {
        if (this._hasDisposed)
          return;
        this._hasDisposed = true;
        this._action();
      }
    }
  }
}
