// Decompiled with JetBrains decompiler
// Type: NLog.Targets.IFileArchiveMode
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets
{
  internal interface IFileArchiveMode
  {
    bool AttemptCleanupOnInitializeFile(string archiveFilePath, int maxArchiveFiles);

    string GenerateFileNameMask(string archiveFilePath);

    List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath);

    DateAndSequenceArchive GenerateArchiveFileName(
      string archiveFilePath,
      DateTime archiveDate,
      List<DateAndSequenceArchive> existingArchiveFiles);

    IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
      string archiveFilePath,
      List<DateAndSequenceArchive> existingArchiveFiles,
      int maxArchiveFiles);
  }
}
