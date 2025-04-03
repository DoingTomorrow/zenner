// Decompiled with JetBrains decompiler
// Type: HandlerLib.DebugQueueWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace HandlerLib
{
  public class DebugQueueWindow : Window, IComponentConnector
  {
    private DebugQueueWindow.ReadDebugQueueDataFunction ReadDebugQueueFunction;
    private DebugQueueData QueuedData;
    private SortedList<int, string> QueueEvents;
    private const int NumberOfGraphicRows = 5;
    private const double GraphicRowMargin = 2.0;
    private List<RowScaling> GraphicRows;
    internal DockPanel DockPanelButtons;
    internal StackPanel StackPanelBottomButtoms;
    internal Button ButtomReadLogger;
    internal StackPanel StackPanelTopButtoms;
    internal DockPanel DockPanelFunctions;
    internal StackPanel StackPanelFunctions;
    internal Grid GridGraphics;
    private bool _contentLoaded;

    public DebugQueueWindow(
      SortedList<int, string> eventList,
      DebugQueueWindow.ReadDebugQueueDataFunction readDebugQueueFunction)
    {
      this.QueueEvents = eventList;
      this.ReadDebugQueueFunction = readDebugQueueFunction;
      this.InitializeComponent();
    }

    private void ButtomReadLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.QueuedData = this.ReadDebugQueueFunction();
        this.ResetGraphics();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ResetGraphics()
    {
      this.GridGraphics.Children.Clear();
      double num1 = this.GridGraphics.ActualWidth - 4.0;
      double num2 = (this.GridGraphics.ActualHeight - 12.0) / 5.0;
      this.GraphicRows = new List<RowScaling>();
      for (int index = 0; index < 5; ++index)
      {
        RowScaling rowScaling = new RowScaling()
        {
          Xmin = 2.0,
          Xmax = 2.0 + num1,
          Ymin = 2.0 + (double) index * (2.0 + num2)
        };
        rowScaling.Ymax = rowScaling.Ymin + num2;
        this.GraphicRows.Add(rowScaling);
      }
      this.DrawRowBaseLine(0, (Brush) Brushes.Red);
      this.DrawDashToRow(0, 0.0, (Brush) Brushes.Black);
      this.DrawDashToRow(0, 100.0, (Brush) Brushes.Black);
      this.DrawRowBaseLine(4, (Brush) Brushes.Red);
      this.DrawDashToRow(4, 0.0, (Brush) Brushes.Black);
      this.DrawDashToRow(4, 100.0, (Brush) Brushes.Black);
    }

    private void DrawRowBaseLine(int row, Brush brush)
    {
      if ((long) (uint) row >= (long) this.GraphicRows.Count)
        throw new Exception("Illegal row");
      Line element = new Line();
      element.X1 = this.GraphicRows[row].Xmin;
      element.Y1 = this.GraphicRows[row].Ymax;
      element.X2 = this.GraphicRows[row].Xmax;
      element.Y2 = this.GraphicRows[row].Ymax;
      element.Stroke = brush;
      element.StrokeThickness = 1.0;
      this.GridGraphics.Children.Add((UIElement) element);
    }

    private void DrawDashToRow(int row, double percent, Brush brush)
    {
      if ((long) (uint) row >= (long) this.GraphicRows.Count)
        throw new Exception("Illegal row");
      double num = this.GraphicRows[row].Xmin + (this.GraphicRows[row].Xmax - this.GraphicRows[row].Xmin) / 100.0 * percent;
      Line element = new Line();
      element.X1 = num;
      element.Y1 = this.GraphicRows[row].Ymin;
      element.X2 = num;
      element.Y2 = this.GraphicRows[row].Ymax;
      element.Stroke = brush;
      element.StrokeThickness = 1.0;
      this.GridGraphics.Children.Add((UIElement) element);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/debugqueuewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.DockPanelButtons = (DockPanel) target;
          break;
        case 2:
          this.StackPanelBottomButtoms = (StackPanel) target;
          break;
        case 3:
          this.ButtomReadLogger = (Button) target;
          this.ButtomReadLogger.Click += new RoutedEventHandler(this.ButtomReadLogger_Click);
          break;
        case 4:
          this.StackPanelTopButtoms = (StackPanel) target;
          break;
        case 5:
          this.DockPanelFunctions = (DockPanel) target;
          break;
        case 6:
          this.StackPanelFunctions = (StackPanel) target;
          break;
        case 7:
          this.GridGraphics = (Grid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public delegate DebugQueueData ReadDebugQueueDataFunction();
  }
}
