// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmReturnJoin
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
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlRoot("return-join", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmReturnJoin
  {
    [XmlElement("return-property")]
    public HbmReturnProperty[] returnproperty;
    [XmlAttribute]
    public string alias;
    [XmlAttribute]
    public string property;
    [XmlAttribute("lock-mode")]
    [DefaultValue(HbmLockMode.Read)]
    public HbmLockMode lockmode;

    public HbmReturnJoin() => this.lockmode = HbmLockMode.Read;
  }
}
