// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.OnScreenKeyEventArgs
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.Windows;

#nullable disable
namespace WpfKb.Controls
{
  public class OnScreenKeyEventArgs : RoutedEventArgs
  {
    public OnScreenKey OnScreenKey { get; private set; }

    public OnScreenKeyEventArgs(RoutedEvent routedEvent, OnScreenKey onScreenKey)
      : base(routedEvent)
    {
      this.OnScreenKey = onScreenKey;
    }
  }
}
