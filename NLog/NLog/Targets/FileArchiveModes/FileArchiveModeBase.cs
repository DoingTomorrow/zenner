// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes
{
  internal abstract class FileArchiveModeBase : IFileArchiveMode
  {
    private int _lastArchiveFileCount = 65534;

    public virtual bool AttemptCleanupOnInitializeFile(string archiveFilePath, int maxArchiveFiles)
    {
      return this._lastArchiveFileCount++ > maxArchiveFiles;
    }

    public string GenerateFileNameMask(string archiveFilePath)
    {
      int archiveFileCount = this._lastArchiveFileCount;
      string fileNameMask = this.GenerateFileNameMask(archiveFilePath, this.GenerateFileNameTemplate(archiveFilePath));
      this._lastArchiveFileCount = archiveFileCount;
      return fileNameMask;
    }

    public virtual List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
    {
      this._lastArchiveFileCount = 65534;
      string directoryName = Path.GetDirectoryName(archiveFilePath);
      FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
      string fileNameMask = this.GenerateFileNameMask(archiveFilePath, fileNameTemplate);
      List<DateAndSequenceArchive> existingArchiveFiles = new List<DateAndSequenceArchive>();
      if (string.IsNullOrEmpty(fileNameMask))
        return existingArchiveFiles;
      DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
      if (!directoryInfo.Exists)
        return existingArchiveFiles;
      foreach (FileInfo file in directoryInfo.GetFiles(fileNameMask))
      {
        DateAndSequenceArchive archiveFileInfo = this.GenerateArchiveFileInfo(file, fileNameTemplate);
        if (archiveFileInfo != null)
          existingArchiveFiles.Add(archiveFileInfo);
      }
      if (existingArchiveFiles.Count > 1)
        existingArchiveFiles.Sort(new Comparison<DateAndSequenceArchive>(FileArchiveModeBase.FileSortOrderComparison));
      this._lastArchiveFileCount = existingArchiveFiles.Count;
      return existingArchiveFiles;
    }

    private static int FileSortOrderComparison(DateAndSequenceArchive x, DateAndSequenceArchive y)
    {
      if (x.Date != y.Date && !x.HasSameFormattedDate(y.Date))
        return x.Date.CompareTo(y.Date);
      return x.Sequence.CompareTo(y.Sequence) != 0 ? x.Sequence.CompareTo(y.Sequence) : string.CompareOrdinal(x.FileName, y.FileName);
    }

    protected virtual FileArchiveModeBase.FileNameTemplate GenerateFileNameTemplate(
      string archiveFilePath)
    {
      ++this._lastArchiveFileCount;
      return new FileArchiveModeBase.FileNameTemplate(Path.GetFileName(archiveFilePath));
    }

    protected virtual string GenerateFileNameMask(
      string archiveFilePath,
      FileArchiveModeBase.FileNameTemplate fileTemplate)
    {
      return fileTemplate != null ? fileTemplate.ReplacePattern("*") : string.Empty;
    }

    protected abstract DateAndSequenceArchive GenerateArchiveFileInfo(
      FileInfo archiveFile,
      FileArchiveModeBase.FileNameTemplate fileTemplate);

    public abstract DateAndSequenceArchive GenerateArchiveFileName(
      string archiveFilePath,
      DateTime archiveDate,
      List<DateAndSequenceArchive> existingArchiveFiles);

    public virtual IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
      string archiveFilePath,
      List<DateAndSequenceArchive> existingArchiveFiles,
      int maxArchiveFiles)
    {
      if (maxArchiveFiles > 0)
      {
        this._lastArchiveFileCount = existingArchiveFiles.Count;
        if (existingArchiveFiles.Count != 0 && existingArchiveFiles.Count >= maxArchiveFiles)
        {
          for (int i = 0; i < existingArchiveFiles.Count - maxArchiveFiles; ++i)
          {
            if (this._lastArchiveFileCount > 0)
              --this._lastArchiveFileCount;
            yield return existingArchiveFiles[i];
          }
        }
      }
    }

    internal sealed class FileNameTemplate
    {
      public const string PatternStartCharacters = "{#";
      public const string PatternEndCharacters = "#}";

      public string Template { get; private set; }

      public int BeginAt { get; private set; }

      public int EndAt { get; private set; }

      private bool FoundPattern => this.BeginAt != -1 && this.EndAt != -1;

      public FileNameTemplate(string template)
      {
        this.Template = template;
        this.BeginAt = template.IndexOf("{#", StringComparison.Ordinal);
        if (this.BeginAt == -1)
          return;
        this.EndAt = template.IndexOf("#}", StringComparison.Ordinal) + "#}".Length;
      }

      public string ReplacePattern(string replacementValue)
      {
        return this.FoundPattern && !string.IsNullOrEmpty(replacementValue) ? this.Template.Substring(0, this.BeginAt) + replacementValue + this.Template.Substring(this.EndAt) : this.Template;
      }
    }
  }
}
