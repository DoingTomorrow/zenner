// Decompiled with JetBrains decompiler
// Type: S3_Handler.MeterObjectComparer
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class MeterObjectComparer : Form
  {
    private S3_ParameterNames[] importendConfigParameters = new S3_ParameterNames[93]
    {
      S3_ParameterNames.Con_HardwareTypeId,
      S3_ParameterNames.Con_BaseTypeId,
      S3_ParameterNames.Con_MeterTypeId,
      S3_ParameterNames.Con_SAP_MaterialNumber,
      S3_ParameterNames.Con_SerialNumber,
      S3_ParameterNames.Con_Manufacturer,
      S3_ParameterNames.Con_Medium_Generation,
      S3_ParameterNames.Con_SelectedList_PrimaryAddress,
      S3_ParameterNames.ApprovalRevison,
      S3_ParameterNames.Bak_VolSum,
      S3_ParameterNames.Vol_VolDisplay,
      S3_ParameterNames.Bak_HeatEnergySum,
      S3_ParameterNames.Energy_HeatEnergyDisplay,
      S3_ParameterNames.Bak_ColdEnergySum,
      S3_ParameterNames.Energy_ColdEnergyDisplay,
      S3_ParameterNames.Bak_Tariff0EnergySum,
      S3_ParameterNames.Energy_Tariff0EnergyDisplay,
      S3_ParameterNames.Bak_Tariff1EnergySum,
      S3_ParameterNames.Energy_Tariff1EnergyDisplay,
      S3_ParameterNames.Cal_DisplayInput_n_0,
      S3_ParameterNames.Cal_DisplayInput_n_1,
      S3_ParameterNames.Cal_DisplayInput_n_2,
      S3_ParameterNames.Cal_DisplayRestInput_n_0,
      S3_ParameterNames.Cal_DisplayRestInput_n_1,
      S3_ParameterNames.Cal_DisplayRestInput_n_2,
      S3_ParameterNames.Bak_TimeZoneInQuarterHours,
      S3_ParameterNames.Con_TdcVolMeasuringCycleTime,
      S3_ParameterNames.Con_FastCycleVolThreshold,
      S3_ParameterNames.volumeCycleTimeCounterInit,
      S3_ParameterNames.temperaturCycleTimeCounterInit,
      S3_ParameterNames.temperaturCycleTimeSlotCounterInit,
      S3_ParameterNames.fastCycleOffCounterInit,
      S3_ParameterNames.Con_EnergyFactor,
      S3_ParameterNames.Con_CalPowerFactor,
      S3_ParameterNames.Device_Setup,
      S3_ParameterNames.Device_Setup_2,
      S3_ParameterNames.Con_Waerme_Grenze_DeltaT_min,
      S3_ParameterNames.Con_Kaelte_Grenze_DeltaT_min,
      S3_ParameterNames.Con_HeatThreshold,
      S3_ParameterNames.Con_Feste_Fuehlertemperatur_VL,
      S3_ParameterNames.Con_Feste_Fuehlertemperatur_RL,
      S3_ParameterNames.Con_Temp_Display_Range_Max,
      S3_ParameterNames.Con_Temp_Display_Range_Min,
      S3_ParameterNames.Con_Adc_Low_Bat_Error_Treshold,
      S3_ParameterNames.Con_Tariff_RefTemp,
      S3_ParameterNames.energyUnitIndex,
      S3_ParameterNames.powerUnitIndex,
      S3_ParameterNames.Con_VolFactor,
      S3_ParameterNames.Con_CalFlowFactor,
      S3_ParameterNames.volumeUnitIndex,
      S3_ParameterNames.flowUnitIndex,
      S3_ParameterNames.SerDev0_IdentNo,
      S3_ParameterNames.SerDev0_Manufacturer,
      S3_ParameterNames.SerDev0_Medium_Generation,
      S3_ParameterNames.SerDev0_SelectedList_PrimaryAddress,
      S3_ParameterNames.SerDev0_SelectedList,
      S3_ParameterNames.SerDev0_RadioId,
      S3_ParameterNames.SerDev1_IdentNo,
      S3_ParameterNames.SerDev1_Manufacturer,
      S3_ParameterNames.SerDev1_Medium_Generation,
      S3_ParameterNames.SerDev1_SelectedList_PrimaryAddress,
      S3_ParameterNames.SerDev1_SelectedList,
      S3_ParameterNames.SerDev1_RadioId,
      S3_ParameterNames.SerDev2_IdentNo,
      S3_ParameterNames.SerDev2_Manufacturer,
      S3_ParameterNames.SerDev2_Medium_Generation,
      S3_ParameterNames.SerDev2_SelectedList_PrimaryAddress,
      S3_ParameterNames.SerDev2_SelectedList,
      S3_ParameterNames.SerDev2_RadioId,
      S3_ParameterNames.SerDev3_IdentNo,
      S3_ParameterNames.SerDev3_Manufacturer,
      S3_ParameterNames.SerDev3_Medium_Generation,
      S3_ParameterNames.SerDev3_SelectedList_PrimaryAddress,
      S3_ParameterNames.SerDev3_SelectedList,
      S3_ParameterNames.SerDev3_RadioId,
      S3_ParameterNames.PulsOutFrequence,
      S3_ParameterNames.Cal_FaktorInput_n_0,
      S3_ParameterNames.Cal_FaktorInput_n_1,
      S3_ParameterNames.Cal_FaktorInput_n_2,
      S3_ParameterNames.Cal_TeilerFaktorInput_n_0,
      S3_ParameterNames.Cal_TeilerFaktorInput_n_1,
      S3_ParameterNames.Cal_TeilerFaktorInput_n_2,
      S3_ParameterNames.input0UnitIndex,
      S3_ParameterNames.input1UnitIndex,
      S3_ParameterNames.input2UnitIndex,
      S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_0,
      S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_1,
      S3_ParameterNames.Con_cal_tf_a0_0,
      S3_ParameterNames.Con_cal_tf_a0_1,
      S3_ParameterNames.Con_cal_tf_a1_0,
      S3_ParameterNames.Con_cal_tf_a1_1,
      S3_ParameterNames.Con_cal_tf_a2_0,
      S3_ParameterNames.Con_cal_tf_a2_1
    };
    internal static MeterObjectComparer TheOnlyMeterComparer;
    internal Thread ComparerThread;
    internal Queue<CompareInfo> NewInfoQueue = new Queue<CompareInfo>();
    private S3_HandlerFunctions MyFunctions;
    private List<CompareInfo> compareList;
    private StringBuilder diffInfo = new StringBuilder();
    private DiagnoseWindow diagWindow;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private DataGridView dataGridView1;
    private CheckBox checkBoxUseWinDiff;
    private CheckBox checkBoxSuppressLineAddresses;
    private Button buttonClear;
    private DataGridViewTextBoxColumn Index;
    private DataGridViewTextBoxColumn Info;
    private DataGridViewTextBoxColumn SapSource;
    private DataGridViewTextBoxColumn SourceDate;
    private DataGridViewTextBoxColumn CompareDate;
    private DataGridViewCheckBoxColumn MapIsEqual;
    private DataGridViewCheckBoxColumn ImportantParamEqual;
    private DataGridViewTextBoxColumn FunctionDiff;
    private DataGridViewTextBoxColumn ChangedParams;
    private DataGridViewTextBoxColumn SegmentsDiff;

    internal static bool CompareObjects(
      string compareInfo,
      S3_HandlerFunctions MyFunctions,
      MeterObjects srcMeterObject,
      MeterObjects destMeterObject)
    {
      MeterObjectComparer.ShowComparerWindow(MyFunctions);
      MeterObjectComparer.TheOnlyMeterComparer.NewInfoQueue.Enqueue(new CompareInfo()
      {
        info = compareInfo,
        srcMeter = MeterObjectComparer.TheOnlyMeterComparer.MyFunctions.MyMeters.GetMeterObject(srcMeterObject),
        destMeter = MeterObjectComparer.TheOnlyMeterComparer.MyFunctions.MyMeters.GetMeterObject(destMeterObject)
      });
      return true;
    }

    internal static bool CompareObjects(
      string compareInfo,
      S3_HandlerFunctions MyFunctions,
      S3_Meter srcMeter,
      S3_Meter destMeter)
    {
      MeterObjectComparer.ShowComparerWindow(MyFunctions);
      MeterObjectComparer.TheOnlyMeterComparer.NewInfoQueue.Enqueue(new CompareInfo()
      {
        info = compareInfo,
        srcMeter = srcMeter,
        destMeter = destMeter
      });
      return true;
    }

    internal static void ShowComparerWindow(S3_HandlerFunctions MyFunctions)
    {
      if (MeterObjectComparer.TheOnlyMeterComparer == null)
      {
        MeterObjectComparer.TheOnlyMeterComparer = new MeterObjectComparer(MyFunctions);
        MeterObjectComparer.TheOnlyMeterComparer.ComparerThread = new Thread((ThreadStart) (() =>
        {
          MeterObjectComparer.TheOnlyMeterComparer.Show();
          while (true)
          {
            while (MeterObjectComparer.TheOnlyMeterComparer.NewInfoQueue.Count > 0)
            {
              CompareInfo newInfo = MeterObjectComparer.TheOnlyMeterComparer.NewInfoQueue.Dequeue();
              if (newInfo != null)
                MeterObjectComparer.TheOnlyMeterComparer.CompareObjectsWork(newInfo);
              else
                MeterObjectComparer.TheOnlyMeterComparer.Show();
            }
            Thread.Sleep(100);
            Application.DoEvents();
          }
        }));
        MeterObjectComparer.TheOnlyMeterComparer.ComparerThread.Name = "Comparer";
        MeterObjectComparer.TheOnlyMeterComparer.ComparerThread.IsBackground = true;
        MeterObjectComparer.TheOnlyMeterComparer.ComparerThread.SetApartmentState(ApartmentState.STA);
        MeterObjectComparer.TheOnlyMeterComparer.ComparerThread.Start();
      }
      else
        MeterObjectComparer.TheOnlyMeterComparer.NewInfoQueue.Enqueue((CompareInfo) null);
    }

    public MeterObjectComparer(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.compareList = new List<CompareInfo>();
    }

    private bool CompareObjectsWork(CompareInfo newInfo)
    {
      int rowIndex = this.dataGridView1.Rows.Add();
      this.dataGridView1["Index", rowIndex].Value = (object) this.compareList.Count.ToString();
      this.dataGridView1["Info", rowIndex].Value = (object) newInfo.info;
      this.dataGridView1["SapSource", rowIndex].Value = (object) newInfo.srcMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_MaterialNumber.ToString()].GetUintValue().ToString();
      this.dataGridView1["SourceDate", rowIndex].Value = (object) newInfo.srcMeter.MeterObjectGeneratedTimeStamp.ToString("dd.MM.yy HH:mm:ss");
      this.dataGridView1["CompareDate", rowIndex].Value = (object) newInfo.destMeter.MeterObjectGeneratedTimeStamp.ToString("dd.MM.yy HH:mm:ss");
      this.dataGridView1["MapIsEqual", rowIndex].Value = (object) this.IsMapqual(newInfo);
      this.dataGridView1["ImportantParamEqual", rowIndex].Value = (object) this.AreAllImportantParametersEqual(newInfo);
      this.dataGridView1["FunctionDiff", rowIndex].Value = (object) this.GetFunctionDiff(newInfo);
      this.dataGridView1["ChangedParams", rowIndex].Value = (object) this.GetChangedImportantParameters(newInfo);
      this.dataGridView1["SegmentsDiff", rowIndex].Value = (object) this.FindDifferentSegments(newInfo);
      this.compareList.Add(newInfo);
      return true;
    }

    private bool IsMapqual(CompareInfo newInfo)
    {
      return this.IsMemoryBlockEqual(newInfo.srcMeter.MyDeviceMemory.meterMemory, newInfo.destMeter.MyDeviceMemory.meterMemory);
    }

    private bool IsMemoryBlockEqual(S3_MemoryBlock srcMemoryBlock, S3_MemoryBlock destMemoryBlock)
    {
      if ((srcMemoryBlock.ByteSize == destMemoryBlock.ByteSize || destMemoryBlock.SegmentType == S3_MemorySegment.CompleteMeter) && srcMemoryBlock.BlockStartAddress == destMemoryBlock.BlockStartAddress)
      {
        if (srcMemoryBlock.childMemoryBlocks == null)
        {
          if (destMemoryBlock.childMemoryBlocks != null)
            goto label_9;
        }
        else if (destMemoryBlock.childMemoryBlocks != null && srcMemoryBlock.childMemoryBlocks.Count == destMemoryBlock.childMemoryBlocks.Count)
        {
          for (int index = 0; index < srcMemoryBlock.childMemoryBlocks.Count; ++index)
          {
            if (!this.IsMemoryBlockEqual(srcMemoryBlock.childMemoryBlocks[index], destMemoryBlock.childMemoryBlocks[index]))
              goto label_9;
          }
        }
        else
          goto label_9;
        return true;
      }
label_9:
      return false;
    }

    private bool AreAllImportantParametersEqual(CompareInfo newInfo)
    {
      SortedList<string, S3_Parameter> parameterByName1 = newInfo.srcMeter.MyParameters.ParameterByName;
      SortedList<string, S3_Parameter> parameterByName2 = newInfo.destMeter.MyParameters.ParameterByName;
      bool flag = true;
      for (int index1 = 0; index1 < this.importendConfigParameters.Length; ++index1)
      {
        int index2 = parameterByName1.IndexOfKey(this.importendConfigParameters[index1].ToString());
        int index3 = parameterByName2.IndexOfKey(this.importendConfigParameters[index1].ToString());
        if (index2 < 0 || index3 < 0)
        {
          flag = false;
        }
        else
        {
          int blockStartAddress = parameterByName1.Values[index2].BlockStartAddress;
          int byteSize = parameterByName1.Values[index2].ByteSize;
          if (parameterByName2.Values[index3].BlockStartAddress != blockStartAddress || parameterByName2.Values[index3].ByteSize != byteSize)
            flag = false;
          else if ((long) newInfo.srcMeter.MyDeviceMemory.GetUlongValue(blockStartAddress, byteSize) != (long) newInfo.destMeter.MyDeviceMemory.GetUlongValue(blockStartAddress, byteSize))
            flag = false;
        }
      }
      return flag;
    }

    private string GetChangedImportantParameters(CompareInfo newInfo)
    {
      this.diffInfo.Length = 0;
      SortedList<string, S3_Parameter> parameterByName1 = newInfo.srcMeter.MyParameters.ParameterByName;
      SortedList<string, S3_Parameter> parameterByName2 = newInfo.destMeter.MyParameters.ParameterByName;
      for (int index1 = 0; index1 < this.importendConfigParameters.Length; ++index1)
      {
        int index2 = parameterByName1.IndexOfKey(this.importendConfigParameters[index1].ToString());
        int index3 = parameterByName2.IndexOfKey(this.importendConfigParameters[index1].ToString());
        if (index2 < 0 || index3 < 0)
        {
          this.AddInfo(this.importendConfigParameters[index1].ToString() + ":missed");
        }
        else
        {
          int blockStartAddress = parameterByName1.Values[index2].BlockStartAddress;
          int byteSize = parameterByName1.Values[index2].ByteSize;
          if (parameterByName2.Values[index3].BlockStartAddress != blockStartAddress || parameterByName2.Values[index3].ByteSize != byteSize)
          {
            this.AddInfo(this.importendConfigParameters[index1].ToString() + ":size");
          }
          else
          {
            ulong ulongValue1 = newInfo.srcMeter.MyDeviceMemory.GetUlongValue(blockStartAddress, byteSize);
            ulong ulongValue2 = newInfo.destMeter.MyDeviceMemory.GetUlongValue(blockStartAddress, byteSize);
            if ((long) ulongValue1 != (long) ulongValue2)
              this.AddInfo(this.importendConfigParameters[index1].ToString() + ":" + ulongValue1.ToString() + "=>" + ulongValue2.ToString());
          }
        }
      }
      return this.diffInfo.ToString();
    }

    private void FindDifferentMemoryBlocks(
      S3_MemoryBlock srcMemoryBlock,
      S3_MemoryBlock destMemoryBlock)
    {
      if (srcMemoryBlock.ByteSize == destMemoryBlock.ByteSize && srcMemoryBlock.SegmentType != S3_MemorySegment.CompleteMeter)
        return;
      if (srcMemoryBlock.childMemoryBlocks == null || destMemoryBlock.childMemoryBlocks == null)
        this.AddInfo(destMemoryBlock.SegmentType.ToString());
      else if (srcMemoryBlock.childMemoryBlocks.Count == destMemoryBlock.childMemoryBlocks.Count)
      {
        for (int index = 0; index < srcMemoryBlock.childMemoryBlocks.Count; ++index)
          this.FindDifferentMemoryBlocks(srcMemoryBlock.childMemoryBlocks[index], destMemoryBlock.childMemoryBlocks[index]);
      }
      else
        this.AddInfo(srcMemoryBlock.SegmentType.ToString());
    }

    private string FindDifferentSegments(CompareInfo newInfo)
    {
      this.diffInfo.Length = 0;
      this.FindDifferentMemoryBlocks(newInfo.srcMeter.MyDeviceMemory.meterMemory, newInfo.destMeter.MyDeviceMemory.meterMemory);
      return this.diffInfo.ToString();
    }

    private string GetFunctionDiff(CompareInfo newInfo)
    {
      this.diffInfo.Length = 0;
      SortedList<ushort, S3_Function> allFunctions1 = newInfo.srcMeter.MyFunctionManager.GetAllFunctions();
      SortedList<ushort, S3_Function> allFunctions2 = newInfo.destMeter.MyFunctionManager.GetAllFunctions();
      foreach (ushort key in (IEnumerable<ushort>) allFunctions2.Keys)
      {
        if (!allFunctions1.ContainsKey(key))
        {
          S3_Function s3Function = allFunctions2[key];
          this.AddInfo("+" + s3Function.FunctionNumber.ToString() + ":" + s3Function.Name);
        }
      }
      foreach (ushort key in (IEnumerable<ushort>) allFunctions1.Keys)
      {
        if (!allFunctions2.ContainsKey(key))
        {
          S3_Function s3Function = allFunctions1[key];
          this.AddInfo("-" + s3Function.FunctionNumber.ToString() + ":" + s3Function.Name);
        }
      }
      return this.diffInfo.ToString();
    }

    private void AddInfo(string newText)
    {
      if (this.diffInfo.Length != 0)
        this.diffInfo.Append("; ");
      this.diffInfo.Append(newText);
    }

    private void MeterObjectComparer_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (Thread.CurrentThread != this.ComparerThread)
        return;
      e.Cancel = true;
      this.Hide();
    }

    private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dataGridView1.SelectedCells.Count == 0)
        return;
      int rowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
      if (this.diagWindow == null)
        this.diagWindow = new DiagnoseWindow(this.MyFunctions);
      this.diagWindow.checkBoxSuppressLineAddresses.Checked = this.checkBoxSuppressLineAddresses.Checked;
      string blockPrint1 = this.diagWindow.GetBlockPrint(this.compareList[rowIndex].srcMeter);
      string blockPrint2 = this.diagWindow.GetBlockPrint(this.compareList[rowIndex].destMeter);
      string str1 = this.diagWindow.WriteInfoFile("oldType", blockPrint1);
      string str2 = this.diagWindow.WriteInfoFile("newType", blockPrint2);
      Process process = new Process();
      if (this.checkBoxUseWinDiff.Checked)
      {
        process.StartInfo.FileName = "WinDiff";
        process.StartInfo.Arguments = "\"" + str1 + "\" \"" + str2 + "\"";
      }
      else
      {
        process.StartInfo.FileName = "TortoiseMerge";
        process.StartInfo.Arguments = "/base:\"" + str1 + "\" /theirs:\"" + str2 + "\"";
      }
      process.Start();
    }

    private void buttonClear_Click(object sender, EventArgs e)
    {
      this.compareList.Clear();
      this.dataGridView1.Rows.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MeterObjectComparer));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.dataGridView1 = new DataGridView();
      this.checkBoxUseWinDiff = new CheckBox();
      this.checkBoxSuppressLineAddresses = new CheckBox();
      this.buttonClear = new Button();
      this.Index = new DataGridViewTextBoxColumn();
      this.Info = new DataGridViewTextBoxColumn();
      this.SapSource = new DataGridViewTextBoxColumn();
      this.SourceDate = new DataGridViewTextBoxColumn();
      this.CompareDate = new DataGridViewTextBoxColumn();
      this.MapIsEqual = new DataGridViewCheckBoxColumn();
      this.ImportantParamEqual = new DataGridViewCheckBoxColumn();
      this.FunctionDiff = new DataGridViewTextBoxColumn();
      this.ChangedParams = new DataGridViewTextBoxColumn();
      this.SegmentsDiff = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1327, 42);
      this.zennerCoroprateDesign2.TabIndex = 17;
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.Index, (DataGridViewColumn) this.Info, (DataGridViewColumn) this.SapSource, (DataGridViewColumn) this.SourceDate, (DataGridViewColumn) this.CompareDate, (DataGridViewColumn) this.MapIsEqual, (DataGridViewColumn) this.ImportantParamEqual, (DataGridViewColumn) this.FunctionDiff, (DataGridViewColumn) this.ChangedParams, (DataGridViewColumn) this.SegmentsDiff);
      this.dataGridView1.Location = new Point(12, 47);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.Size = new Size(1303, 450);
      this.dataGridView1.TabIndex = 18;
      this.dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
      this.checkBoxUseWinDiff.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.checkBoxUseWinDiff.AutoSize = true;
      this.checkBoxUseWinDiff.Location = new Point(12, 515);
      this.checkBoxUseWinDiff.Name = "checkBoxUseWinDiff";
      this.checkBoxUseWinDiff.Size = new Size((int) sbyte.MaxValue, 17);
      this.checkBoxUseWinDiff.TabIndex = 19;
      this.checkBoxUseWinDiff.Text = "Use WinDiff compare";
      this.checkBoxUseWinDiff.UseVisualStyleBackColor = true;
      this.checkBoxSuppressLineAddresses.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.checkBoxSuppressLineAddresses.AutoSize = true;
      this.checkBoxSuppressLineAddresses.Location = new Point(159, 515);
      this.checkBoxSuppressLineAddresses.Name = "checkBoxSuppressLineAddresses";
      this.checkBoxSuppressLineAddresses.Size = new Size(198, 17);
      this.checkBoxSuppressLineAddresses.TabIndex = 28;
      this.checkBoxSuppressLineAddresses.Text = "Suppress line addresses by compare";
      this.checkBoxSuppressLineAddresses.UseVisualStyleBackColor = true;
      this.buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClear.Location = new Point(1205, 509);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(110, 23);
      this.buttonClear.TabIndex = 29;
      this.buttonClear.Text = "Clear";
      this.buttonClear.UseVisualStyleBackColor = true;
      this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
      this.Index.HeaderText = "Index";
      this.Index.Name = "Index";
      this.Index.ReadOnly = true;
      this.Info.HeaderText = "Info";
      this.Info.Name = "Info";
      this.Info.ReadOnly = true;
      this.SapSource.HeaderText = "SAP number";
      this.SapSource.Name = "SapSource";
      this.SapSource.ReadOnly = true;
      this.SourceDate.HeaderText = "SourceDate";
      this.SourceDate.Name = "SourceDate";
      this.SourceDate.ReadOnly = true;
      this.CompareDate.HeaderText = "CompareDate";
      this.CompareDate.Name = "CompareDate";
      this.CompareDate.ReadOnly = true;
      this.MapIsEqual.HeaderText = "Map is equal";
      this.MapIsEqual.Name = "MapIsEqual";
      this.MapIsEqual.ReadOnly = true;
      this.ImportantParamEqual.HeaderText = "Import. param equal";
      this.ImportantParamEqual.Name = "ImportantParamEqual";
      this.ImportantParamEqual.ReadOnly = true;
      this.ImportantParamEqual.Resizable = DataGridViewTriState.True;
      this.ImportantParamEqual.SortMode = DataGridViewColumnSortMode.Automatic;
      this.FunctionDiff.HeaderText = "Functions";
      this.FunctionDiff.Name = "FunctionDiff";
      this.ChangedParams.HeaderText = "Changed params";
      this.ChangedParams.Name = "ChangedParams";
      this.SegmentsDiff.HeaderText = "Segments";
      this.SegmentsDiff.Name = "SegmentsDiff";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1327, 544);
      this.Controls.Add((Control) this.buttonClear);
      this.Controls.Add((Control) this.checkBoxSuppressLineAddresses);
      this.Controls.Add((Control) this.checkBoxUseWinDiff);
      this.Controls.Add((Control) this.dataGridView1);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MeterObjectComparer);
      this.Text = "Meter object comparer";
      this.FormClosing += new FormClosingEventHandler(this.MeterObjectComparer_FormClosing);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
