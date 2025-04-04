// Decompiled with JetBrains decompiler
// Type: Fluent.SeparatorTabItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Fluent
{
  public class SeparatorTabItem : TabItem
  {
    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static SeparatorTabItem()
    {
      Type type = typeof (SeparatorTabItem);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      UIElement.IsEnabledProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) false, (PropertyChangedCallback) null, new CoerceValueCallback(SeparatorTabItem.CoerceIsEnabledAndTabStop)));
      Control.IsTabStopProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) false, (PropertyChangedCallback) null, new CoerceValueCallback(SeparatorTabItem.CoerceIsEnabledAndTabStop)));
      TabItem.IsSelectedProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(SeparatorTabItem.OnIsSelectedChanged)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (SeparatorTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(SeparatorTabItem.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (SeparatorTabItem));
      return basevalue;
    }

    private static void OnIsSelectedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue || !(((FrameworkElement) d).Parent is TabControl parent) || parent.Items.Count <= 1)
        return;
      parent.SelectedIndex = parent.SelectedIndex == parent.Items.Count - 1 ? parent.SelectedIndex - 1 : parent.SelectedIndex + 1;
    }

    private static object CoerceIsEnabledAndTabStop(DependencyObject d, object basevalue)
    {
      return (object) false;
    }
  }
}
