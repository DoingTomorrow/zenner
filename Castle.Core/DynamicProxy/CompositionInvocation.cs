// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.CompositionInvocation
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy
{
  public abstract class CompositionInvocation : AbstractInvocation
  {
    protected object target;

    protected CompositionInvocation(
      object target,
      object proxy,
      IInterceptor[] interceptors,
      MethodInfo proxiedMethod,
      object[] arguments)
      : base(proxy, interceptors, proxiedMethod, arguments)
    {
      this.target = target;
    }

    protected CompositionInvocation(
      object target,
      object proxy,
      IInterceptor[] interceptors,
      MethodInfo proxiedMethod,
      object[] arguments,
      IInterceptorSelector selector,
      ref IInterceptor[] methodInterceptors)
      : base(proxy, CompositionInvocation.GetTargetType(target), interceptors, proxiedMethod, arguments, selector, ref methodInterceptors)
    {
      this.target = target;
    }

    private static Type GetTargetType(object targetObject) => targetObject?.GetType();

    protected void EnsureValidTarget()
    {
      if (this.target == null)
        this.ThrowOnNoTarget();
      if (object.ReferenceEquals(this.target, this.proxyObject))
        throw new InvalidOperationException("This is a DynamicProxy2 error: target of invocation has been set to the proxy itself. This may result in recursively calling the method over and over again until stack overflow, which may destabilize your program.This usually signifies a bug in the calling code. Make sure no interceptor sets proxy as its invocation target.");
    }

    protected void EnsureValidProxyTarget(object newTarget)
    {
      if (newTarget == null)
        throw new ArgumentNullException(nameof (newTarget));
      if (object.ReferenceEquals(newTarget, this.proxyObject))
        throw new InvalidOperationException("This is a DynamicProxy2 error: target of proxy has been set to the proxy itself. This would result in recursively calling proxy methods over and over again until stack overflow, which may destabilize your program.This usually signifies a bug in the calling code. Make sure no interceptor sets proxy as its own target.");
    }

    public override object InvocationTarget => this.target;

    public override Type TargetType => CompositionInvocation.GetTargetType(this.target);

    public override MethodInfo MethodInvocationTarget
    {
      get => InvocationHelper.GetMethodOnObject(this.target, this.Method);
    }
  }
}
