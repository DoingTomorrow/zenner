// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmReturn
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
  [DebuggerStepThrough]
  [XmlRoot("return", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmReturn
  {
    [XmlElement("return-discriminator")]
    public HbmReturnDiscriminator returndiscriminator;
    [XmlElement("return-property")]
    public HbmReturnProperty[] returnproperty;
    [XmlAttribute]
    public string alias;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute]
    public string @class;
    [DefaultValue(HbmLockMode.Read)]
    [XmlAttribute("lock-mode")]
    public HbmLockMode lockmode;

    public HbmReturn() => this.lockmode = HbmLockMode.Read;
  }
}
