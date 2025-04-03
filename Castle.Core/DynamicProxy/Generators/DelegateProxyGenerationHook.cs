// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.DelegateProxyGenerationHook
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class DelegateProxyGenerationHook : IProxyGenerationHook
  {
    public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
    {
      return methodInfo.Name.Equals("Invoke");
    }

    public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
    {
    }

    public void MethodsInspected()
    {
    }

    public override bool Equals(object obj)
    {
      return !object.ReferenceEquals((object) null, obj) && obj.GetType() == typeof (DelegateProxyGenerationHook);
    }

    public override int GetHashCode() => this.GetType().GetHashCode();
  }
}
