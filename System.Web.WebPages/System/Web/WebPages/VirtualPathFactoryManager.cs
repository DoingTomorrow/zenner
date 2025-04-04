// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.VirtualPathFactoryManager
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.WebPages
{
  public class VirtualPathFactoryManager : IVirtualPathFactory
  {
    private static readonly Lazy<VirtualPathFactoryManager> _instance = new Lazy<VirtualPathFactoryManager>((Func<VirtualPathFactoryManager>) (() => new VirtualPathFactoryManager((IVirtualPathFactory) new BuildManagerWrapper())));
    private readonly LinkedList<IVirtualPathFactory> _virtualPathFactories = new LinkedList<IVirtualPathFactory>();

    internal VirtualPathFactoryManager(IVirtualPathFactory defaultFactory)
    {
      this._virtualPathFactories.AddFirst(defaultFactory);
    }

    internal static VirtualPathFactoryManager Instance => VirtualPathFactoryManager._instance.Value;

    internal IEnumerable<IVirtualPathFactory> RegisteredFactories
    {
      get => (IEnumerable<IVirtualPathFactory>) this._virtualPathFactories;
    }

    public static void RegisterVirtualPathFactory(IVirtualPathFactory virtualPathFactory)
    {
      VirtualPathFactoryManager.Instance.RegisterVirtualPathFactoryInternal(virtualPathFactory);
    }

    internal void RegisterVirtualPathFactoryInternal(IVirtualPathFactory virtualPathFactory)
    {
      this._virtualPathFactories.AddBefore(this._virtualPathFactories.Last, virtualPathFactory);
    }

    public bool Exists(string virtualPath)
    {
      return this._virtualPathFactories.Any<IVirtualPathFactory>((Func<IVirtualPathFactory, bool>) (factory => factory.Exists(virtualPath)));
    }

    public object CreateInstance(string virtualPath)
    {
      return this.CreateInstanceOfType<object>(virtualPath);
    }

    internal T CreateInstanceOfType<T>(string virtualPath) where T : class
    {
      IVirtualPathFactory factory = this._virtualPathFactories.FirstOrDefault<IVirtualPathFactory>((Func<IVirtualPathFactory, bool>) (f => f.Exists(virtualPath)));
      return factory != null ? factory.CreateInstance<T>(virtualPath) : default (T);
    }
  }
}
