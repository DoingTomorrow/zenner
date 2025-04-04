// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonToolBar
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [ContentProperty("Children")]
  public class RibbonToolBar : RibbonControl
  {
    private readonly ObservableCollection<FrameworkElement> children = new ObservableCollection<FrameworkElement>();
    private readonly ObservableCollection<RibbonToolBarLayoutDefinition> layoutDefinitions = new ObservableCollection<RibbonToolBarLayoutDefinition>();
    private readonly List<FrameworkElement> actualChildren = new List<FrameworkElement>();
    private bool rebuildVisualAndLogicalChildren = true;
    public static readonly DependencyProperty SeparatorStyleProperty = DependencyProperty.Register(nameof (SeparatorStyle), typeof (Style), typeof (RibbonToolBar), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonToolBar.OnSeparatorStyleChanged)));
    private Dictionary<object, RibbonToolBarControlGroup> cachedControlGroups = new Dictionary<object, RibbonToolBarControlGroup>();
    private Dictionary<int, Separator> separatorCache = new Dictionary<int, Separator>();

    public Style SeparatorStyle
    {
      get => (Style) this.GetValue(RibbonToolBar.SeparatorStyleProperty);
      set => this.SetValue(RibbonToolBar.SeparatorStyleProperty, (object) value);
    }

    private static void OnSeparatorStyleChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonToolBar ribbonToolBar = (RibbonToolBar) d;
      ribbonToolBar.rebuildVisualAndLogicalChildren = true;
      ribbonToolBar.InvalidateMeasure();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ObservableCollection<FrameworkElement> Children => this.children;

    public ObservableCollection<RibbonToolBarLayoutDefinition> LayoutDefinitions
    {
      get => this.layoutDefinitions;
    }

    protected override int VisualChildrenCount
    {
      get
      {
        if (this.layoutDefinitions.Count == 0)
          return this.children.Count;
        if (this.rebuildVisualAndLogicalChildren)
          this.InvalidateMeasure();
        return this.actualChildren.Count;
      }
    }

    protected override Visual GetVisualChild(int index)
    {
      if (this.layoutDefinitions.Count == 0)
        return (Visual) this.children[index];
      if (this.rebuildVisualAndLogicalChildren)
        this.InvalidateMeasure();
      return (Visual) this.actualChildren[index];
    }

    protected override IEnumerator LogicalChildren => (IEnumerator) this.children.GetEnumerator();

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonToolBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonToolBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonToolBar)));
      RibbonControl.CanAddToQuickAccessToolBarProperty.OverrideMetadata(typeof (RibbonToolBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonToolBar), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonToolBar.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonToolBar));
      return basevalue;
    }

    public RibbonToolBar()
    {
      this.children.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnChildrenCollectionChanged);
      this.layoutDefinitions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnLayoutDefinitionsChanged);
    }

    private void OnLayoutDefinitionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.rebuildVisualAndLogicalChildren = true;
      this.InvalidateMeasure();
    }

    private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.rebuildVisualAndLogicalChildren = true;
      this.InvalidateMeasure();
    }

    internal RibbonToolBarLayoutDefinition GetCurrentLayoutDefinition()
    {
      if (this.layoutDefinitions.Count == 0)
        return (RibbonToolBarLayoutDefinition) null;
      if (this.layoutDefinitions.Count == 1)
        return this.layoutDefinitions[0];
      foreach (RibbonToolBarLayoutDefinition layoutDefinition in (Collection<RibbonToolBarLayoutDefinition>) this.layoutDefinitions)
      {
        if (layoutDefinition.Size == this.Size)
          return layoutDefinition;
      }
      return this.layoutDefinitions[0];
    }

    protected override void OnSizePropertyChanged(
      RibbonControlSize previous,
      RibbonControlSize current)
    {
      this.rebuildVisualAndLogicalChildren = true;
      this.InvalidateMeasure();
    }

    protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
    {
      RibbonToolBarLayoutDefinition layoutDefinition = this.GetCurrentLayoutDefinition();
      if (this.rebuildVisualAndLogicalChildren)
      {
        foreach (FrameworkElement actualChild in this.actualChildren)
        {
          if (actualChild is RibbonToolBarControlGroup toolBarControlGroup)
            toolBarControlGroup.Items.Clear();
          this.RemoveVisualChild((Visual) actualChild);
          this.RemoveLogicalChild((object) actualChild);
        }
        this.actualChildren.Clear();
        this.cachedControlGroups.Clear();
      }
      if (layoutDefinition == null)
      {
        if (this.rebuildVisualAndLogicalChildren)
        {
          foreach (FrameworkElement child in (Collection<FrameworkElement>) this.Children)
          {
            this.actualChildren.Add(child);
            this.AddVisualChild((Visual) child);
            this.AddLogicalChild((object) child);
          }
          this.rebuildVisualAndLogicalChildren = false;
        }
        return this.WrapPanelLayuot(availableSize, true);
      }
      System.Windows.Size size = this.CustomLayout(layoutDefinition, availableSize, true, this.rebuildVisualAndLogicalChildren);
      this.rebuildVisualAndLogicalChildren = false;
      return size;
    }

    protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
    {
      RibbonToolBarLayoutDefinition layoutDefinition = this.GetCurrentLayoutDefinition();
      return layoutDefinition == null ? this.WrapPanelLayuot(finalSize, false) : this.CustomLayout(layoutDefinition, finalSize, false, false);
    }

    private System.Windows.Size WrapPanelLayuot(System.Windows.Size availableSize, bool measure)
    {
      bool flag = !measure;
      double num1 = double.IsPositiveInfinity(availableSize.Height) ? 0.0 : availableSize.Height;
      double num2 = 0.0;
      double val1 = 0.0;
      double x = 0.0;
      double num3 = 0.0;
      System.Windows.Size availableSize1 = new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity);
      foreach (FrameworkElement child in (Collection<FrameworkElement>) this.children)
      {
        if (measure)
          child.Measure(availableSize1);
        if (num2 + child.DesiredSize.Height > num1)
        {
          num3 = Math.Max(num3, num2);
          x += val1;
          num2 = 0.0;
          val1 = 0.0;
        }
        if (flag)
          child.Arrange(new System.Windows.Rect(x, num2, child.DesiredSize.Width, child.DesiredSize.Height));
        val1 = Math.Max(val1, child.DesiredSize.Width);
        num2 += child.DesiredSize.Height;
      }
      return new System.Windows.Size(x + val1, num3);
    }

    private FrameworkElement GetControl(RibbonToolBarControlDefinition controlDefinition)
    {
      string name = controlDefinition.Target;
      return this.Children.FirstOrDefault<FrameworkElement>((Func<FrameworkElement, bool>) (x => x.Name == name));
    }

    private RibbonToolBarControlGroup GetControlGroup(
      RibbonToolBarControlGroupDefinition controlGroupDefinition)
    {
      RibbonToolBarControlGroup controlGroup = (RibbonToolBarControlGroup) null;
      if (!this.cachedControlGroups.TryGetValue((object) controlGroupDefinition, out controlGroup))
      {
        controlGroup = new RibbonToolBarControlGroup();
        foreach (RibbonToolBarControlDefinition child in (Collection<RibbonToolBarControlDefinition>) controlGroupDefinition.Children)
          controlGroup.Items.Add((object) this.GetControl(child));
        this.cachedControlGroups.Add((object) controlGroupDefinition, controlGroup);
      }
      return controlGroup;
    }

    private System.Windows.Size CustomLayout(
      RibbonToolBarLayoutDefinition layoutDefinition,
      System.Windows.Size availableSize,
      bool measure,
      bool addchildren)
    {
      bool flag = !measure;
      double num1 = double.IsPositiveInfinity(availableSize.Height) ? 0.0 : availableSize.Height;
      if (addchildren)
        this.separatorCache.Clear();
      double rowHeight = this.GetRowHeight(layoutDefinition);
      int num2 = Math.Min(layoutDefinition.RowCount, layoutDefinition.Rows.Count);
      double num3 = (num1 - (double) num2 * rowHeight) / (double) (num2 + 1);
      double y1 = 0.0;
      double num4 = 0.0;
      double width = 0.0;
      double num5 = 0.0;
      for (int index1 = 0; index1 < layoutDefinition.Rows.Count; ++index1)
      {
        RibbonToolBarRow row = layoutDefinition.Rows[index1];
        double x = num4;
        if (index1 % num2 == 0)
        {
          x = num4 = width;
          y1 = 0.0;
          if (index1 != 0)
          {
            Separator child = (Separator) null;
            if (!this.separatorCache.TryGetValue(index1, out child))
            {
              child = new Separator();
              child.Style = this.SeparatorStyle;
              this.separatorCache.Add(index1, child);
            }
            if (measure)
            {
              child.Height = num1 - child.Margin.Bottom - child.Margin.Top;
              child.Measure(availableSize);
            }
            if (flag)
              child.Arrange(new System.Windows.Rect(x, y1, child.DesiredSize.Width, child.DesiredSize.Height));
            x += child.DesiredSize.Width;
            if (addchildren)
            {
              this.AddVisualChild((Visual) child);
              this.AddLogicalChild((object) child);
              this.actualChildren.Add((FrameworkElement) child);
            }
          }
        }
        double y2 = y1 + num3;
        for (int index2 = 0; index2 < row.Children.Count; ++index2)
        {
          if (row.Children[index2] is RibbonToolBarControlDefinition)
          {
            RibbonToolBarControlDefinition child = (RibbonToolBarControlDefinition) row.Children[index2];
            FrameworkElement control = this.GetControl(child);
            if (control != null)
            {
              if (addchildren)
              {
                this.AddVisualChild((Visual) control);
                this.AddLogicalChild((object) control);
                this.actualChildren.Add(control);
              }
              if (measure)
              {
                if (control is IRibbonControl ribbonControl)
                  ribbonControl.Size = child.Size;
                control.Width = child.Width;
                control.Measure(availableSize);
              }
              if (flag)
                control.Arrange(new System.Windows.Rect(x, y2, control.DesiredSize.Width, control.DesiredSize.Height));
              x += control.DesiredSize.Width;
            }
            else
              continue;
          }
          if (row.Children[index2] is RibbonToolBarControlGroupDefinition)
          {
            RibbonToolBarControlGroup controlGroup = this.GetControlGroup((RibbonToolBarControlGroupDefinition) row.Children[index2]);
            if (addchildren)
            {
              this.AddVisualChild((Visual) controlGroup);
              this.AddLogicalChild((object) controlGroup);
              this.actualChildren.Add((FrameworkElement) controlGroup);
            }
            if (measure)
            {
              controlGroup.IsFirstInRow = index2 == 0;
              controlGroup.IsLastInRow = index2 == row.Children.Count - 1;
              controlGroup.Measure(availableSize);
            }
            if (flag)
              controlGroup.Arrange(new System.Windows.Rect(x, y2, controlGroup.DesiredSize.Width, controlGroup.DesiredSize.Height));
            x += controlGroup.DesiredSize.Width;
          }
        }
        y1 = y2 + rowHeight;
        if (width < x)
          width = x;
        if (num5 < y1)
          num5 = y1;
      }
      return new System.Windows.Size(width, num5 + num3);
    }

    private double GetRowHeight(RibbonToolBarLayoutDefinition layoutDefinition)
    {
      foreach (RibbonToolBarRow row in (Collection<RibbonToolBarRow>) layoutDefinition.Rows)
      {
        using (IEnumerator<DependencyObject> enumerator = row.Children.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            DependencyObject current = enumerator.Current;
            RibbonToolBarControlDefinition controlDefinition = current as RibbonToolBarControlDefinition;
            RibbonToolBarControlGroupDefinition controlGroupDefinition = current as RibbonToolBarControlGroupDefinition;
            FrameworkElement frameworkElement = (FrameworkElement) null;
            if (controlDefinition != null)
              frameworkElement = this.GetControl(controlDefinition);
            else if (controlGroupDefinition != null)
              frameworkElement = (FrameworkElement) this.GetControlGroup(controlGroupDefinition);
            if (frameworkElement == null)
              return 0.0;
            frameworkElement.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return frameworkElement.DesiredSize.Height;
          }
        }
      }
      return 0.0;
    }

    public override FrameworkElement CreateQuickAccessItem() => (FrameworkElement) new Control();
  }
}
