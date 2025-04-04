// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmClass
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlRoot("class", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmClass : 
    AbstractDecoratable,
    IEntityMapping,
    IDecoratable,
    IEntitySqlsMapping,
    IPropertiesContainerMapping,
    IEntityDiscriminableMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    public HbmSubselect subselect;
    public HbmCache cache;
    [XmlElement("synchronize")]
    public HbmSynchronize[] synchronize;
    public HbmComment comment;
    [XmlElement("tuplizer")]
    public HbmTuplizer[] tuplizer;
    [XmlElement("id", typeof (HbmId))]
    [XmlElement("composite-id", typeof (HbmCompositeId))]
    public object Item;
    public HbmDiscriminator discriminator;
    [XmlElement("natural-id")]
    public HbmNaturalId naturalid;
    [XmlElement("version", typeof (HbmVersion))]
    [XmlElement("timestamp", typeof (HbmTimestamp))]
    public object Item1;
    [XmlElement("list", typeof (HbmList))]
    [XmlElement("set", typeof (HbmSet))]
    [XmlElement("property", typeof (HbmProperty))]
    [XmlElement("array", typeof (HbmArray))]
    [XmlElement("bag", typeof (HbmBag))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("idbag", typeof (HbmIdbag))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("map", typeof (HbmMap))]
    [XmlElement("one-to-one", typeof (HbmOneToOne))]
    [XmlElement("primitive-array", typeof (HbmPrimitiveArray))]
    [XmlElement("properties", typeof (HbmProperties))]
    public object[] Items;
    [XmlElement("joined-subclass", typeof (HbmJoinedSubclass))]
    [XmlElement("subclass", typeof (HbmSubclass))]
    [XmlElement("join", typeof (HbmJoin))]
    [XmlElement("union-subclass", typeof (HbmUnionSubclass))]
    public object[] Items1;
    public HbmLoader loader;
    [XmlElement("sql-insert")]
    public HbmCustomSQL sqlinsert;
    [XmlElement("sql-update")]
    public HbmCustomSQL sqlupdate;
    [XmlElement("sql-delete")]
    public HbmCustomSQL sqldelete;
    [XmlElement("filter")]
    public HbmFilter[] filter;
    [XmlElement("resultset")]
    public HbmResultSet[] resultset;
    [XmlElement("query", typeof (HbmQuery))]
    [XmlElement("sql-query", typeof (HbmSqlQuery))]
    public object[] Items2;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string proxy;
    [XmlAttribute]
    public bool lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [XmlAttribute("schema-action")]
    public string schemaaction;
    [XmlAttribute]
    public string table;
    [XmlAttribute]
    public string schema;
    [XmlAttribute]
    public string catalog;
    [XmlAttribute("subselect")]
    public string subselect1;
    [XmlAttribute("discriminator-value")]
    public string discriminatorvalue;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool mutable;
    [XmlAttribute]
    public bool @abstract;
    [XmlIgnore]
    public bool abstractSpecified;
    [DefaultValue(HbmPolymorphismType.Implicit)]
    [XmlAttribute]
    public HbmPolymorphismType polymorphism;
    [XmlAttribute]
    public string where;
    [XmlAttribute]
    public string persister;
    [DefaultValue(false)]
    [XmlAttribute("dynamic-update")]
    public bool dynamicupdate;
    [XmlAttribute("dynamic-insert")]
    [DefaultValue(false)]
    public bool dynamicinsert;
    [XmlAttribute("batch-size")]
    public int batchsize;
    [XmlIgnore]
    public bool batchsizeSpecified;
    [DefaultValue(false)]
    [XmlAttribute("select-before-update")]
    public bool selectbeforeupdate;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(HbmOptimisticLockMode.Version)]
    public HbmOptimisticLockMode optimisticlock;
    [XmlAttribute]
    public string check;
    [XmlAttribute]
    public string rowid;
    [XmlAttribute]
    public string node;

    public HbmClass()
    {
      this.mutable = true;
      this.polymorphism = HbmPolymorphismType.Implicit;
      this.dynamicupdate = false;
      this.dynamicinsert = false;
      this.selectbeforeupdate = false;
      this.optimisticlock = HbmOptimisticLockMode.Version;
    }

    public HbmId Id => this.Item as HbmId;

    public HbmCompositeId CompositeId => this.Item as HbmCompositeId;

    public HbmVersion Version => this.Item1 as HbmVersion;

    public HbmTimestamp Timestamp => this.Item1 as HbmTimestamp;

    [XmlIgnore]
    public IEnumerable<HbmJoin> Joins
    {
      get
      {
        return this.Items1 == null ? (IEnumerable<HbmJoin>) new HbmJoin[0] : this.Items1.OfType<HbmJoin>();
      }
    }

    [XmlIgnore]
    public IEnumerable<HbmSubclass> Subclasses
    {
      get
      {
        return this.Items1 == null ? (IEnumerable<HbmSubclass>) new HbmSubclass[0] : this.Items1.OfType<HbmSubclass>();
      }
    }

    [XmlIgnore]
    public IEnumerable<HbmJoinedSubclass> JoinedSubclasses
    {
      get
      {
        return this.Items1 == null ? (IEnumerable<HbmJoinedSubclass>) new HbmJoinedSubclass[0] : this.Items1.OfType<HbmJoinedSubclass>();
      }
    }

    [XmlIgnore]
    public IEnumerable<HbmUnionSubclass> UnionSubclasses
    {
      get
      {
        return this.Items1 == null ? (IEnumerable<HbmUnionSubclass>) new HbmUnionSubclass[0] : this.Items1.OfType<HbmUnionSubclass>();
      }
    }

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    public string EntityName => this.entityname;

    public string Name => this.name;

    public string Node => this.node;

    public string Proxy => this.proxy;

    public bool? UseLazy => !this.lazySpecified ? new bool?() : new bool?(this.lazy);

    public HbmTuplizer[] Tuplizers => this.tuplizer ?? new HbmTuplizer[0];

    public bool DynamicUpdate => this.dynamicupdate;

    public bool DynamicInsert => this.dynamicinsert;

    public int? BatchSize => !this.batchsizeSpecified ? new int?() : new int?(this.batchsize);

    public bool SelectBeforeUpdate => this.selectbeforeupdate;

    public string Persister => this.persister;

    public bool? IsAbstract => !this.abstractSpecified ? new bool?() : new bool?(this.@abstract);

    public HbmSynchronize[] Synchronize => this.synchronize ?? new HbmSynchronize[0];

    public string DiscriminatorValue => this.discriminatorvalue;

    public HbmLoader SqlLoader => this.loader;

    public HbmCustomSQL SqlInsert => this.sqlinsert;

    public HbmCustomSQL SqlUpdate => this.sqlupdate;

    public HbmCustomSQL SqlDelete => this.sqldelete;

    public string Subselect
    {
      get
      {
        if (!string.IsNullOrEmpty(this.subselect1))
          return this.subselect1;
        return this.subselect == null ? (string) null : this.subselect.Text.JoinString();
      }
    }

    [XmlIgnore]
    public IEnumerable<IEntityPropertyMapping> Properties
    {
      get
      {
        return this.Items == null ? (IEnumerable<IEntityPropertyMapping>) new IEntityPropertyMapping[0] : this.Items.Cast<IEntityPropertyMapping>();
      }
    }
  }
}
