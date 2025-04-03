// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.AttributeUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy
{
  public static class AttributeUtil
  {
    private static readonly IDictionary<Type, IAttributeDisassembler> disassemblers = (IDictionary<Type, IAttributeDisassembler>) new Dictionary<Type, IAttributeDisassembler>();
    private static IAttributeDisassembler fallbackDisassembler = (IAttributeDisassembler) new AttributeDisassembler();

    public static IAttributeDisassembler FallbackDisassembler
    {
      get => AttributeUtil.fallbackDisassembler;
      set => AttributeUtil.fallbackDisassembler = value;
    }

    public static void AddDisassembler<TAttribute>(IAttributeDisassembler disassembler) where TAttribute : Attribute
    {
      AttributeUtil.disassemblers[typeof (TAttribute)] = disassembler != null ? disassembler : throw new ArgumentNullException(nameof (disassembler));
    }

    public static CustomAttributeBuilder CreateBuilder(CustomAttributeData attribute)
    {
      PropertyInfo[] properties;
      object[] propertyValues;
      FieldInfo[] fields;
      object[] fieldValues;
      AttributeUtil.GetSettersAndFields((IEnumerable<CustomAttributeNamedArgument>) attribute.NamedArguments, out properties, out propertyValues, out fields, out fieldValues);
      return new CustomAttributeBuilder(attribute.Constructor, AttributeUtil.GetCtorArguments(attribute.ConstructorArguments), properties, propertyValues, fields, fieldValues);
    }

    private static object[] GetCtorArguments(
      IList<CustomAttributeTypedArgument> constructorArguments)
    {
      object[] ctorArguments = new object[constructorArguments.Count];
      for (int index = 0; index < constructorArguments.Count; ++index)
        ctorArguments[index] = constructorArguments[index].Value;
      return ctorArguments;
    }

    private static void GetSettersAndFields(
      IEnumerable<CustomAttributeNamedArgument> namedArguments,
      out PropertyInfo[] properties,
      out object[] propertyValues,
      out FieldInfo[] fields,
      out object[] fieldValues)
    {
      List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
      List<object> objectList1 = new List<object>();
      List<FieldInfo> fieldInfoList = new List<FieldInfo>();
      List<object> objectList2 = new List<object>();
      foreach (CustomAttributeNamedArgument namedArgument in namedArguments)
      {
        switch (namedArgument.MemberInfo.MemberType)
        {
          case MemberTypes.Field:
            fieldInfoList.Add(namedArgument.MemberInfo as FieldInfo);
            objectList2.Add(namedArgument.TypedValue.Value);
            continue;
          case MemberTypes.Property:
            propertyInfoList.Add(namedArgument.MemberInfo as PropertyInfo);
            objectList1.Add(namedArgument.TypedValue.Value);
            continue;
          default:
            throw new ArgumentException(string.Format("Unexpected member type {0} in custom attribute.", (object) namedArgument.MemberInfo.MemberType));
        }
      }
      properties = propertyInfoList.ToArray();
      propertyValues = objectList1.ToArray();
      fields = fieldInfoList.ToArray();
      fieldValues = objectList2.ToArray();
    }

    public static IEnumerable<CustomAttributeBuilder> GetNonInheritableAttributes(
      this MemberInfo member)
    {
      IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(member);
      foreach (CustomAttributeData attribute in (IEnumerable<CustomAttributeData>) attributes)
      {
        Type attributeType = attribute.Constructor.DeclaringType;
        if (!AttributeUtil.ShouldSkipAttributeReplication(attributeType))
        {
          CustomAttributeBuilder builder;
          try
          {
            builder = AttributeUtil.CreateBuilder(attribute);
          }
          catch (ArgumentException ex)
          {
            throw new ProxyGenerationException(string.Format("Due to limitations in CLR, DynamicProxy was unable to successfully replicate non-inheritable attribute {0} on {1}{2}. To avoid this error you can chose not to replicate this attribute type by calling '{3}.Add(typeof({0}))'.", (object) attributeType.FullName, member.ReflectedType == null ? (object) "" : (object) member.ReflectedType.FullName, member is Type ? (object) "" : (object) ("." + member.Name), (object) typeof (AttributesToAvoidReplicating).FullName), (Exception) ex);
          }
          if (builder != null)
            yield return builder;
        }
      }
    }

    public static IEnumerable<CustomAttributeBuilder> GetNonInheritableAttributes(
      this ParameterInfo parameter)
    {
      IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(parameter);
      foreach (CustomAttributeData attribute in (IEnumerable<CustomAttributeData>) attributes)
      {
        Type attributeType = attribute.Constructor.DeclaringType;
        if (!AttributeUtil.ShouldSkipAttributeReplication(attributeType))
        {
          CustomAttributeBuilder builder = AttributeUtil.CreateBuilder(attribute);
          if (builder != null)
            yield return builder;
        }
      }
    }

    private static bool ShouldSkipAttributeReplication(Type attribute)
    {
      if (!attribute.IsPublic || AttributeUtil.SpecialCaseAttributThatShouldNotBeReplicated(attribute))
        return true;
      object[] customAttributes = attribute.GetCustomAttributes(typeof (AttributeUsageAttribute), true);
      return customAttributes.Length == 0 || ((AttributeUsageAttribute) customAttributes[0]).Inherited;
    }

    private static bool SpecialCaseAttributThatShouldNotBeReplicated(Type attribute)
    {
      return AttributesToAvoidReplicating.Contains(attribute);
    }

    public static CustomAttributeBuilder CreateBuilder<TAttribute>() where TAttribute : Attribute, new()
    {
      return new CustomAttributeBuilder(typeof (TAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
    }

    public static CustomAttributeBuilder CreateBuilder(
      Type attribute,
      object[] constructorArguments)
    {
      return new CustomAttributeBuilder(attribute.GetConstructor(AttributeUtil.GetTypes(constructorArguments)), constructorArguments);
    }

    internal static CustomAttributeBuilder CreateBuilder(Attribute attribute)
    {
      Type type = attribute.GetType();
      IAttributeDisassembler attributeDisassembler;
      return AttributeUtil.disassemblers.TryGetValue(type, out attributeDisassembler) ? attributeDisassembler.Disassemble(attribute) : AttributeUtil.FallbackDisassembler.Disassemble(attribute);
    }

    private static Type[] GetTypes(object[] objects)
    {
      Type[] types = new Type[objects.Length];
      for (int index = 0; index < types.Length; ++index)
        types[index] = objects[index].GetType();
      return types;
    }
  }
}
