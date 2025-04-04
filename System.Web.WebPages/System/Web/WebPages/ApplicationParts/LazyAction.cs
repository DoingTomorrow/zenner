// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationParts.LazyAction
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages.ApplicationParts
{
  internal class LazyAction
  {
    private Lazy<object> _lazyAction;

    public LazyAction(Action action)
    {
      this._lazyAction = new Lazy<object>((Func<object>) (() =>
      {
        action();
        return (object) null;
      }));
    }

    public object EnsurePerformed() => this._lazyAction.Value;
  }
}
