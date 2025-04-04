// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.DataGridCellHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class DataGridCellHelper
  {
    public static readonly DependencyProperty SaveDataGridProperty = DependencyProperty.RegisterAttached("SaveDataGrid", typeof (bool), typeof (DataGridCellHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridCellHelper.CellPropertyChangedCallback)));
    public static readonly DependencyProperty DataGridProperty = DependencyProperty.RegisterAttached("DataGrid", typeof (DataGrid), typeof (DataGridCellHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

    private static void CellPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dependencyObject is DataGridCell dataGridCell) || e.NewValue == e.OldValue || !(e.NewValue is bool))
        return;
      dataGridCell.Loaded -= new RoutedEventHandler(DataGridCellHelper.DataGridCellLoaded);
      dataGridCell.Unloaded -= new RoutedEventHandler(DataGridCellHelper.DataGridCellUnloaded);
      DataGrid dataGrid = (DataGrid) null;
      if ((bool) e.NewValue)
      {
        dataGrid = dataGridCell.TryFindParent<DataGrid>();
        dataGridCell.Loaded += new RoutedEventHandler(DataGridCellHelper.DataGridCellLoaded);
        dataGridCell.Unloaded += new RoutedEventHandler(DataGridCellHelper.DataGridCellUnloaded);
      }
      DataGridCellHelper.SetDataGrid((UIElement) dataGridCell, dataGrid);
    }

    private static void DataGridCellLoaded(object sender, RoutedEventArgs e)
    {
      DataGridCell dataGridCell = (DataGridCell) sender;
      if (DataGridCellHelper.GetDataGrid((UIElement) dataGridCell) != null)
        return;
      DataGrid parent = dataGridCell.TryFindParent<DataGrid>();
      DataGridCellHelper.SetDataGrid((UIElement) dataGridCell, parent);
    }

    private static void DataGridCellUnloaded(object sender, RoutedEventArgs e)
    {
      DataGridCellHelper.SetDataGrid((UIElement) sender, (DataGrid) null);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (DataGridCell))]
    public static bool GetSaveDataGrid(UIElement element)
    {
      return (bool) element.GetValue(DataGridCellHelper.SaveDataGridProperty);
    }

    public static void SetSaveDataGrid(UIElement element, bool value)
    {
      element.SetValue(DataGridCellHelper.SaveDataGridProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (DataGridCell))]
    public static DataGrid GetDataGrid(UIElement element)
    {
      return (DataGrid) element.GetValue(DataGridCellHelper.DataGridProperty);
    }

    public static void SetDataGrid(UIElement element, DataGrid value)
    {
      element.SetValue(DataGridCellHelper.DataGridProperty, (object) value);
    }
  }
}
