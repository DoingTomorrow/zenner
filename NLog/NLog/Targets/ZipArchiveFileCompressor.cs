// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ZipArchiveFileCompressor
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.IO;
using System.IO.Compression;

#nullable disable
namespace NLog.Targets
{
  internal class ZipArchiveFileCompressor : IFileCompressor
  {
    public void CompressFile(string fileName, string archiveFileName)
    {
      using (FileStream fileStream1 = new FileStream(archiveFileName, FileMode.Create))
      {
        using (ZipArchive zipArchive = new ZipArchive((Stream) fileStream1, ZipArchiveMode.Create))
        {
          using (FileStream fileStream2 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(fileName)).Open())
              fileStream2.CopyTo(destination);
          }
        }
      }
    }
  }
}
