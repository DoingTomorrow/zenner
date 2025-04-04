// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeSequence
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
  internal sealed class FileArchiveModeSequence : FileArchiveModeBase
  {
    private readonly string _archiveDateFormat;

    public FileArchiveModeSequence(string archiveDateFormat)
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
      string str1 = Path.GetFileName(archiveFile.FullName) ?? "";
      int num = fileTemplate.Template.Length - fileTemplate.EndAt;
      string str2 = str1.Substring(fileTemplate.BeginAt, str1.Length - num - fileTemplate.BeginAt);
      int int32;
      try
      {
        int32 = Convert.ToInt32(str2, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch (FormatException ex)
      {
        return (DateAndSequenceArchive) null;
      }
      return new DateAndSequenceArchive(archiveFile.FullName, DateTime.MinValue, string.Empty, int32);
    }

    public override DateAndSequenceArchive GenerateArchiveFileName(
      string archiveFilePath,
      DateTime archiveDate,
      List<DateAndSequenceArchive> existingArchiveFiles)
    {
      int num = 0;
      FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
      foreach (DateAndSequenceArchive existingArchiveFile in existingArchiveFiles)
        num = Math.Max(num, existingArchiveFile.Sequence + 1);
      int totalWidth = fileNameTemplate.EndAt - fileNameTemplate.BeginAt - 2;
      string newValue = num.ToString().PadLeft(totalWidth, '0');
      archiveFilePath = Path.Combine(Path.GetDirectoryName(archiveFilePath), fileNameTemplate.ReplacePattern("*").Replace("*", newValue));
      return new DateAndSequenceArchive(archiveFilePath, archiveDate, this._archiveDateFormat, num);
    }

    public override IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
      string archiveFilePath,
      List<DateAndSequenceArchive> existingArchiveFiles,
      int maxArchiveFiles)
    {
      if (maxArchiveFiles > 0 && existingArchiveFiles.Count != 0 && existingArchiveFiles.Count >= maxArchiveFiles)
      {
        int minNumberToKeep = existingArchiveFiles[existingArchiveFiles.Count - 1].Sequence - maxArchiveFiles + 1;
        if (minNumberToKeep > 0)
        {
          foreach (DateAndSequenceArchive existingArchiveFile in existingArchiveFiles)
          {
            if (existingArchiveFile.Sequence < minNumberToKeep)
              yield return existingArchiveFile;
          }
        }
      }
    }
  }
}
