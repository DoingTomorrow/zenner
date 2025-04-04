// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipConstants
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zip
{
  internal static class ZipConstants
  {
    public const uint PackedToRemovableMedia = 808471376;
    public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792;
    public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008;
    public const uint EndOfCentralDirectorySignature = 101010256;
    public const int ZipEntrySignature = 67324752;
    public const int ZipEntryDataDescriptorSignature = 134695760;
    public const int SplitArchiveSignature = 134695760;
    public const int ZipDirEntrySignature = 33639248;
    public const int AesKeySize = 192;
    public const int AesBlockSize = 128;
    public const ushort AesAlgId128 = 26126;
    public const ushort AesAlgId192 = 26127;
    public const ushort AesAlgId256 = 26128;
  }
}
