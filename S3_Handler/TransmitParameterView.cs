// Decompiled with JetBrains decompiler
// Type: S3_Handler.TransmitParameterView
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using DeviceCollector;
using HandlerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Properties;

#nullable disable
namespace S3_Handler
{
  internal class TransmitParameterView : Form
  {
    private static string[] DeviceIdVars = new string[4]
    {
      "SerDev0_IdentNo",
      "SerDev1_IdentNo",
      "SerDev2_IdentNo",
      "SerDev3_IdentNo"
    };
    private static string[] RadioIdVars = new string[4]
    {
      "SerDev0_RadioId",
      "SerDev1_RadioId",
      "SerDev2_RadioId",
      "SerDev3_RadioId"
    };
    private const string FOLLOWING_LIST = "FOLLOWING_LIST";
    private const string ACTIV_LIST = " = activated";
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    private DataTransmitter transmitter;
    private byte? tempRadioAndEncMode = new byte?();
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private TreeView tree;
    private SplitContainer splitContainer1;
    private Label label2;
    private Label label3;
    private TextBox txtParameterInfo;
    private SplitContainer splitContainer;
    private CheckBox ckboxShowAllTransmitParameter;
    private ContextMenuStrip contextMenuTransmitLists;
    private ToolStripMenuItem btnDeleteTaransmitList;
    internal Button btnPrint;
    private Button btnOk;
    private Button btnCancel;
    private TreeView treeLogger;
    private TreeView treeParameter;
    private GroupBox gboxSettings;
    private CheckBox ckboxLoggerDueDate;
    private Label lblLoggerType;
    private ComboBox cboxLoggerType;
    private NumericUpDown txtCountOfValues;
    private Label lblCountOfValues;
    private ToolStripMenuItem btnAddList;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem btnTreeExpand;
    private ToolStripMenuItem btnTreeCollaps;
    private ToolStripMenuItem btnSetAsActiveList;
    private NumericUpDown txtStartStorageNumber;
    private Label lblStartStorageNumber;
    private ToolStripMenuItem btnAddGroup;
    private Label lblRadio;
    private ComboBox cboxRadioMode;
    private Label lblRadioDevice;
    private ComboBox cboxRadioDevice;
    private NumericUpDown txtRadioIdOffset;
    private Label lblRadioIdOffset;
    private ToolStripMenuItem btnAddScenario;
    private ToolStripMenuItem btnAddRadioParameter;
    private ToolStripMenuItem btnRadio3Scenario1;
    private ToolStripMenuItem btnAddRadio3Scen1Volume;
    private ToolStripMenuItem btnAddRadio3Scen1Heat;
    private ToolStripMenuItem btnAddRadio3Scen1Cooling;
    private ToolStripMenuItem btnRadio3Scenario3;
    private ToolStripMenuItem btnAddRadio3Scen3Volume;
    private ToolStripMenuItem btnAddRadio3Scen3Heat;
    private ToolStripMenuItem btnAddRadio3Scen3Cooling;
    private ToolStripMenuItem btnAddRadio3Scen1Input1;
    private ToolStripMenuItem btnAddRadio3Scen1Input2;
    private ToolStripMenuItem btnAddRadio3Scen1Input3;
    private ToolStripMenuItem btnAddRadio3Scen3Input1;
    private ToolStripMenuItem btnAddRadio3Scen3Input2;
    private ToolStripMenuItem btnAddRadio3Scen3Input3;
    private ToolStripMenuItem btnRadio2Walkby;
    private ToolStripMenuItem btnAddRadio2WByVolume;
    private ToolStripMenuItem btnAddRadio2WByHeat;
    private ToolStripMenuItem btnAddRadio2WByCooling;
    private ToolStripMenuItem btnAddRadio2WByInput1;
    private ToolStripMenuItem btnAddRadio2WByInput2;
    private ToolStripMenuItem btnAddRadio2WByInput3;
    private ToolStripMenuItem btnSetScenarioName;
    private ToolStripMenuItem btnRadio3Scenario5;
    private ToolStripMenuItem btnAddRadio3Scen5Volume;
    private ToolStripMenuItem btnAddRadio3Scen5Heat;
    private ToolStripMenuItem btnAddRadio3Scen5Cooling;
    private ToolStripMenuItem btnAddRadio3Scen5Input1;
    private ToolStripMenuItem btnAddRadio3Scen5Input2;
    private ToolStripMenuItem btnAddRadio3Scen5Input3;
    private ToolStripMenuItem btnRadio3Scenario6;
    private ToolStripMenuItem btnAddRadio3Scen6Volume;
    private ToolStripMenuItem btnAddRadio3Scen6Heat;
    private ToolStripMenuItem btnAddRadio3Scen6Cooling;
    private ToolStripMenuItem btnAddRadio3Scen6Input1;
    private ToolStripMenuItem btnAddRadio3Scen6Input2;
    private ToolStripMenuItem btnAddRadio3Scen6Input3;
    private Label labelRadioCycle;
    private Label labelWMBusEncryption;
    private Label labelRadioMode;
    private ComboBox comboBoxRadioMode;
    private ComboBox comboBox_wMBusEncryption;
    private GroupBox groupBoxDeviceSetup;
    private ComboBox comboBoxRadioCycle;

    internal TransmitParameterView(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      MyFunctions.MyMeters.NewWorkMeter("Edit transmit lists");
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyFunctions.MyMeters.WorkMeter;
      this.InitializeComponent();
      this.comboBoxRadioMode.Items.AddRange((object[]) Enum.GetNames(typeof (RADIO_MODE)));
      this.comboBox_wMBusEncryption.Items.AddRange((object[]) Enum.GetNames(typeof (AES_ENCRYPTION_MODE)));
      if (MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioAndEncMode.ToString()))
      {
        this.tempRadioAndEncMode = new byte?(MyMeter.MyParameters.ParameterByName[S3_ParameterNames.radioAndEncMode.ToString()].GetByteValue());
      }
      else
      {
        this.comboBoxRadioMode.Visible = false;
        this.comboBox_wMBusEncryption.Enabled = false;
      }
      this.ShowRadioAndEncryptionMode(-1);
      this.comboBoxRadioCycle.Items.AddRange((object[]) this.MyMeter.GetUsefulRadioCycleSeconds());
      uint? radioCycleSeconds = this.MyMeter.GetRadioCycleSeconds();
      if (radioCycleSeconds.HasValue)
        this.comboBoxRadioCycle.Text = radioCycleSeconds.Value.ToString();
      else
        this.comboBoxRadioCycle.Enabled = false;
      this.tree.ImageList = new ImageList()
      {
        Images = {
          (Image) Resources.FolderNode,
          (Image) Resources.Link,
          (Image) Resources.Property,
          (Image) Resources.Log,
          (Image) Resources.LinkFolder
        }
      };
    }

    private void TransmitParameterView_Load(object sender, EventArgs e)
    {
      this.treeLogger.Nodes.Clear();
      LoggerView.GetLoggerTree(this.treeLogger.Nodes, this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks);
      for (int index1 = this.treeLogger.Nodes.Count - 1; index1 >= 0; --index1)
      {
        for (int index2 = this.treeLogger.Nodes[index1].Nodes.Count - 1; index2 >= 0; --index2)
        {
          if (this.treeLogger.Nodes[index1].Nodes[index2].Tag is LoggerChanal tag && (tag.isMaxValues || tag.isGenerateMidnightEvent))
            this.treeLogger.Nodes[index1].Nodes.RemoveAt(index2);
        }
        if (this.treeLogger.Nodes[index1].Nodes.Count == 0)
          this.treeLogger.Nodes.RemoveAt(index1);
      }
      this.treeLogger.ExpandAll();
      this.cboxLoggerType.DataSource = (object) Util.GetNamesOfEnum(typeof (TransmitParameterView.LoggerType));
      this.cboxLoggerType.SelectedIndex = 0;
      this.cboxRadioMode.DataSource = (object) Util.GetNamesOfEnum(typeof (RADIO_MODE));
      ushort length = 1;
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input1 != null)
        ++length;
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input2 != null)
        ++length;
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input3 != null)
        ++length;
      string[] strArray = new string[(int) length];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = index.ToString() + " ID: " + this.MyMeter.MyParameters.ParameterByName[TransmitParameterView.DeviceIdVars[index]].GetUintValue().ToString("X8") + " RadioID: " + this.MyMeter.MyParameters.ParameterByName[TransmitParameterView.RadioIdVars[index]].GetUintValue().ToString("X8");
      this.cboxRadioDevice.DataSource = (object) strArray;
      this.InitializeTree();
      this.ckboxShowAllTransmitParameter_CheckedChanged(sender, e);
    }

    private void contextMenuTransmitLists_Opening(object sender, CancelEventArgs e)
    {
      this.btnAddList.Enabled = this.tree.SelectedNode != null && (this.tree.SelectedNode.Tag is MBusTransmitter || this.tree.SelectedNode.Tag is RadioListHeader);
      this.btnDeleteTaransmitList.Enabled = this.tree.SelectedNode != null && (this.tree.SelectedNode.Tag is MBusList || this.tree.SelectedNode.Tag is MBusParameter || this.tree.SelectedNode.Tag is ListLink || this.tree.SelectedNode.Tag is MBusParameterGroup || this.tree.SelectedNode.Tag is RadioListHeader || this.tree.SelectedNode.Tag is RadioListHeaderItem);
      this.btnSetAsActiveList.Enabled = this.tree.SelectedNode != null && (this.tree.SelectedNode.Tag is MBusList && this.tree.SelectedNode.Parent.Tag is MBusTransmitter || this.tree.SelectedNode.Tag is RadioListHeader && this.tree.SelectedNode.Parent.Tag is RadioTransmitter);
      this.btnAddGroup.Enabled = this.tree.SelectedNode != null && (this.tree.SelectedNode.Tag is MBusList || this.tree.SelectedNode.Tag is MBusParameter);
      this.btnAddScenario.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioTransmitter;
      this.btnAddRadioParameter.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeaderItem && (((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3_Sz5 || ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3 || ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3_Sz0);
      this.btnRadio3Scenario6.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeaderItem && ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3;
      this.btnRadio3Scenario5.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeaderItem && ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3_Sz5;
      this.btnRadio3Scenario1.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeaderItem && ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3;
      this.btnRadio3Scenario3.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeaderItem && ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3;
      this.btnRadio2Walkby.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeaderItem && ((RadioListHeader) this.tree.SelectedNode.Parent.Tag).Mode == RADIO_MODE.Radio3_Sz0;
      this.btnRadio3Scenario6.Enabled = this.tree.SelectedNode != null && this.tree.SelectedNode.FirstNode == null;
      this.btnRadio3Scenario5.Enabled = this.tree.SelectedNode != null && this.tree.SelectedNode.FirstNode == null;
      this.btnRadio3Scenario3.Enabled = this.tree.SelectedNode != null && this.tree.SelectedNode.FirstNode == null;
      this.btnRadio3Scenario1.Enabled = this.tree.SelectedNode != null && this.tree.SelectedNode.FirstNode == null;
      this.btnRadio2Walkby.Enabled = this.tree.SelectedNode != null && this.tree.SelectedNode.FirstNode == null;
      this.btnSetScenarioName.Visible = this.tree.SelectedNode != null && this.tree.SelectedNode.Tag is RadioListHeader;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioAndEncMode.ToString()))
      {
        byte NewValue = (byte) (((int) this.tempRadioAndEncMode.Value & 240) + (int) Enum.Parse(typeof (AES_ENCRYPTION_MODE), this.comboBox_wMBusEncryption.SelectedItem.ToString()));
        if (!this.tempRadioAndEncMode.HasValue || (int) this.tempRadioAndEncMode.Value != (int) NewValue)
          throw new Exception("Interneal radioAndEncMode management error");
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.radioAndEncMode.ToString()].SetByteValue(NewValue);
      }
      this.MyMeter.SetRadioCycleSeconds(uint.Parse(this.comboBoxRadioCycle.Text));
      if (!this.CheckMaxSizeOfList(this.transmitter.P2P) || !this.CheckMaxSizeOfList(this.transmitter.Heat) || !this.CheckMaxSizeOfList(this.transmitter.Input1) || !this.CheckMaxSizeOfList(this.transmitter.Input2) || !this.CheckMaxSizeOfList(this.transmitter.Input3))
        return;
      this.transmitter.RemoveEmptyList();
      if (!this.CheckParameter(this.transmitter.P2P) || !this.CheckParameter(this.transmitter.Heat) || !this.CheckParameter(this.transmitter.Input1) || !this.CheckParameter(this.transmitter.Input2) || !this.CheckParameter(this.transmitter.Input3))
        return;
      this.transmitter.PrepareMBusParameterGroup();
      if (!this.MyMeter.Compile())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private bool CheckParameter(MBusTransmitter mBusTransmitter)
    {
      if (mBusTransmitter == null || this.transmitter.childMemoryBlocks == null)
        return true;
      for (int index = this.transmitter.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        if (!this.CheckParameter(this.transmitter.childMemoryBlocks[index] as MBusList))
          return false;
      }
      return true;
    }

    private bool CheckParameter(MBusList mBusList)
    {
      if (mBusList == null || mBusList.childMemoryBlocks == null)
        return true;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (S3_MemoryBlock childMemoryBlock in mBusList.childMemoryBlocks)
      {
        string empty = string.Empty;
        string str;
        if (childMemoryBlock is MBusParameter)
        {
          MBusParameter mbusParameter = childMemoryBlock as MBusParameter;
          str = !mbusParameter.IsLogger ? mbusParameter.Name : mbusParameter.Name + mbusParameter.ControlWord0.ParamCode.ToString();
        }
        else if (childMemoryBlock is ListLink)
          str = ((ListLink) childMemoryBlock).Name;
        else
          continue;
        if (dictionary.ContainsKey(str))
        {
          string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(str);
          int num = (int) MessageBox.Show("Please check '" + mBusList.Name + "' list. The '" + parameterNameByName + "' is duplicated.", "Inavlid list detected!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        dictionary.Add(str, (string) null);
      }
      return true;
    }

    private bool CheckMaxSizeOfList(MBusList mBusList)
    {
      if (mBusList == null || mBusList.GetOutputBufferSize(false) <= (int) byte.MaxValue)
        return true;
      int num = (int) MessageBox.Show("Please reduce following list: " + mBusList.Name, "The list is to big!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private bool CheckMaxSizeOfList(MBusTransmitter transmitter)
    {
      if (transmitter == null || transmitter.childMemoryBlocks == null)
        return true;
      for (int index = transmitter.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        if (!this.CheckMaxSizeOfList(transmitter.childMemoryBlocks[index] as MBusList))
          return false;
      }
      return true;
    }

    private void btnTreeExpand_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null)
        return;
      this.tree.SelectedNode.ExpandAll();
    }

    private void btnTreeCollaps_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null)
        return;
      this.tree.SelectedNode.Collapse();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.MyFunctions.MyMeters.Undo();

    private void btnAddGroup_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null)
        return;
      TreeNode list = (TreeNode) null;
      string str = "Parameter Group";
      if (this.tree.SelectedNode.Tag is MBusParameter)
      {
        MBusParameter tag = this.tree.SelectedNode.Tag as MBusParameter;
        if (tag.parentMemoryBlock is MBusList)
        {
          MBusList parentMemoryBlock = tag.parentMemoryBlock as MBusList;
          int pos = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag) + 1;
          MBusParameterGroup mbusParameterGroup = parentMemoryBlock.AddGroup(pos);
          if (mbusParameterGroup == null)
            return;
          list = this.tree.SelectedNode.Parent.Nodes.Insert(this.tree.SelectedNode.Index + 1, str, str, 0, 0);
          list.Tag = (object) mbusParameterGroup;
        }
        else
        {
          RadioList parentMemoryBlock = tag.parentMemoryBlock as RadioList;
          int pos = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag) + 1;
          MBusParameterGroup mbusParameterGroup = parentMemoryBlock.AddGroup(pos);
          if (mbusParameterGroup == null)
            return;
          list = this.tree.SelectedNode.Parent.Nodes.Insert(this.tree.SelectedNode.Index + 1, str, str, 0, 0);
          list.Tag = (object) mbusParameterGroup;
        }
      }
      else if (this.tree.SelectedNode.Tag is MBusList)
      {
        MBusParameterGroup mbusParameterGroup = (this.tree.SelectedNode.Tag as MBusList).AddGroup();
        if (mbusParameterGroup == null)
          return;
        list = this.tree.SelectedNode.Nodes.Insert(1, str, str, 0, 0);
        list.Tag = (object) mbusParameterGroup;
      }
      if (list == null)
        return;
      this.RefreshParameterNumbers(list);
      this.RefreshParameterNumbers(list.Parent);
    }

    private void btnAddList_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null)
        return;
      if (this.tree.SelectedNode.Tag is MBusTransmitter)
      {
        MBusList mbusList = (this.tree.SelectedNode.Tag as MBusTransmitter).InsertNewMBusList(0);
        if (mbusList == null)
          return;
        TreeNode treeNode = this.tree.SelectedNode.Nodes.Insert(0, mbusList.Name, mbusList.Name, 0, 0);
        treeNode.Nodes.Add(new TreeNode("Following lists", 4, 4)
        {
          Tag = (object) "FOLLOWING_LIST"
        });
        treeNode.Tag = (object) mbusList;
      }
      else
      {
        if (!(this.tree.SelectedNode.Tag is RadioListHeader))
          return;
        RadioListHeaderItem radioListHeaderItem = (this.tree.SelectedNode.Tag as RadioListHeader).InsertList(0);
        if (radioListHeaderItem == null)
          return;
        this.tree.SelectedNode.Nodes.Insert(0, radioListHeaderItem.Description, radioListHeaderItem.Description, 0, 0).Tag = (object) radioListHeaderItem;
      }
    }

    private void btnAddRadio3Scen5Volume_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario5ListParameter(S3_ParameterNames.Vol_VolDisplay.ToString());
    }

    private void btnAddRadio3Scen5Heat_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario5ListParameter(S3_ParameterNames.Energy_HeatEnergyDisplay.ToString());
    }

    private void btnAddRadio3Scen5Cooling_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario5ListParameter(S3_ParameterNames.Energy_ColdEnergyDisplay.ToString());
    }

    private void btnAddRadio3Scen5Input1_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario5ListParameter(S3_ParameterNames.Cal_DisplayInput_n_0.ToString());
    }

    private void btnAddRadio3Scen5Input2_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario5ListParameter(S3_ParameterNames.Cal_DisplayInput_n_1.ToString());
    }

    private void btnAddRadio3Scen5Input3_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario5ListParameter(S3_ParameterNames.Cal_DisplayInput_n_2.ToString());
    }

    private void btnAddRadio3Scen3Volume_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario3ListParameter("Vol_VolDisplay");
    }

    private void btnAddRadio3Scen3Heat_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario3ListParameter("Energy_HeatEnergyDisplay");
    }

    private void btnAddRadio3Scen3Cooling_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario3ListParameter("Energy_ColdEnergyDisplay");
    }

    private void btnAddRadio3Scen3Input1_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario3ListParameter("Cal_DisplayInput_n_0");
    }

    private void btnAddRadio3Scen3Input2_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario3ListParameter("Cal_DisplayInput_n_1");
    }

    private void btnAddRadio3Scen3Input3_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario3ListParameter("Cal_DisplayInput_n_2");
    }

    private void btnAddRadio3Scen1Volume_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario1ListParameter("Vol_VolDisplay");
    }

    private void btnAddRadio3Scen1Heat_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario1ListParameter("Energy_HeatEnergyDisplay");
    }

    private void btnAddRadio3Scen1Cooling_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario1ListParameter("Energy_ColdEnergyDisplay");
    }

    private void btnAddRadio3Scen1Input1_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario1ListParameter("Cal_DisplayInput_n_0");
    }

    private void btnAddRadio3Scen1Input2_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario1ListParameter("Cal_DisplayInput_n_1");
    }

    private void btnAddRadio3Scen1Input3_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio3Scenario1ListParameter("Cal_DisplayInput_n_2");
    }

    private void btnAddRadio2WByVolume_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio2WalkbyListParameter("Vol_VolDisplay");
    }

    private void btnAddRadio2WByHeat_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio2WalkbyListParameter("Energy_HeatEnergyDisplay");
    }

    private void btnAddRadio2WByCooling_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio2WalkbyListParameter("Energy_ColdEnergyDisplay");
    }

    private void btnAddRadio2WByInput1_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio2WalkbyListParameter("Cal_DisplayInput_n_0");
    }

    private void btnAddRadio2WByInput2_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio2WalkbyListParameter("Cal_DisplayInput_n_1");
    }

    private void btnAddRadio2WByInput3_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioListHeaderItem))
        return;
      this.InsertRadio2WalkbyListParameter("Cal_DisplayInput_n_2");
    }

    private void btnAddScenario_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null || !(this.tree.SelectedNode.Tag is RadioTransmitter))
        return;
      RadioListHeader radioListHeader = (this.tree.SelectedNode.Tag as RadioTransmitter).InsertHeader(0);
      if (radioListHeader == null)
        return;
      TreeNode treeNode = this.tree.SelectedNode.Nodes.Insert(0, radioListHeader.Name, radioListHeader.Name, 0, 0);
      treeNode.Tag = (object) radioListHeader;
      this.tree.SelectedNode = treeNode;
      this.btnSetScenarioName_Click(sender, e);
      this.RefreshParameterNumbers(treeNode.Parent);
    }

    private void btnSetScenarioName_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode.Tag is RadioListHeader)
      {
        RadioListHeader tag = this.tree.SelectedNode.Tag as RadioListHeader;
        tag.ScenarioName = EditRadioScenarioNameView.Show((Form) this, tag.ScenarioName);
        foreach (TreeNode node in this.tree.SelectedNode.Parent.Nodes)
        {
          if (node.Tag is RadioListHeader && node != this.tree.SelectedNode && (node.Tag as RadioListHeader).Name == tag.Name)
          {
            tag.ScenarioName = string.Empty;
            int num = (int) MessageBox.Show("Please use an other Scenarioname!!!\r\nThe combination of Radiomode and Scenarioname is already in use");
          }
        }
      }
      this.RefreshParameterNumbers(this.tree.SelectedNode);
    }

    private void btnSetAsActiveList_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null)
        return;
      if (this.tree.SelectedNode.Tag is MBusList)
      {
        MBusList tag = this.tree.SelectedNode.Tag as MBusList;
        if (tag.Name == this.transmitter.P2P.Name)
          return;
        ((MBusTransmitter) tag.parentMemoryBlock).SetAsActive(tag);
        foreach (TreeNode node in this.tree.SelectedNode.Parent.Nodes)
          node.Text = node.Text.Replace(" = activated", string.Empty);
        this.SetTreeNodeAsActive(this.tree.SelectedNode);
      }
      else
      {
        if (!(this.tree.SelectedNode.Tag is RadioListHeader))
          return;
        (this.tree.SelectedNode.Tag as RadioListHeader).IsSelected_Memory = true;
        foreach (TreeNode node in this.tree.SelectedNode.Parent.Nodes)
          node.Text = node.Text.Replace(" = activated", string.Empty);
        this.SetTreeNodeAsActive(this.tree.SelectedNode);
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      using (Bitmap bitmap = new Bitmap(this.tree.Width, this.tree.Height))
      {
        this.tree.DrawToBitmap(bitmap, this.tree.ClientRectangle);
        bitmap.Save("TransmitParamer.bmp");
        Process.Start("TransmitParamer.bmp");
      }
    }

    private void tree_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.tree.SelectedNode = this.tree.GetNodeAt(e.X, e.Y);
    }

    private void tree_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete)
        return;
      this.btnDeleteTaransmitList_Click(sender, (EventArgs) null);
    }

    private void btnDeleteTaransmitList_Click(object sender, EventArgs e)
    {
      if (this.tree.SelectedNode == null)
        return;
      if (this.tree.SelectedNode.Tag is S3_MemoryBlock)
      {
        if (!this.MyMeter.MyTransmitParameterManager.Remove(this.tree.SelectedNode.Tag as S3_MemoryBlock))
          return;
        TreeNode parent = this.tree.SelectedNode.Parent;
        parent.Nodes.Remove(this.tree.SelectedNode);
        this.RefreshParameterNumbers(parent);
        this.RefreshParameterNumbers(parent.Parent);
      }
      this.txtParameterInfo.Text = string.Empty;
    }

    private void ckboxShowAllTransmitParameter_CheckedChanged(object sender, EventArgs e)
    {
      this.treeParameter.Nodes.Clear();
      foreach (KeyValuePair<string, S3_Parameter> keyValuePair in this.MyMeter.MyParameters.ParameterByName)
      {
        if ((this.ckboxShowAllTransmitParameter.Checked || keyValuePair.Value.Statics != null && keyValuePair.Value.Statics.DefaultDifVif != null && keyValuePair.Value.Statics.DefaultDifVif.Length != 0) && (this.ckboxShowAllTransmitParameter.Checked || this.MyFunctions.baseTypeEditMode || this.MyMeter.MyResources.IsResourceAvailable(keyValuePair.Value.Statics.NeedResource)))
        {
          string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(keyValuePair.Value.Name);
          if (string.IsNullOrEmpty(parameterNameByName))
            this.treeParameter.Nodes.Add(keyValuePair.Value.Name, keyValuePair.Value.Name).Tag = (object) keyValuePair.Value;
          else
            this.treeParameter.Nodes.Add(keyValuePair.Value.Name, parameterNameByName).Tag = (object) keyValuePair.Value;
        }
      }
    }

    private void treeLogger_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.txtParameterInfo.Text = string.Empty;
      this.txtParameterInfo.BackColor = SystemColors.Window;
      if (!(e.Node.Tag is LoggerChanal))
        return;
      this.txtParameterInfo.Text = (e.Node.Tag as LoggerChanal).ChanalName;
    }

    private void tree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.gboxSettings.Enabled = false;
      this.txtParameterInfo.Text = string.Empty;
      this.txtParameterInfo.BackColor = SystemColors.Window;
      this.cboxLoggerType.Visible = false;
      this.lblLoggerType.Visible = false;
      this.ckboxLoggerDueDate.Visible = false;
      this.txtCountOfValues.Visible = false;
      this.lblCountOfValues.Visible = false;
      this.txtStartStorageNumber.Visible = false;
      this.lblStartStorageNumber.Visible = false;
      this.cboxRadioMode.Visible = false;
      this.lblRadio.Visible = false;
      this.cboxRadioDevice.Visible = false;
      this.lblRadioDevice.Visible = false;
      this.lblRadioIdOffset.Visible = false;
      this.txtRadioIdOffset.Visible = false;
      bool flag = e.Node.Parent != null && e.Node.Parent.Tag is MBusParameterGroup;
      if (flag)
      {
        this.cboxLoggerType.Visible = true;
        this.lblLoggerType.Visible = true;
      }
      if (e.Node.Tag is MBusParameter)
      {
        MBusParameter tag = e.Node.Tag as MBusParameter;
        MBusDifVif mbusDifVif = new MBusDifVif();
        if (tag.VifDif != null && tag.VifDif.Count > 0 && mbusDifVif.LoadDifVif(tag.VifDif.ToArray()))
          this.txtStartStorageNumber.Value = (Decimal) mbusDifVif.StorageNumber;
        switch (tag.ControlWord0.ParamCode)
        {
          case ParamCode.None:
          case ParamCode.LogValue:
            this.cboxLoggerType.SelectedItem = (object) TransmitParameterView.LoggerType.Value.ToString();
            break;
          case ParamCode.Date:
          case ParamCode.LogDate:
            this.cboxLoggerType.SelectedItem = (object) TransmitParameterView.LoggerType.Date.ToString();
            break;
          case ParamCode.DateTime:
          case ParamCode.LogDateTime:
            this.cboxLoggerType.SelectedItem = (object) TransmitParameterView.LoggerType.DateTime.ToString();
            break;
        }
        if (tag.IsLogger)
        {
          this.cboxLoggerType.Visible = true;
          this.lblLoggerType.Visible = true;
          if (!flag)
          {
            this.ckboxLoggerDueDate.Visible = true;
            this.txtCountOfValues.Visible = true;
            this.lblCountOfValues.Visible = true;
            this.txtStartStorageNumber.Visible = true;
            this.lblStartStorageNumber.Visible = true;
          }
          this.MyMeter.MyLoggerManager.GetLoggerChanal(tag.Name);
          this.ckboxLoggerDueDate.Checked = tag.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset;
          this.txtCountOfValues.Value = tag.Iteration == null || tag.Iteration.ControlWord1 == null || tag.Iteration.ControlWord1.LoggerCycleCount <= 0 ? 1M : (Decimal) (tag.Iteration.ControlWord1.LoggerCycleCount + 1);
          this.txtParameterInfo.Text = tag.GetLoggerInfo();
        }
        else
          this.txtParameterInfo.Text = this.MyMeter.MyParameters.ParameterByName[tag.Name].GetParameterInfo(out bool _);
      }
      else if (e.Node.Tag is S3_Parameter)
        this.txtParameterInfo.Text = ((S3_Parameter) e.Node.Tag).GetParameterInfo(out bool _);
      else if (e.Node.Tag is ListLink)
        this.txtParameterInfo.Text = "Link to: " + ((ListLink) e.Node.Tag).Name;
      else if (e.Node.Tag is MBusParameterGroup)
      {
        this.ckboxLoggerDueDate.Visible = true;
        this.txtCountOfValues.Visible = true;
        this.lblCountOfValues.Visible = true;
        this.txtStartStorageNumber.Visible = true;
        this.lblStartStorageNumber.Visible = true;
        MBusParameterGroup tag = e.Node.Tag as MBusParameterGroup;
        this.txtCountOfValues.Value = (Decimal) tag.TotalCountOfValues;
        this.ckboxLoggerDueDate.Checked = tag.IsDueDate;
        this.txtStartStorageNumber.Value = (Decimal) tag.StartStorageNumber;
      }
      else if (e.Node.Tag is RadioListHeader)
      {
        this.lblRadio.Visible = true;
        this.cboxRadioMode.Visible = true;
        this.cboxRadioMode.SelectedItem = (object) (e.Node.Tag as RadioListHeader).Mode.ToString();
      }
      else if (e.Node.Tag is RadioListHeaderItem)
      {
        this.lblRadioDevice.Visible = true;
        this.cboxRadioDevice.Visible = true;
        this.lblRadioIdOffset.Visible = true;
        this.txtRadioIdOffset.Visible = true;
        RadioListHeaderItem tag = e.Node.Tag as RadioListHeaderItem;
        this.cboxRadioDevice.SelectedIndex = tag.IndexOfVirtualDevice;
        this.txtRadioIdOffset.Value = (Decimal) tag.RadioIdOffset;
      }
      this.gboxSettings.Enabled = true;
    }

    private void InitializeTree()
    {
      this.transmitter = this.MyMeter.MyTransmitParameterManager.Transmitter;
      if (this.transmitter == null || this.transmitter.childMemoryBlocks == null)
        return;
      TreeNode treeNode = new TreeNode("Bus and optical interface");
      this.AddMBusList(this.transmitter.P2P, treeNode);
      this.CreateTransmitList(this.transmitter.Heat, treeNode);
      this.CreateTransmitList(this.transmitter.Input1, treeNode);
      this.CreateTransmitList(this.transmitter.Input2, treeNode);
      this.CreateTransmitList(this.transmitter.Input3, treeNode);
      this.tree.Nodes.Add(treeNode);
      if (this.transmitter.Radio != null)
        this.tree.Nodes.Add(this.CreateNodesOfRadio(this.transmitter.Radio));
      foreach (TreeNode node in this.tree.Nodes)
        node.Expand();
      if (this.tree.Nodes.Count > 0 && this.tree.Nodes[0].Nodes.Count > 0)
      {
        this.tree.Nodes[0].Expand();
        this.tree.Nodes[0].Nodes[0].Expand();
        this.tree.Nodes[0].Nodes[1].Expand();
      }
      this.tree.Nodes[0].EnsureVisible();
    }

    private void CreateTransmitList(MBusTransmitter table, TreeNode root)
    {
      if (table == null)
        return;
      byte ushortValue = (byte) this.MyMeter.MyParameters.ParameterByName[table.KeyOfParameterPrimaryAddress].GetUshortValue();
      string name = string.Format("{0} (Adr.: {1})", (object) table.Name, (object) ushortValue);
      root.Nodes.Add(this.CreateNodesOfSerDev(name, table));
      foreach (TreeNode node in root.Nodes[root.Nodes.Count - 1].Nodes)
      {
        if (((MBusList) node.Tag).IsSelected)
        {
          node.Text += " = activated";
          break;
        }
      }
    }

    private TreeNode CreateNodesOfSerDev(string name, MBusTransmitter table)
    {
      TreeNode root = new TreeNode(name);
      root.Tag = (object) table;
      if (table == null || table.childMemoryBlocks == null)
        return root;
      foreach (MBusList childMemoryBlock in table.childMemoryBlocks)
        this.AddMBusList(childMemoryBlock, root);
      return root;
    }

    private void AddMBusList(MBusList mbusList, TreeNode root)
    {
      TreeNode treeNode = new TreeNode(mbusList.Name);
      treeNode.Tag = (object) mbusList;
      TreeNode node = new TreeNode("Following lists", 4, 4);
      node.Tag = (object) "FOLLOWING_LIST";
      treeNode.Nodes.Add(node);
      if (mbusList.childMemoryBlocks != null && mbusList.childMemoryBlocks.Count > 0 && mbusList.childMemoryBlocks[0] is ListLink)
      {
        foreach (S3_MemoryBlock childMemoryBlock in mbusList.childMemoryBlocks)
        {
          if (childMemoryBlock is ListLink)
          {
            string name = ((ListLink) childMemoryBlock).Name;
            node.Nodes.Add(name, name, 1, 1).Tag = (object) childMemoryBlock;
          }
        }
      }
      this.GetParameterListNodes(mbusList.childMemoryBlocks, treeNode);
      root.Nodes.Add(treeNode);
      this.RefreshParameterNumbers(treeNode);
    }

    private TreeNode GetParameterListNodes(List<S3_MemoryBlock> parameter, TreeNode listNode)
    {
      if (parameter != null)
      {
        int num1 = 1;
        foreach (S3_MemoryBlock s3MemoryBlock in parameter)
        {
          if (s3MemoryBlock is MBusParameter)
          {
            MBusParameter mbusParameter = (MBusParameter) s3MemoryBlock;
            int num2 = mbusParameter.IsLogger ? 3 : 2;
            listNode.Nodes.Add(new TreeNode(mbusParameter.Name, num2, num2)
            {
              Name = mbusParameter.Name,
              Tag = (object) mbusParameter
            });
            ++num1;
          }
          else if (s3MemoryBlock is MBusParameterGroup)
          {
            MBusParameterGroup mbusParameterGroup = (MBusParameterGroup) s3MemoryBlock;
            mbusParameterGroup.Reduce();
            TreeNode treeNode = new TreeNode(mbusParameterGroup.Name, 0, 0);
            treeNode.Name = mbusParameterGroup.Name;
            treeNode.Tag = (object) mbusParameterGroup;
            this.GetParameterListNodes(mbusParameterGroup.childMemoryBlocks, treeNode);
            listNode.Nodes.Add(treeNode);
            this.RefreshParameterNumbers(treeNode);
            ++num1;
          }
        }
      }
      return listNode;
    }

    private TreeNode FindLoggerNode(LoggerChanal log, TreeNodeCollection nodes)
    {
      foreach (TreeNode node in nodes)
      {
        if (node.Tag == log)
          return node;
        TreeNode loggerNode = this.FindLoggerNode(log, node.Nodes);
        if (loggerNode != null)
          return loggerNode;
      }
      return (TreeNode) null;
    }

    private TreeNode CreateNodesOfRadio(RadioTransmitter table)
    {
      TreeNode nodesOfRadio = new TreeNode(table.Name);
      nodesOfRadio.Tag = (object) table;
      if (table == null || table.childMemoryBlocks == null)
        return nodesOfRadio;
      TreeNode treeNode = (TreeNode) null;
      int num = 0;
      for (int index = 0; index < table.childMemoryBlocks.Count; ++index)
      {
        S3_MemoryBlock childMemoryBlock1 = table.childMemoryBlocks[index];
        switch (childMemoryBlock1)
        {
          case RadioListHeader _:
            RadioListHeader radioListHeader = childMemoryBlock1 as RadioListHeader;
            treeNode = new TreeNode(radioListHeader.Name);
            treeNode.Tag = (object) radioListHeader;
            num = 0;
            if (radioListHeader.IsSelected)
              this.MarkTreeNodeAsActive(treeNode);
            if (radioListHeader.childMemoryBlocks != null)
            {
              foreach (S3_MemoryBlock childMemoryBlock2 in radioListHeader.childMemoryBlocks)
              {
                if (childMemoryBlock2 is RadioListHeaderItem)
                {
                  RadioListHeaderItem radioListHeaderItem = childMemoryBlock2 as RadioListHeaderItem;
                  treeNode.Nodes.Add(new TreeNode(radioListHeaderItem.Description)
                  {
                    Tag = (object) radioListHeaderItem
                  });
                }
              }
            }
            nodesOfRadio.Nodes.Add(treeNode);
            break;
          case RadioList _:
            TreeNode node = treeNode.Nodes[num++];
            this.GetParameterListNodes(childMemoryBlock1.childMemoryBlocks, node);
            this.RefreshParameterNumbers(node);
            break;
        }
      }
      return nodesOfRadio;
    }

    private void treeLogger_ItemDrag(object sender, ItemDragEventArgs e)
    {
      if (!(((TreeNode) e.Item).Tag is LoggerChanal tag) || tag.isMaxValues || tag.isGenerateMidnightEvent)
        return;
      int num = (int) this.DoDragDrop(e.Item, DragDropEffects.Copy);
    }

    private void tree_ItemDrag(object sender, ItemDragEventArgs e)
    {
      object tag = ((TreeNode) e.Item).Tag;
      int num1;
      switch (tag)
      {
        case MBusList _:
        case ListLink _:
        case MBusParameter _:
        case RadioListHeader _:
          num1 = 1;
          break;
        default:
          num1 = tag is RadioListHeaderItem ? 1 : 0;
          break;
      }
      if (num1 == 0)
        return;
      int num2 = (int) this.DoDragDrop(e.Item, DragDropEffects.Copy);
    }

    private void treeParameter_ItemDrag(object sender, ItemDragEventArgs e)
    {
      int num = (int) this.DoDragDrop(e.Item, DragDropEffects.Copy);
    }

    private void tree_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Copy;

    private void tree_DragOver(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.tree.GetNodeAt(this.tree.PointToClient(new Point(e.X, e.Y)));
      if (nodeAt == null)
        return;
      this.tree.SelectedNode = nodeAt;
      this.tree.Select();
      if (nodeAt == this.tree.TopNode && this.tree.TopNode.PrevNode != null)
        this.tree.TopNode.PrevNode.EnsureVisible();
      if (nodeAt == this.tree.TopNode && this.tree.TopNode.Parent != null)
        this.tree.TopNode.Parent.EnsureVisible();
      TreeNode data = (TreeNode) e.Data.GetData(typeof (TreeNode));
      object tag = data.Tag;
      int num1;
      switch (tag)
      {
        case MBusList _:
          MBusList mbusList = tag as MBusList;
          bool flag1 = nodeAt.Tag is MBusList && tag != nodeAt.Tag;
          bool flag2 = nodeAt.Tag is ListLink && tag != nodeAt.Parent.Parent.Tag;
          bool flag3 = nodeAt.Tag is string && nodeAt.Tag.ToString() == "FOLLOWING_LIST" && tag != nodeAt.Parent.Tag;
          if (flag1)
          {
            e.Effect = e.AllowedEffect;
            return;
          }
          if (flag2)
          {
            if (nodeAt.Parent.Nodes.ContainsKey(mbusList.Name))
            {
              e.Effect = DragDropEffects.None;
              return;
            }
            e.Effect = e.AllowedEffect;
            return;
          }
          if (flag3)
          {
            if (nodeAt.Nodes.ContainsKey(mbusList.Name))
            {
              e.Effect = DragDropEffects.None;
              return;
            }
            e.Effect = e.AllowedEffect;
            return;
          }
          e.Effect = DragDropEffects.None;
          return;
        case S3_Parameter _:
          num1 = 1;
          break;
        default:
          num1 = tag is LoggerChanal ? 1 : 0;
          break;
      }
      if (num1 != 0)
      {
        if (nodeAt.Tag is MBusList || nodeAt.Tag is MBusParameter || nodeAt.Tag is MBusParameterGroup || nodeAt.Tag is RadioListHeaderItem)
          e.Effect = e.AllowedEffect;
        else
          e.Effect = DragDropEffects.None;
      }
      else
      {
        switch (tag)
        {
          case ListLink _:
            if (nodeAt.Tag is ListLink && nodeAt.Parent.Nodes.Contains(data))
            {
              e.Effect = e.AllowedEffect;
              break;
            }
            e.Effect = DragDropEffects.None;
            break;
          case MBusParameter _:
            if (nodeAt.Tag is MBusParameter && nodeAt.Parent.Nodes.Contains(data))
            {
              e.Effect = e.AllowedEffect;
              break;
            }
            e.Effect = DragDropEffects.None;
            break;
          case RadioListHeader _:
            if (nodeAt.Tag is RadioListHeader && nodeAt.Parent.Nodes.Contains(data))
            {
              e.Effect = e.AllowedEffect;
              break;
            }
            e.Effect = DragDropEffects.None;
            break;
          case RadioListHeaderItem _:
            if (nodeAt.Tag is RadioListHeaderItem && nodeAt.Parent.Nodes.Contains(data))
            {
              e.Effect = e.AllowedEffect;
              break;
            }
            e.Effect = DragDropEffects.None;
            break;
          case RadioList _:
            int num2 = !(nodeAt.Tag is RadioList) ? 0 : (nodeAt.Parent.Nodes.Contains(data) ? 1 : 0);
            e.Effect = num2 == 0 ? DragDropEffects.None : e.AllowedEffect;
            break;
        }
      }
    }

    private void tree_DragDrop(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.tree.GetNodeAt(this.tree.PointToClient(new Point(e.X, e.Y)));
      if (nodeAt == null)
        return;
      TreeNode data = (TreeNode) e.Data.GetData(typeof (TreeNode));
      S3_MemoryBlock tag1 = data.Tag as S3_MemoryBlock;
      if (nodeAt.Tag is MBusParameter)
      {
        MBusParameter tag2 = nodeAt.Tag as MBusParameter;
        if (tag2.parentMemoryBlock is MBusList)
        {
          MBusList parentMemoryBlock = tag2.parentMemoryBlock as MBusList;
          int num = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag2) + 1;
          if (tag2 == tag1)
            return;
          MBusParameter mbusParameter = (MBusParameter) null;
          switch (tag1)
          {
            case S3_Parameter _:
              mbusParameter = parentMemoryBlock.AddParameter(tag1 as S3_Parameter, new int?(num));
              break;
            case LoggerChanal _:
              mbusParameter = parentMemoryBlock.AddLogger(tag1 as LoggerChanal, new int?(num));
              break;
            case MBusParameter _:
              int index1 = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag2);
              if (!parentMemoryBlock.childMemoryBlocks.Remove(tag1))
                return;
              parentMemoryBlock.childMemoryBlocks.Insert(index1, tag1);
              int index2 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
              nodeAt.Parent.Nodes.Remove(data);
              nodeAt.Parent.Nodes.Insert(index2, data);
              this.tree.SelectedNode = data;
              this.tree.Select();
              break;
          }
          if (mbusParameter != null)
          {
            int index3 = nodeAt.Parent.Nodes.IndexOf(nodeAt) + 1;
            TreeNode treeNode = nodeAt.Parent.Nodes.Insert(index3, mbusParameter.Name);
            treeNode.Text = index3.ToString() + ". " + mbusParameter.Name;
            treeNode.Name = mbusParameter.Name;
            treeNode.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
            treeNode.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
            treeNode.Tag = (object) mbusParameter;
          }
          this.RefreshParameterNumbers(nodeAt.Parent);
        }
        else if (tag2.parentMemoryBlock is MBusParameterGroup)
        {
          MBusParameterGroup parentMemoryBlock = tag2.parentMemoryBlock as MBusParameterGroup;
          int num = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag2) + 1;
          if (tag2 == tag1)
            return;
          MBusParameter mbusParameter = (MBusParameter) null;
          if (tag1 is LoggerChanal)
            mbusParameter = parentMemoryBlock.AddLogger(tag1 as LoggerChanal, new int?(num));
          else if (tag1 is MBusParameter)
          {
            int index4 = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag2);
            if (!parentMemoryBlock.childMemoryBlocks.Remove(tag1))
              return;
            parentMemoryBlock.childMemoryBlocks.Insert(index4, tag1);
            int index5 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
            nodeAt.Parent.Nodes.Remove(data);
            nodeAt.Parent.Nodes.Insert(index5, data);
            this.tree.SelectedNode = data;
            this.tree.Select();
          }
          if (mbusParameter != null)
          {
            int index = nodeAt.Parent.Nodes.IndexOf(nodeAt) + 1;
            TreeNode treeNode = nodeAt.Parent.Nodes.Insert(index, mbusParameter.Name);
            treeNode.Text = index.ToString() + ". " + mbusParameter.Name;
            treeNode.Name = mbusParameter.Name;
            treeNode.ImageIndex = 3;
            treeNode.SelectedImageIndex = 3;
            treeNode.Tag = (object) mbusParameter;
          }
          this.RefreshParameterNumbers(nodeAt.Parent);
          this.RefreshParameterNumbers(nodeAt.Parent.Parent);
        }
        else
        {
          if (!(tag2.parentMemoryBlock is RadioList) || tag2 == tag1)
            return;
          RadioList parentMemoryBlock = tag2.parentMemoryBlock as RadioList;
          int num = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag2) + 1;
          RadioListHeader tag3 = nodeAt.Parent.Parent.Tag as RadioListHeader;
          RadioListHeaderItem tag4 = nodeAt.Parent.Tag as RadioListHeaderItem;
          MBusParameter mbusParameter = (MBusParameter) null;
          switch (tag1)
          {
            case S3_Parameter _:
              mbusParameter = parentMemoryBlock.AddParameter(tag1 as S3_Parameter, new int?(num), tag3, tag4.IndexOfVirtualDevice);
              break;
            case LoggerChanal _:
              mbusParameter = parentMemoryBlock.AddLogger(tag1 as LoggerChanal, new int?(num), tag3, tag4.IndexOfVirtualDevice);
              break;
            case MBusParameter _:
              int index6 = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag2);
              if (!parentMemoryBlock.childMemoryBlocks.Remove(tag1))
                return;
              parentMemoryBlock.childMemoryBlocks.Insert(index6, tag1);
              int index7 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
              nodeAt.Parent.Nodes.Remove(data);
              nodeAt.Parent.Nodes.Insert(index7, data);
              this.tree.SelectedNode = data;
              this.tree.Select();
              break;
          }
          if (mbusParameter != null)
          {
            int index8 = nodeAt.Parent.Nodes.IndexOf(nodeAt) + 1;
            TreeNode treeNode = nodeAt.Parent.Nodes.Insert(index8, mbusParameter.Name);
            treeNode.Text = index8.ToString() + ". " + mbusParameter.Name;
            treeNode.Name = mbusParameter.Name;
            treeNode.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
            treeNode.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
            treeNode.Tag = (object) mbusParameter;
          }
          this.RefreshParameterNumbers(nodeAt.Parent);
        }
      }
      else if (nodeAt.Tag is MBusList)
      {
        MBusList tag5 = nodeAt.Tag as MBusList;
        if (tag1 is S3_Parameter)
        {
          MBusParameter mbusParameter = tag5.AddParameter(tag1 as S3_Parameter, new int?());
          if (mbusParameter == null)
            return;
          int count = nodeAt.Nodes.Count;
          TreeNode treeNode = nodeAt.Nodes.Insert(count, mbusParameter.Name);
          treeNode.Text = count.ToString() + ". " + mbusParameter.Name;
          treeNode.Name = mbusParameter.Name;
          treeNode.ImageIndex = 2;
          treeNode.SelectedImageIndex = 2;
          treeNode.Tag = (object) mbusParameter;
          this.RefreshParameterNumbers(nodeAt);
        }
        if (tag1 is MBusList)
        {
          MBusList mbusList = tag1 as MBusList;
          MBusTransmitter parentMemoryBlock = mbusList.parentMemoryBlock as MBusTransmitter;
          int index9 = parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag5);
          if (!parentMemoryBlock.childMemoryBlocks.Remove((S3_MemoryBlock) mbusList))
            return;
          parentMemoryBlock.childMemoryBlocks.Insert(index9, (S3_MemoryBlock) mbusList);
          int index10 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
          nodeAt.Parent.Nodes.Remove(data);
          nodeAt.Parent.Nodes.Insert(index10, data);
          this.tree.SelectedNode = data;
          this.tree.Select();
        }
        else
        {
          if (!(tag1 is LoggerChanal))
            return;
          MBusParameter mbusParameter = tag5.AddLogger(tag1 as LoggerChanal, new int?());
          if (mbusParameter == null)
            return;
          int count = nodeAt.Nodes.Count;
          TreeNode treeNode = nodeAt.Nodes.Insert(count, mbusParameter.Name);
          treeNode.Text = count.ToString() + ". " + mbusParameter.Name;
          treeNode.Name = mbusParameter.Name;
          treeNode.ImageIndex = 3;
          treeNode.SelectedImageIndex = 3;
          treeNode.Tag = (object) mbusParameter;
          this.RefreshParameterNumbers(nodeAt);
        }
      }
      else if (nodeAt.Tag is string && nodeAt.Tag.ToString() == "FOLLOWING_LIST")
      {
        MBusList tag6 = nodeAt.Parent.Tag as MBusList;
        if (!(tag1 is MBusList))
          return;
        ListLink listLink = tag6.AddLink(((MBusList) tag1).Name, new int?(0));
        if (listLink == null)
          return;
        TreeNode treeNode = nodeAt.Nodes.Insert(0, listLink.Name);
        treeNode.Text = listLink.Name;
        treeNode.Name = listLink.Name;
        treeNode.ImageIndex = 1;
        treeNode.SelectedImageIndex = 1;
        treeNode.Tag = (object) listLink;
        nodeAt.Expand();
      }
      else if (nodeAt.Tag is ListLink)
      {
        switch (tag1)
        {
          case MBusList _:
            ListLink tag7 = nodeAt.Tag as ListLink;
            MBusList parentMemoryBlock1 = tag7.parentMemoryBlock as MBusList;
            int num1 = parentMemoryBlock1.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag7) + 1;
            ListLink listLink1 = parentMemoryBlock1.AddLink(((MBusList) tag1).Name, new int?(num1));
            if (listLink1 == null)
              break;
            int index11 = nodeAt.Parent.Nodes.IndexOf(nodeAt) + 1;
            TreeNode treeNode1 = nodeAt.Parent.Nodes.Insert(index11, listLink1.Name);
            treeNode1.Text = listLink1.Name;
            treeNode1.Name = listLink1.Name;
            treeNode1.ImageIndex = 1;
            treeNode1.SelectedImageIndex = 1;
            treeNode1.Tag = (object) listLink1;
            break;
          case ListLink _:
            ListLink listLink2 = tag1 as ListLink;
            ListLink tag8 = nodeAt.Tag as ListLink;
            if (listLink2 == tag8)
              break;
            MBusList parentMemoryBlock2 = tag8.parentMemoryBlock as MBusList;
            int index12 = parentMemoryBlock2.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag8);
            if (!parentMemoryBlock2.childMemoryBlocks.Remove((S3_MemoryBlock) listLink2))
              break;
            parentMemoryBlock2.childMemoryBlocks.Insert(index12, (S3_MemoryBlock) listLink2);
            int index13 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
            nodeAt.Parent.Nodes.Remove(data);
            nodeAt.Parent.Nodes.Insert(index13, data);
            this.tree.SelectedNode = data;
            this.tree.Select();
            break;
        }
      }
      else if (nodeAt.Tag is MBusParameterGroup)
      {
        if (tag1 is LoggerChanal)
        {
          MBusParameterGroup tag9 = nodeAt.Tag as MBusParameterGroup;
          MBusParameter mbusParameter = tag9.AddLogger(tag1 as LoggerChanal, new int?());
          if (mbusParameter == null)
            return;
          TreeNode treeNode2 = nodeAt.Nodes.Insert(0, mbusParameter.Name);
          treeNode2.Text = mbusParameter.Name;
          treeNode2.Name = mbusParameter.Name;
          treeNode2.ImageIndex = 3;
          treeNode2.SelectedImageIndex = 3;
          treeNode2.Tag = (object) mbusParameter;
          nodeAt.ExpandAll();
          if (tag9.childMemoryBlocks.Count == 1)
          {
            MBusDifVif mbusDifVif = new MBusDifVif();
            mbusDifVif.LoadDifVif(mbusParameter.VifDif.ToArray());
            tag9.StartStorageNumber = mbusDifVif.StorageNumber;
          }
        }
        this.RefreshParameterNumbers(nodeAt);
        this.RefreshParameterNumbers(nodeAt.Parent);
      }
      else if (nodeAt.Tag is RadioListHeaderItem)
      {
        RadioListHeaderItem tag10 = nodeAt.Tag as RadioListHeaderItem;
        RadioListHeader tag11 = nodeAt.Parent.Tag as RadioListHeader;
        RadioTransmitter tag12 = nodeAt.Parent.Parent.Tag as RadioTransmitter;
        int num2 = tag11.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag10);
        int num3 = tag12.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag11);
        if (tag1 is S3_Parameter || tag1 is LoggerChanal)
        {
          int index14 = num3 + 1 + num2;
          RadioList childMemoryBlock = tag11.parentMemoryBlock.childMemoryBlocks[index14] as RadioList;
          MBusParameter mbusParameter = (MBusParameter) null;
          if (tag1 is S3_Parameter)
            mbusParameter = childMemoryBlock.AddParameter(tag1 as S3_Parameter, new int?(0), tag11, tag10.IndexOfVirtualDevice);
          else if (tag1 is LoggerChanal)
            mbusParameter = childMemoryBlock.AddLogger(tag1 as LoggerChanal, new int?(0), tag11, tag10.IndexOfVirtualDevice);
          if (mbusParameter != null)
          {
            int index15 = nodeAt.Nodes.IndexOf(nodeAt) + 1;
            TreeNode treeNode3 = nodeAt.Nodes.Insert(index15, mbusParameter.Name);
            treeNode3.Text = index15.ToString() + ". " + mbusParameter.Name;
            treeNode3.Name = mbusParameter.Name;
            treeNode3.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
            treeNode3.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
            treeNode3.Tag = (object) mbusParameter;
          }
          this.RefreshParameterNumbers(nodeAt);
        }
        else
        {
          if (!(tag1 is RadioListHeaderItem))
            return;
          RadioListHeaderItem radioListHeaderItem = tag1 as RadioListHeaderItem;
          int num4 = radioListHeaderItem.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) radioListHeaderItem);
          tag11.childMemoryBlocks.Remove(tag1);
          int index16 = num2;
          tag11.childMemoryBlocks.Insert(index16, (S3_MemoryBlock) radioListHeaderItem);
          int num5 = tag12.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag11) + 1;
          S3_MemoryBlock childMemoryBlock = tag12.childMemoryBlocks[num5 + num4];
          tag12.childMemoryBlocks.Remove(childMemoryBlock);
          tag12.childMemoryBlocks.Insert(num5 + index16, childMemoryBlock);
          int index17 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
          nodeAt.Parent.Nodes.Remove(data);
          nodeAt.Parent.Nodes.Insert(index17, data);
          this.tree.SelectedNode = data;
          this.tree.Select();
          this.RefreshParameterNumbers(nodeAt.Parent);
        }
      }
      else
      {
        if (!(nodeAt.Tag is RadioListHeader) || !(tag1 is RadioListHeader))
          return;
        RadioListHeader tag13 = nodeAt.Tag as RadioListHeader;
        RadioTransmitter tag14 = nodeAt.Parent.Tag as RadioTransmitter;
        RadioListHeader radioListHeader = tag1 as RadioListHeader;
        if (radioListHeader == tag13)
          return;
        int count = radioListHeader.childMemoryBlocks.Count;
        int index18 = tag14.childMemoryBlocks.IndexOf((S3_MemoryBlock) radioListHeader);
        int index19 = tag14.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag13);
        if (index18 < index19)
          index19 += tag13.childMemoryBlocks.Count - 1;
        S3_MemoryBlock childMemoryBlock1 = tag14.childMemoryBlocks[index18];
        tag14.childMemoryBlocks.Remove(childMemoryBlock1);
        tag14.childMemoryBlocks.Insert(index19, childMemoryBlock1);
        for (int index20 = 1; index20 < count; ++index20)
        {
          if (index19 < index18)
            ++index18;
          S3_MemoryBlock childMemoryBlock2 = tag14.childMemoryBlocks[index18];
          tag14.childMemoryBlocks.Remove(childMemoryBlock2);
          index19 = tag14.childMemoryBlocks.IndexOf((S3_MemoryBlock) radioListHeader) + index20;
          tag14.childMemoryBlocks.Insert(index19, childMemoryBlock2);
        }
        int index21 = nodeAt.Parent.Nodes.IndexOf(nodeAt);
        nodeAt.Parent.Nodes.Remove(data);
        nodeAt.Parent.Nodes.Insert(index21, data);
        this.tree.SelectedNode = data;
        this.tree.Select();
        this.RefreshParameterNumbers(nodeAt.Parent);
      }
    }

    private string GetLoggerNameForGroupParameter(MBusParameter p)
    {
      string str = "";
      LoggerChanal loggerChanal = this.MyMeter.MyLoggerManager.GetLoggerChanal(p.Name);
      string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(loggerChanal.chanalParameter.Name);
      return (!string.IsNullOrEmpty(parameterNameByName) ? str + parameterNameByName : str + loggerChanal.chanalParameter.Name) + " " + p.ControlWord0.ParamCode.ToString();
    }

    private string GetLoggerName(MBusParameter p)
    {
      string str1 = "";
      LoggerChanal loggerChanal = this.MyMeter.MyLoggerManager.GetLoggerChanal(p.Name);
      string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(loggerChanal.chanalParameter.Name);
      string str2 = (!string.IsNullOrEmpty(parameterNameByName) ? str1 + parameterNameByName : str1 + loggerChanal.chanalParameter.Name) + " " + p.ControlWord0.ParamCode.ToString();
      if (p.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset)
        str2 += " (due date)";
      string str3 = str2 + " by " + loggerChanal.myLoggerConfig.IntervalString;
      return p.Iteration == null || p.Iteration.ControlWord1 == null ? str3 + " (1 value)" : str3 + " (" + (p.Iteration.ControlWord1.LoggerCycleCount + 1).ToString() + " values)";
    }

    private void RefreshParameterNumbers(TreeNode list)
    {
      if (list == null)
        return;
      if (list.Tag is MBusList tag1)
      {
        int outputBufferSize = tag1.GetOutputBufferSize(false);
        list.Text = outputBufferSize <= 254 ? tag1.Name + ", Size: " + outputBufferSize.ToString() + " bytes" : tag1.Name + ", Size: " + outputBufferSize.ToString() + " !!!!! MAX: 255 bytes !!!!!";
        foreach (TreeNode node in list.Nodes)
        {
          if (node.Tag is MBusParameter)
          {
            MBusParameter tag = node.Tag as MBusParameter;
            string empty = string.Empty;
            string str;
            if (tag.IsLogger)
            {
              str = this.GetLoggerName(tag);
            }
            else
            {
              string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(node.Name);
              str = !string.IsNullOrEmpty(parameterNameByName) ? parameterNameByName : node.Name;
            }
            node.Text = list.Nodes.IndexOf(node).ToString() + ". " + str;
          }
        }
      }
      if (list.Tag is MBusParameterGroup)
      {
        MBusParameterGroup tag2 = list.Tag as MBusParameterGroup;
        list.Text = list.Parent.Nodes.IndexOf(list).ToString() + ". " + tag2.Name;
        foreach (TreeNode node in list.Nodes)
        {
          string forGroupParameter = this.GetLoggerNameForGroupParameter(node.Tag as MBusParameter);
          node.Text = list.Nodes.IndexOf(node).ToString() + ". " + forGroupParameter;
        }
      }
      if (list.Tag is RadioListHeaderItem tag4)
      {
        list.Text = tag4.Description;
        foreach (TreeNode node in list.Nodes)
        {
          if (node.Tag is MBusParameter)
          {
            MBusParameter tag3 = node.Tag as MBusParameter;
            string empty = string.Empty;
            if (tag3.Name != null)
            {
              string str;
              if (tag3.IsLogger)
              {
                str = this.GetLoggerName(tag3);
              }
              else
              {
                string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(tag3.Name);
                str = !string.IsNullOrEmpty(parameterNameByName) ? parameterNameByName : tag3.Name;
              }
              node.Text = list.Nodes.IndexOf(node).ToString() + ". " + str;
            }
          }
        }
      }
      if (list.Tag is RadioListHeader tag5)
      {
        list.Text = tag5.Name;
        if (tag5.IsSelected)
          this.MarkTreeNodeAsActive(list);
      }
      if (!(list.Tag is RadioTransmitter))
        return;
      foreach (TreeNode node in list.Nodes)
      {
        RadioListHeader tag6 = node.Tag as RadioListHeader;
        node.Text = tag6.Name;
        if (tag6.IsSelected)
          this.MarkTreeNodeAsActive(node);
      }
    }

    private void MarkTreeNodeAsActive(TreeNode theNode)
    {
      theNode.Text += " = activated";
      if (!(theNode.Tag is RadioListHeader))
        return;
      RadioListHeader tag = theNode.Tag as RadioListHeader;
      if (this.tempRadioAndEncMode.HasValue)
      {
        ComboBox comboBoxRadioMode = this.comboBoxRadioMode;
        byte? tempRadioAndEncMode = this.tempRadioAndEncMode;
        int num = (tempRadioAndEncMode.HasValue ? new int?((int) tempRadioAndEncMode.GetValueOrDefault() >> 4) : new int?()).Value;
        comboBoxRadioMode.SelectedIndex = num;
      }
      else
        this.comboBoxRadioMode.SelectedIndex = (int) tag.Mode;
    }

    private void SetTreeNodeAsActive(TreeNode theNode)
    {
      theNode.Text += " = activated";
      if (!(theNode.Tag is RadioListHeader))
        return;
      RadioListHeader tag = theNode.Tag as RadioListHeader;
      if (this.tempRadioAndEncMode.HasValue)
      {
        this.tempRadioAndEncMode = new byte?((byte) (((int) this.tempRadioAndEncMode.Value & 15) + ((int) tag.Mode << 4)));
        this.comboBoxRadioMode.SelectedIndex = (int) this.tempRadioAndEncMode.Value >> 4;
      }
      else
        this.comboBoxRadioMode.SelectedIndex = (int) tag.Mode;
      this.ShowRadioAndEncryptionMode(this.cboxRadioMode.SelectedIndex);
    }

    private void ShowRadioAndEncryptionMode(int radioModeIndexFromList)
    {
      if (this.tempRadioAndEncMode.HasValue)
      {
        this.comboBoxRadioMode.SelectedIndex = (int) this.tempRadioAndEncMode.Value >> 4;
        this.comboBox_wMBusEncryption.SelectedItem = (object) ((AES_ENCRYPTION_MODE) ((int) this.tempRadioAndEncMode.Value & 15)).ToString();
      }
      else
      {
        this.comboBoxRadioMode.SelectedIndex = radioModeIndexFromList;
        this.comboBox_wMBusEncryption.SelectedItem = (object) AES_ENCRYPTION_MODE.MODE_5.ToString();
      }
    }

    private void ChangeParameter(object sender, EventArgs e)
    {
      if (!this.gboxSettings.Enabled || this.tree.SelectedNode == null)
        return;
      if (this.tree.SelectedNode.Tag is RadioListHeaderItem tag3)
      {
        tag3.IndexOfVirtualDevice = this.cboxRadioDevice.SelectedIndex;
        tag3.RadioIdOffset = Convert.ToUInt16(this.txtRadioIdOffset.Value);
        this.RefreshParameterNumbers(this.tree.SelectedNode);
        this.RefreshParameterNumbers(this.tree.SelectedNode.Parent);
      }
      else if (this.tree.SelectedNode.Tag is RadioListHeader tag2)
      {
        tag2.Mode = (RADIO_MODE) Enum.Parse(typeof (RADIO_MODE), this.cboxRadioMode.SelectedItem.ToString(), true);
        this.RefreshParameterNumbers(this.tree.SelectedNode);
        this.RefreshParameterNumbers(this.tree.SelectedNode.Parent);
      }
      else if (this.tree.SelectedNode.Tag is MBusParameterGroup tag1)
      {
        tag1.IsDueDate = this.ckboxLoggerDueDate.Checked;
        tag1.StartStorageNumber = (int) this.txtStartStorageNumber.Value;
        tag1.SetTotalCountOfValues((int) this.txtCountOfValues.Value);
        this.txtCountOfValues.Value = (Decimal) tag1.TotalCountOfValues;
        this.RefreshParameterNumbers(this.tree.SelectedNode);
        this.RefreshParameterNumbers(this.tree.SelectedNode.Parent);
      }
      else
      {
        if (!(this.tree.SelectedNode.Tag is MBusParameter tag) || !tag.IsLogger)
          return;
        ParamCode paramCode = ParamCode.None;
        switch ((TransmitParameterView.LoggerType) Enum.Parse(typeof (TransmitParameterView.LoggerType), this.cboxLoggerType.SelectedValue.ToString(), true))
        {
          case TransmitParameterView.LoggerType.Value:
            paramCode = ParamCode.LogValue;
            break;
          case TransmitParameterView.LoggerType.Date:
            paramCode = ParamCode.LogDate;
            break;
          case TransmitParameterView.LoggerType.DateTime:
            paramCode = ParamCode.LogDateTime;
            break;
        }
        ControlLogger controlLogger = this.ckboxLoggerDueDate.Checked ? ControlLogger.LoggerDueDateReset : ControlLogger.LoggerReset;
        tag.UpdateLogger(paramCode, (int) this.txtCountOfValues.Value, controlLogger);
        if (tag.VifDif != null && tag.VifDif.Count > 0)
        {
          MBusDifVif mbusDifVif1 = new MBusDifVif();
          if (mbusDifVif1.LoadDifVif(tag.VifDif.ToArray()))
          {
            mbusDifVif1.StorageNumber = (int) this.txtStartStorageNumber.Value;
            tag.VifDif = new List<byte>((IEnumerable<byte>) mbusDifVif1.DifVifArray);
            tag.ControlWord0.DifVifCount = tag.VifDif.Count;
            tag.RecalculateByteSize();
            if (tag.Iteration != null && tag.Iteration.VifDif != null && tag.Iteration.VifDif.Count > 0)
            {
              tag.firstChildMemoryBlockOffset = tag.ByteSize;
              MBusDifVif mbusDifVif2 = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
              if (mbusDifVif2.LoadDifVif(tag.Iteration.VifDif.ToArray()))
              {
                mbusDifVif2.StorageNumber = (int) this.txtStartStorageNumber.Value;
                tag.Iteration.VifDif = new List<byte>((IEnumerable<byte>) mbusDifVif2.DifVifArray);
                tag.Iteration.ControlWord0.DifVifCount = tag.Iteration.VifDif.Count;
                tag.Iteration.RecalculateByteSize();
              }
            }
          }
        }
        this.RefreshParameterNumbers(this.tree.SelectedNode);
        this.RefreshParameterNumbers(this.tree.SelectedNode.Parent);
        this.RefreshParameterNumbers(this.tree.SelectedNode.Parent.Parent);
        this.txtParameterInfo.Text = tag.GetLoggerInfo();
      }
    }

    private void InsertRadio3Scenario5ListParameter(string parameter)
    {
      try
      {
        RadioListHeaderItem tag = this.tree.SelectedNode.Tag as RadioListHeaderItem;
        RadioListHeader parentMemoryBlock = tag.parentMemoryBlock as RadioListHeader;
        int index1 = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) parentMemoryBlock) + 1 + parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag);
        List<MBusParameter> mbusParameterList = (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[index1] as RadioList).InsertRadio3Scenario5Parameter(this.MyMeter.MyParameters.ParameterByName[parameter], parentMemoryBlock, tag.IndexOfVirtualDevice);
        if (mbusParameterList == null)
          return;
        int index2 = 0;
        foreach (MBusParameter mbusParameter in mbusParameterList)
        {
          TreeNode treeNode = this.tree.SelectedNode.Nodes.Insert(index2, mbusParameter.Name);
          treeNode.Text = index2.ToString() + ". " + mbusParameter.Name;
          treeNode.Name = mbusParameter.Name;
          treeNode.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.Tag = (object) mbusParameter;
          ++index2;
        }
        this.RefreshParameterNumbers(this.tree.SelectedNode);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void InsertRadio3Scenario3ListParameter(string parameter)
    {
      try
      {
        RadioListHeaderItem tag = this.tree.SelectedNode.Tag as RadioListHeaderItem;
        RadioListHeader parentMemoryBlock = tag.parentMemoryBlock as RadioListHeader;
        int index1 = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) parentMemoryBlock) + 1 + parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag);
        List<MBusParameter> mbusParameterList = (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[index1] as RadioList).InsertRadio3Scenario3Parameter(this.MyMeter.MyParameters.ParameterByName[parameter], parentMemoryBlock, tag.IndexOfVirtualDevice);
        if (mbusParameterList == null)
          return;
        int index2 = 0;
        foreach (MBusParameter mbusParameter in mbusParameterList)
        {
          TreeNode treeNode = this.tree.SelectedNode.Nodes.Insert(index2, mbusParameter.Name);
          treeNode.Text = index2.ToString() + ". " + mbusParameter.Name;
          treeNode.Name = mbusParameter.Name;
          treeNode.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.Tag = (object) mbusParameter;
          ++index2;
        }
        this.RefreshParameterNumbers(this.tree.SelectedNode);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void InsertRadio3Scenario1ListParameter(string parameter)
    {
      try
      {
        RadioListHeaderItem tag = this.tree.SelectedNode.Tag as RadioListHeaderItem;
        RadioListHeader parentMemoryBlock = tag.parentMemoryBlock as RadioListHeader;
        int index1 = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) parentMemoryBlock) + 1 + parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag);
        List<MBusParameter> mbusParameterList = (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[index1] as RadioList).InsertRadio3Scenario1Parameter(this.MyMeter.MyParameters.ParameterByName[parameter], parentMemoryBlock, tag.IndexOfVirtualDevice);
        if (mbusParameterList == null)
          return;
        int index2 = 0;
        foreach (MBusParameter mbusParameter in mbusParameterList)
        {
          TreeNode treeNode = this.tree.SelectedNode.Nodes.Insert(index2, mbusParameter.Name);
          treeNode.Text = index2.ToString() + ". " + mbusParameter.Name;
          treeNode.Name = mbusParameter.Name;
          treeNode.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.Tag = (object) mbusParameter;
          ++index2;
        }
        this.RefreshParameterNumbers(this.tree.SelectedNode);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void InsertRadio2WalkbyListParameter(string parameter)
    {
      try
      {
        RadioListHeaderItem tag = this.tree.SelectedNode.Tag as RadioListHeaderItem;
        RadioListHeader parentMemoryBlock = tag.parentMemoryBlock as RadioListHeader;
        int index1 = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) parentMemoryBlock) + 1 + parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) tag);
        List<MBusParameter> mbusParameterList = (parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[index1] as RadioList).InsertRadio2WalkbyParameter(this.MyMeter.MyParameters.ParameterByName[parameter], parentMemoryBlock, tag.IndexOfVirtualDevice);
        if (mbusParameterList == null)
          return;
        int index2 = 0;
        foreach (MBusParameter mbusParameter in mbusParameterList)
        {
          TreeNode treeNode = this.tree.SelectedNode.Nodes.Insert(index2, mbusParameter.Name);
          treeNode.Text = index2.ToString() + ". " + mbusParameter.Name;
          treeNode.Name = mbusParameter.Name;
          treeNode.ImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.SelectedImageIndex = mbusParameter.IsLogger ? 3 : 2;
          treeNode.Tag = (object) mbusParameter;
          ++index2;
        }
        this.RefreshParameterNumbers(this.tree.SelectedNode);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void comboBoxRadioMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.tempRadioAndEncMode.HasValue)
        return;
      this.tempRadioAndEncMode = new byte?((byte) (((int) this.tempRadioAndEncMode.Value & 15) + (this.comboBoxRadioMode.SelectedIndex << 4)));
    }

    private void comboBox_wMBusEncryption_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.tempRadioAndEncMode.HasValue)
        this.comboBox_wMBusEncryption.SelectedItem = (object) AES_ENCRYPTION_MODE.MODE_5.ToString();
      else
        this.tempRadioAndEncMode = new byte?((byte) (((int) this.tempRadioAndEncMode.Value & 240) + (int) Enum.Parse(typeof (AES_ENCRYPTION_MODE), this.comboBox_wMBusEncryption.SelectedItem.ToString())));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TransmitParameterView));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.tree = new TreeView();
      this.contextMenuTransmitLists = new ContextMenuStrip(this.components);
      this.btnAddScenario = new ToolStripMenuItem();
      this.btnAddList = new ToolStripMenuItem();
      this.btnAddRadioParameter = new ToolStripMenuItem();
      this.btnRadio2Walkby = new ToolStripMenuItem();
      this.btnAddRadio2WByVolume = new ToolStripMenuItem();
      this.btnAddRadio2WByHeat = new ToolStripMenuItem();
      this.btnAddRadio2WByCooling = new ToolStripMenuItem();
      this.btnAddRadio2WByInput1 = new ToolStripMenuItem();
      this.btnAddRadio2WByInput2 = new ToolStripMenuItem();
      this.btnAddRadio2WByInput3 = new ToolStripMenuItem();
      this.btnRadio3Scenario1 = new ToolStripMenuItem();
      this.btnAddRadio3Scen1Volume = new ToolStripMenuItem();
      this.btnAddRadio3Scen1Heat = new ToolStripMenuItem();
      this.btnAddRadio3Scen1Cooling = new ToolStripMenuItem();
      this.btnAddRadio3Scen1Input1 = new ToolStripMenuItem();
      this.btnAddRadio3Scen1Input2 = new ToolStripMenuItem();
      this.btnAddRadio3Scen1Input3 = new ToolStripMenuItem();
      this.btnRadio3Scenario3 = new ToolStripMenuItem();
      this.btnAddRadio3Scen3Volume = new ToolStripMenuItem();
      this.btnAddRadio3Scen3Heat = new ToolStripMenuItem();
      this.btnAddRadio3Scen3Cooling = new ToolStripMenuItem();
      this.btnAddRadio3Scen3Input1 = new ToolStripMenuItem();
      this.btnAddRadio3Scen3Input2 = new ToolStripMenuItem();
      this.btnAddRadio3Scen3Input3 = new ToolStripMenuItem();
      this.btnRadio3Scenario5 = new ToolStripMenuItem();
      this.btnAddRadio3Scen5Volume = new ToolStripMenuItem();
      this.btnAddRadio3Scen5Heat = new ToolStripMenuItem();
      this.btnAddRadio3Scen5Cooling = new ToolStripMenuItem();
      this.btnAddRadio3Scen5Input1 = new ToolStripMenuItem();
      this.btnAddRadio3Scen5Input2 = new ToolStripMenuItem();
      this.btnAddRadio3Scen5Input3 = new ToolStripMenuItem();
      this.btnRadio3Scenario6 = new ToolStripMenuItem();
      this.btnAddRadio3Scen6Volume = new ToolStripMenuItem();
      this.btnAddRadio3Scen6Heat = new ToolStripMenuItem();
      this.btnAddRadio3Scen6Cooling = new ToolStripMenuItem();
      this.btnAddRadio3Scen6Input1 = new ToolStripMenuItem();
      this.btnAddRadio3Scen6Input2 = new ToolStripMenuItem();
      this.btnAddRadio3Scen6Input3 = new ToolStripMenuItem();
      this.btnAddGroup = new ToolStripMenuItem();
      this.btnSetScenarioName = new ToolStripMenuItem();
      this.btnSetAsActiveList = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.btnTreeExpand = new ToolStripMenuItem();
      this.btnTreeCollaps = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.btnDeleteTaransmitList = new ToolStripMenuItem();
      this.splitContainer1 = new SplitContainer();
      this.treeParameter = new TreeView();
      this.ckboxShowAllTransmitParameter = new CheckBox();
      this.label2 = new Label();
      this.treeLogger = new TreeView();
      this.label3 = new Label();
      this.gboxSettings = new GroupBox();
      this.lblStartStorageNumber = new Label();
      this.txtRadioIdOffset = new NumericUpDown();
      this.lblRadioIdOffset = new Label();
      this.lblRadioDevice = new Label();
      this.cboxRadioDevice = new ComboBox();
      this.lblRadio = new Label();
      this.cboxRadioMode = new ComboBox();
      this.txtStartStorageNumber = new NumericUpDown();
      this.txtCountOfValues = new NumericUpDown();
      this.lblCountOfValues = new Label();
      this.ckboxLoggerDueDate = new CheckBox();
      this.lblLoggerType = new Label();
      this.cboxLoggerType = new ComboBox();
      this.labelRadioCycle = new Label();
      this.labelWMBusEncryption = new Label();
      this.labelRadioMode = new Label();
      this.comboBox_wMBusEncryption = new ComboBox();
      this.comboBoxRadioMode = new ComboBox();
      this.txtParameterInfo = new TextBox();
      this.splitContainer = new SplitContainer();
      this.groupBoxDeviceSetup = new GroupBox();
      this.comboBoxRadioCycle = new ComboBox();
      this.btnPrint = new Button();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.contextMenuTransmitLists.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.gboxSettings.SuspendLayout();
      this.txtRadioIdOffset.BeginInit();
      this.txtStartStorageNumber.BeginInit();
      this.txtCountOfValues.BeginInit();
      this.splitContainer.BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.groupBoxDeviceSetup.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(3, 2, 3, 2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1378, 62);
      this.zennerCoroprateDesign2.TabIndex = 18;
      this.tree.AllowDrop = true;
      this.tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tree.ContextMenuStrip = this.contextMenuTransmitLists;
      this.tree.HideSelection = false;
      this.tree.Location = new Point(2, 2);
      this.tree.Margin = new Padding(4, 5, 4, 5);
      this.tree.Name = "tree";
      this.tree.Size = new Size(528, 638);
      this.tree.TabIndex = 19;
      this.tree.ItemDrag += new ItemDragEventHandler(this.tree_ItemDrag);
      this.tree.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
      this.tree.DragDrop += new DragEventHandler(this.tree_DragDrop);
      this.tree.DragEnter += new DragEventHandler(this.tree_DragEnter);
      this.tree.DragOver += new DragEventHandler(this.tree_DragOver);
      this.tree.KeyUp += new KeyEventHandler(this.tree_KeyUp);
      this.tree.MouseDown += new MouseEventHandler(this.tree_MouseDown);
      this.contextMenuTransmitLists.ImageScalingSize = new Size(24, 24);
      this.contextMenuTransmitLists.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.btnAddScenario,
        (ToolStripItem) this.btnAddList,
        (ToolStripItem) this.btnAddRadioParameter,
        (ToolStripItem) this.btnAddGroup,
        (ToolStripItem) this.btnSetScenarioName,
        (ToolStripItem) this.btnSetAsActiveList,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.btnTreeExpand,
        (ToolStripItem) this.btnTreeCollaps,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.btnDeleteTaransmitList
      });
      this.contextMenuTransmitLists.Name = "contextMenuTransmitLists";
      this.contextMenuTransmitLists.Size = new Size(251, 304);
      this.contextMenuTransmitLists.Opening += new CancelEventHandler(this.contextMenuTransmitLists_Opening);
      this.btnAddScenario.Name = "btnAddScenario";
      this.btnAddScenario.Size = new Size(250, 32);
      this.btnAddScenario.Text = "Add scenario";
      this.btnAddScenario.Visible = false;
      this.btnAddScenario.Click += new System.EventHandler(this.btnAddScenario_Click);
      this.btnAddList.Name = "btnAddList";
      this.btnAddList.Size = new Size(250, 32);
      this.btnAddList.Text = "Add list";
      this.btnAddList.Click += new System.EventHandler(this.btnAddList_Click);
      this.btnAddRadioParameter.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.btnRadio2Walkby,
        (ToolStripItem) this.btnRadio3Scenario1,
        (ToolStripItem) this.btnRadio3Scenario3,
        (ToolStripItem) this.btnRadio3Scenario5,
        (ToolStripItem) this.btnRadio3Scenario6
      });
      this.btnAddRadioParameter.Name = "btnAddRadioParameter";
      this.btnAddRadioParameter.Size = new Size(250, 32);
      this.btnAddRadioParameter.Text = "Add radio parameter";
      this.btnAddRadioParameter.Visible = false;
      this.btnRadio2Walkby.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.btnAddRadio2WByVolume,
        (ToolStripItem) this.btnAddRadio2WByHeat,
        (ToolStripItem) this.btnAddRadio2WByCooling,
        (ToolStripItem) this.btnAddRadio2WByInput1,
        (ToolStripItem) this.btnAddRadio2WByInput2,
        (ToolStripItem) this.btnAddRadio2WByInput3
      });
      this.btnRadio2Walkby.Name = "btnRadio2Walkby";
      this.btnRadio2Walkby.Size = new Size(252, 34);
      this.btnRadio2Walkby.Text = "Radio2 Walkby";
      this.btnAddRadio2WByVolume.Name = "btnAddRadio2WByVolume";
      this.btnAddRadio2WByVolume.Size = new Size(176, 34);
      this.btnAddRadio2WByVolume.Text = "Volume";
      this.btnAddRadio2WByVolume.Click += new System.EventHandler(this.btnAddRadio2WByVolume_Click);
      this.btnAddRadio2WByHeat.Name = "btnAddRadio2WByHeat";
      this.btnAddRadio2WByHeat.Size = new Size(176, 34);
      this.btnAddRadio2WByHeat.Text = "Heat";
      this.btnAddRadio2WByHeat.Click += new System.EventHandler(this.btnAddRadio2WByHeat_Click);
      this.btnAddRadio2WByCooling.Name = "btnAddRadio2WByCooling";
      this.btnAddRadio2WByCooling.Size = new Size(176, 34);
      this.btnAddRadio2WByCooling.Text = "Cooling";
      this.btnAddRadio2WByCooling.Click += new System.EventHandler(this.btnAddRadio2WByCooling_Click);
      this.btnAddRadio2WByInput1.Name = "btnAddRadio2WByInput1";
      this.btnAddRadio2WByInput1.Size = new Size(176, 34);
      this.btnAddRadio2WByInput1.Text = "Input1";
      this.btnAddRadio2WByInput1.Click += new System.EventHandler(this.btnAddRadio2WByInput1_Click);
      this.btnAddRadio2WByInput2.Name = "btnAddRadio2WByInput2";
      this.btnAddRadio2WByInput2.Size = new Size(176, 34);
      this.btnAddRadio2WByInput2.Text = "Input2";
      this.btnAddRadio2WByInput2.Click += new System.EventHandler(this.btnAddRadio2WByInput2_Click);
      this.btnAddRadio2WByInput3.Name = "btnAddRadio2WByInput3";
      this.btnAddRadio2WByInput3.Size = new Size(176, 34);
      this.btnAddRadio2WByInput3.Text = "Input3";
      this.btnAddRadio2WByInput3.Click += new System.EventHandler(this.btnAddRadio2WByInput3_Click);
      this.btnRadio3Scenario1.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.btnAddRadio3Scen1Volume,
        (ToolStripItem) this.btnAddRadio3Scen1Heat,
        (ToolStripItem) this.btnAddRadio3Scen1Cooling,
        (ToolStripItem) this.btnAddRadio3Scen1Input1,
        (ToolStripItem) this.btnAddRadio3Scen1Input2,
        (ToolStripItem) this.btnAddRadio3Scen1Input3
      });
      this.btnRadio3Scenario1.Name = "btnRadio3Scenario1";
      this.btnRadio3Scenario1.Size = new Size(252, 34);
      this.btnRadio3Scenario1.Text = "Radio3 Scenario1";
      this.btnAddRadio3Scen1Volume.Name = "btnAddRadio3Scen1Volume";
      this.btnAddRadio3Scen1Volume.Size = new Size(176, 34);
      this.btnAddRadio3Scen1Volume.Text = "Volume";
      this.btnAddRadio3Scen1Volume.Click += new System.EventHandler(this.btnAddRadio3Scen1Volume_Click);
      this.btnAddRadio3Scen1Heat.Name = "btnAddRadio3Scen1Heat";
      this.btnAddRadio3Scen1Heat.Size = new Size(176, 34);
      this.btnAddRadio3Scen1Heat.Text = "Heat";
      this.btnAddRadio3Scen1Heat.Click += new System.EventHandler(this.btnAddRadio3Scen1Heat_Click);
      this.btnAddRadio3Scen1Cooling.Name = "btnAddRadio3Scen1Cooling";
      this.btnAddRadio3Scen1Cooling.Size = new Size(176, 34);
      this.btnAddRadio3Scen1Cooling.Text = "Cooling";
      this.btnAddRadio3Scen1Cooling.Click += new System.EventHandler(this.btnAddRadio3Scen1Cooling_Click);
      this.btnAddRadio3Scen1Input1.Name = "btnAddRadio3Scen1Input1";
      this.btnAddRadio3Scen1Input1.Size = new Size(176, 34);
      this.btnAddRadio3Scen1Input1.Text = "Input1";
      this.btnAddRadio3Scen1Input1.Click += new System.EventHandler(this.btnAddRadio3Scen1Input1_Click);
      this.btnAddRadio3Scen1Input2.Name = "btnAddRadio3Scen1Input2";
      this.btnAddRadio3Scen1Input2.Size = new Size(176, 34);
      this.btnAddRadio3Scen1Input2.Text = "Input2";
      this.btnAddRadio3Scen1Input2.Click += new System.EventHandler(this.btnAddRadio3Scen1Input2_Click);
      this.btnAddRadio3Scen1Input3.Name = "btnAddRadio3Scen1Input3";
      this.btnAddRadio3Scen1Input3.Size = new Size(176, 34);
      this.btnAddRadio3Scen1Input3.Text = "Input3";
      this.btnAddRadio3Scen1Input3.Click += new System.EventHandler(this.btnAddRadio3Scen1Input3_Click);
      this.btnRadio3Scenario3.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.btnAddRadio3Scen3Volume,
        (ToolStripItem) this.btnAddRadio3Scen3Heat,
        (ToolStripItem) this.btnAddRadio3Scen3Cooling,
        (ToolStripItem) this.btnAddRadio3Scen3Input1,
        (ToolStripItem) this.btnAddRadio3Scen3Input2,
        (ToolStripItem) this.btnAddRadio3Scen3Input3
      });
      this.btnRadio3Scenario3.Name = "btnRadio3Scenario3";
      this.btnRadio3Scenario3.Size = new Size(252, 34);
      this.btnRadio3Scenario3.Text = "Radio3 Scenario3";
      this.btnAddRadio3Scen3Volume.Name = "btnAddRadio3Scen3Volume";
      this.btnAddRadio3Scen3Volume.Size = new Size(176, 34);
      this.btnAddRadio3Scen3Volume.Text = "Volume";
      this.btnAddRadio3Scen3Volume.Click += new System.EventHandler(this.btnAddRadio3Scen3Volume_Click);
      this.btnAddRadio3Scen3Heat.Name = "btnAddRadio3Scen3Heat";
      this.btnAddRadio3Scen3Heat.Size = new Size(176, 34);
      this.btnAddRadio3Scen3Heat.Text = "Heat";
      this.btnAddRadio3Scen3Heat.Click += new System.EventHandler(this.btnAddRadio3Scen3Heat_Click);
      this.btnAddRadio3Scen3Cooling.Name = "btnAddRadio3Scen3Cooling";
      this.btnAddRadio3Scen3Cooling.Size = new Size(176, 34);
      this.btnAddRadio3Scen3Cooling.Text = "Cooling";
      this.btnAddRadio3Scen3Cooling.Click += new System.EventHandler(this.btnAddRadio3Scen3Cooling_Click);
      this.btnAddRadio3Scen3Input1.Name = "btnAddRadio3Scen3Input1";
      this.btnAddRadio3Scen3Input1.Size = new Size(176, 34);
      this.btnAddRadio3Scen3Input1.Text = "Input1";
      this.btnAddRadio3Scen3Input1.Click += new System.EventHandler(this.btnAddRadio3Scen3Input1_Click);
      this.btnAddRadio3Scen3Input2.Name = "btnAddRadio3Scen3Input2";
      this.btnAddRadio3Scen3Input2.Size = new Size(176, 34);
      this.btnAddRadio3Scen3Input2.Text = "Input2";
      this.btnAddRadio3Scen3Input2.Click += new System.EventHandler(this.btnAddRadio3Scen3Input2_Click);
      this.btnAddRadio3Scen3Input3.Name = "btnAddRadio3Scen3Input3";
      this.btnAddRadio3Scen3Input3.Size = new Size(176, 34);
      this.btnAddRadio3Scen3Input3.Text = "Input3";
      this.btnAddRadio3Scen3Input3.Click += new System.EventHandler(this.btnAddRadio3Scen3Input3_Click);
      this.btnRadio3Scenario5.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.btnAddRadio3Scen5Volume,
        (ToolStripItem) this.btnAddRadio3Scen5Heat,
        (ToolStripItem) this.btnAddRadio3Scen5Cooling,
        (ToolStripItem) this.btnAddRadio3Scen5Input1,
        (ToolStripItem) this.btnAddRadio3Scen5Input2,
        (ToolStripItem) this.btnAddRadio3Scen5Input3
      });
      this.btnRadio3Scenario5.Name = "btnRadio3Scenario5";
      this.btnRadio3Scenario5.Size = new Size(252, 34);
      this.btnRadio3Scenario5.Text = "Radio3 Scenario5";
      this.btnAddRadio3Scen5Volume.Name = "btnAddRadio3Scen5Volume";
      this.btnAddRadio3Scen5Volume.Size = new Size(176, 34);
      this.btnAddRadio3Scen5Volume.Text = "Volume";
      this.btnAddRadio3Scen5Volume.Click += new System.EventHandler(this.btnAddRadio3Scen5Volume_Click);
      this.btnAddRadio3Scen5Heat.Name = "btnAddRadio3Scen5Heat";
      this.btnAddRadio3Scen5Heat.Size = new Size(176, 34);
      this.btnAddRadio3Scen5Heat.Text = "Heat";
      this.btnAddRadio3Scen5Heat.Click += new System.EventHandler(this.btnAddRadio3Scen5Heat_Click);
      this.btnAddRadio3Scen5Cooling.Name = "btnAddRadio3Scen5Cooling";
      this.btnAddRadio3Scen5Cooling.Size = new Size(176, 34);
      this.btnAddRadio3Scen5Cooling.Text = "Cooling";
      this.btnAddRadio3Scen5Cooling.Click += new System.EventHandler(this.btnAddRadio3Scen5Cooling_Click);
      this.btnAddRadio3Scen5Input1.Name = "btnAddRadio3Scen5Input1";
      this.btnAddRadio3Scen5Input1.Size = new Size(176, 34);
      this.btnAddRadio3Scen5Input1.Text = "Input1";
      this.btnAddRadio3Scen5Input1.Click += new System.EventHandler(this.btnAddRadio3Scen5Input1_Click);
      this.btnAddRadio3Scen5Input2.Name = "btnAddRadio3Scen5Input2";
      this.btnAddRadio3Scen5Input2.Size = new Size(176, 34);
      this.btnAddRadio3Scen5Input2.Text = "Input2";
      this.btnAddRadio3Scen5Input2.Click += new System.EventHandler(this.btnAddRadio3Scen5Input2_Click);
      this.btnAddRadio3Scen5Input3.Name = "btnAddRadio3Scen5Input3";
      this.btnAddRadio3Scen5Input3.Size = new Size(176, 34);
      this.btnAddRadio3Scen5Input3.Text = "Input3";
      this.btnAddRadio3Scen5Input3.Click += new System.EventHandler(this.btnAddRadio3Scen5Input3_Click);
      this.btnRadio3Scenario6.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.btnAddRadio3Scen6Volume,
        (ToolStripItem) this.btnAddRadio3Scen6Heat,
        (ToolStripItem) this.btnAddRadio3Scen6Cooling,
        (ToolStripItem) this.btnAddRadio3Scen6Input1,
        (ToolStripItem) this.btnAddRadio3Scen6Input2,
        (ToolStripItem) this.btnAddRadio3Scen6Input3
      });
      this.btnRadio3Scenario6.Name = "btnRadio3Scenario6";
      this.btnRadio3Scenario6.Size = new Size(252, 34);
      this.btnRadio3Scenario6.Text = "Radio3 Scenario6";
      this.btnAddRadio3Scen6Volume.Name = "btnAddRadio3Scen6Volume";
      this.btnAddRadio3Scen6Volume.Size = new Size(176, 34);
      this.btnAddRadio3Scen6Volume.Text = "Volume";
      this.btnAddRadio3Scen6Volume.Click += new System.EventHandler(this.btnAddRadio3Scen5Volume_Click);
      this.btnAddRadio3Scen6Heat.Name = "btnAddRadio3Scen6Heat";
      this.btnAddRadio3Scen6Heat.Size = new Size(176, 34);
      this.btnAddRadio3Scen6Heat.Text = "Heat";
      this.btnAddRadio3Scen6Heat.Click += new System.EventHandler(this.btnAddRadio3Scen5Heat_Click);
      this.btnAddRadio3Scen6Cooling.Name = "btnAddRadio3Scen6Cooling";
      this.btnAddRadio3Scen6Cooling.Size = new Size(176, 34);
      this.btnAddRadio3Scen6Cooling.Text = "Cooling";
      this.btnAddRadio3Scen6Cooling.Click += new System.EventHandler(this.btnAddRadio3Scen5Cooling_Click);
      this.btnAddRadio3Scen6Input1.Name = "btnAddRadio3Scen6Input1";
      this.btnAddRadio3Scen6Input1.Size = new Size(176, 34);
      this.btnAddRadio3Scen6Input1.Text = "Input1";
      this.btnAddRadio3Scen6Input1.Click += new System.EventHandler(this.btnAddRadio3Scen5Input1_Click);
      this.btnAddRadio3Scen6Input2.Name = "btnAddRadio3Scen6Input2";
      this.btnAddRadio3Scen6Input2.Size = new Size(176, 34);
      this.btnAddRadio3Scen6Input2.Text = "Input2";
      this.btnAddRadio3Scen6Input2.Click += new System.EventHandler(this.btnAddRadio3Scen5Input2_Click);
      this.btnAddRadio3Scen6Input3.Name = "btnAddRadio3Scen6Input3";
      this.btnAddRadio3Scen6Input3.Size = new Size(176, 34);
      this.btnAddRadio3Scen6Input3.Text = "Input3";
      this.btnAddRadio3Scen6Input3.Click += new System.EventHandler(this.btnAddRadio3Scen5Input3_Click);
      this.btnAddGroup.Name = "btnAddGroup";
      this.btnAddGroup.Size = new Size(250, 32);
      this.btnAddGroup.Text = "Add group";
      this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
      this.btnSetScenarioName.Name = "btnSetScenarioName";
      this.btnSetScenarioName.Size = new Size(250, 32);
      this.btnSetScenarioName.Text = "Set Scenario Name";
      this.btnSetScenarioName.Visible = false;
      this.btnSetScenarioName.Click += new System.EventHandler(this.btnSetScenarioName_Click);
      this.btnSetAsActiveList.Name = "btnSetAsActiveList";
      this.btnSetAsActiveList.Size = new Size(250, 32);
      this.btnSetAsActiveList.Text = "Set as active list ";
      this.btnSetAsActiveList.Click += new System.EventHandler(this.btnSetAsActiveList_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(247, 6);
      this.btnTreeExpand.Name = "btnTreeExpand";
      this.btnTreeExpand.Size = new Size(250, 32);
      this.btnTreeExpand.Text = "Expand";
      this.btnTreeExpand.Click += new System.EventHandler(this.btnTreeExpand_Click);
      this.btnTreeCollaps.Name = "btnTreeCollaps";
      this.btnTreeCollaps.Size = new Size(250, 32);
      this.btnTreeCollaps.Text = "Collaps";
      this.btnTreeCollaps.Click += new System.EventHandler(this.btnTreeCollaps_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(247, 6);
      this.btnDeleteTaransmitList.Name = "btnDeleteTaransmitList";
      this.btnDeleteTaransmitList.Size = new Size(250, 32);
      this.btnDeleteTaransmitList.Text = "Delete";
      this.btnDeleteTaransmitList.Click += new System.EventHandler(this.btnDeleteTaransmitList_Click);
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(4, 465);
      this.splitContainer1.Margin = new Padding(4, 5, 4, 5);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.treeParameter);
      this.splitContainer1.Panel1.Controls.Add((Control) this.ckboxShowAllTransmitParameter);
      this.splitContainer1.Panel1.Controls.Add((Control) this.label2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.treeLogger);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label3);
      this.splitContainer1.Size = new Size(821, 177);
      this.splitContainer1.SplitterDistance = 409;
      this.splitContainer1.SplitterWidth = 6;
      this.splitContainer1.TabIndex = 22;
      this.treeParameter.AllowDrop = true;
      this.treeParameter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.treeParameter.ContextMenuStrip = this.contextMenuTransmitLists;
      this.treeParameter.Location = new Point(0, 35);
      this.treeParameter.Margin = new Padding(4, 5, 4, 5);
      this.treeParameter.Name = "treeParameter";
      this.treeParameter.Size = new Size(403, 141);
      this.treeParameter.TabIndex = 23;
      this.treeParameter.ItemDrag += new ItemDragEventHandler(this.treeParameter_ItemDrag);
      this.treeParameter.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
      this.ckboxShowAllTransmitParameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.ckboxShowAllTransmitParameter.AutoSize = true;
      this.ckboxShowAllTransmitParameter.Location = new Point(309, 8);
      this.ckboxShowAllTransmitParameter.Margin = new Padding(4, 5, 4, 5);
      this.ckboxShowAllTransmitParameter.Name = "ckboxShowAllTransmitParameter";
      this.ckboxShowAllTransmitParameter.Size = new Size(94, 24);
      this.ckboxShowAllTransmitParameter.TabIndex = 22;
      this.ckboxShowAllTransmitParameter.Text = "Show all";
      this.ckboxShowAllTransmitParameter.UseVisualStyleBackColor = true;
      this.ckboxShowAllTransmitParameter.CheckedChanged += new System.EventHandler(this.ckboxShowAllTransmitParameter_CheckedChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 6);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(157, 20);
      this.label2.TabIndex = 21;
      this.label2.Text = "Available parameters";
      this.treeLogger.AllowDrop = true;
      this.treeLogger.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.treeLogger.ContextMenuStrip = this.contextMenuTransmitLists;
      this.treeLogger.Location = new Point(4, 35);
      this.treeLogger.Margin = new Padding(4, 5, 4, 5);
      this.treeLogger.Name = "treeLogger";
      this.treeLogger.Size = new Size(393, 141);
      this.treeLogger.TabIndex = 22;
      this.treeLogger.ItemDrag += new ItemDragEventHandler(this.treeLogger_ItemDrag);
      this.treeLogger.AfterSelect += new TreeViewEventHandler(this.treeLogger_AfterSelect);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 9);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(126, 20);
      this.label3.TabIndex = 21;
      this.label3.Text = "Available Logger";
      this.gboxSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxSettings.Controls.Add((Control) this.lblStartStorageNumber);
      this.gboxSettings.Controls.Add((Control) this.txtRadioIdOffset);
      this.gboxSettings.Controls.Add((Control) this.lblRadioIdOffset);
      this.gboxSettings.Controls.Add((Control) this.lblRadioDevice);
      this.gboxSettings.Controls.Add((Control) this.cboxRadioDevice);
      this.gboxSettings.Controls.Add((Control) this.lblRadio);
      this.gboxSettings.Controls.Add((Control) this.cboxRadioMode);
      this.gboxSettings.Controls.Add((Control) this.txtStartStorageNumber);
      this.gboxSettings.Controls.Add((Control) this.txtCountOfValues);
      this.gboxSettings.Controls.Add((Control) this.lblCountOfValues);
      this.gboxSettings.Controls.Add((Control) this.ckboxLoggerDueDate);
      this.gboxSettings.Controls.Add((Control) this.lblLoggerType);
      this.gboxSettings.Controls.Add((Control) this.cboxLoggerType);
      this.gboxSettings.Location = new Point(3, 251);
      this.gboxSettings.Margin = new Padding(4, 5, 4, 5);
      this.gboxSettings.Name = "gboxSettings";
      this.gboxSettings.Padding = new Padding(4, 5, 4, 5);
      this.gboxSettings.Size = new Size(818, 114);
      this.gboxSettings.TabIndex = 23;
      this.gboxSettings.TabStop = false;
      this.gboxSettings.Text = "Node setup";
      this.lblStartStorageNumber.AutoSize = true;
      this.lblStartStorageNumber.Location = new Point(516, 22);
      this.lblStartStorageNumber.Margin = new Padding(4, 0, 4, 0);
      this.lblStartStorageNumber.Name = "lblStartStorageNumber";
      this.lblStartStorageNumber.Size = new Size(169, 20);
      this.lblStartStorageNumber.TabIndex = 5;
      this.lblStartStorageNumber.Text = "Start Storage Number:";
      this.lblStartStorageNumber.Visible = false;
      this.txtRadioIdOffset.Location = new Point(387, 58);
      this.txtRadioIdOffset.Margin = new Padding(4, 5, 4, 5);
      this.txtRadioIdOffset.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioIdOffset.Name = "txtRadioIdOffset";
      this.txtRadioIdOffset.Size = new Size(102, 26);
      this.txtRadioIdOffset.TabIndex = 12;
      this.txtRadioIdOffset.Visible = false;
      this.txtRadioIdOffset.ValueChanged += new System.EventHandler(this.ChangeParameter);
      this.lblRadioIdOffset.AutoSize = true;
      this.lblRadioIdOffset.Location = new Point(264, 62);
      this.lblRadioIdOffset.Margin = new Padding(4, 0, 4, 0);
      this.lblRadioIdOffset.Name = "lblRadioIdOffset";
      this.lblRadioIdOffset.Size = new Size(117, 20);
      this.lblRadioIdOffset.TabIndex = 11;
      this.lblRadioIdOffset.Text = "RadioID offset:";
      this.lblRadioIdOffset.Visible = false;
      this.lblRadioDevice.AutoSize = true;
      this.lblRadioDevice.Location = new Point(506, 62);
      this.lblRadioDevice.Margin = new Padding(4, 0, 4, 0);
      this.lblRadioDevice.Name = "lblRadioDevice";
      this.lblRadioDevice.Size = new Size(61, 20);
      this.lblRadioDevice.TabIndex = 10;
      this.lblRadioDevice.Text = "Device:";
      this.lblRadioDevice.Visible = false;
      this.cboxRadioDevice.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboxRadioDevice.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioDevice.FormattingEnabled = true;
      this.cboxRadioDevice.Location = new Point(574, 58);
      this.cboxRadioDevice.Margin = new Padding(4, 5, 4, 5);
      this.cboxRadioDevice.Name = "cboxRadioDevice";
      this.cboxRadioDevice.Size = new Size(238, 28);
      this.cboxRadioDevice.TabIndex = 9;
      this.cboxRadioDevice.Visible = false;
      this.cboxRadioDevice.SelectedIndexChanged += new System.EventHandler(this.ChangeParameter);
      this.lblRadio.AutoSize = true;
      this.lblRadio.Location = new Point(16, 65);
      this.lblRadio.Margin = new Padding(4, 0, 4, 0);
      this.lblRadio.Name = "lblRadio";
      this.lblRadio.Size = new Size(55, 20);
      this.lblRadio.TabIndex = 8;
      this.lblRadio.Text = "Radio:";
      this.lblRadio.Visible = false;
      this.cboxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioMode.FormattingEnabled = true;
      this.cboxRadioMode.Location = new Point(75, 58);
      this.cboxRadioMode.Margin = new Padding(4, 5, 4, 5);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(180, 28);
      this.cboxRadioMode.TabIndex = 7;
      this.cboxRadioMode.Visible = false;
      this.cboxRadioMode.SelectedIndexChanged += new System.EventHandler(this.ChangeParameter);
      this.txtStartStorageNumber.Location = new Point(690, 18);
      this.txtStartStorageNumber.Margin = new Padding(4, 5, 4, 5);
      this.txtStartStorageNumber.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtStartStorageNumber.Name = "txtStartStorageNumber";
      this.txtStartStorageNumber.Size = new Size(93, 26);
      this.txtStartStorageNumber.TabIndex = 6;
      this.txtStartStorageNumber.Visible = false;
      this.txtStartStorageNumber.ValueChanged += new System.EventHandler(this.ChangeParameter);
      this.txtCountOfValues.Location = new Point(324, 20);
      this.txtCountOfValues.Margin = new Padding(4, 5, 4, 5);
      this.txtCountOfValues.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.txtCountOfValues.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.txtCountOfValues.Name = "txtCountOfValues";
      this.txtCountOfValues.Size = new Size(63, 26);
      this.txtCountOfValues.TabIndex = 4;
      this.txtCountOfValues.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.txtCountOfValues.Visible = false;
      this.txtCountOfValues.ValueChanged += new System.EventHandler(this.ChangeParameter);
      this.lblCountOfValues.AutoSize = true;
      this.lblCountOfValues.Location = new Point(266, 22);
      this.lblCountOfValues.Margin = new Padding(4, 0, 4, 0);
      this.lblCountOfValues.Name = "lblCountOfValues";
      this.lblCountOfValues.Size = new Size(56, 20);
      this.lblCountOfValues.TabIndex = 3;
      this.lblCountOfValues.Text = "Count:";
      this.lblCountOfValues.Visible = false;
      this.ckboxLoggerDueDate.AutoSize = true;
      this.ckboxLoggerDueDate.Location = new Point(402, 22);
      this.ckboxLoggerDueDate.Margin = new Padding(4, 5, 4, 5);
      this.ckboxLoggerDueDate.Name = "ckboxLoggerDueDate";
      this.ckboxLoggerDueDate.Size = new Size(101, 24);
      this.ckboxLoggerDueDate.TabIndex = 2;
      this.ckboxLoggerDueDate.Text = "Due date";
      this.ckboxLoggerDueDate.UseVisualStyleBackColor = true;
      this.ckboxLoggerDueDate.Visible = false;
      this.ckboxLoggerDueDate.CheckedChanged += new System.EventHandler(this.ChangeParameter);
      this.lblLoggerType.AutoSize = true;
      this.lblLoggerType.Location = new Point(16, 25);
      this.lblLoggerType.Margin = new Padding(4, 0, 4, 0);
      this.lblLoggerType.Name = "lblLoggerType";
      this.lblLoggerType.Size = new Size(47, 20);
      this.lblLoggerType.TabIndex = 1;
      this.lblLoggerType.Text = "Type:";
      this.lblLoggerType.Visible = false;
      this.cboxLoggerType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxLoggerType.FormattingEnabled = true;
      this.cboxLoggerType.Location = new Point(75, 18);
      this.cboxLoggerType.Margin = new Padding(4, 5, 4, 5);
      this.cboxLoggerType.Name = "cboxLoggerType";
      this.cboxLoggerType.Size = new Size(180, 28);
      this.cboxLoggerType.TabIndex = 0;
      this.cboxLoggerType.Visible = false;
      this.cboxLoggerType.SelectedIndexChanged += new System.EventHandler(this.ChangeParameter);
      this.labelRadioCycle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labelRadioCycle.AutoSize = true;
      this.labelRadioCycle.Location = new Point(622, 31);
      this.labelRadioCycle.Margin = new Padding(4, 0, 4, 0);
      this.labelRadioCycle.Name = "labelRadioCycle";
      this.labelRadioCycle.Size = new Size(110, 20);
      this.labelRadioCycle.TabIndex = 14;
      this.labelRadioCycle.Text = "Radio cycle[s]:";
      this.labelWMBusEncryption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labelWMBusEncryption.AutoSize = true;
      this.labelWMBusEncryption.Location = new Point(322, 31);
      this.labelWMBusEncryption.Margin = new Padding(4, 0, 4, 0);
      this.labelWMBusEncryption.Name = "labelWMBusEncryption";
      this.labelWMBusEncryption.Size = new Size(142, 20);
      this.labelWMBusEncryption.TabIndex = 14;
      this.labelWMBusEncryption.Text = "wMBus encryption:";
      this.labelRadioMode.AutoSize = true;
      this.labelRadioMode.Location = new Point(5, 33);
      this.labelRadioMode.Margin = new Padding(4, 0, 4, 0);
      this.labelRadioMode.Name = "labelRadioMode";
      this.labelRadioMode.Size = new Size(99, 20);
      this.labelRadioMode.TabIndex = 13;
      this.labelRadioMode.Text = "Radio mode:";
      this.comboBox_wMBusEncryption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.comboBox_wMBusEncryption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_wMBusEncryption.FormattingEnabled = true;
      this.comboBox_wMBusEncryption.Location = new Point(471, 28);
      this.comboBox_wMBusEncryption.Margin = new Padding(4, 5, 4, 5);
      this.comboBox_wMBusEncryption.Name = "comboBox_wMBusEncryption";
      this.comboBox_wMBusEncryption.Size = new Size(128, 28);
      this.comboBox_wMBusEncryption.TabIndex = 7;
      this.comboBox_wMBusEncryption.SelectedIndexChanged += new System.EventHandler(this.comboBox_wMBusEncryption_SelectedIndexChanged);
      this.comboBoxRadioMode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.comboBoxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxRadioMode.FormattingEnabled = true;
      this.comboBoxRadioMode.Location = new Point(112, 28);
      this.comboBoxRadioMode.Margin = new Padding(4, 5, 4, 5);
      this.comboBoxRadioMode.Name = "comboBoxRadioMode";
      this.comboBoxRadioMode.Size = new Size(202, 28);
      this.comboBoxRadioMode.TabIndex = 7;
      this.comboBoxRadioMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxRadioMode_SelectedIndexChanged);
      this.txtParameterInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtParameterInfo.BorderStyle = BorderStyle.FixedSingle;
      this.txtParameterInfo.Font = new Font("Lucida Console", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtParameterInfo.Location = new Point(3, 2);
      this.txtParameterInfo.Margin = new Padding(4, 5, 4, 5);
      this.txtParameterInfo.Multiline = true;
      this.txtParameterInfo.Name = "txtParameterInfo";
      this.txtParameterInfo.ScrollBars = ScrollBars.Vertical;
      this.txtParameterInfo.Size = new Size(816, 244);
      this.txtParameterInfo.TabIndex = 23;
      this.splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer.Location = new Point(4, 54);
      this.splitContainer.Margin = new Padding(11, 12, 11, 12);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Panel1.Controls.Add((Control) this.tree);
      this.splitContainer.Panel2.Controls.Add((Control) this.groupBoxDeviceSetup);
      this.splitContainer.Panel2.Controls.Add((Control) this.splitContainer1);
      this.splitContainer.Panel2.Controls.Add((Control) this.txtParameterInfo);
      this.splitContainer.Panel2.Controls.Add((Control) this.gboxSettings);
      this.splitContainer.Size = new Size(1367, 647);
      this.splitContainer.SplitterDistance = 536;
      this.splitContainer.SplitterWidth = 6;
      this.splitContainer.TabIndex = 24;
      this.groupBoxDeviceSetup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxDeviceSetup.Controls.Add((Control) this.comboBoxRadioCycle);
      this.groupBoxDeviceSetup.Controls.Add((Control) this.comboBox_wMBusEncryption);
      this.groupBoxDeviceSetup.Controls.Add((Control) this.labelRadioCycle);
      this.groupBoxDeviceSetup.Controls.Add((Control) this.comboBoxRadioMode);
      this.groupBoxDeviceSetup.Controls.Add((Control) this.labelWMBusEncryption);
      this.groupBoxDeviceSetup.Controls.Add((Control) this.labelRadioMode);
      this.groupBoxDeviceSetup.Location = new Point(3, 372);
      this.groupBoxDeviceSetup.Margin = new Padding(3, 2, 3, 2);
      this.groupBoxDeviceSetup.Name = "groupBoxDeviceSetup";
      this.groupBoxDeviceSetup.Padding = new Padding(3, 2, 3, 2);
      this.groupBoxDeviceSetup.Size = new Size(822, 71);
      this.groupBoxDeviceSetup.TabIndex = 24;
      this.groupBoxDeviceSetup.TabStop = false;
      this.groupBoxDeviceSetup.Text = "Device setup";
      this.comboBoxRadioCycle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.comboBoxRadioCycle.FormattingEnabled = true;
      this.comboBoxRadioCycle.Location = new Point(739, 28);
      this.comboBoxRadioCycle.Margin = new Padding(3, 2, 3, 2);
      this.comboBoxRadioCycle.Name = "comboBoxRadioCycle";
      this.comboBoxRadioCycle.Size = new Size(77, 28);
      this.comboBoxRadioCycle.TabIndex = 15;
      this.btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(4, 706);
      this.btnPrint.Margin = new Padding(4, 5, 4, 5);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(99, 45);
      this.btnPrint.TabIndex = 25;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Image = (Image) componentResourceManager.GetObject("btnOk.Image");
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.ImeMode = ImeMode.NoControl;
      this.btnOk.Location = new Point(1109, 706);
      this.btnOk.Margin = new Padding(4, 5, 4, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(129, 38);
      this.btnOk.TabIndex = 26;
      this.btnOk.Text = "Save";
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(1242, 706);
      this.btnCancel.Margin = new Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(129, 38);
      this.btnCancel.TabIndex = 25;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1378, 744);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.splitContainer);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4, 5, 4, 5);
      this.MinimumSize = new Size(1400, 800);
      this.Name = nameof (TransmitParameterView);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Transmit parameter lists";
      this.Load += new System.EventHandler(this.TransmitParameterView_Load);
      this.contextMenuTransmitLists.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.gboxSettings.ResumeLayout(false);
      this.gboxSettings.PerformLayout();
      this.txtRadioIdOffset.EndInit();
      this.txtStartStorageNumber.EndInit();
      this.txtCountOfValues.EndInit();
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.Panel2.PerformLayout();
      this.splitContainer.EndInit();
      this.splitContainer.ResumeLayout(false);
      this.groupBoxDeviceSetup.ResumeLayout(false);
      this.groupBoxDeviceSetup.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum LoggerType
    {
      Value,
      Date,
      DateTime,
    }
  }
}
