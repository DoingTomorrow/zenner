// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.CustomUri
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Text;

#nullable disable
namespace Castle.Core.Resource
{
  [Serializable]
  public sealed class CustomUri
  {
    public static readonly string SchemeDelimiter = "://";
    public static readonly string UriSchemeFile = "file";
    public static readonly string UriSchemeAssembly = "assembly";
    private string scheme;
    private string host;
    private string path;
    private bool isUnc;
    private bool isFile;
    private bool isAssembly;

    public CustomUri(string resourceIdentifier)
    {
      if (resourceIdentifier == null)
        throw new ArgumentNullException(nameof (resourceIdentifier));
      if (resourceIdentifier == string.Empty)
        throw new ArgumentException("Empty resource identifier is not allowed", nameof (resourceIdentifier));
      this.ParseIdentifier(resourceIdentifier);
    }

    public bool IsUnc => this.isUnc;

    public bool IsFile => this.isFile;

    public bool IsAssembly => this.isAssembly;

    public string Scheme => this.scheme;

    public string Host => this.host;

    public string Path => this.path;

    private void ParseIdentifier(string identifier)
    {
      int length = identifier.IndexOf(':');
      if (length == -1 && (identifier[0] != '\\' || identifier[1] != '\\') && identifier[0] != '/')
        throw new ArgumentException("Invalid Uri: no scheme delimiter found on " + identifier);
      bool flag = true;
      if (identifier[0] == '\\' && identifier[1] == '\\')
      {
        this.isUnc = true;
        this.isFile = true;
        this.scheme = CustomUri.UriSchemeFile;
        flag = false;
      }
      else if (identifier[length + 1] == '/' && identifier[length + 2] == '/')
      {
        this.scheme = identifier.Substring(0, length);
        this.isFile = this.scheme == CustomUri.UriSchemeFile;
        this.isAssembly = this.scheme == CustomUri.UriSchemeAssembly;
        identifier = identifier.Substring(length + CustomUri.SchemeDelimiter.Length);
      }
      else
      {
        this.isFile = true;
        this.scheme = CustomUri.UriSchemeFile;
      }
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in identifier.ToCharArray())
      {
        if (flag && (ch == '\\' || ch == '/'))
        {
          if (this.host == null && !this.IsFile)
          {
            this.host = stringBuilder.ToString();
            stringBuilder.Length = 0;
          }
          stringBuilder.Append('/');
        }
        else
          stringBuilder.Append(ch);
      }
      this.path = Environment.ExpandEnvironmentVariables(stringBuilder.ToString());
    }
  }
}
