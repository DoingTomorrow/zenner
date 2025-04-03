// Decompiled with JetBrains decompiler
// Type: GMM_Handler.HandlerWindow
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using CorporateDesign;
using GmmDbLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class HandlerWindow : Form
  {
    private bool WindowInitialised = false;
    internal string StartComponentName;
    private ZR_HandlerFunctions MyHandler;
    private Meter DiagnosticMeter;
    private byte[] MeterEEPromFromFile;
    private bool BreakLoop;
    private static string[] PastTempSensorVars = new string[48]
    {
      "DefaultFunction.n_ref_man_1",
      "DefaultFunction.n_ref_man_2",
      "DefaultFunction.n_ref_exp_1",
      "DefaultFunction.n_ref_exp_2",
      "DefaultFunction.v_cal_man_1",
      "DefaultFunction.v_cal_man_2",
      "DefaultFunction.v_cal_exp_1",
      "DefaultFunction.v_cal_exp_2",
      "DefaultFunction.o_cal_man_1",
      "DefaultFunction.o_cal_man_2",
      "DefaultFunction.o_cal_exp_1",
      "DefaultFunction.o_cal_exp_2",
      "DefaultFunction.tf_man_1",
      "DefaultFunction.tf_man_2",
      "DefaultFunction.tf_man_3",
      "DefaultFunction.tf_man_4",
      "DefaultFunction.tf_man_5",
      "DefaultFunction.tf_man_6",
      "DefaultFunction.kf_rl_man_1",
      "DefaultFunction.kf_rl_man_2",
      "DefaultFunction.kf_rl_man_3",
      "DefaultFunction.kf_rl_man_4",
      "DefaultFunction.kf_rl_man_5",
      "DefaultFunction.kf_rl_man_6",
      "DefaultFunction.kf_vl_man_1",
      "DefaultFunction.kf_vl_man_2",
      "DefaultFunction.kf_vl_man_3",
      "DefaultFunction.kf_vl_man_4",
      "DefaultFunction.kf_vl_man_5",
      "DefaultFunction.kf_vl_man_6",
      "DefaultFunction.tf_exp_1",
      "DefaultFunction.tf_exp_2",
      "DefaultFunction.tf_exp_3",
      "DefaultFunction.tf_exp_4",
      "DefaultFunction.tf_exp_5",
      "DefaultFunction.tf_exp_6",
      "DefaultFunction.kf_rl_exp_1",
      "DefaultFunction.kf_rl_exp_2",
      "DefaultFunction.kf_rl_exp_3",
      "DefaultFunction.kf_rl_exp_4",
      "DefaultFunction.kf_rl_exp_5",
      "DefaultFunction.kf_rl_exp_6",
      "DefaultFunction.kf_vl_exp_1",
      "DefaultFunction.kf_vl_exp_2",
      "DefaultFunction.kf_vl_exp_3",
      "DefaultFunction.kf_vl_exp_4",
      "DefaultFunction.kf_vl_exp_5",
      "DefaultFunction.kf_vl_exp_6"
    };
    private SortedList<string, long> PastData;
    private TypeAnalysis TheAnalysis;
    private ErrTypeAnalysis TheErrAnalysis;
    private IContainer components = (IContainer) null;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem showToolStripMenuItem;
    private ToolStripMenuItem goToToolStripMenuItem;
    private ToolStripMenuItem globalMeterManagerToolStripMenuItem;
    private ToolStripMenuItem backToolStripMenuItem;
    private ToolStripMenuItem quitToolStripMenuItem;
    private ToolStripMenuItem serialBusToolStripMenuItem;
    private ToolStripMenuItem asyncComToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem designerToolStripMenuItem;
    private ToolStripMenuItem workToolStripMenuItem;
    private Button buttonCancle;
    private Button buttonOk;
    private GroupBox groupBox1;
    private Label label1;
    private ComboBox comboBoxDiagnosticObject;
    private ToolStripMenuItem blockListToolStripMenuItem;
    private CheckBox checkBoxShowFunctionNumbers;
    private CheckBox checkBoxShowFunctionNames;
    private CheckBox checkBoxShowBlockTypes;
    private ToolStripMenuItem parametersToolStripMenuItem;
    private Label label2;
    private ComboBox comboBoxCompareObject;
    private ToolStripMenuItem epromDifferenceToolStripMenuItem;
    private CheckBox checkBoxShowDiffsOnly;
    private ToolStripMenuItem parameterDifferenceToolStripMenuItem;
    private ToolStripMenuItem epromParameterByAddressToolStripMenuItem;
    private ToolStripMenuItem ramParameterByAddressToolStripMenuItem;
    private ToolStripMenuItem blockListDifferencesToolStripMenuItem;
    private Label label3;
    private ComboBox comboBoxEquelFunction;
    private Label label4;
    private ComboBox comboBoxGetListSelection;
    private ToolStripMenuItem MenuItemIsEquelResult;
    private ToolStripMenuItem MenuItemGetListList;
    private ToolStripMenuItem MenuItemShowMeterResources;
    private ToolStripMenuItem MenuItemCloseMeter;
    private ToolStripMenuItem MenuItemRamTest;
    private Button buttonBreakLoop;
    private Panel panel1;
    private Panel panel2;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private TextBox textBoxStatus;
    private ToolStripMenuItem MenuItemGetRendomNumber;
    private ToolStripMenuItem MenuItemRepare;
    private GroupBox groupBoxVars;
    private Button buttonChangeData;
    private Label label7;
    private Label labelValueRam;
    private Label labelValueEProm;
    private TextBox textBoxNewValue;
    private TextBox textBoxValueRam;
    private TextBox textBoxValueEprom;
    private Button buttonWriteToRam;
    private Button buttonWriteToEprom;
    private Button buttonReadVar;
    private Label label8;
    private TextBox textBoxByteSize;
    private CheckBox checkBoxUseOnlyDefaultValues;
    private ToolStripMenuItem saveMeterToolStripMenuItem;
    private ToolStripMenuItem workMeterToolStripMenuItem;
    private ToolStripMenuItem dbMeterToolStripMenuItem;
    private ToolStripMenuItem typeMeterToolStripMenuItem;
    private ToolStripMenuItem readMeterToolStripMenuItem;
    private ToolStripMenuItem connectedMeterToolStripMenuItem;
    private ToolStripMenuItem testLoggerEntriesToolStripMenuItem;
    private ToolStripMenuItem areParametersEqualToDBoverridesToolStripMenuItem;
    private CheckBox checkBoxDisableChecks;
    private ToolStripMenuItem resetAllDataToolStripMenuItem;
    private ToolStripMenuItem dataToolStripMenuItem;
    private ToolStripMenuItem CopyTempSensorCalibrationToolStripMenuItem;
    private ToolStripMenuItem PastTempSensorCalibrationToolStripMenuItem;
    private ToolStripMenuItem typeAnalysisWindowToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripSeparator toolStripSeparator3;
    private CheckBox checkBoxReadWithoutBackup;
    private ToolStripMenuItem MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripMenuItem MenuItemSetMaxValuesCritical;
    private ToolStripMenuItem MenuItemShowDeviceTimeEvents;
    private ToolStripMenuItem MenuItemDestroyBackupChecksum;
    private ToolStripMenuItem MenuItemErr8002Types;
    private CheckBox checkBoxIgnorIntervalMinutesRaster;
    private ListBox listBoxVariables;
    private ToolStripMenuItem MenuItemSetWriteProtectionAtClonedDevice;
    private ToolStripMenuItem blockListDifferencesEEPromDirectToolStripMenuItem;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem saveMeterEepromToFileToolStripMenuItem;
    private ToolStripMenuItem blockListDifferencesFromFoadedEepromToolStripMenuItem;
    private ToolStripMenuItem workMeterToolStripMenuItem1;
    private ToolStripMenuItem dbMeterToolStripMenuItem1;
    private ToolStripMenuItem typeMeterToolStripMenuItem1;
    private ToolStripMenuItem readMeterToolStripMenuItem1;
    private ToolStripMenuItem loadMeterEepromFromFileToolStripMenuItem;
    private OpenFileDialog openFileDialog1;
    private SaveFileDialog saveFileDialog1;
    private CheckBox checkBoxBackupForEachRead;
    private ToolStripMenuItem MenuItemIsEndTimeOk;

    public HandlerWindow(ZR_HandlerFunctions MyHandlerIn)
    {
      this.InitializeComponent();
      FormTranslatorSupport.TranslateWindow(Tg.GMM_HandlerWindow, (Form) this);
      this.MyHandler = MyHandlerIn;
      string[] names1 = Enum.GetNames(typeof (ZR_HandlerFunctions.MeterObjects));
      for (int index = 0; index < names1.Length; ++index)
      {
        this.comboBoxDiagnosticObject.Items.Add((object) names1[index]);
        this.comboBoxCompareObject.Items.Add((object) names1[index]);
      }
      foreach (object name in Enum.GetNames(typeof (ZR_HandlerFunctions.IsEqualFunctions)))
        this.comboBoxEquelFunction.Items.Add(name);
      string[] names2 = Enum.GetNames(typeof (ZR_HandlerFunctions.GetListFunctions));
      for (int index = 0; index < names2.Length; ++index)
      {
        this.comboBoxGetListSelection.Items.Add((object) names2[index]);
        this.listBoxVariables.Items.Add((object) names2[index]);
      }
      this.checkBoxDisableChecks.Checked = this.MyHandler.DisableChecks;
      this.checkBoxReadWithoutBackup.Checked = this.MyHandler.ReadWithoutBackup;
      this.checkBoxBackupForEachRead.Checked = this.MyHandler.BackupForEachReadInternal;
      this.checkBoxIgnorIntervalMinutesRaster.Checked = this.MyHandler.IgnoreIntervalMinutesRaster;
      this.WindowInitialised = true;
    }

    private void HandlerWindow_Load(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.comboBoxCompareObject.SelectedIndex = 0;
      this.comboBoxDiagnosticObject.SelectedIndex = 1;
      this.comboBoxEquelFunction.SelectedIndex = 0;
      this.comboBoxGetListSelection.SelectedIndex = 0;
      this.checkBoxShowFunctionNames.Checked = this.MyHandler.MyInfoFlags.ShowFunctionNames;
      this.checkBoxShowFunctionNumbers.Checked = this.MyHandler.MyInfoFlags.ShowFunctionNumbers;
      this.checkBoxShowBlockTypes.Checked = this.MyHandler.MyInfoFlags.ShowBlockTypes;
      this.checkBoxShowDiffsOnly.Checked = this.MyHandler.MyInfoFlags.ShowDiffsOnly;
      if (this.MyHandler.MyMeters.WorkMeter != null)
      {
        this.DiagnosticMeter = this.MyHandler.MyMeters.WorkMeter;
        this.listBoxVariables.Items.Clear();
        for (int index = 0; index < this.DiagnosticMeter.AllParameters.Count; ++index)
          this.listBoxVariables.Items.Add((object) this.DiagnosticMeter.AllParameters.GetKey(index).ToString());
      }
      this.textBoxByteSize.Text = string.Empty;
      this.textBoxValueEprom.Text = string.Empty;
      this.textBoxValueRam.Text = string.Empty;
      this.textBoxNewValue.Text = string.Empty;
      this.checkBoxUseOnlyDefaultValues.Checked = this.MyHandler.UseOnlyDefaultValues;
      if (this.MyHandler.MyMeters.ReadMeter != null)
        this.comboBoxCompareObject.SelectedIndex = 0;
      else if (this.MyHandler.MyMeters.DbMeter != null)
      {
        this.comboBoxCompareObject.SelectedIndex = 3;
      }
      else
      {
        if (this.MyHandler.MyMeters.TypeMeter == null)
          return;
        this.comboBoxCompareObject.SelectedIndex = 2;
      }
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      this.MyHandler.UseOnlyDefaultValues = this.checkBoxUseOnlyDefaultValues.Checked;
    }

    internal void InitStartMenu(string ComponentList)
    {
    }

    private void globalMeterManagerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "GMM";
      this.Close();
    }

    private void backToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Close();
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Exit";
      this.Close();
    }

    private void designerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Designer";
      this.Close();
    }

    private void serialBusToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "DeviceCollector";
      this.Close();
    }

    private void asyncComToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "AsyncCom";
      this.Close();
    }

    private void blockListToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      StringBuilder TheText = new StringBuilder();
      TheMeter.MyLinker.GetBlockListInfo(TheText);
      this.MyHandler.WriteAndShowFile("BlockList", TheText.ToString());
    }

    private void blockListDifferencesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetInfoSelectionFlags();
        string SelectionString1 = this.comboBoxDiagnosticObject.SelectedItem.ToString();
        Meter TheMeter1;
        if (!this.MyHandler.GetMeterObject(SelectionString1, out TheMeter1))
          return;
        string SelectionString2 = this.comboBoxCompareObject.SelectedItem.ToString();
        Meter TheMeter2;
        if (!this.MyHandler.GetMeterObject(SelectionString2, out TheMeter2) || TheMeter2 == null)
          return;
        StringBuilder stringBuilder1 = new StringBuilder(SelectionString1 + ": ");
        StringBuilder stringBuilder2 = new StringBuilder(SelectionString2 + ": ");
        TheMeter1.MyLinker.GetBlockListInfo(stringBuilder1);
        TheMeter2.MyLinker.GetBlockListInfo(stringBuilder2);
        this.MyHandler.WriteFilesAndShowFileDifferences("BlockList", stringBuilder1, "BlockListCompare", stringBuilder2);
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("View diff", "Error on start viewer", true);
      }
    }

    private void blockListDifferencesEEPromDirectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetInfoSelectionFlags();
        string SelectionString1 = this.comboBoxDiagnosticObject.SelectedItem.ToString();
        Meter TheMeter1;
        if (!this.MyHandler.GetMeterObject(SelectionString1, out TheMeter1))
          return;
        string SelectionString2 = this.comboBoxCompareObject.SelectedItem.ToString();
        Meter TheMeter2;
        if (!this.MyHandler.GetMeterObject(SelectionString2, out TheMeter2) || TheMeter2 == null)
          return;
        StringBuilder stringBuilder1 = new StringBuilder(SelectionString1 + ": ");
        StringBuilder stringBuilder2 = new StringBuilder(SelectionString2 + ": ");
        TheMeter1.MyLinker.GetBlockListInfo(stringBuilder1);
        TheMeter2.MyLinker.GetBlockListDiffInfo(stringBuilder1, stringBuilder2, TheMeter2.Eprom);
        this.MyHandler.WriteFilesAndShowFileDifferences("BlockList", stringBuilder1, "BlockListCompare", stringBuilder2);
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("View diff", "Error on start viewer", true);
      }
    }

    private void blockListDifferencesFromFoadedEepromToolStripMenuItem_Click(
      object sender,
      EventArgs e)
    {
      if (this.MeterEEPromFromFile == null)
        return;
      try
      {
        this.SetInfoSelectionFlags();
        string SelectionString = this.comboBoxDiagnosticObject.SelectedItem.ToString();
        Meter TheMeter;
        if (!this.MyHandler.GetMeterObject(SelectionString, out TheMeter))
          return;
        StringBuilder stringBuilder1 = new StringBuilder(SelectionString + ": ");
        StringBuilder stringBuilder2 = new StringBuilder("From loaded EEProm: ");
        TheMeter.MyLinker.GetBlockListInfo(stringBuilder1);
        TheMeter.MyLinker.GetBlockListDiffInfo(stringBuilder1, stringBuilder2, this.MeterEEPromFromFile);
        this.MyHandler.WriteFilesAndShowFileDifferences("BlockList", stringBuilder1, "BlockListCompare", stringBuilder2);
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("View diff", "Error on start viewer", true);
      }
    }

    private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      StringBuilder TheText = new StringBuilder();
      TheMeter.MyLinker.GetParameterInfo(TheText);
      this.MyHandler.WriteAndShowFile("Parameters", TheText.ToString());
    }

    private void SetInfoSelectionFlags()
    {
      this.MyHandler.MyInfoFlags.ShowFunctionNames = this.checkBoxShowFunctionNames.Checked;
      this.MyHandler.MyInfoFlags.ShowFunctionNumbers = this.checkBoxShowFunctionNumbers.Checked;
      this.MyHandler.MyInfoFlags.ShowBlockTypes = this.checkBoxShowBlockTypes.Checked;
      this.MyHandler.MyInfoFlags.ShowDiffsOnly = this.checkBoxShowDiffsOnly.Checked;
    }

    private void epromDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter1;
      Meter TheMeter2;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter1) || !this.MyHandler.GetMeterObject(this.comboBoxCompareObject.SelectedItem.ToString(), out TheMeter2))
        return;
      StringBuilder TheText = new StringBuilder();
      DataChecker.GetEpromDiffs(TheMeter1.Eprom, TheMeter2.Eprom, this.MyHandler.MyInfoFlags.ShowDiffsOnly, TheText);
      this.MyHandler.WriteAndShowFile("EPromDiffs", TheText.ToString());
    }

    private void parameterDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter1;
      Meter TheMeter2;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter1) || !this.MyHandler.GetMeterObject(this.comboBoxCompareObject.SelectedItem.ToString(), out TheMeter2) || TheMeter2 == null)
        return;
      StringBuilder TheText = new StringBuilder();
      DataChecker.GetParameterDiffs(TheMeter1.AllParameters, TheMeter2.AllParameters, TheText);
      this.MyHandler.WriteAndShowFile("ParameterDiffs", TheText.ToString());
    }

    private void epromParameterByAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      StringBuilder TheText = new StringBuilder();
      DataChecker.GetEpromParameterByAddress(TheMeter.AllEpromParametersByAddress, TheText);
      this.MyHandler.WriteAndShowFile("EpromParametersByAddress", TheText.ToString());
    }

    private void ramParameterByAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      StringBuilder TheText = new StringBuilder();
      DataChecker.GetRamParameterByAddress(TheMeter, TheText);
      this.MyHandler.WriteAndShowFile("RamParametersByAddress", TheText.ToString());
    }

    private void MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter_Click(
      object sender,
      EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      this.MyHandler.WriteAndShowFile("StaticDiffs", DataChecker.GetStaticDiffToExternalConnectedMeter(TheMeter));
    }

    private void MenuItemIsEquelResult_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.MyHandler.IsEqual((ZR_HandlerFunctions.IsEqualFunctions) Enum.Parse(typeof (ZR_HandlerFunctions.IsEqualFunctions), this.comboBoxEquelFunction.Items[this.comboBoxEquelFunction.SelectedIndex].ToString()), (ZR_HandlerFunctions.MeterObjects) Enum.Parse(typeof (ZR_HandlerFunctions.MeterObjects), this.comboBoxDiagnosticObject.Items[this.comboBoxDiagnosticObject.SelectedIndex].ToString()), (ZR_HandlerFunctions.MeterObjects) Enum.Parse(typeof (ZR_HandlerFunctions.MeterObjects), this.comboBoxCompareObject.Items[this.comboBoxCompareObject.SelectedIndex].ToString())))
        {
          int num1 = (int) GMM_MessageBox.ShowMessage("IsEqual result", "The objects are equal");
        }
        else
        {
          int num2 = (int) GMM_MessageBox.ShowMessage("IsEqual result", "The objects are not equal", true);
        }
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("IsEqual result", "Compare error", true);
      }
    }

    private void MenuItemGetListList_Click(object sender, EventArgs e)
    {
      try
      {
        string[] list = this.MyHandler.GetList((ZR_HandlerFunctions.GetListFunctions) Enum.Parse(typeof (ZR_HandlerFunctions.GetListFunctions), this.comboBoxGetListSelection.Items[this.comboBoxGetListSelection.SelectedIndex].ToString()), (ZR_HandlerFunctions.MeterObjects) Enum.Parse(typeof (ZR_HandlerFunctions.MeterObjects), this.comboBoxDiagnosticObject.Items[this.comboBoxDiagnosticObject.SelectedIndex].ToString()));
        if (list == null)
        {
          int num1 = (int) GMM_MessageBox.ShowMessage("GetList result", "No list available", true);
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (int index = 0; index < list.Length; ++index)
            stringBuilder.AppendLine(list[index]);
          int num2 = (int) GMM_MessageBox.ShowMessage("GetList result", stringBuilder.ToString());
        }
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("GetList result", "Function error", true);
      }
    }

    private void MenuItemShowMeterResources_Click(object sender, EventArgs e)
    {
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("Meter resources", "Meter object not available", true);
      }
      else
      {
        StringBuilder TheText = new StringBuilder();
        if (!DataChecker.GetMeterResourcesList(TheMeter, TheText))
        {
          int num2 = (int) GMM_MessageBox.ShowMessage("Meter resources", "Resource data not available.", true);
        }
        else
          this.MyHandler.WriteAndShowFile("MeterResources", TheText.ToString());
      }
    }

    private void MenuItemCloseMeter_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.WorkMeter == null)
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      else
        this.MyHandler.MyMeters.SetMeterKey(0U);
    }

    private void MenuItemRamTest_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (!this.MyHandler.SerBus.SetEmergencyMode())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("EPROM test", "Emergency mode error");
      }
      else
      {
        Random random = new Random();
        this.BreakLoop = false;
        while (!this.BreakLoop)
        {
          int num2 = random.Next(1500) + 32;
          int num3 = random.Next(500);
          int num4 = random.Next(30);
          this.textBoxStatus.Text = "StartAddress: 0x" + num2.ToString("x04") + "    Size: " + num3.ToString("d3") + "     Rate: " + num4.ToString() + "%";
          int StartAddress = num2 - 32;
          int num5 = num2 + num3 + 32;
          int num6 = num2 + num3 - 1;
          int length = num3 + 32 + 32;
          ByteField MemoryData1;
          if (!this.MyHandler.SerBus.ReadMemory(MemoryLocation.EEPROM, StartAddress, length, out MemoryData1))
          {
            int num7 = (int) GMM_MessageBox.ShowMessage("EPROM test", "Read error");
            break;
          }
          ByteField NewData = new ByteField(length);
          bool[] flagArray = new bool[length];
          for (int index = 0; index < MemoryData1.Count; ++index)
            NewData.Add(MemoryData1.Data[index]);
          for (int index = 32; index < 32 + num3; ++index)
          {
            if (random.Next(100) < num4)
            {
              flagArray[index] = true;
              NewData.Data[index] = (byte) random.Next((int) byte.MaxValue);
            }
          }
          if (!this.MyHandler.SerBus.UpdateMemory(MemoryLocation.EEPROM, StartAddress, MemoryData1, NewData))
          {
            int num8 = (int) GMM_MessageBox.ShowMessage("EPROM test", "Write error");
            break;
          }
          ByteField MemoryData2;
          if (!this.MyHandler.SerBus.ReadMemory(MemoryLocation.EEPROM, StartAddress, length, out MemoryData2))
          {
            int num9 = (int) GMM_MessageBox.ShowMessage("EPROM test", "Test read error");
            break;
          }
          for (int index1 = 0; index1 < MemoryData1.Count; ++index1)
          {
            if ((int) MemoryData2.Data[index1] != (int) NewData.Data[index1])
            {
              StringBuilder stringBuilder = new StringBuilder();
              stringBuilder.AppendLine("Test read error at address: " + (index1 + StartAddress).ToString("x04"));
              stringBuilder.AppendLine();
              for (int index2 = index1 - 32; index2 < index1 + 32; ++index2)
              {
                if (index2 < 0)
                  index2 = 0;
                stringBuilder.Append((index2 + StartAddress).ToString("x04") + ":");
                stringBuilder.Append(flagArray[index2]);
                stringBuilder.AppendLine();
              }
              int num10 = (int) GMM_MessageBox.ShowMessage("EPROM test", stringBuilder.ToString());
              goto label_29;
            }
          }
          Application.DoEvents();
        }
      }
label_29:
      this.textBoxStatus.Text = string.Empty;
      this.Cursor = Cursors.Default;
    }

    private void buttonBreakLoop_Click(object sender, EventArgs e) => this.BreakLoop = true;

    private void MenuItemGetRendomNumber_Click(object sender, EventArgs e)
    {
      try
      {
        long Key;
        MeterDBAccess.ValueTypes ValueType;
        if (!this.MyHandler.MyDataBaseAccess.GetDeviceKeys(this.MyHandler.MyMeters.WorkMeter.MyIdent.MeterID, out Key, out ValueType))
        {
          int num1 = (int) GMM_MessageBox.ShowMessage("Handler info", "No key");
        }
        else if (ValueType == MeterDBAccess.ValueTypes.GovernmentRandomNr)
        {
          int num2 = (int) GMM_MessageBox.ShowMessage("Handler info", "DatabaseKey: " + Key.ToString());
        }
        else
        {
          int num3 = (int) GMM_MessageBox.ShowMessage("Handler info", "MeterKey: " + Key.ToString());
        }
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Handler", "Object error");
      }
    }

    private void listBoxVariables_SelectedIndexChanged_1(object sender, EventArgs e)
    {
      this.GetNewVarData();
    }

    private void buttonReadVar_Click(object sender, EventArgs e) => this.GetNewVarData();

    private void GetNewVarData()
    {
      Parameter activeParameter = this.GetActiveParameter();
      if (activeParameter == null)
        return;
      if (activeParameter.ExistOnEprom)
        this.labelValueEProm.Text = "Value EPROM (0x" + activeParameter.Address.ToString("x04") + ")";
      else
        this.labelValueEProm.Text = "Value EPROM (not available)";
      if (activeParameter.ExistOnCPU)
        this.labelValueRam.Text = "Value RAM (0x" + activeParameter.AddressCPU.ToString("x04") + ")";
      else
        this.labelValueRam.Text = "Value RAM (not available)";
      this.textBoxByteSize.Text = activeParameter.Size.ToString();
      this.panel1.Enabled = false;
      this.textBoxValueRam.Text = "---";
      if (activeParameter.ExistOnCPU && this.DiagnosticMeter != null && this.DiagnosticMeter.MyCommunication != null)
      {
        if (!this.DiagnosticMeter.MyCommunication.ReadParameterValue(activeParameter, MemoryLocation.RAM))
        {
          this.textBoxValueRam.Text = "???";
          int num = (int) GMM_MessageBox.ShowMessage("Hander message", "RAM: read error", true);
        }
        else
          this.textBoxValueRam.Text = this.ParameterString(activeParameter.ValueCPU, activeParameter.Size);
      }
      if (activeParameter.ExistOnEprom)
        this.textBoxValueEprom.Text = this.ParameterString(activeParameter.ValueEprom, activeParameter.Size);
      else
        this.textBoxValueEprom.Text = "---";
      this.panel1.Enabled = true;
    }

    private string ParameterString(long Value, int ByteSize)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(Value.ToString());
      stringBuilder.Append(" = 0x");
      stringBuilder.Append(Value.ToString("x"));
      if (ByteSize == 4)
      {
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) Value);
        stringBuilder.Append(" = '");
        stringBuilder.Append(dateTime.ToShortDateString());
        stringBuilder.Append(" ");
        stringBuilder.Append(dateTime.ToString("HH:mm:ss"));
        stringBuilder.Append("'");
      }
      return stringBuilder.ToString();
    }

    private void buttonChangeData_Click(object sender, EventArgs e) => this.SetEpromData();

    private bool SetEpromData()
    {
      long TheData;
      if (!this.GetData(out TheData))
        return false;
      Parameter activeParameter = this.GetActiveParameter();
      if (activeParameter == null)
        return false;
      if (!activeParameter.ExistOnEprom)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Handler message", "No eprom value available");
        return false;
      }
      activeParameter.ValueEprom = TheData;
      activeParameter.UpdateByteList();
      return true;
    }

    private void buttonWriteToEprom_Click(object sender, EventArgs e)
    {
      if (!this.SetEpromData())
        return;
      Parameter activeParameter = this.GetActiveParameter();
      if (activeParameter == null)
        return;
      this.panel1.Enabled = false;
      this.DiagnosticMeter.MyCommunication.WriteParameterValue(activeParameter, MemoryLocation.EEPROM);
      this.panel1.Enabled = true;
    }

    private void buttonWriteToRam_Click(object sender, EventArgs e)
    {
      long TheData;
      if (!this.GetData(out TheData))
        return;
      Parameter activeParameter = this.GetActiveParameter();
      if (activeParameter == null)
        return;
      if (!activeParameter.ExistOnCPU)
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("Handler message", "No ram value available");
      }
      else
      {
        activeParameter.ValueCPU = TheData;
        this.panel1.Enabled = false;
        if (!this.DiagnosticMeter.MyCommunication.WriteParameterValue(activeParameter, MemoryLocation.RAM))
        {
          int num2 = (int) GMM_MessageBox.ShowMessage("Hander message", "Write error", true);
        }
        this.panel1.Enabled = true;
      }
    }

    private Parameter GetActiveParameter()
    {
      if (this.DiagnosticMeter == null || this.listBoxVariables.SelectedItem == null)
        return (Parameter) null;
      string key = this.listBoxVariables.SelectedItem.ToString();
      return key.Length > 0 ? (Parameter) this.DiagnosticMeter.AllParameters[(object) key] : (Parameter) null;
    }

    private bool GetData(out long TheData)
    {
      TheData = 0L;
      try
      {
        this.textBoxNewValue.Text.Trim();
        if (this.textBoxNewValue.Text.IndexOf('.') > 0)
        {
          DateTime TheTime = DateTime.Parse(this.textBoxNewValue.Text);
          TheData = (long) ZR_Calendar.Cal_GetMeterTime(TheTime);
        }
        else
          TheData = !this.textBoxNewValue.Text.StartsWith("0x") ? long.Parse(this.textBoxNewValue.Text) : long.Parse(this.textBoxNewValue.Text.Substring(2), NumberStyles.HexNumber);
        return true;
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Handler message", "Illegal data", true);
        return false;
      }
    }

    private void MenuItemRepare_Click(object sender, EventArgs e)
    {
      this.MyHandler.MyMeters.RepareAndCompress();
    }

    private void workMeterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MyHandler.MyMeters.SavedMeter = this.MyHandler.MyMeters.WorkMeter;
    }

    private void dbMeterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MyHandler.MyMeters.SavedMeter = this.MyHandler.MyMeters.DbMeter;
    }

    private void typeMeterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MyHandler.MyMeters.SavedMeter = this.MyHandler.MyMeters.TypeMeter;
    }

    private void readMeterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MyHandler.MyMeters.SavedMeter = this.MyHandler.MyMeters.ReadMeter;
    }

    private void connectedMeterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MyHandler.MyMeters.SavedMeter = this.MyHandler.MyMeters.ConnectedMeter;
    }

    private void testLoggerEntriesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        if (DataChecker.IsLoggerEqualToTable(this.DiagnosticMeter))
        {
          int num = (int) GMM_MessageBox.ShowMessage("Handler message", "Alle daten sind gleich");
          return;
        }
      }
      catch
      {
      }
      int num1 = (int) GMM_MessageBox.ShowMessage("Handler message", "Die Logger Daten sind verschieden", true);
    }

    private void areParametersEqualToDBoverridesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        if (DataChecker.AreOverridesEqualToDatabase(this.DiagnosticMeter))
        {
          int num = (int) GMM_MessageBox.ShowMessage("Handler message", "All checked data are ok");
          return;
        }
      }
      catch
      {
      }
      int num1 = (int) GMM_MessageBox.ShowMessage("Handler message", "The data are inconsistent", true);
    }

    private void MenuItemIsEndTimeOk_Click(object sender, EventArgs e)
    {
      if (!this.MyHandler.GetMeterObject(this.comboBoxCompareObject.SelectedItem.ToString(), out Meter _))
        return;
      string Info = "Calculation Error";
      try
      {
        if (DataChecker.IsEndTimeOk(this.DiagnosticMeter, out Info))
        {
          int num = (int) GMM_MessageBox.ShowMessage("Handler message", "All times ok: " + Info);
          return;
        }
      }
      catch
      {
      }
      int num1 = (int) GMM_MessageBox.ShowMessage("Handler message", "Times not ok: " + Info, true);
    }

    private void checkBoxDisableChecks_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.WindowInitialised)
        return;
      this.MyHandler.DisableChecks = this.checkBoxDisableChecks.Checked;
      this.MyHandler.checksumErrorsAsWarning = this.checkBoxDisableChecks.Checked;
    }

    private void checkBoxReadWithoutBackup_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.WindowInitialised)
        return;
      this.MyHandler.ReadWithoutBackup = this.checkBoxReadWithoutBackup.Checked;
    }

    private void checkBoxBackupForEachRead_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.WindowInitialised)
        return;
      this.MyHandler.BackupForEachReadInternal = this.checkBoxBackupForEachRead.Checked;
    }

    private void checkBoxIgnorIntervalMinutesRaster_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.WindowInitialised)
        return;
      this.MyHandler.IgnoreIntervalMinutesRaster = this.checkBoxIgnorIntervalMinutesRaster.Checked;
    }

    private void resetAllDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MyHandler.ChangeMeterData(new List<Parameter.ParameterGroups>()
      {
        Parameter.ParameterGroups.CONSUMATION,
        Parameter.ParameterGroups.EXTERNAL_IDENT
      });
    }

    private void CopyTempSensorCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.WorkMeter == null)
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("Handler", "No Data");
      }
      else
      {
        try
        {
          this.PastData = new SortedList<string, long>();
          for (int index = 0; index < HandlerWindow.PastTempSensorVars.Length; ++index)
          {
            Parameter allParameter = (Parameter) this.MyHandler.MyMeters.WorkMeter.AllParameters[(object) HandlerWindow.PastTempSensorVars[index]];
            this.PastData.Add(HandlerWindow.PastTempSensorVars[index], allParameter.ValueEprom);
          }
          this.PastTempSensorCalibrationToolStripMenuItem.Enabled = true;
        }
        catch
        {
          int num2 = (int) GMM_MessageBox.ShowMessage("Handler", "Copy error", true);
          return;
        }
        int num3 = (int) GMM_MessageBox.ShowMessage("Handler", "Copy ok");
      }
    }

    private void PastTempSensorCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.WorkMeter == null)
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("Handler", "No Data");
      }
      else
      {
        try
        {
          for (int index = 0; index < HandlerWindow.PastTempSensorVars.Length; ++index)
          {
            Parameter allParameter = (Parameter) this.MyHandler.MyMeters.WorkMeter.AllParameters[(object) HandlerWindow.PastTempSensorVars[index]];
            allParameter.ValueEprom = this.PastData[HandlerWindow.PastTempSensorVars[index]];
            allParameter.UpdateByteList();
          }
        }
        catch
        {
          int num2 = (int) GMM_MessageBox.ShowMessage("Handler", "Past error", true);
          return;
        }
        int num3 = (int) GMM_MessageBox.ShowMessage("Handler", "Past ok");
      }
    }

    private void typeAnalysisWindowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.TheAnalysis == null)
        this.TheAnalysis = new TypeAnalysis(this.MyHandler);
      int num = (int) this.TheAnalysis.ShowDialog();
    }

    private void MenuItemErr8002Types_Click(object sender, EventArgs e)
    {
      if (this.TheErrAnalysis == null)
        this.TheErrAnalysis = new ErrTypeAnalysis(this.MyHandler);
      int num = (int) this.TheErrAnalysis.ShowDialog();
    }

    private void MenuItemSetMaxValuesCritical_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      DataChecker.SetAllMaxValuesCritical(TheMeter);
    }

    private void MenuItemShowDeviceTimeEvents_Click(object sender, EventArgs e)
    {
      this.SetInfoSelectionFlags();
      Meter TheMeter;
      if (!this.MyHandler.GetMeterObject(this.comboBoxDiagnosticObject.SelectedItem.ToString(), out TheMeter))
        return;
      int num = (int) GMM_MessageBox.ShowMessage("Time events", DataChecker.GetAllEventTimes(TheMeter));
    }

    private void MenuItemDestroyBackupChecksum_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.WorkMeter == null || this.MyHandler.MyMeters.WorkMeter.MyCommunication == null)
        return;
      int index = this.MyHandler.MyMeters.WorkMeter.AllParameters.IndexOfKey((object) "EEP_Header.EEP_HEADER_BackupChecksum");
      if (index <= 0)
        return;
      Parameter byIndex = (Parameter) this.MyHandler.MyMeters.WorkMeter.AllParameters.GetByIndex(index);
      ++byIndex.ValueEprom;
      this.MyHandler.MyMeters.WorkMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
    }

    private void listBoxVariables_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData == Keys.Down)
      {
        if (this.listBoxVariables.SelectedIndex <= 0)
          return;
        --this.listBoxVariables.SelectedIndex;
      }
      else
      {
        if (e.KeyData != Keys.Up || this.listBoxVariables.SelectedIndex >= this.listBoxVariables.Items.Count - 1)
          return;
        ++this.listBoxVariables.SelectedIndex;
      }
    }

    private void MenuItemSetWriteProtectionAtClonedDevice_Click(object sender, EventArgs e)
    {
    }

    private void workMeterToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.WorkMeter == null)
        return;
      this.SaveEEProm(this.MyHandler.MyMeters.WorkMeter.Eprom);
    }

    private void dbMeterToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.DbMeter == null)
        return;
      this.SaveEEProm(this.MyHandler.MyMeters.DbMeter.Eprom);
    }

    private void typeMeterToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.TypeMeter == null)
        return;
      this.SaveEEProm(this.MyHandler.MyMeters.TypeMeter.Eprom);
    }

    private void readMeterToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.MyHandler.MyMeters.ReadMeter == null)
        return;
      this.SaveEEProm(this.MyHandler.MyMeters.ReadMeter.Eprom);
    }

    private void SaveEEProm(byte[] TheEEPromData)
    {
      this.saveFileDialog1.FileName = SystemValues.LoggDataPath;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(this.saveFileDialog1.FileName))
        {
          for (int index = 0; index < TheEEPromData.Length; ++index)
          {
            if (index % 16 == 0)
            {
              if (index != 0)
                streamWriter.WriteLine();
              streamWriter.Write(index.ToString("x04") + ":");
            }
            streamWriter.Write(" " + TheEEPromData[index].ToString("x02"));
          }
          streamWriter.WriteLine();
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("GMM handler", "File error" + Environment.NewLine + ex.Message);
      }
    }

    private void loadMeterEepromFromFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.openFileDialog1.FileName = SystemValues.LoggDataPath;
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      List<byte> byteList = new List<byte>();
      try
      {
        using (StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName))
        {
label_8:
          string str = streamReader.ReadLine();
          if (str != null)
          {
            for (int startIndex = 6; startIndex < str.Length - 1; startIndex += 3)
              byteList.Add(byte.Parse(str.Substring(startIndex, 2), NumberStyles.HexNumber));
            goto label_8;
          }
        }
        this.MeterEEPromFromFile = new byte[byteList.Count];
        for (int index = 0; index < this.MeterEEPromFromFile.Length; ++index)
          this.MeterEEPromFromFile[index] = byteList[index];
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("GMM handler", "File error" + Environment.NewLine + ex.Message);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HandlerWindow));
      this.menuStrip1 = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.saveMeterEepromToFileToolStripMenuItem = new ToolStripMenuItem();
      this.workMeterToolStripMenuItem1 = new ToolStripMenuItem();
      this.dbMeterToolStripMenuItem1 = new ToolStripMenuItem();
      this.typeMeterToolStripMenuItem1 = new ToolStripMenuItem();
      this.readMeterToolStripMenuItem1 = new ToolStripMenuItem();
      this.loadMeterEepromFromFileToolStripMenuItem = new ToolStripMenuItem();
      this.workToolStripMenuItem = new ToolStripMenuItem();
      this.MenuItemCloseMeter = new ToolStripMenuItem();
      this.MenuItemRamTest = new ToolStripMenuItem();
      this.MenuItemRepare = new ToolStripMenuItem();
      this.saveMeterToolStripMenuItem = new ToolStripMenuItem();
      this.workMeterToolStripMenuItem = new ToolStripMenuItem();
      this.dbMeterToolStripMenuItem = new ToolStripMenuItem();
      this.typeMeterToolStripMenuItem = new ToolStripMenuItem();
      this.readMeterToolStripMenuItem = new ToolStripMenuItem();
      this.connectedMeterToolStripMenuItem = new ToolStripMenuItem();
      this.resetAllDataToolStripMenuItem = new ToolStripMenuItem();
      this.MenuItemDestroyBackupChecksum = new ToolStripMenuItem();
      this.MenuItemSetWriteProtectionAtClonedDevice = new ToolStripMenuItem();
      this.showToolStripMenuItem = new ToolStripMenuItem();
      this.typeAnalysisWindowToolStripMenuItem = new ToolStripMenuItem();
      this.MenuItemErr8002Types = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.blockListToolStripMenuItem = new ToolStripMenuItem();
      this.blockListDifferencesToolStripMenuItem = new ToolStripMenuItem();
      this.blockListDifferencesEEPromDirectToolStripMenuItem = new ToolStripMenuItem();
      this.blockListDifferencesFromFoadedEepromToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.parametersToolStripMenuItem = new ToolStripMenuItem();
      this.epromDifferenceToolStripMenuItem = new ToolStripMenuItem();
      this.parameterDifferenceToolStripMenuItem = new ToolStripMenuItem();
      this.epromParameterByAddressToolStripMenuItem = new ToolStripMenuItem();
      this.ramParameterByAddressToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.MenuItemIsEquelResult = new ToolStripMenuItem();
      this.MenuItemGetListList = new ToolStripMenuItem();
      this.MenuItemShowMeterResources = new ToolStripMenuItem();
      this.MenuItemGetRendomNumber = new ToolStripMenuItem();
      this.testLoggerEntriesToolStripMenuItem = new ToolStripMenuItem();
      this.areParametersEqualToDBoverridesToolStripMenuItem = new ToolStripMenuItem();
      this.MenuItemIsEndTimeOk = new ToolStripMenuItem();
      this.toolStripSeparator5 = new ToolStripSeparator();
      this.MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter = new ToolStripMenuItem();
      this.MenuItemSetMaxValuesCritical = new ToolStripMenuItem();
      this.MenuItemShowDeviceTimeEvents = new ToolStripMenuItem();
      this.goToToolStripMenuItem = new ToolStripMenuItem();
      this.globalMeterManagerToolStripMenuItem = new ToolStripMenuItem();
      this.backToolStripMenuItem = new ToolStripMenuItem();
      this.quitToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.designerToolStripMenuItem = new ToolStripMenuItem();
      this.serialBusToolStripMenuItem = new ToolStripMenuItem();
      this.asyncComToolStripMenuItem = new ToolStripMenuItem();
      this.dataToolStripMenuItem = new ToolStripMenuItem();
      this.CopyTempSensorCalibrationToolStripMenuItem = new ToolStripMenuItem();
      this.PastTempSensorCalibrationToolStripMenuItem = new ToolStripMenuItem();
      this.buttonCancle = new Button();
      this.buttonOk = new Button();
      this.groupBox1 = new GroupBox();
      this.checkBoxShowDiffsOnly = new CheckBox();
      this.checkBoxShowBlockTypes = new CheckBox();
      this.checkBoxShowFunctionNames = new CheckBox();
      this.checkBoxShowFunctionNumbers = new CheckBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.comboBoxGetListSelection = new ComboBox();
      this.comboBoxEquelFunction = new ComboBox();
      this.comboBoxCompareObject = new ComboBox();
      this.comboBoxDiagnosticObject = new ComboBox();
      this.buttonBreakLoop = new Button();
      this.panel1 = new Panel();
      this.checkBoxBackupForEachRead = new CheckBox();
      this.checkBoxReadWithoutBackup = new CheckBox();
      this.checkBoxIgnorIntervalMinutesRaster = new CheckBox();
      this.checkBoxDisableChecks = new CheckBox();
      this.checkBoxUseOnlyDefaultValues = new CheckBox();
      this.groupBoxVars = new GroupBox();
      this.listBoxVariables = new ListBox();
      this.buttonReadVar = new Button();
      this.buttonWriteToRam = new Button();
      this.buttonWriteToEprom = new Button();
      this.buttonChangeData = new Button();
      this.label7 = new Label();
      this.labelValueRam = new Label();
      this.label8 = new Label();
      this.labelValueEProm = new Label();
      this.textBoxNewValue = new TextBox();
      this.textBoxValueRam = new TextBox();
      this.textBoxByteSize = new TextBox();
      this.textBoxValueEprom = new TextBox();
      this.textBoxStatus = new TextBox();
      this.panel2 = new Panel();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.openFileDialog1 = new OpenFileDialog();
      this.saveFileDialog1 = new SaveFileDialog();
      this.menuStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupBoxVars.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip1.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.workToolStripMenuItem,
        (ToolStripItem) this.showToolStripMenuItem,
        (ToolStripItem) this.goToToolStripMenuItem,
        (ToolStripItem) this.dataToolStripMenuItem
      });
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.Name = "menuStrip1";
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveMeterEepromToFileToolStripMenuItem,
        (ToolStripItem) this.loadMeterEepromFromFileToolStripMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.fileToolStripMenuItem, "fileToolStripMenuItem");
      this.saveMeterEepromToFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.workMeterToolStripMenuItem1,
        (ToolStripItem) this.dbMeterToolStripMenuItem1,
        (ToolStripItem) this.typeMeterToolStripMenuItem1,
        (ToolStripItem) this.readMeterToolStripMenuItem1
      });
      this.saveMeterEepromToFileToolStripMenuItem.Name = "saveMeterEepromToFileToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.saveMeterEepromToFileToolStripMenuItem, "saveMeterEepromToFileToolStripMenuItem");
      this.workMeterToolStripMenuItem1.Name = "workMeterToolStripMenuItem1";
      componentResourceManager.ApplyResources((object) this.workMeterToolStripMenuItem1, "workMeterToolStripMenuItem1");
      this.workMeterToolStripMenuItem1.Click += new System.EventHandler(this.workMeterToolStripMenuItem1_Click);
      this.dbMeterToolStripMenuItem1.Name = "dbMeterToolStripMenuItem1";
      componentResourceManager.ApplyResources((object) this.dbMeterToolStripMenuItem1, "dbMeterToolStripMenuItem1");
      this.dbMeterToolStripMenuItem1.Click += new System.EventHandler(this.dbMeterToolStripMenuItem1_Click);
      this.typeMeterToolStripMenuItem1.Name = "typeMeterToolStripMenuItem1";
      componentResourceManager.ApplyResources((object) this.typeMeterToolStripMenuItem1, "typeMeterToolStripMenuItem1");
      this.typeMeterToolStripMenuItem1.Click += new System.EventHandler(this.typeMeterToolStripMenuItem1_Click);
      this.readMeterToolStripMenuItem1.Name = "readMeterToolStripMenuItem1";
      componentResourceManager.ApplyResources((object) this.readMeterToolStripMenuItem1, "readMeterToolStripMenuItem1");
      this.readMeterToolStripMenuItem1.Click += new System.EventHandler(this.readMeterToolStripMenuItem1_Click);
      this.loadMeterEepromFromFileToolStripMenuItem.Name = "loadMeterEepromFromFileToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.loadMeterEepromFromFileToolStripMenuItem, "loadMeterEepromFromFileToolStripMenuItem");
      this.loadMeterEepromFromFileToolStripMenuItem.Click += new System.EventHandler(this.loadMeterEepromFromFileToolStripMenuItem_Click);
      this.workToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.MenuItemCloseMeter,
        (ToolStripItem) this.MenuItemRamTest,
        (ToolStripItem) this.MenuItemRepare,
        (ToolStripItem) this.saveMeterToolStripMenuItem,
        (ToolStripItem) this.resetAllDataToolStripMenuItem,
        (ToolStripItem) this.MenuItemDestroyBackupChecksum,
        (ToolStripItem) this.MenuItemSetWriteProtectionAtClonedDevice
      });
      this.workToolStripMenuItem.Name = "workToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.workToolStripMenuItem, "workToolStripMenuItem");
      this.MenuItemCloseMeter.Name = "MenuItemCloseMeter";
      componentResourceManager.ApplyResources((object) this.MenuItemCloseMeter, "MenuItemCloseMeter");
      this.MenuItemCloseMeter.Click += new System.EventHandler(this.MenuItemCloseMeter_Click);
      this.MenuItemRamTest.Name = "MenuItemRamTest";
      componentResourceManager.ApplyResources((object) this.MenuItemRamTest, "MenuItemRamTest");
      this.MenuItemRamTest.Click += new System.EventHandler(this.MenuItemRamTest_Click);
      this.MenuItemRepare.Name = "MenuItemRepare";
      componentResourceManager.ApplyResources((object) this.MenuItemRepare, "MenuItemRepare");
      this.MenuItemRepare.Click += new System.EventHandler(this.MenuItemRepare_Click);
      this.saveMeterToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.workMeterToolStripMenuItem,
        (ToolStripItem) this.dbMeterToolStripMenuItem,
        (ToolStripItem) this.typeMeterToolStripMenuItem,
        (ToolStripItem) this.readMeterToolStripMenuItem,
        (ToolStripItem) this.connectedMeterToolStripMenuItem
      });
      this.saveMeterToolStripMenuItem.Name = "saveMeterToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.saveMeterToolStripMenuItem, "saveMeterToolStripMenuItem");
      this.workMeterToolStripMenuItem.Name = "workMeterToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.workMeterToolStripMenuItem, "workMeterToolStripMenuItem");
      this.workMeterToolStripMenuItem.Click += new System.EventHandler(this.workMeterToolStripMenuItem_Click);
      this.dbMeterToolStripMenuItem.Name = "dbMeterToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.dbMeterToolStripMenuItem, "dbMeterToolStripMenuItem");
      this.dbMeterToolStripMenuItem.Click += new System.EventHandler(this.dbMeterToolStripMenuItem_Click);
      this.typeMeterToolStripMenuItem.Name = "typeMeterToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.typeMeterToolStripMenuItem, "typeMeterToolStripMenuItem");
      this.typeMeterToolStripMenuItem.Click += new System.EventHandler(this.typeMeterToolStripMenuItem_Click);
      this.readMeterToolStripMenuItem.Name = "readMeterToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.readMeterToolStripMenuItem, "readMeterToolStripMenuItem");
      this.readMeterToolStripMenuItem.Click += new System.EventHandler(this.readMeterToolStripMenuItem_Click);
      this.connectedMeterToolStripMenuItem.Name = "connectedMeterToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.connectedMeterToolStripMenuItem, "connectedMeterToolStripMenuItem");
      this.connectedMeterToolStripMenuItem.Click += new System.EventHandler(this.connectedMeterToolStripMenuItem_Click);
      this.resetAllDataToolStripMenuItem.Name = "resetAllDataToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.resetAllDataToolStripMenuItem, "resetAllDataToolStripMenuItem");
      this.resetAllDataToolStripMenuItem.Click += new System.EventHandler(this.resetAllDataToolStripMenuItem_Click);
      this.MenuItemDestroyBackupChecksum.Name = "MenuItemDestroyBackupChecksum";
      componentResourceManager.ApplyResources((object) this.MenuItemDestroyBackupChecksum, "MenuItemDestroyBackupChecksum");
      this.MenuItemDestroyBackupChecksum.Click += new System.EventHandler(this.MenuItemDestroyBackupChecksum_Click);
      this.MenuItemSetWriteProtectionAtClonedDevice.Name = "MenuItemSetWriteProtectionAtClonedDevice";
      componentResourceManager.ApplyResources((object) this.MenuItemSetWriteProtectionAtClonedDevice, "MenuItemSetWriteProtectionAtClonedDevice");
      this.MenuItemSetWriteProtectionAtClonedDevice.Click += new System.EventHandler(this.MenuItemSetWriteProtectionAtClonedDevice_Click);
      this.showToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[25]
      {
        (ToolStripItem) this.typeAnalysisWindowToolStripMenuItem,
        (ToolStripItem) this.MenuItemErr8002Types,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.blockListToolStripMenuItem,
        (ToolStripItem) this.blockListDifferencesToolStripMenuItem,
        (ToolStripItem) this.blockListDifferencesEEPromDirectToolStripMenuItem,
        (ToolStripItem) this.blockListDifferencesFromFoadedEepromToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.parametersToolStripMenuItem,
        (ToolStripItem) this.epromDifferenceToolStripMenuItem,
        (ToolStripItem) this.parameterDifferenceToolStripMenuItem,
        (ToolStripItem) this.epromParameterByAddressToolStripMenuItem,
        (ToolStripItem) this.ramParameterByAddressToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.MenuItemIsEquelResult,
        (ToolStripItem) this.MenuItemGetListList,
        (ToolStripItem) this.MenuItemShowMeterResources,
        (ToolStripItem) this.MenuItemGetRendomNumber,
        (ToolStripItem) this.testLoggerEntriesToolStripMenuItem,
        (ToolStripItem) this.areParametersEqualToDBoverridesToolStripMenuItem,
        (ToolStripItem) this.MenuItemIsEndTimeOk,
        (ToolStripItem) this.toolStripSeparator5,
        (ToolStripItem) this.MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter,
        (ToolStripItem) this.MenuItemSetMaxValuesCritical,
        (ToolStripItem) this.MenuItemShowDeviceTimeEvents
      });
      this.showToolStripMenuItem.Name = "showToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.showToolStripMenuItem, "showToolStripMenuItem");
      this.typeAnalysisWindowToolStripMenuItem.Name = "typeAnalysisWindowToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.typeAnalysisWindowToolStripMenuItem, "typeAnalysisWindowToolStripMenuItem");
      this.typeAnalysisWindowToolStripMenuItem.Click += new System.EventHandler(this.typeAnalysisWindowToolStripMenuItem_Click);
      this.MenuItemErr8002Types.Name = "MenuItemErr8002Types";
      componentResourceManager.ApplyResources((object) this.MenuItemErr8002Types, "MenuItemErr8002Types");
      this.MenuItemErr8002Types.Click += new System.EventHandler(this.MenuItemErr8002Types_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator2, "toolStripSeparator2");
      this.blockListToolStripMenuItem.Name = "blockListToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.blockListToolStripMenuItem, "blockListToolStripMenuItem");
      this.blockListToolStripMenuItem.Click += new System.EventHandler(this.blockListToolStripMenuItem_Click);
      this.blockListDifferencesToolStripMenuItem.Name = "blockListDifferencesToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.blockListDifferencesToolStripMenuItem, "blockListDifferencesToolStripMenuItem");
      this.blockListDifferencesToolStripMenuItem.Click += new System.EventHandler(this.blockListDifferencesToolStripMenuItem_Click);
      this.blockListDifferencesEEPromDirectToolStripMenuItem.Name = "blockListDifferencesEEPromDirectToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.blockListDifferencesEEPromDirectToolStripMenuItem, "blockListDifferencesEEPromDirectToolStripMenuItem");
      this.blockListDifferencesEEPromDirectToolStripMenuItem.Click += new System.EventHandler(this.blockListDifferencesEEPromDirectToolStripMenuItem_Click);
      this.blockListDifferencesFromFoadedEepromToolStripMenuItem.Name = "blockListDifferencesFromFoadedEepromToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.blockListDifferencesFromFoadedEepromToolStripMenuItem, "blockListDifferencesFromFoadedEepromToolStripMenuItem");
      this.blockListDifferencesFromFoadedEepromToolStripMenuItem.Click += new System.EventHandler(this.blockListDifferencesFromFoadedEepromToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator3, "toolStripSeparator3");
      this.parametersToolStripMenuItem.Name = "parametersToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.parametersToolStripMenuItem, "parametersToolStripMenuItem");
      this.parametersToolStripMenuItem.Click += new System.EventHandler(this.parametersToolStripMenuItem_Click);
      this.epromDifferenceToolStripMenuItem.Name = "epromDifferenceToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.epromDifferenceToolStripMenuItem, "epromDifferenceToolStripMenuItem");
      this.epromDifferenceToolStripMenuItem.Click += new System.EventHandler(this.epromDifferenceToolStripMenuItem_Click);
      this.parameterDifferenceToolStripMenuItem.Name = "parameterDifferenceToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.parameterDifferenceToolStripMenuItem, "parameterDifferenceToolStripMenuItem");
      this.parameterDifferenceToolStripMenuItem.Click += new System.EventHandler(this.parameterDifferenceToolStripMenuItem_Click);
      this.epromParameterByAddressToolStripMenuItem.Name = "epromParameterByAddressToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.epromParameterByAddressToolStripMenuItem, "epromParameterByAddressToolStripMenuItem");
      this.epromParameterByAddressToolStripMenuItem.Click += new System.EventHandler(this.epromParameterByAddressToolStripMenuItem_Click);
      this.ramParameterByAddressToolStripMenuItem.Name = "ramParameterByAddressToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.ramParameterByAddressToolStripMenuItem, "ramParameterByAddressToolStripMenuItem");
      this.ramParameterByAddressToolStripMenuItem.Click += new System.EventHandler(this.ramParameterByAddressToolStripMenuItem_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator4, "toolStripSeparator4");
      this.MenuItemIsEquelResult.Name = "MenuItemIsEquelResult";
      componentResourceManager.ApplyResources((object) this.MenuItemIsEquelResult, "MenuItemIsEquelResult");
      this.MenuItemIsEquelResult.Click += new System.EventHandler(this.MenuItemIsEquelResult_Click);
      this.MenuItemGetListList.Name = "MenuItemGetListList";
      componentResourceManager.ApplyResources((object) this.MenuItemGetListList, "MenuItemGetListList");
      this.MenuItemGetListList.Click += new System.EventHandler(this.MenuItemGetListList_Click);
      this.MenuItemShowMeterResources.Name = "MenuItemShowMeterResources";
      componentResourceManager.ApplyResources((object) this.MenuItemShowMeterResources, "MenuItemShowMeterResources");
      this.MenuItemShowMeterResources.Click += new System.EventHandler(this.MenuItemShowMeterResources_Click);
      this.MenuItemGetRendomNumber.Name = "MenuItemGetRendomNumber";
      componentResourceManager.ApplyResources((object) this.MenuItemGetRendomNumber, "MenuItemGetRendomNumber");
      this.MenuItemGetRendomNumber.Click += new System.EventHandler(this.MenuItemGetRendomNumber_Click);
      this.testLoggerEntriesToolStripMenuItem.Name = "testLoggerEntriesToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.testLoggerEntriesToolStripMenuItem, "testLoggerEntriesToolStripMenuItem");
      this.testLoggerEntriesToolStripMenuItem.Click += new System.EventHandler(this.testLoggerEntriesToolStripMenuItem_Click);
      this.areParametersEqualToDBoverridesToolStripMenuItem.Name = "areParametersEqualToDBoverridesToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.areParametersEqualToDBoverridesToolStripMenuItem, "areParametersEqualToDBoverridesToolStripMenuItem");
      this.areParametersEqualToDBoverridesToolStripMenuItem.Click += new System.EventHandler(this.areParametersEqualToDBoverridesToolStripMenuItem_Click);
      this.MenuItemIsEndTimeOk.Name = "MenuItemIsEndTimeOk";
      componentResourceManager.ApplyResources((object) this.MenuItemIsEndTimeOk, "MenuItemIsEndTimeOk");
      this.MenuItemIsEndTimeOk.Click += new System.EventHandler(this.MenuItemIsEndTimeOk_Click);
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator5, "toolStripSeparator5");
      this.MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter.Name = "MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter";
      componentResourceManager.ApplyResources((object) this.MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter, "MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter");
      this.MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter.Click += new System.EventHandler(this.MenuItemStaticDiffFromDiagnosticObjectToExternalConnectedMeter_Click);
      this.MenuItemSetMaxValuesCritical.Name = "MenuItemSetMaxValuesCritical";
      componentResourceManager.ApplyResources((object) this.MenuItemSetMaxValuesCritical, "MenuItemSetMaxValuesCritical");
      this.MenuItemSetMaxValuesCritical.Click += new System.EventHandler(this.MenuItemSetMaxValuesCritical_Click);
      this.MenuItemShowDeviceTimeEvents.Name = "MenuItemShowDeviceTimeEvents";
      componentResourceManager.ApplyResources((object) this.MenuItemShowDeviceTimeEvents, "MenuItemShowDeviceTimeEvents");
      this.MenuItemShowDeviceTimeEvents.Click += new System.EventHandler(this.MenuItemShowDeviceTimeEvents_Click);
      this.goToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.globalMeterManagerToolStripMenuItem,
        (ToolStripItem) this.backToolStripMenuItem,
        (ToolStripItem) this.quitToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.designerToolStripMenuItem,
        (ToolStripItem) this.serialBusToolStripMenuItem,
        (ToolStripItem) this.asyncComToolStripMenuItem
      });
      this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.goToToolStripMenuItem, "goToToolStripMenuItem");
      this.globalMeterManagerToolStripMenuItem.Name = "globalMeterManagerToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.globalMeterManagerToolStripMenuItem, "globalMeterManagerToolStripMenuItem");
      this.globalMeterManagerToolStripMenuItem.Click += new System.EventHandler(this.globalMeterManagerToolStripMenuItem_Click);
      this.backToolStripMenuItem.Name = "backToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.backToolStripMenuItem, "backToolStripMenuItem");
      this.backToolStripMenuItem.Click += new System.EventHandler(this.backToolStripMenuItem_Click);
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.quitToolStripMenuItem, "quitToolStripMenuItem");
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator1, "toolStripSeparator1");
      this.designerToolStripMenuItem.Name = "designerToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.designerToolStripMenuItem, "designerToolStripMenuItem");
      this.designerToolStripMenuItem.Click += new System.EventHandler(this.designerToolStripMenuItem_Click);
      this.serialBusToolStripMenuItem.Name = "serialBusToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.serialBusToolStripMenuItem, "serialBusToolStripMenuItem");
      this.serialBusToolStripMenuItem.Click += new System.EventHandler(this.serialBusToolStripMenuItem_Click);
      this.asyncComToolStripMenuItem.Name = "asyncComToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.asyncComToolStripMenuItem, "asyncComToolStripMenuItem");
      this.asyncComToolStripMenuItem.Click += new System.EventHandler(this.asyncComToolStripMenuItem_Click);
      this.dataToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.CopyTempSensorCalibrationToolStripMenuItem,
        (ToolStripItem) this.PastTempSensorCalibrationToolStripMenuItem
      });
      this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.dataToolStripMenuItem, "dataToolStripMenuItem");
      this.CopyTempSensorCalibrationToolStripMenuItem.Name = "CopyTempSensorCalibrationToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.CopyTempSensorCalibrationToolStripMenuItem, "CopyTempSensorCalibrationToolStripMenuItem");
      this.CopyTempSensorCalibrationToolStripMenuItem.Click += new System.EventHandler(this.CopyTempSensorCalibrationToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.PastTempSensorCalibrationToolStripMenuItem, "PastTempSensorCalibrationToolStripMenuItem");
      this.PastTempSensorCalibrationToolStripMenuItem.Name = "PastTempSensorCalibrationToolStripMenuItem";
      this.PastTempSensorCalibrationToolStripMenuItem.Click += new System.EventHandler(this.PastTempSensorCalibrationToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.buttonCancle, "buttonCancle");
      this.buttonCancle.DialogResult = DialogResult.Cancel;
      this.buttonCancle.Name = "buttonCancle";
      this.buttonCancle.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.buttonOk, "buttonOk");
      this.buttonOk.DialogResult = DialogResult.OK;
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.checkBoxShowDiffsOnly);
      this.groupBox1.Controls.Add((Control) this.checkBoxShowBlockTypes);
      this.groupBox1.Controls.Add((Control) this.checkBoxShowFunctionNames);
      this.groupBox1.Controls.Add((Control) this.checkBoxShowFunctionNumbers);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.comboBoxGetListSelection);
      this.groupBox1.Controls.Add((Control) this.comboBoxEquelFunction);
      this.groupBox1.Controls.Add((Control) this.comboBoxCompareObject);
      this.groupBox1.Controls.Add((Control) this.comboBoxDiagnosticObject);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.checkBoxShowDiffsOnly, "checkBoxShowDiffsOnly");
      this.checkBoxShowDiffsOnly.Name = "checkBoxShowDiffsOnly";
      this.checkBoxShowDiffsOnly.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.checkBoxShowBlockTypes, "checkBoxShowBlockTypes");
      this.checkBoxShowBlockTypes.Name = "checkBoxShowBlockTypes";
      this.checkBoxShowBlockTypes.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.checkBoxShowFunctionNames, "checkBoxShowFunctionNames");
      this.checkBoxShowFunctionNames.Name = "checkBoxShowFunctionNames";
      this.checkBoxShowFunctionNames.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.checkBoxShowFunctionNumbers, "checkBoxShowFunctionNumbers");
      this.checkBoxShowFunctionNumbers.Name = "checkBoxShowFunctionNumbers";
      this.checkBoxShowFunctionNumbers.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.comboBoxGetListSelection.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxGetListSelection.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBoxGetListSelection, "comboBoxGetListSelection");
      this.comboBoxGetListSelection.Name = "comboBoxGetListSelection";
      this.comboBoxEquelFunction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxEquelFunction.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBoxEquelFunction, "comboBoxEquelFunction");
      this.comboBoxEquelFunction.Name = "comboBoxEquelFunction";
      this.comboBoxCompareObject.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBoxCompareObject, "comboBoxCompareObject");
      this.comboBoxCompareObject.Name = "comboBoxCompareObject";
      this.comboBoxDiagnosticObject.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBoxDiagnosticObject, "comboBoxDiagnosticObject");
      this.comboBoxDiagnosticObject.Name = "comboBoxDiagnosticObject";
      componentResourceManager.ApplyResources((object) this.buttonBreakLoop, "buttonBreakLoop");
      this.buttonBreakLoop.Name = "buttonBreakLoop";
      this.buttonBreakLoop.UseVisualStyleBackColor = true;
      this.buttonBreakLoop.Click += new System.EventHandler(this.buttonBreakLoop_Click);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Controls.Add((Control) this.checkBoxBackupForEachRead);
      this.panel1.Controls.Add((Control) this.checkBoxReadWithoutBackup);
      this.panel1.Controls.Add((Control) this.checkBoxIgnorIntervalMinutesRaster);
      this.panel1.Controls.Add((Control) this.checkBoxDisableChecks);
      this.panel1.Controls.Add((Control) this.checkBoxUseOnlyDefaultValues);
      this.panel1.Controls.Add((Control) this.groupBoxVars);
      this.panel1.Controls.Add((Control) this.textBoxStatus);
      this.panel1.Controls.Add((Control) this.groupBox1);
      this.panel1.Controls.Add((Control) this.buttonBreakLoop);
      this.panel1.Controls.Add((Control) this.buttonOk);
      this.panel1.Controls.Add((Control) this.buttonCancle);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.checkBoxBackupForEachRead, "checkBoxBackupForEachRead");
      this.checkBoxBackupForEachRead.Name = "checkBoxBackupForEachRead";
      this.checkBoxBackupForEachRead.UseVisualStyleBackColor = true;
      this.checkBoxBackupForEachRead.CheckedChanged += new System.EventHandler(this.checkBoxBackupForEachRead_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxReadWithoutBackup, "checkBoxReadWithoutBackup");
      this.checkBoxReadWithoutBackup.Name = "checkBoxReadWithoutBackup";
      this.checkBoxReadWithoutBackup.UseVisualStyleBackColor = true;
      this.checkBoxReadWithoutBackup.CheckedChanged += new System.EventHandler(this.checkBoxReadWithoutBackup_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxIgnorIntervalMinutesRaster, "checkBoxIgnorIntervalMinutesRaster");
      this.checkBoxIgnorIntervalMinutesRaster.Name = "checkBoxIgnorIntervalMinutesRaster";
      this.checkBoxIgnorIntervalMinutesRaster.UseVisualStyleBackColor = true;
      this.checkBoxIgnorIntervalMinutesRaster.CheckedChanged += new System.EventHandler(this.checkBoxIgnorIntervalMinutesRaster_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxDisableChecks, "checkBoxDisableChecks");
      this.checkBoxDisableChecks.Name = "checkBoxDisableChecks";
      this.checkBoxDisableChecks.UseVisualStyleBackColor = true;
      this.checkBoxDisableChecks.CheckedChanged += new System.EventHandler(this.checkBoxDisableChecks_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBoxUseOnlyDefaultValues, "checkBoxUseOnlyDefaultValues");
      this.checkBoxUseOnlyDefaultValues.Name = "checkBoxUseOnlyDefaultValues";
      this.checkBoxUseOnlyDefaultValues.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.groupBoxVars, "groupBoxVars");
      this.groupBoxVars.Controls.Add((Control) this.listBoxVariables);
      this.groupBoxVars.Controls.Add((Control) this.buttonReadVar);
      this.groupBoxVars.Controls.Add((Control) this.buttonWriteToRam);
      this.groupBoxVars.Controls.Add((Control) this.buttonWriteToEprom);
      this.groupBoxVars.Controls.Add((Control) this.buttonChangeData);
      this.groupBoxVars.Controls.Add((Control) this.label7);
      this.groupBoxVars.Controls.Add((Control) this.labelValueRam);
      this.groupBoxVars.Controls.Add((Control) this.label8);
      this.groupBoxVars.Controls.Add((Control) this.labelValueEProm);
      this.groupBoxVars.Controls.Add((Control) this.textBoxNewValue);
      this.groupBoxVars.Controls.Add((Control) this.textBoxValueRam);
      this.groupBoxVars.Controls.Add((Control) this.textBoxByteSize);
      this.groupBoxVars.Controls.Add((Control) this.textBoxValueEprom);
      this.groupBoxVars.Name = "groupBoxVars";
      this.groupBoxVars.TabStop = false;
      componentResourceManager.ApplyResources((object) this.listBoxVariables, "listBoxVariables");
      this.listBoxVariables.Name = "listBoxVariables";
      this.listBoxVariables.SelectedIndexChanged += new System.EventHandler(this.listBoxVariables_SelectedIndexChanged_1);
      this.listBoxVariables.KeyDown += new KeyEventHandler(this.listBoxVariables_KeyDown);
      componentResourceManager.ApplyResources((object) this.buttonReadVar, "buttonReadVar");
      this.buttonReadVar.Name = "buttonReadVar";
      this.buttonReadVar.UseVisualStyleBackColor = true;
      this.buttonReadVar.Click += new System.EventHandler(this.buttonReadVar_Click);
      componentResourceManager.ApplyResources((object) this.buttonWriteToRam, "buttonWriteToRam");
      this.buttonWriteToRam.Name = "buttonWriteToRam";
      this.buttonWriteToRam.UseVisualStyleBackColor = true;
      this.buttonWriteToRam.Click += new System.EventHandler(this.buttonWriteToRam_Click);
      componentResourceManager.ApplyResources((object) this.buttonWriteToEprom, "buttonWriteToEprom");
      this.buttonWriteToEprom.Name = "buttonWriteToEprom";
      this.buttonWriteToEprom.UseVisualStyleBackColor = true;
      this.buttonWriteToEprom.Click += new System.EventHandler(this.buttonWriteToEprom_Click);
      componentResourceManager.ApplyResources((object) this.buttonChangeData, "buttonChangeData");
      this.buttonChangeData.Name = "buttonChangeData";
      this.buttonChangeData.UseVisualStyleBackColor = true;
      this.buttonChangeData.Click += new System.EventHandler(this.buttonChangeData_Click);
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.labelValueRam, "labelValueRam");
      this.labelValueRam.Name = "labelValueRam";
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this.labelValueEProm, "labelValueEProm");
      this.labelValueEProm.Name = "labelValueEProm";
      componentResourceManager.ApplyResources((object) this.textBoxNewValue, "textBoxNewValue");
      this.textBoxNewValue.Name = "textBoxNewValue";
      componentResourceManager.ApplyResources((object) this.textBoxValueRam, "textBoxValueRam");
      this.textBoxValueRam.Name = "textBoxValueRam";
      componentResourceManager.ApplyResources((object) this.textBoxByteSize, "textBoxByteSize");
      this.textBoxByteSize.Name = "textBoxByteSize";
      componentResourceManager.ApplyResources((object) this.textBoxValueEprom, "textBoxValueEprom");
      this.textBoxValueEprom.Name = "textBoxValueEprom";
      componentResourceManager.ApplyResources((object) this.textBoxStatus, "textBoxStatus");
      this.textBoxStatus.Name = "textBoxStatus";
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Controls.Add((Control) this.zennerCoroprateDesign1);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.openFileDialog1.FileName = "openFileDialog1";
      this.saveFileDialog1.DefaultExt = "txt";
      this.saveFileDialog1.FileName = "MeterEEProm";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (HandlerWindow);
      this.Load += new System.EventHandler(this.HandlerWindow_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBoxVars.ResumeLayout(false);
      this.groupBoxVars.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
