// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.WindowCommands
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class WindowCommands : ItemsControl, INotifyPropertyChanged
  {
    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof (Theme), typeof (Theme), typeof (WindowCommands), new PropertyMetadata((object) Theme.Light, new PropertyChangedCallback(WindowCommands.OnThemeChanged)));
    public static readonly DependencyProperty LightTemplateProperty = DependencyProperty.Register(nameof (LightTemplate), typeof (ControlTemplate), typeof (WindowCommands), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty DarkTemplateProperty = DependencyProperty.Register(nameof (DarkTemplate), typeof (ControlTemplate), typeof (WindowCommands), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ShowSeparatorsProperty = DependencyProperty.Register(nameof (ShowSeparators), typeof (bool), typeof (WindowCommands), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(WindowCommands.OnShowSeparatorsChanged)));
    public static readonly DependencyProperty ShowLastSeparatorProperty = DependencyProperty.Register(nameof (ShowLastSeparator), typeof (bool), typeof (WindowCommands), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(WindowCommands.OnShowLastSeparatorChanged)));
    public static readonly DependencyProperty SeparatorHeightProperty = DependencyProperty.Register(nameof (SeparatorHeight), typeof (int), typeof (WindowCommands), (PropertyMetadata) new FrameworkPropertyMetadata((object) 15, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
    private Window _parentWindow;

    public Theme Theme
    {
      get => (Theme) this.GetValue(WindowCommands.ThemeProperty);
      set => this.SetValue(WindowCommands.ThemeProperty, (object) value);
    }

    public ControlTemplate LightTemplate
    {
      get => (ControlTemplate) this.GetValue(WindowCommands.LightTemplateProperty);
      set => this.SetValue(WindowCommands.LightTemplateProperty, (object) value);
    }

    public ControlTemplate DarkTemplate
    {
      get => (ControlTemplate) this.GetValue(WindowCommands.DarkTemplateProperty);
      set => this.SetValue(WindowCommands.DarkTemplateProperty, (object) value);
    }

    public bool ShowSeparators
    {
      get => (bool) this.GetValue(WindowCommands.ShowSeparatorsProperty);
      set => this.SetValue(WindowCommands.ShowSeparatorsProperty, (object) value);
    }

    public bool ShowLastSeparator
    {
      get => (bool) this.GetValue(WindowCommands.ShowLastSeparatorProperty);
      set => this.SetValue(WindowCommands.ShowLastSeparatorProperty, (object) value);
    }

    public int SeparatorHeight
    {
      get => (int) this.GetValue(WindowCommands.SeparatorHeightProperty);
      set => this.SetValue(WindowCommands.SeparatorHeightProperty, (object) value);
    }

    private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      WindowCommands windowCommands = (WindowCommands) d;
      if ((Theme) e.NewValue == Theme.Light)
      {
        if (windowCommands.LightTemplate == null)
          return;
        windowCommands.SetValue(Control.TemplateProperty, (object) windowCommands.LightTemplate);
      }
      else
      {
        if ((Theme) e.NewValue != Theme.Dark || windowCommands.DarkTemplate == null)
          return;
        windowCommands.SetValue(Control.TemplateProperty, (object) windowCommands.DarkTemplate);
      }
    }

    private static void OnShowSeparatorsChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((WindowCommands) d).ResetSeparators();
    }

    private static void OnShowLastSeparatorChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((WindowCommands) d).ResetSeparators(false);
    }

    static WindowCommands()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (WindowCommands), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (WindowCommands)));
    }

    public WindowCommands() => this.Loaded += new RoutedEventHandler(this.WindowCommands_Loaded);

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new WindowCommandsItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is WindowCommandsItem;
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      this.AttachVisibilityHandler(element as WindowCommandsItem, item as UIElement);
      this.ResetSeparators();
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      base.ClearContainerForItemOverride(element, item);
      this.DetachVisibilityHandler(element as WindowCommandsItem);
      this.ResetSeparators(false);
    }

    private void AttachVisibilityHandler(WindowCommandsItem container, UIElement item)
    {
      if (container == null)
        return;
      if (item == null)
      {
        container.Visibility = Visibility.Collapsed;
      }
      else
      {
        container.Visibility = item.Visibility;
        PropertyChangeNotifier propertyChangeNotifier = new PropertyChangeNotifier((DependencyObject) item, UIElement.VisibilityProperty);
        propertyChangeNotifier.ValueChanged += new EventHandler(this.VisibilityPropertyChanged);
        container.VisibilityPropertyChangeNotifier = propertyChangeNotifier;
      }
    }

    private void DetachVisibilityHandler(WindowCommandsItem container)
    {
      if (container == null)
        return;
      container.VisibilityPropertyChangeNotifier = (PropertyChangeNotifier) null;
    }

    private void VisibilityPropertyChanged(object sender, EventArgs e)
    {
      if (!(sender is UIElement uiElement))
        return;
      WindowCommandsItem windowCommandsItem = this.GetWindowCommandsItem((object) uiElement);
      if (windowCommandsItem == null)
        return;
      windowCommandsItem.Visibility = uiElement.Visibility;
      this.ResetSeparators();
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      this.ResetSeparators();
    }

    private void ResetSeparators(bool reset = true)
    {
      if (this.Items.Count == 0)
        return;
      List<WindowCommandsItem> list = this.GetWindowCommandsItems().ToList<WindowCommandsItem>();
      if (reset)
      {
        foreach (WindowCommandsItem windowCommandsItem in list)
          windowCommandsItem.IsSeparatorVisible = this.ShowSeparators;
      }
      WindowCommandsItem windowCommandsItem1 = list.LastOrDefault<WindowCommandsItem>((Func<WindowCommandsItem, bool>) (i => i.IsVisible));
      if (windowCommandsItem1 == null)
        return;
      windowCommandsItem1.IsSeparatorVisible = this.ShowSeparators && this.ShowLastSeparator;
    }

    private WindowCommandsItem GetWindowCommandsItem(object item)
    {
      return item is WindowCommandsItem windowCommandsItem ? windowCommandsItem : (WindowCommandsItem) this.ItemContainerGenerator.ContainerFromItem(item);
    }

    private IEnumerable<WindowCommandsItem> GetWindowCommandsItems()
    {
      return this.Items.Cast<object>().Select<object, WindowCommandsItem>((Func<object, WindowCommandsItem>) (item => this.GetWindowCommandsItem(item))).Where<WindowCommandsItem>((Func<WindowCommandsItem, bool>) (i => i != null));
    }

    private void WindowCommands_Loaded(object sender, RoutedEventArgs e)
    {
      this.Loaded -= new RoutedEventHandler(this.WindowCommands_Loaded);
      if (this.ParentWindow != null)
        return;
      this.ParentWindow = this.TryFindParent<Window>();
    }

    public Window ParentWindow
    {
      get => this._parentWindow;
      set
      {
        if (object.Equals((object) this._parentWindow, (object) value))
          return;
        this._parentWindow = value;
        this.RaisePropertyChanged(nameof (ParentWindow));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
