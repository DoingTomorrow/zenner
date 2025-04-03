// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.InheritanceInvocation
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy
{
  public abstract class InheritanceInvocation : AbstractInvocation
  {
    private readonly Type targetType;

    protected InheritanceInvocation(
      Type targetType,
      object proxy,
      IInterceptor[] interceptors,
      MethodInfo proxiedMethod,
      object[] arguments)
      : base(proxy, interceptors, proxiedMethod, arguments)
    {
      this.targetType = targetType;
    }

    protected InheritanceInvocation(
      Type targetType,
      object proxy,
      IInterceptor[] interceptors,
      MethodInfo proxiedMethod,
      object[] arguments,
      IInterceptorSelector selector,
      ref IInterceptor[] methodInterceptors)
      : base(proxy, targetType, interceptors, proxiedMethod, arguments, selector, ref methodInterceptors)
    {
      this.targetType = targetType;
    }

    public override object InvocationTarget => this.Proxy;

    public override Type TargetType => this.targetType;

    public override MethodInfo MethodInvocationTarget
    {
      get => InvocationHelper.GetMethodOnType(this.targetType, this.Method);
    }

    protected abstract override void InvokeMethodOnTarget();
  }
}
