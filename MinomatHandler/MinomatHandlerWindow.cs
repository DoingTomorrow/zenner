// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MinomatHandlerWindow
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using AsyncCom;
using GmmDbLib;
using MinomatHandler.Components;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Properties;

#nullable disable
namespace MinomatHandler
{
  public class MinomatHandlerWindow : Form
  {
    private TextBoxesControl textBoxesCtrl;
    private DataTableControl dataTableCtrl;
    private DataTableWithDateTimeControl dataTableWithDateTimeCtrl;
    private RichTextBoxControl richTextBoxCtrl;
    private MinomatV4 minomatV4;
    internal string NextComponentName = "Exit";
    private IContainer components = (IContainer) null;
    private MenuStrip menuStrip;
    private ToolStripMenuItem ToolStripMenuItemComponents;
    private Panel panel;
    private TreeView tree;
    private GroupBox groupBox1;
    private ToolStripMenuItem menuSCGiParser;
    private ToolStripMenuItem menuSCGiParserDlg;
    private Label label1;
    private ComboBox cboxSourceAddress;
    private StatusStrip statusStrip1;
    private ToolStripProgressBar progress;
    private ToolStripStatusLabel lblStatus;
    private TextBox txtOutput;
    private CheckBox btnUsePSW;

    public MinomatHandlerWindow(MinomatHandlerFunctions MyMinomatHandlerFunctions)
    {
      this.InitializeComponent();
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      this.Text = string.Format("MinomatHandler v{0}.{1}.{2}", (object) version.Major, (object) version.Minor, (object) version.Build);
      this.tree.ImageList = new ImageList()
      {
        Images = {
          (Image) Resources.FolderNode,
          (Image) Resources.Property,
          (Image) Resources.Function
        }
      };
      this.ToolStripMenuItemComponents.DropDownItems.AddRange(MyMinomatHandlerFunctions.GetComponentMenuItems());
      this.menuStrip.Visible = true;
      this.minomatV4 = MyMinomatHandlerFunctions.MyMinomatV4;
      this.minomatV4.Connection.OnRequest += new EventHandler<SCGiPacket>(this.Connection_OnRequest);
      this.minomatV4.Connection.OnResponse += new CommunicationEventHandler(this.Connection_OnResponse);
      this.minomatV4.OnError += new EventHandlerEx<Exception>(this.MinomatV4_OnError);
      this.PrepareTree();
      this.PrepareControls();
      FormTranslatorSupport.TranslateWindow(Tg.MinomatHandlerWindow, (Form) this);
    }

    private void MinomatHandlerWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.minomatV4.OnMessage -= new EventHandler<MinomatV4.StateEventArgs>(this.MinomatV4_OnMessage);
      this.minomatV4.OnError -= new EventHandlerEx<Exception>(this.MinomatV4_OnError);
      this.minomatV4.Connection.OnRequest -= new EventHandler<SCGiPacket>(this.Connection_OnRequest);
      this.minomatV4.Connection.OnResponse -= new CommunicationEventHandler(this.Connection_OnResponse);
    }

    private void MinomatHandlerWindow_Load(object sender, EventArgs e)
    {
      if (this.minomatV4 == null || this.minomatV4.Connection == null)
        return;
      SortedList<AsyncComSettings, object> settings = this.minomatV4.Connection.GetSettings();
      StringBuilder stringBuilder = new StringBuilder();
      if (!AsyncFunctions.IsSettingsEqual(settings, AsyncComSettings.Baudrate, "38400"))
        stringBuilder.AppendLine("Invalid settings to read Minomat! Expected: 38400 baudrate");
      if (!AsyncFunctions.IsSettingsEqual(settings, AsyncComSettings.Parity, "no"))
        stringBuilder.AppendLine("Invalid settings to read Minomat! Expected: no parity");
      if (stringBuilder.Length > 0)
      {
        int num = (int) GMM_MessageBox.ShowMessage("MinomatHandler", stringBuilder.ToString(), true);
      }
      foreach (object obj in Enum.GetValues(typeof (SCGiAddress)))
        this.cboxSourceAddress.Items.Add(obj);
      this.cboxSourceAddress.Text = SCGiAddress.RS232.ToString();
      this.minomatV4.OnMessage += new EventHandler<MinomatV4.StateEventArgs>(this.MinomatV4_OnMessage);
    }

    private void MinomatV4_OnMessage(object sender, MinomatV4.StateEventArgs e)
    {
      if (e.ProgressValue > 0)
      {
        this.progress.Visible = true;
        this.progress.Value = e.ProgressValue;
      }
      if (!string.IsNullOrEmpty(e.Message))
        this.lblStatus.Text = e.Message;
      this.minomatV4.CancelCurrentMethod = this.dataTableWithDateTimeCtrl.isCanceled;
      Application.DoEvents();
    }

    private void MinomatV4_OnError(object sender, Exception e)
    {
      if (e == null || string.IsNullOrEmpty(e.Message))
        return;
      this.txtOutput.AppendText("ERROR: ");
      this.txtOutput.AppendText(e.Message);
      this.txtOutput.AppendText(Environment.NewLine);
    }

    private void menuSCGiParserDlg_Click(object sender, EventArgs e)
    {
      SCGiParserWindow.Show((Form) this);
    }

    private void cboxSourceAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.minomatV4 == null)
        return;
      this.minomatV4.Connection.SourceAddress = (SCGiAddress) this.cboxSourceAddress.SelectedItem;
    }

    private void tree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.lblStatus.Text = string.Empty;
      foreach (Control control in (ArrangedElementCollection) this.panel.Controls)
        control.Visible = false;
      if (!Enum.IsDefined(typeof (SCGiCommand), (object) e.Node.Text))
        return;
      SCGiCommand cmd = (SCGiCommand) Enum.Parse(typeof (SCGiCommand), e.Node.Text);
      switch (cmd)
      {
        case SCGiCommand.MinomatV4:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.RichTextBox, MinomatHandlerWindow.Show.Button1, false, "Minomat V4 Settings", string.Empty);
          break;
        case SCGiCommand.Minol:
          break;
        case SCGiCommand.Firmware:
          break;
        case SCGiCommand.LPSR:
          break;
        case SCGiCommand.GSM:
          break;
        case SCGiCommand.SCGi1_9:
          break;
        case SCGiCommand.ComServer:
          break;
        case SCGiCommand.ResetConfigurationState:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "State of ResetConfig", "State");
          break;
        case SCGiCommand.MinolId:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Minol", "Minol ID");
          break;
        case SCGiCommand.NodeId:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Node", "Node ID");
          break;
        case SCGiCommand.NetworkId:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Network", "Network ID");
          break;
        case SCGiCommand.SystemTime:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Device", "System Time");
          break;
        case SCGiCommand.RadioChannel:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Radio channel", "Channel number", "Channel description");
          break;
        case SCGiCommand.TransceiverChannelId:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Transceiver", "Channel ID");
          break;
        case SCGiCommand.RoutingTable:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.DataTable, MinomatHandlerWindow.Show.Button1, false, "Routing table", string.Empty);
          break;
        case SCGiCommand.FirmwareVersion:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Firmware", "Version", string.Empty);
          break;
        case SCGiCommand.UserappName:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Userapp", "Name");
          break;
        case SCGiCommand.FirmwareBuildTime:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Firmware", "Build time", string.Empty);
          break;
        case SCGiCommand.UserappBuildTime:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Userapp", "Build time", string.Empty);
          break;
        case SCGiCommand.ErrorFlags:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Firmware", "Error flags", string.Empty);
          break;
        case SCGiCommand.TransmissionPower:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Radio", "Transmission Power");
          break;
        case SCGiCommand.MultiChannelSettings:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Channel settings", "MF Channel 0", "MF Channel 1", "MF Channel 2", "MF Channel 3");
          break;
        case SCGiCommand.TransceiverFrequencyOffset:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Transceiver", "Frequency Offset");
          break;
        case SCGiCommand.TemperatureOffset:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Sensor", "Temperature Offset");
          break;
        case SCGiCommand.PhaseDetailsBuffer:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "PhasesDetails", "Hex sting");
          break;
        case SCGiCommand.PhaseDetails:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Phase Details", "Phase", "State", "SubState");
          break;
        case SCGiCommand.RestartMinomat:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Restart Minomat", string.Empty);
          break;
        case SCGiCommand.MessUnitNumberMax:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Minol", "Maximum Mess Unit Number");
          break;
        case SCGiCommand.MaxMessUnitNumberNotConfigured:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Mess Unit Number Not Explicit Configured", "Max.");
          break;
        case SCGiCommand.Scenario:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Minol", "Scenario", "Length of request payload");
          break;
        case SCGiCommand.StartTestReception:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Start Test Reception", "Action", "Protocol");
          break;
        case SCGiCommand.TestReceptionResult:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.DataTable, MinomatHandlerWindow.Show.Button1, false, "Test Reception Result", string.Empty);
          break;
        case SCGiCommand.RegisterMessUnit:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Register Mess Unit", "Serialnumber", "Measurement Category", "Measurement Type", "Radio Protocol");
          break;
        case SCGiCommand.ResetConfiguration:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Reset Configuration", string.Empty);
          break;
        case SCGiCommand.StartNetworkSetup:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Start Network Setup", "Mode");
          break;
        case SCGiCommand.DeleteMessUnit:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Delete Mess Unit", "ID");
          break;
        case SCGiCommand.InfoOfRegisteredMessUnit:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Get Info Of Registered Mess Unit", "Serialnumber", "Measurement Category", "Measurement Type", "Radio Protocol");
          break;
        case SCGiCommand.RegisteredMessUnits:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.RichTextBox, MinomatHandlerWindow.Show.Button1, true, "Registered Mess Units", string.Empty);
          break;
        case SCGiCommand.SimPin:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "SIM", "PIN");
          break;
        case SCGiCommand.MasterMinolID:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Modem", "Master MinolID");
          break;
        case SCGiCommand.APN:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "APN", "Servername");
          break;
        case SCGiCommand.GPRSUserName:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "GPRS", "User Name");
          break;
        case SCGiCommand.GPRSPassword:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "GPRS", "Password");
          break;
        case SCGiCommand.HttpServer:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "HTTP-Server", "Port", "Servername");
          break;
        case SCGiCommand.HttpResourceName:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Resource Name", "Path");
          break;
        case SCGiCommand.StartGSMTestReception:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Start GSM Test Reception", string.Empty);
          break;
        case SCGiCommand.GSMTestReceptionState:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "GSM Test Reception", "State");
          break;
        case SCGiCommand.ForceNetworkOptimization:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Force Network Optimization", "State");
          break;
        case SCGiCommand.StartNetworkOptimization:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Start Network Optimization", string.Empty);
          break;
        case SCGiCommand.RegisterOrDeregisterSlave:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, true, "Register or Deregister Slave", "Slave Node ID", "Slave Minol ID");
          break;
        case SCGiCommand.RegisteredSlaves:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.DataTable, MinomatHandlerWindow.Show.Button1, false, "Registered Slaves", "Slave Type");
          break;
        case SCGiCommand.Flash:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.RichTextBox, MinomatHandlerWindow.Show.Button1, true, "FLASH memory", "Chip number", "Page number");
          break;
        case SCGiCommand.Eeprom:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.RichTextBox, MinomatHandlerWindow.Show.Button1, true, "EEPROM memory", "Chip number");
          break;
        case SCGiCommand.AppInitialSettings:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Application Initial Settings", "Challenge", "GSM-ID", "SAP Configuration Number", "MD5");
          break;
        case SCGiCommand.ActionTimepoint:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Action Timepoint", "Action", "Timepoint type", "Timepoint");
          break;
        case SCGiCommand.MeasurementData:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.DataTableWithDateTime, MinomatHandlerWindow.Show.Button1, true, "Measurement Data", "ID", "Data Type", "Start", "End");
          break;
        case SCGiCommand.HttpState:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "HTTP State", "State", "Server Port", "Server Name");
          break;
        case SCGiCommand.GsmState:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "GSM State", "State A Byte0", "State B Byte1");
          break;
        case SCGiCommand.ModemBuildDate:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Modem", "Build Date");
          break;
        case SCGiCommand.StartHttpConnection:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Start HTTP Connection", string.Empty);
          break;
        case SCGiCommand.ConfigurationString:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.RichTextBox, MinomatHandlerWindow.Show.Button1, false, "Minomat Configuration String", string.Empty);
          break;
        case SCGiCommand.SwitchToNetworkModel:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Switch To Network Model", string.Empty);
          break;
        case SCGiCommand.GetComServerFile:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.RichTextBox, MinomatHandlerWindow.Show.Button1, false, "COM Server File Content", "Type");
          break;
        case SCGiCommand.MessUnitMetadata:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Mess Unit Metadata", "ID", "Timepoint", "Timepoint Slave", "Status", "Error Timepoint");
          break;
        case SCGiCommand.ActivateModemAT_Mode:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Activate Modem AT-Mode", string.Empty);
          break;
        case SCGiCommand.LED:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "LED", "Red", "Yellow", "Green 1", "Green 2");
          break;
        case SCGiCommand.ModemUpdateImageClear:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Modem Update Image Clear", string.Empty);
          break;
        case SCGiCommand.ModemUpdate:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Modem Update", "URL (case sensitive)");
          break;
        case SCGiCommand.ModemReboot:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Modem Reboot", string.Empty);
          break;
        case SCGiCommand.ModemCounter:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, true, "Modem Counter Readout", "Counter type", "Value", "Error", "Timepoint");
          break;
        case SCGiCommand.TcpConfiguration:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Modem TCP Configuration", "Port", "Servername");
          break;
        case SCGiCommand.SendSMS:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Modem Send SMS", "Phonenumber", "Message");
          break;
        case SCGiCommand.ModemDueDate:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, (MinomatHandlerWindow.Show) 0, false, "Modem", "Due Date");
          break;
        case SCGiCommand.GsmLinkQuality:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "GSM Link Quality", "Quality");
          break;
        case SCGiCommand.ModemUpdateTiming:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, false, "Update Timing", "Start time", "Finish time");
          break;
        case SCGiCommand.ModemUpdateTest:
          this.ShowControl(cmd, MinomatHandlerWindow.UserCtrl.TextBoxes, MinomatHandlerWindow.Show.Button1, true, "Modem Update Test", "URL (case sensitive)");
          break;
        default:
          throw new NotImplementedException("Not implemented CMD: " + cmd.ToString());
      }
    }

    private void PrepareTree()
    {
      this.tree.BeginUpdate();
      this.tree.Nodes.Clear();
      TreeNode treeNode1 = this.tree.Nodes.Add(SCGiCommand.MinomatV4.ToString());
      TreeNode treeNode2 = treeNode1.Nodes.Add("", SCGiCommand.Minol.ToString(), 0, 0);
      treeNode2.Nodes.Add("MeasurementData", SCGiCommand.MeasurementData.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.MessUnitMetadata.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.RegisteredMessUnits.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.InfoOfRegisteredMessUnit.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.RegisterMessUnit.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.StartTestReception.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.TestReceptionResult.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.DeleteMessUnit.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.RadioChannel.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.PhaseDetailsBuffer.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.StartNetworkSetup.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.ResetConfiguration.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.ResetConfigurationState.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.RegisterOrDeregisterSlave.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.RegisteredSlaves.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.MinolId.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.ForceNetworkOptimization.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.PhaseDetails.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.SwitchToNetworkModel.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.StartNetworkOptimization.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.MessUnitNumberMax.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.MaxMessUnitNumberNotConfigured.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.Scenario.ToString(), 1, 1);
      treeNode2.Nodes.Add("", SCGiCommand.Flash.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.Eeprom.ToString(), 2, 2);
      treeNode2.Nodes.Add("", SCGiCommand.LED.ToString(), 2, 2);
      TreeNode treeNode3 = treeNode1.Nodes.Add("", SCGiCommand.Firmware.ToString(), 0, 0);
      treeNode3.Nodes.Add("", SCGiCommand.UserappName.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.FirmwareVersion.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.FirmwareBuildTime.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.UserappBuildTime.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.NodeId.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.NetworkId.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.ErrorFlags.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.TransmissionPower.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.TransceiverChannelId.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.MultiChannelSettings.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.TransceiverFrequencyOffset.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.SystemTime.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.TemperatureOffset.ToString(), 1, 1);
      treeNode3.Nodes.Add("", SCGiCommand.RestartMinomat.ToString(), 2, 2);
      treeNode1.Nodes.Add("", SCGiCommand.LPSR.ToString(), 0, 0).Nodes.Add("", SCGiCommand.RoutingTable.ToString(), 1, 1);
      TreeNode treeNode4 = treeNode1.Nodes.Add("", SCGiCommand.GSM.ToString(), 0, 0);
      treeNode4.Nodes.Add("", SCGiCommand.SendSMS.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.MasterMinolID.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.StartHttpConnection.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.ActivateModemAT_Mode.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.StartGSMTestReception.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.GsmState.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.HttpState.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.GsmLinkQuality.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.GSMTestReceptionState.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.AppInitialSettings.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.SimPin.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.APN.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.GPRSUserName.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.GPRSPassword.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.TcpConfiguration.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.HttpServer.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.HttpResourceName.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.ModemDueDate.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.ActionTimepoint.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.ModemBuildDate.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.ModemUpdateTest.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.ModemUpdateTiming.ToString(), 1, 1);
      treeNode4.Nodes.Add("", SCGiCommand.ModemUpdateImageClear.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.ModemUpdate.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.ModemReboot.ToString(), 2, 2);
      treeNode4.Nodes.Add("", SCGiCommand.ModemCounter.ToString(), 2, 2);
      treeNode1.Nodes.Add("", SCGiCommand.SCGi1_9.ToString(), 0, 0).Nodes.Add("", SCGiCommand.ConfigurationString.ToString(), 1, 1);
      treeNode1.Nodes.Add("", SCGiCommand.ComServer.ToString(), 0, 0).Nodes.Add("", SCGiCommand.GetComServerFile.ToString(), 1, 1);
      treeNode1.Expand();
      treeNode2.Expand();
      this.tree.SelectedNode = treeNode2.Nodes["MeasurementData"];
      this.tree.EndUpdate();
    }

    private void PrepareControls()
    {
      this.textBoxesCtrl = new TextBoxesControl();
      this.textBoxesCtrl.Dock = DockStyle.Fill;
      this.textBoxesCtrl.Visible = false;
      this.textBoxesCtrl.btnGet.Click += new System.EventHandler(this.TextBoxesControl_ButtonGet_Click);
      this.textBoxesCtrl.btnSet.Click += new System.EventHandler(this.TextBoxesControl_ButtonSet_Click);
      this.panel.Controls.Add((Control) this.textBoxesCtrl);
      this.dataTableCtrl = new DataTableControl();
      this.dataTableCtrl.Dock = DockStyle.Fill;
      this.dataTableCtrl.Visible = false;
      this.dataTableCtrl.btnGet.Click += new System.EventHandler(this.DataTableControl_ButtonGet_Click);
      this.panel.Controls.Add((Control) this.dataTableCtrl);
      this.richTextBoxCtrl = new RichTextBoxControl();
      this.richTextBoxCtrl.Dock = DockStyle.Fill;
      this.richTextBoxCtrl.Visible = false;
      this.richTextBoxCtrl.btnGet.Click += new System.EventHandler(this.RichTextBox_ButtonGet_Click);
      this.panel.Controls.Add((Control) this.richTextBoxCtrl);
      this.dataTableWithDateTimeCtrl = new DataTableWithDateTimeControl();
      this.dataTableWithDateTimeCtrl.Dock = DockStyle.Fill;
      this.dataTableWithDateTimeCtrl.Visible = false;
      this.dataTableWithDateTimeCtrl.btnGet.Click += new System.EventHandler(this.DataTableControl_ButtonGet_Click);
      this.dataTableWithDateTimeCtrl.btnStop.Visible = false;
      this.panel.Controls.Add((Control) this.dataTableWithDateTimeCtrl);
    }

    private void ShowControl(
      SCGiCommand cmd,
      MinomatHandlerWindow.UserCtrl control,
      MinomatHandlerWindow.Show btns,
      bool isFunction,
      string titel,
      string valueName1)
    {
      this.ShowControl(cmd, control, btns, isFunction, titel, valueName1, string.Empty);
    }

    private void ShowControl(
      SCGiCommand cmd,
      MinomatHandlerWindow.UserCtrl control,
      MinomatHandlerWindow.Show btns,
      bool isFunction,
      string titel,
      string valueName1,
      string valueName2)
    {
      this.ShowControl(cmd, control, btns, isFunction, titel, valueName1, valueName2, string.Empty);
    }

    private void ShowControl(
      SCGiCommand cmd,
      MinomatHandlerWindow.UserCtrl control,
      MinomatHandlerWindow.Show btns,
      bool isFunction,
      string titel,
      string valueName1,
      string valueName2,
      string valueName3)
    {
      this.ShowControl(cmd, control, btns, isFunction, titel, valueName1, valueName2, valueName3, string.Empty);
    }

    private void ShowControl(
      SCGiCommand cmd,
      MinomatHandlerWindow.UserCtrl control,
      MinomatHandlerWindow.Show btns,
      bool isFunction,
      string titel,
      string valueName1,
      string valueName2,
      string valueName3,
      string valueName4)
    {
      this.ShowControl(cmd, control, btns, isFunction, titel, valueName1, valueName2, valueName3, valueName4, string.Empty);
    }

    private void ShowControl(
      SCGiCommand cmd,
      MinomatHandlerWindow.UserCtrl control,
      MinomatHandlerWindow.Show btns,
      bool isFunction,
      string titel,
      string valueName1,
      string valueName2,
      string valueName3,
      string valueName4,
      string valueName5)
    {
      this.dataTableWithDateTimeCtrl.btnValue1.Click -= new System.EventHandler(this.DataTableWithDateTime_btnValue1_Click);
      switch (control)
      {
        case MinomatHandlerWindow.UserCtrl.TextBoxes:
          this.textBoxesCtrl.Visible = true;
          this.textBoxesCtrl.btnGet.Tag = (object) cmd.ToString();
          this.textBoxesCtrl.btnGet.Text = isFunction ? "Execute" : "Read";
          this.textBoxesCtrl.btnGet.Visible = (btns | MinomatHandlerWindow.Show.Button1) == MinomatHandlerWindow.Show.Button1;
          this.textBoxesCtrl.btnSet.Tag = (object) cmd.ToString();
          this.textBoxesCtrl.btnSet.Visible = (btns | MinomatHandlerWindow.Show.Button2) == MinomatHandlerWindow.Show.Button2;
          this.textBoxesCtrl.btnSet.Image = (Image) Resources.SaveButton;
          this.textBoxesCtrl.btnSet.Text = isFunction ? "Execute" : "Write";
          this.textBoxesCtrl.lblTitel.Text = titel;
          this.textBoxesCtrl.lblCMD.Text = SCGiCommandManager.GetAsHexString(cmd);
          this.textBoxesCtrl.lblValueName1.Text = valueName1;
          this.textBoxesCtrl.lblValueName1.Visible = !string.IsNullOrEmpty(valueName1);
          this.textBoxesCtrl.txtValue1.Visible = this.textBoxesCtrl.lblValueName1.Visible;
          this.textBoxesCtrl.txtValue1.Text = string.Empty;
          this.textBoxesCtrl.txtValue1.BackColor = SystemColors.Window;
          this.textBoxesCtrl.txtValue1.SelectedItem = (object) null;
          this.textBoxesCtrl.txtValue1.Items.Clear();
          this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.Simple;
          this.textBoxesCtrl.txtValue1.Enabled = true;
          this.textBoxesCtrl.lblValueName2.Text = valueName2;
          this.textBoxesCtrl.lblValueName2.Visible = !string.IsNullOrEmpty(valueName2);
          this.textBoxesCtrl.txtValue2.Visible = this.textBoxesCtrl.lblValueName2.Visible;
          this.textBoxesCtrl.txtValue2.Text = string.Empty;
          this.textBoxesCtrl.txtValue2.BackColor = SystemColors.Window;
          this.textBoxesCtrl.txtValue2.SelectedItem = (object) null;
          this.textBoxesCtrl.txtValue2.Items.Clear();
          this.textBoxesCtrl.txtValue2.DropDownStyle = ComboBoxStyle.Simple;
          this.textBoxesCtrl.txtValue2.Enabled = true;
          this.textBoxesCtrl.lblValueName3.Text = valueName3;
          this.textBoxesCtrl.lblValueName3.Visible = !string.IsNullOrEmpty(valueName3);
          this.textBoxesCtrl.txtValue3.Visible = this.textBoxesCtrl.lblValueName3.Visible;
          this.textBoxesCtrl.txtValue3.Text = string.Empty;
          this.textBoxesCtrl.txtValue3.BackColor = SystemColors.Window;
          this.textBoxesCtrl.txtValue3.SelectedItem = (object) null;
          this.textBoxesCtrl.txtValue3.Items.Clear();
          this.textBoxesCtrl.txtValue3.DropDownStyle = ComboBoxStyle.Simple;
          this.textBoxesCtrl.txtValue3.Enabled = true;
          this.textBoxesCtrl.lblValueName4.Text = valueName4;
          this.textBoxesCtrl.lblValueName4.Visible = !string.IsNullOrEmpty(valueName4);
          this.textBoxesCtrl.txtValue4.Visible = this.textBoxesCtrl.lblValueName4.Visible;
          this.textBoxesCtrl.txtValue4.Text = string.Empty;
          this.textBoxesCtrl.txtValue4.BackColor = SystemColors.Window;
          this.textBoxesCtrl.txtValue4.SelectedItem = (object) null;
          this.textBoxesCtrl.txtValue4.Items.Clear();
          this.textBoxesCtrl.txtValue4.DropDownStyle = ComboBoxStyle.Simple;
          this.textBoxesCtrl.txtValue4.Enabled = true;
          this.textBoxesCtrl.lblValueName5.Text = valueName5;
          this.textBoxesCtrl.lblValueName5.Visible = !string.IsNullOrEmpty(valueName5);
          this.textBoxesCtrl.txtValue5.Visible = this.textBoxesCtrl.lblValueName5.Visible;
          this.textBoxesCtrl.txtValue5.Text = string.Empty;
          this.textBoxesCtrl.txtValue5.BackColor = SystemColors.Window;
          this.textBoxesCtrl.txtValue5.SelectedItem = (object) null;
          this.textBoxesCtrl.txtValue5.Items.Clear();
          this.textBoxesCtrl.txtValue5.DropDownStyle = ComboBoxStyle.Simple;
          this.textBoxesCtrl.txtValue5.Enabled = true;
          int num;
          switch (cmd)
          {
            case SCGiCommand.Scenario:
              this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (Scenario)))
                this.textBoxesCtrl.txtValue1.Items.Add(obj);
              this.textBoxesCtrl.txtValue2.DropDownStyle = ComboBoxStyle.DropDownList;
              this.textBoxesCtrl.txtValue2.Items.Add((object) "10 Byte");
              this.textBoxesCtrl.txtValue2.Items.Add((object) "4 Byte");
              this.textBoxesCtrl.txtValue1.SelectedIndex = 0;
              this.textBoxesCtrl.txtValue2.SelectedIndex = 0;
              return;
            case SCGiCommand.StartTestReception:
              this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (StartTestReceptionAction)))
                this.textBoxesCtrl.txtValue1.Items.Add(obj);
              this.textBoxesCtrl.txtValue2.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (RadioProtocol)))
                this.textBoxesCtrl.txtValue2.Items.Add(obj);
              this.textBoxesCtrl.txtValue1.SelectedIndex = 0;
              this.textBoxesCtrl.txtValue2.SelectedIndex = 0;
              return;
            case SCGiCommand.RegisterMessUnit:
              num = 1;
              break;
            default:
              num = cmd == SCGiCommand.InfoOfRegisteredMessUnit ? 1 : 0;
              break;
          }
          if (num != 0)
          {
            this.textBoxesCtrl.txtValue2.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (object obj in Enum.GetValues(typeof (MeasurementCategory)))
              this.textBoxesCtrl.txtValue2.Items.Add(obj);
            this.textBoxesCtrl.txtValue3.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (object obj in Enum.GetValues(typeof (MeasurementValueType)))
              this.textBoxesCtrl.txtValue3.Items.Add(obj);
            this.textBoxesCtrl.txtValue4.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (object obj in Enum.GetValues(typeof (RadioProtocol)))
              this.textBoxesCtrl.txtValue4.Items.Add(obj);
            if (cmd == SCGiCommand.InfoOfRegisteredMessUnit)
            {
              this.textBoxesCtrl.txtValue2.Enabled = false;
              this.textBoxesCtrl.txtValue3.Enabled = false;
              this.textBoxesCtrl.txtValue4.Enabled = false;
            }
            this.textBoxesCtrl.txtValue2.SelectedIndex = 0;
            this.textBoxesCtrl.txtValue3.SelectedIndex = 0;
            this.textBoxesCtrl.txtValue4.SelectedIndex = 0;
            break;
          }
          switch (cmd)
          {
            case SCGiCommand.StartNetworkSetup:
              this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (NetworkSetupMode)))
                this.textBoxesCtrl.txtValue1.Items.Add(obj);
              this.textBoxesCtrl.txtValue1.SelectedIndex = 0;
              return;
            case SCGiCommand.RegisterOrDeregisterSlave:
              this.textBoxesCtrl.btnGet.Text = "Register";
              this.textBoxesCtrl.btnSet.Text = "Deregister";
              this.textBoxesCtrl.btnSet.Image = (Image) Resources.RemoveButton;
              return;
            case SCGiCommand.ActionTimepoint:
              this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (ActionMode)))
                this.textBoxesCtrl.txtValue1.Items.Add(obj);
              this.textBoxesCtrl.txtValue2.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (ActionTimepointType)))
                this.textBoxesCtrl.txtValue2.Items.Add(obj);
              this.textBoxesCtrl.txtValue3.Text = DateTime.Now.ToString();
              this.textBoxesCtrl.txtValue1.SelectedIndex = 0;
              this.textBoxesCtrl.txtValue2.SelectedIndex = 0;
              return;
            case SCGiCommand.LED:
              this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
              this.textBoxesCtrl.txtValue1.Items.Add((object) "ON");
              this.textBoxesCtrl.txtValue1.Items.Add((object) "OFF");
              this.textBoxesCtrl.txtValue2.DropDownStyle = ComboBoxStyle.DropDownList;
              this.textBoxesCtrl.txtValue2.Items.Add((object) "ON");
              this.textBoxesCtrl.txtValue2.Items.Add((object) "OFF");
              this.textBoxesCtrl.txtValue3.DropDownStyle = ComboBoxStyle.DropDownList;
              this.textBoxesCtrl.txtValue3.Items.Add((object) "ON");
              this.textBoxesCtrl.txtValue3.Items.Add((object) "OFF");
              this.textBoxesCtrl.txtValue4.DropDownStyle = ComboBoxStyle.DropDownList;
              this.textBoxesCtrl.txtValue4.Items.Add((object) "ON");
              this.textBoxesCtrl.txtValue4.Items.Add((object) "OFF");
              this.textBoxesCtrl.txtValue1.SelectedIndex = 0;
              this.textBoxesCtrl.txtValue2.SelectedIndex = 0;
              this.textBoxesCtrl.txtValue3.SelectedIndex = 0;
              this.textBoxesCtrl.txtValue4.SelectedIndex = 0;
              return;
            case SCGiCommand.ModemUpdate:
              this.textBoxesCtrl.txtValue1.Text = "http://fetest.minolforyou.com/modem-2.1.11.dwl";
              return;
            case SCGiCommand.ModemCounter:
              this.textBoxesCtrl.btnGet.Text = "Read";
              this.textBoxesCtrl.btnSet.Text = "Reset";
              this.textBoxesCtrl.btnSet.Image = (Image) Resources.RemoveButton;
              this.textBoxesCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (object obj in Enum.GetValues(typeof (ModemCounterType)))
                this.textBoxesCtrl.txtValue1.Items.Add(obj);
              return;
            default:
              return;
          }
        case MinomatHandlerWindow.UserCtrl.DataTable:
          this.dataTableCtrl.Visible = true;
          this.dataTableCtrl.table.BackColor = Color.WhiteSmoke;
          this.dataTableCtrl.btnGet.Tag = (object) cmd.ToString();
          this.dataTableCtrl.lblTitel.Text = titel;
          this.dataTableCtrl.lblCMD.Text = SCGiCommandManager.GetAsHexString(cmd);
          this.dataTableCtrl.table.DataSource = (object) null;
          this.dataTableCtrl.lblValueName1.Text = valueName1;
          this.dataTableCtrl.lblValueName1.Visible = !string.IsNullOrEmpty(valueName1);
          this.dataTableCtrl.txtValue1.Visible = this.dataTableCtrl.lblValueName1.Visible;
          this.dataTableCtrl.txtValue1.Text = string.Empty;
          this.dataTableCtrl.txtValue1.BackColor = SystemColors.Window;
          this.dataTableCtrl.txtValue1.SelectedItem = (object) null;
          this.dataTableCtrl.txtValue1.Items.Clear();
          this.dataTableCtrl.txtValue1.DropDownStyle = ComboBoxStyle.Simple;
          this.dataTableCtrl.txtValue1.Enabled = true;
          if (cmd != SCGiCommand.RegisteredSlaves)
            break;
          this.dataTableCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
          foreach (object obj in Enum.GetValues(typeof (SlaveType)))
            this.dataTableCtrl.txtValue1.Items.Add(obj);
          break;
        case MinomatHandlerWindow.UserCtrl.RichTextBox:
          this.richTextBoxCtrl.Visible = true;
          this.richTextBoxCtrl.btnGet.Tag = (object) cmd.ToString();
          this.richTextBoxCtrl.lblTitel.Text = titel;
          this.richTextBoxCtrl.btnStop.Visible = false;
          this.richTextBoxCtrl.txtContent.Text = string.Empty;
          this.richTextBoxCtrl.lblValueName1.Text = valueName1;
          this.richTextBoxCtrl.lblValueName1.Visible = !string.IsNullOrEmpty(valueName1);
          this.richTextBoxCtrl.txtValue1.Visible = this.richTextBoxCtrl.lblValueName1.Visible;
          this.richTextBoxCtrl.txtValue1.Text = string.Empty;
          this.richTextBoxCtrl.txtValue1.BackColor = SystemColors.Window;
          this.richTextBoxCtrl.txtValue1.SelectedItem = (object) null;
          this.richTextBoxCtrl.txtValue1.Items.Clear();
          this.richTextBoxCtrl.txtValue1.DropDownStyle = ComboBoxStyle.Simple;
          this.richTextBoxCtrl.txtValue1.Enabled = true;
          this.richTextBoxCtrl.lblValueName2.Text = valueName2;
          this.richTextBoxCtrl.lblValueName2.Visible = !string.IsNullOrEmpty(valueName2);
          this.richTextBoxCtrl.txtValue2.Visible = this.richTextBoxCtrl.lblValueName2.Visible;
          this.richTextBoxCtrl.txtValue2.Text = string.Empty;
          this.richTextBoxCtrl.txtValue2.BackColor = SystemColors.Window;
          this.richTextBoxCtrl.txtValue2.SelectedItem = (object) null;
          this.richTextBoxCtrl.txtValue2.Items.Clear();
          this.richTextBoxCtrl.txtValue2.DropDownStyle = ComboBoxStyle.Simple;
          this.richTextBoxCtrl.txtValue2.Enabled = true;
          if (cmd == SCGiCommand.GetComServerFile)
          {
            this.richTextBoxCtrl.txtValue1.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (object obj in Enum.GetValues(typeof (ComServerFileType)))
              this.richTextBoxCtrl.txtValue1.Items.Add(obj);
            this.richTextBoxCtrl.txtValue1.SelectedIndex = 0;
          }
          if (cmd != SCGiCommand.Flash)
            break;
          this.richTextBoxCtrl.txtValue1.Text = "0";
          this.richTextBoxCtrl.txtValue2.Text = "0";
          break;
        case MinomatHandlerWindow.UserCtrl.DataTableWithDateTime:
          this.dataTableWithDateTimeCtrl.Visible = true;
          this.dataTableWithDateTimeCtrl.table.BackColor = Color.WhiteSmoke;
          this.dataTableWithDateTimeCtrl.btnGet.Tag = (object) cmd.ToString();
          this.dataTableWithDateTimeCtrl.lblTitel.Text = titel;
          this.dataTableWithDateTimeCtrl.lblCMD.Text = SCGiCommandManager.GetAsHexString(cmd);
          this.dataTableWithDateTimeCtrl.table.DataSource = (object) null;
          this.dataTableWithDateTimeCtrl.ckboxValue1.Visible = false;
          this.dataTableWithDateTimeCtrl.lblValueName1.Text = valueName1;
          this.dataTableWithDateTimeCtrl.lblValueName1.Visible = !string.IsNullOrEmpty(valueName1);
          this.dataTableWithDateTimeCtrl.txtValue1.Visible = this.dataTableWithDateTimeCtrl.lblValueName1.Visible;
          this.dataTableWithDateTimeCtrl.txtValue1.Text = string.Empty;
          this.dataTableWithDateTimeCtrl.txtValue1.BackColor = SystemColors.Window;
          this.dataTableWithDateTimeCtrl.txtValue1.SelectedItem = (object) null;
          this.dataTableWithDateTimeCtrl.txtValue1.Items.Clear();
          this.dataTableWithDateTimeCtrl.txtValue1.DropDownStyle = ComboBoxStyle.Simple;
          this.dataTableWithDateTimeCtrl.txtValue1.Enabled = true;
          this.dataTableWithDateTimeCtrl.lblValueName2.Text = valueName2;
          this.dataTableWithDateTimeCtrl.lblValueName2.Visible = !string.IsNullOrEmpty(valueName2);
          this.dataTableWithDateTimeCtrl.txtValue2.Visible = this.dataTableWithDateTimeCtrl.lblValueName2.Visible;
          this.dataTableWithDateTimeCtrl.txtValue2.Text = string.Empty;
          this.dataTableWithDateTimeCtrl.txtValue2.BackColor = SystemColors.Window;
          this.dataTableWithDateTimeCtrl.txtValue2.SelectedItem = (object) null;
          this.dataTableWithDateTimeCtrl.txtValue2.Items.Clear();
          this.dataTableWithDateTimeCtrl.txtValue2.DropDownStyle = ComboBoxStyle.Simple;
          this.dataTableWithDateTimeCtrl.txtValue2.Enabled = true;
          this.dataTableWithDateTimeCtrl.lblValueName3.Text = valueName3;
          this.dataTableWithDateTimeCtrl.lblValueName3.Visible = !string.IsNullOrEmpty(valueName3);
          this.dataTableWithDateTimeCtrl.txtValue3.Visible = this.dataTableWithDateTimeCtrl.lblValueName3.Visible;
          this.dataTableWithDateTimeCtrl.txtValue3.Text = string.Empty;
          this.dataTableWithDateTimeCtrl.txtValue3.BackColor = SystemColors.Window;
          this.dataTableWithDateTimeCtrl.txtValue3.Enabled = true;
          this.dataTableWithDateTimeCtrl.lblValueName4.Text = valueName4;
          this.dataTableWithDateTimeCtrl.lblValueName4.Visible = !string.IsNullOrEmpty(valueName4);
          this.dataTableWithDateTimeCtrl.txtValue4.Visible = this.dataTableWithDateTimeCtrl.lblValueName4.Visible;
          this.dataTableWithDateTimeCtrl.txtValue4.Text = string.Empty;
          this.dataTableWithDateTimeCtrl.txtValue4.BackColor = SystemColors.Window;
          this.dataTableWithDateTimeCtrl.txtValue4.Enabled = true;
          this.dataTableWithDateTimeCtrl.progress.Visible = false;
          if (cmd != SCGiCommand.MeasurementData)
            break;
          this.dataTableWithDateTimeCtrl.btnValue1.Tag = (object) cmd;
          this.dataTableWithDateTimeCtrl.btnValue1.Click += new System.EventHandler(this.DataTableWithDateTime_btnValue1_Click);
          this.dataTableWithDateTimeCtrl.txtValue2.DropDownStyle = ComboBoxStyle.DropDownList;
          foreach (object obj in Enum.GetValues(typeof (MeasurementDataType)))
            this.dataTableWithDateTimeCtrl.txtValue2.Items.Add(obj);
          this.dataTableWithDateTimeCtrl.txtValue2.SelectedIndex = 2;
          this.dataTableWithDateTimeCtrl.txtValue3.Value = new DateTime(2011, 1, 1);
          this.dataTableWithDateTimeCtrl.txtValue4.Value = DateTime.Now;
          this.dataTableWithDateTimeCtrl.ckboxValue1.Visible = true;
          this.dataTableWithDateTimeCtrl.ckboxValue1.Text = "Use Bulk-Query";
          this.dataTableWithDateTimeCtrl.ckboxValue1.Checked = false;
          break;
        default:
          throw new NotImplementedException("Control: " + control.ToString());
      }
    }

    private void DataTableWithDateTime_btnValue1_Click(object sender, EventArgs e)
    {
      try
      {
        if ((SCGiCommand) Enum.Parse(typeof (SCGiCommand), (sender as Button).Tag.ToString()) != SCGiCommand.MeasurementData)
          return;
        int num1 = (int) MessageBox.Show("Please press the button on Minomat.", "Press button", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        Cursor.Current = Cursors.WaitCursor;
        List<uint> registeredMessUnits = this.minomatV4.GetRegisteredMessUnits();
        GenericCheckBoxForm genericCheckBoxForm = new GenericCheckBoxForm();
        if (registeredMessUnits != null)
        {
          registeredMessUnits.Sort();
          foreach (uint num2 in registeredMessUnits)
            genericCheckBoxForm.items.Items.Add((object) num2, false);
        }
        if (genericCheckBoxForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (object checkedItem in genericCheckBoxForm.items.CheckedItems)
            stringBuilder.Append(checkedItem).Append(", ");
          this.dataTableWithDateTimeCtrl.txtValue1.Text = stringBuilder.ToString();
        }
        else
          this.dataTableWithDateTimeCtrl.txtValue1.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void SetControlDefaultSettings(bool clearControlContent)
    {
      this.textBoxesCtrl.txtValue1.BackColor = SystemColors.Window;
      this.textBoxesCtrl.txtValue2.BackColor = SystemColors.Window;
      this.textBoxesCtrl.txtValue3.BackColor = SystemColors.Window;
      this.textBoxesCtrl.txtValue4.BackColor = SystemColors.Window;
      this.textBoxesCtrl.txtValue5.BackColor = SystemColors.Window;
      if (!clearControlContent)
        return;
      this.textBoxesCtrl.txtValue1.Text = string.Empty;
      this.textBoxesCtrl.txtValue2.Text = string.Empty;
      this.textBoxesCtrl.txtValue3.Text = string.Empty;
      this.textBoxesCtrl.txtValue4.Text = string.Empty;
      this.textBoxesCtrl.txtValue5.Text = string.Empty;
      this.textBoxesCtrl.txtValue1.SelectedIndex = -1;
      this.textBoxesCtrl.txtValue2.SelectedIndex = -1;
      this.textBoxesCtrl.txtValue3.SelectedIndex = -1;
      this.textBoxesCtrl.txtValue4.SelectedIndex = -1;
      this.textBoxesCtrl.txtValue5.SelectedIndex = -1;
      this.richTextBoxCtrl.txtContent.Text = string.Empty;
    }

    private void TextBoxesControl_ButtonGet_Click(object sender, EventArgs e)
    {
      this.lblStatus.Text = string.Empty;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        if (!this.minomatV4.Connection.Open())
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        else
        {
          bool flag1 = true;
          SCGiCommand scGiCommand = (SCGiCommand) Enum.Parse(typeof (SCGiCommand), (sender as Button).Tag.ToString());
          this.txtOutput.Clear();
          this.txtOutput.AppendText(scGiCommand.ToString());
          this.txtOutput.AppendText(Environment.NewLine);
          switch (scGiCommand)
          {
            case SCGiCommand.ResetConfigurationState:
              this.SetControlDefaultSettings(true);
              this.textBoxesCtrl.txtValue1.Text = this.minomatV4.GetResetConfigurationState().ToString();
              break;
            case SCGiCommand.MinolId:
              this.SetControlDefaultSettings(true);
              uint? minolId = this.minomatV4.GetMinolId();
              if (minolId.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = minolId.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.NodeId:
              this.SetControlDefaultSettings(true);
              ushort? nodeId = this.minomatV4.GetNodeId();
              if (nodeId.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = nodeId.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.NetworkId:
              this.SetControlDefaultSettings(true);
              ushort? networkId = this.minomatV4.GetNetworkId();
              if (networkId.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = networkId.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.SystemTime:
              this.SetControlDefaultSettings(true);
              DateTime? systemTime = this.minomatV4.GetSystemTime();
              if (systemTime.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = systemTime.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.RadioChannel:
              this.SetControlDefaultSettings(true);
              RadioChannel radioChannel = this.minomatV4.GetRadioChannel();
              if (radioChannel != null)
              {
                this.textBoxesCtrl.txtValue2.Text = radioChannel.Description;
                if (radioChannel.Error == RadioChannelError.None && radioChannel.ID.HasValue)
                {
                  this.textBoxesCtrl.txtValue1.Text = radioChannel.ID.ToString();
                  break;
                }
                flag1 = false;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.TransceiverChannelId:
              this.SetControlDefaultSettings(true);
              ushort? transceiverChannelId = this.minomatV4.GetTransceiverChannelId();
              if (transceiverChannelId.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = transceiverChannelId.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.FirmwareVersion:
              this.SetControlDefaultSettings(true);
              string firmwareVersion = this.minomatV4.GetFirmwareVersion();
              if (!string.IsNullOrEmpty(firmwareVersion))
              {
                this.textBoxesCtrl.txtValue1.Text = firmwareVersion;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.UserappName:
              this.SetControlDefaultSettings(true);
              string userappName = this.minomatV4.GetUserappName();
              if (!string.IsNullOrEmpty(userappName))
              {
                this.textBoxesCtrl.txtValue1.Text = userappName;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.FirmwareBuildTime:
              this.SetControlDefaultSettings(true);
              string firmwareBuildTime = this.minomatV4.GetFirmwareBuildTime();
              if (!string.IsNullOrEmpty(firmwareBuildTime))
              {
                this.textBoxesCtrl.txtValue1.Text = firmwareBuildTime;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.UserappBuildTime:
              this.SetControlDefaultSettings(true);
              string userappBuildTime = this.minomatV4.GetUserappBuildTime();
              if (!string.IsNullOrEmpty(userappBuildTime))
              {
                this.textBoxesCtrl.txtValue1.Text = userappBuildTime;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.ErrorFlags:
              this.SetControlDefaultSettings(true);
              uint? errorFlags = this.minomatV4.GetErrorFlags();
              if (errorFlags.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = errorFlags.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.TransmissionPower:
              this.SetControlDefaultSettings(true);
              ushort? transmissionPower = this.minomatV4.GetTransmissionPower();
              if (transmissionPower.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = transmissionPower.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.MultiChannelSettings:
              this.SetControlDefaultSettings(true);
              MultiChannelSettings multiChannelSettings = this.minomatV4.GetMultiChannelSettings();
              if (multiChannelSettings != null)
              {
                this.textBoxesCtrl.txtValue1.Text = multiChannelSettings.MFChannel0.ToString();
                this.textBoxesCtrl.txtValue2.Text = multiChannelSettings.MFChannel1.ToString();
                ComboBox txtValue3 = this.textBoxesCtrl.txtValue3;
                byte num = multiChannelSettings.MFChannel2;
                string str1 = num.ToString();
                txtValue3.Text = str1;
                ComboBox txtValue4 = this.textBoxesCtrl.txtValue4;
                num = multiChannelSettings.MFChannel3;
                string str2 = num.ToString();
                txtValue4.Text = str2;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.TransceiverFrequencyOffset:
              this.SetControlDefaultSettings(true);
              ushort? transceiverFrequencyOffset = this.minomatV4.GetTransceiverFrequencyOffset();
              if (transceiverFrequencyOffset.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = transceiverFrequencyOffset.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.TemperatureOffset:
              this.SetControlDefaultSettings(true);
              ushort? temperatureOffset = this.minomatV4.GetTemperatureOffset();
              if (temperatureOffset.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = temperatureOffset.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.PhaseDetailsBuffer:
              this.SetControlDefaultSettings(true);
              byte[] phaseDetailsBuffer = this.minomatV4.GetPhaseDetailsBuffer();
              if (phaseDetailsBuffer != null)
              {
                this.textBoxesCtrl.txtValue1.Text = Util.ByteArrayToHexString(phaseDetailsBuffer);
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.PhaseDetails:
              this.SetControlDefaultSettings(true);
              PhaseDetails phaseDetails = this.minomatV4.GetPhaseDetails();
              if (phaseDetails != null)
              {
                this.textBoxesCtrl.txtValue1.SelectedText = phaseDetails.Phase.ToString();
                this.textBoxesCtrl.txtValue2.SelectedText = phaseDetails.State.ToString();
                this.textBoxesCtrl.txtValue3.SelectedText = phaseDetails.SubPhase.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.RestartMinomat:
              if (this.minomatV4.RestartMinomat())
              {
                int num = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num1 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.MessUnitNumberMax:
              this.SetControlDefaultSettings(true);
              ushort? messUnitNumberMax = this.minomatV4.GetMessUnitNumberMax();
              if (messUnitNumberMax.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = messUnitNumberMax.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.MaxMessUnitNumberNotConfigured:
              this.SetControlDefaultSettings(true);
              ushort? notConfiguredMax = this.minomatV4.GetMessUnitNumberNotConfiguredMax();
              if (notConfiguredMax.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = notConfiguredMax.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.Scenario:
              this.SetControlDefaultSettings(true);
              Scenario? scenario = this.minomatV4.GetScenario();
              if (scenario.HasValue)
              {
                this.textBoxesCtrl.txtValue1.SelectedItem = (object) scenario;
                break;
              }
              break;
            case SCGiCommand.StartTestReception:
              if (this.minomatV4.StartTestReception(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text))
              {
                int num2 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num3 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.RegisterMessUnit:
              List<uint> registeredMessUnits = this.minomatV4.GetRegisteredMessUnits();
              int num4 = 0;
              if (registeredMessUnits != null)
                num4 = registeredMessUnits.Count;
              uint? nullable = this.minomatV4.RegisterMessUnit(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text, this.textBoxesCtrl.txtValue3.Text, this.textBoxesCtrl.txtValue4.Text);
              bool flag2 = nullable.HasValue && nullable.Value > 0U;
              bool flag3 = flag2 && (long) (num4 + 1) == (long) nullable.Value;
              if (flag2 & flag3)
              {
                int num5 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num6 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.ResetConfiguration:
              if (this.minomatV4.ResetConfiguration())
              {
                int num7 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num8 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.StartNetworkSetup:
              if (this.minomatV4.StartNetworkSetup(this.textBoxesCtrl.txtValue1.Text))
              {
                int num9 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num10 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.DeleteMessUnit:
              if (this.minomatV4.DeleteMessUnit(this.textBoxesCtrl.txtValue1.Text))
              {
                int num11 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num12 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.InfoOfRegisteredMessUnit:
              MessUnit registeredMessUnit = this.minomatV4.GetInfoOfRegisteredMessUnit(this.textBoxesCtrl.txtValue1.Text);
              if (registeredMessUnit != null)
              {
                this.textBoxesCtrl.txtValue1.Text = registeredMessUnit.ID.ToString();
                this.textBoxesCtrl.txtValue2.SelectedItem = (object) registeredMessUnit.Category;
                this.textBoxesCtrl.txtValue3.SelectedItem = (object) registeredMessUnit.Type;
                this.textBoxesCtrl.txtValue4.SelectedItem = (object) registeredMessUnit.Protocol;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.SimPin:
              this.SetControlDefaultSettings(true);
              string simPin = this.minomatV4.GetSimPin();
              if (simPin != null)
              {
                this.textBoxesCtrl.txtValue1.Text = simPin;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.MasterMinolID:
              this.SetControlDefaultSettings(true);
              uint? masterMinolIdOfModem = this.minomatV4.GetMasterMinolIDOfModem();
              if (masterMinolIdOfModem.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = masterMinolIdOfModem.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.APN:
              this.SetControlDefaultSettings(true);
              string apn = this.minomatV4.GetAPN();
              if (apn != null)
              {
                this.textBoxesCtrl.txtValue1.Text = apn;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.GPRSUserName:
              this.SetControlDefaultSettings(true);
              string gprsUserName = this.minomatV4.GetGPRSUserName();
              if (gprsUserName != null)
              {
                this.textBoxesCtrl.txtValue1.Text = gprsUserName;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.GPRSPassword:
              this.SetControlDefaultSettings(true);
              string gprsPassword = this.minomatV4.GetGPRSPassword();
              if (gprsPassword != null)
              {
                this.textBoxesCtrl.txtValue1.Text = gprsPassword;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.HttpServer:
              this.SetControlDefaultSettings(true);
              EndPoint httpServer = this.minomatV4.GetHttpServer();
              if (httpServer != null)
              {
                this.textBoxesCtrl.txtValue1.Text = httpServer.Port.ToString();
                this.textBoxesCtrl.txtValue2.Text = httpServer.Servername;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.HttpResourceName:
              this.SetControlDefaultSettings(true);
              this.textBoxesCtrl.txtValue1.Text = this.minomatV4.GetHttpResourceName();
              break;
            case SCGiCommand.StartGSMTestReception:
              if (this.minomatV4.StartGSMTestReception())
              {
                int num13 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num14 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.GSMTestReceptionState:
              this.SetControlDefaultSettings(true);
              this.textBoxesCtrl.txtValue1.Text = this.minomatV4.GetGSMTestReceptionState().ToString();
              break;
            case SCGiCommand.ForceNetworkOptimization:
              this.SetControlDefaultSettings(true);
              this.textBoxesCtrl.txtValue1.Text = this.minomatV4.ForceNetworkOptimisation().ToString();
              break;
            case SCGiCommand.StartNetworkOptimization:
              if (this.minomatV4.StartNetworkOptimization())
              {
                int num15 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num16 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.RegisterOrDeregisterSlave:
              if (this.minomatV4.RegisterSlave(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text))
              {
                int num17 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num18 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.AppInitialSettings:
              this.SetControlDefaultSettings(true);
              AppInitialSettings appInitialSettings = this.minomatV4.GetAppInitialSettings();
              if (appInitialSettings != null)
              {
                this.textBoxesCtrl.txtValue1.Text = appInitialSettings.Challenge.ToString();
                this.textBoxesCtrl.txtValue2.Text = appInitialSettings.GsmId.ToString();
                this.textBoxesCtrl.txtValue3.Text = appInitialSettings.SapConfigNumber.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.ActionTimepoint:
              if (this.minomatV4.SetActionTimepoint(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text, this.textBoxesCtrl.txtValue3.Text))
              {
                int num19 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num20 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.HttpState:
              this.SetControlDefaultSettings(true);
              HttpState httpState = this.minomatV4.GetHttpState();
              if (httpState != null)
              {
                this.textBoxesCtrl.txtValue1.Text = httpState.State.ToString();
                this.textBoxesCtrl.txtValue2.Text = httpState.Endpoint.Port.ToString();
                this.textBoxesCtrl.txtValue3.Text = httpState.Endpoint.Servername;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.GsmState:
              this.SetControlDefaultSettings(true);
              GsmState gsmState = this.minomatV4.GetGsmState();
              if (gsmState != null)
              {
                this.textBoxesCtrl.txtValue1.Text = gsmState.StateA.ToString();
                this.textBoxesCtrl.txtValue2.Text = gsmState.StateB.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.ModemBuildDate:
              this.SetControlDefaultSettings(true);
              this.textBoxesCtrl.txtValue1.Text = this.minomatV4.GetModemBuildDate();
              break;
            case SCGiCommand.StartHttpConnection:
              if (this.minomatV4.StartHttpConnection())
              {
                int num21 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num22 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.SwitchToNetworkModel:
              if (this.minomatV4.SwitchToNetworkModel())
              {
                int num23 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num24 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.MessUnitMetadata:
              MessUnitMetadata messUnitMetadata = this.minomatV4.GetMessUnitMetadata(this.textBoxesCtrl.txtValue1.Text);
              if (messUnitMetadata != null)
              {
                this.textBoxesCtrl.txtValue1.Text = messUnitMetadata.ID.ToString();
                this.textBoxesCtrl.txtValue2.Text = messUnitMetadata.Timepoint.ToString();
                ComboBox txtValue3 = this.textBoxesCtrl.txtValue3;
                DateTime timepointSlave = messUnitMetadata.TimepointSlave;
                string str3 = timepointSlave.ToString();
                txtValue3.Text = str3;
                this.textBoxesCtrl.txtValue4.Text = messUnitMetadata.State.ToString();
                if (messUnitMetadata.TimepointError.HasValue)
                {
                  ComboBox txtValue5 = this.textBoxesCtrl.txtValue5;
                  timepointSlave = messUnitMetadata.TimepointError.Value;
                  string str4 = timepointSlave.ToString();
                  txtValue5.Text = str4;
                  break;
                }
                this.textBoxesCtrl.txtValue5.Text = "NULL";
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.ActivateModemAT_Mode:
              if (this.minomatV4.ActivateModemAT_Mode())
              {
                int num25 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num26 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.LED:
              if (this.minomatV4.SetLED(this.textBoxesCtrl.txtValue1.Text == "ON", this.textBoxesCtrl.txtValue2.Text == "ON", this.textBoxesCtrl.txtValue3.Text == "ON", this.textBoxesCtrl.txtValue4.Text == "ON"))
              {
                int num27 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num28 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.ModemUpdateImageClear:
              if (this.minomatV4.ModemUpdateImageClear())
              {
                int num29 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num30 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.ModemUpdate:
              if (this.minomatV4.ModemUpdate(this.textBoxesCtrl.txtValue1.Text))
              {
                int num31 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num32 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.ModemReboot:
              if (this.minomatV4.ModemReboot())
              {
                int num33 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num34 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            case SCGiCommand.ModemCounter:
              ModemCounter modemCounter = this.minomatV4.GetModemCounter(this.textBoxesCtrl.txtValue1.Text);
              if (modemCounter != null)
              {
                this.textBoxesCtrl.txtValue1.Text = modemCounter.Type.ToString();
                ComboBox txtValue2 = this.textBoxesCtrl.txtValue2;
                ushort errors = modemCounter.Value;
                string str5 = errors.ToString();
                txtValue2.Text = str5;
                ComboBox txtValue3 = this.textBoxesCtrl.txtValue3;
                errors = modemCounter.Errors;
                string str6 = errors.ToString();
                txtValue3.Text = str6;
                DateTime? timepoint = modemCounter.Timepoint;
                if (timepoint.HasValue)
                {
                  ComboBox txtValue4 = this.textBoxesCtrl.txtValue4;
                  timepoint = modemCounter.Timepoint;
                  string str7 = timepoint.ToString();
                  txtValue4.Text = str7;
                  break;
                }
                this.textBoxesCtrl.txtValue4.Text = string.Empty;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.TcpConfiguration:
              this.SetControlDefaultSettings(true);
              EndPoint tcpConfiguration = this.minomatV4.GetTcpConfiguration();
              if (tcpConfiguration != null)
              {
                this.textBoxesCtrl.txtValue1.Text = tcpConfiguration.Port.ToString();
                this.textBoxesCtrl.txtValue2.Text = tcpConfiguration.Servername;
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.SendSMS:
              flag1 = this.minomatV4.SendSMS(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text);
              break;
            case SCGiCommand.ModemDueDate:
              DateTime? modemDueDate = this.minomatV4.GetModemDueDate();
              if (modemDueDate.HasValue)
              {
                this.textBoxesCtrl.txtValue1.Text = modemDueDate.Value.ToShortDateString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.GsmLinkQuality:
              this.SetControlDefaultSettings(true);
              this.textBoxesCtrl.txtValue1.Text = this.minomatV4.GetGsmLinkQuality().ToString();
              break;
            case SCGiCommand.ModemUpdateTiming:
              ModemUpdateTiming modemUpdateTiming = this.minomatV4.GetModemUpdateTiming();
              if (modemUpdateTiming != null)
              {
                this.textBoxesCtrl.txtValue1.Text = modemUpdateTiming.StartTime.ToString();
                this.textBoxesCtrl.txtValue2.Text = modemUpdateTiming.FinishTime.ToString();
                break;
              }
              flag1 = false;
              break;
            case SCGiCommand.ModemUpdateTest:
              if (this.minomatV4.ModemUpdateTest(this.textBoxesCtrl.txtValue1.Text))
              {
                int num35 = (int) MessageBox.Show("Successful", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                break;
              }
              int num36 = (int) MessageBox.Show("Failed", "Execute", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
            default:
              throw new NotImplementedException("CMD: " + scGiCommand.ToString());
          }
          this.textBoxesCtrl.txtValue1.BackColor = flag1 ? Color.LightGreen : Color.LightPink;
          this.textBoxesCtrl.txtValue2.BackColor = flag1 ? Color.LightGreen : Color.LightPink;
          this.textBoxesCtrl.txtValue3.BackColor = flag1 ? Color.LightGreen : Color.LightPink;
          this.textBoxesCtrl.txtValue4.BackColor = flag1 ? Color.LightGreen : Color.LightPink;
          this.textBoxesCtrl.txtValue5.BackColor = flag1 ? Color.LightGreen : Color.LightPink;
        }
      }
      catch
      {
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void TextBoxesControl_ButtonSet_Click(object sender, EventArgs e)
    {
      this.lblStatus.Text = string.Empty;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.SetControlDefaultSettings(false);
        if (!this.minomatV4.Connection.Open())
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        else
        {
          SCGiCommand scGiCommand = (SCGiCommand) Enum.Parse(typeof (SCGiCommand), (sender as Button).Tag.ToString());
          this.txtOutput.Clear();
          this.txtOutput.AppendText(scGiCommand.ToString());
          this.txtOutput.AppendText(Environment.NewLine);
          bool flag;
          switch (scGiCommand)
          {
            case SCGiCommand.MinolId:
              flag = this.minomatV4.SetMinolId(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.NodeId:
              flag = this.minomatV4.SetNodeId(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.NetworkId:
              flag = this.minomatV4.SetNetworkId(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.SystemTime:
              flag = this.minomatV4.SetSystemTime(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.RadioChannel:
              flag = this.minomatV4.SetRadioChannel(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.TransceiverChannelId:
              flag = this.minomatV4.SetTransceiverChannelId(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.TransmissionPower:
              flag = this.minomatV4.SetTransmissionPower(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.MultiChannelSettings:
              flag = this.minomatV4.SetMultiChannelSettings(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text, this.textBoxesCtrl.txtValue3.Text, this.textBoxesCtrl.txtValue4.Text);
              break;
            case SCGiCommand.TransceiverFrequencyOffset:
              flag = this.minomatV4.SetTransceiverFrequencyOffset(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.TemperatureOffset:
              flag = this.minomatV4.SetTemperatureOffset(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.PhaseDetailsBuffer:
              flag = this.minomatV4.SetPhaseDetailsBuffer(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.MaxMessUnitNumberNotConfigured:
              flag = this.minomatV4.SetMessUnitNumberNotConfiguredMax(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.Scenario:
              flag = !(this.textBoxesCtrl.txtValue2.Text == "4 Byte") ? this.minomatV4.SetScenario(this.textBoxesCtrl.txtValue1.Text) : this.minomatV4.SetScenario_1Byte(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.SimPin:
              flag = this.minomatV4.SetSimPin(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.MasterMinolID:
              flag = this.minomatV4.SetMasterMinolIDOfModem(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.APN:
              flag = this.minomatV4.SetAPN(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.GPRSUserName:
              flag = this.minomatV4.SetGPRSUserName(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.GPRSPassword:
              flag = this.minomatV4.SetGPRSPassword(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.HttpServer:
              flag = this.minomatV4.SetHttpServer(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text);
              break;
            case SCGiCommand.HttpResourceName:
              flag = this.minomatV4.SetHttpResourceName(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.RegisterOrDeregisterSlave:
              flag = this.minomatV4.DeregisterSlave(this.textBoxesCtrl.txtValue2.Text);
              break;
            case SCGiCommand.AppInitialSettings:
              flag = this.minomatV4.SetAppInitialSettings(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text, this.textBoxesCtrl.txtValue3.Text, this.textBoxesCtrl.txtValue4.Text);
              break;
            case SCGiCommand.ModemCounter:
              flag = this.minomatV4.ResetModemCounter(this.textBoxesCtrl.txtValue1.Text);
              break;
            case SCGiCommand.TcpConfiguration:
              flag = this.minomatV4.SetTcpConfiguration(this.textBoxesCtrl.txtValue1.Text, this.textBoxesCtrl.txtValue2.Text);
              break;
            case SCGiCommand.ModemDueDate:
              flag = this.minomatV4.SetModemDueDate(this.textBoxesCtrl.txtValue1.Text);
              break;
            default:
              throw new NotImplementedException("CMD: " + scGiCommand.ToString());
          }
          if (flag)
          {
            int num1 = (int) MessageBox.Show("Successful!", "Write", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            this.textBoxesCtrl.txtValue1.BackColor = Color.LightPink;
            int num2 = (int) MessageBox.Show("Failed!", "Write", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      catch
      {
        this.textBoxesCtrl.txtValue1.BackColor = Color.LightPink;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void DataTableControl_ButtonGet_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.lblStatus.Text = string.Empty;
      this.dataTableCtrl.table.BackColor = Color.WhiteSmoke;
      this.dataTableCtrl.table.DataSource = (object) null;
      this.dataTableWithDateTimeCtrl.isCanceled = false;
      this.dataTableWithDateTimeCtrl.btnGet.Enabled = false;
      this.progress.Visible = false;
      this.progress.Value = 0;
      try
      {
        if (!this.minomatV4.Connection.Open())
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        else
        {
          bool flag = true;
          SCGiCommand scGiCommand = (SCGiCommand) Enum.Parse(typeof (SCGiCommand), (sender as Button).Tag.ToString());
          this.txtOutput.Clear();
          this.txtOutput.AppendText(scGiCommand.ToString());
          this.txtOutput.AppendText(Environment.NewLine);
          switch (scGiCommand)
          {
            case SCGiCommand.RoutingTable:
              RoutingTable routingTable = this.minomatV4.GetRoutingTable();
              if (routingTable != null)
              {
                this.dataTableCtrl.table.DataSource = (object) routingTable.CreateTable();
                break;
              }
              flag = false;
              break;
            case SCGiCommand.TestReceptionResult:
              TestReceptionResult testReceptionResult = this.minomatV4.GetTestReceptionResult();
              if (testReceptionResult != null)
              {
                this.dataTableCtrl.table.DataSource = (object) testReceptionResult.CreateTable();
                break;
              }
              flag = false;
              break;
            case SCGiCommand.RegisteredSlaves:
              TableOfSlaves registeredSlaves = this.minomatV4.GetRegisteredSlaves(this.dataTableCtrl.txtValue1.Text);
              if (registeredSlaves != null)
              {
                this.dataTableCtrl.table.DataSource = (object) registeredSlaves.CreateTable();
                break;
              }
              flag = false;
              break;
            case SCGiCommand.MeasurementData:
              try
              {
                this.dataTableWithDateTimeCtrl.btnStop.Visible = true;
                this.minomatV4.OnMeasurementDataReceived += new EventHandler<MeasurementData>(this.MinomatV4_OnMeasurementDataReceived);
                this.dataTableWithDateTimeCtrl.table.Columns.Clear();
                this.dataTableWithDateTimeCtrl.table.Columns.Add("ID", "ID");
                this.dataTableWithDateTimeCtrl.table.Columns.Add("Timepoint", "Timepoint");
                this.dataTableWithDateTimeCtrl.table.Columns.Add("Type", "Type");
                this.dataTableWithDateTimeCtrl.table.Columns.Add("Value", "Value");
                if (this.minomatV4.GetMeasurementData(this.dataTableWithDateTimeCtrl.txtValue1.Text, this.dataTableWithDateTimeCtrl.txtValue2.Text, this.dataTableWithDateTimeCtrl.txtValue3.Text, this.dataTableWithDateTimeCtrl.txtValue4.Text, this.dataTableWithDateTimeCtrl.ckboxValue1.Checked) == null)
                {
                  flag = false;
                  break;
                }
                break;
              }
              finally
              {
                this.minomatV4.OnMeasurementDataReceived -= new EventHandler<MeasurementData>(this.MinomatV4_OnMeasurementDataReceived);
                this.dataTableWithDateTimeCtrl.btnStop.Visible = false;
              }
            default:
              throw new NotImplementedException("CMD: " + scGiCommand.ToString());
          }
          this.dataTableCtrl.table.BackColor = flag ? Color.LightGreen : Color.LightPink;
        }
      }
      catch
      {
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.dataTableWithDateTimeCtrl.isCanceled = false;
        this.dataTableWithDateTimeCtrl.btnGet.Enabled = true;
        this.progress.Visible = false;
        this.progress.Value = 0;
        this.minomatV4.CancelCurrentMethod = false;
      }
    }

    private void MinomatV4_OnMeasurementDataReceived(object sender, MeasurementData e)
    {
      ValueIdent.ValueIdPart_MeterType typeOfMinolDevice = NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice((long) e.Header.ID);
      foreach (KeyValuePair<DateTime, Decimal?> keyValuePair in e.Data)
        this.dataTableWithDateTimeCtrl.table.Rows.Add((object) e.Header.ID, (object) keyValuePair.Key, (object) typeOfMinolDevice, (object) keyValuePair.Value);
      this.dataTableWithDateTimeCtrl.Update();
    }

    private void RichTextBox_ButtonGet_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.lblStatus.Text = string.Empty;
      this.richTextBoxCtrl.txtContent.Text = string.Empty;
      this.richTextBoxCtrl.isCanceled = false;
      this.richTextBoxCtrl.btnGet.Enabled = false;
      try
      {
        if (!this.minomatV4.Connection.Open())
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        else
        {
          SCGiCommand scGiCommand = (SCGiCommand) Enum.Parse(typeof (SCGiCommand), (sender as Button).Tag.ToString());
          this.txtOutput.Clear();
          this.txtOutput.AppendText(scGiCommand.ToString());
          this.txtOutput.AppendText(Environment.NewLine);
          switch (scGiCommand)
          {
            case SCGiCommand.MinomatV4:
              try
              {
                this.minomatV4.OnMinomatV4ParameterReceived += new EventHandler<MinomatV4Parameter>(this.MinomatV4_OnMinomatV4ParameterReceived);
                DateTime now = DateTime.Now;
                this.richTextBoxCtrl.txtContent.Text = "---------------- MINOMAT V4 SETTINGS ( " + now.ToString("u") + " ) ----------------\n\n";
                this.minomatV4.ReadSettings();
                this.richTextBoxCtrl.txtContent.AppendText("\n---------------- MINOMAT V4 SETTINGS ( " + Util.ElapsedToString(DateTime.Now - now) + " ) ----------------");
                break;
              }
              finally
              {
                this.minomatV4.OnMinomatV4ParameterReceived -= new EventHandler<MinomatV4Parameter>(this.MinomatV4_OnMinomatV4ParameterReceived);
              }
            case SCGiCommand.RegisteredMessUnits:
              List<uint> registeredMessUnits = this.minomatV4.GetRegisteredMessUnits();
              if (registeredMessUnits != null)
              {
                registeredMessUnits.Sort();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (uint num in registeredMessUnits)
                  stringBuilder.Append(num).Append(", ");
                this.richTextBoxCtrl.txtContent.Text = stringBuilder.ToString();
                break;
              }
              this.richTextBoxCtrl.txtContent.Text = string.Empty;
              break;
            case SCGiCommand.Flash:
              this.richTextBoxCtrl.btnStop.Visible = true;
              ushort result1;
              if (!ushort.TryParse(this.richTextBoxCtrl.txtValue1.Text, out result1))
              {
                int num = (int) MessageBox.Show("Invalid start chip number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
              }
              ushort result2;
              if (!ushort.TryParse(this.richTextBoxCtrl.txtValue2.Text, out result2))
              {
                int num = (int) MessageBox.Show("Invalid start page number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
              }
              for (ushort chipNumber = result1; chipNumber < ushort.MaxValue; ++chipNumber)
              {
                for (ushort pageNumber = result2; pageNumber < ushort.MaxValue; ++pageNumber)
                {
                  this.richTextBoxCtrl.txtValue1.Text = chipNumber.ToString();
                  this.richTextBoxCtrl.txtValue2.Text = pageNumber.ToString();
                  this.richTextBoxCtrl.txtContent.AppendText("Chip: " + chipNumber.ToString());
                  this.richTextBoxCtrl.txtContent.AppendText(" Page:" + pageNumber.ToString());
                  this.richTextBoxCtrl.txtContent.AppendText(" ");
                  try
                  {
                    if (this.richTextBoxCtrl.isCanceled)
                      return;
                    FlashBlock flash = this.minomatV4.GetFlash(chipNumber, pageNumber);
                    if (flash == null)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("not exist!");
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                      break;
                    }
                    if ((int) flash.ChipNumber != (int) chipNumber)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("error: Wrong chip number received!");
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                      break;
                    }
                    if ((int) flash.PageNumber != (int) pageNumber)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("error: Wrong page number received!");
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                      break;
                    }
                    byte[] array = flash.ToArray();
                    bool flag = true;
                    for (int index = 0; index < array.Length & flag; ++index)
                    {
                      if (array[index] != byte.MaxValue)
                      {
                        flag = false;
                        break;
                      }
                    }
                    if (!flag)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText(Util.ByteArrayToHexString(array));
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                    }
                    else
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("empty (0xFF), Size: " + array.Length.ToString());
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                    }
                  }
                  catch (Exception ex)
                  {
                    this.richTextBoxCtrl.txtContent.AppendText("error:" + ex.ToString());
                    this.richTextBoxCtrl.txtContent.AppendText("\n");
                  }
                  Application.DoEvents();
                }
              }
              break;
            case SCGiCommand.Eeprom:
              this.richTextBoxCtrl.btnStop.Visible = true;
              ushort result3;
              if (!ushort.TryParse(this.richTextBoxCtrl.txtValue1.Text, out result3))
              {
                int num = (int) MessageBox.Show("Invalid start chip number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
              }
              for (ushort chipNumber = result3; chipNumber < ushort.MaxValue; ++chipNumber)
              {
                int num = 0;
                for (int offset = 0; offset <= 65536; offset += 4096)
                {
                  this.richTextBoxCtrl.txtValue1.Text = chipNumber.ToString();
                  this.richTextBoxCtrl.txtContent.AppendText(num++.ToString());
                  this.richTextBoxCtrl.txtContent.AppendText(" Chip: " + chipNumber.ToString());
                  this.richTextBoxCtrl.txtContent.AppendText(" Offset: " + offset.ToString());
                  this.richTextBoxCtrl.txtContent.AppendText(" ");
                  try
                  {
                    if (this.richTextBoxCtrl.isCanceled)
                      return;
                    EepromBlock eeprom = this.minomatV4.GetEeprom(chipNumber, (ushort) offset, (ushort) 4096);
                    if (eeprom == null)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("not exist!");
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                      break;
                    }
                    if ((int) eeprom.ChipNumber != (int) chipNumber)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("error: Wrong chip number received!");
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                      break;
                    }
                    byte[] array = eeprom.ToArray();
                    bool flag = true;
                    for (int index = 0; index < array.Length & flag; ++index)
                    {
                      if (array[index] != byte.MaxValue)
                      {
                        flag = false;
                        break;
                      }
                    }
                    if (!flag)
                    {
                      this.richTextBoxCtrl.txtContent.AppendText(Util.ByteArrayToHexString(array));
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                    }
                    else
                    {
                      this.richTextBoxCtrl.txtContent.AppendText("empty (0xFF), Size: " + array.Length.ToString());
                      this.richTextBoxCtrl.txtContent.AppendText("\n");
                    }
                  }
                  catch (SCGiError ex)
                  {
                    this.richTextBoxCtrl.txtContent.AppendText("ERROR: " + ex.Message);
                    this.richTextBoxCtrl.txtContent.AppendText("\n");
                  }
                  Application.DoEvents();
                }
              }
              break;
            case SCGiCommand.ConfigurationString:
              this.SetControlDefaultSettings(true);
              this.richTextBoxCtrl.txtContent.Text = this.minomatV4.GetConfigurationString();
              break;
            case SCGiCommand.GetComServerFile:
              ComServerFile comServerFile = this.minomatV4.GetComServerFile(this.richTextBoxCtrl.txtValue1.Text);
              if (comServerFile == null)
                break;
              this.richTextBoxCtrl.txtContent.Text = comServerFile.ToString();
              break;
            default:
              throw new NotImplementedException("CMD: " + scGiCommand.ToString());
          }
        }
      }
      catch
      {
      }
      finally
      {
        this.richTextBoxCtrl.btnGet.Enabled = true;
        this.richTextBoxCtrl.btnStop.Visible = false;
        Cursor.Current = Cursors.Default;
      }
    }

    private void MinomatV4_OnMinomatV4ParameterReceived(object sender, MinomatV4Parameter e)
    {
      this.richTextBoxCtrl.txtContent.AppendText(e.ToString());
      this.richTextBoxCtrl.txtContent.Focus();
      this.richTextBoxCtrl.txtContent.SelectionStart = this.richTextBoxCtrl.txtContent.Text.Length;
    }

    private void Connection_OnResponse(object sender, SCGiFrame e)
    {
      this.WriteOutput(sender, e, "<-- ");
    }

    private void Connection_OnRequest(object sender, SCGiPacket e)
    {
      this.WriteOutput(sender, e, "--> ");
    }

    private void WriteOutput(object sender, SCGiFrame e, string prefix)
    {
      if (e == null || e.Count <= 0)
        return;
      foreach (SCGiPacket e1 in (List<SCGiPacket>) e)
        this.WriteOutput(sender, e1, prefix);
    }

    private void WriteOutput(object sender, SCGiPacket e, string prefix)
    {
      if (e == null)
        return;
      byte[] byteArray = e.ToByteArray();
      this.txtOutput.AppendText(prefix);
      this.txtOutput.AppendText(Util.ByteArrayToHexString(byteArray));
      this.txtOutput.AppendText(Environment.NewLine);
    }

    private void btnUsePSW_CheckedChanged(object sender, EventArgs e)
    {
      if (this.btnUsePSW.Checked)
        this.minomatV4.Authentication = new SCGiHeaderEx(true, SCGiSequenceHeaderType.Authentication, Util.HexStringToByteArray("01AA02AA03AA"));
      else
        this.minomatV4.Authentication = (SCGiHeaderEx) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MinomatHandlerWindow));
      this.menuStrip = new MenuStrip();
      this.ToolStripMenuItemComponents = new ToolStripMenuItem();
      this.menuSCGiParser = new ToolStripMenuItem();
      this.menuSCGiParserDlg = new ToolStripMenuItem();
      this.panel = new Panel();
      this.tree = new TreeView();
      this.groupBox1 = new GroupBox();
      this.txtOutput = new TextBox();
      this.label1 = new Label();
      this.cboxSourceAddress = new ComboBox();
      this.statusStrip1 = new StatusStrip();
      this.progress = new ToolStripProgressBar();
      this.lblStatus = new ToolStripStatusLabel();
      this.btnUsePSW = new CheckBox();
      this.menuStrip.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.ToolStripMenuItemComponents,
        (ToolStripItem) this.menuSCGiParser
      });
      this.menuStrip.Location = new Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new Size(856, 24);
      this.menuStrip.TabIndex = 1;
      this.menuStrip.Text = "menuStrip1";
      this.ToolStripMenuItemComponents.Name = "ToolStripMenuItemComponents";
      this.ToolStripMenuItemComponents.Size = new Size(83, 20);
      this.ToolStripMenuItemComponents.Text = "Component";
      this.menuSCGiParser.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.menuSCGiParserDlg
      });
      this.menuSCGiParser.Name = "menuSCGiParser";
      this.menuSCGiParser.Size = new Size(48, 20);
      this.menuSCGiParser.Text = "Tools";
      this.menuSCGiParserDlg.Name = "menuSCGiParserDlg";
      this.menuSCGiParserDlg.Size = new Size(134, 22);
      this.menuSCGiParserDlg.Text = "SCGi Parser";
      this.menuSCGiParserDlg.Click += new System.EventHandler(this.menuSCGiParserDlg_Click);
      this.panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel.Location = new Point(6, 11);
      this.panel.Name = "panel";
      this.panel.Size = new Size(542, 390);
      this.panel.TabIndex = 2;
      this.tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.tree.FullRowSelect = true;
      this.tree.HideSelection = false;
      this.tree.Location = new Point(6, 57);
      this.tree.Name = "tree";
      this.tree.Size = new Size(284, 480);
      this.tree.TabIndex = 3;
      this.tree.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.txtOutput);
      this.groupBox1.Controls.Add((Control) this.panel);
      this.groupBox1.Location = new Point(296, 27);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(554, 510);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.txtOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOutput.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtOutput.Location = new Point(6, 407);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = ScrollBars.Vertical;
      this.txtOutput.Size = new Size(542, 97);
      this.txtOutput.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Source Address:";
      this.cboxSourceAddress.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxSourceAddress.FormattingEnabled = true;
      this.cboxSourceAddress.Location = new Point(98, 30);
      this.cboxSourceAddress.Name = "cboxSourceAddress";
      this.cboxSourceAddress.Size = new Size(98, 21);
      this.cboxSourceAddress.TabIndex = 6;
      this.cboxSourceAddress.SelectedIndexChanged += new System.EventHandler(this.cboxSourceAddress_SelectedIndexChanged);
      this.statusStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.progress,
        (ToolStripItem) this.lblStatus
      });
      this.statusStrip1.Location = new Point(0, 540);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(856, 22);
      this.statusStrip1.TabIndex = 7;
      this.statusStrip1.Text = "statusStrip1";
      this.progress.Name = "progress";
      this.progress.Size = new Size(100, 16);
      this.progress.Visible = false;
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(0, 17);
      this.btnUsePSW.AutoSize = true;
      this.btnUsePSW.Checked = true;
      this.btnUsePSW.CheckState = CheckState.Checked;
      this.btnUsePSW.Location = new Point(203, 33);
      this.btnUsePSW.Name = "btnUsePSW";
      this.btnUsePSW.Size = new Size(73, 17);
      this.btnUsePSW.TabIndex = 8;
      this.btnUsePSW.Text = "Use PSW";
      this.btnUsePSW.UseVisualStyleBackColor = true;
      this.btnUsePSW.CheckedChanged += new System.EventHandler(this.btnUsePSW_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(856, 562);
      this.Controls.Add((Control) this.btnUsePSW);
      this.Controls.Add((Control) this.statusStrip1);
      this.Controls.Add((Control) this.cboxSourceAddress);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.tree);
      this.Controls.Add((Control) this.menuStrip);
      this.Controls.Add((Control) this.groupBox1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MinomatHandlerWindow);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "MinomatHandler";
      this.FormClosing += new FormClosingEventHandler(this.MinomatHandlerWindow_FormClosing);
      this.Load += new System.EventHandler(this.MinomatHandlerWindow_Load);
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum UserCtrl
    {
      TextBoxes,
      DataTable,
      RichTextBox,
      DataTableWithDateTime,
    }

    [Flags]
    private enum Show
    {
      Button1 = 1,
      Button2 = 2,
    }
  }
}
