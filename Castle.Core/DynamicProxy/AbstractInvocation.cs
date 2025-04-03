// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.AbstractInvocation
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy
{
  [Serializable]
  public abstract class AbstractInvocation : IInvocation, ISerializable
  {
    private readonly IInterceptor[] interceptors;
    private readonly object[] arguments;
    private int execIndex = -1;
    private Type[] genericMethodArguments;
    private readonly MethodInfo proxiedMethod;
    protected readonly object proxyObject;

    protected AbstractInvocation(
      object proxy,
      IInterceptor[] interceptors,
      MethodInfo proxiedMethod,
      object[] arguments)
    {
      this.proxyObject = proxy;
      this.interceptors = interceptors;
      this.proxiedMethod = proxiedMethod;
      this.arguments = arguments;
    }

    protected AbstractInvocation(
      object proxy,
      Type targetType,
      IInterceptor[] interceptors,
      MethodInfo proxiedMethod,
      object[] arguments,
      IInterceptorSelector selector,
      ref IInterceptor[] methodInterceptors)
      : this(proxy, interceptors, proxiedMethod, arguments)
    {
      methodInterceptors = this.SelectMethodInterceptors(selector, methodInterceptors, targetType);
      this.interceptors = methodInterceptors;
    }

    private IInterceptor[] SelectMethodInterceptors(
      IInterceptorSelector selector,
      IInterceptor[] methodInterceptors,
      Type targetType)
    {
      return methodInterceptors ?? selector.SelectInterceptors(targetType, this.Method, this.interceptors) ?? new IInterceptor[0];
    }

    public void SetGenericMethodArguments(Type[] arguments)
    {
      this.genericMethodArguments = arguments;
    }

    public abstract object InvocationTarget { get; }

    public abstract Type TargetType { get; }

    public abstract MethodInfo MethodInvocationTarget { get; }

    public Type[] GenericArguments => this.genericMethodArguments;

    public object Proxy => this.proxyObject;

    public MethodInfo Method => this.proxiedMethod;

    public MethodInfo GetConcreteMethod() => this.EnsureClosedMethod(this.Method);

    public MethodInfo GetConcreteMethodInvocationTarget() => this.MethodInvocationTarget;

    public object ReturnValue { get; set; }

    public object[] Arguments => this.arguments;

    public void SetArgumentValue(int index, object value) => this.arguments[index] = value;

    public object GetArgumentValue(int index) => this.arguments[index];

    public void Proceed()
    {
      if (this.interceptors == null)
      {
        this.InvokeMethodOnTarget();
      }
      else
      {
        ++this.execIndex;
        if (this.execIndex == this.interceptors.Length)
        {
          this.InvokeMethodOnTarget();
        }
        else
        {
          if (this.execIndex > this.interceptors.Length)
            throw new InvalidOperationException("This is a DynamicProxy2 error: invocation.Proceed() has been called more times than expected.This usually signifies a bug in the calling code. Make sure that" + (this.interceptors.Length <= 1 ? " interceptor" : " each one of " + (object) this.interceptors.Length + " interceptors") + " selected for the method '" + (object) this.Method + "'calls invocation.Proceed() at most once.");
          this.interceptors[this.execIndex].Intercept((IInvocation) this);
        }
      }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.SetType(typeof (RemotableInvocation));
      info.AddValue("invocation", (object) new RemotableInvocation((IInvocation) this));
    }

    protected abstract void InvokeMethodOnTarget();

    protected void ThrowOnNoTarget()
    {
      string str1 = this.interceptors.Length != 0 ? "The interceptor attempted to 'Proceed'" : "There are no interceptors specified";
      string str2;
      string str3;
      if (this.Method.DeclaringType.IsClass && this.Method.IsAbstract)
      {
        str2 = "is abstract";
        str3 = "an abstract method";
      }
      else
      {
        str2 = "has no target";
        str3 = "method without target";
      }
      throw new NotImplementedException(string.Format("This is a DynamicProxy2 error: {0} for method '{1}' which {2}. When calling {3} there is no implementation to 'proceed' to and it is the responsibility of the interceptor to mimic the implementation (set return value, out arguments etc)", (object) str1, (object) this.Method, (object) str2, (object) str3));
    }

    private MethodInfo EnsureClosedMethod(MethodInfo method)
    {
      return method.ContainsGenericParameters ? method.GetGenericMethodDefinition().MakeGenericMethod(this.genericMethodArguments) : method;
    }
  }
}
