// Decompiled with JetBrains decompiler
// Type: EDC_Handler.MBusListStructure
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EDC_Handler
{
  public sealed class MBusListStructure
  {
    public List<MBusList> Lists { get; set; }

    public MBusList SelectedInstallList { get; set; }

    public MBusList SelectedTransmitList { get; set; }

    public MBusListStructure() => this.Lists = new List<MBusList>();

    public static MBusListStructure Parse(
      byte[] buffer,
      ushort startAddress,
      int size,
      DeviceVersion version)
    {
      if (buffer == null || buffer.Length < 4)
        return (MBusListStructure) null;
      MBusListStructure mbusListStructure = new MBusListStructure();
      ushort startIndex1 = 0;
      ushort addressOfTransmitList = BitConverter.ToUInt16(buffer, (int) startIndex1);
      ushort startIndex2 = (ushort) ((uint) startIndex1 + 2U);
      ushort addressOfInstallList = BitConverter.ToUInt16(buffer, (int) startIndex2);
      ushort offset = (ushort) ((uint) startIndex2 + 2U);
      for (ushort uint16 = BitConverter.ToUInt16(buffer, (int) offset); uint16 != (ushort) 0 && uint16 != ushort.MaxValue && (int) offset < size; uint16 = BitConverter.ToUInt16(buffer, (int) offset))
      {
        MBusList mbusList = MBusList.Parse(buffer, startAddress, version, ref offset);
        if (mbusList != null)
          mbusListStructure.Lists.Add(mbusList);
      }
      mbusListStructure.SelectedInstallList = mbusListStructure.Lists.Find((Predicate<MBusList>) (e => (int) e.StartAddress == (int) addressOfInstallList));
      mbusListStructure.SelectedTransmitList = mbusListStructure.Lists.Find((Predicate<MBusList>) (e => (int) e.StartAddress == (int) addressOfTransmitList));
      return mbusListStructure;
    }

    internal string GetNameOfSelectedTransmitList()
    {
      return this.SelectedTransmitList != null ? "LIST_" + ((char) (this.Lists.IndexOf(this.SelectedTransmitList) - 1 + 65)).ToString() : string.Empty;
    }

    internal MBusList Find(string listName)
    {
      if (string.IsNullOrEmpty(listName) || this.Lists == null || this.Lists.Count == 0)
        return (MBusList) null;
      foreach (MBusList list in this.Lists)
      {
        if ("LIST_" + ((char) (this.Lists.IndexOf(list) - 1 + 65)).ToString() == listName)
          return list;
      }
      return (MBusList) null;
    }

    internal List<string> GetListNames()
    {
      if (this.Lists == null || this.Lists.Count == 0)
        return (List<string>) null;
      List<string> listNames = new List<string>();
      foreach (MBusList list in this.Lists)
      {
        if (this.Lists.IndexOf(list) != 0)
          listNames.Add("LIST_" + ((char) (this.Lists.IndexOf(list) - 1 + 65)).ToString());
      }
      return listNames;
    }
  }
}
