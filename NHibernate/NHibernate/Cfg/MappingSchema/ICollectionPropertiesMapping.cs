// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.ICollectionPropertiesMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  public interface ICollectionPropertiesMapping : 
    IEntityPropertyMapping,
    IDecoratable,
    IReferencePropertyMapping,
    ICollectionSqlsMapping
  {
    bool Inverse { get; }

    bool Mutable { get; }

    string OrderBy { get; }

    string Where { get; }

    int? BatchSize { get; }

    string PersisterQualifiedName { get; }

    string CollectionType { get; }

    HbmCollectionFetchMode? FetchMode { get; }

    HbmOuterJoinStrategy? OuterJoin { get; }

    HbmCollectionLazy? Lazy { get; }

    string Table { get; }

    string Schema { get; }

    string Catalog { get; }

    string Check { get; }

    object ElementRelationship { get; }

    string Sort { get; }

    bool? Generic { get; }

    IEnumerable<HbmFilter> Filters { get; }

    HbmKey Key { get; }

    HbmCache Cache { get; }
  }
}
