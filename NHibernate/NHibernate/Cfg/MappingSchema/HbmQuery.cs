// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmQuery
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
  [XmlRoot("query", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DesignerCategory("code")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [Serializable]
  public class HbmQuery : HbmBase
  {
    [XmlElement("query-param")]
    public HbmQueryParam[] Items;
    [XmlText]
    public string[] Text;
    [XmlAttribute]
    public string name;
    [XmlAttribute("flush-mode")]
    public HbmFlushMode flushmode;
    [XmlIgnore]
    public bool flushmodeSpecified;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool cacheable;
    [XmlAttribute("cache-region")]
    public string cacheregion;
    [XmlAttribute("fetch-size")]
    public int fetchsize;
    [XmlIgnore]
    public bool fetchsizeSpecified;
    [XmlAttribute(DataType = "positiveInteger")]
    public string timeout;
    [XmlAttribute("cache-mode")]
    public HbmCacheMode cachemode;
    [XmlIgnore]
    public bool cachemodeSpecified;
    [XmlAttribute("read-only")]
    public bool @readonly;
    [XmlIgnore]
    public bool readonlySpecified;
    [XmlAttribute]
    public string comment;

    public HbmQuery() => this.cacheable = false;

    public string GetText() => HbmBase.JoinString(this.Text);
  }
}
