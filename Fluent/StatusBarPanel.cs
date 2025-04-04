// Decompiled with JetBrains decompiler
// Type: Fluent.StatusBarPanel
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Fluent
{
  public class StatusBarPanel : Panel
  {
    private List<UIElement> leftChildren = new List<UIElement>();
    private List<UIElement> rightChildren = new List<UIElement>();
    private List<UIElement> otherChildren = new List<UIElement>();
    private int lastRightIndex;
    private int lastLeftIndex;

    protected override Size MeasureOverride(Size availableSize)
    {
      this.leftChildren.Clear();
      this.rightChildren.Clear();
      this.otherChildren.Clear();
      for (int index = 0; index < this.InternalChildren.Count; ++index)
      {
        if (this.InternalChildren[index] is FrameworkElement internalChild)
        {
          if (internalChild.HorizontalAlignment == HorizontalAlignment.Left)
            this.leftChildren.Add((UIElement) internalChild);
          else if (internalChild.HorizontalAlignment == HorizontalAlignment.Right)
            this.rightChildren.Add((UIElement) internalChild);
          else
            this.otherChildren.Add((UIElement) internalChild);
        }
      }
      this.lastRightIndex = this.rightChildren.Count;
      this.lastLeftIndex = this.leftChildren.Count;
      Size availableSize1 = new Size(double.PositiveInfinity, double.PositiveInfinity);
      Size availableSize2 = new Size(0.0, 0.0);
      double width = 0.0;
      double num = 0.0;
      bool flag = true;
      for (int index = 0; index < this.rightChildren.Count; ++index)
      {
        if (flag)
        {
          this.rightChildren[index].Measure(availableSize1);
          num = Math.Max(this.rightChildren[index].DesiredSize.Height, num);
          if (width + this.rightChildren[index].DesiredSize.Width <= availableSize.Width)
          {
            width += this.rightChildren[index].DesiredSize.Width;
          }
          else
          {
            flag = false;
            this.rightChildren[index].Measure(availableSize2);
            this.lastRightIndex = index;
            this.lastLeftIndex = 0;
          }
        }
        else
          this.rightChildren[index].Measure(availableSize2);
      }
      for (int index = 0; index < this.leftChildren.Count; ++index)
      {
        if (flag)
        {
          this.leftChildren[index].Measure(availableSize1);
          num = Math.Max(this.leftChildren[index].DesiredSize.Height, num);
          if (width + this.leftChildren[index].DesiredSize.Width <= availableSize.Width)
          {
            width += this.leftChildren[index].DesiredSize.Width;
          }
          else
          {
            flag = false;
            this.leftChildren[index].Measure(availableSize2);
            this.lastLeftIndex = index;
          }
        }
        else
          this.leftChildren[index].Measure(availableSize2);
      }
      for (int index = 0; index < this.otherChildren.Count; ++index)
        this.otherChildren[index].Measure(availableSize2);
      return new Size(width, num);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      System.Windows.Rect finalRect = new System.Windows.Rect(0.0, 0.0, 0.0, 0.0);
      double num = 0.0;
      for (int index = this.rightChildren.Count - 1; index >= 0; --index)
      {
        if (this.lastRightIndex > index)
        {
          num += this.rightChildren[index].DesiredSize.Width;
          this.rightChildren[index].Arrange(new System.Windows.Rect(finalSize.Width - num, 0.0, this.rightChildren[index].DesiredSize.Width, finalSize.Height));
        }
        else
          this.rightChildren[index].Arrange(finalRect);
      }
      double x = 0.0;
      for (int index = 0; index < this.leftChildren.Count; ++index)
      {
        if (index < this.lastLeftIndex)
        {
          this.leftChildren[index].Arrange(new System.Windows.Rect(x, 0.0, this.leftChildren[index].DesiredSize.Width, finalSize.Height));
          x += this.leftChildren[index].DesiredSize.Width;
        }
        else
          this.leftChildren[index].Arrange(finalRect);
      }
      for (int index = 0; index < this.otherChildren.Count; ++index)
        this.otherChildren[index].Arrange(finalRect);
      return finalSize;
    }
  }
}
