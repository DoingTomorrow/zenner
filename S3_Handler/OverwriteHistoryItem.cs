// Decompiled with JetBrains decompiler
// Type: S3_Handler.OverwriteHistoryItem
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class OverwriteHistoryItem
  {
    internal string FlowInfo;
    internal uint FirmwareVersion;
    internal uint SAP_MaterialNumber;
    internal uint MeterInfoId;
    internal bool[] UsedOverwriteSelection;
    internal OverwriteOptions UsedOverwriteOptions;
    internal List<ConfigurationParameter> ConfigParameterList;
    internal string ConfigPart;

    internal OverwriteHistoryItem(string flowInfo) => this.FlowInfo = flowInfo;

    internal OverwriteHistoryItem(
      uint firmwareVersion,
      uint sAP_MaterialNumber,
      uint meterInfoId,
      bool[] overwriteSelection,
      OverwriteOptions overwriteOptions)
    {
      this.FirmwareVersion = firmwareVersion;
      this.SAP_MaterialNumber = sAP_MaterialNumber;
      this.MeterInfoId = meterInfoId;
      this.UsedOverwriteSelection = overwriteSelection;
      this.UsedOverwriteOptions = overwriteOptions;
    }

    internal OverwriteHistoryItem(
      string configPart,
      SortedList<OverrideID, ConfigurationParameter> configParameterList)
    {
      try
      {
        this.ConfigPart = configPart;
        this.ConfigParameterList = new List<ConfigurationParameter>();
        foreach (ConfigurationParameter configurationParameter in (IEnumerable<ConfigurationParameter>) configParameterList.Values)
          this.ConfigParameterList.Add(configurationParameter.Clone());
      }
      catch (Exception ex)
      {
        this.ConfigParameterList = (List<ConfigurationParameter>) null;
        this.FlowInfo = "Configuration list exceptione";
      }
    }

    internal OverwriteHistoryItem Clone() => (OverwriteHistoryItem) this.MemberwiseClone();

    public string ToString(ref int level)
    {
      string str = "";
      for (int index = 0; index < level; ++index)
        str += "   ";
      if (this.FlowInfo != null)
      {
        if (this.FlowInfo == "{")
        {
          ++level;
          return str + "{ // Start of type creating by temporary using of WorkMeter" + Environment.NewLine;
        }
        if (this.FlowInfo == "}")
        {
          --level;
          str = "";
          for (int index = 0; index < level; ++index)
            str += "   ";
        }
        return str + "Flow info: " + this.FlowInfo + Environment.NewLine;
      }
      StringBuilder stringBuilder = new StringBuilder();
      if (this.ConfigParameterList != null)
      {
        stringBuilder.AppendLine();
        int length = stringBuilder.Length;
        stringBuilder.Append(str);
        stringBuilder.Append("* Configuration(" + this.ConfigPart + "): ");
        foreach (ConfigurationParameter configParameter in this.ConfigParameterList)
        {
          if (stringBuilder.Length - length > 130)
          {
            stringBuilder.AppendLine();
            length = stringBuilder.Length;
            stringBuilder.Append(str + "  + ");
          }
          else
            stringBuilder.Append("; ");
          stringBuilder.Append(configParameter.ParameterID.ToString() + "=" + configParameter.GetStringValueDb());
        }
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        return stringBuilder.ToString();
      }
      stringBuilder.AppendLine();
      stringBuilder.Append(str);
      stringBuilder.Append("* Overwrite from: MeterInfoId: " + this.MeterInfoId.ToString("d04"));
      stringBuilder.Append("; Firmware: " + new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion).ToString());
      stringBuilder.Append("; SAP: " + this.SAP_MaterialNumber.ToString());
      stringBuilder.AppendLine("; OverwriteOptions: " + this.UsedOverwriteOptions.ToString());
      int length1 = stringBuilder.Length;
      stringBuilder.Append(str);
      stringBuilder.Append("* OverwriteSelections: ");
      for (int index = 0; index < this.UsedOverwriteSelection.Length; ++index)
      {
        if (this.UsedOverwriteSelection[index])
        {
          if (stringBuilder.Length - length1 > 130)
          {
            stringBuilder.AppendLine();
            length1 = stringBuilder.Length;
            stringBuilder.Append(str + "  + ");
          }
          else
            stringBuilder.Append("; ");
          OverwriteSelections overwriteSelections = (OverwriteSelections) index;
          stringBuilder.Append(overwriteSelections.ToString());
        }
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine();
      return stringBuilder.ToString();
    }
  }
}
