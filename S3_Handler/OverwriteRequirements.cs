// Decompiled with JetBrains decompiler
// Type: S3_Handler.OverwriteRequirements
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace S3_Handler
{
  public class OverwriteRequirements
  {
    public int ParameterTypeMeterInfoId;
    public SortedList<OverwriteSelections, int> SelectedOverwriteSections = new SortedList<OverwriteSelections, int>();

    public OverwriteRequirements(string requirements)
    {
      if (requirements.Contains<char>('='))
      {
        string[] strArray = requirements.Split('=');
        this.ParameterTypeMeterInfoId = int.Parse(strArray[0]);
        string str = strArray[1];
        char[] chArray = new char[1]{ ',' };
        foreach (string key in str.Split(chArray))
          this.SelectedOverwriteSections.Add(OverwriteWorkMeter.OverwriteSelectionShortcutsList[key].overwriteSelection, 0);
      }
      else
      {
        string[] strArray = requirements.Split(',');
        this.ParameterTypeMeterInfoId = int.Parse(strArray[0]);
        for (int index = 1; index < strArray.Length; ++index)
          this.SelectedOverwriteSections.Add((OverwriteSelections) Enum.Parse(typeof (OverwriteSelections), strArray[index], true), 0);
      }
    }
  }
}
