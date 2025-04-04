// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeDynamicTemplate
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes
{
  internal sealed class FileArchiveModeDynamicTemplate : IFileArchiveMode
  {
    private readonly IFileArchiveMode _archiveHelper;

    private static string CreateDynamicTemplate(string archiveFilePath)
    {
      string extension = Path.GetExtension(archiveFilePath);
      return Path.ChangeExtension(archiveFilePath, ".{#}" + extension);
    }

    public FileArchiveModeDynamicTemplate(IFileArchiveMode archiveHelper)
    {
      this._archiveHelper = archiveHelper;
    }

    public bool AttemptCleanupOnInitializeFile(string archiveFilePath, int maxArchiveFiles)
    {
      return this._archiveHelper.AttemptCleanupOnInitializeFile(archiveFilePath, maxArchiveFiles);
    }

    public IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
      string archiveFilePath,
      List<DateAndSequenceArchive> existingArchiveFiles,
      int maxArchiveFiles)
    {
      return this._archiveHelper.CheckArchiveCleanup(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath), existingArchiveFiles, maxArchiveFiles);
    }

    public DateAndSequenceArchive GenerateArchiveFileName(
      string archiveFilePath,
      DateTime archiveDate,
      List<DateAndSequenceArchive> existingArchiveFiles)
    {
      return this._archiveHelper.GenerateArchiveFileName(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath), archiveDate, existingArchiveFiles);
    }

    public string GenerateFileNameMask(string archiveFilePath)
    {
      return this._archiveHelper.GenerateFileNameMask(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath));
    }

    public List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
    {
      return this._archiveHelper.GetExistingArchiveFiles(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath));
    }
  }
}
