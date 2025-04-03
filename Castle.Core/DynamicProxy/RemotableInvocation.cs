// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.RemotableInvocation
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
  public class RemotableInvocation : MarshalByRefObject, IInvocation, ISerializable
  {
    private readonly IInvocation parent;

    public RemotableInvocation(IInvocation parent) => this.parent = parent;

    protected RemotableInvocation(SerializationInfo info, StreamingContext context)
    {
      this.parent = (IInvocation) info.GetValue("invocation", typeof (IInvocation));
    }

    public void SetArgumentValue(int index, object value)
    {
      this.parent.SetArgumentValue(index, value);
    }

    public object GetArgumentValue(int index) => this.parent.GetArgumentValue(index);

    public Type[] GenericArguments => this.parent.GenericArguments;

    public void Proceed() => this.parent.Proceed();

    public object Proxy => this.parent.Proxy;

    public object InvocationTarget => this.parent.InvocationTarget;

    public Type TargetType => this.parent.TargetType;

    public object[] Arguments => this.parent.Arguments;

    public MethodInfo Method => this.parent.Method;

    public MethodInfo GetConcreteMethod() => this.parent.GetConcreteMethod();

    public MethodInfo MethodInvocationTarget => this.parent.MethodInvocationTarget;

    public MethodInfo GetConcreteMethodInvocationTarget()
    {
      return this.parent.GetConcreteMethodInvocationTarget();
    }

    public object ReturnValue
    {
      get => this.parent.ReturnValue;
      set => this.parent.ReturnValue = value;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.SetType(typeof (RemotableInvocation));
      info.AddValue("invocation", (object) new RemotableInvocation((IInvocation) this));
    }
  }
}
