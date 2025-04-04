// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsStream
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.IO;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsStream
  {
    protected XlsFat m_fat;
    protected XlsFat m_minifat;
    protected Stream m_fileStream;
    protected XlsHeader m_hdr;
    protected uint m_startSector;
    protected bool m_isMini;
    protected XlsRootDirectory m_rootDir;

    public XlsStream(XlsHeader hdr, uint startSector, bool isMini, XlsRootDirectory rootDir)
    {
      this.m_fileStream = hdr.FileStream;
      this.m_fat = hdr.FAT;
      this.m_hdr = hdr;
      this.m_startSector = startSector;
      this.m_isMini = isMini;
      this.m_rootDir = rootDir;
      this.CalculateMiniFat(rootDir);
    }

    public void CalculateMiniFat(XlsRootDirectory rootDir)
    {
      this.m_minifat = this.m_hdr.GetMiniFAT(rootDir);
    }

    public uint BaseOffset
    {
      get => (uint) ((ulong) (this.m_startSector + 1U) * (ulong) this.m_hdr.SectorSize);
    }

    public uint BaseSector => this.m_startSector;

    public byte[] ReadStream()
    {
      uint sector = this.m_startSector;
      uint num1 = 0;
      int count = this.m_isMini ? this.m_hdr.MiniSectorSize : this.m_hdr.SectorSize;
      XlsFat xlsFat = this.m_isMini ? this.m_minifat : this.m_fat;
      long num2 = 0;
      if (this.m_isMini && this.m_rootDir != null)
        num2 = (long) (this.m_rootDir.RootEntry.StreamFirstSector + 1U) * (long) this.m_hdr.SectorSize;
      byte[] buffer = new byte[count];
      using (MemoryStream memoryStream = new MemoryStream(count * 8))
      {
        lock (this.m_fileStream)
        {
label_5:
          if (num1 == 0U || (int) sector - (int) num1 != 1)
            this.m_fileStream.Seek((this.m_isMini ? (long) sector : (long) (sector + 1U)) * (long) count + num2, SeekOrigin.Begin);
          num1 = num1 == 0U || (int) num1 != (int) sector ? sector : throw new InvalidOperationException("The excel file may be corrupt. We appear to be stuck");
          this.m_fileStream.Read(buffer, 0, count);
          memoryStream.Write(buffer, 0, count);
          sector = xlsFat.GetNextSector(sector);
          switch (sector)
          {
            case 0:
              throw new InvalidOperationException("Next sector cannot be 0. Possibly corrupt excel file");
            case 4294967294:
              break;
            default:
              goto label_5;
          }
        }
        return memoryStream.ToArray();
      }
    }
  }
}
