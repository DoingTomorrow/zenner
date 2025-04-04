// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmOneToOne
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
  [XmlRoot("one-to-one", Namespace = "urn:nhibernate-mapping-2.2", IsNullable = false)]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "urn:nhibernate-mapping-2.2")]
  [GeneratedCode("HbmXsd", "3.2.0.1001")]
  [Serializable]
  public class HbmOneToOne : 
    AbstractDecoratable,
    IEntityPropertyMapping,
    IDecoratable,
    IFormulasMapping,
    IRelationship
  {
    [XmlElement("meta")]
    public HbmMeta[] meta;
    [XmlElement("formula")]
    public HbmFormula[] formula;
    [XmlAttribute]
    public string name;
    [XmlAttribute("formula")]
    public string formula1;
    [XmlAttribute]
    public string access;
    [XmlAttribute]
    public string @class;
    [XmlAttribute("entity-name")]
    public string entityname;
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
    [DefaultValue(false)]
    [XmlAttribute]
    public bool constrained;
    [XmlAttribute("foreign-key")]
    public string foreignkey;
    [XmlAttribute("property-ref")]
    public string propertyref;
    [XmlAttribute]
    public HbmLaziness lazy;
    [XmlIgnore]
    public bool lazySpecified;
    [XmlAttribute]
    public string node;
    [XmlAttribute("embed-xml")]
    [DefaultValue(true)]
    public bool embedxml;

    public HbmOneToOne()
    {
      this.constrained = false;
      this.embedxml = true;
    }

    public string Name => this.name;

    public string Access => this.access;

    public bool OptimisticLock => true;

    protected override HbmMeta[] Metadatas => this.meta ?? new HbmMeta[0];

    [XmlIgnore]
    public IEnumerable<HbmFormula> Formulas
    {
      get => (IEnumerable<HbmFormula>) this.formula ?? this.AsFormulas();
    }

    private IEnumerable<HbmFormula> AsFormulas()
    {
      if (!string.IsNullOrEmpty(this.formula1))
        yield return new HbmFormula()
        {
          Text = new string[1]{ this.formula1 }
        };
    }

    public string EntityName => this.entityname;

    public string Class => this.@class;

    public HbmNotFoundMode NotFoundMode => HbmNotFoundMode.Exception;

    public HbmLaziness? Lazy
    {
      get => !this.lazySpecified ? new HbmLaziness?() : new HbmLaziness?(this.lazy);
    }

    public bool IsLazyProperty
    {
      get
      {
        HbmLaziness? lazy = this.Lazy;
        return lazy.GetValueOrDefault() == HbmLaziness.Proxy && lazy.HasValue;
      }
    }
  }
}
