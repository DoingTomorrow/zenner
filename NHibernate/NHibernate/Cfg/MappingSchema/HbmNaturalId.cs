// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmNaturalId
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
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("natural-id", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmNaturalId : IPropertiesContainerMapping
  {
    [XmlElement("many-to-one", typeof (HbmManyToOne))]
    [XmlElement("component", typeof (HbmComponent))]
    [XmlElement("dynamic-component", typeof (HbmDynamicComponent))]
    [XmlElement("any", typeof (HbmAny))]
    [XmlElement("property", typeof (HbmProperty))]
    public object[] Items;
    [XmlAttribute]
    [DefaultValue(false)]
    public bool mutable;

    public HbmNaturalId() => this.mutable = false;

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
