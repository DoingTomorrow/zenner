// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmJoin
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
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("join", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmJoin : IEntitySqlsMapping, IPropertiesContainerMapping
  {
    public HbmSubselect subselect;
    public HbmComment comment;
    public HbmKey key;
    [XmlElement("array", typeof (HbmArray))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("property", typeof (HbmProperty))]
    [XmlElement("set", typeof (HbmSet))]
    [XmlElement("primitive-array", typeof (HbmPrimitiveArray))]
    [XmlElement("bag", typeof (HbmBag))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("idbag", typeof (HbmIdbag))]
    [XmlElement("list", typeof (HbmList))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("map", typeof (HbmMap))]
    public object[] Items;
    [XmlElement("sql-insert")]
    public HbmCustomSQL sqlinsert;
    [XmlElement("sql-update")]
    public HbmCustomSQL sqlupdate;
    [XmlElement("sql-delete")]
    public HbmCustomSQL sqldelete;
    [XmlAttribute]
    public string table;
    [XmlAttribute]
    public string schema;
    [XmlAttribute]
    public string catalog;
    [XmlAttribute("subselect")]
    public string subselect1;
    [XmlAttribute]
    [DefaultValue(HbmJoinFetch.Join)]
    public HbmJoinFetch fetch;
    [XmlAttribute]
    [DefaultValue(false)]
    public bool inverse;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool optional;

    public HbmJoin()
    {
      this.fetch = HbmJoinFetch.Join;
      this.inverse = false;
      this.optional = false;
    }

    public HbmLoader SqlLoader => (HbmLoader) null;

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
