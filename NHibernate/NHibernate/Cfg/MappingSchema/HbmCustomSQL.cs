// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmCustomSQL
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
  [DebuggerStepThrough]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlType(Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("sql-delete", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmCustomSQL
  {
    [XmlAttribute]
    public bool callable;
    [XmlIgnore]
    public bool callableSpecified;
    [XmlAttribute]
    public HbmCustomSQLCheck check;
    [XmlIgnore]
    public bool checkSpecified;
    [XmlText]
    public string[] Text;
  }
}
