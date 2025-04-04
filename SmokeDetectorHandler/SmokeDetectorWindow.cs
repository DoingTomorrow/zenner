// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.SmokeDetectorWindow
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using CorporateDesign;
using GmmDbLib;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Forms.Layout;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public class SmokeDetectorWindow : Form
  {
    private static Logger logger = LogManager.GetLogger(nameof (SmokeDetectorWindow));
    internal string NextComponentName = "Exit";
    private SmokeDetectorHandlerFunctions handler;
    private bool isCanceled;
    private Stopwatch stopwatch;
    private IContainer components = (IContainer) null;
    private MenuStrip menu;
    private ToolStripMenuItem ToolStripMenuItemComponents;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private ToolStripStatusLabel lblStatus;
    private StatusStrip statusStrip;
    private Button btnClear;
    private Button btnReadDevice;
    private ToolStripProgressBar progress;
    private CheckBox cboxReadLoggerEvents;
    private Button btnConfigurator;
    private TextBox txtWorkDevice;
    private Button btnWriteDevice;
    private Button btnTestCommandDialog;
    private GroupBox gboxReadOptions;
    private CheckBox cboxReadManufacturingParameter;
    private CheckBox cboxReadTestModeParameter;
    private Button btnLoadBackup;
    private Button btnCreateBackup;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Button bntRadioCmd;
    private Button bntLoRaCmd;
    private Button bntMBusCmd;
    private Button bntCommonCmd;
    private Button btnSpecialCommands;

    public SmokeDetectorWindow(SmokeDetectorHandlerFunctions MyFunctions)
    {
      this.InitializeComponent();
      this.handler = MyFunctions;
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      this.Text = string.Format("Smoke Detector v{0}.{1}.{2}", (object) version.Major, (object) version.Minor, (object) version.Build);
      this.ToolStripMenuItemComponents.DropDownItems.AddRange(MyFunctions.GetComponentMenuItems());
      this.menu.Visible = true;
      this.stopwatch = new Stopwatch();
      this.ResetShowData();
      FormTranslatorSupport.TranslateWindow(Tg.SmokeDetectorWindow, (Form) this);
    }

    private void SmokeDetectorWindow_Load(object sender, EventArgs e)
    {
      if (!UserManager.CheckPermission("SmokeDetectorHandler"))
      {
        int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "No permission for Smoke Detector Handler!", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
      else
      {
        this.cboxHandlerObject.DataSource = (object) Util.GetNamesOfEnum(typeof (HandlerMeterType));
        this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
        this.handler.MyDeviceCollector.OnMessage += new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
        this.handler.OnProgress += new ValueEventHandler<int>(this.MyFunctions_OnProgress);
      }
    }

    private void SmokeDetectorWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.handler.MyDeviceCollector.OnMessage -= new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      this.handler.OnProgress -= new ValueEventHandler<int>(this.MyFunctions_OnProgress);
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.UpdateUI();
    }

    private void ResetShowData()
    {
      this.isCanceled = false;
      this.lblStatus.Text = string.Empty;
      this.txtWorkDevice.Text = string.Empty;
      this.progress.Visible = false;
      this.progress.Value = 0;
      System.Windows.Forms.Application.DoEvents();
      this.stopwatch.Reset();
      this.stopwatch.Start();
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
      if (this.progress.ProgressBar == null)
        return;
      this.progress.Enabled = true;
      if (e >= 0 && e <= 100)
      {
        try
        {
          this.progress.Value = e;
        }
        catch (Exception ex)
        {
          SmokeDetectorWindow.logger.Error("Ignore Exception: " + ex.Message);
        }
      }
      System.Windows.Forms.Application.DoEvents();
    }

    private void btnReadDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.DisableAllButtons();
        this.ResetShowData();
        this.progress.Visible = true;
        Cursor.Current = Cursors.WaitCursor;
        bool flag = this.handler.ReadDevice(this.GetReadOptions());
        if (flag)
          this.stopwatch.Stop();
        else
          ZR_ClassLibMessages.ShowAndClearErrors();
        if (flag)
          this.lblStatus.Text = "Time elapsed: " + Util.ElapsedToString(this.stopwatch.Elapsed);
        else
          this.lblStatus.Text = "Error occurred!";
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.UpdateUI();
        this.progress.Visible = false;
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnWriteDevice_Click(object sender, EventArgs e)
    {
      try
      {
        this.DisableAllButtons();
        Cursor.Current = Cursors.WaitCursor;
        if (this.handler.WriteDevice())
        {
          this.stopwatch.Stop();
          this.lblStatus.Text = "Time elapsed: " + Util.ElapsedToString(this.stopwatch.Elapsed);
          int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Successfully!", "Write device", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num1 = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, "Unable to write to device! " + Environment.NewLine + ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription, "Write", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.UpdateUI();
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnConfigurator_Click(object sender, EventArgs e)
    {
      Configurator.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnLoadBackup_Click(object sender, EventArgs e)
    {
      BackupEditor.ShowDialog((Form) this, this.handler);
      this.UpdateUI();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.handler.ClearAllData();
      this.ResetShowData();
      this.UpdateUI();
    }

    private void btnTestCommandDialog_Click(object sender, EventArgs e)
    {
      TestCommand.ShowDialog((Form) this, this.handler);
    }

    private void btnCreateBackup_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.handler.SaveDevice((byte) 0))
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

    private void DisableAllButtons()
    {
      foreach (Control control in (ArrangedElementCollection) this.Controls)
      {
        if (control is Button || control is GroupBox)
          control.Enabled = false;
      }
    }

    private void UpdateUI()
    {
      this.txtWorkDevice.Text = string.Empty;
      if (this.handler == null)
        return;
      if (this.handler.ConnectedMeterMinoprotectII != null)
        this.txtWorkDevice.Text = this.handler.ConnectedMeterMinoprotectII.ToString();
      MinoprotectIII handlerMeter = this.GetHandlerMeter();
      if (handlerMeter != null)
        this.txtWorkDevice.Text = handlerMeter.ToString();
      this.btnClear.Enabled = handlerMeter != null || this.handler.ConnectedMeterMinoprotectII != null;
      this.btnConfigurator.Enabled = this.handler.WorkMeter != null;
      this.btnWriteDevice.Enabled = this.handler.WorkMeter != null;
      this.btnCreateBackup.Enabled = this.handler.WorkMeter != null;
      this.btnLoadBackup.Enabled = true;
      this.btnReadDevice.Enabled = true;
      this.btnTestCommandDialog.Enabled = true;
      this.gboxReadOptions.Enabled = true;
      this.bntRadioCmd.Enabled = true;
      this.bntLoRaCmd.Enabled = true;
      this.bntMBusCmd.Enabled = true;
      this.bntCommonCmd.Enabled = true;
      this.btnSpecialCommands.Enabled = true;
    }

    private ReadPart GetReadOptions()
    {
      ReadPart readOptions = ReadPart.LoRa;
      if (this.cboxReadLoggerEvents.Checked)
        readOptions |= ReadPart.LoggerEvents;
      if (this.cboxReadTestModeParameter.Checked)
        readOptions |= ReadPart.TestModeParameter;
      if (this.cboxReadManufacturingParameter.Checked)
        readOptions |= ReadPart.ManufacturingParameter;
      return readOptions;
    }

    private MinoprotectIII GetHandlerMeter()
    {
      MinoprotectIII handlerMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          handlerMeter = this.handler.WorkMeter;
          break;
        case HandlerMeterType.ConnectedMeter:
          handlerMeter = this.handler.ConnectedMeter;
          break;
        case HandlerMeterType.BackupMeter:
          handlerMeter = this.handler.BackupMeter;
          break;
        default:
          throw new NotImplementedException();
      }
      return handlerMeter;
    }

    private void bntRadioCmd_Click(object sender, EventArgs e)
    {
      this.handler.Device.DeviceCMD.ReadVersion(new ProgressHandler((Action<ProgressArg>) (x => { })), new CancellationTokenSource().Token);
      RadioCommandWindow radioCommandWindow = new RadioCommandWindow(this.handler.Radio, (IPort) this.handler.Port);
      ElementHost.EnableModelessKeyboardInterop((Window) radioCommandWindow);
      radioCommandWindow.ShowDialog();
    }

    private void bntLoRaCmd_Click(object sender, EventArgs e)
    {
      this.handler.Device.DeviceCMD.ReadVersion(new ProgressHandler((Action<ProgressArg>) (x => { })), new CancellationTokenSource().Token);
      LoRaCommandWindow loRaCommandWindow = new LoRaCommandWindow(this.handler.LoRa);
      ElementHost.EnableModelessKeyboardInterop((Window) loRaCommandWindow);
      loRaCommandWindow.ShowDialog();
    }

    private void bntMBusCmd_Click(object sender, EventArgs e)
    {
      this.handler.Device.DeviceCMD.ReadVersion(new ProgressHandler((Action<ProgressArg>) (x => { })), new CancellationTokenSource().Token);
      MBusCommandWindow mbusCommandWindow = new MBusCommandWindow(this.handler.MBus);
      ElementHost.EnableModelessKeyboardInterop((Window) mbusCommandWindow);
      mbusCommandWindow.ShowDialog();
    }

    private void bntCommonCmd_Click(object sender, EventArgs e)
    {
      this.handler.Device.DeviceCMD.ReadVersion(new ProgressHandler((Action<ProgressArg>) (x => { })), new CancellationTokenSource().Token);
      CommandWindowCommon commandWindowCommon = new CommandWindowCommon(this.handler.Device);
      ElementHost.EnableModelessKeyboardInterop((Window) commandWindowCommon);
      commandWindowCommon.ShowDialog();
    }

    private void btnSpecialCommands_Click(object sender, EventArgs e)
    {
      this.handler.Device.DeviceCMD.ReadVersion(new ProgressHandler((Action<ProgressArg>) (x => { })), new CancellationTokenSource().Token);
      SpecialCommandWindow specialCommandWindow = new SpecialCommandWindow(this.handler.Special);
      ElementHost.EnableModelessKeyboardInterop((Window) specialCommandWindow);
      specialCommandWindow.ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SmokeDetectorWindow));
      this.menu = new MenuStrip();
      this.ToolStripMenuItemComponents = new ToolStripMenuItem();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.lblStatus = new ToolStripStatusLabel();
      this.statusStrip = new StatusStrip();
      this.progress = new ToolStripProgressBar();
      this.btnClear = new Button();
      this.btnReadDevice = new Button();
      this.cboxReadLoggerEvents = new CheckBox();
      this.btnConfigurator = new Button();
      this.txtWorkDevice = new TextBox();
      this.btnWriteDevice = new Button();
      this.btnTestCommandDialog = new Button();
      this.gboxReadOptions = new GroupBox();
      this.cboxReadManufacturingParameter = new CheckBox();
      this.cboxReadTestModeParameter = new CheckBox();
      this.btnLoadBackup = new Button();
      this.btnCreateBackup = new Button();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.bntRadioCmd = new Button();
      this.bntLoRaCmd = new Button();
      this.bntMBusCmd = new Button();
      this.bntCommonCmd = new Button();
      this.btnSpecialCommands = new Button();
      this.menu.SuspendLayout();
      this.statusStrip.SuspendLayout();
      this.gboxReadOptions.SuspendLayout();
      this.SuspendLayout();
      this.menu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.ToolStripMenuItemComponents
      });
      this.menu.Location = new Point(0, 0);
      this.menu.Name = "menu";
      this.menu.Size = new Size(828, 25);
      this.menu.TabIndex = 18;
      this.menu.Text = "menu";
      this.ToolStripMenuItemComponents.Name = "ToolStripMenuItemComponents";
      this.ToolStripMenuItemComponents.Size = new Size(88, 21);
      this.ToolStripMenuItemComponents.Text = "Component";
      this.zennerCoroprateDesign2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign2.Location = new Point(0, 24);
      this.zennerCoroprateDesign2.Margin = new Padding(2, 2, 2, 2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(828, 38);
      this.zennerCoroprateDesign2.TabIndex = 19;
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(50, 17);
      this.lblStatus.Text = "{status}";
      this.statusStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.progress,
        (ToolStripItem) this.lblStatus
      });
      this.statusStrip.Location = new Point(0, 540);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(828, 22);
      this.statusStrip.TabIndex = 34;
      this.statusStrip.Text = "statusStrip1";
      this.progress.Name = "progress";
      this.progress.Size = new Size(100, 16);
      this.progress.Step = 1;
      this.progress.Visible = false;
      this.btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClear.Enabled = false;
      this.btnClear.Image = (Image) componentResourceManager.GetObject("btnClear.Image");
      this.btnClear.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnClear.Location = new Point(634, 524);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(185, 36);
      this.btnClear.TabIndex = 26;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      this.btnReadDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReadDevice.Image = (Image) componentResourceManager.GetObject("btnReadDevice.Image");
      this.btnReadDevice.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnReadDevice.Location = new Point(634, 152);
      this.btnReadDevice.Name = "btnReadDevice";
      this.btnReadDevice.Size = new Size(186, 36);
      this.btnReadDevice.TabIndex = 20;
      this.btnReadDevice.Text = "Read";
      this.btnReadDevice.UseVisualStyleBackColor = true;
      this.btnReadDevice.Click += new System.EventHandler(this.btnReadDevice_Click);
      this.cboxReadLoggerEvents.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboxReadLoggerEvents.Checked = true;
      this.cboxReadLoggerEvents.CheckState = CheckState.Checked;
      this.cboxReadLoggerEvents.Location = new Point(16, 16);
      this.cboxReadLoggerEvents.Name = "cboxReadLoggerEvents";
      this.cboxReadLoggerEvents.Size = new Size(156, 16);
      this.cboxReadLoggerEvents.TabIndex = 35;
      this.cboxReadLoggerEvents.Text = "Logger";
      this.cboxReadLoggerEvents.UseVisualStyleBackColor = true;
      this.btnConfigurator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConfigurator.Enabled = false;
      this.btnConfigurator.Image = (Image) componentResourceManager.GetObject("btnConfigurator.Image");
      this.btnConfigurator.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnConfigurator.Location = new Point(634, 188);
      this.btnConfigurator.Name = "btnConfigurator";
      this.btnConfigurator.Size = new Size(186, 36);
      this.btnConfigurator.TabIndex = 36;
      this.btnConfigurator.Text = "Configurator";
      this.btnConfigurator.UseVisualStyleBackColor = true;
      this.btnConfigurator.Click += new System.EventHandler(this.btnConfigurator_Click);
      this.txtWorkDevice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWorkDevice.BackColor = Color.White;
      this.txtWorkDevice.Font = new Font("Consolas", 8.25f);
      this.txtWorkDevice.Location = new Point(5, 67);
      this.txtWorkDevice.Multiline = true;
      this.txtWorkDevice.Name = "txtWorkDevice";
      this.txtWorkDevice.ScrollBars = ScrollBars.Vertical;
      this.txtWorkDevice.Size = new Size(623, 470);
      this.txtWorkDevice.TabIndex = 38;
      this.btnWriteDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnWriteDevice.Enabled = false;
      this.btnWriteDevice.Image = (Image) componentResourceManager.GetObject("btnWriteDevice.Image");
      this.btnWriteDevice.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnWriteDevice.Location = new Point(634, 225);
      this.btnWriteDevice.Name = "btnWriteDevice";
      this.btnWriteDevice.Size = new Size(186, 36);
      this.btnWriteDevice.TabIndex = 40;
      this.btnWriteDevice.Text = "Write";
      this.btnWriteDevice.UseVisualStyleBackColor = true;
      this.btnWriteDevice.Click += new System.EventHandler(this.btnWriteDevice_Click);
      this.btnTestCommandDialog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTestCommandDialog.Image = (Image) componentResourceManager.GetObject("btnTestCommandDialog.Image");
      this.btnTestCommandDialog.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnTestCommandDialog.Location = new Point(635, 335);
      this.btnTestCommandDialog.Name = "btnTestCommandDialog";
      this.btnTestCommandDialog.Size = new Size(185, 36);
      this.btnTestCommandDialog.TabIndex = 41;
      this.btnTestCommandDialog.Text = "Test";
      this.btnTestCommandDialog.UseVisualStyleBackColor = true;
      this.btnTestCommandDialog.Click += new System.EventHandler(this.btnTestCommandDialog_Click);
      this.gboxReadOptions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.gboxReadOptions.Controls.Add((Control) this.cboxReadManufacturingParameter);
      this.gboxReadOptions.Controls.Add((Control) this.cboxReadTestModeParameter);
      this.gboxReadOptions.Controls.Add((Control) this.cboxReadLoggerEvents);
      this.gboxReadOptions.Location = new Point(635, 67);
      this.gboxReadOptions.Name = "gboxReadOptions";
      this.gboxReadOptions.Size = new Size(185, 82);
      this.gboxReadOptions.TabIndex = 42;
      this.gboxReadOptions.TabStop = false;
      this.gboxReadOptions.Text = "Read";
      this.cboxReadManufacturingParameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboxReadManufacturingParameter.Checked = true;
      this.cboxReadManufacturingParameter.CheckState = CheckState.Checked;
      this.cboxReadManufacturingParameter.Location = new Point(16, 59);
      this.cboxReadManufacturingParameter.Name = "cboxReadManufacturingParameter";
      this.cboxReadManufacturingParameter.Size = new Size(156, 16);
      this.cboxReadManufacturingParameter.TabIndex = 37;
      this.cboxReadManufacturingParameter.Text = "Manufacturing parameter";
      this.cboxReadManufacturingParameter.UseVisualStyleBackColor = true;
      this.cboxReadTestModeParameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboxReadTestModeParameter.Checked = true;
      this.cboxReadTestModeParameter.CheckState = CheckState.Checked;
      this.cboxReadTestModeParameter.Location = new Point(16, 38);
      this.cboxReadTestModeParameter.Name = "cboxReadTestModeParameter";
      this.cboxReadTestModeParameter.Size = new Size(156, 16);
      this.cboxReadTestModeParameter.TabIndex = 36;
      this.cboxReadTestModeParameter.Text = "Test parameter";
      this.cboxReadTestModeParameter.UseVisualStyleBackColor = true;
      this.btnLoadBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLoadBackup.Image = (Image) componentResourceManager.GetObject("btnLoadBackup.Image");
      this.btnLoadBackup.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoadBackup.Location = new Point(634, 262);
      this.btnLoadBackup.Name = "btnLoadBackup";
      this.btnLoadBackup.Size = new Size(186, 36);
      this.btnLoadBackup.TabIndex = 43;
      this.btnLoadBackup.Text = "Load backup";
      this.btnLoadBackup.UseVisualStyleBackColor = true;
      this.btnLoadBackup.Click += new System.EventHandler(this.btnLoadBackup_Click);
      this.btnCreateBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreateBackup.Enabled = false;
      this.btnCreateBackup.Image = (Image) componentResourceManager.GetObject("btnCreateBackup.Image");
      this.btnCreateBackup.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCreateBackup.Location = new Point(634, 299);
      this.btnCreateBackup.Name = "btnCreateBackup";
      this.btnCreateBackup.Size = new Size(186, 36);
      this.btnCreateBackup.TabIndex = 44;
      this.btnCreateBackup.Text = "Create backup";
      this.btnCreateBackup.UseVisualStyleBackColor = true;
      this.btnCreateBackup.Click += new System.EventHandler(this.btnCreateBackup_Click);
      this.label36.BackColor = Color.White;
      this.label36.Location = new Point(180, 32);
      this.label36.Name = "label36";
      this.label36.Size = new Size(84, 14);
      this.label36.TabIndex = 53;
      this.label36.Text = "Handler object:";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(272, 29);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 20);
      this.cboxHandlerObject.TabIndex = 52;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.bntRadioCmd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntRadioCmd.Location = new Point(636, 386);
      this.bntRadioCmd.Name = "bntRadioCmd";
      this.bntRadioCmd.Size = new Size(184, 21);
      this.bntRadioCmd.TabIndex = 54;
      this.bntRadioCmd.Text = "Radio Commands";
      this.bntRadioCmd.UseVisualStyleBackColor = true;
      this.bntRadioCmd.Click += new System.EventHandler(this.bntRadioCmd_Click);
      this.bntLoRaCmd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntLoRaCmd.Location = new Point(636, 409);
      this.bntLoRaCmd.Name = "bntLoRaCmd";
      this.bntLoRaCmd.Size = new Size(184, 21);
      this.bntLoRaCmd.TabIndex = 55;
      this.bntLoRaCmd.Text = "LoRa Commands";
      this.bntLoRaCmd.UseVisualStyleBackColor = true;
      this.bntLoRaCmd.Click += new System.EventHandler(this.bntLoRaCmd_Click);
      this.bntMBusCmd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntMBusCmd.Location = new Point(636, 432);
      this.bntMBusCmd.Name = "bntMBusCmd";
      this.bntMBusCmd.Size = new Size(184, 21);
      this.bntMBusCmd.TabIndex = 56;
      this.bntMBusCmd.Text = "MBus Commands";
      this.bntMBusCmd.UseVisualStyleBackColor = true;
      this.bntMBusCmd.Click += new System.EventHandler(this.bntMBusCmd_Click);
      this.bntCommonCmd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bntCommonCmd.Location = new Point(636, 455);
      this.bntCommonCmd.Name = "bntCommonCmd";
      this.bntCommonCmd.Size = new Size(184, 21);
      this.bntCommonCmd.TabIndex = 57;
      this.bntCommonCmd.Text = "Common Commands";
      this.bntCommonCmd.UseVisualStyleBackColor = true;
      this.bntCommonCmd.Click += new System.EventHandler(this.bntCommonCmd_Click);
      this.btnSpecialCommands.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSpecialCommands.Location = new Point(636, 481);
      this.btnSpecialCommands.Name = "btnSpecialCommands";
      this.btnSpecialCommands.Size = new Size(184, 21);
      this.btnSpecialCommands.TabIndex = 58;
      this.btnSpecialCommands.Text = "Special Commands";
      this.btnSpecialCommands.UseVisualStyleBackColor = true;
      this.btnSpecialCommands.Click += new System.EventHandler(this.btnSpecialCommands_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(828, 562);
      this.Controls.Add((Control) this.btnSpecialCommands);
      this.Controls.Add((Control) this.bntCommonCmd);
      this.Controls.Add((Control) this.bntMBusCmd);
      this.Controls.Add((Control) this.bntLoRaCmd);
      this.Controls.Add((Control) this.bntRadioCmd);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.btnLoadBackup);
      this.Controls.Add((Control) this.btnCreateBackup);
      this.Controls.Add((Control) this.gboxReadOptions);
      this.Controls.Add((Control) this.btnTestCommandDialog);
      this.Controls.Add((Control) this.btnWriteDevice);
      this.Controls.Add((Control) this.txtWorkDevice);
      this.Controls.Add((Control) this.btnConfigurator);
      this.Controls.Add((Control) this.btnClear);
      this.Controls.Add((Control) this.statusStrip);
      this.Controls.Add((Control) this.btnReadDevice);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Controls.Add((Control) this.menu);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SmokeDetectorWindow);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Smoke Detector";
      this.FormClosing += new FormClosingEventHandler(this.SmokeDetectorWindow_FormClosing);
      this.Load += new System.EventHandler(this.SmokeDetectorWindow_Load);
      this.menu.ResumeLayout(false);
      this.menu.PerformLayout();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.gboxReadOptions.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
