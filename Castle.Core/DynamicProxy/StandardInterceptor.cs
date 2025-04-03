// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.StandardInterceptor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.DynamicProxy
{
  [Serializable]
  public class StandardInterceptor : MarshalByRefObject, IInterceptor
  {
    public void Intercept(IInvocation invocation)
    {
      this.PreProceed(invocation);
      this.PerformProceed(invocation);
      this.PostProceed(invocation);
    }

    protected virtual void PerformProceed(IInvocation invocation) => invocation.Proceed();

    protected virtual void PreProceed(IInvocation invocation)
    {
    }

    protected virtual void PostProceed(IInvocation invocation)
    {
    }
  }
}
