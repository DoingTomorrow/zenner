// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmCompositeMapKey
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
  [XmlRoot("composite-map-key", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmCompositeMapKey : IComponentMapping, IPropertiesContainerMapping
  {
    [XmlElement("key-property", typeof (HbmKeyProperty))]
    [XmlElement("key-many-to-one", typeof (HbmKeyManyToOne))]
    public object[] Items;
    [XmlAttribute]
    public string @class;

    [XmlIgnore]
    public IEnumerable<IEntityPropertyMapping> Properties
    {
      get
      {
        return this.Items == null ? (IEnumerable<IEntityPropertyMapping>) new IEntityPropertyMapping[0] : this.Items.Cast<IEntityPropertyMapping>();
      }
    }

    public string Class => this.@class;

    public HbmParent Parent => (HbmParent) null;

    public string EmbeddedNode => (string) null;

    public string Name => (string) null;
  }
}
