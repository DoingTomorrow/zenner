﻿// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MethodFinder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class MethodFinder
  {
    private static readonly Dictionary<Type, object> cachedMethodInfosByType = new Dictionary<Type, object>();
    private static readonly object lockObject = new object();

    public static MethodInfo[] GetAllInstanceMethods(Type type, BindingFlags flags)
    {
      if ((flags & ~(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) != BindingFlags.Default)
        throw new ArgumentException("MethodFinder only supports the Public, NonPublic, and Instance binding flags.", nameof (flags));
      MethodInfo[] methodsInCache;
      lock (MethodFinder.lockObject)
      {
        if (!MethodFinder.cachedMethodInfosByType.ContainsKey(type))
          MethodFinder.cachedMethodInfosByType.Add(type, MethodFinder.RemoveDuplicates(type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));
        methodsInCache = (MethodInfo[]) MethodFinder.cachedMethodInfosByType[type];
      }
      return MethodFinder.MakeFilteredCopy(methodsInCache, flags & (BindingFlags.Public | BindingFlags.NonPublic));
    }

    private static object RemoveDuplicates(MethodInfo[] infos)
    {
      Dictionary<MethodInfo, object> dictionary = new Dictionary<MethodInfo, object>((IEqualityComparer<MethodInfo>) MethodSignatureComparer.Instance);
      foreach (MethodInfo info in infos)
      {
        if (!dictionary.ContainsKey(info))
          dictionary.Add(info, (object) null);
      }
      MethodInfo[] array = new MethodInfo[dictionary.Count];
      dictionary.Keys.CopyTo(array, 0);
      return (object) array;
    }

    private static MethodInfo[] MakeFilteredCopy(
      MethodInfo[] methodsInCache,
      BindingFlags visibilityFlags)
    {
      if ((visibilityFlags & ~(BindingFlags.Public | BindingFlags.NonPublic)) != BindingFlags.Default)
        throw new ArgumentException("Only supports BindingFlags.Public and NonPublic.", nameof (visibilityFlags));
      bool flag1 = (visibilityFlags & BindingFlags.Public) == BindingFlags.Public;
      bool flag2 = (visibilityFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic;
      List<MethodInfo> methodInfoList = new List<MethodInfo>(methodsInCache.Length);
      foreach (MethodInfo methodInfo in methodsInCache)
      {
        if (methodInfo.IsPublic && flag1 || !methodInfo.IsPublic && flag2)
          methodInfoList.Add(methodInfo);
      }
      return methodInfoList.ToArray();
    }
  }
}
