// Decompiled with JetBrains decompiler
// Type: HandlerLib.AddressRangeInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Collections.Generic;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class AddressRangeInfo : AddressRange
  {
    public string RangeInfo;
    public int Order;

    public AddressRangeInfo(string rangeInfo, AddressRange theRange)
      : base(theRange)
    {
      this.RangeInfo = rangeInfo;
    }

    public static List<AddressRangeInfo> GetRangeInfos(
      string rangeInfo,
      List<AddressRange> theRanges)
    {
      List<AddressRangeInfo> rangeInfos = new List<AddressRangeInfo>();
      if (theRanges != null)
      {
        foreach (AddressRange theRange in theRanges)
          rangeInfos.Add(new AddressRangeInfo(rangeInfo, theRange));
      }
      return rangeInfos;
    }

    public static void AddGaps(List<AddressRangeInfo> theList)
    {
      for (int index = 0; index < theList.Count - 1; ++index)
      {
        int size = (int) theList[index + 1].StartAddress - (int) theList[index].EndAddress - 1;
        if (size > 0)
          theList.Insert(index + 1, new AddressRangeInfo("   *** gap", new AddressRange(theList[index].EndAddress + 1U, (uint) size)));
      }
    }
  }
}
