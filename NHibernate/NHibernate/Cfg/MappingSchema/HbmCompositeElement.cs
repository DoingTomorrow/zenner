// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmCompositeElement
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
  [XmlRoot("composite-element", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [Serializable]
  public class HbmCompositeElement : 
    AbstractDecoratable,
    IComponentMapping,
    IPropertiesContainerMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    public HbmParent parent;
    [XmlElement("nested-composite-element", typeof (HbmNestedCompositeElement))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("property", typeof (HbmProperty))]
    [XmlElement("any", typeof (HbmAny))]
    public object[] Items;
    [XmlAttribute]
    public string @class;
    [XmlAttribute]
    public string node;

    public string Class => this.@class;

    public HbmParent Parent => this.parent;

    public string EmbeddedNode => this.node;

    public string Name => (string) null;

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

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
