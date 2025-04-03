// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ParameterListEditor
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using CommonWPF;
using GmmDbLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class ParameterListEditor : Window, IComponentConnector
  {
    private bool FilterList;
    private bool changingFunction = false;
    internal Button ButtonOk;
    internal Button ButtonAddGroup;
    internal Button ButtonAdd;
    internal Button ButtonDelete;
    internal TextBlock TextBlockUsingInfo;
    internal DataGrid DataGridParameters;
    private bool _contentLoaded;

    internal static bool EditItemParameter(int connectionItemID)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Define profile item parameters");
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          string selectSql1 = "SELECT * FROM ConnectionItems WHERE ConnectionItemID = " + connectionItemID.ToString();
          Schema.ConnectionItemsDataTable connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection).Fill((DataTable) connectionItemsDataTable);
          if (connectionItemsDataTable.Count != 1)
            throw new Exception("ConnectionItem not found");
          string[] names = Enum.GetNames(typeof (ConnectionProfileParameter));
          int[] values = (int[]) Enum.GetValues(typeof (ConnectionProfileParameter));
          List<string> stringList = new List<string>();
          string headerText;
          if (connectionItemsDataTable[0].ItemType == "DeviceModel")
          {
            headerText = "Parameter Editor.   DeviceModel: " + connectionItemsDataTable[0].Name;
            for (int index = 0; index < names.Length; ++index)
            {
              if (values[index] < 10000 || values[index] >= 20000 && values[index] < 30000)
                stringList.Add(names[index]);
            }
          }
          else if (connectionItemsDataTable[0].ItemType == "EquipmentModel")
          {
            headerText = "Parameter Editor.   EquipmentModel: " + connectionItemsDataTable[0].Name;
            for (int index = 0; index < names.Length; ++index)
            {
              if (values[index] < 10000 || values[index] >= 30000 && values[index] < 40000)
                stringList.Add(names[index]);
            }
          }
          else
          {
            if (!(connectionItemsDataTable[0].ItemType == "ProfileType"))
              throw new Exception("Illegal item type");
            headerText = "Parameter Editor.   ProfileType: " + connectionItemsDataTable[0].Name;
            for (int index = 0; index < names.Length; ++index)
            {
              if (values[index] < 10000 || values[index] >= 40000 && values[index] < 50000)
                stringList.Add(names[index]);
            }
          }
          string selectSql2 = "SELECT * FROM ConnectionItemParameters WHERE ConnectionItemID = " + connectionItemID.ToString();
          Schema.ConnectionItemParametersDataTable parametersDataTable1 = new Schema.ConnectionItemParametersDataTable();
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection).Fill((DataTable) parametersDataTable1);
          ObservableCollection<ParameterEditData> itemParameters = new ObservableCollection<ParameterEditData>();
          foreach (Schema.ConnectionItemParametersRow itemParametersRow in (TypedTableBase<Schema.ConnectionItemParametersRow>) parametersDataTable1)
          {
            string parameterValue = (string) null;
            if (!itemParametersRow.IsParameterValueNull())
              parameterValue = itemParametersRow.ParameterValue;
            itemParameters.Add(new ParameterEditData(((ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter).ToString(), parameterValue));
          }
          ParameterListEditor parameterListEditor = new ParameterListEditor(headerText, stringBuilder.ToString(), itemParameters, stringList.ToArray(), false);
          if (parameterListEditor.ShowDialog().Value)
          {
            newConnection.Open();
            DbTransaction transaction = newConnection.BeginTransaction();
            Schema.ConnectionItemParametersDataTable parametersDataTable2 = new Schema.ConnectionItemParametersDataTable();
            DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, transaction, out DbCommandBuilder _);
            dataAdapter.Fill((DataTable) parametersDataTable2);
            foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionItemParametersRow>) parametersDataTable2)
              dataRow.Delete();
            dataAdapter.Update((DataTable) parametersDataTable2);
            int num = 0;
            foreach (ParameterEditData resultParameter in parameterListEditor.ResultParameters)
            {
              Schema.ConnectionItemParametersRow row = parametersDataTable2.NewConnectionItemParametersRow();
              row.ConnectionItemID = connectionItemID;
              row.ConnectionItemParameter = (int) Enum.Parse(typeof (ConnectionProfileParameter), resultParameter.SelectedType);
              row.ParameterOrder = num++;
              if (string.IsNullOrEmpty(resultParameter.ParameterValue))
                row.SetParameterValueNull();
              else
                row.ParameterValue = resultParameter.ParameterValue;
              parametersDataTable2.AddConnectionItemParametersRow(row);
            }
            dataAdapter.Update((DataTable) parametersDataTable2);
            transaction.Commit();
          }
          return true;
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      return false;
    }

    internal static bool EditProfileParameter(int connectionProfileID, Window owner)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Define profile parameters");
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          string selectSql1 = "SELECT * FROM ConnectionProfiles WHERE ConnectionProfileID = " + connectionProfileID.ToString();
          Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection).Fill((DataTable) profilesDataTable);
          if (profilesDataTable.Count != 1)
            throw new Exception("ConnectionProfile not found");
          string headerText = "Parameter Editor.   ConnectionProfileID: " + connectionProfileID.ToString();
          string[] names = Enum.GetNames(typeof (ConnectionProfileParameter));
          int[] values = (int[]) Enum.GetValues(typeof (ConnectionProfileParameter));
          List<string> stringList = new List<string>();
          for (int index = 0; index < names.Length; ++index)
          {
            if (values[index] < 20000)
              stringList.Add(names[index]);
          }
          string selectSql2 = "SELECT * FROM ConnectionProfileParameters WHERE ConnectionProfileID = " + connectionProfileID.ToString();
          Schema.ConnectionProfileParametersDataTable parametersDataTable1 = new Schema.ConnectionProfileParametersDataTable();
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection).Fill((DataTable) parametersDataTable1);
          ObservableCollection<ParameterEditData> itemParameters = new ObservableCollection<ParameterEditData>();
          foreach (Schema.ConnectionProfileParametersRow profileParametersRow in (TypedTableBase<Schema.ConnectionProfileParametersRow>) parametersDataTable1)
          {
            string parameterValue = (string) null;
            if (!profileParametersRow.IsParameterValueNull())
              parameterValue = profileParametersRow.ParameterValue;
            itemParameters.Add(new ParameterEditData(((ConnectionProfileParameter) profileParametersRow.ConnectionProfileParameter).ToString(), parameterValue));
          }
          ParameterListEditor parameterListEditor = new ParameterListEditor(headerText, stringBuilder.ToString(), itemParameters, stringList.ToArray(), false);
          parameterListEditor.Owner = owner;
          if (parameterListEditor.ShowDialog().Value)
          {
            newConnection.Open();
            DbTransaction transaction = newConnection.BeginTransaction();
            Schema.ConnectionProfileParametersDataTable parametersDataTable2 = new Schema.ConnectionProfileParametersDataTable();
            DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, transaction, out DbCommandBuilder _);
            dataAdapter.Fill((DataTable) parametersDataTable2);
            foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionProfileParametersRow>) parametersDataTable2)
              dataRow.Delete();
            dataAdapter.Update((DataTable) parametersDataTable2);
            int num = 0;
            foreach (ParameterEditData resultParameter in parameterListEditor.ResultParameters)
            {
              Schema.ConnectionProfileParametersRow row = parametersDataTable2.NewConnectionProfileParametersRow();
              row.ConnectionProfileID = connectionProfileID;
              row.ConnectionProfileParameter = (int) Enum.Parse(typeof (ConnectionProfileParameter), resultParameter.SelectedType);
              row.ParameterOrder = num++;
              if (string.IsNullOrEmpty(resultParameter.ParameterValue))
                row.SetParameterValueNull();
              else
                row.ParameterValue = resultParameter.ParameterValue;
              parametersDataTable2.AddConnectionProfileParametersRow(row);
            }
            dataAdapter.Update((DataTable) parametersDataTable2);
            transaction.Commit();
          }
          return true;
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      return false;
    }

    internal static bool EditFilter(string name, string baseName = null)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Define profile filter");
        stringBuilder.AppendLine("Short description:");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Define first base groups.");
        stringBuilder.AppendLine("A group is a set of following records that has the same 'Filter group' number");
        stringBuilder.AppendLine("Base groups are groups that are not used in any 'Parameter type' = '__Filter group' record.");
        stringBuilder.AppendLine("If all base groups deliver true as result then the profile is used.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Most filters don't need sub groups.");
        stringBuilder.AppendLine("Sub groups are used for complex logical operations.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Following of base groups define sub groups if necessary.");
        stringBuilder.AppendLine("A sub group is a group whose 'Filter group' number is used in any pre defined 'Parameter type' = '__Filter group' record as 'Parameter value'.");
        stringBuilder.AppendLine("The result of a sub group is used as value insude the parent group.");
        stringBuilder.AppendLine("A parent group is a base group or a sub group that has a 'Parameter type' = '__Filter group' record");
        stringBuilder.AppendLine("As sub group can use a following sub group and so on.");
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          string[] names = Enum.GetNames(typeof (ConnectionProfileParameter));
          List<string> stringList = new List<string>();
          stringList.Add("__ Filter group");
          foreach (string str in names)
          {
            if (str != "None")
              stringList.Add(str);
          }
          stringList.Sort();
          ObservableCollection<ParameterEditData> itemParameters = new ObservableCollection<ParameterEditData>();
          int num1 = -1;
          if (baseName != null)
          {
            string selectSql = "SELECT * FROM ConnectionProfileFilters WHERE ConnectionProfileParameter = 0 AND ParameterValue = '" + baseName + "'";
            Schema.ConnectionProfileFiltersDataTable filtersDataTable = new Schema.ConnectionProfileFiltersDataTable();
            DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) filtersDataTable);
            num1 = filtersDataTable.Count != 0 ? filtersDataTable[0].ConnectionFilterID : throw new Exception("The base filter doesn't exists.");
          }
          string selectSql1 = "SELECT * FROM ConnectionProfileFilters WHERE ConnectionProfileParameter = 0 AND ParameterValue = '" + name + "'";
          Schema.ConnectionProfileFiltersDataTable filtersDataTable1 = new Schema.ConnectionProfileFiltersDataTable();
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection).Fill((DataTable) filtersDataTable1);
          if (baseName != null)
          {
            if (filtersDataTable1.Count > 0)
              throw new Exception("The new filter exists.");
          }
          else if (filtersDataTable1.Count > 0)
            num1 = filtersDataTable1[0].ConnectionFilterID;
          if (num1 >= 0)
          {
            string selectSql2 = "SELECT * FROM ConnectionProfileFilters WHERE ConnectionFilterID = " + num1.ToString();
            Schema.ConnectionProfileFiltersDataTable filtersDataTable2 = new Schema.ConnectionProfileFiltersDataTable();
            DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, out DbCommandBuilder _).Fill((DataTable) filtersDataTable2);
            foreach (Schema.ConnectionProfileFiltersRow profileFiltersRow in (TypedTableBase<Schema.ConnectionProfileFiltersRow>) filtersDataTable2)
            {
              if (profileFiltersRow.ParameterOrder != 0)
              {
                string selectedType = profileFiltersRow.ConnectionProfileParameter != 0 ? ((ConnectionProfileParameter) profileFiltersRow.ConnectionProfileParameter).ToString() : "__ Filter group";
                string parameterValue = (string) null;
                if (!profileFiltersRow.IsParameterValueNull())
                  parameterValue = profileFiltersRow.ParameterValue;
                itemParameters.Add(new ParameterEditData(selectedType, parameterValue)
                {
                  SelectedGroup = profileFiltersRow.FilterGroupNumber.ToString(),
                  SelectedGroupFunction = ((ConnectionProfileFilterGroupFunctions) profileFiltersRow.GroupFunction).ToString()
                });
              }
            }
          }
          ParameterListEditor parameterListEditor = new ParameterListEditor("Edit filter: " + name, stringBuilder.ToString(), itemParameters, stringList.ToArray(), true);
          if (parameterListEditor.ShowDialog().Value)
          {
            newConnection.Open();
            DbTransaction transaction = newConnection.BeginTransaction();
            string selectSql3 = "SELECT * FROM ConnectionProfileFilters WHERE ConnectionProfileParameter = 0 AND ParameterValue = '" + name + "'";
            Schema.ConnectionProfileFiltersDataTable filtersDataTable3 = new Schema.ConnectionProfileFiltersDataTable();
            DbCommandBuilder commandBuilder;
            DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql3, newConnection, transaction, out commandBuilder);
            dataAdapter.Fill((DataTable) filtersDataTable3);
            int num2 = -1;
            if (filtersDataTable3.Count > 0)
            {
              num2 = filtersDataTable3[0].ConnectionFilterID;
              string selectSql4 = "SELECT * FROM ConnectionProfileFilters WHERE ConnectionFilterID = " + num2.ToString();
              filtersDataTable3 = new Schema.ConnectionProfileFiltersDataTable();
              dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql4, newConnection, transaction, out commandBuilder);
              dataAdapter.Fill((DataTable) filtersDataTable3);
              foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionProfileFiltersRow>) filtersDataTable3)
                dataRow.Delete();
              dataAdapter.Update((DataTable) filtersDataTable3);
            }
            if (num2 < 0)
              num2 = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("ConnectionProfileFilters");
            int num3 = 0;
            Schema.ConnectionProfileFiltersRow row1 = filtersDataTable3.NewConnectionProfileFiltersRow();
            row1.ConnectionFilterID = num2;
            row1.FilterGroupNumber = 0;
            row1.ConnectionProfileParameter = 0;
            Schema.ConnectionProfileFiltersRow profileFiltersRow = row1;
            int num4 = num3;
            int num5 = num4 + 1;
            profileFiltersRow.ParameterOrder = num4;
            row1.GroupFunction = 0;
            row1.ParameterValue = name;
            filtersDataTable3.AddConnectionProfileFiltersRow(row1);
            foreach (ParameterEditData resultParameter in parameterListEditor.ResultParameters)
            {
              Schema.ConnectionProfileFiltersRow row2 = filtersDataTable3.NewConnectionProfileFiltersRow();
              row2.ConnectionFilterID = num2;
              row2.FilterGroupNumber = int.Parse(resultParameter.SelectedGroup);
              row2.ConnectionProfileParameter = !(resultParameter.SelectedType == "__ Filter group") ? (int) Enum.Parse(typeof (ConnectionProfileParameter), resultParameter.SelectedType) : 0;
              row2.ParameterOrder = num5++;
              row2.GroupFunction = (int) Enum.Parse(typeof (ConnectionProfileFilterGroupFunctions), resultParameter.SelectedGroupFunction);
              if (!string.IsNullOrEmpty(resultParameter.ParameterValue))
                row2.ParameterValue = resultParameter.ParameterValue;
              filtersDataTable3.AddConnectionProfileFiltersRow(row2);
            }
            dataAdapter.Update((DataTable) filtersDataTable3);
            transaction.Commit();
          }
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      return true;
    }

    public List<string> TypesList { get; set; }

    public List<string> GroupFunctionsList { get; set; }

    public ObservableCollection<ParameterEditData> ItemParameters { get; set; }

    public List<ParameterEditData> ResultParameters { get; private set; }

    private ParameterListEditor(
      string headerText,
      string usingInfo,
      ObservableCollection<ParameterEditData> itemParameters,
      string[] parameterTypes,
      bool filterList)
    {
      this.DataContext = (object) this;
      this.ItemParameters = itemParameters;
      this.FilterList = filterList;
      this.TypesList = new List<string>((IEnumerable<string>) parameterTypes);
      foreach (ParameterEditData itemParameter in (Collection<ParameterEditData>) this.ItemParameters)
        itemParameter.GroupFunctionChangedCall = new ParameterEditData.GroupFunctionChanged(this.GroupFunctionChanged);
      this.GroupFunctionsList = new List<string>((IEnumerable<string>) Enum.GetNames(typeof (ConnectionProfileFilterGroupFunctions)));
      if (filterList)
        this.WindowState = WindowState.Maximized;
      this.InitializeComponent();
      this.Title = "Parameters Editor  " + headerText;
      this.TextBlockUsingInfo.Text = usingInfo;
      if (!filterList)
        return;
      this.DataGridParameters.Columns[0].Visibility = Visibility.Visible;
      this.DataGridParameters.Columns[1].Visibility = Visibility.Visible;
      this.ButtonAddGroup.Visibility = Visibility.Visible;
    }

    internal void GroupFunctionChanged(ParameterEditData changedParam)
    {
      if (this.changingFunction)
        return;
      this.changingFunction = true;
      foreach (ParameterEditData itemParameter in (Collection<ParameterEditData>) this.ItemParameters)
      {
        if (itemParameter != changedParam && !(itemParameter.SelectedGroup != changedParam.SelectedGroup))
          itemParameter.SelectedGroupFunction = changedParam.SelectedGroupFunction;
      }
      this.changingFunction = false;
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e)
    {
      this.ResultParameters = this.ItemParameters.OrderBy<ParameterEditData, ParameterEditData>((System.Func<ParameterEditData, ParameterEditData>) (x => x), (IComparer<ParameterEditData>) new ParameterEditDataComp()).ToList<ParameterEditData>();
      int num1 = 0;
      string str = (string) null;
      int index = 0;
      while (index < this.ResultParameters.Count)
      {
        if (index == 0)
        {
          str = this.ResultParameters[index].SelectedGroup;
          this.ResultParameters[index].SelectedGroup = num1.ToString();
        }
        else if (this.ResultParameters[index].SelectedGroup == str)
        {
          this.ResultParameters[index].SelectedGroup = num1.ToString();
        }
        else
        {
          str = this.ResultParameters[index].SelectedGroup;
          ++num1;
          this.ResultParameters[index].SelectedGroup = num1.ToString();
        }
        if (this.ResultParameters[index].SelectedType == null || this.ResultParameters[index].SelectedType == ConnectionProfileParameter.None.ToString())
        {
          this.ResultParameters.RemoveAt(index);
        }
        else
        {
          if (index < this.ResultParameters.Count - 1 && this.ResultParameters[index].SelectedGroup == this.ResultParameters[index + 1].SelectedGroup && this.ResultParameters[index].SelectedType == this.ResultParameters[index + 1].SelectedType)
          {
            if (this.FilterList)
            {
              if (this.ResultParameters[index].ParameterValue == this.ResultParameters[index + 1].ParameterValue)
              {
                int num2 = (int) MessageBox.Show("Parameter type and value" + this.ResultParameters[index].SelectedType + " is multiple defined!!");
                return;
              }
            }
            else
            {
              int num3 = (int) MessageBox.Show("Parameter type " + this.ResultParameters[index].SelectedType + " is multiple defined!!");
              return;
            }
          }
          ++index;
        }
      }
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
      ParameterEditData parameterEditData1 = new ParameterEditData(ConnectionProfileParameter.None.ToString(), (string) null);
      parameterEditData1.GroupFunctionChangedCall = new ParameterEditData.GroupFunctionChanged(this.GroupFunctionChanged);
      if (this.DataGridParameters.Items.Count > 0)
      {
        if (this.DataGridParameters.SelectedCells != null && this.DataGridParameters.SelectedCells.Count > 0)
        {
          ParameterEditData parameterEditData2 = (ParameterEditData) this.DataGridParameters.SelectedCells[0].Item;
          parameterEditData1.SelectedGroup = parameterEditData2.SelectedGroup;
          parameterEditData1.SelectedGroupFunction = parameterEditData2.SelectedGroupFunction;
          this.ItemParameters.Insert(this.ItemParameters.IndexOf(parameterEditData2) + 1, parameterEditData1);
        }
        else
        {
          ParameterEditData parameterEditData3 = (ParameterEditData) this.DataGridParameters.Items[this.DataGridParameters.Items.Count - 1];
          parameterEditData1.SelectedGroup = parameterEditData3.SelectedGroup;
          parameterEditData1.SelectedGroupFunction = parameterEditData3.SelectedGroupFunction;
          this.ItemParameters.Add(parameterEditData1);
        }
      }
      else
        this.ItemParameters.Add(parameterEditData1);
    }

    private void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
    {
      ParameterEditData parameterEditData1 = new ParameterEditData(ConnectionProfileParameter.None.ToString(), (string) null);
      parameterEditData1.GroupFunctionChangedCall = new ParameterEditData.GroupFunctionChanged(this.GroupFunctionChanged);
      if (this.DataGridParameters.Items.Count > 0)
      {
        ParameterEditData parameterEditData2 = (ParameterEditData) this.DataGridParameters.Items[this.DataGridParameters.Items.Count - 1];
        parameterEditData1.SelectedGroup = (int.Parse(parameterEditData2.SelectedGroup) + 1).ToString();
        parameterEditData1.SelectedGroupFunction = ConnectionProfileFilterGroupFunctions.OR.ToString();
        this.ItemParameters.Add(parameterEditData1);
      }
      else
        this.ItemParameters.Add(parameterEditData1);
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridParameters.SelectedItem != null)
      {
        this.ItemParameters.Remove((ParameterEditData) this.DataGridParameters.SelectedItem);
      }
      else
      {
        DataGridCellInfo selectedCell;
        int num;
        if (this.DataGridParameters.SelectedCells != null && this.DataGridParameters.SelectedCells.Count > 0)
        {
          selectedCell = this.DataGridParameters.SelectedCells[0];
          num = selectedCell.Item is ParameterEditData ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
          return;
        selectedCell = this.DataGridParameters.SelectedCells[0];
        this.ItemParameters.Remove((ParameterEditData) selectedCell.Item);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/parameterlisteditor.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonOk = (Button) target;
          this.ButtonOk.Click += new RoutedEventHandler(this.ButtonOk_Click);
          break;
        case 2:
          this.ButtonAddGroup = (Button) target;
          this.ButtonAddGroup.Click += new RoutedEventHandler(this.ButtonAddGroup_Click);
          break;
        case 3:
          this.ButtonAdd = (Button) target;
          this.ButtonAdd.Click += new RoutedEventHandler(this.ButtonAdd_Click);
          break;
        case 4:
          this.ButtonDelete = (Button) target;
          this.ButtonDelete.Click += new RoutedEventHandler(this.ButtonDelete_Click);
          break;
        case 5:
          this.TextBlockUsingInfo = (TextBlock) target;
          break;
        case 6:
          this.DataGridParameters = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
