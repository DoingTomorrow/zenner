// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonContextualTabGroup
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  public class RibbonContextualTabGroup : Control
  {
    private readonly List<RibbonTabItem> items = new List<RibbonTabItem>();
    private Window parentWidow;
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (string), typeof (RibbonContextualTabGroup), (PropertyMetadata) new UIPropertyMetadata((object) nameof (RibbonContextualTabGroup), new PropertyChangedCallback(RibbonContextualTabGroup.OnHeaderChanged)));
    public static readonly DependencyProperty IndentExtenderProperty = DependencyProperty.Register(nameof (IndentExtender), typeof (double), typeof (RibbonContextualTabGroup), (PropertyMetadata) new UIPropertyMetadata((object) 0.0));
    public static readonly DependencyProperty IsWindowMaximizedProperty = DependencyProperty.Register(nameof (IsWindowMaximized), typeof (bool), typeof (RibbonContextualTabGroup), (PropertyMetadata) new UIPropertyMetadata((object) false));

    public string Header
    {
      get => (string) this.GetValue(RibbonContextualTabGroup.HeaderProperty);
      set => this.SetValue(RibbonContextualTabGroup.HeaderProperty, (object) value);
    }

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }

    public double IndentExtender
    {
      get => (double) this.GetValue(RibbonContextualTabGroup.IndentExtenderProperty);
      set => this.SetValue(RibbonContextualTabGroup.IndentExtenderProperty, (object) value);
    }

    internal List<RibbonTabItem> Items => this.items;

    public bool IsWindowMaximized
    {
      get => (bool) this.GetValue(RibbonContextualTabGroup.IsWindowMaximizedProperty);
      set => this.SetValue(RibbonContextualTabGroup.IsWindowMaximizedProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonContextualTabGroup()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonContextualTabGroup), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonContextualTabGroup)));
      UIElement.VisibilityProperty.OverrideMetadata(typeof (RibbonContextualTabGroup), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(RibbonContextualTabGroup.OnVisibilityChanged)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonContextualTabGroup), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonContextualTabGroup.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonContextualTabGroup));
      return basevalue;
    }

    private static void OnVisibilityChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonContextualTabGroup contextualTabGroup = (RibbonContextualTabGroup) d;
      for (int index = 0; index < contextualTabGroup.Items.Count; ++index)
        contextualTabGroup.Items[index].Visibility = contextualTabGroup.Visibility;
      if (!(contextualTabGroup.Parent is RibbonTitleBar))
        return;
      ((UIElement) contextualTabGroup.Parent).InvalidateMeasure();
    }

    internal void AppendTabItem(RibbonTabItem item)
    {
      this.Items.Add(item);
      item.Visibility = this.Visibility;
      this.UpdateGroupBorders();
    }

    private void UpdateGroupBorders()
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        this.items[index].HasLeftGroupBorder = index == 0;
        this.items[index].HasRightGroupBorder = index == this.items.Count - 1;
      }
    }

    internal void RemoveTabItem(RibbonTabItem item)
    {
      this.Items.Remove(item);
      this.UpdateGroupBorders();
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (e.ClickCount == 1 && this.items.Count > 0)
      {
        if (this.items[0].TabControlParent != null && this.items[0].TabControlParent.SelectedItem is RibbonTabItem)
          (this.items[0].TabControlParent.SelectedItem as RibbonTabItem).IsSelected = false;
        e.Handled = true;
        if (this.items[0].TabControlParent != null && this.items[0].TabControlParent.IsMinimized)
          this.items[0].TabControlParent.IsMinimized = false;
        this.items[0].IsSelected = true;
      }
      base.OnMouseLeftButtonUp(e);
    }

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
      base.OnMouseDoubleClick(e);
      Window window = Window.GetWindow((DependencyObject) this);
      if (window == null)
        return;
      if (window.WindowState == WindowState.Maximized)
        window.WindowState = WindowState.Normal;
      else
        window.WindowState = WindowState.Maximized;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (this.parentWidow != null)
        this.parentWidow.StateChanged -= new EventHandler(this.OnParentWindowStateChanged);
      this.parentWidow = Window.GetWindow((DependencyObject) this);
      if (this.parentWidow == null)
        return;
      this.parentWidow.StateChanged += new EventHandler(this.OnParentWindowStateChanged);
      this.IsWindowMaximized = this.parentWidow.WindowState == WindowState.Maximized;
    }

    private void OnParentWindowStateChanged(object sender, EventArgs e)
    {
      this.IsWindowMaximized = this.parentWidow.WindowState == WindowState.Maximized;
    }
  }
}
