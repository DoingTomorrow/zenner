// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.MapAccessor
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
  public class MapAccessor : IPropertyAccessor
  {
    public IGetter GetGetter(Type theClass, string propertyName)
    {
      return (IGetter) new MapAccessor.MapGetter(propertyName);
    }

    public ISetter GetSetter(Type theClass, string propertyName)
    {
      return (ISetter) new MapAccessor.MapSetter(propertyName);
    }

    public bool CanAccessThroughReflectionOptimizer => false;

    [Serializable]
    public sealed class MapSetter : ISetter
    {
      private readonly string name;

      internal MapSetter(string name) => this.name = name;

      public MethodInfo Method => (MethodInfo) null;

      public string PropertyName => (string) null;

      public void Set(object target, object value)
      {
        ((IDictionary) target)[(object) this.name] = value;
      }
    }

    [Serializable]
    public sealed class MapGetter : IGetter
    {
      private readonly string name;

      internal MapGetter(string name) => this.name = name;

      public MethodInfo Method => (MethodInfo) null;

      public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
      {
        return this.Get(owner);
      }

      public string PropertyName => (string) null;

      public Type ReturnType => typeof (object);

      public object Get(object target) => ((IDictionary) target)[(object) this.name];
    }
  }
}
