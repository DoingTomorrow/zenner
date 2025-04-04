// Decompiled with JetBrains decompiler
// Type: Fluent.MenuPanel
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  public class MenuPanel : Panel
  {
    private bool isInvalidated;

    public double ResizeMinWidth { get; set; }

    public double ResizeMinHeight { get; set; }

    public MenuPanel() => this.Loaded += new RoutedEventHandler(this.OnMenuLoaded);

    private void OnMenuLoaded(object sender, RoutedEventArgs e) => this.UpdateMenuSizes();

    [SuppressMessage("Microsoft.Performance", "CA1800")]
    internal void UpdateMenuSizes()
    {
      if (this.Children.Count <= 0)
        return;
      this.Width = double.NaN;
      double num = 0.0;
      double val2 = 0.0;
      double val1 = 0.0;
      for (int index = 0; index < this.Children.Count; ++index)
      {
        if (this.Children[index] is FrameworkElement child)
        {
          switch (child)
          {
            case MenuItem _:
            case Separator _:
              child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
              if (child is MenuItem)
                num = Math.Max(num, child.MinWidth);
              if (!double.IsPositiveInfinity(child.DesiredSize.Width))
                val1 = Math.Max(val1, child.DesiredSize.Width);
              val2 += child.DesiredSize.Height;
              continue;
            case MenuPanel _:
              child.Width = double.NaN;
              break;
          }
          num = Math.Max(num, child.MinWidth);
          val2 += child.MinHeight;
        }
      }
      this.ResizeMinWidth = Math.Max(0.0, num);
      this.ResizeMinHeight = Math.Max(0.0, val2);
      if (this.ResizeMinHeight != 0.0)
        this.MinHeight = this.ResizeMinHeight;
      if (this.ResizeMinWidth != 0.0)
        this.Width = Math.Max(val1, this.ResizeMinWidth);
      if (this.ResizeMinWidth < val1)
        this.ResizeMinWidth = val1;
      if (!(VisualTreeHelper.GetParent((DependencyObject) this) is MenuPanel))
        return;
      this.Width = double.NaN;
    }

    protected override void OnVisualChildrenChanged(
      DependencyObject visualAdded,
      DependencyObject visualRemoved)
    {
      base.OnVisualChildrenChanged(visualAdded, visualRemoved);
      if (!this.IsLoaded)
        return;
      this.UpdateMenuSizes();
    }

    private void OnItemSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.InvalidateUpdateMenuSizes();
    }

    private void OnItemVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.InvalidateUpdateMenuSizes();
    }

    private void InvalidateUpdateMenuSizes()
    {
      if (this.isInvalidated)
        return;
      this.isInvalidated = true;
      this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
      {
        this.isInvalidated = false;
        this.UpdateMenuSizes();
        this.UpdateLayout();
      }));
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      double height1 = 0.0;
      double num = 0.0;
      if (!double.IsPositiveInfinity(availableSize.Width))
        num = availableSize.Width;
      List<UIElement> uiElementList = new List<UIElement>();
      foreach (object internalChild in this.InternalChildren)
      {
        switch (internalChild)
        {
          case MenuItem menuItem when menuItem.Visibility != Visibility.Collapsed:
            menuItem.Measure(availableSize);
            height1 += menuItem.DesiredSize.Height;
            num = Math.Max(num, menuItem.DesiredSize.Width);
            continue;
          case Separator separator when separator.Visibility != Visibility.Collapsed:
            separator.Measure(availableSize);
            height1 += separator.DesiredSize.Height;
            num = Math.Max(num, separator.DesiredSize.Width);
            continue;
          case UIElement uiElement:
            uiElementList.Add(uiElement);
            continue;
          default:
            continue;
        }
      }
      if (!double.IsPositiveInfinity(availableSize.Height))
      {
        if (height1 < availableSize.Height)
        {
          double height2 = (availableSize.Height - height1) / (double) uiElementList.Count;
          foreach (FrameworkElement frameworkElement in uiElementList)
          {
            if (frameworkElement.Visibility != Visibility.Collapsed)
            {
              frameworkElement.Measure(new Size(availableSize.Width, height2));
              num = Math.Max(num, Math.Max(frameworkElement.DesiredSize.Width, frameworkElement.MinWidth));
              height1 += Math.Max(frameworkElement.DesiredSize.Height, frameworkElement.MinHeight);
            }
          }
        }
        else
        {
          foreach (FrameworkElement frameworkElement in uiElementList)
          {
            if (frameworkElement.Visibility != Visibility.Collapsed)
            {
              frameworkElement.Measure(new Size());
              num = Math.Max(num, Math.Max(frameworkElement.DesiredSize.Width, frameworkElement.MinWidth));
              height1 += Math.Max(frameworkElement.DesiredSize.Height, frameworkElement.MinHeight);
            }
          }
        }
      }
      else
      {
        foreach (FrameworkElement frameworkElement in uiElementList)
        {
          if (frameworkElement.Visibility != Visibility.Collapsed)
          {
            frameworkElement.Measure(availableSize);
            num = Math.Max(num, Math.Max(frameworkElement.DesiredSize.Width, frameworkElement.MinWidth));
            height1 += Math.Max(frameworkElement.DesiredSize.Height, frameworkElement.MinHeight);
          }
        }
      }
      if (num < this.ResizeMinWidth)
        num = this.ResizeMinWidth;
      if (num > availableSize.Width)
        num = availableSize.Width;
      if (height1 < this.ResizeMinHeight)
        height1 = this.ResizeMinHeight;
      return new Size(num, height1);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      double num1 = 0.0;
      List<UIElement> uiElementList = new List<UIElement>();
      foreach (object internalChild in this.InternalChildren)
      {
        switch (internalChild)
        {
          case MenuItem menuItem when menuItem.Visibility != Visibility.Collapsed:
            num1 += menuItem.DesiredSize.Height;
            continue;
          case Separator separator when separator.Visibility != Visibility.Collapsed:
            num1 += separator.DesiredSize.Height;
            continue;
          default:
            uiElementList.Add(internalChild as UIElement);
            continue;
        }
      }
      double y = 0.0;
      double num2 = Math.Max(0.0, (finalSize.Height - num1) / (double) uiElementList.Count);
      foreach (object internalChild in this.InternalChildren)
      {
        if (internalChild is UIElement uiElement && uiElement.Visibility != Visibility.Collapsed)
        {
          double height = num2;
          switch (uiElement)
          {
            case MenuItem _:
            case Separator _:
            case MenuPanel _:
              height = uiElement.DesiredSize.Height;
              break;
          }
          uiElement.Arrange(new System.Windows.Rect(0.0, y, finalSize.Width, height));
          y += height;
        }
      }
      return finalSize;
    }
  }
}
