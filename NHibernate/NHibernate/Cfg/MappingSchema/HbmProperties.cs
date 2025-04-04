// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmProperties
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
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("properties", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmProperties : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IComponentMapping,
    IPropertiesContainerMapping
  {
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("property", typeof (HbmProperty))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    public object[] Items;
    [XmlAttribute]
    public string name;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool unique;
    [XmlAttribute]
    [DefaultValue(true)]
    public bool insert;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool update;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(true)]
    public bool optimisticlock;
    [XmlAttribute]
    public string node;

    public HbmProperties()
    {
      this.unique = false;
      this.insert = true;
      this.update = true;
      this.optimisticlock = true;
    }

    public bool IsLazyProperty => false;

    public string Class => (string) null;

    public HbmParent Parent => (HbmParent) null;

    public string EmbeddedNode => this.node;

    public string Name => this.name;

    public string Access => "embedded";

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
