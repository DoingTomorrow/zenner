// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.TypeResolver
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public sealed class TypeResolver
  {
    private Dictionary<TypeResolver.AnonTypeId, System.Type> anonymousTypes = new Dictionary<TypeResolver.AnonTypeId, System.Type>();
    private ModuleBuilder moduleBuilder;
    private int anonymousTypeIndex = 0;
    private HashSet<Assembly> assemblies = new HashSet<Assembly>()
    {
      typeof (ExpressionType).Assembly,
      typeof (string).Assembly,
      typeof (List<>).Assembly,
      typeof (Binding).Assembly,
      typeof (WebHttpBehavior).Assembly,
      typeof (XElement).Assembly,
      Assembly.GetExecutingAssembly()
    };

    public ReadOnlyCollection<System.Type> knownTypes { get; private set; }

    public TypeResolver(IEnumerable<Assembly> assemblies = null, IEnumerable<System.Type> knownTypes = null)
    {
      this.moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName()
      {
        Name = "AnonymousTypes"
      }, AssemblyBuilderAccess.Run).DefineDynamicModule("AnonymousTypes");
      if (assemblies != null)
      {
        foreach (Assembly assembly in assemblies)
          this.assemblies.Add(assembly);
      }
      this.knownTypes = new ReadOnlyCollection<System.Type>((IList<System.Type>) new List<System.Type>(((IEnumerable<System.Type>) typeof (string).Assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (t => (t.IsPrimitive || t == typeof (string) || t.IsEnum) && !(t == typeof (IntPtr)) && !(t == typeof (UIntPtr)))).Union<System.Type>((IEnumerable<System.Type>) ((object) knownTypes ?? (object) System.Type.EmptyTypes))));
    }

    public bool HasMappedKnownType(System.Type input) => this.HasMappedKnownType(input, out System.Type _);

    public bool HasMappedKnownType(System.Type input, out System.Type knownType)
    {
      HashSet<System.Type> collection = new HashSet<System.Type>((IEnumerable<System.Type>) this.knownTypes);
      knownType = (System.Type) null;
      foreach (System.Type knownType1 in this.knownTypes)
      {
        if (input == knownType1)
        {
          knownType = knownType1;
          return true;
        }
        int num1;
        if (!(input == knownType1.MakeArrayType()))
        {
          if (!(input == typeof (IEnumerable<>).MakeGenericType(knownType1)))
          {
            num1 = TypeResolver.IsIEnumerableOf(input, knownType1) ? 1 : 0;
            goto label_8;
          }
        }
        num1 = 1;
label_8:
        if (num1 != 0)
        {
          collection.Add(input);
          this.knownTypes = new ReadOnlyCollection<System.Type>((IList<System.Type>) new List<System.Type>((IEnumerable<System.Type>) collection));
          knownType = knownType1;
          return true;
        }
        int num2;
        if (knownType1.IsValueType)
          num2 = input == typeof (Nullable<>).MakeGenericType(knownType1) ? 1 : 0;
        else
          num2 = 0;
        if (num2 != 0)
        {
          collection.Add(input);
          this.knownTypes = new ReadOnlyCollection<System.Type>((IList<System.Type>) new List<System.Type>((IEnumerable<System.Type>) collection));
          knownType = knownType1;
          return true;
        }
      }
      return false;
    }

    public System.Type GetType(string typeName, IEnumerable<System.Type> genericArgumentTypes)
    {
      return this.GetType(typeName).MakeGenericType(genericArgumentTypes.ToArray<System.Type>());
    }

    public System.Type GetType(string typeName)
    {
      if (string.IsNullOrEmpty(typeName))
        throw new ArgumentNullException(nameof (typeName));
      if (typeName.EndsWith("[]"))
        return this.GetType(typeName.Substring(0, typeName.Length - 2)).MakeArrayType();
      if (this.knownTypes.Any<System.Type>((Func<System.Type, bool>) (k => k.FullName == typeName)))
        return this.knownTypes.First<System.Type>((Func<System.Type, bool>) (k => k.FullName == typeName));
      foreach (Assembly assembly in this.assemblies)
      {
        System.Type type = assembly.GetType(typeName);
        if (type != (System.Type) null)
          return type;
      }
      System.Type type1 = System.Type.GetType(typeName, false, true);
      return type1 != (System.Type) null ? type1 : throw new ArgumentException("Could not find a matching type", typeName);
    }

    internal static string GetNameOfExpression(Expression e)
    {
      string name;
      switch (e)
      {
        case LambdaExpression _:
          name = typeof (LambdaExpression).Name;
          break;
        case ParameterExpression _:
          name = typeof (ParameterExpression).Name;
          break;
        case BinaryExpression _:
          name = typeof (BinaryExpression).Name;
          break;
        case MethodCallExpression _:
          name = typeof (MethodCallExpression).Name;
          break;
        default:
          name = e.GetType().Name;
          break;
      }
      return name;
    }

    public MethodInfo GetMethod(
      System.Type declaringType,
      string name,
      System.Type[] parameterTypes,
      System.Type[] genArgTypes)
    {
      foreach (MethodInfo methodInfo in ((IEnumerable<MethodInfo>) declaringType.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.Name == name)))
      {
        try
        {
          MethodInfo method = methodInfo;
          if (methodInfo.IsGenericMethod)
            method = methodInfo.MakeGenericMethod(genArgTypes);
          IEnumerable<System.Type> second = ((IEnumerable<ParameterInfo>) method.GetParameters()).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType));
          if (this.MatchPiecewise<System.Type>((IEnumerable<System.Type>) parameterTypes, second))
            return method;
        }
        catch (ArgumentException ex)
        {
        }
      }
      return (MethodInfo) null;
    }

    private bool MatchPiecewise<T>(IEnumerable<T> first, IEnumerable<T> second)
    {
      T[] array1 = first.ToArray<T>();
      T[] array2 = second.ToArray<T>();
      if (array1.Length != array2.Length)
        return false;
      for (int index = 0; index < array1.Length; ++index)
      {
        if (!array1[index].Equals((object) array2[index]))
          return false;
      }
      return true;
    }

    public System.Type GetOrCreateAnonymousTypeFor(
      string name,
      TypeResolver.NameTypePair[] properties,
      TypeResolver.NameTypePair[] ctr_params)
    {
      TypeResolver.AnonTypeId key = new TypeResolver.AnonTypeId(name, ((IEnumerable<TypeResolver.NameTypePair>) properties).Concat<TypeResolver.NameTypePair>((IEnumerable<TypeResolver.NameTypePair>) ctr_params));
      if (this.anonymousTypes.ContainsKey(key))
        return this.anonymousTypes[key];
      TypeBuilder typeBuilder = this.moduleBuilder.DefineType("<>f__AnonymousType" + (object) this.anonymousTypeIndex++, TypeAttributes.Public);
      FieldBuilder[] fieldBuilderArray = new FieldBuilder[properties.Length];
      PropertyBuilder[] propertyBuilderArray = new PropertyBuilder[properties.Length];
      for (int index = 0; index < properties.Length; ++index)
      {
        fieldBuilderArray[index] = typeBuilder.DefineField("_generatedfield_" + properties[index].Name, properties[index].Type, FieldAttributes.Private);
        propertyBuilderArray[index] = typeBuilder.DefineProperty(properties[index].Name, PropertyAttributes.None, properties[index].Type, new System.Type[0]);
        MethodBuilder mdBuilder = typeBuilder.DefineMethod("get_" + properties[index].Name, MethodAttributes.Public, properties[index].Type, new System.Type[0]);
        ILGenerator ilGenerator = mdBuilder.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldfld, (FieldInfo) fieldBuilderArray[index]);
        ilGenerator.Emit(OpCodes.Ret);
        propertyBuilderArray[index].SetGetMethod(mdBuilder);
      }
      ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.Standard, ((IEnumerable<TypeResolver.NameTypePair>) ctr_params).Select<TypeResolver.NameTypePair, System.Type>((Func<TypeResolver.NameTypePair, System.Type>) (prop => prop.Type)).ToArray<System.Type>());
      ILGenerator ilGenerator1 = constructorBuilder.GetILGenerator();
      for (int index = 0; index < ctr_params.Length; ++index)
      {
        ilGenerator1.Emit(OpCodes.Ldarg_0);
        ilGenerator1.Emit(OpCodes.Ldarg, index + 1);
        ilGenerator1.Emit(OpCodes.Stfld, (FieldInfo) fieldBuilderArray[index]);
        constructorBuilder.DefineParameter(index + 1, ParameterAttributes.None, ctr_params[index].Name);
      }
      ilGenerator1.Emit(OpCodes.Ret);
      System.Type type = typeBuilder.CreateType();
      this.anonymousTypes.Add(key, type);
      return type;
    }

    public static System.Type GetElementType(System.Type collectionType)
    {
      System.Type ienumerable = TypeResolver.FindIEnumerable(collectionType);
      return ienumerable == (System.Type) null ? collectionType : ienumerable.GetGenericArguments()[0];
    }

    private static System.Type FindIEnumerable(System.Type collectionType)
    {
      if (collectionType == (System.Type) null || collectionType == typeof (string))
        return (System.Type) null;
      if (collectionType.IsArray)
        return typeof (IEnumerable<>).MakeGenericType(collectionType.GetElementType());
      if (collectionType.IsGenericType)
      {
        foreach (System.Type genericArgument in collectionType.GetGenericArguments())
        {
          System.Type ienumerable = typeof (IEnumerable<>).MakeGenericType(genericArgument);
          if (ienumerable.IsAssignableFrom(collectionType))
            return ienumerable;
        }
      }
      System.Type[] interfaces = collectionType.GetInterfaces();
      if (interfaces != null && interfaces.Length != 0)
      {
        foreach (System.Type collectionType1 in interfaces)
        {
          System.Type ienumerable = TypeResolver.FindIEnumerable(collectionType1);
          if (ienumerable != (System.Type) null)
            return ienumerable;
        }
      }
      return collectionType.BaseType != (System.Type) null && collectionType.BaseType != typeof (object) ? TypeResolver.FindIEnumerable(collectionType.BaseType) : (System.Type) null;
    }

    public static bool IsIEnumerableOf(System.Type enumerableType, System.Type elementType)
    {
      if (elementType.MakeArrayType() == enumerableType)
        return true;
      if (!enumerableType.IsGenericType)
        return false;
      System.Type[] genericArguments = enumerableType.GetGenericArguments();
      return genericArguments.Length == 1 && elementType.IsAssignableFrom(genericArguments[0]) && typeof (IEnumerable<>).MakeGenericType(genericArguments).IsAssignableFrom(enumerableType);
    }

    public static bool HasBaseType(System.Type thisType, System.Type baseType)
    {
      for (; thisType.BaseType != (System.Type) null && thisType.BaseType != typeof (object); thisType = thisType.BaseType)
      {
        if (thisType.BaseType == baseType)
          return true;
      }
      return false;
    }

    public static IEnumerable<System.Type> GetBaseTypes(System.Type expectedType)
    {
      List<System.Type> baseTypes = new List<System.Type>();
      baseTypes.Add(expectedType);
      if (expectedType.IsArray)
      {
        expectedType = expectedType.GetElementType();
        baseTypes.Add(expectedType);
      }
      else
        baseTypes.Add(expectedType.MakeArrayType());
      while (expectedType.BaseType != (System.Type) null && expectedType.BaseType != typeof (object))
      {
        expectedType = expectedType.BaseType;
        baseTypes.Add(expectedType);
      }
      return (IEnumerable<System.Type>) baseTypes;
    }

    public static List<System.Type> GetDerivedTypes(System.Type baseType)
    {
      return ((IEnumerable<System.Type>) baseType.Assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (anyType => TypeResolver.HasBaseType(anyType, baseType))).ToList<System.Type>();
    }

    public static bool IsNullableType(System.Type t) => t.IsValueType && t.Name == "Nullable`1";

    public static bool HasInheritedProperty(System.Type declaringType, PropertyInfo pInfo)
    {
      if (pInfo.DeclaringType != declaringType)
        return true;
      for (; declaringType.BaseType != (System.Type) null && declaringType.BaseType != typeof (object); declaringType = declaringType.BaseType)
      {
        foreach (PropertyInfo property in declaringType.BaseType.GetProperties())
        {
          if (property.Name == pInfo.Name && property.PropertyType == pInfo.PropertyType)
            return true;
        }
      }
      return false;
    }

    public static string ToGenericTypeFullNameString(System.Type t)
    {
      if (t.FullName == null && t.IsGenericParameter)
        return t.GenericParameterPosition == 0 ? "T" : "T" + (object) t.GenericParameterPosition;
      if (!t.IsGenericType)
        return t.FullName;
      string str = t.FullName.Substring(0, t.FullName.IndexOf('`')) + "<";
      System.Type[] genericArguments = t.GetGenericArguments();
      List<string> source = new List<string>();
      for (int index = 0; index < genericArguments.Length; ++index)
      {
        str = str + "{" + (object) index + "},";
        string typeFullNameString = TypeResolver.ToGenericTypeFullNameString(genericArguments[index]);
        source.Add(typeFullNameString);
      }
      return string.Format(str.TrimEnd(',') + ">", (object[]) source.ToArray<string>());
    }

    public static string ToGenericTypeNameString(System.Type t)
    {
      string typeFullNameString = TypeResolver.ToGenericTypeFullNameString(t);
      return typeFullNameString.Substring(typeFullNameString.LastIndexOf('.') + 1).TrimEnd('>');
    }

    public class NameTypePair
    {
      public string Name { get; set; }

      public System.Type Type { get; set; }

      public override int GetHashCode() => this.Name.GetHashCode() + this.Type.GetHashCode();

      public override bool Equals(object obj)
      {
        if (!(obj is TypeResolver.NameTypePair))
          return false;
        TypeResolver.NameTypePair nameTypePair = obj as TypeResolver.NameTypePair;
        return this.Name.Equals(nameTypePair.Name) && this.Type.Equals(nameTypePair.Type);
      }
    }

    private class AnonTypeId
    {
      public string Name { get; private set; }

      public IEnumerable<TypeResolver.NameTypePair> Properties { get; private set; }

      public AnonTypeId(string name, IEnumerable<TypeResolver.NameTypePair> properties)
      {
        this.Name = name;
        this.Properties = properties;
      }

      public override int GetHashCode()
      {
        int hashCode = this.Name.GetHashCode();
        foreach (TypeResolver.NameTypePair property in this.Properties)
          hashCode += property.GetHashCode();
        return hashCode;
      }

      public override bool Equals(object obj)
      {
        if (!(obj is TypeResolver.AnonTypeId))
          return false;
        TypeResolver.AnonTypeId anonTypeId = obj as TypeResolver.AnonTypeId;
        return this.Name.Equals(anonTypeId.Name) && this.Properties.SequenceEqual<TypeResolver.NameTypePair>(anonTypeId.Properties);
      }
    }
  }
}
