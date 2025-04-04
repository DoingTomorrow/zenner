// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;

#nullable disable
namespace NLog.Targets.FileArchiveModes
{
  internal static class FileArchiveModeFactory
  {
    public static IFileArchiveMode CreateArchiveStyle(
      string archiveFilePath,
      ArchiveNumberingMode archiveNumbering,
      string dateFormat,
      bool customArchiveFileName,
      int maxArchiveFiles)
    {
      if (FileArchiveModeFactory.ContainsFileNamePattern(archiveFilePath))
      {
        IFileArchiveMode strictFileArchiveMode = FileArchiveModeFactory.CreateStrictFileArchiveMode(archiveNumbering, dateFormat, maxArchiveFiles);
        if (strictFileArchiveMode != null)
          return strictFileArchiveMode;
      }
      if (archiveNumbering != ArchiveNumberingMode.Sequence)
      {
        if (!customArchiveFileName)
        {
          IFileArchiveMode strictFileArchiveMode = FileArchiveModeFactory.CreateStrictFileArchiveMode(archiveNumbering, dateFormat, maxArchiveFiles);
          if (strictFileArchiveMode != null)
            return (IFileArchiveMode) new FileArchiveModeDynamicTemplate(strictFileArchiveMode);
        }
        else
          InternalLogger.Info<string>("FileTarget: Pattern {{#}} is missing in ArchiveFileName `{0}` (Fallback to dynamic wildcard)", archiveFilePath);
      }
      return (IFileArchiveMode) new FileArchiveModeDynamicSequence(archiveNumbering, dateFormat, customArchiveFileName);
    }

    private static IFileArchiveMode CreateStrictFileArchiveMode(
      ArchiveNumberingMode archiveNumbering,
      string dateFormat,
      int maxArchiveFiles)
    {
      switch (archiveNumbering)
      {
        case ArchiveNumberingMode.Sequence:
          return (IFileArchiveMode) new FileArchiveModeSequence(dateFormat);
        case ArchiveNumberingMode.Rolling:
          return (IFileArchiveMode) new FileArchiveModeRolling();
        case ArchiveNumberingMode.Date:
          return (IFileArchiveMode) new FileArchiveModeDate(dateFormat, FileArchiveModeFactory.ShouldDeleteOldArchives(maxArchiveFiles));
        case ArchiveNumberingMode.DateAndSequence:
          return (IFileArchiveMode) new FileArchiveModeDateAndSequence(dateFormat);
        default:
          return (IFileArchiveMode) null;
      }
    }

    public static bool ContainsFileNamePattern(string fileName)
    {
      int num1 = fileName.IndexOf("{#", StringComparison.Ordinal);
      int num2 = fileName.IndexOf("#}", StringComparison.Ordinal);
      return num1 != -1 && num2 != -1 && num1 < num2;
    }

    public static bool ShouldDeleteOldArchives(int maxArchiveFiles) => maxArchiveFiles > 0;
  }
}
