// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.IndexPropertyAccessor
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
  public class IndexPropertyAccessor : IPropertyAccessor
  {
    private readonly string propertyName;
    private readonly string entityName;

    public IndexPropertyAccessor(string collectionRole, string entityName)
    {
      this.propertyName = collectionRole.Substring(entityName.Length + 1);
      this.entityName = entityName;
    }

    public IGetter GetGetter(Type theClass, string propertyName)
    {
      return (IGetter) new IndexPropertyAccessor.IndexGetter(this);
    }

    public ISetter GetSetter(Type theClass, string propertyName)
    {
      return (ISetter) new IndexPropertyAccessor.IndexSetter();
    }

    public bool CanAccessThroughReflectionOptimizer => false;

    [Serializable]
    public sealed class IndexSetter : ISetter
    {
      public void Set(object target, object value)
      {
      }

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;
    }

    [Serializable]
    public class IndexGetter : IGetter
    {
      private readonly IndexPropertyAccessor encloser;

      public IndexGetter(IndexPropertyAccessor encloser) => this.encloser = encloser;

      public object Get(object target) => BackrefPropertyAccessor.Unknown;

      public Type ReturnType => typeof (object);

      public string PropertyName => (string) null;

      public MethodInfo Method => (MethodInfo) null;

      public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
      {
        return session == null ? BackrefPropertyAccessor.Unknown : session.PersistenceContext.GetIndexInOwner(this.encloser.entityName, this.encloser.propertyName, owner, mergeMap);
      }
    }
  }
}
