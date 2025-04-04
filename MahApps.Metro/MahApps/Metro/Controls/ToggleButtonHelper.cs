// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ToggleButtonHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class ToggleButtonHelper
  {
    public static readonly DependencyProperty ContentDirectionProperty = DependencyProperty.RegisterAttached("ContentDirection", typeof (FlowDirection), typeof (ToggleButtonHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlowDirection.LeftToRight, new PropertyChangedCallback(ToggleButtonHelper.ContentDirectionPropertyChanged)));

    [AttachedPropertyBrowsableForType(typeof (ToggleButton))]
    [AttachedPropertyBrowsableForType(typeof (RadioButton))]
    [Category("MahApps.Metro")]
    public static FlowDirection GetContentDirection(UIElement element)
    {
      return (FlowDirection) element.GetValue(ToggleButtonHelper.ContentDirectionProperty);
    }

    public static void SetContentDirection(UIElement element, FlowDirection value)
    {
      element.SetValue(ToggleButtonHelper.ContentDirectionProperty, (object) value);
    }

    private static void ContentDirectionPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ToggleButton))
        throw new InvalidOperationException("The property 'ContentDirection' may only be set on ToggleButton elements.");
    }
  }
}
