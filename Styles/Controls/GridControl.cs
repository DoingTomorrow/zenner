// Decompiled with JetBrains decompiler
// Type: Styles.Controls.GridControl
// Assembly: Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ABC9E615-D09A-48E5-A13F-BC53DD762FA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Styles.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Styles.Controls
{
  public class GridControl : Grid
  {
    public static readonly DependencyProperty ShowCustomGridLinesProperty = DependencyProperty.Register(nameof (ShowCustomGridLines), typeof (bool), typeof (GridControl), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register(nameof (GridLineBrush), typeof (Brush), typeof (GridControl), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Black));
    public static readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register(nameof (GridLineThickness), typeof (double), typeof (GridControl), (PropertyMetadata) new UIPropertyMetadata((object) 1.0));

    public bool ShowCustomGridLines
    {
      get => (bool) this.GetValue(GridControl.ShowCustomGridLinesProperty);
      set => this.SetValue(GridControl.ShowCustomGridLinesProperty, (object) value);
    }

    public Brush GridLineBrush
    {
      get => (Brush) this.GetValue(GridControl.GridLineBrushProperty);
      set => this.SetValue(GridControl.GridLineBrushProperty, (object) value);
    }

    public double GridLineThickness
    {
      get => (double) this.GetValue(GridControl.GridLineThicknessProperty);
      set => this.SetValue(GridControl.GridLineThicknessProperty, (object) value);
    }

    protected override void OnRender(DrawingContext dc)
    {
      if (this.ShowCustomGridLines)
      {
        foreach (RowDefinition rowDefinition in (IEnumerable<RowDefinition>) this.RowDefinitions)
          dc.DrawLine(new Pen(this.GridLineBrush, this.GridLineThickness), new Point(0.0, rowDefinition.Offset), new Point(this.ActualWidth, rowDefinition.Offset));
        foreach (ColumnDefinition columnDefinition in (IEnumerable<ColumnDefinition>) this.ColumnDefinitions)
          dc.DrawLine(new Pen(this.GridLineBrush, this.GridLineThickness), new Point(columnDefinition.Offset, 0.0), new Point(columnDefinition.Offset, this.ActualHeight));
        dc.DrawRectangle((Brush) Brushes.Transparent, new Pen(this.GridLineBrush, this.GridLineThickness), new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
      }
      base.OnRender(dc);
    }

    static GridControl()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (GridControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (GridControl)));
    }
  }
}
