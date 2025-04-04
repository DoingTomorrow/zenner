// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.ReadOnlyAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class ReadOnlyAccessor : IPropertyAccessor
  {
    public IGetter GetGetter(Type type, string propertyName)
    {
      return (IGetter) (BasicPropertyAccessor.GetGetterOrNull(type, propertyName) ?? throw new PropertyNotFoundException(type, propertyName, "getter"));
    }

    public ISetter GetSetter(Type type, string propertyName)
    {
      return (ISetter) new ReadOnlyAccessor.NoopSetter();
    }

    public bool CanAccessThroughReflectionOptimizer => true;

    [Serializable]
    private class NoopSetter : ISetter
    {
      public void Set(object target, object value)
      {
      }

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;
    }
  }
}
