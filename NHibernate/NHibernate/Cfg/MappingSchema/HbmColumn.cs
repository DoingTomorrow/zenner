// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmColumn
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
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlRoot("column", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmColumn
  {
    public HbmComment comment;
    [XmlAttribute]
    public string name;
    [XmlAttribute(DataType = "positiveInteger")]
    public string length;
    [XmlAttribute(DataType = "positiveInteger")]
    public string precision;
    [XmlAttribute(DataType = "nonNegativeInteger")]
    public string scale;
    [XmlAttribute("not-null")]
    public bool notnull;
    [XmlIgnore]
    public bool notnullSpecified;
    [XmlAttribute]
    public bool unique;
    [XmlIgnore]
    public bool uniqueSpecified;
    [XmlAttribute("unique-key")]
    public string uniquekey;
    [XmlAttribute("sql-type")]
    public string sqltype;
    [XmlAttribute]
    public string index;
    [XmlAttribute]
    public string check;
    [XmlAttribute]
    public string @default;
  }
}
