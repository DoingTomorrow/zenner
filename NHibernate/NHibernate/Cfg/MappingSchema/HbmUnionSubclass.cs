// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmUnionSubclass
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
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("union-subclass", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [Serializable]
  public class HbmUnionSubclass : 
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
    [XmlElement("set", typeof (HbmSet))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("idbag", typeof (HbmIdbag))]
    [XmlElement("list", typeof (HbmList))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("array", typeof (HbmArray))]
    [XmlElement("bag", typeof (HbmBag))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("map", typeof (HbmMap))]
    [XmlElement("one-to-one", typeof (HbmOneToOne))]
    [XmlElement("primitive-array", typeof (HbmPrimitiveArray))]
    [XmlElement("properties", typeof (HbmProperties))]
    [XmlElement("property", typeof (HbmProperty))]
    public object[] Items;
    [XmlElement("union-subclass")]
    public HbmUnionSubclass[] unionsubclass1;
    public HbmLoader loader;
    [XmlElement("sql-insert")]
    public HbmCustomSQL sqlinsert;
    [XmlElement("sql-update")]
    public HbmCustomSQL sqlupdate;
    [XmlElement("sql-delete")]
    public HbmCustomSQL sqldelete;
    [XmlElement("resultset")]
    public HbmResultSet[] resultset;
    [XmlElement("sql-query", typeof (HbmSqlQuery))]
    [XmlElement("query", typeof (HbmQuery))]
    public object[] Items1;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string proxy;
    [XmlAttribute]
    public string table;
    [XmlAttribute]
    public string schema;
    [XmlAttribute]
    public string catalog;
    [XmlAttribute("subselect")]
    public string subselect1;
    [XmlAttribute("dynamic-update")]
    [DefaultValue(false)]
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

    public HbmUnionSubclass()
    {
      this.dynamicupdate = false;
      this.dynamicinsert = false;
      this.selectbeforeupdate = false;
    }

    [XmlIgnore]
    public IEnumerable<HbmUnionSubclass> UnionSubclasses
    {
      get
      {
        return (IEnumerable<HbmUnionSubclass>) this.unionsubclass1 ?? (IEnumerable<HbmUnionSubclass>) new HbmUnionSubclass[0];
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
