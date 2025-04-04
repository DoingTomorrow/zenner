// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeDate
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes
{
  internal sealed class FileArchiveModeDate : FileArchiveModeBase
  {
    private readonly string _archiveDateFormat;
    private readonly bool _archiveCleanupEnabled;

    public FileArchiveModeDate(string archiveDateFormat, bool archiveCleanupEnabled)
    {
      this._archiveDateFormat = archiveDateFormat;
      this._archiveCleanupEnabled = archiveCleanupEnabled;
    }

    public override List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
    {
      return this._archiveCleanupEnabled ? base.GetExistingArchiveFiles(archiveFilePath) : new List<DateAndSequenceArchive>();
    }

    protected override DateAndSequenceArchive GenerateArchiveFileInfo(
      FileInfo archiveFile,
      FileArchiveModeBase.FileNameTemplate fileTemplate)
    {
      string str = Path.GetFileName(archiveFile.FullName) ?? "";
      int startIndex = fileTemplate.ReplacePattern("*").LastIndexOf('*');
      DateTime result;
      return startIndex + this._archiveDateFormat.Length <= str.Length && DateTime.TryParseExact(str.Substring(startIndex, this._archiveDateFormat.Length), this._archiveDateFormat, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? new DateAndSequenceArchive(archiveFile.FullName, result, this._archiveDateFormat, -1) : (DateAndSequenceArchive) null;
    }

    public override DateAndSequenceArchive GenerateArchiveFileName(
      string archiveFilePath,
      DateTime archiveDate,
      List<DateAndSequenceArchive> existingArchiveFiles)
    {
      FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
      archiveFilePath = Path.Combine(Path.GetDirectoryName(archiveFilePath), fileNameTemplate.ReplacePattern("*").Replace("*", archiveDate.ToString(this._archiveDateFormat)));
      return new DateAndSequenceArchive(archiveFilePath, archiveDate, this._archiveDateFormat, 0);
    }
  }
}
