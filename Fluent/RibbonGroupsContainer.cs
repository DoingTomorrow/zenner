// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonGroupsContainer
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class RibbonGroupsContainer : Panel, IScrollInfo
  {
    public static readonly DependencyProperty ReduceOrderProperty = DependencyProperty.Register(nameof (ReduceOrder), typeof (string), typeof (RibbonGroupsContainer), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(RibbonGroupsContainer.ReduceOrderPropertyChanged)));
    private string[] reduceOrder = new string[0];
    private int reduceOrderIndex;
    private ScrollData scrollData;

    public string ReduceOrder
    {
      get => (string) this.GetValue(RibbonGroupsContainer.ReduceOrderProperty);
      set => this.SetValue(RibbonGroupsContainer.ReduceOrderProperty, (object) value);
    }

    private static void ReduceOrderPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonGroupsContainer ribbonGroupsContainer = (RibbonGroupsContainer) d;
      ribbonGroupsContainer.reduceOrder = ((string) e.NewValue).Split(new char[2]
      {
        ',',
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      ribbonGroupsContainer.reduceOrderIndex = ribbonGroupsContainer.reduceOrder.Length - 1;
      ribbonGroupsContainer.InvalidateMeasure();
      ribbonGroupsContainer.InvalidateArrange();
    }

    public RibbonGroupsContainer()
    {
      this.Focusable = false;
      FocusManager.SetIsFocusScope((DependencyObject) this, false);
    }

    protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
    {
      return new UIElementCollection((UIElement) this, (FrameworkElement) this);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(double.PositiveInfinity, availableSize.Height);
      Size sizeIntermediate = this.GetChildrenDesiredSizeIntermediate();
      if (this.reduceOrder.Length == 0)
      {
        this.VerifyScrollData(availableSize.Width, sizeIntermediate.Width);
        return sizeIntermediate;
      }
      for (; sizeIntermediate.Width <= availableSize.Width && this.reduceOrderIndex < this.reduceOrder.Length - 1; sizeIntermediate = this.GetChildrenDesiredSizeIntermediate())
      {
        ++this.reduceOrderIndex;
        this.IncreaseGroupBoxSize(this.reduceOrder[this.reduceOrderIndex]);
      }
      for (; sizeIntermediate.Width > availableSize.Width && this.reduceOrderIndex >= 0; sizeIntermediate = this.GetChildrenDesiredSizeIntermediate())
      {
        this.DecreaseGroupBoxSize(this.reduceOrder[this.reduceOrderIndex]);
        --this.reduceOrderIndex;
      }
      foreach (object internalChild in this.InternalChildren)
      {
        if (internalChild is RibbonGroupBox ribbonGroupBox)
        {
          if (ribbonGroupBox.State != ribbonGroupBox.StateIntermediate || ribbonGroupBox.Scale != ribbonGroupBox.ScaleIntermediate)
          {
            ribbonGroupBox.SuppressCacheReseting = true;
            ribbonGroupBox.State = ribbonGroupBox.StateIntermediate;
            ribbonGroupBox.Scale = ribbonGroupBox.ScaleIntermediate;
            ribbonGroupBox.InvalidateLayout();
            ribbonGroupBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            ribbonGroupBox.SuppressCacheReseting = false;
          }
          if (ribbonGroupBox.DesiredSizeIntermediate != ribbonGroupBox.DesiredSize)
          {
            ribbonGroupBox.ClearCache();
            return this.MeasureOverride(availableSize);
          }
        }
      }
      this.VerifyScrollData(availableSize.Width, sizeIntermediate.Width);
      return sizeIntermediate;
    }

    private Size GetChildrenDesiredSizeIntermediate()
    {
      double width = 0.0;
      double num = 0.0;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        if (internalChild is RibbonGroupBox ribbonGroupBox)
        {
          Size sizeIntermediate = ribbonGroupBox.DesiredSizeIntermediate;
          width += sizeIntermediate.Width;
          num = Math.Max(num, sizeIntermediate.Height);
        }
      }
      return new Size(width, num);
    }

    private void IncreaseGroupBoxSize(string name)
    {
      RibbonGroupBox group = this.FindGroup(name);
      bool flag = name.StartsWith("(", StringComparison.OrdinalIgnoreCase);
      if (group == null)
        return;
      if (flag)
        ++group.ScaleIntermediate;
      else
        group.StateIntermediate = group.StateIntermediate != RibbonGroupBoxState.Large ? group.StateIntermediate - 1 : RibbonGroupBoxState.Large;
    }

    private void DecreaseGroupBoxSize(string name)
    {
      RibbonGroupBox group = this.FindGroup(name);
      bool flag = name.StartsWith("(", StringComparison.OrdinalIgnoreCase);
      if (group == null)
        return;
      if (flag)
        --group.ScaleIntermediate;
      else
        group.StateIntermediate = group.StateIntermediate != RibbonGroupBoxState.Collapsed ? group.StateIntermediate + 1 : group.StateIntermediate;
    }

    private RibbonGroupBox FindGroup(string name)
    {
      if (name.StartsWith("(", StringComparison.OrdinalIgnoreCase))
        name = name.Substring(1, name.Length - 2);
      foreach (FrameworkElement internalChild in this.InternalChildren)
      {
        if (internalChild.Name == name)
          return internalChild as RibbonGroupBox;
      }
      return (RibbonGroupBox) null;
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
      return finalSize;
    }

    public ScrollViewer ScrollOwner
    {
      get => this.ScrollData.ScrollOwner;
      set => this.ScrollData.ScrollOwner = value;
    }

    public void SetHorizontalOffset(double offset)
    {
      double num = RibbonGroupsContainer.CoerceOffset(RibbonGroupsContainer.ValidateInputOffset(offset, "HorizontalOffset"), this.scrollData.ExtentWidth, this.scrollData.ViewportWidth);
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
      double withMinimalScroll = RibbonGroupsContainer.ComputeScrollOffsetWithMinimalScroll(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
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
      double num = RibbonGroupsContainer.CoerceOffset(this.ScrollData.OffsetX, extentWidth, viewportWidth);
      bool flag2 = flag1 & viewportWidth == this.ScrollData.ViewportWidth & extentWidth == this.ScrollData.ExtentWidth & this.ScrollData.OffsetX == num;
      this.ScrollData.ViewportWidth = viewportWidth;
      this.ScrollData.ExtentWidth = extentWidth;
      this.ScrollData.OffsetX = num;
      if (flag2 || this.ScrollOwner == null)
        return;
      this.ScrollOwner.InvalidateScrollInfo();
    }

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
