// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.SliderHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class SliderHelper
  {
    public static readonly DependencyProperty ChangeValueByProperty = DependencyProperty.RegisterAttached("ChangeValueBy", typeof (MouseWheelChange), typeof (SliderHelper), new PropertyMetadata((object) MouseWheelChange.SmallChange));
    public static readonly DependencyProperty EnableMouseWheelProperty = DependencyProperty.RegisterAttached("EnableMouseWheel", typeof (MouseWheelState), typeof (SliderHelper), new PropertyMetadata((object) MouseWheelState.None, new PropertyChangedCallback(SliderHelper.OnEnableMouseWheelChanged)));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Slider))]
    public static MouseWheelChange GetChangeValueBy(Slider element)
    {
      return (MouseWheelChange) element.GetValue(SliderHelper.ChangeValueByProperty);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Slider))]
    public static MouseWheelState GetEnableMouseWheel(Slider element)
    {
      return (MouseWheelState) element.GetValue(SliderHelper.EnableMouseWheelProperty);
    }

    private static void OnEnableMouseWheelChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is Slider slider))
        return;
      SliderHelper.UnregisterEvents(slider);
      if ((MouseWheelState) e.NewValue == MouseWheelState.None)
        return;
      SliderHelper.RegisterEvents(slider);
    }

    private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      Slider slider = (Slider) sender;
      if (!slider.IsFocused && !MouseWheelState.MouseHover.Equals(slider.GetValue(SliderHelper.EnableMouseWheelProperty)))
        return;
      double num = (MouseWheelChange) slider.GetValue(SliderHelper.ChangeValueByProperty) == MouseWheelChange.LargeChange ? slider.LargeChange : slider.SmallChange;
      if (e.Delta > 0)
        slider.Value += num;
      else
        slider.Value -= num;
    }

    private static void OnUnloaded(object sender, RoutedEventArgs e)
    {
      SliderHelper.UnregisterEvents((Slider) sender);
    }

    public static void SetChangeValueBy(Slider element, MouseWheelChange value)
    {
      element.SetValue(SliderHelper.ChangeValueByProperty, (object) value);
    }

    public static void SetEnableMouseWheel(Slider element, MouseWheelState value)
    {
      element.SetValue(SliderHelper.EnableMouseWheelProperty, (object) value);
    }

    private static void UnregisterEvents(Slider slider)
    {
      slider.Unloaded -= new RoutedEventHandler(SliderHelper.OnUnloaded);
      slider.PreviewMouseWheel -= new MouseWheelEventHandler(SliderHelper.OnPreviewMouseWheel);
    }

    private static void RegisterEvents(Slider slider)
    {
      slider.Unloaded += new RoutedEventHandler(SliderHelper.OnUnloaded);
      slider.PreviewMouseWheel += new MouseWheelEventHandler(SliderHelper.OnPreviewMouseWheel);
    }
  }
}
