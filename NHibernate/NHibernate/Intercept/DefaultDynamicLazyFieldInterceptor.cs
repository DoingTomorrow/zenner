// Decompiled with JetBrains decompiler
// Type: NHibernate.Intercept.DefaultDynamicLazyFieldInterceptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Proxy.DynamicProxy;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Intercept
{
  [Serializable]
  public class DefaultDynamicLazyFieldInterceptor : IFieldInterceptorAccessor, NHibernate.Proxy.DynamicProxy.IInterceptor
  {
    public DefaultDynamicLazyFieldInterceptor(object targetInstance)
    {
      this.TargetInstance = targetInstance != null ? targetInstance : throw new ArgumentNullException(nameof (targetInstance));
    }

    public IFieldInterceptor FieldInterceptor { get; set; }

    public object TargetInstance { get; private set; }

    public object Intercept(InvocationInfo info)
    {
      string name = info.TargetMethod.Name;
      if (this.FieldInterceptor != null)
      {
        if (ReflectHelper.IsPropertyGet(info.TargetMethod))
        {
          if ("get_FieldInterceptor".Equals(name))
            return (object) this.FieldInterceptor;
          object obj1 = info.TargetMethod.Invoke(this.TargetInstance, info.Arguments);
          object obj2 = this.FieldInterceptor.Intercept(info.Target, ReflectHelper.GetPropertyName(info.TargetMethod), obj1);
          if (obj2 != AbstractFieldInterceptor.InvokeImplementation)
            return obj2;
        }
        else if (ReflectHelper.IsPropertySet(info.TargetMethod))
        {
          if ("set_FieldInterceptor".Equals(name))
          {
            this.FieldInterceptor = (IFieldInterceptor) info.Arguments[0];
            return (object) null;
          }
          this.FieldInterceptor.MarkDirty();
          this.FieldInterceptor.Intercept(info.Target, ReflectHelper.GetPropertyName(info.TargetMethod), info.Arguments[0]);
        }
      }
      else if ("set_FieldInterceptor".Equals(name))
      {
        this.FieldInterceptor = (IFieldInterceptor) info.Arguments[0];
        return (object) null;
      }
      return info.TargetMethod.Invoke(this.TargetInstance, info.Arguments);
    }
  }
}
