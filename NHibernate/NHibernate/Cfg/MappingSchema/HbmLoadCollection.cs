// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmLoadCollection
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
  [DesignerCategory("code")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("load-collection", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmLoadCollection
  {
    [XmlElement("return-property")]
    public HbmReturnProperty[] returnproperty;
    [XmlAttribute]
    public string alias;
    [XmlAttribute]
    public string role;
    [DefaultValue(HbmLockMode.Read)]
    [XmlAttribute("lock-mode")]
    public HbmLockMode lockmode;

    public HbmLoadCollection() => this.lockmode = HbmLockMode.Read;
  }
}
