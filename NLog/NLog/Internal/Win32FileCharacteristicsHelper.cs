// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Win32FileCharacteristicsHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal class Win32FileCharacteristicsHelper : FileCharacteristicsHelper
  {
    public override FileCharacteristics GetFileCharacteristics(
      string fileName,
      FileStream fileStream)
    {
      Win32FileNativeMethods.BY_HANDLE_FILE_INFORMATION lpFileInformation;
      return fileStream != null && Win32FileNativeMethods.GetFileInformationByHandle(fileStream.SafeFileHandle.DangerousGetHandle(), out lpFileInformation) ? new FileCharacteristics(DateTime.FromFileTimeUtc(lpFileInformation.ftCreationTime), DateTime.FromFileTimeUtc(lpFileInformation.ftLastWriteTime), (long) lpFileInformation.nFileSizeLow + ((long) lpFileInformation.nFileSizeHigh << 32)) : (FileCharacteristics) null;
    }
  }
}
