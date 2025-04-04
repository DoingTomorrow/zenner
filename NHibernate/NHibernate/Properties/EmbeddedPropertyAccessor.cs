// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.EmbeddedPropertyAccessor
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
  public class EmbeddedPropertyAccessor : IPropertyAccessor
  {
    public IGetter GetGetter(Type theClass, string propertyName)
    {
      return (IGetter) new EmbeddedPropertyAccessor.EmbeddedGetter(theClass);
    }

    public ISetter GetSetter(Type theClass, string propertyName)
    {
      return (ISetter) new EmbeddedPropertyAccessor.EmbeddedSetter(theClass);
    }

    public bool CanAccessThroughReflectionOptimizer => false;

    [Serializable]
    public sealed class EmbeddedGetter : IGetter
    {
      private readonly Type clazz;

      internal EmbeddedGetter(Type clazz) => this.clazz = clazz;

      public object Get(object target) => target;

      public Type ReturnType => this.clazz;

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;

      public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
      {
        return this.Get(owner);
      }

      public override string ToString()
      {
        return string.Format("EmbeddedGetter({0})", (object) this.clazz.FullName);
      }
    }

    [Serializable]
    public sealed class EmbeddedSetter : ISetter
    {
      private readonly Type clazz;

      internal EmbeddedSetter(Type clazz) => this.clazz = clazz;

      public void Set(object target, object value)
      {
      }

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;

      public override string ToString()
      {
        return string.Format("EmbeddedSetter({0})", (object) this.clazz.FullName);
      }
    }
  }
}
