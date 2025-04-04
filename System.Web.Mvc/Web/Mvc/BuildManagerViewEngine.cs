// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.BuildManagerViewEngine
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public abstract class BuildManagerViewEngine : VirtualPathProviderViewEngine
  {
    private IBuildManager _buildManager;
    private IViewPageActivator _viewPageActivator;
    private IResolver<IViewPageActivator> _activatorResolver;

    protected BuildManagerViewEngine()
      : this((IViewPageActivator) null, (IResolver<IViewPageActivator>) null, (IDependencyResolver) null)
    {
    }

    protected BuildManagerViewEngine(IViewPageActivator viewPageActivator)
      : this(viewPageActivator, (IResolver<IViewPageActivator>) null, (IDependencyResolver) null)
    {
    }

    internal BuildManagerViewEngine(
      IViewPageActivator viewPageActivator,
      IResolver<IViewPageActivator> activatorResolver,
      IDependencyResolver dependencyResolver)
    {
      if (viewPageActivator != null)
        this._viewPageActivator = viewPageActivator;
      else
        this._activatorResolver = activatorResolver ?? (IResolver<IViewPageActivator>) new SingleServiceResolver<IViewPageActivator>((Func<IViewPageActivator>) (() => (IViewPageActivator) null), (IViewPageActivator) new BuildManagerViewEngine.DefaultViewPageActivator(dependencyResolver), "BuildManagerViewEngine constructor");
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

    protected IViewPageActivator ViewPageActivator
    {
      get
      {
        if (this._viewPageActivator != null)
          return this._viewPageActivator;
        this._viewPageActivator = this._activatorResolver.Current;
        return this._viewPageActivator;
      }
    }

    protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
    {
      return this.BuildManager.FileExists(virtualPath);
    }

    internal class DefaultViewPageActivator : IViewPageActivator
    {
      private Func<IDependencyResolver> _resolverThunk;

      public DefaultViewPageActivator()
        : this((IDependencyResolver) null)
      {
      }

      public DefaultViewPageActivator(IDependencyResolver resolver)
      {
        if (resolver == null)
          this._resolverThunk = (Func<IDependencyResolver>) (() => DependencyResolver.Current);
        else
          this._resolverThunk = (Func<IDependencyResolver>) (() => resolver);
      }

      public object Create(ControllerContext controllerContext, Type type)
      {
        return this._resolverThunk().GetService(type) ?? Activator.CreateInstance(type);
      }
    }
  }
}
