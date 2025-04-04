// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.TaskHelpers
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal static class TaskHelpers
  {
    private const int MAX_DELAY = 2147483647;

    public static Task Delay(TimeSpan delay, CancellationToken token)
    {
      if ((long) delay.TotalMilliseconds <= (long) int.MaxValue)
        return Task.Delay(delay, token);
      TimeSpan remainder = delay - TimeSpan.FromMilliseconds((double) int.MaxValue);
      return Task.Delay(int.MaxValue, token).ContinueWith<Task>((Func<Task, Task>) (_ => TaskHelpers.Delay(remainder, token)), TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }
  }
}
