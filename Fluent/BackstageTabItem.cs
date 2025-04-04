// Decompiled with JetBrains decompiler
// Type: Fluent.BackstageTabItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  public class BackstageTabItem : ContentControl, IKeyTipedControl
  {
    public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof (BackstageTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(BackstageTabItem.OnIsSelectedChanged)));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (BackstageTabItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    [Category("Appearance")]
    [Bindable(true)]
    public bool IsSelected
    {
      get => (bool) this.GetValue(BackstageTabItem.IsSelectedProperty);
      set => this.SetValue(BackstageTabItem.IsSelectedProperty, (object) value);
    }

    internal BackstageTabControl TabControlParent
    {
      get
      {
        return ItemsControl.ItemsControlFromItemContainer((DependencyObject) this) as BackstageTabControl;
      }
    }

    public object Header
    {
      get => this.GetValue(BackstageTabItem.HeaderProperty);
      set => this.SetValue(BackstageTabItem.HeaderProperty, value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static BackstageTabItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BackstageTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BackstageTabItem)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (BackstageTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(BackstageTabItem.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (BackstageTabItem));
      return basevalue;
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      if (!this.IsSelected || this.TabControlParent == null)
        return;
      this.TabControlParent.SelectedContent = newContent;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (e.Source == this || !this.IsSelected)
      {
        if (this.TabControlParent != null && this.TabControlParent.SelectedItem is BackstageTabItem)
          ((BackstageTabItem) this.TabControlParent.SelectedItem).IsSelected = false;
        this.IsSelected = true;
      }
      e.Handled = true;
    }

    private static void OnIsSelectedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      BackstageTabItem source = (BackstageTabItem) d;
      if ((bool) e.NewValue)
      {
        if (source.Parent is BackstageTabControl parent && parent.SelectedItem != source)
        {
          if (parent.SelectedItem is BackstageTabItem)
            (parent.SelectedItem as BackstageTabItem).IsSelected = false;
          parent.SelectedItem = (object) source;
        }
        source.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, (object) source));
      }
      else
        source.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, (object) source));
    }

    protected virtual void OnSelected(RoutedEventArgs e) => this.HandleIsSelectedChanged(e);

    protected virtual void OnUnselected(RoutedEventArgs e) => this.HandleIsSelectedChanged(e);

    private void HandleIsSelectedChanged(RoutedEventArgs e) => this.RaiseEvent(e);

    public void OnKeyTipPressed()
    {
      if (this.TabControlParent != null && this.TabControlParent.SelectedItem is RibbonTabItem)
        ((BackstageTabItem) this.TabControlParent.SelectedItem).IsSelected = false;
      this.IsSelected = true;
    }
  }
}
