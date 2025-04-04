// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonTabItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [ContentProperty("Groups")]
  [TemplatePart(Name = "PART_ContentContainer", Type = typeof (Border))]
  public class RibbonTabItem : Control, IKeyTipedControl
  {
    private Border contentContainer;
    private double desiredWidth;
    private ObservableCollection<RibbonGroupBox> groups;
    private RibbonGroupsContainer groupsInnerContainer = new RibbonGroupsContainer();
    private ScrollViewer groupsContainer = new ScrollViewer();
    private double cachedWidth;
    public static readonly DependencyProperty IsMinimizedProperty = DependencyProperty.Register(nameof (IsMinimized), typeof (bool), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    private static readonly DependencyPropertyKey IsContextualPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsContextual), typeof (bool), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsContextualProperty = RibbonTabItem.IsContextualPropertyKey.DependencyProperty;
    public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof (RibbonTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(RibbonTabItem.OnIsSelectedChanged)));
    public static readonly DependencyProperty IndentProperty = DependencyProperty.Register(nameof (Indent), typeof (double), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) 12.0));
    public static readonly DependencyProperty IsSeparatorVisibleProperty = DependencyProperty.Register(nameof (IsSeparatorVisible), typeof (bool), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty GroupProperty = DependencyProperty.Register(nameof (Group), typeof (RibbonContextualTabGroup), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonTabItem.OnGroupChanged)));
    public static readonly DependencyProperty HasLeftGroupBorderProperty = DependencyProperty.Register(nameof (HasLeftGroupBorder), typeof (bool), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty HasRightGroupBorderProperty = DependencyProperty.Register(nameof (HasRightGroupBorder), typeof (bool), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (RibbonTabItem), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonTabItem.OnHeaderChanged)));

    public ScrollViewer GroupsContainer => this.groupsContainer;

    public bool IsMinimized
    {
      get => (bool) this.GetValue(RibbonTabItem.IsMinimizedProperty);
      set => this.SetValue(RibbonTabItem.IsMinimizedProperty, (object) value);
    }

    public bool IsOpen
    {
      get => (bool) this.GetValue(RibbonTabItem.IsOpenProperty);
      set => this.SetValue(RibbonTabItem.IsOpenProperty, (object) value);
    }

    public string ReduceOrder
    {
      get => this.groupsInnerContainer.ReduceOrder;
      set => this.groupsInnerContainer.ReduceOrder = value;
    }

    public bool IsContextual
    {
      get => (bool) this.GetValue(RibbonTabItem.IsContextualProperty);
      private set => this.SetValue(RibbonTabItem.IsContextualPropertyKey, (object) value);
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        return new ArrayList()
        {
          (object) this.groupsContainer
        }.GetEnumerator();
      }
    }

    [Category("Appearance")]
    [Bindable(true)]
    public bool IsSelected
    {
      get => (bool) this.GetValue(RibbonTabItem.IsSelectedProperty);
      set => this.SetValue(RibbonTabItem.IsSelectedProperty, (object) value);
    }

    internal RibbonTabControl TabControlParent
    {
      get
      {
        return ItemsControl.ItemsControlFromItemContainer((DependencyObject) this) as RibbonTabControl;
      }
    }

    public double Indent
    {
      get => (double) this.GetValue(RibbonTabItem.IndentProperty);
      set => this.SetValue(RibbonTabItem.IndentProperty, (object) value);
    }

    public bool IsSeparatorVisible
    {
      get => (bool) this.GetValue(RibbonTabItem.IsSeparatorVisibleProperty);
      set => this.SetValue(RibbonTabItem.IsSeparatorVisibleProperty, (object) value);
    }

    public RibbonContextualTabGroup Group
    {
      get => (RibbonContextualTabGroup) this.GetValue(RibbonTabItem.GroupProperty);
      set => this.SetValue(RibbonTabItem.GroupProperty, (object) value);
    }

    private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonTabItem ribbonTabItem = d as RibbonTabItem;
      if (e.OldValue != null)
        (e.OldValue as RibbonContextualTabGroup).RemoveTabItem(ribbonTabItem);
      if (e.NewValue != null)
      {
        (e.NewValue as RibbonContextualTabGroup).AppendTabItem(ribbonTabItem);
        ribbonTabItem.IsContextual = true;
      }
      else
        ribbonTabItem.IsContextual = false;
    }

    internal double DesiredWidth
    {
      get => this.desiredWidth;
      set
      {
        this.desiredWidth = value;
        this.InvalidateMeasure();
      }
    }

    public bool HasLeftGroupBorder
    {
      get => (bool) this.GetValue(RibbonTabItem.HasLeftGroupBorderProperty);
      set => this.SetValue(RibbonTabItem.HasLeftGroupBorderProperty, (object) value);
    }

    public bool HasRightGroupBorder
    {
      get => (bool) this.GetValue(RibbonTabItem.HasRightGroupBorderProperty);
      set => this.SetValue(RibbonTabItem.HasRightGroupBorderProperty, (object) value);
    }

    public ObservableCollection<RibbonGroupBox> Groups
    {
      get
      {
        if (this.groups == null)
        {
          this.groups = new ObservableCollection<RibbonGroupBox>();
          this.groups.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupsCollectionChanged);
        }
        return this.groups;
      }
    }

    private void OnGroupsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.groupsInnerContainer != null)
              this.groupsInnerContainer.Children.Insert(e.NewStartingIndex + index, (UIElement) e.NewItems[index]);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.groupsInnerContainer != null)
                this.groupsInnerContainer.Children.Remove(current as UIElement);
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Replace:
          foreach (object oldItem in (IEnumerable) e.OldItems)
          {
            if (this.groupsInnerContainer != null)
              this.groupsInnerContainer.Children.Remove(oldItem as UIElement);
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.groupsInnerContainer != null)
                this.groupsInnerContainer.Children.Add(current as UIElement);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    public object Header
    {
      get => this.GetValue(RibbonTabItem.HeaderProperty);
      set => this.SetValue(RibbonTabItem.HeaderProperty, value);
    }

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(FrameworkElement.ToolTipProperty);
    }

    private static void OnFocusableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }

    private static object CoerceFocusable(DependencyObject d, object basevalue)
    {
      if (d is RibbonTabItem ribbonTabItem)
      {
        Ribbon parentRibbon = ribbonTabItem.FindParentRibbon();
        if (parentRibbon != null)
          return (object) (bool) (!(bool) basevalue ? 0 : (parentRibbon.Focusable ? 1 : 0));
      }
      return basevalue;
    }

    private Ribbon FindParentRibbon()
    {
      for (DependencyObject parent = this.Parent; parent != null; parent = VisualTreeHelper.GetParent(parent))
      {
        if (parent is Ribbon parentRibbon)
          return parentRibbon;
      }
      return (Ribbon) null;
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonTabItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonTabItem)));
      UIElement.FocusableProperty.AddOwner(typeof (RibbonTabItem), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(RibbonTabItem.OnFocusableChanged), new CoerceValueCallback(RibbonTabItem.CoerceFocusable)));
      FrameworkElement.ToolTipProperty.OverrideMetadata(typeof (RibbonTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonTabItem.CoerceToolTip)));
      UIElement.VisibilityProperty.AddOwner(typeof (RibbonTabItem), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(RibbonTabItem.OnVisibilityChanged)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonTabItem.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonTabItem));
      return basevalue;
    }

    private static void OnVisibilityChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonTabItem ribbonTabItem = d as RibbonTabItem;
      if (!ribbonTabItem.IsSelected || (Visibility) e.NewValue != Visibility.Collapsed || ribbonTabItem.TabControlParent == null)
        return;
      ribbonTabItem.TabControlParent.SelectedItem = ribbonTabItem.TabControlParent.Items[0];
    }

    private static object CoerceToolTip(DependencyObject d, object basevalue)
    {
      RibbonTabItem ribbonTabItem = (RibbonTabItem) d;
      if (basevalue == null && ribbonTabItem.Header is string)
        basevalue = ribbonTabItem.Header;
      return basevalue;
    }

    public RibbonTabItem()
    {
      this.AddLogicalChild((object) this.groupsContainer);
      this.groupsContainer.Content = (object) this.groupsInnerContainer;
      ContextMenuService.Coerce((DependencyObject) this);
    }

    protected override Size MeasureOverride(Size constraint)
    {
      if (this.contentContainer == null)
        return base.MeasureOverride(constraint);
      this.contentContainer.Padding = new Thickness(this.Indent, this.contentContainer.Padding.Top, this.Indent, this.contentContainer.Padding.Bottom);
      Size size = base.MeasureOverride(constraint);
      double num1 = this.contentContainer.DesiredSize.Width - this.contentContainer.Margin.Left - this.contentContainer.Margin.Right;
      this.contentContainer.Child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      double width = this.contentContainer.Child.DesiredSize.Width;
      if (num1 < width + this.Indent * 2.0)
      {
        double num2 = Math.Max(0.0, (num1 - width) / 2.0);
        this.contentContainer.Padding = new Thickness(num2, this.contentContainer.Padding.Top, num2, this.contentContainer.Padding.Bottom);
      }
      else if (this.desiredWidth != 0.0)
        size.Width = constraint.Width <= this.desiredWidth || this.desiredWidth <= num1 ? width + this.Indent * 2.0 + this.contentContainer.Margin.Left + this.contentContainer.Margin.Right : this.desiredWidth;
      if (this.cachedWidth != size.Width && this.IsContextual && this.Group != null)
      {
        this.cachedWidth = size.Width;
        if (VisualTreeHelper.GetParent((DependencyObject) this.Group) is FrameworkElement parent)
          parent.InvalidateMeasure();
      }
      return size;
    }

    public override void OnApplyTemplate()
    {
      this.contentContainer = this.GetTemplateChild("PART_ContentContainer") as Border;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (e.Source == this && e.ClickCount == 2)
      {
        e.Handled = true;
        if (this.TabControlParent == null)
          return;
        this.TabControlParent.IsMinimized = !this.TabControlParent.IsMinimized;
      }
      else
      {
        if (e.Source != this && this.IsSelected)
          return;
        if (this.TabControlParent != null && this.TabControlParent.SelectedItem is RibbonTabItem)
          (this.TabControlParent.SelectedItem as RibbonTabItem).IsSelected = false;
        e.Handled = true;
        this.IsSelected = true;
      }
    }

    private static void OnIsSelectedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonTabItem source = d as RibbonTabItem;
      if ((bool) e.NewValue)
      {
        if (source.TabControlParent != null && source.TabControlParent.SelectedItem is RibbonTabItem && source.TabControlParent.SelectedItem != source)
          (source.TabControlParent.SelectedItem as RibbonTabItem).IsSelected = false;
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
        (this.TabControlParent.SelectedItem as RibbonTabItem).IsSelected = false;
      this.IsSelected = true;
    }
  }
}
