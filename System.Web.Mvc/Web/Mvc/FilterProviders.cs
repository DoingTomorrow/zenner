// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FilterProviders
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public static class FilterProviders
  {
    static FilterProviders()
    {
      FilterProviders.Providers = new FilterProviderCollection();
      FilterProviders.Providers.Add((IFilterProvider) GlobalFilters.Filters);
      FilterProviders.Providers.Add((IFilterProvider) new FilterAttributeFilterProvider());
      FilterProviders.Providers.Add((IFilterProvider) new ControllerInstanceFilterProvider());
    }

    public static FilterProviderCollection Providers { get; private set; }
  }
}
