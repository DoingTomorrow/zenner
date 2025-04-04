// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FileStreamResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.IO;

#nullable disable
namespace System.Web.Mvc
{
  public class FileStreamResult : FileResult
  {
    private const int BufferSize = 4096;

    public FileStreamResult(Stream fileStream, string contentType)
      : base(contentType)
    {
      this.FileStream = fileStream != null ? fileStream : throw new ArgumentNullException(nameof (fileStream));
    }

    public Stream FileStream { get; private set; }

    protected override void WriteFile(HttpResponseBase response)
    {
      Stream outputStream = response.OutputStream;
      using (this.FileStream)
      {
        byte[] buffer = new byte[4096];
        while (true)
        {
          int count = this.FileStream.Read(buffer, 0, 4096);
          if (count != 0)
            outputStream.Write(buffer, 0, count);
          else
            break;
        }
      }
    }
  }
}
