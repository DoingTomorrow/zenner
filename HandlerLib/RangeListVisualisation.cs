// Decompiled with JetBrains decompiler
// Type: HandlerLib.RangeListVisualisation
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public class RangeListVisualisation
  {
    public RangeListVisualisation(string caption, List<AddressRangeInfo> ranges)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("*** " + caption + " ***");
      stringBuilder1.AppendLine();
      if (ranges == null)
      {
        stringBuilder1.AppendLine("Ranges not defined");
      }
      else
      {
        int totalWidth = 0;
        int num1 = 1;
        foreach (AddressRangeInfo range in ranges)
        {
          range.Order = num1;
          ++num1;
          if (!string.IsNullOrEmpty(range.RangeInfo) && range.RangeInfo.Length > totalWidth)
            totalWidth = range.RangeInfo.Length;
        }
        foreach (AddressRangeInfo range in ranges)
        {
          int num2 = (int) (Math.Log10((double) range.ByteSize) * 10.0);
          stringBuilder1.Append(range.RangeInfo.PadRight(totalWidth) + " ");
          stringBuilder1.Append(range.Order.ToString("d2") + " ");
          StringBuilder stringBuilder2 = stringBuilder1;
          uint num3 = range.StartAddress;
          string str1 = "0x" + num3.ToString("x08");
          stringBuilder2.Append(str1);
          stringBuilder1.Append(" - ");
          StringBuilder stringBuilder3 = stringBuilder1;
          num3 = range.EndAddress;
          string str2 = "0x" + num3.ToString("x08");
          stringBuilder3.Append(str2);
          StringBuilder stringBuilder4 = stringBuilder1;
          num3 = range.ByteSize;
          string str3 = ": 0x" + num3.ToString("x04");
          stringBuilder4.Append(str3);
          stringBuilder1.Append(" ");
          for (int index = 0; index < num2; ++index)
            stringBuilder1.Append("#");
          stringBuilder1.AppendLine();
        }
      }
      GmmMessage.Show(stringBuilder1.ToString(), "Range view", true);
    }
  }
}
