// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmManyToOne
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
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [XmlRoot("many-to-one", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [Serializable]
  public class HbmManyToOne : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IColumnsMapping,
    IFormulasMapping,
    IRelationship
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("column", typeof (HbmColumn))]
    [XmlElement("formula", typeof (HbmFormula))]
    public object[] Items;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    public string @class;
    [XmlAttribute("entity-name")]
    public string entityname;
    [XmlAttribute]
    public string column;
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
    public string cascade;
    [XmlAttribute("outer-join")]
    public HbmOuterJoinStrategy outerjoin;
    [XmlIgnore]
    public bool outerjoinSpecified;
    [XmlAttribute]
    public HbmFetchMode fetch;
    [XmlIgnore]
    public bool fetchSpecified;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool update;
    [DefaultValue(true)]
    [XmlAttribute]
    public bool insert;
    [DefaultValue(true)]
    [XmlAttribute("optimistic-lock")]
    public bool optimisticlock;
    [XmlAttribute("foreign-key")]
    public string foreignkey;
    [XmlAttribute("property-ref")]
    public string propertyref;
    [XmlAttribute]
    public string formula;
    [XmlAttribute]
    public HbmLaziness lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [XmlAttribute("not-found")]
    [DefaultValue(HbmNotFoundMode.Exception)]
    public HbmNotFoundMode notfound;
    [XmlAttribute]
    public string node;
    [DefaultValue(true)]
    [XmlAttribute("embed-xml")]
    public bool embedxml;

    public HbmManyToOne()
    {
      this.unique = false;
      this.update = true;
      this.insert = true;
      this.optimisticlock = true;
      this.notfound = HbmNotFoundMode.Exception;
      this.embedxml = true;
    }

    public string Name => this.name;

    public string Access => this.access;

    public bool IsLazyProperty => false;

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

    public string EntityName => this.entityname;

    public string Class => this.@class;

    public HbmNotFoundMode NotFoundMode => this.notfound;

    [XmlIgnore]
    public IEnumerable<object> ColumnsAndFormulas
    {
      get
      {
        return (IEnumerable<object>) this.Items ?? this.Columns.Cast<object>().Concat<object>(this.Formulas.Cast<object>());
      }
    }

    public HbmLaziness? Lazy
    {
      get => !this.lazySpecified ? new HbmLaziness?() : new HbmLaziness?(this.lazy);
    }
  }
}
