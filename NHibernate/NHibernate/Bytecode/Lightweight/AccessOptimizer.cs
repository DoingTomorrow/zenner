// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.Lightweight.AccessOptimizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;

#nullable disable
namespace NHibernate.Bytecode.Lightweight
{
  public class AccessOptimizer : IAccessOptimizer
  {
    private readonly GetPropertyValuesInvoker getDelegate;
    private readonly SetPropertyValuesInvoker setDelegate;
    private readonly IGetter[] getters;
    private readonly ISetter[] setters;
    private readonly GetterCallback getterCallback;
    private readonly SetterCallback setterCallback;

    public AccessOptimizer(
      GetPropertyValuesInvoker getDelegate,
      SetPropertyValuesInvoker setDelegate,
      IGetter[] getters,
      ISetter[] setters)
    {
      this.getDelegate = getDelegate;
      this.setDelegate = setDelegate;
      this.getters = getters;
      this.setters = setters;
      this.getterCallback = new GetterCallback(this.OnGetterCallback);
      this.setterCallback = new SetterCallback(this.OnSetterCallback);
    }

    public object[] GetPropertyValues(object target)
    {
      return this.getDelegate(target, this.getterCallback);
    }

    public void SetPropertyValues(object target, object[] values)
    {
      this.setDelegate(target, values, this.setterCallback);
    }

    private object OnGetterCallback(object target, int i) => this.getters[i].Get(target);

    private void OnSetterCallback(object target, int i, object value)
    {
      this.setters[i].Set(target, value);
    }
  }
}
