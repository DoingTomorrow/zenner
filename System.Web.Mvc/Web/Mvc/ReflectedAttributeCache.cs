// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ReflectedAttributeCache
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  internal static class ReflectedAttributeCache
  {
    private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionMethodSelectorAttribute>> _actionMethodSelectorAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionMethodSelectorAttribute>>();
    private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionNameSelectorAttribute>> _actionNameSelectorAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionNameSelectorAttribute>>();
    private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<FilterAttribute>> _methodFilterAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<FilterAttribute>>();
    private static readonly ConcurrentDictionary<Type, ReadOnlyCollection<FilterAttribute>> _typeFilterAttributeCache = new ConcurrentDictionary<Type, ReadOnlyCollection<FilterAttribute>>();

    public static ICollection<FilterAttribute> GetTypeFilterAttributes(Type type)
    {
      return (ICollection<FilterAttribute>) ReflectedAttributeCache.GetAttributes<Type, FilterAttribute>(ReflectedAttributeCache._typeFilterAttributeCache, type);
    }

    public static ICollection<FilterAttribute> GetMethodFilterAttributes(MethodInfo methodInfo)
    {
      return (ICollection<FilterAttribute>) ReflectedAttributeCache.GetAttributes<MethodInfo, FilterAttribute>(ReflectedAttributeCache._methodFilterAttributeCache, methodInfo);
    }

    public static ICollection<ActionMethodSelectorAttribute> GetActionMethodSelectorAttributes(
      MethodInfo methodInfo)
    {
      return (ICollection<ActionMethodSelectorAttribute>) ReflectedAttributeCache.GetAttributes<MethodInfo, ActionMethodSelectorAttribute>(ReflectedAttributeCache._actionMethodSelectorAttributeCache, methodInfo);
    }

    public static ICollection<ActionNameSelectorAttribute> GetActionNameSelectorAttributes(
      MethodInfo methodInfo)
    {
      return (ICollection<ActionNameSelectorAttribute>) ReflectedAttributeCache.GetAttributes<MethodInfo, ActionNameSelectorAttribute>(ReflectedAttributeCache._actionNameSelectorAttributeCache, methodInfo);
    }

    private static ReadOnlyCollection<TAttribute> GetAttributes<TMemberInfo, TAttribute>(
      ConcurrentDictionary<TMemberInfo, ReadOnlyCollection<TAttribute>> lookup,
      TMemberInfo memberInfo)
      where TMemberInfo : MemberInfo
      where TAttribute : Attribute
    {
      return lookup.GetOrAdd(memberInfo, (Func<TMemberInfo, ReadOnlyCollection<TAttribute>>) (mi => new ReadOnlyCollection<TAttribute>((IList<TAttribute>) memberInfo.GetCustomAttributes(typeof (TAttribute), true))));
    }
  }
}
