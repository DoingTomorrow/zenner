// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_HandlerWindow
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using CommonWPF;
using CorporateDesign;
using GmmDbLib;
using HandlerLib;
using NLog;
using ReadoutConfiguration;
using S3_Handler.Properties;
using StartupLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class S3_HandlerWindow : Form
  {
    internal static Logger HandlerWindowLogger = LogManager.GetLogger(nameof (S3_HandlerWindow));
    private S3_HandlerFunctions MyFunctions;
    private ParameterViewWindow1 MyParameterView;
    private DiagnoseWindow MyDiagnostic;
    internal string StartComponentName = string.Empty;
    internal string BaseStatusText = string.Empty;
    private DateTime ConnectStartTime = DateTime.MinValue;
    private Color ConnectedColor = Color.LightGreen;
    private Color WaitColor = Color.Yellow;
    private string startFunctionInfo;
    private DateTime startFunctionTime;
    private bool functionResult;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem componentToolStripMenuItem;
    private ToolStripMenuItem MenuItemGlobalMeterManager;
    private ToolStripMenuItem MenuItemBack;
    private ToolStripMenuItem MenuItemQuit;
    private Button buttonReadDevice;
    private Button buttonWriteDevice;
    private Button buttonLoadFromDatabase;
    private Button buttonSaveToDatabase;
    private Button buttonLoadType;
    private Button buttonCreateType;
    private TextBox textBoxDeviceTypeInfo;
    private Label label2;
    private TextBox textBoxStatus;
    private ToolStripSeparator toolStripSeparator1;
    private Button buttonClear;
    private GroupBox groupBoxDeviceDatabaseBackup;
    private GroupBox groupBoxConnectedDevice;
    private GroupBox groupBoxDatabaseType;
    private TextBox textBoxDbDeviceInfo;
    private TextBox textBoxConnectedDeviceInfo;
    private Button buttonWriteClone;
    private Label label1;
    private TextBox textBoxJob;
    private TextBox textBoxWorkDeviceInfo;
    private Label label3;
    private Button ToolBarUndoButton;
    private ComboBox comboBoxUndoList;
    private Button buttonOverwriteWorkFromType;
    private ToolStripMenuItem ToolStripMenuConfigurator;
    private CheckBox checkBoxLockAsBaseType;
    private Button buttonWriteCompatibleDevice;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private SplitContainer splitContainer3;
    private Label labelMiConAlive;
    private Button buttonConnectDevice;
    private Button buttonOverwriteFlow;
    private SplitContainer splitContainer4;
    private GroupBox groupBoxShowEditData;
    private Button buttonCommunicationWindow;
    private Button buttonReadoutConfig;
    private Button buttonShowMenu;
    private Button buttonTransmitParameterLists;
    private Button buttonShowLogger;
    private Button buttonPrint;
    private Button buttonVolumeSamplingMonitor;
    private Button buttonDeviceIdentification;
    private Button buttonConfiguration;
    private Button buttonCalibrationWindow;
    private Button buttonProtection;
    private Button buttonTimeSettings;
    private Button buttonTdcMeasurement;
    private Button buttonAdcMesurement;
    private Button buttonSettings;
    private GroupBox groupBoxDeveloperFunctions;
    private Button btnC5FwUpdate;
    private Button buttonShowVariables;
    private Button buttonShowDiagnostic;
    private Button buttonTestWindow;
    private Button buttonTestWindow2;
    private GroupBox groupBoxCommon32BitCommands;
    private Button buttonTestWindow32BitCommands;
    private Button buttonTestWindowRadioCommands;
    private Button buttonTestWindowMBusCommands;
    private Button buttonTestWindowsLoRaCommands;

    public S3_HandlerWindow(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
      if (this.Size.Width > workingArea.Width || this.Size.Height > workingArea.Height)
        this.Size = new Size(workingArea.Width, workingArea.Height);
      if (Thread.CurrentThread.Name != "GMM main")
        this.Text = this.Text + " [" + Thread.CurrentThread.Name + "]";
      if (!this.MyFunctions.IsPlugin)
      {
        this.componentToolStripMenuItem.Visible = false;
      }
      else
      {
        bool flag = false;
        if (PlugInLoader.IsWindowEnabled("GMM"))
        {
          flag = true;
        }
        else
        {
          this.MenuItemGlobalMeterManager.Visible = false;
          this.MenuItemBack.Visible = false;
          this.MenuItemQuit.Visible = false;
          this.toolStripSeparator1.Visible = false;
        }
        if (PlugInLoader.IsWindowEnabled("Configurator"))
          flag = true;
        else
          this.ToolStripMenuConfigurator.Visible = false;
        if (!flag)
          this.componentToolStripMenuItem.Visible = false;
      }
      this.groupBoxCommon32BitCommands.Enabled = false;
      this.groupBoxDeveloperFunctions.Visible = this.MyFunctions.IsHandlerCompleteEnabled();
      if (!UserManager.CheckPermission(UserManager.Role_Developer))
      {
        this.groupBoxDeveloperFunctions.Visible = false;
        this.buttonSettings.Visible = false;
        this.buttonProtection.Visible = false;
        this.buttonCalibrationWindow.Visible = false;
        this.buttonVolumeSamplingMonitor.Visible = false;
        this.buttonTdcMeasurement.Visible = false;
        this.groupBoxDatabaseType.Enabled = false;
        this.buttonWriteClone.Visible = false;
        this.buttonWriteCompatibleDevice.Visible = false;
      }
      FormTranslatorSupport.TranslateWindow(Tg.S3_HandlerWindow, (Form) this);
    }

    private void S3_HandlerWindow_Load(object sender, EventArgs e)
    {
      this.MyFunctions.OnS3_HandlerMessage += new EventHandler<GMM_EventArgs>(this.WorkReceivedMessages);
      int num = this.splitContainer1.Size.Height / 4;
      this.splitContainer1.SplitterDistance = num * 3;
      this.splitContainer2.SplitterDistance = num * 2;
      this.splitContainer3.SplitterDistance = num;
      this.UpdateAllWindows(true);
    }

    private void WorkReceivedMessages(object sender, GMM_EventArgs TheMessage)
    {
      if (!this.IsHandleCreated)
        return;
      if (this.textBoxStatus.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.WorkReceivedMessages), sender, (object) TheMessage);
        }
        catch
        {
        }
      }
      else
      {
        switch (TheMessage.TheMessageType)
        {
          case GMM_EventArgs.MessageType.Alive:
            switch (this.labelMiConAlive.Text)
            {
              case "|":
                this.labelMiConAlive.Text = "/";
                return;
              case "/":
                this.labelMiConAlive.Text = "-";
                return;
              case "-":
                this.labelMiConAlive.Text = "\\";
                return;
              case "\\":
                this.labelMiConAlive.Text = "|";
                return;
              default:
                return;
            }
          case GMM_EventArgs.MessageType.PrimaryAddressMessage:
            if (TheMessage.InfoText != null)
            {
              this.textBoxStatus.Text = TheMessage.InfoText;
              break;
            }
            this.textBoxStatus.Text = this.BaseStatusText + "0x" + TheMessage.InfoNumber.ToString("X4");
            break;
          case GMM_EventArgs.MessageType.EndMessage:
            this.textBoxStatus.Text = string.Empty;
            break;
          case GMM_EventArgs.MessageType.StatusChanged:
            if (!(sender is S3_HandlerFunctions))
              break;
            if (TheMessage.InfoNumber == 20 || TheMessage.InfoNumber == 21)
              this.UpdateAllWindows(true);
            else
              this.UpdateAllWindows(false);
            break;
          case GMM_EventArgs.MessageType.MessageAndProgressPercentage:
            this.textBoxStatus.Text = "Progress: " + TheMessage.ProgressPercentage.ToString() + "%";
            if (!string.IsNullOrEmpty(TheMessage.InfoText))
            {
              TextBox textBoxStatus = this.textBoxStatus;
              textBoxStatus.Text = textBoxStatus.Text + " - " + TheMessage.InfoText;
            }
            this.Refresh();
            break;
        }
      }
    }

    private void MenuItemGlobalMeterManager_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "GMM";
      this.Hide();
    }

    private void MenuItemBack_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Hide();
    }

    private void MenuItemQuit_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Exit";
      this.Hide();
    }

    private void ToolStripMenuConfigurator_Click(object sender, EventArgs e)
    {
      if (!this.MyFunctions.IsPlugin)
        return;
      if (this.MyFunctions.MyMeters.WorkMeter != null)
      {
        this.MyFunctions.NewConfigurationDataAvailable = true;
        if (PlugInLoader.ConfigListStatic == null)
        {
          PlugInLoader.ConfigListStatic = ReadoutConfigFunctions.GetPartialProfile(17).GetConfigListObject();
        }
        else
        {
          ConnectionProfile partialProfile = ReadoutConfigFunctions.GetPartialProfile(PlugInLoader.ConfigListStatic.ConnectionProfileID);
          if (partialProfile != null && partialProfile.DeviceModel.Parameters != null && partialProfile.DeviceModel.Parameters.ContainsKey(ConnectionProfileParameter.Handler) && partialProfile.DeviceModel.Parameters[ConnectionProfileParameter.Handler] != "S3_Handler")
            PlugInLoader.ConfigListStatic.Reset(ReadoutConfigFunctions.GetPartialProfile(17).GetConfigListObject().GetSortedList());
        }
      }
      this.StartComponentName = "Configurator";
      this.Hide();
    }

    private void buttonClear_Click(object sender, EventArgs e) => this.MyFunctions.Clear();

    private void buttonReadDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Read device");
        this.MyFunctions.MyMeters.ConnectedMeter = (S3_Meter) null;
        this.MyFunctions.MyMeters.WorkMeter = (S3_Meter) null;
        this.textBoxConnectedDeviceInfo.Clear();
        this.textBoxWorkDeviceInfo.Clear();
        Application.DoEvents();
        this.functionResult = this.MyFunctions.ReadConnectedDevice();
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonWriteDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Write device");
        this.functionResult = this.MyFunctions.WriteChangesToConnectedDevice();
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonLoadFromDatabase_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Load from database");
        OpenDevice openDevice = new OpenDevice(this.MyFunctions);
        if (openDevice.ShowDialog() == DialogResult.OK)
        {
          this.MyFunctions.SaveMeterDataTabel = openDevice.SaveMeterDataTabel;
          this.MyFunctions.SelectedSaveMeterDataTableRowIndex = openDevice.SelectedSaveMeterDataTableRowIndex;
          DateTime localTime = openDevice.DataTimePoint.ToLocalTime();
          this.functionResult = this.MyFunctions.OpenDevice(openDevice.MeterId, localTime);
        }
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonSaveToDatabase_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Save to database");
        this.functionResult = this.MyFunctions.MyMeters.SaveDevice();
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonWriteClone_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Write clone");
        this.functionResult = this.MyFunctions.MyMeters.WriteClone();
        this.UpdateAllWindows(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonWriteCompatibleDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Write compatible device");
        this.functionResult = this.MyFunctions.WriteAllToCompatibledDevice();
        this.UpdateAllWindows(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonCreateType_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Create type");
        this.functionResult = new SaveType(this.MyFunctions).ShowDialog() == DialogResult.OK;
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonLoadType_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartFunction("Load type");
        int num = (int) new OpenType(this.MyFunctions).ShowDialog();
        this.functionResult = true;
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonShowMenu_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.MyFunctions.MyMeters.WorkMeter == null)
          return;
        int num = (int) new S3_LCD_Menu((Form) this, this.MyFunctions, this.MyFunctions.MyMeters.WorkMeter).ShowDialog();
        this.UpdateAllWindows();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonShowVariables_Click(object sender, EventArgs e)
    {
      S3_Meter activeMeter = this.MyFunctions.MyMeters.GetActiveMeter();
      if (activeMeter == null)
        return;
      if (this.MyParameterView == null || this.MyParameterView.IsDisposed)
      {
        this.MyParameterView = new ParameterViewWindow1();
        this.MyParameterView.Show();
      }
      else
        this.MyParameterView.Visible = true;
      this.MyParameterView.ShowParameterList(activeMeter);
      this.UpdateAllWindows();
    }

    private void buttonTransmitParameterLists_Click(object sender, EventArgs e)
    {
      if (this.MyFunctions.MyMeters.WorkMeter == null)
        return;
      try
      {
        int num = (int) new TransmitParameterView(this.MyFunctions, this.MyFunctions.MyMeters.WorkMeter).ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateAllWindows();
    }

    private void buttonShowLogger_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyMeters.NewWorkMeter("Change logger");
      try
      {
        if (new LoggerView(this.MyFunctions).ShowDialog() != DialogResult.OK)
          this.MyFunctions.MyMeters.Undo();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Logger editor error");
        ZR_ClassLibMessages.ShowAndClearErrors();
        this.MyFunctions.MyMeters.Undo();
      }
      this.UpdateAllWindows();
    }

    private void buttonShowDiagnostic_Click(object sender, EventArgs e)
    {
      if (this.MyDiagnostic == null || this.MyDiagnostic.IsDisposed)
      {
        this.MyDiagnostic = new DiagnoseWindow(this.MyFunctions);
        this.MyDiagnostic.Owner = (Form) this;
        this.MyDiagnostic.Show();
      }
      else
        this.MyDiagnostic.Visible = true;
    }

    private void StartFunction(string actionInfo)
    {
      this.startFunctionTime = DateTime.Now;
      S3_HandlerWindow.HandlerWindowLogger.Info("StartFunction: " + actionInfo);
      this.startFunctionInfo = actionInfo;
      this.functionResult = false;
      ZR_ClassLibMessages.ClearErrors();
      this.Cursor = Cursors.WaitCursor;
      this.Enabled = false;
    }

    private void UpdateAllWindows() => this.UpdateAllWindows(true);

    private void UpdateAllWindows(bool endUpdate)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      this.ToolBarUndoButton.Enabled = false;
      try
      {
        if (endUpdate)
        {
          this.Cursor = Cursors.WaitCursor;
          ZR_ClassLibMessages.ShowAndClearErrors();
          this.Enabled = true;
          string[] cloneInfo = this.MyFunctions.MyMeters.GetCloneInfo();
          if (cloneInfo == null)
          {
            this.comboBoxUndoList.DataSource = (object) null;
          }
          else
          {
            this.comboBoxUndoList.DataSource = (object) cloneInfo;
            this.comboBoxUndoList.SelectedIndex = 0;
            if (cloneInfo.Length > 1)
              this.ToolBarUndoButton.Enabled = true;
          }
        }
        this.textBoxDeviceTypeInfo.Clear();
        this.textBoxDbDeviceInfo.Clear();
        this.textBoxConnectedDeviceInfo.Clear();
        this.textBoxWorkDeviceInfo.Clear();
        this.textBoxStatus.Refresh();
        Application.DoEvents();
        bool flag1 = this.MyFunctions.MyMeters.WorkMeter != null;
        this.buttonCreateType.Enabled = flag1;
        if (this.MyFunctions.MyMeters.TypeMeter != null)
        {
          this.buttonCreateType.Text = "Overwrite type from work ...";
          stringBuilder1.Length = 0;
          stringBuilder1.AppendLine(new S3_CommonDeviceIdentification(this.MyFunctions.MyMeters.TypeMeter).GetDeviceIdDescription());
          this.textBoxDeviceTypeInfo.Text = stringBuilder1.ToString();
        }
        else
        {
          this.textBoxDeviceTypeInfo.Text = "Type not loaded";
          this.buttonCreateType.Text = "Create new type from work ...";
        }
        this.buttonSaveToDatabase.Enabled = flag1;
        if (this.MyFunctions.MyMeters.DbMeter != null && this.MyFunctions.MyMeters.WorkMeter != null)
        {
          stringBuilder1.Length = 0;
          stringBuilder1.AppendLine(new S3_CommonDeviceIdentification(this.MyFunctions.MyMeters.DbMeter).GetDeviceIdDescription());
          this.textBoxDbDeviceInfo.Text = stringBuilder1.ToString();
          this.buttonWriteClone.Enabled = true;
        }
        else
        {
          this.textBoxDbDeviceInfo.Text = "Backup not loaded";
          this.buttonWriteClone.Enabled = false;
        }
        bool flag2 = false;
        try
        {
          if (ReadoutConfigFunctions.GetPartialProfile(this.MyFunctions.ConfigList.ConnectionProfileID).DeviceModel.Parameters[ConnectionProfileParameter.Handler] == "S3_Handler")
            flag2 = true;
        }
        catch
        {
        }
        if (!flag2)
        {
          this.groupBoxConnectedDevice.Enabled = false;
          this.textBoxConnectedDeviceInfo.Text = "Illegal readout configuration";
        }
        else
        {
          this.groupBoxConnectedDevice.Enabled = true;
          if (this.MyFunctions.MyMeters.ConnectedMeter != null)
          {
            stringBuilder1.Length = 0;
            DateTime generatedTimeStamp1 = this.MyFunctions.MyMeters.ConnectedMeter.MeterObjectGeneratedTimeStamp;
            if (this.MyFunctions.MyMeters.ConnectedMeter.MeterObjectGeneratedTimeStamp != DateTime.MinValue)
            {
              DateTime generatedTimeStamp2 = this.MyFunctions.MyMeters.ConnectedMeter.MeterObjectGeneratedTimeStamp;
              DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.MyFunctions.MyMeters.ConnectedMeter.MeterTimeAsSeconds1980);
              int totalMinutes = (int) generatedTimeStamp2.Subtract(dateTime).TotalMinutes;
              stringBuilder1.Append("Read time: " + generatedTimeStamp2.ToString("dd.MM.yyyy HH.mm.ss"));
              stringBuilder1.Append(" ; Meter time: " + dateTime.ToString("dd.MM.yyyy HH:mm"));
              stringBuilder1.AppendLine(" ; Diff minutes: " + totalMinutes.ToString());
            }
            stringBuilder1.AppendLine(new S3_CommonDeviceIdentification(this.MyFunctions.MyMeters.ConnectedMeter).GetDeviceIdDescription());
            this.textBoxConnectedDeviceInfo.Text = stringBuilder1.ToString();
            this.buttonTimeSettings.Enabled = true;
            this.buttonAdcMesurement.Enabled = true;
            if (this.MyFunctions.MyCommands.SingleParameter(CommParameter.Baudrate, string.Empty) == "115200")
              this.buttonVolumeSamplingMonitor.Enabled = true;
            else
              this.buttonVolumeSamplingMonitor.Enabled = false;
          }
          else
          {
            if (this.MyFunctions.MyCommands is S3_CommandsCHANGED && this.MyFunctions.MyCommands.GetDeviceIdentification() != null)
            {
              DeviceIdentification deviceIdentification = this.MyFunctions.MyCommands.GetDeviceIdentification();
              StringBuilder stringBuilder2 = new StringBuilder();
              stringBuilder2.AppendLine("connected ...");
              stringBuilder2.AppendLine("Firmware:      " + deviceIdentification.FirmwareVersionObj.ToString());
              stringBuilder2.AppendLine("Manufacturer:  " + deviceIdentification.ManufacturerName);
              stringBuilder2.AppendLine("Medium:        " + deviceIdentification.MediumAsString);
              this.textBoxConnectedDeviceInfo.Text = stringBuilder2.ToString();
              this.groupBoxCommon32BitCommands.Enabled = true;
            }
            else
              this.textBoxConnectedDeviceInfo.Text = "Device not connected";
            this.buttonTimeSettings.Enabled = false;
            this.buttonAdcMesurement.Enabled = false;
            this.buttonVolumeSamplingMonitor.Enabled = false;
          }
          this.buttonWriteDevice.Enabled = this.MyFunctions.WriteEnabled;
          this.buttonWriteCompatibleDevice.Enabled = this.MyFunctions.WriteEnabled;
        }
        if (this.MyFunctions.MyMeters.WorkMeter != null)
        {
          if (this.MyParameterView != null && !this.MyParameterView.IsDisposed)
            this.MyParameterView.ShowParameterList(this.MyFunctions.MyMeters.WorkMeter);
          if (this.MyDiagnostic != null && !this.MyDiagnostic.IsDisposed)
            this.MyDiagnostic.UpdateBaseInfos();
          stringBuilder1.Length = 0;
          stringBuilder1.AppendLine(new S3_CommonDeviceIdentification(this.MyFunctions.MyMeters.WorkMeter).GetDeviceIdDescription());
          this.textBoxWorkDeviceInfo.Text = stringBuilder1.ToString();
          this.buttonCreateType.Enabled = true;
          this.buttonOverwriteWorkFromType.Enabled = true;
          this.buttonTransmitParameterLists.Enabled = true;
          this.buttonShowMenu.Enabled = true;
          this.buttonDeviceIdentification.Enabled = true;
          this.buttonShowLogger.Enabled = true;
          this.buttonConfiguration.Enabled = true;
          this.buttonProtection.Enabled = true;
        }
        else
        {
          this.buttonCreateType.Enabled = false;
          this.buttonOverwriteWorkFromType.Enabled = false;
          this.textBoxWorkDeviceInfo.Text = "Work object not available";
          this.buttonTransmitParameterLists.Enabled = false;
          this.buttonShowMenu.Enabled = false;
          this.buttonDeviceIdentification.Enabled = false;
          this.buttonShowLogger.Enabled = false;
          this.buttonConfiguration.Enabled = false;
          this.buttonProtection.Enabled = false;
        }
        if (this.MyFunctions.baseTypeEditMode)
          this.BackColor = Color.LightCyan;
        else
          this.BackColor = Control.DefaultBackColor;
        switch (this.MyFunctions.SelectedCommunicationStructure)
        {
          case S3_HandlerFunctions.CommunicationStructure.classicDeviceCollector:
            this.groupBoxConnectedDevice.BackColor = Color.LightPink;
            this.groupBoxCommon32BitCommands.Enabled = false;
            break;
          case S3_HandlerFunctions.CommunicationStructure.compatible:
            this.groupBoxConnectedDevice.BackColor = Control.DefaultBackColor;
            this.groupBoxCommon32BitCommands.Enabled = true;
            break;
          case S3_HandlerFunctions.CommunicationStructure.commonDefined:
            this.groupBoxConnectedDevice.BackColor = Color.LightSalmon;
            this.groupBoxCommon32BitCommands.Enabled = true;
            break;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_HandlerWindow.HandlerWindowLogger);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Update all Windows", S3_HandlerWindow.HandlerWindowLogger);
      }
      finally
      {
        this.Cursor = Cursors.Default;
        ZR_ClassLibMessages.ShowAndClearErrors();
        if (this.startFunctionInfo != null)
        {
          this.textBoxStatus.Text = "End function: " + this.startFunctionInfo + " ; Result: " + this.functionResult.ToString();
          TimeSpan timeSpan = DateTime.Now.Subtract(this.startFunctionTime);
          if (timeSpan < new TimeSpan(1, 0, 0))
            this.textBoxStatus.AppendText(" ; Duration: " + timeSpan.TotalSeconds.ToString("0.000"));
          this.startFunctionTime = DateTime.MinValue;
          this.startFunctionInfo = (string) null;
          this.functionResult = false;
          S3_HandlerWindow.HandlerWindowLogger.Info(this.textBoxStatus.Text);
        }
      }
    }

    private void buttonAdcMesurement_Click(object sender, EventArgs e)
    {
      if (this.MyFunctions.MyMeters.ConnectedMeter == null)
        return;
      if (this.MyFunctions.adc_Measurement == null)
        this.MyFunctions.adc_Measurement = new ADC_Measurement(this.MyFunctions);
      int num = (int) this.MyFunctions.adc_Measurement.ShowDialog();
      this.UpdateAllWindows();
    }

    private void buttonTdcMeasurement_Click(object sender, EventArgs e)
    {
      if (this.MyFunctions.tdc_Measurement == null)
        this.MyFunctions.tdc_Measurement = new TDC_Measurement(this.MyFunctions);
      int num = (int) this.MyFunctions.tdc_Measurement.ShowDialog();
      this.UpdateAllWindows();
    }

    private void buttonTestWindow_Click(object sender, EventArgs e)
    {
      int num = (int) new TestWindow(this.MyFunctions, this.MyFunctions.MyMeters.WorkMeter).ShowDialog();
      this.UpdateAllWindows();
    }

    private void buttonSettings_Click(object sender, EventArgs e)
    {
      int num = (int) new Settings(this.MyFunctions, this.MyFunctions.MyMeters.WorkMeter).ShowDialog();
      this.UpdateAllWindows();
    }

    private void buttonVolumeSamplingMonitor_Click(object sender, EventArgs e)
    {
      int num = (int) new MeterSamplingMonitor(this.MyFunctions).ShowDialog();
    }

    private void ToolBarUndoButton_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyMeters.Undo();
      this.UpdateAllWindows(true);
    }

    private void buttonOverwriteWorkFromType_Click(object sender, EventArgs e)
    {
      int num = (int) new OverwriteFromTypeSelection(this.MyFunctions).ShowDialog((IWin32Window) this);
      this.UpdateAllWindows();
    }

    private void buttonTimeSettings_Click(object sender, EventArgs e)
    {
      int num = (int) new TimeSettings(this.MyFunctions).ShowDialog();
      this.UpdateAllWindows();
    }

    private void buttonTestWindow2_Click(object sender, EventArgs e)
    {
      int num = (int) new MoreTests(this.MyFunctions, this.MyFunctions.MyMeters.WorkMeter).ShowDialog();
      this.UpdateAllWindows(true);
    }

    private void helpToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void checkBoxLockAsBaseType_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.lockLoadedTypeAsBaseType = this.checkBoxLockAsBaseType.Checked;
    }

    private void buttonCalibrationWindow_Click(object sender, EventArgs e)
    {
      int num = (int) new CalibrationWindow(this.MyFunctions).ShowDialog();
    }

    private void buttonProtection_Click(object sender, EventArgs e)
    {
      int num = (int) new DeviceProtection(this.MyFunctions).ShowDialog();
      this.UpdateAllWindows();
    }

    private void btnC5FwUpdate_Click(object sender, EventArgs e)
    {
      this.StartFunction("Update C5 Firmware");
      this.functionResult = new C5_FwUpdate(this.MyFunctions).ShowDialog() == DialogResult.OK;
      this.UpdateAllWindows();
    }

    private void buttonTestWindow32BitCommands_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.ShowTestWindow(CommonCommandWindowsName.COMMON);
    }

    private void buttonTestWindowMBusCommands_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.ShowTestWindow(CommonCommandWindowsName.MBUS);
    }

    private void buttonTestWindowsLoRaCommands_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.ShowTestWindow(CommonCommandWindowsName.LORA);
    }

    private void buttonTestWindowRadioCommands_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.ShowTestWindow(CommonCommandWindowsName.RADIO);
    }

    private void buttonReadoutConfig_Click(object sender, EventArgs e)
    {
      try
      {
        ReadoutConfigFunctions.ChooseConfiguration(new ReadoutPreferences(this.MyFunctions.MyCommands.GetReadoutConfiguration(), ConnectionProfileFilter.CreateHandlerFilter("S3_Handler")));
        this.UpdateAllWindows(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonCommunicationWindow_Click(object sender, EventArgs e)
    {
      try
      {
        this.MyFunctions.MyCommands.ShowCommunicatioWindow();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      this.MyFunctions.MyCommands.Close();
      base.OnClosed(e);
    }

    private void buttonDeviceIdentification_Click(object sender, EventArgs e)
    {
      try
      {
        new ChangeIdentWindow(this.MyFunctions.GetDeviceIdentification()).ShowDialog();
        this.UpdateAllWindows(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonConfiguration_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.MyFunctions.MyMeters.WorkMeter == null)
          return;
        ConfiguratorWindow.ShowDialog((IHandler) this.MyFunctions);
        this.UpdateAllWindows(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonPrint_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.MyFunctions.MyMeters.WorkMeter == null)
          return;
        this.MyFunctions.Print((string) null);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonConnectDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.MyFunctions.ConnectDevice();
        this.UpdateAllWindows(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }

    private void buttonOverwriteInfo_Click(object sender, EventArgs e)
    {
      if (this.MyFunctions.MyMeters.WorkMeter == null)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Overwrite Flow:" + Environment.NewLine);
      int level = 0;
      foreach (OverwriteHistoryItem overwriteHistoryItem in this.MyFunctions.MyMeters.WorkMeter.overwriteHistorie)
        stringBuilder.Append(overwriteHistoryItem.ToString(ref level));
      if (stringBuilder.Length > 0)
        GmmMessage.Show(stringBuilder.ToString(), "--- Overwrite flow ---" + Environment.NewLine + Environment.NewLine, true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (S3_HandlerWindow));
      this.menuStrip1 = new MenuStrip();
      this.componentToolStripMenuItem = new ToolStripMenuItem();
      this.MenuItemGlobalMeterManager = new ToolStripMenuItem();
      this.MenuItemBack = new ToolStripMenuItem();
      this.MenuItemQuit = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.ToolStripMenuConfigurator = new ToolStripMenuItem();
      this.buttonReadDevice = new Button();
      this.buttonWriteDevice = new Button();
      this.buttonLoadFromDatabase = new Button();
      this.buttonSaveToDatabase = new Button();
      this.buttonLoadType = new Button();
      this.buttonCreateType = new Button();
      this.textBoxDeviceTypeInfo = new TextBox();
      this.label2 = new Label();
      this.textBoxStatus = new TextBox();
      this.buttonClear = new Button();
      this.groupBoxDeviceDatabaseBackup = new GroupBox();
      this.buttonWriteClone = new Button();
      this.textBoxDbDeviceInfo = new TextBox();
      this.groupBoxConnectedDevice = new GroupBox();
      this.buttonConnectDevice = new Button();
      this.buttonWriteCompatibleDevice = new Button();
      this.textBoxConnectedDeviceInfo = new TextBox();
      this.groupBoxDatabaseType = new GroupBox();
      this.buttonOverwriteWorkFromType = new Button();
      this.checkBoxLockAsBaseType = new CheckBox();
      this.label1 = new Label();
      this.textBoxJob = new TextBox();
      this.textBoxWorkDeviceInfo = new TextBox();
      this.label3 = new Label();
      this.comboBoxUndoList = new ComboBox();
      this.splitContainer1 = new SplitContainer();
      this.splitContainer2 = new SplitContainer();
      this.splitContainer3 = new SplitContainer();
      this.buttonOverwriteFlow = new Button();
      this.ToolBarUndoButton = new Button();
      this.labelMiConAlive = new Label();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.splitContainer4 = new SplitContainer();
      this.groupBoxShowEditData = new GroupBox();
      this.buttonCommunicationWindow = new Button();
      this.buttonReadoutConfig = new Button();
      this.buttonShowMenu = new Button();
      this.buttonTransmitParameterLists = new Button();
      this.buttonShowLogger = new Button();
      this.buttonPrint = new Button();
      this.buttonVolumeSamplingMonitor = new Button();
      this.buttonDeviceIdentification = new Button();
      this.buttonConfiguration = new Button();
      this.buttonCalibrationWindow = new Button();
      this.buttonProtection = new Button();
      this.buttonTimeSettings = new Button();
      this.buttonTdcMeasurement = new Button();
      this.buttonAdcMesurement = new Button();
      this.buttonSettings = new Button();
      this.groupBoxDeveloperFunctions = new GroupBox();
      this.btnC5FwUpdate = new Button();
      this.buttonShowVariables = new Button();
      this.buttonShowDiagnostic = new Button();
      this.buttonTestWindow = new Button();
      this.buttonTestWindow2 = new Button();
      this.groupBoxCommon32BitCommands = new GroupBox();
      this.buttonTestWindow32BitCommands = new Button();
      this.buttonTestWindowRadioCommands = new Button();
      this.buttonTestWindowMBusCommands = new Button();
      this.buttonTestWindowsLoRaCommands = new Button();
      this.menuStrip1.SuspendLayout();
      this.groupBoxDeviceDatabaseBackup.SuspendLayout();
      this.groupBoxConnectedDevice.SuspendLayout();
      this.groupBoxDatabaseType.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.splitContainer3.BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.splitContainer4.BeginInit();
      this.splitContainer4.Panel1.SuspendLayout();
      this.splitContainer4.Panel2.SuspendLayout();
      this.splitContainer4.SuspendLayout();
      this.groupBoxShowEditData.SuspendLayout();
      this.groupBoxDeveloperFunctions.SuspendLayout();
      this.groupBoxCommon32BitCommands.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip1.ImageScalingSize = new Size(24, 24);
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.componentToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Padding = new Padding(4, 2, 0, 2);
      this.menuStrip1.Size = new Size(907, 24);
      this.menuStrip1.TabIndex = 16;
      this.menuStrip1.Text = "menuStrip1";
      this.componentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.MenuItemGlobalMeterManager,
        (ToolStripItem) this.MenuItemBack,
        (ToolStripItem) this.MenuItemQuit,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.ToolStripMenuConfigurator
      });
      this.componentToolStripMenuItem.Name = "componentToolStripMenuItem";
      this.componentToolStripMenuItem.Size = new Size(83, 20);
      this.componentToolStripMenuItem.Text = "Component";
      this.MenuItemGlobalMeterManager.Name = "MenuItemGlobalMeterManager";
      this.MenuItemGlobalMeterManager.Size = new Size(186, 22);
      this.MenuItemGlobalMeterManager.Text = "GlobalMeterManager";
      this.MenuItemGlobalMeterManager.Click += new System.EventHandler(this.MenuItemGlobalMeterManager_Click);
      this.MenuItemBack.Name = "MenuItemBack";
      this.MenuItemBack.Size = new Size(186, 22);
      this.MenuItemBack.Text = "Back";
      this.MenuItemBack.Click += new System.EventHandler(this.MenuItemBack_Click);
      this.MenuItemQuit.Name = "MenuItemQuit";
      this.MenuItemQuit.Size = new Size(186, 22);
      this.MenuItemQuit.Text = "Quit";
      this.MenuItemQuit.Click += new System.EventHandler(this.MenuItemQuit_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(183, 6);
      this.ToolStripMenuConfigurator.Name = "ToolStripMenuConfigurator";
      this.ToolStripMenuConfigurator.Size = new Size(186, 22);
      this.ToolStripMenuConfigurator.Text = "Configurator";
      this.ToolStripMenuConfigurator.Click += new System.EventHandler(this.ToolStripMenuConfigurator_Click);
      this.buttonReadDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonReadDevice.Location = new Point(485, 51);
      this.buttonReadDevice.Name = "buttonReadDevice";
      this.buttonReadDevice.Size = new Size(187, 23);
      this.buttonReadDevice.TabIndex = 17;
      this.buttonReadDevice.Text = "Read device";
      this.buttonReadDevice.UseVisualStyleBackColor = true;
      this.buttonReadDevice.Click += new System.EventHandler(this.buttonReadDevice_Click);
      this.buttonWriteDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonWriteDevice.Enabled = false;
      this.buttonWriteDevice.Location = new Point(485, 79);
      this.buttonWriteDevice.Name = "buttonWriteDevice";
      this.buttonWriteDevice.Size = new Size(187, 23);
      this.buttonWriteDevice.TabIndex = 17;
      this.buttonWriteDevice.Text = "Write device";
      this.buttonWriteDevice.UseVisualStyleBackColor = true;
      this.buttonWriteDevice.Click += new System.EventHandler(this.buttonWriteDevice_Click);
      this.buttonLoadFromDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonLoadFromDatabase.Location = new Point(485, 21);
      this.buttonLoadFromDatabase.Name = "buttonLoadFromDatabase";
      this.buttonLoadFromDatabase.Size = new Size(187, 23);
      this.buttonLoadFromDatabase.TabIndex = 17;
      this.buttonLoadFromDatabase.Text = "Load from database ...";
      this.buttonLoadFromDatabase.UseVisualStyleBackColor = true;
      this.buttonLoadFromDatabase.Click += new System.EventHandler(this.buttonLoadFromDatabase_Click);
      this.buttonSaveToDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSaveToDatabase.Enabled = false;
      this.buttonSaveToDatabase.Location = new Point(485, 50);
      this.buttonSaveToDatabase.Name = "buttonSaveToDatabase";
      this.buttonSaveToDatabase.Size = new Size(187, 23);
      this.buttonSaveToDatabase.TabIndex = 17;
      this.buttonSaveToDatabase.Text = "Save work to database";
      this.buttonSaveToDatabase.UseVisualStyleBackColor = true;
      this.buttonSaveToDatabase.Click += new System.EventHandler(this.buttonSaveToDatabase_Click);
      this.buttonLoadType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonLoadType.Location = new Point(485, 21);
      this.buttonLoadType.Name = "buttonLoadType";
      this.buttonLoadType.Size = new Size(187, 23);
      this.buttonLoadType.TabIndex = 17;
      this.buttonLoadType.Text = "Load from database ...";
      this.buttonLoadType.UseVisualStyleBackColor = true;
      this.buttonLoadType.Click += new System.EventHandler(this.buttonLoadType_Click);
      this.buttonCreateType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCreateType.Location = new Point(485, 50);
      this.buttonCreateType.Name = "buttonCreateType";
      this.buttonCreateType.Size = new Size(187, 23);
      this.buttonCreateType.TabIndex = 17;
      this.buttonCreateType.Text = "Save work as type to database ...";
      this.buttonCreateType.UseVisualStyleBackColor = true;
      this.buttonCreateType.Click += new System.EventHandler(this.buttonCreateType_Click);
      this.textBoxDeviceTypeInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxDeviceTypeInfo.Location = new Point(6, 16);
      this.textBoxDeviceTypeInfo.Multiline = true;
      this.textBoxDeviceTypeInfo.Name = "textBoxDeviceTypeInfo";
      this.textBoxDeviceTypeInfo.ReadOnly = true;
      this.textBoxDeviceTypeInfo.ScrollBars = ScrollBars.Vertical;
      this.textBoxDeviceTypeInfo.Size = new Size(473, 124);
      this.textBoxDeviceTypeInfo.TabIndex = 19;
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(251, 686);
      this.label2.Name = "label2";
      this.label2.Size = new Size(37, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Status";
      this.label2.Click += new System.EventHandler(this.label2_Click);
      this.textBoxStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxStatus.Location = new Point(294, 682);
      this.textBoxStatus.Name = "textBoxStatus";
      this.textBoxStatus.ReadOnly = true;
      this.textBoxStatus.Size = new Size(523, 20);
      this.textBoxStatus.TabIndex = 20;
      this.buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClear.Location = new Point(825, 682);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(71, 23);
      this.buttonClear.TabIndex = 17;
      this.buttonClear.Text = "Clear all";
      this.buttonClear.UseVisualStyleBackColor = true;
      this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
      this.groupBoxDeviceDatabaseBackup.Controls.Add((Control) this.buttonLoadFromDatabase);
      this.groupBoxDeviceDatabaseBackup.Controls.Add((Control) this.buttonWriteClone);
      this.groupBoxDeviceDatabaseBackup.Controls.Add((Control) this.buttonSaveToDatabase);
      this.groupBoxDeviceDatabaseBackup.Controls.Add((Control) this.textBoxDbDeviceInfo);
      this.groupBoxDeviceDatabaseBackup.Dock = DockStyle.Fill;
      this.groupBoxDeviceDatabaseBackup.Location = new Point(0, 0);
      this.groupBoxDeviceDatabaseBackup.Name = "groupBoxDeviceDatabaseBackup";
      this.groupBoxDeviceDatabaseBackup.Size = new Size(677, 145);
      this.groupBoxDeviceDatabaseBackup.TabIndex = 21;
      this.groupBoxDeviceDatabaseBackup.TabStop = false;
      this.groupBoxDeviceDatabaseBackup.Text = "Device backup";
      this.buttonWriteClone.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonWriteClone.Enabled = false;
      this.buttonWriteClone.Location = new Point(485, 117);
      this.buttonWriteClone.Name = "buttonWriteClone";
      this.buttonWriteClone.Size = new Size(187, 23);
      this.buttonWriteClone.TabIndex = 17;
      this.buttonWriteClone.Text = "Write as clone to connected device";
      this.buttonWriteClone.UseVisualStyleBackColor = true;
      this.buttonWriteClone.Click += new System.EventHandler(this.buttonWriteClone_Click);
      this.textBoxDbDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxDbDeviceInfo.Location = new Point(6, 21);
      this.textBoxDbDeviceInfo.Multiline = true;
      this.textBoxDbDeviceInfo.Name = "textBoxDbDeviceInfo";
      this.textBoxDbDeviceInfo.ReadOnly = true;
      this.textBoxDbDeviceInfo.ScrollBars = ScrollBars.Vertical;
      this.textBoxDbDeviceInfo.Size = new Size(473, 119);
      this.textBoxDbDeviceInfo.TabIndex = 19;
      this.groupBoxConnectedDevice.Controls.Add((Control) this.buttonConnectDevice);
      this.groupBoxConnectedDevice.Controls.Add((Control) this.buttonReadDevice);
      this.groupBoxConnectedDevice.Controls.Add((Control) this.buttonWriteCompatibleDevice);
      this.groupBoxConnectedDevice.Controls.Add((Control) this.buttonWriteDevice);
      this.groupBoxConnectedDevice.Controls.Add((Control) this.textBoxConnectedDeviceInfo);
      this.groupBoxConnectedDevice.Cursor = Cursors.Default;
      this.groupBoxConnectedDevice.Dock = DockStyle.Fill;
      this.groupBoxConnectedDevice.Location = new Point(0, 0);
      this.groupBoxConnectedDevice.Name = "groupBoxConnectedDevice";
      this.groupBoxConnectedDevice.Size = new Size(677, 146);
      this.groupBoxConnectedDevice.TabIndex = 23;
      this.groupBoxConnectedDevice.TabStop = false;
      this.groupBoxConnectedDevice.Text = "Connected device";
      this.buttonConnectDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonConnectDevice.Location = new Point(485, 19);
      this.buttonConnectDevice.Name = "buttonConnectDevice";
      this.buttonConnectDevice.Size = new Size(187, 23);
      this.buttonConnectDevice.TabIndex = 17;
      this.buttonConnectDevice.Text = "Connect device";
      this.buttonConnectDevice.UseVisualStyleBackColor = true;
      this.buttonConnectDevice.Click += new System.EventHandler(this.buttonConnectDevice_Click);
      this.buttonWriteCompatibleDevice.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonWriteCompatibleDevice.Enabled = false;
      this.buttonWriteCompatibleDevice.Location = new Point(485, 109);
      this.buttonWriteCompatibleDevice.Name = "buttonWriteCompatibleDevice";
      this.buttonWriteCompatibleDevice.Size = new Size(187, 23);
      this.buttonWriteCompatibleDevice.TabIndex = 17;
      this.buttonWriteCompatibleDevice.Text = "Write compatible device";
      this.buttonWriteCompatibleDevice.UseVisualStyleBackColor = true;
      this.buttonWriteCompatibleDevice.Click += new System.EventHandler(this.buttonWriteCompatibleDevice_Click);
      this.textBoxConnectedDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxConnectedDeviceInfo.Location = new Point(6, 19);
      this.textBoxConnectedDeviceInfo.Multiline = true;
      this.textBoxConnectedDeviceInfo.Name = "textBoxConnectedDeviceInfo";
      this.textBoxConnectedDeviceInfo.ReadOnly = true;
      this.textBoxConnectedDeviceInfo.ScrollBars = ScrollBars.Vertical;
      this.textBoxConnectedDeviceInfo.Size = new Size(473, 113);
      this.textBoxConnectedDeviceInfo.TabIndex = 19;
      this.groupBoxDatabaseType.Controls.Add((Control) this.buttonLoadType);
      this.groupBoxDatabaseType.Controls.Add((Control) this.buttonOverwriteWorkFromType);
      this.groupBoxDatabaseType.Controls.Add((Control) this.buttonCreateType);
      this.groupBoxDatabaseType.Controls.Add((Control) this.textBoxDeviceTypeInfo);
      this.groupBoxDatabaseType.Controls.Add((Control) this.checkBoxLockAsBaseType);
      this.groupBoxDatabaseType.Dock = DockStyle.Fill;
      this.groupBoxDatabaseType.Location = new Point(0, 0);
      this.groupBoxDatabaseType.Name = "groupBoxDatabaseType";
      this.groupBoxDatabaseType.Size = new Size(677, 144);
      this.groupBoxDatabaseType.TabIndex = 24;
      this.groupBoxDatabaseType.TabStop = false;
      this.groupBoxDatabaseType.Text = "Type";
      this.buttonOverwriteWorkFromType.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOverwriteWorkFromType.Location = new Point(485, 110);
      this.buttonOverwriteWorkFromType.Name = "buttonOverwriteWorkFromType";
      this.buttonOverwriteWorkFromType.Size = new Size(187, 23);
      this.buttonOverwriteWorkFromType.TabIndex = 17;
      this.buttonOverwriteWorkFromType.Text = "Overwrite work (from type) ...";
      this.buttonOverwriteWorkFromType.UseVisualStyleBackColor = true;
      this.buttonOverwriteWorkFromType.Click += new System.EventHandler(this.buttonOverwriteWorkFromType_Click);
      this.checkBoxLockAsBaseType.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.checkBoxLockAsBaseType.AutoSize = true;
      this.checkBoxLockAsBaseType.Location = new Point(555, 90);
      this.checkBoxLockAsBaseType.Name = "checkBoxLockAsBaseType";
      this.checkBoxLockAsBaseType.RightToLeft = RightToLeft.Yes;
      this.checkBoxLockAsBaseType.Size = new Size(113, 17);
      this.checkBoxLockAsBaseType.TabIndex = 20;
      this.checkBoxLockAsBaseType.Text = "Lock as base type";
      this.checkBoxLockAsBaseType.UseVisualStyleBackColor = true;
      this.checkBoxLockAsBaseType.CheckedChanged += new System.EventHandler(this.checkBoxLockAsBaseType_CheckedChanged);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 686);
      this.label1.Name = "label1";
      this.label1.Size = new Size(24, 13);
      this.label1.TabIndex = 18;
      this.label1.Text = "Job";
      this.textBoxJob.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxJob.Location = new Point(50, 682);
      this.textBoxJob.Name = "textBoxJob";
      this.textBoxJob.ReadOnly = true;
      this.textBoxJob.Size = new Size(170, 20);
      this.textBoxJob.TabIndex = 25;
      this.textBoxWorkDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxWorkDeviceInfo.Location = new Point(48, 6);
      this.textBoxWorkDeviceInfo.Multiline = true;
      this.textBoxWorkDeviceInfo.Name = "textBoxWorkDeviceInfo";
      this.textBoxWorkDeviceInfo.ReadOnly = true;
      this.textBoxWorkDeviceInfo.ScrollBars = ScrollBars.Vertical;
      this.textBoxWorkDeviceInfo.Size = new Size(621, 134);
      this.textBoxWorkDeviceInfo.TabIndex = 19;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(36, 13);
      this.label3.TabIndex = 18;
      this.label3.Text = "Work:";
      this.comboBoxUndoList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxUndoList.FormattingEnabled = true;
      this.comboBoxUndoList.Location = new Point(224, 29);
      this.comboBoxUndoList.Name = "comboBoxUndoList";
      this.comboBoxUndoList.Size = new Size(214, 21);
      this.comboBoxUndoList.TabIndex = 45;
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(12, 72);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Panel1MinSize = 300;
      this.splitContainer1.Panel2.Controls.Add((Control) this.buttonOverwriteFlow);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label3);
      this.splitContainer1.Panel2.Controls.Add((Control) this.textBoxWorkDeviceInfo);
      this.splitContainer1.Panel2MinSize = 100;
      this.splitContainer1.Size = new Size(677, 602);
      this.splitContainer1.SplitterDistance = 443;
      this.splitContainer1.TabIndex = 46;
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.splitContainer3);
      this.splitContainer2.Panel1MinSize = 200;
      this.splitContainer2.Panel2.Controls.Add((Control) this.groupBoxConnectedDevice);
      this.splitContainer2.Panel2MinSize = 100;
      this.splitContainer2.Size = new Size(677, 443);
      this.splitContainer2.SplitterDistance = 293;
      this.splitContainer2.TabIndex = 46;
      this.splitContainer3.Dock = DockStyle.Fill;
      this.splitContainer3.Location = new Point(0, 0);
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Orientation = Orientation.Horizontal;
      this.splitContainer3.Panel1.Controls.Add((Control) this.groupBoxDatabaseType);
      this.splitContainer3.Panel1MinSize = 100;
      this.splitContainer3.Panel2.Controls.Add((Control) this.groupBoxDeviceDatabaseBackup);
      this.splitContainer3.Panel2MinSize = 100;
      this.splitContainer3.Size = new Size(677, 293);
      this.splitContainer3.SplitterDistance = 144;
      this.splitContainer3.TabIndex = 46;
      this.buttonOverwriteFlow.Location = new Point(12, 34);
      this.buttonOverwriteFlow.Margin = new Padding(2, 2, 2, 2);
      this.buttonOverwriteFlow.Name = "buttonOverwriteFlow";
      this.buttonOverwriteFlow.Size = new Size(31, 16);
      this.buttonOverwriteFlow.TabIndex = 20;
      this.buttonOverwriteFlow.Text = "OF";
      this.buttonOverwriteFlow.UseVisualStyleBackColor = true;
      this.buttonOverwriteFlow.Click += new System.EventHandler(this.buttonOverwriteInfo_Click);
      this.ToolBarUndoButton.Image = (Image) Resources.UndoIcon;
      this.ToolBarUndoButton.ImeMode = ImeMode.NoControl;
      this.ToolBarUndoButton.Location = new Point(196, 27);
      this.ToolBarUndoButton.Margin = new Padding(0);
      this.ToolBarUndoButton.Name = "ToolBarUndoButton";
      this.ToolBarUndoButton.Size = new Size(24, 24);
      this.ToolBarUndoButton.TabIndex = 44;
      this.ToolBarUndoButton.Click += new System.EventHandler(this.ToolBarUndoButton_Click);
      this.labelMiConAlive.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.labelMiConAlive.AutoSize = true;
      this.labelMiConAlive.Location = new Point(226, 687);
      this.labelMiConAlive.Name = "labelMiConAlive";
      this.labelMiConAlive.Size = new Size(9, 13);
      this.labelMiConAlive.TabIndex = 18;
      this.labelMiConAlive.Text = "|";
      this.labelMiConAlive.TextAlign = ContentAlignment.MiddleRight;
      this.zennerCoroprateDesign2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.zennerCoroprateDesign2.Location = new Point(0, 29);
      this.zennerCoroprateDesign2.Margin = new Padding(2, 2, 2, 2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(907, 28);
      this.zennerCoroprateDesign2.TabIndex = 15;
      this.splitContainer4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.splitContainer4.Location = new Point(699, 73);
      this.splitContainer4.Margin = new Padding(2, 2, 2, 2);
      this.splitContainer4.Name = "splitContainer4";
      this.splitContainer4.Orientation = Orientation.Horizontal;
      this.splitContainer4.Panel1.Controls.Add((Control) this.groupBoxShowEditData);
      this.splitContainer4.Panel2.Controls.Add((Control) this.buttonSettings);
      this.splitContainer4.Panel2.Controls.Add((Control) this.groupBoxDeveloperFunctions);
      this.splitContainer4.Panel2.Controls.Add((Control) this.groupBoxCommon32BitCommands);
      this.splitContainer4.Size = new Size(195, 600);
      this.splitContainer4.SplitterDistance = 321;
      this.splitContainer4.SplitterWidth = 3;
      this.splitContainer4.TabIndex = 47;
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonCommunicationWindow);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonReadoutConfig);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonShowMenu);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonTransmitParameterLists);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonShowLogger);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonPrint);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonVolumeSamplingMonitor);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonDeviceIdentification);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonConfiguration);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonCalibrationWindow);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonProtection);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonTimeSettings);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonTdcMeasurement);
      this.groupBoxShowEditData.Controls.Add((Control) this.buttonAdcMesurement);
      this.groupBoxShowEditData.Dock = DockStyle.Fill;
      this.groupBoxShowEditData.Location = new Point(0, 0);
      this.groupBoxShowEditData.Name = "groupBoxShowEditData";
      this.groupBoxShowEditData.Size = new Size(195, 321);
      this.groupBoxShowEditData.TabIndex = 23;
      this.groupBoxShowEditData.TabStop = false;
      this.groupBoxShowEditData.Text = "Show/Edit data";
      this.buttonCommunicationWindow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonCommunicationWindow.Location = new Point(2, 48);
      this.buttonCommunicationWindow.Name = "buttonCommunicationWindow";
      this.buttonCommunicationWindow.Size = new Size(186, 23);
      this.buttonCommunicationWindow.TabIndex = 17;
      this.buttonCommunicationWindow.Text = "Communication window";
      this.buttonCommunicationWindow.UseVisualStyleBackColor = true;
      this.buttonCommunicationWindow.Click += new System.EventHandler(this.buttonCommunicationWindow_Click);
      this.buttonReadoutConfig.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonReadoutConfig.Location = new Point(2, 19);
      this.buttonReadoutConfig.Name = "buttonReadoutConfig";
      this.buttonReadoutConfig.Size = new Size(186, 23);
      this.buttonReadoutConfig.TabIndex = 17;
      this.buttonReadoutConfig.Text = "Readout configuration";
      this.buttonReadoutConfig.UseVisualStyleBackColor = true;
      this.buttonReadoutConfig.Click += new System.EventHandler(this.buttonReadoutConfig_Click);
      this.buttonShowMenu.Location = new Point(2, 77);
      this.buttonShowMenu.Name = "buttonShowMenu";
      this.buttonShowMenu.Size = new Size(90, 23);
      this.buttonShowMenu.TabIndex = 17;
      this.buttonShowMenu.Text = "LCD menu";
      this.buttonShowMenu.UseVisualStyleBackColor = true;
      this.buttonShowMenu.Click += new System.EventHandler(this.buttonShowMenu_Click);
      this.buttonTransmitParameterLists.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonTransmitParameterLists.Location = new Point(2, 102);
      this.buttonTransmitParameterLists.Name = "buttonTransmitParameterLists";
      this.buttonTransmitParameterLists.Size = new Size(186, 23);
      this.buttonTransmitParameterLists.TabIndex = 17;
      this.buttonTransmitParameterLists.Text = "Transmit paramter lists";
      this.buttonTransmitParameterLists.UseVisualStyleBackColor = true;
      this.buttonTransmitParameterLists.Click += new System.EventHandler(this.buttonTransmitParameterLists_Click);
      this.buttonShowLogger.Location = new Point(2, 152);
      this.buttonShowLogger.Name = "buttonShowLogger";
      this.buttonShowLogger.Size = new Size(90, 23);
      this.buttonShowLogger.TabIndex = 17;
      this.buttonShowLogger.Text = "Logger";
      this.buttonShowLogger.UseVisualStyleBackColor = true;
      this.buttonShowLogger.Click += new System.EventHandler(this.buttonShowLogger_Click);
      this.buttonPrint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonPrint.Location = new Point(2, 277);
      this.buttonPrint.Name = "buttonPrint";
      this.buttonPrint.Size = new Size(186, 23);
      this.buttonPrint.TabIndex = 17;
      this.buttonPrint.Text = "Print";
      this.buttonPrint.UseVisualStyleBackColor = true;
      this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
      this.buttonVolumeSamplingMonitor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonVolumeSamplingMonitor.Location = new Point(2, 252);
      this.buttonVolumeSamplingMonitor.Name = "buttonVolumeSamplingMonitor";
      this.buttonVolumeSamplingMonitor.Size = new Size(186, 23);
      this.buttonVolumeSamplingMonitor.TabIndex = 17;
      this.buttonVolumeSamplingMonitor.Text = "Volume sampling monitor";
      this.buttonVolumeSamplingMonitor.UseVisualStyleBackColor = true;
      this.buttonVolumeSamplingMonitor.Click += new System.EventHandler(this.buttonVolumeSamplingMonitor_Click);
      this.buttonDeviceIdentification.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonDeviceIdentification.Location = new Point(99, 77);
      this.buttonDeviceIdentification.Name = "buttonDeviceIdentification";
      this.buttonDeviceIdentification.Size = new Size(90, 23);
      this.buttonDeviceIdentification.TabIndex = 17;
      this.buttonDeviceIdentification.Text = "Identification";
      this.buttonDeviceIdentification.UseVisualStyleBackColor = true;
      this.buttonDeviceIdentification.Click += new System.EventHandler(this.buttonDeviceIdentification_Click);
      this.buttonConfiguration.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonConfiguration.Location = new Point(99, 152);
      this.buttonConfiguration.Name = "buttonConfiguration";
      this.buttonConfiguration.Size = new Size(90, 23);
      this.buttonConfiguration.TabIndex = 17;
      this.buttonConfiguration.Text = "Configuration";
      this.buttonConfiguration.UseVisualStyleBackColor = true;
      this.buttonConfiguration.Click += new System.EventHandler(this.buttonConfiguration_Click);
      this.buttonCalibrationWindow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonCalibrationWindow.Location = new Point(99, 177);
      this.buttonCalibrationWindow.Name = "buttonCalibrationWindow";
      this.buttonCalibrationWindow.Size = new Size(90, 23);
      this.buttonCalibrationWindow.TabIndex = 17;
      this.buttonCalibrationWindow.Text = "Calibrate";
      this.buttonCalibrationWindow.UseVisualStyleBackColor = true;
      this.buttonCalibrationWindow.Click += new System.EventHandler(this.buttonCalibrationWindow_Click);
      this.buttonProtection.Location = new Point(2, 177);
      this.buttonProtection.Name = "buttonProtection";
      this.buttonProtection.Size = new Size(90, 23);
      this.buttonProtection.TabIndex = 17;
      this.buttonProtection.Text = "Protection";
      this.buttonProtection.UseVisualStyleBackColor = true;
      this.buttonProtection.Click += new System.EventHandler(this.buttonProtection_Click);
      this.buttonTimeSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonTimeSettings.Location = new Point(2, (int) sbyte.MaxValue);
      this.buttonTimeSettings.Name = "buttonTimeSettings";
      this.buttonTimeSettings.Size = new Size(186, 23);
      this.buttonTimeSettings.TabIndex = 17;
      this.buttonTimeSettings.Text = "Times and device locking";
      this.buttonTimeSettings.UseVisualStyleBackColor = true;
      this.buttonTimeSettings.Click += new System.EventHandler(this.buttonTimeSettings_Click);
      this.buttonTdcMeasurement.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonTdcMeasurement.Location = new Point(2, 227);
      this.buttonTdcMeasurement.Name = "buttonTdcMeasurement";
      this.buttonTdcMeasurement.Size = new Size(186, 23);
      this.buttonTdcMeasurement.TabIndex = 17;
      this.buttonTdcMeasurement.Text = "Volume measurement";
      this.buttonTdcMeasurement.UseVisualStyleBackColor = true;
      this.buttonTdcMeasurement.Click += new System.EventHandler(this.buttonTdcMeasurement_Click);
      this.buttonAdcMesurement.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonAdcMesurement.Location = new Point(2, 202);
      this.buttonAdcMesurement.Name = "buttonAdcMesurement";
      this.buttonAdcMesurement.Size = new Size(186, 23);
      this.buttonAdcMesurement.TabIndex = 17;
      this.buttonAdcMesurement.Text = "Temperature measurement";
      this.buttonAdcMesurement.UseVisualStyleBackColor = true;
      this.buttonAdcMesurement.Click += new System.EventHandler(this.buttonAdcMesurement_Click);
      this.buttonSettings.Dock = DockStyle.Bottom;
      this.buttonSettings.Location = new Point(0, 253);
      this.buttonSettings.Name = "buttonSettings";
      this.buttonSettings.Size = new Size(195, 23);
      this.buttonSettings.TabIndex = 22;
      this.buttonSettings.Text = "Handler settings";
      this.buttonSettings.UseVisualStyleBackColor = true;
      this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
      this.groupBoxDeveloperFunctions.Controls.Add((Control) this.btnC5FwUpdate);
      this.groupBoxDeveloperFunctions.Controls.Add((Control) this.buttonShowVariables);
      this.groupBoxDeveloperFunctions.Controls.Add((Control) this.buttonShowDiagnostic);
      this.groupBoxDeveloperFunctions.Controls.Add((Control) this.buttonTestWindow);
      this.groupBoxDeveloperFunctions.Controls.Add((Control) this.buttonTestWindow2);
      this.groupBoxDeveloperFunctions.Location = new Point(4, 86);
      this.groupBoxDeveloperFunctions.Name = "groupBoxDeveloperFunctions";
      this.groupBoxDeveloperFunctions.Size = new Size(185, 144);
      this.groupBoxDeveloperFunctions.TabIndex = 21;
      this.groupBoxDeveloperFunctions.TabStop = false;
      this.groupBoxDeveloperFunctions.Text = "Developer functions";
      this.btnC5FwUpdate.Location = new Point(6, 118);
      this.btnC5FwUpdate.Name = "btnC5FwUpdate";
      this.btnC5FwUpdate.Size = new Size(173, 23);
      this.btnC5FwUpdate.TabIndex = 18;
      this.btnC5FwUpdate.Text = "Firmware update";
      this.btnC5FwUpdate.UseVisualStyleBackColor = true;
      this.btnC5FwUpdate.Click += new System.EventHandler(this.btnC5FwUpdate_Click);
      this.buttonShowVariables.Location = new Point(6, 18);
      this.buttonShowVariables.Name = "buttonShowVariables";
      this.buttonShowVariables.Size = new Size(173, 23);
      this.buttonShowVariables.TabIndex = 17;
      this.buttonShowVariables.Text = "Firmware parameter";
      this.buttonShowVariables.UseVisualStyleBackColor = true;
      this.buttonShowVariables.Click += new System.EventHandler(this.buttonShowVariables_Click);
      this.buttonShowDiagnostic.Location = new Point(6, 93);
      this.buttonShowDiagnostic.Name = "buttonShowDiagnostic";
      this.buttonShowDiagnostic.Size = new Size(173, 23);
      this.buttonShowDiagnostic.TabIndex = 17;
      this.buttonShowDiagnostic.Text = "Diagnostic";
      this.buttonShowDiagnostic.UseVisualStyleBackColor = true;
      this.buttonShowDiagnostic.Click += new System.EventHandler(this.buttonShowDiagnostic_Click);
      this.buttonTestWindow.Location = new Point(6, 43);
      this.buttonTestWindow.Name = "buttonTestWindow";
      this.buttonTestWindow.Size = new Size(173, 23);
      this.buttonTestWindow.TabIndex = 17;
      this.buttonTestWindow.Text = "Test window 1";
      this.buttonTestWindow.UseVisualStyleBackColor = true;
      this.buttonTestWindow.Click += new System.EventHandler(this.buttonTestWindow_Click);
      this.buttonTestWindow2.Location = new Point(6, 68);
      this.buttonTestWindow2.Name = "buttonTestWindow2";
      this.buttonTestWindow2.Size = new Size(173, 23);
      this.buttonTestWindow2.TabIndex = 17;
      this.buttonTestWindow2.Text = "Test window 2";
      this.buttonTestWindow2.UseVisualStyleBackColor = true;
      this.buttonTestWindow2.Click += new System.EventHandler(this.buttonTestWindow2_Click);
      this.groupBoxCommon32BitCommands.Controls.Add((Control) this.buttonTestWindow32BitCommands);
      this.groupBoxCommon32BitCommands.Controls.Add((Control) this.buttonTestWindowRadioCommands);
      this.groupBoxCommon32BitCommands.Controls.Add((Control) this.buttonTestWindowMBusCommands);
      this.groupBoxCommon32BitCommands.Controls.Add((Control) this.buttonTestWindowsLoRaCommands);
      this.groupBoxCommon32BitCommands.Dock = DockStyle.Top;
      this.groupBoxCommon32BitCommands.Location = new Point(0, 0);
      this.groupBoxCommon32BitCommands.Name = "groupBoxCommon32BitCommands";
      this.groupBoxCommon32BitCommands.Size = new Size(195, 77);
      this.groupBoxCommon32BitCommands.TabIndex = 20;
      this.groupBoxCommon32BitCommands.TabStop = false;
      this.groupBoxCommon32BitCommands.Text = "Commands Test Windows";
      this.buttonTestWindow32BitCommands.Location = new Point(6, 18);
      this.buttonTestWindow32BitCommands.Name = "buttonTestWindow32BitCommands";
      this.buttonTestWindow32BitCommands.Size = new Size(82, 23);
      this.buttonTestWindow32BitCommands.TabIndex = 17;
      this.buttonTestWindow32BitCommands.Text = "Common";
      this.buttonTestWindow32BitCommands.UseVisualStyleBackColor = true;
      this.buttonTestWindow32BitCommands.Click += new System.EventHandler(this.buttonTestWindow32BitCommands_Click);
      this.buttonTestWindowRadioCommands.Location = new Point(94, 42);
      this.buttonTestWindowRadioCommands.Name = "buttonTestWindowRadioCommands";
      this.buttonTestWindowRadioCommands.Size = new Size(82, 23);
      this.buttonTestWindowRadioCommands.TabIndex = 17;
      this.buttonTestWindowRadioCommands.Text = "Radio";
      this.buttonTestWindowRadioCommands.UseVisualStyleBackColor = true;
      this.buttonTestWindowRadioCommands.Click += new System.EventHandler(this.buttonTestWindowRadioCommands_Click);
      this.buttonTestWindowMBusCommands.Location = new Point(94, 18);
      this.buttonTestWindowMBusCommands.Name = "buttonTestWindowMBusCommands";
      this.buttonTestWindowMBusCommands.Size = new Size(82, 23);
      this.buttonTestWindowMBusCommands.TabIndex = 17;
      this.buttonTestWindowMBusCommands.Text = "MBus";
      this.buttonTestWindowMBusCommands.UseVisualStyleBackColor = true;
      this.buttonTestWindowMBusCommands.Click += new System.EventHandler(this.buttonTestWindowMBusCommands_Click);
      this.buttonTestWindowsLoRaCommands.Location = new Point(6, 42);
      this.buttonTestWindowsLoRaCommands.Name = "buttonTestWindowsLoRaCommands";
      this.buttonTestWindowsLoRaCommands.Size = new Size(82, 23);
      this.buttonTestWindowsLoRaCommands.TabIndex = 17;
      this.buttonTestWindowsLoRaCommands.Text = "LoRa";
      this.buttonTestWindowsLoRaCommands.UseVisualStyleBackColor = true;
      this.buttonTestWindowsLoRaCommands.Click += new System.EventHandler(this.buttonTestWindowsLoRaCommands_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(907, 715);
      this.Controls.Add((Control) this.splitContainer4);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.comboBoxUndoList);
      this.Controls.Add((Control) this.ToolBarUndoButton);
      this.Controls.Add((Control) this.textBoxJob);
      this.Controls.Add((Control) this.buttonClear);
      this.Controls.Add((Control) this.textBoxStatus);
      this.Controls.Add((Control) this.labelMiConAlive);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Controls.Add((Control) this.menuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new Padding(2, 2, 2, 2);
      this.MinimumSize = new Size(539, 469);
      this.Name = nameof (S3_HandlerWindow);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "S3_Handler";
      this.Load += new System.EventHandler(this.S3_HandlerWindow_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBoxDeviceDatabaseBackup.ResumeLayout(false);
      this.groupBoxDeviceDatabaseBackup.PerformLayout();
      this.groupBoxConnectedDevice.ResumeLayout(false);
      this.groupBoxConnectedDevice.PerformLayout();
      this.groupBoxDatabaseType.ResumeLayout(false);
      this.groupBoxDatabaseType.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      this.splitContainer3.EndInit();
      this.splitContainer3.ResumeLayout(false);
      this.splitContainer4.Panel1.ResumeLayout(false);
      this.splitContainer4.Panel2.ResumeLayout(false);
      this.splitContainer4.EndInit();
      this.splitContainer4.ResumeLayout(false);
      this.groupBoxShowEditData.ResumeLayout(false);
      this.groupBoxDeveloperFunctions.ResumeLayout(false);
      this.groupBoxCommon32BitCommands.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
