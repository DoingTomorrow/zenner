// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.QueryServices
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.PlatformServices;

#nullable disable
namespace System.Reactive.Linq
{
  internal static class QueryServices
  {
    private static IQueryServices s_services = QueryServices.Initialize();

    public static T GetQueryImpl<T>(T defaultInstance)
    {
      return QueryServices.s_services.Extend<T>(defaultInstance);
    }

    private static IQueryServices Initialize()
    {
      return PlatformEnlightenmentProvider.Current.GetService<IQueryServices>() ?? (IQueryServices) new DefaultQueryServices();
    }
  }
}
