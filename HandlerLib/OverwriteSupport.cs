// Decompiled with JetBrains decompiler
// Type: HandlerLib.OverwriteSupport
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace HandlerLib
{
  public static class OverwriteSupport
  {
    public static SortedList<string, CommonOverwriteGroups> OverwriteGroupProperties = new SortedList<string, CommonOverwriteGroups>();

    static OverwriteSupport()
    {
      OverwriteSupport.OverwriteGroupProperties.Add("ID", CommonOverwriteGroups.IdentData);
      OverwriteSupport.OverwriteGroupProperties.Add("TI", CommonOverwriteGroups.TypeIdentification);
      OverwriteSupport.OverwriteGroupProperties.Add("UC", CommonOverwriteGroups.UltrasonicCalibration);
      OverwriteSupport.OverwriteGroupProperties.Add("UH", CommonOverwriteGroups.UltrasonicHydraulicTestSetup);
      OverwriteSupport.OverwriteGroupProperties.Add("UL", CommonOverwriteGroups.UltrasonicLimits);
      OverwriteSupport.OverwriteGroupProperties.Add("UT", CommonOverwriteGroups.UltrasonicTempSensorCurve);
      OverwriteSupport.OverwriteGroupProperties.Add("MD", CommonOverwriteGroups.MenuDefinition);
      OverwriteSupport.OverwriteGroupProperties.Add("RD", CommonOverwriteGroups.RamData);
      OverwriteSupport.OverwriteGroupProperties.Add("BC", CommonOverwriteGroups.BasicConfiguration);
      OverwriteSupport.OverwriteGroupProperties.Add("TS", CommonOverwriteGroups.TemperatureSettings);
      OverwriteSupport.OverwriteGroupProperties.Add("TL", CommonOverwriteGroups.TemperatureLimits);
      OverwriteSupport.OverwriteGroupProperties.Add("DP", CommonOverwriteGroups.DeviceProtection);
      OverwriteSupport.OverwriteGroupProperties.Add("RC", CommonOverwriteGroups.RTC_Calibration);
      OverwriteSupport.OverwriteGroupProperties.Add("CP", CommonOverwriteGroups.ConfigurationParameters);
      OverwriteSupport.OverwriteGroupProperties.Add("CC", CommonOverwriteGroups.CarrierFrequencyCalibration);
      OverwriteSupport.OverwriteGroupProperties.Add("ZC", CommonOverwriteGroups.ZeroFlowCalibration);
    }

    public static CommonOverwriteGroups GetOverwriteGroupFromShortcat(string shortcut)
    {
      return OverwriteSupport.OverwriteGroupProperties.ContainsKey(shortcut) ? OverwriteSupport.OverwriteGroupProperties[shortcut] : throw new ArgumentException("Unknown overwrite shortcut:" + shortcut);
    }

    public static string GetShortcatFromOverwriteGroup(CommonOverwriteGroups theGroup)
    {
      int index = OverwriteSupport.OverwriteGroupProperties.IndexOfValue(theGroup);
      if (index < 0)
        throw new ArgumentException("Unknown overwrite group:" + theGroup.ToString());
      return OverwriteSupport.OverwriteGroupProperties.Keys[index];
    }

    public static CommonOverwriteGroups[] GetIncludingOverwriteGroups(CommonOverwriteGroups theGroup)
    {
      switch (theGroup)
      {
        case CommonOverwriteGroups.UltrasonicLimits:
          return new CommonOverwriteGroups[1]
          {
            CommonOverwriteGroups.UltrasonicCalibration
          };
        case CommonOverwriteGroups.UltrasonicTempSensorCurve:
          return new CommonOverwriteGroups[1]
          {
            CommonOverwriteGroups.UltrasonicCalibration
          };
        default:
          return new CommonOverwriteGroups[0];
      }
    }

    public static List<TypeOverwriteData> PrepareOverwriteData(string overwriteString)
    {
      if (string.IsNullOrEmpty(overwriteString))
        throw new ArgumentException("illegal overwriteString");
      List<TypeOverwriteData> typeOverwriteDataList = new List<TypeOverwriteData>();
      string[] strArray1 = overwriteString.Split(new char[1]
      {
        '|'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length < 1)
        throw new ArgumentException("illegal overwriteString");
      bool flag = true;
      foreach (string str1 in strArray1)
      {
        TypeOverwriteData typeOverwriteData = new TypeOverwriteData();
        string[] strArray2 = str1.Split(new char[1]{ ':' }, StringSplitOptions.RemoveEmptyEntries);
        string str2;
        if (strArray2.Length == 1)
        {
          str2 = strArray2[0];
        }
        else
        {
          if (strArray2.Length != 2)
            throw new ArgumentException("illegal conditions");
          if (flag)
            throw new ArgumentException("Basetype can not have a condition");
          OverwriteConditions result;
          if (!Enum.TryParse<OverwriteConditions>(strArray2[0], out result))
            throw new ArgumentException("unknown condition");
          typeOverwriteData.Condition = result;
          str2 = strArray2[1];
        }
        string[] strArray3 = str2.Split(new char[1]{ '=' }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray3.Length < 1 || strArray3.Length > 2)
          throw new ArgumentException("illegal group definition");
        int result1;
        if (!int.TryParse(strArray3[0], out result1))
          throw new ArgumentException("illegal MeterInfoID");
        typeOverwriteData.MeterInfoID = result1;
        if (strArray3.Length == 2)
        {
          if (flag)
            throw new ArgumentException("Basetype can not have groups");
          string[] strArray4 = strArray3[1].Split(new char[1]
          {
            ','
          }, StringSplitOptions.RemoveEmptyEntries);
          List<CommonOverwriteGroups> commonOverwriteGroupsList = new List<CommonOverwriteGroups>();
          foreach (string shortcut in strArray4)
            commonOverwriteGroupsList.Add(OverwriteSupport.GetOverwriteGroupFromShortcat(shortcut));
          typeOverwriteData.OverwriteGroups = commonOverwriteGroupsList.ToArray();
        }
        typeOverwriteDataList.Add(typeOverwriteData);
        flag = false;
      }
      return typeOverwriteDataList;
    }

    public static void CheckAllGroupsCompatible(
      CommonOverwriteGroups[] overwriteGroups,
      CompatibilityInfo compatibilityInfo)
    {
      string str = "";
      if (compatibilityInfo == null)
        throw new Exception("Try to overwrite but no compatibility information is available.");
      if (!compatibilityInfo.IsFullCompatible)
      {
        foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
        {
          if (compatibilityInfo.CompatibleGroupShortcuts.IndexOf(OverwriteSupport.GetShortcatFromOverwriteGroup(overwriteGroup)) < 0)
          {
            foreach (CommonOverwriteGroups includingOverwriteGroup in OverwriteSupport.GetIncludingOverwriteGroups(overwriteGroup))
            {
              if (compatibilityInfo.CompatibleGroupShortcuts.IndexOf(OverwriteSupport.GetShortcatFromOverwriteGroup(includingOverwriteGroup)) >= 0)
                goto label_13;
            }
            if (str.Length > 0)
              str += ";";
            str += overwriteGroup.ToString();
label_13:;
          }
        }
      }
      if (str.Length > 0)
        throw new Exception("Try to overwrite not compatible groups: " + str);
    }
  }
}
