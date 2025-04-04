// Decompiled with JetBrains decompiler
// Type: System.Reactive.ExceptionHelpers
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.PlatformServices;

#nullable disable
namespace System.Reactive
{
  internal static class ExceptionHelpers
  {
    private static Lazy<IExceptionServices> s_services = new Lazy<IExceptionServices>(new Func<IExceptionServices>(ExceptionHelpers.Initialize));

    public static void Throw(this Exception exception)
    {
      ExceptionHelpers.s_services.Value.Rethrow(exception);
    }

    public static void ThrowIfNotNull(this Exception exception)
    {
      if (exception == null)
        return;
      ExceptionHelpers.s_services.Value.Rethrow(exception);
    }

    private static IExceptionServices Initialize()
    {
      return PlatformEnlightenmentProvider.Current.GetService<IExceptionServices>() ?? (IExceptionServices) new DefaultExceptionServices();
    }
  }
}
