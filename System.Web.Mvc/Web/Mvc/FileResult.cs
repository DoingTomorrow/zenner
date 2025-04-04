// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FileResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Net.Mime;
using System.Text;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class FileResult : ActionResult
  {
    private string _fileDownloadName;

    protected FileResult(string contentType)
    {
      this.ContentType = !string.IsNullOrEmpty(contentType) ? contentType : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (contentType));
    }

    public string ContentType { get; private set; }

    public string FileDownloadName
    {
      get => this._fileDownloadName ?? string.Empty;
      set => this._fileDownloadName = value;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      HttpResponseBase response = context.HttpContext.Response;
      response.ContentType = this.ContentType;
      if (!string.IsNullOrEmpty(this.FileDownloadName))
      {
        string headerValue = FileResult.ContentDispositionUtil.GetHeaderValue(this.FileDownloadName);
        context.HttpContext.Response.AddHeader("Content-Disposition", headerValue);
      }
      this.WriteFile(response);
    }

    protected abstract void WriteFile(HttpResponseBase response);

    internal static class ContentDispositionUtil
    {
      private const string HexDigits = "0123456789ABCDEF";

      private static void AddByteToStringBuilder(byte b, StringBuilder builder)
      {
        builder.Append('%');
        int num = (int) b;
        FileResult.ContentDispositionUtil.AddHexDigitToStringBuilder(num >> 4, builder);
        FileResult.ContentDispositionUtil.AddHexDigitToStringBuilder(num % 16, builder);
      }

      private static void AddHexDigitToStringBuilder(int digit, StringBuilder builder)
      {
        builder.Append("0123456789ABCDEF"[digit]);
      }

      private static string CreateRfc2231HeaderValue(string filename)
      {
        StringBuilder builder = new StringBuilder("attachment; filename*=UTF-8''");
        foreach (byte b in Encoding.UTF8.GetBytes(filename))
        {
          if (FileResult.ContentDispositionUtil.IsByteValidHeaderValueCharacter(b))
            builder.Append((char) b);
          else
            FileResult.ContentDispositionUtil.AddByteToStringBuilder(b, builder);
        }
        return builder.ToString();
      }

      public static string GetHeaderValue(string fileName)
      {
        foreach (char ch in fileName)
        {
          if (ch > '\u007F')
            return FileResult.ContentDispositionUtil.CreateRfc2231HeaderValue(fileName);
        }
        return new ContentDisposition()
        {
          FileName = fileName
        }.ToString();
      }

      private static bool IsByteValidHeaderValueCharacter(byte b)
      {
        if ((byte) 48 <= b && b <= (byte) 57 || (byte) 97 <= b && b <= (byte) 122 || (byte) 65 <= b && b <= (byte) 90)
          return true;
        switch (b)
        {
          case 33:
          case 36:
          case 38:
          case 43:
          case 45:
          case 46:
          case 58:
          case 95:
          case 126:
            return true;
          default:
            return false;
        }
      }
    }
  }
}
