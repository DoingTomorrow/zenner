// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [XmlRoot("key", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmKey : IColumnsMapping
  {
    [XmlElement("column")]
    public HbmColumn[] column;
    [XmlAttribute("column")]
    public string column1;
    [XmlAttribute("property-ref")]
    public string propertyref;
    [XmlAttribute("foreign-key")]
    public string foreignkey;
    [DefaultValue(HbmOndelete.Noaction)]
    [XmlAttribute("on-delete")]
    public HbmOndelete ondelete;
    [XmlAttribute("not-null")]
    public bool notnull;
    [XmlIgnore]
    public bool notnullSpecified;
    [XmlAttribute]
    public bool update;
    [XmlIgnore]
    public bool updateSpecified;
    [XmlAttribute]
    public bool unique;
    [XmlIgnore]
    public bool uniqueSpecified;

    public HbmKey() => this.ondelete = HbmOndelete.Noaction;

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get => (IEnumerable<HbmColumn>) this.column ?? this.AsColumns();
    }

    private IEnumerable<HbmColumn> AsColumns()
    {
      if (!string.IsNullOrEmpty(this.column1))
        yield return new HbmColumn()
        {
          name = this.column1,
          notnull = this.notnull,
          notnullSpecified = this.notnullSpecified,
          unique = this.unique,
          uniqueSpecified = this.uniqueSpecified
        };
    }

    public bool? IsNullable => !this.notnullSpecified ? new bool?() : new bool?(!this.notnull);

    public bool? IsUpdatable => !this.updateSpecified ? new bool?() : new bool?(this.update);
  }
}
