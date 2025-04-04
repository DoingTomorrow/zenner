// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Entity.IEntityTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Tuple.Entity
{
  public interface IEntityTuplizer : ITuplizer
  {
    bool IsLifecycleImplementor { get; }

    bool IsValidatableImplementor { get; }

    Type ConcreteProxyClass { get; }

    bool IsInstrumented { get; }

    object Instantiate(object id);

    object GetIdentifier(object entity);

    void SetIdentifier(object entity, object id);

    void ResetIdentifier(object entity, object currentId, object currentVersion);

    object GetVersion(object entity);

    void SetPropertyValue(object entity, int i, object value);

    void SetPropertyValue(object entity, string propertyName, object value);

    object[] GetPropertyValuesToInsert(
      object entity,
      IDictionary mergeMap,
      ISessionImplementor session);

    object GetPropertyValue(object entity, string propertyName);

    void AfterInitialize(
      object entity,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session);

    bool HasProxy { get; }

    object CreateProxy(object id, ISessionImplementor session);

    bool HasUninitializedLazyProperties(object entity);
  }
}
