// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Razor.MvcWebPageRazorHost
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.WebPages.Razor;

#nullable disable
namespace System.Web.Mvc.Razor
{
  public class MvcWebPageRazorHost : WebPageRazorHost
  {
    public MvcWebPageRazorHost(string virtualPath, string physicalPath)
      : base(virtualPath, physicalPath)
    {
      this.RegisterSpecialFile(RazorViewEngine.ViewStartFileName, typeof (ViewStartPage));
      this.DefaultPageBaseClass = typeof (WebViewPage).FullName;
      this.GetRidOfNamespace("System.Web.WebPages.Html");
    }

    public virtual RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
    {
      return incomingCodeGenerator is CSharpRazorCodeGenerator ? (RazorCodeGenerator) new MvcCSharpRazorCodeGenerator(incomingCodeGenerator.ClassName, incomingCodeGenerator.RootNamespaceName, incomingCodeGenerator.SourceFileName, incomingCodeGenerator.Host) : base.DecorateCodeGenerator(incomingCodeGenerator);
    }

    public virtual ParserBase DecorateCodeParser(ParserBase incomingCodeParser)
    {
      switch (incomingCodeParser)
      {
        case CSharpCodeParser _:
          return (ParserBase) new MvcCSharpRazorCodeParser();
        case VBCodeParser _:
          return (ParserBase) new MvcVBRazorCodeParser();
        default:
          return base.DecorateCodeParser(incomingCodeParser);
      }
    }

    private void GetRidOfNamespace(string ns)
    {
      if (!this.NamespaceImports.Contains(ns))
        return;
      this.NamespaceImports.Remove(ns);
    }
  }
}
