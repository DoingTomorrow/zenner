// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.AttributeDisassembler
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  [Serializable]
  public class AttributeDisassembler : IAttributeDisassembler
  {
    public CustomAttributeBuilder Disassemble(Attribute attribute)
    {
      Type type = attribute.GetType();
      try
      {
        ConstructorInfo ci;
        object[] constructorAndArgs = AttributeDisassembler.GetConstructorAndArgs(type, attribute, out ci);
        Attribute instance = (Attribute) Activator.CreateInstance(type, constructorAndArgs);
        PropertyInfo[] properties;
        object[] propertyValues = AttributeDisassembler.GetPropertyValues(type, out properties, attribute, instance);
        FieldInfo[] fields;
        object[] fieldValues = AttributeDisassembler.GetFieldValues(type, out fields, attribute, instance);
        return new CustomAttributeBuilder(ci, constructorAndArgs, properties, propertyValues, fields, fieldValues);
      }
      catch (Exception ex)
      {
        return this.HandleError(type, ex);
      }
    }

    protected virtual CustomAttributeBuilder HandleError(Type attributeType, Exception exception)
    {
      throw new ProxyGenerationException("DynamicProxy was unable to disassemble attribute " + attributeType.Name + " using default AttributeDisassembler. " + string.Format("To handle the disassembly process properly implement the {0} interface, ", (object) typeof (IAttributeDisassembler)) + "and register your disassembler to handle this type of attributes using " + typeof (AttributeUtil).Name + ".AddDisassembler<" + attributeType.Name + ">(yourDisassembler) method", exception);
    }

    private static object[] GetConstructorAndArgs(
      Type attType,
      Attribute attribute,
      out ConstructorInfo ci)
    {
      object[] args = new object[0];
      ci = attType.GetConstructors()[0];
      ParameterInfo[] parameters = ci.GetParameters();
      if (parameters.Length != 0)
      {
        args = new object[parameters.Length];
        AttributeDisassembler.InitializeConstructorArgs(attType, attribute, args, parameters);
      }
      return args;
    }

    private static object[] GetPropertyValues(
      Type attType,
      out PropertyInfo[] properties,
      Attribute original,
      Attribute replicated)
    {
      List<PropertyInfo> propertyCandidates = AttributeDisassembler.GetPropertyCandidates(attType);
      List<object> objectList = new List<object>(propertyCandidates.Count);
      List<PropertyInfo> propertyInfoList = new List<PropertyInfo>(propertyCandidates.Count);
      foreach (PropertyInfo propertyInfo in propertyCandidates)
      {
        object first = propertyInfo.GetValue((object) original, (object[]) null);
        object second = propertyInfo.GetValue((object) replicated, (object[]) null);
        if (!AttributeDisassembler.AreAttributeElementsEqual(first, second))
        {
          propertyInfoList.Add(propertyInfo);
          objectList.Add(first);
        }
      }
      properties = propertyInfoList.ToArray();
      return objectList.ToArray();
    }

    private static object[] GetFieldValues(
      Type attType,
      out FieldInfo[] fields,
      Attribute original,
      Attribute replicated)
    {
      FieldInfo[] fields1 = attType.GetFields(BindingFlags.Instance | BindingFlags.Public);
      List<object> objectList = new List<object>(fields1.Length);
      List<FieldInfo> fieldInfoList = new List<FieldInfo>(fields1.Length);
      foreach (FieldInfo fieldInfo in fields1)
      {
        object first = fieldInfo.GetValue((object) original);
        object second = fieldInfo.GetValue((object) replicated);
        if (!AttributeDisassembler.AreAttributeElementsEqual(first, second))
        {
          fieldInfoList.Add(fieldInfo);
          objectList.Add(first);
        }
      }
      fields = fieldInfoList.ToArray();
      return objectList.ToArray();
    }

    private static void InitializeConstructorArgs(
      Type attType,
      Attribute attribute,
      object[] args,
      ParameterInfo[] parameterInfos)
    {
      for (int index = 0; index < args.Length; ++index)
        args[index] = AttributeDisassembler.GetArgValue(attType, attribute, parameterInfos[index]);
    }

    private static object GetArgValue(
      Type attType,
      Attribute attribute,
      ParameterInfo parameterInfo)
    {
      Type parameterType = parameterInfo.ParameterType;
      PropertyInfo[] properties = attType.GetProperties();
      foreach (PropertyInfo propertyInfo in properties)
      {
        if ((propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length == 0) && string.Compare(propertyInfo.Name, parameterInfo.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
          return AttributeDisassembler.ConvertValue(propertyInfo.GetValue((object) attribute, (object[]) null), parameterType);
      }
      PropertyInfo bestMatch = (PropertyInfo) null;
      foreach (PropertyInfo propertyInfo in properties)
      {
        if (propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length == 0)
          bestMatch = AttributeDisassembler.ReplaceIfBetterMatch(parameterInfo, propertyInfo, bestMatch);
      }
      return bestMatch != null ? AttributeDisassembler.ConvertValue(bestMatch.GetValue((object) attribute, (object[]) null), parameterType) : AttributeDisassembler.GetDefaultValueFor(parameterType);
    }

    private static PropertyInfo ReplaceIfBetterMatch(
      ParameterInfo parameterInfo,
      PropertyInfo propertyInfo,
      PropertyInfo bestMatch)
    {
      bool flag = bestMatch == null || bestMatch.PropertyType != parameterInfo.ParameterType;
      return propertyInfo.PropertyType == parameterInfo.ParameterType && flag || parameterInfo.ParameterType == typeof (string) && flag ? propertyInfo : bestMatch;
    }

    private static object ConvertValue(object obj, Type paramType)
    {
      if (obj == null)
        return (object) null;
      return paramType == typeof (string) ? (object) obj.ToString() : obj;
    }

    private static object GetDefaultValueFor(Type type)
    {
      if (type == typeof (bool))
        return (object) false;
      if (type.IsEnum)
        return Enum.GetValues(type).GetValue(0);
      if (type == typeof (char))
        return (object) char.MinValue;
      return type.IsPrimitive ? (object) 0 : (object) null;
    }

    private static List<PropertyInfo> GetPropertyCandidates(Type attributeType)
    {
      List<PropertyInfo> propertyCandidates = new List<PropertyInfo>();
      foreach (PropertyInfo property in attributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (property.CanRead && property.CanWrite)
          propertyCandidates.Add(property);
      }
      return propertyCandidates;
    }

    private static bool AreAttributeElementsEqual(object first, object second)
    {
      if (first == null)
        return second == null;
      return first is string first1 ? AttributeDisassembler.AreStringsEqual(first1, second as string) : first.Equals(second);
    }

    private static bool AreStringsEqual(string first, string second)
    {
      return first.Equals(second, StringComparison.Ordinal);
    }

    public bool Equals(AttributeDisassembler other)
    {
      return !object.ReferenceEquals((object) null, (object) other);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (AttributeDisassembler) && this.Equals((AttributeDisassembler) obj);
    }

    public override int GetHashCode() => this.GetType().GetHashCode();
  }
}
