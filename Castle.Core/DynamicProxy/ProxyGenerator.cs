// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.ProxyGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace Castle.DynamicProxy
{
  [CLSCompliant(true)]
  public class ProxyGenerator
  {
    private ILogger logger = (ILogger) NullLogger.Instance;
    private readonly IProxyBuilder proxyBuilder;

    public ProxyGenerator(IProxyBuilder builder)
    {
      this.proxyBuilder = builder;
      if (!this.HasSecurityPermission())
        return;
      this.Logger = (ILogger) new TraceLogger("Castle.DynamicProxy", LoggerLevel.Warn);
    }

    private bool HasSecurityPermission()
    {
      return new SecurityPermission(SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy).IsGranted();
    }

    public ProxyGenerator()
      : this((IProxyBuilder) new DefaultProxyBuilder())
    {
    }

    public ILogger Logger
    {
      get => this.logger;
      set
      {
        this.logger = value;
        this.proxyBuilder.Logger = value;
      }
    }

    public IProxyBuilder ProxyBuilder => this.proxyBuilder;

    public TInterface CreateInterfaceProxyWithTarget<TInterface>(
      TInterface target,
      params IInterceptor[] interceptors)
      where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithTarget(typeof (TInterface), (object) target, ProxyGenerationOptions.Default, interceptors);
    }

    public TInterface CreateInterfaceProxyWithTarget<TInterface>(
      TInterface target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
      where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithTarget(typeof (TInterface), (object) target, options, interceptors);
    }

    public object CreateInterfaceProxyWithTarget(
      Type interfaceToProxy,
      object target,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithTarget(interfaceToProxy, target, ProxyGenerationOptions.Default, interceptors);
    }

    public object CreateInterfaceProxyWithTarget(
      Type interfaceToProxy,
      object target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithTarget(interfaceToProxy, (Type[]) null, target, options, interceptors);
    }

    public object CreateInterfaceProxyWithTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithTarget(interfaceToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, interceptors);
    }

    public virtual object CreateInterfaceProxyWithTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      if (interfaceToProxy == null)
        throw new ArgumentNullException(nameof (interfaceToProxy));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (interceptors == null)
        throw new ArgumentNullException(nameof (interceptors));
      if (!interfaceToProxy.IsInterface)
        throw new ArgumentException("Specified type is not an interface", nameof (interfaceToProxy));
      Type type = target.GetType();
      if (!interfaceToProxy.IsAssignableFrom(type))
        throw new ArgumentException("Target does not implement interface " + interfaceToProxy.FullName, nameof (target));
      this.CheckNotGenericTypeDefinition(interfaceToProxy, nameof (interfaceToProxy));
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) additionalInterfacesToProxy, nameof (additionalInterfacesToProxy));
      return Activator.CreateInstance(this.CreateInterfaceProxyTypeWithTarget(interfaceToProxy, additionalInterfacesToProxy, type, options), this.GetConstructorArguments(target, interceptors, options).ToArray());
    }

    protected List<object> GetConstructorArguments(
      object target,
      IInterceptor[] interceptors,
      ProxyGenerationOptions options)
    {
      List<object> constructorArguments = new List<object>(options.MixinData.Mixins)
      {
        (object) interceptors,
        target
      };
      if (options.Selector != null)
        constructorArguments.Add((object) options.Selector);
      return constructorArguments;
    }

    public object CreateInterfaceProxyWithTargetInterface(
      Type interfaceToProxy,
      object target,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, target, ProxyGenerationOptions.Default, interceptors);
    }

    public TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(
      TInterface target,
      params IInterceptor[] interceptors)
      where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithTargetInterface(typeof (TInterface), (object) target, ProxyGenerationOptions.Default, interceptors);
    }

    public TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(
      TInterface target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
      where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithTargetInterface(typeof (TInterface), (object) target, options, interceptors);
    }

    public object CreateInterfaceProxyWithTargetInterface(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, interceptors);
    }

    public object CreateInterfaceProxyWithTargetInterface(
      Type interfaceToProxy,
      object target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, (Type[]) null, target, options, interceptors);
    }

    public virtual object CreateInterfaceProxyWithTargetInterface(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      if (target != null && !interfaceToProxy.IsInstanceOfType(target))
        throw new ArgumentException("targetType");
      if (interfaceToProxy == null)
        throw new ArgumentNullException(nameof (interfaceToProxy));
      if (interceptors == null)
        throw new ArgumentNullException(nameof (interceptors));
      if (!interfaceToProxy.IsInterface)
        throw new ArgumentException("Specified type is not an interface", nameof (interfaceToProxy));
      bool flag = false;
      if (target != null && !interfaceToProxy.IsAssignableFrom(target.GetType()))
      {
        if (RemotingServices.IsTransparentProxy(target))
        {
          if (RemotingServices.GetRealProxy(target) is IRemotingTypeInfo realProxy)
          {
            if (!realProxy.CanCastTo(interfaceToProxy, target))
              throw new ArgumentException("Target does not implement interface " + interfaceToProxy.FullName, nameof (target));
            flag = true;
          }
        }
        else
        {
          if (!Marshal.IsComObject(target))
            throw new ArgumentException("Target does not implement interface " + interfaceToProxy.FullName, nameof (target));
          Guid guid = interfaceToProxy.GUID;
          if (guid != Guid.Empty)
          {
            IntPtr iunknownForObject = Marshal.GetIUnknownForObject(target);
            IntPtr ppv = IntPtr.Zero;
            if (Marshal.QueryInterface(iunknownForObject, ref guid, out ppv) == 0 && ppv == IntPtr.Zero)
              throw new ArgumentException("Target COM object does not implement interface " + interfaceToProxy.FullName, nameof (target));
          }
        }
      }
      this.CheckNotGenericTypeDefinition(interfaceToProxy, nameof (interfaceToProxy));
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) additionalInterfacesToProxy, nameof (additionalInterfacesToProxy));
      Type withTargetInterface = this.CreateInterfaceProxyTypeWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, options);
      List<object> constructorArguments = this.GetConstructorArguments(target, interceptors, options);
      return flag ? withTargetInterface.GetConstructors()[0].Invoke(constructorArguments.ToArray()) : Activator.CreateInstance(withTargetInterface, constructorArguments.ToArray());
    }

    public TInterface CreateInterfaceProxyWithoutTarget<TInterface>(IInterceptor interceptor) where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithoutTarget(typeof (TInterface), interceptor);
    }

    public TInterface CreateInterfaceProxyWithoutTarget<TInterface>(
      params IInterceptor[] interceptors)
      where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithoutTarget(typeof (TInterface), interceptors);
    }

    public TInterface CreateInterfaceProxyWithoutTarget<TInterface>(
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
      where TInterface : class
    {
      return (TInterface) this.CreateInterfaceProxyWithoutTarget(typeof (TInterface), Type.EmptyTypes, options, interceptors);
    }

    public object CreateInterfaceProxyWithoutTarget(Type interfaceToProxy, IInterceptor interceptor)
    {
      return this.CreateInterfaceProxyWithoutTarget(interfaceToProxy, Type.EmptyTypes, ProxyGenerationOptions.Default, interceptor);
    }

    public object CreateInterfaceProxyWithoutTarget(
      Type interfaceToProxy,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithoutTarget(interfaceToProxy, Type.EmptyTypes, ProxyGenerationOptions.Default, interceptors);
    }

    public object CreateInterfaceProxyWithoutTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, ProxyGenerationOptions.Default, interceptors);
    }

    public object CreateInterfaceProxyWithoutTarget(
      Type interfaceToProxy,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateInterfaceProxyWithoutTarget(interfaceToProxy, Type.EmptyTypes, options, interceptors);
    }

    public virtual object CreateInterfaceProxyWithoutTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      if (interfaceToProxy == null)
        throw new ArgumentNullException(nameof (interfaceToProxy));
      if (interceptors == null)
        throw new ArgumentNullException(nameof (interceptors));
      if (!interfaceToProxy.IsInterface)
        throw new ArgumentException("Specified type is not an interface", nameof (interfaceToProxy));
      this.CheckNotGenericTypeDefinition(interfaceToProxy, nameof (interfaceToProxy));
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) additionalInterfacesToProxy, nameof (additionalInterfacesToProxy));
      return Activator.CreateInstance(this.CreateInterfaceProxyTypeWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, options), this.GetConstructorArguments((object) null, interceptors, options).ToArray());
    }

    public TClass CreateClassProxyWithTarget<TClass>(
      TClass target,
      params IInterceptor[] interceptors)
      where TClass : class
    {
      return (TClass) this.CreateClassProxyWithTarget(typeof (TClass), Type.EmptyTypes, (object) target, ProxyGenerationOptions.Default, new object[0], interceptors);
    }

    public TClass CreateClassProxyWithTarget<TClass>(
      TClass target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
      where TClass : class
    {
      return (TClass) this.CreateClassProxyWithTarget(typeof (TClass), Type.EmptyTypes, (object) target, options, new object[0], interceptors);
    }

    public object CreateClassProxyWithTarget(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, new object[0], interceptors);
    }

    public object CreateClassProxyWithTarget(
      Type classToProxy,
      object target,
      ProxyGenerationOptions options,
      object[] constructorArguments,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, options, constructorArguments, interceptors);
    }

    public object CreateClassProxyWithTarget(
      Type classToProxy,
      object target,
      object[] constructorArguments,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, constructorArguments, interceptors);
    }

    public object CreateClassProxyWithTarget(
      Type classToProxy,
      object target,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, new object[0], interceptors);
    }

    public object CreateClassProxyWithTarget(
      Type classToProxy,
      object target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, options, new object[0], interceptors);
    }

    public object CreateClassProxyWithTarget(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, options, new object[0], interceptors);
    }

    public virtual object CreateClassProxyWithTarget(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      object target,
      ProxyGenerationOptions options,
      object[] constructorArguments,
      params IInterceptor[] interceptors)
    {
      if (classToProxy == null)
        throw new ArgumentNullException(nameof (classToProxy));
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      if (!classToProxy.IsClass)
        throw new ArgumentException("'classToProxy' must be a class", nameof (classToProxy));
      this.CheckNotGenericTypeDefinition(classToProxy, nameof (classToProxy));
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) additionalInterfacesToProxy, nameof (additionalInterfacesToProxy));
      Type proxyTypeWithTarget = this.CreateClassProxyTypeWithTarget(classToProxy, additionalInterfacesToProxy, options);
      List<object> proxyArguments = this.BuildArgumentListForClassProxyWithTarget(target, options, interceptors);
      if (constructorArguments != null && constructorArguments.Length != 0)
        proxyArguments.AddRange((IEnumerable<object>) constructorArguments);
      return this.CreateClassProxyInstance(proxyTypeWithTarget, proxyArguments, classToProxy, constructorArguments);
    }

    public TClass CreateClassProxy<TClass>(params IInterceptor[] interceptors) where TClass : class
    {
      return (TClass) this.CreateClassProxy(typeof (TClass), ProxyGenerationOptions.Default, interceptors);
    }

    public TClass CreateClassProxy<TClass>(
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
      where TClass : class
    {
      return (TClass) this.CreateClassProxy(typeof (TClass), options, interceptors);
    }

    public object CreateClassProxy(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxy(classToProxy, additionalInterfacesToProxy, ProxyGenerationOptions.Default, interceptors);
    }

    public object CreateClassProxy(
      Type classToProxy,
      ProxyGenerationOptions options,
      object[] constructorArguments,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxy(classToProxy, (Type[]) null, options, constructorArguments, interceptors);
    }

    public object CreateClassProxy(
      Type classToProxy,
      object[] constructorArguments,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxy(classToProxy, (Type[]) null, ProxyGenerationOptions.Default, constructorArguments, interceptors);
    }

    public object CreateClassProxy(Type classToProxy, params IInterceptor[] interceptors)
    {
      return this.CreateClassProxy(classToProxy, (Type[]) null, ProxyGenerationOptions.Default, (object[]) null, interceptors);
    }

    public object CreateClassProxy(
      Type classToProxy,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxy(classToProxy, (Type[]) null, options, interceptors);
    }

    public object CreateClassProxy(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options,
      params IInterceptor[] interceptors)
    {
      return this.CreateClassProxy(classToProxy, additionalInterfacesToProxy, options, (object[]) null, interceptors);
    }

    public virtual object CreateClassProxy(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options,
      object[] constructorArguments,
      params IInterceptor[] interceptors)
    {
      if (classToProxy == null)
        throw new ArgumentNullException(nameof (classToProxy));
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      if (!classToProxy.IsClass)
        throw new ArgumentException("'classToProxy' must be a class", nameof (classToProxy));
      this.CheckNotGenericTypeDefinition(classToProxy, nameof (classToProxy));
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) additionalInterfacesToProxy, nameof (additionalInterfacesToProxy));
      Type classProxyType = this.CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options);
      List<object> proxyArguments = this.BuildArgumentListForClassProxy(options, interceptors);
      if (constructorArguments != null && constructorArguments.Length != 0)
        proxyArguments.AddRange((IEnumerable<object>) constructorArguments);
      return this.CreateClassProxyInstance(classProxyType, proxyArguments, classToProxy, constructorArguments);
    }

    protected object CreateClassProxyInstance(
      Type proxyType,
      List<object> proxyArguments,
      Type classToProxy,
      object[] constructorArguments)
    {
      try
      {
        return Activator.CreateInstance(proxyType, proxyArguments.ToArray());
      }
      catch (MissingMethodException ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("Can not instantiate proxy of class: {0}.", (object) classToProxy.FullName);
        stringBuilder.AppendLine();
        if (constructorArguments == null || constructorArguments.Length == 0)
        {
          stringBuilder.Append("Could not find a parameterless constructor.");
        }
        else
        {
          stringBuilder.AppendLine("Could not find a constructor that would match given arguments:");
          foreach (object constructorArgument in constructorArguments)
            stringBuilder.AppendLine(constructorArgument.GetType().ToString());
        }
        throw new ArgumentException(stringBuilder.ToString(), nameof (constructorArguments));
      }
    }

    protected void CheckNotGenericTypeDefinition(Type type, string argumentName)
    {
      if (type != null && type.IsGenericTypeDefinition)
        throw new ArgumentException("You can't specify a generic type definition.", argumentName);
    }

    protected void CheckNotGenericTypeDefinitions(IEnumerable<Type> types, string argumentName)
    {
      if (types == null)
        return;
      foreach (Type type in types)
        this.CheckNotGenericTypeDefinition(type, argumentName);
    }

    protected List<object> BuildArgumentListForClassProxyWithTarget(
      object target,
      ProxyGenerationOptions options,
      IInterceptor[] interceptors)
    {
      List<object> objectList = new List<object>();
      objectList.Add(target);
      objectList.AddRange(options.MixinData.Mixins);
      objectList.Add((object) interceptors);
      if (options.Selector != null)
        objectList.Add((object) options.Selector);
      return objectList;
    }

    protected List<object> BuildArgumentListForClassProxy(
      ProxyGenerationOptions options,
      IInterceptor[] interceptors)
    {
      List<object> objectList = new List<object>(options.MixinData.Mixins)
      {
        (object) interceptors
      };
      if (options.Selector != null)
        objectList.Add((object) options.Selector);
      return objectList;
    }

    protected Type CreateClassProxyType(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      return this.ProxyBuilder.CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options);
    }

    protected Type CreateInterfaceProxyTypeWithTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      Type targetType,
      ProxyGenerationOptions options)
    {
      return this.ProxyBuilder.CreateInterfaceProxyTypeWithTarget(interfaceToProxy, additionalInterfacesToProxy, targetType, options);
    }

    protected Type CreateInterfaceProxyTypeWithTargetInterface(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      return this.ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, options);
    }

    protected Type CreateInterfaceProxyTypeWithoutTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      return this.ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, options);
    }

    protected Type CreateClassProxyTypeWithTarget(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
    {
      return this.ProxyBuilder.CreateClassProxyTypeWithTarget(classToProxy, additionalInterfacesToProxy, options);
    }
  }
}
