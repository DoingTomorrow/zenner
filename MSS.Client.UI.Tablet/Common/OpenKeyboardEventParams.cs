// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.Common.OpenKeyboardEventParams
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Tablet.Common.Interfaces;
using System.Windows;

#nullable disable
namespace MSS.Client.UI.Tablet.Common
{
  public class OpenKeyboardEventParams : IEventParams
  {
    public FrameworkElement FrameworkElement { get; set; }

    public RoutedEventArgs RoutedEventArgs { get; set; }

    public object UserControlElement { get; set; }
  }
}
