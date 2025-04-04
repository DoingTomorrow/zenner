// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DependencyResolver
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class DependencyResolver
  {
    private static DependencyResolver _instance = new DependencyResolver();
    private IDependencyResolver _current;
    private DependencyResolver.CacheDependencyResolver _currentCache;

    public DependencyResolver()
    {
      this.InnerSetResolver((IDependencyResolver) new DependencyResolver.DefaultDependencyResolver());
    }

    public static IDependencyResolver Current => DependencyResolver._instance.InnerCurrent;

    internal static IDependencyResolver CurrentCache
    {
      get => DependencyResolver._instance.InnerCurrentCache;
    }

    public IDependencyResolver InnerCurrent => this._current;

    internal IDependencyResolver InnerCurrentCache => (IDependencyResolver) this._currentCache;

    public static void SetResolver(IDependencyResolver resolver)
    {
      DependencyResolver._instance.InnerSetResolver(resolver);
    }

    public static void SetResolver(object commonServiceLocator)
    {
      DependencyResolver._instance.InnerSetResolver(commonServiceLocator);
    }

    public static void SetResolver(
      Func<Type, object> getService,
      Func<Type, IEnumerable<object>> getServices)
    {
      DependencyResolver._instance.InnerSetResolver(getService, getServices);
    }

    public void InnerSetResolver(IDependencyResolver resolver)
    {
      this._current = resolver != null ? resolver : throw new ArgumentNullException(nameof (resolver));
      this._currentCache = new DependencyResolver.CacheDependencyResolver(this._current);
    }

    public void InnerSetResolver(object commonServiceLocator)
    {
      Type type = commonServiceLocator != null ? commonServiceLocator.GetType() : throw new ArgumentNullException(nameof (commonServiceLocator));
      MethodInfo method1 = type.GetMethod("GetInstance", new Type[1]
      {
        typeof (Type)
      });
      MethodInfo method2 = type.GetMethod("GetAllInstances", new Type[1]
      {
        typeof (Type)
      });
      if (method1 == (MethodInfo) null || method1.ReturnType != typeof (object) || method2 == (MethodInfo) null || method2.ReturnType != typeof (IEnumerable<object>))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DependencyResolver_DoesNotImplementICommonServiceLocator, new object[1]
        {
          (object) type.FullName
        }), nameof (commonServiceLocator));
      this.InnerSetResolver((IDependencyResolver) new DependencyResolver.DelegateBasedDependencyResolver((Func<Type, object>) Delegate.CreateDelegate(typeof (Func<Type, object>), commonServiceLocator, method1), (Func<Type, IEnumerable<object>>) Delegate.CreateDelegate(typeof (Func<Type, IEnumerable<object>>), commonServiceLocator, method2)));
    }

    public void InnerSetResolver(
      Func<Type, object> getService,
      Func<Type, IEnumerable<object>> getServices)
    {
      if (getService == null)
        throw new ArgumentNullException(nameof (getService));
      if (getServices == null)
        throw new ArgumentNullException(nameof (getServices));
      this.InnerSetResolver((IDependencyResolver) new DependencyResolver.DelegateBasedDependencyResolver(getService, getServices));
    }

    private sealed class CacheDependencyResolver : IDependencyResolver
    {
      private readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();
      private readonly ConcurrentDictionary<Type, IEnumerable<object>> _cacheMultiple = new ConcurrentDictionary<Type, IEnumerable<object>>();
      private readonly IDependencyResolver _resolver;

      public CacheDependencyResolver(IDependencyResolver resolver) => this._resolver = resolver;

      public object GetService(Type serviceType)
      {
        return this._cache.GetOrAdd(serviceType, new Func<Type, object>(this._resolver.GetService));
      }

      public IEnumerable<object> GetServices(Type serviceType)
      {
        return this._cacheMultiple.GetOrAdd(serviceType, new Func<Type, IEnumerable<object>>(this._resolver.GetServices));
      }
    }

    private class DefaultDependencyResolver : IDependencyResolver
    {
      public object GetService(Type serviceType)
      {
        if (!serviceType.IsInterface)
        {
          if (!serviceType.IsAbstract)
          {
            try
            {
              return Activator.CreateInstance(serviceType);
            }
            catch
            {
              return (object) null;
            }
          }
        }
        return (object) null;
      }

      public IEnumerable<object> GetServices(Type serviceType) => Enumerable.Empty<object>();
    }

    private class DelegateBasedDependencyResolver : IDependencyResolver
    {
      private Func<Type, object> _getService;
      private Func<Type, IEnumerable<object>> _getServices;

      public DelegateBasedDependencyResolver(
        Func<Type, object> getService,
        Func<Type, IEnumerable<object>> getServices)
      {
        this._getService = getService;
        this._getServices = getServices;
      }

      public object GetService(Type type)
      {
        try
        {
          return this._getService(type);
        }
        catch
        {
          return (object) null;
        }
      }

      public IEnumerable<object> GetServices(Type type) => this._getServices(type);
    }
  }
}
