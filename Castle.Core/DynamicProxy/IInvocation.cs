// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.IInvocation
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy
{
  public interface IInvocation
  {
    object Proxy { get; }

    object InvocationTarget { get; }

    Type TargetType { get; }

    object[] Arguments { get; }

    void SetArgumentValue(int index, object value);

    object GetArgumentValue(int index);

    Type[] GenericArguments { get; }

    MethodInfo Method { get; }

    MethodInfo GetConcreteMethod();

    MethodInfo MethodInvocationTarget { get; }

    MethodInfo GetConcreteMethodInvocationTarget();

    object ReturnValue { get; set; }

    void Proceed();
  }
}
