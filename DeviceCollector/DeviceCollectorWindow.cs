// Decompiled with JetBrains decompiler
// Type: DeviceCollector.DeviceCollectorWindow
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using CorporateDesign;
using GmmDbLib;
using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class DeviceCollectorWindow : Form
  {
    private IContainer components = (IContainer) null;
    private MainMenu SerialBusMenu;
    private TabPage tabPageMemory;
    private TabPage tabPageDeviceParameter;
    private ListBox listBoxLocation;
    private Label label1;
    private TextBox textBoxStartAddress;
    private Label label2;
    private TextBox textBoxNumberOfBytes;
    private Label labelLocation;
    private Button buttonShowMemory;
    private Button buttonReadMemory;
    private TabPage tabPageBusInfo;
    private MenuItem menuFile;
    private MenuItem menuItemReadEEPromLoop;
    private TabControl tabControl;
    private TabPage tabPageBusSetup;
    private TextBox textBoxRepeadsOnError;
    private Label label8;
    private MenuItem menuHelp;
    private MenuItem menuComponent;
    private MenuItem menuStartAsyncCom;
    private MenuItem menuStartDesigner;
    private MenuItem menuStartGlobalMeterManager;
    private Button buttonOk;
    private MenuItem menuMeterMonitor;
    private GroupBox groupBox1;
    private TextBox textBoxAndMask;
    private Label label10;
    private Label label11;
    private TextBox textBoxOrMask;
    private Button buttonWriteBitfield;
    private GroupBox groupBox2;
    private Label label9;
    private TextBox textBoxValue;
    private Button buttonDeleteMeterKey;
    private Button buttonSetBaudrate;
    private DataGrid dataGridParameterList;
    private MenuItem menuRead;
    private MenuItem menuRunBackup;
    private MenuItem menuDeleteBusInfo;
    private MenuItem menuSearchSingleDeviceByAddress;
    private MenuItem menuSearchSingleDeviceBySerialNumber;
    private MenuItem menuWriteBusinfo;
    private MenuItem menuReadBusinfo;
    private MenuItem menuReset;
    private MenuItem menuReadDeviceParameter;
    private MenuItem menuGetVersion;
    private MenuItem menuSetEmergencyMode;
    private MenuItem menuSchnittstellenFehlerLoop;
    private MenuItem menuSerchBaudrate;
    private MenuItem menuEEPromWriteReadLoop;
    private MenuItem menuItem1;
    private MenuItem menuSelectDeviceByPrimaryAddress;
    private MenuItem menuScanByAddress;
    private MenuItem menuItem9;
    private MenuItem menuItem10;
    private Button buttonBreak;
    private Label labelStatus;
    private ContextMenu contextMenu1;
    private MenuItem menuCtSetPrimaryAddress;
    private MenuItem menuCtReadMeterParameter;
    private MenuItem menuItem15;
    private MenuItem menuItemBack;
    private MenuItem menuItemQuit;
    private MenuItem menuTest;
    private MenuItem menuFunction;
    private ToolTip toolTip1;
    private Button buttonToolbarReadDevice;
    private CheckBox checkBoxLoggToFile;
    private Label label4;
    private ComboBox comboBoxLoggToCom;
    private CheckBox checkBoxLoggAllTemp;
    private MenuItem menuItemIO_Test;
    private MenuItem menuScanBySerialNumber;
    private MenuItem menuAddDeviceByParameter;
    private MenuItem menuDeleteDeviceFromBus;
    private Label label15;
    private Label label16;
    private Label label17;
    private TextBox textBoxScanStartNumber;
    private TextBox textBoxOrganizeStartAddress;
    private TextBox textBoxScanStartAddress;
    public MenuItem menuCtDeleteFromBusinfo;
    private MenuItem menuLineSerieX;
    private TabPage tabPageAllParameter;
    private Button buttonShowAll;
    private Button buttonReadAll;
    private MenuItem menuShowAllParameters;
    private MenuItem menuItem12;
    private MenuItem menuReadAll;
    private ComboBox comboBoxBusMode;
    private Label label18;
    private MenuItem menuStartReceiver;
    private MenuItem menuStopReceiver;
    private MenuItem menuBus;
    private MenuItem menuRadio;
    private Button buttonClearAll;
    private MenuItem menuItem7;
    private MenuItem menuDeleteData;
    private MenuItem menuEEPromReset;
    private CheckBox checkBoxKeepDestinationAddress;
    private GroupBox groupBoxLogger;
    private CheckBox checkBoxLoggToZDF_File;
    private GroupBox groupBox3;
    private TextBox textBoxZDF_FileName;
    private MenuItem menuItemShowMeterData;
    private MenuItem menuItem3;
    private CheckBox checkBoxWatchRange;
    private TextBox textBoxAllParameters;
    private Label labelReadoutSystemText;
    private Label labelReadoutSystemVersionText;
    private Label labelReadoutSystem;
    private Label labelReadoutSystemVersion;
    private ProgressBar progressBar;
    private GroupBox groupBoxDeviceInfo;
    private TextBox textBoxManufacturer;
    private Label label5;
    private TextBox textBoxMedium;
    private Label label6;
    private Label label7;
    private TextBox textBoxSerialNr;
    private Label label19;
    private TextBox textBoxReceiveLevel;
    private MenuItem menuTransmitRadioFrame;
    private Button buttonSelectZDF_File;
    private GroupBox groupBoxRadioReadoutSystem;
    private GroupBox groupBoxRadioReceiverSetup;
    private GroupBox groupBoxBaseBusSettings;
    private MenuItem menuRequestLoop;
    private MenuItem menuItem14;
    private Button buttonToolbarReadAllDevices;
    private Button buttonToolbarDeleteBusinfo;
    private Button buttonToolbarScanBusByAddress;
    private Button buttonToolbarScanBusBySerialNumber;
    private Button buttonToolbarSearchSingelDeviceByAddress;
    private Button buttonToolbarSearchSingelDeviceBySerialNumber;
    private MenuItem menuItemCmSetParameterListLine;
    private MenuItem menuCmSetToDefaultParameterList;
    private MenuItem menuCmSetToFullParameterList;
    private MenuItem menuItemShiftToNextAddress;
    private MenuItem menuItemSerialBusHelp;
    private MenuItem menuItemShowWaveFlowParameter;
    private MenuItem menuItemWafeFlowParameterTest;
    private MenuItem menuItem17;
    private MenuItem menuItemLoadZDF_File;
    private MenuItem menuItemExportDataTable;
    private MenuItem menuConnectAcrossBaudrates;
    private MenuItem menuGetVersionTestCycle;
    private CheckBox checkBoxFastSecoundaryAddressing;
    private CheckBox checkBoxOnlySecondaryAddressing;
    private DataGridView dataGridBusTable;
    private TextBox textBoxBusFilePath;
    private TextBox textBoxLoopTime;
    private Label label14;
    private MenuItem menuDevice;
    private MenuItem menuSetSelectedDeviceTo300_Baud;
    private MenuItem menuSetSelectedDeviceTo2400_Baud;
    private MenuItem menuSetSelectedDeviceTo9600_Baud;
    private MenuItem menuItemSetSelectedDeviceToDefaultParameterList;
    private MenuItem menuItemSetSelectedDeviceToFullParameterList;
    private MenuItem menuItemSetAllDevicesToDefaultParameterList;
    private MenuItem menuItemSetAllDevicesToFullParameterList;
    private MenuItem menuSetPrimaryAddress;
    private MenuItem menuOrganize;
    private MenuItem menuSetSelectedDeviceTo38400_Baud;
    private MenuItem menuItemSelectParameterListLine;
    private MenuItem menuItem23;
    private MenuItem menuLineBaud;
    private CheckBox checkBoxChangeInterfaceBaudrateToo;
    private MenuItem menuItemStartWindow;
    private MenuItem menuItemExportExcelDataTable;
    private GroupBox gboxMinomat;
    private Label label13;
    private TextBox textBoxMinomatSerial;
    private DateTimePicker dateTimePickerToTime;
    private DateTimePicker dateTimePickerFromTime;
    private Label label12;
    private Label label20;
    private MenuItem menuItem4;
    private MenuItem menuItemShowInfo;
    private CheckBox checkBoxUseExternalKeySignal;
    private CheckBox checkBoxBeepByReading;
    private MenuItem menuItemRequestLoopAll;
    private DataGridView dataGridViewAllParameter;
    private Button buttonEraseFlash;
    private MenuItem menuCtMBusConverterLine;
    private MenuItem menuCtShowHideColumnsDeviceList;
    private ContextMenu contextMenuAllParameters;
    private MenuItem menuCtShowHideColumnsAllParameters;
    private MenuItem menuItem6;
    private MenuItem menuItem11;
    private MenuItem menuItemSetupProfiles;
    private MenuItem menuItemConfigurator;
    private MenuItem menuItemSelectParameterList;
    private Button btnSetDefaultSettings;
    private MenuItem menuItemMemoryAccess;
    private MenuItem menuItemAblaufTest;
    private TextBox txtFilterBySerialNumber;
    private Label label3;
    private MenuItem menuItem5;
    private Button btnExportToCSV;
    private SplitContainer splitContainerAllParameters;
    private MenuItem menuItem13;
    private MenuItem menuCtMBusConverter;
    private MenuItem menuItemMBusConverterLine;
    private MenuItem menuItemMBusConverter;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private MenuItem menuCtSelectParameterList;
    private MenuItem menuMbusParser;
    private CheckBox checkBoxIsMultitelegrammEnabled;
    private CheckBox checkBoxUseReqUd2_5B;
    private CheckBox checkBoxApplicationReset;
    private CheckBox checkBoxSND_NKE;
    private MenuItem menuItemMinoConnectTest;
    private CheckBox checkBoxAutoSaveSetup;
    private Button buttonAsyncCom;
    private static Logger logger = LogManager.GetLogger(nameof (DeviceCollectorWindow));
    private const string ZDF_FileNameConfig = "ZDF_FileName";
    private const string LoggZDF_FileConfig = "LoggZDF_File";
    private int InfoCounter = 0;
    private int ErrorCounter = 0;
    private DeviceCollectorFunctions MyBus;
    private bool LoopIsRunning = false;
    private MemoryDump MyDump;
    private bool comboBoxBusModeEventsEnabled = true;
    private bool IsRunning = false;
    private bool BusinfoIsManualChanged;
    internal string StartComponentName;
    private DeviceCollector.MBusConverterDiagnostic MBusConverter;
    private EventHandler<GMM_EventArgs> EventObject;
    private bool initActive;
    internal string BaseMessage = string.Empty;
    private int aliveCounter;
    private bool ReadActive = false;
    private bool ModeChangeBreak = false;
    private MemoryAccess MyMemory;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DeviceCollectorWindow));
      this.splitContainerAllParameters = new SplitContainer();
      this.textBoxAllParameters = new TextBox();
      this.dataGridViewAllParameter = new DataGridView();
      this.btnExportToCSV = new Button();
      this.txtFilterBySerialNumber = new TextBox();
      this.buttonClearAll = new Button();
      this.label3 = new Label();
      this.buttonReadAll = new Button();
      this.buttonShowAll = new Button();
      this.SerialBusMenu = new MainMenu(this.components);
      this.menuFile = new MenuItem();
      this.menuReadBusinfo = new MenuItem();
      this.menuWriteBusinfo = new MenuItem();
      this.menuItem17 = new MenuItem();
      this.menuItemLoadZDF_File = new MenuItem();
      this.menuItemExportDataTable = new MenuItem();
      this.menuItemExportExcelDataTable = new MenuItem();
      this.menuItem11 = new MenuItem();
      this.menuItemSetupProfiles = new MenuItem();
      this.menuBus = new MenuItem();
      this.menuDeleteBusInfo = new MenuItem();
      this.menuDeleteDeviceFromBus = new MenuItem();
      this.menuItem10 = new MenuItem();
      this.menuScanByAddress = new MenuItem();
      this.menuScanBySerialNumber = new MenuItem();
      this.menuItem9 = new MenuItem();
      this.menuSearchSingleDeviceByAddress = new MenuItem();
      this.menuSearchSingleDeviceBySerialNumber = new MenuItem();
      this.menuAddDeviceByParameter = new MenuItem();
      this.menuItem1 = new MenuItem();
      this.menuSelectDeviceByPrimaryAddress = new MenuItem();
      this.menuItem4 = new MenuItem();
      this.menuItemShowInfo = new MenuItem();
      this.menuDevice = new MenuItem();
      this.menuSetPrimaryAddress = new MenuItem();
      this.menuOrganize = new MenuItem();
      this.menuItemSelectParameterListLine = new MenuItem();
      this.menuItemSetSelectedDeviceToDefaultParameterList = new MenuItem();
      this.menuItemSetSelectedDeviceToFullParameterList = new MenuItem();
      this.menuItemSelectParameterList = new MenuItem();
      this.menuItem23 = new MenuItem();
      this.menuItemSetAllDevicesToDefaultParameterList = new MenuItem();
      this.menuItemSetAllDevicesToFullParameterList = new MenuItem();
      this.menuLineBaud = new MenuItem();
      this.menuSetSelectedDeviceTo300_Baud = new MenuItem();
      this.menuSetSelectedDeviceTo2400_Baud = new MenuItem();
      this.menuSetSelectedDeviceTo9600_Baud = new MenuItem();
      this.menuSetSelectedDeviceTo38400_Baud = new MenuItem();
      this.menuItemMBusConverterLine = new MenuItem();
      this.menuItemMBusConverter = new MenuItem();
      this.menuRadio = new MenuItem();
      this.menuStartReceiver = new MenuItem();
      this.menuStopReceiver = new MenuItem();
      this.menuItem7 = new MenuItem();
      this.menuDeleteData = new MenuItem();
      this.menuRead = new MenuItem();
      this.menuReadDeviceParameter = new MenuItem();
      this.menuReadAll = new MenuItem();
      this.menuItem14 = new MenuItem();
      this.menuRequestLoop = new MenuItem();
      this.menuItemRequestLoopAll = new MenuItem();
      this.menuSerchBaudrate = new MenuItem();
      this.menuItem12 = new MenuItem();
      this.menuShowAllParameters = new MenuItem();
      this.menuLineSerieX = new MenuItem();
      this.menuGetVersion = new MenuItem();
      this.menuGetVersionTestCycle = new MenuItem();
      this.menuConnectAcrossBaudrates = new MenuItem();
      this.menuFunction = new MenuItem();
      this.menuReset = new MenuItem();
      this.menuRunBackup = new MenuItem();
      this.menuSetEmergencyMode = new MenuItem();
      this.menuTransmitRadioFrame = new MenuItem();
      this.menuItemShiftToNextAddress = new MenuItem();
      this.menuTest = new MenuItem();
      this.menuItemMemoryAccess = new MenuItem();
      this.menuItemReadEEPromLoop = new MenuItem();
      this.menuMeterMonitor = new MenuItem();
      this.menuSchnittstellenFehlerLoop = new MenuItem();
      this.menuEEPromWriteReadLoop = new MenuItem();
      this.menuEEPromReset = new MenuItem();
      this.menuItemIO_Test = new MenuItem();
      this.menuItemShowWaveFlowParameter = new MenuItem();
      this.menuItemWafeFlowParameterTest = new MenuItem();
      this.menuItemAblaufTest = new MenuItem();
      this.menuItem5 = new MenuItem();
      this.menuMbusParser = new MenuItem();
      this.menuItemMinoConnectTest = new MenuItem();
      this.menuComponent = new MenuItem();
      this.menuItemStartWindow = new MenuItem();
      this.menuStartGlobalMeterManager = new MenuItem();
      this.menuItemBack = new MenuItem();
      this.menuItemQuit = new MenuItem();
      this.menuItem15 = new MenuItem();
      this.menuStartDesigner = new MenuItem();
      this.menuItemConfigurator = new MenuItem();
      this.menuStartAsyncCom = new MenuItem();
      this.menuHelp = new MenuItem();
      this.menuItemSerialBusHelp = new MenuItem();
      this.menuItem6 = new MenuItem();
      this.tabControl = new TabControl();
      this.tabPageBusInfo = new TabPage();
      this.contextMenu1 = new ContextMenu();
      this.menuCtReadMeterParameter = new MenuItem();
      this.menuItemShowMeterData = new MenuItem();
      this.menuItem3 = new MenuItem();
      this.menuCtSetPrimaryAddress = new MenuItem();
      this.menuCtDeleteFromBusinfo = new MenuItem();
      this.menuItemCmSetParameterListLine = new MenuItem();
      this.menuCmSetToDefaultParameterList = new MenuItem();
      this.menuCmSetToFullParameterList = new MenuItem();
      this.menuCtSelectParameterList = new MenuItem();
      this.menuCtMBusConverterLine = new MenuItem();
      this.menuCtMBusConverter = new MenuItem();
      this.menuItem13 = new MenuItem();
      this.menuCtShowHideColumnsDeviceList = new MenuItem();
      this.textBoxBusFilePath = new TextBox();
      this.dataGridBusTable = new DataGridView();
      this.tabPageAllParameter = new TabPage();
      this.contextMenuAllParameters = new ContextMenu();
      this.menuCtShowHideColumnsAllParameters = new MenuItem();
      this.tabPageDeviceParameter = new TabPage();
      this.groupBoxDeviceInfo = new GroupBox();
      this.textBoxManufacturer = new TextBox();
      this.label5 = new Label();
      this.textBoxMedium = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.textBoxSerialNr = new TextBox();
      this.groupBoxRadioReadoutSystem = new GroupBox();
      this.labelReadoutSystemText = new Label();
      this.labelReadoutSystemVersionText = new Label();
      this.labelReadoutSystem = new Label();
      this.labelReadoutSystemVersion = new Label();
      this.dataGridParameterList = new DataGrid();
      this.tabPageBusSetup = new TabPage();
      this.btnSetDefaultSettings = new Button();
      this.gboxMinomat = new GroupBox();
      this.dateTimePickerToTime = new DateTimePicker();
      this.dateTimePickerFromTime = new DateTimePicker();
      this.label12 = new Label();
      this.label20 = new Label();
      this.textBoxMinomatSerial = new TextBox();
      this.label13 = new Label();
      this.groupBoxRadioReceiverSetup = new GroupBox();
      this.textBoxReceiveLevel = new TextBox();
      this.label19 = new Label();
      this.groupBox3 = new GroupBox();
      this.buttonSelectZDF_File = new Button();
      this.textBoxZDF_FileName = new TextBox();
      this.checkBoxLoggToZDF_File = new CheckBox();
      this.groupBoxBaseBusSettings = new GroupBox();
      this.checkBoxSND_NKE = new CheckBox();
      this.checkBoxUseReqUd2_5B = new CheckBox();
      this.checkBoxApplicationReset = new CheckBox();
      this.checkBoxIsMultitelegrammEnabled = new CheckBox();
      this.textBoxLoopTime = new TextBox();
      this.checkBoxFastSecoundaryAddressing = new CheckBox();
      this.label14 = new Label();
      this.checkBoxOnlySecondaryAddressing = new CheckBox();
      this.checkBoxBeepByReading = new CheckBox();
      this.checkBoxUseExternalKeySignal = new CheckBox();
      this.checkBoxChangeInterfaceBaudrateToo = new CheckBox();
      this.checkBoxKeepDestinationAddress = new CheckBox();
      this.label15 = new Label();
      this.comboBoxBusMode = new ComboBox();
      this.label16 = new Label();
      this.label8 = new Label();
      this.label17 = new Label();
      this.textBoxRepeadsOnError = new TextBox();
      this.textBoxScanStartAddress = new TextBox();
      this.label18 = new Label();
      this.textBoxScanStartNumber = new TextBox();
      this.textBoxOrganizeStartAddress = new TextBox();
      this.groupBoxLogger = new GroupBox();
      this.comboBoxLoggToCom = new ComboBox();
      this.label4 = new Label();
      this.checkBoxLoggToFile = new CheckBox();
      this.checkBoxLoggAllTemp = new CheckBox();
      this.checkBoxAutoSaveSetup = new CheckBox();
      this.tabPageMemory = new TabPage();
      this.checkBoxWatchRange = new CheckBox();
      this.groupBox2 = new GroupBox();
      this.buttonDeleteMeterKey = new Button();
      this.label9 = new Label();
      this.textBoxValue = new TextBox();
      this.buttonSetBaudrate = new Button();
      this.groupBox1 = new GroupBox();
      this.textBoxAndMask = new TextBox();
      this.label10 = new Label();
      this.label11 = new Label();
      this.textBoxOrMask = new TextBox();
      this.buttonWriteBitfield = new Button();
      this.listBoxLocation = new ListBox();
      this.label1 = new Label();
      this.textBoxStartAddress = new TextBox();
      this.label2 = new Label();
      this.textBoxNumberOfBytes = new TextBox();
      this.labelLocation = new Label();
      this.buttonEraseFlash = new Button();
      this.buttonShowMemory = new Button();
      this.buttonReadMemory = new Button();
      this.progressBar = new ProgressBar();
      this.buttonOk = new Button();
      this.buttonBreak = new Button();
      this.labelStatus = new Label();
      this.buttonToolbarReadAllDevices = new Button();
      this.buttonToolbarReadDevice = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.buttonToolbarDeleteBusinfo = new Button();
      this.buttonToolbarScanBusByAddress = new Button();
      this.buttonToolbarScanBusBySerialNumber = new Button();
      this.buttonToolbarSearchSingelDeviceByAddress = new Button();
      this.buttonToolbarSearchSingelDeviceBySerialNumber = new Button();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.buttonAsyncCom = new Button();
      this.splitContainerAllParameters.BeginInit();
      this.splitContainerAllParameters.Panel1.SuspendLayout();
      this.splitContainerAllParameters.Panel2.SuspendLayout();
      this.splitContainerAllParameters.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewAllParameter).BeginInit();
      this.tabControl.SuspendLayout();
      this.tabPageBusInfo.SuspendLayout();
      ((ISupportInitialize) this.dataGridBusTable).BeginInit();
      this.tabPageAllParameter.SuspendLayout();
      this.tabPageDeviceParameter.SuspendLayout();
      this.groupBoxDeviceInfo.SuspendLayout();
      this.groupBoxRadioReadoutSystem.SuspendLayout();
      this.dataGridParameterList.BeginInit();
      this.tabPageBusSetup.SuspendLayout();
      this.gboxMinomat.SuspendLayout();
      this.groupBoxRadioReceiverSetup.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBoxBaseBusSettings.SuspendLayout();
      this.groupBoxLogger.SuspendLayout();
      this.tabPageMemory.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.splitContainerAllParameters, "splitContainerAllParameters");
      this.splitContainerAllParameters.FixedPanel = FixedPanel.Panel1;
      this.splitContainerAllParameters.Name = "splitContainerAllParameters";
      this.splitContainerAllParameters.Panel1.Controls.Add((Control) this.textBoxAllParameters);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.dataGridViewAllParameter);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.btnExportToCSV);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.txtFilterBySerialNumber);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.buttonClearAll);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.label3);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.buttonReadAll);
      this.splitContainerAllParameters.Panel2.Controls.Add((Control) this.buttonShowAll);
      this.textBoxAllParameters.BorderStyle = BorderStyle.FixedSingle;
      componentResourceManager.ApplyResources((object) this.textBoxAllParameters, "textBoxAllParameters");
      this.textBoxAllParameters.Name = "textBoxAllParameters";
      this.textBoxAllParameters.ReadOnly = true;
      this.dataGridViewAllParameter.AllowUserToAddRows = false;
      this.dataGridViewAllParameter.AllowUserToDeleteRows = false;
      this.dataGridViewAllParameter.AllowUserToOrderColumns = true;
      componentResourceManager.ApplyResources((object) this.dataGridViewAllParameter, "dataGridViewAllParameter");
      this.dataGridViewAllParameter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewAllParameter.Name = "dataGridViewAllParameter";
      this.dataGridViewAllParameter.ReadOnly = true;
      this.dataGridViewAllParameter.RowTemplate.Height = 24;
      this.dataGridViewAllParameter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      componentResourceManager.ApplyResources((object) this.btnExportToCSV, "btnExportToCSV");
      this.btnExportToCSV.Name = "btnExportToCSV";
      this.btnExportToCSV.UseVisualStyleBackColor = true;
      this.btnExportToCSV.Click += new System.EventHandler(this.btnExportToCSV_Click);
      componentResourceManager.ApplyResources((object) this.txtFilterBySerialNumber, "txtFilterBySerialNumber");
      this.txtFilterBySerialNumber.Name = "txtFilterBySerialNumber";
      componentResourceManager.ApplyResources((object) this.buttonClearAll, "buttonClearAll");
      this.buttonClearAll.Name = "buttonClearAll";
      this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.buttonReadAll, "buttonReadAll");
      this.buttonReadAll.Name = "buttonReadAll";
      this.buttonReadAll.Click += new System.EventHandler(this.buttonReadAll_Click);
      componentResourceManager.ApplyResources((object) this.buttonShowAll, "buttonShowAll");
      this.buttonShowAll.Name = "buttonShowAll";
      this.buttonShowAll.Click += new System.EventHandler(this.buttonShowAll_Click);
      this.SerialBusMenu.MenuItems.AddRange(new MenuItem[9]
      {
        this.menuFile,
        this.menuBus,
        this.menuDevice,
        this.menuRadio,
        this.menuRead,
        this.menuFunction,
        this.menuTest,
        this.menuComponent,
        this.menuHelp
      });
      this.menuFile.Index = 0;
      this.menuFile.MenuItems.AddRange(new MenuItem[8]
      {
        this.menuReadBusinfo,
        this.menuWriteBusinfo,
        this.menuItem17,
        this.menuItemLoadZDF_File,
        this.menuItemExportDataTable,
        this.menuItemExportExcelDataTable,
        this.menuItem11,
        this.menuItemSetupProfiles
      });
      componentResourceManager.ApplyResources((object) this.menuFile, "menuFile");
      this.menuReadBusinfo.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuReadBusinfo, "menuReadBusinfo");
      this.menuReadBusinfo.Click += new System.EventHandler(this.menuReadBusinfo_Click);
      this.menuWriteBusinfo.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuWriteBusinfo, "menuWriteBusinfo");
      this.menuWriteBusinfo.Click += new System.EventHandler(this.menuWriteBusinfo_Click);
      this.menuItem17.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItem17, "menuItem17");
      this.menuItemLoadZDF_File.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuItemLoadZDF_File, "menuItemLoadZDF_File");
      this.menuItemLoadZDF_File.Click += new System.EventHandler(this.menuItemLoadZDF_File_Click);
      this.menuItemExportDataTable.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItemExportDataTable, "menuItemExportDataTable");
      this.menuItemExportDataTable.Click += new System.EventHandler(this.menuItemExportDataTable_Click);
      this.menuItemExportExcelDataTable.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuItemExportExcelDataTable, "menuItemExportExcelDataTable");
      this.menuItemExportExcelDataTable.Click += new System.EventHandler(this.menuItemExportExcelDataTable_Click);
      this.menuItem11.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItem11, "menuItem11");
      this.menuItemSetupProfiles.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuItemSetupProfiles, "menuItemSetupProfiles");
      this.menuItemSetupProfiles.Click += new System.EventHandler(this.menuItemSetupProfiles_Click);
      this.menuBus.Index = 1;
      this.menuBus.MenuItems.AddRange(new MenuItem[13]
      {
        this.menuDeleteBusInfo,
        this.menuDeleteDeviceFromBus,
        this.menuItem10,
        this.menuScanByAddress,
        this.menuScanBySerialNumber,
        this.menuItem9,
        this.menuSearchSingleDeviceByAddress,
        this.menuSearchSingleDeviceBySerialNumber,
        this.menuAddDeviceByParameter,
        this.menuItem1,
        this.menuSelectDeviceByPrimaryAddress,
        this.menuItem4,
        this.menuItemShowInfo
      });
      componentResourceManager.ApplyResources((object) this.menuBus, "menuBus");
      this.menuDeleteBusInfo.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuDeleteBusInfo, "menuDeleteBusInfo");
      this.menuDeleteBusInfo.Click += new System.EventHandler(this.menuDeleteBusInfo_Click);
      this.menuDeleteDeviceFromBus.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuDeleteDeviceFromBus, "menuDeleteDeviceFromBus");
      this.menuDeleteDeviceFromBus.Click += new System.EventHandler(this.menuDeleteDeviceFromBus_Click);
      this.menuItem10.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItem10, "menuItem10");
      this.menuScanByAddress.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuScanByAddress, "menuScanByAddress");
      this.menuScanByAddress.Click += new System.EventHandler(this.menuScanByAddress_Click);
      this.menuScanBySerialNumber.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuScanBySerialNumber, "menuScanBySerialNumber");
      this.menuScanBySerialNumber.Click += new System.EventHandler(this.menuScanBySerialNumber_Click);
      this.menuItem9.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuItem9, "menuItem9");
      this.menuSearchSingleDeviceByAddress.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuSearchSingleDeviceByAddress, "menuSearchSingleDeviceByAddress");
      this.menuSearchSingleDeviceByAddress.Click += new System.EventHandler(this.menuSearchSingleDeviceByAddress_Click);
      this.menuSearchSingleDeviceBySerialNumber.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuSearchSingleDeviceBySerialNumber, "menuSearchSingleDeviceBySerialNumber");
      this.menuSearchSingleDeviceBySerialNumber.Click += new System.EventHandler(this.menuSearchSingleDeviceBySerialNumber_Click);
      this.menuAddDeviceByParameter.Index = 8;
      componentResourceManager.ApplyResources((object) this.menuAddDeviceByParameter, "menuAddDeviceByParameter");
      this.menuAddDeviceByParameter.Click += new System.EventHandler(this.menuAddDeviceByParameter_Click);
      this.menuItem1.Index = 9;
      componentResourceManager.ApplyResources((object) this.menuItem1, "menuItem1");
      this.menuSelectDeviceByPrimaryAddress.Index = 10;
      componentResourceManager.ApplyResources((object) this.menuSelectDeviceByPrimaryAddress, "menuSelectDeviceByPrimaryAddress");
      this.menuSelectDeviceByPrimaryAddress.Click += new System.EventHandler(this.menuSelectDeviceByPrimaryAddress_Click);
      this.menuItem4.Index = 11;
      componentResourceManager.ApplyResources((object) this.menuItem4, "menuItem4");
      this.menuItemShowInfo.Index = 12;
      componentResourceManager.ApplyResources((object) this.menuItemShowInfo, "menuItemShowInfo");
      this.menuItemShowInfo.Click += new System.EventHandler(this.menuItemShowInfo_Click);
      this.menuDevice.Index = 2;
      this.menuDevice.MenuItems.AddRange(new MenuItem[16]
      {
        this.menuSetPrimaryAddress,
        this.menuOrganize,
        this.menuItemSelectParameterListLine,
        this.menuItemSetSelectedDeviceToDefaultParameterList,
        this.menuItemSetSelectedDeviceToFullParameterList,
        this.menuItemSelectParameterList,
        this.menuItem23,
        this.menuItemSetAllDevicesToDefaultParameterList,
        this.menuItemSetAllDevicesToFullParameterList,
        this.menuLineBaud,
        this.menuSetSelectedDeviceTo300_Baud,
        this.menuSetSelectedDeviceTo2400_Baud,
        this.menuSetSelectedDeviceTo9600_Baud,
        this.menuSetSelectedDeviceTo38400_Baud,
        this.menuItemMBusConverterLine,
        this.menuItemMBusConverter
      });
      componentResourceManager.ApplyResources((object) this.menuDevice, "menuDevice");
      this.menuSetPrimaryAddress.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuSetPrimaryAddress, "menuSetPrimaryAddress");
      this.menuSetPrimaryAddress.Click += new System.EventHandler(this.menuSetPrimaryAddress_Click);
      this.menuOrganize.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuOrganize, "menuOrganize");
      this.menuOrganize.Click += new System.EventHandler(this.menuOrganize_Click);
      this.menuItemSelectParameterListLine.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItemSelectParameterListLine, "menuItemSelectParameterListLine");
      this.menuItemSetSelectedDeviceToDefaultParameterList.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuItemSetSelectedDeviceToDefaultParameterList, "menuItemSetSelectedDeviceToDefaultParameterList");
      this.menuItemSetSelectedDeviceToDefaultParameterList.Click += new System.EventHandler(this.menuItemSetSelectedDeviceToDefaultParameterList_Click);
      this.menuItemSetSelectedDeviceToFullParameterList.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItemSetSelectedDeviceToFullParameterList, "menuItemSetSelectedDeviceToFullParameterList");
      this.menuItemSetSelectedDeviceToFullParameterList.Click += new System.EventHandler(this.menuItemSetSelectedDeviceToFullParameterList_Click);
      this.menuItemSelectParameterList.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuItemSelectParameterList, "menuItemSelectParameterList");
      this.menuItemSelectParameterList.Click += new System.EventHandler(this.menuItemSelectParameterList_Click);
      this.menuItem23.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItem23, "menuItem23");
      this.menuItemSetAllDevicesToDefaultParameterList.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuItemSetAllDevicesToDefaultParameterList, "menuItemSetAllDevicesToDefaultParameterList");
      this.menuItemSetAllDevicesToDefaultParameterList.Click += new System.EventHandler(this.menuItemSetAllDevicesToDefaultParameterList_Click);
      this.menuItemSetAllDevicesToFullParameterList.Index = 8;
      componentResourceManager.ApplyResources((object) this.menuItemSetAllDevicesToFullParameterList, "menuItemSetAllDevicesToFullParameterList");
      this.menuItemSetAllDevicesToFullParameterList.Click += new System.EventHandler(this.menuItemSetAllDevicesToFullParameterList_Click);
      this.menuLineBaud.Index = 9;
      componentResourceManager.ApplyResources((object) this.menuLineBaud, "menuLineBaud");
      this.menuSetSelectedDeviceTo300_Baud.Index = 10;
      componentResourceManager.ApplyResources((object) this.menuSetSelectedDeviceTo300_Baud, "menuSetSelectedDeviceTo300_Baud");
      this.menuSetSelectedDeviceTo300_Baud.Click += new System.EventHandler(this.menuSetSelectedDeviceTo300_Baud_Click);
      this.menuSetSelectedDeviceTo2400_Baud.Index = 11;
      componentResourceManager.ApplyResources((object) this.menuSetSelectedDeviceTo2400_Baud, "menuSetSelectedDeviceTo2400_Baud");
      this.menuSetSelectedDeviceTo2400_Baud.Click += new System.EventHandler(this.menuSetSelectedDeviceTo2400_Baud_Click);
      this.menuSetSelectedDeviceTo9600_Baud.Index = 12;
      componentResourceManager.ApplyResources((object) this.menuSetSelectedDeviceTo9600_Baud, "menuSetSelectedDeviceTo9600_Baud");
      this.menuSetSelectedDeviceTo9600_Baud.Click += new System.EventHandler(this.menuSetSelectedDeviceTo9600_Baud_Click);
      this.menuSetSelectedDeviceTo38400_Baud.Index = 13;
      componentResourceManager.ApplyResources((object) this.menuSetSelectedDeviceTo38400_Baud, "menuSetSelectedDeviceTo38400_Baud");
      this.menuSetSelectedDeviceTo38400_Baud.Click += new System.EventHandler(this.menuSetSelectedDeviceTo38400_Baud_Click);
      this.menuItemMBusConverterLine.Index = 14;
      componentResourceManager.ApplyResources((object) this.menuItemMBusConverterLine, "menuItemMBusConverterLine");
      this.menuItemMBusConverter.Index = 15;
      componentResourceManager.ApplyResources((object) this.menuItemMBusConverter, "menuItemMBusConverter");
      this.menuItemMBusConverter.Click += new System.EventHandler(this.menuItemMBusConverter_Click);
      this.menuRadio.Index = 3;
      this.menuRadio.MenuItems.AddRange(new MenuItem[4]
      {
        this.menuStartReceiver,
        this.menuStopReceiver,
        this.menuItem7,
        this.menuDeleteData
      });
      componentResourceManager.ApplyResources((object) this.menuRadio, "menuRadio");
      this.menuStartReceiver.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuStartReceiver, "menuStartReceiver");
      this.menuStartReceiver.Click += new System.EventHandler(this.menuStartReceiver_Click);
      this.menuStopReceiver.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuStopReceiver, "menuStopReceiver");
      this.menuStopReceiver.Click += new System.EventHandler(this.menuStopReceiver_Click);
      this.menuItem7.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItem7, "menuItem7");
      this.menuDeleteData.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuDeleteData, "menuDeleteData");
      this.menuDeleteData.Click += new System.EventHandler(this.menuDeleteData_Click);
      this.menuRead.Index = 4;
      this.menuRead.MenuItems.AddRange(new MenuItem[12]
      {
        this.menuReadDeviceParameter,
        this.menuReadAll,
        this.menuItem14,
        this.menuRequestLoop,
        this.menuItemRequestLoopAll,
        this.menuSerchBaudrate,
        this.menuItem12,
        this.menuShowAllParameters,
        this.menuLineSerieX,
        this.menuGetVersion,
        this.menuGetVersionTestCycle,
        this.menuConnectAcrossBaudrates
      });
      componentResourceManager.ApplyResources((object) this.menuRead, "menuRead");
      this.menuReadDeviceParameter.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuReadDeviceParameter, "menuReadDeviceParameter");
      this.menuReadDeviceParameter.Click += new System.EventHandler(this.menuReadDeviceParameter_Click);
      this.menuReadAll.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuReadAll, "menuReadAll");
      this.menuReadAll.Click += new System.EventHandler(this.menuReadAll_Click);
      this.menuItem14.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItem14, "menuItem14");
      this.menuRequestLoop.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuRequestLoop, "menuRequestLoop");
      this.menuRequestLoop.Click += new System.EventHandler(this.menuRequestLoop_Click);
      this.menuItemRequestLoopAll.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItemRequestLoopAll, "menuItemRequestLoopAll");
      this.menuItemRequestLoopAll.Click += new System.EventHandler(this.menuItemRequestLoopAll_Click);
      this.menuSerchBaudrate.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuSerchBaudrate, "menuSerchBaudrate");
      this.menuSerchBaudrate.Click += new System.EventHandler(this.menuSerchBaudrate_Click);
      this.menuItem12.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItem12, "menuItem12");
      this.menuShowAllParameters.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuShowAllParameters, "menuShowAllParameters");
      this.menuShowAllParameters.Click += new System.EventHandler(this.menuShowAllParameters_Click);
      this.menuLineSerieX.Index = 8;
      componentResourceManager.ApplyResources((object) this.menuLineSerieX, "menuLineSerieX");
      this.menuGetVersion.Index = 9;
      componentResourceManager.ApplyResources((object) this.menuGetVersion, "menuGetVersion");
      this.menuGetVersion.Click += new System.EventHandler(this.menuGetVersion_Click);
      this.menuGetVersionTestCycle.Index = 10;
      componentResourceManager.ApplyResources((object) this.menuGetVersionTestCycle, "menuGetVersionTestCycle");
      this.menuGetVersionTestCycle.Click += new System.EventHandler(this.menuGetVersionTestCycle_Click);
      this.menuConnectAcrossBaudrates.Index = 11;
      componentResourceManager.ApplyResources((object) this.menuConnectAcrossBaudrates, "menuConnectAcrossBaudrates");
      this.menuConnectAcrossBaudrates.Click += new System.EventHandler(this.menuConnectAcrossBaudrates_Click);
      this.menuFunction.Index = 5;
      this.menuFunction.MenuItems.AddRange(new MenuItem[5]
      {
        this.menuReset,
        this.menuRunBackup,
        this.menuSetEmergencyMode,
        this.menuTransmitRadioFrame,
        this.menuItemShiftToNextAddress
      });
      componentResourceManager.ApplyResources((object) this.menuFunction, "menuFunction");
      this.menuReset.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuReset, "menuReset");
      this.menuReset.Click += new System.EventHandler(this.menuReset_Click);
      this.menuRunBackup.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuRunBackup, "menuRunBackup");
      this.menuRunBackup.Click += new System.EventHandler(this.menuRunBackup_Click);
      this.menuSetEmergencyMode.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuSetEmergencyMode, "menuSetEmergencyMode");
      this.menuSetEmergencyMode.Click += new System.EventHandler(this.menuSetEmergencyMode_Click);
      this.menuTransmitRadioFrame.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuTransmitRadioFrame, "menuTransmitRadioFrame");
      this.menuTransmitRadioFrame.Click += new System.EventHandler(this.menuTransmitRadioFrame_Click);
      this.menuItemShiftToNextAddress.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItemShiftToNextAddress, "menuItemShiftToNextAddress");
      this.menuItemShiftToNextAddress.Click += new System.EventHandler(this.menuItemShiftToNextAddress_Click);
      this.menuTest.Index = 6;
      this.menuTest.MenuItems.AddRange(new MenuItem[13]
      {
        this.menuItemMemoryAccess,
        this.menuItemReadEEPromLoop,
        this.menuMeterMonitor,
        this.menuSchnittstellenFehlerLoop,
        this.menuEEPromWriteReadLoop,
        this.menuEEPromReset,
        this.menuItemIO_Test,
        this.menuItemShowWaveFlowParameter,
        this.menuItemWafeFlowParameterTest,
        this.menuItemAblaufTest,
        this.menuItem5,
        this.menuMbusParser,
        this.menuItemMinoConnectTest
      });
      componentResourceManager.ApplyResources((object) this.menuTest, "menuTest");
      this.menuItemMemoryAccess.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuItemMemoryAccess, "menuItemMemoryAccess");
      this.menuItemMemoryAccess.Click += new System.EventHandler(this.menuItemMemoryAccess_Click);
      this.menuItemReadEEPromLoop.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuItemReadEEPromLoop, "menuItemReadEEPromLoop");
      this.menuItemReadEEPromLoop.Click += new System.EventHandler(this.menuItemReadEEPromLoop_Click);
      this.menuMeterMonitor.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuMeterMonitor, "menuMeterMonitor");
      this.menuMeterMonitor.Click += new System.EventHandler(this.menuMeterMonitor_Click);
      this.menuSchnittstellenFehlerLoop.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuSchnittstellenFehlerLoop, "menuSchnittstellenFehlerLoop");
      this.menuSchnittstellenFehlerLoop.Click += new System.EventHandler(this.menuSchnittstellenFehlerLoop_Click);
      this.menuEEPromWriteReadLoop.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuEEPromWriteReadLoop, "menuEEPromWriteReadLoop");
      this.menuEEPromWriteReadLoop.Click += new System.EventHandler(this.menuEEPromWriteReadLoop_Click);
      this.menuEEPromReset.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuEEPromReset, "menuEEPromReset");
      this.menuEEPromReset.Click += new System.EventHandler(this.menuEEPromReset_Click);
      this.menuItemIO_Test.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItemIO_Test, "menuItemIO_Test");
      this.menuItemIO_Test.Click += new System.EventHandler(this.menuItemIO_Test_Click);
      this.menuItemShowWaveFlowParameter.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuItemShowWaveFlowParameter, "menuItemShowWaveFlowParameter");
      this.menuItemShowWaveFlowParameter.Click += new System.EventHandler(this.menuItemShowWaveFlowParameter_Click);
      this.menuItemWafeFlowParameterTest.Index = 8;
      componentResourceManager.ApplyResources((object) this.menuItemWafeFlowParameterTest, "menuItemWafeFlowParameterTest");
      this.menuItemWafeFlowParameterTest.Click += new System.EventHandler(this.menuItemWafeFlowParameterTest_Click);
      this.menuItemAblaufTest.Index = 9;
      componentResourceManager.ApplyResources((object) this.menuItemAblaufTest, "menuItemAblaufTest");
      this.menuItemAblaufTest.Click += new System.EventHandler(this.menuItemAblaufTest_Click);
      this.menuItem5.Index = 10;
      componentResourceManager.ApplyResources((object) this.menuItem5, "menuItem5");
      this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
      this.menuMbusParser.Index = 11;
      componentResourceManager.ApplyResources((object) this.menuMbusParser, "menuMbusParser");
      this.menuMbusParser.Click += new System.EventHandler(this.menuMbusParser_Click);
      this.menuItemMinoConnectTest.Index = 12;
      componentResourceManager.ApplyResources((object) this.menuItemMinoConnectTest, "menuItemMinoConnectTest");
      this.menuItemMinoConnectTest.Click += new System.EventHandler(this.menuItemMinoConnectTest_Click);
      this.menuComponent.Index = 7;
      this.menuComponent.MenuItems.AddRange(new MenuItem[8]
      {
        this.menuItemStartWindow,
        this.menuStartGlobalMeterManager,
        this.menuItemBack,
        this.menuItemQuit,
        this.menuItem15,
        this.menuStartDesigner,
        this.menuItemConfigurator,
        this.menuStartAsyncCom
      });
      componentResourceManager.ApplyResources((object) this.menuComponent, "menuComponent");
      this.menuItemStartWindow.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuItemStartWindow, "menuItemStartWindow");
      this.menuItemStartWindow.Click += new System.EventHandler(this.menuItemStartWindow_Click);
      this.menuStartGlobalMeterManager.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuStartGlobalMeterManager, "menuStartGlobalMeterManager");
      this.menuStartGlobalMeterManager.Click += new System.EventHandler(this.menuStartGlobalMeterManager_Click);
      this.menuItemBack.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItemBack, "menuItemBack");
      this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
      this.menuItemQuit.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuItemQuit, "menuItemQuit");
      this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
      this.menuItem15.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuItem15, "menuItem15");
      this.menuStartDesigner.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuStartDesigner, "menuStartDesigner");
      this.menuStartDesigner.Click += new System.EventHandler(this.menuStartDesigner_Click);
      this.menuItemConfigurator.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuItemConfigurator, "menuItemConfigurator");
      this.menuItemConfigurator.Click += new System.EventHandler(this.menuItemConfigurator_Click);
      this.menuStartAsyncCom.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuStartAsyncCom, "menuStartAsyncCom");
      this.menuStartAsyncCom.Click += new System.EventHandler(this.menuStartAsyncCom_Click);
      this.menuHelp.Index = 8;
      this.menuHelp.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItemSerialBusHelp,
        this.menuItem6
      });
      componentResourceManager.ApplyResources((object) this.menuHelp, "menuHelp");
      this.menuItemSerialBusHelp.Index = 0;
      this.menuItemSerialBusHelp.RadioCheck = true;
      componentResourceManager.ApplyResources((object) this.menuItemSerialBusHelp, "menuItemSerialBusHelp");
      this.menuItemSerialBusHelp.Click += new System.EventHandler(this.menuItemSerialBusHelp_Click);
      this.menuItem6.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuItem6, "menuItem6");
      this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
      componentResourceManager.ApplyResources((object) this.tabControl, "tabControl");
      this.tabControl.Controls.Add((Control) this.tabPageBusInfo);
      this.tabControl.Controls.Add((Control) this.tabPageAllParameter);
      this.tabControl.Controls.Add((Control) this.tabPageDeviceParameter);
      this.tabControl.Controls.Add((Control) this.tabPageBusSetup);
      this.tabControl.Controls.Add((Control) this.tabPageMemory);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
      this.tabPageBusInfo.ContextMenu = this.contextMenu1;
      this.tabPageBusInfo.Controls.Add((Control) this.textBoxBusFilePath);
      this.tabPageBusInfo.Controls.Add((Control) this.dataGridBusTable);
      componentResourceManager.ApplyResources((object) this.tabPageBusInfo, "tabPageBusInfo");
      this.tabPageBusInfo.Name = "tabPageBusInfo";
      this.tabPageBusInfo.UseVisualStyleBackColor = true;
      this.contextMenu1.MenuItems.AddRange(new MenuItem[13]
      {
        this.menuCtReadMeterParameter,
        this.menuItemShowMeterData,
        this.menuItem3,
        this.menuCtSetPrimaryAddress,
        this.menuCtDeleteFromBusinfo,
        this.menuItemCmSetParameterListLine,
        this.menuCmSetToDefaultParameterList,
        this.menuCmSetToFullParameterList,
        this.menuCtSelectParameterList,
        this.menuCtMBusConverterLine,
        this.menuCtMBusConverter,
        this.menuItem13,
        this.menuCtShowHideColumnsDeviceList
      });
      this.menuCtReadMeterParameter.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuCtReadMeterParameter, "menuCtReadMeterParameter");
      this.menuCtReadMeterParameter.Click += new System.EventHandler(this.menuCtReadMeterParameter_Click);
      this.menuItemShowMeterData.Index = 1;
      componentResourceManager.ApplyResources((object) this.menuItemShowMeterData, "menuItemShowMeterData");
      this.menuItemShowMeterData.Click += new System.EventHandler(this.menuItemShowMeterData_Click);
      this.menuItem3.Index = 2;
      componentResourceManager.ApplyResources((object) this.menuItem3, "menuItem3");
      this.menuCtSetPrimaryAddress.Index = 3;
      componentResourceManager.ApplyResources((object) this.menuCtSetPrimaryAddress, "menuCtSetPrimaryAddress");
      this.menuCtSetPrimaryAddress.Click += new System.EventHandler(this.menuCtSetPrimaryAddress_Click);
      this.menuCtDeleteFromBusinfo.Index = 4;
      componentResourceManager.ApplyResources((object) this.menuCtDeleteFromBusinfo, "menuCtDeleteFromBusinfo");
      this.menuCtDeleteFromBusinfo.Click += new System.EventHandler(this.menuCtDeleteFromBusinfo_Click);
      this.menuItemCmSetParameterListLine.Index = 5;
      componentResourceManager.ApplyResources((object) this.menuItemCmSetParameterListLine, "menuItemCmSetParameterListLine");
      this.menuCmSetToDefaultParameterList.Index = 6;
      componentResourceManager.ApplyResources((object) this.menuCmSetToDefaultParameterList, "menuCmSetToDefaultParameterList");
      this.menuCmSetToDefaultParameterList.Click += new System.EventHandler(this.menuCmSetToDefaultParameterList_Click);
      this.menuCmSetToFullParameterList.Index = 7;
      componentResourceManager.ApplyResources((object) this.menuCmSetToFullParameterList, "menuCmSetToFullParameterList");
      this.menuCmSetToFullParameterList.Click += new System.EventHandler(this.menuCmSetToFullParameterList_Click);
      this.menuCtSelectParameterList.Index = 8;
      componentResourceManager.ApplyResources((object) this.menuCtSelectParameterList, "menuCtSelectParameterList");
      this.menuCtSelectParameterList.Click += new System.EventHandler(this.menuCtSelectParameterList_Click);
      this.menuCtMBusConverterLine.Index = 9;
      componentResourceManager.ApplyResources((object) this.menuCtMBusConverterLine, "menuCtMBusConverterLine");
      this.menuCtMBusConverter.Index = 10;
      componentResourceManager.ApplyResources((object) this.menuCtMBusConverter, "menuCtMBusConverter");
      this.menuCtMBusConverter.Click += new System.EventHandler(this.menuCtMBusConverter_Click);
      this.menuItem13.Index = 11;
      componentResourceManager.ApplyResources((object) this.menuItem13, "menuItem13");
      this.menuCtShowHideColumnsDeviceList.Index = 12;
      componentResourceManager.ApplyResources((object) this.menuCtShowHideColumnsDeviceList, "menuCtShowHideColumnsDeviceList");
      componentResourceManager.ApplyResources((object) this.textBoxBusFilePath, "textBoxBusFilePath");
      this.textBoxBusFilePath.Name = "textBoxBusFilePath";
      this.textBoxBusFilePath.ReadOnly = true;
      this.dataGridBusTable.AllowUserToAddRows = false;
      this.dataGridBusTable.AllowUserToDeleteRows = false;
      this.dataGridBusTable.AllowUserToResizeRows = false;
      componentResourceManager.ApplyResources((object) this.dataGridBusTable, "dataGridBusTable");
      this.dataGridBusTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridBusTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridBusTable.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dataGridBusTable.Name = "dataGridBusTable";
      this.dataGridBusTable.RowTemplate.Height = 24;
      this.dataGridBusTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridBusTable.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dataGridBusTable_CellBeginEdit);
      this.dataGridBusTable.CellEndEdit += new DataGridViewCellEventHandler(this.dataGridBusTable_CellEndEdit);
      this.dataGridBusTable.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dataGridBusTable_CellMouseDoubleClick);
      this.dataGridBusTable.Sorted += new System.EventHandler(this.dataGridBusTable_Sorted);
      this.dataGridBusTable.KeyUp += new KeyEventHandler(this.dataGridBusTable_KeyUp);
      this.dataGridBusTable.MouseClick += new MouseEventHandler(this.dataGridBusTable_MouseClick);
      this.tabPageAllParameter.ContextMenu = this.contextMenuAllParameters;
      this.tabPageAllParameter.Controls.Add((Control) this.splitContainerAllParameters);
      componentResourceManager.ApplyResources((object) this.tabPageAllParameter, "tabPageAllParameter");
      this.tabPageAllParameter.Name = "tabPageAllParameter";
      this.tabPageAllParameter.UseVisualStyleBackColor = true;
      this.contextMenuAllParameters.MenuItems.AddRange(new MenuItem[1]
      {
        this.menuCtShowHideColumnsAllParameters
      });
      this.menuCtShowHideColumnsAllParameters.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuCtShowHideColumnsAllParameters, "menuCtShowHideColumnsAllParameters");
      this.tabPageDeviceParameter.Controls.Add((Control) this.groupBoxDeviceInfo);
      this.tabPageDeviceParameter.Controls.Add((Control) this.groupBoxRadioReadoutSystem);
      this.tabPageDeviceParameter.Controls.Add((Control) this.dataGridParameterList);
      componentResourceManager.ApplyResources((object) this.tabPageDeviceParameter, "tabPageDeviceParameter");
      this.tabPageDeviceParameter.Name = "tabPageDeviceParameter";
      this.tabPageDeviceParameter.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.groupBoxDeviceInfo, "groupBoxDeviceInfo");
      this.groupBoxDeviceInfo.Controls.Add((Control) this.textBoxManufacturer);
      this.groupBoxDeviceInfo.Controls.Add((Control) this.label5);
      this.groupBoxDeviceInfo.Controls.Add((Control) this.textBoxMedium);
      this.groupBoxDeviceInfo.Controls.Add((Control) this.label6);
      this.groupBoxDeviceInfo.Controls.Add((Control) this.label7);
      this.groupBoxDeviceInfo.Controls.Add((Control) this.textBoxSerialNr);
      this.groupBoxDeviceInfo.Name = "groupBoxDeviceInfo";
      this.groupBoxDeviceInfo.TabStop = false;
      componentResourceManager.ApplyResources((object) this.textBoxManufacturer, "textBoxManufacturer");
      this.textBoxManufacturer.Name = "textBoxManufacturer";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.textBoxMedium, "textBoxMedium");
      this.textBoxMedium.Name = "textBoxMedium";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.textBoxSerialNr, "textBoxSerialNr");
      this.textBoxSerialNr.Name = "textBoxSerialNr";
      componentResourceManager.ApplyResources((object) this.groupBoxRadioReadoutSystem, "groupBoxRadioReadoutSystem");
      this.groupBoxRadioReadoutSystem.Controls.Add((Control) this.labelReadoutSystemText);
      this.groupBoxRadioReadoutSystem.Controls.Add((Control) this.labelReadoutSystemVersionText);
      this.groupBoxRadioReadoutSystem.Controls.Add((Control) this.labelReadoutSystem);
      this.groupBoxRadioReadoutSystem.Controls.Add((Control) this.labelReadoutSystemVersion);
      this.groupBoxRadioReadoutSystem.Name = "groupBoxRadioReadoutSystem";
      this.groupBoxRadioReadoutSystem.TabStop = false;
      componentResourceManager.ApplyResources((object) this.labelReadoutSystemText, "labelReadoutSystemText");
      this.labelReadoutSystemText.Name = "labelReadoutSystemText";
      componentResourceManager.ApplyResources((object) this.labelReadoutSystemVersionText, "labelReadoutSystemVersionText");
      this.labelReadoutSystemVersionText.Name = "labelReadoutSystemVersionText";
      this.labelReadoutSystem.BorderStyle = BorderStyle.Fixed3D;
      this.labelReadoutSystem.FlatStyle = FlatStyle.System;
      componentResourceManager.ApplyResources((object) this.labelReadoutSystem, "labelReadoutSystem");
      this.labelReadoutSystem.Name = "labelReadoutSystem";
      this.labelReadoutSystemVersion.BorderStyle = BorderStyle.Fixed3D;
      this.labelReadoutSystemVersion.FlatStyle = FlatStyle.System;
      componentResourceManager.ApplyResources((object) this.labelReadoutSystemVersion, "labelReadoutSystemVersion");
      this.labelReadoutSystemVersion.Name = "labelReadoutSystemVersion";
      componentResourceManager.ApplyResources((object) this.dataGridParameterList, "dataGridParameterList");
      this.dataGridParameterList.DataMember = "";
      this.dataGridParameterList.HeaderForeColor = SystemColors.ControlText;
      this.dataGridParameterList.Name = "dataGridParameterList";
      this.dataGridParameterList.PreferredColumnWidth = 150;
      this.tabPageBusSetup.Controls.Add((Control) this.btnSetDefaultSettings);
      this.tabPageBusSetup.Controls.Add((Control) this.gboxMinomat);
      this.tabPageBusSetup.Controls.Add((Control) this.groupBoxRadioReceiverSetup);
      this.tabPageBusSetup.Controls.Add((Control) this.groupBox3);
      this.tabPageBusSetup.Controls.Add((Control) this.groupBoxBaseBusSettings);
      this.tabPageBusSetup.Controls.Add((Control) this.groupBoxLogger);
      this.tabPageBusSetup.Controls.Add((Control) this.checkBoxAutoSaveSetup);
      componentResourceManager.ApplyResources((object) this.tabPageBusSetup, "tabPageBusSetup");
      this.tabPageBusSetup.Name = "tabPageBusSetup";
      this.tabPageBusSetup.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.btnSetDefaultSettings, "btnSetDefaultSettings");
      this.btnSetDefaultSettings.Name = "btnSetDefaultSettings";
      this.btnSetDefaultSettings.UseVisualStyleBackColor = true;
      this.btnSetDefaultSettings.Click += new System.EventHandler(this.btnSetDefaultSettings_Click);
      this.gboxMinomat.Controls.Add((Control) this.dateTimePickerToTime);
      this.gboxMinomat.Controls.Add((Control) this.dateTimePickerFromTime);
      this.gboxMinomat.Controls.Add((Control) this.label12);
      this.gboxMinomat.Controls.Add((Control) this.label20);
      this.gboxMinomat.Controls.Add((Control) this.textBoxMinomatSerial);
      this.gboxMinomat.Controls.Add((Control) this.label13);
      componentResourceManager.ApplyResources((object) this.gboxMinomat, "gboxMinomat");
      this.gboxMinomat.Name = "gboxMinomat";
      this.gboxMinomat.TabStop = false;
      this.dateTimePickerToTime.Format = DateTimePickerFormat.Short;
      componentResourceManager.ApplyResources((object) this.dateTimePickerToTime, "dateTimePickerToTime");
      this.dateTimePickerToTime.Name = "dateTimePickerToTime";
      this.dateTimePickerToTime.ValueChanged += new System.EventHandler(this.dateTimePickerToTime_ValueChanged);
      this.dateTimePickerFromTime.Format = DateTimePickerFormat.Short;
      componentResourceManager.ApplyResources((object) this.dateTimePickerFromTime, "dateTimePickerFromTime");
      this.dateTimePickerFromTime.Name = "dateTimePickerFromTime";
      this.dateTimePickerFromTime.ValueChanged += new System.EventHandler(this.dateTimePickerFromTime_ValueChanged);
      componentResourceManager.ApplyResources((object) this.label12, "label12");
      this.label12.Name = "label12";
      componentResourceManager.ApplyResources((object) this.label20, "label20");
      this.label20.Name = "label20";
      componentResourceManager.ApplyResources((object) this.textBoxMinomatSerial, "textBoxMinomatSerial");
      this.textBoxMinomatSerial.Name = "textBoxMinomatSerial";
      this.textBoxMinomatSerial.Leave += new System.EventHandler(this.textBoxMinomatSerial_Leave);
      componentResourceManager.ApplyResources((object) this.label13, "label13");
      this.label13.Name = "label13";
      this.groupBoxRadioReceiverSetup.Controls.Add((Control) this.textBoxReceiveLevel);
      this.groupBoxRadioReceiverSetup.Controls.Add((Control) this.label19);
      componentResourceManager.ApplyResources((object) this.groupBoxRadioReceiverSetup, "groupBoxRadioReceiverSetup");
      this.groupBoxRadioReceiverSetup.Name = "groupBoxRadioReceiverSetup";
      this.groupBoxRadioReceiverSetup.TabStop = false;
      componentResourceManager.ApplyResources((object) this.textBoxReceiveLevel, "textBoxReceiveLevel");
      this.textBoxReceiveLevel.Name = "textBoxReceiveLevel";
      componentResourceManager.ApplyResources((object) this.label19, "label19");
      this.label19.Name = "label19";
      componentResourceManager.ApplyResources((object) this.groupBox3, "groupBox3");
      this.groupBox3.Controls.Add((Control) this.buttonSelectZDF_File);
      this.groupBox3.Controls.Add((Control) this.textBoxZDF_FileName);
      this.groupBox3.Controls.Add((Control) this.checkBoxLoggToZDF_File);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.TabStop = false;
      componentResourceManager.ApplyResources((object) this.buttonSelectZDF_File, "buttonSelectZDF_File");
      this.buttonSelectZDF_File.Name = "buttonSelectZDF_File";
      this.buttonSelectZDF_File.Click += new System.EventHandler(this.buttonSelectZDF_File_Click);
      componentResourceManager.ApplyResources((object) this.textBoxZDF_FileName, "textBoxZDF_FileName");
      this.textBoxZDF_FileName.Name = "textBoxZDF_FileName";
      this.textBoxZDF_FileName.Leave += new System.EventHandler(this.textBoxZDF_FileName_Leave);
      componentResourceManager.ApplyResources((object) this.checkBoxLoggToZDF_File, "checkBoxLoggToZDF_File");
      this.checkBoxLoggToZDF_File.Name = "checkBoxLoggToZDF_File";
      this.checkBoxLoggToZDF_File.CheckedChanged += new System.EventHandler(this.checkBoxLoggToZDF_File_CheckedChanged);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxSND_NKE);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxUseReqUd2_5B);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxApplicationReset);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxIsMultitelegrammEnabled);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.textBoxLoopTime);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxFastSecoundaryAddressing);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.label14);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxOnlySecondaryAddressing);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxBeepByReading);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxUseExternalKeySignal);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxChangeInterfaceBaudrateToo);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.checkBoxKeepDestinationAddress);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.label15);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.comboBoxBusMode);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.label16);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.label8);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.label17);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.textBoxRepeadsOnError);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.textBoxScanStartAddress);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.label18);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.textBoxScanStartNumber);
      this.groupBoxBaseBusSettings.Controls.Add((Control) this.textBoxOrganizeStartAddress);
      componentResourceManager.ApplyResources((object) this.groupBoxBaseBusSettings, "groupBoxBaseBusSettings");
      this.groupBoxBaseBusSettings.Name = "groupBoxBaseBusSettings";
      this.groupBoxBaseBusSettings.TabStop = false;
      componentResourceManager.ApplyResources((object) this.checkBoxSND_NKE, "checkBoxSND_NKE");
      this.checkBoxSND_NKE.Name = "checkBoxSND_NKE";
      this.checkBoxSND_NKE.CheckedChanged += new System.EventHandler(this.checkBoxSND_NKE_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxUseReqUd2_5B, "checkBoxUseReqUd2_5B");
      this.checkBoxUseReqUd2_5B.Name = "checkBoxUseReqUd2_5B";
      this.checkBoxUseReqUd2_5B.CheckedChanged += new System.EventHandler(this.checkBoxUseReqUd2_5B_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxApplicationReset, "checkBoxApplicationReset");
      this.checkBoxApplicationReset.Name = "checkBoxApplicationReset";
      this.checkBoxApplicationReset.CheckedChanged += new System.EventHandler(this.checkBoxApplicationReset_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxIsMultitelegrammEnabled, "checkBoxIsMultitelegrammEnabled");
      this.checkBoxIsMultitelegrammEnabled.Name = "checkBoxIsMultitelegrammEnabled";
      this.checkBoxIsMultitelegrammEnabled.CheckedChanged += new System.EventHandler(this.checkBoxIsMultitelegrammEnabled_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.textBoxLoopTime, "textBoxLoopTime");
      this.textBoxLoopTime.Name = "textBoxLoopTime";
      this.textBoxLoopTime.Leave += new System.EventHandler(this.textBoxLoopTime_Leave);
      componentResourceManager.ApplyResources((object) this.checkBoxFastSecoundaryAddressing, "checkBoxFastSecoundaryAddressing");
      this.checkBoxFastSecoundaryAddressing.Name = "checkBoxFastSecoundaryAddressing";
      this.checkBoxFastSecoundaryAddressing.CheckedChanged += new System.EventHandler(this.checkBoxFastSecoundaryAddressing_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.label14, "label14");
      this.label14.Name = "label14";
      componentResourceManager.ApplyResources((object) this.checkBoxOnlySecondaryAddressing, "checkBoxOnlySecondaryAddressing");
      this.checkBoxOnlySecondaryAddressing.Name = "checkBoxOnlySecondaryAddressing";
      this.checkBoxOnlySecondaryAddressing.CheckedChanged += new System.EventHandler(this.checkBoxOnlySecondaryAddressing_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxBeepByReading, "checkBoxBeepByReading");
      this.checkBoxBeepByReading.Name = "checkBoxBeepByReading";
      this.checkBoxBeepByReading.CheckedChanged += new System.EventHandler(this.checkBoxBeepByReading_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxUseExternalKeySignal, "checkBoxUseExternalKeySignal");
      this.checkBoxUseExternalKeySignal.Name = "checkBoxUseExternalKeySignal";
      this.checkBoxUseExternalKeySignal.CheckedChanged += new System.EventHandler(this.checkBoxUseExternalKeySignal_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxChangeInterfaceBaudrateToo, "checkBoxChangeInterfaceBaudrateToo");
      this.checkBoxChangeInterfaceBaudrateToo.Name = "checkBoxChangeInterfaceBaudrateToo";
      this.checkBoxChangeInterfaceBaudrateToo.CheckedChanged += new System.EventHandler(this.checkBoxChangeInterfaceBaudrateToo_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxKeepDestinationAddress, "checkBoxKeepDestinationAddress");
      this.checkBoxKeepDestinationAddress.Name = "checkBoxKeepDestinationAddress";
      this.checkBoxKeepDestinationAddress.CheckedChanged += new System.EventHandler(this.checkBoxKeepDestinationAddress_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.label15, "label15");
      this.label15.Name = "label15";
      this.comboBoxBusMode.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.comboBoxBusMode, "comboBoxBusMode");
      this.comboBoxBusMode.Name = "comboBoxBusMode";
      this.comboBoxBusMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxBusMode_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label16, "label16");
      this.label16.Name = "label16";
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this.label17, "label17");
      this.label17.Name = "label17";
      componentResourceManager.ApplyResources((object) this.textBoxRepeadsOnError, "textBoxRepeadsOnError");
      this.textBoxRepeadsOnError.Name = "textBoxRepeadsOnError";
      this.textBoxRepeadsOnError.KeyDown += new KeyEventHandler(this.textBoxRepeadsOnError_KeyDown);
      this.textBoxRepeadsOnError.Leave += new System.EventHandler(this.textBoxRepeadsOnError_Leave);
      componentResourceManager.ApplyResources((object) this.textBoxScanStartAddress, "textBoxScanStartAddress");
      this.textBoxScanStartAddress.Name = "textBoxScanStartAddress";
      this.textBoxScanStartAddress.Leave += new System.EventHandler(this.textBoxScanStartAddress_Leave);
      componentResourceManager.ApplyResources((object) this.label18, "label18");
      this.label18.Name = "label18";
      componentResourceManager.ApplyResources((object) this.textBoxScanStartNumber, "textBoxScanStartNumber");
      this.textBoxScanStartNumber.Name = "textBoxScanStartNumber";
      this.textBoxScanStartNumber.Leave += new System.EventHandler(this.textBoxScanStartNumber_Leave);
      componentResourceManager.ApplyResources((object) this.textBoxOrganizeStartAddress, "textBoxOrganizeStartAddress");
      this.textBoxOrganizeStartAddress.Name = "textBoxOrganizeStartAddress";
      this.textBoxOrganizeStartAddress.Leave += new System.EventHandler(this.textBoxOrganizeStartAddress_Leave);
      this.groupBoxLogger.Controls.Add((Control) this.comboBoxLoggToCom);
      this.groupBoxLogger.Controls.Add((Control) this.label4);
      this.groupBoxLogger.Controls.Add((Control) this.checkBoxLoggToFile);
      this.groupBoxLogger.Controls.Add((Control) this.checkBoxLoggAllTemp);
      componentResourceManager.ApplyResources((object) this.groupBoxLogger, "groupBoxLogger");
      this.groupBoxLogger.Name = "groupBoxLogger";
      this.groupBoxLogger.TabStop = false;
      componentResourceManager.ApplyResources((object) this.comboBoxLoggToCom, "comboBoxLoggToCom");
      this.comboBoxLoggToCom.Items.AddRange(new object[13]
      {
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items1"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items2"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items3"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items4"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items5"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items6"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items7"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items8"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items9"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items10"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items11"),
        (object) componentResourceManager.GetString("comboBoxLoggToCom.Items12")
      });
      this.comboBoxLoggToCom.Name = "comboBoxLoggToCom";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.checkBoxLoggToFile, "checkBoxLoggToFile");
      this.checkBoxLoggToFile.Name = "checkBoxLoggToFile";
      componentResourceManager.ApplyResources((object) this.checkBoxLoggAllTemp, "checkBoxLoggAllTemp");
      this.checkBoxLoggAllTemp.Name = "checkBoxLoggAllTemp";
      componentResourceManager.ApplyResources((object) this.checkBoxAutoSaveSetup, "checkBoxAutoSaveSetup");
      this.checkBoxAutoSaveSetup.Name = "checkBoxAutoSaveSetup";
      this.checkBoxAutoSaveSetup.CheckedChanged += new System.EventHandler(this.checkBoxAutoSaveSetup_CheckedChanged);
      this.tabPageMemory.Controls.Add((Control) this.checkBoxWatchRange);
      this.tabPageMemory.Controls.Add((Control) this.groupBox2);
      this.tabPageMemory.Controls.Add((Control) this.groupBox1);
      this.tabPageMemory.Controls.Add((Control) this.listBoxLocation);
      this.tabPageMemory.Controls.Add((Control) this.label1);
      this.tabPageMemory.Controls.Add((Control) this.textBoxStartAddress);
      this.tabPageMemory.Controls.Add((Control) this.label2);
      this.tabPageMemory.Controls.Add((Control) this.textBoxNumberOfBytes);
      this.tabPageMemory.Controls.Add((Control) this.labelLocation);
      this.tabPageMemory.Controls.Add((Control) this.buttonEraseFlash);
      this.tabPageMemory.Controls.Add((Control) this.buttonShowMemory);
      this.tabPageMemory.Controls.Add((Control) this.buttonReadMemory);
      componentResourceManager.ApplyResources((object) this.tabPageMemory, "tabPageMemory");
      this.tabPageMemory.Name = "tabPageMemory";
      this.tabPageMemory.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.checkBoxWatchRange, "checkBoxWatchRange");
      this.checkBoxWatchRange.Name = "checkBoxWatchRange";
      this.checkBoxWatchRange.CheckedChanged += new System.EventHandler(this.checkBoxWatchRange_CheckedChanged);
      this.groupBox2.Controls.Add((Control) this.buttonDeleteMeterKey);
      this.groupBox2.Controls.Add((Control) this.label9);
      this.groupBox2.Controls.Add((Control) this.textBoxValue);
      this.groupBox2.Controls.Add((Control) this.buttonSetBaudrate);
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.buttonDeleteMeterKey, "buttonDeleteMeterKey");
      this.buttonDeleteMeterKey.Name = "buttonDeleteMeterKey";
      this.buttonDeleteMeterKey.Click += new System.EventHandler(this.buttonDeleteMeterKey_Click);
      componentResourceManager.ApplyResources((object) this.label9, "label9");
      this.label9.Name = "label9";
      this.textBoxValue.AcceptsReturn = true;
      componentResourceManager.ApplyResources((object) this.textBoxValue, "textBoxValue");
      this.textBoxValue.Name = "textBoxValue";
      componentResourceManager.ApplyResources((object) this.buttonSetBaudrate, "buttonSetBaudrate");
      this.buttonSetBaudrate.Name = "buttonSetBaudrate";
      this.buttonSetBaudrate.Click += new System.EventHandler(this.buttonSetBaudrate_Click);
      this.groupBox1.Controls.Add((Control) this.textBoxAndMask);
      this.groupBox1.Controls.Add((Control) this.label10);
      this.groupBox1.Controls.Add((Control) this.label11);
      this.groupBox1.Controls.Add((Control) this.textBoxOrMask);
      this.groupBox1.Controls.Add((Control) this.buttonWriteBitfield);
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      this.textBoxAndMask.AcceptsReturn = true;
      componentResourceManager.ApplyResources((object) this.textBoxAndMask, "textBoxAndMask");
      this.textBoxAndMask.Name = "textBoxAndMask";
      componentResourceManager.ApplyResources((object) this.label10, "label10");
      this.label10.Name = "label10";
      componentResourceManager.ApplyResources((object) this.label11, "label11");
      this.label11.Name = "label11";
      this.textBoxOrMask.AcceptsReturn = true;
      componentResourceManager.ApplyResources((object) this.textBoxOrMask, "textBoxOrMask");
      this.textBoxOrMask.Name = "textBoxOrMask";
      componentResourceManager.ApplyResources((object) this.buttonWriteBitfield, "buttonWriteBitfield");
      this.buttonWriteBitfield.Name = "buttonWriteBitfield";
      this.buttonWriteBitfield.Click += new System.EventHandler(this.buttonWriteBitfield_Click);
      this.listBoxLocation.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("listBoxLocation.Items"),
        (object) componentResourceManager.GetString("listBoxLocation.Items1")
      });
      componentResourceManager.ApplyResources((object) this.listBoxLocation, "listBoxLocation");
      this.listBoxLocation.Name = "listBoxLocation";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.textBoxStartAddress, "textBoxStartAddress");
      this.textBoxStartAddress.Name = "textBoxStartAddress";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.textBoxNumberOfBytes, "textBoxNumberOfBytes");
      this.textBoxNumberOfBytes.Name = "textBoxNumberOfBytes";
      componentResourceManager.ApplyResources((object) this.labelLocation, "labelLocation");
      this.labelLocation.Name = "labelLocation";
      this.buttonEraseFlash.BackColor = SystemColors.Control;
      componentResourceManager.ApplyResources((object) this.buttonEraseFlash, "buttonEraseFlash");
      this.buttonEraseFlash.Name = "buttonEraseFlash";
      this.buttonEraseFlash.UseVisualStyleBackColor = false;
      this.buttonEraseFlash.Click += new System.EventHandler(this.buttonShowMemory_Click);
      componentResourceManager.ApplyResources((object) this.buttonShowMemory, "buttonShowMemory");
      this.buttonShowMemory.Name = "buttonShowMemory";
      this.buttonShowMemory.Click += new System.EventHandler(this.buttonShowMemory_Click);
      componentResourceManager.ApplyResources((object) this.buttonReadMemory, "buttonReadMemory");
      this.buttonReadMemory.Name = "buttonReadMemory";
      this.buttonReadMemory.Click += new System.EventHandler(this.buttonReadMemory_Click);
      componentResourceManager.ApplyResources((object) this.progressBar, "progressBar");
      this.progressBar.Name = "progressBar";
      componentResourceManager.ApplyResources((object) this.buttonOk, "buttonOk");
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      componentResourceManager.ApplyResources((object) this.buttonBreak, "buttonBreak");
      this.buttonBreak.Name = "buttonBreak";
      this.buttonBreak.Click += new System.EventHandler(this.buttonBreak_Click);
      this.buttonBreak.MouseLeave += new System.EventHandler(this.buttonBreak_MouseLeave);
      this.buttonBreak.MouseMove += new MouseEventHandler(this.buttonBreak_MouseMove);
      componentResourceManager.ApplyResources((object) this.labelStatus, "labelStatus");
      this.labelStatus.BorderStyle = BorderStyle.Fixed3D;
      this.labelStatus.Name = "labelStatus";
      this.buttonToolbarReadAllDevices.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarReadAllDevices, "buttonToolbarReadAllDevices");
      this.buttonToolbarReadAllDevices.Name = "buttonToolbarReadAllDevices";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarReadAllDevices, componentResourceManager.GetString("buttonToolbarReadAllDevices.ToolTip"));
      this.buttonToolbarReadAllDevices.Click += new System.EventHandler(this.buttonReadAllDevices_Click);
      this.buttonToolbarReadDevice.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarReadDevice, "buttonToolbarReadDevice");
      this.buttonToolbarReadDevice.Name = "buttonToolbarReadDevice";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarReadDevice, componentResourceManager.GetString("buttonToolbarReadDevice.ToolTip"));
      this.buttonToolbarReadDevice.Click += new System.EventHandler(this.buttonReadDevice_Click);
      this.buttonToolbarDeleteBusinfo.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarDeleteBusinfo, "buttonToolbarDeleteBusinfo");
      this.buttonToolbarDeleteBusinfo.Name = "buttonToolbarDeleteBusinfo";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarDeleteBusinfo, componentResourceManager.GetString("buttonToolbarDeleteBusinfo.ToolTip"));
      this.buttonToolbarDeleteBusinfo.Click += new System.EventHandler(this.buttonToolbarDeleteBusinfo_Click);
      this.buttonToolbarScanBusByAddress.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarScanBusByAddress, "buttonToolbarScanBusByAddress");
      this.buttonToolbarScanBusByAddress.Name = "buttonToolbarScanBusByAddress";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarScanBusByAddress, componentResourceManager.GetString("buttonToolbarScanBusByAddress.ToolTip"));
      this.buttonToolbarScanBusByAddress.Click += new System.EventHandler(this.buttonToolbarScanBusByAddress_Click);
      this.buttonToolbarScanBusBySerialNumber.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarScanBusBySerialNumber, "buttonToolbarScanBusBySerialNumber");
      this.buttonToolbarScanBusBySerialNumber.Name = "buttonToolbarScanBusBySerialNumber";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarScanBusBySerialNumber, componentResourceManager.GetString("buttonToolbarScanBusBySerialNumber.ToolTip"));
      this.buttonToolbarScanBusBySerialNumber.Click += new System.EventHandler(this.buttonToolbarScanBusBySerialNumber_Click);
      this.buttonToolbarSearchSingelDeviceByAddress.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarSearchSingelDeviceByAddress, "buttonToolbarSearchSingelDeviceByAddress");
      this.buttonToolbarSearchSingelDeviceByAddress.Name = "buttonToolbarSearchSingelDeviceByAddress";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarSearchSingelDeviceByAddress, componentResourceManager.GetString("buttonToolbarSearchSingelDeviceByAddress.ToolTip"));
      this.buttonToolbarSearchSingelDeviceByAddress.Click += new System.EventHandler(this.buttonToolbarSearchSingelDeviceByAddress_Click);
      this.buttonToolbarSearchSingelDeviceBySerialNumber.FlatAppearance.BorderSize = 0;
      componentResourceManager.ApplyResources((object) this.buttonToolbarSearchSingelDeviceBySerialNumber, "buttonToolbarSearchSingelDeviceBySerialNumber");
      this.buttonToolbarSearchSingelDeviceBySerialNumber.Name = "buttonToolbarSearchSingelDeviceBySerialNumber";
      this.toolTip1.SetToolTip((Control) this.buttonToolbarSearchSingelDeviceBySerialNumber, componentResourceManager.GetString("buttonToolbarSearchSingelDeviceBySerialNumber.ToolTip"));
      this.buttonToolbarSearchSingelDeviceBySerialNumber.Click += new System.EventHandler(this.buttonToolbarSearchSingelDeviceBySerialNumber_Click);
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this.buttonAsyncCom, "buttonAsyncCom");
      this.buttonAsyncCom.Name = "buttonAsyncCom";
      this.buttonAsyncCom.UseVisualStyleBackColor = true;
      this.buttonAsyncCom.Click += new System.EventHandler(this.buttonAsyncCom_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.buttonAsyncCom);
      this.Controls.Add((Control) this.buttonToolbarScanBusByAddress);
      this.Controls.Add((Control) this.buttonToolbarDeleteBusinfo);
      this.Controls.Add((Control) this.buttonToolbarReadAllDevices);
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.labelStatus);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.tabControl);
      this.Controls.Add((Control) this.buttonBreak);
      this.Controls.Add((Control) this.buttonToolbarReadDevice);
      this.Controls.Add((Control) this.buttonToolbarScanBusBySerialNumber);
      this.Controls.Add((Control) this.buttonToolbarSearchSingelDeviceByAddress);
      this.Controls.Add((Control) this.buttonToolbarSearchSingelDeviceBySerialNumber);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Menu = this.SerialBusMenu;
      this.Name = nameof (DeviceCollectorWindow);
      this.Activated += new System.EventHandler(this.SerialBus_Activated);
      this.Closing += new CancelEventHandler(this.SerialBus_Closing);
      this.Closed += new System.EventHandler(this.SerialBus_Closed);
      this.Load += new System.EventHandler(this.SerialBus_Load);
      this.VisibleChanged += new System.EventHandler(this.DeviceCollectorWindow_VisibleChanged);
      this.splitContainerAllParameters.Panel1.ResumeLayout(false);
      this.splitContainerAllParameters.Panel1.PerformLayout();
      this.splitContainerAllParameters.Panel2.ResumeLayout(false);
      this.splitContainerAllParameters.Panel2.PerformLayout();
      this.splitContainerAllParameters.EndInit();
      this.splitContainerAllParameters.ResumeLayout(false);
      ((ISupportInitialize) this.dataGridViewAllParameter).EndInit();
      this.tabControl.ResumeLayout(false);
      this.tabPageBusInfo.ResumeLayout(false);
      this.tabPageBusInfo.PerformLayout();
      ((ISupportInitialize) this.dataGridBusTable).EndInit();
      this.tabPageAllParameter.ResumeLayout(false);
      this.tabPageDeviceParameter.ResumeLayout(false);
      this.groupBoxDeviceInfo.ResumeLayout(false);
      this.groupBoxDeviceInfo.PerformLayout();
      this.groupBoxRadioReadoutSystem.ResumeLayout(false);
      this.dataGridParameterList.EndInit();
      this.tabPageBusSetup.ResumeLayout(false);
      this.gboxMinomat.ResumeLayout(false);
      this.gboxMinomat.PerformLayout();
      this.groupBoxRadioReceiverSetup.ResumeLayout(false);
      this.groupBoxRadioReceiverSetup.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBoxBaseBusSettings.ResumeLayout(false);
      this.groupBoxBaseBusSettings.PerformLayout();
      this.groupBoxLogger.ResumeLayout(false);
      this.tabPageMemory.ResumeLayout(false);
      this.tabPageMemory.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }

    internal DeviceCollectorWindow(DeviceCollectorFunctions TheBus)
    {
      this.initActive = true;
      this.InitializeComponent();
      FormTranslatorSupport.TranslateWindow(Tg.DeviceCollectorWindow, (Form) this);
      this.buttonToolbarReadDevice.Image = Images.pics.DeviceRead_20x20.Image;
      this.buttonToolbarReadAllDevices.Image = Images.pics.ReadAll_20x20.Image;
      this.buttonToolbarDeleteBusinfo.Image = Images.pics.DeleteAllDevices_20x20.Image;
      this.buttonToolbarScanBusByAddress.Image = Images.pics.ScanByAddress_20x20.Image;
      this.buttonToolbarScanBusBySerialNumber.Image = Images.pics.ScanBySerialNo_20x20.Image;
      this.buttonToolbarSearchSingelDeviceByAddress.Image = Images.pics.SeachByAddress_20x20.Image;
      this.buttonToolbarSearchSingelDeviceBySerialNumber.Image = Images.pics.SearchBySerialNo_20x20.Image;
      this.MyBus = TheBus;
      if (Thread.CurrentThread.Name != "GMM main")
        this.Text = this.Text + " [" + Thread.CurrentThread.Name + "]";
      this.MyBus.BreakRequest = true;
      this.ClearFields();
      foreach (string name in Enum.GetNames(typeof (BusMode)))
      {
        string str1 = name;
        BusMode busMode = BusMode.MBus;
        string str2 = busMode.ToString();
        if (!(str1 == str2) || UserManager.CheckPermission(UserRights.Rights.MBus))
        {
          string str3 = name;
          busMode = BusMode.MBusPointToPoint;
          string str4 = busMode.ToString();
          if (!(str3 == str4) || UserManager.CheckPermission(UserRights.Rights.MBus))
          {
            string str5 = name;
            busMode = BusMode.RelayDevice;
            string str6 = busMode.ToString();
            if (!(str5 == str6) || UserManager.CheckPermission(UserRights.Rights.MBus))
            {
              string str7 = name;
              busMode = BusMode.Minol_Device;
              string str8 = busMode.ToString();
              if (!(str7 == str8) || UserManager.CheckPermission(UserRights.Rights.MinolExpertHandler))
              {
                string str9 = name;
                busMode = BusMode.MinomatV2;
                string str10 = busMode.ToString();
                if (!(str9 == str10) || UserManager.CheckPermission(UserRights.Rights.MinomatV2))
                {
                  string str11 = name;
                  busMode = BusMode.MinomatV3;
                  string str12 = busMode.ToString();
                  int num1;
                  if (!(str11 == str12))
                  {
                    string str13 = name;
                    busMode = BusMode.MinomatV4;
                    string str14 = busMode.ToString();
                    if (!(str13 == str14))
                    {
                      string str15 = name;
                      busMode = BusMode.RadioMS;
                      string str16 = busMode.ToString();
                      if (!(str15 == str16))
                      {
                        string str17 = name;
                        busMode = BusMode.MinomatRadioTest;
                        string str18 = busMode.ToString();
                        if (!(str17 == str18))
                        {
                          num1 = 0;
                          goto label_14;
                        }
                      }
                    }
                  }
                  num1 = !UserManager.CheckPermission(UserRights.Rights.MinomatV4) ? 1 : 0;
label_14:
                  if (num1 == 0)
                  {
                    string str19 = name;
                    busMode = BusMode.Radio2;
                    string str20 = busMode.ToString();
                    if (!(str19 == str20) || UserManager.CheckPermission(UserRights.Rights.WalkBy))
                    {
                      string str21 = name;
                      busMode = BusMode.Radio3;
                      string str22 = busMode.ToString();
                      if (!(str21 == str22) || UserManager.CheckPermission(UserRights.Rights.Radio3))
                      {
                        string str23 = name;
                        busMode = BusMode.Radio3_868_95_RUSSIA;
                        string str24 = busMode.ToString();
                        if (!(str23 == str24) || UserManager.CheckPermission(UserRights.Rights.Radio3))
                        {
                          string str25 = name;
                          busMode = BusMode.wMBusC1A;
                          string str26 = busMode.ToString();
                          int num2;
                          if (!(str25 == str26))
                          {
                            string str27 = name;
                            busMode = BusMode.wMBusC1B;
                            string str28 = busMode.ToString();
                            if (!(str27 == str28))
                            {
                              string str29 = name;
                              busMode = BusMode.wMBusS1;
                              string str30 = busMode.ToString();
                              if (!(str29 == str30))
                              {
                                string str31 = name;
                                busMode = BusMode.wMBusS1M;
                                string str32 = busMode.ToString();
                                if (!(str31 == str32))
                                {
                                  string str33 = name;
                                  busMode = BusMode.wMBusS2;
                                  string str34 = busMode.ToString();
                                  if (!(str33 == str34))
                                  {
                                    string str35 = name;
                                    busMode = BusMode.wMBusT1;
                                    string str36 = busMode.ToString();
                                    if (!(str35 == str36))
                                    {
                                      string str37 = name;
                                      busMode = BusMode.wMBusT2_meter;
                                      string str38 = busMode.ToString();
                                      if (!(str37 == str38))
                                      {
                                        string str39 = name;
                                        busMode = BusMode.wMBusT2_other;
                                        string str40 = busMode.ToString();
                                        if (!(str39 == str40))
                                        {
                                          num2 = 0;
                                          goto label_28;
                                        }
                                      }
                                    }
                                  }
                                }
                              }
                            }
                          }
                          num2 = !UserManager.CheckPermission(UserRights.Rights.WirelessMBus) ? 1 : 0;
label_28:
                          if (num2 == 0)
                          {
                            string str41 = name;
                            busMode = BusMode.WaveFlowRadio;
                            string str42 = busMode.ToString();
                            if (!(str41 == str42) || UserManager.CheckPermission(UserRights.Rights.Waveflow))
                            {
                              string str43 = name;
                              busMode = BusMode.SmokeDetector;
                              string str44 = busMode.ToString();
                              if (!(str43 == str44) || UserManager.CheckPermission(UserRights.Rights.SmokeDetectorHandler))
                                this.comboBoxBusMode.Items.Add((object) name);
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      this.EventObject = new EventHandler<GMM_EventArgs>(this.WorkMessage);
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.UpdateSettings();
      this.SetEnabledFunctions();
      this.listBoxLocation.SelectedIndex = 0;
      new DataGridViewColumnSelector(this.dataGridBusTable, this.menuCtShowHideColumnsDeviceList).ColumnsShowAlways = new string[1]
      {
        "Nr."
      };
      DataGridViewColumnSelector viewColumnSelector = new DataGridViewColumnSelector(this.dataGridViewAllParameter, this.menuCtShowHideColumnsAllParameters);
      if (!UserManager.CheckPermission(UserRights.Rights.Developer))
        this.HideColumns(this.dataGridBusTable, "IsSel", "SelRep", "ReadRep", "DeselRep");
      this.initActive = false;
    }

    private void DeviceCollectorWindow_VisibleChanged(object sender, EventArgs e)
    {
      if (!this.IsHandleCreated)
        return;
      if (this.Visible)
      {
        this.MyBus.OnMessage += this.EventObject;
        this.MyBus.MyCom.OnAsyncComMessage += this.EventObject;
      }
      else
      {
        this.MyBus.OnMessage -= this.EventObject;
        this.MyBus.MyCom.OnAsyncComMessage -= this.EventObject;
      }
    }

    internal void InitStartMenu(string ComponentList)
    {
      this.StartComponentName = "";
      if (ComponentList == null)
      {
        this.menuComponent.Visible = false;
      }
      else
      {
        this.menuStartAsyncCom.Visible = UserManager.CheckPermission(UserRights.Rights.AsyncCom);
        this.menuStartDesigner.Visible = UserManager.CheckPermission(UserRights.Rights.Designer);
        this.menuItemConfigurator.Visible = UserManager.CheckPermission(UserRights.Rights.Configurator);
        this.menuComponent.Visible = this.menuStartAsyncCom.Visible || this.menuStartDesigner.Visible || this.menuItemConfigurator.Visible;
      }
    }

    internal void ClearTables()
    {
      this.dataGridBusTable.DataSource = (object) null;
      this.dataGridParameterList.DataSource = (object) null;
      this.dataGridViewAllParameter.Columns.Clear();
    }

    internal void RefreshBusInfo()
    {
      if (this.MyBus.MyDeviceList == null)
        return;
      DataTable BusTable;
      this.MyBus.MyDeviceList.GetBusTable(out BusTable, this.MyBus.MyBusMode);
      this.dataGridBusTable.DataSource = (object) BusTable;
      this.textBoxBusFilePath.Text = this.MyBus.MyBusInfo.BusInfoFilename;
      this.gboxMinomat.Enabled = this.MyBus.MyBusMode == BusMode.MinomatV2 || this.MyBus.MyBusMode == BusMode.MinomatRadioTest;
      if (this.MyBus.MyDeviceList.SelectedDevice != null)
      {
        int tableIndex = this.MyBus.MyDeviceList.SelectedDevice.TableIndex;
        if (tableIndex >= 0)
        {
          for (int index = 0; index < this.dataGridBusTable.Rows.Count; ++index)
          {
            if (tableIndex < BusTable.Rows.Count && ((DataRowView) this.dataGridBusTable.Rows[index].DataBoundItem).Row == BusTable.Rows[tableIndex])
            {
              if (!this.dataGridBusTable.Rows[index].Selected)
              {
                this.dataGridBusTable.CurrentCell = this.dataGridBusTable.Rows[index].Cells[0];
                this.dataGridBusTable.ClearSelection();
                this.dataGridBusTable.Rows[index].Selected = true;
                this.dataGridBusTable.Refresh();
                break;
              }
              break;
            }
          }
        }
      }
      this.textBoxRepeadsOnError.Text = this.MyBus.MaxRequestRepeat.ToString();
      this.comboBoxBusModeEventsEnabled = false;
      if (this.comboBoxBusMode.Items.Contains((object) this.MyBus.MyBusMode.ToString()))
        this.comboBoxBusMode.SelectedIndex = this.comboBoxBusMode.Items.IndexOf((object) this.MyBus.MyBusMode.ToString());
      else if (!string.IsNullOrEmpty(this.comboBoxBusMode.Text))
        this.MyBus.SetBaseMode((BusMode) Enum.Parse(typeof (BusMode), this.comboBoxBusMode.Text));
      this.textBoxMinomatSerial.Text = this.MyBus.DaKonId;
      DateTime readFromTime = this.MyBus.ReadFromTime;
      if (!ZR_ClassLibrary.Util.AreEqual((object) this.MyBus.ReadFromTime, (object) DateTime.MinValue) && !ZR_ClassLibrary.Util.AreEqual((object) this.MyBus.ReadFromTime, (object) DateTime.MaxValue))
        this.dateTimePickerFromTime.Value = this.MyBus.ReadFromTime;
      DateTime readToTime = this.MyBus.ReadToTime;
      if (!ZR_ClassLibrary.Util.AreEqual((object) this.MyBus.ReadToTime, (object) DateTime.MinValue) && !ZR_ClassLibrary.Util.AreEqual((object) this.MyBus.ReadToTime, (object) DateTime.MaxValue))
        this.dateTimePickerToTime.Value = this.MyBus.ReadToTime;
      this.comboBoxBusModeEventsEnabled = true;
      if (this.LoopIsRunning)
        return;
      this.SetEnabledFunctions();
    }

    private void btnSetDefaultSettings_Click(object sender, EventArgs e)
    {
      this.MyBus.SetDefaultSettings();
      this.UpdateSettings();
    }

    private void dataGridBusTable_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyValue != 46 || this.dataGridBusTable.SelectedRows.Count != 1)
        return;
      DataGridViewRow selectedRow = this.dataGridBusTable.SelectedRows[0];
      if (selectedRow.Cells.Count > 5 && selectedRow.Cells[2].Value.ToString() == "Collision")
      {
        this.MyBus.MyDeviceList.RemoveFaultyDevices(Convert.ToByte(selectedRow.Cells[5].Value.ToString()));
        this.dataGridBusTable.Rows.Remove(selectedRow);
      }
    }

    private void comboBoxBusMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.comboBoxBusModeEventsEnabled || !this.IsRunning)
        return;
      if (this.ModeChangeBreak)
      {
        this.ModeChangeBreak = false;
      }
      else
      {
        this.BusinfoIsManualChanged = true;
        if (GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("13"), MessageBoxButtons.OKCancel) != DialogResult.OK)
        {
          this.ModeChangeBreak = true;
          this.RefreshBusInfo();
        }
        else
        {
          this.MyBus.SetBaseMode((BusMode) Enum.Parse(typeof (BusMode), this.comboBoxBusMode.Text));
          this.tabControl.SelectedTab = this.tabPageBusInfo;
          this.Refresh();
          if (this.MyBus.MyBusMode == BusMode.MBusPointToPoint)
          {
            int num = (int) new AddDevice(this.MyBus).ShowDialog();
          }
          else if (this.MyBus.MyBusMode == BusMode.Minol_Device)
            this.MyBus.AddDevice(DeviceTypes.Minol_Device, -1, -1L);
          this.RefreshBusInfo();
          this.gboxMinomat.Enabled = this.MyBus.MyBusMode == BusMode.MinomatV2;
        }
      }
    }

    private void textBoxRepeadsOnError_Leave(object sender, EventArgs e) => this.CheckRepeats();

    private void textBoxRepeadsOnError_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.CheckRepeats();
    }

    private void CheckRepeats()
    {
      bool flag = true;
      int num1 = 0;
      try
      {
        num1 = int.Parse(this.textBoxRepeadsOnError.Text);
        if (num1 >= 1 && num1 <= 10)
          flag = false;
      }
      catch
      {
      }
      if (flag)
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Value error on Repeat setup");
        this.textBoxRepeadsOnError.Text = this.MyBus.MaxRequestRepeat.ToString();
      }
      else
        this.MyBus.MaxRequestRepeat = num1;
    }

    private void textBoxScanStartAddress_Leave(object sender, EventArgs e)
    {
      try
      {
        this.MyBus.ScanStartAddress = ZR_ClassLibrary.Util.ToInteger((object) this.textBoxScanStartAddress.Text);
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", this.label15.Text + ": " + ex.Message);
        this.textBoxScanStartAddress.Text = this.MyBus.ScanStartAddress.ToString();
      }
    }

    private void textBoxScanStartNumber_Leave(object sender, EventArgs e)
    {
      this.MyBus.ScanStartSerialnumber = this.textBoxScanStartNumber.Text;
    }

    private void textBoxOrganizeStartAddress_Leave(object sender, EventArgs e)
    {
      try
      {
        this.MyBus.OrganizeStartAddress = ZR_ClassLibrary.Util.ToInteger((object) this.textBoxOrganizeStartAddress.Text);
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", this.label17.Text + ": " + ex.Message);
        this.textBoxOrganizeStartAddress.Text = this.MyBus.OrganizeStartAddress.ToString();
      }
    }

    private void textBoxLoopTime_Leave(object sender, EventArgs e)
    {
      try
      {
        this.MyBus.CycleTime = ZR_ClassLibrary.Util.ToInteger((object) this.textBoxLoopTime.Text);
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", this.label14.Text + ": " + ex.Message);
        this.textBoxLoopTime.Text = this.MyBus.CycleTime.ToString();
      }
    }

    private void dateTimePickerFromTime_ValueChanged(object sender, EventArgs e)
    {
      this.MyBus.ReadFromTime = this.dateTimePickerFromTime.Value;
    }

    private void dateTimePickerToTime_ValueChanged(object sender, EventArgs e)
    {
      this.MyBus.ReadToTime = this.dateTimePickerToTime.Value;
    }

    private void textBoxMinomatSerial_Leave(object sender, EventArgs e)
    {
      this.MyBus.DaKonId = this.textBoxMinomatSerial.Text;
    }

    private void textBoxZDF_FileName_Leave(object sender, EventArgs e)
    {
      this.MyBus.LogFilePath = this.textBoxZDF_FileName.Text;
    }

    private void checkBoxOnlySecondaryAddressing_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.OnlySecondaryAddressing = this.checkBoxOnlySecondaryAddressing.Checked;
    }

    private void checkBoxIsMultitelegrammEnabled_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.IsMultiTelegrammEnabled = this.checkBoxIsMultitelegrammEnabled.Checked;
    }

    private void checkBoxApplicationReset_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.SendFirstApplicationReset = this.checkBoxApplicationReset.Checked;
    }

    private void checkBoxUseReqUd2_5B_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.UseREQ_UD2_5B = this.checkBoxUseReqUd2_5B.Checked;
    }

    private void checkBoxSND_NKE_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.SendFirstSND_NKE = this.checkBoxSND_NKE.Checked;
    }

    private void checkBoxFastSecoundaryAddressing_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.FastSecondaryAddressing = this.checkBoxFastSecoundaryAddressing.Checked;
    }

    private void checkBoxKeepDestinationAddress_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.KeepExistingDestinationAddress = this.checkBoxKeepDestinationAddress.Checked;
    }

    private void checkBoxChangeInterfaceBaudrateToo_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.ChangeInterfaceBaudrateToo = this.checkBoxChangeInterfaceBaudrateToo.Checked;
    }

    private void checkBoxUseExternalKeySignal_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.UseExternalKeyForReading = this.checkBoxUseExternalKeySignal.Checked;
    }

    private void checkBoxBeepByReading_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.BeepSignalForReadResult = this.checkBoxBeepByReading.Checked;
    }

    private void checkBoxAutoSaveSetup_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initActive)
        return;
      this.MyBus.Autosave = this.checkBoxAutoSaveSetup.Checked;
    }

    private void checkBoxLoggToZDF_File_CheckedChanged(object sender, EventArgs e)
    {
      this.MyBus.LogToFileEnabled = this.checkBoxLoggToZDF_File.Checked;
    }

    private void UpdateSettings()
    {
      if (this.MyBus == null)
        return;
      try
      {
        this.comboBoxBusMode.SelectedIndex = this.comboBoxBusMode.Items.IndexOf((object) this.MyBus.MyBusMode.ToString());
        this.textBoxMinomatSerial.Text = this.MyBus.DaKonId;
        this.textBoxZDF_FileName.Text = this.MyBus.LogFilePath;
        this.textBoxScanStartNumber.Text = this.MyBus.ScanStartSerialnumber;
        this.textBoxOrganizeStartAddress.Text = this.MyBus.OrganizeStartAddress.ToString();
        this.textBoxRepeadsOnError.Text = this.MyBus.MaxRequestRepeat.ToString();
        this.textBoxScanStartAddress.Text = this.MyBus.ScanStartAddress.ToString();
        this.textBoxLoopTime.Text = this.MyBus.CycleTime.ToString();
        this.checkBoxOnlySecondaryAddressing.Checked = this.MyBus.OnlySecondaryAddressing;
        this.checkBoxFastSecoundaryAddressing.Checked = this.MyBus.FastSecondaryAddressing;
        this.checkBoxChangeInterfaceBaudrateToo.Checked = this.MyBus.ChangeInterfaceBaudrateToo;
        this.checkBoxUseExternalKeySignal.Checked = this.MyBus.UseExternalKeyForReading;
        this.checkBoxBeepByReading.Checked = this.MyBus.BeepSignalForReadResult;
        this.checkBoxKeepDestinationAddress.Checked = this.MyBus.KeepExistingDestinationAddress;
        this.checkBoxLoggToZDF_File.Checked = this.MyBus.LogToFileEnabled;
        this.checkBoxIsMultitelegrammEnabled.Checked = this.MyBus.IsMultiTelegrammEnabled;
        this.checkBoxUseReqUd2_5B.Checked = this.MyBus.UseREQ_UD2_5B;
        this.checkBoxApplicationReset.Checked = this.MyBus.SendFirstApplicationReset;
        this.checkBoxSND_NKE.Checked = this.MyBus.SendFirstSND_NKE;
        if (this.MyBus.ReadFromTime != DateTime.MinValue)
          this.dateTimePickerFromTime.Value = this.MyBus.ReadFromTime;
        if (this.MyBus.ReadToTime != DateTime.MinValue)
          this.dateTimePickerToTime.Value = this.MyBus.ReadToTime;
        this.checkBoxAutoSaveSetup.Checked = this.MyBus.Autosave;
      }
      catch (Exception ex)
      {
        DeviceCollectorWindow.logger.Error<string, string>("{0} {1}", ex.Message, ex.StackTrace);
      }
    }

    private void WorkMessage(object sender, GMM_EventArgs MessageObj)
    {
      if (this.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.WorkMessage), sender, (object) MessageObj);
        }
        catch
        {
        }
      }
      else
      {
        switch (MessageObj.TheMessageType)
        {
          case GMM_EventArgs.MessageType.StandardMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage + MessageObj.InfoNumber.ToString();
            break;
          case GMM_EventArgs.MessageType.Alive:
            ++this.aliveCounter;
            break;
          case GMM_EventArgs.MessageType.PrimaryAddressMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage + MessageObj.InfoNumber.ToString("x04");
            break;
          case GMM_EventArgs.MessageType.ScanAddressMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage + "  found:" + MessageObj.InfoNumber.ToString();
            this.RefreshBusInfo();
            break;
          case GMM_EventArgs.MessageType.EndMessage:
            this.labelStatus.Text = this.BaseMessage + string.Empty;
            break;
          case GMM_EventArgs.MessageType.MinomatErrorMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage;
            this.SetEnabledFunctions();
            break;
          case GMM_EventArgs.MessageType.MinomatAliveMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage;
            this.SetEnabledFunctions();
            break;
          case GMM_EventArgs.MessageType.MinomatConnectingMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage + MessageObj.InfoNumber.ToString();
            this.SetEnabledFunctions();
            break;
          case GMM_EventArgs.MessageType.KeyReceived:
            if (this.MyBus.MyBusMode != BusMode.MBus && this.MyBus.MyBusMode != BusMode.MBusPointToPoint && this.MyBus.MyBusMode != BusMode.WaveFlowRadio && this.MyBus.MyBusMode != BusMode.MinomatV2 || !this.MyBus.UseExternalKeyForReading)
              break;
            this.ReadDeviceParameter(false);
            break;
          case GMM_EventArgs.MessageType.MessageAndProgressPercentage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage;
            if (MessageObj.ProgressPercentage >= 0 && MessageObj.ProgressPercentage <= 100)
              this.progressBar.Value = MessageObj.ProgressPercentage;
            if (this.MyBus.MyBusMode == BusMode.RelayDevice)
              break;
            this.RefreshBusInfo();
            break;
          case GMM_EventArgs.MessageType.SimpleMessage:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage;
            break;
          case GMM_EventArgs.MessageType.WalkByPacketReceived:
            this.labelStatus.Text = this.BaseMessage + MessageObj.EventMessage;
            this.RefreshBusInfo();
            this.SetEnabledFunctions();
            this.AddToZDF_ParameterTable(((RadioList) this.MyBus.MyDeviceList).DeviceInfoOfLastReceivedPacket);
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            break;
        }
      }
    }

    private void ClearFields()
    {
      this.InfoCounter = 0;
      this.ErrorCounter = 0;
      this.textBoxSerialNr.Text = "";
      this.textBoxMedium.Text = "";
      this.textBoxManufacturer.Text = "";
      this.progressBar.Value = 0;
      this.labelStatus.Text = "";
      this.BaseMessage = string.Empty;
    }

    private void buttonReadMemory_Click(object sender, EventArgs e)
    {
      this.MyDump = new MemoryDump();
      this.MyBus.SetMessageInfo("ReadMemory at address: 0x");
      int hexDecValue1 = this.GetHexDecValue(this.textBoxNumberOfBytes.Text);
      int hexDecValue2 = this.GetHexDecValue(this.textBoxStartAddress.Text);
      int selectedIndex = this.listBoxLocation.SelectedIndex;
      this.Cursor = Cursors.WaitCursor;
      ByteField MemoryData;
      bool flag = this.MyBus.ReadMemory((MemoryLocation) selectedIndex, hexDecValue2, hexDecValue1, out MemoryData);
      this.Cursor = Cursors.Default;
      if (!flag)
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "ReadMemory Error");
      }
      else if (MemoryData.Count == 0)
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "keine Daten empfangen");
      }
      else
      {
        int count = MemoryData.Count;
        int[] data = new int[count];
        long num3 = 0;
        for (int index = 0; index < count; ++index)
        {
          num3 += (long) MemoryData.Data[index];
          data[index] = (int) MemoryData.Data[index];
        }
        this.labelStatus.Text = "Read memory ok. Checksum: 0x" + num3.ToString("x");
        this.Refresh();
        this.MyDump.SetMemory(hexDecValue2, hexDecValue2 + count - 1, data, this.listBoxLocation.SelectedItem.ToString());
        int num4 = (int) this.MyDump.ShowDialog();
        if (!this.MyDump.WriteChanges)
          return;
        this.MyBus.SetMessageInfo("WriteMemory at address: 0x");
        this.WriteMemory();
      }
    }

    private int GetHexDecValue(string InString)
    {
      InString.TrimStart();
      return InString.Length > 2 && InString[0] == '0' && InString[1] == 'x' ? int.Parse(InString.Substring(2), NumberStyles.HexNumber) : int.Parse(InString);
    }

    private void WriteMemory()
    {
      ArrayList data1 = new ArrayList();
      int selectedIndex = this.listBoxLocation.SelectedIndex;
      this.MyDump.GetChangedData(ref data1);
      int index1;
      for (int index2 = 0; index2 < data1.Count; index2 = index1 - 1 + 1)
      {
        index1 = index2 + 1;
        while (index1 < data1.Count && ((MemoryDump.ChangedDataInfo) data1[index1 - 1]).Address + 1 == ((MemoryDump.ChangedDataInfo) data1[index1]).Address)
          ++index1;
        ByteField data2 = new ByteField(index1 - index2);
        for (int index3 = index2; index3 < index1; ++index3)
          data2.Add(((MemoryDump.ChangedDataInfo) data1[index3]).NewData);
        if (!this.MyBus.WriteMemory(selectedIndex, ((MemoryDump.ChangedDataInfo) data1[index2]).Address, data2))
        {
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Write error");
          return;
        }
      }
      int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Write ok");
    }

    private void buttonShowMemory_Click(object sender, EventArgs e)
    {
      if (this.MyDump != null)
      {
        int num = (int) this.MyDump.ShowDialog();
        if (!this.MyDump.WriteChanges)
          return;
        this.WriteMemory();
      }
      else
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Dump not available");
      }
    }

    private void menuRequestLoop_Click(object sender, EventArgs e)
    {
      this.SetLoopConditions();
      this.ClearFields();
      this.BaseMessage = this.GetCounterStatus();
      this.tabControl.SelectedTab = this.tabPageDeviceParameter;
      int num = 5;
      while (!this.MyBus.BreakRequest)
      {
        if (num > 2)
          this.labelStatus.Text = this.BaseMessage + "Reading";
        if (this.MyBus.MyDeviceList.SelectedDevice != null && this.MyBus.MyDeviceList.SelectedDevice is MBusDevice)
        {
          MBusDevice selectedDevice = (MBusDevice) this.MyBus.MyDeviceList.SelectedDevice;
          selectedDevice.followingTelegrammAnnounced = false;
          selectedDevice.followingTelegrammTransmit_FCB_Odd = !this.MyBus.UseREQ_UD2_5B;
        }
        DeviceInfo Info;
        if (this.MyBus.ReadParameter(out Info))
        {
          if (Info != null)
          {
            this.textBoxSerialNr.Text = Info.MeterNumber;
            this.textBoxMedium.Text = Info.Medium.ToString();
            this.textBoxManufacturer.Text = Info.Manufacturer.ToString();
            ++this.InfoCounter;
            Info.GenerateParameterTable();
            this.dataGridParameterList.DataSource = (object) Info.ParameterTable;
            this.Refresh();
            this.AddToZDF_ParameterTable(Info);
          }
        }
        else
          ++this.ErrorCounter;
        this.BaseMessage = this.GetCounterStatus();
        this.labelStatus.Text = this.BaseMessage + " ... waiting";
        try
        {
          num = int.Parse(this.textBoxLoopTime.Text) * 2;
        }
        catch
        {
          this.textBoxLoopTime.Text = "5";
          num = 5;
        }
        this.Refresh();
        for (int index = 0; index < num && !this.MyBus.BreakRequest; ++index)
        {
          this.progressBar.Value = 100 / num * (index + 1);
          Application.DoEvents();
          Thread.Sleep(500);
        }
        this.Refresh();
        Application.DoEvents();
      }
      this.ClearFields();
      this.ResetLoopConditions();
    }

    private void menuItemRequestLoopAll_Click(object sender, EventArgs e)
    {
      if (this.MyBus.MyDeviceList.bus == null || this.MyBus.MyDeviceList.bus.Count < 0)
        return;
      this.SetLoopConditions();
      this.ClearFields();
      this.BaseMessage = this.GetCounterStatus();
      this.tabControl.SelectedTab = this.tabPageAllParameter;
      int num1 = 0;
      while (!this.MyBus.BreakRequest)
      {
        this.labelStatus.Text = "Reading";
        this.MyBus.MyDeviceList.SelectedDevice = (BusDevice) this.MyBus.MyDeviceList.bus[num1++];
        DeviceInfo Info;
        if (this.MyBus.ReadParameter(out Info))
        {
          this.textBoxSerialNr.Text = Info.MeterNumber;
          this.textBoxMedium.Text = Info.Medium.ToString();
          this.textBoxManufacturer.Text = Info.Manufacturer.ToString();
          ++this.InfoCounter;
          Info.GenerateParameterTable();
          this.dataGridParameterList.DataSource = (object) Info.ParameterTable;
          this.BaseMessage = this.GetCounterStatus();
          this.Refresh();
          this.AddToZDF_ParameterTable(Info);
        }
        else
          ++this.ErrorCounter;
        Thread.Sleep(100);
        Application.DoEvents();
        if (num1 == this.MyBus.MyDeviceList.bus.Count)
        {
          num1 = 0;
          this.labelStatus.Text += " ... waiting";
          this.BaseMessage = this.GetCounterStatus();
          this.labelStatus.Text = this.BaseMessage + " ... waiting";
          this.Refresh();
          int num2;
          try
          {
            num2 = int.Parse(this.textBoxLoopTime.Text) * 2;
          }
          catch
          {
            this.textBoxLoopTime.Text = "5";
            num2 = 5;
          }
          for (int index = 0; index < num2 && !this.MyBus.BreakRequest; ++index)
          {
            this.progressBar.Value = 100 / num2 * (index + 1);
            Application.DoEvents();
            Thread.Sleep(500);
          }
          this.Refresh();
        }
      }
      this.ClearFields();
      this.ResetLoopConditions();
    }

    private string GetCounterStatus()
    {
      StringBuilder stringBuilder = new StringBuilder(100);
      stringBuilder.Append("(ok:");
      stringBuilder.Append(this.InfoCounter.ToString());
      stringBuilder.Append(" err:");
      stringBuilder.Append(this.ErrorCounter.ToString());
      stringBuilder.Append(") ");
      return stringBuilder.ToString();
    }

    private void SetEnabledFunctions()
    {
      bool flag1 = UserManager.CheckPermission(UserRights.Rights.Developer);
      bool flag2 = !flag1 && UserManager.CheckPermission("Demo");
      bool flag3 = this.MyBus.MyBusMode == BusMode.MBus;
      bool flag4 = this.MyBus.MyBusMode == BusMode.MBusPointToPoint;
      bool flag5 = this.MyBus.MyBusMode == BusMode.WaveFlowRadio;
      bool flag6 = this.MyBus.MyBusMode == BusMode.MinomatV2;
      bool flag7 = this.MyBus.MyBusMode == BusMode.Minol_Device;
      bool flag8 = this.MyBus.MyBusMode == BusMode.RelayDevice;
      bool flag9 = this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.MinomatRadioTest || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.wMBusT2_other;
      bool flag10 = flag6 & flag1;
      bool flag11 = false;
      if (this.dataGridBusTable.DataSource != null && ((DataTable) this.dataGridBusTable.DataSource).Rows.Count > 0)
        flag11 = true;
      bool flag12 = this.MyBus.MyDeviceList != null && this.MyBus.MyDeviceList.SelectedDevice != null;
      bool flag13 = flag12 && this.MyBus.MyDeviceList.SelectedDevice is MBusDevice;
      bool flag14 = flag12 && this.MyBus.MyDeviceList.SelectedDevice is Serie3MBus;
      bool flag15 = flag12 && this.MyBus.MyDeviceList.SelectedDevice is Serie2MBus && !flag14;
      bool flag16 = flag12 && this.MyBus.MyDeviceList.SelectedDevice is Serie1MBus && !flag15 && !flag14;
      bool flag17 = flag16 | flag15;
      bool flag18 = flag15 | flag14;
      bool flag19 = flag16 | flag15 | flag14;
      bool flag20 = this.dataGridViewAllParameter.Rows.Count > 0;
      bool flag21 = this.MyBus.MyDeviceList != null && this.MyBus.MyDeviceList.MBusConverterAvailable;
      this.menuFile.Enabled = true;
      this.menuBus.Enabled = true;
      this.menuDevice.Enabled = true;
      this.menuRadio.Enabled = true;
      this.menuRead.Enabled = true;
      this.menuFunction.Enabled = true;
      this.menuTest.Enabled = true;
      this.menuComponent.Enabled = true;
      this.menuHelp.Enabled = true;
      this.menuBus.Visible = flag3 | flag4 | flag5 | flag7 | flag6 | flag8 | flag9;
      this.menuDevice.Visible = flag3 | flag4 | flag5;
      this.menuRadio.Visible = false;
      this.menuFunction.Visible = flag1;
      this.menuTest.Visible = flag1;
      this.menuWriteBusinfo.Enabled = !flag2;
      this.menuReadBusinfo.Enabled = !flag2;
      this.menuItemLoadZDF_File.Enabled = !flag2;
      this.menuItemExportDataTable.Enabled = !flag2 & flag20;
      this.menuItemExportExcelDataTable.Enabled = !flag2 & flag20;
      this.menuItemSetupProfiles.Visible = flag1;
      this.menuItem11.Visible = flag1;
      this.menuDeleteBusInfo.Enabled = flag11 | flag6;
      this.menuDeleteDeviceFromBus.Enabled = flag11 & flag12;
      this.menuScanByAddress.Enabled = ((flag3 ? 1 : (!flag4 ? 0 : (!flag11 ? 1 : 0))) | (flag6 ? 1 : 0) | (flag8 ? 1 : 0) | (flag9 ? 1 : 0)) != 0;
      this.menuScanBySerialNumber.Enabled = flag3;
      this.menuSearchSingleDeviceByAddress.Enabled = ((flag3 ? 1 : (!flag4 ? 0 : (!flag11 ? 1 : 0))) | (flag6 ? 1 : 0)) != 0;
      this.menuSearchSingleDeviceBySerialNumber.Enabled = flag3 | flag5 | flag6;
      this.menuAddDeviceByParameter.Enabled = flag3 | flag5 || flag4 && !flag11 || flag7 && !flag11;
      this.menuSelectDeviceByPrimaryAddress.Enabled = flag3 & flag11;
      this.menuItemShowInfo.Visible = flag10;
      this.menuSetPrimaryAddress.Visible = flag3 | flag4;
      this.menuSetPrimaryAddress.Enabled = flag12;
      this.menuOrganize.Visible = flag3;
      this.menuOrganize.Enabled = flag11;
      this.menuItemSelectParameterListLine.Visible = flag17 | flag14 & flag3;
      this.menuItemSetSelectedDeviceToDefaultParameterList.Visible = flag17;
      this.menuItemSetSelectedDeviceToFullParameterList.Visible = flag17;
      this.menuItemSelectParameterList.Visible = flag14 & flag3;
      this.menuItem23.Visible = flag3;
      this.menuItemSetAllDevicesToDefaultParameterList.Visible = flag3;
      this.menuItemSetAllDevicesToDefaultParameterList.Enabled = flag11;
      this.menuItemSetAllDevicesToFullParameterList.Visible = flag3;
      this.menuItemSetAllDevicesToFullParameterList.Enabled = flag11;
      bool flag22 = flag3 | flag4;
      bool flag23 = flag22 & flag12;
      this.menuLineBaud.Visible = flag22;
      this.menuSetSelectedDeviceTo300_Baud.Visible = flag22;
      this.menuSetSelectedDeviceTo300_Baud.Enabled = flag23;
      this.menuSetSelectedDeviceTo2400_Baud.Visible = flag22;
      this.menuSetSelectedDeviceTo2400_Baud.Enabled = flag23;
      this.menuSetSelectedDeviceTo9600_Baud.Visible = flag22;
      this.menuSetSelectedDeviceTo9600_Baud.Enabled = flag23;
      this.menuSetSelectedDeviceTo38400_Baud.Visible = flag22;
      this.menuSetSelectedDeviceTo38400_Baud.Enabled = flag23;
      this.menuItemMBusConverterLine.Visible = flag21;
      this.menuItemMBusConverter.Visible = flag21;
      this.menuReadDeviceParameter.Enabled = flag12;
      this.menuReadAll.Visible = flag3 | flag5;
      this.menuReadAll.Enabled = flag12;
      this.menuRequestLoop.Enabled = flag12;
      this.menuItemRequestLoopAll.Visible = flag3;
      this.menuItemRequestLoopAll.Enabled = flag3 && this.MyBus.MyDeviceList.bus != null && this.MyBus.MyDeviceList.bus.Count > 0;
      this.menuSerchBaudrate.Visible = flag18 & flag1;
      this.menuShowAllParameters.Enabled = flag11;
      this.menuLineSerieX.Visible = flag18;
      this.menuGetVersion.Visible = flag18;
      this.menuGetVersionTestCycle.Visible = flag18;
      this.menuConnectAcrossBaudrates.Visible = flag18 & flag1;
      if (flag9 && this.MyBus.RadioReader.IsBusy)
      {
        this.buttonBreak.Enabled = !this.MyBus.BreakRequest;
        this.menuScanByAddress.Enabled = this.MyBus.BreakRequest;
      }
      this.menuCtReadMeterParameter.Enabled = this.menuReadDeviceParameter.Enabled;
      this.menuItemShowMeterData.Enabled = flag12;
      this.menuItemCmSetParameterListLine.Visible = this.menuItemSelectParameterListLine.Visible;
      this.menuCmSetToDefaultParameterList.Visible = this.menuItemSetSelectedDeviceToDefaultParameterList.Visible;
      this.menuCmSetToFullParameterList.Visible = this.menuItemSetSelectedDeviceToFullParameterList.Visible;
      this.menuCtSelectParameterList.Visible = this.menuItemSelectParameterList.Visible;
      this.menuCtSetPrimaryAddress.Visible = this.menuSetPrimaryAddress.Visible;
      this.menuCtDeleteFromBusinfo.Enabled = this.menuDeleteDeviceFromBus.Enabled;
      this.menuCtMBusConverterLine.Visible = this.menuItemMBusConverterLine.Visible;
      this.menuCtMBusConverter.Visible = this.menuItemMBusConverter.Visible;
      this.buttonToolbarReadDevice.Enabled = this.menuReadDeviceParameter.Enabled;
      this.buttonToolbarReadAllDevices.Enabled = this.menuReadAll.Enabled;
      this.buttonToolbarDeleteBusinfo.Enabled = this.menuDeleteBusInfo.Enabled;
      this.buttonToolbarScanBusByAddress.Enabled = this.menuScanByAddress.Enabled;
      this.buttonToolbarScanBusBySerialNumber.Enabled = this.menuScanBySerialNumber.Enabled;
      this.buttonToolbarSearchSingelDeviceByAddress.Enabled = this.menuSearchSingleDeviceByAddress.Enabled;
      this.buttonToolbarSearchSingelDeviceBySerialNumber.Enabled = this.menuSearchSingleDeviceBySerialNumber.Enabled;
      if (flag19 & flag1)
      {
        if (this.tabControl.TabPages.IndexOf(this.tabPageMemory) < 0)
          this.tabControl.TabPages.Add(this.tabPageMemory);
      }
      else if (this.tabControl.TabPages.IndexOf(this.tabPageMemory) >= 0)
        this.tabControl.TabPages.RemoveAt(this.tabControl.TabPages.IndexOf(this.tabPageMemory));
      this.groupBoxLogger.Visible = flag1;
      this.groupBoxRadioReceiverSetup.Visible = false;
      this.groupBoxRadioReadoutSystem.Visible = false;
      this.checkBoxLoggToFile.Enabled = !flag2;
      this.checkBoxLoggToZDF_File.Enabled = !flag2;
      this.buttonReadAll.Visible = this.menuReadAll.Enabled;
      if (flag12)
      {
        if (this.MyBus.MyDeviceList.SelectedDevice is Serie3MBus)
        {
          this.buttonEraseFlash.Visible = true;
          this.listBoxLocation.Visible = false;
          this.listBoxLocation.SelectedIndex = 1;
          this.labelLocation.Visible = false;
        }
        else
        {
          this.buttonEraseFlash.Visible = false;
          this.listBoxLocation.Visible = true;
          this.labelLocation.Visible = false;
        }
      }
      else
        this.buttonEraseFlash.Visible = false;
    }

    private void SetLoopConditions()
    {
      bool flag = this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.MinomatRadioTest;
      this.Cursor = Cursors.WaitCursor;
      this.buttonToolbarReadDevice.Enabled = false;
      this.buttonToolbarReadAllDevices.Enabled = false;
      this.buttonToolbarDeleteBusinfo.Enabled = false;
      this.buttonToolbarScanBusByAddress.Enabled = false;
      this.buttonToolbarScanBusBySerialNumber.Enabled = false;
      this.buttonToolbarSearchSingelDeviceByAddress.Enabled = false;
      this.buttonToolbarSearchSingelDeviceBySerialNumber.Enabled = false;
      this.buttonBreak.Enabled = true;
      for (int index = 0; index < this.SerialBusMenu.MenuItems.Count; ++index)
        this.SerialBusMenu.MenuItems[index].Enabled = false;
      if (!flag)
      {
        this.dataGridBusTable.Enabled = false;
        this.ControlBox = false;
        this.tabPageBusInfo.ContextMenu = (ContextMenu) null;
      }
      this.LoopIsRunning = true;
      this.MyBus.BreakRequest = false;
      this.buttonOk.Enabled = false;
      Application.DoEvents();
    }

    private void ResetLoopConditions()
    {
      bool flag = this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.MinomatRadioTest;
      this.LoopIsRunning = false;
      this.labelStatus.Text = "";
      this.tabPageBusInfo.ContextMenu = this.contextMenu1;
      this.ControlBox = true;
      this.dataGridBusTable.Enabled = true;
      this.SetEnabledFunctions();
      this.buttonBreak.Enabled = false;
      this.Cursor = Cursors.Default;
      if (!flag)
        this.MyBus.BreakRequest = false;
      this.buttonOk.Enabled = true;
    }

    private void menuSchnittstellenFehlerLoop_Click(object sender, EventArgs e)
    {
      this.MyBus.BreakRequest = false;
      int num1 = 0;
      this.LoopIsRunning = true;
      Random random = new Random();
      ArrayList ParameterList = new ArrayList();
      int index1 = -1;
      bool flag = true;
      ByteField DataBlock = new ByteField();
      this.MyBus.MyCom.GetCommParameter(ref ParameterList);
      for (int index2 = 0; index2 < ParameterList.Count; index2 += 2)
      {
        if (ParameterList[index2].ToString() == "Baudrate")
        {
          index1 = index2 + 1;
          break;
        }
      }
      if (ParameterList[index1].ToString() == "2400")
        flag = false;
      this.tabControl.SelectedTab = this.tabPageDeviceParameter;
      DeviceInfo Info;
      while (!this.MyBus.BreakRequest && this.MyBus.ReadParameter(out Info))
      {
        ++num1;
        this.textBoxSerialNr.Text = Info.MeterNumber;
        this.textBoxMedium.Text = Info.Medium.ToString();
        this.textBoxManufacturer.Text = Info.Manufacturer.ToString();
        Info.GenerateParameterTable();
        this.dataGridParameterList.DataSource = (object) Info.ParameterTable;
        Label labelStatus = this.labelStatus;
        int num2 = this.MyBus.GetJobCounter();
        string str1 = num2.ToString();
        num2 = this.MyBus.GetErrorCounter();
        string str2 = num2.ToString();
        string str3 = "Jobs:" + str1 + "; Errors:" + str2;
        labelStatus.Text = str3;
        this.progressBar.Value = 100;
        ParameterList[index1] = !flag ? (object) "9600" : (object) "2400";
        this.MyBus.MyCom.SetCommParameter(ParameterList);
        if (this.MyBus.MyCom.Open())
        {
          Thread.Sleep(random.Next(1000));
          if (DataBlock.Count > 50)
            DataBlock = new ByteField();
          DataBlock.Add(85);
          if (!this.MyBus.MyCom.TransmitBlock(ref DataBlock))
          {
            int num3 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Send errorblock error");
          }
          ParameterList[index1] = !flag ? (object) "2400" : (object) "9600";
          this.MyBus.MyCom.SetCommParameter(ParameterList);
          int num4 = int.Parse(this.textBoxLoopTime.Text) * 2;
          for (int index3 = 0; index3 < num4 && !this.MyBus.BreakRequest; ++index3)
          {
            this.progressBar.Value = 100 / num4 * (index3 + 1);
            Thread.Sleep(500);
            Application.DoEvents();
          }
          this.Refresh();
        }
        else
          break;
      }
      if (!this.MyBus.BreakRequest)
      {
        int num5 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("2"));
      }
      this.ClearFields();
      this.LoopIsRunning = false;
    }

    private void menuItemReadEEPromLoop_Click(object sender, EventArgs e)
    {
      this.MyBus.OnMessage -= this.EventObject;
      this.SetLoopConditions();
      this.ClearFields();
      this.tabControl.SelectedTab = this.tabPageDeviceParameter;
      this.MyBus.BreakRequest = false;
      int MaxRepeat = this.MyBus.SetMaxRepeat(1);
      this.MyBus.ClearCounters();
      this.MyBus.StartTestloopReadEEProm();
      while (!this.MyBus.BreakRequest)
      {
        ++this.InfoCounter;
        this.labelStatus.Text = "Jobs:" + this.MyBus.GetJobCounter().ToString() + "; Errors:" + this.MyBus.GetErrorCounter().ToString();
        Application.DoEvents();
      }
      this.MyBus.StopTestLoop();
      this.MyBus.SetMaxRepeat(MaxRepeat);
      this.ResetLoopConditions();
      this.MyBus.OnMessage += this.EventObject;
      this.labelStatus.Text = "";
    }

    private void menuItem6_Click(object sender, EventArgs e)
    {
      ArrayList FullNames = new ArrayList();
      Assembly assembly = Assembly.GetAssembly(typeof (DeviceCollectorWindow));
      FullNames.Add((object) assembly.FullName);
      foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
        FullNames.Add((object) referencedAssembly.FullName);
      for (int index1 = 0; index1 < FullNames.Count; ++index1)
      {
        for (int index2 = index1 + 1; index2 < FullNames.Count; ++index2)
        {
          if (FullNames[index1].ToString() == FullNames[index2].ToString())
          {
            FullNames.RemoveAt(index2);
            --index2;
          }
        }
      }
      FullNames.Sort();
      int num = (int) new ZR_About(FullNames).ShowDialog();
    }

    private void menuStartAsyncCom_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "AsyncCom";
      this.Hide();
    }

    private void menuStartHandler_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "Handler";
      this.Hide();
    }

    private void menuStartDesigner_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "Designer";
      this.Hide();
    }

    private void menuItemConfigurator_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "Configurator";
      this.Hide();
    }

    private void menuItemStartWindow_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "StartWindow";
      this.Hide();
    }

    private void menuStartGlobalMeterManager_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "GMM";
      this.Hide();
    }

    private void menuItemBack_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "";
      this.Hide();
    }

    private void menuItemQuit_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "Exit";
      this.Hide();
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "";
      this.Hide();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.StopLoop();
      this.StartComponentName = "";
      this.Hide();
    }

    private void SerialBus_Activated(object sender, EventArgs e)
    {
      this.textBoxBusFilePath.Text = this.MyBus.MyBusInfo.BusInfoFilename;
    }

    private void SerialBus_Load(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.RefreshBusInfo();
      this.IsRunning = true;
      this.tabControl.SelectedTab = this.tabPageBusInfo;
    }

    private void SerialBus_Closing(object sender, CancelEventArgs e)
    {
      if (this.BusinfoIsManualChanged && GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("3"), MessageBoxButtons.OKCancel) != DialogResult.Cancel)
        this.WriteBusinfo();
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("DeviceCollector", "ZDF_FileName", this.textBoxZDF_FileName.Text);
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("DeviceCollector", "LoggZDF_File", this.checkBoxLoggToZDF_File.Checked.ToString());
    }

    private void SerialBus_Closed(object sender, EventArgs e)
    {
      this.StopLoop();
      this.Hide();
    }

    private void StopLoop()
    {
      this.MyBus.BreakRequest = true;
      if (this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.MinomatRadioTest)
        return;
      if (this.LoopIsRunning)
        Thread.Sleep(800);
      this.MyBus.BreakRequest = false;
    }

    private void menuMeterMonitor_Click(object sender, EventArgs e)
    {
      this.ClearFields();
      this.tabControl.SelectedTab = this.tabPageDeviceParameter;
      this.MyBus.BreakRequest = false;
      this.MyBus.StartMeterMonitor(5);
      int num = 0;
      ByteField MonitorData;
      while (!this.MyBus.BreakRequest && this.MyBus.GetMeterMonitorData(out MonitorData))
      {
        if (MonitorData.Count > 30)
        {
          for (int index = 0; (int) MonitorData.Data[index] == (int) MonitorData.Data[index + 3] - 1; index += 3)
          {
            if (index > 30)
            {
              num = 0;
              break;
            }
          }
          for (int index = 1; (int) MonitorData.Data[index] == (int) MonitorData.Data[index + 3] - 1; index += 3)
          {
            if (index > 30)
            {
              num = 2;
              break;
            }
          }
          for (int index = 2; (int) MonitorData.Data[index] == (int) MonitorData.Data[index + 3] - 1; index += 3)
          {
            if (index > 30)
            {
              num = 1;
              break;
            }
          }
        }
        for (int index = 0; index < MonitorData.Count; ++index)
        {
          switch (num)
          {
            case 0:
              num = 1;
              break;
            case 1:
              num = 2;
              break;
            default:
              num = 0;
              break;
          }
        }
        Application.DoEvents();
      }
    }

    private void buttonDeleteMeterKey_Click(object sender, EventArgs e)
    {
      if (this.MyBus.DeleteMeterKey(this.GetHexDecValue(this.textBoxValue.Text)))
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Delete MeterKey ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Delete MeterKey error!");
      }
    }

    private void buttonSetBaudrate_Click(object sender, EventArgs e)
    {
      if (this.MyBus.SetBaudrate(this.GetHexDecValue(this.textBoxValue.Text)))
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Baudrate change ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Baudrate change error!");
      }
    }

    private void buttonWriteBitfield_Click(object sender, EventArgs e)
    {
      if (this.MyBus.WriteBitfield(this.GetHexDecValue(this.textBoxStartAddress.Text), (byte) this.GetHexDecValue(this.textBoxAndMask.Text), (byte) this.GetHexDecValue(this.textBoxOrMask.Text)))
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Write Bitfield ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Write Bitfield error");
      }
    }

    private void menuWriteBusinfo_Click(object sender, EventArgs e) => this.WriteBusinfo();

    private void WriteBusinfo()
    {
      if (!this.MyBus.MyBusInfo.SelectBusinfoSaveFilename())
        return;
      this.MyBus.WriteBusInfo(this.MyBus.MyBusInfo.BusInfoFilename);
      this.textBoxBusFilePath.Text = this.MyBus.MyBusInfo.BusInfoFilename;
      this.BusinfoIsManualChanged = false;
    }

    private void menuReadBusinfo_Click(object sender, EventArgs e)
    {
      this.MyBus.BreakRequest = true;
      if (!this.MyBus.MyBusInfo.SelectBusinfoOpenFilename())
        return;
      this.MyBus.ReadBusInfo(this.MyBus.MyBusInfo.BusInfoFilename);
      this.RefreshBusInfo();
      this.SetEnabledFunctions();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void menuDeleteBusInfo_Click(object sender, EventArgs e) => this.DeleteBusinfo();

    private void DeleteBusinfo()
    {
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.MyBus.DeleteBusInfo();
      this.RefreshBusInfo();
      this.Refresh();
    }

    private void menuAddDeviceByParameter_Click(object sender, EventArgs e)
    {
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      int num = (int) new AddDevice(this.MyBus).ShowDialog();
      this.RefreshBusInfo();
    }

    private void menuDeleteDeviceFromBus_Click(object sender, EventArgs e)
    {
      if (this.MyBus.MyDeviceList == null)
        return;
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.MyBus.MyDeviceList.DeleteSelectedDevice();
      this.RefreshBusInfo();
    }

    private void menuCtDeleteFromBusinfo_Click(object sender, EventArgs e)
    {
      if (this.MyBus.MyDeviceList == null)
        return;
      this.MyBus.MyDeviceList.DeleteSelectedDevice();
      this.RefreshBusInfo();
    }

    private void menuSearchSingleDeviceByAddress_Click(object sender, EventArgs e)
    {
      this.SearchSingleDeviceByAddress();
    }

    private void menuSearchSingleDeviceBySerialNumber_Click(object sender, EventArgs e)
    {
      this.SearchSingleDeviceBySerialNumber();
    }

    private void menuSelectDeviceByPrimaryAddress_Click(object sender, EventArgs e)
    {
    }

    private void menuScanByAddress_Click(object sender, EventArgs e) => this.ScanByAddress();

    private void ReadAll()
    {
      if (this.MyBus.MyDeviceList == null)
        return;
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageAllParameter;
      int num = 0;
      string str = "/" + this.MyBus.MyDeviceList.bus.Count.ToString();
      while (!this.MyBus.BreakRequest && this.MyBus.MyDeviceList.SelectDeviceByIndex(num++))
      {
        DeviceInfo Info;
        bool flag = this.MyBus.ReadParameter(out Info);
        this.AddToZDF_ParameterTable(Info);
        this.labelStatus.Text = "Read all: " + num.ToString() + str;
        if (!flag)
          this.labelStatus.Text += " !!! Read error !!! ";
        this.RefreshBusInfo();
      }
      this.ResetLoopConditions();
    }

    private void ScanByAddress()
    {
      if (this.MyBus.MyDeviceList == null)
        return;
      ZR_ClassLibMessages.ClearErrors();
      this.checkBoxOnlySecondaryAddressing.Checked = false;
      this.BusinfoIsManualChanged = true;
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      if (this.MyBus.MyBusMode == BusMode.MBusPointToPoint)
      {
        this.MyBus.MyDeviceList.DeleteBusList();
        this.MyBus.MyCom.ResetEarliestTransmitTime();
        this.MyBus.MyDeviceList.SearchSingleDeviceByPrimaryAddress(254);
      }
      else if (this.MyBus.MyBusMode == BusMode.MinomatV2)
      {
        if (!((MinomatList) this.MyBus.MyDeviceList).ReadMinomat())
          ZR_ClassLibMessages.ShowAndClearErrors("Scan by Address", "Minomat readout failed.");
      }
      else if (this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.MinomatRadioTest)
      {
        if (!((RadioList) this.MyBus.MyDeviceList).ReadRadio())
          ZR_ClassLibMessages.ShowAndClearErrors("Read radio", "Radio reading failed.");
        this.MyBus.BreakRequest = true;
        this.BusinfoIsManualChanged = false;
        this.SetEnabledFunctions();
      }
      else
      {
        int ScanAddress = 0;
        bool flag = true;
        try
        {
          ScanAddress = int.Parse(this.textBoxScanStartAddress.Text);
          if (ScanAddress >= 0 && ScanAddress < 252)
            flag = false;
        }
        catch
        {
        }
        if (flag)
        {
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("4"));
          return;
        }
        this.MyBus.SetMessageInfo("Scanning: ");
        this.MyBus.MyDeviceList.ScanFromAddress(ScanAddress);
        this.progressBar.Value = 0;
      }
      this.RefreshBusInfo();
      this.MyBus.SetMessageInfo("");
      this.ResetLoopConditions();
    }

    private void ScanBySerialNumber()
    {
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.MyBus.SetMessageInfo("Scanning: ");
      this.Cursor = Cursors.WaitCursor;
      this.BusinfoIsManualChanged = true;
      this.MyBus.MyDeviceList.ScanFromSerialNumber(this.textBoxScanStartNumber.Text);
      this.RefreshBusInfo();
      this.MyBus.SetMessageInfo("");
      this.ResetLoopConditions();
    }

    private void SearchSingleDeviceByAddress()
    {
      this.checkBoxOnlySecondaryAddressing.Checked = false;
      this.BusinfoIsManualChanged = true;
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      AddressReader addressReader = new AddressReader(AddressReader.AddressNode.ShortAdr);
      if (addressReader.ShowDialog() == DialogResult.OK)
      {
        if (this.MyBus.SearchSingleDeviceByPrimaryAddress(addressReader.Address))
        {
          this.RefreshBusInfo();
          this.Refresh();
        }
        else
        {
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("1"));
        }
      }
      this.ResetLoopConditions();
    }

    private void SearchSingleDeviceBySerialNumber()
    {
      this.BusinfoIsManualChanged = true;
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      AddressReader addressReader = new AddressReader(AddressReader.AddressNode.SerialNrAndWildcard);
      if (addressReader.ShowDialog() == DialogResult.OK && !this.MyBus.SearchSingleDeviceBySerialNumber(addressReader.AddressString))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(DeviceCollectorFunctions.SerialBusMessage.GetString("1"));
        if (ZR_ClassLibMessages.GetLastError() != 0)
          stringBuilder.AppendLine(ZR_ClassLibMessages.GetLastErrorStringTranslated());
        ZR_ClassLibMessages.ClearErrors();
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", stringBuilder.ToString());
      }
      this.RefreshBusInfo();
      this.ResetLoopConditions();
    }

    private void Organize()
    {
      this.BusinfoIsManualChanged = true;
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.MyBus.MyDeviceList.OrganizeBus(int.Parse(this.textBoxOrganizeStartAddress.Text));
      this.RefreshBusInfo();
      this.Cursor = Cursors.Default;
      this.labelStatus.Text = string.Empty;
      this.ResetLoopConditions();
    }

    private void SetPrimaryAddress()
    {
      this.BusinfoIsManualChanged = true;
      this.SetLoopConditions();
      AddressReader addressReader = new AddressReader(AddressReader.AddressNode.ShortAdr);
      if (addressReader.ShowDialog() == DialogResult.OK)
      {
        if (!this.checkBoxKeepDestinationAddress.Checked ? this.MyBus.SetPrimaryAddress(addressReader.Address) : this.MyBus.SetPrimaryAddressWithoutShift(addressReader.Address))
        {
          this.RefreshBusInfo();
          this.Refresh();
        }
        else
        {
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("5"));
        }
      }
      this.ResetLoopConditions();
    }

    private void menuGetVersion_Click(object sender, EventArgs e) => this.GetVersion((int[]) null);

    private void menuConnectAcrossBaudrates_Click(object sender, EventArgs e)
    {
      this.GetVersion(Serie2MBus.AllBaudrates);
    }

    private void GetVersion(int[] TestBaudrates)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.SetLoopConditions();
      this.Cursor = Cursors.WaitCursor;
      this.labelStatus.Text = "Read version";
      if (this.MyBus.MyDeviceList.SelectedDevice is Serie3MBus)
      {
        ReadVersionData versionData;
        bool flag = this.MyBus.ReadVersion(out versionData);
        this.Cursor = Cursors.Default;
        this.labelStatus.Text = "Off";
        if (flag)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("Manufacturer =  " + versionData.ManufacturerString);
          stringBuilder.AppendLine("Medium = " + versionData.MBusMediumString);
          stringBuilder.AppendLine("MBusGeneration = 0x" + versionData.MBusGeneration.ToString("x2"));
          stringBuilder.AppendLine("Version = " + versionData.GetVersionString());
          stringBuilder.AppendLine("MBusSerialNr = " + versionData.MBusSerialNr.ToString("d08"));
          if (versionData.BuildTime != DateTime.MinValue)
          {
            stringBuilder.AppendLine("BuildRevision = " + versionData.BuildRevision.ToString());
            stringBuilder.AppendLine("BuildTime = " + versionData.BuildTime.ToShortDateString() + " " + versionData.BuildTime.ToShortTimeString());
            stringBuilder.AppendLine("Signature = 0x" + versionData.FirmwareSignature.ToString("x04"));
          }
          stringBuilder.AppendLine("Hardware = " + versionData.HardwareIdentificationString);
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", stringBuilder.ToString());
        }
        else
          ZR_ClassLibMessages.ShowAndClearErrors("DeviceCollector", "ReadVersion Error");
      }
      else
      {
        short Manufacturer;
        byte Medium;
        byte MBusMeterType;
        long Version;
        int MBusSerialNr;
        int HardwareTypeId;
        int HardwareMask;
        bool flag = this.MyBus.ReadVersion(TestBaudrates, out Manufacturer, out Medium, out MBusMeterType, out Version, out MBusSerialNr, out HardwareTypeId, out HardwareMask);
        string str1 = this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "");
        this.Cursor = Cursors.Default;
        this.labelStatus.Text = "Off";
        if (flag)
        {
          string str2 = "Manufacturer  = " + MBusDevice.GetManufacturer(Manufacturer) + "\r\nMedium = ...... 0x" + Medium.ToString("x2") + "\r\nMBusMeterType = 0x" + MBusMeterType.ToString("x2");
          int count = this.MyBus.MyDeviceList.SelectedDevice.ReceiveBuffer.Count;
          string MessageString = str2 + "\r\nVersion = ..... " + ParameterService.GetVersionString(Version, count) + "\r\nMBusSerialNr  = " + MBusSerialNr.ToString("d8") + "\r\nBaudrate = .... " + str1.ToString();
          if (Version >= 67108864L)
            MessageString = MessageString + "\r\nHardwareTypeId = 0x" + HardwareTypeId.ToString("x08") + "\r\nHardware = " + ParameterService.GetHardwareString((uint) HardwareMask);
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", MessageString);
        }
        else
          ZR_ClassLibMessages.ShowAndClearErrors("DeviceCollector", "ReadVersion Error");
      }
      this.ResetLoopConditions();
    }

    private void menuGetVersionTestCycle_Click(object sender, EventArgs e)
    {
      this.SetLoopConditions();
      this.ClearFields();
      this.tabControl.Enabled = false;
      string text = this.textBoxLoopTime.Text;
      this.textBoxLoopTime.Text = "0";
      this.MyBus.SetMessageInfo("Loop; ");
      int num = 0;
      while (!this.MyBus.BreakRequest)
      {
        string str = "Read version loop." + this.GetCounterStatus();
        this.MyBus.SetMessageInfo(str + ": ");
        this.labelStatus.Text = str;
        this.Refresh();
        this.progressBar.Value = num * 10 % 100 + 10;
        if (this.MyBus.ReadVersion((int[]) null, out short _, out byte _, out byte _, out long _, out int _))
          ++this.InfoCounter;
        else
          ++this.ErrorCounter;
        ++num;
        Application.DoEvents();
      }
      this.textBoxLoopTime.Text = text;
      this.MyBus.SetMessageInfo("");
      this.progressBar.Value = 0;
      this.tabControl.Enabled = true;
      this.ResetLoopConditions();
    }

    private void menuScanBySerialNumber_Click(object sender, EventArgs e)
    {
      this.ScanBySerialNumber();
    }

    private void menuSetPrimaryAddress_Click(object sender, EventArgs e)
    {
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.SetPrimaryAddress();
    }

    private void menuCtSetPrimaryAddress_Click(object sender, EventArgs e)
    {
      this.SetPrimaryAddress();
    }

    private void menuReadDeviceParameter_Click(object sender, EventArgs e)
    {
      this.ReadDeviceParameter(true);
    }

    private void menuCtReadMeterParameter_Click(object sender, EventArgs e)
    {
      this.ReadDeviceParameter(true);
    }

    private bool ReadDeviceParameter(bool ShowErrorMessages)
    {
      if (this.ReadActive)
        return false;
      this.ReadActive = true;
      bool flag1 = false;
      ZR_ClassLibMessages.ClearErrors();
      if (this.MyBus.MyDeviceList.SelectedDevice != null && this.MyBus.MyDeviceList.SelectedDevice is MBusDevice)
      {
        MBusDevice selectedDevice = (MBusDevice) this.MyBus.MyDeviceList.SelectedDevice;
        selectedDevice.followingTelegrammAnnounced = false;
        selectedDevice.followingTelegrammTransmit_FCB_Odd = !this.MyBus.UseREQ_UD2_5B;
      }
      try
      {
        if (this.MyBus.MyBusMode == BusMode.MBusPointToPoint)
        {
          if (this.MyBus.MyDeviceList.bus.Count == 0)
            this.ScanByAddress();
        }
        else if (this.MyBus.MyBusMode == BusMode.RelayDevice)
        {
          this.tabControl.SelectedTab = this.tabPageAllParameter;
          List<DeviceInfo> parameters = this.MyBus.GetParameters();
          if (parameters != null)
          {
            foreach (DeviceInfo DevInfo in parameters)
            {
              this.AddToSingleDeviceParameterTable(DevInfo);
              this.AddToZDF_ParameterTable(DevInfo);
            }
          }
          flag1 = true;
          goto label_33;
        }
        if (this.MyBus.MyDeviceList.bus.Count == 0)
        {
          if (ShowErrorMessages)
          {
            int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("1"));
          }
        }
        else
        {
          this.dataGridParameterList.DataSource = (object) null;
          bool breakRequest = this.MyBus.BreakRequest;
          this.SetLoopConditions();
          DeviceInfo Info;
          bool flag2 = this.MyBus.ReadParameter(out Info);
          bool flag3 = this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.MinomatRadioTest;
          if (flag3)
            this.MyBus.BreakRequest = breakRequest;
          if (!flag2)
          {
            this.ClearFields();
          }
          else
          {
            this.AddToSingleDeviceParameterTable(Info);
            if (!flag3)
              this.AddToZDF_ParameterTable(Info);
            this.tabControl.SelectedTab = this.tabPageDeviceParameter;
            this.ResetLoopConditions();
            this.labelStatus.Text = "Read ok";
            if (this.MyBus.BeepSignalForReadResult)
              SystemSounds.Beep.Play();
            flag1 = true;
            goto label_33;
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
      }
      this.ResetLoopConditions();
      this.labelStatus.Text = "Read error";
      if (this.MyBus.BeepSignalForReadResult)
        SystemSounds.Hand.Play();
      if (ShowErrorMessages)
      {
        ZR_ClassLibMessages.AddErrorDescription(DeviceCollectorFunctions.SerialBusMessage.GetString("2"));
        ZR_ClassLibMessages.ShowAndClearErrors("DeviceCollector");
      }
label_33:
      this.RefreshBusInfo();
      this.ReadActive = false;
      return flag1;
    }

    private void menuItemShowMeterData_Click(object sender, EventArgs e) => this.ShowMeterData();

    private void ShowMeterData()
    {
      if (this.MyBus.MyDeviceList.SelectedDevice.Info == null)
        return;
      this.tabControl.SelectedTab = this.tabPageDeviceParameter;
      this.AddToSingleDeviceParameterTable(this.MyBus.MyDeviceList.SelectedDevice.Info);
      this.AddToZDF_ParameterTable(this.MyBus.MyDeviceList.SelectedDevice.Info);
    }

    private void AddToSingleDeviceParameterTable(DeviceInfo DevInfo)
    {
      if (DevInfo == null)
        return;
      this.textBoxSerialNr.Text = DevInfo.MeterNumber;
      this.textBoxMedium.Text = DevInfo.Medium.ToString();
      this.textBoxManufacturer.Text = DevInfo.Manufacturer.ToString();
      DevInfo.GenerateParameterTable();
      this.dataGridParameterList.DataSource = (object) DevInfo.ParameterTable;
      this.Refresh();
    }

    private void menuReset_Click(object sender, EventArgs e)
    {
      if (this.MyBus.ResetDevice())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Reset ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Reset not ok");
      }
    }

    private void menuRunBackup_Click(object sender, EventArgs e)
    {
      if (this.MyBus.RunBackup())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Backup ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Backup error");
      }
    }

    private void menuSetEmergencyMode_Click(object sender, EventArgs e)
    {
      if (this.MyBus.SetEmergencyMode())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "EmergencyMode ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "EmergencyMode error");
      }
    }

    private void menuSerchBaudrate_Click(object sender, EventArgs e)
    {
      if (this.MyBus.SerchDeviceAcrossBaudrates())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Baudrate ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Device not found");
      }
    }

    private void menuEEPromWriteReadLoop_Click(object sender, EventArgs e)
    {
      this.ClearFields();
      this.tabControl.SelectedTab = this.tabPageDeviceParameter;
      this.SetLoopConditions();
      int MaxRepeat = this.MyBus.SetMaxRepeat(1);
      this.MyBus.ClearCounters();
      this.MyBus.StartTestloopWriteReadEEProm();
      this.Cursor = Cursors.WaitCursor;
      while (!this.MyBus.BreakRequest)
      {
        Label labelStatus = this.labelStatus;
        int num = this.MyBus.GetJobCounter();
        string str1 = num.ToString();
        num = this.MyBus.GetErrorCounter();
        string str2 = num.ToString();
        string str3 = "Jobs:" + str1 + "; Errors:" + str2;
        labelStatus.Text = str3;
        Application.DoEvents();
      }
      this.MyBus.StopTestLoop();
      this.MyBus.SetMaxRepeat(MaxRepeat);
      this.ResetLoopConditions();
      this.labelStatus.Text = "";
    }

    private void buttonBreak_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyBus.BreakRequest = true;
      this.progressBar.Value = 0;
      this.SetEnabledFunctions();
      this.labelStatus.Text = string.Empty;
      this.Cursor = Cursors.Default;
    }

    private void buttonReadDevice_Click(object sender, EventArgs e)
    {
      this.ReadDeviceParameter(true);
    }

    private void buttonReadAllDevices_Click(object sender, EventArgs e) => this.ReadAll();

    private void buttonToolbarDeleteBusinfo_Click(object sender, EventArgs e)
    {
      this.DeleteBusinfo();
    }

    private void buttonToolbarScanBusByAddress_Click(object sender, EventArgs e)
    {
      this.ScanByAddress();
    }

    private void buttonToolbarScanBusBySerialNumber_Click(object sender, EventArgs e)
    {
      this.ScanBySerialNumber();
    }

    private void buttonToolbarSearchSingelDeviceByAddress_Click(object sender, EventArgs e)
    {
      this.SearchSingleDeviceByAddress();
    }

    private void buttonToolbarSearchSingelDeviceBySerialNumber_Click(object sender, EventArgs e)
    {
      this.SearchSingleDeviceBySerialNumber();
    }

    private void menuItemIO_Test_Click(object sender, EventArgs e)
    {
      int num = (int) new IO_Test(this.MyBus).ShowDialog();
    }

    private void buttonShowAll_Click(object sender, EventArgs e)
    {
      this.textBoxAllParameters.Text = this.MyBus.MyDeviceList.GetAllParameters();
      this.NewAllParameterGrid(this.textBoxAllParameters.Text);
    }

    private void menuShowAllParameters_Click(object sender, EventArgs e)
    {
      this.tabControl.SelectedTab = this.tabPageAllParameter;
      this.textBoxAllParameters.Text = this.MyBus.MyDeviceList.GetAllParameters();
      this.NewAllParameterGrid(this.textBoxAllParameters.Text);
    }

    private void menuReadAll_Click(object sender, EventArgs e) => this.ReadAll();

    private void buttonReadAll_Click(object sender, EventArgs e) => this.ReadAll();

    private void AddToZDF_ParameterTable(DeviceInfo DevInfo)
    {
      if (DevInfo == null || !string.IsNullOrEmpty(this.txtFilterBySerialNumber.Text) && !DevInfo.MeterNumber.StartsWith(this.txtFilterBySerialNumber.Text))
        return;
      string zdfParameterString1 = DevInfo.GetZDFParameterString();
      this.AddAllParameter(zdfParameterString1);
      foreach (DeviceInfo subDevice in DevInfo.SubDevices)
        this.AddAllParameter(subDevice.GetZDFParameterString());
      if (this.checkBoxLoggToZDF_File.Checked)
      {
        string path = Path.Combine(Application.StartupPath, this.textBoxZDF_FileName.Text);
        try
        {
          using (StreamWriter streamWriter = File.AppendText(path))
          {
            streamWriter.WriteLine(zdfParameterString1);
            foreach (DeviceInfo subDevice in DevInfo.SubDevices)
            {
              string zdfParameterString2 = subDevice.GetZDFParameterString();
              streamWriter.WriteLine(zdfParameterString2);
            }
          }
        }
        catch (Exception ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          DeviceCollectorWindow.logger.Error(ex, message);
        }
      }
    }

    private void menuStartReceiver_Click(object sender, EventArgs e) => this.StartReceiver();

    private void StartReceiver()
    {
      try
      {
        this.MyBus.MyReceiver.ReceiverLevel = int.Parse(this.textBoxReceiveLevel.Text);
      }
      catch
      {
      }
      if (!this.MyBus.StartReceiver())
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Receiver start error");
      }
      else
      {
        this.tabControl.SelectedTab = this.tabPageDeviceParameter;
        this.labelReadoutSystem.Text = this.MyBus.MyReceiver.ReceiverType;
        this.labelReadoutSystemVersion.Text = this.MyBus.MyReceiver.ReceiverVersion;
        this.groupBoxRadioReadoutSystem.Visible = true;
        this.MyBus.BreakRequest = false;
        while (!this.MyBus.BreakRequest)
        {
          DeviceInfo Info;
          if (this.MyBus.ReadParameter(out Info))
          {
            this.textBoxSerialNr.Text = Info.MeterNumber;
            this.textBoxMedium.Text = Info.Medium.ToString();
            this.textBoxManufacturer.Text = Info.Manufacturer;
            Info.GenerateParameterTable();
            this.AddAllParameter(Info.GetZDFParameterString());
            this.dataGridParameterList.DataSource = (object) Info.ParameterTable;
            this.RefreshBusInfo();
          }
        }
        this.MyBus.SendMessage("", 0, GMM_EventArgs.MessageType.EndMessage);
        this.MyBus.StopReceiver();
      }
    }

    private void menuStopReceiver_Click(object sender, EventArgs e)
    {
      if (this.MyBus.BreakRequest)
        this.MyBus.StopReceiver();
      else
        this.MyBus.BreakRequest = true;
    }

    private void buttonClearAll_Click(object sender, EventArgs e)
    {
      this.dataGridViewAllParameter.Columns.Clear();
      this.textBoxAllParameters.Text = "";
    }

    private void menuDeleteData_Click(object sender, EventArgs e)
    {
      this.MyBus.DeleteBusInfo();
      this.ClearTables();
      this.RefreshBusInfo();
      this.Refresh();
    }

    private void menuEEPromReset_Click(object sender, EventArgs e)
    {
      ByteField data = new ByteField(100);
      for (int index = 0; index < 100; ++index)
        data.Add(0);
      this.MyBus.WriteMemory(MemoryLocation.EEPROM, 0, data);
    }

    private void checkBoxWatchRange_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxWatchRange.Checked)
      {
        this.MyBus.MyDeviceList.SelectedDevice.WatchNumberOfBytes = this.GetHexDecValue(this.textBoxNumberOfBytes.Text);
        this.MyBus.MyDeviceList.SelectedDevice.WatchStartAddress = this.GetHexDecValue(this.textBoxStartAddress.Text);
        this.MyBus.MyDeviceList.SelectedDevice.WatchMemoryLocation = (MemoryLocation) this.listBoxLocation.SelectedIndex;
      }
      else
        this.MyBus.MyDeviceList.SelectedDevice.WatchNumberOfBytes = 0;
    }

    private bool AddAllParameter(string ZDF_String)
    {
      this.AddParameterToZDF_View(ZDF_String);
      this.AddParameterToDataGrid(ZDF_String, true);
      return true;
    }

    private bool AddParameterToZDF_View(string ZDF_String)
    {
      if (this.textBoxAllParameters.Text.Length > 10000)
        this.textBoxAllParameters.Text = this.textBoxAllParameters.Text.Substring(this.textBoxAllParameters.Text.IndexOf(Environment.NewLine, 5000) + Environment.NewLine.Length);
      if (this.textBoxAllParameters.Text.Length == 0)
        this.textBoxAllParameters.Text = ZDF_String;
      else
        this.textBoxAllParameters.AppendText(Environment.NewLine + ZDF_String);
      return true;
    }

    private bool AddParameterToDataGrid(string ZDF_String, bool RefreshGrid)
    {
      if (string.IsNullOrEmpty(ZDF_String))
        return false;
      string[] strArray = ZDF_String.Split(';');
      if (strArray.Length == 0 || strArray.Length % 2 != 0)
        return false;
      bool flag = false;
      if (this.dataGridViewAllParameter.Columns.Count == 0 || this.dataGridViewAllParameter.Columns.Count < strArray.Length / 2)
      {
        flag = true;
      }
      else
      {
        int index = 0;
        int num = 0;
        while (index < strArray.Length)
        {
          if (!this.dataGridViewAllParameter.Columns.Contains(strArray[index]))
          {
            flag = true;
            break;
          }
          index += 2;
          ++num;
        }
      }
      this.dataGridViewAllParameter.SuspendLayout();
      if (flag)
        this.dataGridViewAllParameter.ColumnHeadersVisible = false;
      this.dataGridViewAllParameter.AutoGenerateColumns = false;
      if (this.dataGridViewAllParameter.Columns.Count == 0)
        RefreshGrid = false;
      if (flag)
      {
        for (int index = 0; index < strArray.Length; index += 2)
        {
          string str = strArray[index];
          if (!this.dataGridViewAllParameter.Columns.Contains(str))
            this.dataGridViewAllParameter.Columns[this.dataGridViewAllParameter.Columns.Add(str, str)].FillWeight = 1f;
        }
      }
      int rowIndex = this.dataGridViewAllParameter.Rows.Add();
      for (int index = 0; index < strArray.Length; index += 2)
      {
        string columnName = strArray[index];
        string str = strArray[index + 1];
        this.dataGridViewAllParameter[columnName, rowIndex].Value = (object) str;
      }
      if (RefreshGrid)
      {
        this.dataGridViewAllParameter.ClearSelection();
        if (this.dataGridViewAllParameter.RowCount > 0)
        {
          this.dataGridViewAllParameter.FirstDisplayedScrollingRowIndex = this.dataGridViewAllParameter.RowCount - 1;
          this.dataGridViewAllParameter.Rows[this.dataGridViewAllParameter.RowCount - 1].Selected = true;
        }
        this.dataGridViewAllParameter.Refresh();
      }
      this.dataGridViewAllParameter.ColumnHeadersVisible = true;
      this.dataGridViewAllParameter.ResumeLayout();
      return true;
    }

    private bool NewAllParameterGrid(string AllParameterString)
    {
      string oldValue = Environment.NewLine.Substring(1);
      this.dataGridViewAllParameter.Columns.Clear();
      string[] strArray = AllParameterString.Split(Environment.NewLine[0]);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (!(strArray[index].Trim() == string.Empty))
        {
          string parameter = ParameterService.GetParameter(strArray[index], "SID");
          if (parameter == string.Empty || string.IsNullOrEmpty(this.txtFilterBySerialNumber.Text) || parameter.StartsWith(this.txtFilterBySerialNumber.Text))
            this.AddAllParameter(strArray[index].Replace(oldValue, ""));
        }
      }
      return true;
    }

    private void menuTransmitRadioFrame_Click(object sender, EventArgs e)
    {
      if (this.MyBus.TransmitRadioFrame())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Transmit ok.");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Transmit error.");
      }
    }

    private void buttonSelectZDF_File_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = SystemValues.ZDF_DataPath;
      openFileDialog.Filter = "ZENNER data file (*.zdf)|*.zdf| All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = true;
      openFileDialog.CheckFileExists = false;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.textBoxZDF_FileName.Text = openFileDialog.FileName;
    }

    private void buttonLoadZDF_File_Click(object sender, EventArgs e) => this.LoadZDF_File();

    private void menuItemLoadZDF_File_Click(object sender, EventArgs e) => this.LoadZDF_File();

    private void LoadZDF_File()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = SystemValues.ImportPath;
      openFileDialog.Filter = "ZDF files (*.zdf)|*.zdf| All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = false;
      openFileDialog.Title = "Read ZDF File";
      openFileDialog.CheckFileExists = true;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.SetLoopConditions();
      this.dataGridViewAllParameter.Columns.Clear();
      try
      {
        using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
        {
          int num1 = 0;
          string ZDF_String;
          while ((ZDF_String = streamReader.ReadLine()) != null)
          {
            if (num1 % 100 == 0)
            {
              this.labelStatus.Text = "read line: " + num1.ToString();
              Application.DoEvents();
              if (this.MyBus.BreakRequest)
                break;
            }
            ++num1;
            if (!this.AddParameterToDataGrid(ZDF_String, false))
            {
              int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("6"));
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "File error" + Environment.NewLine + ex.Message);
      }
      this.ResetLoopConditions();
    }

    private void menuItemExportDataTable_Click(object sender, EventArgs e)
    {
      this.ExportDataTabel();
    }

    private void ExportDataTabel()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.InitialDirectory = SystemValues.ExportPath;
      saveFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
      saveFileDialog.FilterIndex = 1;
      saveFileDialog.RestoreDirectory = true;
      saveFileDialog.Title = "Write to file";
      saveFileDialog.CheckFileExists = false;
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
        {
          char ch = '\t';
          StringBuilder stringBuilder = new StringBuilder(2000);
          bool flag1 = true;
          DataGridViewColumn dataGridViewColumnStart = this.dataGridViewAllParameter.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
          List<int> intList = new List<int>();
          for (; dataGridViewColumnStart != null; dataGridViewColumnStart = this.dataGridViewAllParameter.Columns.GetNextColumn(dataGridViewColumnStart, DataGridViewElementStates.Visible, DataGridViewElementStates.None))
          {
            if (!flag1)
              stringBuilder.Append(ch);
            stringBuilder.Append(dataGridViewColumnStart.HeaderText);
            flag1 = false;
            intList.Add(dataGridViewColumnStart.Index);
          }
          streamWriter.WriteLine(stringBuilder.ToString());
          foreach (DataGridViewRow row in (IEnumerable) this.dataGridViewAllParameter.Rows)
          {
            stringBuilder.Length = 0;
            bool flag2 = true;
            for (int index = 0; index < intList.Count; ++index)
            {
              DataGridViewCell cell = row.Cells[intList[index]];
              if (!flag2)
                stringBuilder.Append(ch);
              if (cell.Value != null)
                stringBuilder.Append(cell.Value.ToString());
              else
                stringBuilder.Append(" ");
              flag2 = false;
            }
            streamWriter.WriteLine(stringBuilder.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "File error" + Environment.NewLine + ex.Message);
      }
    }

    private void menuItemExportExcelDataTable_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        ExcelConnect excelConnect = new ExcelConnect();
        DataGridViewColumn dataGridViewColumnStart = this.dataGridViewAllParameter.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
        List<int> colIdsToExport = new List<int>();
        for (; dataGridViewColumnStart != null; dataGridViewColumnStart = this.dataGridViewAllParameter.Columns.GetNextColumn(dataGridViewColumnStart, DataGridViewElementStates.Visible, DataGridViewElementStates.None))
          colIdsToExport.Add(dataGridViewColumnStart.Index);
        DataTable TheTable = new DataTable();
        foreach (DataGridViewColumn column in (BaseCollection) this.dataGridViewAllParameter.Columns)
          TheTable.Columns.Add(column.HeaderText);
        foreach (DataGridViewRow row1 in (IEnumerable) this.dataGridViewAllParameter.Rows)
        {
          DataRow row2 = TheTable.NewRow();
          foreach (DataGridViewCell cell in (BaseCollection) row1.Cells)
            row2[cell.ColumnIndex] = cell.Value;
          TheTable.Rows.Add(row2);
        }
        excelConnect.AddTable(TheTable, "DeviceCollector Data", colIdsToExport, true);
        excelConnect.ShowWorkbook();
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        int num = (int) GMM_MessageBox.ShowMessage("GMM", DeviceCollectorFunctions.SerialBusMessage.GetString("ExcelConnectError") + Environment.NewLine + ex.Message, true);
      }
      this.Cursor = Cursors.Default;
    }

    private void menuItemSetSelectedDeviceToDefaultParameterList_Click(object sender, EventArgs e)
    {
      this.SetToDefaultParameterList();
    }

    private void menuCmSetToDefaultParameterList_Click(object sender, EventArgs e)
    {
      this.SetToDefaultParameterList();
    }

    private void SetToDefaultParameterList()
    {
      if (!this.MyBus.SetParameterListDefault())
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Set default parameter list: Error");
      }
      else
        this.labelStatus.Text = "Set default parameter list: Done";
    }

    private void menuItemSetSelectedDeviceToFullParameterList_Click(object sender, EventArgs e)
    {
      this.SetToFullParameterList();
    }

    private void menuCmSetToFullParameterList_Click(object sender, EventArgs e)
    {
      this.SetToFullParameterList();
    }

    private void SetToFullParameterList()
    {
      if (!this.MyBus.SetParameterListAll())
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Set full parameter list: Error");
      }
      else
        this.labelStatus.Text = "Set full parameter list: Done";
    }

    private void menuItemSetAllDevicesToDefaultParameterList_Click(object sender, EventArgs e)
    {
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.MyBus.BreakRequest = false;
      string str = "/" + this.MyBus.MyDeviceList.bus.Count.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; !this.MyBus.BreakRequest && index < this.MyBus.MyDeviceList.bus.Count; ++index)
      {
        this.labelStatus.Text = "Set default parameter list: " + index.ToString() + str;
        if (!this.MyBus.SetParameterListDefault(index))
          stringBuilder.AppendLine("Error on device: " + ((BusDevice) this.MyBus.MyDeviceList.bus[index]).Info.MeterNumber);
      }
      this.labelStatus.Text = "Set default parameter list: Done";
      if (stringBuilder.Length > 0)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", stringBuilder.ToString(), true);
      }
      this.ResetLoopConditions();
    }

    private void menuItemSetAllDevicesToFullParameterList_Click(object sender, EventArgs e)
    {
      this.SetLoopConditions();
      this.tabControl.SelectedTab = this.tabPageBusInfo;
      this.MyBus.BreakRequest = false;
      string str = "/" + this.MyBus.MyDeviceList.bus.Count.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; !this.MyBus.BreakRequest && index < this.MyBus.MyDeviceList.bus.Count; ++index)
      {
        this.labelStatus.Text = "Set full parameter list: " + index.ToString() + str;
        if (!this.MyBus.SetParameterListAllParameters(index))
          stringBuilder.AppendLine("Error on device: " + ((BusDevice) this.MyBus.MyDeviceList.bus[index]).Info.MeterNumber);
      }
      this.labelStatus.Text = "Set full parameter list: Done";
      if (stringBuilder.Length > 0)
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", stringBuilder.ToString(), true);
      }
      this.ResetLoopConditions();
    }

    private void menuItemShiftToNextAddress_Click(object sender, EventArgs e)
    {
      this.MyBus.BreakRequest = false;
      for (int index1 = 0; index1 < this.MyBus.MyDeviceList.bus.Count; ++index1)
      {
        if (this.MyBus.BreakRequest)
          return;
        byte primaryDeviceAddress1 = ((MBusDevice) this.MyBus.MyDeviceList.bus[index1]).PrimaryDeviceAddress;
        for (int index2 = index1 + 1; index2 < this.MyBus.MyDeviceList.bus.Count; ++index2)
        {
          byte primaryDeviceAddress2 = ((MBusDevice) this.MyBus.MyDeviceList.bus[index2]).PrimaryDeviceAddress;
          if ((int) primaryDeviceAddress1 == (int) primaryDeviceAddress2)
          {
            bool[] flagArray = new bool[256];
            for (byte index3 = 0; (int) index3 < this.MyBus.MyDeviceList.bus.Count; ++index3)
              flagArray[(int) ((MBusDevice) this.MyBus.MyDeviceList.bus[(int) index3]).PrimaryDeviceAddress] = true;
            byte Byte = 1;
            while ((int) Byte < flagArray.Length && flagArray[(int) Byte])
              ++Byte;
            MBusDevice bu = (MBusDevice) this.MyBus.MyDeviceList.bus[index1];
            uint SerialNumberOut;
            if (MBusDevice.StringToMBusSerialNumber(bu.Info.MeterNumber, out SerialNumberOut))
            {
              if (!this.MyBus.FastSecondaryAddressing)
                bu.DeselectDevice();
              if (bu.SelectDeviceOnBus(SerialNumberOut, ushort.MaxValue, byte.MaxValue, byte.MaxValue))
              {
                bu.TransmitBuffer = new ByteField(5);
                bu.TransmitBuffer.Add(16);
                bu.TransmitBuffer.Add(91);
                bu.TransmitBuffer.Add(253);
                bu.TransmitBuffer.Add((byte) 88);
                bu.TransmitBuffer.Add(22);
                this.MyBus.MyCom.TransmitBlock(ref bu.TransmitBuffer);
                Thread.Sleep(int.Parse(this.textBoxValue.Text));
                bu.GenerateLongframeStart();
                bu.TransmitBuffer.Add(83);
                bu.TransmitBuffer.Add(primaryDeviceAddress1);
                bu.TransmitBuffer.Add(81);
                bu.TransmitBuffer.Add(1);
                bu.TransmitBuffer.Add(122);
                bu.TransmitBuffer.Add(Byte);
                bu.FinishLongFrame();
                this.MyBus.MyCom.TransmitBlock(ref bu.TransmitBuffer);
                for (int index4 = index1 + 1; index4 < this.MyBus.MyDeviceList.bus.Count; ++index4)
                {
                  if ((int) ((MBusDevice) this.MyBus.MyDeviceList.bus[index4]).PrimaryDeviceAddress == (int) primaryDeviceAddress1)
                    ((MBusDevice) this.MyBus.MyDeviceList.bus[index4]).PrimaryDeviceAddress = Byte;
                }
                Thread.Sleep(500);
                break;
              }
              break;
            }
            break;
          }
        }
      }
      this.RefreshBusInfo();
    }

    private void menuItemSerialBusHelp_Click(object sender, EventArgs e)
    {
    }

    private void menuItemShowWaveFlowParameter_Click(object sender, EventArgs e)
    {
      if (!(this.MyBus.MyDeviceList.SelectedDevice is WaveFlowDevice))
        return;
      int num = (int) MessageBox.Show(((WaveFlowDevice) this.MyBus.MyDeviceList.SelectedDevice).GetParameterString());
    }

    private void menuItemWafeFlowParameterTest_Click(object sender, EventArgs e)
    {
    }

    private void dataGridBusTable_CellMouseDoubleClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      this.ReadDeviceParameter(true);
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.RadioMS || this.MyBus.MyBusMode == BusMode.MinomatRadioTest;
      if (this.tabControl.SelectedIndex != 0 || flag)
        return;
      this.RefreshBusInfo();
    }

    private void dataGridBusTable_MouseClick(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hitTestInfo = this.dataGridBusTable.HitTest(e.X, e.Y);
      if (hitTestInfo.RowIndex < 0)
        return;
      try
      {
        DataRow row = ((DataRowView) this.dataGridBusTable.Rows[hitTestInfo.RowIndex].DataBoundItem).Row;
        if (row == null || row.ItemArray != null && row.ItemArray.Length > 2 && row.ItemArray[2].ToString() == "Collision")
          return;
        if (!this.MyBus.MyDeviceList.SelectDeviceByIndex(((DataTable) this.dataGridBusTable.DataSource).Rows.IndexOf(row)))
        {
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Select error");
        }
        this.SetEnabledFunctions();
      }
      catch (Exception ex)
      {
        DeviceCollectorWindow.logger.Error(ex.Message);
      }
    }

    private void menuSetSelectedDeviceTo300_Baud_Click(object sender, EventArgs e)
    {
      this.SetBaudrate(300);
    }

    private void menuSetSelectedDeviceTo2400_Baud_Click(object sender, EventArgs e)
    {
      this.SetBaudrate(2400);
    }

    private void menuSetSelectedDeviceTo9600_Baud_Click(object sender, EventArgs e)
    {
      this.SetBaudrate(9600);
    }

    private void menuSetSelectedDeviceTo38400_Baud_Click(object sender, EventArgs e)
    {
      this.SetBaudrate(38400);
    }

    private void SetBaudrate(int Baudrate)
    {
      if (this.MyBus.SetBaudrate(Baudrate))
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Baudrate change ok");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Baudrate change error!");
      }
    }

    private void menuOrganize_Click(object sender, EventArgs e) => this.Organize();

    private void menuItemShowInfo_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        object InfoObject;
        if (!this.MyBus.MyDeviceList.GetDeviceCollectorInfo(out InfoObject))
          return;
        int num = (int) MessageBox.Show(this.getFieldsAndValues(InfoObject));
      }
      catch
      {
      }
    }

    public string getFieldsAndValues(object o)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (o != null)
      {
        foreach (FieldInfo field in o.GetType().GetFields())
        {
          if (!field.FieldType.IsValueType)
            stringBuilder.Append(this.getFieldsAndValues(field.GetValue(o)));
          else
            stringBuilder.AppendLine(field.Name + ":" + field.GetValue(o)?.ToString());
        }
      }
      return stringBuilder.ToString();
    }

    private void dataGridBusTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      int columnIndex = this.dataGridBusTable["DeviceInfoText", 0].ColumnIndex;
      if (e.ColumnIndex == columnIndex)
        return;
      e.Cancel = true;
    }

    private void dataGridBusTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        if (!this.MyBus.MyDeviceList.SelectDeviceByIndex(((DataTable) this.dataGridBusTable.DataSource).Rows.IndexOf(((DataRowView) this.dataGridBusTable.Rows[e.RowIndex].DataBoundItem).Row)))
        {
          int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", "Select error");
        }
        else
        {
          string str = ((DataGridView) sender)[e.ColumnIndex, e.RowIndex].Value.ToString();
          if (this.MyBus.MyDeviceList.SelectedDevice is MBusDevice selectedDevice && str != selectedDevice.DeviceInfoText)
          {
            selectedDevice.DeviceInfoText = str;
            this.BusinfoIsManualChanged = true;
          }
        }
      }
      catch
      {
      }
    }

    private void HideColumns(DataGridView dataGrid, params string[] columnNames)
    {
      if (dataGrid == null || dataGrid.ColumnCount <= 0 || columnNames.Length == 0)
        return;
      foreach (string columnName in columnNames)
      {
        if (dataGrid.Columns.Contains(columnName))
          dataGrid.Columns[columnName].Visible = false;
      }
    }

    private void menuItemSetupProfiles_Click(object sender, EventArgs e)
    {
      int num = (int) new SetupProfiles(this.MyBus).ShowDialog();
    }

    private void buttonBreak_MouseMove(object sender, MouseEventArgs e)
    {
      this.Cursor = Cursors.Default;
      this.Refresh();
      Application.DoEvents();
    }

    private void buttonBreak_MouseLeave(object sender, EventArgs e)
    {
      if (this.LoopIsRunning)
        this.Cursor = Cursors.WaitCursor;
      this.Refresh();
      Application.DoEvents();
    }

    private void menuItemMemoryAccess_Click(object sender, EventArgs e)
    {
      if (this.MyMemory == null)
        this.MyMemory = new MemoryAccess(this.MyBus);
      if (this.MyMemory.CanFocus)
        return;
      this.MyMemory.Show((IWin32Window) this);
    }

    private void menuItemAblaufTest_Click(object sender, EventArgs e)
    {
      this.MyBus.SetBaudrate(4800, true);
      this.GetVersion((int[]) null);
    }

    private void menuItem5_Click(object sender, EventArgs e)
    {
      new RadioScannerForm(this.MyBus).Show();
    }

    private void btnExportToCSV_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.FileName = this.MyBus.MyBusInfo.BusInfoFilename + ".csv";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
      {
        string str1 = string.Empty;
        for (int index = 0; index < this.dataGridViewAllParameter.Columns.Count; ++index)
          str1 = str1 + this.dataGridViewAllParameter.Columns[index].HeaderText + ";";
        streamWriter.WriteLine(str1);
        for (int index1 = 0; index1 < this.dataGridViewAllParameter.Rows.Count; ++index1)
        {
          string str2 = string.Empty;
          for (int index2 = 0; index2 < this.dataGridViewAllParameter.Columns.Count; ++index2)
          {
            string s = this.dataGridViewAllParameter.Rows[index1].Cells[index2].Value.ToString();
            str2 = !DateTime.TryParse(s, out DateTime _) ? str2 + s.Replace('.', ',') + ";" : str2 + s + ";";
          }
          streamWriter.WriteLine(str2);
        }
      }
    }

    private void menuItemMBusConverter_Click(object sender, EventArgs e)
    {
      this.MBusConverterDiagnostic();
    }

    private void menuCtMBusConverter_Click(object sender, EventArgs e)
    {
      this.MBusConverterDiagnostic();
    }

    private void MBusConverterDiagnostic()
    {
      if (this.MBusConverter == null)
        this.MBusConverter = new DeviceCollector.MBusConverterDiagnostic(this.MyBus);
      int num = (int) this.MBusConverter.ShowDialog();
    }

    private void menuItemSelectParameterList_Click(object sender, EventArgs e)
    {
      this.SelectParameterList();
    }

    private void menuCtSelectParameterList_Click(object sender, EventArgs e)
    {
      this.SelectParameterList();
    }

    private void SelectParameterList()
    {
      int num = (int) new ListSelectWindow(this.MyBus).ShowDialog();
    }

    private void dataGridBusTable_Sorted(object sender, EventArgs e)
    {
      this.SortDeviceListTableEqualToDataGridView();
    }

    private void SortDeviceListTableEqualToDataGridView()
    {
      if (this.MyBus == null || this.MyBus.MyDeviceList == null || this.MyBus.MyDeviceList.bus == null || this.MyBus.MyDeviceList.bus.Count == 0 || this.dataGridBusTable.RowCount == 0 || this.dataGridBusTable.DataSource == null || this.dataGridBusTable.Rows.Count == 0 || !this.dataGridBusTable.Columns.Contains("SerialNr."))
        return;
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.dataGridBusTable.Rows.Count; ++index)
      {
        string str = this.dataGridBusTable.Rows[index].Cells["SerialNr."].Value.ToString();
        foreach (object bu in this.MyBus.MyDeviceList.bus)
        {
          if (bu is BusDevice busDevice && busDevice.Info != null && busDevice.Info.MeterNumber == str)
          {
            arrayList.Add((object) busDevice);
            break;
          }
        }
        if (arrayList.Count == this.MyBus.MyDeviceList.bus.Count)
          this.MyBus.MyDeviceList.bus = arrayList;
      }
    }

    private void menuMbusParser_Click(object sender, EventArgs e) => MBusParserWindow.ShowWindow();

    private void menuItemMinoConnectTest_Click(object sender, EventArgs e)
    {
      int num = (int) new MinoConnectTest(this.MyBus).ShowDialog();
    }

    private void buttonAsyncCom_Click(object sender, EventArgs e)
    {
      this.MyBus.MyCom.ShowComWindow();
    }
  }
}
