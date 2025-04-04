// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonTitleBar
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_HeaderHolder", Type = typeof (FrameworkElement))]
  [TemplatePart(Name = "PART_QuickAccessToolbarHolder", Type = typeof (FrameworkElement))]
  [TemplatePart(Name = "PART_ItemsContainer", Type = typeof (Panel))]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (RibbonContextualTabGroup))]
  public class RibbonTitleBar : HeaderedItemsControl
  {
    private FrameworkElement quickAccessToolbarHolder;
    private FrameworkElement headerHolder;
    private Panel itemsContainer;
    private System.Windows.Rect quickAccessToolbarRect;
    private System.Windows.Rect headerRect;
    private System.Windows.Rect itemsRect;
    public static readonly DependencyProperty QuickAccessToolBarProperty = DependencyProperty.Register(nameof (QuickAccessToolBar), typeof (UIElement), typeof (RibbonTitleBar), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonTitleBar.OnQuickAccessToolbarChanged)));
    public static readonly DependencyProperty HeaderAlignmentProperty = DependencyProperty.Register(nameof (HeaderAlignment), typeof (HorizontalAlignment), typeof (RibbonTitleBar), (PropertyMetadata) new UIPropertyMetadata((object) HorizontalAlignment.Center));

    public UIElement QuickAccessToolBar
    {
      get => (UIElement) this.GetValue(RibbonTitleBar.QuickAccessToolBarProperty);
      set => this.SetValue(RibbonTitleBar.QuickAccessToolBarProperty, (object) value);
    }

    private static void OnQuickAccessToolbarChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((UIElement) d).InvalidateMeasure();
    }

    public HorizontalAlignment HeaderAlignment
    {
      get => (HorizontalAlignment) this.GetValue(RibbonTitleBar.HeaderAlignmentProperty);
      set => this.SetValue(RibbonTitleBar.HeaderAlignmentProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonTitleBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonTitleBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonTitleBar)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonTitleBar), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonTitleBar.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonTitleBar));
      return basevalue;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new RibbonContextualTabGroup();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is RibbonContextualTabGroup;
    }

    public override void OnApplyTemplate()
    {
      this.quickAccessToolbarHolder = this.GetTemplateChild("PART_QuickAccessToolbarHolder") as FrameworkElement;
      this.headerHolder = this.GetTemplateChild("PART_HeaderHolder") as FrameworkElement;
      this.itemsContainer = this.GetTemplateChild("PART_ItemsContainer") as Panel;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      if (this.quickAccessToolbarHolder == null || this.headerHolder == null || this.itemsContainer == null)
        return base.MeasureOverride(constraint);
      Size constraint1 = constraint;
      if (double.IsPositiveInfinity(constraint1.Width) || double.IsPositiveInfinity(constraint1.Height))
        constraint1 = base.MeasureOverride(constraint1);
      this.Update(constraint1);
      this.itemsContainer.Measure(this.itemsRect.Size);
      this.headerHolder.Measure(this.headerRect.Size);
      this.quickAccessToolbarHolder.Measure(this.quickAccessToolbarRect.Size);
      return constraint1;
    }

    protected override Size ArrangeOverride(Size arrangeBounds)
    {
      if (this.quickAccessToolbarHolder == null || this.headerHolder == null || this.itemsContainer == null)
        return base.ArrangeOverride(arrangeBounds);
      this.itemsContainer.Arrange(this.itemsRect);
      this.headerHolder.Arrange(this.headerRect);
      this.quickAccessToolbarHolder.Arrange(this.quickAccessToolbarRect);
      return arrangeBounds;
    }

    private void Update(Size constraint)
    {
      List<RibbonContextualTabGroup> contextualTabGroupList = new List<RibbonContextualTabGroup>();
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] is RibbonContextualTabGroup)
        {
          RibbonContextualTabGroup contextualTabGroup = this.Items[index] as RibbonContextualTabGroup;
          if (contextualTabGroup.Visibility == Visibility.Visible && contextualTabGroup.Items.Count > 0)
            contextualTabGroupList.Add(contextualTabGroup);
        }
      }
      Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
      if (contextualTabGroupList.Count == 0 || (contextualTabGroupList[0].Items[0].Parent as RibbonTabControl).CanScroll)
      {
        this.itemsRect = new System.Windows.Rect(0.0, 0.0, 0.0, 0.0);
        this.quickAccessToolbarHolder.Measure(availableSize);
        if (constraint.Width <= this.quickAccessToolbarHolder.DesiredSize.Width + 50.0)
        {
          this.quickAccessToolbarRect = new System.Windows.Rect(0.0, 0.0, Math.Max(0.0, constraint.Width - 50.0), this.quickAccessToolbarHolder.DesiredSize.Height);
          this.quickAccessToolbarHolder.Measure(this.quickAccessToolbarRect.Size);
        }
        if (constraint.Width > this.quickAccessToolbarHolder.DesiredSize.Width + 50.0)
        {
          this.quickAccessToolbarRect = new System.Windows.Rect(0.0, 0.0, this.quickAccessToolbarHolder.DesiredSize.Width, this.quickAccessToolbarHolder.DesiredSize.Height);
          this.headerHolder.Measure(availableSize);
          double num = constraint.Width - this.quickAccessToolbarHolder.DesiredSize.Width;
          if (this.HeaderAlignment == HorizontalAlignment.Left)
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarHolder.DesiredSize.Width, 0.0, Math.Min(num, this.headerHolder.DesiredSize.Width), constraint.Height);
          else if (this.HeaderAlignment == HorizontalAlignment.Center)
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarHolder.DesiredSize.Width + Math.Max(0.0, num / 2.0 - this.headerHolder.DesiredSize.Width / 2.0), 0.0, Math.Min(num, this.headerHolder.DesiredSize.Width), constraint.Height);
          else if (this.HeaderAlignment == HorizontalAlignment.Right)
          {
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarHolder.DesiredSize.Width + Math.Max(0.0, num - this.headerHolder.DesiredSize.Width), 0.0, Math.Min(num, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
          else
          {
            if (this.HeaderAlignment != HorizontalAlignment.Stretch)
              return;
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarHolder.DesiredSize.Width, 0.0, num, constraint.Height);
          }
        }
        else
          this.headerRect = new System.Windows.Rect(Math.Max(0.0, constraint.Width - 50.0), 0.0, 50.0, constraint.Height);
      }
      else
      {
        RibbonTabItem ribbonTabItem1 = contextualTabGroupList[0].Items[0];
        RibbonTabItem ribbonTabItem2 = contextualTabGroupList[contextualTabGroupList.Count - 1].Items[contextualTabGroupList[contextualTabGroupList.Count - 1].Items.Count - 1];
        double x1 = ribbonTabItem1.TranslatePoint(new Point(0.0, 0.0), (UIElement) this).X;
        double x2 = ribbonTabItem2.TranslatePoint(new Point(ribbonTabItem2.DesiredSize.Width, 0.0), (UIElement) this).X;
        this.itemsRect = new System.Windows.Rect(x1, 0.0, Math.Max(0.0, Math.Min(x2, constraint.Width) - x1), constraint.Height);
        this.quickAccessToolbarHolder.Measure(availableSize);
        double width1 = this.quickAccessToolbarHolder.DesiredSize.Width;
        this.quickAccessToolbarRect = new System.Windows.Rect(0.0, 0.0, Math.Min(width1, x1), this.quickAccessToolbarHolder.DesiredSize.Height);
        if (width1 > x1)
        {
          this.quickAccessToolbarHolder.Measure(this.quickAccessToolbarRect.Size);
          this.quickAccessToolbarRect = new System.Windows.Rect(0.0, 0.0, this.quickAccessToolbarHolder.DesiredSize.Width, this.quickAccessToolbarHolder.DesiredSize.Height);
          width1 = this.quickAccessToolbarHolder.DesiredSize.Width;
        }
        this.headerHolder.Measure(availableSize);
        if (this.HeaderAlignment == HorizontalAlignment.Left)
        {
          if (x1 - width1 > 150.0)
          {
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarRect.Width, 0.0, Math.Min(x1 - width1, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
          else
          {
            double val1 = Math.Max(0.0, constraint.Width - x2);
            this.headerRect = new System.Windows.Rect(Math.Min(x2, constraint.Width), 0.0, Math.Min(val1, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
        }
        else if (this.HeaderAlignment == HorizontalAlignment.Center)
        {
          if (x1 - width1 < 150.0 && x1 - width1 > 0.0 && x1 - width1 < constraint.Width - x2 || x2 < constraint.Width / 2.0)
          {
            double val1 = Math.Max(0.0, constraint.Width - x2);
            this.headerRect = new System.Windows.Rect(Math.Min(Math.Max(x2, constraint.Width / 2.0 - this.headerHolder.DesiredSize.Width / 2.0), constraint.Width), 0.0, Math.Min(val1, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
          else
          {
            double val1 = Math.Max(0.0, x1 - width1);
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarHolder.DesiredSize.Width + Math.Max(0.0, val1 / 2.0 - this.headerHolder.DesiredSize.Width / 2.0), 0.0, Math.Min(val1, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
        }
        else if (this.HeaderAlignment == HorizontalAlignment.Right)
        {
          if (x1 - width1 > 150.0)
          {
            double val1 = Math.Max(0.0, x1 - width1);
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarHolder.DesiredSize.Width + Math.Max(0.0, val1 - this.headerHolder.DesiredSize.Width), 0.0, Math.Min(val1, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
          else
          {
            double val1 = Math.Max(0.0, constraint.Width - x2);
            this.headerRect = new System.Windows.Rect(Math.Min(Math.Max(x2, constraint.Width - this.headerHolder.DesiredSize.Width), constraint.Width), 0.0, Math.Min(val1, this.headerHolder.DesiredSize.Width), constraint.Height);
          }
        }
        else
        {
          if (this.HeaderAlignment != HorizontalAlignment.Stretch)
            return;
          if (x1 - width1 > 150.0)
          {
            this.headerRect = new System.Windows.Rect(this.quickAccessToolbarRect.Width, 0.0, x1 - width1, constraint.Height);
          }
          else
          {
            double width2 = Math.Max(0.0, constraint.Width - x2);
            this.headerRect = new System.Windows.Rect(Math.Min(x2, constraint.Width), 0.0, width2, constraint.Height);
          }
        }
      }
    }
  }
}
