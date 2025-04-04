// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.Configurator
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using CorporateDesign;
using DeviceCollector;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public class Configurator : Form
  {
    private readonly DateTime NULLDATE = new DateTime(2000, 1, 1);
    private MinoprotectIII tempWorkMeter;
    private MinoprotectIII tempConnectedMeter;
    private MinoprotectIII tempBackupMeter;
    private IContainer components = (IContainer) null;
    private Button btnCancel;
    private Button btnSave;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private GroupBox gboxVersion;
    private ToolTip toolTip;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Panel panel;
    private Label label13;
    private NumericUpDown txtVersionNumber;
    private Label label21;
    private NumericUpDown txtDeviceIdentity;
    private Label label20;
    private NumericUpDown txtRevision;
    private Label label19;
    private NumericUpDown txtSubversion;
    private Label label25;
    private NumericUpDown txtHardwareTypeID;
    private Label label27;
    private NumericUpDown txtHardwareVersion_i;
    private GroupBox groupBox1;
    private Label label1;
    private NumericUpDown txtNetworkAddressForInterlinkedSmokeAlarms;
    private Label label2;
    private NumericUpDown txtRadioFrequencyAdjustment;
    private Label label3;
    private NumericUpDown txtRadioTransmitPower;
    private Label label4;
    private NumericUpDown txtEpsilonValue;
    private Label label5;
    private NumericUpDown txtRadioTransmitInterval;
    private Label label6;
    private GroupBox gboxTestModeParameter;
    private Label label17;
    private Label label18;
    private Label label22;
    private Label label12;
    private NumericUpDown txtTypeOfTelegram;
    private Label label10;
    private Label label9;
    private NumericUpDown txtStatusByteRadioProtocol;
    private Label label8;
    private Label label7;
    private NumericUpDown txtRadioLibraryVersionNumber;
    private ComboBox cboxFunctionTestMode;
    private TextBox txtInternalDiagnosticParameter;
    private ComboBox cboxRadioMode;
    private DateTimePicker txtDateOfFirstActivation;
    private DateTimePicker txtCurrentDatetime;
    private GroupBox groupBox6;
    private CheckedListBox listCurrentStateOfEvents;
    private GroupBox gboxManufacturingParameter;
    private Label label11;
    private NumericUpDown txtMeterID;
    private Label label14;
    private NumericUpDown txtMeterInfoID;
    private DateTimePicker txtPCBdate;
    private Label label15;
    private Label label16;
    private NumericUpDown txtPCBnumber;
    private NumericUpDown txtSerialnumber;
    private Label label32;
    private TextBox txtManufacturer;
    private Label label33;
    private NumericUpDown txtMBusGeneration;
    private Label label23;
    private ComboBox cboxMBusMedium;
    private Label label24;
    private Label label26;
    private NumericUpDown txtSapOrderNumber;
    private Label lblMBusMedium;
    private Label lblHardwareVersion;
    private Label label28;
    private NumericUpDown txtHardwareVersion_k;
    private Button btnCalculateEpsilonValue;
    private ComboBox cboxRadioProtocol;

    public Configurator() => this.InitializeComponent();

    private void Configurator_Load(object sender, EventArgs e) => this.InitializeForm();

    internal static void ShowDialog(Form owner, SmokeDetectorHandlerFunctions handler)
    {
      if (handler == null)
        return;
      using (Configurator configurator = new Configurator())
      {
        if (handler.WorkMeter != null)
          configurator.tempWorkMeter = handler.WorkMeter.DeepCopy();
        if (handler.ConnectedMeter != null)
          configurator.tempConnectedMeter = handler.ConnectedMeter.DeepCopy();
        if (handler.BackupMeter != null)
          configurator.tempBackupMeter = handler.BackupMeter.DeepCopy();
        if (configurator.ShowDialog((IWin32Window) owner) != DialogResult.OK)
          return;
        handler.WorkMeter = configurator.tempWorkMeter;
        handler.ConnectedMeter = configurator.tempConnectedMeter;
        handler.BackupMeter = configurator.tempBackupMeter;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.SaveValues();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoadValues();
    }

    private void btnCalculateEpsilonValue_Click(object sender, EventArgs e)
    {
      this.txtEpsilonValue.Value = 5759M - this.txtSerialnumber.Value % 65536M % 5000M;
    }

    private void txtHardwareVersion_ValueChanged(object sender, EventArgs e)
    {
      try
      {
        this.lblHardwareVersion.Text = string.Format("0x{0:X4}, {0}", (object) BitConverter.ToUInt16(new byte[2]
        {
          (byte) this.txtHardwareVersion_i.Value,
          (byte) this.txtHardwareVersion_k.Value
        }, 0));
      }
      catch
      {
        this.lblHardwareVersion.Text = string.Empty;
      }
    }

    private void cboxMBusMedium_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.lblMBusMedium.Text = "0x" + ((byte) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMedium.SelectedItem.ToString(), true)).ToString("X2");
      }
      catch
      {
        this.lblMBusMedium.Text = string.Empty;
      }
    }

    private void LoadValues()
    {
      try
      {
        MinoprotectIII handlerMeter = this.GetHandlerMeter();
        this.panel.Visible = handlerMeter != null;
        this.panel.CausesValidation = false;
        if (handlerMeter == null)
          return;
        if (handlerMeter.Version != null)
        {
          this.gboxVersion.Enabled = true;
          this.txtVersionNumber.Value = (Decimal) handlerMeter.Version.VersionNumber;
          this.txtSubversion.Value = (Decimal) handlerMeter.Version.Subversion;
          this.txtRevision.Value = (Decimal) handlerMeter.Version.Revision;
          this.txtDeviceIdentity.Value = (Decimal) handlerMeter.Version.DeviceIdentity;
          this.txtHardwareTypeID.Value = (Decimal) handlerMeter.Version.HardwareTypeID;
          this.txtSapOrderNumber.Value = (Decimal) handlerMeter.Version.SapNumber;
          byte[] bytes = BitConverter.GetBytes(handlerMeter.Version.HardwareVersion);
          this.txtHardwareVersion_i.Value = (Decimal) bytes[1];
          this.txtHardwareVersion_k.Value = (Decimal) bytes[0];
          this.txtSerialnumber.Value = (Decimal) handlerMeter.Version.Serialnumber;
          this.txtManufacturer.Text = handlerMeter.Version.Manufacturer;
          this.txtMBusGeneration.Value = (Decimal) handlerMeter.Version.Generation;
          this.cboxMBusMedium.SelectedItem = (object) handlerMeter.Version.Medium.ToString();
        }
        else
          this.gboxVersion.Enabled = false;
        if (handlerMeter.ManufacturingParameter != null)
        {
          this.gboxManufacturingParameter.Enabled = true;
          this.txtMeterID.Value = (Decimal) handlerMeter.ManufacturingParameter.MeterID;
          this.txtMeterInfoID.Value = (Decimal) handlerMeter.ManufacturingParameter.MeterInfoID;
          this.txtPCBnumber.Value = (Decimal) handlerMeter.ManufacturingParameter.PCBNumberOfDate;
          this.txtPCBdate.Value = !handlerMeter.ManufacturingParameter.PCBDate.HasValue ? new DateTime(2000, 1, 1) : handlerMeter.ManufacturingParameter.PCBDate.Value;
        }
        else
          this.gboxManufacturingParameter.Enabled = false;
        if (handlerMeter.Parameter != null)
        {
          MinoprotectIII_Parameter parameter = handlerMeter.Parameter;
          this.txtDateOfFirstActivation.Value = parameter.DateOfFirstActivation.HasValue ? parameter.DateOfFirstActivation.Value : new DateTime(2000, 1, 1);
          this.txtRadioTransmitInterval.Value = (Decimal) parameter.RadioTransmitInterval;
          this.txtEpsilonValue.Value = (Decimal) parameter.RadioEpsilonValue;
          this.txtRadioTransmitPower.Value = (Decimal) parameter.RadioTransmitPower;
          this.txtRadioFrequencyAdjustment.Value = (Decimal) parameter.RadioFrequencyAdjustment;
          this.txtNetworkAddressForInterlinkedSmokeAlarms.Value = (Decimal) parameter.NetworkAddressForInterlinkedSmokeAlarms;
          this.txtRadioLibraryVersionNumber.Value = (Decimal) parameter.RadioLibraryVersionNumber;
          this.cboxRadioProtocol.SelectedItem = (object) parameter.RadioProtocol.ToString();
          this.txtStatusByteRadioProtocol.Value = (Decimal) parameter.StatusByte;
          this.txtCurrentDatetime.Value = parameter.CurrentDateTime.HasValue ? parameter.CurrentDateTime.Value : new DateTime(2000, 1, 1);
          this.txtTypeOfTelegram.Value = (Decimal) parameter.TypeOfTelegram;
          for (int index = 0; index < this.listCurrentStateOfEvents.Items.Count; ++index)
          {
            SmokeDetectorEvent smokeDetectorEvent = (SmokeDetectorEvent) Enum.Parse(typeof (SmokeDetectorEvent), this.listCurrentStateOfEvents.GetItemText(this.listCurrentStateOfEvents.Items[index]), true);
            bool flag = (parameter.CurrentStateOfEvents & smokeDetectorEvent) == smokeDetectorEvent;
            this.listCurrentStateOfEvents.SetItemChecked(index, flag);
          }
        }
        if (handlerMeter.TestModeParameter != null)
        {
          this.gboxTestModeParameter.Enabled = true;
          TestModeParameter testModeParameter = handlerMeter.TestModeParameter;
          this.cboxRadioMode.SelectedItem = (object) testModeParameter.RadioMode.ToString();
          this.cboxFunctionTestMode.SelectedItem = (object) testModeParameter.FunctionTestMode.ToString();
          this.txtInternalDiagnosticParameter.Text = Util.ByteArrayToHexString(testModeParameter.InternalDiagnosticParameter);
        }
        else
          this.gboxTestModeParameter.Enabled = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format("Error message: {0} Exception is: '{1}'. Stack trace: {2}", (object) ex.Message, (object) ex.GetBaseException().GetType().ToString(), (object) ex.StackTrace), "Error by load the values", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void SaveValues()
    {
      try
      {
        MinoprotectIII handlerMeter = this.GetHandlerMeter();
        if (handlerMeter == null)
          return;
        if (handlerMeter.Version != null)
        {
          handlerMeter.Version.HardwareTypeID = Convert.ToUInt16(this.txtHardwareTypeID.Value);
          handlerMeter.Version.SapNumber = Convert.ToUInt32(this.txtSapOrderNumber.Value);
          handlerMeter.Version.HardwareVersion = BitConverter.ToUInt16(new byte[2]
          {
            (byte) this.txtHardwareVersion_k.Value,
            (byte) this.txtHardwareVersion_i.Value
          }, 0);
          handlerMeter.Version.Serialnumber = Convert.ToUInt32(this.txtSerialnumber.Value);
          handlerMeter.Version.Manufacturer = this.txtManufacturer.Text;
          handlerMeter.Version.Generation = Convert.ToByte(this.txtMBusGeneration.Value);
          handlerMeter.Version.Medium = (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMedium.SelectedItem.ToString(), true);
        }
        if (handlerMeter.Parameter != null)
        {
          handlerMeter.Parameter.DateOfFirstActivation = !(this.txtDateOfFirstActivation.Value == this.NULLDATE) ? new DateTime?(this.txtDateOfFirstActivation.Value) : new DateTime?();
          handlerMeter.Parameter.RadioTransmitInterval = Convert.ToUInt16(this.txtRadioTransmitInterval.Value);
          handlerMeter.Parameter.RadioEpsilonValue = Convert.ToUInt16(this.txtEpsilonValue.Value);
          handlerMeter.Parameter.RadioTransmitPower = Convert.ToByte(this.txtRadioTransmitPower.Value);
          handlerMeter.Parameter.RadioFrequencyAdjustment = Convert.ToByte(this.txtRadioFrequencyAdjustment.Value);
          handlerMeter.Parameter.NetworkAddressForInterlinkedSmokeAlarms = Convert.ToUInt32(this.txtNetworkAddressForInterlinkedSmokeAlarms.Value);
          handlerMeter.Parameter.RadioLibraryVersionNumber = Convert.ToByte(this.txtRadioLibraryVersionNumber.Value);
          handlerMeter.Parameter.RadioProtocol = (RadioProtocol) Enum.Parse(typeof (RadioProtocol), this.cboxRadioProtocol.SelectedItem.ToString(), true);
          handlerMeter.Parameter.StatusByte = Convert.ToByte(this.txtStatusByteRadioProtocol.Value);
          handlerMeter.Parameter.CurrentDateTime = new DateTime?(this.txtCurrentDatetime.Value);
          handlerMeter.Parameter.CurrentStateOfEvents = this.GetCheckedSmokeDetectorEvents();
          handlerMeter.Parameter.TypeOfTelegram = Convert.ToByte(this.txtTypeOfTelegram.Value);
        }
        if (handlerMeter.TestModeParameter != null)
        {
          handlerMeter.TestModeParameter.RadioMode = (RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioMode.SelectedItem.ToString(), true);
          handlerMeter.TestModeParameter.FunctionTestMode = (FunctionTestMode) Enum.Parse(typeof (FunctionTestMode), this.cboxFunctionTestMode.SelectedItem.ToString(), true);
          handlerMeter.TestModeParameter.InternalDiagnosticParameter = Util.HexStringToByteArray(this.txtInternalDiagnosticParameter.Text);
        }
        if (handlerMeter.ManufacturingParameter == null)
          return;
        handlerMeter.ManufacturingParameter.MeterID = Convert.ToUInt32(this.txtMeterID.Value);
        handlerMeter.ManufacturingParameter.MeterInfoID = Convert.ToUInt32(this.txtMeterInfoID.Value);
        handlerMeter.ManufacturingParameter.PCBDate = new DateTime?(this.txtPCBdate.Value);
        handlerMeter.ManufacturingParameter.PCBNumberOfDate = Convert.ToUInt16(this.txtPCBnumber.Value);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error occurred while save the values", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private SmokeDetectorEvent GetCheckedSmokeDetectorEvents()
    {
      SmokeDetectorEvent smokeDetectorEvents = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
      foreach (object checkedItem in this.listCurrentStateOfEvents.CheckedItems)
      {
        SmokeDetectorEvent smokeDetectorEvent = (SmokeDetectorEvent) Enum.Parse(typeof (SmokeDetectorEvent), checkedItem.ToString(), true);
        smokeDetectorEvents |= smokeDetectorEvent;
      }
      return smokeDetectorEvents;
    }

    private MinoprotectIII GetHandlerMeter()
    {
      MinoprotectIII handlerMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          handlerMeter = this.tempWorkMeter;
          break;
        case HandlerMeterType.ConnectedMeter:
          handlerMeter = this.tempConnectedMeter;
          break;
        case HandlerMeterType.BackupMeter:
          handlerMeter = this.tempBackupMeter;
          break;
        default:
          throw new NotImplementedException();
      }
      return handlerMeter;
    }

    private void InitializeForm()
    {
      this.cboxRadioMode.DataSource = (object) Util.GetNamesOfEnum(typeof (RadioMode));
      this.cboxFunctionTestMode.DataSource = (object) Util.GetNamesOfEnum(typeof (FunctionTestMode));
      this.cboxMBusMedium.DataSource = (object) Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxRadioProtocol.DataSource = (object) Util.GetNamesOfEnum(typeof (RadioProtocol));
      this.listCurrentStateOfEvents.Items.Clear();
      foreach (object obj in Util.GetNamesOfEnum(typeof (SmokeDetectorEvent)))
        this.listCurrentStateOfEvents.Items.Add(obj, false);
      this.cboxHandlerObject.DataSource = (object) Util.GetNamesOfEnum(typeof (HandlerMeterType));
      this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Configurator));
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.gboxVersion = new GroupBox();
      this.lblMBusMedium = new Label();
      this.lblHardwareVersion = new Label();
      this.label28 = new Label();
      this.txtHardwareVersion_k = new NumericUpDown();
      this.label26 = new Label();
      this.txtSapOrderNumber = new NumericUpDown();
      this.cboxMBusMedium = new ComboBox();
      this.label24 = new Label();
      this.txtMBusGeneration = new NumericUpDown();
      this.label23 = new Label();
      this.txtManufacturer = new TextBox();
      this.label33 = new Label();
      this.txtSerialnumber = new NumericUpDown();
      this.label32 = new Label();
      this.label27 = new Label();
      this.txtHardwareVersion_i = new NumericUpDown();
      this.label25 = new Label();
      this.txtHardwareTypeID = new NumericUpDown();
      this.label21 = new Label();
      this.txtDeviceIdentity = new NumericUpDown();
      this.label20 = new Label();
      this.txtRevision = new NumericUpDown();
      this.label19 = new Label();
      this.txtSubversion = new NumericUpDown();
      this.label13 = new Label();
      this.txtVersionNumber = new NumericUpDown();
      this.toolTip = new ToolTip(this.components);
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.panel = new Panel();
      this.gboxManufacturingParameter = new GroupBox();
      this.label16 = new Label();
      this.txtPCBnumber = new NumericUpDown();
      this.txtPCBdate = new DateTimePicker();
      this.label15 = new Label();
      this.label14 = new Label();
      this.txtMeterInfoID = new NumericUpDown();
      this.label11 = new Label();
      this.txtMeterID = new NumericUpDown();
      this.groupBox6 = new GroupBox();
      this.listCurrentStateOfEvents = new CheckedListBox();
      this.gboxTestModeParameter = new GroupBox();
      this.txtInternalDiagnosticParameter = new TextBox();
      this.cboxRadioMode = new ComboBox();
      this.cboxFunctionTestMode = new ComboBox();
      this.label17 = new Label();
      this.label18 = new Label();
      this.label22 = new Label();
      this.groupBox1 = new GroupBox();
      this.cboxRadioProtocol = new ComboBox();
      this.btnCalculateEpsilonValue = new Button();
      this.txtCurrentDatetime = new DateTimePicker();
      this.txtDateOfFirstActivation = new DateTimePicker();
      this.label12 = new Label();
      this.txtTypeOfTelegram = new NumericUpDown();
      this.label10 = new Label();
      this.label9 = new Label();
      this.txtStatusByteRadioProtocol = new NumericUpDown();
      this.label8 = new Label();
      this.label7 = new Label();
      this.txtRadioLibraryVersionNumber = new NumericUpDown();
      this.label1 = new Label();
      this.txtNetworkAddressForInterlinkedSmokeAlarms = new NumericUpDown();
      this.label2 = new Label();
      this.txtRadioFrequencyAdjustment = new NumericUpDown();
      this.label3 = new Label();
      this.txtRadioTransmitPower = new NumericUpDown();
      this.label4 = new Label();
      this.txtEpsilonValue = new NumericUpDown();
      this.label5 = new Label();
      this.txtRadioTransmitInterval = new NumericUpDown();
      this.label6 = new Label();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.gboxVersion.SuspendLayout();
      this.txtHardwareVersion_k.BeginInit();
      this.txtSapOrderNumber.BeginInit();
      this.txtMBusGeneration.BeginInit();
      this.txtSerialnumber.BeginInit();
      this.txtHardwareVersion_i.BeginInit();
      this.txtHardwareTypeID.BeginInit();
      this.txtDeviceIdentity.BeginInit();
      this.txtRevision.BeginInit();
      this.txtSubversion.BeginInit();
      this.txtVersionNumber.BeginInit();
      this.panel.SuspendLayout();
      this.gboxManufacturingParameter.SuspendLayout();
      this.txtPCBnumber.BeginInit();
      this.txtMeterInfoID.BeginInit();
      this.txtMeterID.BeginInit();
      this.groupBox6.SuspendLayout();
      this.gboxTestModeParameter.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.txtTypeOfTelegram.BeginInit();
      this.txtStatusByteRadioProtocol.BeginInit();
      this.txtRadioLibraryVersionNumber.BeginInit();
      this.txtNetworkAddressForInterlinkedSmokeAlarms.BeginInit();
      this.txtRadioFrequencyAdjustment.BeginInit();
      this.txtRadioTransmitPower.BeginInit();
      this.txtEpsilonValue.BeginInit();
      this.txtRadioTransmitInterval.BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(708, 514);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(77, 29);
      this.btnCancel.TabIndex = 23;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(608, 514);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(77, 29);
      this.btnSave.TabIndex = 22;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.gboxVersion.Controls.Add((Control) this.lblMBusMedium);
      this.gboxVersion.Controls.Add((Control) this.lblHardwareVersion);
      this.gboxVersion.Controls.Add((Control) this.label28);
      this.gboxVersion.Controls.Add((Control) this.txtHardwareVersion_k);
      this.gboxVersion.Controls.Add((Control) this.label26);
      this.gboxVersion.Controls.Add((Control) this.txtSapOrderNumber);
      this.gboxVersion.Controls.Add((Control) this.cboxMBusMedium);
      this.gboxVersion.Controls.Add((Control) this.label24);
      this.gboxVersion.Controls.Add((Control) this.txtMBusGeneration);
      this.gboxVersion.Controls.Add((Control) this.label23);
      this.gboxVersion.Controls.Add((Control) this.txtManufacturer);
      this.gboxVersion.Controls.Add((Control) this.label33);
      this.gboxVersion.Controls.Add((Control) this.txtSerialnumber);
      this.gboxVersion.Controls.Add((Control) this.label32);
      this.gboxVersion.Controls.Add((Control) this.label27);
      this.gboxVersion.Controls.Add((Control) this.txtHardwareVersion_i);
      this.gboxVersion.Controls.Add((Control) this.label25);
      this.gboxVersion.Controls.Add((Control) this.txtHardwareTypeID);
      this.gboxVersion.Controls.Add((Control) this.label21);
      this.gboxVersion.Controls.Add((Control) this.txtDeviceIdentity);
      this.gboxVersion.Controls.Add((Control) this.label20);
      this.gboxVersion.Controls.Add((Control) this.txtRevision);
      this.gboxVersion.Controls.Add((Control) this.label19);
      this.gboxVersion.Controls.Add((Control) this.txtSubversion);
      this.gboxVersion.Controls.Add((Control) this.label13);
      this.gboxVersion.Controls.Add((Control) this.txtVersionNumber);
      this.gboxVersion.Location = new Point(6, 5);
      this.gboxVersion.Name = "gboxVersion";
      this.gboxVersion.Size = new Size(409, 177);
      this.gboxVersion.TabIndex = 47;
      this.gboxVersion.TabStop = false;
      this.gboxVersion.Text = "Device specific parameter";
      this.lblMBusMedium.Location = new Point(202, 86);
      this.lblMBusMedium.Name = "lblMBusMedium";
      this.lblMBusMedium.Size = new Size(32, 15);
      this.lblMBusMedium.TabIndex = 95;
      this.lblMBusMedium.Tag = (object) "";
      this.lblMBusMedium.Text = "0x00";
      this.lblMBusMedium.TextAlign = ContentAlignment.MiddleLeft;
      this.lblHardwareVersion.Location = new Point(202, 150);
      this.lblHardwareVersion.Name = "lblHardwareVersion";
      this.lblHardwareVersion.Size = new Size(123, 15);
      this.lblHardwareVersion.TabIndex = 94;
      this.lblHardwareVersion.Tag = (object) "";
      this.lblHardwareVersion.Text = "0x0000, 0";
      this.lblHardwareVersion.TextAlign = ContentAlignment.MiddleLeft;
      this.label28.Location = new Point(150, 153);
      this.label28.Name = "label28";
      this.label28.Size = new Size(10, 15);
      this.label28.TabIndex = 93;
      this.label28.Tag = (object) "";
      this.label28.Text = ".";
      this.label28.TextAlign = ContentAlignment.MiddleRight;
      this.txtHardwareVersion_k.Location = new Point(160, 148);
      this.txtHardwareVersion_k.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtHardwareVersion_k.Name = "txtHardwareVersion_k";
      this.txtHardwareVersion_k.Size = new Size(40, 20);
      this.txtHardwareVersion_k.TabIndex = 92;
      this.txtHardwareVersion_k.ValueChanged += new System.EventHandler(this.txtHardwareVersion_ValueChanged);
      this.label26.Location = new Point(5, 128);
      this.label26.Name = "label26";
      this.label26.Size = new Size(102, 15);
      this.label26.TabIndex = 91;
      this.label26.Tag = (object) "";
      this.label26.Text = "SAP number:";
      this.label26.TextAlign = ContentAlignment.MiddleRight;
      this.txtSapOrderNumber.Location = new Point(108, (int) sbyte.MaxValue);
      this.txtSapOrderNumber.Maximum = new Decimal(new int[4]
      {
        1048576,
        0,
        0,
        0
      });
      this.txtSapOrderNumber.Name = "txtSapOrderNumber";
      this.txtSapOrderNumber.Size = new Size(92, 20);
      this.txtSapOrderNumber.TabIndex = 90;
      this.cboxMBusMedium.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMedium.FormattingEnabled = true;
      this.cboxMBusMedium.Location = new Point(108, 82);
      this.cboxMBusMedium.Name = "cboxMBusMedium";
      this.cboxMBusMedium.Size = new Size(92, 21);
      this.cboxMBusMedium.TabIndex = 89;
      this.cboxMBusMedium.SelectedIndexChanged += new System.EventHandler(this.cboxMBusMedium_SelectedIndexChanged);
      this.label24.Location = new Point(5, 84);
      this.label24.Name = "label24";
      this.label24.Size = new Size(102, 15);
      this.label24.TabIndex = 88;
      this.label24.Tag = (object) "cfg_mbus_medium";
      this.label24.Text = "M-Bus medium:";
      this.label24.TextAlign = ContentAlignment.MiddleRight;
      this.txtMBusGeneration.Location = new Point(108, 61);
      this.txtMBusGeneration.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusGeneration.Name = "txtMBusGeneration";
      this.txtMBusGeneration.Size = new Size(92, 20);
      this.txtMBusGeneration.TabIndex = 87;
      this.label23.Location = new Point(5, 62);
      this.label23.Name = "label23";
      this.label23.Size = new Size(102, 15);
      this.label23.TabIndex = 86;
      this.label23.Tag = (object) "";
      this.label23.Text = "M-Bus generation:";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.txtManufacturer.Location = new Point(108, 40);
      this.txtManufacturer.MaxLength = 3;
      this.txtManufacturer.Name = "txtManufacturer";
      this.txtManufacturer.Size = new Size(92, 20);
      this.txtManufacturer.TabIndex = 85;
      this.label33.Location = new Point(5, 41);
      this.label33.Name = "label33";
      this.label33.Size = new Size(102, 15);
      this.label33.TabIndex = 84;
      this.label33.Tag = (object) "";
      this.label33.Text = "Manufacturer:";
      this.label33.TextAlign = ContentAlignment.MiddleRight;
      this.txtSerialnumber.Location = new Point(108, 19);
      this.txtSerialnumber.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumber.Name = "txtSerialnumber";
      this.txtSerialnumber.Size = new Size(92, 20);
      this.txtSerialnumber.TabIndex = 83;
      this.label32.Location = new Point(5, 20);
      this.label32.Name = "label32";
      this.label32.Size = new Size(102, 15);
      this.label32.TabIndex = 82;
      this.label32.Tag = (object) "";
      this.label32.Text = "Serial number:";
      this.label32.TextAlign = ContentAlignment.MiddleRight;
      this.label27.Location = new Point(5, 149);
      this.label27.Name = "label27";
      this.label27.Size = new Size(102, 15);
      this.label27.TabIndex = 81;
      this.label27.Tag = (object) "";
      this.label27.Text = "Hardware version:";
      this.label27.TextAlign = ContentAlignment.MiddleRight;
      this.txtHardwareVersion_i.Location = new Point(108, 148);
      this.txtHardwareVersion_i.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtHardwareVersion_i.Name = "txtHardwareVersion_i";
      this.txtHardwareVersion_i.Size = new Size(40, 20);
      this.txtHardwareVersion_i.TabIndex = 6;
      this.txtHardwareVersion_i.ValueChanged += new System.EventHandler(this.txtHardwareVersion_ValueChanged);
      this.label25.Location = new Point(5, 106);
      this.label25.Name = "label25";
      this.label25.Size = new Size(102, 15);
      this.label25.TabIndex = 79;
      this.label25.Tag = (object) "";
      this.label25.Text = "HardwareTypeID:";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.txtHardwareTypeID.Location = new Point(108, 105);
      this.txtHardwareTypeID.Maximum = new Decimal(new int[4]
      {
        4096,
        0,
        0,
        0
      });
      this.txtHardwareTypeID.Name = "txtHardwareTypeID";
      this.txtHardwareTypeID.Size = new Size(92, 20);
      this.txtHardwareTypeID.TabIndex = 5;
      this.label21.Location = new Point(230, 85);
      this.label21.Name = "label21";
      this.label21.Size = new Size(88, 15);
      this.label21.TabIndex = 77;
      this.label21.Tag = (object) "";
      this.label21.Text = "Device identity:";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.txtDeviceIdentity.Location = new Point(319, 84);
      this.txtDeviceIdentity.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtDeviceIdentity.Name = "txtDeviceIdentity";
      this.txtDeviceIdentity.ReadOnly = true;
      this.txtDeviceIdentity.Size = new Size(51, 20);
      this.txtDeviceIdentity.TabIndex = 4;
      this.label20.Location = new Point(234, 63);
      this.label20.Name = "label20";
      this.label20.Size = new Size(84, 15);
      this.label20.TabIndex = 75;
      this.label20.Tag = (object) "";
      this.label20.Text = "Revision:";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.txtRevision.Location = new Point(319, 62);
      this.txtRevision.Maximum = new Decimal(new int[4]
      {
        4096,
        0,
        0,
        0
      });
      this.txtRevision.Name = "txtRevision";
      this.txtRevision.ReadOnly = true;
      this.txtRevision.Size = new Size(51, 20);
      this.txtRevision.TabIndex = 3;
      this.label19.Location = new Point(234, 42);
      this.label19.Name = "label19";
      this.label19.Size = new Size(84, 15);
      this.label19.TabIndex = 73;
      this.label19.Tag = (object) "";
      this.label19.Text = "Subversion:";
      this.label19.TextAlign = ContentAlignment.MiddleRight;
      this.txtSubversion.Location = new Point(319, 41);
      this.txtSubversion.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtSubversion.Name = "txtSubversion";
      this.txtSubversion.ReadOnly = true;
      this.txtSubversion.Size = new Size(51, 20);
      this.txtSubversion.TabIndex = 2;
      this.label13.Location = new Point(234, 20);
      this.label13.Name = "label13";
      this.label13.Size = new Size(84, 15);
      this.label13.TabIndex = 71;
      this.label13.Tag = (object) "";
      this.label13.Text = "Version number:";
      this.label13.TextAlign = ContentAlignment.MiddleRight;
      this.txtVersionNumber.Location = new Point(319, 19);
      this.txtVersionNumber.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtVersionNumber.Name = "txtVersionNumber";
      this.txtVersionNumber.ReadOnly = true;
      this.txtVersionNumber.Size = new Size(51, 20);
      this.txtVersionNumber.TabIndex = 1;
      this.toolTip.AutoPopDelay = 10000;
      this.toolTip.InitialDelay = 500;
      this.toolTip.ReshowDelay = 100;
      this.toolTip.ShowAlways = true;
      this.label36.BackColor = Color.White;
      this.label36.Location = new Point(203, 7);
      this.label36.Name = "label36";
      this.label36.Size = new Size(84, 15);
      this.label36.TabIndex = 51;
      this.label36.Text = "Handler object:";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(295, 5);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 21);
      this.cboxHandlerObject.TabIndex = 50;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel.Controls.Add((Control) this.gboxManufacturingParameter);
      this.panel.Controls.Add((Control) this.groupBox6);
      this.panel.Controls.Add((Control) this.gboxTestModeParameter);
      this.panel.Controls.Add((Control) this.groupBox1);
      this.panel.Controls.Add((Control) this.btnCancel);
      this.panel.Controls.Add((Control) this.btnSave);
      this.panel.Controls.Add((Control) this.gboxVersion);
      this.panel.Location = new Point(0, 36);
      this.panel.Name = "panel";
      this.panel.Size = new Size(797, 557);
      this.panel.TabIndex = 52;
      this.gboxManufacturingParameter.Controls.Add((Control) this.label16);
      this.gboxManufacturingParameter.Controls.Add((Control) this.txtPCBnumber);
      this.gboxManufacturingParameter.Controls.Add((Control) this.txtPCBdate);
      this.gboxManufacturingParameter.Controls.Add((Control) this.label15);
      this.gboxManufacturingParameter.Controls.Add((Control) this.label14);
      this.gboxManufacturingParameter.Controls.Add((Control) this.txtMeterInfoID);
      this.gboxManufacturingParameter.Controls.Add((Control) this.label11);
      this.gboxManufacturingParameter.Controls.Add((Control) this.txtMeterID);
      this.gboxManufacturingParameter.Location = new Point(422, (int) sbyte.MaxValue);
      this.gboxManufacturingParameter.Name = "gboxManufacturingParameter";
      this.gboxManufacturingParameter.Size = new Size(368, 79);
      this.gboxManufacturingParameter.TabIndex = 85;
      this.gboxManufacturingParameter.TabStop = false;
      this.gboxManufacturingParameter.Text = "Manufacturing parameter";
      this.label16.Location = new Point(195, 47);
      this.label16.Name = "label16";
      this.label16.Size = new Size(73, 15);
      this.label16.TabIndex = 87;
      this.label16.Tag = (object) "";
      this.label16.Text = "PCB number:";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.txtPCBnumber.Location = new Point(269, 46);
      this.txtPCBnumber.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtPCBnumber.Name = "txtPCBnumber";
      this.txtPCBnumber.Size = new Size(91, 20);
      this.txtPCBnumber.TabIndex = 86;
      this.txtPCBdate.Format = DateTimePickerFormat.Short;
      this.txtPCBdate.Location = new Point(269, 20);
      this.txtPCBdate.Name = "txtPCBdate";
      this.txtPCBdate.Size = new Size(91, 20);
      this.txtPCBdate.TabIndex = 84;
      this.label15.Location = new Point(196, 21);
      this.label15.Name = "label15";
      this.label15.Size = new Size(72, 15);
      this.label15.TabIndex = 85;
      this.label15.Tag = (object) "";
      this.label15.Text = "PCB date:";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.label14.Location = new Point(8, 47);
      this.label14.Name = "label14";
      this.label14.Size = new Size(67, 15);
      this.label14.TabIndex = 83;
      this.label14.Tag = (object) "";
      this.label14.Text = "MeterInfoID:";
      this.label14.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterInfoID.Location = new Point(76, 46);
      this.txtMeterInfoID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterInfoID.Name = "txtMeterInfoID";
      this.txtMeterInfoID.Size = new Size(105, 20);
      this.txtMeterInfoID.TabIndex = 82;
      this.label11.Location = new Point(8, 21);
      this.label11.Name = "label11";
      this.label11.Size = new Size(67, 15);
      this.label11.TabIndex = 81;
      this.label11.Tag = (object) "";
      this.label11.Text = "MeterID:";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterID.Location = new Point(76, 20);
      this.txtMeterID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterID.Name = "txtMeterID";
      this.txtMeterID.Size = new Size(105, 20);
      this.txtMeterID.TabIndex = 80;
      this.groupBox6.Controls.Add((Control) this.listCurrentStateOfEvents);
      this.groupBox6.Location = new Point(422, 214);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new Size(368, 286);
      this.groupBox6.TabIndex = 84;
      this.groupBox6.TabStop = false;
      this.groupBox6.Tag = (object) "";
      this.groupBox6.Text = "Current state of events";
      this.listCurrentStateOfEvents.CheckOnClick = true;
      this.listCurrentStateOfEvents.Dock = DockStyle.Fill;
      this.listCurrentStateOfEvents.FormattingEnabled = true;
      this.listCurrentStateOfEvents.Location = new Point(3, 16);
      this.listCurrentStateOfEvents.Name = "listCurrentStateOfEvents";
      this.listCurrentStateOfEvents.Size = new Size(362, 267);
      this.listCurrentStateOfEvents.TabIndex = 0;
      this.gboxTestModeParameter.Controls.Add((Control) this.txtInternalDiagnosticParameter);
      this.gboxTestModeParameter.Controls.Add((Control) this.cboxRadioMode);
      this.gboxTestModeParameter.Controls.Add((Control) this.cboxFunctionTestMode);
      this.gboxTestModeParameter.Controls.Add((Control) this.label17);
      this.gboxTestModeParameter.Controls.Add((Control) this.label18);
      this.gboxTestModeParameter.Controls.Add((Control) this.label22);
      this.gboxTestModeParameter.Location = new Point(422, 5);
      this.gboxTestModeParameter.Name = "gboxTestModeParameter";
      this.gboxTestModeParameter.Size = new Size(368, 116);
      this.gboxTestModeParameter.TabIndex = 83;
      this.gboxTestModeParameter.TabStop = false;
      this.gboxTestModeParameter.Text = "Test mode parameter";
      this.txtInternalDiagnosticParameter.Location = new Point(13, 88);
      this.txtInternalDiagnosticParameter.Name = "txtInternalDiagnosticParameter";
      this.txtInternalDiagnosticParameter.Size = new Size(349, 20);
      this.txtInternalDiagnosticParameter.TabIndex = 21;
      this.cboxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioMode.FormattingEnabled = true;
      this.cboxRadioMode.Location = new Point(122, 15);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(240, 21);
      this.cboxRadioMode.TabIndex = 19;
      this.cboxFunctionTestMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxFunctionTestMode.FormattingEnabled = true;
      this.cboxFunctionTestMode.Location = new Point(122, 42);
      this.cboxFunctionTestMode.Name = "cboxFunctionTestMode";
      this.cboxFunctionTestMode.Size = new Size(240, 21);
      this.cboxFunctionTestMode.TabIndex = 20;
      this.label17.Location = new Point(8, 70);
      this.label17.Name = "label17";
      this.label17.Size = new Size(152, 15);
      this.label17.TabIndex = 75;
      this.label17.Tag = (object) "";
      this.label17.Text = "Internal diagnostic parameter";
      this.label17.TextAlign = ContentAlignment.MiddleRight;
      this.label18.Location = new Point(10, 44);
      this.label18.Name = "label18";
      this.label18.Size = new Size(107, 15);
      this.label18.TabIndex = 73;
      this.label18.Tag = (object) "";
      this.label18.Text = "Function test mode:";
      this.label18.TextAlign = ContentAlignment.MiddleRight;
      this.label22.Location = new Point(10, 18);
      this.label22.Name = "label22";
      this.label22.Size = new Size(107, 15);
      this.label22.TabIndex = 71;
      this.label22.Tag = (object) "";
      this.label22.Text = "Radio mode:";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.groupBox1.Controls.Add((Control) this.cboxRadioProtocol);
      this.groupBox1.Controls.Add((Control) this.btnCalculateEpsilonValue);
      this.groupBox1.Controls.Add((Control) this.txtCurrentDatetime);
      this.groupBox1.Controls.Add((Control) this.txtDateOfFirstActivation);
      this.groupBox1.Controls.Add((Control) this.label12);
      this.groupBox1.Controls.Add((Control) this.txtTypeOfTelegram);
      this.groupBox1.Controls.Add((Control) this.label10);
      this.groupBox1.Controls.Add((Control) this.label9);
      this.groupBox1.Controls.Add((Control) this.txtStatusByteRadioProtocol);
      this.groupBox1.Controls.Add((Control) this.label8);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.txtRadioLibraryVersionNumber);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.txtNetworkAddressForInterlinkedSmokeAlarms);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.txtRadioFrequencyAdjustment);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtRadioTransmitPower);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.txtEpsilonValue);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.txtRadioTransmitInterval);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Location = new Point(6, 188);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(409, 312);
      this.groupBox1.TabIndex = 83;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Operating parameter";
      this.cboxRadioProtocol.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioProtocol.FormattingEnabled = true;
      this.cboxRadioProtocol.Location = new Point(231, 199);
      this.cboxRadioProtocol.Name = "cboxRadioProtocol";
      this.cboxRadioProtocol.Size = new Size(112, 21);
      this.cboxRadioProtocol.TabIndex = 95;
      this.btnCalculateEpsilonValue.Location = new Point(342, 67);
      this.btnCalculateEpsilonValue.Name = "btnCalculateEpsilonValue";
      this.btnCalculateEpsilonValue.Size = new Size(63, 23);
      this.btnCalculateEpsilonValue.TabIndex = 94;
      this.btnCalculateEpsilonValue.Text = "Calculate";
      this.btnCalculateEpsilonValue.UseVisualStyleBackColor = true;
      this.btnCalculateEpsilonValue.Click += new System.EventHandler(this.btnCalculateEpsilonValue_Click);
      this.txtCurrentDatetime.CustomFormat = "dd.MM.yy HH:mm";
      this.txtCurrentDatetime.Format = DateTimePickerFormat.Custom;
      this.txtCurrentDatetime.Location = new Point(231, 252);
      this.txtCurrentDatetime.Name = "txtCurrentDatetime";
      this.txtCurrentDatetime.Size = new Size(112, 20);
      this.txtCurrentDatetime.TabIndex = 16;
      this.txtDateOfFirstActivation.Format = DateTimePickerFormat.Short;
      this.txtDateOfFirstActivation.Location = new Point(231, 17);
      this.txtDateOfFirstActivation.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.txtDateOfFirstActivation.Name = "txtDateOfFirstActivation";
      this.txtDateOfFirstActivation.Size = new Size(112, 20);
      this.txtDateOfFirstActivation.TabIndex = 7;
      this.label12.Location = new Point(5, 279);
      this.label12.Name = "label12";
      this.label12.Size = new Size(226, 15);
      this.label12.TabIndex = 93;
      this.label12.Tag = (object) "";
      this.label12.Text = "Type of telegram:";
      this.label12.TextAlign = ContentAlignment.MiddleRight;
      this.txtTypeOfTelegram.Location = new Point(231, 278);
      this.txtTypeOfTelegram.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtTypeOfTelegram.Name = "txtTypeOfTelegram";
      this.txtTypeOfTelegram.Size = new Size(112, 20);
      this.txtTypeOfTelegram.TabIndex = 18;
      this.label10.Location = new Point(5, 253);
      this.label10.Name = "label10";
      this.label10.Size = new Size(226, 15);
      this.label10.TabIndex = 89;
      this.label10.Tag = (object) "";
      this.label10.Text = "Current datetime:";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.label9.Location = new Point(5, 227);
      this.label9.Name = "label9";
      this.label9.Size = new Size(226, 15);
      this.label9.TabIndex = 87;
      this.label9.Tag = (object) "";
      this.label9.Text = "Status byte (radio protocol):";
      this.label9.TextAlign = ContentAlignment.MiddleRight;
      this.txtStatusByteRadioProtocol.Location = new Point(231, 226);
      this.txtStatusByteRadioProtocol.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtStatusByteRadioProtocol.Name = "txtStatusByteRadioProtocol";
      this.txtStatusByteRadioProtocol.Size = new Size(112, 20);
      this.txtStatusByteRadioProtocol.TabIndex = 15;
      this.label8.Location = new Point(5, 201);
      this.label8.Name = "label8";
      this.label8.Size = new Size(226, 15);
      this.label8.TabIndex = 85;
      this.label8.Tag = (object) "";
      this.label8.Text = "Radio protocol:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.label7.Location = new Point(5, 175);
      this.label7.Name = "label7";
      this.label7.Size = new Size(226, 15);
      this.label7.TabIndex = 83;
      this.label7.Tag = (object) "";
      this.label7.Text = "Radio library version number (dec):";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioLibraryVersionNumber.Location = new Point(231, 174);
      this.txtRadioLibraryVersionNumber.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtRadioLibraryVersionNumber.Name = "txtRadioLibraryVersionNumber";
      this.txtRadioLibraryVersionNumber.Size = new Size(112, 20);
      this.txtRadioLibraryVersionNumber.TabIndex = 13;
      this.label1.Location = new Point(5, 149);
      this.label1.Name = "label1";
      this.label1.Size = new Size(226, 15);
      this.label1.TabIndex = 81;
      this.label1.Tag = (object) "";
      this.label1.Text = "Network address for interlinked smoke alarms:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.txtNetworkAddressForInterlinkedSmokeAlarms.Location = new Point(231, 148);
      this.txtNetworkAddressForInterlinkedSmokeAlarms.Maximum = new Decimal(new int[4]
      {
        16777216,
        0,
        0,
        0
      });
      this.txtNetworkAddressForInterlinkedSmokeAlarms.Name = "txtNetworkAddressForInterlinkedSmokeAlarms";
      this.txtNetworkAddressForInterlinkedSmokeAlarms.Size = new Size(112, 20);
      this.txtNetworkAddressForInterlinkedSmokeAlarms.TabIndex = 12;
      this.label2.Location = new Point(5, 123);
      this.label2.Name = "label2";
      this.label2.Size = new Size(226, 15);
      this.label2.TabIndex = 79;
      this.label2.Tag = (object) "";
      this.label2.Text = "Radio frequency adjustment:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioFrequencyAdjustment.Location = new Point(231, 122);
      this.txtRadioFrequencyAdjustment.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtRadioFrequencyAdjustment.Name = "txtRadioFrequencyAdjustment";
      this.txtRadioFrequencyAdjustment.Size = new Size(112, 20);
      this.txtRadioFrequencyAdjustment.TabIndex = 11;
      this.label3.Location = new Point(5, 97);
      this.label3.Name = "label3";
      this.label3.Size = new Size(226, 15);
      this.label3.TabIndex = 77;
      this.label3.Tag = (object) "";
      this.label3.Text = "Radio transmit power:";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTransmitPower.Location = new Point(231, 96);
      this.txtRadioTransmitPower.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtRadioTransmitPower.Name = "txtRadioTransmitPower";
      this.txtRadioTransmitPower.Size = new Size(112, 20);
      this.txtRadioTransmitPower.TabIndex = 10;
      this.label4.Location = new Point(5, 70);
      this.label4.Name = "label4";
      this.label4.Size = new Size(226, 15);
      this.label4.TabIndex = 75;
      this.label4.Tag = (object) "";
      this.label4.Text = "Epsilon - value:";
      this.label4.TextAlign = ContentAlignment.MiddleRight;
      this.txtEpsilonValue.Location = new Point(231, 69);
      this.txtEpsilonValue.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtEpsilonValue.Name = "txtEpsilonValue";
      this.txtEpsilonValue.Size = new Size(112, 20);
      this.txtEpsilonValue.TabIndex = 9;
      this.label5.Location = new Point(5, 44);
      this.label5.Name = "label5";
      this.label5.Size = new Size(226, 15);
      this.label5.TabIndex = 73;
      this.label5.Tag = (object) "";
      this.label5.Text = "Radio transmit interval:";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTransmitInterval.Location = new Point(231, 43);
      this.txtRadioTransmitInterval.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioTransmitInterval.Name = "txtRadioTransmitInterval";
      this.txtRadioTransmitInterval.Size = new Size(112, 20);
      this.txtRadioTransmitInterval.TabIndex = 8;
      this.label6.Location = new Point(5, 18);
      this.label6.Name = "label6";
      this.label6.Size = new Size(226, 15);
      this.label6.TabIndex = 71;
      this.label6.Tag = (object) "";
      this.label6.Text = "Date of first activation (1.1.2000 is 0xFFFF):";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(797, 36);
      this.zennerCoroprateDesign1.TabIndex = 20;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(797, 592);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Configurator);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (Configurator);
      this.Load += new System.EventHandler(this.Configurator_Load);
      this.gboxVersion.ResumeLayout(false);
      this.gboxVersion.PerformLayout();
      this.txtHardwareVersion_k.EndInit();
      this.txtSapOrderNumber.EndInit();
      this.txtMBusGeneration.EndInit();
      this.txtSerialnumber.EndInit();
      this.txtHardwareVersion_i.EndInit();
      this.txtHardwareTypeID.EndInit();
      this.txtDeviceIdentity.EndInit();
      this.txtRevision.EndInit();
      this.txtSubversion.EndInit();
      this.txtVersionNumber.EndInit();
      this.panel.ResumeLayout(false);
      this.gboxManufacturingParameter.ResumeLayout(false);
      this.txtPCBnumber.EndInit();
      this.txtMeterInfoID.EndInit();
      this.txtMeterID.EndInit();
      this.groupBox6.ResumeLayout(false);
      this.gboxTestModeParameter.ResumeLayout(false);
      this.gboxTestModeParameter.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.txtTypeOfTelegram.EndInit();
      this.txtStatusByteRadioProtocol.EndInit();
      this.txtRadioLibraryVersionNumber.EndInit();
      this.txtNetworkAddressForInterlinkedSmokeAlarms.EndInit();
      this.txtRadioFrequencyAdjustment.EndInit();
      this.txtRadioTransmitPower.EndInit();
      this.txtEpsilonValue.EndInit();
      this.txtRadioTransmitInterval.EndInit();
      this.ResumeLayout(false);
    }
  }
}
