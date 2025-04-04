// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.COpenMeter
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class COpenMeter : Form
  {
    private Button btOK;
    private Button btCancel;
    private Button btSeek;
    private Label label1;
    private TextBox txtbxSuchstring;
    private DataGrid dtagrdSerial;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;
    private string ErrorMsg;
    private DataTable dtatblMeter;
    private DataTable MeterDataTable;
    public int m_iMeterID;
    public int m_iMeterInfoID;
    public string m_sSerialNr;
    private DataGrid MeterDataDataGrid;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button btSeekMeterID;
    private DataGridTableStyle dataGridTableStyle1;
    private DataGridTextBoxColumn dataGridTextBoxTimePoint;
    private Label label2;
    private Label label3;
    private Button buttonSeachType;
    private Label labelLoadStatus;
    public DateTime DataTimePoint;
    private Button buttonSearchDay;
    public string DeviceFilter;

    public COpenMeter()
    {
      this.InitializeComponent();
      this.MeterDataTable = new DataTable("MeterData");
      this.m_iMeterID = 0;
      this.m_iMeterInfoID = 0;
      this.m_sSerialNr = "0";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (COpenMeter));
      this.btOK = new Button();
      this.btCancel = new Button();
      this.btSeek = new Button();
      this.txtbxSuchstring = new TextBox();
      this.label1 = new Label();
      this.btSeekMeterID = new Button();
      this.dtagrdSerial = new DataGrid();
      this.dtatblMeter = new DataTable();
      this.MeterDataDataGrid = new DataGrid();
      this.dataGridTableStyle1 = new DataGridTableStyle();
      this.dataGridTextBoxTimePoint = new DataGridTextBoxColumn();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.label2 = new Label();
      this.label3 = new Label();
      this.buttonSeachType = new Button();
      this.labelLoadStatus = new Label();
      this.buttonSearchDay = new Button();
      this.dtagrdSerial.BeginInit();
      this.dtatblMeter.BeginInit();
      this.MeterDataDataGrid.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.btOK, "btOK");
      this.btOK.DialogResult = DialogResult.OK;
      this.btOK.Name = "btOK";
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      componentResourceManager.ApplyResources((object) this.btSeek, "btSeek");
      this.btSeek.Name = "btSeek";
      this.btSeek.Click += new System.EventHandler(this.btSuchen_Click);
      componentResourceManager.ApplyResources((object) this.txtbxSuchstring, "txtbxSuchstring");
      this.txtbxSuchstring.Name = "txtbxSuchstring";
      this.txtbxSuchstring.KeyDown += new KeyEventHandler(this.txtbxSuchstring_KeyDown);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.btSeekMeterID, "btSeekMeterID");
      this.btSeekMeterID.Name = "btSeekMeterID";
      this.btSeekMeterID.Click += new System.EventHandler(this.btSeekMeterID_Click);
      componentResourceManager.ApplyResources((object) this.dtagrdSerial, "dtagrdSerial");
      this.dtagrdSerial.HeaderForeColor = SystemColors.ControlText;
      this.dtagrdSerial.Name = "dtagrdSerial";
      this.dtagrdSerial.ReadOnly = true;
      this.dtagrdSerial.CurrentCellChanged += new System.EventHandler(this.dtagrdSerial_CurrentCellChanged);
      this.MeterDataDataGrid.AllowSorting = false;
      componentResourceManager.ApplyResources((object) this.MeterDataDataGrid, "MeterDataDataGrid");
      this.MeterDataDataGrid.HeaderForeColor = SystemColors.ControlText;
      this.MeterDataDataGrid.Name = "MeterDataDataGrid";
      this.MeterDataDataGrid.ReadOnly = true;
      this.MeterDataDataGrid.TableStyles.AddRange(new DataGridTableStyle[1]
      {
        this.dataGridTableStyle1
      });
      this.MeterDataDataGrid.DoubleClick += new System.EventHandler(this.MeterDataDataGrid_DoubleClick);
      this.MeterDataDataGrid.Click += new System.EventHandler(this.MeterDataDataGrid_Click);
      this.dataGridTableStyle1.DataGrid = this.MeterDataDataGrid;
      this.dataGridTableStyle1.GridColumnStyles.AddRange(new DataGridColumnStyle[1]
      {
        (DataGridColumnStyle) this.dataGridTextBoxTimePoint
      });
      this.dataGridTableStyle1.HeaderForeColor = SystemColors.ControlText;
      this.dataGridTableStyle1.MappingName = "MeterData";
      this.dataGridTableStyle1.ReadOnly = true;
      this.dataGridTextBoxTimePoint.Format = "dd.MM.yyyy HH:mm:ss";
      this.dataGridTextBoxTimePoint.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridTextBoxTimePoint, "dataGridTextBoxTimePoint");
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.buttonSeachType, "buttonSeachType");
      this.buttonSeachType.Name = "buttonSeachType";
      this.buttonSeachType.Click += new System.EventHandler(this.buttonSeachType_Click);
      componentResourceManager.ApplyResources((object) this.labelLoadStatus, "labelLoadStatus");
      this.labelLoadStatus.Name = "labelLoadStatus";
      componentResourceManager.ApplyResources((object) this.buttonSearchDay, "buttonSearchDay");
      this.buttonSearchDay.Name = "buttonSearchDay";
      this.buttonSearchDay.Click += new System.EventHandler(this.buttonSearchDay_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.labelLoadStatus);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.MeterDataDataGrid);
      this.Controls.Add((Control) this.dtagrdSerial);
      this.Controls.Add((Control) this.buttonSeachType);
      this.Controls.Add((Control) this.buttonSearchDay);
      this.Controls.Add((Control) this.btSeekMeterID);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtbxSuchstring);
      this.Controls.Add((Control) this.btSeek);
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btOK);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Name = nameof (COpenMeter);
      this.dtagrdSerial.EndInit();
      this.dtatblMeter.EndInit();
      this.MeterDataDataGrid.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void btSuchen_Click(object sender, EventArgs e) => this.SearchOnSerialNumber();

    private void txtbxSuchstring_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData != Keys.Return)
        return;
      this.SearchOnSerialNumber();
      e.Handled = true;
    }

    private void SearchOnSerialNumber()
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        if (this.txtbxSuchstring.Text.Length <= 0)
          return;
        string SqlCommand = this.DeviceFilter != null ? "SELECT Meter.MeterID, Meter.SerialNr AS SerialNr, Meter.MeterInfoID AS MeterInfoId, Meter.ProductionDate AS ProductionDate, Meter.ApprovalDate AS ApprovalDate, Meter.OrderNr AS OrderNr FROM Meter,MeterInfo WHERE Meter.SerialNr LIKE '" + this.txtbxSuchstring.Text + "' AND Meter.MeterInfoID = MeterInfo.MeterInfoID  AND MeterInfo.PPSArtikelNr = 'MinolDevice' ORDER by Meter.SerialNr" : "select * from Meter where SerialNr like '" + this.txtbxSuchstring.Text + "' order by SerialNr";
        this.dtatblMeter = new DataTable();
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(this.dtatblMeter);
          dbConnection.Close();
        }
        this.dtagrdSerial.DataSource = (object) this.dtatblMeter;
        this.showBackups();
      }
      catch (Exception ex)
      {
        this.ErrorMsg = ex.ToString();
        int num = (int) MessageBox.Show(this.ErrorMsg);
      }
    }

    private void btSeekMeterID_Click(object sender, EventArgs e)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        if (this.txtbxSuchstring.Text.Length <= 0)
          return;
        string SqlCommand = "select * from Meter where MeterID like '" + this.txtbxSuchstring.Text + "' order by MeterID";
        this.dtatblMeter.Clear();
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(this.dtatblMeter);
          dbConnection.Close();
        }
        this.dtagrdSerial.DataSource = (object) this.dtatblMeter;
        this.showBackups();
      }
      catch (Exception ex)
      {
        this.ErrorMsg = ex.ToString();
        int num = (int) MessageBox.Show(this.ErrorMsg);
      }
    }

    private void buttonSeachType_Click(object sender, EventArgs e)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        if (this.txtbxSuchstring.Text.Length <= 0)
          return;
        string SqlCommand = "select * from Meter,MeterInfo where MeterInfo.PPSArtikelNr = '" + this.txtbxSuchstring.Text + "' AND Meter.MeterInfoID = MeterInfo.MeterInfoID ORDER BY Meter.SerialNr";
        this.dtatblMeter.Clear();
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(this.dtatblMeter);
          dbConnection.Close();
        }
        this.dtagrdSerial.DataSource = (object) this.dtatblMeter;
        this.showBackups();
      }
      catch (Exception ex)
      {
        this.ErrorMsg = ex.ToString();
        int num = (int) MessageBox.Show(this.ErrorMsg);
      }
    }

    private void dtaGrdMenueViewItemMouseUp(object sender, MouseEventArgs e)
    {
      if (this.dtatblMeter.Rows.Count <= 0)
        return;
      DataRow rowInDtatblMeter = this.getDataRowInDtatblMeter();
      if (rowInDtatblMeter != null)
      {
        this.m_iMeterID = int.Parse(rowInDtatblMeter["MeterID"].ToString());
        this.m_sSerialNr = rowInDtatblMeter["SerialNr"].ToString();
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.m_iMeterID = 0;
      this.m_iMeterInfoID = 0;
      this.m_sSerialNr = "0";
    }

    private void dtagrdSerial_DoubleClick(object sender, EventArgs e) => this.showBackups();

    private void buttonShowBackupData_Click(object sender, EventArgs e) => this.showBackups();

    private void showBackups()
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        this.labelLoadStatus.Text = this.dtatblMeter.Rows.Count.ToString() + " devices";
        if (this.dtatblMeter.Rows.Count > 0)
        {
          DataRow rowInDtatblMeter = this.getDataRowInDtatblMeter();
          if (rowInDtatblMeter != null)
          {
            this.m_iMeterID = int.Parse(rowInDtatblMeter["MeterID"].ToString());
            this.m_iMeterInfoID = int.Parse(rowInDtatblMeter["MeterInfoID"].ToString());
            this.m_sSerialNr = rowInDtatblMeter["SerialNr"].ToString();
          }
          this.Cursor = Cursors.WaitCursor;
          string SqlCommand = "SELECT * from MeterData WHERE MeterID = " + this.m_iMeterID.ToString() + " AND PValueID = " + 1.ToString() + " ORDER BY TimePoint DESC";
          this.MeterDataTable.Clear();
          using (IDbConnection dbConnection = primaryDb.GetDbConnection())
          {
            dbConnection.Open();
            primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(this.MeterDataTable);
            dbConnection.Close();
          }
          this.MeterDataDataGrid.DataSource = (object) this.MeterDataTable;
          if (this.MeterDataTable.Rows.Count > 0)
          {
            this.MeterDataDataGrid.Select(0);
            this.DataTimePoint = DateTime.Parse(this.getDataRowInMeterDataTable()["TimePoint"].ToString());
          }
          this.Cursor = Cursors.Default;
        }
        else
        {
          this.MeterDataTable.Clear();
          this.MeterDataDataGrid.DataSource = (object) this.MeterDataTable;
        }
      }
      catch (Exception ex)
      {
        this.ErrorMsg = ex.ToString();
        int num = (int) MessageBox.Show(this.ErrorMsg);
      }
    }

    private DataRow getDataRowInDtatblMeter()
    {
      return ((DataRowView) this.dtagrdSerial.BindingContext[(object) this.dtatblMeter].Current).Row;
    }

    private DataRow getDataRowInMeterDataTable()
    {
      BindingManagerBase bindingManagerBase = this.MeterDataDataGrid.BindingContext[(object) this.MeterDataTable];
      return bindingManagerBase.Position == -1 ? (DataRow) null : ((DataRowView) bindingManagerBase.Current).Row;
    }

    private void MeterDataDataGrid_DoubleClick(object sender, EventArgs e)
    {
      DataRow inMeterDataTable = this.getDataRowInMeterDataTable();
      if (inMeterDataTable == null)
        return;
      this.DataTimePoint = DateTime.Parse(inMeterDataTable["TimePoint"].ToString());
      this.DialogResult = DialogResult.OK;
    }

    private void MeterDataDataGrid_Click(object sender, EventArgs e)
    {
      DataRow inMeterDataTable = this.getDataRowInMeterDataTable();
      if (inMeterDataTable == null)
        return;
      this.DataTimePoint = DateTime.Parse(inMeterDataTable["TimePoint"].ToString());
    }

    private void dtagrdSerial_CurrentCellChanged(object sender, EventArgs e) => this.showBackups();

    private void buttonSearchDay_Click(object sender, EventArgs e)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        if (this.txtbxSuchstring.Text.Length <= 0)
          return;
        DateTime result;
        if (!DateTime.TryParse(this.txtbxSuchstring.Text, out result))
        {
          TextBox txtbxSuchstring = this.txtbxSuchstring;
          DateTime dateTime = DateTime.Now;
          dateTime = dateTime.Date;
          string shortDateString = dateTime.ToShortDateString();
          txtbxSuchstring.Text = shortDateString;
        }
        else
        {
          DateTime date = result.Date;
          DateTime dateTime = date.AddDays(1.0);
          string SqlCommand = "SELECT Meter.MeterID, Meter.SerialNr AS SerialNr, Meter.MeterInfoID AS MeterInfoId, Meter.ProductionDate AS ProductionDate, Meter.ApprovalDate AS ApprovalDate, Meter.OrderNr AS OrderNr FROM Meter,MeterInfo,MeterType WHERE Meter.MeterInfoID = MeterInfo.MeterInfoID AND MeterInfo.MeterTypeID = MeterType.MeterTypeID AND MeterType.MTypeTableName = 'MTypeZelsius' AND ProductionDate >= @ProductionDay AND ProductionDate <= @NextDay ORDER BY ProductionDate DESC";
          this.dtatblMeter.Clear();
          using (IDbConnection dbConnection = primaryDb.GetDbConnection())
          {
            dbConnection.Open();
            ZRDataAdapter zrDataAdapter = primaryDb.ZRDataAdapter(SqlCommand, dbConnection);
            DbParameter parameter1 = zrDataAdapter.SelectCommand.CreateParameter();
            parameter1.DbType = DbType.DateTime;
            parameter1.ParameterName = "@ProductionDay";
            parameter1.Value = (object) date;
            zrDataAdapter.SelectCommand.Parameters.Add((object) parameter1);
            DbParameter parameter2 = zrDataAdapter.SelectCommand.CreateParameter();
            parameter2.DbType = DbType.DateTime;
            parameter2.ParameterName = "@NextDay";
            parameter2.Value = (object) dateTime;
            zrDataAdapter.SelectCommand.Parameters.Add((object) parameter2);
            zrDataAdapter.Fill(this.dtatblMeter);
            dbConnection.Close();
          }
          this.dtagrdSerial.DataSource = (object) this.dtatblMeter;
          this.showBackups();
        }
      }
      catch (Exception ex)
      {
        this.ErrorMsg = ex.ToString();
        int num = (int) MessageBox.Show(this.ErrorMsg);
      }
    }
  }
}
