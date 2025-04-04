// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmManyToMany
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [DesignerCategory("code")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("many-to-many", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [Serializable]
  public class HbmManyToMany : IColumnsMapping, IFormulasMapping, IRelationship
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("column", typeof (HbmColumn))]
    [XmlElement("formula", typeof (HbmFormula))]
    public object[] Items;
    [XmlElement("filter")]
    public HbmFilter[] filter;
    [XmlAttribute]
    public string @class;
    [XmlAttribute]
    public string node;
    [XmlAttribute("embed-xml")]
    [DefaultValue(true)]
    public bool embedxml;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute]
    public string column;
    [XmlAttribute]
    public string formula;
    [XmlAttribute("not-found")]
    [DefaultValue(HbmNotFoundMode.Exception)]
    public HbmNotFoundMode notfound;
    [XmlAttribute("outer-join")]
    public HbmOuterJoinStrategy outerjoin;
    [XmlIgnore]
    public bool outerjoinSpecified;
    [XmlAttribute]
    public HbmFetchMode fetch;
    [XmlIgnore]
    public bool fetchSpecified;
    [XmlAttribute]
    public HbmRestrictedLaziness lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [XmlAttribute("foreign-key")]
    public string foreignkey;
    [DefaultValue(false)]
    [XmlAttribute]
    public bool unique;
    [XmlAttribute]
    public string where;
    [XmlAttribute("order-by")]
    public string orderby;
    [XmlAttribute("property-ref")]
    public string propertyref;

    public HbmManyToMany()
    {
      this.embedxml = true;
      this.notfound = HbmNotFoundMode.Exception;
      this.unique = false;
    }

    [XmlIgnore]
    public IEnumerable<HbmColumn> Columns
    {
      get => this.Items == null ? this.AsColumns() : this.Items.OfType<HbmColumn>();
    }

    private IEnumerable<HbmColumn> AsColumns()
    {
      if (!string.IsNullOrEmpty(this.column))
        yield return new HbmColumn()
        {
          name = this.column,
          unique = this.unique,
          uniqueSpecified = true
        };
    }

    [XmlIgnore]
    public IEnumerable<HbmFormula> Formulas
    {
      get => this.Items == null ? this.AsFormulas() : this.Items.OfType<HbmFormula>();
    }

    private IEnumerable<HbmFormula> AsFormulas()
    {
      if (!string.IsNullOrEmpty(this.formula))
        yield return new HbmFormula()
        {
          Text = new string[1]{ this.formula }
        };
    }

    public string EntityName => this.entityname;

    public string Class => this.@class;

    public HbmNotFoundMode NotFoundMode => this.notfound;
  }
}
