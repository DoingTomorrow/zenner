// Decompiled with JetBrains decompiler
// Type: Fluent.GalleryGroupContainer
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  public class GalleryGroupContainer : HeaderedItemsControl
  {
    private bool maxMinWidthNeedsToBeUpdated;
    public static readonly DependencyProperty IsHeaderedProperty = DependencyProperty.Register(nameof (IsHeadered), typeof (bool), typeof (GalleryGroupContainer), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (GalleryGroupContainer), (PropertyMetadata) new UIPropertyMetadata((object) Orientation.Horizontal));
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (GalleryGroupContainer), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (GalleryGroupContainer), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty MinItemsInRowProperty = DependencyProperty.Register(nameof (MinItemsInRow), typeof (int), typeof (GalleryGroupContainer), (PropertyMetadata) new UIPropertyMetadata((object) 0, new PropertyChangedCallback(GalleryGroupContainer.OnMaxMinItemsInRowChanged)));
    public static readonly DependencyProperty MaxItemsInRowProperty = DependencyProperty.Register(nameof (MaxItemsInRow), typeof (int), typeof (GalleryGroupContainer), (PropertyMetadata) new UIPropertyMetadata((object) int.MaxValue, new PropertyChangedCallback(GalleryGroupContainer.OnMaxMinItemsInRowChanged)));
    private Panel previousItemsPanel;
    private int previousItemsCount;

    public bool IsHeadered
    {
      get => (bool) this.GetValue(GalleryGroupContainer.IsHeaderedProperty);
      set => this.SetValue(GalleryGroupContainer.IsHeaderedProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(GalleryGroupContainer.OrientationProperty);
      set => this.SetValue(GalleryGroupContainer.OrientationProperty, (object) value);
    }

    public double ItemWidth
    {
      get => (double) this.GetValue(GalleryGroupContainer.ItemWidthProperty);
      set => this.SetValue(GalleryGroupContainer.ItemWidthProperty, (object) value);
    }

    public double ItemHeight
    {
      get => (double) this.GetValue(GalleryGroupContainer.ItemHeightProperty);
      set => this.SetValue(GalleryGroupContainer.ItemHeightProperty, (object) value);
    }

    public int MinItemsInRow
    {
      get => (int) this.GetValue(GalleryGroupContainer.MinItemsInRowProperty);
      set => this.SetValue(GalleryGroupContainer.MinItemsInRowProperty, (object) value);
    }

    private static void OnMaxMinItemsInRowChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((GalleryGroupContainer) d).maxMinWidthNeedsToBeUpdated = true;
    }

    public int MaxItemsInRow
    {
      get => (int) this.GetValue(GalleryGroupContainer.MaxItemsInRowProperty);
      set => this.SetValue(GalleryGroupContainer.MaxItemsInRowProperty, (object) value);
    }

    static GalleryGroupContainer()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (GalleryGroupContainer), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (GalleryGroupContainer)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (GalleryGroupContainer), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(GalleryGroupContainer.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (GalleryGroupContainer));
      return basevalue;
    }

    private void UpdateMaxWidth()
    {
      this.maxMinWidthNeedsToBeUpdated = false;
      Panel itemsPanel = GalleryGroupContainer.FindItemsPanel((DependencyObject) this);
      if (itemsPanel == null)
      {
        int num = this.IsLoaded ? 1 : 0;
        this.Dispatcher.BeginInvoke((Delegate) new Action(((UIElement) this).InvalidateMeasure), DispatcherPriority.ContextIdle);
      }
      else if (this.Orientation == Orientation.Vertical)
      {
        itemsPanel.MinWidth = 0.0;
        itemsPanel.MaxWidth = double.PositiveInfinity;
      }
      else
      {
        double itemWidth = this.GetItemWidth();
        if (double.IsNaN(itemWidth))
          return;
        itemsPanel.MinWidth = (double) Math.Min(this.Items.Count, this.MinItemsInRow) * itemWidth + 0.1;
        itemsPanel.MaxWidth = (double) Math.Min(this.Items.Count, this.MaxItemsInRow) * itemWidth + 0.1;
      }
    }

    public Size GetItemSize()
    {
      if (!double.IsNaN(this.ItemWidth) && !double.IsNaN(this.ItemHeight))
        return new Size(this.ItemWidth, this.ItemHeight);
      if (this.Items.Count == 0 || !(this.ItemContainerGenerator.ContainerFromItem(this.Items[0]) is UIElement uiElement))
        return Size.Empty;
      uiElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      Size desiredSize = uiElement.DesiredSize;
      uiElement.InvalidateMeasure();
      return desiredSize;
    }

    private double GetItemWidth() => this.GetItemSize().Width;

    private static Panel FindItemsPanel(DependencyObject obj)
    {
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(obj); ++childIndex)
      {
        if (obj is Panel itemsPanel1 && itemsPanel1.IsItemsHost)
          return itemsPanel1;
        Panel itemsPanel2 = GalleryGroupContainer.FindItemsPanel(VisualTreeHelper.GetChild(obj, childIndex));
        if (itemsPanel2 != null)
          return itemsPanel2;
      }
      return (Panel) null;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      Panel itemsPanel = GalleryGroupContainer.FindItemsPanel((DependencyObject) this);
      if (itemsPanel != this.previousItemsPanel || this.previousItemsCount != this.Items.Count || this.maxMinWidthNeedsToBeUpdated)
      {
        this.previousItemsPanel = itemsPanel;
        this.previousItemsCount = this.Items.Count;
        this.UpdateMaxWidth();
      }
      return base.MeasureOverride(constraint);
    }
  }
}
