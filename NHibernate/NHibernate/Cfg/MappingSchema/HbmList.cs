// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmList
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
  [XmlRoot("list", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmList : 
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
    [XmlElement("many-to-any", typeof (HbmManyToAny))]
    [XmlElement("element", typeof (HbmElement))]
    [XmlElement("one-to-many", typeof (HbmOneToMany))]
    [XmlElement("composite-element", typeof (HbmCompositeElement))]
    [XmlElement("many-to-many", typeof (HbmManyToMany))]
    public object Item1;
    public HbmLoader loader;
    [XmlElement("sql-insert")]
    public HbmCustomSQL sqlinsert;
    [XmlElement("sql-update")]
    public HbmCustomSQL sqlupdate;
    [XmlElement("sql-delete")]
    public HbmCustomSQL sqldelete;
    [XmlElement("sql-delete-all")]
    public HbmCustomSQL sqldeleteall;
    [XmlElement("filter")]
    public HbmFilter[] filter;
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
    public HbmCollectionLazy lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool inverse;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool mutable;
    [XmlAttribute]
    public string cascade;
    [XmlAttribute("order-by")]
    public string orderby;
    [XmlAttribute]
    public string where;
    [XmlAttribute("batch-size")]
    public int batchsize;
    [XmlIgnore]
    public bool batchsizeSpecified;
    [XmlAttribute("outer-join")]
    public HbmOuterJoinStrategy outerjoin;
    [XmlIgnore]
    public bool outerjoinSpecified;
    [XmlAttribute]
    public HbmCollectionFetchMode fetch;
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
    [XmlAttribute("embed-xml")]
    [DefaultValue(true)]
    public bool embedxml;
    [XmlAttribute]
    public bool generic;
    [XmlIgnore]
    public bool genericSpecified;

    public HbmList()
    {
      this.inverse = false;
      this.mutable = true;
      this.optimisticlock = true;
      this.embedxml = true;
    }

    public string Name => this.name;

    public string Access => this.access;

    public bool OptimisticLock => this.optimisticlock;

    public bool IsLazyProperty => false;

    public string Cascade => this.cascade;

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

    public HbmListIndex ListIndex => this.Item as HbmListIndex;

    public HbmIndex Index => this.Item as HbmIndex;

    public bool Inverse => this.inverse;

    public bool Mutable => this.mutable;

    public string OrderBy => this.orderby;

    public string Where => this.where;

    public int? BatchSize => !this.batchsizeSpecified ? new int?() : new int?(this.batchsize);

    public string PersisterQualifiedName => this.persister;

    public string CollectionType => this.collectiontype;

    public HbmCollectionFetchMode? FetchMode
    {
      get
      {
        return !this.fetchSpecified ? new HbmCollectionFetchMode?() : new HbmCollectionFetchMode?(this.fetch);
      }
    }

    public HbmOuterJoinStrategy? OuterJoin
    {
      get
      {
        return !this.outerjoinSpecified ? new HbmOuterJoinStrategy?() : new HbmOuterJoinStrategy?(this.outerjoin);
      }
    }

    public HbmCollectionLazy? Lazy
    {
      get => !this.lazySpecified ? new HbmCollectionLazy?() : new HbmCollectionLazy?(this.lazy);
    }

    public string Table => this.table;

    public string Schema => this.schema;

    public string Catalog => this.catalog;

    public string Check => this.check;

    public object ElementRelationship => this.Item1;

    public string Sort => (string) null;

    public bool? Generic => !this.genericSpecified ? new bool?() : new bool?(this.generic);

    [XmlIgnore]
    public IEnumerable<HbmFilter> Filters
    {
      get => (IEnumerable<HbmFilter>) this.filter ?? (IEnumerable<HbmFilter>) new HbmFilter[0];
    }

    public HbmKey Key => this.key;

    public HbmCache Cache => this.cache;
  }
}
