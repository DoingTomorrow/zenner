// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsFat
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsFat
  {
    private readonly List<uint> m_fat;
    private readonly XlsHeader m_hdr;
    private readonly int m_sectors;
    private readonly int m_sectors_for_fat;
    private readonly int m_sectorSize;
    private readonly bool m_isMini;
    private readonly XlsRootDirectory m_rootDir;

    public XlsFat(
      XlsHeader hdr,
      List<uint> sectors,
      int sizeOfSector,
      bool isMini,
      XlsRootDirectory rootDir)
    {
      this.m_isMini = isMini;
      this.m_rootDir = rootDir;
      this.m_hdr = hdr;
      this.m_sectors_for_fat = sectors.Count;
      sizeOfSector = hdr.SectorSize;
      uint num = 0;
      byte[] buffer = new byte[sizeOfSector];
      Stream fileStream = hdr.FileStream;
      using (MemoryStream input = new MemoryStream(sizeOfSector * this.m_sectors_for_fat))
      {
        lock (fileStream)
        {
          for (int index = 0; index < sectors.Count; ++index)
          {
            uint sector = sectors[index];
            if (num == 0U || (int) sector - (int) num != 1)
              fileStream.Seek((long) (sector + 1U) * (long) sizeOfSector, SeekOrigin.Begin);
            num = sector;
            fileStream.Read(buffer, 0, sizeOfSector);
            input.Write(buffer, 0, sizeOfSector);
          }
        }
        input.Seek(0L, SeekOrigin.Begin);
        BinaryReader binaryReader = new BinaryReader((Stream) input);
        this.m_sectors = (int) input.Length / 4;
        this.m_fat = new List<uint>(this.m_sectors);
        for (int index = 0; index < this.m_sectors; ++index)
          this.m_fat.Add(binaryReader.ReadUInt32());
        binaryReader.Close();
        input.Close();
      }
    }

    public int SectorsForFat => this.m_sectors_for_fat;

    public int SectorsCount => this.m_sectors;

    public XlsHeader Header => this.m_hdr;

    public uint GetNextSector(uint sector)
    {
      if ((long) this.m_fat.Count <= (long) sector)
        throw new ArgumentException("Error reading as FAT table : There's no such sector in FAT.");
      uint nextSector = this.m_fat[(int) sector];
      switch (nextSector)
      {
        case 4294967292:
        case 4294967293:
          throw new InvalidOperationException("Error reading stream from FAT area.");
        default:
          return nextSector;
      }
    }
  }
}
