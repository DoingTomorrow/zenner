// Decompiled with JetBrains decompiler
// Type: Fluent.QuickAccessMenuItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Target")]
  public class QuickAccessMenuItem : MenuItem
  {
    internal Ribbon Ribbon;
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof (Target), typeof (Control), typeof (QuickAccessMenuItem), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(QuickAccessMenuItem.OnTargetChanged)));

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static QuickAccessMenuItem()
    {
      System.Windows.Controls.MenuItem.IsCheckableProperty.AddOwner(typeof (QuickAccessMenuItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    }

    public QuickAccessMenuItem()
    {
      this.Checked += new RoutedEventHandler(this.OnChecked);
      this.Unchecked += new RoutedEventHandler(this.OnUnchecked);
      this.Loaded += new RoutedEventHandler(this.OnFirstLoaded);
      this.Loaded += new RoutedEventHandler(this.OnItemLoaded);
    }

    public Control Target
    {
      get => (Control) this.GetValue(QuickAccessMenuItem.TargetProperty);
      set => this.SetValue(QuickAccessMenuItem.TargetProperty, (object) value);
    }

    private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      QuickAccessMenuItem target = (QuickAccessMenuItem) d;
      IRibbonControl newValue = e.NewValue as IRibbonControl;
      if (target.Header == null && newValue != null)
        RibbonControl.Bind((object) newValue, (FrameworkElement) target, "Header", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      if (newValue != null && LogicalTreeHelper.GetParent((DependencyObject) newValue) == null)
        target.AddLogicalChild((object) newValue);
      if (!(e.OldValue is IRibbonControl oldValue) || LogicalTreeHelper.GetParent((DependencyObject) oldValue) != target)
        return;
      target.RemoveLogicalChild((object) oldValue);
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        if (this.Target == null || LogicalTreeHelper.GetParent((DependencyObject) this.Target) != this)
          return base.LogicalChildren;
        return new ArrayList() { (object) this.Target }.GetEnumerator();
      }
    }

    private void OnChecked(object sender, RoutedEventArgs e)
    {
      if (this.Ribbon == null)
        return;
      this.Ribbon.AddToQuickAccessToolBar((UIElement) this.Target);
    }

    private void OnUnchecked(object sender, RoutedEventArgs e)
    {
      if (!this.IsLoaded || this.Ribbon == null)
        return;
      this.Ribbon.RemoveFromQuickAccessToolBar((UIElement) this.Target);
    }

    private void OnItemLoaded(object sender, RoutedEventArgs e)
    {
      if (!this.IsLoaded || this.Ribbon == null)
        return;
      this.IsChecked = this.Ribbon.IsInQuickAccessToolBar((UIElement) this.Target);
    }

    private void OnFirstLoaded(object sender, RoutedEventArgs e)
    {
      this.Loaded -= new RoutedEventHandler(this.OnFirstLoaded);
      if (!this.IsChecked || this.Ribbon == null)
        return;
      this.Ribbon.AddToQuickAccessToolBar((UIElement) this.Target);
    }
  }
}
