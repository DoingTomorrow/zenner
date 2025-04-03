// Decompiled with JetBrains decompiler
// Type: S3_Handler.SaveType
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  public class SaveType : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private DataGridView dataGridViewHardwareTypes;
    private Button buttonOk;
    private Label label1;
    private Label label2;
    private TextBox textBoxMeterInfoDescription;
    private Label label3;
    private TextBox textBoxMeterTypeDescription;
    private TextBox textBoxMeterInfoId;
    private TextBox textBoxSAP_Number;
    private Label label6;
    private ComboBox comboBoxSaveOptions;
    private GroupBox groupBoxType;
    private RadioButton radioButtonTypeNotDefined;
    private RadioButton radioButtonTypeBase;
    private RadioButton radioButtonTypeSpecial;
    private GroupBox groupBoxMeterInfo;
    private GroupBox groupBoxHardwareIdentification;
    private RadioButton radioButtonHardwareFromWork;
    private RadioButton radioButtonHardwareFromRead;
    private RadioButton radioButtonHardwareFromType;
    private RadioButton radioButtonHardwareManual;
    private TextBox textBoxHardwareTypeId;
    private CheckBox checkBoxNewType;
    private GroupBox groupBoxDeviceHardware;
    private ComboBox comboBoxCommunicationInterfaces;
    private ComboBox comboBoxVolumeMeterBounding;
    private ComboBox comboBoxVolumeMeterType;
    private CheckBox checkBoxChangeDeviceHardware;
    private RadioButton radioButtonTypeAuto;
    private RadioButton radioButtonParameterType;
    private Label label4;
    private TextBox textBoxParameterTypeUsing;
    private Label label5;
    private TextBox textBoxTypeCreationString;

    public SaveType(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      try
      {
        Schema.HardwareTypeDataTable Table = new Schema.HardwareTypeDataTable();
        this.MyFunctions.MyDatabase.GetDataTableBySQLCommand("SELECT * FROM HardwareType WHERE HardwareResource LIKE '%;Serie3;%' ORDER BY FirmwareVersion DESC,HardwareVersion DESC", (DataTable) Table);
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn(SaveType.HardwareTableRows.HardwareTypeId.ToString(), typeof (int)));
        dataTable.Columns.Add(new DataColumn(SaveType.HardwareTableRows.HardwareType.ToString(), typeof (string)));
        dataTable.Columns.Add(new DataColumn(SaveType.HardwareTableRows.FirmwareVersion.ToString(), typeof (string)));
        dataTable.Columns.Add(new DataColumn(SaveType.HardwareTableRows.HardwareResources.ToString(), typeof (string)));
        foreach (Schema.HardwareTypeRow hardwareTypeRow in (TypedTableBase<Schema.HardwareTypeRow>) Table)
        {
          DataRow row = dataTable.NewRow();
          row[0] = (object) hardwareTypeRow.HardwareTypeID;
          row[1] = (object) ParameterService.GetHardwareString((uint) hardwareTypeRow.HardwareVersion);
          row[2] = (object) ParameterService.GetVersionString((long) hardwareTypeRow.FirmwareVersion, 8);
          row[3] = (object) hardwareTypeRow.HardwareResource;
          dataTable.Rows.Add(row);
        }
        this.dataGridViewHardwareTypes.DataSource = (object) dataTable;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Read data from table MeterHardware");
      }
      if (!this.MyFunctions.IsHandlerCompleteEnabled())
        this.radioButtonTypeBase.Visible = false;
      this.comboBoxSaveOptions.DataSource = (object) Enum.GetNames(typeof (SaveOptions));
      this.comboBoxVolumeMeterType.Items.AddRange((object[]) Enum.GetNames(typeof (DHI_VolumeMeterType)));
      this.comboBoxVolumeMeterType.SelectedIndex = 0;
      this.comboBoxVolumeMeterBounding.Items.AddRange((object[]) Enum.GetNames(typeof (DHI_VolumeMeterBounding)));
      this.comboBoxVolumeMeterBounding.SelectedIndex = 0;
      this.comboBoxCommunicationInterfaces.Items.AddRange((object[]) new string[4]
      {
        "NONE",
        "MBus",
        "radio",
        "RS485"
      });
      this.comboBoxCommunicationInterfaces.SelectedIndex = 0;
    }

    private void SaveType_Load(object sender, EventArgs e)
    {
      uint num;
      if (this.MyFunctions.MyMeters.TypeMeter == null)
      {
        this.Text = "Create new type";
        this.radioButtonHardwareFromType.Enabled = false;
        this.radioButtonHardwareFromWork.Checked = true;
        this.radioButtonTypeNotDefined.Checked = true;
        num = 0U;
        this.checkBoxNewType.Enabled = true;
        this.checkBoxNewType.Checked = true;
        this.comboBoxSaveOptions.SelectedIndex = 0;
        this.textBoxMeterInfoDescription.Text = "New type from: " + DateTime.Now.ToString("dd.MM.yy HH:mm:ss");
        this.textBoxMeterTypeDescription.Text = this.textBoxMeterInfoDescription.Text;
      }
      else
      {
        this.Text = "Overwrite existing type";
        this.radioButtonHardwareFromType.Checked = true;
        num = this.MyFunctions.MyMeters.TypeMeter.MyIdentification.MeterInfoId;
        if (this.MyFunctions.MyMeters.TypeMeter.TypeOverrideString != null)
          this.textBoxParameterTypeUsing.Text = this.MyFunctions.MyMeters.TypeMeter.TypeOverrideString;
        if (this.MyFunctions.MyMeters.TypeMeter.TypeCreationString != null)
          this.textBoxTypeCreationString.Text = this.MyFunctions.MyMeters.TypeMeter.TypeCreationString;
        this.comboBoxSaveOptions.SelectedIndex = 1;
      }
      if (num == 0U)
      {
        this.textBoxMeterInfoId.Text = "Create new";
      }
      else
      {
        this.textBoxMeterInfoId.Text = num.ToString();
        try
        {
          string SQLCommand1 = "SELECT * FROM MeterInfo WHERE MeterInfoID = " + num.ToString();
          Schema.MeterInfoDataTable Table1 = new Schema.MeterInfoDataTable();
          this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand1, (DataTable) Table1);
          if (Table1.Count == 1)
          {
            Schema.MeterInfoRow meterInfoRow = Table1[0];
            this.textBoxMeterInfoDescription.Text = meterInfoRow.Description;
            if (meterInfoRow.IsPPSArtikelNrNull() || meterInfoRow.PPSArtikelNr == "")
              this.radioButtonTypeNotDefined.Checked = true;
            else if (meterInfoRow.PPSArtikelNr == SapNumberRequestValues.S3_BASETYPE.ToString())
              this.radioButtonTypeBase.Checked = true;
            else if (meterInfoRow.PPSArtikelNr == SapNumberRequestValues.PARAMETER_TYPE.ToString())
            {
              this.radioButtonParameterType.Checked = true;
            }
            else
            {
              this.radioButtonTypeSpecial.Checked = true;
              this.textBoxSAP_Number.Text = meterInfoRow.PPSArtikelNr;
            }
            string SQLCommand2 = "SELECT * FROM MeterType WHERE MeterTypeId = " + meterInfoRow.MeterTypeID.ToString();
            Schema.MeterTypeDataTable Table2 = new Schema.MeterTypeDataTable();
            this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand2, (DataTable) Table2);
            if (Table2.Count == 1)
              this.textBoxMeterTypeDescription.Text = Table2[0].Description;
          }
        }
        catch
        {
        }
      }
      if (this.MyFunctions.MyMeters.ConnectedMeter != null)
        return;
      this.radioButtonHardwareFromRead.Enabled = false;
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      string empty = string.Empty;
      ZR_ClassLibMessages.ClearErrors();
      if (this.textBoxMeterInfoDescription.Text.Trim().Length == 0)
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Please insert MeterInfoDescription");
      }
      else if (this.textBoxMeterTypeDescription.Text.Trim().Length == 0)
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Please insert textBoxMeterTypeDescription");
      }
      else
      {
        string text = this.textBoxSAP_Number.Text;
        uint meterInfoId = 0;
        if (!(this.textBoxMeterInfoId.Text == "Create new"))
          meterInfoId = uint.Parse(this.textBoxMeterInfoId.Text);
        SaveOptions selectedIndex = (SaveOptions) this.comboBoxSaveOptions.SelectedIndex;
        string meterInfoDescription = this.textBoxMeterInfoDescription.Text.Trim();
        string meterTypeDescription = this.textBoxMeterTypeDescription.Text.Trim();
        string typeOverrideString = this.textBoxParameterTypeUsing.Text.Trim();
        if (typeOverrideString.Length == 0)
          typeOverrideString = (string) null;
        try
        {
          string str1 = text;
          SapNumberRequestValues numberRequestValues = SapNumberRequestValues.S3_BASETYPE;
          string str2 = numberRequestValues.ToString();
          int num3;
          if (!(str1 == str2))
          {
            string str3 = text;
            numberRequestValues = SapNumberRequestValues.PARAMETER_TYPE;
            string str4 = numberRequestValues.ToString();
            num3 = str3 == str4 ? 1 : 0;
          }
          else
            num3 = 1;
          if (num3 == 0)
          {
            if (this.dataGridViewHardwareTypes.SelectedRows.Count != 1)
            {
              int num4 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Please select a hardware");
              return;
            }
            int NewValue = int.Parse(this.dataGridViewHardwareTypes.Rows[this.dataGridViewHardwareTypes.SelectedCells[0].RowIndex].Cells["HardwareTypeID"].Value.ToString());
            S3_Parameter s3Parameter = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName["Con_HardwareTypeId"];
            if (NewValue != s3Parameter.GetIntValue())
            {
              this.MyFunctions.MyMeters.NewWorkMeter("prepare for write type");
              S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
              workMeter.MyParameters.ParameterByName["Con_HardwareTypeId"].SetUlongValue((ulong) NewValue);
              workMeter.MyIdentification.LoadDeviceIdFromParameter();
              if (ZR_ClassLibMessages.GetLastError() != 0)
              {
                ZR_ClassLibMessages.ShowAndClearErrors();
                return;
              }
            }
          }
          bool flag;
          if (this.checkBoxChangeDeviceHardware.Checked)
          {
            DeviceHardwareIdentification hardwareIdentification = new DeviceHardwareIdentification();
            hardwareIdentification.VolumeMeterType = (DHI_VolumeMeterType) this.comboBoxVolumeMeterType.SelectedIndex;
            hardwareIdentification.VolumeMeterBounding = (DHI_VolumeMeterBounding) this.comboBoxVolumeMeterBounding.SelectedIndex;
            hardwareIdentification.CommunicationInterfaces.Add(DHI_CommunicationInterface.ZVEI);
            hardwareIdentification.CommunicationInterfaces.Add(DHI_CommunicationInterface.IrDA);
            switch (this.comboBoxCommunicationInterfaces.SelectedIndex)
            {
              case 1:
                hardwareIdentification.CommunicationInterfaces.Add(DHI_CommunicationInterface.MBus);
                break;
              case 2:
                hardwareIdentification.CommunicationInterfaces.Add(DHI_CommunicationInterface.radio3_WMBus_868);
                break;
              case 3:
                hardwareIdentification.CommunicationInterfaces.Add(DHI_CommunicationInterface.RS485);
                break;
            }
            flag = this.MyFunctions.MyMeters.SaveType(ref meterInfoId, text, hardwareIdentification, selectedIndex, meterInfoDescription, meterTypeDescription, typeOverrideString);
          }
          else
            flag = this.MyFunctions.MyMeters.SaveType(ref meterInfoId, text, (DeviceHardwareIdentification) null, selectedIndex, meterInfoDescription, meterTypeDescription, typeOverrideString);
          if (!flag)
          {
            ZR_ClassLibMessages.AddErrorDescription("Save type error");
          }
          else
          {
            int num5 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Type saved");
            this.Close();
          }
        }
        catch (Exception ex)
        {
          int num6 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Error on create type: " + ex.ToString());
          return;
        }
        ZR_ClassLibMessages.ShowAndClearErrors("S3_Handler");
      }
    }

    private void radioButtonTypeSpecial_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonTypeSpecial.Checked)
        return;
      this.textBoxSAP_Number.Text = this.MyFunctions.MyMeters.WorkMeter.MyIdentification.SAP_MaterialNumber.ToString();
      this.textBoxSAP_Number.Enabled = true;
      this.groupBoxHardwareIdentification.Enabled = true;
      this.SetHardware();
    }

    private void radioButtonTypeAuto_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonTypeAuto.Checked)
        return;
      this.textBoxSAP_Number.Text = SapNumberRequestValues.Auto.ToString();
      this.textBoxSAP_Number.Enabled = false;
      this.groupBoxHardwareIdentification.Enabled = true;
      this.SetHardware();
    }

    private void radioButtonTypeNotDefined_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonTypeNotDefined.Checked)
        return;
      this.textBoxSAP_Number.Text = SapNumberRequestValues.NotDefined.ToString();
      this.textBoxSAP_Number.Enabled = false;
      this.groupBoxHardwareIdentification.Enabled = true;
      this.SetHardware();
    }

    private void radioButtonTypeBase_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonTypeBase.Checked)
        return;
      this.textBoxSAP_Number.Text = SapNumberRequestValues.S3_BASETYPE.ToString();
      this.textBoxSAP_Number.Enabled = false;
      this.groupBoxHardwareIdentification.Enabled = true;
      this.SetHardware();
    }

    private void radioButtonParameterType_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonParameterType.Checked)
        return;
      this.textBoxSAP_Number.Text = SapNumberRequestValues.PARAMETER_TYPE.ToString();
      this.textBoxSAP_Number.Enabled = false;
      this.groupBoxHardwareIdentification.Enabled = true;
      this.SetHardware();
    }

    private void radioButtonHardwareFromWork_CheckedChanged(object sender, EventArgs e)
    {
      this.SetHardware();
    }

    private void radioButtonHardwareFromRead_CheckedChanged(object sender, EventArgs e)
    {
      this.SetHardware();
    }

    private void radioButtonHardwareFromType_CheckedChanged(object sender, EventArgs e)
    {
      this.SetHardware();
    }

    private void radioButtonHardwareManual_CheckedChanged(object sender, EventArgs e)
    {
      this.SetHardware();
    }

    private void SetHardware()
    {
      if (this.radioButtonHardwareFromWork.Checked)
      {
        this.dataGridViewHardwareTypes.Enabled = false;
        this.SelectHardwareTypeId(this.MyFunctions.MyMeters.WorkMeter.MyIdentification.HardwareTypeId);
      }
      else if (this.radioButtonHardwareFromType.Checked)
      {
        this.dataGridViewHardwareTypes.Enabled = false;
        this.SelectHardwareTypeId(this.MyFunctions.MyMeters.TypeMeter.MyIdentification.HardwareTypeId);
      }
      else if (this.radioButtonHardwareFromRead.Checked)
      {
        this.dataGridViewHardwareTypes.Enabled = false;
        this.SelectHardwareTypeId(this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.HardwareTypeId);
      }
      else
      {
        if (!this.radioButtonHardwareManual.Checked)
          return;
        this.dataGridViewHardwareTypes.Enabled = true;
        this.SelectHardwareTypeId(this.MyFunctions.MyMeters.WorkMeter.MyIdentification.HardwareTypeId);
      }
    }

    private void SelectHardwareTypeId(uint HardwareTypeId)
    {
      if (HardwareTypeId == 0U)
        HardwareTypeId = this.MyFunctions.MyMeters.WorkMeter.MyIdentification.HardwareTypeId;
      this.dataGridViewHardwareTypes.ClearSelection();
      for (int index = 0; index < this.dataGridViewHardwareTypes.Rows.Count; ++index)
      {
        uint num = uint.Parse(this.dataGridViewHardwareTypes.Rows[index].Cells["HardwareTypeID"].Value.ToString());
        if ((int) HardwareTypeId == (int) num)
        {
          this.dataGridViewHardwareTypes.Rows[index].Selected = true;
          return;
        }
      }
      this.textBoxHardwareTypeId.Text = HardwareTypeId.ToString();
    }

    private void dataGridViewHardwareTypes_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void dataGridViewHardwareTypes_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dataGridViewHardwareTypes.SelectedRows.Count < 1)
        this.textBoxHardwareTypeId.Clear();
      else
        this.textBoxHardwareTypeId.Text = this.dataGridViewHardwareTypes.Rows[this.dataGridViewHardwareTypes.SelectedCells[0].RowIndex].Cells["HardwareTypeID"].Value.ToString();
    }

    private void radioButtonNewType_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void checkBoxNewType_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxNewType.Checked)
      {
        this.textBoxMeterInfoId.Enabled = false;
        this.textBoxMeterInfoId.Text = "Create new";
      }
      else if (this.MyFunctions.MyMeters.TypeMeter != null)
      {
        this.textBoxMeterInfoId.Text = this.MyFunctions.MyMeters.TypeMeter.MyIdentification.MeterInfoId.ToString();
        this.textBoxMeterInfoId.Enabled = false;
      }
      else
      {
        this.textBoxMeterInfoId.Text = this.MyFunctions.MyMeters.WorkMeter.MyIdentification.MeterInfoId.ToString();
        this.textBoxMeterInfoId.Enabled = true;
      }
    }

    private void checkBoxChangeDeviceHardware_CheckedChanged(object sender, EventArgs e)
    {
      bool flag = this.checkBoxChangeDeviceHardware.Checked;
      this.comboBoxVolumeMeterType.Enabled = flag;
      this.comboBoxVolumeMeterBounding.Enabled = flag;
      this.comboBoxCommunicationInterfaces.Enabled = flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.dataGridViewHardwareTypes = new DataGridView();
      this.buttonOk = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.textBoxMeterInfoDescription = new TextBox();
      this.label3 = new Label();
      this.textBoxMeterTypeDescription = new TextBox();
      this.textBoxMeterInfoId = new TextBox();
      this.textBoxSAP_Number = new TextBox();
      this.label6 = new Label();
      this.comboBoxSaveOptions = new ComboBox();
      this.groupBoxType = new GroupBox();
      this.radioButtonTypeAuto = new RadioButton();
      this.radioButtonTypeNotDefined = new RadioButton();
      this.radioButtonParameterType = new RadioButton();
      this.radioButtonTypeBase = new RadioButton();
      this.radioButtonTypeSpecial = new RadioButton();
      this.groupBoxMeterInfo = new GroupBox();
      this.checkBoxNewType = new CheckBox();
      this.groupBoxHardwareIdentification = new GroupBox();
      this.radioButtonHardwareFromWork = new RadioButton();
      this.radioButtonHardwareFromRead = new RadioButton();
      this.radioButtonHardwareFromType = new RadioButton();
      this.textBoxHardwareTypeId = new TextBox();
      this.radioButtonHardwareManual = new RadioButton();
      this.groupBoxDeviceHardware = new GroupBox();
      this.comboBoxCommunicationInterfaces = new ComboBox();
      this.comboBoxVolumeMeterBounding = new ComboBox();
      this.comboBoxVolumeMeterType = new ComboBox();
      this.checkBoxChangeDeviceHardware = new CheckBox();
      this.label4 = new Label();
      this.textBoxParameterTypeUsing = new TextBox();
      this.label5 = new Label();
      this.textBoxTypeCreationString = new TextBox();
      ((ISupportInitialize) this.dataGridViewHardwareTypes).BeginInit();
      this.groupBoxType.SuspendLayout();
      this.groupBoxMeterInfo.SuspendLayout();
      this.groupBoxHardwareIdentification.SuspendLayout();
      this.groupBoxDeviceHardware.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(937, 105);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.dataGridViewHardwareTypes.AllowUserToAddRows = false;
      this.dataGridViewHardwareTypes.AllowUserToDeleteRows = false;
      this.dataGridViewHardwareTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewHardwareTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewHardwareTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewHardwareTypes.Location = new Point(12, 65);
      this.dataGridViewHardwareTypes.MultiSelect = false;
      this.dataGridViewHardwareTypes.Name = "dataGridViewHardwareTypes";
      this.dataGridViewHardwareTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewHardwareTypes.ShowEditingIcon = false;
      this.dataGridViewHardwareTypes.Size = new Size(912, 239);
      this.dataGridViewHardwareTypes.TabIndex = 17;
      this.dataGridViewHardwareTypes.RowEnter += new DataGridViewCellEventHandler(this.dataGridViewHardwareTypes_RowEnter);
      this.dataGridViewHardwareTypes.SelectionChanged += new System.EventHandler(this.dataGridViewHardwareTypes_SelectionChanged);
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.Location = new Point(791, 538);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(134, 33);
      this.buttonOk.TabIndex = 18;
      this.buttonOk.Text = "Save type";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 49);
      this.label1.Name = "label1";
      this.label1.Size = new Size(84, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "Select hardware";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 316);
      this.label2.Name = "label2";
      this.label2.Size = new Size(108, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "MeterInfo Description";
      this.textBoxMeterInfoDescription.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxMeterInfoDescription.Location = new Point(134, 313);
      this.textBoxMeterInfoDescription.Name = "textBoxMeterInfoDescription";
      this.textBoxMeterInfoDescription.Size = new Size(791, 20);
      this.textBoxMeterInfoDescription.TabIndex = 20;
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(13, 342);
      this.label3.Name = "label3";
      this.label3.Size = new Size(114, 13);
      this.label3.TabIndex = 19;
      this.label3.Text = "MeterType Description";
      this.textBoxMeterTypeDescription.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxMeterTypeDescription.Location = new Point(134, 339);
      this.textBoxMeterTypeDescription.Name = "textBoxMeterTypeDescription";
      this.textBoxMeterTypeDescription.Size = new Size(791, 20);
      this.textBoxMeterTypeDescription.TabIndex = 20;
      this.textBoxMeterInfoId.Enabled = false;
      this.textBoxMeterInfoId.Location = new Point(6, 20);
      this.textBoxMeterInfoId.Name = "textBoxMeterInfoId";
      this.textBoxMeterInfoId.Size = new Size(142, 20);
      this.textBoxMeterInfoId.TabIndex = 22;
      this.textBoxSAP_Number.Location = new Point(6, 20);
      this.textBoxSAP_Number.Name = "textBoxSAP_Number";
      this.textBoxSAP_Number.Size = new Size(137, 20);
      this.textBoxSAP_Number.TabIndex = 22;
      this.label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(749, 435);
      this.label6.Name = "label6";
      this.label6.Size = new Size(69, 13);
      this.label6.TabIndex = 19;
      this.label6.Text = "Save options";
      this.comboBoxSaveOptions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.comboBoxSaveOptions.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxSaveOptions.FormattingEnabled = true;
      this.comboBoxSaveOptions.Location = new Point(752, 449);
      this.comboBoxSaveOptions.Name = "comboBoxSaveOptions";
      this.comboBoxSaveOptions.Size = new Size(170, 21);
      this.comboBoxSaveOptions.TabIndex = 23;
      this.groupBoxType.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxType.Controls.Add((Control) this.radioButtonTypeAuto);
      this.groupBoxType.Controls.Add((Control) this.radioButtonTypeNotDefined);
      this.groupBoxType.Controls.Add((Control) this.radioButtonParameterType);
      this.groupBoxType.Controls.Add((Control) this.radioButtonTypeBase);
      this.groupBoxType.Controls.Add((Control) this.radioButtonTypeSpecial);
      this.groupBoxType.Controls.Add((Control) this.textBoxSAP_Number);
      this.groupBoxType.Location = new Point(172, 429);
      this.groupBoxType.Name = "groupBoxType";
      this.groupBoxType.Size = new Size(154, 145);
      this.groupBoxType.TabIndex = 24;
      this.groupBoxType.TabStop = false;
      this.groupBoxType.Text = "Type (SAP number)";
      this.radioButtonTypeAuto.AutoSize = true;
      this.radioButtonTypeAuto.Location = new Point(6, 63);
      this.radioButtonTypeAuto.Name = "radioButtonTypeAuto";
      this.radioButtonTypeAuto.Size = new Size(47, 17);
      this.radioButtonTypeAuto.TabIndex = 23;
      this.radioButtonTypeAuto.TabStop = true;
      this.radioButtonTypeAuto.Text = "Auto";
      this.radioButtonTypeAuto.UseVisualStyleBackColor = true;
      this.radioButtonTypeAuto.CheckedChanged += new System.EventHandler(this.radioButtonTypeAuto_CheckedChanged);
      this.radioButtonTypeNotDefined.AutoSize = true;
      this.radioButtonTypeNotDefined.Location = new Point(6, 81);
      this.radioButtonTypeNotDefined.Name = "radioButtonTypeNotDefined";
      this.radioButtonTypeNotDefined.Size = new Size(98, 17);
      this.radioButtonTypeNotDefined.TabIndex = 23;
      this.radioButtonTypeNotDefined.TabStop = true;
      this.radioButtonTypeNotDefined.Text = "Not defined = 0";
      this.radioButtonTypeNotDefined.UseVisualStyleBackColor = true;
      this.radioButtonTypeNotDefined.CheckedChanged += new System.EventHandler(this.radioButtonTypeNotDefined_CheckedChanged);
      this.radioButtonParameterType.AutoSize = true;
      this.radioButtonParameterType.Location = new Point(6, 117);
      this.radioButtonParameterType.Name = "radioButtonParameterType";
      this.radioButtonParameterType.Size = new Size(96, 17);
      this.radioButtonParameterType.TabIndex = 23;
      this.radioButtonParameterType.TabStop = true;
      this.radioButtonParameterType.Text = "Parameter type";
      this.radioButtonParameterType.UseVisualStyleBackColor = true;
      this.radioButtonParameterType.CheckedChanged += new System.EventHandler(this.radioButtonParameterType_CheckedChanged);
      this.radioButtonTypeBase.AutoSize = true;
      this.radioButtonTypeBase.Location = new Point(6, 99);
      this.radioButtonTypeBase.Name = "radioButtonTypeBase";
      this.radioButtonTypeBase.Size = new Size(72, 17);
      this.radioButtonTypeBase.TabIndex = 23;
      this.radioButtonTypeBase.TabStop = true;
      this.radioButtonTypeBase.Text = "Base type";
      this.radioButtonTypeBase.UseVisualStyleBackColor = true;
      this.radioButtonTypeBase.CheckedChanged += new System.EventHandler(this.radioButtonTypeBase_CheckedChanged);
      this.radioButtonTypeSpecial.AutoSize = true;
      this.radioButtonTypeSpecial.Location = new Point(6, 45);
      this.radioButtonTypeSpecial.Name = "radioButtonTypeSpecial";
      this.radioButtonTypeSpecial.Size = new Size(83, 17);
      this.radioButtonTypeSpecial.TabIndex = 23;
      this.radioButtonTypeSpecial.TabStop = true;
      this.radioButtonTypeSpecial.Text = "Special type";
      this.radioButtonTypeSpecial.UseVisualStyleBackColor = true;
      this.radioButtonTypeSpecial.CheckedChanged += new System.EventHandler(this.radioButtonTypeSpecial_CheckedChanged);
      this.groupBoxMeterInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxMeterInfo.Controls.Add((Control) this.checkBoxNewType);
      this.groupBoxMeterInfo.Controls.Add((Control) this.textBoxMeterInfoId);
      this.groupBoxMeterInfo.Location = new Point(10, 429);
      this.groupBoxMeterInfo.Name = "groupBoxMeterInfo";
      this.groupBoxMeterInfo.Size = new Size(154, 145);
      this.groupBoxMeterInfo.TabIndex = 24;
      this.groupBoxMeterInfo.TabStop = false;
      this.groupBoxMeterInfo.Text = "MeterinfoId";
      this.checkBoxNewType.AutoSize = true;
      this.checkBoxNewType.Location = new Point(6, 48);
      this.checkBoxNewType.Name = "checkBoxNewType";
      this.checkBoxNewType.Size = new Size(71, 17);
      this.checkBoxNewType.TabIndex = 23;
      this.checkBoxNewType.Text = "New type";
      this.checkBoxNewType.UseVisualStyleBackColor = true;
      this.checkBoxNewType.CheckedChanged += new System.EventHandler(this.checkBoxNewType_CheckedChanged);
      this.groupBoxHardwareIdentification.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxHardwareIdentification.Controls.Add((Control) this.radioButtonHardwareFromWork);
      this.groupBoxHardwareIdentification.Controls.Add((Control) this.radioButtonHardwareFromRead);
      this.groupBoxHardwareIdentification.Controls.Add((Control) this.radioButtonHardwareFromType);
      this.groupBoxHardwareIdentification.Controls.Add((Control) this.textBoxHardwareTypeId);
      this.groupBoxHardwareIdentification.Controls.Add((Control) this.radioButtonHardwareManual);
      this.groupBoxHardwareIdentification.Location = new Point(332, 429);
      this.groupBoxHardwareIdentification.Name = "groupBoxHardwareIdentification";
      this.groupBoxHardwareIdentification.Size = new Size(154, 145);
      this.groupBoxHardwareIdentification.TabIndex = 24;
      this.groupBoxHardwareIdentification.TabStop = false;
      this.groupBoxHardwareIdentification.Text = "HardwareTypeId";
      this.radioButtonHardwareFromWork.AutoSize = true;
      this.radioButtonHardwareFromWork.Location = new Point(6, 106);
      this.radioButtonHardwareFromWork.Name = "radioButtonHardwareFromWork";
      this.radioButtonHardwareFromWork.Size = new Size(104, 17);
      this.radioButtonHardwareFromWork.TabIndex = 23;
      this.radioButtonHardwareFromWork.TabStop = true;
      this.radioButtonHardwareFromWork.Text = "From WorkMeter";
      this.radioButtonHardwareFromWork.UseVisualStyleBackColor = true;
      this.radioButtonHardwareFromWork.CheckedChanged += new System.EventHandler(this.radioButtonHardwareFromWork_CheckedChanged);
      this.radioButtonHardwareFromRead.AutoSize = true;
      this.radioButtonHardwareFromRead.Location = new Point(6, 86);
      this.radioButtonHardwareFromRead.Name = "radioButtonHardwareFromRead";
      this.radioButtonHardwareFromRead.Size = new Size(104, 17);
      this.radioButtonHardwareFromRead.TabIndex = 23;
      this.radioButtonHardwareFromRead.TabStop = true;
      this.radioButtonHardwareFromRead.Text = "From ReadMeter";
      this.radioButtonHardwareFromRead.UseVisualStyleBackColor = true;
      this.radioButtonHardwareFromRead.CheckedChanged += new System.EventHandler(this.radioButtonHardwareFromRead_CheckedChanged);
      this.radioButtonHardwareFromType.AutoSize = true;
      this.radioButtonHardwareFromType.Location = new Point(6, 66);
      this.radioButtonHardwareFromType.Name = "radioButtonHardwareFromType";
      this.radioButtonHardwareFromType.Size = new Size(71, 17);
      this.radioButtonHardwareFromType.TabIndex = 23;
      this.radioButtonHardwareFromType.TabStop = true;
      this.radioButtonHardwareFromType.Text = "From type";
      this.radioButtonHardwareFromType.UseVisualStyleBackColor = true;
      this.radioButtonHardwareFromType.CheckedChanged += new System.EventHandler(this.radioButtonHardwareFromType_CheckedChanged);
      this.textBoxHardwareTypeId.Enabled = false;
      this.textBoxHardwareTypeId.Location = new Point(6, 19);
      this.textBoxHardwareTypeId.Name = "textBoxHardwareTypeId";
      this.textBoxHardwareTypeId.Size = new Size(137, 20);
      this.textBoxHardwareTypeId.TabIndex = 22;
      this.radioButtonHardwareManual.AutoSize = true;
      this.radioButtonHardwareManual.Location = new Point(6, 46);
      this.radioButtonHardwareManual.Name = "radioButtonHardwareManual";
      this.radioButtonHardwareManual.Size = new Size(62, 17);
      this.radioButtonHardwareManual.TabIndex = 23;
      this.radioButtonHardwareManual.TabStop = true;
      this.radioButtonHardwareManual.Text = "Manuell";
      this.radioButtonHardwareManual.UseVisualStyleBackColor = true;
      this.radioButtonHardwareManual.CheckedChanged += new System.EventHandler(this.radioButtonHardwareManual_CheckedChanged);
      this.groupBoxDeviceHardware.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxDeviceHardware.Controls.Add((Control) this.comboBoxCommunicationInterfaces);
      this.groupBoxDeviceHardware.Controls.Add((Control) this.comboBoxVolumeMeterBounding);
      this.groupBoxDeviceHardware.Controls.Add((Control) this.comboBoxVolumeMeterType);
      this.groupBoxDeviceHardware.Controls.Add((Control) this.checkBoxChangeDeviceHardware);
      this.groupBoxDeviceHardware.Location = new Point(492, 429);
      this.groupBoxDeviceHardware.Name = "groupBoxDeviceHardware";
      this.groupBoxDeviceHardware.Size = new Size(200, 142);
      this.groupBoxDeviceHardware.TabIndex = 25;
      this.groupBoxDeviceHardware.TabStop = false;
      this.groupBoxDeviceHardware.Text = "Device hardware";
      this.comboBoxCommunicationInterfaces.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxCommunicationInterfaces.Enabled = false;
      this.comboBoxCommunicationInterfaces.FormattingEnabled = true;
      this.comboBoxCommunicationInterfaces.Location = new Point(6, 98);
      this.comboBoxCommunicationInterfaces.Name = "comboBoxCommunicationInterfaces";
      this.comboBoxCommunicationInterfaces.Size = new Size(188, 21);
      this.comboBoxCommunicationInterfaces.TabIndex = 1;
      this.comboBoxVolumeMeterBounding.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxVolumeMeterBounding.Enabled = false;
      this.comboBoxVolumeMeterBounding.FormattingEnabled = true;
      this.comboBoxVolumeMeterBounding.Location = new Point(6, 71);
      this.comboBoxVolumeMeterBounding.Name = "comboBoxVolumeMeterBounding";
      this.comboBoxVolumeMeterBounding.Size = new Size(188, 21);
      this.comboBoxVolumeMeterBounding.TabIndex = 1;
      this.comboBoxVolumeMeterType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxVolumeMeterType.Enabled = false;
      this.comboBoxVolumeMeterType.FormattingEnabled = true;
      this.comboBoxVolumeMeterType.Location = new Point(6, 44);
      this.comboBoxVolumeMeterType.Name = "comboBoxVolumeMeterType";
      this.comboBoxVolumeMeterType.Size = new Size(188, 21);
      this.comboBoxVolumeMeterType.TabIndex = 1;
      this.checkBoxChangeDeviceHardware.AutoSize = true;
      this.checkBoxChangeDeviceHardware.Location = new Point(6, 20);
      this.checkBoxChangeDeviceHardware.Name = "checkBoxChangeDeviceHardware";
      this.checkBoxChangeDeviceHardware.Size = new Size(110, 17);
      this.checkBoxChangeDeviceHardware.TabIndex = 0;
      this.checkBoxChangeDeviceHardware.Text = "Change hardware";
      this.checkBoxChangeDeviceHardware.UseVisualStyleBackColor = true;
      this.checkBoxChangeDeviceHardware.CheckedChanged += new System.EventHandler(this.checkBoxChangeDeviceHardware_CheckedChanged);
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(13, 368);
      this.label4.Name = "label4";
      this.label4.Size = new Size(106, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "Parameter type using";
      this.textBoxParameterTypeUsing.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxParameterTypeUsing.Location = new Point(134, 365);
      this.textBoxParameterTypeUsing.Name = "textBoxParameterTypeUsing";
      this.textBoxParameterTypeUsing.Size = new Size(791, 20);
      this.textBoxParameterTypeUsing.TabIndex = 20;
      this.label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(13, 394);
      this.label5.Name = "label5";
      this.label5.Size = new Size(97, 13);
      this.label5.TabIndex = 19;
      this.label5.Text = "TypeCreationString";
      this.textBoxTypeCreationString.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxTypeCreationString.Location = new Point(134, 391);
      this.textBoxTypeCreationString.Name = "textBoxTypeCreationString";
      this.textBoxTypeCreationString.Size = new Size(791, 20);
      this.textBoxTypeCreationString.TabIndex = 20;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(937, 583);
      this.Controls.Add((Control) this.groupBoxDeviceHardware);
      this.Controls.Add((Control) this.groupBoxHardwareIdentification);
      this.Controls.Add((Control) this.groupBoxMeterInfo);
      this.Controls.Add((Control) this.groupBoxType);
      this.Controls.Add((Control) this.comboBoxSaveOptions);
      this.Controls.Add((Control) this.textBoxTypeCreationString);
      this.Controls.Add((Control) this.textBoxParameterTypeUsing);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.textBoxMeterTypeDescription);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textBoxMeterInfoDescription);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.dataGridViewHardwareTypes);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (SaveType);
      this.Text = "Save type";
      this.Load += new System.EventHandler(this.SaveType_Load);
      ((ISupportInitialize) this.dataGridViewHardwareTypes).EndInit();
      this.groupBoxType.ResumeLayout(false);
      this.groupBoxType.PerformLayout();
      this.groupBoxMeterInfo.ResumeLayout(false);
      this.groupBoxMeterInfo.PerformLayout();
      this.groupBoxHardwareIdentification.ResumeLayout(false);
      this.groupBoxHardwareIdentification.PerformLayout();
      this.groupBoxDeviceHardware.ResumeLayout(false);
      this.groupBoxDeviceHardware.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum HardwareTableRows
    {
      HardwareTypeId,
      HardwareType,
      FirmwareVersion,
      HardwareResources,
    }
  }
}
