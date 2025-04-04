// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsRootDirectory
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsRootDirectory
  {
    private readonly List<XlsDirectoryEntry> m_entries;
    private readonly XlsDirectoryEntry m_root;

    public XlsRootDirectory(XlsHeader hdr)
    {
      XlsStream xlsStream = new XlsStream(hdr, hdr.RootDirectoryEntryStart, false, (XlsRootDirectory) null);
      byte[] src = xlsStream.ReadStream();
      List<XlsDirectoryEntry> xlsDirectoryEntryList = new List<XlsDirectoryEntry>();
      for (int srcOffset = 0; srcOffset < src.Length; srcOffset += 128)
      {
        byte[] numArray = new byte[128];
        Buffer.BlockCopy((Array) src, srcOffset, (Array) numArray, 0, numArray.Length);
        xlsDirectoryEntryList.Add(new XlsDirectoryEntry(numArray, hdr));
      }
      this.m_entries = xlsDirectoryEntryList;
      for (int index = 0; index < xlsDirectoryEntryList.Count; ++index)
      {
        XlsDirectoryEntry xlsDirectoryEntry = xlsDirectoryEntryList[index];
        if (this.m_root == null && xlsDirectoryEntry.EntryType == STGTY.STGTY_ROOT)
          this.m_root = xlsDirectoryEntry;
        if (xlsDirectoryEntry.ChildSid != uint.MaxValue)
          xlsDirectoryEntry.Child = xlsDirectoryEntryList[(int) xlsDirectoryEntry.ChildSid];
        if (xlsDirectoryEntry.LeftSiblingSid != uint.MaxValue)
          xlsDirectoryEntry.LeftSibling = xlsDirectoryEntryList[(int) xlsDirectoryEntry.LeftSiblingSid];
        if (xlsDirectoryEntry.RightSiblingSid != uint.MaxValue)
          xlsDirectoryEntry.RightSibling = xlsDirectoryEntryList[(int) xlsDirectoryEntry.RightSiblingSid];
      }
      xlsStream.CalculateMiniFat(this);
    }

    public ReadOnlyCollection<XlsDirectoryEntry> Entries => this.m_entries.AsReadOnly();

    public XlsDirectoryEntry RootEntry => this.m_root;

    public XlsDirectoryEntry FindEntry(string EntryName)
    {
      foreach (XlsDirectoryEntry entry in this.m_entries)
      {
        if (string.Equals(entry.EntryName, EntryName, StringComparison.CurrentCultureIgnoreCase))
          return entry;
      }
      return (XlsDirectoryEntry) null;
    }
  }
}
