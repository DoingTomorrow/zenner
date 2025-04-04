// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.TabControlHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class TabControlHelper
  {
    public static readonly DependencyProperty IsUnderlinedProperty = DependencyProperty.RegisterAttached("IsUnderlined", typeof (bool), typeof (TabControlHelper), new PropertyMetadata((object) false));
    public static readonly DependencyProperty TransitionProperty = DependencyProperty.RegisterAttached("Transition", typeof (TransitionType), typeof (TabControlHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TabControl))]
    [AttachedPropertyBrowsableForType(typeof (TabItem))]
    public static bool GetIsUnderlined(UIElement element)
    {
      return (bool) element.GetValue(TabControlHelper.IsUnderlinedProperty);
    }

    public static void SetIsUnderlined(UIElement element, bool value)
    {
      element.SetValue(TabControlHelper.IsUnderlinedProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static TransitionType GetTransition(DependencyObject obj)
    {
      return (TransitionType) obj.GetValue(TabControlHelper.TransitionProperty);
    }

    public static void SetTransition(DependencyObject obj, TransitionType value)
    {
      obj.SetValue(TabControlHelper.TransitionProperty, (object) value);
    }
  }
}
