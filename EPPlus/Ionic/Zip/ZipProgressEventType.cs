// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zip
{
  internal enum ZipProgressEventType
  {
    Adding_Started,
    Adding_AfterAddEntry,
    Adding_Completed,
    Reading_Started,
    Reading_BeforeReadEntry,
    Reading_AfterReadEntry,
    Reading_Completed,
    Reading_ArchiveBytesRead,
    Saving_Started,
    Saving_BeforeWriteEntry,
    Saving_AfterWriteEntry,
    Saving_Completed,
    Saving_AfterSaveTempArchive,
    Saving_BeforeRenameTempArchive,
    Saving_AfterRenameTempArchive,
    Saving_AfterCompileSelfExtractor,
    Saving_EntryBytesRead,
    Extracting_BeforeExtractEntry,
    Extracting_AfterExtractEntry,
    Extracting_ExtractEntryWouldOverwrite,
    Extracting_EntryBytesWritten,
    Extracting_BeforeExtractAll,
    Extracting_AfterExtractAll,
    Error_Saving,
  }
}
