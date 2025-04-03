// Decompiled with JetBrains decompiler
// Type: S3_Handler.OverwriteRequirementsList
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class OverwriteRequirementsList
  {
    public SortedList<OverwriteListTypes, OverwriteRequirements> OverReqList = new SortedList<OverwriteListTypes, OverwriteRequirements>();

    public OverwriteRequirementsList(string typeOverwriteString)
    {
      string str = typeOverwriteString;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string removeEmptyEntry in Util.RemoveEmptyEntries(str.Split(chArray1)))
      {
        char[] chArray2 = new char[1]{ ':' };
        string[] strArray = Util.RemoveEmptyEntries(removeEmptyEntry.Split(chArray2));
        this.OverReqList.Add((OverwriteListTypes) Enum.Parse(typeof (OverwriteListTypes), strArray[0], true), new OverwriteRequirements(strArray[1]));
      }
    }

    public OverwriteRequirementsList(string typeCreationString, bool useCompactMBusList)
    {
      string str = typeCreationString;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string removeEmptyEntry in Util.RemoveEmptyEntries(str.Split(chArray1)))
      {
        char[] chArray2 = new char[1]{ ':' };
        string[] strArray = Util.RemoveEmptyEntries(removeEmptyEntry.Split(chArray2));
        if (strArray.Length == 1)
          this.OverReqList.Add(OverwriteListTypes.OverwriteType, new OverwriteRequirements(strArray[0]));
        else if (strArray.Length == 2 && (strArray[0] == "SP" & useCompactMBusList || strArray[0] == "LP" && !useCompactMBusList))
          this.OverReqList.Add(OverwriteListTypes.OverwriteType, new OverwriteRequirements(strArray[1]));
      }
    }

    public static bool CheckTypeCreationString(string typeCreationString)
    {
      string[] strArray1 = Util.RemoveEmptyEntries(typeCreationString.Split(';'));
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        string[] strArray2 = Util.RemoveEmptyEntries(strArray1[index1].Split(':'));
        string[] strArray3;
        if (strArray2.Length == 1)
        {
          strArray3 = Util.RemoveEmptyEntries(strArray2[0].Split('='));
        }
        else
        {
          if (strArray2.Length != 2 || index1 == 0)
            return false;
          strArray3 = Util.RemoveEmptyEntries(strArray2[1].Split('='));
          if (strArray2[0] != "SP" && strArray2[0] != "LP")
            return false;
        }
        if (strArray3.Length == 1)
        {
          if (index1 != 0)
            return false;
        }
        else
        {
          if (strArray3.Length != 2 || index1 == 0)
            return false;
          string[] strArray4 = Util.RemoveEmptyEntries(strArray3[1].Split(','));
          if (strArray4.Length == 0)
            return false;
          for (int index2 = 0; index2 < strArray4.Length; ++index2)
          {
            if (!OverwriteWorkMeter.OverwriteSelectionShortcutsList.Keys.Contains(strArray4[index2]))
              return false;
          }
        }
        if (!Util.TryParse(strArray3[0], out uint _))
          return false;
      }
      return true;
    }
  }
}
