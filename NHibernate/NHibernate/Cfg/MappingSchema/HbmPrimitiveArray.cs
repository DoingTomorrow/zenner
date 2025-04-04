// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmPrimitiveArray
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [DesignerCategory("code")]
  [DebuggerStepThrough]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("primitive-array", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmPrimitiveArray : 
    AbstractDecoratable,
    ICollectionPropertiesMapping,
    IEntityPropertyMapping,
    IDecoratable,
    IReferencePropertyMapping,
    ICollectionSqlsMapping,
    IIndexedCollectionMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    public HbmSubselect subselect;
    public HbmCache cache;
    [XmlElement("synchronize")]
    public HbmSynchronize[] synchronize;
    public HbmComment comment;
    public HbmKey key;
    [XmlElement("list-index", typeof (HbmListIndex))]
    [XmlElement("index", typeof (HbmIndex))]
    public object Item;
    public HbmElement element;
    public HbmLoader loader;
    [XmlElement("sql-insert")]
    public HbmCustomSQL sqlinsert;
    [XmlElement("sql-update")]
    public HbmCustomSQL sqlupdate;
    [XmlElement("sql-delete")]
    public HbmCustomSQL sqldelete;
    [XmlElement("sql-delete-all")]
    public HbmCustomSQL sqldeleteall;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    public string table;
    [XmlAttribute]
    public string schema;
    [XmlAttribute]
    public string catalog;
    [XmlAttribute("subselect")]
    public string subselect1;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool mutable;
    [XmlAttribute]
    public string where;
    [XmlAttribute("batch-size", DataType = "positiveInteger")]
    public string batchsize;
    [XmlAttribute("outer-join")]
    public HbmPrimitivearrayOuterjoin outerjoin;
    [XmlIgnore]
    public bool outerjoinSpecified;
    [XmlAttribute]
    public HbmPrimitivearrayFetch fetch;
    [XmlIgnore]
    public bool fetchSpecified;
    [XmlAttribute]
    public string persister;
    [XmlAttribute("collection-type")]
    public string collectiontype;
    [XmlAttribute]
    public string check;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(true)]
    public bool optimisticlock;
    [XmlAttribute]
    public string node;
    [DefaultValue(true)]
    [XmlAttribute("embed-xml")]
    public bool embedxml;

    public HbmPrimitiveArray()
    {
      this.mutable = true;
      this.optimisticlock = true;
      this.embedxml = true;
    }

    public string Name => this.name;

    public string Access => this.access;

    public bool OptimisticLock => this.optimisticlock;

    public bool IsLazyProperty => false;

    public string Cascade => "none";

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    public HbmLoader SqlLoader => this.loader;

    public HbmCustomSQL SqlInsert => this.sqlinsert;

    public HbmCustomSQL SqlUpdate => this.sqlupdate;

    public HbmCustomSQL SqlDelete => this.sqldelete;

    public HbmCustomSQL SqlDeleteAll => this.sqldeleteall;

    public string Subselect
    {
      get
      {
        if (!string.IsNullOrEmpty(this.subselect1))
          return this.subselect1;
        return this.subselect == null ? (string) null : this.subselect.Text.JoinString();
      }
    }

    public bool Inverse => false;

    public bool Mutable => this.mutable;

    public string OrderBy => (string) null;

    public string Where => this.where;

    public int? BatchSize => new int?();

    public string PersisterQualifiedName => this.persister;

    public string CollectionType => this.collectiontype;

    public HbmCollectionFetchMode? FetchMode
    {
      get
      {
        return !this.fetchSpecified ? new HbmCollectionFetchMode?() : new HbmCollectionFetchMode?((HbmCollectionFetchMode) this.fetch);
      }
    }

    public HbmOuterJoinStrategy? OuterJoin
    {
      get
      {
        return !this.outerjoinSpecified ? new HbmOuterJoinStrategy?() : new HbmOuterJoinStrategy?((HbmOuterJoinStrategy) this.outerjoin);
      }
    }

    public HbmCollectionLazy? Lazy => new HbmCollectionLazy?();

    public string Table => this.table;

    public string Schema => this.schema;

    public string Catalog => this.catalog;

    public string Check => this.check;

    public object ElementRelationship => (object) this.element;

    public string Sort => (string) null;

    public bool? Generic => new bool?();

    [XmlIgnore]
    public IEnumerable<HbmFilter> Filters
    {
      get
      {
        yield break;
      }
    }

    public HbmKey Key => this.key;

    public HbmCache Cache => this.cache;

    public HbmListIndex ListIndex => this.Item as HbmListIndex;

    public HbmIndex Index => this.Item as HbmIndex;
  }
}
