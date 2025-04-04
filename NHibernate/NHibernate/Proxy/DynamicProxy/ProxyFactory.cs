// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.ProxyFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public sealed class ProxyFactory
  {
    private static readonly ConstructorInfo defaultBaseConstructor = typeof (object).GetConstructor(new Type[0]);
    private static readonly MethodInfo getTypeFromHandle = typeof (Type).GetMethod("GetTypeFromHandle");
    private static readonly MethodInfo getValue = typeof (SerializationInfo).GetMethod("GetValue", BindingFlags.Instance | BindingFlags.Public, (Binder) null, new Type[2]
    {
      typeof (string),
      typeof (Type)
    }, (ParameterModifier[]) null);
    private static readonly MethodInfo setType = typeof (SerializationInfo).GetMethod("SetType", BindingFlags.Instance | BindingFlags.Public, (Binder) null, new Type[1]
    {
      typeof (Type)
    }, (ParameterModifier[]) null);
    private static readonly MethodInfo addValue = typeof (SerializationInfo).GetMethod("AddValue", BindingFlags.Instance | BindingFlags.Public, (Binder) null, new Type[2]
    {
      typeof (string),
      typeof (object)
    }, (ParameterModifier[]) null);

    public ProxyFactory()
      : this((IProxyMethodBuilder) new DefaultyProxyMethodBuilder())
    {
    }

    public ProxyFactory(IProxyAssemblyBuilder proxyAssemblyBuilder)
      : this((IProxyMethodBuilder) new DefaultyProxyMethodBuilder(), proxyAssemblyBuilder)
    {
    }

    public ProxyFactory(IProxyMethodBuilder proxyMethodBuilder)
      : this((IProxyMethodBuilder) new DefaultyProxyMethodBuilder(), (IProxyAssemblyBuilder) new DefaultProxyAssemblyBuilder())
    {
    }

    public ProxyFactory(
      IProxyMethodBuilder proxyMethodBuilder,
      IProxyAssemblyBuilder proxyAssemblyBuilder)
    {
      this.ProxyMethodBuilder = proxyMethodBuilder != null ? proxyMethodBuilder : throw new ArgumentNullException(nameof (proxyMethodBuilder));
      this.ProxyAssemblyBuilder = proxyAssemblyBuilder;
      this.Cache = (IProxyCache) new ProxyCache();
    }

    public IProxyCache Cache { get; private set; }

    public IProxyMethodBuilder ProxyMethodBuilder { get; private set; }

    public IProxyAssemblyBuilder ProxyAssemblyBuilder { get; private set; }

    public object CreateProxy(
      Type instanceType,
      IInterceptor interceptor,
      params Type[] baseInterfaces)
    {
      object instance = Activator.CreateInstance(this.CreateProxyType(instanceType, baseInterfaces));
      ((IProxy) instance).Interceptor = interceptor;
      return instance;
    }

    public Type CreateProxyType(Type baseType, params Type[] interfaces)
    {
      Type[] baseInterfaces = object.ReferenceEquals((object) null, (object) interfaces) ? new Type[0] : ((IEnumerable<Type>) interfaces).Where<Type>((Func<Type, bool>) (t => t != null)).ToArray<Type>();
      Type proxyType;
      if (this.Cache.TryGetProxyType(baseType, baseInterfaces, out proxyType))
        return proxyType;
      lock (this.Cache)
      {
        if (!this.Cache.TryGetProxyType(baseType, baseInterfaces, out proxyType))
        {
          proxyType = this.CreateUncachedProxyType(baseType, baseInterfaces);
          if (proxyType != null && this.Cache != null)
            this.Cache.StoreProxyType(proxyType, baseType, baseInterfaces);
        }
        return proxyType;
      }
    }

    private Type CreateUncachedProxyType(Type baseType, Type[] baseInterfaces)
    {
      AppDomain currentDomain = AppDomain.CurrentDomain;
      string name1 = string.Format("{0}Proxy", (object) baseType.Name);
      string assemblyName = string.Format("{0}Assembly", (object) name1);
      string moduleName = string.Format("{0}Module", (object) name1);
      AssemblyName name2 = new AssemblyName(assemblyName);
      AssemblyBuilder assemblyBuilder = this.ProxyAssemblyBuilder.DefineDynamicAssembly(currentDomain, name2);
      ModuleBuilder moduleBuilder = this.ProxyAssemblyBuilder.DefineDynamicModule(assemblyBuilder, moduleName);
      TypeAttributes attr = TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;
      HashSet<Type> typeSet = new HashSet<Type>();
      typeSet.Merge<Type>((IEnumerable<Type>) baseInterfaces);
      Type type1 = baseType;
      if (baseType.IsInterface)
      {
        type1 = typeof (ProxyDummy);
        typeSet.Add(baseType);
      }
      foreach (Type currentType in typeSet.ToArray<Type>())
        typeSet.Merge<Type>(this.GetInterfaces(currentType));
      typeSet.Add(typeof (ISerializable));
      TypeBuilder typeBuilder = moduleBuilder.DefineType(name1, attr, type1, typeSet.ToArray<Type>());
      ConstructorBuilder defaultConstructor = ProxyFactory.DefineConstructor(typeBuilder, type1);
      ProxyImplementor proxyImplementor = new ProxyImplementor();
      proxyImplementor.ImplementProxy(typeBuilder);
      FieldInfo interceptorField = (FieldInfo) proxyImplementor.InterceptorField;
      foreach (MethodInfo method in this.GetProxiableMethods(baseType, (IEnumerable<Type>) typeSet).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.DeclaringType != typeof (ISerializable))))
        this.ProxyMethodBuilder.CreateProxiedMethod(interceptorField, method, typeBuilder);
      ProxyFactory.AddSerializationSupport(baseType, baseInterfaces, typeBuilder, interceptorField, defaultConstructor);
      Type type2 = typeBuilder.CreateType();
      this.ProxyAssemblyBuilder.Save(assemblyBuilder);
      return type2;
    }

    private IEnumerable<Type> GetInterfaces(Type currentType) => this.GetAllInterfaces(currentType);

    private IEnumerable<Type> GetAllInterfaces(Type currentType)
    {
      Type[] interfaces = currentType.GetInterfaces();
      foreach (Type currentType1 in interfaces)
      {
        yield return currentType1;
        IEnumerator<Type> enumerator = this.GetAllInterfaces(currentType1).GetEnumerator();
        while (enumerator.MoveNext())
        {
          Type @interface = enumerator.Current;
          yield return @interface;
        }
      }
    }

    private IEnumerable<MethodInfo> GetProxiableMethods(Type type, IEnumerable<Type> interfaces)
    {
      return ((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.IsProxiable())).Concat<MethodInfo>(interfaces.SelectMany<Type, MethodInfo>((Func<Type, IEnumerable<MethodInfo>>) (interfaceType => (IEnumerable<MethodInfo>) interfaceType.GetMethods()))).Distinct<MethodInfo>();
    }

    private static ConstructorBuilder DefineConstructor(TypeBuilder typeBuilder, Type parentType)
    {
      ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[0]);
      ConstructorInfo con = parentType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[0], (ParameterModifier[]) null);
      if (con == null || con.IsPrivate || con.IsAssembly)
        con = ProxyFactory.defaultBaseConstructor;
      ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
      constructorBuilder.SetImplementationFlags(MethodImplAttributes.IL);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Call, con);
      ilGenerator.Emit(OpCodes.Ret);
      return constructorBuilder;
    }

    private static void ImplementGetObjectData(
      Type baseType,
      Type[] baseInterfaces,
      TypeBuilder typeBuilder,
      FieldInfo interceptorField)
    {
      Type[] parameterTypes = new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      };
      ILGenerator ilGenerator = typeBuilder.DefineMethod("GetObjectData", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, typeof (void), parameterTypes).GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldtoken, typeof (ProxyObjectReference));
      ilGenerator.Emit(OpCodes.Call, ProxyFactory.getTypeFromHandle);
      ilGenerator.Emit(OpCodes.Callvirt, ProxyFactory.setType);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldstr, "__interceptor");
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldfld, interceptorField);
      ilGenerator.Emit(OpCodes.Callvirt, ProxyFactory.addValue);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldstr, "__baseType");
      ilGenerator.Emit(OpCodes.Ldstr, baseType.AssemblyQualifiedName);
      ilGenerator.Emit(OpCodes.Callvirt, ProxyFactory.addValue);
      int length = baseInterfaces.Length;
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldstr, "__baseInterfaceCount");
      ilGenerator.Emit(OpCodes.Ldc_I4, length);
      ilGenerator.Emit(OpCodes.Box, typeof (int));
      ilGenerator.Emit(OpCodes.Callvirt, ProxyFactory.addValue);
      int num = 0;
      foreach (Type baseInterface in baseInterfaces)
      {
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldstr, string.Format("__baseInterface{0}", (object) num++));
        ilGenerator.Emit(OpCodes.Ldstr, baseInterface.AssemblyQualifiedName);
        ilGenerator.Emit(OpCodes.Callvirt, ProxyFactory.addValue);
      }
      ilGenerator.Emit(OpCodes.Ret);
    }

    private static void DefineSerializationConstructor(
      TypeBuilder typeBuilder,
      FieldInfo interceptorField,
      ConstructorBuilder defaultConstructor)
    {
      Type[] parameterTypes = new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      };
      ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, parameterTypes);
      ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
      LocalBuilder local = ilGenerator.DeclareLocal(typeof (Type));
      constructorBuilder.SetImplementationFlags(MethodImplAttributes.IL);
      ilGenerator.Emit(OpCodes.Ldtoken, typeof (IInterceptor));
      ilGenerator.Emit(OpCodes.Call, ProxyFactory.getTypeFromHandle);
      ilGenerator.Emit(OpCodes.Stloc, local);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Call, (ConstructorInfo) defaultConstructor);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldstr, "__interceptor");
      ilGenerator.Emit(OpCodes.Ldloc, local);
      ilGenerator.Emit(OpCodes.Callvirt, ProxyFactory.getValue);
      ilGenerator.Emit(OpCodes.Castclass, typeof (IInterceptor));
      ilGenerator.Emit(OpCodes.Stfld, interceptorField);
      ilGenerator.Emit(OpCodes.Ret);
    }

    private static void AddSerializationSupport(
      Type baseType,
      Type[] baseInterfaces,
      TypeBuilder typeBuilder,
      FieldInfo interceptorField,
      ConstructorBuilder defaultConstructor)
    {
      CustomAttributeBuilder customBuilder = new CustomAttributeBuilder(typeof (SerializableAttribute).GetConstructor(new Type[0]), new object[0]);
      typeBuilder.SetCustomAttribute(customBuilder);
      ProxyFactory.DefineSerializationConstructor(typeBuilder, interceptorField, defaultConstructor);
      ProxyFactory.ImplementGetObjectData(baseType, baseInterfaces, typeBuilder, interceptorField);
    }
  }
}
