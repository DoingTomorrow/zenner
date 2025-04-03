// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.AddressRange
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class AddressRange : IComparable<AddressRange>
  {
    private uint startAddress = 0;
    private uint endAddress = uint.MaxValue;

    public uint StartAddress
    {
      get => this.startAddress;
      set
      {
        uint num = value - this.startAddress;
        this.startAddress = value;
        this.endAddress += num;
      }
    }

    public uint EndAddress
    {
      get => this.endAddress;
      set => this.endAddress = value;
    }

    public uint ByteSize
    {
      get => (uint) ((int) this.endAddress - (int) this.startAddress + 1);
      set => this.endAddress = (uint) ((int) this.startAddress + (int) value - 1);
    }

    public AddressRange(uint startAddress, uint size)
    {
      this.StartAddress = startAddress;
      this.ByteSize = size;
    }

    public AddressRange(uint startAddress)
    {
      this.StartAddress = startAddress;
      this.ByteSize = 1U;
    }

    public AddressRange(AddressRange theRange)
    {
      this.StartAddress = theRange.startAddress;
      this.ByteSize = theRange.ByteSize;
    }

    public bool IsInAdressRange(uint Address)
    {
      return this.StartAddress <= Address && Address <= this.EndAddress;
    }

    public void SetStartAddressHoldEndAddress(uint newStartAddress)
    {
      this.startAddress = newStartAddress <= this.endAddress ? newStartAddress : throw new Exception("try to set StartAddress higher then EndAddress");
    }

    public static bool IsInAddressRanges(List<AddressRange> adrRanges, uint address)
    {
      bool flag = false;
      if (adrRanges == null || adrRanges.Count <= 0)
        return true;
      foreach (AddressRange adrRange in adrRanges)
        flag |= adrRange.IsInAdressRange(address);
      return flag;
    }

    public AddressRange Clone() => new AddressRange(this.startAddress, this.ByteSize);

    public override string ToString()
    {
      return "0x" + this.StartAddress.ToString("X4") + "-0x" + this.EndAddress.ToString("X4") + " size:" + this.ByteSize.ToString();
    }

    public static List<AddressRange> GetRangesByExcludeRange(
      AddressRange theRange,
      AddressRange excludeRange)
    {
      List<AddressRange> rangesByExcludeRange = new List<AddressRange>();
      if (excludeRange.startAddress > theRange.endAddress && excludeRange.endAddress < theRange.startAddress)
      {
        rangesByExcludeRange.Add(theRange);
      }
      else
      {
        if (excludeRange.startAddress > theRange.startAddress && excludeRange.startAddress <= theRange.endAddress)
          rangesByExcludeRange.Add(new AddressRange(theRange.startAddress, excludeRange.startAddress - theRange.startAddress));
        if (excludeRange.endAddress >= theRange.startAddress && excludeRange.endAddress < theRange.endAddress)
          rangesByExcludeRange.Add(new AddressRange(excludeRange.endAddress + 1U, theRange.endAddress - excludeRange.endAddress));
      }
      return rangesByExcludeRange;
    }

    public int CompareTo(AddressRange obj)
    {
      if (obj == null)
        return 1;
      int num = this.startAddress.CompareTo(obj.startAddress);
      return num != 0 ? num : this.ByteSize.CompareTo(obj.ByteSize);
    }
  }
}
