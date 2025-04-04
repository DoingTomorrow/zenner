// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsHeader
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using Excel.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsHeader
  {
    private readonly byte[] m_bytes;
    private readonly Stream m_file;
    private XlsFat m_fat;
    private XlsFat m_minifat;

    private XlsHeader(Stream file)
    {
      this.m_bytes = new byte[512];
      this.m_file = file;
    }

    public ulong Signature => BitConverter.ToUInt64(this.m_bytes, 0);

    public bool IsSignatureValid => this.Signature == 16220472316735377360UL;

    public Guid ClassId
    {
      get
      {
        byte[] numArray = new byte[16];
        Buffer.BlockCopy((Array) this.m_bytes, 8, (Array) numArray, 0, 16);
        return new Guid(numArray);
      }
    }

    public ushort Version => BitConverter.ToUInt16(this.m_bytes, 24);

    public ushort DllVersion => BitConverter.ToUInt16(this.m_bytes, 26);

    public ushort ByteOrder => BitConverter.ToUInt16(this.m_bytes, 28);

    public int SectorSize => 1 << (int) BitConverter.ToUInt16(this.m_bytes, 30);

    public int MiniSectorSize => 1 << (int) BitConverter.ToUInt16(this.m_bytes, 32);

    public int FatSectorCount => BitConverter.ToInt32(this.m_bytes, 44);

    public uint RootDirectoryEntryStart => BitConverter.ToUInt32(this.m_bytes, 48);

    public uint TransactionSignature => BitConverter.ToUInt32(this.m_bytes, 52);

    public uint MiniStreamCutoff => BitConverter.ToUInt32(this.m_bytes, 56);

    public uint MiniFatFirstSector => BitConverter.ToUInt32(this.m_bytes, 60);

    public int MiniFatSectorCount => BitConverter.ToInt32(this.m_bytes, 64);

    public uint DifFirstSector => BitConverter.ToUInt32(this.m_bytes, 68);

    public int DifSectorCount => BitConverter.ToInt32(this.m_bytes, 72);

    public Stream FileStream => this.m_file;

    public XlsFat GetMiniFAT(XlsRootDirectory rootDir)
    {
      if (this.m_minifat != null)
        return this.m_minifat;
      if (this.MiniFatSectorCount == 0 || this.MiniSectorSize == -2)
        return (XlsFat) null;
      int miniSectorSize = this.MiniSectorSize;
      this.m_minifat = new XlsFat(this, new List<uint>(this.MiniFatSectorCount)
      {
        BitConverter.ToUInt32(this.m_bytes, 60)
      }, this.MiniSectorSize, true, rootDir);
      return this.m_minifat;
    }

    public XlsFat FAT
    {
      get
      {
        if (this.m_fat != null)
          return this.m_fat;
        int sectorSize = this.SectorSize;
        List<uint> sectors = new List<uint>(this.FatSectorCount);
        for (int startIndex = 76; startIndex < sectorSize; startIndex += 4)
        {
          uint uint32 = BitConverter.ToUInt32(this.m_bytes, startIndex);
          if (uint32 != uint.MaxValue)
            sectors.Add(uint32);
          else
            goto label_23;
        }
        int difSectorCount;
        if ((difSectorCount = this.DifSectorCount) != 0)
        {
          lock (this.m_file)
          {
            uint num1 = this.DifFirstSector;
            byte[] buffer = new byte[sectorSize];
            uint num2 = 0;
            while (difSectorCount > 0)
            {
              sectors.Capacity += 128;
              if (num2 == 0U || (int) num1 - (int) num2 != 1)
                this.m_file.Seek((long) (num1 + 1U) * (long) sectorSize, SeekOrigin.Begin);
              num2 = num1;
              this.m_file.Read(buffer, 0, sectorSize);
              for (int startIndex = 0; startIndex < 508; startIndex += 4)
              {
                uint uint32 = BitConverter.ToUInt32(buffer, startIndex);
                if (uint32 != uint.MaxValue)
                  sectors.Add(uint32);
                else
                  goto label_23;
              }
              uint uint32_1 = BitConverter.ToUInt32(buffer, 508);
              if (uint32_1 != uint.MaxValue)
              {
                if (difSectorCount-- > 1)
                  num1 = uint32_1;
                else
                  sectors.Add(uint32_1);
              }
              else
                break;
            }
          }
        }
label_23:
        this.m_fat = new XlsFat(this, sectors, this.SectorSize, false, (XlsRootDirectory) null);
        return this.m_fat;
      }
    }

    public static XlsHeader ReadHeader(Stream file)
    {
      XlsHeader xlsHeader = new XlsHeader(file);
      lock (file)
      {
        file.Seek(0L, SeekOrigin.Begin);
        file.Read(xlsHeader.m_bytes, 0, 512);
      }
      if (!xlsHeader.IsSignatureValid)
        throw new HeaderException("Error: Invalid file signature.");
      return xlsHeader.ByteOrder == (ushort) 65534 ? xlsHeader : throw new FormatException("Error: Invalid byte order specified in header.");
    }
  }
}
