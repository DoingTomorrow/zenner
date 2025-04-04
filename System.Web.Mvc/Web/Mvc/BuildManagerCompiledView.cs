// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.BuildManagerCompiledView
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.IO;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class BuildManagerCompiledView : IView
  {
    internal IViewPageActivator ViewPageActivator;
    private IBuildManager _buildManager;
    private ControllerContext _controllerContext;

    protected BuildManagerCompiledView(ControllerContext controllerContext, string viewPath)
      : this(controllerContext, viewPath, (IViewPageActivator) null)
    {
    }

    protected BuildManagerCompiledView(
      ControllerContext controllerContext,
      string viewPath,
      IViewPageActivator viewPageActivator)
      : this(controllerContext, viewPath, viewPageActivator, (IDependencyResolver) null)
    {
    }

    internal BuildManagerCompiledView(
      ControllerContext controllerContext,
      string viewPath,
      IViewPageActivator viewPageActivator,
      IDependencyResolver dependencyResolver)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(viewPath))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (viewPath));
      this._controllerContext = controllerContext;
      this.ViewPath = viewPath;
      this.ViewPageActivator = viewPageActivator ?? (IViewPageActivator) new BuildManagerViewEngine.DefaultViewPageActivator(dependencyResolver);
    }

    internal IBuildManager BuildManager
    {
      get
      {
        if (this._buildManager == null)
          this._buildManager = (IBuildManager) new BuildManagerWrapper();
        return this._buildManager;
      }
      set => this._buildManager = value;
    }

    public string ViewPath { get; protected set; }

    public void Render(ViewContext viewContext, TextWriter writer)
    {
      if (viewContext == null)
        throw new ArgumentNullException(nameof (viewContext));
      object instance = (object) null;
      Type compiledType = this.BuildManager.GetCompiledType(this.ViewPath);
      if (compiledType != (Type) null)
        instance = this.ViewPageActivator.Create(this._controllerContext, compiledType);
      if (instance == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.CshtmlView_ViewCouldNotBeCreated, new object[1]
        {
          (object) this.ViewPath
        }));
      this.RenderView(viewContext, writer, instance);
    }

    protected abstract void RenderView(ViewContext viewContext, TextWriter writer, object instance);
  }
}
