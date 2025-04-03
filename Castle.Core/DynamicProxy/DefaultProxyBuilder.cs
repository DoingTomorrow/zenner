// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.DefaultProxyBuilder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Logging;
using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;

#nullable disable
namespace Castle.DynamicProxy
{
  public class DefaultProxyBuilder : IProxyBuilder
  {
    private readonly ModuleScope scope;
    private ILogger logger = (ILogger) NullLogger.Instance;

    public DefaultProxyBuilder()
      : this(new ModuleScope())
    {
    }

    public DefaultProxyBuilder(ModuleScope scope) => this.scope = scope;

    public ILogger Logger
    {
      get => this.logger;
      set => this.logger = value;
    }

    public ModuleScope ModuleScope => this.scope;

    [Obsolete("Use CreateClassProxyType method instead.")]
    public Type CreateClassProxy(Type classToProxy, ProxyGenerationOptions options)
    {
      return this.CreateClassProxyType(classToProxy, Type.EmptyTypes, options);
    }

    [Obsolete("Use CreateClassProxyType method instead.")]
    public Type CreateClassProxy(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      return this.CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options);
    }

    public Type CreateClassProxyType(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      this.AssertValidType(classToProxy);
      this.AssertValidTypes((IEnumerable<Type>) additionalInterfacesToProxy);
      ClassProxyGenerator classProxyGenerator = new ClassProxyGenerator(this.scope, classToProxy);
      classProxyGenerator.Logger = this.logger;
      return classProxyGenerator.GenerateCode(additionalInterfacesToProxy, options);
    }

    public Type CreateClassProxyTypeWithTarget(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      this.AssertValidType(classToProxy);
      this.AssertValidTypes((IEnumerable<Type>) additionalInterfacesToProxy);
      ClassProxyWithTargetGenerator withTargetGenerator = new ClassProxyWithTargetGenerator(this.scope, classToProxy, additionalInterfacesToProxy, options);
      withTargetGenerator.Logger = this.logger;
      return withTargetGenerator.GetGeneratedType();
    }

    public Type CreateInterfaceProxyTypeWithTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      Type targetType,
      ProxyGenerationOptions options)
    {
      this.AssertValidType(interfaceToProxy);
      this.AssertValidTypes((IEnumerable<Type>) additionalInterfacesToProxy);
      InterfaceProxyWithTargetGenerator withTargetGenerator = new InterfaceProxyWithTargetGenerator(this.scope, interfaceToProxy);
      withTargetGenerator.Logger = this.logger;
      return withTargetGenerator.GenerateCode(targetType, additionalInterfacesToProxy, options);
    }

    public Type CreateInterfaceProxyTypeWithTargetInterface(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      this.AssertValidType(interfaceToProxy);
      this.AssertValidTypes((IEnumerable<Type>) additionalInterfacesToProxy);
      InterfaceProxyWithTargetInterfaceGenerator interfaceGenerator = new InterfaceProxyWithTargetInterfaceGenerator(this.scope, interfaceToProxy);
      interfaceGenerator.Logger = this.logger;
      return interfaceGenerator.GenerateCode(interfaceToProxy, additionalInterfacesToProxy, options);
    }

    public Type CreateInterfaceProxyTypeWithoutTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      this.AssertValidType(interfaceToProxy);
      this.AssertValidTypes((IEnumerable<Type>) additionalInterfacesToProxy);
      InterfaceProxyWithoutTargetGenerator withoutTargetGenerator = new InterfaceProxyWithoutTargetGenerator(this.scope, interfaceToProxy);
      withoutTargetGenerator.Logger = this.logger;
      return withoutTargetGenerator.GenerateCode(typeof (object), additionalInterfacesToProxy, options);
    }

    private void AssertValidType(Type target)
    {
      if (target.IsGenericTypeDefinition)
        throw new GeneratorException("Type " + target.FullName + " is a generic type definition. Can not create proxy for open generic types.");
      if (!this.IsPublic(target) && !this.IsAccessible(target))
        throw new GeneratorException("Type " + target.FullName + " is not visible to DynamicProxy. Can not create proxy for types that are not accessible. Make the type public, or internal and mark your assembly with [assembly: InternalsVisibleTo(InternalsVisible.ToDynamicProxyGenAssembly2)] attribute.");
    }

    private void AssertValidTypes(IEnumerable<Type> targetTypes)
    {
      if (targetTypes == null)
        return;
      foreach (Type targetType in targetTypes)
        this.AssertValidType(targetType);
    }

    private bool IsAccessible(Type target)
    {
      bool isNested = target.IsNested;
      bool flag = isNested && (target.IsNestedAssembly || target.IsNestedFamORAssem);
      return (!target.IsVisible && !isNested || flag) && InternalsHelper.IsInternalToDynamicProxy(target.Assembly);
    }

    private bool IsPublic(Type target) => target.IsPublic || target.IsNestedPublic;
  }
}
