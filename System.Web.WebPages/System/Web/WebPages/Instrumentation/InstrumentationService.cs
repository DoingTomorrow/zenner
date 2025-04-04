// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Instrumentation.InstrumentationService
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.IO;

#nullable disable
namespace System.Web.WebPages.Instrumentation
{
  public class InstrumentationService
  {
    private static readonly bool _isAvailable = HttpContextAdapter.IsInstrumentationAvailable;
    private bool _localIsAvailable = InstrumentationService._isAvailable && PageInstrumentationServiceAdapter.IsEnabled;

    public InstrumentationService()
    {
      this.ExtractInstrumentationService = new Func<HttpContextBase, PageInstrumentationServiceAdapter>(this.GetInstrumentationService);
      this.CreateContext = new Func<string, TextWriter, int, int, bool, PageExecutionContextAdapter>(this.CreateSystemWebContext);
    }

    public bool IsAvailable
    {
      get => this._localIsAvailable;
      internal set => this._localIsAvailable = value;
    }

    internal Func<HttpContextBase, PageInstrumentationServiceAdapter> ExtractInstrumentationService { get; set; }

    internal Func<string, TextWriter, int, int, bool, PageExecutionContextAdapter> CreateContext { get; set; }

    public void BeginContext(
      HttpContextBase context,
      string virtualPath,
      TextWriter writer,
      int startPosition,
      int length,
      bool isLiteral)
    {
      this.RunOnListeners(context, (Action<PageExecutionListenerAdapter>) (listener => listener.BeginContext(this.CreateContext(virtualPath, writer, startPosition, length, isLiteral))));
    }

    public void EndContext(
      HttpContextBase context,
      string virtualPath,
      TextWriter writer,
      int startPosition,
      int length,
      bool isLiteral)
    {
      this.RunOnListeners(context, (Action<PageExecutionListenerAdapter>) (listener => listener.EndContext(this.CreateContext(virtualPath, writer, startPosition, length, isLiteral))));
    }

    private PageExecutionContextAdapter CreateSystemWebContext(
      string virtualPath,
      TextWriter writer,
      int startPosition,
      int length,
      bool isLiteral)
    {
      return new PageExecutionContextAdapter()
      {
        VirtualPath = virtualPath,
        TextWriter = writer,
        StartPosition = startPosition,
        Length = length,
        IsLiteral = isLiteral
      };
    }

    private PageInstrumentationServiceAdapter GetInstrumentationService(HttpContextBase context)
    {
      return new HttpContextAdapter((object) context).PageInstrumentation;
    }

    private void RunOnListeners(HttpContextBase context, Action<PageExecutionListenerAdapter> act)
    {
      if (!this.IsAvailable)
        return;
      PageInstrumentationServiceAdapter instrumentationServiceAdapter = this.ExtractInstrumentationService(context);
      if (instrumentationServiceAdapter == null)
        return;
      foreach (PageExecutionListenerAdapter executionListener in instrumentationServiceAdapter.ExecutionListeners)
        act(executionListener);
    }
  }
}
