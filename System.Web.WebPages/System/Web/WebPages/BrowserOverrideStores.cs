// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.BrowserOverrideStores
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  public class BrowserOverrideStores
  {
    private static BrowserOverrideStores _instance = new BrowserOverrideStores();
    private BrowserOverrideStore _currentOverrideStore = (BrowserOverrideStore) new CookieBrowserOverrideStore();

    public static BrowserOverrideStore Current
    {
      get => BrowserOverrideStores._instance.CurrentInternal;
      set => BrowserOverrideStores._instance.CurrentInternal = value;
    }

    internal BrowserOverrideStore CurrentInternal
    {
      get => this._currentOverrideStore;
      set
      {
        this._currentOverrideStore = value ?? (BrowserOverrideStore) new RequestBrowserOverrideStore();
      }
    }
  }
}
