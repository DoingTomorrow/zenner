// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.TypeExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class TypeExtensions
  {
    private const BindingFlags PropertiesOfClassHierarchy = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
    private const BindingFlags PropertiesOrFieldOfClass = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
      foreach (Type baseType in type.GetInterfaces())
        yield return baseType;
      Type analizing = type;
      while (analizing != null && analizing != typeof (object))
      {
        analizing = analizing.BaseType;
        yield return analizing;
      }
    }

    public static IEnumerable<Type> GetHierarchyFromBase(this Type type)
    {
      List<Type> source = new List<Type>();
      for (Type type1 = type; type1 != null && type1 != typeof (object); type1 = type1.BaseType)
        source.Add(type1);
      return source.AsEnumerable<Type>().Reverse<Type>();
    }

    public static Type GetPropertyOrFieldType(this MemberInfo propertyOrField)
    {
      if (propertyOrField.MemberType == MemberTypes.Property)
        return ((PropertyInfo) propertyOrField).PropertyType;
      return propertyOrField.MemberType == MemberTypes.Field ? ((FieldInfo) propertyOrField).FieldType : throw new ArgumentOutOfRangeException(nameof (propertyOrField), "Expected PropertyInfo or FieldInfo; found :" + (object) propertyOrField.MemberType);
    }

    public static MemberInfo DecodeMemberAccessExpression<TEntity>(
      Expression<Func<TEntity, object>> expression)
    {
      if (expression.Body.NodeType == ExpressionType.MemberAccess)
        return ((MemberExpression) expression.Body).Member;
      if (expression.Body.NodeType == ExpressionType.Convert && expression.Body.Type == typeof (object))
        return ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member;
      throw new Exception(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", (object) expression.Body.NodeType));
    }

    public static MemberInfo DecodeMemberAccessExpressionOf<TEntity>(
      Expression<Func<TEntity, object>> expression)
    {
      MemberInfo member;
      if (expression.Body.NodeType != ExpressionType.MemberAccess)
      {
        if (expression.Body.NodeType != ExpressionType.Convert || expression.Body.Type != typeof (object))
          throw new Exception(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", (object) expression.Body.NodeType));
        member = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member;
      }
      else
        member = ((MemberExpression) expression.Body).Member;
      return typeof (TEntity).IsInterface ? member : (MemberInfo) typeof (TEntity).GetProperty(member.Name, member.GetPropertyOrFieldType());
    }

    public static MemberInfo DecodeMemberAccessExpression<TEntity, TProperty>(
      Expression<Func<TEntity, TProperty>> expression)
    {
      if (expression.Body.NodeType == ExpressionType.MemberAccess)
        return ((MemberExpression) expression.Body).Member;
      if (expression.Body.NodeType == ExpressionType.Convert && expression.Body.Type == typeof (object))
        return ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member;
      throw new Exception(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", (object) expression.Body.NodeType));
    }

    public static MemberInfo DecodeMemberAccessExpressionOf<TEntity, TProperty>(
      Expression<Func<TEntity, TProperty>> expression)
    {
      MemberInfo member;
      if (expression.Body.NodeType != ExpressionType.MemberAccess)
      {
        if (expression.Body.NodeType != ExpressionType.Convert || expression.Body.Type != typeof (object))
          throw new Exception(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", (object) expression.Body.NodeType));
        member = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member;
      }
      else
        member = ((MemberExpression) expression.Body).Member;
      return typeof (TEntity).IsInterface ? member : (MemberInfo) typeof (TEntity).GetProperty(member.Name, member.GetPropertyOrFieldType());
    }

    public static MemberInfo GetMemberFromDeclaringType(this MemberInfo source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (source.DeclaringType.Equals(source.ReflectedType))
        return source;
      switch (source)
      {
        case PropertyInfo _:
          return (MemberInfo) source.DeclaringType.GetProperty(source.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        case FieldInfo _:
          return (MemberInfo) source.DeclaringType.GetField(source.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        default:
          return (MemberInfo) null;
      }
    }

    public static IEnumerable<MemberInfo> GetMemberFromDeclaringClasses(this MemberInfo source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (source is PropertyInfo)
      {
        Type reflectedType = source.ReflectedType;
        Type memberType = source.GetPropertyOrFieldType();
        return reflectedType.GetPropertiesOfHierarchy().Cast<PropertyInfo>().Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => source.Name.Equals(x.Name) && memberType.Equals(x.PropertyType))).Cast<MemberInfo>();
      }
      if (!(source is FieldInfo))
        return Enumerable.Empty<MemberInfo>();
      return (IEnumerable<MemberInfo>) new MemberInfo[1]
      {
        source.GetMemberFromDeclaringType()
      };
    }

    public static IEnumerable<MemberInfo> GetPropertyFromInterfaces(this MemberInfo source)
    {
      PropertyInfo propertyInfo = source != null ? source as PropertyInfo : throw new ArgumentNullException(nameof (source));
      if (propertyInfo != null && !source.ReflectedType.IsInterface)
      {
        Type[] interfaces = source.ReflectedType.GetInterfaces();
        if (interfaces.Length != 0)
        {
          MethodInfo propertyGetter = propertyInfo.GetGetMethod();
          foreach (Type interfaceType in interfaces)
          {
            InterfaceMapping memberMap = source.ReflectedType.GetInterfaceMap(interfaceType);
            PropertyInfo[] interfaceProperties = interfaceType.GetProperties();
            for (int i = 0; i < memberMap.TargetMethods.Length; ++i)
            {
              if (memberMap.TargetMethods[i] == propertyGetter)
                yield return (MemberInfo) ((IEnumerable<PropertyInfo>) interfaceProperties).Single<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.GetGetMethod() == memberMap.InterfaceMethods[i]));
            }
          }
        }
      }
    }

    public static Type DetermineCollectionElementType(this Type genericCollection)
    {
      if (genericCollection.IsGenericType)
      {
        List<Type> list = ((IEnumerable<Type>) genericCollection.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).ToList<Type>();
        if (genericCollection.IsInterface)
          list.Add(genericCollection);
        Type type = list.FirstOrDefault<Type>((Func<Type, bool>) (t => t.GetGenericTypeDefinition() == typeof (IEnumerable<>)));
        if (type != null)
          return type.GetGenericArguments()[0];
      }
      return (Type) null;
    }

    public static Type DetermineCollectionElementOrDictionaryValueType(this Type genericCollection)
    {
      if (genericCollection.IsGenericType)
      {
        List<Type> list = ((IEnumerable<Type>) genericCollection.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).ToList<Type>();
        if (genericCollection.IsInterface)
          list.Add(genericCollection);
        Type type1 = list.FirstOrDefault<Type>((Func<Type, bool>) (t => t.GetGenericTypeDefinition() == typeof (IEnumerable<>)));
        if (type1 != null)
        {
          Type type2 = list.FirstOrDefault<Type>((Func<Type, bool>) (t => t.GetGenericTypeDefinition() == typeof (IDictionary<,>)));
          return type2 == null ? type1.GetGenericArguments()[0] : type2.GetGenericArguments()[1];
        }
      }
      return (Type) null;
    }

    public static Type DetermineDictionaryKeyType(this Type genericDictionary)
    {
      if (genericDictionary.IsGenericType)
      {
        Type dictionaryInterface = TypeExtensions.GetDictionaryInterface(genericDictionary);
        if (dictionaryInterface != null)
          return dictionaryInterface.GetGenericArguments()[0];
      }
      return (Type) null;
    }

    private static Type GetDictionaryInterface(Type genericDictionary)
    {
      List<Type> list = ((IEnumerable<Type>) genericDictionary.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).ToList<Type>();
      if (genericDictionary.IsInterface)
        list.Add(genericDictionary);
      return list.FirstOrDefault<Type>((Func<Type, bool>) (t => t.GetGenericTypeDefinition() == typeof (IDictionary<,>)));
    }

    public static Type DetermineDictionaryValueType(this Type genericDictionary)
    {
      if (genericDictionary.IsGenericType)
      {
        Type dictionaryInterface = TypeExtensions.GetDictionaryInterface(genericDictionary);
        if (dictionaryInterface != null)
          return dictionaryInterface.GetGenericArguments()[1];
      }
      return (Type) null;
    }

    public static bool IsGenericCollection(this Type source)
    {
      return source.IsGenericType && typeof (IEnumerable).IsAssignableFrom(source);
    }

    public static MemberInfo GetFirstPropertyOfType(
      this Type propertyContainerType,
      Type propertyType)
    {
      return propertyContainerType.GetFirstPropertyOfType(propertyType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static MemberInfo GetFirstPropertyOfType(
      this Type propertyContainerType,
      Type propertyType,
      Func<PropertyInfo, bool> acceptPropertyClauses)
    {
      return propertyContainerType.GetFirstPropertyOfType(propertyType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, acceptPropertyClauses);
    }

    public static MemberInfo GetFirstPropertyOfType(
      this Type propertyContainerType,
      Type propertyType,
      BindingFlags bindingFlags)
    {
      return propertyContainerType.GetFirstPropertyOfType(propertyType, bindingFlags, (Func<PropertyInfo, bool>) (x => true));
    }

    public static MemberInfo GetFirstPropertyOfType(
      this Type propertyContainerType,
      Type propertyType,
      BindingFlags bindingFlags,
      Func<PropertyInfo, bool> acceptPropertyClauses)
    {
      if (acceptPropertyClauses == null)
        throw new ArgumentNullException(nameof (acceptPropertyClauses));
      return propertyContainerType == null || propertyType == null ? (MemberInfo) null : (MemberInfo) ((IEnumerable<PropertyInfo>) propertyContainerType.GetProperties(bindingFlags)).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => acceptPropertyClauses(p) && propertyType.Equals(p.PropertyType)));
    }

    public static IEnumerable<MemberInfo> GetInterfaceProperties(this Type type)
    {
      if (type.IsInterface)
      {
        List<Type> analyzedInterface = new List<Type>();
        Queue<Type> interfacesQueue = new Queue<Type>();
        analyzedInterface.Add(type);
        interfacesQueue.Enqueue(type);
        while (interfacesQueue.Count > 0)
        {
          Type subType = interfacesQueue.Dequeue();
          foreach (Type type1 in ((IEnumerable<Type>) subType.GetInterfaces()).Where<Type>((Func<Type, bool>) (subInterface => !analyzedInterface.Contains(subInterface))))
          {
            analyzedInterface.Add(type1);
            interfacesQueue.Enqueue(type1);
          }
          foreach (PropertyInfo property in subType.GetProperties())
            yield return (MemberInfo) property;
        }
      }
    }

    public static bool IsEnumOrNullableEnum(this Type type)
    {
      if (type == null)
        return false;
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>) ? type.GetGenericArguments()[0].IsEnum : type.IsEnum;
    }

    public static bool IsFlagEnumOrNullableFlagEnum(this Type type)
    {
      if (type == null)
        return false;
      Type type1 = type;
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
        type1 = type.GetGenericArguments()[0];
      return type1.IsEnum && type1.GetCustomAttributes(typeof (FlagsAttribute), false).Length > 0;
    }

    public static IEnumerable<Type> GetGenericInterfaceTypeDefinitions(this Type type)
    {
      if (type.IsGenericType && type.IsInterface)
        yield return type.GetGenericTypeDefinition();
      foreach (Type t in ((IEnumerable<Type>) type.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)))
        yield return t.GetGenericTypeDefinition();
    }

    public static Type GetFirstImplementorOf(this Type source, Type abstractType)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (abstractType == null)
        throw new ArgumentNullException(nameof (abstractType));
      if (source.IsInterface)
        return (Type) null;
      return source.Equals(abstractType) ? source : source.GetHierarchyFromBase().FirstOrDefault<Type>((Func<Type, bool>) (t => !t.Equals(abstractType) && abstractType.IsAssignableFrom(t)));
    }

    public static bool HasPublicPropertyOf(this Type source, Type typeOfProperty)
    {
      return source.GetFirstPropertyOfType(typeOfProperty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) != null;
    }

    public static bool HasPublicPropertyOf(
      this Type source,
      Type typeOfProperty,
      Func<PropertyInfo, bool> acceptPropertyClauses)
    {
      return source.GetFirstPropertyOfType(typeOfProperty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, acceptPropertyClauses) != null;
    }

    public static MemberInfo GetMemberFromReflectedType(this MemberInfo member, Type reflectedType)
    {
      if (member == null)
        throw new ArgumentNullException(nameof (member));
      if (reflectedType == null)
        throw new ArgumentNullException(nameof (reflectedType));
      if (member is FieldInfo fieldInfo && fieldInfo.IsPrivate)
        return member;
      if (member is PropertyInfo propertyInfo)
      {
        MethodInfo getMethod = propertyInfo.GetGetMethod(true);
        if (getMethod.IsPrivate)
          return member;
        if (propertyInfo.DeclaringType.IsInterface)
        {
          Type[] interfaces = reflectedType.GetInterfaces();
          Type declaringType = propertyInfo.DeclaringType;
          if (!((IEnumerable<Type>) interfaces).Contains<Type>(declaringType))
            return member;
          PropertyInfo[] properties = reflectedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          InterfaceMapping memberMap = reflectedType.GetInterfaceMap(declaringType);
          for (int i = 0; i < memberMap.TargetMethods.Length; ++i)
          {
            if (memberMap.InterfaceMethods[i] == getMethod)
              return (MemberInfo) ((IEnumerable<PropertyInfo>) properties).Single<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.GetGetMethod(true) == memberMap.TargetMethods[i]));
          }
          return member;
        }
      }
      return reflectedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Cast<MemberInfo>().Concat<MemberInfo>((IEnumerable<MemberInfo>) reflectedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (m => m.Name.Equals(member.Name) && m.GetPropertyOrFieldType().Equals(member.GetPropertyOrFieldType()))) ?? member;
    }

    public static MemberInfo GetPropertyOrFieldMatchingName(this Type source, string memberName)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (memberName == null)
        return (MemberInfo) null;
      string nameToFind = memberName.Trim();
      return source.GetPropertiesOfHierarchy().Concat<MemberInfo>(source.GetFieldsOfHierarchy()).Concat<MemberInfo>(source.GetPropertiesOfInterfacesImplemented()).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (x => x.Name == nameToFind));
    }

    private static IEnumerable<MemberInfo> GetPropertiesOfInterfacesImplemented(this Type source)
    {
      if (source.IsInterface)
      {
        foreach (MemberInfo interfaceProperty in source.GetInterfaceProperties())
          yield return interfaceProperty;
      }
      else
      {
        foreach (Type type in source.GetInterfaces())
        {
          IEnumerator<MemberInfo> enumerator = type.GetInterfaceProperties().GetEnumerator();
          while (enumerator.MoveNext())
          {
            MemberInfo interfaceProperty = enumerator.Current;
            yield return interfaceProperty;
          }
        }
      }
    }

    internal static IEnumerable<MemberInfo> GetPropertiesOfHierarchy(this Type type)
    {
      if (!type.IsInterface)
      {
        for (Type analizing = type; analizing != null && analizing != typeof (object); analizing = analizing.BaseType)
        {
          foreach (PropertyInfo property in analizing.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            yield return (MemberInfo) property;
        }
      }
    }

    private static IEnumerable<MemberInfo> GetFieldsOfHierarchy(this Type type)
    {
      if (!type.IsInterface)
      {
        for (Type analizing = type; analizing != null && analizing != typeof (object); analizing = analizing.BaseType)
        {
          foreach (FieldInfo fieldInfo in TypeExtensions.GetUserDeclaredFields(analizing))
            yield return (MemberInfo) fieldInfo;
        }
      }
    }

    private static IEnumerable<FieldInfo> GetUserDeclaredFields(Type type)
    {
      return ((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (x => !x.Name.StartsWith("<")));
    }
  }
}
