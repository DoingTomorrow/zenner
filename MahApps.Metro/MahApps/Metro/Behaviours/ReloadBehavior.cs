// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.ReloadBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public static class ReloadBehavior
  {
    public static DependencyProperty OnDataContextChangedProperty = DependencyProperty.RegisterAttached("OnDataContextChanged", typeof (bool), typeof (ReloadBehavior), new PropertyMetadata(new PropertyChangedCallback(ReloadBehavior.OnDataContextChanged)));
    public static DependencyProperty OnSelectedTabChangedProperty = DependencyProperty.RegisterAttached("OnSelectedTabChanged", typeof (bool), typeof (ReloadBehavior), new PropertyMetadata(new PropertyChangedCallback(ReloadBehavior.OnSelectedTabChanged)));
    public static readonly DependencyProperty MetroContentControlProperty = DependencyProperty.RegisterAttached("MetroContentControl", typeof (ContentControl), typeof (ReloadBehavior), new PropertyMetadata((object) null));

    [Category("MahApps.Metro")]
    public static bool GetOnDataContextChanged(MetroContentControl element)
    {
      return (bool) element.GetValue(ReloadBehavior.OnDataContextChangedProperty);
    }

    public static void SetOnDataContextChanged(MetroContentControl element, bool value)
    {
      element.SetValue(ReloadBehavior.OnDataContextChangedProperty, (object) value);
    }

    private static void OnDataContextChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((FrameworkElement) d).DataContextChanged += new DependencyPropertyChangedEventHandler(ReloadBehavior.ReloadDataContextChanged);
    }

    private static void ReloadDataContextChanged(
      object sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((MetroContentControl) sender).Reload();
    }

    [Category("MahApps.Metro")]
    public static bool GetOnSelectedTabChanged(ContentControl element)
    {
      return (bool) element.GetValue(ReloadBehavior.OnDataContextChangedProperty);
    }

    public static void SetOnSelectedTabChanged(ContentControl element, bool value)
    {
      element.SetValue(ReloadBehavior.OnDataContextChangedProperty, (object) value);
    }

    private static void OnSelectedTabChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((FrameworkElement) d).Loaded += new RoutedEventHandler(ReloadBehavior.ReloadLoaded);
    }

    private static void ReloadLoaded(object sender, RoutedEventArgs e)
    {
      ContentControl contentControl = (ContentControl) sender;
      TabControl element = ReloadBehavior.Ancestors((DependencyObject) contentControl).OfType<TabControl>().FirstOrDefault<TabControl>();
      if (element == null)
        return;
      ReloadBehavior.SetMetroContentControl((UIElement) element, contentControl);
      element.SelectionChanged -= new SelectionChangedEventHandler(ReloadBehavior.ReloadSelectionChanged);
      element.SelectionChanged += new SelectionChangedEventHandler(ReloadBehavior.ReloadSelectionChanged);
    }

    private static IEnumerable<DependencyObject> Ancestors(DependencyObject obj)
    {
      for (DependencyObject parent = VisualTreeHelper.GetParent(obj); parent != null; parent = VisualTreeHelper.GetParent(obj))
      {
        yield return parent;
        obj = parent;
      }
    }

    private static void ReloadSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.OriginalSource != sender)
        return;
      ContentControl metroContentControl1 = ReloadBehavior.GetMetroContentControl((UIElement) sender);
      if (metroContentControl1 is MetroContentControl metroContentControl2)
        metroContentControl2.Reload();
      if (!(metroContentControl1 is TransitioningContentControl transitioningContentControl))
        return;
      transitioningContentControl.ReloadTransition();
    }

    public static void SetMetroContentControl(UIElement element, ContentControl value)
    {
      element.SetValue(ReloadBehavior.MetroContentControlProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static ContentControl GetMetroContentControl(UIElement element)
    {
      return (ContentControl) element.GetValue(ReloadBehavior.MetroContentControlProperty);
    }
  }
}
