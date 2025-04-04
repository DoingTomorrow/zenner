// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ConcurrencyAbstractionLayer
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.PlatformServices;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal static class ConcurrencyAbstractionLayer
  {
    public static IConcurrencyAbstractionLayer Current { get; } = ConcurrencyAbstractionLayer.Initialize();

    private static IConcurrencyAbstractionLayer Initialize()
    {
      return PlatformEnlightenmentProvider.Current.GetService<IConcurrencyAbstractionLayer>() ?? (IConcurrencyAbstractionLayer) new DefaultConcurrencyAbstractionLayer();
    }
  }
}
