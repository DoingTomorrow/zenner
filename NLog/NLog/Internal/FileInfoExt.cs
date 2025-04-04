// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileInfoExt
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal static class FileInfoExt
  {
    public static DateTime GetLastWriteTimeUtc(this FileInfo fileInfo) => fileInfo.LastWriteTimeUtc;

    public static DateTime GetCreationTimeUtc(this FileInfo fileInfo) => fileInfo.CreationTimeUtc;
  }
}
