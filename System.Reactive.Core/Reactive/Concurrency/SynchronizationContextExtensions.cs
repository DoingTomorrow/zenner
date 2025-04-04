// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SynchronizationContextExtensions
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal static class SynchronizationContextExtensions
  {
    public static void PostWithStartComplete<T>(
      this SynchronizationContext context,
      Action<T> action,
      T state)
    {
      context.OperationStarted();
      context.Post((SendOrPostCallback) (o =>
      {
        try
        {
          action((T) o);
        }
        finally
        {
          context.OperationCompleted();
        }
      }), (object) state);
    }

    public static void PostWithStartComplete(this SynchronizationContext context, Action action)
    {
      context.OperationStarted();
      context.Post((SendOrPostCallback) (_ =>
      {
        try
        {
          action();
        }
        finally
        {
          context.OperationCompleted();
        }
      }), (object) null);
    }
  }
}
