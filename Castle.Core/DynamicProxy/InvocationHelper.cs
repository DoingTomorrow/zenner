// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.InvocationHelper
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy
{
  internal static class InvocationHelper
  {
    private static readonly Dictionary<KeyValuePair<MethodInfo, Type>, MethodInfo> cache = new Dictionary<KeyValuePair<MethodInfo, Type>, MethodInfo>();
    private static readonly Lock @lock = Lock.Create();

    public static MethodInfo GetMethodOnObject(object target, MethodInfo proxiedMethod)
    {
      return target == null ? (MethodInfo) null : InvocationHelper.GetMethodOnType(target.GetType(), proxiedMethod);
    }

    public static MethodInfo GetMethodOnType(Type type, MethodInfo proxiedMethod)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      using (IUpgradeableLockHolder upgradeableLockHolder = InvocationHelper.@lock.ForReadingUpgradeable())
      {
        MethodInfo fromCache1 = InvocationHelper.GetFromCache(proxiedMethod, type);
        if (fromCache1 != null)
          return fromCache1;
        upgradeableLockHolder.Upgrade();
        MethodInfo fromCache2 = InvocationHelper.GetFromCache(proxiedMethod, type);
        if (fromCache2 != null)
          return fromCache2;
        MethodInfo method = InvocationHelper.ObtainMethod(proxiedMethod, type);
        InvocationHelper.PutToCache(proxiedMethod, type, method);
        return method;
      }
    }

    private static MethodInfo ObtainMethod(MethodInfo proxiedMethod, Type type)
    {
      Type[] typeArray = (Type[]) null;
      if (proxiedMethod.IsGenericMethod)
      {
        typeArray = proxiedMethod.GetGenericArguments();
        proxiedMethod = proxiedMethod.GetGenericMethodDefinition();
      }
      Type declaringType = proxiedMethod.DeclaringType;
      MethodInfo methodInfo = (MethodInfo) null;
      if (declaringType.IsInterface)
      {
        InterfaceMapping interfaceMap = type.GetInterfaceMap(declaringType);
        int index = Array.IndexOf<MethodInfo>(interfaceMap.InterfaceMethods, proxiedMethod);
        methodInfo = interfaceMap.TargetMethods[index];
      }
      else
      {
        foreach (MethodInfo allInstanceMethod in MethodFinder.GetAllInstanceMethods(type, BindingFlags.Public | BindingFlags.NonPublic))
        {
          if (MethodSignatureComparer.Instance.Equals(allInstanceMethod.GetBaseDefinition(), proxiedMethod))
          {
            methodInfo = allInstanceMethod;
            break;
          }
        }
      }
      if (methodInfo == null)
        throw new ArgumentException(string.Format("Could not find method overriding {0} on type {1}. This is most likely a bug. Please report it.", (object) proxiedMethod, (object) type));
      return typeArray == null ? methodInfo : methodInfo.MakeGenericMethod(typeArray);
    }

    private static void PutToCache(MethodInfo methodInfo, Type type, MethodInfo value)
    {
      KeyValuePair<MethodInfo, Type> key = new KeyValuePair<MethodInfo, Type>(methodInfo, type);
      InvocationHelper.cache.Add(key, value);
    }

    private static MethodInfo GetFromCache(MethodInfo methodInfo, Type type)
    {
      KeyValuePair<MethodInfo, Type> key = new KeyValuePair<MethodInfo, Type>(methodInfo, type);
      MethodInfo fromCache;
      InvocationHelper.cache.TryGetValue(key, out fromCache);
      return fromCache;
    }
  }
}
