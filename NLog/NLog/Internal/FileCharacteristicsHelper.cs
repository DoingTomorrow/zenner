// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileCharacteristicsHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal abstract class FileCharacteristicsHelper
  {
    public static FileCharacteristicsHelper CreateHelper(bool forcedManaged)
    {
      return !forcedManaged && PlatformDetector.IsDesktopWin32 && !PlatformDetector.IsMono ? (FileCharacteristicsHelper) new Win32FileCharacteristicsHelper() : (FileCharacteristicsHelper) new PortableFileCharacteristicsHelper();
    }

    public abstract FileCharacteristics GetFileCharacteristics(
      string fileName,
      FileStream fileStream);

    public static DateTime? ValidateFileCreationTime<T>(
      T fileInfo,
      Func<T, DateTime?> primary,
      Func<T, DateTime?> fallback,
      Func<T, DateTime?> finalFallback = null)
    {
      DateTime? nullable = primary(fileInfo);
      if (nullable.HasValue)
      {
        DateTime dateTime = nullable.Value;
        if (dateTime.Year < 1980 && !PlatformDetector.IsDesktopWin32)
        {
          nullable = fallback(fileInfo);
          if (finalFallback != null)
          {
            if (nullable.HasValue)
            {
              dateTime = nullable.Value;
              if (dateTime.Year >= 1980)
                goto label_6;
            }
            nullable = finalFallback(fileInfo);
          }
        }
      }
label_6:
      return nullable;
    }
  }
}
