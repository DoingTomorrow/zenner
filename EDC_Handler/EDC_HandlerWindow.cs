// Decompiled with JetBrains decompiler
// Type: EDC_Handler.EDC_HandlerWindow
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using CorporateDesign;
using GmmDbLib;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public class EDC_HandlerWindow : Form
  {
    private static Logger logger = LogManager.GetLogger(nameof (EDC_HandlerWindow));
    internal string NextComponentName = "Exit";
    private EDC_HandlerFunctions handler;
    private bool isCanceled;
    private Stopwatch stopwatch;
    private const string translaterBaseKey = "EDC_HandlerWindow";
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private MenuStrip menuStrip;
    private ToolStripMenuItem ToolStripMenuItemComponents;
    private GroupBox gboxTypeDevice;
    private Button btnOverwriteWorkFromType;
    private TextBox txtTypeInfo;
    private Button btnClear;
    private GroupBox gboxConnectedDevice;
    private TextBox txtConnectedDeviceInfo;
    private GroupBox groupBoxShowEditData;
    private Button btnParameter;
    private Button btnMBusList;
    private Button btnLogger;
    private GroupBox gboxBackupDevice;
    private Button btnLoadBackup;
    private Button btnCreateBackup;
    private TextBox txtBackupDeviceInfo;
    private Button btnTestDialog;
    private StatusStrip statusStrip;
    private ToolStripProgressBar progress;
    private ToolStripStatusLabel lblStatus;
    private Button btnVolumeMonitor;
    private Button btnConfigurator;
    private Button btnTypeEditor;
    private CheckBox ckboxReadLoggerToo;
    private Button btnFirmware;
    private Button btnReadDevice;
    private Button btnWriteDevice;
    private CheckBox cboxReadCyclic;
    private GroupBox gboxWorkDevice;
    private TextBox txtWorkDeviceInfo;
    private Button btnZoomWork;
    private Button btnZoomType;
    private Button btnZoomConnected;
    private Button btnZoomDeviceBackup;
    private Button btnSimulator;
    private Button button_HardwareTypeEditor;

    public EDC_HandlerWindow(EDC_HandlerFunctions handler)
    {
      this.InitializeComponent();
      this.handler = handler;
      Version version = typeof (EDC_HandlerWindow).Assembly.GetName().Version;
      this.Text = string.Format("EDC Handler v{0}.{1}.{2}", (object) version.Major, (object) version.Minor, (object) version.Build);
      this.ToolStripMenuItemComponents.DropDownItems.AddRange(handler.GetComponentMenuItems());
      this.menuStrip.Visible = true;
      this.btnWriteDevice.Enabled = false;
      this.stopwatch = new Stopwatch();
      FormTranslatorSupport.TranslateWindow(Tg.EDC_HandlerWindow, (Form) this);
      this.ResetUI();
    }

    private void EDC_HandlerWindow_Load(object sender, EventArgs e)
    {
      this.UpdateUI();
      this.handler.MyDeviceCollector.OnMessage += new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      this.handler.OnProgress += new ValueEventHandler<int>(this.MyFunctions_OnProgress);
    }

    private void EDC_HandlerWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.handler.MyDeviceCollector != null)
        this.handler.MyDeviceCollector.OnMessage -= new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      this.handler.OnProgress -= new ValueEventHandler<int>(this.MyFunctions_OnProgress);
    }

    private void btnSimulator_Click(object sender, EventArgs e)
    {
      Simulator.ShowDialog((Form) this, this.handler);
    }

    private void btnVolumeMonitor_Click(object sender, EventArgs e)
    {
      VolumeMonitor.ShowDialog((Form) this, this.handler);
    }

    private void btnUpgradeFirmware_Click(object sender, EventArgs e)
    {
      FirmwareEditor.ShowDialog((Form) this, this.handler);
    }

    private void btnConfigurator_Click(object sender, EventArgs e)
    {
      if (this.handler.Meter.Version.Type == EDC_Hardware.EDC_Radio)
        ConfiguratorEdcRadio.ShowDialog((Form) this, this.handler);
      else if (this.handler.Meter.Version.Type == EDC_Hardware.EDC_mBus || this.handler.Meter.Version.Type == EDC_Hardware.EDC_ModBus || this.handler.Meter.Version.Type == EDC_Hardware.EDC_mBus_Modbus || this.handler.Meter.Version.Type == EDC_Hardware.EDC_mBus_CJ188 || this.handler.Meter.Version.Type == EDC_Hardware.EDC_RS485_Modbus || this.handler.Meter.Version.Type == EDC_Hardware.EDC_RS485_CJ188)
        ConfiguratorEdcWired.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnOverwriteWorkFromType_Click(object sender, EventArgs e)
    {
      OverwriteEditor.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnTypeEditor_Click(object sender, EventArgs e)
    {
      TypeEditor.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnLoadBackup_Click(object sender, EventArgs e)
    {
      byte[] zippedBuffer = BackupWindow.ShowDialog((Window) null, (ICreateMeter) this.handler, new string[7]
      {
        "EDC_Radio",
        "EDC_mBus",
        "EDC_ModBus",
        "EDC_mBus_Modbus",
        "EDC_mBus_CJ188",
        "EDC_RS485_Modbus",
        "EDC_RS485_CJ188"
      }, false);
      if (zippedBuffer == null)
        return;
      this.handler.OpenDevice(zippedBuffer);
      this.UpdateUI();
    }

    private void btnSaveBackup_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.handler.SaveDevice())
        {
          this.UpdateUI();
          int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Successfully saved!", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num1 = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Unable to create backup! " + ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription, "Backup failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Unable to create backup! " + ex.Message, "Backup failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadDevice_Click(object sender, EventArgs e)
    {
      try
      {
        if (!UserManager.CheckPermission("EDC_Handler"))
          throw new PermissionException("EDC_Handler");
        this.handler.ClearAllData();
        this.DisableUI();
        this.ResetUI();
        this.progress.Visible = true;
        Cursor.Current = Cursors.WaitCursor;
        if (this.cboxReadCyclic.Checked)
        {
          int num1 = 0;
          int num2 = 0;
          while (this.cboxReadCyclic.Checked)
          {
            ZR_ClassLibMessages.ClearErrors();
            if (!this.handler.ReadDevice(this.ckboxReadLoggerToo.Checked))
            {
              this.DisableUI();
              this.ResetUI();
              this.txtConnectedDeviceInfo.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
              ++num2;
            }
            else
            {
              this.UpdateUI();
              ++num1;
            }
            this.lblStatus.Text = string.Format("Cyclic read: OK ({0}) Failed ({1}), Time elapsed: {2}", (object) num1, (object) num2, (object) Util.ElapsedToString(this.stopwatch.Elapsed));
            System.Windows.Forms.Application.DoEvents();
            Cursor.Current = Cursors.WaitCursor;
          }
        }
        else
        {
          bool flag = this.handler.ReadDevice(this.ckboxReadLoggerToo.Checked);
          this.stopwatch.Stop();
          if (flag)
          {
            this.lblStatus.Text = "Time elapsed: " + Util.ElapsedToString(this.stopwatch.Elapsed);
          }
          else
          {
            this.lblStatus.Text = "Error occurred!";
            int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      catch (NotSupportedException ex)
      {
        if (DialogResult.Yes != System.Windows.Forms.MessageBox.Show((IWin32Window) this, ex.Message + Environment.NewLine + "Do you want to make a firmware upgrade?", "Old firmware", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
          return;
        FirmwareEditor.ShowDialog((Form) this, this.handler);
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Failed read meter! " + ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.UpdateUI();
        this.ValidateDevice();
      }
    }

    private void btnWriteDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.DisableUI();
        Cursor.Current = Cursors.WaitCursor;
        if (this.handler.WriteDevice())
        {
          this.stopwatch.Stop();
          this.lblStatus.Text = "Time elapsed: " + Util.ElapsedToString(this.stopwatch.Elapsed);
          int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Successfully!", "Write device", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num1 = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Unable to write to device! Error: " + ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription, "Failed write device", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.UpdateUI();
      }
      finally
      {
        this.btnReadDevice.Enabled = true;
        this.btnWriteDevice.Enabled = true;
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnParameter_Click(object sender, EventArgs e)
    {
      ParameterEditor.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnMBusList_Click(object sender, EventArgs e)
    {
      MbusListEditor.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnLogger_Click(object sender, EventArgs e)
    {
      LoggerEditor.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.handler.ClearAllData();
      this.ResetUI();
      this.btnWriteDevice.Enabled = false;
      this.UpdateUI();
    }

    private void btnTestCommands_Click(object sender, EventArgs e)
    {
      TestCommand.ShowDialog((Form) this, this.handler);
    }

    private void btnZoomWork_Click(object sender, EventArgs e)
    {
      ZoomEditor.ShowDialog((Form) this, this.handler.WorkMeter);
    }

    private void btnZoomConnected_Click(object sender, EventArgs e)
    {
      ZoomEditor.ShowDialog((Form) this, this.handler.ConnectedMeter);
    }

    private void btnZoomDeviceBackup_Click(object sender, EventArgs e)
    {
      ZoomEditor.ShowDialog((Form) this, this.handler.BackupMeter);
    }

    private void btnZoomType_Click(object sender, EventArgs e)
    {
      ZoomEditor.ShowDialog((Form) this, this.handler.TypeMeter);
    }

    private void MyDeviceCollector_OnMessage(object sender, GMM_EventArgs e)
    {
      if (this.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage), sender, (object) e);
        }
        catch
        {
        }
      }
      else
      {
        e.Cancel = this.isCanceled;
        if (e.Cancel)
          this.handler.MyDeviceCollector.BreakRequest = true;
        if (string.IsNullOrEmpty(e.EventMessage) || e.TheMessageType == GMM_EventArgs.MessageType.Alive)
          return;
        this.lblStatus.Text = e.EventMessage;
      }
    }

    private void MyFunctions_OnProgress(object sender, int e)
    {
      if (e >= 0 && e <= 100)
        this.progress.Value = e;
      System.Windows.Forms.Application.DoEvents();
    }

    private void ResetUI()
    {
      this.isCanceled = false;
      this.lblStatus.Text = string.Empty;
      this.txtTypeInfo.Text = string.Empty;
      this.txtBackupDeviceInfo.Text = string.Empty;
      this.txtConnectedDeviceInfo.Text = string.Empty;
      this.txtWorkDeviceInfo.Text = string.Empty;
      this.progress.Visible = false;
      this.progress.Value = 0;
      System.Windows.Forms.Application.DoEvents();
      this.stopwatch.Reset();
      this.stopwatch.Start();
    }

    private void UpdateUI()
    {
      if (this.handler == null)
        return;
      if (this.handler.ConnectedMeter != null)
        this.txtConnectedDeviceInfo.Text = this.handler.ConnectedMeter.ToString();
      else
        this.txtConnectedDeviceInfo.Text = string.Empty;
      if (this.handler.WorkMeter != null)
        this.txtWorkDeviceInfo.Text = this.handler.WorkMeter.ToString();
      else
        this.txtWorkDeviceInfo.Text = string.Empty;
      if (this.handler.TypeMeter != null)
      {
        this.txtTypeInfo.Text = this.handler.TypeMeter.ToString();
        if (this.handler.TypeMeter.DBDeviceInfo != null && this.handler.TypeMeter.DBDeviceInfo.MeterInfo != null)
          this.gboxTypeDevice.Text = "Type (MeterInfoID: " + this.handler.TypeMeter.DBDeviceInfo.MeterInfo.MeterInfoID.ToString() + " " + this.handler.TypeMeter.DBDeviceInfo.MeterInfo.Description + ")";
        else
          this.gboxTypeDevice.Text = "Base type";
      }
      else
      {
        this.txtTypeInfo.Text = string.Empty;
        this.gboxTypeDevice.Text = "Type";
      }
      if (this.handler.BackupMeter != null)
        this.txtBackupDeviceInfo.Text = this.handler.BackupMeter.ToString();
      else
        this.txtBackupDeviceInfo.Text = string.Empty;
      bool flag1 = this.handler.ConnectedMeter != null || this.handler.WorkMeter != null || this.handler.BackupMeter != null || this.handler.TypeMeter != null;
      this.btnClear.Enabled = flag1;
      this.btnConfigurator.Enabled = flag1;
      this.btnParameter.Enabled = flag1;
      this.btnMBusList.Enabled = flag1;
      this.btnLogger.Enabled = flag1;
      bool flag2 = this.handler.WorkMeter != null;
      this.btnWriteDevice.Enabled = flag2;
      this.btnCreateBackup.Enabled = flag2;
      this.btnTypeEditor.Enabled = true;
      this.btnLoadBackup.Enabled = true;
      this.btnOverwriteWorkFromType.Enabled = this.handler.TypeMeter != null;
      this.btnReadDevice.Enabled = true;
      this.btnFirmware.Enabled = true;
      this.btnTestDialog.Enabled = true;
      this.btnVolumeMonitor.Enabled = true;
      this.btnSimulator.Enabled = true;
      this.progress.Visible = false;
      bool flag3 = UserManager.CheckPermission("EDC_Handler.View.Expert");
      this.gboxTypeDevice.Visible = flag3;
      this.gboxBackupDevice.Visible = flag3;
      this.gboxConnectedDevice.Visible = flag3;
      this.gboxWorkDevice.Visible = flag3;
      this.btnClear.Visible = flag3;
      this.btnConfigurator.Visible = flag3;
      this.btnParameter.Visible = flag3;
      this.btnMBusList.Visible = flag3;
      this.btnLogger.Visible = flag3;
      this.btnVolumeMonitor.Visible = flag3;
      this.btnSimulator.Visible = flag3;
      this.btnFirmware.Visible = UserManager.CheckPermission("EDC_Handler.View.FirmwareUpdate");
      this.btnTestDialog.Visible = UserManager.CheckPermission("EDC_Handler.View.TestCommandos");
      Cursor.Current = Cursors.Default;
    }

    private void DisableUI()
    {
      this.btnReadDevice.Enabled = false;
      this.btnWriteDevice.Enabled = false;
      this.btnCreateBackup.Enabled = false;
      this.btnTypeEditor.Enabled = false;
      this.btnLoadBackup.Enabled = false;
      this.btnFirmware.Enabled = false;
      this.btnTestDialog.Enabled = false;
      this.btnVolumeMonitor.Enabled = false;
      this.btnLogger.Enabled = false;
      this.btnConfigurator.Enabled = false;
      this.btnParameter.Enabled = false;
      this.btnMBusList.Enabled = false;
      this.btnSimulator.Enabled = false;
      this.btnOverwriteWorkFromType.Enabled = false;
    }

    private void ValidateDevice()
    {
      string text = this.handler.ValidateMeter(this.handler.WorkMeter);
      if (string.IsNullOrEmpty(text))
        return;
      int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, text, "Invalid Work device", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void button_HardwareTypeEditor_Click(object sender, EventArgs e)
    {
      this.handler.ReadVersion();
      new HardwareTypeEditor(new string[7]
      {
        "EDC_mBus",
        "EDC_Radio",
        "EDC_ModBus",
        "EDC_mBus_Modbus",
        "EDC_mBus_CJ188",
        "EDC_RS485_Modbus",
        "EDC_RS485_CJ188"
      }).ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EDC_HandlerWindow));
      this.menuStrip = new MenuStrip();
      this.ToolStripMenuItemComponents = new ToolStripMenuItem();
      this.gboxTypeDevice = new GroupBox();
      this.btnZoomType = new Button();
      this.btnTypeEditor = new Button();
      this.btnOverwriteWorkFromType = new Button();
      this.txtTypeInfo = new TextBox();
      this.btnClear = new Button();
      this.gboxConnectedDevice = new GroupBox();
      this.btnZoomConnected = new Button();
      this.ckboxReadLoggerToo = new CheckBox();
      this.cboxReadCyclic = new CheckBox();
      this.btnWriteDevice = new Button();
      this.btnReadDevice = new Button();
      this.txtConnectedDeviceInfo = new TextBox();
      this.groupBoxShowEditData = new GroupBox();
      this.btnSimulator = new Button();
      this.btnFirmware = new Button();
      this.btnConfigurator = new Button();
      this.btnVolumeMonitor = new Button();
      this.btnTestDialog = new Button();
      this.btnParameter = new Button();
      this.btnMBusList = new Button();
      this.btnLogger = new Button();
      this.gboxBackupDevice = new GroupBox();
      this.btnZoomDeviceBackup = new Button();
      this.btnLoadBackup = new Button();
      this.btnCreateBackup = new Button();
      this.txtBackupDeviceInfo = new TextBox();
      this.statusStrip = new StatusStrip();
      this.progress = new ToolStripProgressBar();
      this.lblStatus = new ToolStripStatusLabel();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.gboxWorkDevice = new GroupBox();
      this.btnZoomWork = new Button();
      this.txtWorkDeviceInfo = new TextBox();
      this.button_HardwareTypeEditor = new Button();
      this.menuStrip.SuspendLayout();
      this.gboxTypeDevice.SuspendLayout();
      this.gboxConnectedDevice.SuspendLayout();
      this.groupBoxShowEditData.SuspendLayout();
      this.gboxBackupDevice.SuspendLayout();
      this.statusStrip.SuspendLayout();
      this.gboxWorkDevice.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.ToolStripMenuItemComponents
      });
      this.menuStrip.Location = new Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new Size(784, 24);
      this.menuStrip.TabIndex = 17;
      this.menuStrip.Text = "menuStrip1";
      this.ToolStripMenuItemComponents.Name = "ToolStripMenuItemComponents";
      this.ToolStripMenuItemComponents.Size = new Size(83, 20);
      this.ToolStripMenuItemComponents.Text = "Component";
      this.gboxTypeDevice.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxTypeDevice.Controls.Add((Control) this.btnZoomType);
      this.gboxTypeDevice.Controls.Add((Control) this.btnTypeEditor);
      this.gboxTypeDevice.Controls.Add((Control) this.btnOverwriteWorkFromType);
      this.gboxTypeDevice.Controls.Add((Control) this.txtTypeInfo);
      this.gboxTypeDevice.Location = new Point(7, 71);
      this.gboxTypeDevice.Name = "gboxTypeDevice";
      this.gboxTypeDevice.Size = new Size(617, 109);
      this.gboxTypeDevice.TabIndex = 33;
      this.gboxTypeDevice.TabStop = false;
      this.gboxTypeDevice.Text = "Type";
      this.btnZoomType.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnZoomType.Image = (Image) componentResourceManager.GetObject("btnZoomType.Image");
      this.btnZoomType.Location = new Point(372, 22);
      this.btnZoomType.Name = "btnZoomType";
      this.btnZoomType.Size = new Size(23, 23);
      this.btnZoomType.TabIndex = 36;
      this.btnZoomType.UseVisualStyleBackColor = true;
      this.btnZoomType.Click += new System.EventHandler(this.btnZoomType_Click);
      this.btnTypeEditor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTypeEditor.Image = (Image) componentResourceManager.GetObject("btnTypeEditor.Image");
      this.btnTypeEditor.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnTypeEditor.Location = new Point(422, 19);
      this.btnTypeEditor.Name = "btnTypeEditor";
      this.btnTypeEditor.Size = new Size(186, 39);
      this.btnTypeEditor.TabIndex = 20;
      this.btnTypeEditor.Text = "Type";
      this.btnTypeEditor.UseVisualStyleBackColor = true;
      this.btnTypeEditor.Click += new System.EventHandler(this.btnTypeEditor_Click);
      this.btnOverwriteWorkFromType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOverwriteWorkFromType.Enabled = false;
      this.btnOverwriteWorkFromType.Location = new Point(422, 64);
      this.btnOverwriteWorkFromType.Name = "btnOverwriteWorkFromType";
      this.btnOverwriteWorkFromType.Size = new Size(186, 39);
      this.btnOverwriteWorkFromType.TabIndex = 17;
      this.btnOverwriteWorkFromType.Text = "Overwrite work (from type) ...";
      this.btnOverwriteWorkFromType.UseVisualStyleBackColor = true;
      this.btnOverwriteWorkFromType.Click += new System.EventHandler(this.btnOverwriteWorkFromType_Click);
      this.txtTypeInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTypeInfo.BackColor = Color.White;
      this.txtTypeInfo.Font = new Font("Consolas", 8.25f);
      this.txtTypeInfo.Location = new Point(6, 19);
      this.txtTypeInfo.Multiline = true;
      this.txtTypeInfo.Name = "txtTypeInfo";
      this.txtTypeInfo.ReadOnly = true;
      this.txtTypeInfo.ScrollBars = ScrollBars.Vertical;
      this.txtTypeInfo.Size = new Size(410, 84);
      this.txtTypeInfo.TabIndex = 19;
      this.btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClear.Enabled = false;
      this.btnClear.Image = (Image) componentResourceManager.GetObject("btnClear.Image");
      this.btnClear.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnClear.Location = new Point(636, 488);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(130, 39);
      this.btnClear.TabIndex = 25;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      this.gboxConnectedDevice.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxConnectedDevice.Controls.Add((Control) this.btnZoomConnected);
      this.gboxConnectedDevice.Controls.Add((Control) this.ckboxReadLoggerToo);
      this.gboxConnectedDevice.Controls.Add((Control) this.cboxReadCyclic);
      this.gboxConnectedDevice.Controls.Add((Control) this.btnWriteDevice);
      this.gboxConnectedDevice.Controls.Add((Control) this.btnReadDevice);
      this.gboxConnectedDevice.Controls.Add((Control) this.txtConnectedDeviceInfo);
      this.gboxConnectedDevice.Location = new Point(7, 301);
      this.gboxConnectedDevice.Name = "gboxConnectedDevice";
      this.gboxConnectedDevice.Size = new Size(617, 125);
      this.gboxConnectedDevice.TabIndex = 32;
      this.gboxConnectedDevice.TabStop = false;
      this.gboxConnectedDevice.Text = "Connected device";
      this.btnZoomConnected.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnZoomConnected.Image = (Image) componentResourceManager.GetObject("btnZoomConnected.Image");
      this.btnZoomConnected.Location = new Point(372, 22);
      this.btnZoomConnected.Name = "btnZoomConnected";
      this.btnZoomConnected.Size = new Size(23, 23);
      this.btnZoomConnected.TabIndex = 21;
      this.btnZoomConnected.UseVisualStyleBackColor = true;
      this.btnZoomConnected.Click += new System.EventHandler(this.btnZoomConnected_Click);
      this.ckboxReadLoggerToo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.ckboxReadLoggerToo.Location = new Point(422, 16);
      this.ckboxReadLoggerToo.Name = "ckboxReadLoggerToo";
      this.ckboxReadLoggerToo.Size = new Size(85, 17);
      this.ckboxReadLoggerToo.TabIndex = 22;
      this.ckboxReadLoggerToo.Text = "Read logger";
      this.cboxReadCyclic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboxReadCyclic.Location = new Point(509, 16);
      this.cboxReadCyclic.Name = "cboxReadCyclic";
      this.cboxReadCyclic.Size = new Size(86, 17);
      this.cboxReadCyclic.TabIndex = 34;
      this.cboxReadCyclic.Text = "Read cyclic";
      this.cboxReadCyclic.Visible = false;
      this.btnWriteDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnWriteDevice.Image = (Image) componentResourceManager.GetObject("btnWriteDevice.Image");
      this.btnWriteDevice.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnWriteDevice.Location = new Point(422, 81);
      this.btnWriteDevice.Name = "btnWriteDevice";
      this.btnWriteDevice.Size = new Size(186, 39);
      this.btnWriteDevice.TabIndex = 21;
      this.btnWriteDevice.Text = "Write";
      this.btnWriteDevice.UseVisualStyleBackColor = true;
      this.btnWriteDevice.Click += new System.EventHandler(this.btnWriteDevice_Click);
      this.btnReadDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReadDevice.Image = (Image) componentResourceManager.GetObject("btnReadDevice.Image");
      this.btnReadDevice.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnReadDevice.Location = new Point(422, 39);
      this.btnReadDevice.Name = "btnReadDevice";
      this.btnReadDevice.Size = new Size(186, 39);
      this.btnReadDevice.TabIndex = 20;
      this.btnReadDevice.Text = "Read";
      this.btnReadDevice.UseVisualStyleBackColor = true;
      this.btnReadDevice.Click += new System.EventHandler(this.btnReadDevice_Click);
      this.txtConnectedDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConnectedDeviceInfo.BackColor = Color.White;
      this.txtConnectedDeviceInfo.Font = new Font("Consolas", 8.25f);
      this.txtConnectedDeviceInfo.Location = new Point(6, 19);
      this.txtConnectedDeviceInfo.Multiline = true;
      this.txtConnectedDeviceInfo.Name = "txtConnectedDeviceInfo";
      this.txtConnectedDeviceInfo.ReadOnly = true;
      this.txtConnectedDeviceInfo.ScrollBars = ScrollBars.Vertical;
      this.txtConnectedDeviceInfo.Size = new Size(410, 100);
      this.txtConnectedDeviceInfo.TabIndex = 19;
      this.groupBoxShowEditData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxShowEditData.Controls.Add((Control) this.button_HardwareTypeEditor);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnSimulator);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnFirmware);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnConfigurator);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnVolumeMonitor);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnTestDialog);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnParameter);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnMBusList);
      this.groupBoxShowEditData.Controls.Add((Control) this.btnLogger);
      this.groupBoxShowEditData.Location = new Point(630, 68);
      this.groupBoxShowEditData.Name = "groupBoxShowEditData";
      this.groupBoxShowEditData.Size = new Size(144, 376);
      this.groupBoxShowEditData.TabIndex = 31;
      this.groupBoxShowEditData.TabStop = false;
      this.btnSimulator.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSimulator.Location = new Point(5, 292);
      this.btnSimulator.Name = "btnSimulator";
      this.btnSimulator.Size = new Size(130, 35);
      this.btnSimulator.TabIndex = 22;
      this.btnSimulator.Text = "Simulator";
      this.btnSimulator.UseVisualStyleBackColor = true;
      this.btnSimulator.Click += new System.EventHandler(this.btnSimulator_Click);
      this.btnFirmware.Image = (Image) componentResourceManager.GetObject("btnFirmware.Image");
      this.btnFirmware.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnFirmware.Location = new Point(5, 173);
      this.btnFirmware.Name = "btnFirmware";
      this.btnFirmware.Size = new Size(130, 35);
      this.btnFirmware.TabIndex = 21;
      this.btnFirmware.Text = "Firmware";
      this.btnFirmware.UseVisualStyleBackColor = true;
      this.btnFirmware.Click += new System.EventHandler(this.btnUpgradeFirmware_Click);
      this.btnConfigurator.Image = (Image) componentResourceManager.GetObject("btnConfigurator.Image");
      this.btnConfigurator.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnConfigurator.Location = new Point(5, 17);
      this.btnConfigurator.Name = "btnConfigurator";
      this.btnConfigurator.Size = new Size(130, 35);
      this.btnConfigurator.TabIndex = 20;
      this.btnConfigurator.Text = "Configurator";
      this.btnConfigurator.UseVisualStyleBackColor = true;
      this.btnConfigurator.Click += new System.EventHandler(this.btnConfigurator_Click);
      this.btnVolumeMonitor.Image = (Image) componentResourceManager.GetObject("btnVolumeMonitor.Image");
      this.btnVolumeMonitor.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnVolumeMonitor.Location = new Point(5, 251);
      this.btnVolumeMonitor.Name = "btnVolumeMonitor";
      this.btnVolumeMonitor.Size = new Size(130, 35);
      this.btnVolumeMonitor.TabIndex = 19;
      this.btnVolumeMonitor.Text = "Volume monitor";
      this.btnVolumeMonitor.UseVisualStyleBackColor = true;
      this.btnVolumeMonitor.Click += new System.EventHandler(this.btnVolumeMonitor_Click);
      this.btnTestDialog.Image = (Image) componentResourceManager.GetObject("btnTestDialog.Image");
      this.btnTestDialog.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnTestDialog.Location = new Point(5, 212);
      this.btnTestDialog.Name = "btnTestDialog";
      this.btnTestDialog.Size = new Size(130, 35);
      this.btnTestDialog.TabIndex = 18;
      this.btnTestDialog.Text = "Test";
      this.btnTestDialog.UseVisualStyleBackColor = true;
      this.btnTestDialog.Click += new System.EventHandler(this.btnTestCommands_Click);
      this.btnParameter.Image = (Image) componentResourceManager.GetObject("btnParameter.Image");
      this.btnParameter.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnParameter.Location = new Point(5, 56);
      this.btnParameter.Name = "btnParameter";
      this.btnParameter.Size = new Size(130, 35);
      this.btnParameter.TabIndex = 17;
      this.btnParameter.Text = "Parameter";
      this.btnParameter.UseVisualStyleBackColor = true;
      this.btnParameter.Click += new System.EventHandler(this.btnParameter_Click);
      this.btnMBusList.Image = (Image) componentResourceManager.GetObject("btnMBusList.Image");
      this.btnMBusList.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnMBusList.Location = new Point(5, 95);
      this.btnMBusList.Name = "btnMBusList";
      this.btnMBusList.Size = new Size(130, 35);
      this.btnMBusList.TabIndex = 17;
      this.btnMBusList.Text = "M-Bus list";
      this.btnMBusList.UseVisualStyleBackColor = true;
      this.btnMBusList.Click += new System.EventHandler(this.btnMBusList_Click);
      this.btnLogger.Enabled = false;
      this.btnLogger.Image = (Image) componentResourceManager.GetObject("btnLogger.Image");
      this.btnLogger.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLogger.Location = new Point(5, 134);
      this.btnLogger.Name = "btnLogger";
      this.btnLogger.Size = new Size(130, 35);
      this.btnLogger.TabIndex = 17;
      this.btnLogger.Text = "Logger";
      this.btnLogger.UseVisualStyleBackColor = true;
      this.btnLogger.Click += new System.EventHandler(this.btnLogger_Click);
      this.gboxBackupDevice.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxBackupDevice.Controls.Add((Control) this.btnZoomDeviceBackup);
      this.gboxBackupDevice.Controls.Add((Control) this.btnLoadBackup);
      this.gboxBackupDevice.Controls.Add((Control) this.btnCreateBackup);
      this.gboxBackupDevice.Controls.Add((Control) this.txtBackupDeviceInfo);
      this.gboxBackupDevice.Location = new Point(7, 186);
      this.gboxBackupDevice.Name = "gboxBackupDevice";
      this.gboxBackupDevice.Size = new Size(617, 109);
      this.gboxBackupDevice.TabIndex = 30;
      this.gboxBackupDevice.TabStop = false;
      this.gboxBackupDevice.Text = "Device backup";
      this.btnZoomDeviceBackup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnZoomDeviceBackup.Image = (Image) componentResourceManager.GetObject("btnZoomDeviceBackup.Image");
      this.btnZoomDeviceBackup.Location = new Point(372, 25);
      this.btnZoomDeviceBackup.Name = "btnZoomDeviceBackup";
      this.btnZoomDeviceBackup.Size = new Size(23, 23);
      this.btnZoomDeviceBackup.TabIndex = 35;
      this.btnZoomDeviceBackup.UseVisualStyleBackColor = true;
      this.btnZoomDeviceBackup.Click += new System.EventHandler(this.btnZoomDeviceBackup_Click);
      this.btnLoadBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLoadBackup.Image = (Image) componentResourceManager.GetObject("btnLoadBackup.Image");
      this.btnLoadBackup.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoadBackup.Location = new Point(422, 21);
      this.btnLoadBackup.Name = "btnLoadBackup";
      this.btnLoadBackup.Size = new Size(186, 39);
      this.btnLoadBackup.TabIndex = 17;
      this.btnLoadBackup.Text = "Load backup";
      this.btnLoadBackup.UseVisualStyleBackColor = true;
      this.btnLoadBackup.Click += new System.EventHandler(this.btnLoadBackup_Click);
      this.btnCreateBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreateBackup.Enabled = false;
      this.btnCreateBackup.Image = (Image) componentResourceManager.GetObject("btnCreateBackup.Image");
      this.btnCreateBackup.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCreateBackup.Location = new Point(422, 64);
      this.btnCreateBackup.Name = "btnCreateBackup";
      this.btnCreateBackup.Size = new Size(186, 39);
      this.btnCreateBackup.TabIndex = 17;
      this.btnCreateBackup.Text = "Create backup";
      this.btnCreateBackup.UseVisualStyleBackColor = true;
      this.btnCreateBackup.Click += new System.EventHandler(this.btnSaveBackup_Click);
      this.txtBackupDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBackupDeviceInfo.BackColor = Color.White;
      this.txtBackupDeviceInfo.Font = new Font("Consolas", 8.25f);
      this.txtBackupDeviceInfo.Location = new Point(6, 21);
      this.txtBackupDeviceInfo.Multiline = true;
      this.txtBackupDeviceInfo.Name = "txtBackupDeviceInfo";
      this.txtBackupDeviceInfo.ReadOnly = true;
      this.txtBackupDeviceInfo.ScrollBars = ScrollBars.Vertical;
      this.txtBackupDeviceInfo.Size = new Size(410, 81);
      this.txtBackupDeviceInfo.TabIndex = 19;
      this.statusStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.progress,
        (ToolStripItem) this.lblStatus
      });
      this.statusStrip.Location = new Point(0, 540);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(784, 22);
      this.statusStrip.TabIndex = 22;
      this.statusStrip.Text = "statusStrip1";
      this.progress.Name = "progress";
      this.progress.Size = new Size(100, 16);
      this.progress.Step = 1;
      this.progress.Visible = false;
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(46, 17);
      this.lblStatus.Text = "{status}";
      this.zennerCoroprateDesign2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign2.Location = new Point(0, 24);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(782, 41);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.gboxWorkDevice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxWorkDevice.Controls.Add((Control) this.btnZoomWork);
      this.gboxWorkDevice.Controls.Add((Control) this.txtWorkDeviceInfo);
      this.gboxWorkDevice.Location = new Point(7, 432);
      this.gboxWorkDevice.Name = "gboxWorkDevice";
      this.gboxWorkDevice.Size = new Size(424, 102);
      this.gboxWorkDevice.TabIndex = 33;
      this.gboxWorkDevice.TabStop = false;
      this.gboxWorkDevice.Text = "Work device";
      this.btnZoomWork.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnZoomWork.Image = (Image) componentResourceManager.GetObject("btnZoomWork.Image");
      this.btnZoomWork.Location = new Point(372, 21);
      this.btnZoomWork.Name = "btnZoomWork";
      this.btnZoomWork.Size = new Size(23, 23);
      this.btnZoomWork.TabIndex = 20;
      this.btnZoomWork.UseVisualStyleBackColor = true;
      this.btnZoomWork.Click += new System.EventHandler(this.btnZoomWork_Click);
      this.txtWorkDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWorkDeviceInfo.BackColor = Color.White;
      this.txtWorkDeviceInfo.Font = new Font("Consolas", 8.25f);
      this.txtWorkDeviceInfo.Location = new Point(6, 18);
      this.txtWorkDeviceInfo.Multiline = true;
      this.txtWorkDeviceInfo.Name = "txtWorkDeviceInfo";
      this.txtWorkDeviceInfo.ReadOnly = true;
      this.txtWorkDeviceInfo.ScrollBars = ScrollBars.Vertical;
      this.txtWorkDeviceInfo.Size = new Size(410, 77);
      this.txtWorkDeviceInfo.TabIndex = 19;
      this.button_HardwareTypeEditor.ImageAlign = ContentAlignment.MiddleLeft;
      this.button_HardwareTypeEditor.Location = new Point(6, 333);
      this.button_HardwareTypeEditor.Name = "button_HardwareTypeEditor";
      this.button_HardwareTypeEditor.Size = new Size(130, 35);
      this.button_HardwareTypeEditor.TabIndex = 23;
      this.button_HardwareTypeEditor.Text = "Hardware Type Editor";
      this.button_HardwareTypeEditor.UseVisualStyleBackColor = true;
      this.button_HardwareTypeEditor.Click += new System.EventHandler(this.button_HardwareTypeEditor_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.gboxWorkDevice);
      this.Controls.Add((Control) this.gboxTypeDevice);
      this.Controls.Add((Control) this.btnClear);
      this.Controls.Add((Control) this.gboxConnectedDevice);
      this.Controls.Add((Control) this.groupBoxShowEditData);
      this.Controls.Add((Control) this.gboxBackupDevice);
      this.Controls.Add((Control) this.statusStrip);
      this.Controls.Add((Control) this.menuStrip);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (EDC_HandlerWindow);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "EDC";
      this.FormClosing += new FormClosingEventHandler(this.EDC_HandlerWindow_FormClosing);
      this.Load += new System.EventHandler(this.EDC_HandlerWindow_Load);
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.gboxTypeDevice.ResumeLayout(false);
      this.gboxTypeDevice.PerformLayout();
      this.gboxConnectedDevice.ResumeLayout(false);
      this.gboxConnectedDevice.PerformLayout();
      this.groupBoxShowEditData.ResumeLayout(false);
      this.gboxBackupDevice.ResumeLayout(false);
      this.gboxBackupDevice.PerformLayout();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.gboxWorkDevice.ResumeLayout(false);
      this.gboxWorkDevice.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
