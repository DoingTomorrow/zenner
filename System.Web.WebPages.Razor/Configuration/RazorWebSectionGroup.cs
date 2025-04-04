// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.Configuration;

#nullable disable
namespace System.Web.WebPages.Razor.Configuration
{
  public class RazorWebSectionGroup : ConfigurationSectionGroup
  {
    public static readonly string GroupName = "system.web.webPages.razor";
    private bool _hostSet;
    private bool _pagesSet;
    private HostSection _host;
    private RazorPagesSection _pages;

    [ConfigurationProperty("host", IsRequired = false)]
    public HostSection Host
    {
      get => !this._hostSet ? (HostSection) this.Sections["host"] : this._host;
      set
      {
        this._host = value;
        this._hostSet = true;
      }
    }

    [ConfigurationProperty("pages", IsRequired = false)]
    public RazorPagesSection Pages
    {
      get => !this._pagesSet ? (RazorPagesSection) this.Sections["pages"] : this._pages;
      set
      {
        this._pages = value;
        this._pagesSet = true;
      }
    }
  }
}
