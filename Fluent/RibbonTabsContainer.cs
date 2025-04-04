// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonTabsContainer
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class RibbonTabsContainer : Panel, IScrollInfo
  {
    private ScrollData scrollData;

    public RibbonTabsContainer() => this.Focusable = false;

    [SuppressMessage("Microsoft.Maintainability", "CA1502")]
    protected override Size MeasureOverride(Size availableSize)
    {
      if (this.InternalChildren.Count == 0)
        return base.MeasureOverride(availableSize);
      Size size = this.MeasureChildrenDesiredSize(availableSize);
      if (availableSize.Width > size.Width)
      {
        this.UpdateSeparators(false, false);
        this.VerifyScrollData(availableSize.Width, size.Width);
        return size;
      }
      double num1 = size.Width - availableSize.Width;
      double indent = (this.InternalChildren[0] as RibbonTabItem).Indent;
      RibbonTabItem[] array1 = this.InternalChildren.Cast<RibbonTabItem>().Where<RibbonTabItem>((Func<RibbonTabItem, bool>) (x => x.IsContextual && x.Visibility != Visibility.Collapsed)).ToArray<RibbonTabItem>();
      double length = (double) array1.Length;
      IEnumerable<RibbonTabItem> source = this.InternalChildren.Cast<RibbonTabItem>().Where<RibbonTabItem>((Func<RibbonTabItem, bool>) (x => !x.IsContextual && x.Visibility != Visibility.Collapsed));
      double num2 = (double) source.Count<RibbonTabItem>();
      double num3 = length + num2;
      if (num1 < num2 * indent * 2.0)
      {
        double num4 = num1 / num2;
        foreach (RibbonTabItem ribbonTabItem in source)
          ribbonTabItem.Measure(new Size(Math.Max(0.0, ribbonTabItem.DesiredSize.Width - num4), ribbonTabItem.DesiredSize.Height));
        Size childrenDesiredSize = this.GetChildrenDesiredSize();
        if (childrenDesiredSize.Width > availableSize.Width)
          childrenDesiredSize.Width = availableSize.Width;
        this.UpdateSeparators(true, false);
        this.VerifyScrollData(availableSize.Width, childrenDesiredSize.Width);
        return childrenDesiredSize;
      }
      if (num1 < num3 * indent * 2.0)
      {
        double num5 = num2 * indent * 2.0;
        double num6 = (num1 - num5) / length;
        foreach (RibbonTabItem ribbonTabItem in source)
        {
          double width = ribbonTabItem.DesiredSize.Width;
          ribbonTabItem.Measure(new Size(Math.Max(0.0, ribbonTabItem.DesiredSize.Width - indent * 2.0), ribbonTabItem.DesiredSize.Height));
          num1 -= width - ribbonTabItem.DesiredSize.Width;
        }
        foreach (RibbonTabItem ribbonTabItem in ((IEnumerable<RibbonTabItem>) array1).Reverse<RibbonTabItem>())
        {
          double width = ribbonTabItem.DesiredSize.Width;
          ribbonTabItem.Measure(new Size(Math.Max(0.0, ribbonTabItem.DesiredSize.Width - num6), ribbonTabItem.DesiredSize.Height));
          num1 -= width - ribbonTabItem.DesiredSize.Width;
          if (num1 < 0.0)
            break;
        }
        Size childrenDesiredSize = this.GetChildrenDesiredSize();
        if (childrenDesiredSize.Width > availableSize.Width)
          childrenDesiredSize.Width = availableSize.Width;
        this.UpdateSeparators(true, false);
        this.VerifyScrollData(availableSize.Width, childrenDesiredSize.Width);
        return childrenDesiredSize;
      }
      foreach (RibbonTabItem ribbonTabItem in source)
      {
        double width = ribbonTabItem.DesiredSize.Width;
        ribbonTabItem.Measure(new Size(Math.Max(0.0, ribbonTabItem.DesiredSize.Width - indent * 2.0), ribbonTabItem.DesiredSize.Height));
        num1 -= width - ribbonTabItem.DesiredSize.Width;
      }
      foreach (RibbonTabItem ribbonTabItem in ((IEnumerable<RibbonTabItem>) array1).Reverse<RibbonTabItem>())
      {
        double width = ribbonTabItem.DesiredSize.Width;
        ribbonTabItem.Measure(new Size(Math.Max(0.0, ribbonTabItem.DesiredSize.Width - indent * 2.0), ribbonTabItem.DesiredSize.Height));
        num1 -= width - ribbonTabItem.DesiredSize.Width;
        if (num1 < 0.0)
        {
          Size childrenDesiredSize = this.GetChildrenDesiredSize();
          if (childrenDesiredSize.Width > availableSize.Width)
            childrenDesiredSize.Width = availableSize.Width;
          this.UpdateSeparators(true, false);
          this.VerifyScrollData(availableSize.Width, childrenDesiredSize.Width);
          return childrenDesiredSize;
        }
      }
      RibbonTabItem[] array2 = source.OrderByDescending<RibbonTabItem, double>((Func<RibbonTabItem, double>) (x => x.DesiredSize.Width)).ToArray<RibbonTabItem>();
      double num7 = 0.0;
      int index1 = 0;
      for (int index2 = 0; index2 < array2.Length - 1; ++index2)
      {
        double num8 = array2[index2].DesiredSize.Width - array2[index2 + 1].DesiredSize.Width;
        num7 += num8 * (double) (index2 + 1);
        index1 = index2 + 1;
        if (num7 > num1)
          break;
      }
      if (num7 > num1)
      {
        double width = array2[index1].DesiredSize.Width;
        if (num7 > num1)
          width += (num7 - num1) / (double) index1;
        for (int index3 = 0; index3 < index1; ++index3)
          array2[index3].Measure(new Size(width, availableSize.Height));
        Size childrenDesiredSize = this.GetChildrenDesiredSize();
        if (childrenDesiredSize.Width > availableSize.Width)
          childrenDesiredSize.Width = availableSize.Width;
        this.UpdateSeparators(true, true);
        this.VerifyScrollData(availableSize.Width, childrenDesiredSize.Width);
        return childrenDesiredSize;
      }
      double num9 = ((IEnumerable<RibbonTabItem>) array2).Sum<RibbonTabItem>((Func<RibbonTabItem, double>) (x => x.DesiredSize.Width));
      double num10 = 30.0 * (double) array2.Length;
      if (num1 < num9 - num10)
      {
        double width = (num9 - num1) / num2;
        for (int index4 = 0; (double) index4 < num2; ++index4)
          array2[index4].Measure(new Size(width, availableSize.Height));
        Size childrenDesiredSize = this.GetChildrenDesiredSize();
        this.UpdateSeparators(true, true);
        this.VerifyScrollData(availableSize.Width, childrenDesiredSize.Width);
        return childrenDesiredSize;
      }
      for (int index5 = 0; (double) index5 < num2; ++index5)
        array2[index5].Measure(new Size(30.0, availableSize.Height));
      double num11 = num1 - (num9 - num10);
      RibbonTabItem[] array3 = ((IEnumerable<RibbonTabItem>) array1).OrderByDescending<RibbonTabItem, double>((Func<RibbonTabItem, double>) (x => x.DesiredSize.Width)).ToArray<RibbonTabItem>();
      double num12 = 0.0;
      int index6 = 0;
      for (int index7 = 0; index7 < array3.Length - 1; ++index7)
      {
        double num13 = array3[index7].DesiredSize.Width - array3[index7 + 1].DesiredSize.Width;
        num12 += num13 * (double) (index7 + 1);
        index6 = index7 + 1;
        if (num12 > num11)
          break;
      }
      if (num12 > num11)
      {
        double width = array3[index6].DesiredSize.Width;
        if (num12 > num11)
          width += (num12 - num11) / (double) index6;
        for (int index8 = 0; index8 < index6; ++index8)
          array3[index8].Measure(new Size(width, availableSize.Height));
        Size childrenDesiredSize = this.GetChildrenDesiredSize();
        if (childrenDesiredSize.Width > availableSize.Width)
          childrenDesiredSize.Width = availableSize.Width;
        this.UpdateSeparators(true, true);
        this.VerifyScrollData(availableSize.Width, childrenDesiredSize.Width);
        return childrenDesiredSize;
      }
      double width1 = Math.Max(30.0, (((IEnumerable<RibbonTabItem>) array3).Sum<RibbonTabItem>((Func<RibbonTabItem, double>) (x => x.DesiredSize.Width)) - num11) / length);
      for (int index9 = 0; index9 < array3.Length; ++index9)
        array3[index9].Measure(new Size(width1, availableSize.Height));
      Size childrenDesiredSize1 = this.GetChildrenDesiredSize();
      this.UpdateSeparators(true, true);
      this.VerifyScrollData(availableSize.Width, childrenDesiredSize1.Width);
      return childrenDesiredSize1;
    }

    private Size MeasureChildrenDesiredSize(Size availableSize)
    {
      double width = 0.0;
      double num = 0.0;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        internalChild.Measure(availableSize);
        width += internalChild.DesiredSize.Width;
        num = Math.Max(num, internalChild.DesiredSize.Height);
      }
      return new Size(width, num);
    }

    private Size GetChildrenDesiredSize()
    {
      double width = 0.0;
      double num = 0.0;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        width += internalChild.DesiredSize.Width;
        num = Math.Max(num, internalChild.DesiredSize.Height);
      }
      return new Size(width, num);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      System.Windows.Rect finalRect = new System.Windows.Rect(finalSize);
      finalRect.X = -this.HorizontalOffset;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        finalRect.Width = internalChild.DesiredSize.Width;
        finalRect.Height = Math.Max(finalSize.Height, internalChild.DesiredSize.Height);
        internalChild.Arrange(finalRect);
        finalRect.X += internalChild.DesiredSize.Width;
      }
      for (int index = 0; index < this.InternalChildren.Count; ++index)
      {
        if (this.InternalChildren[index] is RibbonTabItem && (this.InternalChildren[index] as RibbonTabItem).Group != null)
        {
          ((this.InternalChildren[index] as RibbonTabItem).Group.Parent as RibbonTitleBar).InvalidateMeasure();
          break;
        }
      }
      return finalSize;
    }

    private void UpdateSeparators(bool regularTabs, bool contextualTabs)
    {
      foreach (RibbonTabItem child in this.Children)
      {
        if (child.IsContextual)
        {
          if (child.IsSeparatorVisible != contextualTabs)
            child.IsSeparatorVisible = contextualTabs;
        }
        else if (child.IsSeparatorVisible != regularTabs)
          child.IsSeparatorVisible = regularTabs;
      }
    }

    public ScrollViewer ScrollOwner
    {
      get => this.ScrollData.ScrollOwner;
      set => this.ScrollData.ScrollOwner = value;
    }

    public void SetHorizontalOffset(double offset)
    {
      double num = RibbonTabsContainer.CoerceOffset(RibbonTabsContainer.ValidateInputOffset(offset, "HorizontalOffset"), this.scrollData.ExtentWidth, this.scrollData.ViewportWidth);
      if (this.ScrollData.OffsetX == num)
        return;
      this.scrollData.OffsetX = num;
      this.InvalidateArrange();
    }

    public double ExtentWidth => this.ScrollData.ExtentWidth;

    public double HorizontalOffset => this.ScrollData.OffsetX;

    public double ViewportWidth => this.ScrollData.ViewportWidth;

    public void LineLeft() => this.SetHorizontalOffset(this.HorizontalOffset - 16.0);

    public void LineRight() => this.SetHorizontalOffset(this.HorizontalOffset + 16.0);

    public System.Windows.Rect MakeVisible(Visual visual, System.Windows.Rect rectangle)
    {
      if (rectangle.IsEmpty || visual == null || visual == this || !this.IsAncestorOf((DependencyObject) visual))
        return System.Windows.Rect.Empty;
      rectangle = visual.TransformToAncestor((Visual) this).TransformBounds(rectangle);
      System.Windows.Rect rect = new System.Windows.Rect(this.HorizontalOffset, rectangle.Top, this.ViewportWidth, rectangle.Height);
      rectangle.X += rect.X;
      double withMinimalScroll = RibbonTabsContainer.ComputeScrollOffsetWithMinimalScroll(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
      this.SetHorizontalOffset(withMinimalScroll);
      rect.X = withMinimalScroll;
      rectangle.Intersect(rect);
      rectangle.X -= rect.X;
      return rectangle;
    }

    private static double ComputeScrollOffsetWithMinimalScroll(
      double topView,
      double bottomView,
      double topChild,
      double bottomChild)
    {
      bool flag1 = topChild < topView && bottomChild < bottomView;
      bool flag2 = bottomChild > bottomView && topChild > topView;
      bool flag3 = bottomChild - topChild > bottomView - topView;
      if (flag1 && !flag3 || flag2 && flag3)
        return topChild;
      return flag1 || flag2 ? bottomChild - (bottomView - topView) : topView;
    }

    public void MouseWheelDown()
    {
    }

    public void MouseWheelLeft()
    {
    }

    public void MouseWheelRight()
    {
    }

    public void MouseWheelUp()
    {
    }

    public void LineDown()
    {
    }

    public void LineUp()
    {
    }

    public void PageDown()
    {
    }

    public void PageLeft()
    {
    }

    public void PageRight()
    {
    }

    public void PageUp()
    {
    }

    public void SetVerticalOffset(double offset)
    {
    }

    public bool CanVerticallyScroll
    {
      get => false;
      set
      {
      }
    }

    public bool CanHorizontallyScroll
    {
      get => true;
      set
      {
      }
    }

    public double ExtentHeight => 0.0;

    public double VerticalOffset => 0.0;

    public double ViewportHeight => 0.0;

    private ScrollData ScrollData => this.scrollData ?? (this.scrollData = new ScrollData());

    private static double ValidateInputOffset(double offset, string parameterName)
    {
      return !double.IsNaN(offset) ? Math.Max(0.0, offset) : throw new ArgumentOutOfRangeException(parameterName);
    }

    private void VerifyScrollData(double viewportWidth, double extentWidth)
    {
      bool flag1 = true;
      if (double.IsInfinity(viewportWidth))
        viewportWidth = extentWidth;
      double b = RibbonTabsContainer.CoerceOffset(this.ScrollData.OffsetX, extentWidth, viewportWidth);
      bool flag2 = flag1 & RibbonTabsContainer.AreClose(viewportWidth, this.ScrollData.ViewportWidth) & RibbonTabsContainer.AreClose(extentWidth, this.ScrollData.ExtentWidth) & RibbonTabsContainer.AreClose(this.ScrollData.OffsetX, b);
      this.ScrollData.ViewportWidth = viewportWidth;
      this.ScrollData.ExtentWidth = !RibbonTabsContainer.AreClose(viewportWidth, extentWidth) ? extentWidth : viewportWidth;
      this.ScrollData.OffsetX = b;
      if (flag2 || this.ScrollOwner == null)
        return;
      this.ScrollOwner.InvalidateScrollInfo();
    }

    private static bool AreClose(double a, double b) => Math.Abs(a - b) < 1.5;

    private static double CoerceOffset(double offset, double extent, double viewport)
    {
      if (offset > extent - viewport)
        offset = extent - viewport;
      if (offset < 0.0)
        offset = 0.0;
      return offset;
    }
  }
}
