// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonContextualGroupsContainer
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class RibbonContextualGroupsContainer : Panel
  {
    private readonly List<Size> sizes = new List<Size>();

    protected override Size ArrangeOverride(Size finalSize)
    {
      System.Windows.Rect finalRect = new System.Windows.Rect(finalSize);
      int index = 0;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        finalRect.Width = this.sizes[index].Width;
        finalRect.Height = Math.Max(finalSize.Height, this.sizes[index].Height);
        internalChild.Arrange(finalRect);
        finalRect.X += this.sizes[index].Width;
        ++index;
      }
      return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      double width1 = 0.0;
      this.sizes.Clear();
      Size availableSize1 = new Size(double.PositiveInfinity, double.PositiveInfinity);
      foreach (RibbonContextualTabGroup internalChild in this.InternalChildren)
      {
        double num1 = 0.0;
        for (int index = 0; index < internalChild.Items.Count; ++index)
          num1 += internalChild.Items[index].DesiredSize.Width;
        internalChild.Measure(availableSize1);
        double width2 = internalChild.DesiredSize.Width;
        bool flag = false;
        if (width2 > num1)
        {
          double num2 = (width2 - num1) / (double) internalChild.Items.Count;
          for (int index = 0; index < internalChild.Items.Count; ++index)
          {
            if (internalChild.Items[index].DesiredWidth == 0.0)
            {
              internalChild.Items[index].DesiredWidth = internalChild.Items[index].DesiredSize.Width + num2;
              internalChild.Items[index].Measure(new Size(internalChild.Items[index].DesiredWidth, internalChild.Items[index].DesiredSize.Height));
              flag = true;
            }
          }
        }
        if (flag)
        {
          for (Visual parent = (Visual) internalChild.Items[0]; parent != null; parent = VisualTreeHelper.GetParent((DependencyObject) parent) as Visual)
          {
            if (parent is UIElement uiElement)
            {
              if (uiElement is RibbonTabsContainer)
              {
                uiElement.InvalidateMeasure();
                break;
              }
              uiElement.InvalidateMeasure();
            }
          }
          num1 = 0.0;
          for (int index = 0; index < internalChild.Items.Count; ++index)
            num1 += internalChild.Items[index].DesiredSize.Width;
        }
        double val2 = num1;
        width1 += val2;
        if (width1 > availableSize.Width)
        {
          val2 -= width1 - availableSize.Width;
          width1 = availableSize.Width;
        }
        internalChild.Measure(new Size(Math.Max(0.0, val2), availableSize.Height));
        this.sizes.Add(new Size(Math.Max(0.0, val2), availableSize.Height));
      }
      double num = availableSize.Height;
      if (double.IsPositiveInfinity(num))
        num = 0.0;
      return new Size(width1, num);
    }
  }
}
