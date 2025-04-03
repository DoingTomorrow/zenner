// Decompiled with JetBrains decompiler
// Type: AutoMapper.TypeInfo
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class TypeInfo
  {
    private readonly MemberInfo[] _publicGetters;
    private readonly MemberInfo[] _publicAccessors;
    private readonly MethodInfo[] _publicGetMethods;
    private readonly ConstructorInfo[] _constructors;
    private readonly MethodInfo[] _extensionMethods;

    public Type Type { get; private set; }

    public TypeInfo(Type type)
      : this(type, (IEnumerable<MethodInfo>) new MethodInfo[0])
    {
    }

    public TypeInfo(
      Type type,
      IEnumerable<MethodInfo> sourceExtensionMethodSearch)
    {
      this.Type = type;
      IEnumerable<MemberInfo> publicReadableMembers = this.GetAllPublicReadableMembers();
      IEnumerable<MemberInfo> publicWritableMembers = this.GetAllPublicWritableMembers();
      this._publicGetters = this.BuildPublicReadAccessors(publicReadableMembers);
      this._publicAccessors = this.BuildPublicAccessors(publicWritableMembers);
      this._publicGetMethods = this.BuildPublicNoArgMethods();
      this._constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      this._extensionMethods = this.BuildPublicNoArgExtensionMethods(sourceExtensionMethodSearch);
    }

    public IEnumerable<ConstructorInfo> GetConstructors()
    {
      return (IEnumerable<ConstructorInfo>) this._constructors;
    }

    public IEnumerable<MemberInfo> GetPublicReadAccessors()
    {
      return (IEnumerable<MemberInfo>) this._publicGetters;
    }

    public IEnumerable<MemberInfo> GetPublicWriteAccessors()
    {
      return (IEnumerable<MemberInfo>) this._publicAccessors;
    }

    public IEnumerable<MethodInfo> GetPublicNoArgMethods()
    {
      return (IEnumerable<MethodInfo>) this._publicGetMethods;
    }

    public IEnumerable<MethodInfo> GetPublicNoArgExtensionMethods()
    {
      return (IEnumerable<MethodInfo>) this._extensionMethods;
    }

    private MethodInfo[] BuildPublicNoArgExtensionMethods(
      IEnumerable<MethodInfo> sourceExtensionMethodSearch)
    {
      MethodInfo[] array = sourceExtensionMethodSearch.ToArray<MethodInfo>();
      List<MethodInfo> list1 = ((IEnumerable<MethodInfo>) array).Where<MethodInfo>((Func<MethodInfo, bool>) (method => (object) method.GetParameters()[0].ParameterType == (object) this.Type)).ToList<MethodInfo>();
      List<Type> list2 = ((IEnumerable<Type>) this.Type.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).ToList<Type>();
      if (this.Type.IsInterface && this.Type.IsGenericType)
        list2.Add(this.Type);
      foreach (MethodInfo methodInfo in ((IEnumerable<MethodInfo>) array).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.IsGenericMethodDefinition)))
      {
        MethodInfo method = methodInfo;
        Type parameterType = method.GetParameters()[0].ParameterType;
        Type type = list2.Where<Type>((Func<Type, bool>) (t => t.GetGenericTypeDefinition().GetGenericArguments().Length == parameterType.GetGenericArguments().Length)).FirstOrDefault<Type>((Func<Type, bool>) (t => method.MakeGenericMethod(t.GetGenericArguments()).GetParameters()[0].ParameterType.IsAssignableFrom(t)));
        if ((object) type != null)
          list1.Add(method.MakeGenericMethod(type.GetGenericArguments()));
      }
      return list1.ToArray();
    }

    private MemberInfo[] BuildPublicReadAccessors(IEnumerable<MemberInfo> allMembers)
    {
      return allMembers.OfType<PropertyInfo>().GroupBy<PropertyInfo, string>((Func<PropertyInfo, string>) (x => x.Name)).Select<IGrouping<string, PropertyInfo>, PropertyInfo>((Func<IGrouping<string, PropertyInfo>, PropertyInfo>) (x => x.First<PropertyInfo>())).OfType<MemberInfo>().Concat<MemberInfo>(allMembers.Where<MemberInfo>((Func<MemberInfo, bool>) (x => x is FieldInfo))).ToArray<MemberInfo>();
    }

    private MemberInfo[] BuildPublicAccessors(IEnumerable<MemberInfo> allMembers)
    {
      return allMembers.OfType<PropertyInfo>().GroupBy<PropertyInfo, string>((Func<PropertyInfo, string>) (x => x.Name)).Select<IGrouping<string, PropertyInfo>, PropertyInfo>((Func<IGrouping<string, PropertyInfo>, PropertyInfo>) (x => !x.Any<PropertyInfo>((Func<PropertyInfo, bool>) (y => y.CanWrite && y.CanRead)) ? x.First<PropertyInfo>() : x.First<PropertyInfo>((Func<PropertyInfo, bool>) (y => y.CanWrite && y.CanRead)))).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.CanWrite || pi.PropertyType.IsListOrDictionaryType())).OfType<MemberInfo>().Concat<MemberInfo>(allMembers.Where<MemberInfo>((Func<MemberInfo, bool>) (x => x is FieldInfo))).ToArray<MemberInfo>();
    }

    private IEnumerable<MemberInfo> GetAllPublicReadableMembers()
    {
      return this.GetAllPublicMembers(new Func<PropertyInfo, bool>(this.PropertyReadable), BindingFlags.Instance | BindingFlags.Public);
    }

    private IEnumerable<MemberInfo> GetAllPublicWritableMembers()
    {
      return this.GetAllPublicMembers(new Func<PropertyInfo, bool>(this.PropertyWritable), BindingFlags.Instance | BindingFlags.Public);
    }

    private bool PropertyReadable(PropertyInfo propertyInfo) => propertyInfo.CanRead;

    private bool PropertyWritable(PropertyInfo propertyInfo)
    {
      bool flag = (object) typeof (string) != (object) propertyInfo.PropertyType && typeof (IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
      return propertyInfo.CanWrite || flag;
    }

    private IEnumerable<MemberInfo> GetAllPublicMembers(
      Func<PropertyInfo, bool> propertyAvailableFor,
      BindingFlags bindingAttr)
    {
      List<Type> source = new List<Type>();
      for (Type type = this.Type; (object) type != null; type = type.BaseType)
        source.Add(type);
      if (this.Type.IsInterface)
        source.AddRange((IEnumerable<Type>) this.Type.GetInterfaces());
      return source.Where<Type>((Func<Type, bool>) (x => (object) x != null)).SelectMany<Type, MemberInfo>((Func<Type, IEnumerable<MemberInfo>>) (x => ((IEnumerable<MemberInfo>) x.GetMembers(bindingAttr | BindingFlags.DeclaredOnly)).Where<MemberInfo>((Func<MemberInfo, bool>) (m =>
      {
        if ((object) (m as FieldInfo) != null)
          return true;
        return (object) (m as PropertyInfo) != null && propertyAvailableFor((PropertyInfo) m) && !((IEnumerable<ParameterInfo>) ((PropertyInfo) m).GetIndexParameters()).Any<ParameterInfo>();
      }))));
    }

    private MethodInfo[] BuildPublicNoArgMethods()
    {
      return ((IEnumerable<MethodInfo>) this.Type.GetMethods(BindingFlags.Instance | BindingFlags.Public)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => (object) m.ReturnType != (object) typeof (void) && m.GetParameters().Length == 0)).ToArray<MethodInfo>();
    }
  }
}
