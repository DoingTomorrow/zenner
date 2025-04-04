// Decompiled with JetBrains decompiler
// Type: RestSharp.Extensions.MiscExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.IO;
using System.Text;

#nullable disable
namespace RestSharp.Extensions
{
  public static class MiscExtensions
  {
    public static void SaveAs(this byte[] input, string path) => File.WriteAllBytes(path, input);

    public static byte[] ReadAsBytes(this Stream input)
    {
      byte[] buffer = new byte[16384];
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int count;
        while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
          memoryStream.Write(buffer, 0, count);
        return memoryStream.ToArray();
      }
    }

    public static void CopyTo(this Stream input, Stream output)
    {
      byte[] buffer = new byte[32768];
      while (true)
      {
        int count = input.Read(buffer, 0, buffer.Length);
        if (count > 0)
          output.Write(buffer, 0, count);
        else
          break;
      }
    }

    public static string AsString(this byte[] buffer)
    {
      return buffer == null ? "" : Encoding.UTF8.GetString(buffer);
    }
  }
}
