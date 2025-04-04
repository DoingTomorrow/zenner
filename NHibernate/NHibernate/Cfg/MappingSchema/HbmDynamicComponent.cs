// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmDynamicComponent
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
  [XmlRoot("dynamic-component", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [Serializable]
  public class HbmDynamicComponent : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IComponentMapping,
    IPropertiesContainerMapping
  {
    [XmlElement("list", typeof (HbmList))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("set", typeof (HbmSet))]
    [XmlElement("bag", typeof (HbmBag))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("array", typeof (HbmArray))]
    [XmlElement("map", typeof (HbmMap))]
    [XmlElement("one-to-one", typeof (HbmOneToOne))]
    [XmlElement("primitive-array", typeof (HbmPrimitiveArray))]
    [XmlElement("property", typeof (HbmProperty))]
    public object[] Items;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool unique;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool update;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool insert;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(true)]
    public bool optimisticlock;
    [XmlAttribute]
    public string node;

    public HbmDynamicComponent()
    {
      this.unique = false;
      this.update = true;
      this.insert = true;
      this.optimisticlock = true;
    }

    public string Class => (string) null;

    public HbmParent Parent => (HbmParent) null;

    public bool IsLazyProperty => false;

    public string EmbeddedNode => this.node;

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

    protected override HbmMeta[] Metadatas => new HbmMeta[0];
  }
}
