// Decompiled with JetBrains decompiler
// Type: HandlerLib.CompatibleFirmwareSelectWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandlerLib
{
  internal class CompatibleFirmwareSelectWindow : Window
  {
    private Window ParentWND = (Window) null;
    private Canvas canvas = new Canvas();
    private DataGrid myDataGrid = new DataGrid();

    public CompatibleFirmwareSelectWindow(Window parent, ref DataTable dependencyFW)
    {
      this.ParentWND = parent;
      this.AllowsTransparency = false;
      this.WindowStyle = WindowStyle.ThreeDBorderWindow;
      this.Background = (Brush) Brushes.CornflowerBlue;
      this.Topmost = true;
      this.Title = "Choose a compatible firmware ...";
      this.Width = this.ParentWND.Width - 200.0;
      this.Height = this.ParentWND.Height - 50.0;
      this.canvas.Width = this.Width;
      this.canvas.Height = this.Height;
      this.canvas.Background = this.ParentWND.Background;
      this.myDataGrid.Width = this.canvas.Width - 20.0;
      this.myDataGrid.Height = this.canvas.Height - 50.0;
      this.myDataGrid.Visibility = Visibility.Visible;
      this.myDataGrid.ItemsSource = (IEnumerable) dependencyFW.AsDataView();
      this.myDataGrid.AutoGenerateColumns = true;
      this.canvas.Children.Add((UIElement) this.myDataGrid);
      this.Content = (object) this.canvas;
    }
  }
}
