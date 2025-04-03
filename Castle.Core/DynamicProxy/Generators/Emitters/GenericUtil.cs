// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.GenericUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  internal class GenericUtil
  {
    public static Dictionary<string, GenericTypeParameterBuilder> GetGenericArgumentsMap(
      AbstractTypeEmitter parentEmitter)
    {
      if (parentEmitter.GenericTypeParams == null || parentEmitter.GenericTypeParams.Length == 0)
        return new Dictionary<string, GenericTypeParameterBuilder>(0);
      Dictionary<string, GenericTypeParameterBuilder> genericArgumentsMap = new Dictionary<string, GenericTypeParameterBuilder>(parentEmitter.GenericTypeParams.Length);
      foreach (GenericTypeParameterBuilder genericTypeParam in parentEmitter.GenericTypeParams)
        genericArgumentsMap.Add(genericTypeParam.Name, genericTypeParam);
      return genericArgumentsMap;
    }

    public static GenericTypeParameterBuilder[] CopyGenericArguments(
      MethodInfo methodToCopyGenericsFrom,
      TypeBuilder builder,
      Dictionary<string, GenericTypeParameterBuilder> name2GenericType)
    {
      return GenericUtil.CopyGenericArguments(methodToCopyGenericsFrom, name2GenericType, new ApplyGenArgs(builder.DefineGenericParameters));
    }

    public static GenericTypeParameterBuilder[] CopyGenericArguments(
      MethodInfo methodToCopyGenericsFrom,
      MethodBuilder builder,
      Dictionary<string, GenericTypeParameterBuilder> name2GenericType)
    {
      return GenericUtil.CopyGenericArguments(methodToCopyGenericsFrom, name2GenericType, new ApplyGenArgs(builder.DefineGenericParameters));
    }

    private static GenericTypeParameterBuilder[] CopyGenericArguments(
      MethodInfo methodToCopyGenericsFrom,
      Dictionary<string, GenericTypeParameterBuilder> name2GenericType,
      ApplyGenArgs genericParameterGenerator)
    {
      Type[] genericArguments = methodToCopyGenericsFrom.GetGenericArguments();
      if (genericArguments.Length == 0)
        return (GenericTypeParameterBuilder[]) null;
      string[] argumentNames = GenericUtil.GetArgumentNames(genericArguments);
      GenericTypeParameterBuilder[] newGenericParameters = genericParameterGenerator(argumentNames);
      for (int index1 = 0; index1 < newGenericParameters.Length; ++index1)
      {
        try
        {
          GenericParameterAttributes parameterAttributes = genericArguments[index1].GenericParameterAttributes;
          Type[] parameterConstraints = genericArguments[index1].GetGenericParameterConstraints();
          newGenericParameters[index1].SetGenericParameterAttributes(parameterAttributes);
          Type[] all = Array.FindAll<Type>(parameterConstraints, (Predicate<Type>) (type => type.IsInterface));
          Type constraint = Array.Find<Type>(parameterConstraints, (Predicate<Type>) (type => type.IsClass));
          if (all.Length != 0)
          {
            for (int index2 = 0; index2 < all.Length; ++index2)
              all[index2] = GenericUtil.AdjustConstraintToNewGenericParameters(all[index2], methodToCopyGenericsFrom, genericArguments, newGenericParameters);
            newGenericParameters[index1].SetInterfaceConstraints(all);
          }
          if (constraint != null)
          {
            Type genericParameters = GenericUtil.AdjustConstraintToNewGenericParameters(constraint, methodToCopyGenericsFrom, genericArguments, newGenericParameters);
            newGenericParameters[index1].SetBaseTypeConstraint(genericParameters);
          }
          GenericUtil.CopyNonInheritableAttributes(newGenericParameters[index1], genericArguments[index1]);
        }
        catch (NotSupportedException ex)
        {
          newGenericParameters[index1].SetGenericParameterAttributes(GenericParameterAttributes.None);
        }
        name2GenericType[argumentNames[index1]] = newGenericParameters[index1];
      }
      return newGenericParameters;
    }

    private static void CopyNonInheritableAttributes(
      GenericTypeParameterBuilder newGenericParameter,
      Type originalGenericArgument)
    {
      foreach (CustomAttributeBuilder inheritableAttribute in originalGenericArgument.GetNonInheritableAttributes())
        newGenericParameter.SetCustomAttribute(inheritableAttribute);
    }

    private static string[] GetArgumentNames(Type[] originalGenericArguments)
    {
      string[] argumentNames = new string[originalGenericArguments.Length];
      for (int index = 0; index < argumentNames.Length; ++index)
        argumentNames[index] = originalGenericArguments[index].Name;
      return argumentNames;
    }

    private static Type AdjustConstraintToNewGenericParameters(
      Type constraint,
      MethodInfo methodToCopyGenericsFrom,
      Type[] originalGenericParameters,
      GenericTypeParameterBuilder[] newGenericParameters)
    {
      if (constraint.IsGenericType)
      {
        Type[] genericArguments = constraint.GetGenericArguments();
        for (int index = 0; index < genericArguments.Length; ++index)
          genericArguments[index] = GenericUtil.AdjustConstraintToNewGenericParameters(genericArguments[index], methodToCopyGenericsFrom, originalGenericParameters, newGenericParameters);
        return constraint.GetGenericTypeDefinition().MakeGenericType(genericArguments);
      }
      if (!constraint.IsGenericParameter)
        return constraint;
      if (constraint.DeclaringMethod != null)
      {
        int index = Array.IndexOf<Type>(originalGenericParameters, constraint);
        Trace.Assert(index != -1, "When a generic method parameter has a constraint on another method parameter, both parameters must be declared on the same method.");
        return (Type) newGenericParameters[index];
      }
      Trace.Assert(constraint.DeclaringType.IsGenericTypeDefinition);
      Trace.Assert(methodToCopyGenericsFrom.DeclaringType.IsGenericType && constraint.DeclaringType == methodToCopyGenericsFrom.DeclaringType.GetGenericTypeDefinition(), "When a generic method parameter has a constraint on a generic type parameter, the generic type must be the declaring typer of the method.");
      int index1 = Array.IndexOf<Type>(constraint.DeclaringType.GetGenericArguments(), constraint);
      Trace.Assert(index1 != -1, "The generic parameter comes from the given type.");
      return methodToCopyGenericsFrom.DeclaringType.GetGenericArguments()[index1];
    }

    public static Type[] ExtractParametersTypes(
      ParameterInfo[] baseMethodParameters,
      Dictionary<string, GenericTypeParameterBuilder> name2GenericType)
    {
      Type[] parametersTypes = new Type[baseMethodParameters.Length];
      for (int index = 0; index < baseMethodParameters.Length; ++index)
      {
        Type parameterType = baseMethodParameters[index].ParameterType;
        parametersTypes[index] = GenericUtil.ExtractCorrectType(parameterType, name2GenericType);
      }
      return parametersTypes;
    }

    public static Type ExtractCorrectType(
      Type paramType,
      Dictionary<string, GenericTypeParameterBuilder> name2GenericType)
    {
      if (paramType.IsArray)
      {
        int arrayRank = paramType.GetArrayRank();
        Type elementType = paramType.GetElementType();
        if (elementType.IsGenericParameter)
        {
          GenericTypeParameterBuilder parameterBuilder;
          if (!name2GenericType.TryGetValue(elementType.Name, out parameterBuilder))
            return paramType;
          return arrayRank == 1 ? parameterBuilder.MakeArrayType() : parameterBuilder.MakeArrayType(arrayRank);
        }
        return arrayRank == 1 ? elementType.MakeArrayType() : elementType.MakeArrayType(arrayRank);
      }
      GenericTypeParameterBuilder parameterBuilder1;
      return paramType.IsGenericParameter && name2GenericType.TryGetValue(paramType.Name, out parameterBuilder1) ? (Type) parameterBuilder1 : paramType;
    }
  }
}
