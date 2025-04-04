// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.VisibilityHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class VisibilityHelper
  {
    public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof (bool?), typeof (VisibilityHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(VisibilityHelper.IsVisibleChangedCallback)));
    public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.RegisterAttached("IsCollapsed", typeof (bool?), typeof (VisibilityHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(VisibilityHelper.IsCollapsedChangedCallback)));
    public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.RegisterAttached("IsHidden", typeof (bool?), typeof (VisibilityHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(VisibilityHelper.IsHiddenChangedCallback)));

    private static void IsVisibleChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement frameworkElement))
        return;
      bool? newValue = (bool?) e.NewValue;
      bool flag = true;
      int num = (newValue.GetValueOrDefault() == flag ? (newValue.HasValue ? 1 : 0) : 0) != 0 ? 0 : 2;
      frameworkElement.Visibility = (Visibility) num;
    }

    public static void SetIsVisible(DependencyObject element, bool? value)
    {
      element.SetValue(VisibilityHelper.IsVisibleProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static bool? GetIsVisible(DependencyObject element)
    {
      return (bool?) element.GetValue(VisibilityHelper.IsVisibleProperty);
    }

    private static void IsCollapsedChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement frameworkElement))
        return;
      bool? newValue = (bool?) e.NewValue;
      bool flag = true;
      int num = (newValue.GetValueOrDefault() == flag ? (newValue.HasValue ? 1 : 0) : 0) != 0 ? 2 : 0;
      frameworkElement.Visibility = (Visibility) num;
    }

    public static void SetIsCollapsed(DependencyObject element, bool? value)
    {
      element.SetValue(VisibilityHelper.IsCollapsedProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static bool? GetIsCollapsed(DependencyObject element)
    {
      return (bool?) element.GetValue(VisibilityHelper.IsCollapsedProperty);
    }

    private static void IsHiddenChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement frameworkElement))
        return;
      bool? newValue = (bool?) e.NewValue;
      bool flag = true;
      int num = (newValue.GetValueOrDefault() == flag ? (newValue.HasValue ? 1 : 0) : 0) != 0 ? 1 : 0;
      frameworkElement.Visibility = (Visibility) num;
    }

    public static void SetIsHidden(DependencyObject element, bool? value)
    {
      element.SetValue(VisibilityHelper.IsHiddenProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static bool? GetIsHidden(DependencyObject element)
    {
      return (bool?) element.GetValue(VisibilityHelper.IsHiddenProperty);
    }
  }
}
