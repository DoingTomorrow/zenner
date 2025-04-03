// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerView
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class LoggerView : Form
  {
    private bool Initialising = false;
    private static string[] AllChanalFunctions = new string[7]
    {
      LoggerChanalFunctions.SingleWriteFlashChanal.ToString(),
      LoggerChanalFunctions.RamChanal.ToString(),
      LoggerChanalFunctions.ReducedRamChanal.ToString(),
      LoggerChanalFunctions.MidnightEvent.ToString(),
      LoggerChanalFunctions.MaximalValueGenerator.ToString(),
      LoggerChanalFunctions.MaximalValueLogger.ToString(),
      LoggerChanalFunctions.RuntimeFunction.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_SingleWriteFlashChanal = new string[2]
    {
      LoggerChanalFunctions.SingleWriteFlashChanal.ToString(),
      LoggerChanalFunctions.MaximalValueLogger.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_RamChanal = new string[2]
    {
      LoggerChanalFunctions.RamChanal.ToString(),
      LoggerChanalFunctions.MaximalValueGenerator.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_ReducedRamChanal = new string[1]
    {
      LoggerChanalFunctions.ReducedRamChanal.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_MidnightEvent = new string[1]
    {
      LoggerChanalFunctions.MidnightEvent.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_MaximalValueGenerator = new string[1]
    {
      LoggerChanalFunctions.MaximalValueGenerator.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_MaximalValueLogger = new string[1]
    {
      LoggerChanalFunctions.MaximalValueLogger.ToString()
    };
    private static string[] ChangeChanalFunctionsFrom_RuntimeFunction = new string[1]
    {
      LoggerChanalFunctions.RuntimeFunction.ToString()
    };
    private static string[][] ChangeChanalFunctionsFromLists = new string[7][]
    {
      LoggerView.ChangeChanalFunctionsFrom_SingleWriteFlashChanal,
      LoggerView.ChangeChanalFunctionsFrom_RamChanal,
      LoggerView.ChangeChanalFunctionsFrom_ReducedRamChanal,
      LoggerView.ChangeChanalFunctionsFrom_MidnightEvent,
      LoggerView.ChangeChanalFunctionsFrom_MaximalValueGenerator,
      LoggerView.ChangeChanalFunctionsFrom_MaximalValueLogger,
      LoggerView.ChangeChanalFunctionsFrom_RuntimeFunction
    };
    private bool loggerStructureChanged = false;
    private S3_HandlerFunctions MyFunctions;
    private LoggerChanal activeLoggerChanal;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private Label label1;
    private GroupBox groupBoxIntervall;
    private DateTimePicker dateTimePickerStartTime;
    private RadioButton radioButtonTime;
    private RadioButton radioButtonMonth;
    private RadioButton radioButtonYear;
    private DateTimePicker dateTimePickerIntervalTime;
    private Label labelIntervalTime;
    private Button buttonSaveChangedLoggerData;
    private SplitContainer splitContainer4;
    private TextBox textBoxValueDescription;
    private Label label5;
    private TextBox textBoxNumberOfIntervalls;
    private TextBox textBoxChanalLastTime;
    private TextBox textBoxChanalBytes;
    private Label label9;
    private Label labelRamFlashBytes;
    private Label label6;
    private Label label7;
    private Label label10;
    private TreeView treeViewLoggerStruct;
    private DataGridView dataGridViewChanalData;
    private RadioButton radioButtonLoggerStorageFlash;
    private RadioButton radioButtonLoggerStorageRam;
    private TextBox textBoxMeterTime;
    private Label label11;
    private CheckBox checkBoxPhysicalView;
    private TextBox textBoxIntervalDays;
    private Label labelIntervalDays;
    private Button buttonResetAndClearAllLoggers;
    private Button buttonFillTestValues;
    private TreeView treeViewParameters;
    private ContextMenuStrip contextMenuStripLogger;
    private ToolStripMenuItem deleteToolStripMenuItem;
    private TabControl tabControl1;
    private TabPage ChanalData;
    private TabPage ChanalSetup;
    private Label label2;
    private ComboBox comboBoxChanalFunction;
    private Label label3;
    private Button buttonOkAndClose;
    private Label label4;
    private TextBox textBoxReducedNumberOfIntervalls;
    private TextBox textBoxUnusedRamBytes;
    private TextBox textBoxUnusedFlashBytes;
    private Label label13;
    private Label label12;
    private GroupBox groupBoxLoggerStorage;
    private ToolStripMenuItem resetChanalToolStripMenuItem;
    private CheckBox Logger_HalfMonth_ckbx;
    private TextBox textBoxChanalValueName;

    private bool LoggerStructureChanged
    {
      set
      {
        this.loggerStructureChanged = value;
        if (!value)
          return;
        this.buttonOkAndClose.Enabled = true;
        this.buttonFillTestValues.Enabled = false;
        this.buttonResetAndClearAllLoggers.Enabled = false;
        this.buttonSaveChangedLoggerData.Enabled = false;
      }
      get => this.loggerStructureChanged;
    }

    internal LoggerView(S3_HandlerFunctions MyFunctions)
    {
      this.Initialising = true;
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.LoadLoggerStruct();
      this.ShowParameters();
      this.comboBoxChanalFunction.DataSource = (object) LoggerView.AllChanalFunctions;
      this.comboBoxChanalFunction.SelectedIndex = 0;
      this.Initialising = false;
    }

    internal void ShowParameters()
    {
      this.treeViewParameters.Nodes.Clear();
      string[] names = Enum.GetNames(typeof (LoggerView.SpecialParameterNodes));
      for (int index = 0; index < names.Length; ++index)
      {
        S3_Parameter s3Parameter = new S3_Parameter(names[index], S3_VariableTypes.Address, S3_MemorySegment.Function);
        this.treeViewParameters.Nodes.Add(new TreeNode(names[index])
        {
          Tag = (object) s3Parameter
        });
      }
      SortedList<string, TreeNode> sortedList = new SortedList<string, TreeNode>();
      foreach (KeyValuePair<string, S3_Parameter> keyValuePair in this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName)
      {
        if (keyValuePair.Value.Statics != null && keyValuePair.Value.Statics.DefaultDifVif != null && keyValuePair.Value.Statics.DefaultDifVif.Length != 0 && this.MyFunctions.MyMeters.WorkMeter.MyResources.IsResourceAvailable(keyValuePair.Value.Statics.NeedResource, 0))
        {
          string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(keyValuePair.Value.Name);
          sortedList.Add(parameterNameByName, new TreeNode(parameterNameByName)
          {
            Tag = (object) keyValuePair.Value
          });
        }
      }
      foreach (TreeNode node in (IEnumerable<TreeNode>) sortedList.Values)
        this.treeViewParameters.Nodes.Add(node);
    }

    private void treeViewParameters_AfterSelect(object sender, TreeViewEventArgs e)
    {
      try
      {
        this.treeViewLoggerStruct.SelectedNode = (TreeNode) null;
        this.ClearData();
        if (this.treeViewParameters.SelectedNode.Text == LoggerView.SpecialParameterNodes.__NewCycleRamLogger.ToString())
          this.textBoxValueDescription.Text = "Use this by drag and drop to add a new cycle ram logger.";
        else if (this.treeViewParameters.SelectedNode.Text == LoggerView.SpecialParameterNodes.__NewSingleTimeFlashLogger.ToString())
          this.textBoxValueDescription.Text = "Use this by drag and drop to add a new single time flash logger.";
        else
          this.textBoxValueDescription.Text = ((S3_Parameter) this.treeViewParameters.SelectedNode.Tag).GetParameterInfo();
      }
      catch
      {
      }
    }

    private bool LoadLoggerStruct()
    {
      object lastTagObject = (object) null;
      if (this.treeViewLoggerStruct.SelectedNode != null)
        lastTagObject = this.treeViewLoggerStruct.SelectedNode.Tag;
      this.treeViewLoggerStruct.Nodes.Clear();
      TreeNode node = new TreeNode("All loggers");
      this.treeViewLoggerStruct.Nodes.Add(node);
      S3_Meter activeMeter = this.MyFunctions.MyMeters.GetActiveMeter();
      this.textBoxMeterTime.Text = ZR_Calendar.Cal_GetDateTime(activeMeter.MeterTimeAsSeconds1980).ToString("dd.MM.yyyy HH:mm");
      List<S3_MemoryBlock> childMemoryBlocks = activeMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks;
      if (!LoggerView.GetLoggerTree(node.Nodes, childMemoryBlocks))
        return false;
      TreeNode lastNode = this.GetLastNode(this.treeViewLoggerStruct.Nodes, lastTagObject);
      if (lastNode != null)
        this.treeViewLoggerStruct.SelectedNode = lastNode;
      else if (this.treeViewLoggerStruct.Nodes.Count > 0)
        this.treeViewLoggerStruct.SelectedNode = this.treeViewLoggerStruct.Nodes[0];
      this.treeViewLoggerStruct.ExpandAll();
      return true;
    }

    internal static bool GetLoggerTree(
      TreeNodeCollection theNodeCollection,
      List<S3_MemoryBlock> loggerConfigBlocks)
    {
      if (loggerConfigBlocks == null || loggerConfigBlocks.Count == 0)
        return true;
      foreach (LoggerConfiguration loggerConfigBlock in loggerConfigBlocks)
      {
        TreeNode node = new TreeNode("Logger: " + loggerConfigBlock.IntervalString + "[" + loggerConfigBlock.maxEntries.ToString() + "]");
        node.Tag = (object) loggerConfigBlock;
        theNodeCollection.Add(node);
        foreach (LoggerChanal childMemoryBlock in loggerConfigBlock.childMemoryBlocks)
        {
          string str1 = string.Empty;
          if (childMemoryBlock.chanalParameter != null)
          {
            string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(childMemoryBlock.chanalParameter.Name);
            string str2 = Ot.Gtt(Tg.Common, parameterNameByName, parameterNameByName);
            str1 = !(childMemoryBlock.chanalData is LoggerMaxBlockChanalData) ? (!childMemoryBlock.isCleareSource ? str2 : "Res: " + str2) : "Max: " + str2;
          }
          else if (((uint) childMemoryBlock.chanalFlags & 256U) > 0U)
            str1 = "midnight event";
          else if (((uint) childMemoryBlock.chanalFlags & 512U) > 0U)
            str1 = "start runtime code";
          node.Nodes.Add(new TreeNode("Chanal: " + str1)
          {
            Tag = (object) childMemoryBlock
          });
        }
      }
      return true;
    }

    private TreeNode GetLastNode(TreeNodeCollection theCollection, object lastTagObject)
    {
      if (lastTagObject == null || theCollection == null)
        return (TreeNode) null;
      foreach (TreeNode the in theCollection)
      {
        if (the.Tag != null && the.Tag == lastTagObject)
          return the;
        TreeNode lastNode = this.GetLastNode(the.Nodes, lastTagObject);
        if (lastNode != null)
          return lastNode;
      }
      return (TreeNode) null;
    }

    private void treeViewLoggerStruct_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.ShowNodeData(e.Node);
    }

    private void ShowNodeData(TreeNode theNode)
    {
      this.Initialising = true;
      this.ClearData();
      LoggerConfiguration loggerConfiguration = (LoggerConfiguration) null;
      if (theNode.Tag is LoggerChanal)
      {
        this.activeLoggerChanal = (LoggerChanal) theNode.Tag;
        loggerConfiguration = this.activeLoggerChanal.myLoggerConfig;
        this.ShowActiveLoggerChanalData();
      }
      else if (theNode.Tag is LoggerConfiguration)
      {
        this.activeLoggerChanal = (LoggerChanal) null;
        loggerConfiguration = (LoggerConfiguration) theNode.Tag;
        this.ShowActiveLoggerChanalData();
      }
      if (loggerConfiguration != null)
      {
        this.dateTimePickerStartTime.Value = ZR_Calendar.Cal_GetDateTime(loggerConfiguration.startTimeSeconds);
        this.textBoxChanalLastTime.Text = loggerConfiguration.GetLastTime().ToString("dd.MM.yyyy HH:mm");
        this.textBoxNumberOfIntervalls.Text = loggerConfiguration.maxEntries.ToString();
        int num = 0;
        this.radioButtonLoggerStorageFlash.Checked = true;
        this.textBoxReducedNumberOfIntervalls.Clear();
        this.textBoxReducedNumberOfIntervalls.ReadOnly = true;
        if (this.activeLoggerChanal != null)
        {
          if (this.activeLoggerChanal.isRamChanal)
          {
            this.radioButtonLoggerStorageRam.Checked = true;
            if (this.activeLoggerChanal.isReducedRamChanal)
            {
              this.textBoxReducedNumberOfIntervalls.Text = this.activeLoggerChanal.chanalMaxEntries.ToString();
              this.textBoxReducedNumberOfIntervalls.ReadOnly = false;
            }
          }
          this.textBoxChanalBytes.Text = ((int) this.activeLoggerChanal.chanalMaxEntries * this.activeLoggerChanal.EntryBytes).ToString();
          if (this.activeLoggerChanal.chanalParameter != null)
          {
            int byteSize = this.activeLoggerChanal.chanalParameter.ByteSize;
            if (this.checkBoxPhysicalView.Checked)
              this.buttonSaveChangedLoggerData.Enabled = true;
            if (this.activeLoggerChanal.isCleareSource)
              this.textBoxChanalValueName.Text = this.activeLoggerChanal.chanalParameter.Name + " -> reset after logging";
            else
              this.textBoxChanalValueName.Text = this.activeLoggerChanal.chanalParameter.Name;
            this.textBoxValueDescription.Text = this.activeLoggerChanal.chanalParameter.GetParameterInfo();
          }
          LoggerChanalFunctions chanalFunction = this.activeLoggerChanal.GetChanalFunction();
          this.comboBoxChanalFunction.DataSource = (object) LoggerView.ChangeChanalFunctionsFromLists[(int) chanalFunction];
          this.comboBoxChanalFunction.SelectedIndex = 0;
          if (this.comboBoxChanalFunction.Items.Count > 1)
            this.comboBoxChanalFunction.Enabled = true;
          else
            this.comboBoxChanalFunction.Enabled = false;
          if (chanalFunction == LoggerChanalFunctions.ReducedRamChanal || chanalFunction == LoggerChanalFunctions.MaximalValueLogger)
            this.textBoxReducedNumberOfIntervalls.Enabled = true;
          else
            this.textBoxReducedNumberOfIntervalls.Enabled = false;
        }
        else
        {
          this.comboBoxChanalFunction.DataSource = (object) null;
          this.comboBoxChanalFunction.Enabled = false;
          this.textBoxReducedNumberOfIntervalls.Enabled = false;
          if (loggerConfiguration.isRamLogger)
            this.radioButtonLoggerStorageRam.Checked = true;
          if (loggerConfiguration.childMemoryBlocks != null)
          {
            foreach (LoggerChanal childMemoryBlock in loggerConfiguration.childMemoryBlocks)
            {
              if (childMemoryBlock.chanalParameter != null)
                num += childMemoryBlock.chanalParameter.ByteSize;
            }
          }
        }
        if (loggerConfiguration.intervallSeconds == 31557600U)
        {
          this.radioButtonYear.Checked = true;
          this.Logger_HalfMonth_ckbx.Enabled = false;
          this.Logger_HalfMonth_ckbx.Checked = false;
          this.textBoxIntervalDays.Enabled = false;
          this.textBoxIntervalDays.Clear();
          this.dateTimePickerIntervalTime.Enabled = false;
          this.dateTimePickerIntervalTime.Value = new DateTime(2000, 1, 1);
        }
        else if (loggerConfiguration.intervallSeconds == 2629800U)
        {
          this.radioButtonMonth.Checked = true;
          this.Logger_HalfMonth_ckbx.Enabled = true;
          this.Logger_HalfMonth_ckbx.Checked = false;
          this.textBoxIntervalDays.Enabled = false;
          this.textBoxIntervalDays.Clear();
          this.dateTimePickerIntervalTime.Enabled = false;
          this.dateTimePickerIntervalTime.Value = new DateTime(2000, 1, 1);
        }
        else if (loggerConfiguration.intervallSeconds == 1314900U)
        {
          this.radioButtonMonth.Checked = true;
          this.Logger_HalfMonth_ckbx.Enabled = true;
          this.Logger_HalfMonth_ckbx.Checked = true;
          this.textBoxIntervalDays.Enabled = false;
          this.textBoxIntervalDays.Clear();
          this.dateTimePickerIntervalTime.Enabled = false;
          this.dateTimePickerIntervalTime.Value = new DateTime(2000, 1, 1);
        }
        else
        {
          this.radioButtonTime.Checked = true;
          this.Logger_HalfMonth_ckbx.Enabled = false;
          this.Logger_HalfMonth_ckbx.Checked = false;
          this.textBoxIntervalDays.Enabled = true;
          this.dateTimePickerIntervalTime.Enabled = true;
          this.textBoxIntervalDays.Text = (loggerConfiguration.intervallSeconds / 86400U).ToString();
          this.dateTimePickerIntervalTime.Value = new DateTime(2000, 1, 1, 0, 0, 0).AddSeconds((double) loggerConfiguration.intervallSeconds);
        }
      }
      this.textBoxUnusedRamBytes.Text = this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.UnusedRAM.ToString();
      this.textBoxUnusedFlashBytes.Text = this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.UnusedFlash.ToString();
      this.Initialising = false;
    }

    private void ClearData()
    {
      this.activeLoggerChanal = (LoggerChanal) null;
      this.textBoxChanalValueName.Clear();
      this.textBoxValueDescription.Clear();
      this.dataGridViewChanalData.DataSource = (object) null;
    }

    private void ShowActiveLoggerChanalData()
    {
      if (this.activeLoggerChanal == null)
        return;
      if (this.LoggerStructureChanged)
        this.dataGridViewChanalData.DataSource = (object) null;
      else if (this.activeLoggerChanal.chanalData != null)
      {
        bool flag = this.checkBoxPhysicalView.Checked;
        this.dataGridViewChanalData.DataSource = (object) this.activeLoggerChanal.chanalData.GetChanalData(this.checkBoxPhysicalView.Checked);
        this.dataGridViewChanalData.Columns[0].DefaultCellStyle.Format = "x06";
        this.dataGridViewChanalData.Columns[0].ReadOnly = true;
        this.dataGridViewChanalData.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        this.dataGridViewChanalData.Columns[1].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
        this.dataGridViewChanalData.Columns[1].ReadOnly = true;
        this.dataGridViewChanalData.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        this.dataGridViewChanalData.Columns[2].DefaultCellStyle.Format = "X08";
        this.dataGridViewChanalData.Columns[2].ReadOnly = true;
        this.dataGridViewChanalData.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
        this.dataGridViewChanalData.Columns[3].ReadOnly = !this.checkBoxPhysicalView.Checked;
        this.dataGridViewChanalData.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
        if (this.activeLoggerChanal.hasTimeStamp)
        {
          this.dataGridViewChanalData.Columns[4].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
          this.dataGridViewChanalData.Columns[4].ReadOnly = !this.checkBoxPhysicalView.Checked;
          this.dataGridViewChanalData.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
      }
    }

    private void checkBoxPhysicalView_CheckedChanged(object sender, EventArgs e)
    {
      this.ShowActiveLoggerChanalData();
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (this.activeLoggerChanal == null)
        return;
      this.activeLoggerChanal.chanalData.ChangeChanalData();
    }

    private void buttonResetAndClearAllLoggers_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (!this.MyFunctions.MyMeters.WorkMeter.Compile())
      {
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        TreeNode treeNode = (TreeNode) null;
        if (this.treeViewLoggerStruct.SelectedNode != null)
          treeNode = this.treeViewLoggerStruct.SelectedNode;
        bool[] OverwriteSelection = new bool[21];
        OverwriteSelection[18] = true;
        bool flag = this.MyFunctions.OverwriteWorkFromType(OverwriteSelection);
        ZR_ClassLibMessages.ShowAndClearErrors();
        if (!flag)
          return;
        this.LoadLoggerStruct();
        if (treeNode != null)
        {
          TreeNode node = this.FindNode(this.treeViewLoggerStruct.Nodes, treeNode.Text);
          if (node != null)
          {
            this.treeViewLoggerStruct.SelectedNode = node;
            node.EnsureVisible();
          }
        }
        this.LoggerStructureChanged = true;
      }
    }

    private void buttonFillTestValues_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      TreeNode treeNode = (TreeNode) null;
      if (this.treeViewLoggerStruct.SelectedNode != null)
        treeNode = this.treeViewLoggerStruct.SelectedNode;
      this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.FillTestValues();
      this.MyFunctions.MyMeters.NewWorkMeter("Fill logger test values");
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadLoggerStruct();
      if (treeNode != null)
      {
        TreeNode node = this.FindNode(this.treeViewLoggerStruct.Nodes, treeNode.Text);
        if (node != null)
        {
          this.treeViewLoggerStruct.SelectedNode = node;
          node.EnsureVisible();
        }
      }
      this.LoggerStructureChanged = true;
    }

    private TreeNode FindNode(TreeNodeCollection nodeCollection, string nodeText)
    {
      foreach (TreeNode node1 in nodeCollection)
      {
        if (node1.Text == nodeText)
          return node1;
        if (node1.Nodes != null)
        {
          TreeNode node2 = this.FindNode(node1.Nodes, nodeText);
          if (node2 != null)
            return node2;
        }
      }
      return (TreeNode) null;
    }

    private void treeViewParameters_ItemDrag(object sender, ItemDragEventArgs e)
    {
      int num = (int) this.DoDragDrop(((TreeNode) e.Item).Tag, DragDropEffects.All);
    }

    private void treeViewLoggerStruct_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Move;
    }

    private void treeViewLoggerStruct_DragDrop(object sender, DragEventArgs e)
    {
      this.ClearData();
      TreeNode nodeAt = this.treeViewLoggerStruct.GetNodeAt(this.treeViewLoggerStruct.PointToClient(new Point(e.X, e.Y)));
      S3_Parameter data = (S3_Parameter) e.Data.GetData(typeof (S3_Parameter));
      LoggerConfiguration loggerConfiguration1 = (LoggerConfiguration) null;
      LoggerChanal loggerChanal = (LoggerChanal) null;
      TreeNode treeNode1 = (TreeNode) null;
      TreeNode node = (TreeNode) null;
      if (nodeAt != null && nodeAt.Tag != null)
      {
        if (nodeAt.Tag is LoggerConfiguration)
        {
          loggerConfiguration1 = (LoggerConfiguration) nodeAt.Tag;
          treeNode1 = nodeAt;
          node = nodeAt;
        }
        else if (nodeAt.Tag is LoggerChanal)
        {
          loggerChanal = (LoggerChanal) nodeAt.Tag;
          loggerConfiguration1 = loggerChanal.myLoggerConfig;
          node = nodeAt.Parent;
        }
      }
      string name1 = data.Name;
      LoggerView.SpecialParameterNodes specialParameterNodes = LoggerView.SpecialParameterNodes.__NewCycleRamLogger;
      string str1 = specialParameterNodes.ToString();
      int num1;
      if (!(name1 == str1))
      {
        string name2 = data.Name;
        specialParameterNodes = LoggerView.SpecialParameterNodes.__NewSingleTimeFlashLogger;
        string str2 = specialParameterNodes.ToString();
        num1 = name2 == str2 ? 1 : 0;
      }
      else
        num1 = 1;
      if (num1 != 0)
      {
        ZR_ClassLibMessages.ClearErrors();
        int index = 0;
        int count = this.treeViewLoggerStruct.Nodes[0].Nodes.Count;
        LoggerConfiguration loggerConfiguration2;
        if (nodeAt == null)
        {
          loggerConfiguration2 = this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.InsertNewLogger((LoggerConfiguration) null);
          index = count;
        }
        else if (loggerConfiguration1 == null)
          loggerConfiguration2 = this.treeViewLoggerStruct.Nodes[0].Nodes.Count != 0 ? this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.InsertNewLogger((LoggerConfiguration) this.treeViewLoggerStruct.Nodes[0].Nodes[0].Tag) : this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.InsertNewLogger((LoggerConfiguration) null);
        else if (node == this.treeViewLoggerStruct.Nodes[0].Nodes[count - 1])
        {
          loggerConfiguration2 = this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.InsertNewLogger((LoggerConfiguration) null);
          index = count;
        }
        else
        {
          int num2 = this.treeViewLoggerStruct.Nodes[0].Nodes.IndexOf(node);
          loggerConfiguration2 = this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.InsertNewLogger((LoggerConfiguration) this.treeViewLoggerStruct.Nodes[0].Nodes[num2 + 1].Tag);
          index = num2 + 1;
        }
        string name3 = data.Name;
        specialParameterNodes = LoggerView.SpecialParameterNodes.__NewCycleRamLogger;
        string str3 = specialParameterNodes.ToString();
        if (name3 == str3)
          loggerConfiguration2.isRamLogger = true;
        this.treeViewLoggerStruct.Nodes[0].Nodes.Insert(index, new TreeNode("Logger: " + loggerConfiguration2.IntervalString + "[" + loggerConfiguration2.maxEntries.ToString() + "]")
        {
          Tag = (object) loggerConfiguration2
        });
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        if (loggerConfiguration1 == null)
          return;
        this.LoggerStructureChanged = true;
        int num3 = 0;
        TreeNode treeNode2 = new TreeNode("Chanal: " + data.GetTranslatedParameterName());
        if (loggerChanal != null)
        {
          treeNode1 = nodeAt.Parent;
          num3 = treeNode1.Nodes.IndexOf(nodeAt) + 1;
        }
        ZR_ClassLibMessages.ClearErrors();
        LoggerChanal newChanal;
        bool flag = loggerConfiguration1.InsertLoggerChanal(num3, data, out newChanal);
        ZR_ClassLibMessages.ShowAndClearErrors();
        if (flag)
        {
          treeNode2.Tag = (object) newChanal;
          treeNode1.Nodes.Insert(num3, treeNode2);
          this.treeViewLoggerStruct.SelectedNode = treeNode2;
          this.ShowNodeData(treeNode2);
          this.tabControl1.SelectedIndex = 1;
        }
      }
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewLoggerStruct.SelectedNode;
      if (selectedNode == null || !(selectedNode.Tag is LoggerChanal))
        return;
      LoggerChanal tag = (LoggerChanal) selectedNode.Tag;
      if (tag.isMaxValues && tag.chanalParameter != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock1 in tag.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks)
        {
          foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if (childMemoryBlock2 != tag && childMemoryBlock2.chanalParameter != null && childMemoryBlock2.chanalParameter.Name.StartsWith(tag.chanalParameter.Name + "_"))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "Please remove the sub logger first! Logger name: " + this.GetDisplayLoggerName(this.treeViewLoggerStruct.Nodes, childMemoryBlock2));
              ZR_ClassLibMessages.ShowAndClearErrors();
              return;
            }
          }
        }
      }
      tag.myLoggerConfig.RemoveLoggerChanal(tag);
      selectedNode.Remove();
      this.LoggerStructureChanged = true;
    }

    private void resetChanalToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewLoggerStruct.SelectedNode;
      if (selectedNode == null || !(selectedNode.Tag is LoggerChanal))
        return;
      ((LoggerChanal) selectedNode.Tag).ResetChanalDataToOneStoredValue();
      this.LoggerStructureChanged = true;
    }

    private string GetDisplayLoggerName(
      TreeNodeCollection nodeCollection,
      LoggerChanal loggerChanal)
    {
      foreach (TreeNode node in nodeCollection)
      {
        if (node.Tag == loggerChanal)
          return node.Text;
        if (node.Nodes != null)
        {
          string displayLoggerName = this.GetDisplayLoggerName(node.Nodes, loggerChanal);
          if (!string.IsNullOrEmpty(displayLoggerName))
            return displayLoggerName;
        }
      }
      return string.Empty;
    }

    private void buttonOkAndClose_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyMeters.WorkMeter.Compile();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void comboBoxChanalFunction_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.Initialising || this.comboBoxChanalFunction.SelectedIndex < 1)
        return;
      this.LoggerStructureChanged = true;
      if (this.comboBoxChanalFunction.SelectedItem.ToString() == LoggerChanalFunctions.MaximalValueLogger.ToString())
      {
        LoggerChanal tag = (LoggerChanal) this.treeViewLoggerStruct.SelectedNode.Tag;
        this.MyFunctions.MyMeters.WorkMeter.MyLoggerManager.SetChanalToMaximalValueLogger(tag);
        this.treeViewLoggerStruct.SelectedNode.Text = "Chanal: Res: " + tag.chanalParameter.GetTranslatedParameterName();
        this.ShowNodeData(this.treeViewLoggerStruct.SelectedNode);
      }
      else
      {
        if (!(this.comboBoxChanalFunction.SelectedItem.ToString() == LoggerChanalFunctions.MaximalValueGenerator.ToString()))
          return;
        LoggerChanal tag = (LoggerChanal) this.treeViewLoggerStruct.SelectedNode.Tag;
        tag.myLoggerConfig.SetChanalToMaximalValueGenerator(tag);
        this.treeViewLoggerStruct.SelectedNode.Text = "Chanal: Max: " + tag.chanalParameter.GetTranslatedParameterName();
        this.ShowParameters();
        this.ShowNodeData(this.treeViewLoggerStruct.SelectedNode);
      }
    }

    private void textBoxNumberOfIntervalls_Leave(object sender, EventArgs e)
    {
      LoggerConfiguration selectedConfig = this.GetSelectedConfig();
      if (selectedConfig == null)
        return;
      ushort result;
      if (!ushort.TryParse(this.textBoxNumberOfIntervalls.Text, out result))
        this.textBoxNumberOfIntervalls.Text = selectedConfig.maxEntries.ToString();
      else if (result < (ushort) 2)
      {
        this.textBoxNumberOfIntervalls.Text = selectedConfig.maxEntries.ToString();
      }
      else
      {
        this.LoggerStructureChanged = true;
        ZR_ClassLibMessages.ClearErrors();
        selectedConfig.maxEntries = result;
        selectedConfig.ResizeLoggerCanalDataBlocks();
        ZR_ClassLibMessages.ShowAndClearErrors();
        this.LoadLoggerStruct();
      }
    }

    private void textBoxReducedNumberOfIntervalls_Leave(object sender, EventArgs e)
    {
      if (this.Initialising || this.treeViewLoggerStruct.SelectedNode == null || !(this.treeViewLoggerStruct.SelectedNode.Tag is LoggerChanal))
        return;
      LoggerChanal tag = (LoggerChanal) this.treeViewLoggerStruct.SelectedNode.Tag;
      ushort result;
      if (!ushort.TryParse(this.textBoxReducedNumberOfIntervalls.Text, out result))
        this.textBoxReducedNumberOfIntervalls.Text = tag.chanalMaxEntries.ToString();
      else if (result < (ushort) 2 || (int) result > (int) tag.myLoggerConfig.maxEntries)
      {
        this.textBoxNumberOfIntervalls.Text = tag.chanalMaxEntries.ToString();
      }
      else
      {
        this.LoggerStructureChanged = true;
        ZR_ClassLibMessages.ClearErrors();
        tag.chanalMaxEntries = result;
        tag.ResizeLoggerCanalDataBlock();
        ZR_ClassLibMessages.ShowAndClearErrors();
        this.LoadLoggerStruct();
      }
    }

    private void loggerIntervalChanged(object sender, EventArgs e)
    {
      if (this.Initialising)
        return;
      LoggerConfiguration selectedConfig = this.GetSelectedConfig();
      if (selectedConfig == null)
        return;
      this.Logger_HalfMonth_ckbx.Enabled = false;
      ZR_ClassLibMessages.ClearErrors();
      this.LoggerStructureChanged = true;
      if (this.radioButtonYear.Checked)
        selectedConfig.intervallSeconds = 31557600U;
      else if (this.radioButtonMonth.Checked)
      {
        this.Logger_HalfMonth_ckbx.Enabled = true;
        selectedConfig.intervallSeconds = !this.Logger_HalfMonth_ckbx.Checked ? 2629800U : 1314900U;
      }
      else
      {
        uint result;
        if (!uint.TryParse(this.textBoxIntervalDays.Text, out result))
        {
          this.textBoxIntervalDays.Text = "1";
          result = 1U;
        }
        selectedConfig.intervallSeconds = (uint) this.dateTimePickerIntervalTime.Value.TimeOfDay.TotalSeconds + (uint) ((int) result * 3600 * 24);
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadLoggerStruct();
    }

    private void dateTimePickerStartTime_Leave(object sender, EventArgs e)
    {
      if (this.Initialising)
        return;
      LoggerConfiguration selectedConfig = this.GetSelectedConfig();
      if (selectedConfig == null)
        return;
      this.LoggerStructureChanged = true;
      ZR_ClassLibMessages.ClearErrors();
      DateTime timeToSet = this.dateTimePickerStartTime.Value;
      selectedConfig.SetStartTime(timeToSet);
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadLoggerStruct();
    }

    private LoggerConfiguration GetSelectedConfig()
    {
      if (this.treeViewLoggerStruct.SelectedNode == null)
        return (LoggerConfiguration) null;
      LoggerConfiguration selectedConfig;
      if (this.treeViewLoggerStruct.SelectedNode.Tag is LoggerConfiguration)
      {
        selectedConfig = (LoggerConfiguration) this.treeViewLoggerStruct.SelectedNode.Tag;
      }
      else
      {
        if (this.treeViewLoggerStruct.SelectedNode.Tag == null)
          return (LoggerConfiguration) null;
        selectedConfig = ((LoggerChanal) this.treeViewLoggerStruct.SelectedNode.Tag).myLoggerConfig;
      }
      return selectedConfig;
    }

    private void treeViewLoggerStruct_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete)
        return;
      this.deleteToolStripMenuItem_Click(sender, (EventArgs) null);
    }

    private void Logger_HalfMonth_ckbx_CheckedChanged(object sender, EventArgs e)
    {
      LoggerConfiguration selectedConfig = this.GetSelectedConfig();
      if (selectedConfig == null)
        return;
      if (this.radioButtonMonth.Checked)
        selectedConfig.intervallSeconds = !this.Logger_HalfMonth_ckbx.Checked ? 2629800U : 1314900U;
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadLoggerStruct();
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
      this.splitContainer1 = new SplitContainer();
      this.splitContainer2 = new SplitContainer();
      this.treeViewParameters = new TreeView();
      this.label10 = new Label();
      this.treeViewLoggerStruct = new TreeView();
      this.contextMenuStripLogger = new ContextMenuStrip(this.components);
      this.deleteToolStripMenuItem = new ToolStripMenuItem();
      this.resetChanalToolStripMenuItem = new ToolStripMenuItem();
      this.label1 = new Label();
      this.splitContainer4 = new SplitContainer();
      this.label5 = new Label();
      this.textBoxValueDescription = new TextBox();
      this.textBoxUnusedRamBytes = new TextBox();
      this.dateTimePickerStartTime = new DateTimePicker();
      this.textBoxUnusedFlashBytes = new TextBox();
      this.buttonFillTestValues = new Button();
      this.label13 = new Label();
      this.buttonResetAndClearAllLoggers = new Button();
      this.textBoxNumberOfIntervalls = new TextBox();
      this.label12 = new Label();
      this.label2 = new Label();
      this.buttonOkAndClose = new Button();
      this.buttonSaveChangedLoggerData = new Button();
      this.textBoxMeterTime = new TextBox();
      this.label11 = new Label();
      this.label6 = new Label();
      this.groupBoxLoggerStorage = new GroupBox();
      this.radioButtonLoggerStorageFlash = new RadioButton();
      this.radioButtonLoggerStorageRam = new RadioButton();
      this.groupBoxIntervall = new GroupBox();
      this.Logger_HalfMonth_ckbx = new CheckBox();
      this.textBoxIntervalDays = new TextBox();
      this.dateTimePickerIntervalTime = new DateTimePicker();
      this.labelIntervalDays = new Label();
      this.labelIntervalTime = new Label();
      this.radioButtonTime = new RadioButton();
      this.radioButtonMonth = new RadioButton();
      this.radioButtonYear = new RadioButton();
      this.tabControl1 = new TabControl();
      this.ChanalData = new TabPage();
      this.checkBoxPhysicalView = new CheckBox();
      this.dataGridViewChanalData = new DataGridView();
      this.ChanalSetup = new TabPage();
      this.comboBoxChanalFunction = new ComboBox();
      this.textBoxChanalLastTime = new TextBox();
      this.textBoxReducedNumberOfIntervalls = new TextBox();
      this.textBoxChanalBytes = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label7 = new Label();
      this.label9 = new Label();
      this.labelRamFlashBytes = new Label();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.textBoxChanalValueName = new TextBox();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.contextMenuStripLogger.SuspendLayout();
      this.splitContainer4.BeginInit();
      this.splitContainer4.Panel1.SuspendLayout();
      this.splitContainer4.Panel2.SuspendLayout();
      this.splitContainer4.SuspendLayout();
      this.groupBoxLoggerStorage.SuspendLayout();
      this.groupBoxIntervall.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.ChanalData.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewChanalData).BeginInit();
      this.ChanalSetup.SuspendLayout();
      this.SuspendLayout();
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(0, 63);
      this.splitContainer1.Margin = new Padding(4, 5, 4, 5);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer4);
      this.splitContainer1.Size = new Size(1428, 982);
      this.splitContainer1.SplitterDistance = 464;
      this.splitContainer1.SplitterWidth = 6;
      this.splitContainer1.TabIndex = 20;
      this.splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Margin = new Padding(2);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.treeViewParameters);
      this.splitContainer2.Panel1.Controls.Add((Control) this.label10);
      this.splitContainer2.Panel2.Controls.Add((Control) this.treeViewLoggerStruct);
      this.splitContainer2.Panel2.Controls.Add((Control) this.label1);
      this.splitContainer2.Size = new Size(436, 982);
      this.splitContainer2.SplitterDistance = 343;
      this.splitContainer2.SplitterWidth = 6;
      this.splitContainer2.TabIndex = 0;
      this.treeViewParameters.AllowDrop = true;
      this.treeViewParameters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.treeViewParameters.FullRowSelect = true;
      this.treeViewParameters.Location = new Point(9, 29);
      this.treeViewParameters.Margin = new Padding(4, 5, 4, 5);
      this.treeViewParameters.Name = "treeViewParameters";
      this.treeViewParameters.ShowLines = false;
      this.treeViewParameters.ShowPlusMinus = false;
      this.treeViewParameters.ShowRootLines = false;
      this.treeViewParameters.Size = new Size(416, 307);
      this.treeViewParameters.TabIndex = 2;
      this.treeViewParameters.ItemDrag += new ItemDragEventHandler(this.treeViewParameters_ItemDrag);
      this.treeViewParameters.AfterSelect += new TreeViewEventHandler(this.treeViewParameters_AfterSelect);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(4, 3);
      this.label10.Margin = new Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(273, 20);
      this.label10.TabIndex = 1;
      this.label10.Text = "Logger items to add by drag and drop";
      this.treeViewLoggerStruct.AllowDrop = true;
      this.treeViewLoggerStruct.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.treeViewLoggerStruct.ContextMenuStrip = this.contextMenuStripLogger;
      this.treeViewLoggerStruct.FullRowSelect = true;
      this.treeViewLoggerStruct.HideSelection = false;
      this.treeViewLoggerStruct.Location = new Point(9, 40);
      this.treeViewLoggerStruct.Margin = new Padding(4, 5, 4, 5);
      this.treeViewLoggerStruct.Name = "treeViewLoggerStruct";
      this.treeViewLoggerStruct.Size = new Size(416, 527);
      this.treeViewLoggerStruct.TabIndex = 3;
      this.treeViewLoggerStruct.AfterSelect += new TreeViewEventHandler(this.treeViewLoggerStruct_AfterSelect);
      this.treeViewLoggerStruct.DragDrop += new DragEventHandler(this.treeViewLoggerStruct_DragDrop);
      this.treeViewLoggerStruct.DragEnter += new DragEventHandler(this.treeViewLoggerStruct_DragEnter);
      this.treeViewLoggerStruct.KeyUp += new KeyEventHandler(this.treeViewLoggerStruct_KeyUp);
      this.contextMenuStripLogger.ImageScalingSize = new Size(24, 24);
      this.contextMenuStripLogger.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.deleteToolStripMenuItem,
        (ToolStripItem) this.resetChanalToolStripMenuItem
      });
      this.contextMenuStripLogger.Name = "contextMenuStripLogger";
      this.contextMenuStripLogger.Size = new Size(182, 68);
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = new Size(181, 32);
      this.deleteToolStripMenuItem.Text = "Delete";
      this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
      this.resetChanalToolStripMenuItem.Name = "resetChanalToolStripMenuItem";
      this.resetChanalToolStripMenuItem.Size = new Size(181, 32);
      this.resetChanalToolStripMenuItem.Text = "Reset chanal";
      this.resetChanalToolStripMenuItem.Click += new System.EventHandler(this.resetChanalToolStripMenuItem_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 9);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(59, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "Logger";
      this.splitContainer4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer4.Location = new Point(4, 3);
      this.splitContainer4.Margin = new Padding(4, 5, 4, 5);
      this.splitContainer4.Name = "splitContainer4";
      this.splitContainer4.Orientation = Orientation.Horizontal;
      this.splitContainer4.Panel1.Controls.Add((Control) this.label5);
      this.splitContainer4.Panel1.Controls.Add((Control) this.textBoxValueDescription);
      this.splitContainer4.Panel2.Controls.Add((Control) this.textBoxUnusedRamBytes);
      this.splitContainer4.Panel2.Controls.Add((Control) this.dateTimePickerStartTime);
      this.splitContainer4.Panel2.Controls.Add((Control) this.textBoxUnusedFlashBytes);
      this.splitContainer4.Panel2.Controls.Add((Control) this.buttonFillTestValues);
      this.splitContainer4.Panel2.Controls.Add((Control) this.label13);
      this.splitContainer4.Panel2.Controls.Add((Control) this.buttonResetAndClearAllLoggers);
      this.splitContainer4.Panel2.Controls.Add((Control) this.textBoxNumberOfIntervalls);
      this.splitContainer4.Panel2.Controls.Add((Control) this.label12);
      this.splitContainer4.Panel2.Controls.Add((Control) this.label2);
      this.splitContainer4.Panel2.Controls.Add((Control) this.buttonOkAndClose);
      this.splitContainer4.Panel2.Controls.Add((Control) this.buttonSaveChangedLoggerData);
      this.splitContainer4.Panel2.Controls.Add((Control) this.textBoxMeterTime);
      this.splitContainer4.Panel2.Controls.Add((Control) this.label11);
      this.splitContainer4.Panel2.Controls.Add((Control) this.label6);
      this.splitContainer4.Panel2.Controls.Add((Control) this.groupBoxLoggerStorage);
      this.splitContainer4.Panel2.Controls.Add((Control) this.groupBoxIntervall);
      this.splitContainer4.Panel2.Controls.Add((Control) this.tabControl1);
      this.splitContainer4.Size = new Size(940, 974);
      this.splitContainer4.SplitterDistance = 250;
      this.splitContainer4.SplitterWidth = 6;
      this.splitContainer4.TabIndex = 2;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 0);
      this.label5.Margin = new Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(131, 20);
      this.label5.TabIndex = 1;
      this.label5.Text = "Value description";
      this.textBoxValueDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxValueDescription.Location = new Point(8, 29);
      this.textBoxValueDescription.Margin = new Padding(4, 5, 4, 5);
      this.textBoxValueDescription.Multiline = true;
      this.textBoxValueDescription.Name = "textBoxValueDescription";
      this.textBoxValueDescription.ReadOnly = true;
      this.textBoxValueDescription.ScrollBars = ScrollBars.Vertical;
      this.textBoxValueDescription.Size = new Size(919, 194);
      this.textBoxValueDescription.TabIndex = 2;
      this.textBoxUnusedRamBytes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxUnusedRamBytes.Location = new Point(811, 416);
      this.textBoxUnusedRamBytes.Margin = new Padding(4, 5, 4, 5);
      this.textBoxUnusedRamBytes.Name = "textBoxUnusedRamBytes";
      this.textBoxUnusedRamBytes.ReadOnly = true;
      this.textBoxUnusedRamBytes.Size = new Size(98, 26);
      this.textBoxUnusedRamBytes.TabIndex = 5;
      this.textBoxUnusedRamBytes.Leave += new System.EventHandler(this.loggerIntervalChanged);
      this.dateTimePickerStartTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dateTimePickerStartTime.CustomFormat = "dd.MM.yyyy HH:mm";
      this.dateTimePickerStartTime.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerStartTime.Location = new Point(715, 242);
      this.dateTimePickerStartTime.Margin = new Padding(4, 5, 4, 5);
      this.dateTimePickerStartTime.Name = "dateTimePickerStartTime";
      this.dateTimePickerStartTime.Size = new Size(194, 26);
      this.dateTimePickerStartTime.TabIndex = 3;
      this.dateTimePickerStartTime.Leave += new System.EventHandler(this.dateTimePickerStartTime_Leave);
      this.textBoxUnusedFlashBytes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxUnusedFlashBytes.Location = new Point(811, 389);
      this.textBoxUnusedFlashBytes.Margin = new Padding(4, 5, 4, 5);
      this.textBoxUnusedFlashBytes.Name = "textBoxUnusedFlashBytes";
      this.textBoxUnusedFlashBytes.ReadOnly = true;
      this.textBoxUnusedFlashBytes.Size = new Size(98, 26);
      this.textBoxUnusedFlashBytes.TabIndex = 5;
      this.textBoxUnusedFlashBytes.Leave += new System.EventHandler(this.loggerIntervalChanged);
      this.buttonFillTestValues.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonFillTestValues.Location = new Point(616, 531);
      this.buttonFillTestValues.Margin = new Padding(4, 5, 4, 5);
      this.buttonFillTestValues.Name = "buttonFillTestValues";
      this.buttonFillTestValues.Size = new Size(309, 35);
      this.buttonFillTestValues.TabIndex = 4;
      this.buttonFillTestValues.Text = "Fill test values";
      this.buttonFillTestValues.UseVisualStyleBackColor = true;
      this.buttonFillTestValues.Click += new System.EventHandler(this.buttonFillTestValues_Click);
      this.label13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(624, 421);
      this.label13.Margin = new Padding(4, 0, 4, 0);
      this.label13.Name = "label13";
      this.label13.Size = new Size(147, 20);
      this.label13.TabIndex = 1;
      this.label13.Text = "Unused RAM bytes";
      this.buttonResetAndClearAllLoggers.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonResetAndClearAllLoggers.Location = new Point(616, 574);
      this.buttonResetAndClearAllLoggers.Margin = new Padding(4, 5, 4, 5);
      this.buttonResetAndClearAllLoggers.Name = "buttonResetAndClearAllLoggers";
      this.buttonResetAndClearAllLoggers.Size = new Size(309, 35);
      this.buttonResetAndClearAllLoggers.TabIndex = 4;
      this.buttonResetAndClearAllLoggers.Text = "Reset and clear all loggers";
      this.buttonResetAndClearAllLoggers.UseVisualStyleBackColor = true;
      this.buttonResetAndClearAllLoggers.Click += new System.EventHandler(this.buttonResetAndClearAllLoggers_Click);
      this.textBoxNumberOfIntervalls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxNumberOfIntervalls.Location = new Point(790, 23);
      this.textBoxNumberOfIntervalls.Margin = new Padding(4, 5, 4, 5);
      this.textBoxNumberOfIntervalls.Name = "textBoxNumberOfIntervalls";
      this.textBoxNumberOfIntervalls.ShortcutsEnabled = false;
      this.textBoxNumberOfIntervalls.Size = new Size(122, 26);
      this.textBoxNumberOfIntervalls.TabIndex = 2;
      this.textBoxNumberOfIntervalls.Leave += new System.EventHandler(this.textBoxNumberOfIntervalls_Leave);
      this.label12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(624, 394);
      this.label12.Margin = new Padding(4, 0, 4, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(145, 20);
      this.label12.TabIndex = 1;
      this.label12.Text = "Unused flash bytes";
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(625, 242);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(82, 20);
      this.label2.TabIndex = 1;
      this.label2.Text = "Start time:";
      this.buttonOkAndClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOkAndClose.Enabled = false;
      this.buttonOkAndClose.Location = new Point(618, 666);
      this.buttonOkAndClose.Margin = new Padding(4, 5, 4, 5);
      this.buttonOkAndClose.Name = "buttonOkAndClose";
      this.buttonOkAndClose.Size = new Size(309, 37);
      this.buttonOkAndClose.TabIndex = 3;
      this.buttonOkAndClose.Text = "Ok and close";
      this.buttonOkAndClose.UseVisualStyleBackColor = true;
      this.buttonOkAndClose.Click += new System.EventHandler(this.buttonOkAndClose_Click);
      this.buttonSaveChangedLoggerData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSaveChangedLoggerData.Enabled = false;
      this.buttonSaveChangedLoggerData.Location = new Point(616, 620);
      this.buttonSaveChangedLoggerData.Margin = new Padding(4, 5, 4, 5);
      this.buttonSaveChangedLoggerData.Name = "buttonSaveChangedLoggerData";
      this.buttonSaveChangedLoggerData.Size = new Size(309, 37);
      this.buttonSaveChangedLoggerData.TabIndex = 3;
      this.buttonSaveChangedLoggerData.Text = "Save changed logged data";
      this.buttonSaveChangedLoggerData.UseVisualStyleBackColor = true;
      this.buttonSaveChangedLoggerData.Click += new System.EventHandler(this.buttonSave_Click);
      this.textBoxMeterTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxMeterTime.Location = new Point(431, 5);
      this.textBoxMeterTime.Margin = new Padding(4, 5, 4, 5);
      this.textBoxMeterTime.Name = "textBoxMeterTime";
      this.textBoxMeterTime.ReadOnly = true;
      this.textBoxMeterTime.Size = new Size(175, 26);
      this.textBoxMeterTime.TabIndex = 2;
      this.label11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(334, 9);
      this.label11.Margin = new Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(88, 20);
      this.label11.TabIndex = 1;
      this.label11.Text = "Meter time:";
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(627, 28);
      this.label6.Margin = new Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(152, 20);
      this.label6.TabIndex = 1;
      this.label6.Text = "Number of intervalls:";
      this.groupBoxLoggerStorage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxLoggerStorage.Controls.Add((Control) this.radioButtonLoggerStorageFlash);
      this.groupBoxLoggerStorage.Controls.Add((Control) this.radioButtonLoggerStorageRam);
      this.groupBoxLoggerStorage.Enabled = false;
      this.groupBoxLoggerStorage.Location = new Point(614, 283);
      this.groupBoxLoggerStorage.Margin = new Padding(4, 5, 4, 5);
      this.groupBoxLoggerStorage.Name = "groupBoxLoggerStorage";
      this.groupBoxLoggerStorage.Padding = new Padding(4, 5, 4, 5);
      this.groupBoxLoggerStorage.Size = new Size(309, 96);
      this.groupBoxLoggerStorage.TabIndex = 3;
      this.groupBoxLoggerStorage.TabStop = false;
      this.groupBoxLoggerStorage.Text = "Logger storage";
      this.radioButtonLoggerStorageFlash.AutoSize = true;
      this.radioButtonLoggerStorageFlash.Location = new Point(12, 29);
      this.radioButtonLoggerStorageFlash.Margin = new Padding(4, 5, 4, 5);
      this.radioButtonLoggerStorageFlash.Name = "radioButtonLoggerStorageFlash";
      this.radioButtonLoggerStorageFlash.Size = new Size(206, 24);
      this.radioButtonLoggerStorageFlash.TabIndex = 0;
      this.radioButtonLoggerStorageFlash.TabStop = true;
      this.radioButtonLoggerStorageFlash.Text = "Flash (one time storage)";
      this.radioButtonLoggerStorageFlash.UseVisualStyleBackColor = true;
      this.radioButtonLoggerStorageRam.AutoSize = true;
      this.radioButtonLoggerStorageRam.Location = new Point(12, 65);
      this.radioButtonLoggerStorageRam.Margin = new Padding(4, 5, 4, 5);
      this.radioButtonLoggerStorageRam.Name = "radioButtonLoggerStorageRam";
      this.radioButtonLoggerStorageRam.Size = new Size(168, 24);
      this.radioButtonLoggerStorageRam.TabIndex = 0;
      this.radioButtonLoggerStorageRam.TabStop = true;
      this.radioButtonLoggerStorageRam.Text = "RAM (ring storage)";
      this.radioButtonLoggerStorageRam.UseVisualStyleBackColor = true;
      this.groupBoxIntervall.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxIntervall.Controls.Add((Control) this.Logger_HalfMonth_ckbx);
      this.groupBoxIntervall.Controls.Add((Control) this.textBoxIntervalDays);
      this.groupBoxIntervall.Controls.Add((Control) this.dateTimePickerIntervalTime);
      this.groupBoxIntervall.Controls.Add((Control) this.labelIntervalDays);
      this.groupBoxIntervall.Controls.Add((Control) this.labelIntervalTime);
      this.groupBoxIntervall.Controls.Add((Control) this.radioButtonTime);
      this.groupBoxIntervall.Controls.Add((Control) this.radioButtonMonth);
      this.groupBoxIntervall.Controls.Add((Control) this.radioButtonYear);
      this.groupBoxIntervall.Location = new Point(617, 63);
      this.groupBoxIntervall.Margin = new Padding(4, 5, 4, 5);
      this.groupBoxIntervall.Name = "groupBoxIntervall";
      this.groupBoxIntervall.Padding = new Padding(4, 5, 4, 5);
      this.groupBoxIntervall.Size = new Size(309, 169);
      this.groupBoxIntervall.TabIndex = 0;
      this.groupBoxIntervall.TabStop = false;
      this.groupBoxIntervall.Text = "Logger intervall";
      this.Logger_HalfMonth_ckbx.AutoSize = true;
      this.Logger_HalfMonth_ckbx.Location = new Point(98, 55);
      this.Logger_HalfMonth_ckbx.Margin = new Padding(4, 5, 4, 5);
      this.Logger_HalfMonth_ckbx.Name = "Logger_HalfMonth_ckbx";
      this.Logger_HalfMonth_ckbx.Size = new Size(170, 24);
      this.Logger_HalfMonth_ckbx.TabIndex = 6;
      this.Logger_HalfMonth_ckbx.Text = "HalfMonth_Interval";
      this.Logger_HalfMonth_ckbx.UseVisualStyleBackColor = true;
      this.Logger_HalfMonth_ckbx.CheckedChanged += new System.EventHandler(this.Logger_HalfMonth_ckbx_CheckedChanged);
      this.textBoxIntervalDays.Enabled = false;
      this.textBoxIntervalDays.Location = new Point(105, 86);
      this.textBoxIntervalDays.Margin = new Padding(4, 5, 4, 5);
      this.textBoxIntervalDays.Name = "textBoxIntervalDays";
      this.textBoxIntervalDays.Size = new Size(190, 26);
      this.textBoxIntervalDays.TabIndex = 5;
      this.textBoxIntervalDays.Leave += new System.EventHandler(this.loggerIntervalChanged);
      this.dateTimePickerIntervalTime.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateTimePickerIntervalTime.CustomFormat = "HH:mm:ss";
      this.dateTimePickerIntervalTime.Enabled = false;
      this.dateTimePickerIntervalTime.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerIntervalTime.Location = new Point(105, 126);
      this.dateTimePickerIntervalTime.Margin = new Padding(4, 5, 4, 5);
      this.dateTimePickerIntervalTime.Name = "dateTimePickerIntervalTime";
      this.dateTimePickerIntervalTime.ShowUpDown = true;
      this.dateTimePickerIntervalTime.Size = new Size(190, 26);
      this.dateTimePickerIntervalTime.TabIndex = 1;
      this.dateTimePickerIntervalTime.Value = new DateTime(2012, 1, 4, 0, 30, 0, 0);
      this.dateTimePickerIntervalTime.Leave += new System.EventHandler(this.loggerIntervalChanged);
      this.labelIntervalDays.AutoSize = true;
      this.labelIntervalDays.Location = new Point(12, 91);
      this.labelIntervalDays.Margin = new Padding(4, 0, 4, 0);
      this.labelIntervalDays.Name = "labelIntervalDays";
      this.labelIntervalDays.Size = new Size(45, 20);
      this.labelIntervalDays.TabIndex = 1;
      this.labelIntervalDays.Text = "Days";
      this.labelIntervalTime.AutoSize = true;
      this.labelIntervalTime.Location = new Point(12, 126);
      this.labelIntervalTime.Margin = new Padding(4, 0, 4, 0);
      this.labelIntervalTime.Name = "labelIntervalTime";
      this.labelIntervalTime.Size = new Size(83, 20);
      this.labelIntervalTime.TabIndex = 1;
      this.labelIntervalTime.Text = "HH:mm:ss";
      this.radioButtonTime.AutoSize = true;
      this.radioButtonTime.Checked = true;
      this.radioButtonTime.Location = new Point(208, 26);
      this.radioButtonTime.Margin = new Padding(4, 5, 4, 5);
      this.radioButtonTime.Name = "radioButtonTime";
      this.radioButtonTime.Size = new Size(68, 24);
      this.radioButtonTime.TabIndex = 0;
      this.radioButtonTime.TabStop = true;
      this.radioButtonTime.Text = "Time";
      this.radioButtonTime.UseVisualStyleBackColor = true;
      this.radioButtonTime.CheckedChanged += new System.EventHandler(this.loggerIntervalChanged);
      this.radioButtonMonth.AutoSize = true;
      this.radioButtonMonth.Location = new Point(98, 26);
      this.radioButtonMonth.Margin = new Padding(4, 5, 4, 5);
      this.radioButtonMonth.Name = "radioButtonMonth";
      this.radioButtonMonth.Size = new Size(79, 24);
      this.radioButtonMonth.TabIndex = 0;
      this.radioButtonMonth.Text = "Month";
      this.radioButtonMonth.UseVisualStyleBackColor = true;
      this.radioButtonMonth.CheckedChanged += new System.EventHandler(this.loggerIntervalChanged);
      this.radioButtonYear.AutoSize = true;
      this.radioButtonYear.Location = new Point(14, 26);
      this.radioButtonYear.Margin = new Padding(4, 5, 4, 5);
      this.radioButtonYear.Name = "radioButtonYear";
      this.radioButtonYear.Size = new Size(68, 24);
      this.radioButtonYear.TabIndex = 0;
      this.radioButtonYear.Text = "Year";
      this.radioButtonYear.UseVisualStyleBackColor = true;
      this.radioButtonYear.CheckedChanged += new System.EventHandler(this.loggerIntervalChanged);
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.ChanalData);
      this.tabControl1.Controls.Add((Control) this.ChanalSetup);
      this.tabControl1.Location = new Point(4, 9);
      this.tabControl1.Margin = new Padding(4, 5, 4, 5);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(602, 695);
      this.tabControl1.SizeMode = TabSizeMode.FillToRight;
      this.tabControl1.TabIndex = 4;
      this.ChanalData.Controls.Add((Control) this.checkBoxPhysicalView);
      this.ChanalData.Controls.Add((Control) this.dataGridViewChanalData);
      this.ChanalData.Location = new Point(4, 29);
      this.ChanalData.Margin = new Padding(4, 5, 4, 5);
      this.ChanalData.Name = "ChanalData";
      this.ChanalData.Padding = new Padding(4, 5, 4, 5);
      this.ChanalData.Size = new Size(474, 582);
      this.ChanalData.TabIndex = 1;
      this.ChanalData.Text = "Chanal data";
      this.ChanalData.UseVisualStyleBackColor = true;
      this.checkBoxPhysicalView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBoxPhysicalView.AutoSize = true;
      this.checkBoxPhysicalView.Location = new Point(342, 9);
      this.checkBoxPhysicalView.Margin = new Padding(4, 5, 4, 5);
      this.checkBoxPhysicalView.Name = "checkBoxPhysicalView";
      this.checkBoxPhysicalView.RightToLeft = RightToLeft.Yes;
      this.checkBoxPhysicalView.Size = new Size(126, 24);
      this.checkBoxPhysicalView.TabIndex = 3;
      this.checkBoxPhysicalView.Text = "Physical view";
      this.checkBoxPhysicalView.UseVisualStyleBackColor = true;
      this.checkBoxPhysicalView.CheckedChanged += new System.EventHandler(this.checkBoxPhysicalView_CheckedChanged);
      this.dataGridViewChanalData.AllowUserToAddRows = false;
      this.dataGridViewChanalData.AllowUserToDeleteRows = false;
      this.dataGridViewChanalData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewChanalData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewChanalData.Location = new Point(9, 42);
      this.dataGridViewChanalData.Margin = new Padding(4, 5, 4, 5);
      this.dataGridViewChanalData.MultiSelect = false;
      this.dataGridViewChanalData.Name = "dataGridViewChanalData";
      this.dataGridViewChanalData.RowHeadersVisible = false;
      this.dataGridViewChanalData.RowHeadersWidth = 62;
      this.dataGridViewChanalData.SelectionMode = DataGridViewSelectionMode.CellSelect;
      this.dataGridViewChanalData.Size = new Size(459, 501);
      this.dataGridViewChanalData.TabIndex = 0;
      this.ChanalSetup.Controls.Add((Control) this.textBoxChanalValueName);
      this.ChanalSetup.Controls.Add((Control) this.comboBoxChanalFunction);
      this.ChanalSetup.Controls.Add((Control) this.textBoxChanalLastTime);
      this.ChanalSetup.Controls.Add((Control) this.textBoxReducedNumberOfIntervalls);
      this.ChanalSetup.Controls.Add((Control) this.textBoxChanalBytes);
      this.ChanalSetup.Controls.Add((Control) this.label4);
      this.ChanalSetup.Controls.Add((Control) this.label3);
      this.ChanalSetup.Controls.Add((Control) this.label7);
      this.ChanalSetup.Controls.Add((Control) this.label9);
      this.ChanalSetup.Controls.Add((Control) this.labelRamFlashBytes);
      this.ChanalSetup.Location = new Point(4, 29);
      this.ChanalSetup.Margin = new Padding(4, 5, 4, 5);
      this.ChanalSetup.Name = "ChanalSetup";
      this.ChanalSetup.Size = new Size(594, 662);
      this.ChanalSetup.TabIndex = 2;
      this.ChanalSetup.Text = "Chanal setup";
      this.ChanalSetup.UseVisualStyleBackColor = true;
      this.comboBoxChanalFunction.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.comboBoxChanalFunction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxChanalFunction.FormattingEnabled = true;
      this.comboBoxChanalFunction.Location = new Point(175, 64);
      this.comboBoxChanalFunction.Margin = new Padding(4, 5, 4, 5);
      this.comboBoxChanalFunction.Name = "comboBoxChanalFunction";
      this.comboBoxChanalFunction.Size = new Size(398, 28);
      this.comboBoxChanalFunction.TabIndex = 3;
      this.comboBoxChanalFunction.SelectedIndexChanged += new System.EventHandler(this.comboBoxChanalFunction_SelectedIndexChanged);
      this.textBoxChanalLastTime.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxChanalLastTime.Location = new Point(175, 582);
      this.textBoxChanalLastTime.Margin = new Padding(4, 5, 4, 5);
      this.textBoxChanalLastTime.Name = "textBoxChanalLastTime";
      this.textBoxChanalLastTime.ReadOnly = true;
      this.textBoxChanalLastTime.ShortcutsEnabled = false;
      this.textBoxChanalLastTime.Size = new Size(398, 26);
      this.textBoxChanalLastTime.TabIndex = 2;
      this.textBoxReducedNumberOfIntervalls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxReducedNumberOfIntervalls.Location = new Point(336, 115);
      this.textBoxReducedNumberOfIntervalls.Margin = new Padding(4, 5, 4, 5);
      this.textBoxReducedNumberOfIntervalls.Name = "textBoxReducedNumberOfIntervalls";
      this.textBoxReducedNumberOfIntervalls.ReadOnly = true;
      this.textBoxReducedNumberOfIntervalls.ShortcutsEnabled = false;
      this.textBoxReducedNumberOfIntervalls.Size = new Size(237, 26);
      this.textBoxReducedNumberOfIntervalls.TabIndex = 2;
      this.textBoxReducedNumberOfIntervalls.Leave += new System.EventHandler(this.textBoxReducedNumberOfIntervalls_Leave);
      this.textBoxChanalBytes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxChanalBytes.Location = new Point(175, 549);
      this.textBoxChanalBytes.Margin = new Padding(4, 5, 4, 5);
      this.textBoxChanalBytes.Name = "textBoxChanalBytes";
      this.textBoxChanalBytes.ReadOnly = true;
      this.textBoxChanalBytes.ShortcutsEnabled = false;
      this.textBoxChanalBytes.Size = new Size(398, 26);
      this.textBoxChanalBytes.TabIndex = 2;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 119);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(310, 20);
      this.label4.TabIndex = 1;
      this.label4.Text = "Reduced number of RAM chanal intervalls:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 64);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(124, 20);
      this.label3.TabIndex = 1;
      this.label3.Text = "Chanal function:";
      this.label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(20, 618);
      this.label7.Margin = new Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(98, 20);
      this.label7.TabIndex = 1;
      this.label7.Text = "Value name:";
      this.label9.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(20, 585);
      this.label9.Margin = new Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(78, 20);
      this.label9.TabIndex = 1;
      this.label9.Text = "Last time:";
      this.labelRamFlashBytes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.labelRamFlashBytes.AutoSize = true;
      this.labelRamFlashBytes.Location = new Point(20, 549);
      this.labelRamFlashBytes.Margin = new Padding(4, 0, 4, 0);
      this.labelRamFlashBytes.Name = "labelRamFlashBytes";
      this.labelRamFlashBytes.Size = new Size(134, 20);
      this.labelRamFlashBytes.TabIndex = 1;
      this.labelRamFlashBytes.Text = "RAM/Flash bytes:";
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1428, 63);
      this.zennerCoroprateDesign2.TabIndex = 19;
      this.textBoxChanalValueName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxChanalValueName.Location = new Point(175, 618);
      this.textBoxChanalValueName.Margin = new Padding(4, 5, 4, 5);
      this.textBoxChanalValueName.Name = "textBoxChanalValueName";
      this.textBoxChanalValueName.ReadOnly = true;
      this.textBoxChanalValueName.ShortcutsEnabled = false;
      this.textBoxChanalValueName.Size = new Size(398, 26);
      this.textBoxChanalValueName.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ClientSize = new Size(1428, 1045);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Margin = new Padding(4, 5, 4, 5);
      this.MinimumSize = new Size(1273, 994);
      this.Name = nameof (LoggerView);
      this.Text = nameof (LoggerView);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel1.PerformLayout();
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.contextMenuStripLogger.ResumeLayout(false);
      this.splitContainer4.Panel1.ResumeLayout(false);
      this.splitContainer4.Panel1.PerformLayout();
      this.splitContainer4.Panel2.ResumeLayout(false);
      this.splitContainer4.Panel2.PerformLayout();
      this.splitContainer4.EndInit();
      this.splitContainer4.ResumeLayout(false);
      this.groupBoxLoggerStorage.ResumeLayout(false);
      this.groupBoxLoggerStorage.PerformLayout();
      this.groupBoxIntervall.ResumeLayout(false);
      this.groupBoxIntervall.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.ChanalData.ResumeLayout(false);
      this.ChanalData.PerformLayout();
      ((ISupportInitialize) this.dataGridViewChanalData).EndInit();
      this.ChanalSetup.ResumeLayout(false);
      this.ChanalSetup.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum SpecialParameterNodes
    {
      __NewCycleRamLogger,
      __NewSingleTimeFlashLogger,
    }
  }
}
