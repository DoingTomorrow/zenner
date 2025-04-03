// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.IProxyBuilder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Logging;
using System;

#nullable disable
namespace Castle.DynamicProxy
{
  public interface IProxyBuilder
  {
    ILogger Logger { get; set; }

    ModuleScope ModuleScope { get; }

    [Obsolete("Use CreateClassProxyType method instead.")]
    Type CreateClassProxy(Type classToProxy, ProxyGenerationOptions options);

    [Obsolete("Use CreateClassProxyType method instead.")]
    Type CreateClassProxy(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options);

    Type CreateClassProxyType(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options);

    Type CreateInterfaceProxyTypeWithTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      Type targetType,
      ProxyGenerationOptions options);

    Type CreateInterfaceProxyTypeWithoutTarget(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options);

    Type CreateInterfaceProxyTypeWithTargetInterface(
      Type interfaceToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options);

    Type CreateClassProxyTypeWithTarget(
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options);
  }
}
