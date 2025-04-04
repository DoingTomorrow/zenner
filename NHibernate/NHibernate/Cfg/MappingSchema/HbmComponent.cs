// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmComponent
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
  [DesignerCategory("code")]
  [XmlRoot("component", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmComponent : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IComponentMapping,
    IPropertiesContainerMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("tuplizer")]
    public HbmTuplizer[] tuplizer;
    public HbmParent parent;
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("primitive-array", typeof (HbmPrimitiveArray))]
    [XmlElement("set", typeof (HbmSet))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("array", typeof (HbmArray))]
    [XmlElement("bag", typeof (HbmBag))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("property", typeof (HbmProperty))]
    [XmlElement("idbag", typeof (HbmIdbag))]
    [XmlElement("list", typeof (HbmList))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("map", typeof (HbmMap))]
    [XmlElement("one-to-one", typeof (HbmOneToOne))]
    public object[] Items;
    [XmlAttribute]
    public string @class;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    [DefaultValue(false)]
    public bool unique;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool update;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool insert;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool lazy;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(true)]
    public bool optimisticlock;
    [XmlAttribute]
    public string node;

    public HbmComponent()
    {
      this.unique = false;
      this.update = true;
      this.insert = true;
      this.lazy = false;
      this.optimisticlock = true;
    }

    public string Class => this.@class;

    public HbmParent Parent => this.parent;

    public string EmbeddedNode => this.node;

    public bool IsLazyProperty => this.lazy;

    public string Name => this.name;

    public string Access => this.access;

    public bool OptimisticLock => this.optimisticlock;

    [XmlIgnore]
    public IEnumerable<IEntityPropertyMapping> Properties
    {
      get
      {
        return this.Items == null ? (IEnumerable<IEntityPropertyMapping>) new IEntityPropertyMapping[0] : this.Items.Cast<IEntityPropertyMapping>();
      }
    }

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];
  }
}
