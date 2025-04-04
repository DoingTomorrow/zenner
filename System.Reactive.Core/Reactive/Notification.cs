// Decompiled with JetBrains decompiler
// Type: System.Reactive.Notification
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  public static class Notification
  {
    public static Notification<T> CreateOnNext<T>(T value)
    {
      return (Notification<T>) new Notification<T>.OnNextNotification(value);
    }

    public static Notification<T> CreateOnError<T>(Exception error)
    {
      return error != null ? (Notification<T>) new Notification<T>.OnErrorNotification(error) : throw new ArgumentNullException(nameof (error));
    }

    public static Notification<T> CreateOnCompleted<T>()
    {
      return (Notification<T>) new Notification<T>.OnCompletedNotification();
    }
  }
}
