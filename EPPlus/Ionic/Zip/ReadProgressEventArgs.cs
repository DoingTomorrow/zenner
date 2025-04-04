// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ReadProgressEventArgs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zip
{
  internal class ReadProgressEventArgs : ZipProgressEventArgs
  {
    internal ReadProgressEventArgs()
    {
    }

    private ReadProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal static ReadProgressEventArgs Before(string archiveName, int entriesTotal)
    {
      ReadProgressEventArgs progressEventArgs = new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_BeforeReadEntry);
      progressEventArgs.EntriesTotal = entriesTotal;
      return progressEventArgs;
    }

    internal static ReadProgressEventArgs After(
      string archiveName,
      ZipEntry entry,
      int entriesTotal)
    {
      ReadProgressEventArgs progressEventArgs = new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_AfterReadEntry);
      progressEventArgs.EntriesTotal = entriesTotal;
      progressEventArgs.CurrentEntry = entry;
      return progressEventArgs;
    }

    internal static ReadProgressEventArgs Started(string archiveName)
    {
      return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Started);
    }

    internal static ReadProgressEventArgs ByteUpdate(
      string archiveName,
      ZipEntry entry,
      long bytesXferred,
      long totalBytes)
    {
      ReadProgressEventArgs progressEventArgs = new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_ArchiveBytesRead);
      progressEventArgs.CurrentEntry = entry;
      progressEventArgs.BytesTransferred = bytesXferred;
      progressEventArgs.TotalBytesToTransfer = totalBytes;
      return progressEventArgs;
    }

    internal static ReadProgressEventArgs Completed(string archiveName)
    {
      return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Completed);
    }
  }
}
