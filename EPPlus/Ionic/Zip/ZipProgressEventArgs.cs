// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventArgs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace Ionic.Zip
{
  internal class ZipProgressEventArgs : EventArgs
  {
    private int _entriesTotal;
    private bool _cancel;
    private ZipEntry _latestEntry;
    private ZipProgressEventType _flavor;
    private string _archiveName;
    private long _bytesTransferred;
    private long _totalBytesToTransfer;

    internal ZipProgressEventArgs()
    {
    }

    internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
    {
      this._archiveName = archiveName;
      this._flavor = flavor;
    }

    public int EntriesTotal
    {
      get => this._entriesTotal;
      set => this._entriesTotal = value;
    }

    public ZipEntry CurrentEntry
    {
      get => this._latestEntry;
      set => this._latestEntry = value;
    }

    public bool Cancel
    {
      get => this._cancel;
      set => this._cancel = this._cancel || value;
    }

    public ZipProgressEventType EventType
    {
      get => this._flavor;
      set => this._flavor = value;
    }

    public string ArchiveName
    {
      get => this._archiveName;
      set => this._archiveName = value;
    }

    public long BytesTransferred
    {
      get => this._bytesTransferred;
      set => this._bytesTransferred = value;
    }

    public long TotalBytesToTransfer
    {
      get => this._totalBytesToTransfer;
      set => this._totalBytesToTransfer = value;
    }
  }
}
