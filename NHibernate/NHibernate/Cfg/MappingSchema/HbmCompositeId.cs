// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmCompositeId
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [XmlRoot("composite-id", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmCompositeId
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("key-property", typeof (HbmKeyProperty))]
    [XmlElement("key-many-to-one", typeof (HbmKeyManyToOne))]
    public object[] Items;
    [XmlAttribute]
    public string @class;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool mapped;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string node;
    [XmlAttribute]
    public string access;
    [DefaultValue(HbmUnsavedValueType.Undefined)]
    [XmlAttribute("unsaved-value")]
    public HbmUnsavedValueType unsavedvalue;

    public HbmCompositeId()
    {
      this.mapped = false;
      this.unsavedvalue = HbmUnsavedValueType.Undefined;
    }
  }
}
