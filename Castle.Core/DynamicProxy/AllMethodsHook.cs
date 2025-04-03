// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.AllMethodsHook
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy
{
  [Serializable]
  public class AllMethodsHook : IProxyGenerationHook
  {
    protected static readonly ICollection<Type> SkippedTypes = (ICollection<Type>) new Type[3]
    {
      typeof (object),
      typeof (MarshalByRefObject),
      typeof (ContextBoundObject)
    };

    public virtual bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
    {
      return !AllMethodsHook.SkippedTypes.Contains(methodInfo.DeclaringType) && !this.IsFinalizer(methodInfo);
    }

    protected bool IsFinalizer(MethodInfo methodInfo)
    {
      return methodInfo.Name == "Finalize" && methodInfo.GetBaseDefinition().DeclaringType == typeof (object);
    }

    public virtual void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
    {
    }

    public virtual void MethodsInspected()
    {
    }

    public override bool Equals(object obj) => obj != null && obj.GetType() == this.GetType();

    public override int GetHashCode() => this.GetType().GetHashCode();
  }
}
