// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.PlatformEnlightenmentProvider
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class PlatformEnlightenmentProvider
  {
    private static readonly IPlatformEnlightenmentProvider s_current = PlatformEnlightenmentProvider.CreatePlatformProvider();

    public static IPlatformEnlightenmentProvider Current => PlatformEnlightenmentProvider.s_current;

    private static IPlatformEnlightenmentProvider CreatePlatformProvider()
    {
      Type type = Type.GetType("System.Reactive.PlatformServices.CurrentPlatformEnlightenmentProvider, " + new AssemblyName(typeof (IPlatformEnlightenmentProvider).Assembly.FullName)
      {
        Name = "System.Reactive.PlatformServices"
      }.FullName, false);
      return type != (Type) null ? (IPlatformEnlightenmentProvider) Activator.CreateInstance(type) : (IPlatformEnlightenmentProvider) new DefaultPlatformEnlightenmentProvider();
    }
  }
}
