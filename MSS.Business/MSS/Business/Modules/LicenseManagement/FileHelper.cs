// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.FileHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.IO;
using System.Reflection;

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public static class FileHelper
  {
    public static MemoryStream OpenFile(string fileName)
    {
      FileStream fileStream = File.OpenRead(fileName);
      MemoryStream memoryStream = new MemoryStream();
      byte[] buffer = new byte[fileStream.Length];
      fileStream.Read(buffer, 0, buffer.Length);
      memoryStream.Write(buffer, 0, (int) fileStream.Length);
      fileStream.Flush();
      fileStream.Close();
      return memoryStream;
    }

    public static void SaveToDisk(this MemoryStream ms, string fileName)
    {
      FileStream fileStream = File.OpenWrite(fileName);
      fileStream.SetLength(0L);
      fileStream.Flush();
      ms.WriteTo((Stream) fileStream);
      fileStream.Flush();
      fileStream.Close();
    }

    public static string GetCurrentDirectoryPath()
    {
      return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
  }
}
