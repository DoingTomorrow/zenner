// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FileContentResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class FileContentResult : FileResult
  {
    public FileContentResult(byte[] fileContents, string contentType)
      : base(contentType)
    {
      this.FileContents = fileContents != null ? fileContents : throw new ArgumentNullException(nameof (fileContents));
    }

    public byte[] FileContents { get; private set; }

    protected override void WriteFile(HttpResponseBase response)
    {
      response.OutputStream.Write(this.FileContents, 0, this.FileContents.Length);
    }
  }
}
