// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DefaultControllerFactory
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.SessionState;

#nullable disable
namespace System.Web.Mvc
{
  public class DefaultControllerFactory : IControllerFactory
  {
    private static readonly ConcurrentDictionary<Type, SessionStateBehavior> _sessionStateCache = new ConcurrentDictionary<Type, SessionStateBehavior>();
    private static ControllerTypeCache _staticControllerTypeCache = new ControllerTypeCache();
    private IBuildManager _buildManager;
    private IResolver<IControllerActivator> _activatorResolver;
    private IControllerActivator _controllerActivator;
    private ControllerBuilder _controllerBuilder;
    private ControllerTypeCache _instanceControllerTypeCache;

    public DefaultControllerFactory()
      : this((IControllerActivator) null, (IResolver<IControllerActivator>) null, (IDependencyResolver) null)
    {
    }

    public DefaultControllerFactory(IControllerActivator controllerActivator)
      : this(controllerActivator, (IResolver<IControllerActivator>) null, (IDependencyResolver) null)
    {
    }

    internal DefaultControllerFactory(
      IControllerActivator controllerActivator,
      IResolver<IControllerActivator> activatorResolver,
      IDependencyResolver dependencyResolver)
    {
      if (controllerActivator != null)
        this._controllerActivator = controllerActivator;
      else
        this._activatorResolver = activatorResolver ?? (IResolver<IControllerActivator>) new SingleServiceResolver<IControllerActivator>((Func<IControllerActivator>) (() => (IControllerActivator) null), (IControllerActivator) new DefaultControllerFactory.DefaultControllerActivator(dependencyResolver), "DefaultControllerFactory constructor");
    }

    private IControllerActivator ControllerActivator
    {
      get
      {
        if (this._controllerActivator != null)
          return this._controllerActivator;
        this._controllerActivator = this._activatorResolver.Current;
        return this._controllerActivator;
      }
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

    internal ControllerBuilder ControllerBuilder
    {
      get => this._controllerBuilder ?? ControllerBuilder.Current;
      set => this._controllerBuilder = value;
    }

    internal ControllerTypeCache ControllerTypeCache
    {
      get
      {
        return this._instanceControllerTypeCache ?? DefaultControllerFactory._staticControllerTypeCache;
      }
      set => this._instanceControllerTypeCache = value;
    }

    internal static InvalidOperationException CreateAmbiguousControllerException(
      RouteBase route,
      string controllerName,
      ICollection<Type> matchingTypes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Type matchingType in (IEnumerable<Type>) matchingTypes)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append(matchingType.FullName);
      }
      string message;
      if (route is Route route1)
        message = string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DefaultControllerFactory_ControllerNameAmbiguous_WithRouteUrl, new object[3]
        {
          (object) controllerName,
          (object) route1.Url,
          (object) stringBuilder
        });
      else
        message = string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DefaultControllerFactory_ControllerNameAmbiguous_WithoutRouteUrl, new object[2]
        {
          (object) controllerName,
          (object) stringBuilder
        });
      return new InvalidOperationException(message);
    }

    public virtual IController CreateController(
      RequestContext requestContext,
      string controllerName)
    {
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      Type controllerType = !string.IsNullOrEmpty(controllerName) ? this.GetControllerType(requestContext, controllerName) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (controllerName));
      return this.GetControllerInstance(requestContext, controllerType);
    }

    protected internal virtual IController GetControllerInstance(
      RequestContext requestContext,
      Type controllerType)
    {
      if (controllerType == (Type) null)
        throw new HttpException(404, string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DefaultControllerFactory_NoControllerFound, new object[1]
        {
          (object) requestContext.HttpContext.Request.Path
        }));
      if (!typeof (IController).IsAssignableFrom(controllerType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DefaultControllerFactory_TypeDoesNotSubclassControllerBase, new object[1]
        {
          (object) controllerType
        }), nameof (controllerType));
      return this.ControllerActivator.Create(requestContext, controllerType);
    }

    protected internal virtual SessionStateBehavior GetControllerSessionBehavior(
      RequestContext requestContext,
      Type controllerType)
    {
      return controllerType == (Type) null ? SessionStateBehavior.Default : DefaultControllerFactory._sessionStateCache.GetOrAdd(controllerType, (Func<Type, SessionStateBehavior>) (type =>
      {
        SessionStateAttribute sessionStateAttribute = type.GetCustomAttributes(typeof (SessionStateAttribute), true).OfType<SessionStateAttribute>().FirstOrDefault<SessionStateAttribute>();
        return sessionStateAttribute == null ? SessionStateBehavior.Default : sessionStateAttribute.Behavior;
      }));
    }

    protected internal virtual Type GetControllerType(
      RequestContext requestContext,
      string controllerName)
    {
      if (string.IsNullOrEmpty(controllerName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (controllerName));
      object obj;
      if (requestContext != null && requestContext.RouteData.DataTokens.TryGetValue("Namespaces", out obj) && obj is IEnumerable<string> strings && strings.Any<string>())
      {
        HashSet<string> namespaces = new HashSet<string>(strings, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        Type withinNamespaces = this.GetControllerTypeWithinNamespaces(requestContext.RouteData.Route, controllerName, namespaces);
        if (withinNamespaces != (Type) null || false.Equals(requestContext.RouteData.DataTokens["UseNamespaceFallback"]))
          return withinNamespaces;
      }
      if (this.ControllerBuilder.DefaultNamespaces.Count > 0)
      {
        HashSet<string> namespaces = new HashSet<string>((IEnumerable<string>) this.ControllerBuilder.DefaultNamespaces, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        Type withinNamespaces = this.GetControllerTypeWithinNamespaces(requestContext.RouteData.Route, controllerName, namespaces);
        if (withinNamespaces != (Type) null)
          return withinNamespaces;
      }
      return this.GetControllerTypeWithinNamespaces(requestContext.RouteData.Route, controllerName, (HashSet<string>) null);
    }

    private Type GetControllerTypeWithinNamespaces(
      RouteBase route,
      string controllerName,
      HashSet<string> namespaces)
    {
      this.ControllerTypeCache.EnsureInitialized(this.BuildManager);
      ICollection<Type> controllerTypes = this.ControllerTypeCache.GetControllerTypes(controllerName, namespaces);
      switch (controllerTypes.Count)
      {
        case 0:
          return (Type) null;
        case 1:
          return controllerTypes.First<Type>();
        default:
          throw DefaultControllerFactory.CreateAmbiguousControllerException(route, controllerName, controllerTypes);
      }
    }

    public virtual void ReleaseController(IController controller)
    {
      if (!(controller is IDisposable disposable))
        return;
      disposable.Dispose();
    }

    SessionStateBehavior IControllerFactory.GetControllerSessionBehavior(
      RequestContext requestContext,
      string controllerName)
    {
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      Type controllerType = !string.IsNullOrEmpty(controllerName) ? this.GetControllerType(requestContext, controllerName) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (controllerName));
      return this.GetControllerSessionBehavior(requestContext, controllerType);
    }

    private class DefaultControllerActivator : IControllerActivator
    {
      private Func<IDependencyResolver> _resolverThunk;

      public DefaultControllerActivator()
        : this((IDependencyResolver) null)
      {
      }

      public DefaultControllerActivator(IDependencyResolver resolver)
      {
        if (resolver == null)
          this._resolverThunk = (Func<IDependencyResolver>) (() => DependencyResolver.Current);
        else
          this._resolverThunk = (Func<IDependencyResolver>) (() => resolver);
      }

      public IController Create(RequestContext requestContext, Type controllerType)
      {
        try
        {
          return (IController) (this._resolverThunk().GetService(controllerType) ?? Activator.CreateInstance(controllerType));
        }
        catch (Exception ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DefaultControllerFactory_ErrorCreatingController, new object[1]
          {
            (object) controllerType
          }), ex);
        }
      }
    }
  }
}
