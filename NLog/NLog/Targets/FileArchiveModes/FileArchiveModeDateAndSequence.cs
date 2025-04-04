﻿// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeDateAndSequence
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes
{
  internal sealed class FileArchiveModeDateAndSequence : FileArchiveModeBase
  {
    private readonly string _archiveDateFormat;

    public FileArchiveModeDateAndSequence(string archiveDateFormat)
    {
      this._archiveDateFormat = archiveDateFormat;
    }

    public override bool AttemptCleanupOnInitializeFile(string archiveFilePath, int maxArchiveFiles)
    {
      return false;
    }

    protected override DateAndSequenceArchive GenerateArchiveFileInfo(
      FileInfo archiveFile,
      FileArchiveModeBase.FileNameTemplate fileTemplate)
    {
      DateTime date;
      int sequence;
      return !FileArchiveModeDateAndSequence.TryParseDateAndSequence(Path.GetFileName(archiveFile.FullName) ?? string.Empty, this._archiveDateFormat, fileTemplate, out date, out sequence) ? (DateAndSequenceArchive) null : new DateAndSequenceArchive(archiveFile.FullName, date, this._archiveDateFormat, sequence);
    }

    public override DateAndSequenceArchive GenerateArchiveFileName(
      string archiveFilePath,
      DateTime archiveDate,
      List<DateAndSequenceArchive> existingArchiveFiles)
    {
      int num = 0;
      FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
      foreach (DateAndSequenceArchive existingArchiveFile in existingArchiveFiles)
      {
        if (existingArchiveFile.HasSameFormattedDate(archiveDate))
          num = Math.Max(num, existingArchiveFile.Sequence + 1);
      }
      int totalWidth = fileNameTemplate.EndAt - fileNameTemplate.BeginAt - 2;
      string str = num.ToString().PadLeft(totalWidth, '0');
      string path2 = fileNameTemplate.ReplacePattern("*").Replace("*", string.Format("{0}.{1}", (object) archiveDate.ToString(this._archiveDateFormat), (object) str));
      archiveFilePath = Path.Combine(Path.GetDirectoryName(archiveFilePath), path2);
      return new DateAndSequenceArchive(archiveFilePath, archiveDate, this._archiveDateFormat, num);
    }

    private static bool TryParseDateAndSequence(
      string archiveFileNameWithoutPath,
      string dateFormat,
      FileArchiveModeBase.FileNameTemplate fileTemplate,
      out DateTime date,
      out int sequence)
    {
      int num = fileTemplate.Template.Length - fileTemplate.EndAt;
      int beginAt = fileTemplate.BeginAt;
      int length1 = archiveFileNameWithoutPath.Length - num - beginAt;
      if (length1 < 0)
      {
        date = new DateTime();
        sequence = 0;
        return false;
      }
      string str = archiveFileNameWithoutPath.Substring(beginAt, length1);
      int startIndex = str.LastIndexOf('.') + 1;
      string s1 = str.Substring(startIndex);
      if (!int.TryParse(s1, NumberStyles.None, (IFormatProvider) CultureInfo.CurrentCulture, out sequence))
      {
        date = new DateTime();
        return false;
      }
      int length2 = str.Length - s1.Length - 1;
      if (length2 < 0)
      {
        date = new DateTime();
        return false;
      }
      string s2 = str.Substring(0, length2);
      if (!DateTime.TryParseExact(s2, dateFormat, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
        return false;
      InternalLogger.Trace<string, string>("FileTarget: parsed date '{0}' from file-template '{1}'", s2, fileTemplate?.Template);
      return true;
    }
  }
}
