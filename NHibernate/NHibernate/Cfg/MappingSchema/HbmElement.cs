// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmElement
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
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("element", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [Serializable]
  public class HbmElement : IColumnsMapping, IFormulasMapping, ITypeMapping
  {
    [XmlElement("formula", typeof (HbmFormula))]
    [XmlElement("column", typeof (HbmColumn))]
    public object[] Items;
    public HbmType type;
    [XmlAttribute]
    public string column;
    [XmlAttribute]
    public string node;
    [XmlAttribute]
    public string formula;
    [XmlAttribute("type")]
    public string type1;
    [XmlAttribute(DataType = "positiveInteger")]
    public string length;
    [XmlAttribute(DataType = "positiveInteger")]
    public string precision;
    [XmlAttribute(DataType = "nonNegativeInteger")]
    public string scale;
    [XmlAttribute("not-null")]
    [DefaultValue(false)]
    public bool notnull;
    [XmlAttribute]
    [DefaultValue(false)]
    public bool unique;

    public HbmElement()
    {
      this.notnull = false;
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
          length = this.length,
          scale = this.scale,
          precision = this.precision,
          notnull = this.notnull,
          notnullSpecified = true,
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

    public HbmType Type
    {
      get
      {
        HbmType type = this.type;
        if (type != null)
          return type;
        if (string.IsNullOrEmpty(this.type1))
          return (HbmType) null;
        return new HbmType() { name = this.type1 };
      }
    }
  }
}
