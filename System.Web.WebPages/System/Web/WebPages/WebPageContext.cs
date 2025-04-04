// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageContext
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.IO;
using System.Web.WebPages.Html;

#nullable disable
namespace System.Web.WebPages
{
  public class WebPageContext
  {
    private static readonly object _sourceFileKey = new object();
    private Stack<TextWriter> _outputStack;
    private Stack<Dictionary<string, SectionWriter>> _sectionWritersStack;
    private IDictionary<object, object> _pageData;
    private ValidationHelper _validation;
    private ModelStateDictionary _modelStateDictionary;

    public WebPageContext()
      : this((HttpContextBase) null, (WebPageRenderingBase) null, (object) null)
    {
    }

    public WebPageContext(HttpContextBase context, WebPageRenderingBase page, object model)
    {
      this.HttpContext = context;
      this.Page = page;
      this.Model = model;
    }

    public static WebPageContext Current
    {
      get
      {
        System.Web.HttpContext current = System.Web.HttpContext.Current;
        if (current == null)
          return (WebPageContext) null;
        return TemplateStack.GetCurrentTemplate((HttpContextBase) new HttpContextWrapper(current)) is WebPageRenderingBase currentTemplate ? currentTemplate.PageContext : (WebPageContext) null;
      }
    }

    internal HttpContextBase HttpContext { get; set; }

    public object Model { get; internal set; }

    internal ModelStateDictionary ModelState
    {
      get
      {
        if (this._modelStateDictionary == null)
          this._modelStateDictionary = new ModelStateDictionary();
        return this._modelStateDictionary;
      }
    }

    internal ValidationHelper Validation
    {
      get
      {
        if (this._validation == null)
          this._validation = new ValidationHelper(this.HttpContext, this.ModelState);
        return this._validation;
      }
      private set => this._validation = value;
    }

    internal Action<TextWriter> BodyAction { get; set; }

    internal Stack<TextWriter> OutputStack
    {
      get
      {
        if (this._outputStack == null)
          this._outputStack = new Stack<TextWriter>();
        return this._outputStack;
      }
      set => this._outputStack = value;
    }

    public WebPageRenderingBase Page { get; internal set; }

    public IDictionary<object, object> PageData
    {
      get
      {
        if (this._pageData == null)
          this._pageData = (IDictionary<object, object>) new PageDataDictionary<object>();
        return this._pageData;
      }
      internal set => this._pageData = value;
    }

    internal Stack<Dictionary<string, SectionWriter>> SectionWritersStack
    {
      get
      {
        if (this._sectionWritersStack == null)
          this._sectionWritersStack = new Stack<Dictionary<string, SectionWriter>>();
        return this._sectionWritersStack;
      }
      set => this._sectionWritersStack = value;
    }

    internal HashSet<string> SourceFiles
    {
      get
      {
        if (!(this.HttpContext.Items[WebPageContext._sourceFileKey] is HashSet<string> sourceFiles))
        {
          sourceFiles = new HashSet<string>();
          this.HttpContext.Items[WebPageContext._sourceFileKey] = (object) sourceFiles;
        }
        return sourceFiles;
      }
    }

    internal static WebPageContext CreateNestedPageContext<TModel>(
      WebPageContext parentContext,
      IDictionary<object, object> pageData,
      TModel model,
      bool isLayoutPage)
    {
      WebPageContext nestedPageContext = new WebPageContext()
      {
        HttpContext = parentContext.HttpContext,
        OutputStack = parentContext.OutputStack,
        Validation = parentContext.Validation,
        PageData = pageData,
        Model = (object) model
      };
      if (isLayoutPage)
      {
        nestedPageContext.BodyAction = parentContext.BodyAction;
        nestedPageContext.SectionWritersStack = parentContext.SectionWritersStack;
      }
      return nestedPageContext;
    }
  }
}
