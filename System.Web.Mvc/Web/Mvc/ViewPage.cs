// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewPage
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  [FileLevelControlBuilder(typeof (ViewPageControlBuilder))]
  public class ViewPage : Page, IViewDataContainer
  {
    [ThreadStatic]
    private static int _nextId;
    private DynamicViewDataDictionary _dynamicViewData;
    private string _masterLocation;
    private ViewDataDictionary _viewData;

    public AjaxHelper<object> Ajax { get; set; }

    public HtmlHelper<object> Html { get; set; }

    public string MasterLocation
    {
      get => this._masterLocation ?? string.Empty;
      set => this._masterLocation = value;
    }

    public object Model => this.ViewData.Model;

    public TempDataDictionary TempData => this.ViewContext.TempData;

    public UrlHelper Url { get; set; }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewData == null)
          this._dynamicViewData = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewData;
      }
    }

    public ViewContext ViewContext { get; set; }

    public ViewDataDictionary ViewData
    {
      get
      {
        if (this._viewData == null)
          this.SetViewData(new ViewDataDictionary());
        return this._viewData;
      }
      set => this.SetViewData(value);
    }

    public HtmlTextWriter Writer { get; private set; }

    public virtual void InitHelpers()
    {
      this.Ajax = new AjaxHelper<object>(this.ViewContext, (IViewDataContainer) this);
      this.Html = new HtmlHelper<object>(this.ViewContext, (IViewDataContainer) this);
      this.Url = new UrlHelper(this.ViewContext.RequestContext);
    }

    internal static string NextId()
    {
      return (++ViewPage._nextId).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e);
      if (string.IsNullOrEmpty(this.MasterLocation))
        return;
      this.MasterPageFile = this.MasterLocation;
    }

    public override void ProcessRequest(HttpContext context)
    {
      this.ID = ViewPage.NextId();
      base.ProcessRequest(context);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      this.Writer = writer;
      try
      {
        base.Render(writer);
      }
      finally
      {
        this.Writer = (HtmlTextWriter) null;
      }
    }

    public virtual void RenderView(ViewContext viewContext)
    {
      this.ViewContext = viewContext;
      this.InitHelpers();
      bool flag = false;
      ViewPage.SwitchWriter writer = viewContext.HttpContext.Response.Output as ViewPage.SwitchWriter;
      try
      {
        if (writer == null)
        {
          writer = new ViewPage.SwitchWriter();
          flag = true;
        }
        using (writer.Scope(viewContext.Writer))
        {
          if (flag)
          {
            int nextId = ViewPage._nextId;
            try
            {
              ViewPage._nextId = 0;
              viewContext.HttpContext.Server.Execute(HttpHandlerUtil.WrapForServerExecute((IHttpHandler) this), (TextWriter) writer, true);
            }
            finally
            {
              ViewPage._nextId = nextId;
            }
          }
          else
            this.ProcessRequest(HttpContext.Current);
        }
      }
      finally
      {
        if (flag)
          writer.Dispose();
      }
    }

    [Obsolete("The TextWriter is now provided by the ViewContext object passed to the RenderView method.", true)]
    public void SetTextWriter(TextWriter textWriter)
    {
    }

    protected virtual void SetViewData(ViewDataDictionary viewData) => this._viewData = viewData;

    internal class SwitchWriter : TextWriter
    {
      public SwitchWriter()
        : base((IFormatProvider) CultureInfo.CurrentCulture)
      {
      }

      public override Encoding Encoding => this.InnerWriter.Encoding;

      public override IFormatProvider FormatProvider => this.InnerWriter.FormatProvider;

      internal TextWriter InnerWriter { get; set; }

      public override string NewLine
      {
        get => this.InnerWriter.NewLine;
        set => this.InnerWriter.NewLine = value;
      }

      public override void Close() => this.InnerWriter.Close();

      public override void Flush() => this.InnerWriter.Flush();

      public IDisposable Scope(TextWriter writer)
      {
        ViewPage.SwitchWriter.WriterScope writerScope = new ViewPage.SwitchWriter.WriterScope(this, this.InnerWriter);
        try
        {
          if (writer != this)
            this.InnerWriter = writer;
          return (IDisposable) writerScope;
        }
        catch
        {
          writerScope.Dispose();
          throw;
        }
      }

      public override void Write(bool value) => this.InnerWriter.Write(value);

      public override void Write(char value) => this.InnerWriter.Write(value);

      public override void Write(char[] buffer) => this.InnerWriter.Write(buffer);

      public override void Write(char[] buffer, int index, int count)
      {
        this.InnerWriter.Write(buffer, index, count);
      }

      public override void Write(Decimal value) => this.InnerWriter.Write(value);

      public override void Write(double value) => this.InnerWriter.Write(value);

      public override void Write(float value) => this.InnerWriter.Write(value);

      public override void Write(int value) => this.InnerWriter.Write(value);

      public override void Write(long value) => this.InnerWriter.Write(value);

      public override void Write(object value) => this.InnerWriter.Write(value);

      public override void Write(string format, object arg0)
      {
        this.InnerWriter.Write(format, arg0);
      }

      public override void Write(string format, object arg0, object arg1)
      {
        this.InnerWriter.Write(format, arg0, arg1);
      }

      public override void Write(string format, object arg0, object arg1, object arg2)
      {
        this.InnerWriter.Write(format, arg0, arg1, arg2);
      }

      public override void Write(string format, params object[] arg)
      {
        this.InnerWriter.Write(format, arg);
      }

      public override void Write(string value) => this.InnerWriter.Write(value);

      public override void Write(uint value) => this.InnerWriter.Write(value);

      public override void Write(ulong value) => this.InnerWriter.Write(value);

      public override void WriteLine() => this.InnerWriter.WriteLine();

      public override void WriteLine(bool value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(char value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(char[] buffer) => this.InnerWriter.WriteLine(buffer);

      public override void WriteLine(char[] buffer, int index, int count)
      {
        this.InnerWriter.WriteLine(buffer, index, count);
      }

      public override void WriteLine(Decimal value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(double value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(float value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(int value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(long value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(object value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(string format, object arg0)
      {
        this.InnerWriter.WriteLine(format, arg0);
      }

      public override void WriteLine(string format, object arg0, object arg1)
      {
        this.InnerWriter.WriteLine(format, arg0, arg1);
      }

      public override void WriteLine(string format, object arg0, object arg1, object arg2)
      {
        this.InnerWriter.WriteLine(format, arg0, arg1, arg2);
      }

      public override void WriteLine(string format, params object[] arg)
      {
        this.InnerWriter.WriteLine(format, arg);
      }

      public override void WriteLine(string value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(uint value) => this.InnerWriter.WriteLine(value);

      public override void WriteLine(ulong value) => this.InnerWriter.WriteLine(value);

      private sealed class WriterScope : IDisposable
      {
        private ViewPage.SwitchWriter _switchWriter;
        private TextWriter _writerToRestore;

        public WriterScope(ViewPage.SwitchWriter switchWriter, TextWriter writerToRestore)
        {
          this._switchWriter = switchWriter;
          this._writerToRestore = writerToRestore;
        }

        public void Dispose() => this._switchWriter.InnerWriter = this._writerToRestore;
      }
    }
  }
}
