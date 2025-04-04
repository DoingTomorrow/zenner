// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmNestedCompositeElement
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
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("nested-composite-element", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmNestedCompositeElement : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IComponentMapping,
    IPropertiesContainerMapping
  {
    public HbmParent parent;
    [XmlElement("property", typeof (HbmProperty))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("nested-composite-element", typeof (HbmNestedCompositeElement))]
    public object[] Items;
    [XmlAttribute]
    public string @class;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    public string node;

    [XmlIgnore]
    public IEnumerable<IEntityPropertyMapping> Properties
    {
      get
      {
        return this.Items == null ? (IEnumerable<IEntityPropertyMapping>) new IEntityPropertyMapping[0] : this.Items.Cast<IEntityPropertyMapping>();
      }
    }

    public string Class => this.@class;

    public HbmParent Parent => this.parent;

    public string EmbeddedNode => this.node;

    public string Name => this.name;

    public string Access => this.access;

    public bool OptimisticLock => true;

    protected override HbmMeta[] Metadatas => new HbmMeta[0];

    public bool IsLazyProperty => false;
  }
}
