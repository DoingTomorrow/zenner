// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmJoinedSubclass
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
  [XmlRoot("joined-subclass", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmJoinedSubclass : 
    AbstractDecoratable,
    IEntityMapping,
    IDecoratable,
    IEntitySqlsMapping,
    IPropertiesContainerMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    public HbmSubselect subselect;
    [XmlElement("synchronize")]
    public HbmSynchronize[] synchronize;
    public HbmComment comment;
    [XmlElement("tuplizer")]
    public HbmTuplizer[] tuplizer;
    public HbmKey key;
    [XmlElement("primitive-array", typeof (HbmPrimitiveArray))]
    [XmlElement("set", typeof (HbmSet))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("array", typeof (HbmArray))]
    [XmlElement("bag", typeof (HbmBag))]
    [XmlElement("properties", typeof (HbmProperties))]
    [XmlElement("idbag", typeof (HbmIdbag))]
    [XmlElement("list", typeof (HbmList))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("map", typeof (HbmMap))]
    [XmlElement("one-to-one", typeof (HbmOneToOne))]
    [XmlElement("property", typeof (HbmProperty))]
    public object[] Items;
    [XmlElement("joined-subclass")]
    public HbmJoinedSubclass[] joinedsubclass1;
    public HbmLoader loader;
    [XmlElement("sql-insert")]
    public HbmCustomSQL sqlinsert;
    [XmlElement("sql-update")]
    public HbmCustomSQL sqlupdate;
    [XmlElement("sql-delete")]
    public HbmCustomSQL sqldelete;
    [XmlElement("resultset")]
    public HbmResultSet[] resultset;
    [XmlElement("query", typeof (HbmQuery))]
    [XmlElement("sql-query", typeof (HbmSqlQuery))]
    public object[] Items1;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string proxy;
    [XmlAttribute]
    public string table;
    [XmlAttribute("schema-action")]
    public string schemaaction;
    [XmlAttribute]
    public string schema;
    [XmlAttribute]
    public string catalog;
    [XmlAttribute("subselect")]
    public string subselect1;
    [DefaultValue(false)]
    [XmlAttribute("dynamic-update")]
    public bool dynamicupdate;
    [XmlAttribute("dynamic-insert")]
    [DefaultValue(false)]
    public bool dynamicinsert;
    [XmlAttribute("select-before-update")]
    [DefaultValue(false)]
    public bool selectbeforeupdate;
    [XmlAttribute]
    public string extends;
    [XmlAttribute]
    public bool lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [XmlAttribute]
    public bool @abstract;
    [XmlIgnore]
    public bool abstractSpecified;
    [XmlAttribute]
    public string persister;
    [XmlAttribute]
    public string check;
    [XmlAttribute("batch-size", DataType = "positiveInteger")]
    public string batchsize;
    [XmlAttribute]
    public string node;

    public HbmJoinedSubclass()
    {
      this.dynamicupdate = false;
      this.dynamicinsert = false;
      this.selectbeforeupdate = false;
    }

    [XmlIgnore]
    public IEnumerable<HbmJoinedSubclass> JoinedSubclasses
    {
      get
      {
        return (IEnumerable<HbmJoinedSubclass>) this.joinedsubclass1 ?? (IEnumerable<HbmJoinedSubclass>) new HbmJoinedSubclass[0];
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

    public int? BatchSize
    {
      get
      {
        return string.IsNullOrEmpty(this.batchsize) ? new int?() : new int?(int.Parse(this.batchsize));
      }
    }

    public bool SelectBeforeUpdate => this.selectbeforeupdate;

    public string Persister => this.persister;

    public bool? IsAbstract => !this.abstractSpecified ? new bool?() : new bool?(this.@abstract);

    public HbmSynchronize[] Synchronize => this.synchronize ?? new HbmSynchronize[0];

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
