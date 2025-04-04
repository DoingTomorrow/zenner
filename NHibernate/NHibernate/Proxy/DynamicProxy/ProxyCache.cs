// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.ProxyCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public class ProxyCache : IProxyCache
  {
    private static readonly IDictionary<ProxyCacheEntry, Type> cache = (IDictionary<ProxyCacheEntry, Type>) new ThreadSafeDictionary<ProxyCacheEntry, Type>((IDictionary<ProxyCacheEntry, Type>) new Dictionary<ProxyCacheEntry, Type>());

    public bool Contains(Type baseType, params Type[] baseInterfaces)
    {
      if (baseType == null)
        return false;
      ProxyCacheEntry key = new ProxyCacheEntry(baseType, baseInterfaces);
      return ProxyCache.cache.ContainsKey(key);
    }

    public Type GetProxyType(Type baseType, params Type[] baseInterfaces)
    {
      ProxyCacheEntry key = new ProxyCacheEntry(baseType, baseInterfaces);
      return ProxyCache.cache[key];
    }

    public bool TryGetProxyType(Type baseType, Type[] baseInterfaces, out Type proxyType)
    {
      proxyType = (Type) null;
      if (baseType == null)
        return false;
      ProxyCacheEntry key = new ProxyCacheEntry(baseType, baseInterfaces);
      return ProxyCache.cache.TryGetValue(key, out proxyType);
    }

    public void StoreProxyType(Type result, Type baseType, params Type[] baseInterfaces)
    {
      ProxyCacheEntry key = new ProxyCacheEntry(baseType, baseInterfaces);
      ProxyCache.cache[key] = result;
    }
  }
}
