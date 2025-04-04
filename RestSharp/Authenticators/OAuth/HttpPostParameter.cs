// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.HttpPostParameter
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.IO;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  internal class HttpPostParameter(string name, string value) : WebParameter(name, value)
  {
    public virtual HttpPostParameterType Type { get; private set; }

    public virtual string FileName { get; private set; }

    public virtual string FilePath { get; private set; }

    public virtual Stream FileStream { get; set; }

    public virtual string ContentType { get; private set; }

    public static HttpPostParameter CreateFile(
      string name,
      string fileName,
      string filePath,
      string contentType)
    {
      return new HttpPostParameter(name, string.Empty)
      {
        Type = HttpPostParameterType.File,
        FileName = fileName,
        FilePath = filePath,
        ContentType = contentType
      };
    }

    public static HttpPostParameter CreateFile(
      string name,
      string fileName,
      Stream fileStream,
      string contentType)
    {
      return new HttpPostParameter(name, string.Empty)
      {
        Type = HttpPostParameterType.File,
        FileName = fileName,
        FileStream = fileStream,
        ContentType = contentType
      };
    }
  }
}
