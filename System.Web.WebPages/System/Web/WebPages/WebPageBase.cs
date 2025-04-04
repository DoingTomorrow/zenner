// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageBase
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class WebPageBase : WebPageRenderingBase
  {
    private HashSet<string> _renderedSections = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private bool _renderedBody;
    private Action<TextWriter> _body;
    private TextWriter _tempWriter;
    private TextWriter _currentWriter;
    private DynamicPageDataDictionary<object> _dynamicPageData;

    public override string Layout { get; set; }

    public TextWriter Output => this.OutputStack.Peek();

    public Stack<TextWriter> OutputStack => this.PageContext.OutputStack;

    public override IDictionary<object, object> PageData => this.PageContext.PageData;

    public override object Page
    {
      get
      {
        if (this._dynamicPageData == null)
          this._dynamicPageData = new DynamicPageDataDictionary<object>((PageDataDictionary<object>) this.PageData);
        return (object) this._dynamicPageData;
      }
    }

    private Dictionary<string, SectionWriter> PreviousSectionWriters
    {
      get
      {
        Dictionary<string, SectionWriter> dictionary = this.SectionWritersStack.Pop();
        Dictionary<string, SectionWriter> previousSectionWriters = this.SectionWritersStack.Count > 0 ? this.SectionWritersStack.Peek() : (Dictionary<string, SectionWriter>) null;
        this.SectionWritersStack.Push(dictionary);
        return previousSectionWriters;
      }
    }

    private Dictionary<string, SectionWriter> SectionWriters => this.SectionWritersStack.Peek();

    private Stack<Dictionary<string, SectionWriter>> SectionWritersStack
    {
      get => this.PageContext.SectionWritersStack;
    }

    protected virtual void ConfigurePage(WebPageBase parentPage)
    {
    }

    public static WebPageBase CreateInstanceFromVirtualPath(string virtualPath)
    {
      return WebPageBase.CreateInstanceFromVirtualPath(virtualPath, (IVirtualPathFactory) VirtualPathFactoryManager.Instance);
    }

    internal static WebPageBase CreateInstanceFromVirtualPath(
      string virtualPath,
      IVirtualPathFactory virtualPathFactory)
    {
      try
      {
        WebPageBase instance = virtualPathFactory.CreateInstance<WebPageBase>(virtualPath);
        instance.VirtualPath = virtualPath;
        instance.VirtualPathFactory = virtualPathFactory;
        return instance;
      }
      catch (HttpException ex)
      {
        BuildManagerExceptionUtil.ThrowIfUnsupportedExtension(virtualPath, ex);
        throw;
      }
    }

    private WebPageBase CreatePageFromVirtualPath(
      string virtualPath,
      HttpContextBase httpContext,
      Func<string, bool> virtualPathExists,
      DisplayModeProvider displayModeProvider,
      IDisplayMode displayMode)
    {
      try
      {
        DisplayInfo infoForVirtualPath = displayModeProvider.GetDisplayInfoForVirtualPath(virtualPath, httpContext, virtualPathExists, displayMode);
        if (infoForVirtualPath != null)
        {
          WebPageBase instance = this.VirtualPathFactory.CreateInstance<WebPageBase>(infoForVirtualPath.FilePath);
          if (instance != null)
          {
            instance.VirtualPath = virtualPath;
            instance.VirtualPathFactory = this.VirtualPathFactory;
            instance.DisplayModeProvider = this.DisplayModeProvider;
            return instance;
          }
        }
      }
      catch (HttpException ex)
      {
        BuildManagerExceptionUtil.ThrowIfUnsupportedExtension(virtualPath, ex);
        BuildManagerExceptionUtil.ThrowIfCodeDomDefinedExtension(virtualPath, ex);
        throw;
      }
      throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_InvalidPageType, new object[1]
      {
        (object) virtualPath
      }));
    }

    private WebPageContext CreatePageContextFromParameters(bool isLayoutPage, params object[] data)
    {
      object model = (object) null;
      if (data != null && data.Length > 0)
        model = data[0];
      return WebPageContext.CreateNestedPageContext<object>(this.PageContext, PageDataDictionary<object>.CreatePageDataFromParameters(this.PageData, data), model, isLayoutPage);
    }

    public void DefineSection(string name, SectionWriter action)
    {
      if (this.SectionWriters.ContainsKey(name))
        throw new HttpException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, WebPageResources.WebPage_SectionAleadyDefined, new object[1]
        {
          (object) name
        }));
      this.SectionWriters[name] = action;
    }

    internal void EnsurePageCanBeRequestedDirectly(string methodName)
    {
      if (this.PreviousSectionWriters == null)
        throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_CannotRequestDirectly, new object[2]
        {
          (object) this.VirtualPath,
          (object) methodName
        }));
    }

    public void ExecutePageHierarchy(WebPageContext pageContext, TextWriter writer)
    {
      WebPageRenderingBase startPage = (WebPageRenderingBase) null;
      this.ExecutePageHierarchy(pageContext, writer, startPage);
    }

    public void ExecutePageHierarchy(
      WebPageContext pageContext,
      TextWriter writer,
      WebPageRenderingBase startPage)
    {
      this.PushContext(pageContext, writer);
      if (startPage != null)
      {
        if (startPage != this)
        {
          WebPageContext nestedPageContext = WebPageContext.CreateNestedPageContext<object>(pageContext, (IDictionary<object, object>) null, (object) null, false);
          nestedPageContext.Page = startPage;
          startPage.PageContext = nestedPageContext;
        }
        startPage.ExecutePageHierarchy();
      }
      else
        this.ExecutePageHierarchy();
      this.PopContext();
    }

    public override void ExecutePageHierarchy()
    {
      if (WebPageHttpHandler.ShouldGenerateSourceHeader(this.Context))
      {
        try
        {
          string virtualPath = this.VirtualPath;
          if (virtualPath != null)
          {
            string str = this.Context.Request.MapPath(virtualPath);
            if (!str.IsEmpty())
              this.PageContext.SourceFiles.Add(str);
          }
        }
        catch
        {
        }
      }
      TemplateStack.Push(this.Context, (ITemplateFile) this);
      try
      {
        this.Execute();
      }
      finally
      {
        TemplateStack.Pop(this.Context);
      }
    }

    protected virtual void InitializePage()
    {
    }

    public bool IsSectionDefined(string name)
    {
      this.EnsurePageCanBeRequestedDirectly(nameof (IsSectionDefined));
      return this.PreviousSectionWriters.ContainsKey(name);
    }

    public void PopContext()
    {
      string renderedContent = this._tempWriter.ToString();
      this.OutputStack.Pop();
      if (!string.IsNullOrEmpty(this.Layout))
      {
        string partialViewName = this.NormalizeLayoutPagePath(this.Layout);
        this.OutputStack.Push(this._currentWriter);
        this.RenderSurrounding(partialViewName, (Action<TextWriter>) (w => w.Write(renderedContent)));
        this.OutputStack.Pop();
      }
      else
        this._currentWriter.Write(renderedContent);
      this.VerifyRenderedBodyOrSections();
      this.SectionWritersStack.Pop();
    }

    public void PushContext(WebPageContext pageContext, TextWriter writer)
    {
      this._currentWriter = writer;
      this.PageContext = pageContext;
      pageContext.Page = (WebPageRenderingBase) this;
      this.InitializePage();
      this._tempWriter = (TextWriter) new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      this.OutputStack.Push(this._tempWriter);
      this.SectionWritersStack.Push(new Dictionary<string, SectionWriter>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase));
      if (this.PageContext.BodyAction == null)
        return;
      this._body = this.PageContext.BodyAction;
      this.PageContext.BodyAction = (Action<TextWriter>) null;
    }

    public HelperResult RenderBody()
    {
      this.EnsurePageCanBeRequestedDirectly(nameof (RenderBody));
      this._renderedBody = !this._renderedBody ? true : throw new HttpException(WebPageResources.WebPage_RenderBodyAlreadyCalled);
      return this._body != null ? new HelperResult((Action<TextWriter>) (tw => this._body(tw))) : throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_CannotRequestDirectly, new object[2]
      {
        (object) this.VirtualPath,
        (object) nameof (RenderBody)
      }));
    }

    public override HelperResult RenderPage(string path, params object[] data)
    {
      bool isLayoutPage = false;
      object[] data1 = data;
      return this.RenderPageCore(path, isLayoutPage, data1);
    }

    private HelperResult RenderPageCore(string path, bool isLayoutPage, object[] data)
    {
      if (string.IsNullOrEmpty(path))
        throw ExceptionHelper.CreateArgumentNullOrEmptyException(nameof (path));
      return new HelperResult((Action<TextWriter>) (writer =>
      {
        path = this.NormalizePath(path);
        WebPageBase pageFromVirtualPath = this.CreatePageFromVirtualPath(path, this.Context, new Func<string, bool>(this.VirtualPathFactory.Exists), this.DisplayModeProvider, this.DisplayMode);
        WebPageContext contextFromParameters = this.CreatePageContextFromParameters(isLayoutPage, data);
        pageFromVirtualPath.ConfigurePage(this);
        pageFromVirtualPath.ExecutePageHierarchy(contextFromParameters, writer);
      }));
    }

    public HelperResult RenderSection(string name)
    {
      bool required = true;
      return this.RenderSection(name, required);
    }

    public HelperResult RenderSection(string name, bool required)
    {
      this.EnsurePageCanBeRequestedDirectly(nameof (RenderSection));
      if (this.PreviousSectionWriters.ContainsKey(name))
        return new HelperResult((Action<TextWriter>) (tw =>
        {
          SectionWriter sectionWriter = !this._renderedSections.Contains(name) ? this.PreviousSectionWriters[name] : throw new HttpException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, WebPageResources.WebPage_SectionAleadyRendered, new object[1]
          {
            (object) name
          }));
          Dictionary<string, SectionWriter> dictionary = this.SectionWritersStack.Pop();
          bool flag = false;
          try
          {
            if (this.Output != tw)
            {
              this.OutputStack.Push(tw);
              flag = true;
            }
            sectionWriter();
          }
          finally
          {
            if (flag)
              this.OutputStack.Pop();
          }
          this.SectionWritersStack.Push(dictionary);
          this._renderedSections.Add(name);
        }));
      if (required)
        throw new HttpException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, WebPageResources.WebPage_SectionNotDefined, new object[1]
        {
          (object) name
        }));
      return (HelperResult) null;
    }

    private void RenderSurrounding(string partialViewName, Action<TextWriter> body)
    {
      Action<TextWriter> bodyAction = this.PageContext.BodyAction;
      this.PageContext.BodyAction = body;
      bool isLayoutPage = true;
      object[] data = new object[0];
      this.Write(this.RenderPageCore(partialViewName, isLayoutPage, data));
      this.PageContext.BodyAction = bodyAction;
    }

    private void VerifyRenderedBodyOrSections()
    {
      if (this._body == null)
        return;
      if (this.SectionWritersStack.Count > 1 && this.PreviousSectionWriters != null && this.PreviousSectionWriters.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string key in this.PreviousSectionWriters.Keys)
        {
          if (!this._renderedSections.Contains(key))
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Append("; ");
            stringBuilder.Append(key);
          }
        }
        if (stringBuilder.Length > 0)
          throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_SectionsNotRendered, new object[2]
          {
            (object) this.VirtualPath,
            (object) stringBuilder.ToString()
          }));
      }
      else if (!this._renderedBody)
        throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_RenderBodyNotCalled, new object[1]
        {
          (object) this.VirtualPath
        }));
    }

    public override void Write(HelperResult result)
    {
      WebPageExecutingBase.WriteTo(this.Output, result);
    }

    public override void Write(object value) => WebPageExecutingBase.WriteTo(this.Output, value);

    public override void WriteLiteral(object value) => this.Output.Write(value);

    protected internal override TextWriter GetOutputWriter() => this.Output;
  }
}
