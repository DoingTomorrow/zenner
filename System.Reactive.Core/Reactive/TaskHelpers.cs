// Decompiled with JetBrains decompiler
// Type: System.Reactive.TaskHelpers
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.PlatformServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive
{
  internal static class TaskHelpers
  {
    private static Lazy<ITaskServices> s_services = new Lazy<ITaskServices>(new Func<ITaskServices>(TaskHelpers.Initialize));

    private static ITaskServices Initialize()
    {
      return PlatformEnlightenmentProvider.Current.GetService<ITaskServices>() ?? (ITaskServices) new DefaultTaskServices();
    }

    public static bool TrySetCanceled<T>(this TaskCompletionSource<T> tcs, CancellationToken token)
    {
      return TaskHelpers.s_services.Value.TrySetCanceled<T>(tcs, token);
    }
  }
}
