// Decompiled with JetBrains decompiler
// Type: Fluent.QuickAccessToolBar
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_RootPanel", Type = typeof (Panel))]
  [ContentProperty("QuickAccessItems")]
  [TemplatePart(Name = "PART_MenuPanel", Type = typeof (Panel))]
  [TemplatePart(Name = "PART_ShowAbove", Type = typeof (MenuItem))]
  [TemplatePart(Name = "PART_ShowBelow", Type = typeof (MenuItem))]
  public class QuickAccessToolBar : Control
  {
    private DropDownButton toolBarDownButton;
    private DropDownButton menuDownButton;
    private MenuItem showAbove;
    private MenuItem showBelow;
    private ObservableCollection<QuickAccessMenuItem> quickAccessItems;
    private Panel rootPanel;
    private Panel toolBarPanel;
    private Panel toolBarOverflowPanel;
    private ObservableCollection<UIElement> items;
    private Size cachedConstraint;
    private int cachedCount = -1;
    private bool itemsHadChanged;
    private double cachedDeltaWidth;
    private static readonly DependencyPropertyKey HasOverflowItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasOverflowItems), typeof (bool), typeof (QuickAccessToolBar), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty HasOverflowItemsProperty = QuickAccessToolBar.HasOverflowItemsPropertyKey.DependencyProperty;
    public static readonly DependencyProperty ShowAboveRibbonProperty = DependencyProperty.Register(nameof (ShowAboveRibbon), typeof (bool), typeof (QuickAccessToolBar), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty CanQuickAccessLocationChangingProperty = DependencyProperty.Register(nameof (CanQuickAccessLocationChanging), typeof (bool), typeof (QuickAccessToolBar), (PropertyMetadata) new UIPropertyMetadata((object) true));

    public event NotifyCollectionChangedEventHandler ItemsChanged;

    internal ObservableCollection<UIElement> Items
    {
      get
      {
        if (this.items == null)
        {
          this.items = new ObservableCollection<UIElement>();
          this.items.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnItemsCollectionChanged);
        }
        return this.items;
      }
    }

    private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.cachedCount = this.GetNonOverflowItemsCount(this.DesiredSize.Width);
      this.HasOverflowItems = this.cachedCount < this.Items.Count;
      this.itemsHadChanged = true;
      this.InvalidateMeasure();
      if (this.Parent is Ribbon)
        (this.Parent as Ribbon).TitleBar.InvalidateMeasure();
      this.UpdateKeyTips();
      if (e.OldItems != null)
      {
        for (int index = 0; index < e.OldItems.Count; ++index)
          (e.OldItems[index] as FrameworkElement).SizeChanged -= new SizeChangedEventHandler(this.OnChildSizeChanged);
      }
      if (e.NewItems != null)
      {
        for (int index = 0; index < e.NewItems.Count; ++index)
          (e.NewItems[index] as FrameworkElement).SizeChanged += new SizeChangedEventHandler(this.OnChildSizeChanged);
      }
      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        for (int index = 0; index < this.Items.Count; ++index)
          (this.Items[index] as FrameworkElement).SizeChanged -= new SizeChangedEventHandler(this.OnChildSizeChanged);
      }
      if (this.ItemsChanged == null)
        return;
      this.ItemsChanged((object) this, e);
    }

    private void OnChildSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (!(this.Parent is Ribbon))
        return;
      (this.Parent as Ribbon).TitleBar.InvalidateMeasure();
    }

    public bool HasOverflowItems
    {
      get => (bool) this.GetValue(QuickAccessToolBar.HasOverflowItemsProperty);
      private set => this.SetValue(QuickAccessToolBar.HasOverflowItemsPropertyKey, (object) value);
    }

    public ObservableCollection<QuickAccessMenuItem> QuickAccessItems
    {
      get
      {
        if (this.quickAccessItems == null)
        {
          this.quickAccessItems = new ObservableCollection<QuickAccessMenuItem>();
          this.quickAccessItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnQuickAccessItemsCollectionChanged);
        }
        return this.quickAccessItems;
      }
    }

    private void OnQuickAccessItemsCollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.menuDownButton != null)
              this.menuDownButton.Items.Insert(e.NewStartingIndex + index + 1, (object) (QuickAccessMenuItem) e.NewItems[index]);
            else
              this.AddLogicalChild(e.NewItems[index]);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.menuDownButton != null)
                this.menuDownButton.Items.Remove((object) (QuickAccessMenuItem) current);
              else
                this.RemoveLogicalChild(current);
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
            if (this.menuDownButton != null)
              this.menuDownButton.Items.Remove((object) (QuickAccessMenuItem) oldItem);
            else
              this.RemoveLogicalChild(oldItem);
          }
          int num = 0;
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.menuDownButton != null)
                this.menuDownButton.Items.Insert(e.NewStartingIndex + num + 1, (object) (QuickAccessMenuItem) current);
              else
                this.AddLogicalChild(current);
              ++num;
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

    public bool ShowAboveRibbon
    {
      get => (bool) this.GetValue(QuickAccessToolBar.ShowAboveRibbonProperty);
      set => this.SetValue(QuickAccessToolBar.ShowAboveRibbonProperty, (object) value);
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        if (this.rootPanel != null)
          arrayList.Add((object) this.rootPanel);
        return arrayList.GetEnumerator();
      }
    }

    public bool CanQuickAccessLocationChanging
    {
      get => (bool) this.GetValue(QuickAccessToolBar.CanQuickAccessLocationChangingProperty);
      set
      {
        this.SetValue(QuickAccessToolBar.CanQuickAccessLocationChangingProperty, (object) value);
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static QuickAccessToolBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (QuickAccessToolBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (QuickAccessToolBar)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (QuickAccessToolBar), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(QuickAccessToolBar.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (QuickAccessToolBar));
      return basevalue;
    }

    public override void OnApplyTemplate()
    {
      if (this.showAbove != null)
        this.showAbove.Click -= new RoutedEventHandler(this.OnShowAboveClick);
      if (this.showBelow != null)
        this.showBelow.Click -= new RoutedEventHandler(this.OnShowBelowClick);
      this.showAbove = this.GetTemplateChild("PART_ShowAbove") as MenuItem;
      this.showBelow = this.GetTemplateChild("PART_ShowBelow") as MenuItem;
      if (this.showAbove != null)
        this.showAbove.Click += new RoutedEventHandler(this.OnShowAboveClick);
      if (this.showBelow != null)
        this.showBelow.Click += new RoutedEventHandler(this.OnShowBelowClick);
      if (this.menuDownButton != null)
      {
        for (int index = 0; index < this.QuickAccessItems.Count; ++index)
        {
          this.menuDownButton.Items.Remove((object) this.QuickAccessItems[index]);
          this.QuickAccessItems[index].InvalidateProperty(QuickAccessMenuItem.TargetProperty);
        }
      }
      else if (this.quickAccessItems != null)
      {
        for (int index = 0; index < this.quickAccessItems.Count; ++index)
          this.RemoveLogicalChild((object) this.quickAccessItems[index]);
      }
      this.menuDownButton = this.GetTemplateChild("PART_MenuDownButton") as DropDownButton;
      if (this.menuDownButton != null && this.quickAccessItems != null)
      {
        for (int index = 0; index < this.quickAccessItems.Count; ++index)
        {
          this.menuDownButton.Items.Insert(index + 1, (object) this.quickAccessItems[index]);
          this.quickAccessItems[index].InvalidateProperty(QuickAccessMenuItem.TargetProperty);
        }
      }
      if (this.toolBarDownButton != null)
      {
        this.toolBarDownButton.DropDownOpened -= new EventHandler(this.OnToolBarDownOpened);
        this.toolBarDownButton.DropDownClosed -= new EventHandler(this.OnToolBarDownClosed);
      }
      this.toolBarDownButton = this.GetTemplateChild("PART_ToolbarDownButton") as DropDownButton;
      if (this.toolBarDownButton != null)
      {
        this.toolBarDownButton.DropDownOpened += new EventHandler(this.OnToolBarDownOpened);
        this.toolBarDownButton.DropDownClosed += new EventHandler(this.OnToolBarDownClosed);
      }
      this.toolBarPanel = this.GetTemplateChild("PART_ToolBarPanel") as Panel;
      this.toolBarOverflowPanel = this.GetTemplateChild("PART_ToolBarOverflowPanel") as Panel;
      if (this.rootPanel != null)
        this.RemoveLogicalChild((object) this.rootPanel);
      this.rootPanel = this.GetTemplateChild("PART_RootPanel") as Panel;
      if (this.rootPanel != null)
        this.AddLogicalChild((object) this.rootPanel);
      this.cachedDeltaWidth = 0.0;
      this.cachedCount = this.GetNonOverflowItemsCount(this.ActualWidth);
      this.cachedConstraint = new Size();
    }

    private void OnToolBarDownClosed(object sender, EventArgs e)
    {
      this.toolBarOverflowPanel.Children.Clear();
    }

    private void OnToolBarDownOpened(object sender, EventArgs e)
    {
      if (this.toolBarOverflowPanel.Children.Count > 0)
        this.toolBarOverflowPanel.Children.Clear();
      for (int cachedCount = this.cachedCount; cachedCount < this.Items.Count; ++cachedCount)
        this.toolBarOverflowPanel.Children.Add(this.Items[cachedCount]);
    }

    private void OnShowBelowClick(object sender, RoutedEventArgs e) => this.ShowAboveRibbon = false;

    private void OnShowAboveClick(object sender, RoutedEventArgs e) => this.ShowAboveRibbon = true;

    protected override Size MeasureOverride(Size constraint)
    {
      if (this.cachedConstraint == constraint && !this.itemsHadChanged)
        return base.MeasureOverride(constraint);
      this.cachedCount = this.GetNonOverflowItemsCount(constraint.Width);
      this.HasOverflowItems = this.cachedCount < this.Items.Count;
      this.cachedConstraint = constraint;
      this.toolBarOverflowPanel.Children.Clear();
      if (this.itemsHadChanged)
      {
        this.toolBarPanel.Children.Clear();
        for (int index = 0; index < this.cachedCount; ++index)
          this.toolBarPanel.Children.Add(this.Items[index]);
        this.itemsHadChanged = false;
      }
      else if (this.cachedCount > this.toolBarPanel.Children.Count)
      {
        for (int count = this.toolBarPanel.Children.Count; count < this.cachedCount; ++count)
          this.toolBarPanel.Children.Add(this.Items[count]);
      }
      else
      {
        for (int index = this.toolBarPanel.Children.Count - 1; index >= this.cachedCount; --index)
          this.toolBarPanel.Children.Remove(this.Items[index]);
      }
      return base.MeasureOverride(constraint);
    }

    private void UpdateKeyTips()
    {
      for (int index = 0; index < Math.Min(9, this.Items.Count); ++index)
        KeyTip.SetKeys((DependencyObject) this.Items[index], (index + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      for (int index = 9; index < Math.Min(18, this.Items.Count); ++index)
        KeyTip.SetKeys((DependencyObject) this.Items[index], "0" + (18 - index).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      char ch = 'A';
      for (int index = 18; index < Math.Min(44, this.Items.Count); ++index)
        KeyTip.SetKeys((DependencyObject) this.Items[index], "0" + (object) ch++);
    }

    private int GetNonOverflowItemsCount(double width)
    {
      if (this.cachedDeltaWidth == 0.0 && this.rootPanel != null && this.toolBarPanel != null)
      {
        this.rootPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        this.cachedDeltaWidth = this.rootPanel.DesiredSize.Width - this.toolBarPanel.DesiredSize.Width;
      }
      double num = 0.0;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        this.Items[index].Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        num += this.Items[index].DesiredSize.Width;
        if (num + this.cachedDeltaWidth > width)
          return index;
      }
      return this.Items.Count;
    }
  }
}
