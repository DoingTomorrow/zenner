// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SaveProgressEventArgs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zip
{
  internal class SaveProgressEventArgs : ZipProgressEventArgs
  {
    private int _entriesSaved;

    internal SaveProgressEventArgs(
      string archiveName,
      bool before,
      int entriesTotal,
      int entriesSaved,
      ZipEntry entry)
      : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
    {
      this.EntriesTotal = entriesTotal;
      this.CurrentEntry = entry;
      this._entriesSaved = entriesSaved;
    }

    internal SaveProgressEventArgs()
    {
    }

    internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal static SaveProgressEventArgs ByteUpdate(
      string archiveName,
      ZipEntry entry,
      long bytesXferred,
      long totalBytes)
    {
      SaveProgressEventArgs progressEventArgs = new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead);
      progressEventArgs.ArchiveName = archiveName;
      progressEventArgs.CurrentEntry = entry;
      progressEventArgs.BytesTransferred = bytesXferred;
      progressEventArgs.TotalBytesToTransfer = totalBytes;
      return progressEventArgs;
    }

    internal static SaveProgressEventArgs Started(string archiveName)
    {
      return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
    }

    internal static SaveProgressEventArgs Completed(string archiveName)
    {
      return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
    }

    public int EntriesSaved => this._entriesSaved;
  }
}
