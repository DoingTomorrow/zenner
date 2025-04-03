// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.BaseProxyGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using Castle.Core.Logging;
using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public abstract class BaseProxyGenerator
  {
    private readonly ModuleScope scope;
    protected readonly Type targetType;
    private ILogger logger = (ILogger) NullLogger.Instance;
    private ProxyGenerationOptions proxyGenerationOptions;

    protected BaseProxyGenerator(ModuleScope scope, Type targetType)
    {
      this.scope = scope;
      this.targetType = targetType;
    }

    public ILogger Logger
    {
      get => this.logger;
      set => this.logger = value;
    }

    protected ProxyGenerationOptions ProxyGenerationOptions
    {
      get
      {
        return this.proxyGenerationOptions != null ? this.proxyGenerationOptions : throw new InvalidOperationException("ProxyGenerationOptions must be set before being retrieved.");
      }
      set
      {
        this.proxyGenerationOptions = this.proxyGenerationOptions == null ? value : throw new InvalidOperationException("ProxyGenerationOptions can only be set once.");
      }
    }

    protected ModuleScope Scope => this.scope;

    protected FieldReference CreateOptionsField(ClassEmitter emitter)
    {
      return emitter.CreateStaticField("proxyGenerationOptions", typeof (ProxyGenerationOptions));
    }

    protected void InitializeStaticFields(Type builtType)
    {
      builtType.SetStaticField("proxyGenerationOptions", BindingFlags.Public, (object) this.ProxyGenerationOptions);
    }

    protected void CheckNotGenericTypeDefinition(Type type, string argumentName)
    {
      if (type != null && type.IsGenericTypeDefinition)
        throw new ArgumentException("Type cannot be a generic type definition. Type: " + type.FullName, argumentName);
    }

    protected void CheckNotGenericTypeDefinitions(IEnumerable<Type> types, string argumentName)
    {
      if (types == null)
        return;
      foreach (Type type in types)
        this.CheckNotGenericTypeDefinition(type, argumentName);
    }

    protected virtual ClassEmitter BuildClassEmitter(
      string typeName,
      Type parentType,
      IEnumerable<Type> interfaces)
    {
      this.CheckNotGenericTypeDefinition(parentType, nameof (parentType));
      this.CheckNotGenericTypeDefinitions(interfaces, nameof (interfaces));
      return new ClassEmitter(this.Scope, typeName, parentType, interfaces);
    }

    protected void GenerateConstructor(
      ClassEmitter emitter,
      ConstructorInfo baseConstructor,
      params FieldReference[] fields)
    {
      ParameterInfo[] source = (ParameterInfo[]) null;
      if (baseConstructor != null)
        source = baseConstructor.GetParameters();
      ArgumentReference[] sourceArray;
      if (source != null && source.Length != 0)
      {
        sourceArray = new ArgumentReference[fields.Length + source.Length];
        int length = fields.Length;
        for (int index = length; index < length + source.Length; ++index)
        {
          ParameterInfo parameterInfo = source[index - length];
          sourceArray[index] = new ArgumentReference(parameterInfo.ParameterType);
        }
      }
      else
        sourceArray = new ArgumentReference[fields.Length];
      for (int index = 0; index < fields.Length; ++index)
        sourceArray[index] = new ArgumentReference(fields[index].Reference.FieldType);
      ConstructorEmitter constructor = emitter.CreateConstructor(sourceArray);
      if (source != null && source.Length != 0)
      {
        ParameterInfo member = ((IEnumerable<ParameterInfo>) source).Last<ParameterInfo>();
        if (member.ParameterType.IsArray && member.HasAttribute<ParamArrayAttribute>())
          constructor.ConstructorBuilder.DefineParameter(sourceArray.Length, ParameterAttributes.None, member.Name).SetCustomAttribute(AttributeUtil.CreateBuilder<ParamArrayAttribute>());
      }
      for (int index = 0; index < fields.Length; ++index)
        constructor.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) fields[index], sourceArray[index].ToExpression()));
      if (baseConstructor != null)
      {
        ArgumentReference[] destinationArray = new ArgumentReference[source.Length];
        Array.Copy((Array) sourceArray, fields.Length, (Array) destinationArray, 0, source.Length);
        constructor.CodeBuilder.InvokeBaseConstructor(baseConstructor, destinationArray);
      }
      else
        constructor.CodeBuilder.InvokeBaseConstructor();
      constructor.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    protected void GenerateParameterlessConstructor(
      ClassEmitter emitter,
      Type baseClass,
      FieldReference interceptorField)
    {
      ConstructorInfo constructor1 = baseClass.GetConstructor(BindingFlags.Instance | BindingFlags.Public, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
      if (constructor1 == null)
      {
        constructor1 = baseClass.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
        if (constructor1 == null || constructor1.IsPrivate)
          return;
      }
      ConstructorEmitter constructor2 = emitter.CreateConstructor();
      constructor2.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) interceptorField, (Expression) new NewArrayExpression(1, typeof (IInterceptor))));
      constructor2.CodeBuilder.AddStatement((Statement) new AssignArrayStatement((Reference) interceptorField, 0, (Expression) new NewInstanceExpression(typeof (StandardInterceptor), new Type[0], new Expression[0])));
      constructor2.CodeBuilder.InvokeBaseConstructor(constructor1);
      constructor2.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    protected void EnsureOptionsOverrideEqualsAndGetHashCode(ProxyGenerationOptions options)
    {
      if (!this.Logger.IsWarnEnabled || this.OverridesEqualsAndGetHashCode(options.Hook.GetType()))
        return;
      this.Logger.Warn("The IProxyGenerationHook type {0} does not override both Equals and GetHashCode. If these are not correctly overridden caching will fail to work causing performance problems.", (object) options.Hook.GetType().FullName);
    }

    private bool OverridesEqualsAndGetHashCode(Type type)
    {
      MethodInfo method1 = type.GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public);
      if (method1 == null || method1.DeclaringType == typeof (object) || method1.IsAbstract)
        return false;
      MethodInfo method2 = type.GetMethod("GetHashCode", BindingFlags.Instance | BindingFlags.Public);
      return method2 != null && method2.DeclaringType != typeof (object) && !method2.IsAbstract;
    }

    protected void AddMapping(
      Type @interface,
      ITypeContributor implementer,
      IDictionary<Type, ITypeContributor> mapping)
    {
      if (mapping.ContainsKey(@interface))
        return;
      this.AddMappingNoCheck(@interface, implementer, mapping);
    }

    protected void AddMappingNoCheck(
      Type @interface,
      ITypeContributor implementer,
      IDictionary<Type, ITypeContributor> mapping)
    {
      mapping.Add(@interface, implementer);
    }

    protected void AddMappingForISerializable(
      IDictionary<Type, ITypeContributor> typeImplementerMapping,
      ITypeContributor instance)
    {
      this.AddMapping(typeof (ISerializable), instance, typeImplementerMapping);
    }

    protected void HandleExplicitlyPassedProxyTargetAccessor(
      ICollection<Type> targetInterfaces,
      ICollection<Type> additionalInterfaces)
    {
      string str = typeof (IProxyTargetAccessor).ToString();
      throw new ProxyGenerationException("This is a DynamicProxy2 error: " + (!targetInterfaces.Contains(typeof (IProxyTargetAccessor)) ? (!this.ProxyGenerationOptions.MixinData.ContainsMixin(typeof (IProxyTargetAccessor)) ? (!additionalInterfaces.Contains(typeof (IProxyTargetAccessor)) ? string.Format("It looks like we have a bug with regards to how we handle {0}. Please report it.", (object) str) : string.Format("You passed {0} as one of additional interfaces to proxy which is a DynamicProxy infrastructure interface and is implemented by every proxy anyway. Please remove it from the list of additional interfaces to proxy.", (object) str)) : string.Format("Mixin type {0} implements {1} which is a DynamicProxy infrastructure interface and you should never implement it yourself. Are you trying to mix in an existing proxy?", (object) this.ProxyGenerationOptions.MixinData.GetMixinInstance(typeof (IProxyTargetAccessor)).GetType().Name, (object) str)) : string.Format("Target type for the proxy implements {0} which is a DynamicProxy infrastructure interface and you should never implement it yourself. Are you trying to proxy an existing proxy?", (object) str)));
    }

    protected void CreateInterceptorsField(ClassEmitter emitter)
    {
      FieldReference field = emitter.CreateField("__interceptors", typeof (IInterceptor[]));
      emitter.DefineCustomAttributeFor<XmlIgnoreAttribute>(field);
    }

    protected void CreateSelectorField(ClassEmitter emitter)
    {
      if (this.ProxyGenerationOptions.Selector == null)
        return;
      emitter.CreateField("__selector", typeof (IInterceptorSelector));
    }

    protected virtual void CreateTypeAttributes(ClassEmitter emitter)
    {
      emitter.AddCustomAttributes(this.ProxyGenerationOptions);
      emitter.DefineCustomAttribute<XmlIncludeAttribute>(new object[1]
      {
        (object) this.targetType
      });
    }

    protected virtual void CreateFields(ClassEmitter emitter)
    {
      this.CreateOptionsField(emitter);
      this.CreateSelectorField(emitter);
      this.CreateInterceptorsField(emitter);
    }

    protected Type ObtainProxyType(CacheKey cacheKey, Func<string, INamingScope, Type> factory)
    {
      using (IUpgradeableLockHolder upgradeableLockHolder = this.Scope.Lock.ForReadingUpgradeable())
      {
        Type fromCache1 = this.GetFromCache(cacheKey);
        if (fromCache1 != null)
        {
          this.Logger.Debug("Found cached proxy type {0} for target type {1}.", (object) fromCache1.FullName, (object) this.targetType.FullName);
          return fromCache1;
        }
        upgradeableLockHolder.Upgrade();
        Type fromCache2 = this.GetFromCache(cacheKey);
        if (fromCache2 != null)
        {
          this.Logger.Debug("Found cached proxy type {0} for target type {1}.", (object) fromCache2.FullName, (object) this.targetType.FullName);
          return fromCache2;
        }
        this.Logger.Debug("No cached proxy type was found for target type {0}.", (object) this.targetType.FullName);
        this.EnsureOptionsOverrideEqualsAndGetHashCode(this.ProxyGenerationOptions);
        string uniqueName = this.Scope.NamingScope.GetUniqueName("Castle.Proxies." + this.targetType.Name + "Proxy");
        Type type = factory(uniqueName, this.Scope.NamingScope.SafeSubScope());
        this.AddToCache(cacheKey, type);
        return type;
      }
    }

    protected void GenerateConstructors(
      ClassEmitter emitter,
      Type baseType,
      params FieldReference[] fields)
    {
      foreach (ConstructorInfo constructor in baseType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (this.IsConstructorVisible(constructor))
          this.GenerateConstructor(emitter, constructor, fields);
      }
    }

    private bool IsConstructorVisible(ConstructorInfo constructor)
    {
      if (constructor.IsPublic || constructor.IsFamily || constructor.IsFamilyOrAssembly)
        return true;
      return constructor.IsAssembly && InternalsHelper.IsInternalToDynamicProxy(constructor.DeclaringType.Assembly);
    }

    protected ConstructorEmitter GenerateStaticConstructor(ClassEmitter emitter)
    {
      return emitter.CreateTypeConstructor();
    }

    protected void CompleteInitCacheMethod(ConstructorCodeBuilder constCodeBuilder)
    {
      constCodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    protected Type GetFromCache(CacheKey key) => this.scope.GetFromCache(key);

    protected void AddToCache(CacheKey key, Type type) => this.scope.RegisterInCache(key, type);
  }
}
