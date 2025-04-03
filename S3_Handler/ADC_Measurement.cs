// Decompiled with JetBrains decompiler
// Type: S3_Handler.ADC_Measurement
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class ADC_Measurement : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private int TestStartTextOffset;
    private bool stopRunningTest;
    private DataTable adcValuesTable;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonReadAdcValues;
    private SplitContainer splitContainer1;
    private TextBox textBoxInfos;
    private DataGridView dataGridViewInfos;
    private GroupBox groupBoxVolumeSimulationValue;
    private TextBox textBoxVolumeSimulation;
    private Button buttonSendAdcTestVolume;
    private Button buttonSendAdcTestDone;
    private Button buttonSendAdcTestActivate;
    private GroupBox groupBoxCalibration;
    private Label label4;
    private Label label3;
    private Label label2;
    private TextBox textBoxReturnTemp;
    private TextBox textBoxFlowTemp;
    private TextBox textBoxLoops;
    private Button buttonRunCalibration;
    private Button buttonClearData;
    private ComboBox comboBoxCalibrationStep;
    private Label label5;
    private Button buttonRunCalibrationStep;
    private Button buttonBreakLoop;
    private Label label6;
    private Button buttonTestDoneCool;
    private Button buttonSetTestDoneSetTest;
    private Button buttonSetTestDoneSleep;
    private Button buttonReadDeviceValues;

    public ADC_Measurement(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      DeviceMemory deviceMemory = this.MyFunctions.MyMeters.ConnectedMeter.MyDeviceMemory;
      this.InitializeComponent();
      if (this.MyFunctions.adc_Calibration == null)
        this.MyFunctions.adc_Calibration = new ADC_Calibration(MyFunctions);
      this.comboBoxCalibrationStep.DataSource = (object) Enum.GetNames(typeof (ADC_CalibrationSteps));
      this.comboBoxCalibrationStep.SelectedIndex = 0;
      this.adcValuesTable = new DataTable(nameof (adcValuesTable));
      this.adcValuesTable.Columns.Add(new DataColumn("Time", typeof (DateTime)));
      this.adcValuesTable.Columns.Add(new DataColumn("VL", typeof (int)));
      this.adcValuesTable.Columns.Add(new DataColumn("RL", typeof (int)));
      this.adcValuesTable.Columns.Add(new DataColumn("REF1", typeof (int)));
      this.adcValuesTable.Columns.Add(new DataColumn("REF2", typeof (int)));
      this.adcValuesTable.Columns.Add(new DataColumn("BATT", typeof (int)));
      this.adcValuesTable.Columns.Add(new DataColumn("Kal. TF", typeof (float)));
      this.adcValuesTable.Columns.Add(new DataColumn("Kal. TR", typeof (float)));
      this.dataGridViewInfos.DataSource = (object) this.adcValuesTable;
      this.dataGridViewInfos.Columns["Time"].CellTemplate.Style.Format = "HH:mm:ss";
      if (!UserManager.CheckPermission(UserManager.Role_Developer))
      {
        this.groupBoxCalibration.Visible = false;
        this.dataGridViewInfos.Visible = false;
        this.splitContainer1.SplitterDistance = this.splitContainer1.Size.Height - 35;
        this.buttonReadAdcValues.Visible = false;
      }
      else
      {
        this.AddAdcValuesToTable();
        this.ShowAdcValues();
      }
    }

    private void buttonReadAdcValues_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.LocalTestFunction);
      this.textBoxInfos.AppendText("Read ADC Values: ");
      bool ok = this.MyFunctions.adc_Calibration.ReadAdcValues();
      if (ok)
      {
        this.ShowAdcValues();
        this.AddAdcValuesToTable();
      }
      this.RegisterOkErr(ok);
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.LocalTestFunction);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonReadDeviceValues_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.LocalTestFunction);
      this.MyFunctions.MyMeters.RefreshDynamicParameterFromRAM();
      this.textBoxInfos.AppendText(Environment.NewLine + "--- Device Values ---" + Environment.NewLine);
      this.textBoxInfos.AppendText(this.MyFunctions.MyMeters.WorkMeter.GetMeterValuesOverview() + Environment.NewLine);
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.LocalTestFunction);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void ShowAdcValues()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("ADC Values at " + DateTime.Now.ToString("HH:mm:ss") + " : ");
      foreach (KeyValuePair<string, S3_Parameter> keyValuePair in this.MyFunctions.adc_Calibration.adc_Parameter)
        stringBuilder.AppendLine(keyValuePair.Key + ": " + keyValuePair.Value.GetDisplayString());
      this.textBoxInfos.AppendText(stringBuilder.ToString());
    }

    private void AddAdcValuesToTable()
    {
      ADC_Values adcValues = this.MyFunctions.adc_Calibration.adc_Values;
      DataRow row = this.adcValuesTable.NewRow();
      row["Time"] = (object) DateTime.Now;
      row["VL"] = (object) adcValues.Adc_Counter_Value_VL;
      row["RL"] = (object) adcValues.Adc_Counter_Value_RL;
      row["REF1"] = (object) adcValues.Adc_Counter_Value_Ref1;
      row["REF2"] = (object) adcValues.Adc_Counter_Value_Ref2;
      row["BATT"] = (object) adcValues.Adc_Counter_V_Batt;
      row["Kal. TF"] = (object) adcValues.flowTemp;
      row["Kal. TR"] = (object) adcValues.returnTemp;
      this.adcValuesTable.Rows.Add(row);
      int index = this.dataGridViewInfos.Rows.Count - 1;
      this.dataGridViewInfos.CurrentCell = this.dataGridViewInfos.Rows[index].Cells[0];
      this.dataGridViewInfos.ClearSelection();
      this.dataGridViewInfos.Rows[index].Selected = true;
    }

    private void buttonSendAdcTestActivate_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxInfos.AppendText("Send ADC test activate: ");
      this.RegisterOkErr(this.MyFunctions.SetTestMode(DeviceTestMode.Temperature_Tests));
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonSendAdcTestVolume_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.LocalTestFunction);
      Decimal simulatedVolumeInQm = this.GetSimulatedVolumeInQm();
      if (simulatedVolumeInQm != Decimal.MinValue)
      {
        this.textBoxInfos.AppendText("Send volume " + simulatedVolumeInQm.ToString() + "m\u00B3; result: ");
        this.RegisterOkErr(this.MyFunctions.RunEnergyCycle(simulatedVolumeInQm));
      }
      else
      {
        int num = (int) GMM_MessageBox.ShowMessage("Send volume", "Volume wong -> Pleas set volume", true);
      }
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.LocalTestFunction);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonSendAdcTestDone_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxInfos.AppendText("Send test done: ");
      this.RegisterOkErr(this.MyFunctions.SetTestModeOff(272769346L));
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonTestDoneCool_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxInfos.AppendText("Send test done (LCD Cool): ");
      this.RegisterOkErr(this.MyFunctions.SetTestModeOff(272769355L));
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonSetTestDoneSegTest_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxInfos.AppendText("Send test done (LCD SegmentTest): ");
      this.RegisterOkErr(this.MyFunctions.SetTestModeOff(0L));
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonSetTestDoneSleep_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxInfos.AppendText("Send test done (Sleep): ");
      this.RegisterOkErr(this.MyFunctions.SetTestModeOff(-1L));
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonRunCalibrationStep_Click(object sender, EventArgs e)
    {
      this.RunCalibrationStep();
    }

    private bool RunCalibrationStep()
    {
      ZR_ClassLibMessages.ClearErrors();
      int loops = this.GetLoops();
      float flowTemperature = this.GetFlowTemperature();
      float returnTemperature = this.GetReturnTemperature();
      ADC_CalibrationSteps calibrationStep = (ADC_CalibrationSteps) Enum.Parse(typeof (ADC_CalibrationSteps), this.comboBoxCalibrationStep.SelectedItem.ToString());
      if ((double) flowTemperature != double.NaN && (double) returnTemperature != double.NaN && loops > 0)
      {
        this.MyFunctions.OnS3_HandlerMessage += new EventHandler<GMM_EventArgs>(this.S3_HandlerMessage);
        this.textBoxInfos.AppendText("Start ADC calibration." + ZR_Constants.SystemNewLine);
        this.TestStartTextOffset = this.textBoxInfos.Text.Length;
        this.stopRunningTest = false;
        bool ok = this.MyFunctions.RunAdcCalibration(calibrationStep, flowTemperature, returnTemperature, loops);
        this.textBoxInfos.AppendText("ADC calibration finisched. Result: ");
        this.RegisterOkErr(ok);
        if (this.comboBoxCalibrationStep.SelectedIndex < this.comboBoxCalibrationStep.Items.Count - 1)
          ++this.comboBoxCalibrationStep.SelectedIndex;
        else
          this.comboBoxCalibrationStep.SelectedIndex = 0;
        this.textBoxFlowTemp.Focus();
        this.MyFunctions.OnS3_HandlerMessage -= new EventHandler<GMM_EventArgs>(this.S3_HandlerMessage);
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
      return true;
    }

    private void buttonRunCalibration_Click(object sender, EventArgs e)
    {
      this.stopRunningTest = false;
      int num = 0;
      while (!this.stopRunningTest)
      {
        if (!this.MyFunctions.RunAdcCalibration(ADC_CalibrationSteps.StartCalibration, 52f, 1f, 3))
        {
          this.textBoxInfos.Text = "Error";
          break;
        }
        Application.DoEvents();
        Thread.Sleep(2000);
        if (!this.MyFunctions.RunAdcCalibration(ADC_CalibrationSteps.SecondPoint, 52f, 1f, 3))
        {
          this.textBoxInfos.Text = "Error";
          break;
        }
        Application.DoEvents();
        Thread.Sleep(2000);
        if (!this.MyFunctions.RunAdcCalibration(ADC_CalibrationSteps.EndCalibration, 52f, 1f, 3))
        {
          this.textBoxInfos.Text = "Error";
          break;
        }
        Application.DoEvents();
        Thread.Sleep(2000);
        this.textBoxInfos.Text = num.ToString();
        ++num;
      }
    }

    private void S3_HandlerMessage(object sender, GMM_EventArgs TheMessage)
    {
      if (TheMessage.TheMessageType != GMM_EventArgs.MessageType.TestStepDone)
        return;
      this.AddAdcValuesToTable();
      this.textBoxInfos.SelectionStart = this.TestStartTextOffset;
      this.textBoxInfos.SelectionLength = 1000;
      this.textBoxInfos.Cut();
      this.textBoxInfos.AppendText("Test step: " + TheMessage.InfoNumber.ToString() + ZR_Constants.SystemNewLine);
      if (this.stopRunningTest)
        TheMessage.Cancel = true;
    }

    private Decimal GetSimulatedVolumeInQm()
    {
      Decimal result;
      if (!Decimal.TryParse(this.textBoxVolumeSimulation.Text, out result) || result <= 0M)
      {
        this.textBoxVolumeSimulation.Text = 10.1M.ToString();
        this.textBoxVolumeSimulation.BackColor = Color.Yellow;
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal simulated volume");
        return Decimal.MinValue;
      }
      Decimal simulatedVolumeInQm = result / 1000M;
      this.textBoxVolumeSimulation.BackColor = SystemColors.Window;
      return simulatedVolumeInQm;
    }

    private float GetFlowTemperature()
    {
      float result;
      if (!float.TryParse(this.textBoxFlowTemp.Text, out result))
      {
        this.textBoxFlowTemp.Text = 43.23f.ToString();
        this.textBoxFlowTemp.BackColor = Color.Yellow;
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal flow temperature");
        return float.NaN;
      }
      this.textBoxFlowTemp.BackColor = SystemColors.Window;
      this.textBoxFlowTemp.SelectAll();
      return result;
    }

    private float GetReturnTemperature()
    {
      float result;
      if (!float.TryParse(this.textBoxReturnTemp.Text, out result))
      {
        this.textBoxReturnTemp.Text = 40.12f.ToString();
        this.textBoxReturnTemp.BackColor = Color.Yellow;
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal return temperature");
        return float.NaN;
      }
      this.textBoxReturnTemp.BackColor = SystemColors.Window;
      this.textBoxReturnTemp.SelectAll();
      return result;
    }

    private int GetLoops()
    {
      int result;
      if (!int.TryParse(this.textBoxLoops.Text, out result))
      {
        this.textBoxLoops.Text = 3.ToString();
        this.textBoxLoops.BackColor = Color.Yellow;
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal number of test loops");
        return 0;
      }
      this.textBoxLoops.BackColor = SystemColors.Window;
      return result;
    }

    private void RegisterOkErr(bool ok)
    {
      if (ok)
        this.textBoxInfos.AppendText("OK" + Environment.NewLine);
      else
        this.textBoxInfos.AppendText("Error" + Environment.NewLine);
    }

    private void buttonClearData_Click(object sender, EventArgs e)
    {
      this.textBoxInfos.Clear();
      this.adcValuesTable.Rows.Clear();
    }

    private void buttonBreakLoop_Click(object sender, EventArgs e) => this.stopRunningTest = true;

    private void textBoxFlowTemp_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.textBoxReturnTemp.Focus();
    }

    private void textBoxReturnTemp_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.buttonRunCalibrationStep.Focus();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ADC_Measurement));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonReadAdcValues = new Button();
      this.splitContainer1 = new SplitContainer();
      this.textBoxInfos = new TextBox();
      this.dataGridViewInfos = new DataGridView();
      this.groupBoxVolumeSimulationValue = new GroupBox();
      this.label6 = new Label();
      this.textBoxVolumeSimulation = new TextBox();
      this.buttonSendAdcTestVolume = new Button();
      this.buttonSetTestDoneSleep = new Button();
      this.buttonSetTestDoneSetTest = new Button();
      this.buttonTestDoneCool = new Button();
      this.buttonSendAdcTestDone = new Button();
      this.buttonSendAdcTestActivate = new Button();
      this.groupBoxCalibration = new GroupBox();
      this.comboBoxCalibrationStep = new ComboBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.textBoxReturnTemp = new TextBox();
      this.textBoxFlowTemp = new TextBox();
      this.textBoxLoops = new TextBox();
      this.buttonRunCalibrationStep = new Button();
      this.buttonBreakLoop = new Button();
      this.buttonRunCalibration = new Button();
      this.buttonClearData = new Button();
      this.buttonReadDeviceValues = new Button();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewInfos).BeginInit();
      this.groupBoxVolumeSimulationValue.SuspendLayout();
      this.groupBoxCalibration.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1582, 63);
      this.zennerCoroprateDesign2.TabIndex = 3;
      this.buttonReadAdcValues.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadAdcValues.Location = new Point(1305, 892);
      this.buttonReadAdcValues.Margin = new Padding(4, 5, 4, 5);
      this.buttonReadAdcValues.Name = "buttonReadAdcValues";
      this.buttonReadAdcValues.Size = new Size(264, 35);
      this.buttonReadAdcValues.TabIndex = 1;
      this.buttonReadAdcValues.Text = "Read adc values";
      this.buttonReadAdcValues.UseVisualStyleBackColor = true;
      this.buttonReadAdcValues.Click += new System.EventHandler(this.buttonReadAdcValues_Click);
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(0, 71);
      this.splitContainer1.Margin = new Padding(4, 5, 4, 5);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.textBoxInfos);
      this.splitContainer1.Panel2.Controls.Add((Control) this.dataGridViewInfos);
      this.splitContainer1.Size = new Size(1294, 873);
      this.splitContainer1.SplitterDistance = 491;
      this.splitContainer1.SplitterWidth = 6;
      this.splitContainer1.TabIndex = 4;
      this.textBoxInfos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxInfos.Location = new Point(15, 5);
      this.textBoxInfos.Margin = new Padding(4, 5, 4, 5);
      this.textBoxInfos.Multiline = true;
      this.textBoxInfos.Name = "textBoxInfos";
      this.textBoxInfos.ScrollBars = ScrollBars.Both;
      this.textBoxInfos.Size = new Size(1272, 479);
      this.textBoxInfos.TabIndex = 0;
      this.dataGridViewInfos.AllowUserToAddRows = false;
      this.dataGridViewInfos.AllowUserToDeleteRows = false;
      this.dataGridViewInfos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewInfos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewInfos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewInfos.Location = new Point(15, 5);
      this.dataGridViewInfos.Margin = new Padding(4, 5, 4, 5);
      this.dataGridViewInfos.Name = "dataGridViewInfos";
      this.dataGridViewInfos.Size = new Size(1278, 369);
      this.dataGridViewInfos.TabIndex = 0;
      this.groupBoxVolumeSimulationValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.label6);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.textBoxVolumeSimulation);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.buttonSendAdcTestVolume);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.buttonSetTestDoneSleep);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.buttonSetTestDoneSetTest);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.buttonTestDoneCool);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.buttonSendAdcTestDone);
      this.groupBoxVolumeSimulationValue.Controls.Add((Control) this.buttonSendAdcTestActivate);
      this.groupBoxVolumeSimulationValue.Location = new Point(1304, 72);
      this.groupBoxVolumeSimulationValue.Margin = new Padding(4, 5, 4, 5);
      this.groupBoxVolumeSimulationValue.Name = "groupBoxVolumeSimulationValue";
      this.groupBoxVolumeSimulationValue.Padding = new Padding(4, 5, 4, 5);
      this.groupBoxVolumeSimulationValue.Size = new Size(262, 382);
      this.groupBoxVolumeSimulationValue.TabIndex = 2;
      this.groupBoxVolumeSimulationValue.TabStop = false;
      this.groupBoxVolumeSimulationValue.Text = "Volume simulation";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(213, 34);
      this.label6.Margin = new Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(40, 20);
      this.label6.TabIndex = 19;
      this.label6.Text = "Liter";
      this.textBoxVolumeSimulation.Location = new Point(10, 29);
      this.textBoxVolumeSimulation.Margin = new Padding(4, 5, 4, 5);
      this.textBoxVolumeSimulation.Name = "textBoxVolumeSimulation";
      this.textBoxVolumeSimulation.Size = new Size(192, 26);
      this.textBoxVolumeSimulation.TabIndex = 0;
      this.textBoxVolumeSimulation.Text = "100";
      this.textBoxVolumeSimulation.TextAlign = HorizontalAlignment.Right;
      this.buttonSendAdcTestVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSendAdcTestVolume.Location = new Point(10, 114);
      this.buttonSendAdcTestVolume.Margin = new Padding(4, 5, 4, 5);
      this.buttonSendAdcTestVolume.Name = "buttonSendAdcTestVolume";
      this.buttonSendAdcTestVolume.Size = new Size(243, 35);
      this.buttonSendAdcTestVolume.TabIndex = 18;
      this.buttonSendAdcTestVolume.Text = "Send volume simulation";
      this.buttonSendAdcTestVolume.UseVisualStyleBackColor = true;
      this.buttonSendAdcTestVolume.Click += new System.EventHandler(this.buttonSendAdcTestVolume_Click);
      this.buttonSetTestDoneSleep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSetTestDoneSleep.Location = new Point(9, 308);
      this.buttonSetTestDoneSleep.Margin = new Padding(4, 5, 4, 5);
      this.buttonSetTestDoneSleep.Name = "buttonSetTestDoneSleep";
      this.buttonSetTestDoneSleep.Size = new Size(243, 35);
      this.buttonSetTestDoneSleep.TabIndex = 18;
      this.buttonSetTestDoneSleep.Text = "Send test done (Sleep)";
      this.buttonSetTestDoneSleep.UseVisualStyleBackColor = true;
      this.buttonSetTestDoneSleep.Click += new System.EventHandler(this.buttonSetTestDoneSleep_Click);
      this.buttonSetTestDoneSetTest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSetTestDoneSetTest.Location = new Point(9, 263);
      this.buttonSetTestDoneSetTest.Margin = new Padding(4, 5, 4, 5);
      this.buttonSetTestDoneSetTest.Name = "buttonSetTestDoneSetTest";
      this.buttonSetTestDoneSetTest.Size = new Size(243, 35);
      this.buttonSetTestDoneSetTest.TabIndex = 18;
      this.buttonSetTestDoneSetTest.Text = "Send test done (LCD:SegTest)";
      this.buttonSetTestDoneSetTest.UseVisualStyleBackColor = true;
      this.buttonSetTestDoneSetTest.Click += new System.EventHandler(this.buttonSetTestDoneSegTest_Click);
      this.buttonTestDoneCool.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTestDoneCool.Location = new Point(9, 220);
      this.buttonTestDoneCool.Margin = new Padding(4, 5, 4, 5);
      this.buttonTestDoneCool.Name = "buttonTestDoneCool";
      this.buttonTestDoneCool.Size = new Size(243, 35);
      this.buttonTestDoneCool.TabIndex = 18;
      this.buttonTestDoneCool.Text = "Send test done (LCD:Cool)";
      this.buttonTestDoneCool.UseVisualStyleBackColor = true;
      this.buttonTestDoneCool.Click += new System.EventHandler(this.buttonTestDoneCool_Click);
      this.buttonSendAdcTestDone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSendAdcTestDone.Location = new Point(9, 175);
      this.buttonSendAdcTestDone.Margin = new Padding(4, 5, 4, 5);
      this.buttonSendAdcTestDone.Name = "buttonSendAdcTestDone";
      this.buttonSendAdcTestDone.Size = new Size(243, 35);
      this.buttonSendAdcTestDone.TabIndex = 18;
      this.buttonSendAdcTestDone.Text = "Send test done (LCD:Energy)";
      this.buttonSendAdcTestDone.UseVisualStyleBackColor = true;
      this.buttonSendAdcTestDone.Click += new System.EventHandler(this.buttonSendAdcTestDone_Click);
      this.buttonSendAdcTestActivate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSendAdcTestActivate.Location = new Point(10, 69);
      this.buttonSendAdcTestActivate.Margin = new Padding(4, 5, 4, 5);
      this.buttonSendAdcTestActivate.Name = "buttonSendAdcTestActivate";
      this.buttonSendAdcTestActivate.Size = new Size(243, 35);
      this.buttonSendAdcTestActivate.TabIndex = 18;
      this.buttonSendAdcTestActivate.Text = "Send ADC test activate ";
      this.buttonSendAdcTestActivate.UseVisualStyleBackColor = true;
      this.buttonSendAdcTestActivate.Click += new System.EventHandler(this.buttonSendAdcTestActivate_Click);
      this.groupBoxCalibration.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxCalibration.Controls.Add((Control) this.comboBoxCalibrationStep);
      this.groupBoxCalibration.Controls.Add((Control) this.label5);
      this.groupBoxCalibration.Controls.Add((Control) this.label4);
      this.groupBoxCalibration.Controls.Add((Control) this.label3);
      this.groupBoxCalibration.Controls.Add((Control) this.label2);
      this.groupBoxCalibration.Controls.Add((Control) this.textBoxReturnTemp);
      this.groupBoxCalibration.Controls.Add((Control) this.textBoxFlowTemp);
      this.groupBoxCalibration.Controls.Add((Control) this.textBoxLoops);
      this.groupBoxCalibration.Controls.Add((Control) this.buttonRunCalibrationStep);
      this.groupBoxCalibration.Controls.Add((Control) this.buttonBreakLoop);
      this.groupBoxCalibration.Controls.Add((Control) this.buttonRunCalibration);
      this.groupBoxCalibration.Location = new Point(1304, 463);
      this.groupBoxCalibration.Margin = new Padding(4, 5, 4, 5);
      this.groupBoxCalibration.Name = "groupBoxCalibration";
      this.groupBoxCalibration.Padding = new Padding(4, 5, 4, 5);
      this.groupBoxCalibration.Size = new Size(262, 329);
      this.groupBoxCalibration.TabIndex = 0;
      this.groupBoxCalibration.TabStop = false;
      this.groupBoxCalibration.Text = "Calibration";
      this.comboBoxCalibrationStep.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxCalibrationStep.FormattingEnabled = true;
      this.comboBoxCalibrationStep.Location = new Point(84, 149);
      this.comboBoxCalibrationStep.Margin = new Padding(4, 5, 4, 5);
      this.comboBoxCalibrationStep.Name = "comboBoxCalibrationStep";
      this.comboBoxCalibrationStep.Size = new Size(168, 28);
      this.comboBoxCalibrationStep.TabIndex = 5;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(26, 154);
      this.label5.Margin = new Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(47, 20);
      this.label5.TabIndex = 19;
      this.label5.Text = "Step:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(26, 114);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(106, 20);
      this.label4.TabIndex = 19;
      this.label4.Text = "Return temp.:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(26, 74);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(90, 20);
      this.label3.TabIndex = 19;
      this.label3.Text = "Flow temp.:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(26, 34);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(57, 20);
      this.label2.TabIndex = 19;
      this.label2.Text = "Loops:";
      this.textBoxReturnTemp.Location = new Point(141, 109);
      this.textBoxReturnTemp.Margin = new Padding(4, 5, 4, 5);
      this.textBoxReturnTemp.Name = "textBoxReturnTemp";
      this.textBoxReturnTemp.Size = new Size(110, 26);
      this.textBoxReturnTemp.TabIndex = 1;
      this.textBoxReturnTemp.Text = "10,27";
      this.textBoxReturnTemp.KeyUp += new KeyEventHandler(this.textBoxReturnTemp_KeyUp);
      this.textBoxFlowTemp.Location = new Point(141, 69);
      this.textBoxFlowTemp.Margin = new Padding(4, 5, 4, 5);
      this.textBoxFlowTemp.Name = "textBoxFlowTemp";
      this.textBoxFlowTemp.Size = new Size(110, 26);
      this.textBoxFlowTemp.TabIndex = 0;
      this.textBoxFlowTemp.Text = "43,89";
      this.textBoxFlowTemp.KeyUp += new KeyEventHandler(this.textBoxFlowTemp_KeyUp);
      this.textBoxLoops.Location = new Point(141, 29);
      this.textBoxLoops.Margin = new Padding(4, 5, 4, 5);
      this.textBoxLoops.Name = "textBoxLoops";
      this.textBoxLoops.Size = new Size(110, 26);
      this.textBoxLoops.TabIndex = 4;
      this.textBoxLoops.Text = "5";
      this.buttonRunCalibrationStep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonRunCalibrationStep.Location = new Point(10, 202);
      this.buttonRunCalibrationStep.Margin = new Padding(4, 5, 4, 5);
      this.buttonRunCalibrationStep.Name = "buttonRunCalibrationStep";
      this.buttonRunCalibrationStep.Size = new Size(243, 35);
      this.buttonRunCalibrationStep.TabIndex = 2;
      this.buttonRunCalibrationStep.Text = "Run calibration step";
      this.buttonRunCalibrationStep.UseVisualStyleBackColor = true;
      this.buttonRunCalibrationStep.Click += new System.EventHandler(this.buttonRunCalibrationStep_Click);
      this.buttonBreakLoop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonBreakLoop.Location = new Point(10, 285);
      this.buttonBreakLoop.Margin = new Padding(4, 5, 4, 5);
      this.buttonBreakLoop.Name = "buttonBreakLoop";
      this.buttonBreakLoop.Size = new Size(243, 35);
      this.buttonBreakLoop.TabIndex = 3;
      this.buttonBreakLoop.Text = "Break loop";
      this.buttonBreakLoop.UseVisualStyleBackColor = true;
      this.buttonBreakLoop.Click += new System.EventHandler(this.buttonBreakLoop_Click);
      this.buttonRunCalibration.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonRunCalibration.Location = new Point(10, 246);
      this.buttonRunCalibration.Margin = new Padding(4, 5, 4, 5);
      this.buttonRunCalibration.Name = "buttonRunCalibration";
      this.buttonRunCalibration.Size = new Size(243, 35);
      this.buttonRunCalibration.TabIndex = 6;
      this.buttonRunCalibration.Text = "Run calibration";
      this.buttonRunCalibration.UseVisualStyleBackColor = true;
      this.buttonRunCalibration.Click += new System.EventHandler(this.buttonRunCalibration_Click);
      this.buttonClearData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClearData.Location = new Point(1305, 802);
      this.buttonClearData.Margin = new Padding(4, 5, 4, 5);
      this.buttonClearData.Name = "buttonClearData";
      this.buttonClearData.Size = new Size(264, 35);
      this.buttonClearData.TabIndex = 0;
      this.buttonClearData.Text = "Clear data";
      this.buttonClearData.UseVisualStyleBackColor = true;
      this.buttonClearData.Click += new System.EventHandler(this.buttonClearData_Click);
      this.buttonReadDeviceValues.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadDeviceValues.Location = new Point(1304, 847);
      this.buttonReadDeviceValues.Margin = new Padding(4, 5, 4, 5);
      this.buttonReadDeviceValues.Name = "buttonReadDeviceValues";
      this.buttonReadDeviceValues.Size = new Size(264, 35);
      this.buttonReadDeviceValues.TabIndex = 5;
      this.buttonReadDeviceValues.Text = "Read device values";
      this.buttonReadDeviceValues.UseVisualStyleBackColor = true;
      this.buttonReadDeviceValues.Click += new System.EventHandler(this.buttonReadDeviceValues_Click);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1582, 947);
      this.Controls.Add((Control) this.buttonReadDeviceValues);
      this.Controls.Add((Control) this.groupBoxCalibration);
      this.Controls.Add((Control) this.groupBoxVolumeSimulationValue);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.buttonClearData);
      this.Controls.Add((Control) this.buttonReadAdcValues);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (ADC_Measurement);
      this.Text = "ADC Measurement";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.dataGridViewInfos).EndInit();
      this.groupBoxVolumeSimulationValue.ResumeLayout(false);
      this.groupBoxVolumeSimulationValue.PerformLayout();
      this.groupBoxCalibration.ResumeLayout(false);
      this.groupBoxCalibration.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
