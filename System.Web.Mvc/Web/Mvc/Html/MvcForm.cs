// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.MvcForm
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc.Html
{
  public class MvcForm : IDisposable
  {
    private readonly ViewContext _viewContext;
    private bool _disposed;

    [Obsolete("This constructor is obsolete, because its functionality has been moved to MvcForm(ViewContext) now.", true)]
    public MvcForm(HttpResponseBase httpResponse)
    {
      throw new InvalidOperationException(MvcResources.MvcForm_ConstructorObsolete);
    }

    public MvcForm(ViewContext viewContext)
    {
      this._viewContext = viewContext != null ? viewContext : throw new ArgumentNullException(nameof (viewContext));
      this._viewContext.FormContext = new FormContext();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      this._disposed = true;
      FormExtensions.EndForm(this._viewContext);
    }

    public void EndForm() => this.Dispose(true);
  }
}
