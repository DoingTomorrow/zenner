// Decompiled with JetBrains decompiler
// Type: S3_Handler.DiagnoseWindow
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using DeviceCollector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class DiagnoseWindow : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Label label1;
    private ComboBox comboBoxBaseMeter;
    private Label label2;
    private ComboBox comboBoxCompareMeter;
    private Button buttonExportMemory;
    private Button buttonCompareMemory;
    private Button buttonExportBlockList;
    private Button buttonCompareBlockList;
    private Button buttonPrepareBackupBlocks;
    private Button btnGetMemory;
    private Label label3;
    private Label label4;
    private TextBox txtStartAddress;
    private TextBox txtEndAddress;
    private TextBox txtMemory;
    private Button buttonExportResources;
    internal CheckBox checkBoxUseWinDiff;
    internal CheckBox checkBoxSuppressLineAddresses;
    private Button buttonSaveWorkMeter;
    private Button buttonCompile;
    private Button buttonLinkWorkMeter;
    private Button buttonCloneWorkMeter;
    private Button btnGetLogger;
    private Button buttonCompareBlockListNextBackup;
    private Button buttonTypeChecker;
    private Button buttonCompareMeterObjects;
    private Button buttonShowComparer;
    private Button buttonSaveBaseMeterToFile;
    private Button buttonLoadWorkMeterFromFile;
    private Button buttonWriteWorkMeter;

    public DiagnoseWindow(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.UpdateBaseInfos();
      if (this.MyFunctions.SaveMeterDataTabel != null && this.MyFunctions.SelectedSaveMeterDataTableRowIndex > 0 && this.MyFunctions.MyMeters.DbMeter != null)
        this.buttonCompareBlockListNextBackup.Enabled = true;
      else
        this.buttonCompareBlockListNextBackup.Enabled = false;
      this.ShowMemoryOverview();
    }

    internal void ShowMemoryOverview(string startText = "")
    {
      this.txtMemory.Clear();
      if (this.MyFunctions.MyMeters.WorkMeter == null)
        this.txtMemory.Text = "WorkMeter not available";
      else if (this.MyFunctions.MyMeters.WorkMeter.MyDeviceMemory == null)
      {
        this.txtMemory.Text = "WorkMeter.MyDeviceMemory not available";
      }
      else
      {
        DeviceMemory deviceMemory = this.MyFunctions.MyMeters.WorkMeter.MyDeviceMemory;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(startText);
        stringBuilder.AppendLine("Available memory bytes in WorkMeter:");
        bool flag = false;
        for (int index = 0; index < deviceMemory.MemoryBytes.Length; ++index)
        {
          if (!flag)
          {
            if (deviceMemory.ByteIsDefined[index])
            {
              stringBuilder.AppendLine();
              stringBuilder.Append("From Adr: 0x" + index.ToString("x04"));
              flag = true;
            }
          }
          else if (!deviceMemory.ByteIsDefined[index])
          {
            stringBuilder.Append(" to 0x" + index.ToString("x04"));
            flag = false;
          }
        }
        if (flag)
          stringBuilder.Append(" to 0x" + (deviceMemory.MemoryBytes.Length - 1).ToString("x04"));
        this.txtMemory.Text = stringBuilder.ToString();
      }
    }

    internal void UpdateBaseInfos()
    {
      this.comboBoxBaseMeter.Enabled = false;
      this.comboBoxCompareMeter.Enabled = false;
      this.buttonPrepareBackupBlocks.Enabled = false;
      this.comboBoxBaseMeter.Items.Clear();
      this.comboBoxCompareMeter.Items.Clear();
      string[] names = Enum.GetNames(typeof (MeterObjects));
      for (int TheSelect = 0; TheSelect < names.Length; ++TheSelect)
      {
        if (this.MyFunctions.MyMeters.GetMeterObject((MeterObjects) TheSelect) != null)
          this.comboBoxBaseMeter.Items.Add((object) names[TheSelect]);
      }
      if (this.comboBoxBaseMeter.Items.Count > 0)
      {
        this.buttonPrepareBackupBlocks.Enabled = true;
        this.comboBoxBaseMeter.Enabled = true;
        if (!this.SelectBaseMeter(MeterObjects.ConnectedMeter) && !this.SelectBaseMeter(MeterObjects.TypeMeter) && !this.SelectBaseMeter(MeterObjects.DbMeter) && !this.SelectBaseMeter(MeterObjects.SavedMeter))
          this.comboBoxBaseMeter.SelectedIndex = 0;
      }
      this.UpdateCompareMetersList();
    }

    private bool SelectBaseMeter(MeterObjects baseMeterToSelect)
    {
      for (int index = 0; index < this.comboBoxBaseMeter.Items.Count; ++index)
      {
        if (this.comboBoxBaseMeter.Items[index].ToString() == baseMeterToSelect.ToString())
        {
          this.comboBoxBaseMeter.SelectedIndex = index;
          return true;
        }
      }
      return false;
    }

    private void UpdateCompareMetersList()
    {
      if (this.comboBoxBaseMeter.Items.Count == 0)
        return;
      this.comboBoxCompareMeter.Items.Clear();
      string[] names = Enum.GetNames(typeof (MeterObjects));
      string str = this.comboBoxBaseMeter.SelectedItem.ToString();
      for (int TheSelect = 0; TheSelect < names.Length; ++TheSelect)
      {
        if (!(names[TheSelect] == str) && this.MyFunctions.MyMeters.GetMeterObject((MeterObjects) TheSelect) != null)
          this.comboBoxCompareMeter.Items.Add((object) names[TheSelect]);
      }
      if (this.comboBoxCompareMeter.Items.Count <= 0)
        return;
      this.comboBoxCompareMeter.Enabled = true;
      for (int index = 0; index < this.comboBoxCompareMeter.Items.Count; ++index)
      {
        if (this.comboBoxCompareMeter.Items[index].ToString() == MeterObjects.WorkMeter.ToString())
        {
          this.comboBoxCompareMeter.SelectedIndex = index;
          return;
        }
      }
      this.comboBoxCompareMeter.SelectedIndex = 0;
    }

    private void comboBoxBaseMeter_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.UpdateCompareMetersList();
    }

    private S3_Meter GetSelectedMeter(ComboBox TheBox)
    {
      if (TheBox.SelectedIndex < 0)
        return (S3_Meter) null;
      return this.MyFunctions.MyMeters.GetMeterObject((MeterObjects) Enum.Parse(typeof (MeterObjects), TheBox.SelectedItem.ToString()));
    }

    private void buttonExportMemory_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter == null)
        return;
      string memoryPrint = this.GetMemoryPrint(selectedMeter);
      string str = this.WriteInfoFile(this.comboBoxBaseMeter.SelectedItem.ToString(), memoryPrint);
      new Process() { StartInfo = { FileName = str } }.Start();
    }

    private void buttonCompareMemory_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter1 = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter1 == null)
        return;
      S3_Meter selectedMeter2 = this.GetSelectedMeter(this.comboBoxCompareMeter);
      if (selectedMeter2 == null)
        return;
      string memoryPrint1 = this.GetMemoryPrint(selectedMeter1);
      string memoryPrint2 = this.GetMemoryPrint(selectedMeter2);
      string str1 = this.WriteInfoFile(this.comboBoxBaseMeter.SelectedItem.ToString(), memoryPrint1);
      string str2 = this.WriteInfoFile(this.comboBoxCompareMeter.SelectedItem.ToString(), memoryPrint2);
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

    private string GetMemoryPrint(S3_Meter SelectedMeter)
    {
      byte[] memoryBytes = SelectedMeter.MyDeviceMemory.MemoryBytes;
      bool[] byteIsDefined = SelectedMeter.MyDeviceMemory.ByteIsDefined;
      int index1 = SelectedMeter.MyDeviceMemory.minDefinedAddress & -256;
      int num1 = SelectedMeter.MyDeviceMemory.minUnDefinedAddress | (int) byte.MaxValue;
      if (num1 >= memoryBytes.Length)
        num1 = memoryBytes.Length - 1;
      StringBuilder stringBuilder = new StringBuilder(30000);
      while (index1 <= num1)
      {
        if ((index1 & (int) byte.MaxValue) == 0)
        {
          int num2 = index1 + 256;
          if (num2 > num1)
            num2 = num1 + 1;
          int index2 = index1;
          while (index2 < num2 && !byteIsDefined[index2])
            ++index2;
          if (index2 == num2)
          {
            index1 = num2;
            continue;
          }
          if (stringBuilder.Length > 0)
            stringBuilder.AppendLine();
          stringBuilder.Append("------ ");
          for (int index3 = 0; index3 < 16; ++index3)
            stringBuilder.Append(" " + index3.ToString("x02"));
          stringBuilder.AppendLine();
        }
        if ((index1 & 15) == 0)
          stringBuilder.Append(index1.ToString("x06") + ":");
        if (byteIsDefined[index1])
          stringBuilder.Append(" " + memoryBytes[index1].ToString("x02"));
        else
          stringBuilder.Append(" --");
        if ((index1 & 15) == 15)
          stringBuilder.AppendLine();
        int num3 = index1 & 15;
        int num4;
        switch (num3)
        {
          case 4:
          case 8:
            num4 = 1;
            break;
          default:
            num4 = num3 == 12 ? 1 : 0;
            break;
        }
        if (num4 != 0)
          stringBuilder[stringBuilder.Length - 3] = '.';
        ++index1;
      }
      return stringBuilder.ToString();
    }

    internal string WriteInfoFile(string BaseName, string TheData)
    {
      string loggDataPath = SystemValues.LoggDataPath;
      DateTime now = DateTime.Now;
      string path = Path.Combine(loggDataPath, now.ToString("yyMMddHHmmss") + "_Log_" + BaseName + ".txt");
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
      {
        streamWriter.WriteLine("S3_Handler diagnostic            '" + BaseName + "'         " + now.ToLongDateString() + " " + now.ToLongTimeString());
        streamWriter.WriteLine();
        streamWriter.Write(TheData);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    private void buttonExportBlockList_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter == null)
        return;
      string blockPrint = this.GetBlockPrint(selectedMeter);
      string str = this.WriteInfoFile(this.comboBoxBaseMeter.SelectedItem.ToString(), blockPrint);
      new Process() { StartInfo = { FileName = str } }.Start();
    }

    internal string GetBlockPrint(S3_Meter SelectedMeter)
    {
      S3_MemoryBlock meterMemory = SelectedMeter.MyDeviceMemory.meterMemory;
      StringBuilder OutText = new StringBuilder(30000);
      Version gmmVersion = Util.GMM_Version;
      OutText.AppendLine(string.Format("Global Meter Manager v{0} (rev {1})", (object) gmmVersion.ToString(3), (object) gmmVersion.Revision));
      OutText.AppendLine();
      OutText.AppendLine("**** Legende ****");
      OutText.AppendLine("'_' vor einem Datenbyte -> Das Byte ist write protected!");
      OutText.AppendLine("'^' vor einer Parameter Adresse -> Der RAM Parameter wird dynamisch gelesen!");
      OutText.AppendLine();
      string empty = string.Empty;
      this.GetBlockInfo(SelectedMeter, meterMemory, ref OutText, empty);
      OutText.AppendLine();
      OutText.AppendLine();
      OutText.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
      OutText.AppendLine("Ressources +++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
      OutText.AppendLine();
      OutText.AppendLine(this.GetResources(SelectedMeter));
      return OutText.ToString();
    }

    private bool GetBlockInfo(
      S3_Meter SelectedMeter,
      S3_MemoryBlock theMemoryBlock,
      ref StringBuilder OutText,
      string lineStartSpaces)
    {
      try
      {
        int num1;
        switch (theMemoryBlock)
        {
          case S3_Parameter _:
            S3_Parameter s3Parameter1 = (S3_Parameter) theMemoryBlock;
            if (SelectedMeter.MyParameters.IsDynamicParameter(s3Parameter1.BlockStartAddress))
              this.OutputOnly8_Bytes(lineStartSpaces + "^", SelectedMeter, s3Parameter1.BlockStartAddress, s3Parameter1.ByteSize, ref OutText);
            else
              this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, s3Parameter1.BlockStartAddress, s3Parameter1.ByteSize, ref OutText);
            OutText.AppendLine(s3Parameter1.ToString());
            if (s3Parameter1.Name == S3_ParameterNames.Device_Setup.ToString())
            {
              string str1 = lineStartSpaces + "   > ";
              OutText.AppendLine(str1 + "Device_Setup interpretation:");
              string str2 = str1 + " * ";
              ushort ushortValue = s3Parameter1.GetUshortValue();
              switch ((int) ushortValue & 768)
              {
                case 256:
                  OutText.AppendLine(str2 + "Enerty type: Heat meter");
                  break;
                case 512:
                  OutText.AppendLine(str2 + "Enerty type: Cooling meter");
                  break;
                case 768:
                  OutText.AppendLine(str2 + "Enerty type: Heat and cooling meter");
                  break;
                default:
                  OutText.AppendLine(str2 + "Enerty type: not defined");
                  break;
              }
              if (((int) ushortValue & 16) == 0)
                OutText.AppendLine(str2 + "Inline temperatur used");
              else
                OutText.AppendLine(str2 + "Constant defined inline temperatur used");
              if (((int) ushortValue & 32) == 0)
                OutText.AppendLine(str2 + "Outline temperatur used");
              else
                OutText.AppendLine(str2 + "Constant defined outline temperatur used");
              if (((int) ushortValue & 1) == 0)
                OutText.AppendLine(str2 + "Temperature calculation in MWh");
              else
                OutText.AppendLine(str2 + "Temperature calculation in Jule");
              if (((int) ushortValue & 2) == 0)
                OutText.AppendLine(str2 + "Heating/cooling threshold temperatur not used");
              else
                OutText.AppendLine(str2 + "Heating/cooling threshold temperatur used");
              if (((int) ushortValue & 4) == 0)
                OutText.AppendLine(str2 + "DT min limit not used");
              else
                OutText.AppendLine(str2 + "DT min limit used");
              if (((int) ushortValue & 8) == 0)
                OutText.AppendLine(str2 + "Energy inside DT min limits is calculated");
              else
                OutText.AppendLine(str2 + "Energy inside DT min limits is set to 0");
            }
            else if (s3Parameter1.Name == S3_ParameterNames.Device_Setup_2.ToString())
            {
              string str3 = lineStartSpaces + "   > ";
              OutText.AppendLine(str3 + "Device_Setup_2 interpretation:");
              string str4 = str3 + " * ";
              ushort ushortValue = s3Parameter1.GetUshortValue();
              if (((uint) ushortValue & (uint) S3P_Device_Setup_2.DEVICE_SETUP2_TDC_ErrorBits) > 0U)
                OutText.AppendLine(str4 + "Illegal TDC configuration !!!!!!!!!");
              if (((int) ushortValue & (int) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE) == 0)
              {
                if (((int) ushortValue & (int) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE_NOT_SELECTED) == 0)
                {
                  if (((int) ushortValue & (int) S3P_Device_Setup_2.DEVICE_SETUP2_CHANGE_TEMP_IF_FLOW_LINE_SET) == 0)
                    OutText.AppendLine(str4 + "VM-Outline");
                  else
                    OutText.AppendLine(str4 + "VM-Outline; Temp. sensors not xchanged");
                }
                else
                  OutText.AppendLine(str4 + "Error! VM-Outline selecting not possible ");
              }
              else if (((int) ushortValue & (int) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE_NOT_SELECTED) == 0)
              {
                if (((int) ushortValue & (int) S3P_Device_Setup_2.DEVICE_SETUP2_CHANGE_TEMP_IF_FLOW_LINE_SET) == 0)
                  OutText.AppendLine(str4 + "VM-Inline");
                else
                  OutText.AppendLine(str4 + "VM-Inline; Temp. sensors xchanged");
              }
              else if (((int) ushortValue & (int) S3P_Device_Setup_2.DEVICE_SETUP2_CHANGE_TEMP_IF_FLOW_LINE_SET) == 0)
                OutText.AppendLine(str4 + "VM selecting required. Temp. sensors not xchanged");
              else
                OutText.AppendLine(str4 + "VM selecting required; Temp. sensors xchanged");
              ushort num2 = (ushort) ((uint) ushortValue & (uint) S3P_Device_Setup_2.DEVICE_SETUP2_PT_Mask);
              if ((int) num2 == (int) S3P_Device_Setup_2.DEVICE_SETUP2_PT100)
                OutText.AppendLine(str4 + "PT100");
              else if ((int) num2 == (int) S3P_Device_Setup_2.DEVICE_SETUP2_PT500)
                OutText.AppendLine(str4 + "PT500");
              else if ((int) num2 == (int) S3P_Device_Setup_2.DEVICE_SETUP2_PT1000)
                OutText.AppendLine(str4 + "PT1000");
              else
                OutText.AppendLine(str4 + "PT config error !!!!!!!");
              ushort num3 = (ushort) ((uint) ushortValue & (uint) S3P_Device_Setup_2.DEVICE_SETUP2_TDC_MatrixMask);
              if (num3 == (ushort) 0)
                OutText.AppendLine(str4 + "TDC matrix not used");
              else if ((int) num3 == (int) S3P_Device_Setup_2.DEVICE_SETUP2_TDC_MatrixMask)
                OutText.AppendLine(str4 + "TDC matrix used");
              else
                OutText.AppendLine(str4 + "Illedal TDC matrix setup");
            }
            if (s3Parameter1.Name == S3_ParameterNames.TDC_MapTemp.ToString())
            {
              try
              {
                string str5 = lineStartSpaces + "   > ";
                string str6 = str5 + "> ";
                string str7 = str6 + "= ";
                OutText.AppendLine(str5 + "TDC calibration data matrix:");
                int blockStartAddress1 = s3Parameter1.BlockStartAddress;
                S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
                S3_Parameter s3Parameter2 = s3Parameter1;
                int minValue = (int) s3Parameter2.Statics.MinValue;
                int maxValue = (int) s3Parameter2.Statics.MaxValue;
                int blockStartAddress2 = s3Parameter2.BlockStartAddress;
                OutText.AppendLine(str6 + "Temperature values (TDC_MapTemp) [C]:");
                OutText.Append(str7);
                int num4 = 0;
                while (num4 < minValue)
                {
                  float num5 = (float) workMeter.MyDeviceMemory.GetShortValue(blockStartAddress2) / 100f;
                  if (num4 != 0)
                    OutText.Append(';');
                  OutText.Append(num5.ToString("f2"));
                  ++num4;
                  blockStartAddress2 += 2;
                }
                OutText.AppendLine();
                OutText.AppendLine(str6 + "Flow values (TDC_MapFlow) [Liter/h]:");
                OutText.Append(str7);
                int num6 = 0;
                while (num6 < maxValue)
                {
                  float floatValue = workMeter.MyDeviceMemory.GetFloatValue(blockStartAddress2);
                  if (num6 != 0)
                    OutText.Append(';');
                  OutText.Append(floatValue.ToString("f1"));
                  ++num6;
                  blockStartAddress2 += 4;
                }
                OutText.AppendLine();
                OutText.AppendLine(str6 + "Correction (TDC_MapCoef):");
                for (int index = 0; index < minValue; ++index)
                {
                  OutText.Append(str7);
                  int num7 = 0;
                  while (num7 < maxValue)
                  {
                    float floatValue = workMeter.MyDeviceMemory.GetFloatValue(blockStartAddress2);
                    if (num7 != 0)
                      OutText.Append(';');
                    OutText.Append(floatValue.ToString("f3"));
                    ++num7;
                    blockStartAddress2 += 4;
                  }
                  OutText.AppendLine();
                }
              }
              catch
              {
              }
            }
            return true;
          case S3_Function _:
            this.OutputWord(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, ref OutText);
            OutText.AppendLine(((S3_Function) theMemoryBlock).Name);
            break;
          case S3_Pointer _:
            this.OutputPointer(lineStartSpaces, SelectedMeter, theMemoryBlock.BlockStartAddress, " Pointer: " + ((S3_Pointer) theMemoryBlock).PointerName, ref OutText);
            break;
          default:
            if (!this.checkBoxSuppressLineAddresses.Checked)
              OutText.Append(theMemoryBlock.BlockStartAddress.ToString("x04"));
            OutText.Append(lineStartSpaces);
            if (lineStartSpaces.Length > 0)
              --OutText.Length;
            OutText.Append("* " + theMemoryBlock.SegmentType.ToString());
            if (!this.checkBoxSuppressLineAddresses.Checked)
            {
              StringBuilder stringBuilder = OutText;
              num1 = theMemoryBlock.StartAddressOfNextBlock;
              string str = "; Range to: " + num1.ToString("x04");
              stringBuilder.Append(str);
            }
            StringBuilder stringBuilder1 = OutText;
            num1 = theMemoryBlock.ByteSize;
            string str8 = "; Byte size: " + num1.ToString();
            stringBuilder1.Append(str8);
            OutText.AppendLine();
            if (theMemoryBlock.firstChildMemoryBlockOffset > 0)
              this.OutputByteBlock(lineStartSpaces + " so: ", SelectedMeter, theMemoryBlock.BlockStartAddress, theMemoryBlock.firstChildMemoryBlockOffset, ref OutText);
            if (theMemoryBlock.IsHardLinkedAddress)
              this.SetLineBeforeLastLine(Environment.NewLine + "##########################################################################################", ref OutText);
            if (theMemoryBlock.SegmentType == S3_MemorySegment.TransmitParameterTable || theMemoryBlock.SegmentType == S3_MemorySegment.DisplayCode || theMemoryBlock.SegmentType == S3_MemorySegment.ProtectedDisplayCode || theMemoryBlock.SegmentType == S3_MemorySegment.LoggerData || theMemoryBlock.SegmentType == S3_MemorySegment.LoggerRamData)
              this.SetLineBeforeLastLine(Environment.NewLine + "==========================================================================================", ref OutText);
            switch (theMemoryBlock)
            {
              case S3_RuntimeCode _:
                this.OutputByteBlock(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, theMemoryBlock.ByteSize, ref OutText);
                break;
              case S3_DisplayFunction _:
                S3_DisplayFunction s3DisplayFunction = (S3_DisplayFunction) theMemoryBlock;
                OutText.Length -= Environment.NewLine.Length;
                OutText.Append(" Function: " + s3DisplayFunction.MyFunction.Name + " [" + s3DisplayFunction.MyFunction.FunctionNumber.ToString() + "]");
                OutText.AppendLine();
                this.SetLineBeforeLastLine("--------------------------------------------------------------------------------", ref OutText);
                break;
              case S3_DisplayMenu _:
                S3_DisplayMenu s3DisplayMenu = (S3_DisplayMenu) theMemoryBlock;
                OutText.Length -= Environment.NewLine.Length;
                OutText.Append(" Menu: " + s3DisplayMenu.MenuName);
                OutText.AppendLine();
                this.OutputByteBlock(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, s3DisplayMenu.ByteSize, ref OutText);
                break;
              case S3_DisplayMenuBrunches _:
                S3_DisplayMenuBrunches displayMenuBrunches = (S3_DisplayMenuBrunches) theMemoryBlock;
                this.OutputByte(lineStartSpaces, SelectedMeter, theMemoryBlock.BlockStartAddress, (string) null, ref OutText);
                displayMenuBrunches.GetFollowInfo(ref OutText);
                OutText.AppendLine();
                int num8 = 1;
                if (displayMenuBrunches.ClickJump)
                {
                  this.OutputPointer(lineStartSpaces, SelectedMeter, theMemoryBlock.BlockStartAddress + num8, " Jump   -> Click:   " + displayMenuBrunches.ClickPointerName, ref OutText);
                  num8 += 2;
                }
                if (displayMenuBrunches.PressJump)
                {
                  this.OutputPointer(lineStartSpaces, SelectedMeter, theMemoryBlock.BlockStartAddress + num8, " Jump   -> Press:   " + displayMenuBrunches.PressPointerName, ref OutText);
                  num8 += 2;
                }
                if (displayMenuBrunches.HoldJump)
                {
                  this.OutputPointer(lineStartSpaces, SelectedMeter, theMemoryBlock.BlockStartAddress + num8, " Jump   -> Hold:    " + displayMenuBrunches.HoldPointerName, ref OutText);
                  num8 += 2;
                }
                if (displayMenuBrunches.TimeoutJump)
                {
                  this.OutputPointer(lineStartSpaces, SelectedMeter, theMemoryBlock.BlockStartAddress + num8, " Jump   -> Timeout: " + displayMenuBrunches.TimeoutPointerName, ref OutText);
                  int num9 = num8 + 2;
                  break;
                }
                break;
              case S3_FunctionLayer _:
                S3_FunctionLayer s3FunctionLayer = (S3_FunctionLayer) theMemoryBlock;
                OutText.Length -= Environment.NewLine.Length;
                StringBuilder stringBuilder2 = OutText;
                num1 = s3FunctionLayer.LayerNr;
                string str9 = "  Layer number: " + num1.ToString();
                stringBuilder2.AppendLine(str9);
                this.OutputWord(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, ref OutText);
                OutText.AppendLine("== function layer control word");
                break;
              case MBusTransmitter _:
                MBusTransmitter mbusTransmitter = (MBusTransmitter) theMemoryBlock;
                if (!this.checkBoxSuppressLineAddresses.Checked)
                {
                  StringBuilder stringBuilder3 = OutText;
                  num1 = theMemoryBlock.BlockStartAddress;
                  string str10 = num1.ToString("x04");
                  stringBuilder3.Append(str10);
                }
                OutText.Append(lineStartSpaces);
                OutText.AppendLine(" " + mbusTransmitter.Name);
                break;
              case MBusList _:
                MBusList mbusList = (MBusList) theMemoryBlock;
                if (!this.checkBoxSuppressLineAddresses.Checked)
                {
                  StringBuilder stringBuilder4 = OutText;
                  num1 = theMemoryBlock.BlockStartAddress;
                  string str11 = num1.ToString("x04");
                  stringBuilder4.Append(str11);
                }
                string str12 = "MBuListIndex:" + mbusList.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) mbusList).ToString();
                OutText.Append(lineStartSpaces);
                OutText.Append(" " + str12);
                OutText.Append("  ControlWord: " + mbusList.ControlWord.ToString());
                OutText.Append("  VirtualDeviceNr: " + mbusList.VirtualDeviceNumber.ToString());
                OutText.AppendLine("  IsSelected: " + mbusList.IsSelected.ToString());
                break;
              case RadioListHeader _:
                RadioListHeader radioListHeader = (RadioListHeader) theMemoryBlock;
                if (!this.checkBoxSuppressLineAddresses.Checked)
                {
                  StringBuilder stringBuilder5 = OutText;
                  num1 = theMemoryBlock.BlockStartAddress;
                  string str13 = num1.ToString("x04");
                  stringBuilder5.Append(str13);
                }
                OutText.Append(lineStartSpaces);
                OutText.Append(" " + radioListHeader.Mode_Memory.ToString() + " " + radioListHeader.ScenarioName_Memory);
                OutText.AppendLine("  IsSelected: " + radioListHeader.IsSelected_Memory.ToString());
                break;
              case MBusParameterGroup _:
                MBusParameterGroup mbusParameterGroup = (MBusParameterGroup) theMemoryBlock;
                if (!this.checkBoxSuppressLineAddresses.Checked)
                {
                  StringBuilder stringBuilder6 = OutText;
                  num1 = theMemoryBlock.BlockStartAddress;
                  string str14 = num1.ToString("x04");
                  stringBuilder6.Append(str14);
                }
                OutText.Append(lineStartSpaces);
                StringBuilder stringBuilder7 = OutText;
                string name = mbusParameterGroup.Name;
                num1 = mbusParameterGroup.LoggerCycleCount;
                string str15 = num1.ToString();
                string str16 = " Name: " + name + "; Cycles: " + str15;
                stringBuilder7.Append(str16);
                StringBuilder stringBuilder8 = OutText;
                num1 = mbusParameterGroup.ListMaxCount;
                string str17 = num1.ToString();
                num1 = mbusParameterGroup.StartStorageNumber;
                string str18 = num1.ToString();
                string str19 = "; MaxCount: " + str17 + "; StartStorageNumber: " + str18;
                stringBuilder8.AppendLine(str19);
                break;
              case MBusParameter _:
                MBusParameter mbusParameter = (MBusParameter) theMemoryBlock;
                if (!this.checkBoxSuppressLineAddresses.Checked)
                {
                  StringBuilder stringBuilder9 = OutText;
                  num1 = theMemoryBlock.BlockStartAddress;
                  string str20 = num1.ToString("x04");
                  stringBuilder9.Append(str20);
                }
                OutText.Append(lineStartSpaces);
                StringBuilder stringBuilder10 = OutText;
                ushort controlWord = mbusParameter.ControlWord0.ControlWord;
                string str21 = " Control_0: 0x" + controlWord.ToString("X4");
                stringBuilder10.Append(str21);
                if (mbusParameter.IsLogger)
                  OutText.AppendLine(" " + mbusParameter.ControlWord0.ToString());
                else
                  OutText.AppendLine();
                int num10 = 2;
                if (mbusParameter.ControlWord0.ItFollowsNextControlWord)
                {
                  if (!this.checkBoxSuppressLineAddresses.Checked)
                  {
                    StringBuilder stringBuilder11 = OutText;
                    num1 = theMemoryBlock.BlockStartAddress + num10;
                    string str22 = num1.ToString("x04");
                    stringBuilder11.Append(str22);
                  }
                  OutText.Append(lineStartSpaces);
                  StringBuilder stringBuilder12 = OutText;
                  controlWord = mbusParameter.ControlWord1.ControlWord;
                  string str23 = " Control_1: 0x" + controlWord.ToString("X4");
                  stringBuilder12.Append(str23);
                  if (mbusParameter.IsLogger)
                    OutText.AppendLine(" " + mbusParameter.ControlWord1.ToString());
                  else
                    OutText.AppendLine();
                  num10 += 2;
                  if (mbusParameter.ControlWord1.ItFollowsNextControlWord)
                  {
                    if (!this.checkBoxSuppressLineAddresses.Checked)
                    {
                      StringBuilder stringBuilder13 = OutText;
                      num1 = theMemoryBlock.BlockStartAddress + num10;
                      string str24 = num1.ToString("x04");
                      stringBuilder13.Append(str24);
                    }
                    OutText.Append(lineStartSpaces);
                    StringBuilder stringBuilder14 = OutText;
                    controlWord = mbusParameter.ControlWord2.ControlWord;
                    string str25 = " Control_2: 0x" + controlWord.ToString("X4");
                    stringBuilder14.Append(str25);
                    if (mbusParameter.IsLogger)
                      OutText.AppendLine(" " + mbusParameter.ControlWord2.ToString());
                    else
                      OutText.AppendLine();
                    num10 += 2;
                  }
                }
                if (mbusParameter.VifDif.Count > 0)
                {
                  int difVifStartOffset = theMemoryBlock.BlockStartAddress + num10;
                  if (!this.checkBoxSuppressLineAddresses.Checked)
                    OutText.Append(difVifStartOffset.ToString("x04"));
                  OutText.Append(lineStartSpaces);
                  MBusDifVif mbusDifVif = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
                  if (!mbusDifVif.LoadDifVif(theMemoryBlock.MyMeter.MyDeviceMemory.MemoryBytes, difVifStartOffset))
                    OutText.AppendLine("DIF/VIF load error");
                  else
                    OutText.AppendLine(" DIF/VIF: " + mbusDifVif.ToString());
                  num10 += mbusParameter.VifDif.Count;
                  if (num10 % 2 != 0)
                  {
                    int Address = theMemoryBlock.BlockStartAddress + num10;
                    if (!this.checkBoxSuppressLineAddresses.Checked)
                      OutText.Append(Address.ToString("x04"));
                    OutText.Append(lineStartSpaces);
                    OutText.AppendLine(" Fill byte: 0x" + theMemoryBlock.MyMeter.MyDeviceMemory.GetByteValue(Address).ToString("X2"));
                    ++num10;
                  }
                }
                if (mbusParameter.ControlWord0.ControlLogger != ControlLogger.LoggerNext)
                {
                  if (!this.checkBoxSuppressLineAddresses.Checked)
                  {
                    StringBuilder stringBuilder15 = OutText;
                    num1 = theMemoryBlock.BlockStartAddress + num10;
                    string str26 = num1.ToString("x04");
                    stringBuilder15.Append(str26);
                  }
                  OutText.Append(lineStartSpaces);
                  OutText.AppendLine(" " + mbusParameter.Name);
                  break;
                }
                break;
              case NotProtectedRange _:
                NotProtectedRange notProtectedRange = (NotProtectedRange) theMemoryBlock;
                OutText.Append("    " + lineStartSpaces);
                OutText.Append(" Not protected from: 0x");
                OutText.Append(notProtectedRange.NotProtectedAddress.ToString("X4"));
                int num11 = (int) notProtectedRange.NotProtectedAddress + (int) notProtectedRange.NotProtectedLength - 1;
                OutText.Append(" to: 0x");
                OutText.Append(num11.ToString("X4"));
                OutText.Append(" Byte size: 0x");
                OutText.Append(notProtectedRange.NotProtectedLength.ToString("X4"));
                OutText.AppendLine();
                return true;
              case S3_FunctionTable _:
                this.OutputByteBlock(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, theMemoryBlock.ByteSize, ref OutText);
                break;
              case LoggerConfiguration _:
                int blockStartAddress3 = theMemoryBlock.BlockStartAddress;
                int memoryBlockOffset = theMemoryBlock.firstChildMemoryBlockOffset;
                int num12 = 0;
                while (blockStartAddress3 < theMemoryBlock.BlockStartAddress + memoryBlockOffset)
                {
                  string str27 = "";
                  switch (num12)
                  {
                    case 0:
                      this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, blockStartAddress3, 4, ref OutText);
                      str27 = "Start time: " + ZR_Calendar.Cal_GetDateTime(SelectedMeter.MyDeviceMemory.GetUintValue(blockStartAddress3)).ToString("dd.MM.yyyy HH:mm:ss");
                      blockStartAddress3 += 4;
                      break;
                    case 1:
                      this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, blockStartAddress3, 4, ref OutText);
                      str27 = "Interval seconds: " + SelectedMeter.MyDeviceMemory.GetUintValue(blockStartAddress3).ToString();
                      blockStartAddress3 += 4;
                      break;
                    default:
                      this.OutputWord(lineStartSpaces + " ", SelectedMeter, blockStartAddress3, ref OutText);
                      if (num12 == 2)
                        str27 = "logger flags";
                      else if (num12 == 3)
                        str27 = "max entries";
                      else if (num12 == 4)
                        str27 = "first chanal description ptr";
                      else if (num12 == 5)
                        str27 = "next logger ptr";
                      blockStartAddress3 += 2;
                      break;
                  }
                  OutText.AppendLine(" --> " + str27);
                  ++num12;
                }
                break;
              case LoggerChanal _:
                LoggerChanal loggerChanal = (LoggerChanal) theMemoryBlock;
                OutText.Length -= Environment.NewLine.Length;
                OutText.Append(" Function: " + loggerChanal.GetChanalFunction().ToString());
                if (loggerChanal.chanalParameter != null)
                  OutText.AppendLine(" parameter: " + loggerChanal.chanalParameter.Name);
                else
                  OutText.AppendLine();
                int blockStartAddress4 = theMemoryBlock.BlockStartAddress;
                int num13 = 0;
                while (blockStartAddress4 < theMemoryBlock.StartAddressOfNextBlock)
                {
                  this.OutputWord(lineStartSpaces + " ", SelectedMeter, blockStartAddress4, ref OutText);
                  string str28 = "";
                  switch (num13)
                  {
                    case 0:
                      str28 = "flags";
                      break;
                    case 1:
                      str28 = "next chanal adr";
                      break;
                    case 2:
                      str28 = "logger header adr";
                      break;
                    case 3:
                      str28 = "parameter adr";
                      break;
                    case 4:
                      str28 = "data adr";
                      break;
                    case 5:
                      str28 = "number of entries in ram logger";
                      break;
                  }
                  OutText.AppendLine(" --> " + str28);
                  blockStartAddress4 += 2;
                  ++num13;
                }
                break;
              case LoggerChanalData _:
                LoggerChanalData loggerChanalData = (LoggerChanalData) theMemoryBlock;
                int num14 = 15;
                if (loggerChanalData.myChanalInfo.chanalParameter != null)
                {
                  OutText.Length -= Environment.NewLine.Length;
                  OutText.AppendLine(" parameter: " + loggerChanalData.myChanalInfo.chanalParameter.Name);
                  num14 = loggerChanalData.myChanalInfo.chanalParameter.ByteSize;
                }
                int num15 = theMemoryBlock.BlockStartAddress;
                while (num15 < theMemoryBlock.StartAddressOfNextBlock)
                {
                  if (loggerChanalData.myChanalInfo.hasTimeStamp)
                  {
                    this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, num15, num14, ref OutText);
                    OutText.AppendLine(SelectedMeter.MyDeviceMemory.GetUlongValue(num15, num14).ToString());
                    int num16 = num15 + num14;
                    this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, num16, 4, ref OutText);
                    DateTime dateTimeValue = SelectedMeter.MyDeviceMemory.GetDateTimeValue(num16);
                    OutText.AppendLine("store time: " + dateTimeValue.ToString("dd.MM.yyyy HH:mm:ss"));
                    num15 = num16 + 4;
                  }
                  else
                  {
                    this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, num15, num14, ref OutText);
                    OutText.AppendLine(SelectedMeter.MyDeviceMemory.GetUlongValue(num15, num14).ToString());
                    num15 += num14;
                  }
                }
                break;
              default:
                if (theMemoryBlock.SegmentType == S3_MemorySegment.Backup0 || theMemoryBlock.SegmentType == S3_MemorySegment.Backup1)
                {
                  try
                  {
                    int blockStartAddress5 = theMemoryBlock.BlockStartAddress;
                    int num17 = SelectedMeter.MyDeviceMemory.BlockBackupInRAM.BlockStartAddress - blockStartAddress5;
                    S3_Parameter s3Parameter3;
                    for (; blockStartAddress5 < theMemoryBlock.StartAddressOfNextBlock; blockStartAddress5 += s3Parameter3.ByteSize)
                    {
                      s3Parameter3 = SelectedMeter.MyParameters.ParameterByAddress[blockStartAddress5 + num17].Clone();
                      s3Parameter3.IsHardLinkedAddress = false;
                      s3Parameter3.parentMemoryBlock = (S3_MemoryBlock) null;
                      s3Parameter3.BlockStartAddress = blockStartAddress5;
                      int blockStartAddress6 = s3Parameter3.BlockStartAddress;
                      if (SelectedMeter.MyParameters.IsDynamicParameter(s3Parameter3.BlockStartAddress))
                        this.OutputOnly8_Bytes(lineStartSpaces + "^", SelectedMeter, blockStartAddress6, s3Parameter3.ByteSize, ref OutText);
                      else
                        this.OutputOnly8_Bytes(lineStartSpaces + " ", SelectedMeter, blockStartAddress6, s3Parameter3.ByteSize, ref OutText);
                      OutText.AppendLine(s3Parameter3.ToString());
                    }
                    break;
                  }
                  catch
                  {
                    break;
                  }
                }
                else
                {
                  if (theMemoryBlock.SegmentType == S3_MemorySegment.ResetRuntimeCode || theMemoryBlock.SegmentType == S3_MemorySegment.CycleRuntimeCode || theMemoryBlock.SegmentType == S3_MemorySegment.MesurementRuntimeCode || theMemoryBlock.SegmentType == S3_MemorySegment.MBusRuntimeCode)
                  {
                    this.OutputByteBlock(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, theMemoryBlock.ByteSize, ref OutText);
                    break;
                  }
                  if (theMemoryBlock.childMemoryBlocks == null || theMemoryBlock.childMemoryBlocks.Count == 0)
                  {
                    if (theMemoryBlock is HandlerInfo && theMemoryBlock.ByteSize > 5)
                    {
                      string parameterTypeUsingString = (theMemoryBlock as HandlerInfo).mem_ParameterTypeUsingString;
                      OutText.Length -= Environment.NewLine.Length;
                      OutText.AppendLine("; String: \"" + parameterTypeUsingString + "\"");
                    }
                    this.OutputByteBlock(lineStartSpaces + "   ", SelectedMeter, theMemoryBlock.BlockStartAddress, theMemoryBlock.ByteSize, ref OutText);
                    break;
                  }
                  break;
                }
            }
            break;
        }
        if (theMemoryBlock.parentMemoryBlock != null)
        {
          int num18 = theMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf(theMemoryBlock);
          if (num18 > 0)
          {
            S3_MemoryBlock childMemoryBlock = theMemoryBlock.parentMemoryBlock.childMemoryBlocks[num18 - 1];
            if (childMemoryBlock.StartAddressOfNextBlock != theMemoryBlock.BlockStartAddress)
            {
              if (!this.checkBoxSuppressLineAddresses.Checked)
              {
                StringBuilder stringBuilder16 = OutText;
                num1 = theMemoryBlock.BlockStartAddress;
                string str29 = num1.ToString("x04");
                stringBuilder16.Append(str29);
              }
              OutText.Append(lineStartSpaces + "  ->= ");
              int num19 = theMemoryBlock.StartAddressOfNextBlock - childMemoryBlock.StartAddressOfNextBlock;
              OutText.Append(num19.ToString() + " bytes ");
              if (num19 > 0)
                OutText.AppendLine("free before last block");
              else
                OutText.AppendLine(" overlay before last block");
            }
          }
          if (num18 == theMemoryBlock.parentMemoryBlock.childMemoryBlocks.Count - 1 && theMemoryBlock.parentMemoryBlock.StartAddressOfNextBlock != theMemoryBlock.StartAddressOfNextBlock)
          {
            if (!this.checkBoxSuppressLineAddresses.Checked)
            {
              StringBuilder stringBuilder17 = OutText;
              num1 = theMemoryBlock.BlockStartAddress;
              string str30 = num1.ToString("x04");
              stringBuilder17.Append(str30);
            }
            OutText.Append(lineStartSpaces + "  ->= ");
            int num20 = theMemoryBlock.parentMemoryBlock.StartAddressOfNextBlock - theMemoryBlock.StartAddressOfNextBlock;
            OutText.Append(num20.ToString() + " bytes ");
            if (num20 > 0)
              OutText.AppendLine("free to end of parant block [" + theMemoryBlock.parentMemoryBlock.SegmentType.ToString() + "]");
            else
              OutText.AppendLine("overlay end of parant block [" + theMemoryBlock.parentMemoryBlock.SegmentType.ToString() + "]");
          }
        }
        if (theMemoryBlock.childMemoryBlocks != null)
        {
          string lineStartSpaces1 = lineStartSpaces + " |";
          foreach (S3_MemoryBlock childMemoryBlock in theMemoryBlock.childMemoryBlocks)
          {
            if (!this.GetBlockInfo(SelectedMeter, childMemoryBlock, ref OutText, lineStartSpaces1))
              return false;
          }
        }
      }
      catch (Exception ex)
      {
        OutText.AppendLine("Exception:" + Environment.NewLine + ex.ToString());
        return false;
      }
      return true;
    }

    private void SetLineBeforeLastLine(string lineBefore, ref StringBuilder OutText)
    {
      string str = OutText.ToString();
      int num = OutText.Length - Environment.NewLine.Length - 1;
      while (num >= 0 && OutText.ToString(num, Environment.NewLine.Length) != Environment.NewLine)
        --num;
      if (num < 0)
      {
        OutText.Insert(0, lineBefore);
      }
      else
      {
        OutText.Insert(num, Environment.NewLine + lineBefore);
        str = OutText.ToString();
      }
    }

    private void OutputByte(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      string additinalInfoText,
      ref StringBuilder OutText)
    {
      if (!this.checkBoxSuppressLineAddresses.Checked)
        OutText.Append(address.ToString("X04"));
      OutText.Append(lineStartString);
      if (SelectedMeter.MyDeviceMemory.ByteIsDefined[address])
      {
        if (SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) address))
          OutText.Append("_");
        else
          OutText.Append(" ");
        OutText.Append(SelectedMeter.MyDeviceMemory.MemoryBytes[address].ToString("x02") + "  ");
      }
      else
        OutText.Append(" ??");
      OutText.Append(" ");
      if (additinalInfoText == null)
        return;
      OutText.Append(additinalInfoText);
    }

    private void OutputWord(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      string additinalInfoText,
      ref StringBuilder OutText)
    {
      this.OutputWord(lineStartString, SelectedMeter, address, ref OutText);
      OutText.AppendLine(additinalInfoText);
    }

    private void OutputWord(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      ref StringBuilder OutText)
    {
      if (!this.checkBoxSuppressLineAddresses.Checked)
        OutText.Append(address.ToString("X04"));
      OutText.Append(lineStartString);
      if (SelectedMeter.MyDeviceMemory.ByteIsDefined[address] && SelectedMeter.MyDeviceMemory.ByteIsDefined[address + 1])
      {
        if (SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) address) && SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) (address + 1)))
          OutText.Append("_");
        else
          OutText.Append(" ");
        int num = (int) SelectedMeter.MyDeviceMemory.MemoryBytes[address] + ((int) SelectedMeter.MyDeviceMemory.MemoryBytes[address + 1] << 8);
        OutText.Append(num.ToString("x04"));
      }
      else
        OutText.Append(" ????");
      OutText.Append(" ");
    }

    private void OutputPointer(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      string additinalInfoText,
      ref StringBuilder OutText)
    {
      this.OutputPointer(lineStartString, SelectedMeter, address, ref OutText);
      OutText.AppendLine(additinalInfoText);
    }

    private void OutputPointer(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      ref StringBuilder OutText)
    {
      if (!this.checkBoxSuppressLineAddresses.Checked)
        OutText.Append(address.ToString("X04"));
      OutText.Append(lineStartString);
      if (SelectedMeter.MyDeviceMemory.ByteIsDefined[address] && SelectedMeter.MyDeviceMemory.ByteIsDefined[address + 1])
      {
        if (SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) address) && SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) (address + 1)))
          OutText.Append("_");
        else
          OutText.Append(" ");
        int num = (int) SelectedMeter.MyDeviceMemory.MemoryBytes[address] + ((int) SelectedMeter.MyDeviceMemory.MemoryBytes[address + 1] << 8);
        if (!this.checkBoxSuppressLineAddresses.Checked)
          OutText.Append(num.ToString("x04"));
        else
          OutText.Append("????");
      }
      else
        OutText.Append(" ????");
      OutText.Append(" ");
    }

    private void OutputOnly8_Bytes(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      int bytes,
      ref StringBuilder OutText)
    {
      if (!this.checkBoxSuppressLineAddresses.Checked)
        OutText.Append(address.ToString("X04"));
      OutText.Append(lineStartString);
      for (int index = 0; index < 8; ++index)
      {
        if (bytes > 0)
        {
          if (SelectedMeter.MyDeviceMemory.ByteIsDefined[address + index])
          {
            if (SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) (address + index)))
              OutText.Append("_");
            else
              OutText.Append(" ");
            OutText.Append(SelectedMeter.MyDeviceMemory.MemoryBytes[address + index].ToString("x02"));
          }
          else
            OutText.Append(" ??");
        }
        else
          OutText.Append(" ..");
        --bytes;
      }
      OutText.Append(" ");
    }

    private void OutputByteBlock(
      string lineStartString,
      S3_Meter SelectedMeter,
      int address,
      int bytes,
      ref StringBuilder OutText)
    {
      int num1 = 24;
      Math.Round((Decimal) (bytes / num1), MidpointRounding.AwayFromZero);
      int num2 = address;
      while (bytes > 0)
      {
        if (!this.checkBoxSuppressLineAddresses.Checked)
          OutText.Append(num2.ToString("x04"));
        OutText.Append(lineStartString);
        for (int index = 0; index < num1; ++index)
        {
          if (bytes > 0)
          {
            if (SelectedMeter.MyDeviceMemory.ByteIsDefined[num2 + index])
            {
              if (SelectedMeter.MyWriteProtTableManager.IsByteProtected((ushort) (num2 + index)))
                OutText.Append("_");
              else
                OutText.Append(" ");
              OutText.Append(SelectedMeter.MyDeviceMemory.MemoryBytes[num2 + index].ToString("x02"));
            }
            else
              OutText.Append(" ??");
          }
          else
            OutText.Append(" ..");
          --bytes;
        }
        OutText.AppendLine();
        num2 += num1;
      }
    }

    private void buttonCompareBlockList_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter1 = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter1 == null)
        return;
      S3_Meter selectedMeter2 = this.GetSelectedMeter(this.comboBoxCompareMeter);
      if (selectedMeter2 == null)
        return;
      string blockPrint1 = this.GetBlockPrint(selectedMeter1);
      string blockPrint2 = this.GetBlockPrint(selectedMeter2);
      string str1 = this.WriteInfoFile(this.comboBoxBaseMeter.SelectedItem.ToString(), blockPrint1);
      string str2 = this.WriteInfoFile(this.comboBoxCompareMeter.SelectedItem.ToString(), blockPrint2);
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

    private void buttonPrepareBackupBlocks_Click(object sender, EventArgs e)
    {
      int num = (int) GMM_MessageBox.ShowMessage(nameof (DiagnoseWindow), "Function not implemented");
      if (this.MyFunctions.MyMeters.GetMeterObject(MeterObjects.WorkMeter) != null)
        ;
    }

    private void btnGetMemory_Click(object sender, EventArgs e)
    {
      try
      {
        ushort uint16_1 = Convert.ToUInt16(this.txtStartAddress.Text.Replace("0x", string.Empty), 16);
        ushort uint16_2 = Convert.ToUInt16(this.txtEndAddress.Text.Replace("0x", string.Empty), 16);
        ByteField MemoryData;
        this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.FLASH, (int) uint16_1, (int) uint16_2 - (int) uint16_1, out MemoryData);
        this.txtMemory.Text = Util.ByteArrayToHexString(MemoryData.Data);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void buttonExportResources_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter == null)
        return;
      string resources = this.GetResources(selectedMeter);
      string str = this.WriteInfoFile(this.comboBoxBaseMeter.SelectedItem.ToString(), resources);
      new Process() { StartInfo = { FileName = str } }.Start();
    }

    private string GetResources(S3_Meter selectedMeter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string key in (IEnumerable<string>) selectedMeter.MyResources.AvailableMeterResources.Keys)
        stringBuilder.AppendLine(key);
      return stringBuilder.ToString();
    }

    private void DiagnoseWindow_Load(object sender, EventArgs e)
    {
      if (this.MyFunctions.MyMeters.WorkMeter == null || this.MyFunctions.MyMeters.WorkMeter.GenerateIdentificationChecksum())
        return;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Checksum generation error.");
    }

    private void buttonSaveBaseMeter_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.MyMeters.SaveMeter(this.GetSelectedMeter(this.comboBoxBaseMeter));
      this.UpdateCompareMetersList();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonCompile_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.MyFunctions.MyMeters.WorkMeter != null)
      {
        if (this.MyFunctions.MyMeters.WorkMeter.Compile())
          this.txtMemory.AppendText(Environment.NewLine + "Compile ok");
        else
          this.txtMemory.AppendText(Environment.NewLine + "Compile error");
      }
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Work meter not available");
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonLinkWorkMeter_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.MyFunctions.MyMeters.WorkMeter != null)
      {
        if (this.MyFunctions.MyMeters.WorkMeter.MyLinker.Link(this.MyFunctions.MyMeters.WorkMeter.MyDeviceMemory.meterMemory))
          this.txtMemory.AppendText(Environment.NewLine + "Link ok");
        else
          this.txtMemory.AppendText(Environment.NewLine + "Link error");
      }
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Work meter not available");
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonCloneWorkMeter_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.MyFunctions.MyMeters.WorkMeter != null)
      {
        if (this.MyFunctions.MyMeters.NewWorkMeter("Diagnostic clone"))
          this.txtMemory.AppendText(Environment.NewLine + "Clone ok");
        else
          this.txtMemory.AppendText(Environment.NewLine + "Clone error");
      }
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Work meter not available");
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.UpdateBaseInfos();
    }

    private void btnGetLogger_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter == null)
        return;
      this.txtMemory.AppendText("Main meter: " + this.comboBoxBaseMeter.Text + Environment.NewLine);
      ZR_ClassLibMessages.ClearErrors();
      Dictionary<ushort, string> chanalsByAddresses = selectedMeter.MyLoggerManager.GetLoggerChanalsByAddresses();
      if (chanalsByAddresses != null)
      {
        foreach (KeyValuePair<ushort, string> keyValuePair in chanalsByAddresses)
          this.txtMemory.AppendText(string.Format("0x{0} {1}", (object) keyValuePair.Key.ToString("X4"), (object) keyValuePair.Value) + Environment.NewLine);
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonCompareBlockListNextBackup_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      uint meterId = this.MyFunctions.MyMeters.DbMeter.MyIdentification.MeterId;
      this.MyFunctions.MyMeters.SaveMeter(this.MyFunctions.MyMeters.DbMeter);
      DateTime dateTime = (DateTime) this.MyFunctions.SaveMeterDataTabel.Rows[this.MyFunctions.SelectedSaveMeterDataTableRowIndex]["TimePoint"];
      --this.MyFunctions.SelectedSaveMeterDataTableRowIndex;
      DateTime TimePoint = (DateTime) this.MyFunctions.SaveMeterDataTabel.Rows[this.MyFunctions.SelectedSaveMeterDataTableRowIndex]["TimePoint"];
      this.MyFunctions.MyMeters.ConnectedMeter = (S3_Meter) null;
      this.MyFunctions.MyMeters.WorkMeter = (S3_Meter) null;
      if (this.MyFunctions.OpenDevice((int) meterId, TimePoint))
      {
        string blockPrint1 = this.GetBlockPrint(this.MyFunctions.MyMeters.SavedMeter);
        string blockPrint2 = this.GetBlockPrint(this.MyFunctions.MyMeters.DbMeter);
        string str1 = this.WriteInfoFile("BackupFrom_" + dateTime.ToString("dd.MM.yyyy HH_mm_ss") + "_", blockPrint1);
        string str2 = this.WriteInfoFile("BackupFrom_" + TimePoint.ToString("dd.MM.yyyy HH_mm_ss") + "_", blockPrint2);
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
      if (this.MyFunctions.SelectedSaveMeterDataTableRowIndex == 0)
        this.buttonCompareBlockListNextBackup.Enabled = false;
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonCompareMeterObjects_Click(object sender, EventArgs e)
    {
      S3_Meter selectedMeter1 = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter1 == null)
        return;
      S3_Meter selectedMeter2 = this.GetSelectedMeter(this.comboBoxCompareMeter);
      if (selectedMeter2 == null)
        return;
      this.MyFunctions.CompareMeterObjects(this.comboBoxBaseMeter.SelectedItem.ToString() + ".." + this.comboBoxCompareMeter.SelectedItem.ToString(), selectedMeter1, selectedMeter2);
    }

    private void buttonTypeChecker_Click(object sender, EventArgs e)
    {
      int num = (int) new TypeChecker(this.MyFunctions, this).ShowDialog();
    }

    private void buttonShowComparer_Click(object sender, EventArgs e)
    {
      this.MyFunctions.ShowComparerWindow();
    }

    private void buttonSaveBaseMeterToFile_Click(object sender, EventArgs e)
    {
      this.txtMemory.Clear();
      S3_Meter selectedMeter = this.GetSelectedMeter(this.comboBoxBaseMeter);
      if (selectedMeter == null)
        return;
      string loggDataPath = SystemValues.LoggDataPath;
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.InitialDirectory = loggDataPath;
      saveFileDialog.Filter = "meter backup files (*.mbak)|*.mbak|All files (*.*)|*.*";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false, Encoding.ASCII))
      {
        streamWriter.WriteLine("FirmwareVersion;" + selectedMeter.MyIdentification.FirmwareVersion.ToString());
        streamWriter.WriteLine("HardwareVersion;" + selectedMeter.MyIdentification.HardwareMask.ToString());
        streamWriter.WriteLine("HardwareTypeID;" + selectedMeter.MyIdentification.HardwareTypeId.ToString());
        streamWriter.WriteLine("MapID;" + selectedMeter.MyIdentification.MapId.ToString());
        for (int index = 0; index < selectedMeter.MyDeviceMemory.MemoryBytes.Length; ++index)
        {
          if (selectedMeter.MyDeviceMemory.ByteIsDefined[index])
            streamWriter.WriteLine(index.ToString("x04") + ";" + selectedMeter.MyDeviceMemory.MemoryBytes[index].ToString("x02"));
        }
        streamWriter.Flush();
        streamWriter.Close();
        this.txtMemory.Text = "Save file done";
      }
    }

    private void buttonLoadWorkMeterFromFile_Click(object sender, EventArgs e)
    {
      this.txtMemory.Clear();
      string loggDataPath = SystemValues.LoggDataPath;
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = loggDataPath;
      openFileDialog.Filter = "meter backup files (*.mbak)|*.mbak|All files (*.*)|*.*";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        using (StreamReader streamReader = new StreamReader(openFileDialog.FileName, Encoding.ASCII))
        {
          S3_Meter s3Meter = new S3_Meter(this.MyFunctions, 32768);
          uint FirmwareVersion = 0;
          uint HardwareMask = 0;
          uint HardwareTypeId = 0;
          uint mapId = 0;
          while (true)
          {
            string str = streamReader.ReadLine();
            if (str != null)
            {
              string[] strArray = str.Split(';');
              if (char.IsLetter(strArray[0][0]))
              {
                switch (strArray[0])
                {
                  case "FirmwareVersion":
                    FirmwareVersion = uint.Parse(strArray[1]);
                    break;
                  case "HardwareVersion":
                    HardwareMask = uint.Parse(strArray[1]);
                    break;
                  case "HardwareTypeID":
                    HardwareTypeId = uint.Parse(strArray[1]);
                    break;
                  case "MapID":
                    mapId = uint.Parse(strArray[1]);
                    break;
                }
              }
              else
              {
                int index = int.Parse(strArray[0], NumberStyles.HexNumber);
                byte num = byte.Parse(strArray[1], NumberStyles.HexNumber);
                s3Meter.MyDeviceMemory.MemoryBytes[index] = num;
                s3Meter.MyDeviceMemory.ByteIsDefined[index] = true;
              }
            }
            else
              break;
          }
          s3Meter.MyIdentification.AddIdsFromTypeData(FirmwareVersion, HardwareMask, HardwareTypeId, mapId);
          if (!s3Meter.CreateCompleteFromMemory())
          {
            this.txtMemory.Text = "Create meter error";
          }
          else
          {
            this.MyFunctions.MyMeters.WorkMeter = s3Meter;
            streamReader.Close();
            this.ShowMemoryOverview("Read file done" + Environment.NewLine + Environment.NewLine);
          }
        }
      }
      catch
      {
        this.txtMemory.Text = "Read error";
      }
    }

    private void buttonWriteWorkMeter_Click(object sender, EventArgs e)
    {
      try
      {
        this.MyFunctions.MyMeters.WriteWorkMeterToConnectedDevice();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("S3_Handler", ex.ToString());
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DiagnoseWindow));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.label1 = new Label();
      this.comboBoxBaseMeter = new ComboBox();
      this.label2 = new Label();
      this.comboBoxCompareMeter = new ComboBox();
      this.buttonExportMemory = new Button();
      this.buttonCompareMemory = new Button();
      this.buttonExportBlockList = new Button();
      this.buttonCompareBlockList = new Button();
      this.buttonPrepareBackupBlocks = new Button();
      this.btnGetMemory = new Button();
      this.label3 = new Label();
      this.label4 = new Label();
      this.txtStartAddress = new TextBox();
      this.txtEndAddress = new TextBox();
      this.txtMemory = new TextBox();
      this.buttonExportResources = new Button();
      this.checkBoxUseWinDiff = new CheckBox();
      this.checkBoxSuppressLineAddresses = new CheckBox();
      this.buttonSaveWorkMeter = new Button();
      this.buttonCompile = new Button();
      this.buttonLinkWorkMeter = new Button();
      this.buttonCloneWorkMeter = new Button();
      this.btnGetLogger = new Button();
      this.buttonCompareBlockListNextBackup = new Button();
      this.buttonTypeChecker = new Button();
      this.buttonCompareMeterObjects = new Button();
      this.buttonShowComparer = new Button();
      this.buttonSaveBaseMeterToFile = new Button();
      this.buttonLoadWorkMeterFromFile = new Button();
      this.buttonWriteWorkMeter = new Button();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(839, 105);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 59);
      this.label1.Name = "label1";
      this.label1.Size = new Size(60, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Base meter";
      this.comboBoxBaseMeter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxBaseMeter.FormattingEnabled = true;
      this.comboBoxBaseMeter.Location = new Point(96, 56);
      this.comboBoxBaseMeter.Name = "comboBoxBaseMeter";
      this.comboBoxBaseMeter.Size = new Size(155, 21);
      this.comboBoxBaseMeter.TabIndex = 18;
      this.comboBoxBaseMeter.SelectedIndexChanged += new System.EventHandler(this.comboBoxBaseMeter_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 113);
      this.label2.Name = "label2";
      this.label2.Size = new Size(78, 13);
      this.label2.TabIndex = 17;
      this.label2.Text = "Compare meter";
      this.comboBoxCompareMeter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxCompareMeter.FormattingEnabled = true;
      this.comboBoxCompareMeter.Location = new Point(96, 110);
      this.comboBoxCompareMeter.Name = "comboBoxCompareMeter";
      this.comboBoxCompareMeter.Size = new Size(155, 21);
      this.comboBoxCompareMeter.TabIndex = 18;
      this.buttonExportMemory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonExportMemory.Location = new Point(631, 57);
      this.buttonExportMemory.Name = "buttonExportMemory";
      this.buttonExportMemory.Size = new Size(198, 22);
      this.buttonExportMemory.TabIndex = 19;
      this.buttonExportMemory.Text = "Export memory";
      this.buttonExportMemory.UseVisualStyleBackColor = true;
      this.buttonExportMemory.Click += new System.EventHandler(this.buttonExportMemory_Click);
      this.buttonCompareMemory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCompareMemory.Location = new Point(631, 85);
      this.buttonCompareMemory.Name = "buttonCompareMemory";
      this.buttonCompareMemory.Size = new Size(198, 22);
      this.buttonCompareMemory.TabIndex = 19;
      this.buttonCompareMemory.Text = "Compare memory";
      this.buttonCompareMemory.UseVisualStyleBackColor = true;
      this.buttonCompareMemory.Click += new System.EventHandler(this.buttonCompareMemory_Click);
      this.buttonExportBlockList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonExportBlockList.Location = new Point(631, 137);
      this.buttonExportBlockList.Name = "buttonExportBlockList";
      this.buttonExportBlockList.Size = new Size(198, 22);
      this.buttonExportBlockList.TabIndex = 19;
      this.buttonExportBlockList.Text = "Export block list";
      this.buttonExportBlockList.UseVisualStyleBackColor = true;
      this.buttonExportBlockList.Click += new System.EventHandler(this.buttonExportBlockList_Click);
      this.buttonCompareBlockList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCompareBlockList.Location = new Point(631, 165);
      this.buttonCompareBlockList.Name = "buttonCompareBlockList";
      this.buttonCompareBlockList.Size = new Size(198, 22);
      this.buttonCompareBlockList.TabIndex = 19;
      this.buttonCompareBlockList.Text = "Compare block list";
      this.buttonCompareBlockList.UseVisualStyleBackColor = true;
      this.buttonCompareBlockList.Click += new System.EventHandler(this.buttonCompareBlockList_Click);
      this.buttonPrepareBackupBlocks.Location = new Point(317, 85);
      this.buttonPrepareBackupBlocks.Name = "buttonPrepareBackupBlocks";
      this.buttonPrepareBackupBlocks.Size = new Size(238, 23);
      this.buttonPrepareBackupBlocks.TabIndex = 20;
      this.buttonPrepareBackupBlocks.Text = "Prepare backup blocks";
      this.buttonPrepareBackupBlocks.UseVisualStyleBackColor = true;
      this.buttonPrepareBackupBlocks.Click += new System.EventHandler(this.buttonPrepareBackupBlocks_Click);
      this.btnGetMemory.Location = new Point(11, 302);
      this.btnGetMemory.Name = "btnGetMemory";
      this.btnGetMemory.Size = new Size(75, 23);
      this.btnGetMemory.TabIndex = 21;
      this.btnGetMemory.Text = "Get memory";
      this.btnGetMemory.UseVisualStyleBackColor = true;
      this.btnGetMemory.Click += new System.EventHandler(this.btnGetMemory_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(104, 307);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 13);
      this.label3.TabIndex = 22;
      this.label3.Text = "Start:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(253, 307);
      this.label4.Name = "label4";
      this.label4.Size = new Size(29, 13);
      this.label4.TabIndex = 23;
      this.label4.Text = "End:";
      this.txtStartAddress.Location = new Point(145, 307);
      this.txtStartAddress.Name = "txtStartAddress";
      this.txtStartAddress.Size = new Size(100, 20);
      this.txtStartAddress.TabIndex = 24;
      this.txtStartAddress.Text = "0x4000";
      this.txtEndAddress.Location = new Point(288, 307);
      this.txtEndAddress.Name = "txtEndAddress";
      this.txtEndAddress.Size = new Size(100, 20);
      this.txtEndAddress.TabIndex = 25;
      this.txtEndAddress.Text = "0x5300";
      this.txtMemory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMemory.Location = new Point(12, 333);
      this.txtMemory.Multiline = true;
      this.txtMemory.Name = "txtMemory";
      this.txtMemory.ScrollBars = ScrollBars.Vertical;
      this.txtMemory.Size = new Size(815, 362);
      this.txtMemory.TabIndex = 26;
      this.buttonExportResources.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonExportResources.Location = new Point(317, 234);
      this.buttonExportResources.Name = "buttonExportResources";
      this.buttonExportResources.Size = new Size(238, 22);
      this.buttonExportResources.TabIndex = 19;
      this.buttonExportResources.Text = "ExportResources";
      this.buttonExportResources.UseVisualStyleBackColor = true;
      this.buttonExportResources.Click += new System.EventHandler(this.buttonExportResources_Click);
      this.checkBoxUseWinDiff.AutoSize = true;
      this.checkBoxUseWinDiff.Location = new Point(12, 239);
      this.checkBoxUseWinDiff.Name = "checkBoxUseWinDiff";
      this.checkBoxUseWinDiff.Size = new Size(142, 17);
      this.checkBoxUseWinDiff.TabIndex = 27;
      this.checkBoxUseWinDiff.Text = "Use WinDiff for compare";
      this.checkBoxUseWinDiff.UseVisualStyleBackColor = true;
      this.checkBoxSuppressLineAddresses.AutoSize = true;
      this.checkBoxSuppressLineAddresses.Location = new Point(12, 262);
      this.checkBoxSuppressLineAddresses.Name = "checkBoxSuppressLineAddresses";
      this.checkBoxSuppressLineAddresses.Size = new Size(140, 17);
      this.checkBoxSuppressLineAddresses.TabIndex = 27;
      this.checkBoxSuppressLineAddresses.Text = "Suppress line addresses";
      this.checkBoxSuppressLineAddresses.UseVisualStyleBackColor = true;
      this.buttonSaveWorkMeter.Location = new Point(145, 81);
      this.buttonSaveWorkMeter.Name = "buttonSaveWorkMeter";
      this.buttonSaveWorkMeter.Size = new Size(106, 23);
      this.buttonSaveWorkMeter.TabIndex = 20;
      this.buttonSaveWorkMeter.Text = "Save base meter";
      this.buttonSaveWorkMeter.UseVisualStyleBackColor = true;
      this.buttonSaveWorkMeter.Click += new System.EventHandler(this.buttonSaveBaseMeter_Click);
      this.buttonCompile.Location = new Point(317, 116);
      this.buttonCompile.Name = "buttonCompile";
      this.buttonCompile.Size = new Size(238, 23);
      this.buttonCompile.TabIndex = 20;
      this.buttonCompile.Text = "Compile work meter";
      this.buttonCompile.UseVisualStyleBackColor = true;
      this.buttonCompile.Click += new System.EventHandler(this.buttonCompile_Click);
      this.buttonLinkWorkMeter.Location = new Point(317, 147);
      this.buttonLinkWorkMeter.Name = "buttonLinkWorkMeter";
      this.buttonLinkWorkMeter.Size = new Size(238, 23);
      this.buttonLinkWorkMeter.TabIndex = 20;
      this.buttonLinkWorkMeter.Text = "Link work meter";
      this.buttonLinkWorkMeter.UseVisualStyleBackColor = true;
      this.buttonLinkWorkMeter.Click += new System.EventHandler(this.buttonLinkWorkMeter_Click);
      this.buttonCloneWorkMeter.Location = new Point(317, 54);
      this.buttonCloneWorkMeter.Name = "buttonCloneWorkMeter";
      this.buttonCloneWorkMeter.Size = new Size(238, 23);
      this.buttonCloneWorkMeter.TabIndex = 20;
      this.buttonCloneWorkMeter.Text = "Clone work meter";
      this.buttonCloneWorkMeter.UseVisualStyleBackColor = true;
      this.buttonCloneWorkMeter.Click += new System.EventHandler(this.buttonCloneWorkMeter_Click);
      this.btnGetLogger.Location = new Point(317, 176);
      this.btnGetLogger.Name = "btnGetLogger";
      this.btnGetLogger.Size = new Size(238, 23);
      this.btnGetLogger.TabIndex = 28;
      this.btnGetLogger.Text = "Get Logger";
      this.btnGetLogger.UseVisualStyleBackColor = true;
      this.btnGetLogger.Click += new System.EventHandler(this.btnGetLogger_Click);
      this.buttonCompareBlockListNextBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCompareBlockListNextBackup.Location = new Point(631, 193);
      this.buttonCompareBlockListNextBackup.Name = "buttonCompareBlockListNextBackup";
      this.buttonCompareBlockListNextBackup.Size = new Size(198, 22);
      this.buttonCompareBlockListNextBackup.TabIndex = 19;
      this.buttonCompareBlockListNextBackup.Text = "Compare block list next backup";
      this.buttonCompareBlockListNextBackup.UseVisualStyleBackColor = true;
      this.buttonCompareBlockListNextBackup.Click += new System.EventHandler(this.buttonCompareBlockListNextBackup_Click);
      this.buttonTypeChecker.Location = new Point(317, 205);
      this.buttonTypeChecker.Name = "buttonTypeChecker";
      this.buttonTypeChecker.Size = new Size(238, 23);
      this.buttonTypeChecker.TabIndex = 28;
      this.buttonTypeChecker.Text = "Type checker ...";
      this.buttonTypeChecker.UseVisualStyleBackColor = true;
      this.buttonTypeChecker.Click += new System.EventHandler(this.buttonTypeChecker_Click);
      this.buttonCompareMeterObjects.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCompareMeterObjects.Location = new Point(631, 234);
      this.buttonCompareMeterObjects.Name = "buttonCompareMeterObjects";
      this.buttonCompareMeterObjects.Size = new Size(198, 22);
      this.buttonCompareMeterObjects.TabIndex = 19;
      this.buttonCompareMeterObjects.Text = "Compare meter objects";
      this.buttonCompareMeterObjects.UseVisualStyleBackColor = true;
      this.buttonCompareMeterObjects.Click += new System.EventHandler(this.buttonCompareMeterObjects_Click);
      this.buttonShowComparer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonShowComparer.Location = new Point(631, 262);
      this.buttonShowComparer.Name = "buttonShowComparer";
      this.buttonShowComparer.Size = new Size(198, 22);
      this.buttonShowComparer.TabIndex = 19;
      this.buttonShowComparer.Text = "Show comparer";
      this.buttonShowComparer.UseVisualStyleBackColor = true;
      this.buttonShowComparer.Click += new System.EventHandler(this.buttonShowComparer_Click);
      this.buttonSaveBaseMeterToFile.Location = new Point(15, 176);
      this.buttonSaveBaseMeterToFile.Name = "buttonSaveBaseMeterToFile";
      this.buttonSaveBaseMeterToFile.Size = new Size(236, 23);
      this.buttonSaveBaseMeterToFile.TabIndex = 29;
      this.buttonSaveBaseMeterToFile.Text = "Save base meter to file";
      this.buttonSaveBaseMeterToFile.UseVisualStyleBackColor = true;
      this.buttonSaveBaseMeterToFile.Click += new System.EventHandler(this.buttonSaveBaseMeterToFile_Click);
      this.buttonLoadWorkMeterFromFile.Location = new Point(14, 205);
      this.buttonLoadWorkMeterFromFile.Name = "buttonLoadWorkMeterFromFile";
      this.buttonLoadWorkMeterFromFile.Size = new Size(236, 23);
      this.buttonLoadWorkMeterFromFile.TabIndex = 29;
      this.buttonLoadWorkMeterFromFile.Text = "Load work meter from file";
      this.buttonLoadWorkMeterFromFile.UseVisualStyleBackColor = true;
      this.buttonLoadWorkMeterFromFile.Click += new System.EventHandler(this.buttonLoadWorkMeterFromFile_Click);
      this.buttonWriteWorkMeter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonWriteWorkMeter.Location = new Point(317, 262);
      this.buttonWriteWorkMeter.Name = "buttonWriteWorkMeter";
      this.buttonWriteWorkMeter.Size = new Size(238, 22);
      this.buttonWriteWorkMeter.TabIndex = 19;
      this.buttonWriteWorkMeter.Text = "Write WorkMeter to connected device";
      this.buttonWriteWorkMeter.UseVisualStyleBackColor = true;
      this.buttonWriteWorkMeter.Click += new System.EventHandler(this.buttonWriteWorkMeter_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(839, 710);
      this.Controls.Add((Control) this.buttonLoadWorkMeterFromFile);
      this.Controls.Add((Control) this.buttonSaveBaseMeterToFile);
      this.Controls.Add((Control) this.buttonTypeChecker);
      this.Controls.Add((Control) this.btnGetLogger);
      this.Controls.Add((Control) this.checkBoxSuppressLineAddresses);
      this.Controls.Add((Control) this.checkBoxUseWinDiff);
      this.Controls.Add((Control) this.txtMemory);
      this.Controls.Add((Control) this.txtEndAddress);
      this.Controls.Add((Control) this.txtStartAddress);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnGetMemory);
      this.Controls.Add((Control) this.buttonCloneWorkMeter);
      this.Controls.Add((Control) this.buttonSaveWorkMeter);
      this.Controls.Add((Control) this.buttonLinkWorkMeter);
      this.Controls.Add((Control) this.buttonCompile);
      this.Controls.Add((Control) this.buttonPrepareBackupBlocks);
      this.Controls.Add((Control) this.buttonWriteWorkMeter);
      this.Controls.Add((Control) this.buttonExportResources);
      this.Controls.Add((Control) this.buttonShowComparer);
      this.Controls.Add((Control) this.buttonCompareMeterObjects);
      this.Controls.Add((Control) this.buttonCompareBlockListNextBackup);
      this.Controls.Add((Control) this.buttonCompareBlockList);
      this.Controls.Add((Control) this.buttonCompareMemory);
      this.Controls.Add((Control) this.buttonExportBlockList);
      this.Controls.Add((Control) this.buttonExportMemory);
      this.Controls.Add((Control) this.comboBoxCompareMeter);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.comboBoxBaseMeter);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (DiagnoseWindow);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (DiagnoseWindow);
      this.Load += new System.EventHandler(this.DiagnoseWindow_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
