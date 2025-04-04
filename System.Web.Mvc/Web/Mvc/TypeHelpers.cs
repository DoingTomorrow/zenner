// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TypeHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

#nullable disable
namespace System.Web.Mvc
{
  internal static class TypeHelpers
  {
    private static readonly Dictionary<Type, TryGetValueDelegate> _tryGetValueDelegateCache = new Dictionary<Type, TryGetValueDelegate>();
    private static readonly ReaderWriterLockSlim _tryGetValueDelegateCacheLock = new ReaderWriterLockSlim();
    private static readonly MethodInfo _strongTryGetValueImplInfo = typeof (TypeHelpers).GetMethod("StrongTryGetValueImpl", BindingFlags.Static | BindingFlags.NonPublic);
    public static readonly Assembly MsCorLibAssembly = typeof (string).Assembly;
    public static readonly Assembly MvcAssembly = typeof (Controller).Assembly;
    public static readonly Assembly SystemWebAssembly = typeof (HttpContext).Assembly;

    public static TDelegate CreateDelegate<TDelegate>(
      Assembly assembly,
      string typeName,
      string methodName,
      object thisParameter)
      where TDelegate : class
    {
      Type type = assembly.GetType(typeName, false);
      return type == (Type) null ? default (TDelegate) : TypeHelpers.CreateDelegate<TDelegate>(type, methodName, thisParameter);
    }

    public static TDelegate CreateDelegate<TDelegate>(
      Type targetType,
      string methodName,
      object thisParameter)
      where TDelegate : class
    {
      Type[] types = Array.ConvertAll<ParameterInfo, Type>(typeof (TDelegate).GetMethod("Invoke").GetParameters(), (Converter<ParameterInfo, Type>) (pInfo => pInfo.ParameterType));
      MethodInfo method = targetType.GetMethod(methodName, types);
      return method == (MethodInfo) null ? default (TDelegate) : Delegate.CreateDelegate(typeof (TDelegate), thisParameter, method, false) as TDelegate;
    }

    public static TryGetValueDelegate CreateTryGetValueDelegate(Type targetType)
    {
      TypeHelpers._tryGetValueDelegateCacheLock.EnterReadLock();
      TryGetValueDelegate getValueDelegate;
      try
      {
        if (TypeHelpers._tryGetValueDelegateCache.TryGetValue(targetType, out getValueDelegate))
          return getValueDelegate;
      }
      finally
      {
        TypeHelpers._tryGetValueDelegateCacheLock.ExitReadLock();
      }
      Type genericInterface = TypeHelpers.ExtractGenericInterface(targetType, typeof (IDictionary<,>));
      if (genericInterface != (Type) null)
      {
        Type[] genericArguments = genericInterface.GetGenericArguments();
        Type type1 = genericArguments[0];
        Type type2 = genericArguments[1];
        if (type1.IsAssignableFrom(typeof (string)))
          getValueDelegate = (TryGetValueDelegate) Delegate.CreateDelegate(typeof (TryGetValueDelegate), TypeHelpers._strongTryGetValueImplInfo.MakeGenericMethod(type1, type2));
      }
      if (getValueDelegate == null && typeof (IDictionary).IsAssignableFrom(targetType))
        getValueDelegate = new TryGetValueDelegate(TypeHelpers.TryGetValueFromNonGenericDictionary);
      TypeHelpers._tryGetValueDelegateCacheLock.EnterWriteLock();
      try
      {
        TypeHelpers._tryGetValueDelegateCache[targetType] = getValueDelegate;
      }
      finally
      {
        TypeHelpers._tryGetValueDelegateCacheLock.ExitWriteLock();
      }
      return getValueDelegate;
    }

    public static Type ExtractGenericInterface(Type queryType, Type interfaceType)
    {
      Func<Type, bool> predicate = (Func<Type, bool>) (t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType);
      return !predicate(queryType) ? ((IEnumerable<Type>) queryType.GetInterfaces()).FirstOrDefault<Type>(predicate) : queryType;
    }

    public static object GetDefaultValue(Type type)
    {
      return !TypeHelpers.TypeAllowsNullValue(type) ? Activator.CreateInstance(type) : (object) null;
    }

    public static bool IsCompatibleObject<T>(object value)
    {
      if (value is T)
        return true;
      return value == null && TypeHelpers.TypeAllowsNullValue(typeof (T));
    }

    public static bool IsNullableValueType(Type type)
    {
      return Nullable.GetUnderlyingType(type) != (Type) null;
    }

    private static bool StrongTryGetValueImpl<TKey, TValue>(
      object dictionary,
      string key,
      out object value)
    {
      TValue obj;
      bool valueImpl = ((IDictionary<TKey, TValue>) dictionary).TryGetValue((TKey) key, out obj);
      value = (object) obj;
      return valueImpl;
    }

    private static bool TryGetValueFromNonGenericDictionary(
      object dictionary,
      string key,
      out object value)
    {
      IDictionary dictionary1 = (IDictionary) dictionary;
      bool genericDictionary = dictionary1.Contains((object) key);
      value = genericDictionary ? dictionary1[(object) key] : (object) null;
      return genericDictionary;
    }

    public static bool TypeAllowsNullValue(Type type)
    {
      return !type.IsValueType || TypeHelpers.IsNullableValueType(type);
    }
  }
}
