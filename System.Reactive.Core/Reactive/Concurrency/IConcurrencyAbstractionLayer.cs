// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.IConcurrencyAbstractionLayer
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;

#nullable disable
namespace System.Reactive.Concurrency
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public interface IConcurrencyAbstractionLayer
  {
    IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime);

    IDisposable StartPeriodicTimer(Action action, TimeSpan period);

    IDisposable QueueUserWorkItem(Action<object> action, object state);

    void Sleep(TimeSpan timeout);

    IStopwatch StartStopwatch();

    bool SupportsLongRunning { get; }

    void StartThread(Action<object> action, object state);
  }
}
