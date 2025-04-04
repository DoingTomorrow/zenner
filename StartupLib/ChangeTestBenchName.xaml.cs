// Decompiled with JetBrains decompiler
// Type: StartupLib.ChangeTestBenchName
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace StartupLib
{
  public partial class ChangeTestBenchName : Window, IComponentConnector
  {
    private string changedName = (string) null;
    private string currentName = (string) null;
    private string defaultTestBenchName = (string) null;
    private string PcName;
    internal Label LableCurrentName;
    internal TextBox TextBoxCurrentName;
    internal Label LabelTextBoxFilter;
    internal TextBox TextBoxFilter;
    internal Button ButtonRemoveTestBenchName;
    internal Button ButtonChangeToSelection;
    internal DataGrid DataGridTestBenches;
    private bool _contentLoaded;

    public static string GetNewTestBenchName(string currentName, string pcName)
    {
      ChangeTestBenchName changeTestBenchName = new ChangeTestBenchName(currentName, pcName);
      changeTestBenchName.ShowDialog();
      return changeTestBenchName.changedName;
    }

    public static string GetNewTestBenchName(
      string currentName,
      string[] requiredTypes,
      string defaultTestBenchName)
    {
      ChangeTestBenchName changeTestBenchName = new ChangeTestBenchName(currentName, requiredTypes, defaultTestBenchName);
      changeTestBenchName.ShowDialog();
      return changeTestBenchName.changedName;
    }

    public static bool SetTestBenchName(
      string newTestBenchName,
      string PcName,
      string InstallationPath)
    {
      if (string.IsNullOrEmpty(PcName))
        throw new Exception("PcName not defined");
      if (string.IsNullOrEmpty(InstallationPath))
        throw new Exception("InstallationPath not defined");
      if (!string.IsNullOrEmpty(newTestBenchName) && !ChangeTestBenchName.DeleteTestBenchNameFromAllInstallations(newTestBenchName))
        return false;
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        try
        {
          string selectSql1 = "SELECT * FROM Installations WHERE PcName ='" + PcName + "' AND InstallationPath ='" + InstallationPath + "'";
          DbDataAdapter dataAdapter1 = baseDbConnection.GetDataAdapter(selectSql1, newConnection, (DbTransaction) null, out DbCommandBuilder _);
          InstallationData.InstallationsDataTable installationsDataTable = new InstallationData.InstallationsDataTable();
          dataAdapter1.Fill((DataTable) installationsDataTable);
          if (installationsDataTable.Count != 1)
            throw new Exception("Installation not available");
          if (string.IsNullOrEmpty(newTestBenchName))
          {
            installationsDataTable[0].SetInstallataionNameNull();
          }
          else
          {
            string selectSql2 = "SELECT * FROM EQ_Equipment WHERE EquipmentID = EquipmentGroupID AND AccessName = '" + newTestBenchName + "'";
            DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection);
            InstallationData.EQ_EquipmentDataTable equipmentDataTable = new InstallationData.EQ_EquipmentDataTable();
            dataAdapter2.Fill((DataTable) equipmentDataTable);
            if (equipmentDataTable.Count != 1 || !equipmentDataTable[0].IsOutOfOperationDateNull())
              throw new Exception("Illegal equipment for ChangeTestBenchName");
            installationsDataTable[0].InstallataionName = newTestBenchName;
          }
          dataAdapter1.Update((DataTable) installationsDataTable);
        }
        catch (Exception ex)
        {
          throw new Exception("ChangeTestBenchName error", ex);
        }
      }
      return true;
    }

    internal static bool DeleteTestBenchNameFromAllInstallations(string testBenchName, bool ask = true)
    {
      if (string.IsNullOrEmpty(testBenchName))
        return true;
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM Installations WHERE InstallataionName ='" + testBenchName + "'", newConnection, (DbTransaction) null, out DbCommandBuilder _);
        InstallationData.InstallationsDataTable installationsDataTable = new InstallationData.InstallationsDataTable();
        dataAdapter.Fill((DataTable) installationsDataTable);
        if (installationsDataTable.Count > 0)
        {
          if (ask)
          {
            if (MessageBox.Show("This test bench equipment is assigned to other installation !!!" + Environment.NewLine + "(" + installationsDataTable[0].PcName + "; " + installationsDataTable[0].InstallationPath + ")" + Environment.NewLine + Environment.NewLine + "Would you like to move this test bench equipment to current installation?" + Environment.NewLine + "The other test bench will be disconnected from this equipment !!!" + Environment.NewLine + "Without connection to equipment test bench can not run !!!", "Move equipment", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
              return false;
          }
          foreach (InstallationData.InstallationsRow installationsRow in (TypedTableBase<InstallationData.InstallationsRow>) installationsDataTable)
            installationsRow.SetInstallataionNameNull();
          dataAdapter.Update((DataTable) installationsDataTable);
        }
      }
      return true;
    }

    private ChangeTestBenchName(string currentName, string pcName)
    {
      this.PcName = pcName;
      this.currentName = currentName != null ? currentName : "";
      this.InitializeComponent();
      this.LoadTestBenches();
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      using (DbConnection newConnection = baseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM EQ_Equipment");
        stringBuilder.Append(" WHERE EquipmentID = EquipmentGroupID");
        if (this.TextBoxFilter.Text.Length > 0)
          stringBuilder.Append(" AND AccessName LIKE '%" + this.TextBoxFilter.Text + "%'");
        stringBuilder.Append(" ORDER BY AccessName");
        DbDataAdapter dataAdapter = baseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        InstallationData.EQ_EquipmentDataTable equipmentDataTable = new InstallationData.EQ_EquipmentDataTable();
        dataAdapter.Fill((DataTable) equipmentDataTable);
        string selectSql = "SELECT *  FROM EQ_EquipmentValue WHERE ValueType = 'RestrictedToPcName' AND ToDate IS NULL";
        InstallationData.EQ_EquipmentValueDataTable equipmentValueDataTable = new InstallationData.EQ_EquipmentValueDataTable();
        baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) equipmentValueDataTable);
        foreach (InstallationData.EQ_EquipmentValueRow equipmentValueRow in (TypedTableBase<InstallationData.EQ_EquipmentValueRow>) equipmentValueDataTable)
        {
          if (!equipmentValueRow.IsValueNull() && equipmentValueRow.Value.Length >= 3 && !equipmentValueRow.Value.Contains(pcName))
          {
            DataRow[] dataRowArray = equipmentDataTable.Select("EquipmentID = " + equipmentValueRow.EquipmentID.ToString());
            if (dataRowArray.Length == 1)
              dataRowArray[0].Delete();
          }
        }
        this.DataGridTestBenches.ItemsSource = (IEnumerable) equipmentDataTable;
      }
    }

    private void LoadTestBenches()
    {
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      using (DbConnection newConnection = baseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM EQ_Equipment");
        stringBuilder.Append(" WHERE EquipmentID = EquipmentGroupID");
        if (this.TextBoxFilter.Text.Length > 0)
          stringBuilder.Append(" AND AccessName LIKE '%" + this.TextBoxFilter.Text + "%'");
        stringBuilder.Append(" ORDER BY AccessName");
        DbDataAdapter dataAdapter = baseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        InstallationData.EQ_EquipmentDataTable equipmentDataTable = new InstallationData.EQ_EquipmentDataTable();
        dataAdapter.Fill((DataTable) equipmentDataTable);
        string selectSql = "SELECT *  FROM EQ_EquipmentValue WHERE ValueType = 'RestrictedToPcName' AND ToDate IS NULL";
        InstallationData.EQ_EquipmentValueDataTable equipmentValueDataTable = new InstallationData.EQ_EquipmentValueDataTable();
        baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) equipmentValueDataTable);
        foreach (InstallationData.EQ_EquipmentValueRow equipmentValueRow in (TypedTableBase<InstallationData.EQ_EquipmentValueRow>) equipmentValueDataTable)
        {
          if (!equipmentValueRow.IsValueNull() && equipmentValueRow.Value.Length >= 3 && !equipmentValueRow.Value.Contains(this.PcName))
          {
            DataRow[] dataRowArray = equipmentDataTable.Select("EquipmentID = " + equipmentValueRow.EquipmentID.ToString());
            if (dataRowArray.Length == 1)
              dataRowArray[0].Delete();
          }
        }
        this.DataGridTestBenches.ItemsSource = (IEnumerable) equipmentDataTable;
      }
    }

    private ChangeTestBenchName(
      string currentName,
      string[] requiredTypes,
      string defaultTestBenchName)
    {
      this.currentName = currentName != null ? currentName : "";
      if (requiredTypes.Length == 0)
        throw new Exception("Required equipment types not defined");
      this.defaultTestBenchName = defaultTestBenchName != null ? defaultTestBenchName : "";
      this.InitializeComponent();
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      using (DbConnection newConnection = baseDbConnection.GetNewConnection())
      {
        string str1 = "SELECT *  FROM EQ_Equipment WHERE EquipmentID = EquipmentGroupID And OutOfOperationDate Is NULL AND ((EquipmentType = '" + requiredTypes[0] + "') ";
        for (int index = 1; index < requiredTypes.Length; ++index)
          str1 = str1 + " OR (EquipmentType = '" + requiredTypes[index] + "')";
        string selectSql1 = str1 + ") ORDER BY AccessName";
        InstallationData.EQ_EquipmentDataTable equipmentDataTable = new InstallationData.EQ_EquipmentDataTable();
        baseDbConnection.GetDataAdapter(selectSql1, newConnection).Fill((DataTable) equipmentDataTable);
        if (equipmentDataTable.Count == 0)
          throw new Exception("No equipment found");
        string str2 = "SELECT *  FROM EQ_EquipmentValue WHERE ValueType = 'RestrictedToPcName' AND ToDate IS NULL AND (EquipmentID = " + equipmentDataTable[0].EquipmentID.ToString();
        for (int index = 1; index < equipmentDataTable.Count; ++index)
          str2 = str2 + " OR EquipmentID = " + equipmentDataTable[index].EquipmentID.ToString();
        string selectSql2 = str2 + ")";
        InstallationData.EQ_EquipmentValueDataTable equipmentValueDataTable = new InstallationData.EQ_EquipmentValueDataTable();
        baseDbConnection.GetDataAdapter(selectSql2, newConnection).Fill((DataTable) equipmentValueDataTable);
        int? nullable1 = new int?();
        string machineName = Environment.MachineName;
        foreach (InstallationData.EQ_EquipmentValueRow equipmentValueRow in (TypedTableBase<InstallationData.EQ_EquipmentValueRow>) equipmentValueDataTable)
        {
          if (!equipmentValueRow.IsValueNull() && equipmentValueRow.Value.Length >= 3)
          {
            if (equipmentValueRow.Value.Contains(machineName))
            {
              nullable1 = new int?(equipmentValueRow.EquipmentID);
            }
            else
            {
              DataRow[] dataRowArray = equipmentDataTable.Select("EquipmentID = " + equipmentValueRow.EquipmentID.ToString());
              if (dataRowArray.Length == 1)
                dataRowArray[0].Delete();
            }
          }
        }
        if (nullable1.HasValue)
        {
          foreach (InstallationData.EQ_EquipmentRow eqEquipmentRow in (TypedTableBase<InstallationData.EQ_EquipmentRow>) equipmentDataTable)
          {
            int equipmentId = eqEquipmentRow.EquipmentID;
            int? nullable2 = nullable1;
            int valueOrDefault = nullable2.GetValueOrDefault();
            if (equipmentId == valueOrDefault & nullable2.HasValue)
            {
              this.defaultTestBenchName = eqEquipmentRow.AccessName;
              break;
            }
          }
        }
        this.DataGridTestBenches.ItemsSource = (IEnumerable) equipmentDataTable;
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.TextBoxCurrentName.Text = this.currentName;
      if (!string.IsNullOrEmpty(this.defaultTestBenchName))
      {
        foreach (object obj in (IEnumerable) this.DataGridTestBenches.Items)
        {
          if (((InstallationData.EQ_EquipmentRow) ((DataRowView) obj).Row).AccessName == this.defaultTestBenchName)
          {
            this.DataGridTestBenches.SelectedItem = obj;
            break;
          }
        }
      }
      else if (!string.IsNullOrEmpty(this.currentName))
      {
        foreach (object obj in (IEnumerable) this.DataGridTestBenches.Items)
        {
          if (((InstallationData.EQ_EquipmentRow) ((DataRowView) obj).Row).AccessName == this.currentName)
          {
            this.DataGridTestBenches.SelectedItem = obj;
            break;
          }
        }
      }
      this.ButtonRemoveTestBenchName.IsEnabled = !string.IsNullOrEmpty(this.currentName);
    }

    private void ButtonRemoveTestBenchName_Click(object sender, RoutedEventArgs e)
    {
      this.SetTestBenchName("");
    }

    private void ButtonChangeToSelection_Click(object sender, RoutedEventArgs e)
    {
      this.SetTestBenchName(((InstallationData.EQ_EquipmentRow) ((DataRowView) this.DataGridTestBenches.SelectedItem).Row).AccessName);
    }

    private void TextBoxFilter_LostFocus(object sender, RoutedEventArgs e)
    {
      this.LoadTestBenches();
    }

    private void TextBoxFilter_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.LoadTestBenches();
    }

    private void DataGridTestBenches_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      DataGrid dataGridTestBenches = this.DataGridTestBenches;
      if (dataGridTestBenches == null || dataGridTestBenches.SelectedItems == null || dataGridTestBenches.SelectedItems.Count != 1)
        return;
      this.SetTestBenchName(((InstallationData.EQ_EquipmentRow) ((DataRowView) dataGridTestBenches.SelectedItems[0]).Row).AccessName);
    }

    private void DataGridTestBenches_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.DataGridTestBenches.SelectedItem == null)
        this.ButtonChangeToSelection.IsEnabled = false;
      else if (((InstallationData.EQ_EquipmentRow) ((DataRowView) this.DataGridTestBenches.SelectedItem).Row).AccessName != this.currentName)
        this.ButtonChangeToSelection.IsEnabled = true;
      else
        this.ButtonChangeToSelection.IsEnabled = false;
    }

    private void SetTestBenchName(string theName)
    {
      this.changedName = theName;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/StartupLib;component/changetestbenchname.xaml", UriKind.Relative));
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
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.LableCurrentName = (Label) target;
          break;
        case 3:
          this.TextBoxCurrentName = (TextBox) target;
          break;
        case 4:
          this.LabelTextBoxFilter = (Label) target;
          break;
        case 5:
          this.TextBoxFilter = (TextBox) target;
          this.TextBoxFilter.LostFocus += new RoutedEventHandler(this.TextBoxFilter_LostFocus);
          this.TextBoxFilter.KeyDown += new KeyEventHandler(this.TextBoxFilter_KeyDown);
          break;
        case 6:
          this.ButtonRemoveTestBenchName = (Button) target;
          this.ButtonRemoveTestBenchName.Click += new RoutedEventHandler(this.ButtonRemoveTestBenchName_Click);
          break;
        case 7:
          this.ButtonChangeToSelection = (Button) target;
          this.ButtonChangeToSelection.Click += new RoutedEventHandler(this.ButtonChangeToSelection_Click);
          break;
        case 8:
          this.DataGridTestBenches = (DataGrid) target;
          this.DataGridTestBenches.SelectionChanged += new SelectionChangedEventHandler(this.DataGridTestBenches_SelectionChanged);
          this.DataGridTestBenches.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridTestBenches_MouseDoubleClick);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
