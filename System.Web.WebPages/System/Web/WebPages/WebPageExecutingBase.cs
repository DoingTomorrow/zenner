// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageExecutingBase
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web.WebPages.Instrumentation;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class WebPageExecutingBase
  {
    private IVirtualPathFactory _virtualPathFactory;
    private DynamicHttpApplicationState _dynamicAppState;
    private InstrumentationService _instrumentationService;

    internal InstrumentationService InstrumentationService
    {
      get
      {
        if (this._instrumentationService == null)
          this._instrumentationService = new InstrumentationService();
        return this._instrumentationService;
      }
      set => this._instrumentationService = value;
    }

    public virtual HttpApplicationStateBase AppState
    {
      get => this.Context != null ? this.Context.Application : (HttpApplicationStateBase) null;
    }

    public virtual object App
    {
      get
      {
        if (this._dynamicAppState == null && this.AppState != null)
          this._dynamicAppState = new DynamicHttpApplicationState(this.AppState);
        return (object) this._dynamicAppState;
      }
    }

    public virtual HttpContextBase Context { get; set; }

    public virtual string VirtualPath { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual IVirtualPathFactory VirtualPathFactory
    {
      get => this._virtualPathFactory ?? (IVirtualPathFactory) VirtualPathFactoryManager.Instance;
      set => this._virtualPathFactory = value;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract void Execute();

    public virtual string Href(string path, params object[] pathParts)
    {
      return UrlUtil.Url(this.VirtualPath, path, pathParts);
    }

    protected internal void BeginContext(int startPosition, int length, bool isLiteral)
    {
      this.BeginContext(this.GetOutputWriter(), this.VirtualPath, startPosition, length, isLiteral);
    }

    protected internal void BeginContext(
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      this.BeginContext(this.GetOutputWriter(), virtualPath, startPosition, length, isLiteral);
    }

    protected internal void BeginContext(
      TextWriter writer,
      int startPosition,
      int length,
      bool isLiteral)
    {
      this.BeginContext(writer, this.VirtualPath, startPosition, length, isLiteral);
    }

    protected internal void BeginContext(
      TextWriter writer,
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      if (!this.InstrumentationService.IsAvailable)
        return;
      this.InstrumentationService.BeginContext(this.Context, virtualPath, writer, startPosition, length, isLiteral);
    }

    protected internal void EndContext(int startPosition, int length, bool isLiteral)
    {
      this.EndContext(this.GetOutputWriter(), this.VirtualPath, startPosition, length, isLiteral);
    }

    protected internal void EndContext(
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      this.EndContext(this.GetOutputWriter(), virtualPath, startPosition, length, isLiteral);
    }

    protected internal void EndContext(
      TextWriter writer,
      int startPosition,
      int length,
      bool isLiteral)
    {
      this.EndContext(writer, this.VirtualPath, startPosition, length, isLiteral);
    }

    protected internal void EndContext(
      TextWriter writer,
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      if (!this.InstrumentationService.IsAvailable)
        return;
      this.InstrumentationService.EndContext(this.Context, virtualPath, writer, startPosition, length, isLiteral);
    }

    internal virtual string GetDirectory(string virtualPath)
    {
      return VirtualPathUtility.GetDirectory(virtualPath);
    }

    internal string NormalizeLayoutPagePath(string layoutPagePath)
    {
      string virtualPath = this.NormalizePath(layoutPagePath);
      return this.VirtualPathFactory.Exists(virtualPath) ? virtualPath : throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_LayoutPageNotFound, new object[2]
      {
        (object) layoutPagePath,
        (object) virtualPath
      }));
    }

    public virtual string NormalizePath(string path)
    {
      return VirtualPathUtility.Combine(this.VirtualPath, path);
    }

    public abstract void Write(HelperResult result);

    public abstract void Write(object value);

    public abstract void WriteLiteral(object value);

    public virtual void WriteAttribute(
      string name,
      PositionTagged<string> prefix,
      PositionTagged<string> suffix,
      params AttributeValue[] values)
    {
      this.WriteAttributeTo(this.GetOutputWriter(), name, prefix, suffix, values);
    }

    public virtual void WriteAttributeTo(
      TextWriter writer,
      string name,
      PositionTagged<string> prefix,
      PositionTagged<string> suffix,
      params AttributeValue[] values)
    {
      this.WriteAttributeTo(this.VirtualPath, writer, name, prefix, suffix, values);
    }

    protected internal virtual void WriteAttributeTo(
      string pageVirtualPath,
      TextWriter writer,
      string name,
      PositionTagged<string> prefix,
      PositionTagged<string> suffix,
      params AttributeValue[] values)
    {
      bool flag1 = true;
      bool flag2 = false;
      if (values.Length == 0)
      {
        this.WritePositionTaggedLiteral(writer, pageVirtualPath, prefix);
        this.WritePositionTaggedLiteral(writer, pageVirtualPath, suffix);
      }
      else
      {
        for (int index = 0; index < values.Length; ++index)
        {
          AttributeValue attributeValue = values[index];
          PositionTagged<object> positionTagged1 = attributeValue.Value;
          PositionTagged<string> positionTagged2 = index == values.Length - 1 ? suffix : values[index + 1].Prefix;
          bool? nullable = new bool?();
          if (positionTagged1.Value is bool)
            nullable = new bool?((bool) positionTagged1.Value);
          if (positionTagged1.Value != null && (!nullable.HasValue || nullable.Value))
          {
            if (!(positionTagged1.Value is string content))
              content = positionTagged1.Value.ToString();
            if (nullable.HasValue)
              content = name;
            if (flag1)
            {
              this.WritePositionTaggedLiteral(writer, pageVirtualPath, prefix);
              flag1 = false;
            }
            else
              this.WritePositionTaggedLiteral(writer, pageVirtualPath, attributeValue.Prefix);
            int length = positionTagged2.Position - attributeValue.Value.Position;
            bool literal1 = attributeValue.Literal;
            this.BeginContext(writer, pageVirtualPath, attributeValue.Value.Position, length, literal1);
            if (attributeValue.Literal)
              WebPageExecutingBase.WriteLiteralTo(writer, (object) content);
            else
              WebPageExecutingBase.WriteTo(writer, (object) content);
            bool literal2 = attributeValue.Literal;
            this.EndContext(writer, pageVirtualPath, attributeValue.Value.Position, length, literal2);
            flag2 = true;
          }
        }
        if (!flag2)
          return;
        this.WritePositionTaggedLiteral(writer, pageVirtualPath, suffix);
      }
    }

    private void WritePositionTaggedLiteral(
      TextWriter writer,
      string pageVirtualPath,
      string value,
      int position)
    {
      bool isLiteral1 = true;
      this.BeginContext(writer, pageVirtualPath, position, value.Length, isLiteral1);
      WebPageExecutingBase.WriteLiteralTo(writer, (object) value);
      bool isLiteral2 = true;
      this.EndContext(writer, pageVirtualPath, position, value.Length, isLiteral2);
    }

    private void WritePositionTaggedLiteral(
      TextWriter writer,
      string pageVirtualPath,
      PositionTagged<string> value)
    {
      this.WritePositionTaggedLiteral(writer, pageVirtualPath, value.Value, value.Position);
    }

    public static void WriteTo(TextWriter writer, HelperResult content) => content?.WriteTo(writer);

    public static void WriteTo(TextWriter writer, object content)
    {
      writer.Write(HttpUtility.HtmlEncode(content));
    }

    public static void WriteLiteralTo(TextWriter writer, object content) => writer.Write(content);

    protected internal virtual TextWriter GetOutputWriter() => TextWriter.Null;
  }
}
