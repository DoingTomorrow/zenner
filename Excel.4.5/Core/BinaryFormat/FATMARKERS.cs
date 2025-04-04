// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.FATMARKERS
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal enum FATMARKERS : uint
  {
    FAT_DifSector = 4294967292, // 0xFFFFFFFC
    FAT_FatSector = 4294967293, // 0xFFFFFFFD
    FAT_EndOfChain = 4294967294, // 0xFFFFFFFE
    FAT_FreeSpace = 4294967295, // 0xFFFFFFFF
  }
}
