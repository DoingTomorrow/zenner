// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipErrorEventArgs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace Ionic.Zip
{
  internal class ZipErrorEventArgs : ZipProgressEventArgs
  {
    private Exception _exc;

    private ZipErrorEventArgs()
    {
    }

    internal static ZipErrorEventArgs Saving(
      string archiveName,
      ZipEntry entry,
      Exception exception)
    {
      ZipErrorEventArgs zipErrorEventArgs = new ZipErrorEventArgs();
      zipErrorEventArgs.EventType = ZipProgressEventType.Error_Saving;
      zipErrorEventArgs.ArchiveName = archiveName;
      zipErrorEventArgs.CurrentEntry = entry;
      zipErrorEventArgs._exc = exception;
      return zipErrorEventArgs;
    }

    public Exception Exception => this._exc;

    public string FileName => this.CurrentEntry.LocalFileName;
  }
}
