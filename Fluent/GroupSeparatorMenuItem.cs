// Decompiled with JetBrains decompiler
// Type: Fluent.GroupSeparatorMenuItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Header")]
  public class GroupSeparatorMenuItem : MenuItem
  {
    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static GroupSeparatorMenuItem()
    {
      Type type = typeof (GroupSeparatorMenuItem);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(GroupSeparatorMenuItem.OnCoerceStyle)));
      UIElement.IsEnabledProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) false, (PropertyChangedCallback) null, new CoerceValueCallback(GroupSeparatorMenuItem.CoerceIsEnabledAndTabStop)));
      Control.IsTabStopProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) false, (PropertyChangedCallback) null, new CoerceValueCallback(GroupSeparatorMenuItem.CoerceIsEnabledAndTabStop)));
    }

    private static object CoerceIsEnabledAndTabStop(DependencyObject d, object basevalue)
    {
      return (object) false;
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (GroupSeparatorMenuItem));
      return basevalue;
    }
  }
}
