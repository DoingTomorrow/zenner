// Decompiled with JetBrains decompiler
// Type: NHibernate.Metadata.IClassMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System.Collections;

#nullable disable
namespace NHibernate.Metadata
{
  public interface IClassMetadata
  {
    string EntityName { get; }

    string IdentifierPropertyName { get; }

    string[] PropertyNames { get; }

    IType IdentifierType { get; }

    IType[] PropertyTypes { get; }

    bool IsMutable { get; }

    bool IsVersioned { get; }

    int VersionProperty { get; }

    bool[] PropertyNullability { get; }

    bool[] PropertyLaziness { get; }

    int[] NaturalIdentifierProperties { get; }

    bool IsInherited { get; }

    IType GetPropertyType(string propertyName);

    bool HasProxy { get; }

    bool HasIdentifierProperty { get; }

    bool HasNaturalIdentifier { get; }

    bool HasSubclasses { get; }

    object[] GetPropertyValuesToInsert(
      object entity,
      IDictionary mergeMap,
      ISessionImplementor session);

    System.Type GetMappedClass(EntityMode entityMode);

    object Instantiate(object id, EntityMode entityMode);

    object GetPropertyValue(object obj, string propertyName, EntityMode entityMode);

    object[] GetPropertyValues(object entity, EntityMode entityMode);

    void SetPropertyValue(object obj, string propertyName, object value, EntityMode entityMode);

    void SetPropertyValues(object entity, object[] values, EntityMode entityMode);

    object GetIdentifier(object entity, EntityMode entityMode);

    void SetIdentifier(object entity, object id, EntityMode entityMode);

    bool ImplementsLifecycle(EntityMode entityMode);

    bool ImplementsValidatable(EntityMode entityMode);

    object GetVersion(object obj, EntityMode entityMode);
  }
}
