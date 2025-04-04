// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.SafeRaise
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace MahApps.Metro.Controls
{
  internal static class SafeRaise
  {
    public static void Raise(EventHandler eventToRaise, object sender)
    {
      if (eventToRaise == null)
        return;
      eventToRaise(sender, EventArgs.Empty);
    }

    public static void Raise(EventHandler<EventArgs> eventToRaise, object sender)
    {
      SafeRaise.Raise<EventArgs>(eventToRaise, sender, EventArgs.Empty);
    }

    public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
    {
      if (eventToRaise == null)
        return;
      eventToRaise(sender, args);
    }

    public static void Raise<T>(
      EventHandler<T> eventToRaise,
      object sender,
      SafeRaise.GetEventArgs<T> getEventArgs)
      where T : EventArgs
    {
      if (eventToRaise == null)
        return;
      eventToRaise(sender, getEventArgs());
    }

    public delegate T GetEventArgs<T>() where T : EventArgs;
  }
}
