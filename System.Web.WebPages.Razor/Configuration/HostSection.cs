// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.Configuration.HostSection
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.Configuration;

#nullable disable
namespace System.Web.WebPages.Razor.Configuration
{
  public class HostSection : ConfigurationSection
  {
    public static readonly string SectionName = RazorWebSectionGroup.GroupName + "/host";
    private static readonly ConfigurationProperty _typeProperty = new ConfigurationProperty("factoryType", typeof (string), (object) null, ConfigurationPropertyOptions.IsRequired);
    private bool _factoryTypeSet;
    private string _factoryType;

    [ConfigurationProperty("factoryType", IsRequired = true, DefaultValue = null)]
    public string FactoryType
    {
      get => !this._factoryTypeSet ? (string) this[HostSection._typeProperty] : this._factoryType;
      set
      {
        this._factoryType = value;
        this._factoryTypeSet = true;
      }
    }
  }
}
