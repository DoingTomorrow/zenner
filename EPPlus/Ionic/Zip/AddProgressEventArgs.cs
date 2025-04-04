// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.AddProgressEventArgs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zip
{
  internal class AddProgressEventArgs : ZipProgressEventArgs
  {
    internal AddProgressEventArgs()
    {
    }

    private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal static AddProgressEventArgs AfterEntry(
      string archiveName,
      ZipEntry entry,
      int entriesTotal)
    {
      AddProgressEventArgs progressEventArgs = new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry);
      progressEventArgs.EntriesTotal = entriesTotal;
      progressEventArgs.CurrentEntry = entry;
      return progressEventArgs;
    }

    internal static AddProgressEventArgs Started(string archiveName)
    {
      return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
    }

    internal static AddProgressEventArgs Completed(string archiveName)
    {
      return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
    }
  }
}
