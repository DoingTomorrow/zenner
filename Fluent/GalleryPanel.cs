// Decompiled with JetBrains decompiler
// Type: Fluent.GalleryPanel
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  public class GalleryPanel : Panel
  {
    private readonly List<GalleryGroupContainer> galleryGroupContainers = new List<GalleryGroupContainer>();
    private bool haveToBeRefreshed;
    private Func<object, string> groupByAdvanced;
    public static readonly DependencyProperty IsGroupedProperty = DependencyProperty.Register(nameof (IsGrouped), typeof (bool), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(GalleryPanel.OnIsGroupedChanged)));
    public static readonly DependencyProperty GroupByProperty = DependencyProperty.Register(nameof (GroupBy), typeof (string), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(GalleryPanel.OnGroupByChanged)));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) Orientation.Horizontal));
    public static readonly DependencyProperty ItemContainerGeneratorProperty = DependencyProperty.Register(nameof (ItemContainerGenerator), typeof (ItemContainerGenerator), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(GalleryPanel.OnItemContainerGeneratorChanged)));
    public static readonly DependencyProperty GroupStyleProperty = DependencyProperty.Register("GroupHeaderStyle", typeof (Style), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof (Filter), typeof (string), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(GalleryPanel.OnFilterChanged)));
    public static readonly DependencyProperty MinItemsInRowProperty = DependencyProperty.Register(nameof (MinItemsInRow), typeof (int), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) 1));
    public static readonly DependencyProperty MaxItemsInRowProperty = DependencyProperty.Register(nameof (MaxItemsInRow), typeof (int), typeof (GalleryPanel), (PropertyMetadata) new UIPropertyMetadata((object) int.MaxValue));
    private readonly VisualCollection visualCollection;

    public bool IsGrouped
    {
      get => (bool) this.GetValue(GalleryPanel.IsGroupedProperty);
      set => this.SetValue(GalleryPanel.IsGroupedProperty, (object) value);
    }

    private static void OnIsGroupedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((GalleryPanel) d).Invalidate();
    }

    public string GroupBy
    {
      get => (string) this.GetValue(GalleryPanel.GroupByProperty);
      set => this.SetValue(GalleryPanel.GroupByProperty, (object) value);
    }

    private static void OnGroupByChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((GalleryPanel) d).Invalidate();
    }

    public Func<object, string> GroupByAdvanced
    {
      get => this.groupByAdvanced;
      set
      {
        this.groupByAdvanced = value;
        this.Invalidate();
      }
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(GalleryPanel.OrientationProperty);
      set => this.SetValue(GalleryPanel.OrientationProperty, (object) value);
    }

    public ItemContainerGenerator ItemContainerGenerator
    {
      get => (ItemContainerGenerator) this.GetValue(GalleryPanel.ItemContainerGeneratorProperty);
      set => this.SetValue(GalleryPanel.ItemContainerGeneratorProperty, (object) value);
    }

    private static void OnItemContainerGeneratorChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((GalleryPanel) d).Invalidate();
    }

    public Style GroupStyle
    {
      get => (Style) this.GetValue(GalleryPanel.GroupStyleProperty);
      set => this.SetValue(GalleryPanel.GroupStyleProperty, (object) value);
    }

    public double ItemWidth
    {
      get => (double) this.GetValue(GalleryPanel.ItemWidthProperty);
      set => this.SetValue(GalleryPanel.ItemWidthProperty, (object) value);
    }

    public double ItemHeight
    {
      get => (double) this.GetValue(GalleryPanel.ItemHeightProperty);
      set => this.SetValue(GalleryPanel.ItemHeightProperty, (object) value);
    }

    public string Filter
    {
      get => (string) this.GetValue(GalleryPanel.FilterProperty);
      set => this.SetValue(GalleryPanel.FilterProperty, (object) value);
    }

    private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((GalleryPanel) d).Invalidate();
    }

    public int MinItemsInRow
    {
      get => (int) this.GetValue(GalleryPanel.MinItemsInRowProperty);
      set => this.SetValue(GalleryPanel.MinItemsInRowProperty, (object) value);
    }

    public int MaxItemsInRow
    {
      get => (int) this.GetValue(GalleryPanel.MaxItemsInRowProperty);
      set => this.SetValue(GalleryPanel.MaxItemsInRowProperty, (object) value);
    }

    public GalleryPanel() => this.visualCollection = new VisualCollection((Visual) this);

    protected override int VisualChildrenCount
    {
      get => base.VisualChildrenCount + this.visualCollection.Count;
    }

    protected override Visual GetVisualChild(int index)
    {
      return index < base.VisualChildrenCount ? base.GetVisualChild(index) : this.visualCollection[index - base.VisualChildrenCount];
    }

    public double GetActualMinWidth(int minItemsInRow)
    {
      double val1 = 0.0;
      foreach (GalleryGroupContainer galleryGroupContainer in this.galleryGroupContainers)
      {
        int minItemsInRow1 = galleryGroupContainer.MinItemsInRow;
        int maxItemsInRow = galleryGroupContainer.MaxItemsInRow;
        galleryGroupContainer.MaxItemsInRow = galleryGroupContainer.MinItemsInRow = minItemsInRow;
        GalleryPanel.InvalidateMeasureRecursive((UIElement) galleryGroupContainer);
        galleryGroupContainer.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        val1 = Math.Max(val1, galleryGroupContainer.DesiredSize.Width);
        galleryGroupContainer.MinItemsInRow = minItemsInRow1;
        galleryGroupContainer.MaxItemsInRow = maxItemsInRow;
        galleryGroupContainer.InvalidateMeasure();
      }
      return val1;
    }

    private static void InvalidateMeasureRecursive(UIElement visual)
    {
      visual.InvalidateMeasure();
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) visual); ++childIndex)
      {
        if (VisualTreeHelper.GetChild((DependencyObject) visual, childIndex) is UIElement child)
          GalleryPanel.InvalidateMeasureRecursive(child);
      }
    }

    private static void InvalidateArrangeRecursive(UIElement visual)
    {
      visual.InvalidateArrange();
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) visual); ++childIndex)
      {
        if (VisualTreeHelper.GetChild((DependencyObject) visual, childIndex) is UIElement child)
          GalleryPanel.InvalidateMeasureRecursive(child);
      }
    }

    public Size GetItemSize()
    {
      foreach (GalleryGroupContainer galleryGroupContainer in this.galleryGroupContainers)
      {
        Size itemSize = galleryGroupContainer.GetItemSize();
        if (!itemSize.IsEmpty)
          return itemSize;
      }
      return Size.Empty;
    }

    private void Invalidate()
    {
      if (this.haveToBeRefreshed)
        return;
      this.haveToBeRefreshed = true;
      this.Dispatcher.BeginInvoke((Delegate) new Action(this.RefreshDispatchered), DispatcherPriority.Loaded);
    }

    private void RefreshDispatchered()
    {
      if (!this.haveToBeRefreshed)
        return;
      this.Refresh();
      this.haveToBeRefreshed = false;
    }

    private void Refresh()
    {
      foreach (GalleryGroupContainer galleryGroupContainer in this.galleryGroupContainers)
      {
        BindingOperations.ClearAllBindings((DependencyObject) galleryGroupContainer);
        this.visualCollection.Remove((Visual) galleryGroupContainer);
      }
      this.galleryGroupContainers.Clear();
      string[] strArray;
      if (this.Filter != null)
        strArray = this.Filter.Split(',');
      else
        strArray = (string[]) null;
      string[] source = strArray;
      Dictionary<string, GalleryGroupContainer> dictionary = new Dictionary<string, GalleryGroupContainer>();
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (internalChild != null)
        {
          string key = (this.GroupByAdvanced != null ? (this.ItemContainerGenerator == null ? this.GroupByAdvanced((object) internalChild) : this.GroupByAdvanced(this.ItemContainerGenerator.ItemFromContainer((DependencyObject) internalChild))) : (this.ItemContainerGenerator == null ? this.GetPropertyValueAsString((object) internalChild) : this.GetPropertyValueAsString(this.ItemContainerGenerator.ItemFromContainer((DependencyObject) internalChild)))) ?? "Undefined";
          if (!this.IsGrouped || source != null && !((IEnumerable<string>) source).Contains<string>(key))
          {
            internalChild.Measure(new Size(0.0, 0.0));
            internalChild.Arrange(new System.Windows.Rect(0.0, 0.0, 0.0, 0.0));
          }
          if (source == null || ((IEnumerable<string>) source).Contains<string>(key))
          {
            if (!this.IsGrouped)
              key = "Undefined";
            if (!dictionary.ContainsKey(key))
            {
              GalleryGroupContainer target = new GalleryGroupContainer();
              target.Header = (object) key;
              RibbonControl.Bind((object) this, (FrameworkElement) target, "GroupStyle", GalleryPanel.GroupStyleProperty, BindingMode.OneWay);
              RibbonControl.Bind((object) this, (FrameworkElement) target, "Orientation", GalleryGroupContainer.OrientationProperty, BindingMode.OneWay);
              RibbonControl.Bind((object) this, (FrameworkElement) target, "ItemWidth", GalleryGroupContainer.ItemWidthProperty, BindingMode.OneWay);
              RibbonControl.Bind((object) this, (FrameworkElement) target, "ItemHeight", GalleryGroupContainer.ItemHeightProperty, BindingMode.OneWay);
              RibbonControl.Bind((object) this, (FrameworkElement) target, "MaxItemsInRow", GalleryGroupContainer.MaxItemsInRowProperty, BindingMode.OneWay);
              RibbonControl.Bind((object) this, (FrameworkElement) target, "MinItemsInRow", GalleryGroupContainer.MinItemsInRowProperty, BindingMode.OneWay);
              dictionary.Add(key, target);
              this.galleryGroupContainers.Add(target);
              this.visualCollection.Add((Visual) target);
            }
            dictionary[key].Items.Add((object) new GalleryItemPlaceholder(internalChild));
          }
        }
      }
      if ((!this.IsGrouped || this.GroupBy == null && this.GroupByAdvanced == null) && this.galleryGroupContainers.Count != 0)
        this.galleryGroupContainers[0].IsHeadered = false;
      this.InvalidateMeasure();
    }

    protected override void OnVisualChildrenChanged(
      DependencyObject visualAdded,
      DependencyObject visualRemoved)
    {
      base.OnVisualChildrenChanged(visualAdded, visualRemoved);
      if (visualRemoved is GalleryGroupContainer || visualAdded is GalleryGroupContainer)
        return;
      this.Invalidate();
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      double num = 0.0;
      double height = 0.0;
      foreach (GalleryGroupContainer galleryGroupContainer in this.galleryGroupContainers)
      {
        galleryGroupContainer.Measure(availableSize);
        height += galleryGroupContainer.DesiredSize.Height;
        num = Math.Max(num, galleryGroupContainer.DesiredSize.Width);
      }
      return new Size(num, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      System.Windows.Rect finalRect = new System.Windows.Rect(finalSize);
      foreach (GalleryGroupContainer galleryGroupContainer in this.galleryGroupContainers)
      {
        finalRect.Height = galleryGroupContainer.DesiredSize.Height;
        finalRect.Width = Math.Max(finalSize.Width, galleryGroupContainer.DesiredSize.Width);
        galleryGroupContainer.Arrange(finalRect);
        finalRect.Y += galleryGroupContainer.DesiredSize.Height;
        foreach (GalleryItemPlaceholder galleryItemPlaceholder in (IEnumerable) galleryGroupContainer.Items)
        {
          Point point = galleryItemPlaceholder.TranslatePoint(new Point(), (UIElement) this);
          galleryItemPlaceholder.Target.Arrange(new System.Windows.Rect(point.X, point.Y, galleryItemPlaceholder.ArrangedSize.Width, galleryItemPlaceholder.ArrangedSize.Height));
        }
      }
      return finalSize;
    }

    private string GetPropertyValueAsString(object item)
    {
      if (item == null || this.GroupBy == null)
        return "Undefined";
      PropertyInfo property = item.GetType().GetProperty(this.GroupBy, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
      if (property == (PropertyInfo) null)
        return "Undefined";
      object obj = property.GetValue(item, (object[]) null);
      return obj == null ? "Undefined" : obj.ToString();
    }
  }
}
