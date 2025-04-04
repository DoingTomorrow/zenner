// Decompiled with JetBrains decompiler
// Type: Fluent.FrameworkHelper
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public static class FrameworkHelper
  {
    public static readonly Version PresentationFrameworkVersion = Assembly.GetAssembly(typeof (Window)).GetName().Version;
    public static readonly DependencyProperty UseLayoutRoundingProperty = DependencyProperty.RegisterAttached("UseLayoutRounding", typeof (bool), typeof (FrameworkHelper), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(FrameworkHelper.OnUseLayoutRoundingChanged)));

    public static bool GetUseLayoutRounding(DependencyObject obj)
    {
      return (bool) obj.GetValue(FrameworkHelper.UseLayoutRoundingProperty);
    }

    public static void SetUseLayoutRounding(DependencyObject obj, bool value)
    {
      obj.SetValue(FrameworkHelper.UseLayoutRoundingProperty, (object) value);
    }

    private static void OnUseLayoutRoundingChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      d.SetValue(UIElement.SnapsToDevicePixelsProperty, (object) true);
      d.SetValue(FrameworkElement.UseLayoutRoundingProperty, (object) true);
      TextOptions.SetTextFormattingMode(d, TextFormattingMode.Display);
      RenderOptions.SetClearTypeHint(d, ClearTypeHint.Enabled);
    }
  }
}
