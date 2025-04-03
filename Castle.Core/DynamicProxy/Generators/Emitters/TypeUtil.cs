// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.TypeUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public static class TypeUtil
  {
    public static FieldInfo[] GetAllFields(this Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (!type.IsClass)
        throw new ArgumentException(string.Format("Type {0} is not a class type. This method supports only classes", (object) type));
      List<FieldInfo> fieldInfoList = new List<FieldInfo>();
      for (Type type1 = type; type1 != typeof (object); type1 = type1.BaseType)
      {
        FieldInfo[] fields = type1.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        fieldInfoList.AddRange((IEnumerable<FieldInfo>) fields);
      }
      return fieldInfoList.ToArray();
    }

    public static ICollection<Type> GetAllInterfaces(params Type[] types)
    {
      if (types == null)
        return (ICollection<Type>) Type.EmptyTypes;
      object obj = new object();
      IDictionary<Type, object> dictionary = (IDictionary<Type, object>) new Dictionary<Type, object>();
      foreach (Type type in types)
      {
        if (type != null)
        {
          if (type.IsInterface)
            dictionary[type] = obj;
          foreach (Type key in type.GetInterfaces())
            dictionary[key] = obj;
        }
      }
      return (ICollection<Type>) TypeUtil.Sort((IEnumerable<Type>) dictionary.Keys);
    }

    public static ICollection<Type> GetAllInterfaces(this Type type)
    {
      return TypeUtil.GetAllInterfaces(new Type[1]{ type });
    }

    public static Type GetClosedParameterType(this AbstractTypeEmitter type, Type parameter)
    {
      if (parameter.IsGenericTypeDefinition)
        return parameter.GetGenericTypeDefinition().MakeGenericType(type.GetGenericArgumentsFor(parameter));
      if (parameter.IsGenericType)
      {
        Type[] genericArguments = parameter.GetGenericArguments();
        if (TypeUtil.CloseGenericParametersIfAny(type, genericArguments))
          return parameter.GetGenericTypeDefinition().MakeGenericType(genericArguments);
      }
      if (parameter.IsGenericParameter)
        return type.GetGenericArgument(parameter.Name);
      if (parameter.IsArray)
        return type.GetClosedParameterType(parameter.GetElementType()).MakeArrayType();
      return parameter.IsByRef ? type.GetClosedParameterType(parameter.GetElementType()).MakeByRefType() : parameter;
    }

    public static void SetStaticField(
      this Type type,
      string fieldName,
      BindingFlags additionalFlags,
      object value)
    {
      BindingFlags invokeAttr = additionalFlags | BindingFlags.Static | BindingFlags.SetField;
      try
      {
        type.InvokeMember(fieldName, invokeAttr, (Binder) null, (object) null, new object[1]
        {
          value
        });
      }
      catch (MissingFieldException ex)
      {
        throw new ProxyGenerationException(string.Format("Could not find field named '{0}' on type {1}. This is likely a bug in DynamicProxy. Please report it.", (object) fieldName, (object) type), (Exception) ex);
      }
      catch (TargetException ex)
      {
        throw new ProxyGenerationException(string.Format("There was an error trying to set field named '{0}' on type {1}. This is likely a bug in DynamicProxy. Please report it.", (object) fieldName, (object) type), (Exception) ex);
      }
      catch (TargetInvocationException ex)
      {
        if (ex.InnerException is TypeInitializationException)
          throw new ProxyGenerationException(string.Format("There was an error in static constructor on type {0}. This is likely a bug in DynamicProxy. Please report it.", (object) type), (Exception) ex);
        throw;
      }
    }

    public static MemberInfo[] Sort(MemberInfo[] members)
    {
      Array.Sort<MemberInfo>(members, (Comparison<MemberInfo>) ((l, r) => string.Compare(l.Name, r.Name)));
      return members;
    }

    private static bool CloseGenericParametersIfAny(AbstractTypeEmitter emitter, Type[] arguments)
    {
      bool flag = false;
      for (int index = 0; index < arguments.Length; ++index)
      {
        Type closedParameterType = emitter.GetClosedParameterType(arguments[index]);
        if (!object.ReferenceEquals((object) closedParameterType, (object) arguments[index]))
        {
          arguments[index] = closedParameterType;
          flag = true;
        }
      }
      return flag;
    }

    private static Type[] Sort(IEnumerable<Type> types)
    {
      Type[] array = types.ToArray<Type>();
      Array.Sort<Type>(array, (Comparison<Type>) ((l, r) => string.Compare(l.AssemblyQualifiedName, r.AssemblyQualifiedName)));
      return array;
    }
  }
}
