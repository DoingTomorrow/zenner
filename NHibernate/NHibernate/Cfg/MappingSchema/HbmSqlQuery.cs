// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmSqlQuery
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
  [XmlRoot("sql-query", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmSqlQuery : HbmBase
  {
    [XmlElement("synchronize", typeof (HbmSynchronize))]
    [XmlElement("load-collection", typeof (HbmLoadCollection))]
    [XmlElement("return-join", typeof (HbmReturnJoin))]
    [XmlElement("return", typeof (HbmReturn))]
    [XmlElement("query-param", typeof (HbmQueryParam))]
    [XmlElement("return-scalar", typeof (HbmReturnScalar))]
    public object[] Items;
    [XmlText]
    public string[] Text;
    [XmlAttribute]
    public string name;
    [XmlAttribute("resultset-ref")]
    public string resultsetref;
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
    [XmlAttribute]
    [DefaultValue(false)]
    public bool callable;

    public HbmSqlQuery()
    {
      this.cacheable = false;
      this.callable = false;
    }

    public string GetText() => HbmBase.JoinString(this.Text);
  }
}
