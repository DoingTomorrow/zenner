// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PortableFileCharacteristicsHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal class PortableFileCharacteristicsHelper : FileCharacteristicsHelper
  {
    public override FileCharacteristics GetFileCharacteristics(
      string fileName,
      FileStream fileStream)
    {
      if (!string.IsNullOrEmpty(fileName))
      {
        FileInfo fileInfo = new FileInfo(fileName);
        if (fileInfo.Exists)
          return new FileCharacteristics(fileInfo.GetCreationTimeUtc(), fileInfo.GetLastWriteTimeUtc(), fileInfo.Length);
      }
      return (FileCharacteristics) null;
    }
  }
}
