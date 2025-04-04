// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.StartPage
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class StartPage : WebPageRenderingBase
  {
    public WebPageRenderingBase ChildPage { get; set; }

    public override HttpContextBase Context
    {
      get => this.ChildPage.Context;
      set => this.ChildPage.Context = value;
    }

    public override string Layout
    {
      get => this.ChildPage.Layout;
      set
      {
        if (value == null)
          this.ChildPage.Layout = (string) null;
        else
          this.ChildPage.Layout = this.NormalizeLayoutPagePath(value);
      }
    }

    public override IDictionary<object, object> PageData => this.ChildPage.PageData;

    public override object Page => this.ChildPage.Page;

    internal bool RunPageCalled { get; set; }

    public override void ExecutePageHierarchy()
    {
      TemplateStack.Push(this.Context, (ITemplateFile) this);
      try
      {
        this.Execute();
        if (this.RunPageCalled)
          return;
        this.RunPage();
      }
      finally
      {
        TemplateStack.Pop(this.Context);
      }
    }

    public static WebPageRenderingBase GetStartPage(
      WebPageRenderingBase page,
      string fileName,
      IEnumerable<string> supportedExtensions)
    {
      if (page == null)
        throw new ArgumentNullException(nameof (page));
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
        {
          (object) nameof (fileName)
        }), nameof (fileName));
      if (supportedExtensions == null)
        throw new ArgumentNullException(nameof (supportedExtensions));
      return StartPage.GetStartPage(page, page.VirtualPathFactory ?? (IVirtualPathFactory) VirtualPathFactoryManager.Instance, HttpRuntime.AppDomainAppVirtualPath, fileName, supportedExtensions);
    }

    internal static WebPageRenderingBase GetStartPage(
      WebPageRenderingBase page,
      IVirtualPathFactory virtualPathFactory,
      string appDomainAppVirtualPath,
      string fileName,
      IEnumerable<string> supportedExtensions)
    {
      WebPageRenderingBase startPage = page;
      for (string directory = VirtualPathUtility.GetDirectory(page.VirtualPath); !string.IsNullOrEmpty(directory) && directory != "/" && PathUtil.IsWithinAppRoot(appDomainAppVirtualPath, directory); directory = startPage.GetDirectory(directory))
      {
        foreach (string supportedExtension in supportedExtensions)
        {
          string virtualPath = VirtualPathUtility.Combine(directory, fileName + "." + supportedExtension);
          if (virtualPathFactory.Exists(virtualPath))
          {
            StartPage instance = virtualPathFactory.CreateInstance<StartPage>(virtualPath);
            instance.VirtualPath = virtualPath;
            instance.ChildPage = startPage;
            instance.VirtualPathFactory = virtualPathFactory;
            startPage = (WebPageRenderingBase) instance;
            break;
          }
        }
      }
      return startPage;
    }

    public override HelperResult RenderPage(string path, params object[] data)
    {
      return this.ChildPage.RenderPage(this.NormalizePath(path), data);
    }

    public void RunPage()
    {
      this.RunPageCalled = true;
      this.ChildPage.ExecutePageHierarchy();
    }

    public override void Write(HelperResult result) => this.ChildPage.Write(result);

    public override void WriteLiteral(object value) => this.ChildPage.WriteLiteral(value);

    public override void Write(object value) => this.ChildPage.Write(value);

    protected internal override TextWriter GetOutputWriter() => this.ChildPage.GetOutputWriter();
  }
}
