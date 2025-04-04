// Decompiled with JetBrains decompiler
// Type: RestSharp.Reflection.CacheResolver
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Reflection;

#nullable disable
namespace RestSharp.Reflection
{
  internal class CacheResolver
  {
    private readonly MemberMapLoader _memberMapLoader;
    private readonly SafeDictionary<Type, SafeDictionary<string, CacheResolver.MemberMap>> _memberMapsCache = new SafeDictionary<Type, SafeDictionary<string, CacheResolver.MemberMap>>();
    private static readonly SafeDictionary<Type, CacheResolver.CtorDelegate> ConstructorCache = new SafeDictionary<Type, CacheResolver.CtorDelegate>();

    public CacheResolver(MemberMapLoader memberMapLoader)
    {
      this._memberMapLoader = memberMapLoader;
    }

    public static object GetNewInstance(Type type)
    {
      CacheResolver.CtorDelegate ctorDelegate1;
      if (CacheResolver.ConstructorCache.TryGetValue(type, out ctorDelegate1))
        return ctorDelegate1();
      ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
      CacheResolver.CtorDelegate ctorDelegate2 = (CacheResolver.CtorDelegate) (() => constructorInfo.Invoke((object[]) null));
      CacheResolver.ConstructorCache.Add(type, ctorDelegate2);
      return ctorDelegate2();
    }

    public SafeDictionary<string, CacheResolver.MemberMap> LoadMaps(Type type)
    {
      if (type == (Type) null || type == typeof (object))
        return (SafeDictionary<string, CacheResolver.MemberMap>) null;
      SafeDictionary<string, CacheResolver.MemberMap> memberMaps;
      if (this._memberMapsCache.TryGetValue(type, out memberMaps))
        return memberMaps;
      memberMaps = new SafeDictionary<string, CacheResolver.MemberMap>();
      this._memberMapLoader(type, memberMaps);
      this._memberMapsCache.Add(type, memberMaps);
      return memberMaps;
    }

    private static GetHandler CreateGetHandler(FieldInfo fieldInfo)
    {
      return (GetHandler) (instance => fieldInfo.GetValue(instance));
    }

    private static SetHandler CreateSetHandler(FieldInfo fieldInfo)
    {
      return fieldInfo.IsInitOnly || fieldInfo.IsLiteral ? (SetHandler) null : (SetHandler) ((instance, value) => fieldInfo.SetValue(instance, value));
    }

    private static GetHandler CreateGetHandler(PropertyInfo propertyInfo)
    {
      MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
      return getMethodInfo == (MethodInfo) null ? (GetHandler) null : (GetHandler) (instance => getMethodInfo.Invoke(instance, (object[]) Type.EmptyTypes));
    }

    private static SetHandler CreateSetHandler(PropertyInfo propertyInfo)
    {
      MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
      return setMethodInfo == (MethodInfo) null ? (SetHandler) null : (SetHandler) ((instance, value) => setMethodInfo.Invoke(instance, new object[1]
      {
        value
      }));
    }

    private delegate object CtorDelegate();

    internal sealed class MemberMap
    {
      public readonly MemberInfo MemberInfo;
      public readonly Type Type;
      public readonly GetHandler Getter;
      public readonly SetHandler Setter;

      public MemberMap(PropertyInfo propertyInfo)
      {
        this.MemberInfo = (MemberInfo) propertyInfo;
        this.Type = propertyInfo.PropertyType;
        this.Getter = CacheResolver.CreateGetHandler(propertyInfo);
        this.Setter = CacheResolver.CreateSetHandler(propertyInfo);
      }

      public MemberMap(FieldInfo fieldInfo)
      {
        this.MemberInfo = (MemberInfo) fieldInfo;
        this.Type = fieldInfo.FieldType;
        this.Getter = CacheResolver.CreateGetHandler(fieldInfo);
        this.Setter = CacheResolver.CreateSetHandler(fieldInfo);
      }
    }
  }
}
