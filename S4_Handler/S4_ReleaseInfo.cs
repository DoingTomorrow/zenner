// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_ReleaseInfo
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using Microsoft.Win32;
using S4_Handler.Functions;
using SmartFunctionCompiler;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.ExcelOpenDocument;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler
{
  public class S4_ReleaseInfo : Window, IComponentConnector
  {
    private S4_Meter TheMeter;
    private GMMConfig GmmConfig;
    private List<S4_ReleaseInfo.InfoEntry> InfoList;
    internal Button ButtonWriteTypeDoc;
    internal Button ButtonWriteTestTypeDoc;
    internal TextBox TextBoxPath;
    internal Button ButtonSelect;
    internal TextBox TextBoxFile;
    internal TextBox TextBoxOut;
    private bool _contentLoaded;

    internal S4_ReleaseInfo(S4_Meter theMeter, GMMConfig gmmConfig)
    {
      if (theMeter == null)
        throw new Exception("Meter object not defined");
      if (!theMeter.deviceIdentification.SAP_MaterialNumber.HasValue)
        throw new Exception("SAP number inside the meter data not defined. Is the read complete?");
      this.Title = this.Title + "  SAP number: " + theMeter.deviceIdentification.SAP_MaterialNumber.Value.ToString();
      this.TheMeter = theMeter;
      this.GmmConfig = gmmConfig;
      this.InitializeComponent();
      if (gmmConfig != null)
        this.TextBoxPath.Text = this.GmmConfig.GetValue("S4_Handler", "ReleasePath");
      this.CreateReleaseInfoList();
      int maxParamLength = 0;
      foreach (S4_ReleaseInfo.InfoEntry info in this.InfoList)
      {
        if (info.ParameterValue != null && info.ParameterName.Length > maxParamLength)
          maxParamLength = info.ParameterName.Length;
      }
      StringBuilder stringBuilder = new StringBuilder();
      string str = "";
      foreach (S4_ReleaseInfo.InfoEntry info in this.InfoList)
      {
        if (info.Group != str)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.AppendLine();
          stringBuilder.AppendLine(info.Group);
          str = info.Group;
        }
        stringBuilder.AppendLine("   " + info.ToString(maxParamLength));
      }
      this.TextBoxOut.Text = stringBuilder.ToString();
    }

    private void ButtonSelect_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.DefaultExt = ".xlsm";
      openFileDialog.CheckFileExists = true;
      openFileDialog.InitialDirectory = this.TextBoxPath.Text;
      openFileDialog.Filter = "Excel macro file (.xlsm)|*.xlsm|Excel file (.xlsx)|*.xlsx|All Files|*.*";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.TextBoxPath.Text = Path.GetDirectoryName(openFileDialog.FileName);
      this.TextBoxFile.Text = Path.GetFileName(openFileDialog.FileName);
      if (this.GmmConfig != null)
        this.GmmConfig.SetOrUpdateValue("S4_Handler", "ReleasePath", this.TextBoxPath.Text);
      this.CheckTypeDocExists();
    }

    private void CheckTypeDocExists()
    {
      this.ButtonWriteTypeDoc.IsEnabled = File.Exists(Path.Combine(this.TextBoxPath.Text, this.TextBoxFile.Text));
    }

    private void ButtonWriteTypeDoc_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.CreateExcelFile(Path.Combine(this.TextBoxPath.Text, this.TextBoxFile.Text));
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by creating TypeDoc");
      }
    }

    private void ButtonWriteTestTypeDoc_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.DefaultExt = ".xlsx";
      openFileDialog.CheckFileExists = false;
      openFileDialog.Filter = "Excel file (.xlsx)|*.xlsx";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      try
      {
        ExcelOdWorkbook excelOdWorkbook = new ExcelOdWorkbook();
        excelOdWorkbook.New(openFileDialog.FileName, "TypeDoc");
        ExcelOdSheet sheet = excelOdWorkbook.GetSheet("TypeDoc");
        sheet.SetCellString("SAP-No.", "A", 1U);
        sheet.SetCellNumber((double) this.TheMeter.deviceIdentification.SAP_MaterialNumber.Value, "B", 1U);
        sheet.Save();
        excelOdWorkbook.Close();
        this.CreateExcelFile(openFileDialog.FileName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        return;
      }
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void CreateReleaseInfoList()
    {
      this.InfoList = new List<S4_ReleaseInfo.InfoEntry>();
      string group1 = "Identification (check by handler 'Device identification' and device and label prints)";
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Check: all FD_??? values are equal to current values", (object) null, group1));
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Check: all xxxxxxxx value parts are equal", (object) null, group1));
      string parameterName1 = "PrintedSerialNumber";
      if (this.TheMeter.deviceIdentification.PrintedSerialNumberAsString == null)
        throw new Exception(parameterName1 + " not defined");
      string serialNumberAsString = this.TheMeter.deviceIdentification.PrintedSerialNumberAsString;
      string parameterValue1 = serialNumberAsString.Substring(0, serialNumberAsString.Length - 8) + "xxxxxxxx";
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName1, (object) parameterValue1, group1));
      string parameterName2 = "FullSerialNumber";
      if (this.TheMeter.deviceIdentification.PrintedSerialNumberAsString == null)
        throw new Exception(parameterName2 + " not defined");
      string fullSerialNumber = this.TheMeter.deviceIdentification.FullSerialNumber;
      string parameterValue2 = fullSerialNumber.Substring(0, fullSerialNumber.Length - 8) + "xxxxxxxx";
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName2, (object) parameterValue2, group1));
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Manufacturer", (object) this.TheMeter.deviceIdentification.ManufacturerName, group1));
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Medium", (object) this.TheMeter.deviceIdentification.GetMediumAsText(), group1));
      string parameterName3 = "AES_Key";
      if (this.TheMeter.deviceIdentification.AES_Key != null)
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName3, (object) "Defined", group1));
      else
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName3, (object) "not available", group1));
      string parameterName4 = "LoRa_DevEUI";
      if (this.TheMeter.deviceIdentification.LoRa_DevEUI_AsString == null)
        throw new Exception(parameterName4 + " not defined");
      string raDevEuiAsString = this.TheMeter.deviceIdentification.LoRa_DevEUI_AsString;
      string parameterValue3 = raDevEuiAsString.Substring(0, raDevEuiAsString.Length - 8) + "xxxxxxxx";
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName4, (object) parameterValue3, group1));
      string parameterName5 = "LoRa_JoinEUI";
      if (this.TheMeter.deviceIdentification.LoRa_JoinEUI_AsString == null)
        throw new Exception(parameterName5 + " not defined");
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName5, (object) this.TheMeter.deviceIdentification.LoRa_JoinEUI_AsString, group1));
      string parameterName6 = "LoRa_AppKey";
      if (this.TheMeter.deviceIdentification.LoRa_AppKey == null)
        throw new Exception(parameterName6 + " not defined");
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(parameterName6, (object) "Defined", group1));
      ConfigurationParameter.ActiveConfigurationLevel = ConfigurationLevel.Advanced;
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.TheMeter.GetConfigurationParameters("Load parameter for ReleaseInfo");
      string group2 = "Configuration (check by handler 'Prepare release info')";
      int index1 = configurationParameters.IndexOfKey(OverrideID.DeviceClock);
      if (index1 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index1];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.DeviceClock.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index2 = configurationParameters.IndexOfKey(OverrideID.TimeZone);
      if (index2 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index2];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.TimeZone.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index3 = configurationParameters.IndexOfKey(OverrideID.DueDate);
      if (index3 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index3];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.DueDate.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index4 = configurationParameters.IndexOfKey(OverrideID.EndOfBatteryDate);
      if (index4 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index4];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.EndOfBatteryDate.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index5 = configurationParameters.IndexOfKey(OverrideID.BatteryCapacity_mAh);
      if (index5 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index5];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.BatteryCapacity_mAh.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index6 = configurationParameters.IndexOfKey(OverrideID.VolumeResolution);
      if (index6 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index6];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.VolumeResolution.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index7 = configurationParameters.IndexOfKey(OverrideID.VolumeActualValue);
      if (index7 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index7];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.VolumeActualValue.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      int index8 = configurationParameters.IndexOfKey(OverrideID.CommunicationScenario);
      if (index8 >= 0)
      {
        ConfigurationParameter configurationParameter = configurationParameters.Values[index8];
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(OverrideID.CommunicationScenario.ToString(), (object) configurationParameter.GetStringValueWin(), group2));
      }
      string group3 = "Flow levels (check by handler 'Prepare release info')";
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Qmax", (object) this.TheMeter.meterMemory.GetParameterValue<float>(S4_Params.maxPositivFlow).ToString(), group3));
      this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Qmin", (object) this.TheMeter.meterMemory.GetParameterValue<float>(S4_Params.minPositivFlow).ToString(), group3));
      if (new FirmwareVersion(this.TheMeter.deviceIdentification.FirmwareVersion.Value) >= (object) "1.4.3 IUW")
      {
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Q4", (object) this.TheMeter.meterMemory.GetParameterValue<float>(S4_Params.overloadFlowrateQ4).ToString(), group3));
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Q3", (object) this.TheMeter.meterMemory.GetParameterValue<float>(S4_Params.permanentFlowrateQ3).ToString(), group3));
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Q2", (object) this.TheMeter.meterMemory.GetParameterValue<float>(S4_Params.transitionalFlowrateQ2).ToString(), group3));
        this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("Q1", (object) this.TheMeter.meterMemory.GetParameterValue<float>(S4_Params.minimumFlowrateQ1).ToString(), group3));
      }
      string group4 = "Smart functions (check by handler 'Smart functions/Read functions list from device')";
      try
      {
        if (this.TheMeter.meterMemory.SmartFunctionFlashRange != null)
        {
          List<SmartFunctionIdentAndFlashParams> functionsFromMemory = this.TheMeter.MySmartFunctionManager.FunctionsFromMemory;
          if (functionsFromMemory == null || functionsFromMemory.Count == 0)
          {
            this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("No smart functions", (object) null, group4));
          }
          else
          {
            foreach (SmartFunctionIdentAndFlashParams identAndFlashParams in functionsFromMemory)
            {
              this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(identAndFlashParams.Name, (object) null, group4));
              if (identAndFlashParams.FlashParameters != null && identAndFlashParams.FlashParameters.Count != 0)
              {
                foreach (KeyValuePair<string, string> flashParameter in identAndFlashParams.FlashParameters)
                  this.InfoList.Add(new S4_ReleaseInfo.InfoEntry("-> " + flashParameter.Key, (object) flashParameter.Value, group4));
              }
            }
          }
        }
      }
      catch
      {
      }
      string group5 = "Loaded scenarios (check by handler 'Prepare release info')";
      try
      {
        S4_ScenarioManager s4ScenarioManager = new S4_ScenarioManager((S4_DeviceCommandsNFC) null, this.TheMeter.meterMemory);
        if (!s4ScenarioManager.PrepareConfigurationFromMap())
          throw new Exception("Scenarios not available inside the map");
        foreach (KeyValuePair<string, string> scenariosListFrom in s4ScenarioManager.GetShortScenariosListFromMap())
          this.InfoList.Add(new S4_ReleaseInfo.InfoEntry(scenariosListFrom.Key, (object) scenariosListFrom.Value, group5));
      }
      catch
      {
      }
    }

    private void CreateExcelFile(string filePath)
    {
      ExcelOdWorkbook excelOdWorkbook = new ExcelOdWorkbook();
      excelOdWorkbook.Open(filePath);
      try
      {
        ExcelOdSheet sheet = excelOdWorkbook.GetSheet("TypeDoc");
        string cellValue1 = sheet.GetCellValue("A", 1U);
        if (cellValue1 == null || cellValue1 != "SAP-No.")
          throw new Exception("SAP_NumberTag 'SAP-No.' in cell A1 not found");
        string cellValue2 = sheet.GetCellValue("B", 1U);
        string str1 = this.TheMeter.deviceIdentification.SAP_MaterialNumber.ToString();
        if (str1 != cellValue2)
          throw new Exception("The required SAP number = " + cellValue2 + "; Device SAP number = " + str1);
        uint rowIndex1 = 4;
        string str2 = "";
        foreach (S4_ReleaseInfo.InfoEntry info in this.InfoList)
        {
          if (info.Group != str2)
          {
            str2 = info.Group;
            uint rowIndex2 = rowIndex1 + 1U;
            sheet.SetCellString(info.Group, "A", rowIndex2);
            rowIndex1 = rowIndex2 + 1U;
          }
          sheet.SetCellString(info.ParameterName, "A", rowIndex1);
          if (info.ParameterValue != null)
          {
            if (info.ParameterValue.GetType() == typeof (double))
              sheet.SetCellNumber((double) info.ParameterValue, "B", rowIndex1);
            else if (info.ParameterValue.GetType() == typeof (int))
              sheet.SetCellNumber((double) (int) info.ParameterValue, "B", rowIndex1);
            else
              sheet.SetCellString((string) info.ParameterValue, "B", rowIndex1);
          }
          ++rowIndex1;
        }
        sheet.Save();
        excelOdWorkbook.Close();
        new Process()
        {
          StartInfo = {
            FileName = filePath
          }
        }.Start();
        excelOdWorkbook = (ExcelOdWorkbook) null;
      }
      finally
      {
        excelOdWorkbook?.Close();
      }
    }

    private void TextBoxFile_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.CheckTypeDocExists();
    }

    private static double RoundProMil(double value)
    {
      double num = 1.0;
      while (value * num < 1024.0)
        num *= 2.0;
      while (value * num > 2048.0)
        num /= 2.0;
      return Math.Round(value * num) / num;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_releaseinfo.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonWriteTypeDoc = (Button) target;
          this.ButtonWriteTypeDoc.Click += new RoutedEventHandler(this.ButtonWriteTypeDoc_Click);
          break;
        case 2:
          this.ButtonWriteTestTypeDoc = (Button) target;
          this.ButtonWriteTestTypeDoc.Click += new RoutedEventHandler(this.ButtonWriteTestTypeDoc_Click);
          break;
        case 3:
          this.TextBoxPath = (TextBox) target;
          break;
        case 4:
          this.ButtonSelect = (Button) target;
          this.ButtonSelect.Click += new RoutedEventHandler(this.ButtonSelect_Click);
          break;
        case 5:
          this.TextBoxFile = (TextBox) target;
          this.TextBoxFile.TextChanged += new TextChangedEventHandler(this.TextBoxFile_TextChanged);
          break;
        case 6:
          this.TextBoxOut = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    internal class InfoEntry : IComparable<S4_ReleaseInfo.InfoEntry>
    {
      internal string ParameterName;
      internal string Group = "NotDefined";
      internal object ParameterValue;

      internal InfoEntry()
      {
      }

      internal InfoEntry(string parameterName, object parameterValue, string group = "NotDefined")
      {
        this.ParameterName = parameterName;
        this.ParameterValue = parameterValue;
        this.Group = group;
      }

      public int CompareTo(S4_ReleaseInfo.InfoEntry compareObject)
      {
        int num = this.Group.CompareTo(compareObject.Group);
        return num != 0 ? num : this.ParameterName.CompareTo(compareObject.ParameterName);
      }

      public override string ToString()
      {
        return this.ParameterName + ": " + this.ParameterValue.ToString();
      }

      public string ToString(int maxParamLength)
      {
        string str1 = this.ParameterName;
        if (this.ParameterValue != null)
        {
          string str2 = str1 + ":";
          if (str2.Length < maxParamLength + 1)
            str2 += " ";
          if (str2.Length < maxParamLength + 1)
            str2 = str2.PadRight(maxParamLength + 1, '.');
          str1 = str2 + " " + this.ParameterValue.ToString();
        }
        return str1;
      }
    }
  }
}
