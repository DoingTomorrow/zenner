// Decompiled with JetBrains decompiler
// Type: CommonWPF.GmmGraphicControl
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace CommonWPF
{
  public partial class GmmGraphicControl : UserControl, IComponentConnector
  {
    private GmmGraphicControl.GraphicData MyGraphicData;
    internal Canvas CanvasDrawing;
    private bool _contentLoaded;

    public GmmGraphicControl()
    {
      this.InitializeComponent();
      this.MyGraphicData = (GmmGraphicControl.GraphicData) null;
    }

    private void CanvasDrawing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
    }

    private void CanvasDrawing_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
    }

    public void ClearGraphic() => this.CanvasDrawing.Children.Clear();

    public void ShowGraphic(GmmGraphicControl.GraphicData TheGraphicData)
    {
      this.MyGraphicData = TheGraphicData;
      if (this.MyGraphicData.MyPointSizeDivider < 10.0 || this.MyGraphicData.MyPointSizeDivider > 2000.0)
        throw new ApplicationException("GmmGraphicControl: Invalid point size factor (" + this.MyGraphicData.MyPointSizeDivider.ToString() + ")");
      this.MyGraphicData.MyRange.MinY = 0.0;
      this.MyGraphicData.MyRange.MaxY = this.CanvasDrawing.ActualHeight;
      this.MyGraphicData.MyRange.MinX = 0.0;
      this.MyGraphicData.MyRange.MaxX = this.CanvasDrawing.ActualWidth;
      FontFamily fontFamily = new FontFamily("Courier New");
      double fontSize = this.GetFontSize(20.0);
      double num1 = this.CanvasDrawing.ActualWidth / 10.0;
      double num2 = this.CanvasDrawing.ActualHeight / 6.0;
      double num3 = this.CanvasDrawing.ActualHeight / 5.0;
      if (this.MyGraphicData.MyYLimits == null)
        this.MyGraphicData.MyYLimits = this.GetYLimits(this.MyGraphicData);
      if (this.MyGraphicData.MyXLimits == null)
        this.MyGraphicData.MyXLimits = this.GetXLimits(this.MyGraphicData);
      if (this.MyGraphicData.MyYAxis == null)
      {
        double TheStep = Math.Abs((this.MyGraphicData.MyYLimits.Max - this.MyGraphicData.MyYLimits.Min) / 5.0);
        string TheCaption = "Y-Axis";
        if (this.MyGraphicData.MyYAxisCaption != null)
          TheCaption = this.MyGraphicData.MyYAxisCaption;
        int num4 = 0;
        if (this.MyGraphicData.MyYLimits.Min < 0.0)
        {
          while (true)
          {
            ++num4;
            if ((double) (-1 * num4) * TheStep < this.MyGraphicData.MyYLimits.Min)
              break;
          }
          this.MyGraphicData.MyYAxis = new GmmGraphicControl.Yaxis((double) (-1 * num4) * TheStep, TheStep, TheCaption, true);
        }
        else
        {
          int num5 = 0;
          while (true)
          {
            ++num5;
            if (TheStep * (double) num5 <= this.MyGraphicData.MyYLimits.Min)
              this.MyGraphicData.MyYAxis = new GmmGraphicControl.Yaxis((double) (num5 - 1) * TheStep, TheStep, TheCaption, true);
            else
              break;
          }
        }
      }
      if (this.MyGraphicData.MyXAxis == null)
      {
        double TheStep = Math.Abs((this.MyGraphicData.MyXLimits.Max - this.MyGraphicData.MyXLimits.Min) / 5.0);
        string TheCaption = "X-Axis";
        if (this.MyGraphicData.MyXAxisCaption != null)
          TheCaption = this.MyGraphicData.MyXAxisCaption;
        int num6 = 0;
        if (this.MyGraphicData.MyXLimits.Min < 0.0)
        {
          while (true)
          {
            ++num6;
            if ((double) (-1 * num6) * TheStep < this.MyGraphicData.MyXLimits.Min)
              break;
          }
          this.MyGraphicData.MyXAxis = new GmmGraphicControl.Xaxis((double) (-1 * num6) * TheStep, TheStep, TheCaption, true);
        }
        else
        {
          int num7 = 0;
          while (true)
          {
            ++num7;
            if (TheStep * (double) num7 <= this.MyGraphicData.MyXLimits.Min)
              this.MyGraphicData.MyXAxis = new GmmGraphicControl.Xaxis((double) (num7 - 1) * TheStep, TheStep, TheCaption, true);
            else
              break;
          }
        }
      }
      if (this.MyGraphicData.ShowGrid && this.MyGraphicData.MyGrid == null)
        this.MyGraphicData.AddGrid(this.MyGraphicData.MyXAxis.MyStarValue, this.MyGraphicData.MyXAxis.MyStep, this.MyGraphicData.MyYAxis.MyStarValue, this.MyGraphicData.MyYAxis.MyStep);
      if (this.MyGraphicData.MyYAxis != null || this.MyGraphicData.MyXAxis != null)
      {
        this.MyGraphicData.MyRange.MinX = num1;
        this.MyGraphicData.MyRange.MinY = num2;
        this.MyGraphicData.MyRange.MaxY -= num2;
        if (this.MyGraphicData.MyXAxis != null && this.MyGraphicData.MyXAxis.ShowValues)
          this.MyGraphicData.MyRange.MinY = !(this.MyGraphicData.MyXAxis.Caption != string.Empty) ? num3 / 2.0 : num3;
      }
      if (this.MyGraphicData.MyXAxis != null)
        this.MyGraphicData.MyRange.MaxX -= num1;
      this.CanvasDrawing.Background = (Brush) new SolidColorBrush(this.MyGraphicData.MyBackGroundColor);
      this.CanvasDrawing.Children.Clear();
      double min1 = this.MyGraphicData.MyXLimits.Min;
      double max1 = this.MyGraphicData.MyXLimits.Max;
      double min2 = this.MyGraphicData.MyYLimits.Min;
      double max2 = this.MyGraphicData.MyYLimits.Max;
      double minY = this.MyGraphicData.MyRange.MinY;
      double maxY = this.MyGraphicData.MyRange.MaxY;
      double minX = this.MyGraphicData.MyRange.MinX;
      double maxX = this.MyGraphicData.MyRange.MaxX;
      if (this.MyGraphicData.MyGrid != null)
      {
        if (this.MyGraphicData.MyGrid.MyStepX <= 0.0)
          throw new ApplicationException("GmmGraphicControl: Invalid Grid Stepx (" + this.MyGraphicData.MyGrid.MyStepX.ToString() + ")");
        if (this.MyGraphicData.MyGrid.MyStepY <= 0.0)
          throw new ApplicationException("GmmGraphicControl: Invalid Grid StepY " + this.MyGraphicData.MyGrid.MyStepY.ToString() + ")");
        double num8 = 0.0;
        double num9 = this.MyGraphicData.MyGrid.MyStepX / 1000.0;
        while (true)
        {
          Polyline element = new Polyline();
          double TheValue = this.MyGraphicData.MyGrid.MyStartX + num8 * this.MyGraphicData.MyGrid.MyStepX;
          if (TheValue >= this.MyGraphicData.MyXLimits.Min - num9 && TheValue <= this.MyGraphicData.MyXLimits.Max + num9)
          {
            double min3 = this.MyGraphicData.MyYLimits.Min;
            double max3 = this.MyGraphicData.MyYLimits.Max;
            double x = this.TransValue(TheValue, min1, max1, minX, maxX);
            double y1 = this.CanvasDrawing.ActualHeight - this.TransValue(min3, min2, max2, minY, maxY);
            double y2 = this.CanvasDrawing.ActualHeight - this.TransValue(max3, min2, max2, minY, maxY);
            System.Windows.Point point1 = new System.Windows.Point(x, y1);
            System.Windows.Point point2 = new System.Windows.Point(x, y2);
            element.Points.Add(point1);
            element.Points.Add(point2);
            element.Stroke = (Brush) new SolidColorBrush(this.MyGraphicData.MyGridColor);
            element.StrokeThickness = 1.0;
            this.CanvasDrawing.Children.Add((UIElement) element);
          }
          if (TheValue < this.MyGraphicData.MyXLimits.Max)
            ++num8;
          else
            break;
        }
        double num10 = 0.0;
        double num11 = this.MyGraphicData.MyGrid.MyStepY / 1000.0;
        while (true)
        {
          Polyline element = new Polyline();
          double TheValue1 = this.MyGraphicData.MyGrid.MyStartY + num10 * this.MyGraphicData.MyGrid.MyStepY;
          if (TheValue1 >= this.MyGraphicData.MyYLimits.Min - num11 && TheValue1 <= this.MyGraphicData.MyYLimits.Max + num11)
          {
            double TheValue2 = min1;
            double TheValue3 = max1;
            double x1 = this.TransValue(TheValue2, min1, max1, minX, maxX);
            double x2 = this.TransValue(TheValue3, min1, max1, minX, maxX);
            double y = this.CanvasDrawing.ActualHeight - this.TransValue(TheValue1, min2, max2, minY, maxY);
            System.Windows.Point point3 = new System.Windows.Point(x1, y);
            System.Windows.Point point4 = new System.Windows.Point(x2, y);
            element.Points.Add(point3);
            element.Points.Add(point4);
            element.Stroke = (Brush) new SolidColorBrush(this.MyGraphicData.MyGridColor);
            element.StrokeThickness = 1.0;
            this.CanvasDrawing.Children.Add((UIElement) element);
          }
          if (TheValue1 < this.MyGraphicData.MyYLimits.Max)
            ++num10;
          else
            break;
        }
      }
      if (this.MyGraphicData.MyYAxis != null)
      {
        if (this.MyGraphicData.MyYAxis.MyStep <= 0.0)
          throw new ApplicationException("GmmGraphicControl: Invalid Y-Axis Step " + this.MyGraphicData.MyYAxis.MyStep.ToString() + ")");
        Polyline element1 = new Polyline();
        double TheValue4 = this.MyGraphicData.MyXLimits.Min >= 0.0 || this.MyGraphicData.MyXLimits.Max <= 0.0 ? this.MyGraphicData.MyXLimits.Min : 0.0;
        double min4 = this.MyGraphicData.MyYLimits.Min;
        double max4 = this.MyGraphicData.MyYLimits.Max;
        double x3 = this.TransValue(TheValue4, min1, max1, minX, maxX);
        double y3 = this.CanvasDrawing.ActualHeight - this.TransValue(min4, min2, max2, minY, maxY);
        double y4 = this.CanvasDrawing.ActualHeight - this.TransValue(max4, min2, max2, minY, maxY);
        System.Windows.Point point5 = new System.Windows.Point(x3, y3);
        System.Windows.Point point6 = new System.Windows.Point(x3, y4);
        element1.Points.Add(point5);
        element1.Points.Add(point6);
        element1.Stroke = (Brush) new SolidColorBrush(this.MyGraphicData.MyAxisColor);
        element1.StrokeThickness = 3.0;
        this.CanvasDrawing.Children.Add((UIElement) element1);
        double num12 = this.MyGraphicData.MyYAxis.MyStep / 1000.0;
        double num13 = 0.0;
        int num14 = 0;
        if (this.MyGraphicData.MyYAxis.ShowValues)
        {
          while (true)
          {
            double num15 = this.MyGraphicData.MyYAxis.MyStarValue + num13 * this.MyGraphicData.MyYAxis.MyStep;
            if (num15 >= this.MyGraphicData.MyYLimits.Min - num12 && num15 <= this.MyGraphicData.MyYLimits.Max + num12 && num15.ToString().Length > num14)
              num14 = num15.ToString().Length;
            if (num15 <= this.MyGraphicData.MyYLimits.Max + num12)
              ++num13;
            else
              break;
          }
        }
        double num16 = 0.0;
        while (true)
        {
          Polyline element2 = new Polyline();
          double TheValue5 = this.MyGraphicData.MyYAxis.MyStarValue + num16 * this.MyGraphicData.MyYAxis.MyStep;
          if (TheValue5 >= this.MyGraphicData.MyYLimits.Min - num12 && TheValue5 <= this.MyGraphicData.MyYLimits.Max + num12)
          {
            double x4 = this.TransValue(TheValue4, min1, max1, minX, maxX);
            double x5 = x4 - this.CanvasDrawing.ActualWidth / 70.0;
            double y5 = this.CanvasDrawing.ActualHeight - this.TransValue(TheValue5, min2, max2, minY, maxY);
            System.Windows.Point point7 = new System.Windows.Point(x5, y5);
            System.Windows.Point point8 = new System.Windows.Point(x4, y5);
            element2.Points.Add(point7);
            element2.Points.Add(point8);
            element2.Stroke = (Brush) new SolidColorBrush(this.MyGraphicData.MyAxisColor);
            element2.StrokeThickness = 3.0;
            this.CanvasDrawing.Children.Add((UIElement) element2);
            if (this.MyGraphicData.MyYAxis.ShowValues)
            {
              Label element3 = new Label();
              element3.Content = (object) TheValue5.ToString().Trim();
              element3.FontFamily = fontFamily;
              element3.FontSize = fontSize;
              double left = x5 - (double) num14 * element3.FontSize;
              element3.Margin = new Thickness(left, y5 - 1.3 * element3.FontSize, 0.0, 0.0);
              element3.Foreground = (Brush) new SolidColorBrush(this.MyGraphicData.MyLabelColor);
              this.CanvasDrawing.Children.Add((UIElement) element3);
            }
          }
          if (TheValue5 <= this.MyGraphicData.MyYLimits.Max + num12)
            ++num16;
          else
            break;
        }
        Label element4 = new Label();
        element4.Content = (object) this.MyGraphicData.MyYAxis.Caption;
        element4.FontFamily = fontFamily;
        element4.FontSize = fontSize;
        element4.Margin = new Thickness(5.0, y4 / 3.0, 0.0, 0.0);
        element4.Foreground = (Brush) new SolidColorBrush(this.MyGraphicData.MyLabelColor);
        this.CanvasDrawing.Children.Add((UIElement) element4);
      }
      if (this.MyGraphicData.MyXAxis != null)
      {
        if (this.MyGraphicData.MyXAxis.MyStep <= 0.0)
          throw new ApplicationException("GmmGraphicControl: Invalid X-Axis Step " + this.MyGraphicData.MyXAxis.MyStep.ToString() + ")");
        Polyline element5 = new Polyline();
        double min5 = this.MyGraphicData.MyXLimits.Min;
        double max5 = this.MyGraphicData.MyXLimits.Max;
        double TheValue6 = this.MyGraphicData.MyYLimits.Min >= 0.0 || this.MyGraphicData.MyYLimits.Max <= 0.0 ? this.MyGraphicData.MyYLimits.Min : 0.0;
        double x6 = this.TransValue(min5, min1, max1, minX, maxX);
        double x7 = this.TransValue(max5, min1, max1, minX, maxX);
        double y6 = this.CanvasDrawing.ActualHeight - this.TransValue(TheValue6, min2, max2, minY, maxY);
        System.Windows.Point point9 = new System.Windows.Point(x6, y6);
        System.Windows.Point point10 = new System.Windows.Point(x7, y6);
        element5.Points.Add(point9);
        element5.Points.Add(point10);
        element5.Stroke = (Brush) new SolidColorBrush(this.MyGraphicData.MyAxisColor);
        element5.StrokeThickness = 3.0;
        this.CanvasDrawing.Children.Add((UIElement) element5);
        double num17 = this.MyGraphicData.MyXAxis.MyStep / 1000.0;
        double num18 = 0.0;
        double y7 = this.CanvasDrawing.ActualHeight - this.TransValue(TheValue6, min2, max2, minY, maxY);
        double num19 = y7 + this.CanvasDrawing.ActualHeight / 50.0;
        while (true)
        {
          Polyline element6 = new Polyline();
          double TheValue7 = this.MyGraphicData.MyXAxis.MyStarValue + num18 * this.MyGraphicData.MyXAxis.MyStep;
          if (TheValue7 >= this.MyGraphicData.MyXLimits.Min - num17 && TheValue7 <= this.MyGraphicData.MyXLimits.Max + num17)
          {
            double x8 = this.TransValue(TheValue7, min1, max1, minX, maxX);
            System.Windows.Point point11 = new System.Windows.Point(x8, y7);
            System.Windows.Point point12 = new System.Windows.Point(x8, num19);
            element6.Points.Add(point11);
            element6.Points.Add(point12);
            element6.Stroke = (Brush) new SolidColorBrush(this.MyGraphicData.MyAxisColor);
            element6.StrokeThickness = 3.0;
            this.CanvasDrawing.Children.Add((UIElement) element6);
            if (this.MyGraphicData.MyXAxis.ShowValues)
            {
              Label element7 = new Label();
              element7.Content = (object) (this.MyGraphicData.MyXAxis.MyStarValue + num18 * this.MyGraphicData.MyXAxis.MyStep).ToString().Trim();
              element7.FontFamily = fontFamily;
              element7.FontSize = fontSize;
              element7.Margin = new Thickness(x8 - fontSize, num19, 0.0, 0.0);
              element7.Foreground = (Brush) new SolidColorBrush(this.MyGraphicData.MyLabelColor);
              this.CanvasDrawing.Children.Add((UIElement) element7);
            }
          }
          if (TheValue7 <= this.MyGraphicData.MyXLimits.Max + num17)
            ++num18;
          else
            break;
        }
        Label element8 = new Label();
        element8.Content = (object) this.MyGraphicData.MyXAxis.Caption;
        element8.FontFamily = fontFamily;
        element8.FontSize = fontSize;
        double left = this.TransValue(max1, min1, max1, minX, maxX) - (double) element8.Content.ToString().Length * fontSize / 2.0;
        double top = this.CanvasDrawing.ActualHeight - (this.CanvasDrawing.ActualHeight - num19) / 2.0;
        element8.Margin = new Thickness(left, top, 0.0, 0.0);
        element8.Foreground = (Brush) new SolidColorBrush(this.MyGraphicData.MyLabelColor);
        this.CanvasDrawing.Children.Add((UIElement) element8);
      }
      if (this.MyGraphicData.MyGraphicType == GmmGraphicControl.GraphicType.Line)
      {
        foreach (GmmGraphicControl.LineClass lineClass in (IEnumerable<GmmGraphicControl.LineClass>) this.MyGraphicData.MyLines.Values)
        {
          this.CanvasDrawing.Children.Add((UIElement) this.GetLine(lineClass.MyValues, lineClass.MyColor, this.MyGraphicData));
          if (lineClass.ShowMarker)
          {
            foreach (UIElement marker in this.GetMarkers(lineClass.MyValues, lineClass.MyMarkerColor, lineClass.FillMarker, this.MyGraphicData))
              this.CanvasDrawing.Children.Add(marker);
          }
        }
      }
      else
      {
        foreach (GmmGraphicControl.XYLineClass xyLineClass in (IEnumerable<GmmGraphicControl.XYLineClass>) this.MyGraphicData.MyXYLines.Values)
        {
          this.CanvasDrawing.Children.Add((UIElement) this.GetLine(xyLineClass.MyValues, xyLineClass.MyColor, this.MyGraphicData));
          if (xyLineClass.ShowMarker)
          {
            foreach (UIElement marker in this.GetMarkers(xyLineClass.MyValues, xyLineClass.MyMarkerColor, xyLineClass.FillMarker, this.MyGraphicData))
              this.CanvasDrawing.Children.Add(marker);
          }
        }
      }
    }

    private Polyline GetLine(
      List<double> TheData,
      Color TheColor,
      GmmGraphicControl.GraphicData TheGraphicData)
    {
      Polyline line = new Polyline();
      double min1 = TheGraphicData.MyYLimits.Min;
      double max1 = TheGraphicData.MyYLimits.Max;
      double min2 = TheGraphicData.MyXLimits.Min;
      double max2 = TheGraphicData.MyXLimits.Max;
      double minY = TheGraphicData.MyRange.MinY;
      double maxY = TheGraphicData.MyRange.MaxY;
      double minX = TheGraphicData.MyRange.MinX;
      double maxX = TheGraphicData.MyRange.MaxX;
      for (int index = 0; index < TheData.Count; ++index)
      {
        if ((double) index >= min2 && (double) index <= max2)
        {
          System.Windows.Point point = new System.Windows.Point(this.TransValue((double) index, min2, max2, minX, maxX), this.CanvasDrawing.ActualHeight - this.TransValue(TheData[index], min1, max1, minY, maxY));
          line.Points.Add(point);
        }
      }
      line.Stroke = (Brush) new SolidColorBrush(TheColor);
      line.StrokeThickness = 2.0;
      return line;
    }

    private Polyline GetLine(
      List<GmmGraphicControl.XYData> TheData,
      Color TheColor,
      GmmGraphicControl.GraphicData TheGraphicData)
    {
      Polyline line = new Polyline();
      double min1 = TheGraphicData.MyYLimits.Min;
      double max1 = TheGraphicData.MyYLimits.Max;
      double min2 = TheGraphicData.MyXLimits.Min;
      double max2 = TheGraphicData.MyXLimits.Max;
      double minY = TheGraphicData.MyRange.MinY;
      double maxY = TheGraphicData.MyRange.MaxY;
      double minX = TheGraphicData.MyRange.MinX;
      double maxX = TheGraphicData.MyRange.MaxX;
      for (int index = 0; index < TheData.Count; ++index)
      {
        if (TheData[index].X >= min2 && TheData[index].X <= max2)
        {
          System.Windows.Point point = new System.Windows.Point(this.TransValue(TheData[index].X, min2, max2, minX, maxX), this.CanvasDrawing.ActualHeight - this.TransValue(TheData[index].Y, min1, max1, minY, maxY));
          line.Points.Add(point);
        }
      }
      line.Stroke = (Brush) new SolidColorBrush(TheColor);
      line.StrokeThickness = 2.0;
      return line;
    }

    private List<Ellipse> GetMarkers(
      List<double> TheData,
      Color TheColor,
      bool FillTheMarker,
      GmmGraphicControl.GraphicData TheGraphicData)
    {
      List<Ellipse> markers = new List<Ellipse>();
      double min1 = TheGraphicData.MyYLimits.Min;
      double max1 = TheGraphicData.MyYLimits.Max;
      double min2 = TheGraphicData.MyXLimits.Min;
      double max2 = TheGraphicData.MyXLimits.Max;
      double minY = TheGraphicData.MyRange.MinY;
      double maxY = TheGraphicData.MyRange.MaxY;
      double minX = TheGraphicData.MyRange.MinX;
      double maxX = TheGraphicData.MyRange.MaxX;
      for (int index = 0; index < TheData.Count; ++index)
      {
        if ((double) index >= min2 && (double) index <= max2)
        {
          double num1 = this.TransValue((double) index, min2, max2, minX, maxX);
          double num2 = this.CanvasDrawing.ActualHeight - this.TransValue(TheData[index], min1, max1, minY, maxY);
          Ellipse ellipse = new Ellipse();
          double markerSize = this.GetMarkerSize(TheGraphicData.MyPointSizeDivider);
          ellipse.Width = markerSize;
          ellipse.Height = markerSize;
          if (FillTheMarker)
            ellipse.Fill = (Brush) new SolidColorBrush(TheColor);
          ellipse.Stroke = (Brush) new SolidColorBrush(TheColor);
          ellipse.Margin = new Thickness(num1 - ellipse.Width / 2.0, num2 - ellipse.Height / 2.0, 0.0, 0.0);
          markers.Add(ellipse);
        }
      }
      return markers;
    }

    private List<Ellipse> GetMarkers(
      List<GmmGraphicControl.XYData> TheData,
      Color TheColor,
      bool FillTheMarker,
      GmmGraphicControl.GraphicData TheGraphicData)
    {
      List<Ellipse> markers = new List<Ellipse>();
      double min1 = TheGraphicData.MyYLimits.Min;
      double max1 = TheGraphicData.MyYLimits.Max;
      double min2 = TheGraphicData.MyXLimits.Min;
      double max2 = TheGraphicData.MyXLimits.Max;
      double minY = TheGraphicData.MyRange.MinY;
      double maxY = TheGraphicData.MyRange.MaxY;
      double minX = TheGraphicData.MyRange.MinX;
      double maxX = TheGraphicData.MyRange.MaxX;
      for (int index = 0; index < TheData.Count; ++index)
      {
        if (TheData[index].X >= min2 && TheData[index].Y <= max2)
        {
          double num1 = this.TransValue(TheData[index].X, min2, max2, minX, maxX);
          double num2 = this.CanvasDrawing.ActualHeight - this.TransValue(TheData[index].Y, min1, max1, minY, maxY);
          Ellipse ellipse = new Ellipse();
          double markerSize = this.GetMarkerSize(TheGraphicData.MyPointSizeDivider);
          ellipse.Width = markerSize;
          ellipse.Height = markerSize;
          if (FillTheMarker)
            ellipse.Fill = (Brush) new SolidColorBrush(TheColor);
          ellipse.Stroke = (Brush) new SolidColorBrush(TheColor);
          ellipse.Margin = new Thickness(num1 - ellipse.Width / 2.0, num2 - ellipse.Height / 2.0, 0.0, 0.0);
          markers.Add(ellipse);
        }
      }
      return markers;
    }

    private double GetMarkerSize(double TheDivider)
    {
      return this.CanvasDrawing.ActualWidth * this.CanvasDrawing.ActualHeight / (this.CanvasDrawing.ActualWidth + this.CanvasDrawing.ActualHeight) / TheDivider;
    }

    private double GetFontSize(double TheDivider)
    {
      return this.CanvasDrawing.ActualWidth * this.CanvasDrawing.ActualHeight / (this.CanvasDrawing.ActualWidth + this.CanvasDrawing.ActualHeight) / TheDivider;
    }

    private double TransValue(
      double TheValue,
      double MinValue,
      double MaxValue,
      double TransMin,
      double TransMax)
    {
      return TransMin + (TheValue - MinValue) / (MaxValue - MinValue) * (TransMax - TransMin);
    }

    private GmmGraphicControl.LimitClass GetYLimits(GmmGraphicControl.GraphicData TheGraphicData)
    {
      double TheMinValue = double.MaxValue;
      double TheMaxValue = double.MinValue;
      if (TheGraphicData.MyGraphicType == GmmGraphicControl.GraphicType.Line)
      {
        if (TheGraphicData.MyLines.Count > 0)
        {
          foreach (GmmGraphicControl.LineClass lineClass in (IEnumerable<GmmGraphicControl.LineClass>) TheGraphicData.MyLines.Values)
          {
            for (int index = 0; index < lineClass.MyValues.Count; ++index)
            {
              if (lineClass.MyValues[index] < TheMinValue)
                TheMinValue = lineClass.MyValues[index];
              if (lineClass.MyValues[index] > TheMaxValue)
                TheMaxValue = lineClass.MyValues[index];
            }
          }
          return new GmmGraphicControl.LimitClass(TheMinValue, TheMaxValue);
        }
      }
      else if (TheGraphicData.MyXYLines.Count > 0)
      {
        foreach (GmmGraphicControl.XYLineClass xyLineClass in (IEnumerable<GmmGraphicControl.XYLineClass>) TheGraphicData.MyXYLines.Values)
        {
          for (int index = 0; index < xyLineClass.MyValues.Count; ++index)
          {
            if (xyLineClass.MyValues[index].Y > TheMaxValue)
              TheMaxValue = xyLineClass.MyValues[index].Y;
            if (xyLineClass.MyValues[index].Y < TheMinValue)
              TheMinValue = xyLineClass.MyValues[index].Y;
          }
        }
        return new GmmGraphicControl.LimitClass(TheMinValue, TheMaxValue);
      }
      return (GmmGraphicControl.LimitClass) null;
    }

    private GmmGraphicControl.LimitClass GetXLimits(GmmGraphicControl.GraphicData TheGraphicData)
    {
      if (TheGraphicData.MyGraphicType == GmmGraphicControl.GraphicType.Line)
        return new GmmGraphicControl.LimitClass(0.0, (double) (TheGraphicData.MyCountOfValues - 1));
      if (TheGraphicData.MyXYLines.Count <= 0)
        return (GmmGraphicControl.LimitClass) null;
      double TheMinValue = double.MaxValue;
      double TheMaxValue = double.MinValue;
      foreach (GmmGraphicControl.XYLineClass xyLineClass in (IEnumerable<GmmGraphicControl.XYLineClass>) TheGraphicData.MyXYLines.Values)
      {
        for (int index = 0; index < xyLineClass.MyValues.Count; ++index)
        {
          if (xyLineClass.MyValues[index].X > TheMaxValue)
            TheMaxValue = xyLineClass.MyValues[index].X;
          if (xyLineClass.MyValues[index].X < TheMinValue)
            TheMinValue = xyLineClass.MyValues[index].X;
        }
      }
      return new GmmGraphicControl.LimitClass(TheMinValue, TheMaxValue);
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.MyGraphicData == null)
        return;
      this.ShowGraphic(this.MyGraphicData);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/gmmgraphiccontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).SizeChanged += new SizeChangedEventHandler(this.UserControl_SizeChanged);
          break;
        case 2:
          this.CanvasDrawing = (Canvas) target;
          this.CanvasDrawing.MouseLeftButtonDown += new MouseButtonEventHandler(this.CanvasDrawing_MouseLeftButtonDown);
          this.CanvasDrawing.MouseRightButtonDown += new MouseButtonEventHandler(this.CanvasDrawing_MouseRightButtonDown);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public enum GraphicType
    {
      Line,
      XY,
    }

    public class GraphicData
    {
      internal Color MyBackGroundColor;
      internal Color MyLabelColor;
      internal Color MyGridColor;
      internal Color MyAxisColor;
      internal SortedList<int, GmmGraphicControl.LineClass> MyLines;
      internal SortedList<int, GmmGraphicControl.XYLineClass> MyXYLines;
      internal GmmGraphicControl.GridClass MyGrid;
      internal GmmGraphicControl.LimitClass MyYLimits;
      internal GmmGraphicControl.LimitClass MyXLimits;
      internal int MyCountOfValues;
      internal GmmGraphicControl.GraphicRange MyRange;
      internal GmmGraphicControl.Yaxis MyYAxis;
      internal string MyYAxisCaption;
      internal GmmGraphicControl.Xaxis MyXAxis;
      internal string MyXAxisCaption;
      internal double MyPointSizeDivider;
      internal GmmGraphicControl.GraphicType MyGraphicType;
      internal bool ShowGrid;

      public GraphicData() => this.Initialize(GmmGraphicControl.GraphicType.Line);

      public GraphicData(GmmGraphicControl.GraphicType TheGraphicType)
      {
        this.Initialize(TheGraphicType);
      }

      private void Initialize(GmmGraphicControl.GraphicType TheGraphicType)
      {
        this.MyGraphicType = TheGraphicType;
        this.MyBackGroundColor = Colors.Black;
        this.MyLabelColor = Colors.White;
        this.MyGridColor = Colors.White;
        this.MyAxisColor = Colors.White;
        this.MyLines = new SortedList<int, GmmGraphicControl.LineClass>();
        this.MyXYLines = new SortedList<int, GmmGraphicControl.XYLineClass>();
        this.MyGrid = (GmmGraphicControl.GridClass) null;
        this.MyRange = new GmmGraphicControl.GraphicRange();
        this.MyYAxis = (GmmGraphicControl.Yaxis) null;
        this.MyXAxisCaption = (string) null;
        this.MyXAxis = (GmmGraphicControl.Xaxis) null;
        this.MyXAxisCaption = (string) null;
        this.MyYLimits = (GmmGraphicControl.LimitClass) null;
        this.MyXLimits = (GmmGraphicControl.LimitClass) null;
        this.MyPointSizeDivider = 50.0;
        this.ShowGrid = false;
      }

      public void SetCaptuionAxisX(string TheCaption) => this.MyXAxisCaption = TheCaption;

      public void SetCaptuionAxisY(string TheCaption) => this.MyYAxisCaption = TheCaption;

      public void SetBackGroundColor(Color TheColor) => this.MyBackGroundColor = TheColor;

      public void SetLabelColor(Color TheColor) => this.MyLabelColor = TheColor;

      public void SetGridColor(Color TheColor) => this.MyGridColor = TheColor;

      public void SetAxisColor(Color TheColor) => this.MyAxisColor = TheColor;

      public void AddLine(int TheLineNumber, List<GmmGraphicControl.XYData> TheXYData)
      {
        if (this.MyGraphicType != GmmGraphicControl.GraphicType.XY)
          throw new ApplicationException("Function not valid for Line-Graphic");
        if (this.MyXYLines.Count <= 0)
          this.MyXYLines[TheLineNumber] = new GmmGraphicControl.XYLineClass(TheXYData);
        else
          this.MyXYLines[TheLineNumber] = new GmmGraphicControl.XYLineClass(TheXYData);
      }

      public void AddLine(int TheLineNumber, List<double> TheData)
      {
        if (this.MyGraphicType != 0)
          throw new ApplicationException("Function not valid for XY-Graphic");
        if (this.MyLines.Count <= 0)
        {
          this.MyCountOfValues = TheData.Count;
          this.MyLines[TheLineNumber] = new GmmGraphicControl.LineClass(TheData);
        }
        else
        {
          if (this.MyCountOfValues != TheData.Count)
            throw new ApplicationException("AddLine: Invalid array length!");
          this.MyLines[TheLineNumber] = new GmmGraphicControl.LineClass(TheData);
        }
      }

      public void AddLine(
        int TheLineNumber,
        List<double> TheData,
        Color TheLineColor,
        bool ShowMarker,
        Color TheMarkerColor,
        bool FillMarker)
      {
        if (this.MyGraphicType != 0)
          throw new ApplicationException("Function not valid for XY-Graphic");
        if (this.MyLines.Count <= 0)
        {
          this.MyCountOfValues = TheData.Count;
          this.MyLines[TheLineNumber] = new GmmGraphicControl.LineClass(TheData);
        }
        else
        {
          if (this.MyCountOfValues != TheData.Count)
            throw new ApplicationException("AddLine: Invalid array length!");
          this.MyLines[TheLineNumber] = new GmmGraphicControl.LineClass(TheData);
          this.MyLines[TheLineNumber].SetColor(TheLineColor);
          this.MyLines[TheLineNumber].SetShowMarkers(ShowMarker);
          this.MyLines[TheLineNumber].SetMarkerColor(TheMarkerColor, FillMarker);
        }
      }

      public void AddLine(
        int TheLineNumber,
        List<GmmGraphicControl.XYData> TheXYData,
        Color TheLineColor,
        bool ShowMarker,
        Color TheMarkerColor,
        bool FillMarker)
      {
        if (this.MyGraphicType != GmmGraphicControl.GraphicType.XY)
          throw new ApplicationException("Function not valid for Line-Graphic");
        this.MyXYLines[TheLineNumber] = this.MyXYLines.Count > 0 ? new GmmGraphicControl.XYLineClass(TheXYData) : new GmmGraphicControl.XYLineClass(TheXYData);
        this.MyXYLines[TheLineNumber].SetColor(TheLineColor);
        this.MyXYLines[TheLineNumber].SetShowMarkers(ShowMarker);
        this.MyXYLines[TheLineNumber].SetMarkerColor(TheMarkerColor, FillMarker);
      }

      public void SetXLimits(double MinX, double MaxX)
      {
        this.MyXLimits = new GmmGraphicControl.LimitClass(MinX, MaxX);
      }

      public void SetYLimits(double MinY, double MaxY)
      {
        this.MyYLimits = new GmmGraphicControl.LimitClass(MinY, MaxY);
      }

      public void SetColor(int TheLine, Color TheColor)
      {
        if (this.MyGraphicType == GmmGraphicControl.GraphicType.Line)
        {
          if (!this.MyLines.Keys.Contains(TheLine))
            throw new ApplicationException("Line " + TheLine.ToString() + " not available");
          this.MyLines[TheLine].SetColor(TheColor);
        }
        else
        {
          if (!this.MyXYLines.Keys.Contains(TheLine))
            throw new ApplicationException("Line " + TheLine.ToString() + " not available");
          this.MyXYLines[TheLine].SetColor(TheColor);
        }
      }

      public void SetMarkerColor(int TheLine, Color TheColor, bool FillMarker)
      {
        if (this.MyGraphicType == GmmGraphicControl.GraphicType.Line)
        {
          if (!this.MyLines.Keys.Contains(TheLine))
            throw new ApplicationException("Line " + TheLine.ToString() + " not available");
          this.MyLines[TheLine].SetMarkerColor(TheColor, FillMarker);
        }
        else
        {
          if (!this.MyXYLines.Keys.Contains(TheLine))
            throw new ApplicationException("Line " + TheLine.ToString() + " not available");
          this.MyXYLines[TheLine].SetMarkerColor(TheColor, FillMarker);
        }
      }

      public void SetShowMarker(int TheLine, bool ShowMarker)
      {
        if (this.MyGraphicType == GmmGraphicControl.GraphicType.Line)
        {
          if (!this.MyLines.Keys.Contains(TheLine))
            throw new ApplicationException("Line " + TheLine.ToString() + " not available");
          this.MyLines[TheLine].SetShowMarkers(ShowMarker);
        }
        else
        {
          if (!this.MyXYLines.Keys.Contains(TheLine))
            throw new ApplicationException("Line " + TheLine.ToString() + " not available");
          this.MyXYLines[TheLine].SetShowMarkers(ShowMarker);
        }
      }

      public void AddGrid(double StartX, double StepX, double StartY, double StepY)
      {
        this.MyGrid = new GmmGraphicControl.GridClass(StartX, StepX, StartY, StepY);
        this.ShowGrid = true;
      }

      public void AddGrid()
      {
        this.MyGrid = (GmmGraphicControl.GridClass) null;
        this.ShowGrid = true;
      }

      public void SetYAxis(
        double TheStartValue,
        double TheStep,
        string TheCaption,
        bool ShowValues)
      {
        this.MyYAxis = new GmmGraphicControl.Yaxis(TheStartValue, TheStep, TheCaption, ShowValues);
      }

      public void SetXAxis(
        double TheStartValue,
        double TheStep,
        string TheCaption,
        bool ShowValues)
      {
        this.MyXAxis = new GmmGraphicControl.Xaxis(TheStartValue, TheStep, TheCaption, ShowValues);
      }

      public void SetPointtSizeFactor(double TheValue) => this.MyPointSizeDivider = TheValue;
    }

    internal class GraphicRange
    {
      internal double MinX;
      internal double MaxX;
      internal double MinY;
      internal double MaxY;
    }

    internal class LimitClass
    {
      internal double Min;
      internal double Max;

      internal LimitClass(double TheMinValue, double TheMaxValue)
      {
        this.Min = TheMinValue;
        this.Max = TheMaxValue;
      }
    }

    internal class LineClass
    {
      internal List<double> MyValues;
      internal Color MyColor;
      internal Color MyMarkerColor;
      internal bool ShowMarker;
      internal bool FillMarker;

      internal LineClass(List<double> TheValues)
      {
        this.MyValues = TheValues;
        this.MyColor = Colors.Yellow;
        this.MyMarkerColor = Colors.Yellow;
        this.ShowMarker = false;
        this.FillMarker = false;
      }

      internal void SetColor(Color TheColor) => this.MyColor = TheColor;

      internal void SetMarkerColor(Color TheColor, bool FillTheMarker)
      {
        this.MyMarkerColor = TheColor;
        this.FillMarker = FillTheMarker;
      }

      internal void SetShowMarkers(bool ShowTheMarker) => this.ShowMarker = ShowTheMarker;
    }

    internal class XYLineClass
    {
      internal List<GmmGraphicControl.XYData> MyValues;
      internal Color MyColor;
      internal Color MyMarkerColor;
      internal bool ShowMarker;
      internal bool FillMarker;

      internal XYLineClass(List<GmmGraphicControl.XYData> TheValues)
      {
        this.MyValues = TheValues;
        this.MyColor = Colors.Yellow;
        this.MyMarkerColor = Colors.Yellow;
        this.ShowMarker = false;
        this.FillMarker = false;
      }

      internal void SetColor(Color TheColor) => this.MyColor = TheColor;

      internal void SetMarkerColor(Color TheColor, bool FillTheMarker)
      {
        this.MyMarkerColor = TheColor;
        this.FillMarker = FillTheMarker;
      }

      internal void SetShowMarkers(bool ShowTheMarker) => this.ShowMarker = ShowTheMarker;
    }

    internal class GridClass
    {
      internal double MyStartX;
      internal double MyStepX;
      internal double MyStartY;
      internal double MyStepY;

      internal GridClass(double StartX, double StepX, double StartY, double StepY)
      {
        this.MyStartX = StartX;
        this.MyStepX = StepX;
        this.MyStartY = StartY;
        this.MyStepY = StepY;
      }
    }

    internal class Yaxis
    {
      internal double MyStarValue;
      internal double MyStep;
      internal bool ShowValues;
      internal string Caption;

      internal Yaxis(double TheStartValue, double TheStep, string TheCaption, bool ShowValues)
      {
        this.MyStarValue = TheStartValue;
        this.MyStep = TheStep;
        this.ShowValues = ShowValues;
        this.Caption = TheCaption;
      }
    }

    internal class Xaxis
    {
      internal double MyStarValue;
      internal double MyStep;
      internal bool ShowValues;
      internal string Caption;

      internal Xaxis(double TheStartValue, double TheStep, string TheCaption, bool ShowValues)
      {
        this.MyStarValue = TheStartValue;
        this.MyStep = TheStep;
        this.ShowValues = ShowValues;
        this.Caption = TheCaption;
      }
    }

    public class XYData
    {
      public double X { get; private set; }

      public double Y { get; private set; }

      public XYData(double DataX, double DataY)
      {
        this.X = DataX;
        this.Y = DataY;
      }
    }
  }
}
