// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.BaseTypeCheck
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class BaseTypeCheck : Form
  {
    private string SQL;
    private string errorMsg;
    private DataTable MeterInfoTab = new DataTable("MeterInfo");
    private DataGrid dataGrid1;
    private Button OkButton;
    private Label label1;
    private Label InfoTextLabel;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button ZRCancelButton;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public BaseTypeCheck(string InfoText, int theMeterInfoID)
    {
      this.InitializeComponent();
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        this.InfoTextLabel.Text = InfoText;
        this.SQL = "select * from MeterInfo where MeterTypeID = (select MeterTypeID from MeterInfo where MeterInfoID = " + theMeterInfoID.ToString() + ")";
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(this.SQL, dbConnection).Fill(this.MeterInfoTab);
          dbConnection.Close();
        }
        this.dataGrid1.DataSource = (object) this.MeterInfoTab;
      }
      catch (Exception ex)
      {
        this.errorMsg = ex.ToString();
        int num = (int) MessageBox.Show(this.errorMsg);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BaseTypeCheck));
      this.dataGrid1 = new DataGrid();
      this.OkButton = new Button();
      this.ZRCancelButton = new Button();
      this.label1 = new Label();
      this.InfoTextLabel = new Label();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.dataGrid1.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.dataGrid1, "dataGrid1");
      this.dataGrid1.DataMember = "";
      this.dataGrid1.HeaderForeColor = SystemColors.ControlText;
      this.dataGrid1.Name = "dataGrid1";
      componentResourceManager.ApplyResources((object) this.OkButton, "OkButton");
      this.OkButton.DialogResult = DialogResult.OK;
      this.OkButton.Name = "OkButton";
      componentResourceManager.ApplyResources((object) this.ZRCancelButton, "ZRCancelButton");
      this.ZRCancelButton.DialogResult = DialogResult.Cancel;
      this.ZRCancelButton.Name = "ZRCancelButton";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.InfoTextLabel, "InfoTextLabel");
      this.InfoTextLabel.Name = "InfoTextLabel";
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.InfoTextLabel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.ZRCancelButton);
      this.Controls.Add((Control) this.OkButton);
      this.Controls.Add((Control) this.dataGrid1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Name = nameof (BaseTypeCheck);
      this.dataGrid1.EndInit();
      this.ResumeLayout(false);
    }

    public bool checkIfBaseType(int theMeterInfoID)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        DataTable dataTable = new DataTable();
        this.SQL = "select MeterInfoID from MeterInfo where MeterInfoID = " + theMeterInfoID.ToString() + " and PPSArtikelNr = 'BASETYPE'";
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(this.SQL, dbConnection).Fill(dataTable);
          dbConnection.Close();
        }
        return dataTable.Rows.Count > 0;
      }
      catch (Exception ex)
      {
        this.errorMsg = ex.ToString();
        return false;
      }
    }

    public int getBaseTypeMeterInfoID(
      int childMeterInfoID,
      out int baseTypeMeterInfoID,
      out string ErrorMsg)
    {
      ErrorMsg = "";
      DataTable dataTable = new DataTable();
      baseTypeMeterInfoID = -1;
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        string SqlCommand = "select MeterInfoID from MeterInfo where MeterTypeID = (select MeterTypeID from MeterInfo where MeterInfoID = " + childMeterInfoID.ToString() + ") and HardwareTypeID = (select HardwareTypeID from MeterInfo where MeterInfoID = " + childMeterInfoID.ToString() + ") and MeterHardwareID = (select MeterHardwareID from MeterInfo where MeterInfoID = " + childMeterInfoID.ToString() + ") and DefaultFunctionNr = (select DefaultFunctionNr from MeterInfo where MeterInfoID = " + childMeterInfoID.ToString() + ") and PPSArtikelNr = 'BASETYPE'";
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(dataTable);
          dbConnection.Close();
        }
        int baseTypeMeterInfoId;
        if (dataTable.Rows.Count > 0)
        {
          baseTypeMeterInfoID = int.Parse(dataTable.Rows[0]["MeterInfoID"].ToString());
          baseTypeMeterInfoId = 0;
        }
        else
        {
          ErrorMsg = "Can not find a BASETYPE of the MeterinfoType: " + childMeterInfoID.ToString();
          baseTypeMeterInfoId = 1024;
        }
        return baseTypeMeterInfoId;
      }
      catch
      {
        return 2;
      }
    }
  }
}
