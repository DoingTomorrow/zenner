// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageMatch
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  internal sealed class WebPageMatch
  {
    public WebPageMatch(string matchedPath, string pathInfo)
    {
      this.MatchedPath = matchedPath;
      this.PathInfo = pathInfo;
    }

    public string MatchedPath { get; private set; }

    public string PathInfo { get; private set; }
  }
}
