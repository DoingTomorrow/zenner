// Decompiled with JetBrains decompiler
// Type: NHibernate.ByteCode.Castle.LazyFieldInterceptor
// Assembly: NHibernate.ByteCode.Castle, Version=3.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: 1144BD3F-E8FD-45B0-9AA0-77466B846AAB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.ByteCode.Castle.dll

using Castle.DynamicProxy;
using NHibernate.Intercept;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.ByteCode.Castle
{
  [Serializable]
  public class LazyFieldInterceptor : IFieldInterceptorAccessor, Castle.DynamicProxy.IInterceptor
  {
    public IFieldInterceptor FieldInterceptor { get; set; }

    public void Intercept(IInvocation invocation)
    {
      if (this.FieldInterceptor != null)
      {
        if (ReflectHelper.IsPropertyGet(invocation.Method))
        {
          invocation.Proceed();
          object obj = this.FieldInterceptor.Intercept(invocation.InvocationTarget, ReflectHelper.GetPropertyName(invocation.Method), invocation.ReturnValue);
          if (obj == AbstractFieldInterceptor.InvokeImplementation)
            return;
          invocation.ReturnValue = obj;
        }
        else if (ReflectHelper.IsPropertySet(invocation.Method))
        {
          this.FieldInterceptor.MarkDirty();
          this.FieldInterceptor.Intercept(invocation.InvocationTarget, ReflectHelper.GetPropertyName(invocation.Method), (object) null);
          invocation.Proceed();
        }
        else
          invocation.Proceed();
      }
      else
        invocation.Proceed();
    }
  }
}
