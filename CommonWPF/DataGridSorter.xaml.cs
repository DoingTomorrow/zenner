// Decompiled with JetBrains decompiler
// Type: CommonWPF.DataGridSorter
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace CommonWPF
{
  public partial class DataGridSorter : Window, IComponentConnector, IStyleConnector
  {
    private DataGridSorter.SortWindowInputClass MyInputClass;
    internal Grid GridMain;
    internal GmmCorporateControl gmmCorporateControl1;
    internal Grid GridTables;
    internal StackPanel StackPanelButtons;
    internal Button ButtonAdd;
    internal Button ButtonRemove;
    internal Button ButtonClear;
    internal ListBox ListBoxColumnHeaders;
    internal DataGrid DataGridSortData;
    internal Button ButtonCancel;
    internal Button ButtonAccept;
    private bool _contentLoaded;

    private ObservableCollection<DataGridSorter.SortEntry> MyObservailableSortEntryList { get; set; }

    private ObservableCollection<DataGridSorter.HeaderEntry> MyColumnHeaderList { get; set; }

    internal DataGridSorter(DataGridSorter.SortWindowInputClass TheInputClass)
    {
      this.InitializeComponent();
      this.MyInputClass = TheInputClass;
      this.MyObservailableSortEntryList = new ObservableCollection<DataGridSorter.SortEntry>();
      this.DataGridSortData.ItemsSource = (IEnumerable) this.MyObservailableSortEntryList;
      this.MyColumnHeaderList = new ObservableCollection<DataGridSorter.HeaderEntry>();
      foreach (string theColumnHeader in TheInputClass.TheColumnHeaders)
        this.MyColumnHeaderList.Add(new DataGridSorter.HeaderEntry(theColumnHeader));
      this.ListBoxColumnHeaders.ItemsSource = (IEnumerable) this.MyColumnHeaderList;
      if (this.MyColumnHeaderList.Count > 0)
        this.ListBoxColumnHeaders.SelectedIndex = 0;
      foreach (DataGridSorter.SortEntry theSortEntry in this.MyInputClass.TheSortEntryList)
      {
        this.MyColumnHeaderList.Remove(new DataGridSorter.HeaderEntry(theSortEntry.HeaderName));
        this.MyObservailableSortEntryList.Add(new DataGridSorter.SortEntry(theSortEntry.HeaderName, theSortEntry.SortDirectionIsAscending));
      }
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      this.MyInputClass.SortTheDatagrid = false;
      this.Close();
    }

    private void ButtonAccept_Click(object sender, RoutedEventArgs e)
    {
      this.MyInputClass.SortTheDatagrid = true;
      this.MyInputClass.TheSortEntryList = new List<DataGridSorter.SortEntry>();
      foreach (DataGridSorter.SortEntry observailableSortEntry in (Collection<DataGridSorter.SortEntry>) this.MyObservailableSortEntryList)
        this.MyInputClass.TheSortEntryList.Add(observailableSortEntry);
      this.Close();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
      if (this.ListBoxColumnHeaders.SelectedItem == null)
        return;
      DataGridSorter.HeaderEntry selectedItem = (DataGridSorter.HeaderEntry) this.ListBoxColumnHeaders.SelectedItem;
      this.MyColumnHeaderList.Remove(selectedItem);
      this.MyObservailableSortEntryList.Add(new DataGridSorter.SortEntry(selectedItem.HeaderName, true));
    }

    private void ButtonRemove_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridSortData.SelectedIndex < 0)
        return;
      DataGridSorter.SortEntry selectedItem = (DataGridSorter.SortEntry) this.DataGridSortData.SelectedItem;
      this.MyObservailableSortEntryList.Remove(selectedItem);
      this.MyColumnHeaderList.Add(new DataGridSorter.HeaderEntry(selectedItem.HeaderName));
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      for (int index = 0; index < this.DataGridSortData.Items.Count; ++index)
        this.MyColumnHeaderList.Add(new DataGridSorter.HeaderEntry(((DataGridSorter.SortEntry) this.DataGridSortData.Items[index]).HeaderName));
      this.MyObservailableSortEntryList.Clear();
    }

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
      ((DataGridSorter.SortEntry) ((FrameworkElement) e.Source).DataContext).SortDirectionIsAscending = true;
    }

    private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
      ((DataGridSorter.SortEntry) ((FrameworkElement) e.Source).DataContext).SortDirectionIsAscending = false;
    }

    private void ListBoxColumnHeaders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ListBoxColumnHeaders.SelectedItem == null)
        return;
      HitTestResult hitTestResult = VisualTreeHelper.HitTest((Visual) this.ListBoxColumnHeaders, Mouse.GetPosition((IInputElement) this.ListBoxColumnHeaders));
      if ((hitTestResult.VisualHit is TextBlock || hitTestResult.VisualHit is Border) && (!(hitTestResult.VisualHit is Border) || ((FrameworkElement) hitTestResult.VisualHit).TemplatedParent is ListBoxItem))
      {
        DataGridSorter.HeaderEntry selectedItem = (DataGridSorter.HeaderEntry) this.ListBoxColumnHeaders.SelectedItem;
        this.MyColumnHeaderList.Remove(selectedItem);
        this.MyObservailableSortEntryList.Add(new DataGridSorter.SortEntry(selectedItem.HeaderName, true));
      }
    }

    private void DataGridSortData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.DataGridSortData.SelectedIndex < 0)
        return;
      HitTestResult hitTestResult = VisualTreeHelper.HitTest((Visual) this.ListBoxColumnHeaders, Mouse.GetPosition((IInputElement) this.DataGridSortData));
      if (hitTestResult == null || !(hitTestResult.VisualHit is TextBlock) && !(hitTestResult.VisualHit is Border) || hitTestResult.VisualHit is Border && !(((FrameworkElement) hitTestResult.VisualHit).Parent is DataGridCell))
        return;
      DataGridSorter.SortEntry selectedItem = (DataGridSorter.SortEntry) this.DataGridSortData.SelectedItem;
      this.MyObservailableSortEntryList.Remove(selectedItem);
      this.MyColumnHeaderList.Add(new DataGridSorter.HeaderEntry(selectedItem.HeaderName));
    }

    public static void ShowDatagridSorterAndSortDataGrid(DataGrid TheDataGrid)
    {
      DataGridSorter.SortWindowInputClass TheInputClass = new DataGridSorter.SortWindowInputClass();
      for (int index = 0; index < TheDataGrid.Columns.Count; ++index)
        TheInputClass.TheColumnHeaders.Add(TheDataGrid.Columns[index].Header.ToString());
      foreach (DataGridSorter.ColumnHeaderSortParameter headerSortParameter in DataGridSorter.GetSortParametersFromDatagrid(TheDataGrid))
        TheInputClass.TheSortEntryList.Add(new DataGridSorter.SortEntry(headerSortParameter.ColumnHeader, headerSortParameter.SortDirection == ListSortDirection.Ascending));
      new DataGridSorter(TheInputClass).ShowDialog();
      List<DataGridSorter.ColumnHeaderSortParameter> TheSortParameters = new List<DataGridSorter.ColumnHeaderSortParameter>();
      if (!TheInputClass.SortTheDatagrid)
        return;
      foreach (DataGridSorter.SortEntry theSortEntry in TheInputClass.TheSortEntryList)
      {
        ListSortDirection TheSortDirection = !theSortEntry.SortDirectionIsAscending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        DataGridSorter.ColumnHeaderSortParameter headerSortParameter = new DataGridSorter.ColumnHeaderSortParameter(theSortEntry.HeaderName, TheSortDirection);
        TheSortParameters.Add(headerSortParameter);
      }
      DataGridSorter.SortDataGrid(TheDataGrid, TheSortParameters);
    }

    public static void SortDataGrid(
      DataGrid dataGrid,
      List<DataGridSorter.ColumnIndexSortParameter> TheSortParameters)
    {
      dataGrid.Items.SortDescriptions.Clear();
      foreach (DataGridSorter.ColumnIndexSortParameter theSortParameter in TheSortParameters)
      {
        if (dataGrid.Columns.Count > theSortParameter.ColumnIndex)
        {
          DataGridColumn column = dataGrid.Columns[theSortParameter.ColumnIndex];
          dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, theSortParameter.SortDirection));
        }
      }
      foreach (DataGridColumn column in (Collection<DataGridColumn>) dataGrid.Columns)
        column.SortDirection = new ListSortDirection?();
      foreach (DataGridSorter.ColumnIndexSortParameter theSortParameter in TheSortParameters)
      {
        if (dataGrid.Columns.Count > theSortParameter.ColumnIndex)
          dataGrid.Columns[theSortParameter.ColumnIndex].SortDirection = new ListSortDirection?(theSortParameter.SortDirection);
      }
      dataGrid.Items.Refresh();
    }

    public static void SortDataGrid(
      DataGrid TheDataGrid,
      List<DataGridSorter.ColumnHeaderSortParameter> TheSortParameters)
    {
      List<DataGridSorter.ColumnIndexSortParameter> TheSortParameters1 = new List<DataGridSorter.ColumnIndexSortParameter>();
      foreach (DataGridSorter.ColumnHeaderSortParameter theSortParameter in TheSortParameters)
      {
        bool flag = false;
        for (int index = 0; index < TheDataGrid.Columns.Count; ++index)
        {
          if (TheDataGrid.Columns[index].Header != null && TheDataGrid.Columns[index].Header.ToString() == theSortParameter.ColumnHeader)
          {
            flag = true;
            TheSortParameters1.Add(new DataGridSorter.ColumnIndexSortParameter(index, theSortParameter.SortDirection));
            break;
          }
        }
        if (flag)
          ;
      }
      DataGridSorter.SortDataGrid(TheDataGrid, TheSortParameters1);
    }

    public static List<DataGridSorter.ColumnHeaderSortParameter> GetSortParametersFromDatagrid(
      DataGrid TheGrid)
    {
      List<DataGridSorter.ColumnHeaderSortParameter> parametersFromDatagrid = new List<DataGridSorter.ColumnHeaderSortParameter>();
      foreach (SortDescription sortDescription in (Collection<SortDescription>) TheGrid.Items.SortDescriptions)
        parametersFromDatagrid.Add(new DataGridSorter.ColumnHeaderSortParameter(sortDescription.PropertyName, sortDescription.Direction));
      return parametersFromDatagrid;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/datagridsorter.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.GridMain = (Grid) target;
          break;
        case 2:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 3:
          this.GridTables = (Grid) target;
          break;
        case 4:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 5:
          this.ButtonAdd = (Button) target;
          this.ButtonAdd.Click += new RoutedEventHandler(this.ButtonAdd_Click);
          break;
        case 6:
          this.ButtonRemove = (Button) target;
          this.ButtonRemove.Click += new RoutedEventHandler(this.ButtonRemove_Click);
          break;
        case 7:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 8:
          this.ListBoxColumnHeaders = (ListBox) target;
          this.ListBoxColumnHeaders.MouseDoubleClick += new MouseButtonEventHandler(this.ListBoxColumnHeaders_MouseDoubleClick);
          break;
        case 9:
          this.DataGridSortData = (DataGrid) target;
          this.DataGridSortData.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridSortData_MouseDoubleClick);
          break;
        case 11:
          this.ButtonCancel = (Button) target;
          this.ButtonCancel.Click += new RoutedEventHandler(this.ButtonCancel_Click);
          break;
        case 12:
          this.ButtonAccept = (Button) target;
          this.ButtonAccept.Click += new RoutedEventHandler(this.ButtonAccept_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 10)
        return;
      ((ToggleButton) target).Checked += new RoutedEventHandler(this.ToggleButton_Checked);
      ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.ToggleButton_Unchecked);
    }

    public class ColumnIndexSortParameter
    {
      public int ColumnIndex;
      public ListSortDirection SortDirection;

      public ColumnIndexSortParameter(int TheColumnIndex, ListSortDirection TheSortDirection)
      {
        this.ColumnIndex = TheColumnIndex;
        this.SortDirection = TheSortDirection;
      }
    }

    public class ColumnHeaderSortParameter
    {
      public string ColumnHeader;
      public ListSortDirection SortDirection;

      public ColumnHeaderSortParameter(string TheColumnHeader, ListSortDirection TheSortDirection)
      {
        this.ColumnHeader = TheColumnHeader;
        this.SortDirection = TheSortDirection;
      }

      public ColumnHeaderSortParameter()
      {
        this.ColumnHeader = string.Empty;
        this.SortDirection = ListSortDirection.Ascending;
      }
    }

    public class HeaderImageConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        MemoryStream memoryStream = new MemoryStream();
        ((System.Drawing.Image) value).Save((Stream) memoryStream, ImageFormat.Bmp);
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        memoryStream.Seek(0L, SeekOrigin.Begin);
        bitmapImage.StreamSource = (Stream) memoryStream;
        bitmapImage.EndInit();
        return (object) bitmapImage;
      }

      public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    internal class SortWindowInputClass
    {
      internal List<string> TheColumnHeaders;
      internal List<DataGridSorter.SortEntry> TheSortEntryList;
      internal bool SortTheDatagrid;

      internal SortWindowInputClass()
      {
        this.TheColumnHeaders = new List<string>();
        this.TheSortEntryList = new List<DataGridSorter.SortEntry>();
        this.SortTheDatagrid = false;
      }
    }

    public class HeaderEntry : INotifyPropertyChanged
    {
      private string _headername;

      public string HeaderName
      {
        get => this._headername;
        set
        {
          this._headername = value;
          this.onPropertyChanged(nameof (HeaderName));
        }
      }

      public HeaderEntry(string TheHeaderName) => this.HeaderName = TheHeaderName;

      public event PropertyChangedEventHandler PropertyChanged;

      event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
      {
        add => this.PropertyChanged += value;
        remove => this.PropertyChanged -= value;
      }

      protected void onPropertyChanged(string PropertyName)
      {
        PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
        if (propertyChanged == null)
          return;
        propertyChanged((object) this, new PropertyChangedEventArgs(PropertyName));
      }
    }

    public class SortEntry : INotifyPropertyChanged
    {
      private string _headername;
      private bool _sortdirectionisascending;

      public string HeaderName
      {
        get => this._headername;
        set
        {
          this._headername = value;
          this.onPropertyChanged(nameof (HeaderName));
        }
      }

      public bool SortDirectionIsAscending
      {
        get => this._sortdirectionisascending;
        set
        {
          this._sortdirectionisascending = value;
          this.onPropertyChanged(nameof (SortDirectionIsAscending));
        }
      }

      public SortEntry(string TheHeaderName, bool SortAscending)
      {
        this.HeaderName = TheHeaderName;
        this.SortDirectionIsAscending = SortAscending;
      }

      public event PropertyChangedEventHandler PropertyChanged;

      event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
      {
        add => this.PropertyChanged += value;
        remove => this.PropertyChanged -= value;
      }

      protected void onPropertyChanged(string PropertyName)
      {
        PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
        if (propertyChanged == null)
          return;
        propertyChanged((object) this, new PropertyChangedEventArgs(PropertyName));
      }
    }
  }
}
