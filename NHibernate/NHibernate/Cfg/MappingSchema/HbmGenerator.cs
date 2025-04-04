// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmGenerator
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
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("generator", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmGenerator
  {
    [XmlElement("param")]
    public HbmParam[] param;
    [XmlAttribute]
    public string @class;
  }
}
