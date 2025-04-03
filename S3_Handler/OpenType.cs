// Decompiled with JetBrains decompiler
// Type: S3_Handler.OpenType
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using GmmDbLib;
using StartupLib;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  public class OpenType : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private DataTable MyTable;
    public bool GetOnlySelectedInfos = false;
    public int MeterTypeId;
    public string SAP_Number;
    private bool dataChanged = false;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private DataGridView dataGridViewTypes;
    private Button buttonOpen;
    private Button buttonDeleteType;
    private TextBox textBoxType;
    private Label label1;

    public OpenType(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
    }

    private void OpenType_Load(object sender, EventArgs e) => this.LoadTypeTable();

    private void LoadTypeTable()
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT ");
        stringBuilder.Append(" MeterInfo.Description AS Description");
        stringBuilder.Append(" ,MeterInfo.PPSArtikelNr AS SAP_Number");
        stringBuilder.Append(" ,MeterType.GenerateDate AS GenerateDate");
        stringBuilder.Append(" ,MeterInfo.MeterInfoId AS MeterInfoId");
        stringBuilder.Append(" ,MeterInfo.DefaultFunctionNr AS BasetypeMeterInfoId");
        stringBuilder.Append(" ,MeterInfo.HardwareTypeId AS HardwareTypeId");
        stringBuilder.Append(" ,MeterType.MeterTypeID AS MeterTypeID");
        stringBuilder.Append(" ,MeterType.Description AS MeterTypeDescription");
        stringBuilder.Append(" ,MeterHardware.MeterHardwareID AS MeterHardwareID");
        stringBuilder.Append(" ,MeterHardware.MeterName AS MeterName");
        stringBuilder.Append(" ,MTypeZelsius.TypeCreationString AS TypeCreationString");
        stringBuilder.Append(" ,MTypeZelsius.TypeOverrideString AS TypeOverrideString");
        stringBuilder.Append(" ,MeterHardware.Description AS MeterHardwareDescription");
        stringBuilder.Append(" FROM MeterInfo,MeterType,MeterHardware,MTypeZelsius");
        stringBuilder.Append(" WHERE MeterType.Typename = 'S3_Device'");
        stringBuilder.Append(" AND MeterInfo.MeterTypeID = MeterType.MeterTypeID");
        stringBuilder.Append(" AND MeterInfo.MeterTypeID = MTypeZelsius.MeterTypeID");
        stringBuilder.Append(" AND MeterInfo.MeterHardwareID = MeterHardware.MeterHardwareID");
        if (!this.MyFunctions.IsHandlerCompleteEnabled() && !UserManager.CheckPermission(UserRights.Rights.ProfessionalConfig))
          stringBuilder.Append(" AND NOT MeterInfo.PPSArtikelNr = 'S3_BASETYPE'");
        stringBuilder.Append(" ORDER BY MeterInfo.MeterInfoID DESC");
        this.MyTable = new DataTable();
        this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(stringBuilder.ToString(), this.MyTable);
        if (this.MyTable.Rows.Count <= 0)
          return;
        this.dataGridViewTypes.DataSource = (object) this.MyTable;
        int index1 = 0;
        string str = this.MyFunctions.MeterInfoIdOfType.ToString();
        for (int index2 = 0; index2 < this.dataGridViewTypes.Rows.Count; ++index2)
        {
          if (str == this.dataGridViewTypes.Rows[index2].Cells["MeterInfoId"].Value.ToString())
          {
            index1 = index2;
            break;
          }
        }
        this.dataGridViewTypes.ClearSelection();
        this.dataGridViewTypes.Rows[index1].Selected = true;
        this.SetTypeToTextBox();
        this.textBoxType.Focus();
        this.textBoxType.SelectAll();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read type data");
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
    }

    private void buttonOpen_Click(object sender, EventArgs e) => this.OpenTypeWork();

    private void dataGridViewTypes_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyValue != 13)
        return;
      e.Handled = true;
      this.SetTypeToTextBox();
      this.OpenTypeWork();
    }

    private void dataGridViewTypes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      this.SetTypeToTextBox();
      this.OpenTypeWork();
    }

    private void OpenTypeWork()
    {
      ZR_ClassLibMessages.ClearErrors();
      if (!this.GetOnlySelectedInfos)
      {
        this.MyFunctions.MyMeters.OpenType(this.MyFunctions.MeterInfoIdOfType);
        if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.NoError)
          this.DialogResult = DialogResult.OK;
      }
      else
        this.DialogResult = DialogResult.OK;
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.Close();
    }

    private void buttonDeleteType_Click(object sender, EventArgs e)
    {
      if (GMM_MessageBox.ShowMessage("Delete type", "Are you sure to delete the type?", MessageBoxButtons.OKCancel) != DialogResult.OK)
        return;
      bool flag = false;
      BaseDbConnection baseDb = this.MyFunctions.MyDatabase.BaseDb;
      this.MyFunctions.MyDatabase.DbConn.Open();
      DbTransaction transaction = this.MyFunctions.MyDatabase.DbConn.BeginTransaction();
      try
      {
        string selectSql1 = "SELECT * FROM MeterInfo WHERE MeterInfoID = " + this.MyFunctions.MeterInfoIdOfType.ToString();
        Schema.MeterInfoDataTable meterInfoDataTable = new Schema.MeterInfoDataTable();
        DbDataAdapter dataAdapter1 = baseDb.GetDataAdapter(selectSql1, this.MyFunctions.MyDatabase.DbConn, transaction);
        dataAdapter1.Fill((DataTable) meterInfoDataTable);
        if (meterInfoDataTable.Count == 1)
        {
          this.MeterTypeId = meterInfoDataTable[0].MeterTypeID;
          this.SAP_Number = meterInfoDataTable[0].PPSArtikelNr;
          meterInfoDataTable[0].Delete();
          dataAdapter1.Update((DataTable) meterInfoDataTable);
        }
        string selectSql2 = "SELECT * FROM MeterType WHERE MeterTypeID = " + this.MeterTypeId.ToString();
        Schema.MeterTypeDataTable meterTypeDataTable = new Schema.MeterTypeDataTable();
        DbDataAdapter dataAdapter2 = baseDb.GetDataAdapter(selectSql2, this.MyFunctions.MyDatabase.DbConn, transaction);
        dataAdapter2.Fill((DataTable) meterTypeDataTable);
        if (meterTypeDataTable.Count == 1)
        {
          meterTypeDataTable[0].Delete();
          dataAdapter2.Update((DataTable) meterTypeDataTable);
        }
        string selectSql3 = "SELECT * FROM MTypeZelsius WHERE MeterTypeID = " + this.MeterTypeId.ToString();
        Schema.MTypeZelsiusDataTable zelsiusDataTable = new Schema.MTypeZelsiusDataTable();
        DbDataAdapter dataAdapter3 = baseDb.GetDataAdapter(selectSql3, this.MyFunctions.MyDatabase.DbConn, transaction);
        dataAdapter3.Fill((DataTable) zelsiusDataTable);
        if (zelsiusDataTable.Count == 1)
        {
          zelsiusDataTable[0].Delete();
          dataAdapter3.Update((DataTable) zelsiusDataTable);
        }
        try
        {
          string selectSql4 = "Select * from PPS_Cache where PPS_MaterialNumber = '" + this.SAP_Number + "'";
          Schema.PPS_CacheDataTable ppsCacheDataTable = new Schema.PPS_CacheDataTable();
          DbDataAdapter dataAdapter4 = baseDb.GetDataAdapter(selectSql4, this.MyFunctions.MyDatabase.DbConn, transaction);
          dataAdapter4.Fill((DataTable) ppsCacheDataTable);
          foreach (DataRow row in (InternalDataCollectionBase) ppsCacheDataTable.Rows)
            row.Delete();
          dataAdapter4.Update((DataTable) ppsCacheDataTable);
        }
        catch
        {
        }
        flag = true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on delete type" + ZR_Constants.SystemNewLine + ex.ToString());
      }
      finally
      {
        if (flag)
          transaction.Commit();
        else
          transaction.Rollback();
        this.MyFunctions.MyDatabase.DbConn.Close();
      }
      this.LoadTypeTable();
    }

    private void dataGridViewTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      this.SetTypeToTextBox();
    }

    private void dataGridViewTypes_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
      this.SetTypeToTextBox();
    }

    private void SetTypeToTextBox()
    {
      if (this.dataGridViewTypes.SelectedCells.Count == 0)
        return;
      int rowIndex = this.dataGridViewTypes.SelectedCells[0].RowIndex;
      this.MyFunctions.MeterInfoIdOfType = int.Parse(this.dataGridViewTypes.Rows[rowIndex].Cells["MeterInfoId"].Value.ToString());
      this.MeterTypeId = int.Parse(this.dataGridViewTypes.Rows[rowIndex].Cells["MeterTypeID"].Value.ToString());
      this.SAP_Number = this.dataGridViewTypes.Rows[rowIndex].Cells["SAP_Number"].Value.ToString();
      this.textBoxType.Text = this.SAP_Number;
    }

    private void textBoxType_Leave(object sender, EventArgs e)
    {
      if (this.dataChanged)
        return;
      string selectSql = "SELECT * FROM MeterInfo WHERE PPSArtikelNr = '" + this.textBoxType.Text + "'";
      Schema.MeterInfoDataTable meterInfoDataTable = new Schema.MeterInfoDataTable();
      this.MyFunctions.MyDatabase.BaseDb.GetDataAdapter(selectSql, this.MyFunctions.MyDatabase.DbConn).Fill((DataTable) meterInfoDataTable);
      if (meterInfoDataTable.Count == 1)
      {
        this.MyFunctions.MeterInfoIdOfType = meterInfoDataTable[0].MeterInfoID;
        this.MeterTypeId = meterInfoDataTable[0].MeterTypeID;
      }
    }

    private void textBoxType_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyValue != 13)
        return;
      e.Handled = true;
      for (int index = 0; index < this.dataGridViewTypes.Rows.Count; ++index)
      {
        if (this.textBoxType.Text == this.dataGridViewTypes.Rows[index].Cells["SAP_Number"].Value.ToString())
        {
          this.MyFunctions.MeterInfoIdOfType = int.Parse(this.dataGridViewTypes.Rows[index].Cells["MeterInfoId"].Value.ToString());
          this.MeterTypeId = int.Parse(this.dataGridViewTypes.Rows[index].Cells["MeterTypeID"].Value.ToString());
          break;
        }
      }
      this.dataChanged = true;
      this.buttonOpen.Focus();
      this.dataChanged = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dataGridViewTypes = new DataGridView();
      this.buttonOpen = new Button();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonDeleteType = new Button();
      this.textBoxType = new TextBox();
      this.label1 = new Label();
      ((ISupportInitialize) this.dataGridViewTypes).BeginInit();
      this.SuspendLayout();
      this.dataGridViewTypes.AllowUserToAddRows = false;
      this.dataGridViewTypes.AllowUserToDeleteRows = false;
      this.dataGridViewTypes.AllowUserToResizeRows = false;
      this.dataGridViewTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewTypes.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
      this.dataGridViewTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewTypes.Location = new Point(12, 45);
      this.dataGridViewTypes.Name = "dataGridViewTypes";
      this.dataGridViewTypes.ReadOnly = true;
      this.dataGridViewTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewTypes.Size = new Size(960, 306);
      this.dataGridViewTypes.TabIndex = 18;
      this.dataGridViewTypes.CellContentClick += new DataGridViewCellEventHandler(this.dataGridViewTypes_CellContentClick);
      this.dataGridViewTypes.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridViewTypes_CellDoubleClick);
      this.dataGridViewTypes.RowEnter += new DataGridViewCellEventHandler(this.dataGridViewTypes_RowEnter);
      this.dataGridViewTypes.KeyDown += new KeyEventHandler(this.dataGridViewTypes_KeyDown);
      this.buttonOpen.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOpen.Location = new Point(801, 357);
      this.buttonOpen.Name = "buttonOpen";
      this.buttonOpen.Size = new Size(171, 34);
      this.buttonOpen.TabIndex = 19;
      this.buttonOpen.Text = "Open type";
      this.buttonOpen.UseVisualStyleBackColor = true;
      this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(984, 105);
      this.zennerCoroprateDesign2.TabIndex = 17;
      this.buttonDeleteType.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonDeleteType.Location = new Point(624, 358);
      this.buttonDeleteType.Name = "buttonDeleteType";
      this.buttonDeleteType.Size = new Size(171, 34);
      this.buttonDeleteType.TabIndex = 19;
      this.buttonDeleteType.Text = "Delete type";
      this.buttonDeleteType.UseVisualStyleBackColor = true;
      this.buttonDeleteType.Click += new System.EventHandler(this.buttonDeleteType_Click);
      this.textBoxType.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxType.Location = new Point(447, 365);
      this.textBoxType.Name = "textBoxType";
      this.textBoxType.Size = new Size(152, 20);
      this.textBoxType.TabIndex = 20;
      this.textBoxType.KeyDown += new KeyEventHandler(this.textBoxType_KeyDown);
      this.textBoxType.Leave += new System.EventHandler(this.textBoxType_Leave);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(387, 369);
      this.label1.Name = "label1";
      this.label1.Size = new Size(31, 13);
      this.label1.TabIndex = 21;
      this.label1.Text = "Type";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(984, 404);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxType);
      this.Controls.Add((Control) this.buttonDeleteType);
      this.Controls.Add((Control) this.buttonOpen);
      this.Controls.Add((Control) this.dataGridViewTypes);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (OpenType);
      this.Text = nameof (OpenType);
      this.WindowState = FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.OpenType_Load);
      ((ISupportInitialize) this.dataGridViewTypes).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
