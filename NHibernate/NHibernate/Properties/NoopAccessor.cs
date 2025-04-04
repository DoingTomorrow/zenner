// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.NoopAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class NoopAccessor : IPropertyAccessor
  {
    public IGetter GetGetter(Type theClass, string propertyName)
    {
      return (IGetter) new NoopAccessor.NoopGetter();
    }

    public ISetter GetSetter(Type theClass, string propertyName)
    {
      return (ISetter) new NoopAccessor.NoopSetter();
    }

    public bool CanAccessThroughReflectionOptimizer => false;

    [Serializable]
    private class NoopGetter : IGetter
    {
      public object Get(object target) => (object) null;

      public Type ReturnType => typeof (object);

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;

      public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
      {
        return (object) null;
      }
    }

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
