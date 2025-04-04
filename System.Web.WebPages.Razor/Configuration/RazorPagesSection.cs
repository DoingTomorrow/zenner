// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.Configuration.RazorPagesSection
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.Configuration;
using System.Web.Configuration;

#nullable disable
namespace System.Web.WebPages.Razor.Configuration
{
  public class RazorPagesSection : ConfigurationSection
  {
    public static readonly string SectionName = RazorWebSectionGroup.GroupName + "/pages";
    private static readonly ConfigurationProperty _pageBaseTypeProperty = new ConfigurationProperty("pageBaseType", typeof (string), (object) null, ConfigurationPropertyOptions.IsRequired);
    private static readonly ConfigurationProperty _namespacesProperty = new ConfigurationProperty("namespaces", typeof (NamespaceCollection), (object) null, ConfigurationPropertyOptions.IsRequired);
    private bool _pageBaseTypeSet;
    private bool _namespacesSet;
    private string _pageBaseType;
    private NamespaceCollection _namespaces;

    [ConfigurationProperty("pageBaseType", IsRequired = true)]
    public string PageBaseType
    {
      get
      {
        return !this._pageBaseTypeSet ? (string) this[RazorPagesSection._pageBaseTypeProperty] : this._pageBaseType;
      }
      set
      {
        this._pageBaseType = value;
        this._pageBaseTypeSet = true;
      }
    }

    [ConfigurationProperty("namespaces", IsRequired = true)]
    public NamespaceCollection Namespaces
    {
      get
      {
        return !this._namespacesSet ? (NamespaceCollection) this[RazorPagesSection._namespacesProperty] : this._namespaces;
      }
      set
      {
        this._namespaces = value;
        this._namespacesSet = true;
      }
    }
  }
}
