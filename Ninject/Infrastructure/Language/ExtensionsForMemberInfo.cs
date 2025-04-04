// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Language.ExtensionsForMemberInfo
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Infrastructure.Language
{
  public static class ExtensionsForMemberInfo
  {
    private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private static MethodInfo parentDefinitionMethodInfo;

    private static MethodInfo ParentDefinitionMethodInfo
    {
      get
      {
        if (ExtensionsForMemberInfo.parentDefinitionMethodInfo == (MethodInfo) null)
          ExtensionsForMemberInfo.parentDefinitionMethodInfo = typeof (MethodInfo).Assembly.GetType("System.Reflection.RuntimeMethodInfo").GetMethod("GetParentDefinition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        return ExtensionsForMemberInfo.parentDefinitionMethodInfo;
      }
    }

    public static bool HasAttribute<T>(this MemberInfo member)
    {
      return ExtensionsForMemberInfo.HasAttribute(member, typeof (T));
    }

    public static bool HasAttribute(this MemberInfo member, Type type)
    {
      PropertyInfo element = member as PropertyInfo;
      return element != (PropertyInfo) null ? ExtensionsForMemberInfo.IsDefined(element, type, true) : member.IsDefined(type, true);
    }

    public static PropertyInfo GetPropertyFromDeclaredType(
      this MemberInfo memberInfo,
      PropertyInfo propertyDefinition,
      BindingFlags flags)
    {
      return memberInfo.DeclaringType.GetProperty(propertyDefinition.Name, flags, (Binder) null, propertyDefinition.PropertyType, ((IEnumerable<ParameterInfo>) propertyDefinition.GetIndexParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (parameter => parameter.ParameterType)).ToArray<Type>(), (ParameterModifier[]) null);
    }

    public static bool IsPrivate(this PropertyInfo propertyInfo)
    {
      MethodInfo getMethod = propertyInfo.GetGetMethod(true);
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      if (!(getMethod == (MethodInfo) null) && !getMethod.IsPrivate)
        return false;
      return setMethod == (MethodInfo) null || setMethod.IsPrivate;
    }

    public static object[] GetCustomAttributesExtended(
      this MemberInfo member,
      Type attributeType,
      bool inherited)
    {
      return (object[]) Attribute.GetCustomAttributes(member, attributeType, inherited);
    }

    private static PropertyInfo GetParentDefinition(PropertyInfo property)
    {
      MethodInfo methodInfo = property.GetGetMethod(true);
      if ((object) methodInfo == null)
        methodInfo = property.GetSetMethod(true);
      MethodInfo method = methodInfo;
      if (method != (MethodInfo) null)
      {
        MethodInfo parentDefinition = method.GetParentDefinition(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (parentDefinition != (MethodInfo) null)
          return parentDefinition.GetPropertyFromDeclaredType(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      }
      return (PropertyInfo) null;
    }

    private static MethodInfo GetParentDefinition(this MethodInfo method, BindingFlags flags)
    {
      return ExtensionsForMemberInfo.ParentDefinitionMethodInfo == (MethodInfo) null ? (MethodInfo) null : (MethodInfo) ExtensionsForMemberInfo.ParentDefinitionMethodInfo.Invoke((object) method, flags, (Binder) null, (object[]) null, CultureInfo.InvariantCulture);
    }

    private static bool IsDefined(PropertyInfo element, Type attributeType, bool inherit)
    {
      if (element.IsDefined(attributeType, inherit))
        return true;
      if (inherit && ExtensionsForMemberInfo.InternalGetAttributeUsage(attributeType).Inherited)
      {
        for (PropertyInfo parentDefinition = ExtensionsForMemberInfo.GetParentDefinition(element); parentDefinition != (PropertyInfo) null; parentDefinition = ExtensionsForMemberInfo.GetParentDefinition(parentDefinition))
        {
          if (parentDefinition.IsDefined(attributeType, false))
            return true;
        }
      }
      return false;
    }

    private static object[] GetCustomAttributes(
      PropertyInfo propertyInfo,
      Type attributeType,
      bool inherit)
    {
      if (!inherit || !ExtensionsForMemberInfo.InternalGetAttributeUsage(attributeType).Inherited)
        return propertyInfo.GetCustomAttributes(attributeType, inherit);
      Dictionary<Type, bool> attributeUsages = new Dictionary<Type, bool>();
      List<object> attributes = new List<object>();
      attributes.AddRange((IEnumerable<object>) propertyInfo.GetCustomAttributes(attributeType, false));
      for (PropertyInfo parentDefinition = ExtensionsForMemberInfo.GetParentDefinition(propertyInfo); parentDefinition != (PropertyInfo) null; parentDefinition = ExtensionsForMemberInfo.GetParentDefinition(parentDefinition))
      {
        object[] customAttributes = parentDefinition.GetCustomAttributes(attributeType, false);
        ExtensionsForMemberInfo.AddAttributes(attributes, customAttributes, attributeUsages);
      }
      object[] instance = Array.CreateInstance(attributeType, attributes.Count) as object[];
      Array.Copy((Array) attributes.ToArray(), (Array) instance, instance.Length);
      return instance;
    }

    private static void AddAttributes(
      List<object> attributes,
      object[] customAttributes,
      Dictionary<Type, bool> attributeUsages)
    {
      foreach (object customAttribute in customAttributes)
      {
        Type type = customAttribute.GetType();
        if (!attributeUsages.ContainsKey(type))
          attributeUsages[type] = ExtensionsForMemberInfo.InternalGetAttributeUsage(type).Inherited;
        if (attributeUsages[type])
          attributes.Add(customAttribute);
      }
    }

    private static AttributeUsageAttribute InternalGetAttributeUsage(Type type)
    {
      return (AttributeUsageAttribute) type.GetCustomAttributes(typeof (AttributeUsageAttribute), true)[0];
    }
  }
}
