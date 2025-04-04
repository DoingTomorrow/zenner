// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.ExecuteReadingOrder
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Modules.GMM;
using MSS.Client.UI.Common;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class ExecuteReadingOrder : ResizableMetroWindow, IComponentConnector
  {
    private ObservableCollection<ReadingValuesDTO> readingValuesToDelete = new ObservableCollection<ReadingValuesDTO>();
    private ObservableCollection<MeterReadingValueDTO> meterReadingValuesToDelete = new ObservableCollection<MeterReadingValueDTO>();
    private bool isNewRowInRadGridViewGenericMbusReadingValues;
    internal System.Windows.Controls.Button btnChangeProfileType;
    internal System.Windows.Controls.Button btnChangeDeviceModelParameters;
    internal System.Windows.Controls.Button btnChangeDefaultEquipment;
    internal System.Windows.Controls.Button ViewMessagesButton;
    internal RadTreeListView readingOrderStructureTree;
    internal RowDefinition readingValuesGridRow;
    internal RowDefinition genericMBusReadingValuesGridRow;
    internal RadGridView RadGridViewReadingValues;
    internal RadGridView RadGridViewGenericMbusReadingValues;
    internal System.Windows.Controls.Button ShowReadingValuesButton;
    internal System.Windows.Controls.Button ReadingNotPossibleButton;
    internal System.Windows.Controls.Button SaveButton;
    internal System.Windows.Controls.Button StartButton;
    internal System.Windows.Controls.Button StopButton;
    internal System.Windows.Controls.Button CloseButton;
    private bool _contentLoaded;

    public ExecuteReadingOrder()
    {
      this.InitializeComponent();
      this.SourceInitialized += new System.EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.RadGridViewReadingValues.Deleting += new EventHandler<GridViewDeletingEventArgs>(this.RadGridViewReadingValues_Deleting);
      this.RadGridViewReadingValues.PreparedCellForEdit += new EventHandler<GridViewPreparingCellForEditEventArgs>(this.RadGridViewReadingValuesOnPreparedCellForEdit);
      this.RadGridViewReadingValues.AddingNewDataItem += new EventHandler<GridViewAddingNewEventArgs>(this.RadGridViewReadingValues_AddingNewDataItem);
      this.readingOrderStructureTree.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(this.readingOrderStructureTree_SelectionChanged);
      this.RadGridViewGenericMbusReadingValues.Deleting += new EventHandler<GridViewDeletingEventArgs>(this.RadGridViewGenericMbusReadingValues_Deleting);
      this.RadGridViewGenericMbusReadingValues.AddingNewDataItem += new EventHandler<GridViewAddingNewEventArgs>(this.RadGridViewGenericMbusReadingValues_AddingNewDataItem);
      this.RadGridViewGenericMbusReadingValues.BeginningEdit += new EventHandler<GridViewBeginningEditRoutedEventArgs>(this.RadGridViewGenericMbusReadingValues_BeginningEdit);
      this.isNewRowInRadGridViewGenericMbusReadingValues = false;
    }

    ~ExecuteReadingOrder()
    {
      this.readingOrderStructureTree.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(this.ReadingOrderStructureTree_OnSelectionChanged);
      this.SourceInitialized -= new System.EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.RadGridViewReadingValues.Deleting -= new EventHandler<GridViewDeletingEventArgs>(this.RadGridViewReadingValues_Deleting);
      this.RadGridViewReadingValues.PreparedCellForEdit -= new EventHandler<GridViewPreparingCellForEditEventArgs>(this.RadGridViewReadingValuesOnPreparedCellForEdit);
      this.RadGridViewReadingValues.AddingNewDataItem -= new EventHandler<GridViewAddingNewEventArgs>(this.RadGridViewReadingValues_AddingNewDataItem);
      this.readingOrderStructureTree.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(this.readingOrderStructureTree_SelectionChanged);
      this.RadGridViewGenericMbusReadingValues.Deleting -= new EventHandler<GridViewDeletingEventArgs>(this.RadGridViewGenericMbusReadingValues_Deleting);
      this.RadGridViewGenericMbusReadingValues.AddingNewDataItem -= new EventHandler<GridViewAddingNewEventArgs>(this.RadGridViewGenericMbusReadingValues_AddingNewDataItem);
      this.RadGridViewGenericMbusReadingValues.BeginningEdit -= new EventHandler<GridViewBeginningEditRoutedEventArgs>(this.RadGridViewGenericMbusReadingValues_BeginningEdit);
    }

    private void readingOrderStructureTree_SelectionChanged(
      object sender,
      SelectionChangeEventArgs e)
    {
      this.readingValuesToDelete = new ObservableCollection<ReadingValuesDTO>();
      this.meterReadingValuesToDelete = new ObservableCollection<MeterReadingValueDTO>();
    }

    private void RadGridViewReadingValues_AddingNewDataItem(
      object sender,
      GridViewAddingNewEventArgs e)
    {
      RadGridView radGridView = sender as RadGridView;
      radGridView.CurrentColumn = radGridView.Columns[0];
      this.RadGridViewReadingValues.CanUserInsertRows = false;
      this.RadGridViewReadingValues.NewRowPosition = GridViewNewRowPosition.None;
    }

    private void RadGridViewGenericMbusReadingValues_AddingNewDataItem(
      object sender,
      GridViewAddingNewEventArgs e)
    {
      this.isNewRowInRadGridViewGenericMbusReadingValues = true;
      RadGridView radGridView = sender as RadGridView;
      radGridView.CurrentColumn = radGridView.Columns[0];
      this.EnableOrDisableAddingNewItem(this.RadGridViewGenericMbusReadingValues);
    }

    private void RadGridViewGenericMbusReadingValues_BeginningEdit(
      object sender,
      GridViewBeginningEditRoutedEventArgs e)
    {
      if (!this.isNewRowInRadGridViewGenericMbusReadingValues)
        return;
      if (sender is RadGridView radGridView)
        (radGridView.Items.CurrentItem as MeterReadingValueDTO).Date = DateTime.Now;
      this.isNewRowInRadGridViewGenericMbusReadingValues = false;
    }

    private void RadGridViewReadingValuesOnPreparedCellForEdit(
      object sender,
      GridViewPreparingCellForEditEventArgs e)
    {
      if (!((string) e.Column.Header == "Register") || !(e.EditingElement is RadComboBox editingElement))
        return;
      List<ValueIdent.ValueIdPart_MeterType> valueIdPartMeterTypeList = new List<ValueIdent.ValueIdPart_MeterType>();
      ReadingValuesDTO readingValuesDto1 = this.RadGridViewReadingValues.RowInEditMode.Item as ReadingValuesDTO;
      foreach (object obj in (IEnumerable) this.RadGridViewReadingValues.ItemsSource)
      {
        if (obj is ReadingValuesDTO readingValuesDto2 && readingValuesDto2 != readingValuesDto1)
          valueIdPartMeterTypeList.Add((obj as ReadingValuesDTO).Register);
      }
      List<ValueIdent.ValueIdPart_MeterType> list = ValueIdentHelper.GetMeterTypeEnumerableAsValueIdPart().ToList<ValueIdent.ValueIdPart_MeterType>();
      foreach (ValueIdent.ValueIdPart_MeterType valueIdPartMeterType in valueIdPartMeterTypeList)
        list.Remove(valueIdPartMeterType);
      editingElement.ItemsSource = (IEnumerable) list;
      editingElement.SelectedItem = (object) list[0];
    }

    private void RadGridViewReadingValues_Deleting(object sender, GridViewDeletingEventArgs e)
    {
      this.readingValuesToDelete.Add(e.Items.First<object>() as ReadingValuesDTO);
      // ISSUE: reference to a compiler-generated field
      if (ExecuteReadingOrder.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExecuteReadingOrder.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, ObservableCollection<ReadingValuesDTO>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReadingValuesToDeleteCollection", typeof (ExecuteReadingOrder), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ExecuteReadingOrder.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) ExecuteReadingOrder.\u003C\u003Eo__10.\u003C\u003Ep__0, this.DataContext, this.readingValuesToDelete);
      this.RadGridViewReadingValues.CanUserInsertRows = true;
      this.RadGridViewReadingValues.NewRowPosition = GridViewNewRowPosition.Bottom;
    }

    private void RadGridViewGenericMbusReadingValues_Deleting(
      object sender,
      GridViewDeletingEventArgs e)
    {
      this.meterReadingValuesToDelete.Add(e.Items.First<object>() as MeterReadingValueDTO);
      // ISSUE: reference to a compiler-generated field
      if (ExecuteReadingOrder.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExecuteReadingOrder.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, ObservableCollection<MeterReadingValueDTO>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "MeterReadingValuesToDeleteCollection", typeof (ExecuteReadingOrder), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ExecuteReadingOrder.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) ExecuteReadingOrder.\u003C\u003Eo__11.\u003C\u003Ep__0, this.DataContext, this.meterReadingValuesToDelete);
      this.RadGridViewReadingValues.CanUserInsertRows = true;
      this.RadGridViewReadingValues.NewRowPosition = GridViewNewRowPosition.Bottom;
    }

    private void DisplaySettingsChanged(object sender, EventArgs e)
    {
      if (Screen.PrimaryScreen.Bounds.Height <= Screen.PrimaryScreen.Bounds.Width)
        ;
    }

    private void ReadingOrderStructureTree_OnSelectionChanged(
      object sender,
      SelectionChangeEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExecuteReadingOrder.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExecuteReadingOrder.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, ObservableCollection<object>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SelectedNodes", typeof (ExecuteReadingOrder), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ExecuteReadingOrder.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) ExecuteReadingOrder.\u003C\u003Eo__13.\u003C\u003Ep__0, this.DataContext, this.readingOrderStructureTree.SelectedItems);
      this.EnableOrDisableAddingNewItem(this.RadGridViewGenericMbusReadingValues);
      this.EnableOrDisableAddingNewItem(this.RadGridViewReadingValues, true, this.RadGridViewReadingValues.ItemsSource);
    }

    private bool UserCanManualReadings(
      ExecuteOrderStructureNode selectedItem,
      bool forReadingValuesGrid = false,
      object itemsSource = null)
    {
      if (selectedItem == null || selectedItem.MeterId == Guid.Empty)
        return false;
      if (forReadingValuesGrid)
      {
        ObservableCollection<ReadingValuesDTO> observableCollection = (ObservableCollection<ReadingValuesDTO>) itemsSource;
        if (observableCollection == null || observableCollection.Count > 0)
          return false;
      }
      return true;
    }

    private void EnableOrDisableAddingNewItem(
      RadGridView gridView,
      bool forReadingValuesGrid = false,
      object itemsSource = null)
    {
      bool flag = this.UserCanManualReadings(this.readingOrderStructureTree.SelectedItem as ExecuteOrderStructureNode, forReadingValuesGrid, itemsSource);
      gridView.CanUserInsertRows = flag;
      gridView.NewRowPosition = flag ? GridViewNewRowPosition.Bottom : GridViewNewRowPosition.None;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/executereadingorder.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.btnChangeProfileType = (System.Windows.Controls.Button) target;
          break;
        case 2:
          this.btnChangeDeviceModelParameters = (System.Windows.Controls.Button) target;
          break;
        case 3:
          this.btnChangeDefaultEquipment = (System.Windows.Controls.Button) target;
          break;
        case 4:
          this.ViewMessagesButton = (System.Windows.Controls.Button) target;
          break;
        case 5:
          this.readingOrderStructureTree = (RadTreeListView) target;
          this.readingOrderStructureTree.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(this.ReadingOrderStructureTree_OnSelectionChanged);
          break;
        case 6:
          this.readingValuesGridRow = (RowDefinition) target;
          break;
        case 7:
          this.genericMBusReadingValuesGridRow = (RowDefinition) target;
          break;
        case 8:
          this.RadGridViewReadingValues = (RadGridView) target;
          break;
        case 9:
          this.RadGridViewGenericMbusReadingValues = (RadGridView) target;
          break;
        case 10:
          this.ShowReadingValuesButton = (System.Windows.Controls.Button) target;
          break;
        case 11:
          this.ReadingNotPossibleButton = (System.Windows.Controls.Button) target;
          break;
        case 12:
          this.SaveButton = (System.Windows.Controls.Button) target;
          break;
        case 13:
          this.StartButton = (System.Windows.Controls.Button) target;
          break;
        case 14:
          this.StopButton = (System.Windows.Controls.Button) target;
          break;
        case 15:
          this.CloseButton = (System.Windows.Controls.Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
