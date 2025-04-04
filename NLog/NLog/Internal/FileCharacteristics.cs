// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileCharacteristics
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Internal
{
  internal class FileCharacteristics
  {
    public FileCharacteristics(
      DateTime creationTimeUtc,
      DateTime lastWriteTimeUtc,
      long fileLength)
    {
      this.CreationTimeUtc = creationTimeUtc;
      this.LastWriteTimeUtc = lastWriteTimeUtc;
      this.FileLength = fileLength;
    }

    public DateTime CreationTimeUtc { get; private set; }

    public DateTime LastWriteTimeUtc { get; private set; }

    public long FileLength { get; private set; }
  }
}
