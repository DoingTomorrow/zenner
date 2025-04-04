// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.ReflectHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Properties;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  public static class ReflectHelper
  {
    public const BindingFlags AnyVisibilityInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ReflectHelper));
    private static readonly System.Type[] NoClasses = System.Type.EmptyTypes;
    private static readonly MethodInfo Exception_InternalPreserveStackTrace = typeof (Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

    public static bool OverridesEquals(System.Type clazz)
    {
      return ReflectHelper.OverrideMethod(clazz, "Equals", new System.Type[1]
      {
        typeof (object)
      });
    }

    private static bool OverrideMethod(System.Type clazz, string methodName, System.Type[] parametersTypes)
    {
      try
      {
        MethodInfo methodInfo = !clazz.IsInterface ? clazz.GetMethod(methodName, parametersTypes) : ReflectHelper.GetMethodFromInterface(clazz, methodName, parametersTypes);
        return methodInfo != null && !methodInfo.DeclaringType.Equals(typeof (object));
      }
      catch (AmbiguousMatchException ex)
      {
        return true;
      }
    }

    private static MethodInfo GetMethodFromInterface(
      System.Type type,
      string methodName,
      System.Type[] parametersTypes)
    {
      if (type == null)
        return (MethodInfo) null;
      MethodInfo methodFromInterface = type.GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public, (Binder) null, parametersTypes, (ParameterModifier[]) null);
      if (methodFromInterface == null)
      {
        foreach (System.Type type1 in type.GetInterfaces())
        {
          methodFromInterface = ReflectHelper.GetMethodFromInterface(type1, methodName, parametersTypes);
          if (methodFromInterface != null)
            return methodFromInterface;
        }
      }
      return methodFromInterface;
    }

    public static bool OverridesGetHashCode(System.Type clazz)
    {
      return ReflectHelper.OverrideMethod(clazz, "GetHashCode", System.Type.EmptyTypes);
    }

    public static IGetter GetGetter(
      System.Type theClass,
      string propertyName,
      string propertyAccessorName)
    {
      return PropertyAccessorFactory.GetPropertyAccessor(propertyAccessorName).GetGetter(theClass, propertyName);
    }

    public static IType ReflectedPropertyType(System.Type theClass, string name, string access)
    {
      System.Type type1 = ReflectHelper.ReflectedPropertyClass(theClass, name, access);
      System.Type type2 = type1;
      if (type1.IsGenericType && type1.GetGenericTypeDefinition().Equals(typeof (Nullable<>)))
        type2 = type1.GetGenericArguments()[0];
      return TypeFactory.HeuristicType(type2.AssemblyQualifiedName);
    }

    public static System.Type ReflectedPropertyClass(System.Type theClass, string name, string access)
    {
      return ReflectHelper.GetGetter(theClass, name, access).ReturnType;
    }

    public static System.Type ReflectedPropertyClass(
      string className,
      string name,
      string accessorName)
    {
      try
      {
        return ReflectHelper.GetGetter(ReflectHelper.ClassForName(className), name, accessorName).ReturnType;
      }
      catch (Exception ex)
      {
        throw new MappingException(string.Format("class {0} not found while looking for property: {1}", (object) className, (object) name), ex);
      }
    }

    public static System.Type ClassForName(string name)
    {
      return ReflectHelper.TypeFromAssembly(TypeNameParser.Parse(name), true);
    }

    public static System.Type ClassForFullName(string classFullName)
    {
      return ReflectHelper.ClassForFullNameOrNull(classFullName) ?? throw new TypeLoadException("Could not load type " + classFullName + ". Possible cause: the assembly was not loaded or not specified.");
    }

    public static System.Type ClassForFullNameOrNull(string classFullName)
    {
      System.Type type = (System.Type) null;
      AssemblyQualifiedTypeName name = TypeNameParser.Parse(classFullName);
      if (!string.IsNullOrEmpty(name.Assembly))
        type = ReflectHelper.TypeFromAssembly(name, false);
      else if (!string.IsNullOrEmpty(classFullName))
      {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          type = assembly.GetType(classFullName, false, false);
          if (type != null)
            break;
        }
      }
      return type;
    }

    public static System.Type TypeFromAssembly(string type, string assembly, bool throwIfError)
    {
      return ReflectHelper.TypeFromAssembly(new AssemblyQualifiedTypeName(type, assembly), throwIfError);
    }

    public static System.Type TypeFromAssembly(AssemblyQualifiedTypeName name, bool throwOnError)
    {
      try
      {
        System.Type type1 = System.Type.GetType(name.ToString());
        if (type1 != null)
          return type1;
        if (name.Assembly == null)
        {
          string message = "Could not load type " + (object) name + ". Possible cause: no assembly name specified.";
          ReflectHelper.log.Warn((object) message);
          if (throwOnError)
            throw new TypeLoadException(message);
          return (System.Type) null;
        }
        Assembly assembly = Assembly.Load(name.Assembly);
        if (assembly == null)
        {
          ReflectHelper.log.Warn((object) ("Could not load type " + (object) name + ". Possible cause: incorrect assembly name specified."));
          return (System.Type) null;
        }
        System.Type type2 = assembly.GetType(name.Type, throwOnError);
        if (type2 != null)
          return type2;
        ReflectHelper.log.Warn((object) ("Could not load type " + (object) name + "."));
        return (System.Type) null;
      }
      catch (Exception ex)
      {
        if (ReflectHelper.log.IsErrorEnabled)
          ReflectHelper.log.Error((object) ("Could not load type " + (object) name + "."), ex);
        if (!throwOnError)
          return (System.Type) null;
        throw;
      }
    }

    public static bool TryLoadAssembly(string assemblyName)
    {
      if (string.IsNullOrEmpty(assemblyName))
        return false;
      bool flag = true;
      try
      {
        Assembly.Load(assemblyName);
      }
      catch (Exception ex)
      {
        flag = false;
      }
      return flag;
    }

    public static object GetConstantValue(System.Type type, string fieldName)
    {
      try
      {
        return type.GetField(fieldName)?.GetValue((object) null);
      }
      catch
      {
        return (object) null;
      }
    }

    public static ConstructorInfo GetDefaultConstructor(System.Type type)
    {
      if (ReflectHelper.IsAbstractClass(type))
        return (ConstructorInfo) null;
      try
      {
        return type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, CallingConventions.HasThis, ReflectHelper.NoClasses, (ParameterModifier[]) null);
      }
      catch (Exception ex)
      {
        throw new InstantiationException("A default (no-arg) constructor could not be found for: ", ex, type);
      }
    }

    public static ConstructorInfo GetConstructor(System.Type type, IType[] types)
    {
      foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length == types.Length)
        {
          bool flag = true;
          for (int index = 0; index < parameters.Length; ++index)
          {
            if (!parameters[index].ParameterType.IsAssignableFrom(types[index].ReturnedClass))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return constructor;
        }
      }
      throw new InstantiationException(ReflectHelper.FormatConstructorNotFoundMessage((IEnumerable<IType>) types), (Exception) null, type);
    }

    private static string FormatConstructorNotFoundMessage(IEnumerable<IType> types)
    {
      StringBuilder stringBuilder = new StringBuilder("no constructor compatible with (");
      bool flag = true;
      foreach (IType type in types)
      {
        if (!flag)
          stringBuilder.Append(", ");
        flag = false;
        stringBuilder.Append((object) type.ReturnedClass);
      }
      stringBuilder.Append(") found in class: ");
      return stringBuilder.ToString();
    }

    public static bool IsAbstractClass(System.Type type) => type.IsAbstract || type.IsInterface;

    public static bool IsFinalClass(System.Type type) => type.IsSealed;

    public static Exception UnwrapTargetInvocationException(TargetInvocationException ex)
    {
      ReflectHelper.Exception_InternalPreserveStackTrace.Invoke((object) ex.InnerException, new object[0]);
      return ex.InnerException;
    }

    public static MethodInfo TryGetMethod(System.Type type, MethodInfo method)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (method == null)
        return (MethodInfo) null;
      System.Type[] methodSignature = ReflectHelper.GetMethodSignature(method);
      return ReflectHelper.SafeGetMethod(type, method, methodSignature);
    }

    private static System.Type[] GetMethodSignature(MethodInfo method)
    {
      ParameterInfo[] parameters = method.GetParameters();
      System.Type[] methodSignature = new System.Type[parameters.Length];
      for (int index = 0; index < parameters.Length; ++index)
        methodSignature[index] = parameters[index].ParameterType;
      return methodSignature;
    }

    private static MethodInfo SafeGetMethod(System.Type type, MethodInfo method, System.Type[] tps)
    {
      List<System.Type> typeList = new List<System.Type>();
      MethodInfo method1 = (MethodInfo) null;
      try
      {
        typeList.Add(type);
        if (type.IsInterface)
        {
          System.Type[] interfaces = type.GetInterfaces();
          typeList.AddRange((IEnumerable<System.Type>) interfaces);
        }
        foreach (System.Type type1 in typeList)
        {
          MethodInfo method2 = type1.GetMethod(method.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, tps, (ParameterModifier[]) null);
          if (method2 != null)
          {
            method1 = method2;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return method1;
    }

    internal static object GetConstantValue(string qualifiedName)
    {
      return ReflectHelper.GetConstantValue(qualifiedName, (ISessionFactoryImplementor) null);
    }

    internal static object GetConstantValue(string qualifiedName, ISessionFactoryImplementor sfi)
    {
      string str = StringHelper.Qualifier(qualifiedName);
      if (!string.IsNullOrEmpty(str))
      {
        System.Type type = System.Type.GetType(str);
        if (type == null && sfi != null)
          type = System.Type.GetType(sfi.GetImportedClassName(str));
        if (type != null)
          return ReflectHelper.GetConstantValue(type, StringHelper.Unqualify(qualifiedName));
      }
      return (object) null;
    }

    public static MethodInfo GetGenericMethodFrom<T>(
      string methodName,
      System.Type[] genericArgs,
      System.Type[] signature)
    {
      MethodInfo genericMethodFrom = (MethodInfo) null;
      foreach (MethodInfo method in typeof (T).GetMethods())
      {
        if (method.Name.Equals(methodName) && method.IsGenericMethod && signature.Length == method.GetParameters().Length && method.GetGenericArguments().Length == genericArgs.Length)
        {
          bool flag = true;
          genericMethodFrom = method.MakeGenericMethod(genericArgs);
          ParameterInfo[] parameters = genericMethodFrom.GetParameters();
          for (int index = 0; index < signature.Length; ++index)
          {
            if (parameters[index].ParameterType != signature[index])
              flag = false;
          }
          if (flag)
            return genericMethodFrom;
        }
      }
      return genericMethodFrom;
    }

    public static IDictionary<string, string> ToTypeParameters(this object source)
    {
      if (source == null)
        return (IDictionary<string, string>) new Dictionary<string, string>(1);
      PropertyInfo[] properties = source.GetType().GetProperties();
      if (properties.Length == 0)
        return (IDictionary<string, string>) new Dictionary<string, string>(1);
      Dictionary<string, string> typeParameters = new Dictionary<string, string>(properties.Length);
      foreach (PropertyInfo propertyInfo in properties)
      {
        object objB = propertyInfo.GetValue(source, (object[]) null);
        if (!object.ReferenceEquals((object) null, objB))
          typeParameters[propertyInfo.Name] = objB.ToString();
      }
      return (IDictionary<string, string>) typeParameters;
    }

    public static bool IsPropertyGet(MethodInfo method)
    {
      return method.IsSpecialName && method.Name.StartsWith("get_");
    }

    public static bool IsPropertySet(MethodInfo method)
    {
      return method.IsSpecialName && method.Name.StartsWith("set_");
    }

    public static string GetPropertyName(MethodInfo method) => method.Name.Substring(4);

    public static System.Type GetCollectionElementType(this IEnumerable collectionInstance)
    {
      return collectionInstance != null ? ReflectHelper.GetCollectionElementType(collectionInstance.GetType()) : throw new ArgumentNullException(nameof (collectionInstance));
    }

    public static System.Type GetCollectionElementType(System.Type collectionType)
    {
      if (collectionType == null)
        throw new ArgumentNullException(nameof (collectionType));
      if (collectionType.IsArray)
        return collectionType.GetElementType();
      if (collectionType.IsGenericType)
      {
        List<System.Type> list = ((IEnumerable<System.Type>) collectionType.GetInterfaces()).Where<System.Type>((Func<System.Type, bool>) (t => t.IsGenericType)).ToList<System.Type>();
        if (collectionType.IsInterface)
          list.Add(collectionType);
        System.Type type = list.FirstOrDefault<System.Type>((Func<System.Type, bool>) (t => t.GetGenericTypeDefinition() == typeof (IEnumerable<>)));
        if (type != null)
          return type.GetGenericArguments()[0];
      }
      return (System.Type) null;
    }

    public static bool HasProperty(this System.Type source, string propertyName)
    {
      if (source == typeof (object) || source == null || string.IsNullOrEmpty(propertyName))
        return false;
      return source.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null || source.BaseType.HasProperty(propertyName) || ((IEnumerable<System.Type>) source.GetInterfaces()).Any<System.Type>((Func<System.Type, bool>) (@interface => @interface.HasProperty(propertyName)));
    }

    public static bool IsMethodOf(this MethodInfo source, System.Type realDeclaringType)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (realDeclaringType == null)
        throw new ArgumentNullException(nameof (realDeclaringType));
      System.Type methodDeclaringType = source.DeclaringType;
      if (realDeclaringType.Equals(methodDeclaringType) || methodDeclaringType.IsGenericType && !methodDeclaringType.IsGenericTypeDefinition && realDeclaringType.Equals(methodDeclaringType.GetGenericTypeDefinition()))
        return true;
      if (realDeclaringType.IsInterface)
      {
        System.Type[] interfaces = methodDeclaringType.GetInterfaces();
        if (((IEnumerable<System.Type>) interfaces).Contains<System.Type>(realDeclaringType) && ((IEnumerable<MethodInfo>) methodDeclaringType.GetInterfaceMap(realDeclaringType).TargetMethods).Contains<MethodInfo>(source) || realDeclaringType.IsGenericTypeDefinition && ((IEnumerable<System.Type>) interfaces).Where<System.Type>((Func<System.Type, bool>) (t => t.IsGenericType && t.GetGenericTypeDefinition().Equals(realDeclaringType))).Select<System.Type, InterfaceMapping>((Func<System.Type, InterfaceMapping>) (implementedGenericInterface => methodDeclaringType.GetInterfaceMap(implementedGenericInterface))).Any<InterfaceMapping>((Func<InterfaceMapping, bool>) (methodsMap => ((IEnumerable<MethodInfo>) methodsMap.TargetMethods).Contains<MethodInfo>(source))))
          return true;
      }
      return false;
    }
  }
}
