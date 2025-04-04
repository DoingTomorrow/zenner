// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmProperty
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
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [XmlRoot("property", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [Serializable]
  public class HbmProperty : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IColumnsMapping,
    IFormulasMapping,
    ITypeMapping
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("column", typeof (HbmColumn))]
    [XmlElement("formula", typeof (HbmFormula))]
    public object[] Items;
    public HbmType type;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string node;
    [XmlAttribute]
    public string access;
    [XmlAttribute("type")]
    public string type1;
    [XmlAttribute]
    public string column;
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
    [DefaultValue(false)]
    [XmlAttribute]
    public bool unique;
    [XmlAttribute("unique-key")]
    public string uniquekey;
    [XmlAttribute]
    public string index;
    [XmlAttribute]
    public bool update;
    [XmlIgnore]
    public bool updateSpecified;
    [XmlAttribute]
    public bool insert;
    [XmlIgnore]
    public bool insertSpecified;
    [XmlAttribute("optimistic-lock")]
    [DefaultValue(true)]
    public bool optimisticlock;
    [XmlAttribute]
    public string formula;
    [XmlAttribute]
    [DefaultValue(false)]
    public bool lazy;
    [XmlAttribute]
    [DefaultValue(HbmPropertyGeneration.Never)]
    public HbmPropertyGeneration generated;

    public HbmProperty()
    {
      this.unique = false;
      this.optimisticlock = true;
      this.lazy = false;
      this.generated = HbmPropertyGeneration.Never;
    }

    public string Name => this.name;

    public bool IsLazyProperty => this.lazy;

    public string Access => this.access;

    public bool OptimisticLock => this.optimisticlock;

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

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
          notnullSpecified = this.notnullSpecified,
          unique = this.unique,
          uniqueSpecified = true,
          uniquekey = this.uniquekey,
          index = this.index
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
