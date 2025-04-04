// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerTypeCache
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class ControllerTypeCache
  {
    private const string TypeCacheName = "MVC-ControllerTypeCache.xml";
    private volatile Dictionary<string, ILookup<string, Type>> _cache;
    private object _lockObj = new object();

    internal int Count
    {
      get
      {
        int count = 0;
        foreach (IEnumerable<IGrouping<string, Type>> groupings in this._cache.Values)
        {
          foreach (IGrouping<string, Type> source in groupings)
            count += source.Count<Type>();
        }
        return count;
      }
    }

    public void EnsureInitialized(IBuildManager buildManager)
    {
      if (this._cache != null)
        return;
      lock (this._lockObj)
      {
        if (this._cache != null)
          return;
        this._cache = TypeCacheUtil.GetFilteredTypesFromAssemblies("MVC-ControllerTypeCache.xml", new Predicate<Type>(ControllerTypeCache.IsControllerType), buildManager).GroupBy<Type, string>((Func<Type, string>) (t => t.Name.Substring(0, t.Name.Length - "Controller".Length)), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase).ToDictionary<IGrouping<string, Type>, string, ILookup<string, Type>>((Func<IGrouping<string, Type>, string>) (g => g.Key), (Func<IGrouping<string, Type>, ILookup<string, Type>>) (g => g.ToLookup<Type, string>((Func<Type, string>) (t => t.Namespace ?? string.Empty), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      }
    }

    public ICollection<Type> GetControllerTypes(string controllerName, HashSet<string> namespaces)
    {
      HashSet<Type> controllerTypes = new HashSet<Type>();
      ILookup<string, Type> lookup;
      if (this._cache.TryGetValue(controllerName, out lookup))
      {
        if (namespaces != null)
        {
          foreach (string requestedNamespace in namespaces)
          {
            foreach (IGrouping<string, Type> other in (IEnumerable<IGrouping<string, Type>>) lookup)
            {
              if (ControllerTypeCache.IsNamespaceMatch(requestedNamespace, other.Key))
                controllerTypes.UnionWith((IEnumerable<Type>) other);
            }
          }
        }
        else
        {
          foreach (IGrouping<string, Type> other in (IEnumerable<IGrouping<string, Type>>) lookup)
            controllerTypes.UnionWith((IEnumerable<Type>) other);
        }
      }
      return (ICollection<Type>) controllerTypes;
    }

    internal static bool IsControllerType(Type t)
    {
      return t != (Type) null && t.IsPublic && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && !t.IsAbstract && typeof (IController).IsAssignableFrom(t);
    }

    internal static bool IsNamespaceMatch(string requestedNamespace, string targetNamespace)
    {
      switch (requestedNamespace)
      {
        case null:
          return false;
        case "":
          return true;
        default:
          if (!requestedNamespace.EndsWith(".*", StringComparison.OrdinalIgnoreCase))
            return string.Equals(requestedNamespace, targetNamespace, StringComparison.OrdinalIgnoreCase);
          requestedNamespace = requestedNamespace.Substring(0, requestedNamespace.Length - ".*".Length);
          return targetNamespace.StartsWith(requestedNamespace, StringComparison.OrdinalIgnoreCase) && (requestedNamespace.Length == targetNamespace.Length || targetNamespace[requestedNamespace.Length] == '.');
      }
    }
  }
}
