// Decompiled with JetBrains decompiler
// Type: AsyncCom.ZR_AsyncCom
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using CorporateDesign;
using MinoConnect;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  public class ZR_AsyncCom : Form
  {
    private int ComStringLength = 10;
    private static int[] BaudrateTable = new int[10]
    {
      300,
      600,
      1200,
      2400,
      4800,
      9600,
      19200,
      38400,
      57600,
      115200
    };
    private static string[] ParityTable = new string[3]
    {
      "no",
      "even",
      "odd"
    };
    private static Logger logger = LogManager.GetLogger(nameof (ZR_AsyncCom));
    private AsyncFunctions ComX;
    private bool LoggerIsOn;
    private bool EventsEnabled = true;
    private Size StartWindowSize;
    internal string StartComponentName;
    private string[] MinoConnectModesList;
    private Color DefaultEnabledBackColor;
    private MainMenu mainMenu1;
    private MenuItem menuItem1;
    private MenuItem ComOpen;
    private MenuItem TestHalloWorld;
    private MenuItem menuLesen;
    private TextBox LoggerBox;
    private MenuItem ComClose;
    private MenuItem TestTimeout;
    private MenuItem DefaultTimeout;
    private ListBox Baudrate;
    private Label label1;
    private Label label2;
    private Button ok;
    private Button cancel;
    private CheckBox SelectEchoOn;
    private TextBox textBoxWaitBeforeRepeatTime;
    private Label label4;
    private Label label3;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private TextBox textBoxRecTransTime;
    private Label label10;
    private Label label11;
    private TextBox textBoxTransTime_AfterBreak;
    private TextBox textBoxRecTime_BeforFirstByte;
    private TextBox textBoxRecTime_OffsetPerByte;
    private TextBox textBoxRecTime_GlobalOffset;
    private TextBox textBoxTransTime_BreakTime;
    private TextBox textBoxBreakIntervalTime;
    private IContainer components;
    private CheckBox checkBoxComOpenClose;
    private CheckBox checkBoxComData;
    private CheckBox checkBoxComState;
    private CheckBox checkBoxComErrors;
    private CheckBox checkBoxComPolling;
    private CheckBox checkBoxBusFunctions;
    private CheckBox checkBoxBusStatus;
    private MenuItem menuItemHelpMenu;
    private MenuItem menuItemAbout;
    private MenuItem menuStarten;
    private MenuItem menuStartGlobalMeterManager;
    private ListBox listBoxParity;
    private Label label12;
    private MenuItem menuItemSetBreak;
    private MenuItem menuItemClearBreak;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private MenuItem menuBack;
    private MenuItem menuQuit;
    private CheckBox checkBoxTestEcho;
    private ToolTip toolTip1;
    private Button buttonSet;
    private MenuItem menuItemTests;
    private GroupBox groupBoxLoggerSetup;
    private Label labelComServer;
    private ListBox listBoxAvailableComPorts;
    private TabControl tabControl1;
    private TabPage tabPageBasic;
    private TabPage tabPageLogger;
    private TabPage tabPageTiming;
    private TabPage tabPageTerminal;
    private TextBox textBoxTerminal;
    private TabPage tabPageEquipment;
    private GroupBox groupBoxMinoConnect;
    private GroupBox groupBoxDeviceEcho;
    private GroupBox groupBoxWakeUp;
    private System.Windows.Forms.Timer timerTerminal;
    private Label label5;
    private TextBox textBoxType;
    private ComboBox comboBoxSelectedMiConMode;
    private TextBox textBoxTransceiverDeviceInfo;
    private TextBox textBoxAutoPowerOff;
    private Label label13;
    private TextBox textBoxKeyCounter;
    private Label label14;
    private TextBox textBoxMinoConnectRequests;
    private TextBox textBoxActiveComPort;
    private ComboBox comboBoxAvailableCOMservers;
    private Label labelModeState;
    private Button buttonCOMserverAdmin;
    private Label label19;
    private MenuItem menuItemHelp;
    private Label label20;
    private TextBox textBoxCOMserverIP;
    private TextBox textBoxLoggerEntries;
    private Label label21;
    private MenuItem menuItemStartWindow;
    private MenuItem menuItem2;
    private MenuItem menuItemDeviceCollector;
    private Label label23;
    private ComboBox comboBoxTransceiverDevice;
    private Label label24;
    private ComboBox comboBoxDeviceWakeup;
    private GroupBox groupBoxCombiOptoHead;
    private CheckBox checkBoxIrDa;
    private CheckBox checkBoxDoveTailSide;
    private Panel panelBasic;
    private Panel panelEquipment;
    private Panel panelTiming;
    private Button buttonOpenClose;
    private TextBox textBoxOverloadCounter;
    private Label label22;
    private ComboBox comboBoxComType;
    private Label label25;
    private GroupBox groupBoxRemoteCom;
    private Button buttonRefreshPortList;
    private MenuItem menuItemSearchUSB;
    private SplitContainer splitContainer1;
    private RadioButton radioButtonHex;
    private Label label27;
    private Label label26;
    private RadioButton radioButtonASCIdirect;
    private RadioButton radioButtonASCI;
    private Button buttonTransmit;
    private MenuItem menuItemDesigner;
    private TextBox textBoxIrDaPulseLength;
    private Label label28;
    private Button btnUpgradeMinoConnectFirmware;
    private Label label29;
    private TextBox textBoxRecTime_OffsetPerBlock;
    private TextBox textBoxMinoConnectState;
    private CheckBox checkBoxLoggerOn;
    private Button buttonClearLogger;
    private Button buttonRefreshLogger;
    private MenuItem menuItemFrameTestWindowActive;
    private Button buttonDefaultTiming;
    private MenuItem menuItemSendWakeup;
    private MenuItem menuItemWakeupLoopOn;
    private Button btnUpdateComServers;
    private MenuItem menuItemConfigurator;
    private TextBox textBoxTransTime_AfterOpen;
    private Label label15;
    private CheckBox checkBoxBusErrors;

    public ZR_AsyncCom(AsyncFunctions MyCom)
    {
      this.InitializeComponent();
      if (Thread.CurrentThread.Name != "GMM main")
        this.Text = this.Text + " [" + Thread.CurrentThread.Name + "]";
      this.ComX = MyCom;
      this.LoggerIsOn = false;
      this.StartWindowSize = this.ClientSize;
      this.textBoxMinoConnectState.Size = new Size(this.textBoxMinoConnectState.Size.Width, this.textBoxMinoConnectState.Size.Height + (this.textBoxMinoConnectState.Location.Y - this.comboBoxSelectedMiConMode.Location.Y));
      this.textBoxMinoConnectState.Location = this.comboBoxSelectedMiConMode.Location;
      if (!UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer))
      {
        this.menuItemTests.Visible = false;
        this.groupBoxLoggerSetup.Visible = false;
        this.tabControl1.TabPages.Remove(this.tabPageLogger);
        this.tabControl1.TabPages.Remove(this.tabPageTerminal);
      }
      if (this.ComX.ConnectionTypeSelected == AsyncComConnectionType.COM)
        this.ComUpdateComPortList();
      else
        this.UpdateComPortList(true);
      for (int index = 0; index < ZR_AsyncCom.BaudrateTable.Length; ++index)
        this.Baudrate.Items.Add((object) ZR_AsyncCom.BaudrateTable[index].ToString());
      this.comboBoxDeviceWakeup.DataSource = (object) Enum.GetNames(typeof (WakeupSystem));
      this.comboBoxTransceiverDevice.DataSource = (object) Enum.GetNames(typeof (ZR_ClassLibrary.TransceiverDevice));
      this.DefaultEnabledBackColor = this.comboBoxSelectedMiConMode.BackColor;
      this.MinoConnectModesList = Enum.GetNames(typeof (MinoConnectState.BaseStateEnum));
      for (int index = 0; index < this.MinoConnectModesList.Length - 1; ++index)
      {
        if (!this.MinoConnectModesList[index].Contains("Radio") && !this.MinoConnectModesList[index].Contains("WirelessMBus"))
          this.comboBoxSelectedMiConMode.Items.Add((object) this.MinoConnectModesList[index]);
      }
      string[] names = Enum.GetNames(typeof (AsyncComConnectionType));
      int num = 0;
      for (int index = 0; index < names.Length; ++index)
      {
        this.comboBoxComType.Items.Add((object) names[index]);
        if (MyCom.ConnectionTypeSelected.ToString() == names[index])
          num = index;
      }
      this.comboBoxComType.SelectedIndex = num;
      this.SetLoggerEvents();
      this.textBoxActiveComPort.BackColor = Control.DefaultBackColor;
      this.ComX.RefreshComPorts();
      this.SetComState();
      MyCom.OnAsyncComMessage += new EventHandler<GMM_EventArgs>(this.TheStatusChangedMessage);
      this.SetEnabledFunctions();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      if (this.ComX != null)
        this.ComX.OnAsyncComMessage -= new EventHandler<GMM_EventArgs>(this.TheStatusChangedMessage);
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ZR_AsyncCom));
      this.splitContainer1 = new SplitContainer();
      this.label26 = new Label();
      this.textBoxTerminal = new TextBox();
      this.label5 = new Label();
      this.textBoxType = new TextBox();
      this.mainMenu1 = new MainMenu(this.components);
      this.menuItem1 = new MenuItem();
      this.ComOpen = new MenuItem();
      this.ComClose = new MenuItem();
      this.menuItemSetBreak = new MenuItem();
      this.menuItemClearBreak = new MenuItem();
      this.menuItemTests = new MenuItem();
      this.TestHalloWorld = new MenuItem();
      this.menuLesen = new MenuItem();
      this.TestTimeout = new MenuItem();
      this.DefaultTimeout = new MenuItem();
      this.menuItemSearchUSB = new MenuItem();
      this.menuItemFrameTestWindowActive = new MenuItem();
      this.menuItemSendWakeup = new MenuItem();
      this.menuItemWakeupLoopOn = new MenuItem();
      this.menuStarten = new MenuItem();
      this.menuItemStartWindow = new MenuItem();
      this.menuStartGlobalMeterManager = new MenuItem();
      this.menuBack = new MenuItem();
      this.menuQuit = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.menuItemDeviceCollector = new MenuItem();
      this.menuItemDesigner = new MenuItem();
      this.menuItemConfigurator = new MenuItem();
      this.menuItemHelpMenu = new MenuItem();
      this.menuItemHelp = new MenuItem();
      this.menuItemAbout = new MenuItem();
      this.LoggerBox = new TextBox();
      this.Baudrate = new ListBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.ok = new Button();
      this.cancel = new Button();
      this.SelectEchoOn = new CheckBox();
      this.textBoxWaitBeforeRepeatTime = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.textBoxRecTransTime = new TextBox();
      this.label6 = new Label();
      this.textBoxTransTime_AfterBreak = new TextBox();
      this.label7 = new Label();
      this.textBoxRecTime_BeforFirstByte = new TextBox();
      this.label8 = new Label();
      this.textBoxRecTime_OffsetPerByte = new TextBox();
      this.label9 = new Label();
      this.textBoxRecTime_GlobalOffset = new TextBox();
      this.label10 = new Label();
      this.textBoxTransTime_BreakTime = new TextBox();
      this.label11 = new Label();
      this.textBoxBreakIntervalTime = new TextBox();
      this.groupBoxLoggerSetup = new GroupBox();
      this.buttonRefreshLogger = new Button();
      this.buttonClearLogger = new Button();
      this.checkBoxLoggerOn = new CheckBox();
      this.textBoxLoggerEntries = new TextBox();
      this.label21 = new Label();
      this.checkBoxComOpenClose = new CheckBox();
      this.checkBoxComData = new CheckBox();
      this.checkBoxComState = new CheckBox();
      this.checkBoxComErrors = new CheckBox();
      this.checkBoxComPolling = new CheckBox();
      this.checkBoxBusFunctions = new CheckBox();
      this.checkBoxBusErrors = new CheckBox();
      this.checkBoxBusStatus = new CheckBox();
      this.listBoxParity = new ListBox();
      this.label12 = new Label();
      this.checkBoxTestEcho = new CheckBox();
      this.toolTip1 = new ToolTip(this.components);
      this.buttonOpenClose = new Button();
      this.textBoxRecTime_OffsetPerBlock = new TextBox();
      this.textBoxTransTime_AfterOpen = new TextBox();
      this.listBoxAvailableComPorts = new ListBox();
      this.buttonSet = new Button();
      this.labelComServer = new Label();
      this.tabControl1 = new TabControl();
      this.tabPageBasic = new TabPage();
      this.panelBasic = new Panel();
      this.buttonRefreshPortList = new Button();
      this.groupBoxRemoteCom = new GroupBox();
      this.btnUpdateComServers = new Button();
      this.comboBoxAvailableCOMservers = new ComboBox();
      this.buttonCOMserverAdmin = new Button();
      this.textBoxCOMserverIP = new TextBox();
      this.label20 = new Label();
      this.comboBoxComType = new ComboBox();
      this.label25 = new Label();
      this.textBoxActiveComPort = new TextBox();
      this.tabPageEquipment = new TabPage();
      this.panelEquipment = new Panel();
      this.btnUpgradeMinoConnectFirmware = new Button();
      this.label23 = new Label();
      this.textBoxTransceiverDeviceInfo = new TextBox();
      this.groupBoxMinoConnect = new GroupBox();
      this.textBoxMinoConnectState = new TextBox();
      this.labelModeState = new Label();
      this.groupBoxCombiOptoHead = new GroupBox();
      this.checkBoxIrDa = new CheckBox();
      this.checkBoxDoveTailSide = new CheckBox();
      this.textBoxOverloadCounter = new TextBox();
      this.textBoxMinoConnectRequests = new TextBox();
      this.textBoxKeyCounter = new TextBox();
      this.comboBoxSelectedMiConMode = new ComboBox();
      this.textBoxIrDaPulseLength = new TextBox();
      this.textBoxAutoPowerOff = new TextBox();
      this.label22 = new Label();
      this.label19 = new Label();
      this.label14 = new Label();
      this.label28 = new Label();
      this.label13 = new Label();
      this.label24 = new Label();
      this.groupBoxDeviceEcho = new GroupBox();
      this.groupBoxWakeUp = new GroupBox();
      this.comboBoxDeviceWakeup = new ComboBox();
      this.comboBoxTransceiverDevice = new ComboBox();
      this.tabPageTiming = new TabPage();
      this.panelTiming = new Panel();
      this.buttonDefaultTiming = new Button();
      this.label29 = new Label();
      this.label15 = new Label();
      this.tabPageLogger = new TabPage();
      this.tabPageTerminal = new TabPage();
      this.radioButtonASCIdirect = new RadioButton();
      this.radioButtonASCI = new RadioButton();
      this.radioButtonHex = new RadioButton();
      this.label27 = new Label();
      this.buttonTransmit = new Button();
      this.timerTerminal = new System.Windows.Forms.Timer(this.components);
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBoxLoggerSetup.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPageBasic.SuspendLayout();
      this.panelBasic.SuspendLayout();
      this.groupBoxRemoteCom.SuspendLayout();
      this.tabPageEquipment.SuspendLayout();
      this.panelEquipment.SuspendLayout();
      this.groupBoxMinoConnect.SuspendLayout();
      this.groupBoxCombiOptoHead.SuspendLayout();
      this.groupBoxDeviceEcho.SuspendLayout();
      this.groupBoxWakeUp.SuspendLayout();
      this.tabPageTiming.SuspendLayout();
      this.panelTiming.SuspendLayout();
      this.tabPageLogger.SuspendLayout();
      this.tabPageTerminal.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.label26);
      this.splitContainer1.Panel1.Controls.Add((Control) this.textBoxTerminal);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label5);
      this.splitContainer1.Panel2.Controls.Add((Control) this.textBoxType);
      componentResourceManager.ApplyResources((object) this.label26, "label26");
      this.label26.Name = "label26";
      componentResourceManager.ApplyResources((object) this.textBoxTerminal, "textBoxTerminal");
      this.textBoxTerminal.BackColor = Color.White;
      this.textBoxTerminal.ForeColor = Color.Black;
      this.textBoxTerminal.Name = "textBoxTerminal";
      this.textBoxTerminal.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.textBoxType, "textBoxType");
      this.textBoxType.Name = "textBoxType";
      this.textBoxType.TextChanged += new System.EventHandler(this.textBoxType_TextChanged);
      this.textBoxType.Enter += new System.EventHandler(this.textBoxType_Enter);
      this.textBoxType.Leave += new System.EventHandler(this.textBoxType_Leave);
      this.mainMenu1.MenuItems.AddRange(new MenuItem[4]
      {
        this.menuItem1,
        this.menuItemTests,
        this.menuStarten,
        this.menuItemHelpMenu
      });
      this.menuItem1.Index = 0;
      this.menuItem1.MenuItems.AddRange(new MenuItem[4]
      {
        this.ComOpen,
        this.ComClose,
        this.menuItemSetBreak,
        this.menuItemClearBreak
      });
      componentResourceManager.ApplyResources((object) this.menuItem1, "menuItem1");
      this.ComOpen.Index = 0;
      componentResourceManager.ApplyResources((object) this.ComOpen, "ComOpen");
      this.ComOpen.Click += new System.EventHandler(this.ComOpen_Click);
      this.ComClose.Index = 1;
      componentResourceManager.ApplyResources((object) this.ComClose, "ComClose");
      this.ComClose.Click += new System.EventHandler(this.ComClose_Click);
      this.menuItemSetBreak.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItemSetBreak, "menuItemSetBreak");
      this.menuItemSetBreak.Click += new System.EventHandler(this.menuItemSetBreak_Click);
      this.menuItemClearBreak.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuItemClearBreak, "menuItemClearBreak");
      this.menuItemClearBreak.Click += new System.EventHandler(this.menuItemClearBreak_Click);
      this.menuItemTests.Index = 1;
      this.menuItemTests.MenuItems.AddRange(new MenuItem[8]
      {
        this.TestHalloWorld,
        this.menuLesen,
        this.TestTimeout,
        this.DefaultTimeout,
        this.menuItemSearchUSB,
        this.menuItemFrameTestWindowActive,
        this.menuItemSendWakeup,
        this.menuItemWakeupLoopOn
      });
      componentResourceManager.ApplyResources((object) this.menuItemTests, "menuItemTests");
      this.TestHalloWorld.Index = 0;
      componentResourceManager.ApplyResources((object) this.TestHalloWorld, "TestHalloWorld");
      this.TestHalloWorld.Click += new System.EventHandler(this.TestHalloWorld_Click);
      this.menuLesen.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuLesen, "menuLesen");
      this.menuLesen.Click += new System.EventHandler(this.menuLesen_Click);
      this.TestTimeout.Index = 2;
      componentResourceManager.ApplyResources((object) this.TestTimeout, "TestTimeout");
      this.DefaultTimeout.Index = 3;
      componentResourceManager.ApplyResources((object) this.DefaultTimeout, "DefaultTimeout");
      this.menuItemSearchUSB.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItemSearchUSB, "menuItemSearchUSB");
      this.menuItemSearchUSB.Click += new System.EventHandler(this.menuItemSearchUSB_Click);
      this.menuItemFrameTestWindowActive.Index = 5;
      this.menuItemFrameTestWindowActive.RadioCheck = true;
      componentResourceManager.ApplyResources((object) this.menuItemFrameTestWindowActive, "menuItemFrameTestWindowActive");
      this.menuItemFrameTestWindowActive.Click += new System.EventHandler(this.menuItemFrameTestWindowActive_Click);
      this.menuItemSendWakeup.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItemSendWakeup, "menuItemSendWakeup");
      this.menuItemSendWakeup.Click += new System.EventHandler(this.menuItemSendWakeup_Click);
      this.menuItemWakeupLoopOn.Index = 7;
      this.menuItemWakeupLoopOn.RadioCheck = true;
      componentResourceManager.ApplyResources((object) this.menuItemWakeupLoopOn, "menuItemWakeupLoopOn");
      this.menuItemWakeupLoopOn.Click += new System.EventHandler(this.menuItemWakeupLoopOn_Click);
      this.menuStarten.Index = 2;
      this.menuStarten.MenuItems.AddRange(new MenuItem[8]
      {
        this.menuItemStartWindow,
        this.menuStartGlobalMeterManager,
        this.menuBack,
        this.menuQuit,
        this.menuItem2,
        this.menuItemDeviceCollector,
        this.menuItemDesigner,
        this.menuItemConfigurator
      });
      componentResourceManager.ApplyResources((object) this.menuStarten, "menuStarten");
      this.menuItemStartWindow.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuItemStartWindow, "menuItemStartWindow");
      this.menuItemStartWindow.Click += new System.EventHandler(this.menuItemStartWindow_Click);
      this.menuStartGlobalMeterManager.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuStartGlobalMeterManager, "menuStartGlobalMeterManager");
      this.menuStartGlobalMeterManager.Click += new System.EventHandler(this.menuStartGlobalMeterManager_Click);
      this.menuBack.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuBack, "menuBack");
      this.menuBack.Click += new System.EventHandler(this.menuBack_Click);
      this.menuQuit.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuQuit, "menuQuit");
      this.menuQuit.Click += new System.EventHandler(this.menuQuit_Click);
      this.menuItem2.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItem2, "menuItem2");
      this.menuItemDeviceCollector.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuItemDeviceCollector, "menuItemDeviceCollector");
      this.menuItemDeviceCollector.Click += new System.EventHandler(this.menuItemSerialBus_Click);
      this.menuItemDesigner.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItemDesigner, "menuItemDesigner");
      this.menuItemDesigner.Click += new System.EventHandler(this.menuItemDesigner_Click);
      this.menuItemConfigurator.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuItemConfigurator, "menuItemConfigurator");
      this.menuItemConfigurator.Click += new System.EventHandler(this.menuItemConfigurator_Click);
      this.menuItemHelpMenu.Index = 3;
      this.menuItemHelpMenu.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItemHelp,
        this.menuItemAbout
      });
      componentResourceManager.ApplyResources((object) this.menuItemHelpMenu, "menuItemHelpMenu");
      this.menuItemHelp.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuItemHelp, "menuItemHelp");
      this.menuItemHelp.Click += new System.EventHandler(this.menuItemHelp_Click);
      this.menuItemAbout.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuItemAbout, "menuItemAbout");
      this.menuItemAbout.Click += new System.EventHandler(this.menuItem7_Click);
      componentResourceManager.ApplyResources((object) this.LoggerBox, "LoggerBox");
      this.LoggerBox.Name = "LoggerBox";
      componentResourceManager.ApplyResources((object) this.Baudrate, "Baudrate");
      this.Baudrate.Name = "Baudrate";
      this.Baudrate.SelectedIndexChanged += new System.EventHandler(this.Baudrate_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.ok, "ok");
      this.ok.DialogResult = DialogResult.OK;
      this.ok.Name = "ok";
      this.ok.Click += new System.EventHandler(this.ok_Click);
      componentResourceManager.ApplyResources((object) this.cancel, "cancel");
      this.cancel.DialogResult = DialogResult.Cancel;
      this.cancel.Name = "cancel";
      this.cancel.Click += new System.EventHandler(this.cancel_Click);
      componentResourceManager.ApplyResources((object) this.SelectEchoOn, "SelectEchoOn");
      this.SelectEchoOn.Name = "SelectEchoOn";
      this.SelectEchoOn.CheckedChanged += new System.EventHandler(this.SelectEchoOn_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.textBoxWaitBeforeRepeatTime, "textBoxWaitBeforeRepeatTime");
      this.textBoxWaitBeforeRepeatTime.Name = "textBoxWaitBeforeRepeatTime";
      this.toolTip1.SetToolTip((Control) this.textBoxWaitBeforeRepeatTime, componentResourceManager.GetString("textBoxWaitBeforeRepeatTime.ToolTip"));
      this.textBoxWaitBeforeRepeatTime.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.textBoxRecTransTime, "textBoxRecTransTime");
      this.textBoxRecTransTime.Name = "textBoxRecTransTime";
      this.toolTip1.SetToolTip((Control) this.textBoxRecTransTime, componentResourceManager.GetString("textBoxRecTransTime.ToolTip"));
      this.textBoxRecTransTime.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.textBoxTransTime_AfterBreak, "textBoxTransTime_AfterBreak");
      this.textBoxTransTime_AfterBreak.Name = "textBoxTransTime_AfterBreak";
      this.toolTip1.SetToolTip((Control) this.textBoxTransTime_AfterBreak, componentResourceManager.GetString("textBoxTransTime_AfterBreak.ToolTip"));
      this.textBoxTransTime_AfterBreak.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.textBoxRecTime_BeforFirstByte, "textBoxRecTime_BeforFirstByte");
      this.textBoxRecTime_BeforFirstByte.Name = "textBoxRecTime_BeforFirstByte";
      this.toolTip1.SetToolTip((Control) this.textBoxRecTime_BeforFirstByte, componentResourceManager.GetString("textBoxRecTime_BeforFirstByte.ToolTip"));
      this.textBoxRecTime_BeforFirstByte.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this.textBoxRecTime_OffsetPerByte, "textBoxRecTime_OffsetPerByte");
      this.textBoxRecTime_OffsetPerByte.Name = "textBoxRecTime_OffsetPerByte";
      this.toolTip1.SetToolTip((Control) this.textBoxRecTime_OffsetPerByte, componentResourceManager.GetString("textBoxRecTime_OffsetPerByte.ToolTip"));
      this.textBoxRecTime_OffsetPerByte.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label9, "label9");
      this.label9.Name = "label9";
      componentResourceManager.ApplyResources((object) this.textBoxRecTime_GlobalOffset, "textBoxRecTime_GlobalOffset");
      this.textBoxRecTime_GlobalOffset.Name = "textBoxRecTime_GlobalOffset";
      this.toolTip1.SetToolTip((Control) this.textBoxRecTime_GlobalOffset, componentResourceManager.GetString("textBoxRecTime_GlobalOffset.ToolTip"));
      this.textBoxRecTime_GlobalOffset.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label10, "label10");
      this.label10.Name = "label10";
      componentResourceManager.ApplyResources((object) this.textBoxTransTime_BreakTime, "textBoxTransTime_BreakTime");
      this.textBoxTransTime_BreakTime.Name = "textBoxTransTime_BreakTime";
      this.toolTip1.SetToolTip((Control) this.textBoxTransTime_BreakTime, componentResourceManager.GetString("textBoxTransTime_BreakTime.ToolTip"));
      this.textBoxTransTime_BreakTime.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.label11, "label11");
      this.label11.Name = "label11";
      componentResourceManager.ApplyResources((object) this.textBoxBreakIntervalTime, "textBoxBreakIntervalTime");
      this.textBoxBreakIntervalTime.Name = "textBoxBreakIntervalTime";
      this.toolTip1.SetToolTip((Control) this.textBoxBreakIntervalTime, componentResourceManager.GetString("textBoxBreakIntervalTime.ToolTip"));
      this.textBoxBreakIntervalTime.TextChanged += new System.EventHandler(this.TimeChanged);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.buttonRefreshLogger);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.buttonClearLogger);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxLoggerOn);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.textBoxLoggerEntries);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.label21);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxComOpenClose);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxComData);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxComState);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxComErrors);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxComPolling);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxBusFunctions);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxBusErrors);
      this.groupBoxLoggerSetup.Controls.Add((Control) this.checkBoxBusStatus);
      componentResourceManager.ApplyResources((object) this.groupBoxLoggerSetup, "groupBoxLoggerSetup");
      this.groupBoxLoggerSetup.Name = "groupBoxLoggerSetup";
      this.groupBoxLoggerSetup.TabStop = false;
      componentResourceManager.ApplyResources((object) this.buttonRefreshLogger, "buttonRefreshLogger");
      this.buttonRefreshLogger.Name = "buttonRefreshLogger";
      this.buttonRefreshLogger.UseVisualStyleBackColor = true;
      this.buttonRefreshLogger.Click += new System.EventHandler(this.buttonRefreshLogger_Click);
      componentResourceManager.ApplyResources((object) this.buttonClearLogger, "buttonClearLogger");
      this.buttonClearLogger.Name = "buttonClearLogger";
      this.buttonClearLogger.UseVisualStyleBackColor = true;
      this.buttonClearLogger.Click += new System.EventHandler(this.buttonClearLogger_Click);
      componentResourceManager.ApplyResources((object) this.checkBoxLoggerOn, "checkBoxLoggerOn");
      this.checkBoxLoggerOn.Name = "checkBoxLoggerOn";
      this.checkBoxLoggerOn.UseVisualStyleBackColor = true;
      this.checkBoxLoggerOn.CheckedChanged += new System.EventHandler(this.checkBoxLoggerOn_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.textBoxLoggerEntries, "textBoxLoggerEntries");
      this.textBoxLoggerEntries.Name = "textBoxLoggerEntries";
      componentResourceManager.ApplyResources((object) this.label21, "label21");
      this.label21.Name = "label21";
      this.checkBoxComOpenClose.Checked = true;
      this.checkBoxComOpenClose.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxComOpenClose, "checkBoxComOpenClose");
      this.checkBoxComOpenClose.Name = "checkBoxComOpenClose";
      this.checkBoxComData.Checked = true;
      this.checkBoxComData.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxComData, "checkBoxComData");
      this.checkBoxComData.Name = "checkBoxComData";
      this.checkBoxComState.Checked = true;
      this.checkBoxComState.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxComState, "checkBoxComState");
      this.checkBoxComState.Name = "checkBoxComState";
      this.checkBoxComErrors.Checked = true;
      this.checkBoxComErrors.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxComErrors, "checkBoxComErrors");
      this.checkBoxComErrors.Name = "checkBoxComErrors";
      componentResourceManager.ApplyResources((object) this.checkBoxComPolling, "checkBoxComPolling");
      this.checkBoxComPolling.Name = "checkBoxComPolling";
      this.checkBoxBusFunctions.Checked = true;
      this.checkBoxBusFunctions.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxBusFunctions, "checkBoxBusFunctions");
      this.checkBoxBusFunctions.Name = "checkBoxBusFunctions";
      this.checkBoxBusErrors.Checked = true;
      this.checkBoxBusErrors.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxBusErrors, "checkBoxBusErrors");
      this.checkBoxBusErrors.Name = "checkBoxBusErrors";
      this.checkBoxBusStatus.Checked = true;
      this.checkBoxBusStatus.CheckState = CheckState.Checked;
      componentResourceManager.ApplyResources((object) this.checkBoxBusStatus, "checkBoxBusStatus");
      this.checkBoxBusStatus.Name = "checkBoxBusStatus";
      componentResourceManager.ApplyResources((object) this.listBoxParity, "listBoxParity");
      this.listBoxParity.Items.AddRange(new object[3]
      {
        (object) componentResourceManager.GetString("listBoxParity.Items"),
        (object) componentResourceManager.GetString("listBoxParity.Items1"),
        (object) componentResourceManager.GetString("listBoxParity.Items2")
      });
      this.listBoxParity.Name = "listBoxParity";
      this.listBoxParity.SelectedIndexChanged += new System.EventHandler(this.listBoxParity_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label12, "label12");
      this.label12.Name = "label12";
      componentResourceManager.ApplyResources((object) this.checkBoxTestEcho, "checkBoxTestEcho");
      this.checkBoxTestEcho.Name = "checkBoxTestEcho";
      this.checkBoxTestEcho.CheckedChanged += new System.EventHandler(this.checkBoxTestEcho_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.buttonOpenClose, "buttonOpenClose");
      this.buttonOpenClose.Name = "buttonOpenClose";
      this.toolTip1.SetToolTip((Control) this.buttonOpenClose, componentResourceManager.GetString("buttonOpenClose.ToolTip"));
      this.buttonOpenClose.Click += new System.EventHandler(this.buttonOpenClose_Click);
      componentResourceManager.ApplyResources((object) this.textBoxRecTime_OffsetPerBlock, "textBoxRecTime_OffsetPerBlock");
      this.textBoxRecTime_OffsetPerBlock.Name = "textBoxRecTime_OffsetPerBlock";
      this.toolTip1.SetToolTip((Control) this.textBoxRecTime_OffsetPerBlock, componentResourceManager.GetString("textBoxRecTime_OffsetPerBlock.ToolTip"));
      this.textBoxRecTime_OffsetPerBlock.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.textBoxTransTime_AfterOpen, "textBoxTransTime_AfterOpen");
      this.textBoxTransTime_AfterOpen.Name = "textBoxTransTime_AfterOpen";
      this.toolTip1.SetToolTip((Control) this.textBoxTransTime_AfterOpen, componentResourceManager.GetString("textBoxTransTime_AfterOpen.ToolTip"));
      this.textBoxTransTime_AfterOpen.TextChanged += new System.EventHandler(this.TimeChanged);
      componentResourceManager.ApplyResources((object) this.listBoxAvailableComPorts, "listBoxAvailableComPorts");
      this.listBoxAvailableComPorts.FormattingEnabled = true;
      this.listBoxAvailableComPorts.Name = "listBoxAvailableComPorts";
      this.listBoxAvailableComPorts.SelectedIndexChanged += new System.EventHandler(this.listBoxAvailableComPorts_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.buttonSet, "buttonSet");
      this.buttonSet.Name = "buttonSet";
      this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
      componentResourceManager.ApplyResources((object) this.labelComServer, "labelComServer");
      this.labelComServer.Name = "labelComServer";
      componentResourceManager.ApplyResources((object) this.tabControl1, "tabControl1");
      this.tabControl1.Controls.Add((Control) this.tabPageBasic);
      this.tabControl1.Controls.Add((Control) this.tabPageEquipment);
      this.tabControl1.Controls.Add((Control) this.tabPageTiming);
      this.tabControl1.Controls.Add((Control) this.tabPageLogger);
      this.tabControl1.Controls.Add((Control) this.tabPageTerminal);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabPageBasic.Controls.Add((Control) this.panelBasic);
      componentResourceManager.ApplyResources((object) this.tabPageBasic, "tabPageBasic");
      this.tabPageBasic.Name = "tabPageBasic";
      this.tabPageBasic.UseVisualStyleBackColor = true;
      this.panelBasic.Controls.Add((Control) this.buttonRefreshPortList);
      this.panelBasic.Controls.Add((Control) this.groupBoxRemoteCom);
      this.panelBasic.Controls.Add((Control) this.comboBoxComType);
      this.panelBasic.Controls.Add((Control) this.listBoxParity);
      this.panelBasic.Controls.Add((Control) this.label2);
      this.panelBasic.Controls.Add((Control) this.label1);
      this.panelBasic.Controls.Add((Control) this.Baudrate);
      this.panelBasic.Controls.Add((Control) this.label25);
      this.panelBasic.Controls.Add((Control) this.textBoxActiveComPort);
      this.panelBasic.Controls.Add((Control) this.label12);
      this.panelBasic.Controls.Add((Control) this.listBoxAvailableComPorts);
      componentResourceManager.ApplyResources((object) this.panelBasic, "panelBasic");
      this.panelBasic.Name = "panelBasic";
      componentResourceManager.ApplyResources((object) this.buttonRefreshPortList, "buttonRefreshPortList");
      this.buttonRefreshPortList.Name = "buttonRefreshPortList";
      this.buttonRefreshPortList.UseVisualStyleBackColor = true;
      this.buttonRefreshPortList.Click += new System.EventHandler(this.buttonRefreshPortList_Click);
      this.groupBoxRemoteCom.Controls.Add((Control) this.btnUpdateComServers);
      this.groupBoxRemoteCom.Controls.Add((Control) this.labelComServer);
      this.groupBoxRemoteCom.Controls.Add((Control) this.comboBoxAvailableCOMservers);
      this.groupBoxRemoteCom.Controls.Add((Control) this.buttonCOMserverAdmin);
      this.groupBoxRemoteCom.Controls.Add((Control) this.textBoxCOMserverIP);
      this.groupBoxRemoteCom.Controls.Add((Control) this.label20);
      componentResourceManager.ApplyResources((object) this.groupBoxRemoteCom, "groupBoxRemoteCom");
      this.groupBoxRemoteCom.Name = "groupBoxRemoteCom";
      this.groupBoxRemoteCom.TabStop = false;
      this.btnUpdateComServers.Image = (Image) AsyncComRes.RefreshButton;
      componentResourceManager.ApplyResources((object) this.btnUpdateComServers, "btnUpdateComServers");
      this.btnUpdateComServers.Name = "btnUpdateComServers";
      this.btnUpdateComServers.UseVisualStyleBackColor = true;
      this.btnUpdateComServers.Click += new System.EventHandler(this.btnUpdateComServers_Click);
      this.comboBoxAvailableCOMservers.DrawMode = DrawMode.OwnerDrawFixed;
      this.comboBoxAvailableCOMservers.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxAvailableCOMservers.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBoxAvailableCOMservers, "comboBoxAvailableCOMservers");
      this.comboBoxAvailableCOMservers.Name = "comboBoxAvailableCOMservers";
      this.comboBoxAvailableCOMservers.DrawItem += new DrawItemEventHandler(this.comboBoxAvailableCOMservers_DrawItem);
      this.comboBoxAvailableCOMservers.SelectedIndexChanged += new System.EventHandler(this.comboBoxAvailableCOMservers_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.buttonCOMserverAdmin, "buttonCOMserverAdmin");
      this.buttonCOMserverAdmin.Name = "buttonCOMserverAdmin";
      this.buttonCOMserverAdmin.Click += new System.EventHandler(this.buttonCOMserverAdmin_Click);
      componentResourceManager.ApplyResources((object) this.textBoxCOMserverIP, "textBoxCOMserverIP");
      this.textBoxCOMserverIP.Name = "textBoxCOMserverIP";
      this.textBoxCOMserverIP.TextChanged += new System.EventHandler(this.textBoxCOMserverIP_TextChanged);
      this.textBoxCOMserverIP.KeyPress += new KeyPressEventHandler(this.textBoxCOMserverIP_KeyPress);
      componentResourceManager.ApplyResources((object) this.label20, "label20");
      this.label20.Name = "label20";
      this.comboBoxComType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxComType.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBoxComType, "comboBoxComType");
      this.comboBoxComType.Name = "comboBoxComType";
      this.comboBoxComType.SelectedIndexChanged += new System.EventHandler(this.comboBoxComType_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label25, "label25");
      this.label25.Name = "label25";
      componentResourceManager.ApplyResources((object) this.textBoxActiveComPort, "textBoxActiveComPort");
      this.textBoxActiveComPort.Name = "textBoxActiveComPort";
      this.textBoxActiveComPort.Leave += new System.EventHandler(this.textBoxActiveComPort_Leave);
      this.tabPageEquipment.Controls.Add((Control) this.panelEquipment);
      componentResourceManager.ApplyResources((object) this.tabPageEquipment, "tabPageEquipment");
      this.tabPageEquipment.Name = "tabPageEquipment";
      this.tabPageEquipment.UseVisualStyleBackColor = true;
      this.panelEquipment.Controls.Add((Control) this.btnUpgradeMinoConnectFirmware);
      this.panelEquipment.Controls.Add((Control) this.label23);
      this.panelEquipment.Controls.Add((Control) this.textBoxTransceiverDeviceInfo);
      this.panelEquipment.Controls.Add((Control) this.groupBoxMinoConnect);
      this.panelEquipment.Controls.Add((Control) this.label24);
      this.panelEquipment.Controls.Add((Control) this.groupBoxDeviceEcho);
      this.panelEquipment.Controls.Add((Control) this.groupBoxWakeUp);
      this.panelEquipment.Controls.Add((Control) this.comboBoxTransceiverDevice);
      componentResourceManager.ApplyResources((object) this.panelEquipment, "panelEquipment");
      this.panelEquipment.Name = "panelEquipment";
      componentResourceManager.ApplyResources((object) this.btnUpgradeMinoConnectFirmware, "btnUpgradeMinoConnectFirmware");
      this.btnUpgradeMinoConnectFirmware.Name = "btnUpgradeMinoConnectFirmware";
      this.btnUpgradeMinoConnectFirmware.Click += new System.EventHandler(this.btnUpgradeMinoConnectFirmware_Click);
      componentResourceManager.ApplyResources((object) this.label23, "label23");
      this.label23.Name = "label23";
      componentResourceManager.ApplyResources((object) this.textBoxTransceiverDeviceInfo, "textBoxTransceiverDeviceInfo");
      this.textBoxTransceiverDeviceInfo.Name = "textBoxTransceiverDeviceInfo";
      this.textBoxTransceiverDeviceInfo.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.groupBoxMinoConnect, "groupBoxMinoConnect");
      this.groupBoxMinoConnect.Controls.Add((Control) this.textBoxMinoConnectState);
      this.groupBoxMinoConnect.Controls.Add((Control) this.labelModeState);
      this.groupBoxMinoConnect.Controls.Add((Control) this.groupBoxCombiOptoHead);
      this.groupBoxMinoConnect.Controls.Add((Control) this.textBoxOverloadCounter);
      this.groupBoxMinoConnect.Controls.Add((Control) this.textBoxMinoConnectRequests);
      this.groupBoxMinoConnect.Controls.Add((Control) this.textBoxKeyCounter);
      this.groupBoxMinoConnect.Controls.Add((Control) this.comboBoxSelectedMiConMode);
      this.groupBoxMinoConnect.Controls.Add((Control) this.textBoxIrDaPulseLength);
      this.groupBoxMinoConnect.Controls.Add((Control) this.textBoxAutoPowerOff);
      this.groupBoxMinoConnect.Controls.Add((Control) this.label22);
      this.groupBoxMinoConnect.Controls.Add((Control) this.label19);
      this.groupBoxMinoConnect.Controls.Add((Control) this.label14);
      this.groupBoxMinoConnect.Controls.Add((Control) this.label28);
      this.groupBoxMinoConnect.Controls.Add((Control) this.label13);
      this.groupBoxMinoConnect.Name = "groupBoxMinoConnect";
      this.groupBoxMinoConnect.TabStop = false;
      componentResourceManager.ApplyResources((object) this.textBoxMinoConnectState, "textBoxMinoConnectState");
      this.textBoxMinoConnectState.Name = "textBoxMinoConnectState";
      this.textBoxMinoConnectState.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.labelModeState, "labelModeState");
      this.labelModeState.Name = "labelModeState";
      this.groupBoxCombiOptoHead.Controls.Add((Control) this.checkBoxIrDa);
      this.groupBoxCombiOptoHead.Controls.Add((Control) this.checkBoxDoveTailSide);
      componentResourceManager.ApplyResources((object) this.groupBoxCombiOptoHead, "groupBoxCombiOptoHead");
      this.groupBoxCombiOptoHead.Name = "groupBoxCombiOptoHead";
      this.groupBoxCombiOptoHead.TabStop = false;
      componentResourceManager.ApplyResources((object) this.checkBoxIrDa, "checkBoxIrDa");
      this.checkBoxIrDa.Name = "checkBoxIrDa";
      this.checkBoxIrDa.UseVisualStyleBackColor = true;
      this.checkBoxIrDa.CheckedChanged += new System.EventHandler(this.checkBoxIrDa_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxDoveTailSide, "checkBoxDoveTailSide");
      this.checkBoxDoveTailSide.Name = "checkBoxDoveTailSide";
      this.checkBoxDoveTailSide.UseVisualStyleBackColor = true;
      this.checkBoxDoveTailSide.CheckedChanged += new System.EventHandler(this.checkBoxDoveTailSide_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.textBoxOverloadCounter, "textBoxOverloadCounter");
      this.textBoxOverloadCounter.Name = "textBoxOverloadCounter";
      componentResourceManager.ApplyResources((object) this.textBoxMinoConnectRequests, "textBoxMinoConnectRequests");
      this.textBoxMinoConnectRequests.Name = "textBoxMinoConnectRequests";
      componentResourceManager.ApplyResources((object) this.textBoxKeyCounter, "textBoxKeyCounter");
      this.textBoxKeyCounter.Name = "textBoxKeyCounter";
      componentResourceManager.ApplyResources((object) this.comboBoxSelectedMiConMode, "comboBoxSelectedMiConMode");
      this.comboBoxSelectedMiConMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxSelectedMiConMode.FormattingEnabled = true;
      this.comboBoxSelectedMiConMode.Name = "comboBoxSelectedMiConMode";
      this.comboBoxSelectedMiConMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedPlugState_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.textBoxIrDaPulseLength, "textBoxIrDaPulseLength");
      this.textBoxIrDaPulseLength.Name = "textBoxIrDaPulseLength";
      componentResourceManager.ApplyResources((object) this.textBoxAutoPowerOff, "textBoxAutoPowerOff");
      this.textBoxAutoPowerOff.Name = "textBoxAutoPowerOff";
      componentResourceManager.ApplyResources((object) this.label22, "label22");
      this.label22.Name = "label22";
      componentResourceManager.ApplyResources((object) this.label19, "label19");
      this.label19.Name = "label19";
      componentResourceManager.ApplyResources((object) this.label14, "label14");
      this.label14.Name = "label14";
      componentResourceManager.ApplyResources((object) this.label28, "label28");
      this.label28.Name = "label28";
      componentResourceManager.ApplyResources((object) this.label13, "label13");
      this.label13.Name = "label13";
      componentResourceManager.ApplyResources((object) this.label24, "label24");
      this.label24.Name = "label24";
      componentResourceManager.ApplyResources((object) this.groupBoxDeviceEcho, "groupBoxDeviceEcho");
      this.groupBoxDeviceEcho.Controls.Add((Control) this.checkBoxTestEcho);
      this.groupBoxDeviceEcho.Controls.Add((Control) this.SelectEchoOn);
      this.groupBoxDeviceEcho.Name = "groupBoxDeviceEcho";
      this.groupBoxDeviceEcho.TabStop = false;
      componentResourceManager.ApplyResources((object) this.groupBoxWakeUp, "groupBoxWakeUp");
      this.groupBoxWakeUp.Controls.Add((Control) this.comboBoxDeviceWakeup);
      this.groupBoxWakeUp.Name = "groupBoxWakeUp";
      this.groupBoxWakeUp.TabStop = false;
      componentResourceManager.ApplyResources((object) this.comboBoxDeviceWakeup, "comboBoxDeviceWakeup");
      this.comboBoxDeviceWakeup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDeviceWakeup.FormattingEnabled = true;
      this.comboBoxDeviceWakeup.Name = "comboBoxDeviceWakeup";
      this.comboBoxDeviceWakeup.SelectedIndexChanged += new System.EventHandler(this.comboBoxDeviceWakeup_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.comboBoxTransceiverDevice, "comboBoxTransceiverDevice");
      this.comboBoxTransceiverDevice.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxTransceiverDevice.FormattingEnabled = true;
      this.comboBoxTransceiverDevice.Name = "comboBoxTransceiverDevice";
      this.comboBoxTransceiverDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransceiverDevice_SelectedIndexChanged);
      this.tabPageTiming.Controls.Add((Control) this.panelTiming);
      componentResourceManager.ApplyResources((object) this.tabPageTiming, "tabPageTiming");
      this.tabPageTiming.Name = "tabPageTiming";
      this.tabPageTiming.UseVisualStyleBackColor = true;
      this.panelTiming.Controls.Add((Control) this.buttonDefaultTiming);
      this.panelTiming.Controls.Add((Control) this.label3);
      this.panelTiming.Controls.Add((Control) this.textBoxWaitBeforeRepeatTime);
      this.panelTiming.Controls.Add((Control) this.label29);
      this.panelTiming.Controls.Add((Control) this.label8);
      this.panelTiming.Controls.Add((Control) this.textBoxRecTime_BeforFirstByte);
      this.panelTiming.Controls.Add((Control) this.textBoxRecTime_OffsetPerBlock);
      this.panelTiming.Controls.Add((Control) this.textBoxRecTime_OffsetPerByte);
      this.panelTiming.Controls.Add((Control) this.label4);
      this.panelTiming.Controls.Add((Control) this.label9);
      this.panelTiming.Controls.Add((Control) this.textBoxBreakIntervalTime);
      this.panelTiming.Controls.Add((Control) this.label7);
      this.panelTiming.Controls.Add((Control) this.textBoxRecTime_GlobalOffset);
      this.panelTiming.Controls.Add((Control) this.label11);
      this.panelTiming.Controls.Add((Control) this.textBoxTransTime_AfterOpen);
      this.panelTiming.Controls.Add((Control) this.textBoxTransTime_AfterBreak);
      this.panelTiming.Controls.Add((Control) this.textBoxRecTransTime);
      this.panelTiming.Controls.Add((Control) this.label10);
      this.panelTiming.Controls.Add((Control) this.textBoxTransTime_BreakTime);
      this.panelTiming.Controls.Add((Control) this.label15);
      this.panelTiming.Controls.Add((Control) this.label6);
      componentResourceManager.ApplyResources((object) this.panelTiming, "panelTiming");
      this.panelTiming.Name = "panelTiming";
      componentResourceManager.ApplyResources((object) this.buttonDefaultTiming, "buttonDefaultTiming");
      this.buttonDefaultTiming.Name = "buttonDefaultTiming";
      this.buttonDefaultTiming.UseVisualStyleBackColor = true;
      this.buttonDefaultTiming.Click += new System.EventHandler(this.buttonDefaultTiming_Click);
      componentResourceManager.ApplyResources((object) this.label29, "label29");
      this.label29.Name = "label29";
      componentResourceManager.ApplyResources((object) this.label15, "label15");
      this.label15.Name = "label15";
      this.tabPageLogger.Controls.Add((Control) this.LoggerBox);
      this.tabPageLogger.Controls.Add((Control) this.groupBoxLoggerSetup);
      componentResourceManager.ApplyResources((object) this.tabPageLogger, "tabPageLogger");
      this.tabPageLogger.Name = "tabPageLogger";
      this.tabPageLogger.UseVisualStyleBackColor = true;
      this.tabPageTerminal.Controls.Add((Control) this.radioButtonASCIdirect);
      this.tabPageTerminal.Controls.Add((Control) this.radioButtonASCI);
      this.tabPageTerminal.Controls.Add((Control) this.radioButtonHex);
      this.tabPageTerminal.Controls.Add((Control) this.label27);
      this.tabPageTerminal.Controls.Add((Control) this.buttonTransmit);
      this.tabPageTerminal.Controls.Add((Control) this.splitContainer1);
      componentResourceManager.ApplyResources((object) this.tabPageTerminal, "tabPageTerminal");
      this.tabPageTerminal.Name = "tabPageTerminal";
      this.tabPageTerminal.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.radioButtonASCIdirect, "radioButtonASCIdirect");
      this.radioButtonASCIdirect.Name = "radioButtonASCIdirect";
      this.radioButtonASCIdirect.UseVisualStyleBackColor = true;
      this.radioButtonASCIdirect.CheckedChanged += new System.EventHandler(this.radioButtonASCIdirect_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.radioButtonASCI, "radioButtonASCI");
      this.radioButtonASCI.Name = "radioButtonASCI";
      this.radioButtonASCI.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.radioButtonHex, "radioButtonHex");
      this.radioButtonHex.Checked = true;
      this.radioButtonHex.Name = "radioButtonHex";
      this.radioButtonHex.TabStop = true;
      this.radioButtonHex.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label27, "label27");
      this.label27.Name = "label27";
      componentResourceManager.ApplyResources((object) this.buttonTransmit, "buttonTransmit");
      this.buttonTransmit.Name = "buttonTransmit";
      this.buttonTransmit.Click += new System.EventHandler(this.buttonTransmit_Click);
      this.timerTerminal.Tick += new System.EventHandler(this.timerTerminal_Tick);
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.buttonOpenClose);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.buttonSet);
      this.Controls.Add((Control) this.cancel);
      this.Controls.Add((Control) this.ok);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Menu = this.mainMenu1;
      this.Name = nameof (ZR_AsyncCom);
      this.Activated += new System.EventHandler(this.ZR_AsyncCom_Activated);
      this.Load += new System.EventHandler(this.ZR_AsyncCom_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.groupBoxLoggerSetup.ResumeLayout(false);
      this.groupBoxLoggerSetup.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPageBasic.ResumeLayout(false);
      this.panelBasic.ResumeLayout(false);
      this.panelBasic.PerformLayout();
      this.groupBoxRemoteCom.ResumeLayout(false);
      this.groupBoxRemoteCom.PerformLayout();
      this.tabPageEquipment.ResumeLayout(false);
      this.panelEquipment.ResumeLayout(false);
      this.panelEquipment.PerformLayout();
      this.groupBoxMinoConnect.ResumeLayout(false);
      this.groupBoxMinoConnect.PerformLayout();
      this.groupBoxCombiOptoHead.ResumeLayout(false);
      this.groupBoxCombiOptoHead.PerformLayout();
      this.groupBoxDeviceEcho.ResumeLayout(false);
      this.groupBoxWakeUp.ResumeLayout(false);
      this.tabPageTiming.ResumeLayout(false);
      this.panelTiming.ResumeLayout(false);
      this.panelTiming.PerformLayout();
      this.tabPageLogger.ResumeLayout(false);
      this.tabPageLogger.PerformLayout();
      this.tabPageTerminal.ResumeLayout(false);
      this.tabPageTerminal.PerformLayout();
      this.ResumeLayout(false);
    }

    private void SetEnabledFunctions()
    {
      bool flag1 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.DeviceCollector);
      bool flag2 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Designer);
      bool flag3 = this.comboBoxTransceiverDevice.SelectedIndex == 1;
      bool flag4 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Configurator);
      this.menuItemDeviceCollector.Visible = flag1;
      this.menuItemDesigner.Visible = flag2;
      this.menuItemConfigurator.Visible = flag4;
      this.groupBoxMinoConnect.Enabled = flag3;
    }

    internal void InitStartMenu(string ComponentList)
    {
      this.StartComponentName = "";
      if (ComponentList != null)
        return;
      this.menuStarten.Visible = false;
    }

    private void ComOpen_Click(object sender, EventArgs e)
    {
      this.SetValues();
      this.PrivatOpen();
    }

    private void ComClose_Click(object sender, EventArgs e) => this.PrivatClose();

    private void buttonOpenClose_Click(object sender, EventArgs e)
    {
      this.SetValues();
      if (this.ComX.ComIsOpen)
        this.PrivatClose();
      else
        this.PrivatOpen();
      this.btnUpgradeMinoConnectFirmware.Enabled = this.ComX.ComIsOpen && this.ComX.Transceiver == ZR_ClassLibrary.TransceiverDevice.MinoConnect;
    }

    private bool PrivatOpen()
    {
      this.Cursor = Cursors.WaitCursor;
      bool errorMessageBox = this.ComX.ErrorMessageBox;
      this.ComX.ErrorMessageBox = true;
      this.ComX.BreakRequest = false;
      if (this.comboBoxTransceiverDevice.SelectedIndex == 1)
      {
        this.tabControl1.SelectedTab = this.tabPageEquipment;
        Application.DoEvents();
        Thread.Sleep(0);
      }
      bool flag = this.ComX.Open();
      this.ComX.ErrorMessageBox = errorMessageBox;
      this.Cursor = Cursors.Default;
      return flag;
    }

    private void PrivatClose()
    {
      this.Cursor = Cursors.WaitCursor;
      this.ComX.BreakRequest = true;
      this.ComX.Close();
      this.SetComState();
      this.Cursor = Cursors.Default;
    }

    private void TestHalloWorld_Click(object sender, EventArgs e)
    {
      string str = "Hallo World\n\r";
      ByteField DataBlock = new ByteField(str.Length);
      for (int index = 0; index < str.Length; ++index)
        DataBlock.Add((byte) str[index]);
      this.ComX.TransmitBlock(ref DataBlock);
    }

    private void menuLesen_Click(object sender, EventArgs e)
    {
      string DataString = "";
      ByteField DataBlock = new ByteField();
      this.ComX.ReceiveBlock(ref DataBlock, 10, true);
      DataBlock.ToHexString(ref DataString);
      int num = (int) MessageBox.Show(DataString);
    }

    private void menuItem7_Click(object sender, EventArgs e)
    {
      ArrayList FullNames = new ArrayList();
      Assembly assembly1 = Assembly.GetAssembly(typeof (AsyncFunctions));
      FullNames.Add((object) assembly1.FullName);
      Assembly assembly2 = Assembly.GetAssembly(typeof (ZR_About));
      FullNames.Add((object) assembly2.FullName);
      new ZR_About(FullNames).Show();
    }

    private void timerTerminal_Tick(object sender, EventArgs e)
    {
      if (!this.ComX.ComIsOpen)
      {
        this.timerTerminal.Enabled = false;
        if (!this.ComX.Open())
        {
          this.textBoxTerminal.AppendText(" *Com open error* Terminal stopped!" + Environment.NewLine);
          return;
        }
        this.timerTerminal.Enabled = true;
      }
      string DataString;
      if (!this.ComX.ReceiveString(out DataString))
      {
        this.timerTerminal.Enabled = false;
        string str = " *Receive error* Terminal stopped!" + Environment.NewLine;
      }
      if (DataString.Length <= 0)
        return;
      this.textBoxTerminal.AppendText(DataString);
    }

    private void textBoxType_Enter(object sender, EventArgs e)
    {
      if (!this.radioButtonASCIdirect.Checked)
        return;
      this.timerTerminal.Enabled = true;
    }

    private void textBoxType_Leave(object sender, EventArgs e)
    {
      this.timerTerminal.Enabled = false;
    }

    private void radioButtonASCIdirect_CheckedChanged(object sender, EventArgs e)
    {
      if (this.radioButtonASCIdirect.Checked)
      {
        this.textBoxType.Clear();
        this.buttonTransmit.Enabled = false;
      }
      else
        this.buttonTransmit.Enabled = true;
    }

    private void buttonTransmit_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      this.Enabled = false;
      try
      {
        if (this.radioButtonHex.Checked)
        {
          List<byte> byteList = new List<byte>();
          string str1 = this.textBoxType.Text.Replace("\r", "");
          char[] chArray = new char[1]{ ' ' };
          foreach (string str2 in str1.Split(chArray))
          {
            string s = str2.Trim();
            if (s.Length >= 1 && s.Length <= 2)
              byteList.Add(byte.Parse(s, NumberStyles.HexNumber));
          }
          ByteField DataBlock1 = new ByteField(byteList.Count);
          for (int index = 0; index < byteList.Count; ++index)
            DataBlock1.Add(byteList[index]);
          if (!this.ComX.Open())
            return;
          this.textBoxTerminal.Clear();
          if (!this.ComX.TransmitBlock(ref DataBlock1))
          {
            int num = (int) GMM_MessageBox.ShowMessage("AsymcCom terminal message", "Transmit error");
            return;
          }
          ByteField DataBlock2 = new ByteField(300);
          this.ComX.ReceiveBlock(ref DataBlock2, 300, true);
          if (DataBlock2.Count > 0)
          {
            int index = 0;
            while (index < DataBlock2.Count)
            {
              this.textBoxTerminal.AppendText(DataBlock2.Data[index].ToString("x2"));
              this.textBoxTerminal.AppendText(" ");
              ++index;
              if ((index & 15) == 0)
                this.textBoxTerminal.AppendText(Environment.NewLine);
            }
          }
          else
            this.textBoxTerminal.Text = "No answer";
        }
      }
      catch
      {
      }
      this.Cursor = Cursors.Default;
      this.Enabled = true;
    }

    private void textBoxType_TextChanged(object sender, EventArgs e)
    {
      if (!this.radioButtonASCIdirect.Checked || this.textBoxType.Text.Length == 0)
        return;
      string text = this.textBoxType.Text;
      this.textBoxType.Clear();
      if (!this.ComX.ComIsOpen)
        return;
      this.timerTerminal.Enabled = true;
      if (!this.ComX.TransmitString(text))
      {
        this.timerTerminal.Enabled = false;
        this.textBoxTerminal.AppendText(" *TransmitError*" + Environment.NewLine);
      }
      else
        this.textBoxTerminal.AppendText(text);
    }

    private void menuItemHelp_Click(object sender, EventArgs e)
    {
    }

    private void buttonSet_Click(object sender, EventArgs e) => this.SetValues();

    private void cancel_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Hide();
    }

    private void ok_Click(object sender, EventArgs e)
    {
      this.SetValues();
      this.StartComponentName = "";
      this.Hide();
    }

    private void ZR_AsyncCom_Activated(object sender, EventArgs e)
    {
      if (!this.LoggerIsOn)
        return;
      this.RefreshLogger();
    }

    private void ZR_AsyncCom_Load(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Hide();
    }

    private void SetValues()
    {
      if (this.ComX.ComIsOpen)
        return;
      try
      {
        this.ComX.EchoOn = this.SelectEchoOn.Checked;
        this.ComX.TestEcho = this.checkBoxTestEcho.Checked;
        this.ComX.Wakeup = (WakeupSystem) this.comboBoxDeviceWakeup.SelectedIndex;
        this.ComX.Baudrate = ZR_AsyncCom.BaudrateTable[this.Baudrate.SelectedIndex];
        this.ComX.ComPort = this.textBoxActiveComPort.Text;
        this.ComX.Parity = ZR_AsyncCom.ParityTable[this.listBoxParity.SelectedIndex];
        this.ComX.RecTime_BeforFirstByte = int.Parse(this.textBoxRecTime_BeforFirstByte.Text);
        this.ComX.RecTime_OffsetPerByte = double.Parse(this.textBoxRecTime_OffsetPerByte.Text);
        this.ComX.RecTime_OffsetPerBlock = int.Parse(this.textBoxRecTime_OffsetPerBlock.Text);
        this.ComX.RecTime_GlobalOffset = int.Parse(this.textBoxRecTime_GlobalOffset.Text);
        this.ComX.TransTime_AfterOpen = int.Parse(this.textBoxTransTime_AfterOpen.Text);
        this.ComX.TransTime_BreakTime = int.Parse(this.textBoxTransTime_BreakTime.Text);
        this.ComX.TransTime_AfterBreak = int.Parse(this.textBoxTransTime_AfterBreak.Text);
        this.ComX.RecTransTime = int.Parse(this.textBoxRecTransTime.Text);
        this.ComX.WakeupIntervalTime = int.Parse(this.textBoxBreakIntervalTime.Text);
        this.ComX.WaitBeforeRepeatTime = int.Parse(this.textBoxWaitBeforeRepeatTime.Text);
        this.ComX.Transceiver = (ZR_ClassLibrary.TransceiverDevice) this.comboBoxTransceiverDevice.SelectedIndex;
        this.ComX.IrDa = this.checkBoxIrDa.Checked;
        this.ComX.IrDaDaveTailSide = this.checkBoxDoveTailSide.Checked;
        this.ComX.MinoConnectAutoPowerOffTime = int.Parse(this.textBoxAutoPowerOff.Text);
        this.ComX.MinoConnectIrDaPulseLength = int.Parse(this.textBoxIrDaPulseLength.Text);
        this.ComX.MinoConnectBaseState = (MinoConnectState.BaseStateEnum) Enum.Parse(typeof (MinoConnectState.BaseStateEnum), this.comboBoxSelectedMiConMode.SelectedItem.ToString());
        this.comboBoxSelectedMiConMode.BackColor = this.DefaultEnabledBackColor;
        if (this.ComX.ConnectionTypeSelected.ToString() != this.comboBoxComType.SelectedItem.ToString())
          this.listBoxAvailableComPorts.Items.Clear();
        this.ComX.SetType((AsyncComConnectionType) Enum.Parse(typeof (AsyncComConnectionType), this.comboBoxComType.SelectedItem.ToString(), false));
        this.ComX.RefreshComPorts();
        this.SetComState();
      }
      catch (Exception ex)
      {
        ZR_AsyncCom.logger.Error("Can not set value! Error: {0}", ex.Message);
      }
      this.SetLoggerEvents();
    }

    internal void SetComState()
    {
      this.EventsEnabled = false;
      if (this.ComX.ConnectionTypeSelected == AsyncComConnectionType.Remote || this.ComX.ConnectionTypeSelected == AsyncComConnectionType.Remote_VPN)
      {
        if (this.ComX.ConnectionTypeSelected == AsyncComConnectionType.Remote)
        {
          this.ComX.MyMeterVPN.SelectedCOMserver = this.textBoxCOMserverIP.Text;
          this.buttonCOMserverAdmin.Visible = false;
          this.textBoxCOMserverIP.Enabled = true;
        }
        else
        {
          this.buttonCOMserverAdmin.Visible = true;
          this.textBoxCOMserverIP.Enabled = false;
        }
        this.comboBoxAvailableCOMservers.Enabled = true;
        this.btnUpdateComServers.Enabled = true;
        this.comboBoxAvailableCOMservers.Items.Clear();
        string str = this.ComX.SingleParameter(CommParameter.COMserver, "");
        foreach (DictionaryEntry coMserver1 in this.ComX.MyMeterVPN.COMservers)
        {
          COMserver coMserver2 = (COMserver) coMserver1.Value;
          if (coMserver2.Online)
          {
            this.comboBoxAvailableCOMservers.Items.Add((object) coMserver2.Name);
            if (coMserver1.Key.ToString() == str)
              this.comboBoxAvailableCOMservers.Text = coMserver2.Name;
          }
        }
        foreach (DictionaryEntry coMserver3 in this.ComX.MyMeterVPN.COMservers)
        {
          COMserver coMserver4 = (COMserver) coMserver3.Value;
          if (!coMserver4.Online)
          {
            this.comboBoxAvailableCOMservers.Items.Add((object) ("*" + coMserver4.Name));
            if (coMserver3.Key.ToString() == str)
              this.comboBoxAvailableCOMservers.Text = coMserver4.Name;
          }
        }
      }
      else
      {
        this.buttonCOMserverAdmin.Visible = false;
        this.comboBoxAvailableCOMservers.Enabled = false;
        this.btnUpdateComServers.Enabled = false;
        this.textBoxCOMserverIP.Enabled = false;
      }
      this.EventsEnabled = true;
      this.Baudrate.SelectedIndex = this.GetBaudrateIndex(this.ComX.Baudrate);
      this.listBoxParity.SelectedIndex = this.GetParityIndex(this.ComX.Parity);
      this.textBoxTransTime_AfterOpen.Text = this.ComX.TransTime_AfterOpen.ToString();
      this.textBoxTransTime_AfterBreak.Text = this.ComX.TransTime_AfterBreak.ToString();
      this.textBoxRecTime_BeforFirstByte.Text = this.ComX.RecTime_BeforFirstByte.ToString();
      this.textBoxRecTime_OffsetPerByte.Text = this.ComX.RecTime_OffsetPerByte.ToString();
      this.textBoxRecTime_OffsetPerBlock.Text = this.ComX.RecTime_OffsetPerBlock.ToString();
      this.textBoxRecTime_GlobalOffset.Text = this.ComX.RecTime_GlobalOffset.ToString();
      this.textBoxTransTime_BreakTime.Text = this.ComX.TransTime_BreakTime.ToString();
      this.textBoxRecTransTime.Text = this.ComX.RecTransTime.ToString();
      this.textBoxBreakIntervalTime.Text = this.ComX.WakeupIntervalTime.ToString();
      this.textBoxWaitBeforeRepeatTime.Text = this.ComX.WaitBeforeRepeatTime.ToString();
      this.comboBoxDeviceWakeup.SelectedIndex = (int) this.ComX.Wakeup;
      this.ShowEcho();
      this.checkBoxIrDa.Checked = this.ComX.IrDa;
      this.checkBoxDoveTailSide.Checked = this.ComX.IrDaDaveTailSide;
      this.comboBoxTransceiverDevice.SelectedIndex = (int) this.ComX.Transceiver;
      this.textBoxAutoPowerOff.Text = this.ComX.MinoConnectAutoPowerOffTime.ToString();
      this.textBoxIrDaPulseLength.Text = this.ComX.MinoConnectIrDaPulseLength.ToString();
      string str1 = this.ComX.MinoConnectBaseState.ToString();
      this.comboBoxSelectedMiConMode.SelectedIndex = -1;
      for (int index = 0; index < this.comboBoxSelectedMiConMode.Items.Count; ++index)
      {
        if (this.comboBoxSelectedMiConMode.Items[index].ToString() == str1)
        {
          this.comboBoxSelectedMiConMode.SelectedIndex = index;
          break;
        }
      }
      this.comboBoxSelectedMiConMode.BackColor = this.DefaultEnabledBackColor;
      this.textBoxActiveComPort.Text = this.ComX.ComPort;
      this.SetPortListIndex(this.ComX.ComPort);
      this.listBoxAvailableComPorts.Enabled = true;
      this.SetComOpenState();
      this.buttonSet.Enabled = false;
    }

    public void SetComOpenState()
    {
      this.textBoxTransceiverDeviceInfo.Text = this.ComX.transceiverDeviceInfo;
      if (this.ComX.ComIsOpen)
      {
        this.buttonOpenClose.Image = Images.pics.Close_16x16.Image;
        this.panelBasic.Enabled = false;
        this.panelTiming.Enabled = false;
        this.groupBoxMinoConnect.Enabled = false;
        this.groupBoxDeviceEcho.Enabled = false;
        this.groupBoxWakeUp.Enabled = false;
        this.comboBoxTransceiverDevice.Enabled = false;
        this.textBoxMinoConnectState.Visible = true;
      }
      else
      {
        this.buttonOpenClose.Image = Images.pics.Open_16x16.Image;
        this.panelBasic.Enabled = true;
        this.panelTiming.Enabled = true;
        this.groupBoxMinoConnect.Enabled = this.comboBoxTransceiverDevice.SelectedIndex == 1;
        this.groupBoxDeviceEcho.Enabled = true;
        this.groupBoxWakeUp.Enabled = true;
        this.comboBoxTransceiverDevice.Enabled = true;
        this.textBoxMinoConnectState.Visible = false;
      }
    }

    private void SetPortListIndex(string PortString)
    {
      int num = -1;
      if (this.ComX.ConnectionTypeSelected != 0)
      {
        for (int index = 0; index < this.listBoxAvailableComPorts.Items.Count; ++index)
        {
          if (this.listBoxAvailableComPorts.Items[index].ToString() == PortString)
          {
            num = index;
            break;
          }
        }
      }
      else
      {
        for (int index = 0; index < this.listBoxAvailableComPorts.Items.Count; ++index)
        {
          string str = this.listBoxAvailableComPorts.Items[index].ToString();
          int length = str.IndexOf(' ');
          if (length > 0)
            str = str.Substring(0, length);
          if (str == PortString.Trim())
          {
            num = index;
            break;
          }
        }
      }
      if (num >= 0)
        this.textBoxActiveComPort.BackColor = Control.DefaultBackColor;
      else
        this.textBoxActiveComPort.BackColor = Color.Red;
      this.listBoxAvailableComPorts.SelectedIndex = num;
    }

    private void PresetDefaultTimeouts()
    {
      int Baudrate = ZR_AsyncCom.BaudrateTable[this.Baudrate.SelectedIndex];
      this.textBoxRecTime_GlobalOffset.Text = 0.ToString();
      this.textBoxRecTime_BeforFirstByte.Text = this.ComX.TimeBevorFirstByteDefaultFromBaudrate(Baudrate).ToString();
      this.textBoxRecTime_OffsetPerByte.Text = 0.0.ToString();
      this.textBoxRecTime_OffsetPerBlock.Text = 50.ToString();
      this.textBoxTransTime_BreakTime.Text = 700.ToString();
      this.textBoxTransTime_AfterBreak.Text = 50.ToString();
      this.textBoxTransTime_AfterOpen.Text = 200.ToString();
      this.textBoxRecTransTime.Text = 10.ToString();
      this.textBoxBreakIntervalTime.Text = 10000.ToString();
      this.textBoxWaitBeforeRepeatTime.Text = 200.ToString();
      this.buttonSet.Enabled = true;
    }

    private int GetBaudrateIndex(int BaudrateValue)
    {
      for (int baudrateIndex = 0; baudrateIndex < ZR_AsyncCom.BaudrateTable.Length; ++baudrateIndex)
      {
        if (ZR_AsyncCom.BaudrateTable[baudrateIndex] == BaudrateValue)
          return baudrateIndex;
      }
      return 5;
    }

    private int GetParityIndex(string ParityValue)
    {
      for (int parityIndex = 0; parityIndex < ZR_AsyncCom.ParityTable.Length; ++parityIndex)
      {
        if (ZR_AsyncCom.ParityTable[parityIndex] == ParityValue)
          return parityIndex;
      }
      return 1;
    }

    private void menuItemStartWindow_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "StartWindow";
      this.Hide();
    }

    private void menuStartGlobalMeterManager_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "GMM";
      this.Hide();
    }

    private void menuItemSerialBus_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "DeviceCollector";
      this.Hide();
    }

    private void menuItemDesigner_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Designer";
      this.Hide();
    }

    private void menuItemConfigurator_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Configurator";
      this.Hide();
    }

    private void menuItemSetBreak_Click(object sender, EventArgs e) => this.ComX.SetBreak();

    private void menuItemClearBreak_Click(object sender, EventArgs e) => this.ComX.ClearBreak();

    private void menuBack_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Hide();
    }

    private void menuQuit_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Exit";
      this.Hide();
    }

    internal void ShowEcho()
    {
      this.SelectEchoOn.Checked = this.ComX.EchoOn;
      this.checkBoxTestEcho.Checked = this.ComX.TestEcho;
      if (this.ComX.TestEcho)
        this.SelectEchoOn.Enabled = false;
      else
        this.SelectEchoOn.Enabled = true;
    }

    private void UpdateComPortList(bool ForceRefresh)
    {
      ZR_ClassLibMessages.ClearErrors();
      string strComPortIds = "";
      this.listBoxAvailableComPorts.Items.Clear();
      this.listBoxAvailableComPorts.Enabled = false;
      if (!this.ComX.GetComPortIds(out strComPortIds, ForceRefresh))
      {
        this.Cursor = Cursors.Default;
        if (ZR_ClassLibMessages.GetLastError() == 0)
          return;
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.GetLastErrorStringTranslated());
      }
      else
      {
        string[] strArray = strComPortIds.Split(',');
        SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>();
        foreach (string str in strArray)
        {
          if (str.StartsWith("COM"))
          {
            int result = -1;
            if (int.TryParse(str.Substring(3), out result) && !sortedDictionary.ContainsKey(result))
              sortedDictionary.Add(result, str);
          }
        }
        foreach (string str in sortedDictionary.Values)
        {
          if (str.Length > 3)
            this.listBoxAvailableComPorts.Items.Add((object) str);
        }
        if (this.listBoxAvailableComPorts.Items.Count > 0)
          this.listBoxAvailableComPorts.SelectedItem = (object) ("COM" + this.ComX.SingleParameter(CommParameter.Port, ""));
        this.listBoxAvailableComPorts.Enabled = true;
      }
    }

    private void ComUpdateComPortList()
    {
      this.listBoxAvailableComPorts.Items.Clear();
      this.listBoxAvailableComPorts.Enabled = false;
      List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
      foreach (ValueItem valueItem in availableComPorts)
      {
        string str1 = valueItem.ToString();
        int startIndex = str1.IndexOf('{');
        string str2 = startIndex < 0 ? "" : str1.Substring(startIndex);
        this.listBoxAvailableComPorts.Items.Add((object) (valueItem.Value.PadRight(7) + str2));
      }
      string choosedComPort = this.ComX.SingleParameter(CommParameter.Port, "").Trim();
      int index = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == choosedComPort));
      if (index >= 0)
        this.listBoxAvailableComPorts.SelectedIndex = index;
      this.listBoxAvailableComPorts.Enabled = true;
    }

    private void TheStatusChangedMessage(object sender, GMM_EventArgs TheMessage)
    {
      if (this.textBoxActiveComPort.InvokeRequired)
      {
        EventHandler<GMM_EventArgs> method = new EventHandler<GMM_EventArgs>(this.TheStatusChangedMessage);
        try
        {
          this.Invoke((Delegate) method, sender, (object) TheMessage);
        }
        catch (Exception ex)
        {
          ZR_AsyncCom.logger.Error(ex.Message);
        }
      }
      else
      {
        switch (TheMessage.TheMessageType)
        {
          case GMM_EventArgs.MessageType.Alive:
            this.textBoxMinoConnectRequests.Text = (long.Parse(this.textBoxMinoConnectRequests.Text) + 1L).ToString();
            break;
          case GMM_EventArgs.MessageType.KeyReceived:
            this.textBoxKeyCounter.Text = (int.Parse(this.textBoxKeyCounter.Text) + 1).ToString();
            break;
          case GMM_EventArgs.MessageType.StatusThreadStopped:
            this.textBoxOverloadCounter.Text = "0";
            this.textBoxMinoConnectRequests.Text = "0";
            this.textBoxKeyCounter.Text = "0";
            this.SetComOpenState();
            break;
          case GMM_EventArgs.MessageType.StatusChanged:
            this.SetComOpenState();
            this.ShowMinoConnectState();
            break;
          case GMM_EventArgs.MessageType.MinoConnectPlugMessage:
            this.SetComOpenState();
            break;
          case GMM_EventArgs.MessageType.Overload:
            this.textBoxOverloadCounter.Text = (int.Parse(this.textBoxOverloadCounter.Text) + 1).ToString();
            break;
          default:
            if (TheMessage.EventMessage.Length <= 0)
              break;
            this.textBoxTransceiverDeviceInfo.Text = TheMessage.EventMessage.Substring(1).Replace("\r\n", "").Replace("|", ZR_Constants.SystemNewLine);
            break;
        }
      }
    }

    private void ShowMinoConnectState()
    {
      if (!(this.ComX.MyComType is AsyncSerial) || !(((AsyncSerial) this.ComX.MyComType).MySerialPort is MinoConnectSerialPort))
        return;
      MinoConnectSerialPort serialPort = (MinoConnectSerialPort) ((AsyncSerial) this.ComX.MyComType).MySerialPort;
      if (serialPort.StateLastReceived == null)
        return;
      this.textBoxMinoConnectState.Text = serialPort.StateLastReceived.GetStateString(serialPort.StateRequired);
    }

    private void textBoxActiveComPort_Leave(object sender, EventArgs e)
    {
      this.SetPortListIndex(this.textBoxActiveComPort.Text);
    }

    private void comboBoxAvailableCOMservers_DrawItem(object sender, DrawItemEventArgs e)
    {
      Graphics graphics = e.Graphics;
      try
      {
        if (e.Index == -1 || this.comboBoxAvailableCOMservers.Items.Count <= 0 || this.comboBoxAvailableCOMservers.Items[e.Index] == null)
          return;
        string str = this.comboBoxAvailableCOMservers.Items[e.Index].ToString();
        SolidBrush solidBrush1 = new SolidBrush(!this.comboBoxAvailableCOMservers.Enabled ? SystemColors.Window : (!str.StartsWith("*") ? Color.Green : Color.Red));
        SolidBrush solidBrush2 = new SolidBrush(Color.White);
        graphics.FillRectangle((Brush) solidBrush1, e.Bounds);
        graphics.DrawString((string) this.comboBoxAvailableCOMservers.Items[e.Index], e.Font, (Brush) solidBrush2, (RectangleF) e.Bounds);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void buttonRefreshPortList_Click(object sender, EventArgs e)
    {
      this.ComX.BreakRequest = false;
      if (this.ComX.ConnectionTypeSelected == AsyncComConnectionType.COM)
        this.ComUpdateComPortList();
      else
        this.UpdateComPortList(true);
    }

    private void comboBoxComType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void checkBoxHardwareHandshake_CheckedChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void textBoxCOMserverIP_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.buttonSet.Enabled = true;
      if (e.KeyChar != '\r')
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.ComX.SingleParameter(CommParameter.COMserver, this.textBoxCOMserverIP.Text);
      this.SetComState();
      Cursor.Current = Cursors.Default;
    }

    private void comboBoxAvailableCOMservers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      if (!this.EventsEnabled || this.comboBoxAvailableCOMservers.Items.Count <= 0 || this.comboBoxAvailableCOMservers.SelectedIndex == -1)
        return;
      Cursor.Current = Cursors.WaitCursor;
      string ParameterValue = "";
      if (this.ComX.ConnectionTypeSelected == AsyncComConnectionType.Remote)
      {
        ParameterValue = this.comboBoxAvailableCOMservers.SelectedItem.ToString();
      }
      else
      {
        foreach (DictionaryEntry coMserver in this.ComX.MyMeterVPN.COMservers)
        {
          if (((COMserver) coMserver.Value).Name == this.comboBoxAvailableCOMservers.SelectedItem.ToString())
          {
            ParameterValue = coMserver.Key.ToString();
            break;
          }
        }
      }
      this.ComX.SingleParameter(CommParameter.COMserver, ParameterValue);
      this.listBoxAvailableComPorts.Items.Clear();
      if (this.ComX.ConnectionTypeSelected == AsyncComConnectionType.COM)
        this.ComUpdateComPortList();
      else
        this.UpdateComPortList(false);
      this.SetComState();
      Cursor.Current = Cursors.Default;
    }

    private void textBoxCOMserverIP_TextChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void buttonCOMserverAdmin_Click(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      int num = (int) new ManageCOMservers(this.ComX).ShowDialog();
    }

    private void listBoxAvailableComPorts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      if (this.listBoxAvailableComPorts.SelectedIndex < 0)
        return;
      this.textBoxActiveComPort.BackColor = Control.DefaultBackColor;
      string str = this.listBoxAvailableComPorts.Items[this.listBoxAvailableComPorts.SelectedIndex].ToString().Trim();
      int length = str.IndexOf(' ');
      if (length > 0)
        str = str.Substring(0, length);
      this.textBoxActiveComPort.Text = str;
    }

    private void Baudrate_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void listBoxParity_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void comboBoxTransceiverDevice_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      this.groupBoxMinoConnect.Enabled = this.comboBoxTransceiverDevice.SelectedIndex == 1;
    }

    private void checkBoxTestEcho_CheckedChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      if (this.checkBoxTestEcho.Checked)
        this.SelectEchoOn.Enabled = false;
      else
        this.SelectEchoOn.Enabled = true;
    }

    private void SelectEchoOn_CheckedChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void comboBoxDeviceWakeup_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void checkBoxIrDa_CheckedChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      if (this.checkBoxIrDa.Checked)
      {
        this.checkBoxDoveTailSide.Enabled = true;
      }
      else
      {
        this.checkBoxDoveTailSide.Enabled = false;
        this.checkBoxDoveTailSide.Checked = false;
      }
    }

    private void checkBoxDoveTailSide_CheckedChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
    }

    private void comboBoxSelectedPlugState_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonSet.Enabled = true;
      if (this.comboBoxSelectedMiConMode.SelectedIndex < 0 || this.comboBoxSelectedMiConMode.SelectedItem.ToString() == this.ComX.MinoConnectBaseState.ToString())
        this.comboBoxSelectedMiConMode.BackColor = this.DefaultEnabledBackColor;
      else
        this.comboBoxSelectedMiConMode.BackColor = Color.LightPink;
    }

    private void TimeChanged(object sender, EventArgs e) => this.buttonSet.Enabled = true;

    private void menuItemSearchUSB_Click(object sender, EventArgs e)
    {
    }

    private void btnUpgradeMinoConnectFirmware_Click(object sender, EventArgs e)
    {
      if (this.ComX.MyComType == null || !this.ComX.ComIsOpen || this.comboBoxTransceiverDevice.SelectedIndex != 1 || !this.ComX.CallTransceiverFunction(TransceiverDeviceFunction.DisableMinoConnectPolling) || !(this.ComX.MyComType.GetChannel() is SerialPort channel))
        return;
      FlashMinoConnectGmmForm minoConnectGmmForm = new FlashMinoConnectGmmForm(channel);
      minoConnectGmmForm.FirmwareInitialDirectory = SystemValues.SettingsPath;
      int num = (int) minoConnectGmmForm.ShowDialog();
      if (minoConnectGmmForm.IsMinoConnectSuccessfulUpdated)
      {
        this.btnUpgradeMinoConnectFirmware.Enabled = false;
        this.PrivatClose();
      }
      else
        this.ComX.CallTransceiverFunction(TransceiverDeviceFunction.EnableMinoConnectPolling);
    }

    private void checkBoxLoggerOn_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxLoggerOn.Checked)
      {
        this.LoggerIsOn = true;
        this.SetLoggerEvents();
        this.RefreshLogger();
      }
      else
      {
        this.LoggerIsOn = false;
        this.LoggerBox.Visible = false;
        this.SetLoggerEvents();
        this.RefreshLogger();
      }
    }

    private void buttonClearLogger_Click(object sender, EventArgs e)
    {
      int MaxEvents = 100;
      try
      {
        MaxEvents = int.Parse(this.textBoxLoggerEntries.Text);
      }
      catch
      {
        this.textBoxLoggerEntries.Text = "100";
      }
      this.ComX.Logger.NewLogger(MaxEvents);
      this.SetLoggerEvents();
      this.RefreshLogger();
    }

    private void buttonRefreshLogger_Click(object sender, EventArgs e) => this.RefreshLogger();

    private void SetLoggerEvents()
    {
      this.ComX.Logger.ActiveLoggerEvents = 0;
      if (!this.LoggerIsOn)
        return;
      if (this.checkBoxComOpenClose.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 256;
      if (this.checkBoxComData.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 512;
      if (this.checkBoxComState.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 1024;
      if (this.checkBoxComErrors.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 2048;
      if (this.checkBoxComPolling.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 4096;
      if (this.checkBoxBusFunctions.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 8192;
      if (this.checkBoxBusStatus.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 16384;
      if (this.checkBoxBusErrors.Checked)
        this.ComX.Logger.ActiveLoggerEvents |= 32768;
      int MaxEvents = 100;
      try
      {
        MaxEvents = int.Parse(this.textBoxLoggerEntries.Text);
      }
      catch
      {
        this.textBoxLoggerEntries.Text = "100";
      }
      if (MaxEvents == this.ComX.Logger.MaxLoggerEvents)
        return;
      this.ComX.Logger.NewLogger(MaxEvents);
    }

    private void RefreshLogger()
    {
      if (!this.LoggerIsOn)
      {
        this.LoggerBox.Text = string.Empty;
        this.LoggerBox.Visible = false;
      }
      else
      {
        this.ComX.Logger.StartReadout();
        StringBuilder stringBuilder = new StringBuilder(5000);
        bool nextLine;
        do
        {
          string EventLine;
          nextLine = this.ComX.Logger.GetNextLine(out EventLine);
          stringBuilder.Append(EventLine);
        }
        while (nextLine);
        this.LoggerBox.Text = stringBuilder.ToString();
        this.LoggerBox.Visible = true;
      }
    }

    private void menuItemFrameTestWindowActive_Click(object sender, EventArgs e)
    {
      if (this.menuItemFrameTestWindowActive.Checked)
      {
        this.ComX.MBusFrameTestWindowOn = false;
        this.menuItemFrameTestWindowActive.Checked = false;
      }
      else
      {
        this.ComX.MBusFrameTestWindowOn = true;
        this.menuItemFrameTestWindowActive.Checked = true;
      }
    }

    private void buttonDefaultTiming_Click(object sender, EventArgs e)
    {
      this.PresetDefaultTimeouts();
    }

    private void menuItemSendWakeup_Click(object sender, EventArgs e)
    {
      if (!(this.ComX.MyComType is AsyncSerial))
        return;
      this.Cursor = Cursors.WaitCursor;
      AsyncSerial comType = (AsyncSerial) this.ComX.MyComType;
      this.menuItemWakeupLoopOn.Checked = true;
      while (this.menuItemWakeupLoopOn.Checked)
      {
        Application.DoEvents();
        this.ComX.LastWakeupRefreshTime = DateTime.MinValue;
        comType.ManageWakeup();
      }
      this.Cursor = Cursors.Default;
    }

    private void menuItemWakeupLoopOn_Click(object sender, EventArgs e)
    {
      this.menuItemWakeupLoopOn.Checked = !this.menuItemWakeupLoopOn.Checked;
    }

    private void btnUpdateComServers_Click(object sender, EventArgs e)
    {
      this.ComX.RefreshComPorts();
      this.SetComState();
    }
  }
}
